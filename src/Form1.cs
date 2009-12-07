﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using libbloki;
using System.Threading;

namespace Okienka
{
    public partial class Form1 : Form
    {
        private static int numer = 0;

        Watch okno;

        private bool klik = false;
        private bool polacz = false;
        private bool ctrl = false;
        private bool przesun = false;
        private bool symuluj = false;
		
        private Bloki polaczOD = null, polaczDO = null;

        private int ile = 0;
        private int polowaX;
        private int polowaY;

        private IList<Bloki> tabBloki = new List<Bloki>();
        private IList<Zmienna> zmienne = new List<Zmienna>();

        private Graphics graph;

        private Poziom[] pozK = new Poziom[4];
        private Pion[] pioK = new Pion[4];

        private Type typ;

        private Point punktKlikuNaBlok;      //punkt w którym kliknięto na blok (przeciwdziałanie przesunięciu bloku bez przesuwania kursora)
        public Bloki zaznaczony;
        public Bloki aktualnyBlok;

        private BackgroundWorker bw = new BackgroundWorker();
        
        public Form1()
        {
            InitializeComponent();
            graph = panel1.CreateGraphics();
            numer = 0;
        }

        private int ZnajdzBlok(String nazwa)
        {
            int i;
            for (i = 0; i < tabBloki.Count; i++)
                if (tabBloki[i].Name.Equals(nazwa) == true) break;

            return i;
        }

        private bool JestBlokONazwie(String nazwa)
        {
            for (int i = 0; i < tabBloki.Count; i++)
                if (tabBloki[i].Name.Equals(nazwa) == true)
                    return true;

            return false;
        }

        private void UsunBlok(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (zaznaczony.Name.Equals(((Bloki)sender).Name))
                    zaznaczony = null;

                tabBloki.RemoveAt(ZnajdzBlok(((Bloki)sender).Name));
                ((Bloki)sender).Dispose();
                ile--;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                ctrl = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                ctrl = false;
                klik = false;
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            if (klik == true)
            {
                Bloki temp2 = new Bloki();          //potrzebne do dodania do listy
                numer++;
                
                if (typ == typeof(BlokSTART))
                {
                    if (JestBlokONazwie("START"))
                        return;

                    BlokSTART temp = new BlokSTART();

                    temp2.typBloku = typeof(BlokSTART);
                    temp2 = (Bloki)temp;
                    temp2.Name = "START";
                }

                if (typ == typeof(BlokSTOP))
                {
                    if (JestBlokONazwie("STOP"))
                        return;

                    BlokSTOP temp = new BlokSTOP();
					
                    temp2.typBloku = typeof(BlokSTOP);
                    temp2 = (Bloki)temp;
                    temp2.Name = "STOP";
                }

                if (typ == typeof(BlokObliczeniowy))
                {
                    BlokObliczeniowy temp = new BlokObliczeniowy();

                    temp2.typBloku = typeof(BlokObliczeniowy);
                    temp2 = (Bloki)temp;
                }

                if (typ == typeof(BlokDecyzyjny))
                {
                    BlokDecyzyjny temp = new BlokDecyzyjny();

                    temp2.typBloku = typeof(BlokDecyzyjny);
                    temp2 = (Bloki)temp;
                }

                if (typ == typeof(BlokWeWy))
                {
                    BlokWeWy temp = new BlokWeWy();

                    temp2.typBloku = typeof(BlokWeWy);
                    temp2 = (Bloki)temp;
                }

                //globalne dla wszystkich bloków

                if (temp2.Name == "")               //START i STOP mają własne nazwy - tylko raz mogą wystąpić w algorytmie
                    temp2.Name = numer.ToString();      

                temp2.Left = ((MouseEventArgs)e).X;
                temp2.Top = ((MouseEventArgs)e).Y;
                temp2.KeyDown += new KeyEventHandler(UsunBlok);
                temp2.MouseDown += new MouseEventHandler(PrzesunStart);
                temp2.MouseMove += new MouseEventHandler(panel1_MouseMove);
                temp2.MouseUp += new MouseEventHandler(panel1_MouseUp);
                tabBloki.Add(temp2);

                panel1.Controls.Add(tabBloki.Last());

                ile++;
                if (ctrl != true)
                    klik = false;
            }
            
            if (zaznaczony != null)
            {
                zaznaczony.tryb = tryby.normal;
                zaznaczony = null;
            }
        }

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {
            foreach (Control c in panel1.Controls)
            {
                c.Update();
            }
        }

