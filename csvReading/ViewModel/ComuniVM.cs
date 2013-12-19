using csvReading.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace csvReading.ViewModel
{
    public class ComuniVM : INotifyPropertyChanged
    {
        private Comune attuale;
        public Comune Attuale{
            get{
                return attuale;
            }
            set {
                if (value != null) {
                    attuale = value;
                    VaiAComune();
                }
            }
        }

        private ObservableCollection<Comune> comuni;
        public ObservableCollection<Comune> Comuni {
            get {
                return comuni;
            }
            set {
                if (value != null) {
                    comuni = value;
                    NotifyPropertyChanged("Comuni");
                }
            }
        }

        private ObservableCollection<Comune> comuniBelluno;
        public ObservableCollection<Comune> ComuniBelluno
        {
            get
            {
                return comuniBelluno;
            }
        }

        private ObservableCollection<Comune> comuniPadova;
        public ObservableCollection<Comune> ComuniPadova
        {
            get
            {
                return comuniPadova;
            }
        }

        private ObservableCollection<Comune> comuniRovigo;
        public ObservableCollection<Comune> ComuniRovigo
        {
            get
            {
                return comuniRovigo;
            }
        }

        private ObservableCollection<Comune> comuniTreviso;
        public ObservableCollection<Comune> ComuniTreviso
        {
            get
            {
                return comuniTreviso;
            }
        }

        private ObservableCollection<Comune> comuniVenezia;
        public ObservableCollection<Comune> ComuniVenezia
        {
            get
            {
                return comuniVenezia;
            }
        }

        private ObservableCollection<Comune> comuniVerona;
        public ObservableCollection<Comune> ComuniVerona
        {
            get
            {
                return comuniVerona;
            }
        }

        private ObservableCollection<Comune> comuniVicenza;
        public ObservableCollection<Comune> ComuniVicenza
        {
            get
            {
                return comuniVicenza;
            }
        }

        private string prov;
        public string Prov {
            get {
                return prov;
            }
            set {
                if (!value.Equals(prov)) {
                    prov = value;
                    cambiaProvincia();
                    NotifyPropertyChanged("Prov");
                }
            }
        }


        private ICommand vaiAGrafico;
        public ICommand VaiAGrafico
        {
            get
            {
                return vaiAGrafico;
            }
        }

      
        public ComuniVM()
        {
            comuni = new ObservableCollection<Comune>();
            /*comuniBelluno = CsvReader.Instance.ComuniDaProvincia("bl");
            comuniPadova = CsvReader.Instance.ComuniDaProvincia("pd");
            comuniTreviso = CsvReader.Instance.ComuniDaProvincia("tv");
            comuniRovigo = CsvReader.Instance.ComuniDaProvincia("ro");
            comuniVenezia = CsvReader.Instance.ComuniDaProvincia("ve");
            comuniVerona = CsvReader.Instance.ComuniDaProvincia("vr");
            comuniVicenza = CsvReader.Instance.ComuniDaProvincia("vi");*/
            vaiAGrafico = new DelegateCommand(_vaiAGrafico);
        }

        private void cambiaProvincia() {
            switch (prov)
            {
                case "bl": 
                    if(comuniBelluno==null)
                        comuniBelluno = CsvReader.Instance.ComuniDaProvincia("bl");
                    Comuni = comuniBelluno; Prov="Belluno";break;
                case "pd":
                    if (comuniPadova == null)
                        comuniPadova = CsvReader.Instance.ComuniDaProvincia("pd");
                    Comuni = comuniPadova; Prov = "Padova"; break;
                case "ro":
                    if (comuniRovigo == null)
                        comuniRovigo = CsvReader.Instance.ComuniDaProvincia("ro");
                    Comuni = comuniRovigo; Prov="Rovigo";break;
                case "tv":
                    if (comuniTreviso == null)
                        comuniTreviso = CsvReader.Instance.ComuniDaProvincia("tv");
                    Comuni = comuniTreviso; Prov="Treviso";break;
                case "ve":
                    if (comuniVenezia == null)
                        comuniVenezia = CsvReader.Instance.ComuniDaProvincia("ve");
                    Comuni = comuniVenezia; Prov="Venezia";break;
                case "vr":
                    if (comuniVerona == null)
                        comuniVerona = CsvReader.Instance.ComuniDaProvincia("vr");
                    Comuni = comuniVerona; Prov="Verona";break;
                case "vi":
                    if (comuniVicenza == null)
                        comuniVicenza = CsvReader.Instance.ComuniDaProvincia("vi");
                    Comuni = comuniVicenza; Prov="Vicenza";break;
                default:
                    Comuni = null; break;
            }
        
        }

        private void VaiAComune() {
            var rootFrame = (App.Current as App).RootFrame;
            rootFrame.Navigate(new Uri("/City.xaml", UriKind.Relative));
        }

        private void _vaiAGrafico(object o)
        {
            string tipo = o.ToString();
            string uri = "/Graph.xaml?dato=" + tipo + "&comune=" + attuale.Istat;
            var rootFrame = (App.Current as App).RootFrame;
            rootFrame.Navigate(new Uri(uri, UriKind.Relative));
        }


        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
