using csvReading.Model;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;

namespace csvReading
{
    public partial class Graph : PhoneApplicationPage
    {
        private Color lightThemeBackground = Color.FromArgb(255, 255, 255, 255);
        private Color darkThemeBackground = Color.FromArgb(255, 0, 0, 0);

        public double altezzaCanvas;
        public double larghezzaCanvas;
        public double larghezzaGriglia;
        public List<int> valori;
        public List<int> anni;
        public List<double> Dvalori;
        public List<double> Danni;
        string tipoDato;

        #region Metodi statistici
        public int Max()
        {
            int max = 0;
            foreach (int w in valori) if (w > max) max = w;
            return max;
        }

        public double DMax()
        {
            double max = 0;
            foreach (double w in Dvalori) if (w > max) max = w;
            return max;
        }

        public int Min()
        {
            int min = int.MaxValue;
            foreach (int w in valori) if (w < min) min = w;
            return min;
        }

        public double DMin()
        {
            double min = double.MaxValue;
            foreach (double w in Dvalori) if (w < min) min = w;
            return min;
        }

        public double Media()
        {
            double media = 0;
            foreach (int w in valori) media += w;
            media = (double)(media/(double)valori.Count);
            return Math.Round(media,2);
        }

        public double DMedia()
        {
            double media = 0;
            foreach (double w in Dvalori) media += w;
            media = media / Dvalori.Count;
            return Math.Round(media, 2);
        }

        public double Varianza()
        {
            double media = Media();
            double var = 0;
            foreach (int w in valori) var = (w-media)*(w-media);
            var = (double)(var / (double)valori.Count);
            return Math.Round(var, 2);
        }

        public double DVarianza()
        {
            double media = DMedia();
            double var = 0;
            foreach (double w in Dvalori) var = (w - media) * (w - media);
            var = var / Dvalori.Count;
            return Math.Round(var, 2);
        }


        /// <returns>l'anno in cui è avvenuta tale prima ricorrenza</returns>
        public int Anno(int dato)
        {
            for (int w=0; w < valori.Count; w++)
            {
                if (valori[w] == dato)
                {
                    return anni[w];
                }
            }
            return -1;
        }

        /// <returns>l'anno in cui è avvenuta tale prima ricorrenza</returns>
        public int Anno(double dato)
        {
            for (int w = 0; w < Dvalori.Count; w++)
            {
                if (Dvalori[w] == dato)
                {
                    return (int)(Danni[w]);
                }
            }
            return -1;
        }

        public string Statistica()
        {
            string stat = "";

            stat += "Massimo: " + Max().ToString();
            int anno = Anno(Max());
            if (anno > 0) { stat += " raggiunto nel " + anno; }
            stat += "\nMinimo: " + Min().ToString();
            anno = Anno(Min());
            if (anno > 0) { stat += " raggiunto nel " + anno; }
            stat += "\nRange: " + (Max() - Min()).ToString();
            stat += "\nMedia: " + Media();
            stat += "\nVarianza: " + Varianza();

            return stat;
        }

        public string DStatistica()
        {
            string stat = "";

            stat += "Massimo: " +DMax().ToString();
            int anno = Anno(DMax());
            if (anno > 0) { stat += " raggiunto nel " + anno; }
            stat += "\nMinimo: " + DMin().ToString();
            anno = Anno(DMin());
            if (anno > 0) { stat += " raggiunto nel " + anno; }
            stat += "\nRange: " + (DMax() - DMin()).ToString();
            stat += "\nMedia: " + DMedia();
            stat += "\nVarianza: " + DVarianza();

            return stat;
        }
        #endregion


        int idComune;
        Record comune = null;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string codice;
            // recupero id del comune di cui cercare il tutto
            if (NavigationContext.QueryString.TryGetValue("comune", out codice))
            {
                idComune = Int32.Parse(codice);
                comune = CsvReader.Instance.LoadLastData(idComune);
            }
            else { MessageBox.Show("Errore nel caricamento dei dati del grafico selezionato"); }

            //recupera che tipo di dato caricare
            NavigationContext.QueryString.TryGetValue("dato", out tipoDato);

            
            
            if ((tipoDato == "natalita") || (tipoDato == "mortalita"))
            {
                //carica dati
                LoadDoubleData();
                //imposta punti grafico
                Grafico.Points = calcolaDoublePunti();
                //imposta etichette
                AggiornaDoubleEtichette();
                Statistiche.Text = "Statistiche:\n" + DStatistica();
            }
            else
            {
                //carica dati
                LoadData();
                //imposta punti grafico
                Grafico.Points = calcolaPunti();
                //imposta etichette
                AggiornaEtichette();
                Statistiche.Text = "Statistiche:\n" + Statistica();
            }

            /*
            string debug="";
            foreach(int w in anni)debug+=w.ToString();
            debug += "\n\n";
            foreach(int w in valori)debug+=w.ToString();
            MessageBox.Show(debug);
            */