        public void PrzesunStart(object sender, MouseEventArgs e)
        {
            if (zaznaczony != null)
            {
                zaznaczony.tryb = tryby.normal;
                zaznaczony = null;
            }

            if (sender.GetType() == typeof(BlokSTART) ||
                sender.GetType() == typeof(BlokSTOP) ||
                sender.GetType() == typeof(BlokObliczeniowy) ||
                sender.GetType() == typeof(BlokDecyzyjny) ||
                sender.GetType() == typeof(BlokWeWy) ||
                sender.GetType() == typeof(Bloki))
            {
                zaznaczony = (Bloki)sender;
                zaznaczony.tryb = tryby.zaznaczony;

                if (polacz == true)
                {
                    if (polaczOD == null)
                    {
                        polaczOD = zaznaczony;
                        return;
                    }
                    if((polaczOD != null) && (polaczDO == null))
                    {
                        polaczDO = zaznaczony;////////
                        zaznaczony.poprzedniBlok = polaczOD;
                        polaczOD.nastepnyBlok = polaczDO;
                        RysujPolaczenie();
                        polaczOD = null;
                        polaczDO = null;
                        polacz = false;
                        return;
                    }
                }
            }
            else
            {
                polacz = false;
                polaczOD = null;
                polaczDO = null;
            }

            if (przesun == false)
            {
                przesun = true;
                punktKlikuNaBlok = new Point();
                punktKlikuNaBlok.X = e.X;
                punktKlikuNaBlok.Y = e.Y;

                for (int i = 0; i < 4; i++)
                {
                    pozK[i] = new Poziom();
                    pioK[i] = new Pion();
                }

                polowaX = (zaznaczony.Width) / 2;
                polowaY = (zaznaczony.Height) / 2;

                pioK[0].Left = zaznaczony.Left;
                pioK[0].Top = zaznaczony.Top;

                pozK[0].Left = zaznaczony.Left;
                pozK[0].Top = zaznaczony.Top;


                pioK[1].Left = zaznaczony.Left;
                pioK[1].Top = zaznaczony.Top + (zaznaczony.Height - pioK[1].Height);

                pozK[1].Left = zaznaczony.Left;
                pozK[1].Top = zaznaczony.Top + (zaznaczony.Height - pozK[1].Height);


                pioK[2].Left = zaznaczony.Left + (zaznaczony.Width - pioK[2].Width);
                pioK[2].Top = zaznaczony.Top;

                pozK[2].Left = zaznaczony.Left + (zaznaczony.Width - pozK[2].Width);
                pozK[2].Top = zaznaczony.Top;


                pioK[3].Left = zaznaczony.Left + (zaznaczony.Width - pioK[3].Width);
                pioK[3].Top = zaznaczony.Top + (zaznaczony.Height - pioK[3].Height);

                pozK[3].Left = zaznaczony.Left + (zaznaczony.Width - pozK[3].Width);
                pozK[3].Top = zaznaczony.Top + (zaznaczony.Height - pozK[3].Height);

                for (int i = 0; i < 4; i++)
                {
                    panel1.Controls.Add(pioK[i]);
                    panel1.Controls.Add(pozK[i]);
                }
            }

            //if (polaczDO != null && polaczOD != null)
            //{
                //zaznaczony.poprzedniBlok = polaczOD;
                //polaczOD.nastepnyBlok = zaznaczony;
                //RysujPolaczenie();
                //polaczOD = null;
                //polaczDO = null;
            //}
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (przesun == true)
            {
                przesunNarozniki(sender, e);
            }
        }

