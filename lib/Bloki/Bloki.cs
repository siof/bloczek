using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;


namespace libbloki
{
    public enum tryby { normal, zaznaczony, aktualny };
    public enum strzalkaUpDown { up, down, none };
    public enum strzalkaLeftRight { left, right, none };
    
    [Serializable]
    public class Polaczenie
    {
        private String _nazwaOD, _nazwaDO;
        private int _indeksOD, _indeksDO;
        [field: NonSerialized]
        private Bloki _refOD, _refDO, _refLinia1, _refLinia2;

        public String nazwaOD
        {
            get { return _nazwaOD; }
            set { _nazwaOD = value; }
        }

        public String nazwaDO
        {
            get { return _nazwaDO; }
            set { _nazwaDO = value; }
        }
        public int IndeksDO
        {
            get { return _indeksDO; }
            set { _indeksDO = value; }
        }

        public int IndeksOD
        {
            get { return _indeksOD; }
            set { _indeksOD = value; }
        }

        public Bloki RefLinia2
        {
            get { return _refLinia2; }
            set { _refLinia2 = value; }
        }

        public Bloki RefLinia1
        {
            get { return _refLinia1; }
            set { _refLinia1 = value; }
        }
        

        public Bloki RefDO
        {
            get { return _refDO; }
            set { _refDO = value; }
        }

        public Bloki RefOD
        {
            get { return _refOD; }
            set { _refOD = value; }
        }

        public Polaczenie(Bloki RefOD, int IndeksOD,String nazwaOD, Bloki RefDO, int IndeksDO,String nazwaDO, Bloki RefLinia1, Bloki RefLinia2)
        {
            this.RefOD = RefOD;
            this.IndeksOD = IndeksOD;
            this.RefDO = RefDO;
            this.IndeksDO = IndeksDO;
            this.RefLinia1 = RefLinia1;
            this.RefLinia2 = RefLinia2;
            this.nazwaOD = nazwaOD;
            this.nazwaDO = nazwaDO;
        }
    }
	
    [Serializable]
    public class Bloki : UserControl
    {
        private Type _typBloku;

        protected Bloki[] nastepny = new Bloki[2];
        protected Bloki[] _nastepnaLinia = new Bloki[2];
        protected IList<Bloki> poprzedni = new List<Bloki>();
        protected IList<Bloki> _poprzedniaLinia = new List<Bloki>();
		
        protected String nazwa;

        protected tryby _tryb;
        protected Graphics graph;

        public IList<Zmienna> listaZmiennych;
        public IList<Dzialanie> dzialania = new List<Dzialanie>();
        public IList<Zmienna> dodaneZmienne = new List<Zmienna>();

        protected Point[] _punkty = new Point[2]; //polaczenia

        public String znacznikZmiennej = "~~";

        public Point[] punkty
        
        {
            get { return _punkty; }
            set { _punkty = value; }
        }

        public tryby tryb
        {
            get { return _tryb; }
            set
            {
                _tryb = value;
                Rectangle rect = new Rectangle(1, 1, 150, 75);
                PaintEventArgs pe = new PaintEventArgs(graph, rect);
                this.OnPaint(pe);

                pe.Dispose();
            }
        }

        public Type typBloku
        {
            get { return _typBloku; }
            set { _typBloku = value; }
        }

        public String nazwaBloku
        {
            get { return nazwa; }
            set { nazwa = value; }
        }

        public Bloki[] nastepnyBlok
        {
            get { return nastepny; }
            set { nastepny = value; }
        }

        public IList<Bloki> poprzedniBlok
        {
            get { return poprzedni; }
            set { poprzedni = value; }
        }

        public Bloki[] nastepnaLinia
        {
            get { return _nastepnaLinia; }
            set { _nastepnaLinia = value; }
        }

        public IList<Bloki> poprzedniaLinia
        {
            get { return _poprzedniaLinia; }
            set { _poprzedniaLinia = value; }
        }

        public bool SprawdzCzyIstniejeZmienna(String zmienna)
        {
            if (zmienna == null || listaZmiennych == null)
                return false;

            String temp = zmienna.ToString();

            if (temp.Contains(znacznikZmiennej) == true)
                temp = temp.Replace(znacznikZmiennej, "");

            if (temp.Contains('[') == true && temp.Contains(']') == true)
                temp = temp.Remove(temp.IndexOf('['));

            if (temp.Contains(' ') == true)
                temp = temp.Replace(" ", "");

            for (int i = 0; i < listaZmiennych.Count; i++)
            {
                if (listaZmiennych[i].nazwa.Equals(temp) == true)
                    return true;
            }

            return false;
        }

