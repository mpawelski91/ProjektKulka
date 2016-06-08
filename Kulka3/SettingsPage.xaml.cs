using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Kulka3
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }
           protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) 
        { 
            var colors = new List<ColorSelectModel>(); 
            var accentColor = (Color)Resources["PhoneAccentColor"]; 
            colors.Add(new ColorSelectModel("Accent Color", accentColor)); 
            colors.AddRange( 
                    typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public) 
                        .Where(p => p.PropertyType == typeof(Color)) 
                        .Where(p => p.Name != "Transperant" && p.Name != "Black") 
                        .Select(p => new ColorSelectModel(p.Name, (Color)p.GetValue(typeof(Colors), null)))); 
            listPickerColor.ItemsSource = listPickerColor.ItemsSource ?? new ObservableCollection<ColorSelectModel>(colors);
            listPickerBallColor.ItemsSource = listPickerColor.ItemsSource ?? new ObservableCollection<ColorSelectModel>(colors);
            this.Background = new SolidColorBrush(Colors.Black);

            Helper.BackgroundColor = (listPickerColor.SelectedItem as ColorSelectModel).ColorBrush;
            Helper.BallColor = (listPickerBallColor.SelectedItem as ColorSelectModel).ColorBrush;
        }

       
    } 
 
    public class ColorSelectModel 
    { 
        public ColorSelectModel(string text, Color color) 
        { 
            this.Text = text; 
            this.Color = color; 
            this.ColorBrush = new SolidColorBrush(color); 
        } 
        public string Text { get; set; } 
        public Color Color { get; set; } 
        public SolidColorBrush ColorBrush { get; set; } 

    } 
    
}