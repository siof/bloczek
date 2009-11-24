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
        private BlokSTART blokStart;
        private BlokSTOP blokStop;
        private BlokDecyzyjny blokDecyzyjny;
        private BlokObliczeniowy blokObliczeniowy;
        private BlokWeWy blokWeWy;

        protected Type typ;
        protected String nastepny;
        protected String poprzedni;
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

        public String nastepnyBlok
        {
            get { return nastepny; }
            set { nastepny = value; }
        }

        public String poprzedniBlok
        {
            get { return poprzedni; }
            set { poprzedni = value; }
        }

        public UserControl blok
        {
            get
            {
                if (typ == typeof(BlokSTART))
                    return ((UserControl)blokStart);
                else
                {
                    if (typ == typeof(BlokSTOP))
                        return ((UserControl)blokStop);
                    else
                    {
                        if (typ == typeof(BlokDecyzyjny))
                            return ((UserControl)blokDecyzyjny);
                        else
                        {
                            if (typ == typeof(BlokObliczeniowy))
                                return ((UserControl)blokObliczeniowy);
                            else
                            {
                                if (typ == typeof(BlokWeWy))
                                    return ((UserControl)blokWeWy);
                                else
                                    return null;
                            }
                        }
                    }
                }
            }

            set
            {
                if (typ == typeof(BlokSTART))
                    blokStart = (BlokSTART)value;

                if (typ == typeof(BlokSTOP))
                    blokStop = (BlokSTOP)value;

                if (typ == typeof(BlokDecyzyjny))
                    blokDecyzyjny = (BlokDecyzyjny)value;

                if (typ == typeof(BlokObliczeniowy))
                    blokObliczeniowy = (BlokObliczeniowy)value;

                if (typ == typeof(BlokWeWy))
                    blokWeWy = (BlokWeWy)value;
            }
        }
    }
}
