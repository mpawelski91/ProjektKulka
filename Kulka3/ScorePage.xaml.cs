using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Device;
using System.Device.Location;
using System.Windows.Data;

namespace Kulka3
{
    public partial class Scorepage : PhoneApplicationPage
    {
        //private static List<RecordModel> records = new List<RecordModel>();

        public Scorepage()
        {
            InitializeComponent();
            //records = new List<RecordModel>();
            //Helper.ReadRecords();
            //lista.ItemsSource = Helper.records;
            lista.ItemsSource = Helper.records;
        }

        //IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string msg = "";
            //string location = getLocalization();
            string localization = ",";
            if (NavigationContext.QueryString.TryGetValue("msg", out msg))
                CheckRecord(msg + localization);
        }

        private string getLocalization()
        {
            GeoCoordinateWatcher myWatcher = new GeoCoordinateWatcher();

            var myPosition = myWatcher.Position;

            double latitude = 0.0;
            double longitude = 0.0;
            if(!myPosition.Location.IsUnknown)
            {
                latitude = myPosition.Location.Latitude;
                longitude = myPosition.Location.Longitude;
            }


            return "";
        }

        private void CheckRecord(string message)
        {
            int score;
            string nick, localization;

            if (!Helper.records.Any() || Helper.records.Count < 10)
            {
                Helper.ParseSetting(message, out nick, out score, out localization);
                Helper.records.Add(new RecordModel { Nick = nick, Score = score, Localization = localization });
            }
            else if(Helper.records.Count == 10)
            {
                var rec = Helper.records.LastOrDefault().Score;

                Helper.ParseSetting(message, out nick, out score, out localization);

                if (rec < score)
                {
                    Helper.records.RemoveAt(Helper.records.Count - 1);
                    Helper.records.Add(new RecordModel { Nick = nick, Score = score, Localization = localization });
                   
                }
               
            }
            Helper.records = new List<RecordModel>(Helper.records.OrderByDescending(x => x.Score));
            //SaveRecords();
            lista.ItemsSource = Helper.records;
            //Helper.SaveRecords();
        }

        

        //private void ReadRecords()
        //{
        //    //zakladam ze wyniki sa od 1 do 10 i kazdy wynik zapisany jest po danym numerkiem
        //    for (int i = 1; i != 11; i++)
        //    {
        //        if (settings.Contains(i.ToString()))
        //        {
        //            string nick, localization;
        //            int score;
        //            var setting = IsolatedStorageSettings.ApplicationSettings[i.ToString()] as string;
        //            ParseSetting(setting, out nick, out score, out localization);
        //            Helper.records.Add(new RecordModel { Nick = nick, Score = score, Localization = localization });
        //        }
        //    }

        //    var temp = new List<RecordModel>(Helper.records);
        //    Helper.records = new List<RecordModel>(temp.OrderByDescending(x => x.Score));
        //}

        //private void ParseSetting(string setting, out string nick, out int score, out string localization)
        //{
        //    string[] tab = setting.Split(',');
        //    nick = tab[0];
        //    int.TryParse(tab[1], out score);
        //    localization = tab[2];
        //}

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}