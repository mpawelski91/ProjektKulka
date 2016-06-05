using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kulka3
{
    static class Helper
    {
        public static List<RecordModel> records = new List<RecordModel>();

        private static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        public static void ReadRecords()
        {
            for (int i = 1; i != 11; i++)
            {
                if (settings.Contains(i.ToString()))
                {
                    string nick, localization;
                    int score;
                    var setting = IsolatedStorageSettings.ApplicationSettings[i.ToString()] as string;
                    ParseSetting(setting, out nick, out score, out localization);
                    records.Add(new RecordModel { Nick = nick, Score = score, Localization = localization });
                }
            }
        }

        public static void SaveRecords()
        {
            for (int i = 1; i <= 10; i++)
            {
                if (i <= records.Count)
                {
                    if (!settings.Contains(i.ToString()))
                        settings.Add(i.ToString(), records[i - 1].Nick + "," + records[i - 1].Score+","+ records[i - 1].Localization);
                    else
                        settings[i.ToString()] = records[i - 1].Nick + "," + records[i - 1].Score + "," + records[i - 1].Localization;
                }
            }
        }

        public static void ParseSetting(string setting, out string nick, out int score, out string localization)
        {
            string[] tab = setting.Split(',');
            nick = tab[0];
            int.TryParse(tab[1], out score);
            localization = tab[2];
        }
    }
}
