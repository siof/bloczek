using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using libbloki;
using System.Windows.Forms;

namespace Okienka
{
    class Bloki
    {
        private BlokSTART blokStart;
        private BlokSTOP blokStop;
        private BlokDecyzyjny blokDecyzyjny;
        private BlokObliczeniowy blokObliczeniowy;
        private BlokWeWy blokWeWy;

        private Type typ;
        private String nastepny;
        private String poprzedni;
        private String nazwa;

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
