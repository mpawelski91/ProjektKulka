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

namespace Kulka3
{
    public partial class ScoreWindow : PhoneApplicationPage
    {
        //private static List<RecordModel> records = new List<RecordModel>();

        public ScoreWindow()
        {
            InitializeComponent();
            //records = new List<RecordModel>();
            //ReadRecords();
            //lista.ItemsSource = Helper.records;
        }

        //IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string msg = "";
            string location = getLocalization();
            if (NavigationContext.QueryString.TryGetValue("msg", out msg))
                CheckRecord(msg+",ksdhfjkasdf");
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
            if (Helper.records.Any())
            {
                var rec = Helper.records.LastOrDefault().Score;
                string nick, localization;
                int score;
                ParseSetting(message, out nick, out score, out localization);

                if (rec < score)
                {
                    Helper.records.RemoveAt(Helper.records.Count - 1);
                    Helper.records.Add(new RecordModel { Nick = nick, Score = score, Localization = localization });
                    Helper.records = new List<RecordModel>(Helper.records.OrderBy(x => x.Score));

                    

                    //for (int i = 1; i <= 10; i++)
                    //{
                    //    if (settings.Contains(i.ToString()))
                    //        settings[i.ToString()] = nick + "," + score + "," + localization;
                    //    else
                    //        settings.Add(i.ToString(), nick + "," + score + "," + localization);
                    //}
                }
            }
            else
            {
                string nick, localization;
                int score;
                ParseSetting(message, out nick, out score, out localization);
                Helper.records.Add(new RecordModel { Nick = nick, Score = score, Localization = localization });
            }
            lista.ItemsSource = Helper.records;
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
        //            records.Add(new RecordModel { Nick = nick, Score = score, Localization = localization });
        //        }
        //    }

        //    var temp = new List<RecordModel>(records);
        //    records = new List<RecordModel>(temp.OrderBy(x => x.Score));
        //}

        private void ParseSetting(string setting, out string nick, out int score, out string localization)
        {
            string[] tab = setting.Split(',');
            nick = tab[0];
            int.TryParse(tab[1], out score);
            localization = tab[2];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }


    }
}