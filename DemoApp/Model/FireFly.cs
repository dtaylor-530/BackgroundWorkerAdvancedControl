using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace DemoApp
{

    public static class FireflyHelper
    {
        //private static Random Randomer = new Random();

        public static void Add(Canvas canvas,double opacity=0)
        {
            Firefly CurrentFirefly = new Firefly(opacity);
            //CurrentFirefly.Speed = Randomer.Next(1, 3);
            CurrentFirefly.Body.Margin = new Thickness(StrongRandom.Next(10, (int)canvas.ActualWidth - 10),
                                                       StrongRandom.Next(10, (int)canvas.ActualHeight - 10),
                                                       0, 0);
            CurrentFirefly.Body.Fill = Brushes.Black;
            CurrentFirefly.Body.Height = canvas.ActualHeight / 4;
            CurrentFirefly.Body.Width = 1.5 * CurrentFirefly.Body.Height;
            canvas.Children.Add(CurrentFirefly.Body);

        }

    }



    class Firefly
    {
        public Firefly (double opacity)
        {
            Body = new Ellipse { Opacity =opacity};
        }

        public Ellipse Body { get; }
        //public int Speed { get; set; }
    }
}
