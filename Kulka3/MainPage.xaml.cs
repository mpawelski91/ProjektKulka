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

namespace Kulka3
{
    public partial class MainPage : PhoneApplicationPage
    {

        private Accelerometer accelerometer;

        private DispatcherTimer globalTimer;

        public Vector2 HolePosition { get; set; }

        private int ballSpeed = 1;

        private int score;

        public MainPage()
        {
            InitializeComponent();

            accelerometer = new Accelerometer();
            accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds(3);
            accelerometer.CurrentValueChanged += Accelerometer_CurrentValueChanged;
            accelerometer.Start();
            HolePosition = new Vector2 { X = 156, Y = 72 };
            score = 0;

            StartTimers();
        }

        private void StartTimers()
        {
            globalTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            globalTimer.Tick += GlobalTimer_Tick;
            globalTimer.Start();
        }

        private void GlobalTimer_Tick(object sender, EventArgs e)
        {
            ballSpeed++;
        }

        //private void Time_Tick(object sender, EventArgs e)
        //{
        //    SetHolePosition();
        //}

        private void SetHolePosition()
        {
            Random rand = new Random();

            int posX, posY;
            do
            {
                posX = rand.Next(0, (int)Grid.ActualWidth);
                posY = rand.Next(0, (int)Grid.ActualHeight);
            }
            while (posY == Canvas.GetLeft(Ball) || posX == Canvas.GetTop(Ball));

            Canvas.SetTop(Hole, posY);
            Canvas.SetLeft(Hole, posX);

            HolePosition = new Vector2 { X = posX, Y = posY };
        }

        private void Accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            Dispatcher.BeginInvoke(() => UpdateUI(e.SensorReading));
        }

        private void UpdateUI(AccelerometerReading sensorReading)
        {
            var acceleration = sensorReading.Acceleration;
            //Text.Text = "B: " + Canvas.GetLeft(Ball) + " " + Canvas.GetTop(Ball) + " H: " + HolePosition.X + " " + HolePosition.Y;
            if (acceleration.Z < -0.5)
            {
                double previousPosition = Canvas.GetTop(Ball);
                Canvas.SetTop(Ball, previousPosition - ballSpeed);
            }
            else if (acceleration.Z > 0.5)
            {
                double previousPosition = Canvas.GetTop(Ball);
                Canvas.SetTop(Ball, previousPosition + ballSpeed);
            }

            if (acceleration.X < -0.5)
            {
                double previousPosition = Canvas.GetLeft(Ball);
                Canvas.SetLeft(Ball, previousPosition - ballSpeed);
            }
            else if (acceleration.X > 0.5)
            {
                double previousPosition = Canvas.GetLeft(Ball);
                Canvas.SetLeft(Ball, previousPosition + ballSpeed);
            }

            if(globalTimer.IsEnabled)
                CheckCollision();
        }

        //+20 dla piłki bo ma wymiary 40x40 czyli jak krawędz ma srodek + 20
        //dziura ma 80x80
        private void CheckCollision()
        {
            if ((Canvas.GetLeft(Ball) - 20 >= HolePosition.X - 40 && Canvas.GetLeft(Ball) + 20 <= HolePosition.X + 40) &&
                Canvas.GetTop(Ball) - 20 >= HolePosition.Y - 40 && Canvas.GetTop(Ball) + 20 <= HolePosition.Y + 40)
            {
                SetHolePosition();
                score += 100;
            }

            Score.Text = "Wynik: " + score;

            if(Canvas.GetLeft(Ball) > 480 || Canvas.GetTop(Ball) > 740 ||
                Canvas.GetLeft(Ball) < 0 || Canvas.GetTop(Ball) < 0)
            {
                Message.Visibility = Visibility.Visible;
                Text.Text = "PRZEGRAŁEŚ!!!!";
                accelerometer.CurrentValueChanged -= Accelerometer_CurrentValueChanged;
            }
        }

        private void ScoreConfirmed_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ScoreWindow.xaml?msg=" + Nick.Text + "," + score, UriKind.Relative));
        }
    }
}