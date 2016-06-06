using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Kulka3.Resources;
using Microsoft.Devices.Sensors;
using System.Diagnostics;
using System.Windows.Threading;
using System.Windows.Media;
using Microsoft.Xna.Framework.Control;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using System.Windows.Shapes;
using System.Threading.Tasks;

namespace Kulka3
{
    public partial class MainPage : PhoneApplicationPage
    {
        int animation = 0;
        public MainPage()
        {
            InitializeComponent();
        }

        
        private void OpenGame_Click(object sender, RoutedEventArgs e)
        {
            if (animation == 1)
            {
                NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
            }
       }
        private void Storyboard_Completed(object sender, EventArgs e)
        {
            animation++;
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }
        private void ScorePage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ScorePage.xaml", UriKind.Relative));
        }
        private void SettingsPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Terminate();
        }

   


    }
}