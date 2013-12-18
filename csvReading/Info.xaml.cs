using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace csvReading
{
    public partial class Info : PhoneApplicationPage
    {
        private Color lightThemeBackground = Color.FromArgb(255, 255, 255, 255);
        private Color darkThemeBackground = Color.FromArgb(255, 0, 0, 0);

        public Info()
        {
            InitializeComponent();
            DisplayState();
        }

        private void DisplayState()
        {
            SolidColorBrush backgroundBrush = Application.Current.Resources["PhoneBackgroundBrush"] as SolidColorBrush;

            if (backgroundBrush.Color == lightThemeBackground)
            {
                //MessageBox.Show("tema biango");
                Img.Source = new BitmapImage(new Uri("/Images/opendata.png", UriKind.Relative));
                Testo.Margin = new Thickness(0,0,-20,0);
            }
            else
            {
                //MessageBox.Show("tema negro");
                Img.Source = new BitmapImage(new Uri("/Images/open_data.png", UriKind.Relative));
            }
        }
    }
}