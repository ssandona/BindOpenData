using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using csvReading.ViewModel;

namespace csvReading
{
    public partial class ListaComuni : PhoneApplicationPage
    {
        string prov;

        public ListaComuni()
        {
            InitializeComponent();
            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
        }






        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string provincia;
            // recupero il comune di cui cercare il tutto
            if (NavigationContext.QueryString.TryGetValue("prov", out provincia))
            {
                prov = provincia;
            }
            ((ComuniVM)this.DataContext).Prov = provincia;
        }


  

    }
}