            //imposta titolo pagina
            PageTitle.Text = comune.Comune;
            if (comune.Comune.Length > 9) PageTitle.FontSize = 50;
            if (comune.Comune.Length > 18) PageTitle.FontSize = 40;

            ApplicationTitle.Text = tipoDato.ToUpper();
            if (tipoDato == "natalita") ApplicationTitle.Text = "TASSO DI NATALITA'";
            if (tipoDato == "mortalita") ApplicationTitle.Text = "TASSO DI MORTALITA'";

            
        }

        public void LoadDoubleData()
        {
            Dvalori = CsvReader.Instance.LoadDoubleDati(idComune, tipoDato)[0];
            Danni = CsvReader.Instance.LoadDoubleDati(idComune, tipoDato)[1];
        }

        public void LoadData()
        {
            valori = CsvReader.Instance.LoadDati(idComune, tipoDato)[0];
            anni = CsvReader.Instance.LoadDati(idComune, tipoDato)[1];
        }



        public PointCollection calcolaPunti() {
            //List<int> pop = csv.LoadPopolazione(idComune);

            PointCollection listaPunti = new PointCollection();
            double coordX = 0;
            double coordY = 0;

            double range = Max() - Min();

            //MessageBox.Show("Primovalore "+primoValore+"\nche è uguale a: "+anni[primoValore]);
            for (int w = 0; w < valori.Count; w++)
            {
                //Calcolo ascisse
                coordX = larghezzaGriglia * w;

                //Calcolo ordinate
                double invertito = (valori[w]-Min())* altezzaCanvas / range;
                coordY = altezzaCanvas - invertito * 0.9;
                //MessageBox.Show("valore " + w + "\n(" + valori[w] + " , " + anni[w] + ")\ncioè (" + coordX + " , " + coordY+")");
                Point z = new Point(coordX, coordY);
                listaPunti.Add(z);
            }
            return listaPunti;
        }

        public PointCollection calcolaDoublePunti()
        {
            //List<int> pop = csv.LoadPopolazione(idComune);

            PointCollection listaPunti = new PointCollection();
            double coordX = 0;
            double coordY = 0;

            double range = DMax() - DMin();

            //MessageBox.Show("Primovalore "+primoValore+"\nche è uguale a: "+anni[primoValore]);
            for (int w = 0; w < Dvalori.Count; w++)
            {
                //Calcolo ascisse
                coordX = larghezzaGriglia * w;
                //Calcolo ordinate
                double invertito = (Dvalori[w] - DMin()) * altezzaCanvas / range;
                coordY = altezzaCanvas - invertito * 0.9;
                Point z = new Point(coordX, coordY);
                listaPunti.Add(z);
            }
            return listaPunti;
        }


        private void ThemeCheck()
        {
            SolidColorBrush backgroundBrush = Application.Current.Resources["PhoneBackgroundBrush"] as SolidColorBrush;

            if (backgroundBrush.Color == lightThemeBackground)
            {
                LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();

                // Create a vertical linear gradient with four stops.   
                LinearGradientBrush myVerticalGradient = new LinearGradientBrush();
                myVerticalGradient.StartPoint = new Point(0.5, 0);
                myVerticalGradient.EndPoint = new Point(0.5, 1);

                GradientStop a = new GradientStop();
                GradientStop b = new GradientStop();
                a.Color = Color.FromArgb(0xFF,0x00,0xCC,0xFF);
                b.Color = Colors.White;
                a.Offset = 0;
                b.Offset = 1;
                myVerticalGradient.GradientStops.Add(a);
                myVerticalGradient.GradientStops.Add(b);

                Caneva.Background = myVerticalGradient;
                SetRandomColorLight();
            }
            else
            {
                SetRandomColorDark();
            }
        }

        public void AggiornaEtichette() {
             int val=valori.Count;
             int min = Min();
             int max = Max();
             double step = (double)((max - min))/8.000;
             Val0.Text = Math.Round((double)min).ToString();
             Val1.Text=Math.Round(min+step).ToString();
             Val2.Text=Math.Round(min+step*2).ToString();
             Val3.Text = Math.Round(min + step * 3).ToString();
             Val4.Text = Math.Round(min + step * 4).ToString();
             Val5.Text = Math.Round(min + step * 5).ToString();
             Val6.Text = Math.Round(min + step * 6).ToString();
             Val7.Text = Math.Round(min + step * 7).ToString();
             Val8.Text = Math.Round((double)max).ToString();
             Val9.Text = Math.Round(max + step).ToString();

             Anno0.Text = anni[val - 23].ToString();
             Anno1.Text = anni[val - 22].ToString();
             Anno2.Text = anni[val - 21].ToString();
             Anno3.Text = anni[val - 20].ToString();
             Anno4.Text = anni[val - 19].ToString();
             Anno5.Text = anni[val - 18].ToString();
             Anno6.Text = anni[val - 17].ToString();
             Anno7.Text = anni[val - 16].ToString();
             Anno8.Text = anni[val - 15].ToString();
             Anno9.Text = anni[val - 14].ToString();
             Anno10.Text = anni[val - 13].ToString();
             Anno11.Text = anni[val - 12].ToString();
             Anno12.Text = anni[val - 11].ToString();
             Anno13.Text = anni[val - 10].ToString();
             Anno14.Text = anni[val - 9].ToString();
             Anno15.Text = anni[val - 8].ToString();
             Anno16.Text = anni[val - 7].ToString();
             Anno17.Text = anni[val - 6].ToString();
             Anno18.Text = anni[val - 5].ToString();
             Anno19.Text = anni[val - 4].ToString();
             Anno20.Text = anni[val - 3].ToString();
             Anno21.Text = anni[val - 2].ToString();
             Anno22.Text = anni[val - 1].ToString();
        }

