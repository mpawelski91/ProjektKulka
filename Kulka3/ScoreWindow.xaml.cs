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

namespace Kulka3
{
    public partial class ScoreWindow : PhoneApplicationPage
    {
        private List<RecordModel> records;

        public ScoreWindow()
        {
            InitializeComponent();
            records = new List<RecordModel>();
        }

        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string msg = "";
            if (NavigationContext.QueryString.TryGetValue("msg", out msg))
                Debug.WriteLine(msg);
        }

        private void ReadRecords()
        {
            //zakladam ze wyniki sa od 1 do 10 i kazdy wynik zapisany jest po danym numerkiem
            for (int i = 1; i != 11; i++)
            {
                if (settings.Contains(i.ToString()))
                {
                    string nick, localization;
                    int score;
                    var setting = IsolatedStorageSettings.ApplicationSettings[i.ToString()] as string;
                    ParseSetting(setting, out nick, out score, out localization);
                }
            }
        }

        private void ParseSetting(string setting, out string nick, out int score, out string localization)
        {
            throw new NotImplementedException();
        }

    }