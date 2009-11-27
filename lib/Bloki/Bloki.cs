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

    public class Bloki : UserControl
    {
        protected Type typ;
        //protected String nastepny;
        //protected String poprzedni;
        protected Bloki nastepny;
        protected Bloki poprzedni;
        protected String nazwa;

        protected tryby _tryb;
        protected Graphics graph;

        protected Point[] punkty = new Point[2]; //polaczenia

        
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
    }
}