        public int ZnajdzZmienna(String zmienna)
        {
            if (zmienna == null)
                return -1;
            
            String temp = zmienna.ToString();

            if (temp.Contains(znacznikZmiennej) == true)
                temp = temp.Replace(znacznikZmiennej, "");

            if (temp.Contains('[') == true && temp.Contains(']') == true)
                temp = temp.Remove(temp.IndexOf('['));

            if (temp.Contains(' ') == true)
                temp = temp.Replace(" ", "");

            for (int i = 0; i < listaZmiennych.Count; i++)
            {
                if (listaZmiennych[i].nazwa.Equals(temp) == true)
                    return i;
            }

            return -1;
        }

        public void DodajNoweZmienne()
        {
            if (dzialania == null)
            {
                MessageBox.Show("Bloki: DodajNoweZmienne: brak listy dzialan");
                return;
            }

            for (int i = 0; i < this.dzialania.Count; i++)
            {
                if (this.dzialania[i].nowaZmienna == true && SprawdzCzyIstniejeZmienna(this.dzialania[i].lewa) == false)
                {
                    Zmienna temp = new Zmienna();
                    
                    if (dzialania[i].lewa == null)
                    {
                        MessageBox.Show("Bloki: DodajNoweZmienne: lewa nie istnieje");
                        return;
                    }

                    this.dzialania[i].lewa = this.dzialania[i].lewa.Replace(znacznikZmiennej, "");
                    
                    if (this.dzialania[i].lewa.Contains(' '))
                        this.dzialania[i].lewa = this.dzialania[i].lewa.Replace(" ", "");
                    
                    temp.nazwa = dzialania[i].lewa.ToString();
                    dzialania[i].nowaZmienna = false;

                    if (this.dzialania[i].lewa.Contains('[') == true && this.dzialania[i].lewa.Contains(']') == true)
                        temp.tablica = true;

                    if (dzialania[i].dodatkowe != null)
                    {
                        int tmpInd1, tmpInd2;
                        String tmpIlEl = "";
                        int tmpIlElementow = 0;

                        if (temp.tablica == true)
                        {
                            tmpInd1 = temp.nazwa.IndexOf('[') + 1;
                            tmpInd2 = temp.nazwa.IndexOf(']');
                            tmpIlEl = temp.nazwa.Substring(tmpInd1, tmpInd2 - tmpInd1);

                            try
                            {
                                tmpIlElementow = Convert.ToInt32(tmpIlEl);
                            }
                            catch
                            {
                                MessageBox.Show("Bloki: DodajNoweZmienne: blad konwersji indexu\n index musi byc liczba calkowita");
                                return;
                            }

                            temp.nazwa = temp.nazwa.Remove(tmpInd1 - 1);

                            temp.iloscElTablicy = tmpIlElementow;
                        }

                        switch (dzialania[i].dodatkowe)
                        {
                            case "int":
                                temp.typ = typeof(int);
                                if (temp.tablica == true)
                                {
                                    for (int j = 0; j < tmpIlElementow; j++)
                                        temp.wartosci.Add("0");
                                }
                                else
                                    temp.wartosc = "0";
                                break;

                            case "double":
                                temp.typ = typeof(double);
                                if (temp.tablica == true)
                                {
                                    for (int j = 0; j < tmpIlElementow; j++)
                                        temp.wartosci.Add("0.0");
                                }
                                else
                                    temp.wartosc = "0.0";
                                break;

                            case "String":
                                temp.typ = typeof(String);
                                if (temp.tablica == true)
                                {
                                    for (int j = 0; j < tmpIlElementow; j++)
                                        temp.wartosci.Add("");
                                }
                                else
                                    temp.wartosc = "";
                                break;

                            default:
                                break;
                        }
                    }

                    if (listaZmiennych != null)
                        listaZmiennych.Add(temp);
                }
            }
        }

