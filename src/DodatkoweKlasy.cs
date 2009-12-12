using System;
using System.Collections.Generic;
using System.Linq;

namespace Okienka
{
    class Zmienna
    {
        private String _nazwa;
        private String _wartosc;
        private Type _typ;

        public String nazwa
        {
            get { return _nazwa; }
            set { _nazwa = value; }
        }

        public String wartosc
        {
            get { return _wartosc; }
            set { _wartosc = value; }
        }

        public Type typ
        {
            get { return _typ; }
            set { _typ = value; }
        }
    }
}