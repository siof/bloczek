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

        protected Point[] _punkty = new Point[2]; //polaczenia

        public String znacznikZmiennej = "~~";

        //public Type typ
        //{
        //    get { return _typ; }
        //    set { _typ = value; }
        //}

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

            for (int i = 0; i < listaZmiennych.Count(); i++)
            {
                if (listaZmiennych[i].nazwa == temp.ToString())
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

            for (int i = 0; i < listaZmiennych.Count; i++)
            {
                if (listaZmiennych[i].nazwa.Equals(temp) == true)
                    return i;
            }

            return -1;
        }

        public void DodajNoweZmienne()
        {
            for (int i = 0; i < this.dzialania.Count; i++)
            {
                if (this.dzialania[i].nowaZmienna == true && SprawdzCzyIstniejeZmienna(this.dzialania[i].lewa) == false)
                {
                    Zmienna temp = new Zmienna();
                    this.dzialania[i].lewa = this.dzialania[i].lewa.Replace(znacznikZmiennej, "");
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
                            tmpIlElementow = Convert.ToInt32(tmpIlEl);
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

        public int NumerElementuWTablicy(String tab)
        {
            String temp = tab.ToString();
            String tmpNumerEl = "";
            if (temp.Contains('[') && temp.Contains(']'))
            {
                int tmpInd1 = temp.IndexOf('[') + 1;
                int tmpInd2 = temp.IndexOf(']');

                if (tmpInd2 - tmpInd1 > 0)
                {
                    tmpNumerEl = temp.Substring(tmpInd1, tmpInd2 - tmpInd1);

                    return Convert.ToInt32(tmpNumerEl) - 1;
                }
                else
                    return -2;
            }
            else
                return -1;
        }
    }
}
