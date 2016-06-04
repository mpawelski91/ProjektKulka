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

namespace Kulka3
{
    public partial class ScoreWindow : PhoneApplicationPage
    {
        public ScoreWindow()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string msg = "";
            if (NavigationContext.QueryString.TryGetValue("msg", out msg))
                Debug.WriteLine(msg);
        }
    }
}