using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace libbloki
{
    public partial class BlokObliczeniowy : Bloki
    {
        //private BOOpcje frmOpcje;

        public void DodajDzialanie(Dzialanie dzialanie)
        {
            if (dzialanie != null)
                dzialania.Add(dzialanie);
        }

        public void UsunDzialanie(Dzialanie dzialanie)
        {
            if (dzialanie != null)
                dzialania.Remove(dzialanie);
        }
        
        public BlokObliczeniowy()
        {
            InitializeComponent();
            graph = CreateGraphics();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        public void AktualizujTXT()
        {
            if (this.dzialania != null)
            {
                this.txtHint.Active = false;
                
                this.txt.Text = "";
                int i=0;
                String tempString="";
                foreach (Dzialanie d in this.dzialania)
                {
                    tempString +=  d.lewa.ToString() + " " + d.dzialanie1 + " " + d.srodek.ToString() +
                                    " " + d.dzialanie2 + " " + d.prawa +"\n";
                    i++;
                    if(i>=3)
                    {
                        break;
                    }
                }
                this.txt.Text = tempString.ToString();
                

                if (this.dzialania.Count > 3) 
                {
                    tempString = "";
                    this.txt.Text += "...";
                    foreach (Dzialanie d in this.dzialania)
                    {
                        tempString += d.lewa.ToString() + " " + d.dzialanie1 + " " + d.srodek.ToString() +
                                      " " + d.dzialanie2 + " " + d.prawa +"\n";
                    }
                    this.txtHint.Active = true;
                    txtHint.SetToolTip(txt, tempString);
                    txtHint.SetToolTip(this, tempString);

                }
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Font fnt = new Font("Verdana", 16);
            Graphics g = pe.Graphics;
            SolidBrush brush = new SolidBrush(Color.Black);
            switch (tryb)
            {
                case tryby.normal: brush.Color = Color.Wheat; break;
                case tryby.zaznaczony: brush.Color = Color.Orange; break;
                case tryby.aktualny: brush.Color = Color.Red; break;
            }

            txt.BackColor = brush.Color;
            txt.Refresh();

            AktualizujTXT();

            Pen pn = new Pen(Color.Brown,2);
            Rectangle rect = new Rectangle(2, 2, 148, 73);
            g.DrawRectangle(pn, rect);
            g.FillRectangle(brush, rect);

            brush.Dispose();
            pn.Dispose();
            g = null;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // przezroczyste tlo
        }

        public void Wykonaj()
        {
            int tmpNumerEl = 0;
            int tempIndZnajdz = -1;
            for (int i = 0; i < dzialania.Count; i++)
            {
                int index = ZnajdzZmienna(dzialania[i].lewa);

                if (index < 0)
                {
                    MessageBox.Show("BlokObliczeniowy: Wykonaj: dzialanie: " + i.ToString() + "\n zmienna: " + dzialania[i].lewa + " nie znaleziona |lewa");
                    return;
                }

                if (index > listaZmiennych.Count - 1)
                {
                    MessageBox.Show("BlokObliczeniowy: Wykonaj: dzialanie: " + i.ToString() + "\n index poza zakresem listy zmiennych");
                    return;
                }

                Zmienna temp = listaZmiennych[index];
                Zmienna tempSrodek = null;
                Zmienna tempPrawa = null;

                if (dzialania[i].srodekZmienna == true)
                {
                    tempIndZnajdz = ZnajdzZmienna(dzialania[i].srodek);

                    if (tempIndZnajdz < 0)
                    {
                        MessageBox.Show("BlokObliczeniowy: Wykonaj: dzialanie: " + i.ToString() + "\n tempIndZnajdz: " + dzialania[i].srodek + " nie znaleziona |srodek");
                        return;
                    }

                    if (index > listaZmiennych.Count - 1)
                    {
                        MessageBox.Show("BlokObliczeniowy: Wykonaj: dzialanie: " + i.ToString() + "\n tempIndZnajdz poza zakresem listy zmiennych |srodek");
                        return;
                    }

                    tempSrodek = listaZmiennych[tempIndZnajdz];
                }
                else
                    tempSrodek = null;

                if (dzialania[i].prawaZmienna == true)
                {
                    tempIndZnajdz = ZnajdzZmienna(dzialania[i].prawa);

                    if (tempIndZnajdz < 0)
                    {
                        MessageBox.Show("BlokObliczeniowy: Wykonaj: dzialanie: " + i.ToString() + "\n tempIndZnajdz: " + dzialania[i].prawa + " nie znaleziona |prawa");
                        return;
                    }

                    if (index > listaZmiennych.Count - 1)
                    {
                        MessageBox.Show("BlokObliczeniowy: Wykonaj: dzialanie: " + i.ToString() + "\n tempIndZnajdz poza zakresem listy zmiennych |prawa");
                        return;
                    }

                    tempPrawa = listaZmiennych[tempIndZnajdz];
                }
                else
                    tempPrawa = null;

                if (temp.typ == typeof(int))
                {
                    int tmpL = 0;

                    if (dzialania[i].srodekZmienna == true)
                    {
                        try
                        {
                            if (tempSrodek.tablica == true)
                            {
                                tmpNumerEl = NumerElementuWTablicy(dzialania[i].srodek);

                                if (tmpNumerEl > tempSrodek.wartosci.Count - 1)
                                {
                                    MessageBox.Show("BlokObliczeniowy: Wykonaj|int: dzialanie: " + i.ToString() + " |srodek\n tmpNumerEl poza zakresem tablicy");
                                    return;
                                }

                                if (tmpNumerEl < 0)
                                {
                                    MessageBox.Show("BlokObliczeniowy: Wykonaj|int: dzialanie " + i.ToString() + " |srodek\n tmpNumerEl ujemny:" + tmpNumerEl.ToString());
                                    return;
                                }

                                if (tmpNumerEl <= tempSrodek.iloscElTablicy)
                                    tmpL = Convert.ToInt32(tempSrodek.wartosci[tmpNumerEl]);
                            }
                            else
                                tmpL = Convert.ToInt32(tempSrodek.wartosc);
                        }
                        catch
                        {
                            MessageBox.Show("BlokObliczeniowy: Wykonaj: dzialanie " + i.ToString() + " |srodek\n Blad konwersji zmiennej/tablicy na int");
                            return;
                        }
                    }
                    else
                    {
                        try
                        {
                            if (dzialania[i].srodek == "")
                                tmpL = 0;
                            else
                                tmpL = Convert.ToInt32(dzialania[i].srodek);
                        }
                        catch
                        {
                            MessageBox.Show("BlokObliczeniowy: Wykonaj: dzialanie " + i.ToString() + " |srodek\n Blad konwersji na int");
                            return;
                        }
                    }

                    if (dzialania[i].dzialanie2 != null)
                    {
                        try
                        {
                            switch (dzialania[i].dzialanie2)
                            {
                                case "+":
                                    if (dzialania[i].prawaZmienna == true)
                                    {
                                        if (tempPrawa.tablica == true)
                                        {
                                            tmpNumerEl = NumerElementuWTablicy(dzialania[i].prawa);

                                            if (tmpNumerEl > tempPrawa.wartosci.Count - 1)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|int: dzialanie: " + i.ToString() + " |prawa\n tmpNumerEl poza zakresem tablicy");
                                                return;
                                            }

                                            if (tmpNumerEl < 0)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|int: dzialanie " + i.ToString() + " |prawa\n tmpNumerEl ujemny:" + tmpNumerEl.ToString());
                                                return;
                                            }

                                            tmpL += Convert.ToInt32(tempPrawa.wartosci[tmpNumerEl]);
                                        }
                                        else
                                            tmpL += Convert.ToInt32(tempPrawa.wartosc);
                                    }
                                    else
                                        tmpL += Convert.ToInt32(dzialania[i].prawa);
                                    break;
                                case "-":
                                    if (dzialania[i].prawaZmienna == true)
                                    {
                                        if (tempPrawa.tablica == true)
                                        {
                                            tmpNumerEl = NumerElementuWTablicy(dzialania[i].prawa);

                                            if (tmpNumerEl > tempPrawa.wartosci.Count - 1)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|int: dzialanie: " + i.ToString() + " |prawa\n tmpNumerEl poza zakresem tablicy");
                                                return;
                                            }

                                            if (tmpNumerEl < 0)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|int: dzialanie " + i.ToString() + " |prawa\n tmpNumerEl ujemny:" + tmpNumerEl.ToString());
                                                return;
                                            }

                                            tmpL -= Convert.ToInt32(tempPrawa.wartosci[tmpNumerEl]);
                                        }
                                        else
                                            tmpL -= Convert.ToInt32(tempPrawa.wartosc);
                                    }
                                    else
                                        tmpL -= Convert.ToInt32(dzialania[i].prawa);
                                    break;
                                case "*":
                                    if (dzialania[i].prawaZmienna == true)
                                    {
                                        if (tempPrawa.tablica == true)
                                        {
                                            tmpNumerEl = NumerElementuWTablicy(dzialania[i].prawa);

                                            if (tmpNumerEl > tempPrawa.wartosci.Count - 1)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|int: dzialanie: " + i.ToString() + " |prawa\n tmpNumerEl poza zakresem tablicy");
                                                return;
                                            }

                                            if (tmpNumerEl < 0)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|int: dzialanie " + i.ToString() + " |prawa\n tmpNumerEl ujemny:" + tmpNumerEl.ToString());
                                                return;
                                            }

                                            tmpL *= Convert.ToInt32(tempPrawa.wartosci[tmpNumerEl]);
                                        }
                                        else
                                            tmpL *= Convert.ToInt32(tempPrawa.wartosc);
                                    }
                                    else
                                        tmpL *= Convert.ToInt32(dzialania[i].prawa);
                                    break;
                                case "/":
                                    if (dzialania[i].prawaZmienna == true)
                                    {
                                        if (tempPrawa.tablica == true)
                                        {
                                            tmpNumerEl = NumerElementuWTablicy(dzialania[i].prawa);

                                            if (tmpNumerEl > tempPrawa.wartosci.Count - 1)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|int: dzialanie: " + i.ToString() + " |prawa\n tmpNumerEl poza zakresem tablicy");
                                                return;
                                            }

                                            if (tmpNumerEl < 0)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|int: dzialanie " + i.ToString() + " |prawa\n tmpNumerEl ujemny:" + tmpNumerEl.ToString());
                                                return;
                                            }

                                            int tempZeroCheck = Convert.ToInt32(tempPrawa.wartosci[tmpNumerEl]);

                                            if (tempZeroCheck == 0)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|int: dzialanie: " + i.ToString() + " |prawa\n Proba dzielenia przez 0");
                                                return;
                                            }

                                            tmpL /= tempZeroCheck;
                                        }
                                        else
                                        {
                                            int tempZeroCheck = Convert.ToInt32(tempPrawa.wartosc);

                                            if (tempZeroCheck == 0)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|int: dzialanie: " + i.ToString() + " |prawa\n Proba dzielenia przez 0");
                                                return;
                                            }

                                            tmpL /= tempZeroCheck;
                                        }
                                    }
                                    else
                                    {
                                        int tempZeroCheck = Convert.ToInt32(dzialania[i].prawa);

                                        if (tempZeroCheck == 0)
                                        {
                                            MessageBox.Show("BlokObliczeniowy: Wykonaj|int: dzialanie: " + i.ToString() + " |prawa\n Proba dzielenia przez 0");
                                            return;
                                        }

                                        tmpL /= tempZeroCheck;                                    
                                    }
                                    break;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("BlokObliczeniowy: Wykonaj: dzialanie " + i.ToString() + " |prawa\n Blad przy konwersji na int");
                            return;
                        }
                    }

                    if (temp.tablica == true)
                    {
                        tmpNumerEl = NumerElementuWTablicy(dzialania[i].lewa);

                        if (tmpNumerEl > temp.wartosci.Count - 1)
                        {
                            MessageBox.Show("BlokObliczeniowy: Wykonaj|int: dzialanie: " + i.ToString() + " |lewa\n tmpNumerEl poza zakresem tablicy");
                            return;
                        }

                        if (tmpNumerEl < 0)
                        {
                            MessageBox.Show("BlokObliczeniowy: Wykonaj|int: dzialanie " + i.ToString() + " |lewa\n tmpNumerEl ujemny:" + tmpNumerEl.ToString());
                            return;
                        }

                        temp.wartosci[tmpNumerEl] = tmpL.ToString();
                        tmpNumerEl = 0;
                    }
                    else
                        temp.wartosc = tmpL.ToString();

                    continue;
                }

                if (temp.typ == typeof(double))
                {
                    double tmpL = 0.0;

                    try
                    {
                        if (dzialania[i].srodekZmienna == true)
                        {
                            if (tempSrodek.tablica == true)
                            {
                                tmpNumerEl = NumerElementuWTablicy(dzialania[i].srodek);

                                if (tmpNumerEl > tempSrodek.wartosci.Count - 1)
                                {
                                    MessageBox.Show("BlokObliczeniowy: Wykonaj|double: dzialanie: " + i.ToString() + " |srodek\n tmpNumerEl poza zakresem tablicy");
                                    return;
                                }

                                if (tmpNumerEl < 0)
                                {
                                    MessageBox.Show("BlokObliczeniowy: Wykonaj|double: dzialanie " + i.ToString() + " |srodek\n tmpNumerEl ujemny:" + tmpNumerEl.ToString());
                                    return;
                                }

                                if (tmpNumerEl <= tempSrodek.iloscElTablicy)
                                    tmpL = Convert.ToDouble(tempSrodek.wartosci[tmpNumerEl]);
                            }
                            else
                                tmpL = Convert.ToDouble(tempSrodek.wartosc);
                        }
                        else
                            tmpL = Convert.ToDouble(dzialania[i].srodek);
                    }
                    catch
                    {
                        MessageBox.Show("BlokObliczeniowy: Wykonaj: dzialanie " + i.ToString() + " |srodek\n Blad konwersji na double");
                        return;
                    }

                    if (dzialania[i].dzialanie2 != null)
                    {
                        try
                        {
                            switch (dzialania[i].dzialanie2)
                            {
                                case "+":
                                    if (dzialania[i].prawaZmienna == true)
                                    {
                                        if (tempPrawa.tablica == true)
                                        {
                                            tmpNumerEl = NumerElementuWTablicy(dzialania[i].prawa);

                                            if (tmpNumerEl > tempPrawa.wartosci.Count - 1)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|double: dzialanie: " + i.ToString() + " |prawa\n tmpNumerEl poza zakresem tablicy");
                                                return;
                                            }

                                            if (tmpNumerEl < 0)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|double: dzialanie " + i.ToString() + " |prawa\n tmpNumerEl ujemny:" + tmpNumerEl.ToString());
                                                return;
                                            }

                                            tmpL += Convert.ToDouble(tempPrawa.wartosci[tmpNumerEl]);
                                        }
                                        else
                                            tmpL += Convert.ToDouble(tempPrawa.wartosc);
                                    }
                                    else
                                        tmpL += Convert.ToDouble(dzialania[i].prawa);
                                    break;
                                case "-":
                                    if (dzialania[i].prawaZmienna == true)
                                    {
                                        if (tempPrawa.tablica == true)
                                        {
                                            tmpNumerEl = NumerElementuWTablicy(dzialania[i].prawa);

                                            if (tmpNumerEl > tempPrawa.wartosci.Count - 1)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|double: dzialanie: " + i.ToString() + " |prawa\n tmpNumerEl poza zakresem tablicy");
                                                return;
                                            }

                                            if (tmpNumerEl < 0)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|double: dzialanie " + i.ToString() + " |prawa\n tmpNumerEl ujemny:" + tmpNumerEl.ToString());
                                                return;
                                            }

                                            tmpL -= Convert.ToDouble(tempPrawa.wartosci[tmpNumerEl]);
                                        }
                                        else
                                            tmpL -= Convert.ToDouble(tempPrawa.wartosc);
                                    }
                                    else
                                        tmpL -= Convert.ToDouble(dzialania[i].prawa);
                                    break;
                                case "*":
                                    if (dzialania[i].prawaZmienna == true)
                                    {
                                        if (tempPrawa.tablica == true)
                                        {
                                            tmpNumerEl = NumerElementuWTablicy(dzialania[i].prawa);

                                            if (tmpNumerEl > tempPrawa.wartosci.Count - 1)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|double: dzialanie: " + i.ToString() + " |prawa\n tmpNumerEl poza zakresem tablicy");
                                                return;
                                            }

                                            if (tmpNumerEl < 0)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|double: dzialanie " + i.ToString() + " |prawa\n tmpNumerEl ujemny:" + tmpNumerEl.ToString());
                                                return;
                                            }

                                            tmpL *= Convert.ToDouble(tempPrawa.wartosci[tmpNumerEl]);
                                        }
                                        else
                                            tmpL *= Convert.ToDouble(tempPrawa.wartosc);
                                    }
                                    else
                                        tmpL *= Convert.ToDouble(dzialania[i].prawa);
                                    break;
                                case "/":
                                    if (dzialania[i].prawaZmienna == true)
                                    {
                                        if (tempPrawa.tablica == true)
                                        {
                                            tmpNumerEl = NumerElementuWTablicy(dzialania[i].prawa);

                                            if (tmpNumerEl > tempPrawa.wartosci.Count - 1)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|double: dzialanie: " + i.ToString() + " |prawa\n tmpNumerEl poza zakresem tablicy");
                                                return;
                                            }

                                            if (tmpNumerEl < 0)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|double: dzialanie " + i.ToString() + " |prawa\n tmpNumerEl ujemny:" + tmpNumerEl.ToString());
                                                return;
                                            }

                                            double tempZeroCheck = Convert.ToDouble(tempPrawa.wartosci[tmpNumerEl]);

                                            if (tempZeroCheck == 0)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|double: dzialanie: " + i.ToString() + " |prawa\n Proba dzielenia przez 0");
                                                return;
                                            }

                                            tmpL /= tempZeroCheck;
                                        }
                                        else
                                        {
                                            double tempZeroCheck = Convert.ToDouble(tempPrawa.wartosc);

                                            if (tempZeroCheck == 0)
                                            {
                                                MessageBox.Show("BlokObliczeniowy: Wykonaj|double: dzialanie: " + i.ToString() + " |prawa\n Proba dzielenia przez 0");
                                                return;
                                            }

                                            tmpL /= tempZeroCheck;
                                        }
                                    }
                                    else
                                    {
                                        double tempZeroCheck = Convert.ToDouble(dzialania[i].prawa);

                                        if (tempZeroCheck == 0)
                                        {
                                            MessageBox.Show("BlokObliczeniowy: Wykonaj|double: dzialanie: " + i.ToString() + " |prawa\n Proba dzielenia przez 0");
                                            return;
                                        }

                                        tmpL /= tempZeroCheck;
                                    }
                                    break;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("BlokObliczeniowy: Wykonaj: dzialanie " + i.ToString() + " |prawa\n Blad konwersji na double");
                            return;
                        }
                    }

                    if (temp.tablica == true)
                    {
                        tmpNumerEl = NumerElementuWTablicy(dzialania[i].lewa);

                        if (tmpNumerEl > temp.wartosci.Count - 1)
                        {
                            MessageBox.Show("BlokObliczeniowy: Wykonaj|double: dzialanie: " + i.ToString() + " |lewa\n tmpNumerEl poza zakresem tablicy");
                            return;
                        }

                        if (tmpNumerEl < 0)
                        {
                            MessageBox.Show("BlokObliczeniowy: Wykonaj|double: dzialanie " + i.ToString() + " |lewa\n tmpNumerEl ujemny:" + tmpNumerEl.ToString());
                            return;
                        }

                        temp.wartosci[tmpNumerEl] = tmpL.ToString();
                        tmpNumerEl = 0;
                    }
                    else
                        temp.wartosc = tmpL.ToString();

                    continue;
                }

                if (temp.typ == typeof(String))
                {
                    String tmpL = "";

                    if (dzialania[i].srodekZmienna == true)
                    {
                        if (tempSrodek.tablica == true)
                        {
                            tmpNumerEl = NumerElementuWTablicy(dzialania[i].srodek);

                            if (tmpNumerEl > tempSrodek.wartosci.Count - 1)
                            {
                                MessageBox.Show("BlokObliczeniowy: Wykonaj|String: dzialanie: " + i.ToString() + " |srodek\n tmpNumerEl poza zakresem tablicy");
                                return;
                            }

                            if (tmpNumerEl < 0)
                            {
                                MessageBox.Show("BlokObliczeniowy: Wykonaj|String: dzialanie " + i.ToString() + " |srodek\n tmpNumerEl ujemny:" + tmpNumerEl.ToString());
                                return;
                            }

                            tmpL = tempSrodek.wartosci[tmpNumerEl].ToString();
                        }
                        else
                            tmpL = tempSrodek.wartosc.ToString();
                    }
                    else
                        tmpL = dzialania[i].srodek.ToString();

                    if (dzialania[i].dzialanie2 != null)
                    {
                        switch (dzialania[i].dzialanie2)
                        {
                            case "+":
                                if (dzialania[i].prawaZmienna == true)
                                {
                                    if (tempPrawa.tablica == true)
                                    {
                                        tmpNumerEl = NumerElementuWTablicy(dzialania[i].prawa);

                                        if (tmpNumerEl > tempPrawa.wartosci.Count - 1)
                                        {
                                            MessageBox.Show("BlokObliczeniowy: Wykonaj|String: dzialanie: " + i.ToString() + " |prawa\n tmpNumerEl poza zakresem tablicy");
                                            return;
                                        }

                                        if (tmpNumerEl < 0)
                                        {
                                            MessageBox.Show("BlokObliczeniowy: Wykonaj|String: dzialanie " + i.ToString() + " |prawa\n tmpNumerEl ujemny:" + tmpNumerEl.ToString());
                                            return;
                                        }

                                        tmpL += tempPrawa.wartosci[tmpNumerEl];
                                    }
                                    else
                                        tmpL += tempPrawa.wartosc;
                                }
                                else
                                    tmpL += dzialania[i].prawa;
                                break;
                            case "-":
                                if (dzialania[i].prawaZmienna == true)
                                {
                                    if (tempPrawa.tablica == true)
                                    {
                                        tmpNumerEl = NumerElementuWTablicy(dzialania[i].prawa);

                                        if (tmpNumerEl > tempPrawa.wartosci.Count - 1)
                                        {
                                            MessageBox.Show("BlokObliczeniowy: Wykonaj|String: dzialanie: " + i.ToString() + " |prawa\n tmpNumerEl poza zakresem tablicy");
                                            return;
                                        }

                                        if (tmpNumerEl < 0)
                                        {
                                            MessageBox.Show("BlokObliczeniowy: Wykonaj|String: dzialanie " + i.ToString() + " |prawa\n tmpNumerEl ujemny:" + tmpNumerEl.ToString());
                                            return;
                                        }

                                        tmpL = tmpL.Replace(tempPrawa.wartosci[tmpNumerEl], "");
                                    }
                                    else
                                        tmpL = tmpL.Replace(tempPrawa.wartosc, "");
                                }
                                else
                                    tmpL = tmpL.Replace(dzialania[i].prawa, "");
                                break;
                            case "*":
                                break;
                            case "/":
                                break;
                        }
                    }

                    if (temp.tablica == true)
                    {
                        tmpNumerEl = NumerElementuWTablicy(dzialania[i].lewa);

                        if (tmpNumerEl > temp.wartosci.Count - 1)
                        {
                            MessageBox.Show("BlokObliczeniowy: Wykonaj|String: dzialanie: " + i.ToString() + " |lewa\n tmpNumerEl poza zakresem tablicy");
                            return;
                        }

                        if (tmpNumerEl < 0)
                        {
                            MessageBox.Show("BlokObliczeniowy: Wykonaj|String: dzialanie " + i.ToString() + " |lewa\n tmpNumerEl ujemny:" + tmpNumerEl.ToString());
                            return;
                        }

                        temp.wartosci[tmpNumerEl] = tmpL.ToString();
                    }
                    else
                        temp.wartosc = tmpL.ToString();
                    
                    continue;
                }
            }
        }

        private void txt_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            sender = this;
            this.OnMouseDoubleClick(e);
        }

        private void txt_MouseDown(object sender, MouseEventArgs e)
        {
            sender = this;
            this.OnMouseDown(e);
        }

        private void txt_MouseMove(object sender, MouseEventArgs e)
        {
            sender = this;
            this.OnMouseMove(e);
        }

        private void txt_MouseUp(object sender, MouseEventArgs e)
        {
            sender = this;
            this.OnMouseUp(e);
        }
    }
}