        public void AggiornaDoubleEtichette()
        {
            int val = Dvalori.Count;
            double min = DMin();
            double max = DMax();
            double step = (max - min) / 8.000;
            Val0.Text = Math.Round(min,1).ToString();
            Val1.Text = Math.Round(min + step,1).ToString();
            Val2.Text = Math.Round(min + step * 2,1).ToString();
            Val3.Text = Math.Round(min + step * 3,1).ToString();
            Val4.Text = Math.Round(min + step * 4,1).ToString();
            Val5.Text = Math.Round(min + step * 5,1).ToString();
            Val6.Text = Math.Round(min + step * 6,1).ToString();
            Val7.Text = Math.Round(min + step * 7,1).ToString();
            Val8.Text = Math.Round(max,1).ToString();
            Val9.Text = Math.Round(max + step,1).ToString();

            Anno0.Text = Danni[val - 23].ToString();
            Anno1.Text = Danni[val - 22].ToString();
            Anno2.Text = Danni[val - 21].ToString();
            Anno3.Text = Danni[val - 20].ToString();
            Anno4.Text = Danni[val - 19].ToString();
            Anno5.Text = Danni[val - 18].ToString();
            Anno6.Text = Danni[val - 17].ToString();
            Anno7.Text = Danni[val - 16].ToString();
            Anno8.Text = Danni[val - 15].ToString();
            Anno9.Text = Danni[val - 14].ToString();
            Anno10.Text = Danni[val - 13].ToString();
            Anno11.Text = Danni[val - 12].ToString();
            Anno12.Text = Danni[val - 11].ToString();
            Anno13.Text = Danni[val - 10].ToString();
            Anno14.Text = Danni[val - 9].ToString();
            Anno15.Text = Danni[val - 8].ToString();
            Anno16.Text = Danni[val - 7].ToString();
            Anno17.Text = Danni[val - 6].ToString();
            Anno18.Text = Danni[val - 5].ToString();
            Anno19.Text = Danni[val - 4].ToString();
            Anno20.Text = Danni[val - 3].ToString();
            Anno21.Text = Danni[val - 2].ToString();
            Anno22.Text = Danni[val - 1].ToString();
        }

        public Graph()
        {
            InitializeComponent();
            ThemeCheck();
            larghezzaGriglia = 40;
            larghezzaCanvas = Caneva.Width;
            altezzaCanvas = Caneva.Height;
        }


        public void SetRandomColorDark()
        {
            Random generator = new Random();
            int num=generator.Next(0,9);
            switch(num)
            {
                case (0): { Grafico.Stroke = new SolidColorBrush(Colors.White); break; }
                case (1): { Grafico.Stroke = new SolidColorBrush(Colors.Red); break; }
                case (2): { Grafico.Stroke = new SolidColorBrush(Colors.Green); break; }
                case (3): { Grafico.Stroke = new SolidColorBrush(Colors.Purple); break; }
                case (4): { Grafico.Stroke = new SolidColorBrush(Colors.Yellow); break; }
                case (5): { Grafico.Stroke = new SolidColorBrush(Colors.Orange); break; }
                case (6): { Grafico.Stroke = new SolidColorBrush(Colors.Magenta); break; }
                case (7): { Grafico.Stroke = new SolidColorBrush(Colors.Cyan); break; }
                case (8): { Grafico.Stroke = new SolidColorBrush(Colors.Gray); break; }
                default: { Grafico.Stroke = new SolidColorBrush(Colors.White); break; }
            
            }
        }

        public void SetRandomColorLight()
        {
            Random generator = new Random();
            int num = generator.Next(0, 6);
            switch (num)
            {
                case (0): { Grafico.Stroke = new SolidColorBrush(Colors.Blue); break; }
                case (1): { Grafico.Stroke = new SolidColorBrush(Colors.Red); break; }
                case (2): { Grafico.Stroke = new SolidColorBrush(Colors.Green); break; }
                case (3): { Grafico.Stroke = new SolidColorBrush(Colors.Purple); break; }
                case (4): { Grafico.Stroke = new SolidColorBrush(Colors.Orange); break; }
                case (5): { Grafico.Stroke = new SolidColorBrush(Colors.Magenta); break; }
                default: { Grafico.Stroke = new SolidColorBrush(Colors.Blue); break; }

            }
        }

    }
}