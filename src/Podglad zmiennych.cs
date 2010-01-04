using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using libbloki;

namespace Okienka
{
    public partial class Podglad_zmiennych : Form
    {
        public IList<Zmienna> zmienne;
        public IList<Zmienna> obserwowaneZmienne = new List<Zmienna>();

        public Podglad_zmiennych()
        {
            InitializeComponent();
        }

        public Podglad_zmiennych(IList<Zmienna> usr)
        {
            InitializeComponent();

            zmienne = usr;
            WczytajZmienne();
        }

        public bool SprawdzCzyIstniejeZmienna(String zmienna)
        {
            if (zmienna == null || zmienne == null)
                return false;

            String temp = zmienna.ToString();

            for (int i = 0; i < zmienne.Count(); i++)
            {
                if (zmienne[i].nazwa.Equals(temp) == true)
                    return true;
            }

            return false;
        }

        public int ZnajdzZmienna(String zmienna)
        {
            if (zmienna == null)
                return -1;

            String temp = zmienna.ToString();

            if (temp.Contains('[') == true && temp.Contains(']') == true)
                temp = temp.Remove(temp.IndexOf('['));

            if (temp.Contains(' ') == true)
                temp = temp.Replace(" ", "");

            for (int i = 0; i < zmienne.Count; i++)
            {
                if (zmienne[i].nazwa.Equals(temp) == true)
                    return i;
            }

            return -1;
        }

        public bool SprawdzCzyJestWObserwowanych(Zmienna zmienna)
        {
            if (obserwowaneZmienne.Count < 1)
                return false;

            foreach (Zmienna zm in obserwowaneZmienne)
            {
                if (zm.nazwa == zmienna.nazwa)
                    return true;
            }

            return false;
        }

        public void WczytajZmienne()
        {
            lbZmienne.Items.Clear();

            foreach (Zmienna zm in zmienne)
            {
                if (zm.tablica == true)
                    lbZmienne.Items.Add(zm.nazwa.ToString() + "[]");
                else
                    lbZmienne.Items.Add(zm.nazwa.ToString());
            }
        }

        public void AktualizujObserwowaneZmienne()    //ze zmiennych ktorych juz niema i aktualizuj wartości
        {
            if (zmienne == null || obserwowaneZmienne.Count < 1)
                return;

            foreach (Zmienna zm in obserwowaneZmienne)
            {
                if (SprawdzCzyIstniejeZmienna(zm.nazwa) == false)
                    obserwowaneZmienne.Remove(zm);
                else
                {
                    Zmienna temp = zmienne[ZnajdzZmienna(zm.nazwa)];

                    if (zm.tablica == true)
                    {
                        zm.wartosci.Clear();
                        zm.iloscElTablicy = temp.iloscElTablicy;
                        for (int i = 0; i < temp.wartosci.Count; i++)
                            zm.wartosci.Add(temp.wartosci[i].ToString());
                    }
                    else
                        zm.wartosc = temp.wartosc.ToString();
                }
            }
        }

        public void AktualizujListeObserwowanych()
        {
            if (obserwowaneZmienne.Count < 1)
                return;

            lbObserwowane.Items.Clear();
            AktualizujObserwowaneZmienne();

            String temp;

            foreach (Zmienna zm in obserwowaneZmienne)
            {
                temp = zm.nazwa.ToString();
                if (zm.tablica == true)
                {
                    temp += "[" + zm.iloscElTablicy.ToString() + "] (" + zm.typ.ToString() + "): ";
                    for (int i = 0; i < zm.iloscElTablicy; i++)
                    {
                        temp += zm.wartosci[i];
                        if (i != zm.iloscElTablicy - 1)
                            temp += ", ";
                        else
                            temp += ";";
                    }
                }
                else
                {
                    temp += " (" + zm.typ.ToString() + "): " + zm.wartosc;
                }
                lbObserwowane.Items.Add(temp.ToString());
            }
        }

        private void lbZmienne_DoubleClick(object sender, EventArgs e)
        {
            if (lbZmienne.Items.Count>0)
            {
                Zmienna temp = new Zmienna();
            
                String tempNazwa = lbZmienne.SelectedItem.ToString(); ;
                Zmienna tempZm = zmienne[ZnajdzZmienna(tempNazwa)];

                temp.iloscElTablicy = tempZm.iloscElTablicy;
                temp.nazwa = tempZm.nazwa.ToString();
                temp.tablica = tempZm.tablica;
                temp.typ = tempZm.typ;

                if (temp.tablica == true)
                {
                    foreach (String str in tempZm.wartosci)
                    {
                        temp.wartosci.Add(str.ToString());
                    }
                }
                else
                    temp.wartosc = tempZm.wartosc.ToString();

                if (SprawdzCzyJestWObserwowanych(temp) == false)
                    obserwowaneZmienne.Add(temp);

                AktualizujListeObserwowanych();
            }
        }
    }
}
