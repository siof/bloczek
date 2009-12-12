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

    public class Bloki : UserControl
    {
        protected Type typ;

        protected Bloki nastepny,_nastepnaLinia;
        protected Bloki poprzedni,_poprzedniaLinia;
        protected String nazwa;

        protected tryby _tryb;
        protected Graphics graph;
        protected IList<String> zmienne = new List<String>();

        public IList<Zmienna> listaZmiennych;
        protected Point[] _punkty = new Point[2]; //polaczenia
        public RichTextBox txt = new RichTextBox();

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

                return;
            }

        }
    }
}
