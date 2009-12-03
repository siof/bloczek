using System;
using System.Collections.Generic;
using System.Linq;

namespace libbloki
{
    public class Zmienna
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

    public class Działanie
    {
        private bool _nowaZmienna = false;           //czy pierwszy z lewej jest nową zmienną (jeśli nie to listBox ze zmiennymi)
        private String _lewa;         //nazwa lewej zmiennej

        private String _dzialanie1;   //pierwsze działanie (z listboxa jeśli jest więcej możliwych)

        private bool _srodekZmienna = false;         //czy srodkowa wartosc jest zmienna (jesli tak to listbox ze zmiennymi)
        private String _srodek;

        private String _dzialanie2;   //drugie dzialanie (moze byc puste)

        private bool _prawaZmienna = false;          //przy prawa wartosc jest zmienna
        private String _prawa;

        public bool nowaZmienna
        {
            get { return _nowaZmienna; }
            set { _nowaZmienna = value; }
        }

        public String lewa
        {
            get { return _lewa; }
            set { _lewa = value; }
        }

        public String dzialanie1
        {
            get { return _dzialanie1; }
            set { _dzialanie1 = value; }
        }

        public bool srodekZmienna
        {
            get { return _srodekZmienna; }
            set { _srodekZmienna = value; }
        }

        public String srodek
        {
            get { return _srodek; }
            set { _srodek = value; }
        }

        public String dzialanie2
        {
            get { return _dzialanie2; }
            set { _dzialanie2 = value; }
        }

        public bool prawaZmienna
        {
            get { return _prawaZmienna; }
            set { _prawaZmienna = value; }
        }

        public String prawa
        {
            get { return _prawa; }
            set { _prawa = value; }
        }

    }
}