        public void DodajNoweZmienne(Dzialanie dzial)
        {
            if (dzial == null)
            {
                MessageBox.Show("Bloki: DodajNoweZmienne|dzialanie: dzial nie istnieje");
                return;
            }

            if (dzial.nowaZmienna == true && dzial.lewa == null)
            {
                MessageBox.Show("Bloki: DodajNoweZmienne|dzialanie: lewa nie istnieje");
                return;
            }

            if (dzial.nowaZmienna == true && SprawdzCzyIstniejeZmienna(dzial.lewa) == false)
            {
                Zmienna temp = new Zmienna();
                dzial.lewa = dzial.lewa.Replace(znacznikZmiennej, "");

                if (dzial.lewa.Contains(' '))
                    dzial.lewa = dzial.lewa.Replace(" ", "");

                temp.nazwa = dzial.lewa.ToString();
                dzial.nowaZmienna = false;

                if (dzial.lewa.Contains('[') == true && dzial.lewa.Contains(']') == true)
                    temp.tablica = true;

                if (dzial.dodatkowe != null)
                {
                    int tmpInd1, tmpInd2;
                    String tmpIlEl = "";
                    int tmpIlElementow = 0;

                    if (temp.tablica == true)
                    {
                        tmpInd1 = temp.nazwa.IndexOf('[') + 1;
                        tmpInd2 = temp.nazwa.IndexOf(']');
                        tmpIlEl = temp.nazwa.Substring(tmpInd1, tmpInd2 - tmpInd1);

                        try
                        {
                            tmpIlElementow = Convert.ToInt32(tmpIlEl);
                        }
                        catch
                        {
                            MessageBox.Show("Bloki: DodajNoweZmienne|dzialanie: blad konwersji indexu\n index musi byc liczba calkowita");
                            return;
                        }

                        temp.nazwa = temp.nazwa.Remove(tmpInd1 - 1);

                        temp.iloscElTablicy = tmpIlElementow;
                    }

                    switch (dzial.dodatkowe)
                    {
                        case "int":
                            temp.typ = typeof(int);
                            if (temp.tablica == true)
                            {
                                for (int j = 0; j < tmpIlElementow; j++)
                                    temp.wartosci.Add("0");
                            }
                            else
                                temp.wartosc = "0";
                            break;

                        case "double":
                            temp.typ = typeof(double);
                            if (temp.tablica == true)
                            {
                                for (int j = 0; j < tmpIlElementow; j++)
                                    temp.wartosci.Add("0.0");
                            }
                            else
                                temp.wartosc = "0.0";
                            break;

                        case "String":
                            temp.typ = typeof(String);
                            if (temp.tablica == true)
                            {
                                for (int j = 0; j < tmpIlElementow; j++)
                                    temp.wartosci.Add("");
                            }
                            else
                                temp.wartosc = "";
                            break;

                        default:
                            break;
                    }
                }

                if (listaZmiennych != null)
                    listaZmiennych.Add(temp);
                dodaneZmienne.Add(temp);
            }
        }

        public int NumerElementuWTablicy(String tab)
        {
            if (tab == null)
            {
                MessageBox.Show("Bloki: NumerElementuWTablicy: tab nie istnieje");
                return -4;
            }

            String temp = tab.ToString();
            String tmpNumerEl = "";
            if (temp.Contains('[') && temp.Contains(']'))
            {
                int tmpInd1 = temp.IndexOf('[') + 1;
                int tmpInd2 = temp.IndexOf(']');

                if (tmpInd2 - tmpInd1 > 0)
                {
                    tmpNumerEl = znacznikZmiennej + temp.Substring(tmpInd1, tmpInd2 - tmpInd1) + znacznikZmiennej;

                    foreach (Zmienna zm in listaZmiennych)
                    {
                        if (zm.tablica == false)
                        {
                            temp = znacznikZmiennej + zm.nazwa + znacznikZmiennej;
                            if (temp == tmpNumerEl)
                                tmpNumerEl = tmpNumerEl.Replace(tmpNumerEl, zm.wartosc);
                        }
                    }
                    tmpNumerEl = tmpNumerEl.Replace(znacznikZmiennej, "");

                    int tempInd = 0;

                    try
                    {
                        tempInd = Convert.ToInt32(tmpNumerEl) - 1;
                    }
                    catch
                    {
                        MessageBox.Show("Bloki: NumerElementuWTablicy: blad przy konwersji na int");
                        return -4;
                    }

                    return tempInd;
                }
                else
                    return -2;
            }
            else
                return -1;
        }
    }
}