        private void przesunNarozniki(object sender, MouseEventArgs e)
        {
            if (przesun == true)
            {
                if (punktKlikuNaBlok.X != e.X && punktKlikuNaBlok.Y != e.Y) //jeśli zmieniono położenie kursora
                {
                    pozK[0].Left = e.X + zaznaczony.Left - polowaX;
                    pozK[0].Top = e.Y + zaznaczony.Top - polowaY;

                    pioK[0].Left = e.X + zaznaczony.Left - polowaX;
                    pioK[0].Top = e.Y + zaznaczony.Top - polowaY;

                    if (pozK[0].Left < panel1.Margin.Left)
                        pozK[0].Left = panel1.Margin.Left;

                    if (pozK[0].Top < panel1.Top)
                        pozK[0].Top = panel1.Top;

                    pozK[1].Left = pozK[0].Left;
                    pozK[1].Top = pozK[0].Top + (zaznaczony.Height - pozK[1].Height);

                    pioK[1].Left = pozK[0].Left;
                    pioK[1].Top = pozK[0].Top + (zaznaczony.Height - pioK[1].Height);

                    pozK[2].Left = pozK[0].Left + (zaznaczony.Width - pozK[2].Width);
                    pozK[2].Top = pozK[0].Top;

                    pioK[2].Left = pioK[0].Left + (zaznaczony.Width);
                    pioK[2].Top = pozK[0].Top;

                    pozK[3].Left = pozK[0].Left + (zaznaczony.Width - pozK[3].Width);
                    pozK[3].Top = pozK[0].Top + (zaznaczony.Height - pozK[3].Height);

                    pioK[3].Left = pioK[0].Left + (zaznaczony.Width);
                    pioK[3].Top = pioK[0].Top + (zaznaczony.Height - pioK[3].Height);

                    for (int i = 0; i < 4; i++)
                    {
                        pioK[i].Refresh();
                        pozK[i].Refresh();
                    }
                }
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {                         
            if (przesun == true)
            {
                if (punktKlikuNaBlok.X != e.X && punktKlikuNaBlok.Y != e.Y) //jeśli zmieniono położenie kursora
                {
                    zaznaczony.Left = pozK[0].Left;
                    zaznaczony.Top = pozK[0].Top;
                }
                /*if (zaznaczony.typBloku == typeof(BlokObliczeniowy)) //nie działa
                {
                    ((BlokObliczeniowy)zaznaczony).ReDrawText();
                    zaznaczony.Refresh();
                }
                 */

                if (zaznaczony.poprzedniBlok != null)
                {
                    polaczOD = zaznaczony.poprzedniBlok;
                    polaczDO = zaznaczony;
                    RysujPolaczenie();
                    polaczOD = null;
                    polaczDO = null;
                }
                if (zaznaczony.nastepnyBlok != null)
                {
                    polaczOD = zaznaczony;
                    polaczDO = zaznaczony.nastepnyBlok;
                    RysujPolaczenie();
                    polaczOD = null;
                    polaczDO = null;
                    
                }

                zaznaczony.BringToFront();
                przesun = false;
                //zaznaczony.tryb = tryby.normal;
                
                polowaX = 0;
                polowaY = 0;

                for (int i = 0; i < 4; i++)
                {
                    panel1.Controls.Remove(pozK[i]);
                    panel1.Controls.Remove(pioK[i]);
                }
                
                panel1.Refresh();
            }
        }

        private void dodajBlokStart_Click(object sender, EventArgs e)
        {
            typ = typeof(BlokSTART);
            klik = true;
        }

        private void dodajBlokStop_Click(object sender, EventArgs e)
        {
            typ = typeof(BlokSTOP);
            klik = true;
        }

        private void dodajBlokObliczeniowy_Click(object sender, EventArgs e)
        {
            typ = typeof(BlokObliczeniowy);
            klik = true;
        }

        private void dodajBlokDecyzyjny_Click(object sender, EventArgs e)
        {
            typ = typeof(BlokDecyzyjny);
            klik = true;
        }

        private void dodajBlokWeWy_Click(object sender, EventArgs e)
        {
            typ = typeof(BlokWeWy);
            klik = true;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            okno = new Watch();
            okno.Show();
        }

        private void Połączenie_Click(object sender, EventArgs e)
        {
            polacz = true;
            polaczDO = null;
            polaczOD = null;
        }

        private void UsunLinie() //
        {
            if (polaczOD != null && polaczOD.nastepnaLinia != null)
            {
                Bloki temp,temp2;
                temp = polaczOD.nastepnaLinia;
                if (temp.nastepnyBlok != null)
                {
                    temp.nastepnyBlok.poprzedniaLinia = null;
                    temp.Dispose();
                    polaczOD.nastepnaLinia = null;
                }
                else
                {
                    while (temp.nastepnyBlok == null)
                    {
                        temp = temp.nastepnaLinia;
                    } 
                    temp2 = temp;
                    while (temp.poprzedniBlok == null)
                    {
                        temp = temp2.poprzedniaLinia;
                        temp2.Dispose();
                        temp2 = temp;
                    }
                    temp.poprzedniBlok.nastepnaLinia = null;
                    temp.Dispose();
                }

               // polaczOD.nastepnaLinia.nastepnyBlok.poprzedniaLinia = null;
               // polaczOD.nastepnaLinia.Dispose();
                //polaczOD.nastepnaLinia = null;
            }
            if (polaczDO != null && polaczDO.poprzedniaLinia != null)
            {
                Bloki temp, temp2;
                temp = polaczDO.poprzedniaLinia; ;
                if (temp.poprzedniBlok != null)
                {
                    temp.poprzedniBlok.nastepnaLinia = null;
                    temp.Dispose();
                    polaczDO.poprzedniaLinia = null;
                }
                else
                {
                    while (temp.poprzedniBlok == null)
                    {
                        temp = temp.poprzedniaLinia;
                    }
                    temp2 = temp;
                    while (temp.nastepnyBlok == null)
                    {
                        temp = temp2.nastepnaLinia; ;
                        temp2.Dispose();
                        temp2 = temp;
                    }
                    temp.nastepnyBlok.poprzedniaLinia = null;
                    temp.Dispose();
                }

                // Bloki temp;
                //polaczDO.poprzedniaLinia.poprzedniBlok.nastepnaLinia = null;
                //polaczDO.poprzedniaLinia.Dispose();
                //polaczDO.poprzedniaLinia = null;
            }
        }

        private void RysujPolaczenie()//
        {
            UsunLinie();
            
            if ((polaczOD.Top + polaczOD.Height) < polaczDO.Top)
            {
                if ((polaczOD.Left <= (polaczDO.Left + polaczDO.Width)) && (polaczOD.Left >= (polaczDO.Left + ((polaczDO.Width) / 2))))
                {
                    double temp;
                    LiniaPion tmpLinia;

                    temp = ((polaczOD.Left + polaczDO.Left + polaczDO.Width) / 2) - polaczOD.Left;
                    polaczOD.punkty[1].X = (Int32)temp;
                    polaczOD.punkty[1].Y = polaczOD.Height - 2;

                    polaczDO.punkty[0].X = polaczDO.Width - (Int32)temp;
                    polaczDO.punkty[0].Y = 2;

                    tmpLinia = new LiniaPion(strzalkaUpDown.down);

                    tmpLinia.Top = polaczOD.Top + polaczOD.punkty[1].Y;
                    tmpLinia.Left = polaczOD.Left + polaczOD.punkty[1].X;

                    tmpLinia.Width = 3;
                    tmpLinia.Height = polaczDO.Top - tmpLinia.Top;
                    
                    //^^^^^^^^^^^^^^^
                    tmpLinia.poprzedniBlok = polaczOD;
                    tmpLinia.nastepnyBlok = polaczDO;
                    polaczOD.nastepnaLinia = tmpLinia;
                    polaczDO.poprzedniaLinia = tmpLinia;
                    //^^^^^^^^^^^^^^^
                    
                    
                    panel1.Controls.Add(tmpLinia);
                    tmpLinia = null;
                    return;
                }
                if ((polaczDO.Left <= (polaczOD.Left + polaczOD.Width)) && (polaczDO.Left >= (polaczOD.Left + ((polaczOD.Width) / 2))))
                {
                    double temp;
                    LiniaPion tmpLinia;

                    temp = ((polaczDO.Left + polaczOD.Left + polaczOD.Width) / 2) - polaczDO.Left;
                    polaczOD.punkty[1].X = polaczDO.Width - (Int32)temp;
                    polaczOD.punkty[1].Y = polaczOD.Height - 2;

                    polaczDO.punkty[0].X = (Int32)temp;
                    polaczDO.punkty[0].Y = 2;

                    tmpLinia = new LiniaPion(strzalkaUpDown.down);

                    tmpLinia.Top = polaczOD.Top + polaczOD.punkty[1].Y;
                    tmpLinia.Left = polaczDO.Left + polaczDO.punkty[0].X;

                    tmpLinia.Width = 3;
                    tmpLinia.Height = polaczDO.Top - tmpLinia.Top;
                    

                    //^^^^^^^^^^^^^^^
                    tmpLinia.poprzedniBlok = polaczOD;
                    tmpLinia.nastepnyBlok = polaczDO;
                    polaczOD.nastepnaLinia = tmpLinia;
                    polaczDO.poprzedniaLinia = tmpLinia;
                    //^^^^^^^^^^^^^^^
                    panel1.Controls.Add(tmpLinia);
                    tmpLinia = null;
                    return;
                }
                if ((polaczDO.Left <= (polaczOD.Left + (polaczOD.Width) / 2)) && (polaczDO.Left >= polaczOD.Left))
                {
                    double temp;
                    LiniaPion tmpLinia;

                    temp = ((polaczDO.Left + polaczOD.Left + polaczOD.Width) / 2) - polaczDO.Left;
                    polaczOD.punkty[1].X = polaczDO.Width - (Int32)temp;
                    polaczOD.punkty[1].Y = polaczOD.Height - 2;

                    polaczDO.punkty[0].X = (Int32)temp;
                    polaczDO.punkty[0].Y = 2;

                    tmpLinia = new LiniaPion(strzalkaUpDown.down);

                    tmpLinia.Top = polaczOD.Top + polaczOD.punkty[1].Y;
                    tmpLinia.Left = polaczDO.Left + polaczDO.punkty[0].X;

                    tmpLinia.Width = 3;
                    tmpLinia.Height = polaczDO.Top - tmpLinia.Top;
                    

                    //^^^^^^^^^^^^^^^
                    tmpLinia.poprzedniBlok = polaczOD;
                    tmpLinia.nastepnyBlok = polaczDO;
                    polaczOD.nastepnaLinia = tmpLinia;
                    polaczDO.poprzedniaLinia = tmpLinia;
                    //^^^^^^^^^^^^^^^
                    panel1.Controls.Add(tmpLinia);
                    tmpLinia = null;
                    return;
                }
                if ((polaczOD.Left <= (polaczDO.Left + (polaczDO.Width) / 2)) && (polaczOD.Left >= polaczDO.Left))
                {
                    double temp;
                    LiniaPion tmpLinia;

                    temp = ((polaczOD.Left + polaczDO.Left + polaczDO.Width) / 2) - polaczOD.Left;
                    polaczOD.punkty[1].X = (Int32)temp;
                    polaczOD.punkty[1].Y = polaczOD.Height - 2;

                    polaczDO.punkty[0].X = polaczDO.Width - (Int32)temp;
                    polaczDO.punkty[0].Y = 2;

                    tmpLinia = new LiniaPion(strzalkaUpDown.down);

                    tmpLinia.Top = polaczOD.Top + polaczOD.punkty[1].Y;
                    tmpLinia.Left = polaczOD.Left + polaczOD.punkty[1].X;

                    tmpLinia.Width = 3;
                    tmpLinia.Height = polaczDO.Top - tmpLinia.Top;
                    

                    //^^^^^^^^^^^^^^^
                    tmpLinia.poprzedniBlok = polaczOD;
                    tmpLinia.nastepnyBlok = polaczDO;
                    polaczOD.nastepnaLinia = tmpLinia;
                    polaczDO.poprzedniaLinia = tmpLinia;
                    //^^^^^^^^^^^^^^^
                    panel1.Controls.Add(tmpLinia);
                    tmpLinia = null;
                    return;
                }
            }
            else if (polaczOD.Top > (polaczDO.Top + polaczDO.Height))
            {
                if ((polaczOD.Left <= (polaczDO.Left + polaczDO.Width)) && (polaczOD.Left >= (polaczDO.Left + ((polaczDO.Width) / 2))))
                {
                    double temp;
                    LiniaPion tmpLinia;

                    temp = ((polaczOD.Left + polaczDO.Left + polaczDO.Width) / 2) - polaczOD.Left;
                    polaczOD.punkty[1].X = (Int32)temp;
                    polaczOD.punkty[1].Y = 2;

                    polaczDO.punkty[0].X = polaczDO.Width - (Int32)temp;
                    polaczDO.punkty[0].Y = polaczDO.Height - 2;

                    tmpLinia = new LiniaPion(strzalkaUpDown.up);

                    tmpLinia.Top = polaczDO.Top + polaczDO.punkty[0].Y;
                    tmpLinia.Left = polaczDO.Left + polaczDO.punkty[0].X;

                    tmpLinia.Width = 3;
                    tmpLinia.Height = polaczOD.Top - tmpLinia.Top;
                    

                    //^^^^^^^^^^^^^^^
                    tmpLinia.poprzedniBlok = polaczOD;
                    tmpLinia.nastepnyBlok = polaczDO;
                    polaczOD.nastepnaLinia = tmpLinia;
                    polaczDO.poprzedniaLinia = tmpLinia;
                    //^^^^^^^^^^^^^^^
                    panel1.Controls.Add(tmpLinia);
                    tmpLinia = null;
                    return;
                }
                if ((polaczDO.Left <= (polaczOD.Left + polaczOD.Width)) && (polaczDO.Left >= (polaczOD.Left + ((polaczOD.Width) / 2))))
                {
                    double temp;
                    LiniaPion tmpLinia;

                    temp = ((polaczDO.Left + polaczOD.Left + polaczOD.Width) / 2) - polaczDO.Left;
                    polaczOD.punkty[1].X = polaczDO.Width - (Int32)temp;
                    polaczOD.punkty[1].Y = 2;

                    polaczDO.punkty[0].X = (Int32)temp;
                    polaczDO.punkty[0].Y = polaczOD.Height - 2;

                    tmpLinia = new LiniaPion(strzalkaUpDown.up);

                    tmpLinia.Top = polaczDO.Top + polaczDO.punkty[0].Y;
                    tmpLinia.Left = polaczDO.Left + polaczDO.punkty[0].X;

                    tmpLinia.Width = 3;
                    tmpLinia.Height = polaczOD.Top - tmpLinia.Top;
                    

                    //^^^^^^^^^^^^^^^
                    tmpLinia.poprzedniBlok = polaczOD;
                    tmpLinia.nastepnyBlok = polaczDO;
                    polaczOD.nastepnaLinia = tmpLinia;
                    polaczDO.poprzedniaLinia = tmpLinia;
                    //^^^^^^^^^^^^^^^
                    panel1.Controls.Add(tmpLinia);
                    tmpLinia = null;
                    return;
                }
                if ((polaczDO.Left <= (polaczOD.Left + (polaczOD.Width) / 2)) && (polaczDO.Left >= polaczOD.Left))
                {
                    double temp;
                    LiniaPion tmpLinia;

                    temp = ((polaczDO.Left + polaczOD.Left + polaczOD.Width) / 2) - polaczDO.Left;
                    polaczOD.punkty[1].X = polaczDO.Width - (Int32)temp;
                    polaczOD.punkty[1].Y = 2;

                    polaczDO.punkty[0].X = (Int32)temp;
                    polaczDO.punkty[0].Y = polaczOD.Height - 2;

                    tmpLinia = new LiniaPion(strzalkaUpDown.up);

                    tmpLinia.Top = polaczDO.Top + polaczDO.punkty[0].Y;
                    tmpLinia.Left = polaczDO.Left + polaczDO.punkty[0].X;

                    tmpLinia.Width = 3;
                    tmpLinia.Height = polaczOD.Top - tmpLinia.Top;
                    

                    //^^^^^^^^^^^^^^^
                    tmpLinia.poprzedniBlok = polaczOD;
                    tmpLinia.nastepnyBlok = polaczDO;
                    polaczOD.nastepnaLinia = tmpLinia;
                    polaczDO.poprzedniaLinia = tmpLinia;
                    //^^^^^^^^^^^^^^^
                    panel1.Controls.Add(tmpLinia);
                    tmpLinia = null;
                    return;
                }
                if ((polaczOD.Left <= (polaczDO.Left + (polaczDO.Width) / 2)) && (polaczOD.Left >= polaczDO.Left))
                {
                    double temp;
                    LiniaPion tmpLinia;

                    temp = ((polaczOD.Left + polaczDO.Left + polaczDO.Width) / 2) - polaczOD.Left;
                    polaczOD.punkty[1].X = (Int32)temp;
                    polaczOD.punkty[1].Y = 2;

                    polaczDO.punkty[0].X = polaczDO.Width - (Int32)temp;
                    polaczDO.punkty[0].Y = polaczOD.Height - 2;

                    tmpLinia = new LiniaPion(strzalkaUpDown.up);

                    tmpLinia.Top = polaczDO.Top + polaczDO.punkty[0].Y;
                    tmpLinia.Left = polaczDO.Left + polaczDO.punkty[0].X;

                    tmpLinia.Width = 3;
                    tmpLinia.Height = polaczOD.Top - tmpLinia.Top;
                    

                    //^^^^^^^^^^^^^^^
                    tmpLinia.poprzedniBlok = polaczOD;
                    tmpLinia.nastepnyBlok = polaczDO;
                    polaczOD.nastepnaLinia = tmpLinia;
                    polaczDO.poprzedniaLinia = tmpLinia;
                    //^^^^^^^^^^^^^^^
                    panel1.Controls.Add(tmpLinia);
                    tmpLinia = null;
                    return;
                }
            }
            else if (((polaczOD.Top + polaczOD.Height) >= polaczDO.Top) && (polaczOD.Top <= (polaczDO.Top + polaczDO.Height)))
            {
                if ((polaczOD.Left + polaczOD.Width) < polaczDO.Left)
                {
                    if (polaczOD.Top <= polaczDO.Top)
                    {
                        double temp;
                        LiniaPoz tmpLinia;

                        temp = ((polaczOD.Top + polaczDO.Top + polaczDO.Height) / 2) - polaczOD.Top;
                        polaczOD.punkty[1].X = polaczOD.Left + polaczOD.Width;
                        polaczOD.punkty[1].Y = (Int32)temp;

                        polaczDO.punkty[0].X = 2;
                        polaczDO.punkty[0].Y = polaczDO.Height - (Int32)temp;

                        tmpLinia = new LiniaPoz(strzalkaLeftRight.right);

                        tmpLinia.Top = polaczOD.Top + polaczOD.punkty[1].Y;
                        tmpLinia.Left = polaczOD.Left + polaczOD.Width-2;

                        tmpLinia.Width = polaczDO.Left - tmpLinia.Left;
                        tmpLinia.Height = 3;
                        

                        //^^^^^^^^^^^^^^^
                        tmpLinia.poprzedniBlok = polaczOD;
                        tmpLinia.nastepnyBlok = polaczDO;
                        polaczOD.nastepnaLinia = tmpLinia;
                        polaczDO.poprzedniaLinia = tmpLinia;
                        //^^^^^^^^^^^^^^^
                        panel1.Controls.Add(tmpLinia);
                        tmpLinia = null;
                        return;
                    }
                    if (polaczOD.Top > polaczDO.Top)
                    {
                        double temp;
                        LiniaPoz tmpLinia;

                        temp = ((polaczDO.Top + polaczOD.Top + polaczOD.Height) / 2) - polaczDO.Top;
                        polaczOD.punkty[1].X = polaczOD.Left + polaczOD.Width;
                        polaczOD.punkty[1].Y = polaczOD.Height - (Int32)temp;

                        polaczDO.punkty[0].X = 2;
                        polaczDO.punkty[0].Y = (Int32)temp;

                        tmpLinia = new LiniaPoz(strzalkaLeftRight.right);

                        tmpLinia.Top = polaczDO.Top + polaczDO.punkty[0].Y;
                        tmpLinia.Left = polaczOD.Left + polaczOD.Width - 2;

                        tmpLinia.Width = polaczDO.Left - tmpLinia.Left;
                        tmpLinia.Height = 3;
                        

                        //^^^^^^^^^^^^^^^
                        tmpLinia.poprzedniBlok = polaczOD;
                        tmpLinia.nastepnyBlok = polaczDO;
                        polaczOD.nastepnaLinia = tmpLinia;
                        polaczDO.poprzedniaLinia = tmpLinia;
                        //^^^^^^^^^^^^^^^
                        panel1.Controls.Add(tmpLinia);
                        tmpLinia = null;
                        return;
                    }
                }
                else if(polaczOD.Left > (polaczDO.Left + polaczDO.Width))
                {
                    if (polaczOD.Top <= polaczDO.Top)
                    {
                        double temp;
                        LiniaPoz tmpLinia;

                        temp = ((polaczOD.Top + polaczDO.Top + polaczDO.Height) / 2) - polaczOD.Top;
                        polaczOD.punkty[1].X = 2;
                        polaczOD.punkty[1].Y = (Int32)temp;

                        polaczDO.punkty[0].X = polaczDO.Left + polaczDO.Width;
                        polaczDO.punkty[0].Y = polaczDO.Height - (Int32)temp;

                        tmpLinia = new LiniaPoz(strzalkaLeftRight.left);

                        tmpLinia.Top = polaczOD.Top + polaczOD.punkty[1].Y;
                        tmpLinia.Left = polaczDO.Left + polaczDO.Width-2;

                        tmpLinia.Width = polaczOD.Left - tmpLinia.Left;
                        tmpLinia.Height = 3;
                        

                        //^^^^^^^^^^^^^^^
                        tmpLinia.poprzedniBlok = polaczOD;
                        tmpLinia.nastepnyBlok = polaczDO;
                        polaczOD.nastepnaLinia = tmpLinia;
                        polaczDO.poprzedniaLinia = tmpLinia;
                        //^^^^^^^^^^^^^^^
                        panel1.Controls.Add(tmpLinia);
                        tmpLinia = null;
                        return;
                    }
                    if (polaczOD.Top > polaczDO.Top)
                    {
                        double temp;
                        LiniaPoz tmpLinia;

                        temp = ((polaczDO.Top + polaczOD.Top + polaczOD.Height) / 2) - polaczDO.Top;
                        polaczOD.punkty[1].X = 2;
                        polaczOD.punkty[1].Y = polaczOD.Height - (Int32)temp;

                        polaczDO.punkty[0].X = polaczDO.Left + polaczDO.Width;
                        polaczDO.punkty[0].Y = (Int32)temp;

                        tmpLinia = new LiniaPoz(strzalkaLeftRight.left);

                        tmpLinia.Top = polaczDO.Top + polaczDO.punkty[0].Y;
                        tmpLinia.Left = polaczDO.Left + polaczDO.Width - 2;

                        tmpLinia.Width = polaczOD.Left - tmpLinia.Left;
                        tmpLinia.Height = 3;
                        

                        //^^^^^^^^^^^^^^^
                        tmpLinia.poprzedniBlok = polaczOD;
                        tmpLinia.nastepnyBlok = polaczDO;
                        polaczOD.nastepnaLinia = tmpLinia;
                        polaczDO.poprzedniaLinia = tmpLinia;
                        //^^^^^^^^^^^^^^^
                        panel1.Controls.Add(tmpLinia);
                        tmpLinia = null;
                        return;
                    }
                }
            }
            if (((polaczOD.Left + polaczOD.Width) < polaczDO.Left) && ((polaczOD.Top + polaczOD.Height) < polaczDO.Top))
            {
                LiniaPion tmpPion;
                LiniaPoz tmpPoz;

                tmpPion = new LiniaPion(strzalkaUpDown.none);
                tmpPion.Top = polaczOD.Top + polaczOD.Height;
                tmpPion.Left = polaczOD.Left + polaczOD.Width / 2;
                tmpPion.Width = 4;
                tmpPion.Height = (polaczDO.Top + polaczDO.Height / 2) - tmpPion.Top;
                tmpPion.poprzedniBlok = polaczOD;
                tmpPion.nastepnyBlok = null;
                tmpPion.BringToFront();

                tmpPoz = new LiniaPoz(strzalkaLeftRight.right);
                tmpPoz.Top = tmpPion.Top + tmpPion.Height-2;
                tmpPoz.Left = tmpPion.Left;
                tmpPoz.Height = 4;
                tmpPoz.Width = polaczDO.Left - tmpPoz.Left;
                tmpPoz.nastepnyBlok = polaczDO;
                tmpPoz.poprzedniaLinia = tmpPion;

                tmpPion.nastepnaLinia = tmpPoz;

                polaczOD.nastepnaLinia = tmpPion;
                polaczDO.poprzedniaLinia = tmpPoz;

                panel1.Controls.Add(tmpPion);
                panel1.Controls.Add(tmpPoz);
                tmpPion = null;
                tmpPoz = null;
                return;
            }
            if ((polaczOD.Left > (polaczDO.Left + polaczDO.Width)) && ((polaczOD.Top + polaczOD.Height) < polaczDO.Top))
            {
                LiniaPion tmpPion;
                LiniaPoz tmpPoz;

                tmpPion = new LiniaPion(strzalkaUpDown.none);
                tmpPion.Top = polaczOD.Top + polaczOD.Height;
                tmpPion.Left = polaczOD.Left + polaczOD.Width / 2;
                tmpPion.Width = 4;
                tmpPion.Height = (polaczDO.Top + polaczDO.Height / 2) - tmpPion.Top;
                tmpPion.poprzedniBlok = polaczOD;
                tmpPion.nastepnyBlok = null;
                tmpPion.BringToFront();

                tmpPoz = new LiniaPoz(strzalkaLeftRight.left);
                tmpPoz.Top = tmpPion.Top + tmpPion.Height - 2;
                tmpPoz.Left = polaczDO.Left + polaczDO.Width;
                tmpPoz.Height = 4;
                tmpPoz.Width = tmpPion.Left - tmpPoz.Left; 
                tmpPoz.nastepnyBlok = polaczDO;
                tmpPoz.poprzedniaLinia = tmpPion;

                tmpPion.nastepnaLinia = tmpPoz;

                polaczOD.nastepnaLinia = tmpPion;
                polaczDO.poprzedniaLinia = tmpPoz;

                panel1.Controls.Add(tmpPion);
                panel1.Controls.Add(tmpPoz);
                tmpPion = null;
                tmpPoz = null;
                return;
            }
            if ((polaczOD.Left > (polaczDO.Left + polaczDO.Width)) && (polaczOD.Top > (polaczDO.Top + polaczDO.Height)))
            {
                LiniaPion tmpPion;
                LiniaPoz tmpPoz;

                tmpPion = new LiniaPion(strzalkaUpDown.none);
                tmpPion.Top = polaczDO.Top + polaczDO.Height / 2;
                tmpPion.Left = polaczOD.Left + polaczOD.Width / 2;
                tmpPion.Width = 4;
                tmpPion.Height =polaczOD.Top - tmpPion.Top;
                tmpPion.poprzedniBlok = polaczOD;
                tmpPion.nastepnyBlok = null;
                tmpPion.BringToFront();

                tmpPoz = new LiniaPoz(strzalkaLeftRight.left);
                tmpPoz.Top = tmpPion.Top;
                tmpPoz.Left = polaczDO.Left + polaczDO.Width;
                tmpPoz.Height = 4;
                tmpPoz.Width = tmpPion.Left - tmpPoz.Left;
                tmpPoz.nastepnyBlok = polaczDO;
                tmpPoz.poprzedniaLinia = tmpPion;

                tmpPion.nastepnaLinia = tmpPoz;

                polaczOD.nastepnaLinia = tmpPion;
                polaczDO.poprzedniaLinia = tmpPoz;

                panel1.Controls.Add(tmpPion);
                panel1.Controls.Add(tmpPoz);
                tmpPion = null;
                tmpPoz = null;
                return;
            }
            if (((polaczOD.Left + polaczOD.Width) < polaczDO.Left) && (polaczOD.Top > (polaczDO.Top + polaczDO.Height)))
            {
                LiniaPion tmpPion;
                LiniaPoz tmpPoz;

                tmpPion = new LiniaPion(strzalkaUpDown.none);
                tmpPion.Top = polaczDO.Top + polaczDO.Height / 2;
                tmpPion.Left = polaczOD.Left + polaczOD.Width / 2;
                tmpPion.Width = 4;
                tmpPion.Height = polaczOD.Top - tmpPion.Top;
                tmpPion.poprzedniBlok = polaczOD;
                tmpPion.nastepnyBlok = null;
                tmpPion.BringToFront();

                tmpPoz = new LiniaPoz(strzalkaLeftRight.right);
                tmpPoz.Top = tmpPion.Top;
                tmpPoz.Left = tmpPion.Left;
                tmpPoz.Height = 4;
                tmpPoz.Width = polaczDO.Left - tmpPoz.Left;
                tmpPoz.nastepnyBlok = polaczDO;
                tmpPoz.poprzedniaLinia = tmpPion;

                tmpPion.nastepnaLinia = tmpPoz;

                polaczOD.nastepnaLinia = tmpPion;
                polaczDO.poprzedniaLinia = tmpPoz;

                panel1.Controls.Add(tmpPion);
                panel1.Controls.Add(tmpPoz);
                tmpPion = null;
                tmpPoz = null;
                return;
            }

        }

        private void Symulacja(object sender, DoWorkEventArgs e)
        {
            if (tabBloki.Count == 0)
                return;

            aktualnyBlok = tabBloki[ZnajdzBlok("START")];
            aktualnyBlok.tryb = tryby.aktualny;
            
            while (aktualnyBlok.Name != "STOP" && symuluj == true)
            {
                if (symuluj == false)
                    e.Cancel = true;

                aktualnyBlok.Wykonaj();

                if (aktualnyBlok.nastepnyBlok == null)
                {
                    e.Cancel = true;
                    return;
                }
                aktualnyBlok.tryb = tryby.normal;
                aktualnyBlok = aktualnyBlok.nastepnyBlok;
                aktualnyBlok.tryb = tryby.aktualny;
            }

            if (aktualnyBlok.Name == "STOP")
                symuluj = false;
        }

        private void pełnaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (symuluj == false)
            {
                symuluj = true;

                tsPracaKrokowa.Visible = false;

                bw.WorkerReportsProgress = true;
                bw.WorkerSupportsCancellation = true;
                bw.DoWork += new DoWorkEventHandler(Symulacja);

                bw.RunWorkerAsync();
            }
            else
            {
                symuluj = false;

                if (bw.IsBusy)
                    bw.CancelAsync();
            }
        }

        private void krokowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabBloki.Count == 0)
                return;

