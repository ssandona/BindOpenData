using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace csvReading
{
    public partial class City : PhoneApplicationPage
    {
         public City()
        {
            InitializeComponent();
            this.DataContext = App.ViewModel;
        }

    }
}