﻿using System;
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

    public class Bloki : UserControl
    {
        protected Type typ;

        protected Bloki nastepny,_nastepnaLinia;
        protected Bloki poprzedni,_poprzedniaLinia;
        protected String nazwa;

        protected tryby _tryb;
        protected Graphics graph;

        public IList<Zmienna> listaZmiennych;
        public IList<Działanie> dzialania = new List<Działanie>();

        protected Point[] _punkty = new Point[2]; //polaczenia
        public RichTextBox txt = new RichTextBox();

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
            get { return typ; }
            set { typ = value; }
        }

        public String nazwaBloku
        {
            get { return nazwa; }
            set { nazwa = value; }
        }

        public Bloki nastepnyBlok
        {
            get { return nastepny; }
            set { nastepny = value; }
        }

        public Bloki poprzedniBlok
        {
            get { return poprzedni; }
            set { poprzedni = value; }
        }

        public Bloki nastepnaLinia
        {
            get { return _nastepnaLinia; }
            set { _nastepnaLinia = value; }
        }
        public Bloki poprzedniaLinia
        {
            get { return _poprzedniaLinia; }
            set { _poprzedniaLinia = value; }
        }

        public void Wykonaj()
        {
            //żeby nie wykonywać reszty kodu jeśli blok nic nie robi
            if (typBloku == typeof(BlokSTART) || typBloku == typeof(BlokSTOP))
                return;

            if (typBloku == typeof(BlokObliczeniowy))
            {
                ((BlokObliczeniowy)this).Wykonaj();

                return;
            }

            if (typBloku == typeof(BlokWeWy))
            {
                //sprawdzić akcje po kolei i wyświetlać lub pobierać dane (dodatkowe formy sie przydadzą)
                ((BlokWeWy)this).Wykonaj();

                return;
            }

            if (typBloku == typeof(BlokDecyzyjny))
            {
                ((BlokDecyzyjny)this).Wykonaj();

                return;
            }
        }

        public bool SprawdzCzyIstniejeZmienna(String zmienna)
        {
            if (zmienna == null)
                return false;

            String temp = zmienna.ToString();

            if (temp.Contains(znacznikZmiennej) == true)
                temp.Replace(znacznikZmiennej, "");

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
                temp.Replace(znacznikZmiennej, "");

            for (int i = 0; i < listaZmiennych.Count; i++)
            {
                if (listaZmiennych[i].nazwa.Equals(zmienna) == true)
                    return i;
            }

            return -1;
        }

        public void DodajNoweZmienne()
        {
            for (int i = 0; i < dzialania.Count; i++)
            {
                if (dzialania[i].nowaZmienna == true)
                {
                    Zmienna temp = new Zmienna();
                    temp.nazwa = dzialania[i].lewa;

                    listaZmiennych.Add(temp);
                }
            }
        }
    }
}