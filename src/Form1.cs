using System;
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

        private Bloki polaczOD, polaczDO;

        private int ile = 0;
        private int polowaX;
        private int polowaY;

        private IList<Bloki> tabBloki = new List<Bloki>();

        private Graphics graph;

        private Bloki przenoszony;
        private Type typ;

        private Poziom[] pozK = new Poziom[4];
        private Pion[] pioK = new Pion[4];

        private Point punktKlikuNaBlok;      //punkt w którym kliknięto na blok (przeciwdziałanie przesunięciu bloku bez przesuwania kursora)
        public Bloki zaznaczony;    ////////////

        
        public Form1()
        {
            InitializeComponent();
            graph = panel1.CreateGraphics();
            
        }

        private int ZnajdzBlok(String nazwa)
        {
            int i;
            for (i = 0; i < ile + 1; i++)
                if (tabBloki[i].blok.Name.Equals(nazwa) == true) break;

            return i;
        }

        private void toolStripBlokObliczeniowy_Click(object sender, EventArgs e)
        {
            typ = typeof(BlokObliczeniowy);
            klik = true;
        }

        private void UsunBlok(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                tabBloki.RemoveAt(ZnajdzBlok(((UserControl)sender).Name));
                ((UserControl)sender).Dispose();
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
                Bloki temp2 = new Bloki();      //potrzebne do dodania do listy
                numer++;
                
                if (typ == typeof(BlokSTART))
                {
                    BlokSTART temp = new BlokSTART();

                    temp.Name = "START_" + numer;

                    temp2.typBloku = typeof(BlokSTART);
                    temp2.blok = (UserControl)temp;
                }

                if (typ == typeof(BlokSTOP))
                {
                    BlokSTOP temp = new BlokSTOP();

                    temp.Name = "STOP_" + numer;
                    
                    temp2.typBloku = typeof(BlokSTOP);
                    temp2.blok = (UserControl)temp;
                }

                if (typ == typeof(BlokObliczeniowy))
                {
                    BlokObliczeniowy temp = new BlokObliczeniowy();

                    temp.Name = "BlokObliczeniowy_" + numer;
                    temp.ReDrawText();                      //nie działa

                    temp2.typBloku = typeof(BlokObliczeniowy);
                    temp2.blok = (UserControl)temp;
                }

                if (typ == typeof(BlokDecyzyjny))
                {
                    BlokDecyzyjny temp = new BlokDecyzyjny();

                    temp.Name = "BlokDecyzyjny_" + numer;

                    temp2.typBloku = typeof(BlokDecyzyjny);
                    temp2.blok = (UserControl)temp;
                }

                if (typ == typeof(BlokWeWy))
                {
                    BlokWeWy temp = new BlokWeWy();

                    temp.Name = "BlokWeWy_" + numer;

                    temp2.typBloku = typeof(BlokWeWy);
                    temp2.blok = (UserControl)temp;
                }

                //globalne dla wszystkich bloków

                temp2.blok.Left = ((MouseEventArgs)e).X;
                temp2.blok.Top = ((MouseEventArgs)e).Y;
                temp2.blok.KeyDown += new KeyEventHandler(UsunBlok);
                temp2.blok.MouseDown += new MouseEventHandler(PrzesunStart);
                temp2.blok.MouseMove += new MouseEventHandler(panel1_MouseMove);
                temp2.blok.MouseUp += new MouseEventHandler(panel1_MouseUp);
                tabBloki.Add(temp2);

                panel1.Controls.Add(tabBloki.Last().blok);

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

        private void toolStripBlokStart_Click(object sender, EventArgs e)
        {
            typ = typeof(BlokSTART);
            klik = true;
        }

        public void PrzesunStart(object sender, MouseEventArgs e)
        {
            if (przesun == false)
            {
                przenoszony = tabBloki[ZnajdzBlok(((UserControl)sender).Name)];
                przesun = true;
                punktKlikuNaBlok = new Point();
                punktKlikuNaBlok.X = e.X;
                punktKlikuNaBlok.Y = e.Y;

                for (int i = 0; i < 4; i++)
                {
                    pozK[i] = new Poziom();
                    pioK[i] = new Pion();
                }

                polowaX = (przenoszony.blok.Width) / 2;
                polowaY = (przenoszony.blok.Height) / 2;

                pioK[0].Left = przenoszony.blok.Left;
                pioK[0].Top = przenoszony.blok.Top;

                pozK[0].Left = przenoszony.blok.Left;
                pozK[0].Top = przenoszony.blok.Top;


                pioK[1].Left = przenoszony.blok.Left;
                pioK[1].Top = przenoszony.blok.Top + (przenoszony.blok.Height - pioK[1].Height);

                pozK[1].Left = przenoszony.blok.Left;
                pozK[1].Top = przenoszony.blok.Top + (przenoszony.blok.Height -  pozK[1].Height);


                pioK[2].Left = przenoszony.blok.Left + (przenoszony.blok.Width - pioK[2].Width);
                pioK[2].Top = przenoszony.blok.Top;

                pozK[2].Left = przenoszony.blok.Left + (przenoszony.blok.Width - pozK[2].Width);
                pozK[2].Top = przenoszony.blok.Top;


                pioK[3].Left = przenoszony.blok.Left + (przenoszony.blok.Width - pioK[3].Width);
                pioK[3].Top = przenoszony.blok.Top + (przenoszony.blok.Height - pioK[3].Height);

                pozK[3].Left = przenoszony.blok.Left + (przenoszony.blok.Width - pozK[3].Width);
                pozK[3].Top = przenoszony.blok.Top + (przenoszony.blok.Height - pozK[3].Height);

                for (int i = 0; i < 4; i++)
                {
                    panel1.Controls.Add(pioK[i]);
                    panel1.Controls.Add(pozK[i]);
                }
            }

            //###############################
            if (zaznaczony != null)
            {
                zaznaczony.tryb = tryby.normal;
            }

            if (sender.GetType() == typeof(BlokSTART) ||
                sender.GetType() == typeof(BlokSTOP) ||
                sender.GetType() == typeof(BlokObliczeniowy) ||
                sender.GetType() == typeof(BlokDecyzyjny) ||
                sender.GetType() == typeof(BlokWeWy))
            {
                zaznaczony = (Bloki)sender;
                zaznaczony.tryb = tryby.zaznaczony;

                if (polacz == true)
                {
                    if (polaczOD == null)
                    {
                        polaczOD = zaznaczony;
                    }
                    else
                    {
                        polaczDO = zaznaczony;
                        polacz = false;
                    }
                }
            }
            else
            {
                polacz = false;
                polaczOD = null;
            }
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
                    pozK[0].Left = e.X + przenoszony.blok.Left - polowaX;
                    pozK[0].Top = e.Y + przenoszony.blok.Top - polowaY;

                    pioK[0].Left = e.X + przenoszony.blok.Left - polowaX;
                    pioK[0].Top = e.Y + przenoszony.blok.Top - polowaY;

                    if (pozK[0].Left < panel1.Margin.Left)
                        pozK[0].Left = panel1.Margin.Left;

                    if (pozK[0].Top < panel1.Top)
                        pozK[0].Top = panel1.Top;

                    pozK[1].Left = pozK[0].Left;
                    pozK[1].Top = pozK[0].Top + (przenoszony.blok.Height - pozK[1].Height);

                    pioK[1].Left = pozK[0].Left;
                    pioK[1].Top = pozK[0].Top + (przenoszony.blok.Height - pioK[1].Height);

                    pozK[2].Left = pozK[0].Left + (przenoszony.blok.Width - pozK[2].Width);
                    pozK[2].Top = pozK[0].Top;

                    pioK[2].Left = pioK[0].Left + (przenoszony.blok.Width);
                    pioK[2].Top = pozK[0].Top;

                    pozK[3].Left = pozK[0].Left + (przenoszony.blok.Width - pozK[3].Width);
                    pozK[3].Top = pozK[0].Top + (przenoszony.blok.Height - pozK[3].Height);

                    pioK[3].Left = pioK[0].Left + (przenoszony.blok.Width);
                    pioK[3].Top = pioK[0].Top + (przenoszony.blok.Height - pioK[3].Height);

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
                    przenoszony.blok.Left = pozK[0].Left;
                    przenoszony.blok.Top = pozK[0].Top;
                }
                if (przenoszony.typBloku == typeof(BlokObliczeniowy)) //nie działa
                {
                    ((BlokObliczeniowy)przenoszony.blok).ReDrawText();
                    przenoszony.blok.Refresh();
                }
                przenoszony.BringToFront();
                przesun = false;
                przenoszony = null;
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

        private void toolStripBlokStop_Click(object sender, EventArgs e)
        {
            typ = typeof(BlokSTOP);
            klik = true;
        }

        private void toolStripBlokDecyzyjny_Click(object sender, EventArgs e)
        {
            typ = typeof(BlokDecyzyjny);
            klik = true;
        }

        private void toolStripWeWy_Click(object sender, EventArgs e)
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
        }

        private void RysujPolaczenie()
        {
           // if(polaczDO.Top - polaczOD.Top >=  )



            Pen p = new Pen(Color.Black);
            //graph.DrawLine(
        }
    }
}
