using csvReading.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace csvReading.Model
{
    class CsvReader
    {

#region Singleton definition
        /* Singleton*/
    private static CsvReader instance = null;


    

    private CsvReader()
    {
    }

    public static CsvReader Instance
    {
        get
        {
            if (instance==null)
            {
                instance = new CsvReader();
            }
            return instance;
        }
    }

#endregion


        /// <returns>Il numero di abitanti di tale provincia (cod istat) in tale anno</returns>
        public int AbitantiProvincia(int codProv, int anno)
        {
            int contatore = 0;
            try
            {
                var ResourceStream = Application.GetResourceStream(new Uri("file.txt", UriKind.Relative));

                if (ResourceStream != null)
                {
                    using (Stream myFileStream = ResourceStream.Stream)
                    {

                        if (myFileStream.CanRead)
                        {
                            StreamReader myStreamReader = new StreamReader(myFileStream);

                            string line;
                            myStreamReader.ReadLine(); //spreco intestazione

                            //Trova tutti i record con comune e anno ivi specificati, scrivi sempre i dati sullo stesso oggetto
                            line = myStreamReader.ReadLine();
                            while ((line = myStreamReader.ReadLine()) != null)
                            {
                                //substring ha come argomenti il carattere giusto prima e la lungh della sottostringa
                                if ((line.Substring(0, 4) == anno.ToString()) && (line.Substring(5, 2) == codProv.ToString()))
                                {
                                    List<string> dato = new List<string>();
                                    dato = SplitLine(line);
                                    contatore += Parse(dato[10]);
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\n" + e.Data + "\n\n" + e.StackTrace);
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return contatore;
        }

        /// <returns>L'ultimo record temporale di un comune</returns>
        public Record LoadLastData(int comune)
        {
            Record ultimo = null;
            Record penultimo = null;
            try
            {

                var ResourceStream = Application.GetResourceStream(new Uri("file.txt", UriKind.Relative));

                if (ResourceStream != null)
                {
                    using (Stream myFileStream = ResourceStream.Stream)
                    {

                        if (myFileStream.CanRead)
                        {
                            StreamReader myStreamReader = new StreamReader(myFileStream);

                            string line;
                            myStreamReader.ReadLine(); //spreco intestazione
                            
                            //Trova tutti i record con comune e anno ivi specificati, scrivi sempre i dati sullo stesso oggetto
                            line = myStreamReader.ReadLine();
                            while ((line = myStreamReader.ReadLine()) != null)
                            {
                                //substring ha come argomenti il carattere giusto prima e la lungh della sottostringa
                                if (line.Substring(5, 5) == comune.ToString())
                                {
                                    if(ultimo!=null)penultimo = ultimo;
                                    ultimo = new Record(SplitLine(line));
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\n" + e.Data + "\n\n" + e.StackTrace);
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            if (ultimo.Sesso == "Non rilevato") return ultimo;
            return penultimo.somma(ultimo);
        }

        /// <returns>Una lista di record di tale comune</returns>
        public List<Record> LoadRecord(int comune) {
            List<Record> dati = new List<Record>();
            try
            {

                var ResourceStream = Application.GetResourceStream(new Uri("file.txt", UriKind.Relative));

                if (ResourceStream != null)
                {
                    using (Stream myFileStream = ResourceStream.Stream)
                    {
                        if (myFileStream.CanRead)
                        {
                            StreamReader myStreamReader = new StreamReader(myFileStream);
                            string line;
                            myStreamReader.ReadLine(); //spreco intestazione

                            line = myStreamReader.ReadLine();
                            while ((line = myStreamReader.ReadLine()) != null)
                            {
                                //substring ha come argomenti il carattere giusto prima e la lungh della sottostringa
                                if (line.Substring(5, 5) == comune.ToString())
                                {
                                    dati.Add(new Record(SplitLine(line)));
                                }
                            }
                            //ho scorso tutti i valori
                        }
                    }
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\n" + e.Data + "\n\n" + e.StackTrace);
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return dati;

        
        }

        /// <returns>Data una lista di record restituisce una lista
        /// con un record per anno con i dati dei due sessi sommati</returns>
        public List<Record> AggregaSessi(List<Record> lista)
        {
            Record primoVal = null;
            List<Record> listaAggregata = new List<Record>();

            for(int w=0;w<lista.Count;w++)
            {
                

                
                if(((lista[w].Sesso=="Femmina") ||(lista[w].Sesso=="Maschio"))&&(primoVal == null))
                {
                    primoVal = lista[w];
                    continue;
                }

                if(((lista[w].Sesso=="Femmina") ||(lista[w].Sesso=="Maschio"))&&(primoVal != null))
                {
                    listaAggregata.Add(primoVal.somma(lista[w]));
                    primoVal = null;
                }


                if (lista[w].Sesso == "Non rilevato")
                {
                    listaAggregata.Add(lista[w]);
                }

                
                 
            }//end for
            return listaAggregata;
        }

        /// <returns>Un array di due liste di interi, la prima indica i valori e la seconda gli anni</returns>
        public List<int>[] LoadDati(int comune, string dato)
        {
            List<int> listaValori = new List<int>();
            List<int> listaAnni = new List<int>();
            
            List<Record> dati = LoadRecord(comune);
            List<Record> datiAggregati = AggregaSessi(dati);


            //riempi listaAnni
            foreach (Record w in datiAggregati) listaAnni.Add(w.Anno);

            //riempi listaValori
            switch (dato)
            {
                case ("popolazione"): {
                    foreach(Record w in datiAggregati)
                    listaValori.Add(w.PopolazioneMedia); break;
                }
                case ("nati"):
                    {
                        foreach (Record w in datiAggregati)
                            listaValori.Add(w.NatiVivi); break;
                    }
                case ("morti"):
                    {
                        foreach (Record w in datiAggregati)
                            listaValori.Add(w.Morti); break;
                    }
                case ("iscritti"):
                    {
                        foreach (Record w in datiAggregati)
                            listaValori.Add(w.Iscritti); break;
                    }
                case ("cancellati"):
                    {
                        foreach (Record w in datiAggregati)
                            listaValori.Add(w.Cancellati); break;
                    }
                default:
                    {
                        MessageBox.Show("Errore nel caricamento del dato"); break;
                    }
            }

            //compongo l'array di 2 liste valori/anni
            List<int>[] array = new List<int>[2];
            array[0] = listaValori;
            array[1] = listaAnni;
            return array;

        }


        /// <returns>Un array di due liste double&int, la prima indica i valori e la seconda gli anni</returns>
        public List<double>[] LoadDoubleDati(int comune, string dato)
        {
            List<double> listaValori = new List<double>();
            List<double> listaAnni = new List<double>();

            List<Record> dati = LoadRecord(comune);
            List<Record> datiAggregati = AggregaSessi(dati);


            //riempi listaAnni
            foreach (Record w in datiAggregati) listaAnni.Add(w.Anno);

            //riempi listaValori
            switch (dato)
            {
                case ("natalita"):
                    {
                        foreach (Record w in datiAggregati)
                        {
                            double nati = w.NatiVivi;
                            double tot = w.PopolazioneMedia;
                            listaValori.Add(Math.Round((nati / tot) * 1000, 2));
                        }
                        break;
                    }
                case ("mortalita"):
                    {
                        foreach (Record w in datiAggregati)
                        {
                            double morti = w.Morti;
                            double tot = w.PopolazioneMedia;
                            listaValori.Add(Math.Round((morti / tot) * 1000,2));
                        } 
                        break;
                    }
                default:
                    {
                        MessageBox.Show("Errore nel caricamento del dato"); break;
                    }
            }

            //compongo l'array di 2 liste valori/anni
            List<double>[] array = new List<double>[2];
            array[0] = listaValori;
            array[1] = listaAnni;
            return array;
        }




        /// <returns>Una lista di stringhe date dall'esplosione della stringa principale</returns>
        public List<string> SplitLine(string csvLine)
        {
            List<string> line = new List<string>();
            string[] elements = csvLine.Split(';');
            foreach (string s in elements) line.Add(s);
            return line;
        }


        /// <returns>Una lista di interi con tutti i codici di tutti i comuni con lo stesso ordine
        /// della lista restituita dal metodo caricaComuni(). Così se ottengo due liste a e b, a[5]
        /// sarà il codice relativo al comune b[5].</returns>
        public List<int> caricaIstat()
        {
            List<int> comuni = new List<int>();
            try
            {

                var ResourceStream = Application.GetResourceStream(new Uri("file.txt", UriKind.Relative));

                if (ResourceStream != null)
                {
                    using (Stream myFileStream = ResourceStream.Stream)
                    {

                        if (myFileStream.CanRead)
                        {
                            StreamReader myStreamReader = new StreamReader(myFileStream);

                            string line;
                            myStreamReader.ReadLine(); //spreco intestazione

                            //Trova tutti i record con comune e anno ivi specificati, scrivi sempre i dati sullo stesso oggetto
                            line = myStreamReader.ReadLine();
                            while ((line = myStreamReader.ReadLine()) != null)
                            {
                                //substring ha come argomenti il carattere giusto prima e la lungh della sottostringa
                                if(line.Substring(0, 4) == "1990")
                                {
                                    comuni.Add(Int32.Parse(line.Substring(5, 5)));
                                }//end if body
                            }//end while body
                        }//end if leggi stream
                    }//end of using
                }//end if
            }//end try

            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\n" + e.Data + "\n\n" + e.StackTrace);
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return comuni;
}

        public List<string> caricaComuni()
        {
            List<string> comuni = new List<string>();
            try
            {

                var ResourceStream = Application.GetResourceStream(new Uri("file.txt", UriKind.Relative));

                if (ResourceStream != null)
                {
                    using (Stream myFileStream = ResourceStream.Stream)
                    {

                        if (myFileStream.CanRead)
                        {
                            StreamReader myStreamReader = new StreamReader(myFileStream);

                            string line;
                            myStreamReader.ReadLine(); //spreco intestazione

                            //Trova tutti i record con comune e anno ivi specificati, scrivi sempre i dati sullo stesso oggetto
                            line = myStreamReader.ReadLine();
                            while ((line = myStreamReader.ReadLine()) != null)
                            {
                                //substring ha come argomenti il carattere giusto prima e la lungh della sottostringa
                                if (line.Substring(0, 4) == "1990")
                                {
                                    string[] oggetto = new string[6];
                                    oggetto = line.Split(';');
                                    comuni.Add(oggetto[2]);
                                }//end if body
                            }//end while body
                        }//end if leggi stream
                    }//end of using
                }//end if
            }//end try

            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\n" + e.Data + "\n\n" + e.StackTrace);
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return comuni;
        }

        public int Parse(string numero)
        {
            numero.Trim('.');
            List<string> a = (numero.Split('.')).ToList();
            string z = "";
            foreach (string h in a) z += h;
            return Int32.Parse(z);
        }

        public ObservableCollection<Comune> ComuniDaProvincia(string prov)
        {
            ObservableCollection<Comune> comuniAttuali = new ObservableCollection<Comune>();
            List<string> comuni;
            List<int> codici;
            codici = CsvReader.Instance.caricaIstat();
            comuni = CsvReader.Instance.caricaComuni();
            switch (prov)
            {
                case "ve":
                    {
                        for (int z = 0; z < codici.Count; z++)
                        {
                            if (codici[z] / 1000 == 27)
                            {
                                comuniAttuali.Add(ComuneFromId(codici[z], comuni[z]));
                            }
                        }//end of for
                        break;
                    }//end of case ve

                case "pd":
                    {
                        for (int z = 0; z < codici.Count; z++)
                        {
                            if (codici[z] / 1000 == 28)
                            {
                                comuniAttuali.Add(ComuneFromId(codici[z], comuni[z]));
                            }
                        }//end of for
                        break;
                    }//end of case pd

                case "tv":
                    {
                        for (int z = 0; z < codici.Count; z++)
                        {
                            if (codici[z] / 1000 == 26)
                            {
                                comuniAttuali.Add(ComuneFromId(codici[z], comuni[z]));
                            }
                        }//end of for
                        break;
                    }//end of case tv

                case "ro":
                    {
                        for (int z = 0; z < codici.Count; z++)
                        {
                            if (codici[z] / 1000 == 29)
                            {
                                comuniAttuali.Add(ComuneFromId(codici[z], comuni[z]));
                            }
                        }//end of for
                        break;
                    }//end of case ro

                case "vi":
                    {
                        for (int z = 0; z < codici.Count; z++)
                        {
                            if (codici[z] / 1000 == 24)
                            {
                                comuniAttuali.Add(ComuneFromId(codici[z], comuni[z]));
                            }
                        }//end of for
                        break;
                    }//end of case vi

                case "bl":
                    {
                        for (int z = 0; z < codici.Count; z++)
                        {
                            if (codici[z] / 1000 == 25)
                            {
                                comuniAttuali.Add(ComuneFromId(codici[z], comuni[z]));
                            }
                        }//end of for
                        break;
                    }//end of case bl

                case "vr":
                    {
                        for (int z = 0; z < codici.Count; z++)
                        {
                            if (codici[z] / 1000 == 23)
                            {
                                comuniAttuali.Add(ComuneFromId(codici[z], comuni[z]));
                            }
                        }//end of for
                        break;
                    }//end of case vr

                default:
                    {
                        break;
                    }
            }
            // Sample data; replace with real data
            return comuniAttuali;
        }

        private Comune ComuneFromId(int idComune, string _nome){
            Record dati = LoadLastData(idComune);
            string nome = _nome;
            int istat = idComune;
            int numAbitanti = dati.PopolazioneMedia;
            string testo = dati.Comune;
            int morti = dati.Morti;
            int nati = dati.NatiVivi;
            double nat = dati.NatiVivi;
            double mort = dati.Morti;
            double tot = dati.PopolazioneMedia;
            double tassoNatalita = Math.Round((nat / tot) * 1000, 2);
            double tassoMortalita = Math.Round((mort / tot) * 1000, 2);
            string anno = "Dati aggiornati al " + dati.Anno;
            int immigrati = dati.Iscritti;
            int emigrati = dati.Cancellati;
            int _istat = dati.CodiceIstat / 1000;
            int _anno = dati.Anno;
            int abitanti = CsvReader.Instance.AbitantiProvincia(_istat, _anno);
            //MessageBox.Show("Abitati in provincia: "+abitanti.ToString());
            double popol = (double)(dati.PopolazioneMedia);
            double percent = 100 * popol / (double)abitanti;
            double _percentuale = Math.Round(percent, 2);
            //imposto articolo per la percentuale
            string articolo = "il ";
            if (_percentuale < 1) articolo = "lo ";
            if (_percentuale < 2 && _percentuale >= 1) articolo = "l'";
            if (_percentuale < 9 && _percentuale >= 8) articolo = "l'";
            if (_percentuale < 12 && _percentuale >= 11) articolo = "l'";
            int codProv = _istat;
            string provincia = "";
            string percentuale = "";
            switch (codProv)
            {
                case (23):
                    {
                        provincia = "Verona";
                        break;
                    }
                case (24):
                    {
                        provincia = "Vicenza";
                        break;
                    }
                case (25):
                    {
                        provincia = "Belluno";
                        break;
                    }
                case (26):
                    {
                        provincia = "Treviso";
                        break;
                    }
                case (27):
                    {
                        provincia = "Venezia";
                        break;
                    }
                case (28):
                    {
                        provincia = "Padova";
                        break;
                    }
                case (29):
                    {
                        provincia = "Rovigo";
                        break;
                    }
            }
            int fontSize;
            percentuale = "Cioè " + articolo + _percentuale + "% della provincia di " + provincia + ".";
            if (dati.Comune.Length > 9)
                fontSize = 50;
            else fontSize = 60;
            Comune c=new Comune(istat,nome,numAbitanti,testo,morti,nati,tassoNatalita,tassoMortalita,anno,immigrati,emigrati,percentuale,fontSize);
            return c;
        }


    }
}