            if (symuluj == false)
            {
                symuluj = true;
                tsPracaKrokowa.Visible = true;
                
                if (aktualnyBlok != null)
                    aktualnyBlok.tryb = tryby.normal;

                aktualnyBlok = tabBloki[ZnajdzBlok("START")];
                aktualnyBlok.tryb = tryby.aktualny;
            }
            else
            {
                symuluj = false;
                tsPracaKrokowa.Visible = false;

                if (aktualnyBlok != null)
                    aktualnyBlok.tryb = tryby.normal;

                aktualnyBlok = null;
            }
        }

        private void poprzedni_Click(object sender, EventArgs e)
        {
            if (symuluj == true)
            {
                if (aktualnyBlok.poprzedniBlok != null)
                {
                    aktualnyBlok.tryb = tryby.normal;

                    aktualnyBlok = aktualnyBlok.poprzedniBlok;
                    aktualnyBlok.tryb = tryby.aktualny;
                    aktualnyBlok.Wykonaj();
                }
            }
        }

        private void nastepny_Click(object sender, EventArgs e)
        {
            if (symuluj == true)
            {
                if (aktualnyBlok.nastepnyBlok != null)
                {
                    aktualnyBlok.tryb = tryby.normal;

                    aktualnyBlok = aktualnyBlok.nastepnyBlok;
                    aktualnyBlok.tryb = tryby.aktualny;
                    aktualnyBlok.Wykonaj();
                }
            }
        }
    }
}
