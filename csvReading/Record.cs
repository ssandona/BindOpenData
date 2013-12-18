using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace csvReading
{
    class Record
    {

        public Record(List<string> dati)
        {
            this.Comune = dati[2];
            this.Sesso = dati[3];
            this.Anno = Int32.Parse(dati[0]);
            this.CodiceIstat = Int32.Parse(dati[1]);
            this.Morti = Parse(dati[6]);
            this.PopolazioneInizioAnno = Parse(dati[4]);
            this.NatiVivi = Parse(dati[5]);
            this.Morti = Parse(dati[6]);
            this.Iscritti = Parse(dati[7]);
            this.Cancellati = Parse(dati[8]);
            this.PopolazioneFineAnno = Parse(dati[9]);
            this.PopolazioneMedia = Parse(dati[10]);
        }



        public int Anno{get; set;}
        public string Comune { get; set; }
        public int CodiceIstat { get; set; }
        public string Sesso { get; set; }
        public int PopolazioneInizioAnno { get; set; }
        public int NatiVivi { get; set; }
        public int Morti { get; set; }
        public int Iscritti { get; set; }
        public int Cancellati { get; set; }
        public int PopolazioneFineAnno { get; set; }
        public int PopolazioneMedia { get; set; }


        public int Parse(string numero) {
            numero.Trim('.');
            List<string> a = (numero.Split('.')).ToList();
            string z = "";
            foreach (string h in a) z += h;
            return Int32.Parse(z);
        }



        public void PrintRecord() {
            MessageBox.Show("Anno: " + Anno + "\n" + "Comune: " + Comune + "\n" + "Sesso: " + Sesso + "\n" + "Codice Istat: " + CodiceIstat+"\n\n"
                + "PopInizio: " + PopolazioneInizioAnno + "\n" + "PopFine: " + PopolazioneFineAnno + "\n" + "Popmedia: " + PopolazioneMedia + "\n\n"
                + "Nati: " + NatiVivi + "\n" + "Morti: " + Morti + "\n" + "Iscritti: " + Iscritti + "\n" + "Cancellati: " + Cancellati);
        }


    /// <returns>La somma dei dati di due record ()utile perché alcuni record sono divisi dal sesso</returns>
    public Record somma(Record altro){
        string a = (PopolazioneInizioAnno + altro.PopolazioneInizioAnno).ToString();
        string b = (PopolazioneFineAnno + altro.PopolazioneFineAnno).ToString();
        string c = (PopolazioneMedia + altro.PopolazioneMedia).ToString();
        string d = (Morti + altro.Morti).ToString();
        string e = (NatiVivi + altro.NatiVivi).ToString();
        string f = (Iscritti + altro.Iscritti).ToString();
        string g = (Cancellati + altro.Cancellati).ToString();
        List<string> amen = new List<string>();
        amen.Add(Anno.ToString());
        amen.Add(CodiceIstat.ToString());
        amen.Add(Comune);
        amen.Add(Sesso);
        amen.Add(a);
        amen.Add(e);
        amen.Add(d);
        amen.Add(f);
        amen.Add(g);
        amen.Add(b);
        amen.Add(c);
        return new Record(amen);
    }


    }
}
