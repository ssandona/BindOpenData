using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csvReading.Model
{
    public class Comune
    {
        private int istat;
        public int Istat
        {
            get{
                return istat;
            }
        }

        private string nome;
        public string Nome {
            get {
                return nome;
            }
        }

        private int numAbitanti;
        public int NumAbitanti
        {
            get{
                return numAbitanti;
            }
        }

        private string testo;
        public string Testo{
            get{
                return testo;
            }
        }

        private int morti;
        public int Morti
        {
            get{
                return morti;
            }
        }
        private int nati;
        public int Nati
        {
            get{
                return nati;
            }
        }

        private double tassoNatalita;
        public double TassoNatalita
        {
            get{
                return tassoNatalita;
            }
        }

        private double tassoMortalita;
        public double TassoMortalita
        {
            get{
                return tassoMortalita;
            }
        }

        private string anno;
        public string Anno{
            get{
                return anno;
            }
        }

        private int immigrati;
        public int Immigrati
        {
            get{
                return immigrati;
            }
        }

        private int emigrati;
        public int Emigrati
        {
            get{
                return emigrati;
            }
        }

        private string percentuale;
        public string Percentuale{
            get{
                return percentuale;
            }
        }

        private int fontSize;
        public int FontSize {
            get {
                return fontSize;
            }
        }

        public Comune(int istat,string nome,int numAbitanti,string testo,int morti,int nati,double tassoNatalita,double tassoMortalita,string anno,int immigrati,int emigrati,string percentuale,int fontSize){
            this.nome = nome;
            this.istat= istat;
            this.numAbitanti= numAbitanti;
            this.testo = testo;
            this.morti = morti;
            this.nati = nati;
            this.tassoNatalita = tassoNatalita;
            this.tassoMortalita = tassoMortalita;
            this.anno = anno;
            this.immigrati = immigrati;
            this.emigrati = emigrati;
            this.percentuale=percentuale;
            this.fontSize = fontSize;
        }

    }                 
}
