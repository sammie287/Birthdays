﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Birthday
{

    public class Output
    {
        public string Name { get; set; }
        public string Birthday { get; set; }
        public string DaysRemaining { get; set; }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewBirthdays : Page
    {
        public ViewBirthdays()
        {
            this.InitializeComponent();
            var personList = DataAccess.GetPeople();

            List<Output> outputList = new List<Output>();
            foreach (Person person in personList)
            {
                string birthday = person.Birthday.ToString("MMMM dd");
                string outputDaysRemaining = GetDaysRemaining(person.Birthday, person.Name);
                Output output = new Output {Name=person.Name, Birthday=birthday, DaysRemaining=outputDaysRemaining};
                outputList.Add(output);
            }

            Output.ItemsSource = outputList;
        }

        private string GetDaysRemaining(DateTime input, string name)
        {
            DateTime birthday = DateTime.Parse(input.Month + "/" + input.Day + "/" + DateTime.Now.Year);
            TimeSpan difference = birthday - DateTime.Now;
            string output;
            if(difference.Days > 0 || difference.Hours > 0)
            {
                int daysRemaining = difference.Days;
                if(difference.Days == 0)
                {
                    daysRemaining = 1;
                }
                output = daysRemaining + (daysRemaining == 1 ? " day " : " days ") + "left until " + name + "'s birthday.";
            }
            else if (difference.Days == 0 && difference.Hours <= 0)
            {
                output = $"{name}'s birthday is today.";
            }
            else
            {
                output = $"{name}'s birthday has already passed.";
            }
            return output;
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            NavView.SelectedItem = NavView.MenuItems[3];
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if(args.InvokedItem != null)
            {
                switch (args.InvokedItem)
                {
                    case "Main Page":
                        this.Frame.Navigate(typeof(MainPage), null, new SuppressNavigationTransitionInfo());
                        break;

                    case "Add Birthday":
                        this.Frame.Navigate(typeof(AddBirthday), null, new SuppressNavigationTransitionInfo());
                        break;

                    case "Remove Birthday":
                        this.Frame.Navigate(typeof(RemoveBirthday), null, new SuppressNavigationTransitionInfo());
                        break;

                    case "View Birthdays":
                        this.Frame.Navigate(typeof(ViewBirthdays), null, new SuppressNavigationTransitionInfo());
                        break;
                }
            }
        }
    }
}
