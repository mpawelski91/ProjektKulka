using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Devices.Sensors;
using System.Windows.Threading;
using Microsoft.Xna.Framework;
using System.Windows.Shapes;

namespace Kulka3
{
    public partial class GamePage : PhoneApplicationPage
    {
        private Accelerometer accelerometer;

        private DispatcherTimer globalTimer;

        public Vector2 HolePositionRed { get; set; }
        public Vector2 HolePositionRed2 { get; set; }
        public Vector2 HolePositionRed3 { get; set; }
        public Vector2 HolePositionGreen { get; set; }
        private List<Vector2> holes;

        private int ballSpeed = 3;

        private int score;

        public GamePage()
        {
            InitializeComponent();

            accelerometer = new Accelerometer();
            accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds(3);
            accelerometer.CurrentValueChanged += Accelerometer_CurrentValueChanged;
            accelerometer.Start();
            HolePositionRed = new Vector2 { X = 49, Y = 38 };
            HolePositionRed2 = new Vector2 { X = 379, Y = 38 };
            HolePositionRed3 = new Vector2 { X = 49, Y = 611 };
            HolePositionGreen = new Vector2 { X = 379, Y = 611 };
            holes = new List<Vector2>();
            holes.Add(HolePositionRed2);
            holes.Add(HolePositionRed3);
            holes.Add(HolePositionRed);
            holes.Add(HolePositionGreen);
            score = 0;

            //StartTimers();
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
        Random rand = new Random();
        private void SetHolePosition(Ellipse name, string n)
        {
            int posX, posY;
            bool result = true;

            do
            {
                do
                {
                    posX = rand.Next(0, (int)Grid.ActualWidth - 60);
                    posY = rand.Next(0, (int)Grid.ActualHeight - 60);
                }
                while (posY == Canvas.GetLeft(Ball) || posX == Canvas.GetTop(Ball));

                result = CompareWithOthersHoles(posX, posY);
            } while (!result);

            Canvas.SetTop(name, posY);
            Canvas.SetLeft(name, posX);

            switch (n)
            {
                case "red":
                    HolePositionRed = new Vector2 { X = posX, Y = posY };
                    //Canvas.SetTop(HoleRed, posY);
                    //Canvas.SetLeft(HoleRed, posX);
                    holes.Add(HolePositionRed);
                    break;
                case "red2":
                    HolePositionRed2 = new Vector2 { X = posX, Y = posY };
                    //Canvas.SetTop(HoleRed2, posY);
                    //Canvas.SetLeft(HoleRed2, posX);
                    holes.Add(HolePositionRed2);
                    break;
                case "red3":
                    HolePositionRed3 = new Vector2 { X = posX, Y = posY };
                    //Canvas.SetTop(HoleRed3, posY);
                    //Canvas.SetLeft(HoleRed3, posX);
                    holes.Add(HolePositionRed3);
                    break;
                case "green":
                    HolePositionGreen = new Vector2 { X = posX, Y = posY };
                    //Canvas.SetTop(HoleGreen, posY);
                    //Canvas.SetLeft(HoleGreen, posX);
                    holes.Add(HolePositionGreen);
                    break;
            }

        }

        private bool CompareWithOthersHoles(int posX, int posY)
        {
            if (holes.Any())
            {
                foreach(var hole in holes)
                    if(posY == hole.Y || posX == hole.X)
                        return false;
            }
            return true;
        }

        private void Accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            Dispatcher.BeginInvoke(() => UpdateUI(e.SensorReading));
        }

        private void UpdateUI(AccelerometerReading sensorReading)
        {
            var acceleration = sensorReading.Acceleration;
            //Text.Text = "B: " + Canvas.GetLeft(Ball) + " " + Canvas.GetTop(Ball) + " H: " + HolePosition.X + " " + HolePosition.Y;
            if (acceleration.Z < -0.1)
            {
                double previousPosition = Canvas.GetTop(Ball);
                if (0 < previousPosition)
                    Canvas.SetTop(Ball, previousPosition - ballSpeed);
            }
            else if (acceleration.Z > 0.1)
            {
                double previousPosition = Canvas.GetTop(Ball);
                if (Grid.ActualHeight - 40 > previousPosition)
                    Canvas.SetTop(Ball, previousPosition + ballSpeed);
            }

            if (acceleration.X < -0.1)
            {
                double previousPosition = Canvas.GetLeft(Ball);
                if (0 < previousPosition)
                    Canvas.SetLeft(Ball, previousPosition - ballSpeed);
            }
            else if (acceleration.X > 0.1)
            {
                double previousPosition = Canvas.GetLeft(Ball);
                if (Grid.ActualWidth - 40 > previousPosition)
                    Canvas.SetLeft(Ball, previousPosition + ballSpeed);
            }

            //if(globalTimer.IsEnabled)
            CheckCollision();
        }

        //+20 dla piłki bo ma wymiary 40x40 czyli jak krawędz ma srodek + 20
        //dziura ma 80x80
        private void CheckCollision()
        {
            foreach (var hole in holes)
            {
                CheckHole(hole);
            }
            //if ((Canvas.GetLeft(Ball) - 20 >= HolePositionGreen.X - 40 && Canvas.GetLeft(Ball) + 20 <= HolePositionGreen.X + 40) &&
            //    Canvas.GetTop(Ball) - 20 >= HolePositionGreen.Y - 40 && Canvas.GetTop(Ball) + 20 <= HolePositionGreen.Y + 40)
            //{
            //    holes = new List<Vector2>();
            //    SetHolePosition(HoleGreen, "green");
            //    SetHolePosition(HoleRed, "red");
            //    SetHolePosition(HoleRed2, "red2");
            //    SetHolePosition(HoleRed3, "red3");

            //    score += 100;
            //}

            //Score.Text = "Wynik: " + score;

            //if(Canvas.GetLeft(Ball) > 480 || Canvas.GetTop(Ball) > 740 ||
            //    Canvas.GetLeft(Ball) < 0 || Canvas.GetTop(Ball) < 0)
            //{
            //    Message.Visibility = Visibility.Visible;
            //    Text.Text = "PRZEGRAŁEŚ!!!!";
            //    accelerometer.CurrentValueChanged -= Accelerometer_CurrentValueChanged;
            //}
        }

        private void CheckHole(Vector2 hole)
        {
            if ((Canvas.GetLeft(Ball) - 20 >= hole.X - 40 && Canvas.GetLeft(Ball) + 20 <= hole.X + 40) &&
                Canvas.GetTop(Ball) - 20 >= hole.Y - 40 && Canvas.GetTop(Ball) + 20 <= hole.Y + 40)
            {
                if (hole.X == HolePositionGreen.X && hole.Y == HolePositionGreen.Y)
                {
                    holes = new List<Vector2>();
                    SetHolePosition(HoleGreen, "green");
                    SetHolePosition(HoleRed, "red");
                    SetHolePosition(HoleRed2, "red2");
                    SetHolePosition(HoleRed3, "red3");
                    ballSpeed++;
                    score += 100 * ballSpeed;
                    Score.Text = "Wynik: " + score;
                }
                else
                {
                    Message.Visibility = Visibility.Visible;
                    Text.Text = "PRZEGRANA!!!!";
                    accelerometer.CurrentValueChanged -= Accelerometer_CurrentValueChanged;
                }
            }

        }

        private void ScoreConfirmed_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ScorePage.xaml?msg=" + Nick.Text + "," + score, UriKind.Relative));
        }
    }
}