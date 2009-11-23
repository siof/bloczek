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

        private bool klik = false;
        private bool ctrl = false;
        private bool przesun = false;

        private int ile = 0;
        private int polowaX;
        private int polowaY;

        private IList<Bloki> tabBloki = new List<Bloki>();

        private Graphics graph;

        private Bloki przenoszony;
        private Type typ;

        private NaroznikLD nld;
        private NaroznikLG nlg;
        private NaroznikPD npd;
        private NaroznikPG npg;

        private Point punktKlikuNaBlok;      //punkt w którym kliknięto na blok (przeciwdziałanie przesunięciu bloku bez przesuwania kursora)
        
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

                if (nld == null)
                    nld = new NaroznikLD(panel1);
                if (nlg == null)
                    nlg = new NaroznikLG(panel1);
                if (npd == null)
                    npd = new NaroznikPD(panel1);
                if (npg == null)
                    npg = new NaroznikPG(panel1);

                polowaX = (przenoszony.blok.Width) / 2;
                polowaY = (przenoszony.blok.Height) / 2;

                nlg.Left = przenoszony.blok.Left;
                nlg.Top = przenoszony.blok.Top;

                nld.Left = przenoszony.blok.Left;
                nld.Top = przenoszony.blok.Top + (przenoszony.blok.Height - nld.Height);

                npg.Left = przenoszony.blok.Left + (przenoszony.blok.Width - npd.Width);
                npg.Top = przenoszony.blok.Top;

                npd.Left = przenoszony.blok.Left + (przenoszony.blok.Width - npd.Width);
                npd.Top = przenoszony.blok.Top + (przenoszony.blok.Height - npd.Height);

                panel1.Controls.Add(nlg);
                panel1.Controls.Add(nld);
                panel1.Controls.Add(npg);
                panel1.Controls.Add(npd);
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
                    nlg.Left = e.X + przenoszony.blok.Left - polowaX;
                    nlg.Top = e.Y + przenoszony.blok.Top - polowaY;
                    
                    if (nlg.Left < panel1.Margin.Left)
                        nlg.Left = panel1.Margin.Left;

                    if (nlg.Top < panel1.Top)
                        nlg.Top = panel1.Top;

                    nld.Left = nlg.Left;
                    nld.Top = nlg.Top + (przenoszony.blok.Height - nld.Height);

                    npg.Left = nlg.Left + (przenoszony.blok.Width - npd.Width);
                    npg.Top = nlg.Top;

                    npd.Left = nlg.Left + (przenoszony.blok.Width - npd.Width);
                    npd.Top = nlg.Top + (przenoszony.blok.Height - npd.Height);

                    nlg.Refresh();
                    nld.Refresh();
                    npg.Refresh();
                    npd.Refresh();
                }
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {                         
            if (przesun == true)
            {
                if (punktKlikuNaBlok.X != e.X && punktKlikuNaBlok.Y != e.Y) //jeśli zmieniono położenie kursora
                {
                    przenoszony.blok.Left = nlg.Left;
                    przenoszony.blok.Top = nlg.Top;
                }
                if (przenoszony.typBloku == typeof(BlokObliczeniowy)) //nie działa
                {
                    ((BlokObliczeniowy)przenoszony.blok).ReDrawText();
                    przenoszony.blok.Refresh();
                }
                przesun = false;
                przenoszony = null;
                polowaX = 0;
                polowaY = 0;
                panel1.Controls.Remove(nlg);
                panel1.Controls.Remove(nld);
                panel1.Controls.Remove(npg);
                panel1.Controls.Remove(npd);
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
    }
}
