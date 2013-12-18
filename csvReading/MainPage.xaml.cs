using System;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace csvReading
{
    public partial class MainPage : PhoneApplicationPage
    {
        SolidColorBrush selezionato = new SolidColorBrush(Color.FromArgb(0x77, 0x00, 0x33, 0x00));
        SolidColorBrush the_void = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00));
            
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Verona.Fill = the_void;
            Vicenza.Fill = the_void;
            Venezia.Fill = the_void;
            Treviso.Fill = the_void;
            Belluno.Fill = the_void;
            Padova.Fill = the_void;
            Rovigo.Fill = the_void;
        }

        private void VaiAProvincia(object sender, RoutedEventArgs e)
        {
            ((Path)sender).Fill = selezionato;
            string prov = ((Path)sender).Tag.ToString();
                //gestisco via get che comune visualizzare
            NavigationService.Navigate(new Uri("/ListaComuni.xaml?prov="+prov,UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/info.xaml", UriKind.Relative));
        }
    }
}