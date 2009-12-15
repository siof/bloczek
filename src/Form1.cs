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
        private bool polacz = false;
        private bool ctrl = false;
        private bool przesun = false;
        private bool symuluj = false;
        private String blokDecTakNie = "";

        private Bloki polaczOD = null, polaczDO = null;
        private int ile = 0;
        private int polowaX;
        private int polowaY;

        private BDOpcje bDOpcje;
        private BOOpcje bOOpcje;
        private BWeWyOpcje bWeWyOpcje;
        private Czytaj czytaj;
        private libbloki.Console console = new libbloki.Console(); 

       
        private IList<Bloki> tabBloki = new List<Bloki>();
        public IList<Zmienna> zmienne = new List<Zmienna>();
        protected IList<Polaczenie> Polaczenia = new List<Polaczenie>();

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

        private void WywolajBOOpcje(object sender, MouseEventArgs e)
        {
            if (bOOpcje != null)
            {
                bOOpcje.Dispose();
            }

            bOOpcje = new BOOpcje((BlokObliczeniowy)sender);
            bOOpcje.ShowDialog();
            bOOpcje.Dispose();
        }

        private void WywolajBDOpcje(object sender, MouseEventArgs e)
        {
            if (bDOpcje != null)
            {
                bDOpcje.Dispose();
            }

            bDOpcje = new BDOpcje((BlokDecyzyjny)sender);
            bDOpcje.ShowDialog();
            bDOpcje.Dispose();
        }

        private void WywolajBWeWyOpcje(object sender, MouseEventArgs e)
        {
            if (bWeWyOpcje != null)
            {
                bWeWyOpcje.Dispose();
            }

            bWeWyOpcje = new BWeWyOpcje((BlokWeWy)sender);
            bWeWyOpcje.ShowDialog();
            bWeWyOpcje.Dispose();
        }

        private void WyczyscZaznaczenie()
        {
            if (zaznaczony != null)
            {
                if (zaznaczony.GetType() == typeof(LiniaPion) ||
                    zaznaczony.GetType() == typeof(LiniaPoz))
                {
                    Polaczenie tmpPol = new Polaczenie(null, 0, null, 0, zaznaczony, null);
                    OdznaczPolaczenie(tmpPol);
                    
                    zaznaczony = null;
                }
                else
                {
                    zaznaczony.tryb = tryby.normal;
                    zaznaczony = null;
                }
            }
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

        private void UsunBlok(object sender, KeyEventArgs e)  //
        {
            if ((e.KeyCode == Keys.Delete) &&(zaznaczony != null))
            {
                Polaczenie tmpPol = new Polaczenie((Bloki)sender, 0, null, 0, null, null);
                UsunPolaczenia(tmpPol);
                tmpPol.RefOD = null;
                tmpPol.RefDO = (Bloki)sender;
                UsunPolaczenia(tmpPol);

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
                    
                    temp2 = (Bloki)temp;
                    temp2.typBloku = typeof(BlokSTART);
                    temp2.Name = "START";
                }

                if (typ == typeof(BlokSTOP))
                {
                    BlokSTOP temp = new BlokSTOP();

                    temp2 = (Bloki)temp;
                    temp2.typBloku = typeof(BlokSTOP);
                    temp2.Name = "STOP";
                }

                if (typ == typeof(BlokObliczeniowy))
                {
                    BlokObliczeniowy temp = new BlokObliczeniowy();
                    
                    temp2 = (Bloki)temp;
                    temp2.typBloku = typeof(BlokObliczeniowy);
                    temp2.listaZmiennych = zmienne;
                    temp2.MouseDoubleClick += new MouseEventHandler(WywolajBOOpcje);
                }

                if (typ == typeof(BlokDecyzyjny))
                {
                    BlokDecyzyjny temp = new BlokDecyzyjny();
                    
                    temp2 = (Bloki)temp;
                    temp2.typBloku = typeof(BlokDecyzyjny);
                    temp2.listaZmiennych = zmienne;
                    temp2.MouseDoubleClick += new MouseEventHandler(WywolajBDOpcje);
                }

                if (typ == typeof(BlokWeWy))
                {
                    BlokWeWy temp = new BlokWeWy();//
                    
                    temp2 = (Bloki)temp;
                    temp2.typBloku = typeof(BlokWeWy);
                    temp2.listaZmiennych = zmienne;
                    temp2.MouseDoubleClick += new MouseEventHandler(WywolajBWeWyOpcje);
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
            else
            {
                WyczyscZaznaczenie();
            }
        }

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {
            foreach (Control c in panel1.Controls)
            {
                c.Update();
            }
        }
    
        private void DodajPolaczenie(Polaczenie p)
        {
            if (p.RefOD == p.RefDO)
                return;
            if (p.RefOD == null || p.RefDO == null)
                return;

                Polaczenie tmpPol = new Polaczenie(p.RefOD,p.IndeksOD,null,0,null,null);
                List<Polaczenie> tmpList = ZnajdzPolaczenia(tmpPol);
                if(tmpList.Count > 0 )
                {
                    UsunPolaczenie(tmpList.First());
                }
            
            p.RefOD.nastepnyBlok[p.IndeksOD] = p.RefDO;
            p.RefDO.poprzedniBlok.Add(p.RefOD);
            p.IndeksDO = p.RefDO.poprzedniBlok.IndexOf(p.RefOD);            
            Polaczenia.Add(p);
            RysujPolaczenia(p);
        }

        private void UsunPolaczenia(Polaczenie p)
        {
            List<Polaczenie> tmpList;
            tmpList = ZnajdzPolaczenia(p);
            if (tmpList.Count > 0)
            {
                foreach (Polaczenie pol in tmpList)
                {
                    UsunPolaczenie(pol);
                }
            }
        }

        private void UsunPolaczenie(Polaczenie p)
        {
            int i;
            i = Polaczenia.IndexOf(p);
            if (i>=0)
            {
                if (Polaczenia[i].RefLinia1 != null)
                {
                    panel1.Controls.Remove(Polaczenia[i].RefLinia1);
                    Polaczenia[i].RefLinia1.Dispose();
                }

                if (Polaczenia[i].RefLinia2 != null)
                {
                    panel1.Controls.Remove(Polaczenia[i].RefLinia2);
                    Polaczenia[i].RefLinia2.Dispose();
                }
                Polaczenia[i].RefOD.nastepnyBlok[Polaczenia[i].IndeksOD] = null;
                Polaczenia[i].RefDO.poprzedniBlok.Remove(Polaczenia[i].RefOD);
                Polaczenia.RemoveAt(i);
            }
        }

        private List<Polaczenie> ZnajdzPolaczenia(Polaczenie p)
        {
            List<Polaczenie> tmpList = new List<Polaczenie>();
            if (p.RefOD != null && p.RefDO != null)
            {
                foreach (Polaczenie pol in Polaczenia)
                {
                    if (p.RefOD == pol.RefOD && p.RefDO == pol.RefDO)
                    {
                        tmpList.Add(pol);
                    }
                }
            }
            else
            {
                if (p.RefOD != null)
                {
                    foreach (Polaczenie pol in Polaczenia)
                    {
                        if (p.RefOD == pol.RefOD && p.IndeksOD == pol.IndeksOD)
                            tmpList.Add(pol);
                    }
                }

                if (p.RefDO != null)
                {
                    foreach (Polaczenie pol in Polaczenia)
                    {
                        if (p.RefDO == pol.RefDO)
                            tmpList.Add(pol);
                    }
                }

                if (p.RefLinia1 != null)
                {
                    foreach (Polaczenie pol in Polaczenia)
                    {
                        if (p.RefLinia1 == pol.RefLinia1 || p.RefLinia1 == pol.RefLinia2)
                            tmpList.Add(pol);
                    }
                }

                if (p.RefLinia2 != null)
                {
                    foreach (Polaczenie pol in Polaczenia)
                    {
                        if (p.RefLinia2 == pol.RefLinia1 || p.RefLinia2 == pol.RefLinia2)
                            tmpList.Add(pol);
                    }
                }
            }
            return tmpList;
        }

        private void ZaznaczPolaczenie(object sender, MouseEventArgs e)
        {
            zaznaczony = (Bloki)sender;
            Polaczenie tmpPol = new Polaczenie(null,0,null,0,(Bloki)sender,null);
            List<Polaczenie> tmpList;
            tmpList = ZnajdzPolaczenia(tmpPol);
            if (tmpList.Count > 0)
            {
                if (tmpList.First().RefLinia1 != null)
                {
                    tmpList.First().RefLinia1.tryb = tryby.zaznaczony;
                    tmpList.First().RefLinia1.Refresh();
                }
                
                if (tmpList.First().RefLinia2 != null)
                {
                    tmpList.First().RefLinia2.tryb = tryby.zaznaczony;
                    tmpList.First().RefLinia2.Refresh();
                }
            }
        }

        private void OdznaczPolaczenie(Polaczenie p)
        {
            List<Polaczenie> tmpList;
            tmpList = ZnajdzPolaczenia(p);
            if (tmpList.Count > 0)
            {
                if (tmpList.First().RefLinia1 != null)
                {
                    tmpList.First().RefLinia1.tryb = tryby.normal;
                    tmpList.First().RefLinia1.Refresh();
                }
               
               if (tmpList.First().RefLinia2 != null)
                {
                    tmpList.First().RefLinia2.tryb = tryby.normal;
                    tmpList.First().RefLinia2.Refresh();
                }
            }
        }

        private void RysujPolaczenia(Polaczenie p)
        {
            List<Polaczenie> tmpList = new List<Polaczenie>();
            if (p.RefOD != null && p.RefDO != null)
            {
                foreach (Polaczenie pol in Polaczenia)
                {
                    if (p.RefOD == pol.RefOD && p.RefDO == pol.RefDO)
                        RysujPolaczenie(pol);
                }
            }
            else
            {
                if (p.RefOD != null)
                {
                    foreach (Polaczenie pol in Polaczenia)
                    {
                        if (p.RefOD == pol.RefOD)
                            RysujPolaczenie(pol);
                    }
                }
               
               if (p.RefDO != null)
                {
                    foreach (Polaczenie pol in Polaczenia)
                    {
                        if (p.RefDO == pol.RefDO)
                            RysujPolaczenie(pol);
                    }
                }
               
                if (p.RefLinia1 != null)
                {
                    foreach (Polaczenie pol in Polaczenia)
                    {
                        if (p.RefLinia1 == pol.RefLinia1 || p.RefLinia1 == pol.RefLinia2)
                            RysujPolaczenie(pol);
                    }
                }
                
                if (p.RefLinia2 != null)
                {
                    foreach (Polaczenie pol in Polaczenia)
                    {
                        if (p.RefLinia2 == pol.RefLinia1 || p.RefLinia2 == pol.RefLinia2)
                            RysujPolaczenie(pol);
                    }
                }
            }
        }

        private void RysujPolaczenie(Polaczenie p)
        {
            if (p.RefLinia1 != null)
            {
                panel1.Controls.Remove(p.RefLinia1);
                p.RefLinia1.Dispose();
            }
            
            if (p.RefLinia2 != null)
            {
                panel1.Controls.Remove(p.RefLinia2);
                p.RefLinia2.Dispose();
            }
            if ((p.RefOD.GetType() == typeof(BlokDecyzyjny)) && (p.RefDO.GetType() == typeof(BlokDecyzyjny)))
            {
                if (p.IndeksOD == 0)    //NIE
                {
                    //linia lamana
                    //p.RefDO = p.RefOD.nastepnyBlok[0];
                    if ((p.RefOD.Left < p.RefDO.Left + p.RefDO.Width / 2) && (p.RefOD.Top+ p.RefOD.Height / 2 < p.RefDO.Top ))
                    {
                        LiniaPion tmpPion;
                        LiniaPoz tmpPoz;

                        tmpPion = new LiniaPion(strzalkaUpDown.none);
                        tmpPion.Top = p.RefOD.Top + p.RefOD.Height / 2;
                        tmpPion.Left = p.RefOD.Left;
                        tmpPion.Width = 4;
                        tmpPion.Height = (p.RefDO.Top) - tmpPion.Top;

                        tmpPoz = new LiniaPoz(strzalkaLeftRight.right);
                        tmpPoz.Top = tmpPion.Top + tmpPion.Height - 2;
                        tmpPoz.Left = tmpPion.Left;
                        tmpPoz.Height = 4;
                        tmpPoz.Width = p.RefDO.Left+p.RefDO.Width/2 - tmpPoz.Left;

                        tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        p.RefLinia1 = tmpPion;
                        p.RefLinia2 = tmpPoz;

                        panel1.Controls.Add(tmpPion);
                        panel1.Controls.Add(tmpPoz);
                        tmpPion = null;
                        tmpPoz = null;

                    }

                    if ((p.RefOD.Left > p.RefDO.Left + p.RefDO.Width / 2) && (p.RefOD.Top + p.RefOD.Height / 2< p.RefDO.Top ))
                    {
                        LiniaPion tmpPion;
                        LiniaPoz tmpPoz;

                        tmpPion = new LiniaPion(strzalkaUpDown.none);
                        tmpPion.Top = p.RefOD.Top + p.RefOD.Height / 2;
                        tmpPion.Left = p.RefOD.Left;
                        tmpPion.Width = 4;
                        tmpPion.Height = p.RefDO.Top - tmpPion.Top;

                        tmpPoz = new LiniaPoz(strzalkaLeftRight.left);
                        tmpPoz.Top = tmpPion.Top + tmpPion.Height - 2;
                        tmpPoz.Left = p.RefDO.Left + p.RefDO.Width/2;
                        tmpPoz.Height = 4;
                        tmpPoz.Width = tmpPion.Left - tmpPoz.Left;

                        tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        p.RefLinia1 = tmpPion;
                        p.RefLinia2 = tmpPoz;

                        panel1.Controls.Add(tmpPion);
                        panel1.Controls.Add(tmpPoz);
                        tmpPion = null;
                        tmpPoz = null;
                    }

                    if ((p.RefOD.Left > p.RefDO.Left + p.RefDO.Width / 2) && (p.RefOD.Top + p.RefOD.Height / 2 > p.RefDO.Top))
                    {
                        LiniaPion tmpPion;
                        LiniaPoz tmpPoz;

                        tmpPion = new LiniaPion(strzalkaUpDown.none);
                        tmpPion.Top = p.RefDO.Top;
                        tmpPion.Left = p.RefOD.Left;
                        tmpPion.Width = 4;
                        tmpPion.Height = p.RefOD.Top - tmpPion.Top + 4;

                        tmpPoz = new LiniaPoz(strzalkaLeftRight.left);
                        tmpPoz.Top = tmpPion.Top;
                        tmpPoz.Left = p.RefDO.Left + p.RefDO.Width/2;
                        tmpPoz.Height = 4;
                        tmpPoz.Width = tmpPion.Left - tmpPoz.Left + 3;

                        tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        p.RefLinia1 = tmpPion;
                        p.RefLinia2 = tmpPoz;

                        panel1.Controls.Add(tmpPion);
                        panel1.Controls.Add(tmpPoz);

                        tmpPion = null;
                        tmpPoz = null;
                    }

                    if ((p.RefOD.Left < p.RefDO.Left+p.RefDO.Width/2) && (p.RefOD.Top+p.RefOD.Height/2 > p.RefDO.Top))
                    {

                        LiniaPion tmpPion;
                        LiniaPoz tmpPoz;

                        tmpPion = new LiniaPion(strzalkaUpDown.none);
                        tmpPion.Top = p.RefDO.Top;
                        tmpPion.Left = p.RefOD.Left;
                        tmpPion.Width = 4;
                        tmpPion.Height = p.RefOD.Top - tmpPion.Top + 4;

                        tmpPoz = new LiniaPoz(strzalkaLeftRight.right);
                        tmpPoz.Top = tmpPion.Top;
                        tmpPoz.Left = tmpPion.Left;
                        tmpPoz.Height = 4;
                        tmpPoz.Width = p.RefDO.Left+p.RefDO.Width/2 - tmpPoz.Left + 3;

                        tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        p.RefLinia1 = tmpPion;
                        p.RefLinia2 = tmpPoz;

                        panel1.Controls.Add(tmpPion);
                        panel1.Controls.Add(tmpPoz);
                        tmpPion = null;
                        tmpPoz = null;
                    }
                }

                if (p.IndeksOD == 1)   //TAK
                {
                    p.RefDO = p.RefOD.nastepnyBlok[1];
                    //linia lamana
                    if ((p.RefOD.Left + p.RefOD.Width < p.RefDO.Left+ p.RefDO.Width/2) && (p.RefOD.Top+p.RefOD.Height/2 < p.RefDO.Top))
                    {
                        LiniaPion tmpPion;
                        LiniaPoz tmpPoz;

                        tmpPion = new LiniaPion(strzalkaUpDown.none);
                        tmpPion.Top = p.RefOD.Top + p.RefOD.Height / 2;
                        tmpPion.Left = p.RefOD.Left + p.RefOD.Width;
                        tmpPion.Width = 4;
                        tmpPion.Height = p.RefDO.Top - tmpPion.Top;

                        tmpPoz = new LiniaPoz(strzalkaLeftRight.right);
                        tmpPoz.Top = tmpPion.Top + tmpPion.Height - 2;
                        tmpPoz.Left = tmpPion.Left;
                        tmpPoz.Height = 4;
                        tmpPoz.Width = p.RefDO.Left+p.RefDO.Width/2 - tmpPoz.Left;

                        tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        p.RefLinia1 = tmpPion;
                        p.RefLinia2 = tmpPoz;

                        panel1.Controls.Add(tmpPion);
                        panel1.Controls.Add(tmpPoz);
                        tmpPion = null;
                        tmpPoz = null;
                    }

                    if ((p.RefOD.Left + p.RefOD.Width > p.RefDO.Left + p.RefDO.Width / 2) && (p.RefOD.Top + p.RefOD.Height / 2 < p.RefDO.Top))
                    {
                        LiniaPion tmpPion;
                        LiniaPoz tmpPoz;

                        tmpPion = new LiniaPion(strzalkaUpDown.none);
                        tmpPion.Top = p.RefOD.Top + p.RefOD.Height / 2;
                        tmpPion.Left = p.RefOD.Left + p.RefOD.Width;
                        tmpPion.Width = 4;
                        tmpPion.Height = p.RefDO.Top - tmpPion.Top;

                        tmpPoz = new LiniaPoz(strzalkaLeftRight.left);
                        tmpPoz.Top = tmpPion.Top + tmpPion.Height - 2;
                        tmpPoz.Left = p.RefDO.Left + p.RefDO.Width/2;
                        tmpPoz.Height = 4;
                        tmpPoz.Width = tmpPion.Left - tmpPoz.Left;

                        tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        p.RefLinia1 = tmpPion;
                        p.RefLinia2 = tmpPoz;

                        panel1.Controls.Add(tmpPion);
                        panel1.Controls.Add(tmpPoz);
                        tmpPion = null;
                        tmpPoz = null;

                    }

                    if ((p.RefOD.Left + p.RefOD.Width > p.RefDO.Left + p.RefDO.Width / 2) && (p.RefOD.Top + p.RefOD.Height / 2 > p.RefDO.Top))
                    {
                        LiniaPion tmpPion;
                        LiniaPoz tmpPoz;

                        tmpPion = new LiniaPion(strzalkaUpDown.none);
                        tmpPion.Top = p.RefDO.Top;
                        tmpPion.Left = p.RefOD.Left + p.RefOD.Width;
                        tmpPion.Width = 4;
                        tmpPion.Height = p.RefOD.Top - tmpPion.Top + 4;

                        tmpPoz = new LiniaPoz(strzalkaLeftRight.left);
                        tmpPoz.Top = tmpPion.Top;
                        tmpPoz.Left = p.RefDO.Left + p.RefDO.Width/2;
                        tmpPoz.Height = 4;
                        tmpPoz.Width = tmpPion.Left - tmpPoz.Left + 3;

                        tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        p.RefLinia1 = tmpPion;
                        p.RefLinia2 = tmpPoz;

                        panel1.Controls.Add(tmpPion);
                        panel1.Controls.Add(tmpPoz);

                        tmpPion = null;
                        tmpPoz = null;

                    }

                    if ((p.RefOD.Left + p.RefOD.Width < p.RefDO.Left + p.RefDO.Width / 2) && (p.RefOD.Top + p.RefOD.Height / 2 > p.RefDO.Top))
                    {
                        LiniaPion tmpPion;
                        LiniaPoz tmpPoz;

                        tmpPion = new LiniaPion(strzalkaUpDown.none);
                        tmpPion.Top = p.RefDO.Top;
                        tmpPion.Left = p.RefOD.Left + p.RefOD.Width;
                        tmpPion.Width = 4;
                        tmpPion.Height = p.RefOD.Top - tmpPion.Top + 4;
                        tmpPion.poprzedniBlok.Add(p.RefOD);
                        //tmpPion.nastepnyBlok[0] = p.RefDO;

                        tmpPoz = new LiniaPoz(strzalkaLeftRight.right);
                        tmpPoz.Top = tmpPion.Top;
                        tmpPoz.Left = tmpPion.Left;
                        tmpPoz.Height = 4;
                        tmpPoz.Width = p.RefDO.Left + p.RefDO.Width/2 - tmpPoz.Left + 3;

                        tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        p.RefLinia1 = tmpPion;
                        p.RefLinia2 = tmpPoz;

                        panel1.Controls.Add(tmpPion);
                        panel1.Controls.Add(tmpPoz);
                        tmpPion = null;
                        tmpPoz = null;
                    }
                }
                return;
            }


            
//////////////////////////////////////////////
            if (p.RefOD.GetType() == typeof(BlokDecyzyjny))
            {
                if (p.IndeksOD == 0)    //NIE
                {
                    //linia lamana
                    p.RefDO = p.RefOD.nastepnyBlok[0];
                    if ((p.RefOD.Left < p.RefDO.Left) && (p.RefOD.Top < p.RefDO.Top))
                    {
                        LiniaPion tmpPion;
                        LiniaPoz tmpPoz;

                        tmpPion = new LiniaPion(strzalkaUpDown.none);
                        tmpPion.Top = p.RefOD.Top + p.RefOD.Height / 2;
                        tmpPion.Left = p.RefOD.Left;
                        tmpPion.Width = 4;
                        tmpPion.Height = (p.RefDO.Top + p.RefDO.Height / 2) - tmpPion.Top;

                        tmpPoz = new LiniaPoz(strzalkaLeftRight.right);
                        tmpPoz.Top = tmpPion.Top + tmpPion.Height - 2;
                        tmpPoz.Left = tmpPion.Left;
                        tmpPoz.Height = 4;
                        tmpPoz.Width = p.RefDO.Left - tmpPoz.Left;

                        tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        p.RefLinia1 = tmpPion;
                        p.RefLinia2 = tmpPoz;

                        panel1.Controls.Add(tmpPion);
                        panel1.Controls.Add(tmpPoz);
                        tmpPion = null;
                        tmpPoz = null;

                    }

                    if ((p.RefOD.Left > p.RefDO.Left + 1) && (p.RefOD.Top < p.RefDO.Top))
                    {
                        LiniaPion tmpPion;
                        LiniaPoz tmpPoz;

                        tmpPion = new LiniaPion(strzalkaUpDown.none);
                        tmpPion.Top = p.RefOD.Top + p.RefOD.Height / 2;
                        tmpPion.Left = p.RefOD.Left;
                        tmpPion.Width = 4;
                        tmpPion.Height = (p.RefDO.Top + p.RefDO.Height / 2) - tmpPion.Top;

                        tmpPoz = new LiniaPoz(strzalkaLeftRight.left);
                        tmpPoz.Top = tmpPion.Top + tmpPion.Height - 2;
                        tmpPoz.Left = p.RefDO.Left + p.RefDO.Width;
                        tmpPoz.Height = 4;
                        tmpPoz.Width = tmpPion.Left - tmpPoz.Left;

                        tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        p.RefLinia1 = tmpPion;
                        p.RefLinia2 = tmpPoz;

                        panel1.Controls.Add(tmpPion);
                        panel1.Controls.Add(tmpPoz);
                        tmpPion = null;
                        tmpPoz = null;
                    }

                    if ((p.RefOD.Left > p.RefDO.Left) && (p.RefOD.Top > p.RefDO.Top))
                    {
                        LiniaPion tmpPion;
                        LiniaPoz tmpPoz;

                        tmpPion = new LiniaPion(strzalkaUpDown.none);
                        tmpPion.Top = p.RefDO.Top + p.RefDO.Height / 2;
                        tmpPion.Left = p.RefOD.Left;
                        tmpPion.Width = 4;
                        tmpPion.Height = p.RefOD.Top + p.RefDO.Height / 2 - tmpPion.Top + 4;

                        tmpPoz = new LiniaPoz(strzalkaLeftRight.left);
                        tmpPoz.Top = tmpPion.Top;
                        tmpPoz.Left = p.RefDO.Left + p.RefDO.Width;
                        tmpPoz.Height = 4;
                        tmpPoz.Width = tmpPion.Left - tmpPoz.Left + 3;

                        tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        p.RefLinia1 = tmpPion;
                        p.RefLinia2 = tmpPoz;

                        panel1.Controls.Add(tmpPion);
                        panel1.Controls.Add(tmpPoz);

                        tmpPion = null;
                        tmpPoz = null;
                    }

                    if ((p.RefOD.Left < p.RefDO.Left) && (p.RefOD.Top > p.RefDO.Top))
                    {

                        LiniaPion tmpPion;
                        LiniaPoz tmpPoz;

                        tmpPion = new LiniaPion(strzalkaUpDown.none);
                        tmpPion.Top = p.RefDO.Top + p.RefDO.Height / 2;
                        tmpPion.Left = p.RefOD.Left;
                        tmpPion.Width = 4;
                        tmpPion.Height = p.RefOD.Top + p.RefOD.Height / 2 - tmpPion.Top + 4;

                        tmpPoz = new LiniaPoz(strzalkaLeftRight.right);
                        tmpPoz.Top = tmpPion.Top;
                        tmpPoz.Left = tmpPion.Left;
                        tmpPoz.Height = 4;
                        tmpPoz.Width = p.RefDO.Left - tmpPoz.Left + 3;

                        tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        p.RefLinia1 = tmpPion;
                        p.RefLinia2 = tmpPoz;

                        panel1.Controls.Add(tmpPion);
                        panel1.Controls.Add(tmpPoz);
                        tmpPion = null;
                        tmpPoz = null;
                    }
                }

                if (p.IndeksOD == 1)   //TAK
                {
                    p.RefDO = p.RefOD.nastepnyBlok[1];
                    //linia lamana
                    if ((p.RefOD.Left + p.RefOD.Width < p.RefDO.Left) && (p.RefOD.Top < p.RefDO.Top))
                    {
                        LiniaPion tmpPion;
                        LiniaPoz tmpPoz;

                        tmpPion = new LiniaPion(strzalkaUpDown.none);
                        tmpPion.Top = p.RefOD.Top + p.RefOD.Height / 2;
                        tmpPion.Left = p.RefOD.Left + p.RefOD.Width;
                        tmpPion.Width = 4;
                        tmpPion.Height = (p.RefDO.Top + p.RefDO.Height / 2) - tmpPion.Top;

                        tmpPoz = new LiniaPoz(strzalkaLeftRight.right);
                        tmpPoz.Top = tmpPion.Top + tmpPion.Height - 2;
                        tmpPoz.Left = tmpPion.Left;
                        tmpPoz.Height = 4;
                        tmpPoz.Width = p.RefDO.Left - tmpPoz.Left;

                        tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        p.RefLinia1 = tmpPion;
                        p.RefLinia2 = tmpPoz;

                        panel1.Controls.Add(tmpPion);
                        panel1.Controls.Add(tmpPoz);
                        tmpPion = null;
                        tmpPoz = null;
                    }

                    if ((p.RefOD.Left + p.RefOD.Width > p.RefDO.Left + 1) && (p.RefOD.Top < p.RefDO.Top))
                    {
                        LiniaPion tmpPion;
                        LiniaPoz tmpPoz;

                        tmpPion = new LiniaPion(strzalkaUpDown.none);
                        tmpPion.Top = p.RefOD.Top + p.RefOD.Height / 2;
                        tmpPion.Left = p.RefOD.Left + p.RefOD.Width;
                        tmpPion.Width = 4;
                        tmpPion.Height = (p.RefDO.Top + p.RefDO.Height / 2) - tmpPion.Top;

                        tmpPoz = new LiniaPoz(strzalkaLeftRight.left);
                        tmpPoz.Top = tmpPion.Top + tmpPion.Height - 2;
                        tmpPoz.Left = p.RefDO.Left + p.RefDO.Width;
                        tmpPoz.Height = 4;
                        tmpPoz.Width = tmpPion.Left - tmpPoz.Left;

                        tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        p.RefLinia1 = tmpPion;
                        p.RefLinia2 = tmpPoz;

                        panel1.Controls.Add(tmpPion);
                        panel1.Controls.Add(tmpPoz);
                        tmpPion = null;
                        tmpPoz = null;

                    }

                    if ((p.RefOD.Left + p.RefOD.Width > p.RefDO.Left) && (p.RefOD.Top > p.RefDO.Top))
                    {
                        LiniaPion tmpPion;
                        LiniaPoz tmpPoz;

                        tmpPion = new LiniaPion(strzalkaUpDown.none);
                        tmpPion.Top = p.RefDO.Top + p.RefDO.Height / 2;
                        tmpPion.Left = p.RefOD.Left + p.RefOD.Width;
                        tmpPion.Width = 4;
                        tmpPion.Height = p.RefOD.Top + p.RefDO.Height / 2 - tmpPion.Top + 4;

                        tmpPoz = new LiniaPoz(strzalkaLeftRight.left);
                        tmpPoz.Top = tmpPion.Top;
                        tmpPoz.Left = p.RefDO.Left + p.RefDO.Width;
                        tmpPoz.Height = 4;
                        tmpPoz.Width = tmpPion.Left - tmpPoz.Left + 3;

                        tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        p.RefLinia1 = tmpPion;
                        p.RefLinia2 = tmpPoz;

                        panel1.Controls.Add(tmpPion);
                        panel1.Controls.Add(tmpPoz);

                        tmpPion = null;
                        tmpPoz = null;

                    }

                    if ((p.RefOD.Left + p.RefOD.Width < p.RefDO.Left) && (p.RefOD.Top > p.RefDO.Top))
                    {
                        LiniaPion tmpPion;
                        LiniaPoz tmpPoz;

                        tmpPion = new LiniaPion(strzalkaUpDown.none);
                        tmpPion.Top = p.RefDO.Top + p.RefDO.Height / 2;
                        tmpPion.Left = p.RefOD.Left + p.RefOD.Width;
                        tmpPion.Width = 4;
                        tmpPion.Height = p.RefOD.Top + p.RefOD.Height / 2 - tmpPion.Top + 4;
                        tmpPion.poprzedniBlok.Add(p.RefOD);
                        tmpPion.nastepnyBlok[0] = p.RefDO;

                        tmpPoz = new LiniaPoz(strzalkaLeftRight.right);
                        tmpPoz.Top = tmpPion.Top;
                        tmpPoz.Left = tmpPion.Left;
                        tmpPoz.Height = 4;
                        tmpPoz.Width = p.RefDO.Left - tmpPoz.Left + 3;

                        tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                        p.RefLinia1 = tmpPion;
                        p.RefLinia2 = tmpPoz;

                        panel1.Controls.Add(tmpPion);
                        panel1.Controls.Add(tmpPoz);
                        tmpPion = null;
                        tmpPoz = null;
                    }
                }
                return;
            }

            
//////////////////////////////////////////////
            if (p.RefDO.GetType() == typeof(BlokDecyzyjny))
            {
                if (((p.RefOD.Top + p.RefOD.Height) >= p.RefDO.Top) && (p.RefOD.Top <= (p.RefDO.Top)))
                {
                    if ((p.RefOD.Left + p.RefOD.Width) < p.RefDO.Left)
                    {
                        if (p.RefOD.Top <= p.RefDO.Top)
                        {
                            double temp;
                            LiniaPoz tmpLinia;

                            temp = ((p.RefOD.Top + p.RefDO.Top + p.RefDO.Height) / 2) - p.RefOD.Top;
                            p.RefOD.punkty[1].X = p.RefOD.Left + p.RefOD.Width;
                            p.RefOD.punkty[1].Y = (Int32)temp;

                            p.RefDO.punkty[0].X = 93;
                            p.RefDO.punkty[0].Y = 2;

                            tmpLinia = new LiniaPoz(strzalkaLeftRight.right);

                            tmpLinia.Top = p.RefDO.Top + p.RefDO.punkty[0].Y - 2;
                            tmpLinia.Left = p.RefOD.Left + p.RefOD.Width - 2;

                            tmpLinia.Width = p.RefDO.Left - tmpLinia.Left + p.RefDO.punkty[0].X - 2;
                            tmpLinia.Height = 3;

                            tmpLinia.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                            tmpLinia.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);

                            p.RefLinia1 = tmpLinia;
                            p.RefLinia2 = null;

                            panel1.Controls.Add(tmpLinia);
                            tmpLinia = null;

                        }
                        if (p.RefOD.Top > p.RefDO.Top)
                        {
                            double temp;
                            LiniaPoz tmpLinia;

                            temp = ((p.RefDO.Top + p.RefOD.Top + p.RefOD.Height) / 2) - p.RefDO.Top;
                            p.RefOD.punkty[1].X = p.RefOD.Left + p.RefOD.Width;
                            p.RefOD.punkty[1].Y = p.RefOD.Height - (Int32)temp;

                            p.RefDO.punkty[0].X = 93;
                            p.RefDO.punkty[0].Y = 2;

                            tmpLinia = new LiniaPoz(strzalkaLeftRight.right);

                            tmpLinia.Top = p.RefDO.Top + p.RefDO.punkty[0].Y;
                            tmpLinia.Left = p.RefOD.Left + p.RefOD.Width - 2;

                            tmpLinia.Width = p.RefDO.Left - tmpLinia.Left;
                            tmpLinia.Height = 3;
                            

                            tmpLinia.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                            tmpLinia.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);

                            p.RefLinia1 = tmpLinia;
                            p.RefLinia2 = null;

                            panel1.Controls.Add(tmpLinia);
                            tmpLinia = null;

                        }
                    }
                    else if (p.RefOD.Left > (p.RefDO.Left + p.RefDO.Width))
                    {
                        if (p.RefOD.Top <= p.RefDO.Top)
                        {
                            double temp;
                            LiniaPoz tmpLinia;

                            temp = ((p.RefOD.Top + p.RefDO.Top + p.RefDO.Height) / 2) - p.RefOD.Top;
                            p.RefOD.punkty[1].X = 2;
                            p.RefOD.punkty[1].Y = (Int32)temp;

                            p.RefDO.punkty[0].X = 93;
                            p.RefDO.punkty[0].Y = 2;

                            tmpLinia = new LiniaPoz(strzalkaLeftRight.left);

                            tmpLinia.Top = p.RefDO.Top + p.RefDO.punkty[0].Y;
                            tmpLinia.Left = p.RefDO.Left + p.RefDO.Width / 2 - 2;

                            tmpLinia.Width = p.RefOD.Left - tmpLinia.Left;
                            tmpLinia.Height = 3;

                            tmpLinia.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                            tmpLinia.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);

                            p.RefLinia1 = tmpLinia;
                            p.RefLinia2 = null;

                            panel1.Controls.Add(tmpLinia);
                            tmpLinia = null;

                        }
                        if (p.RefOD.Top > p.RefDO.Top)
                        {
                            double temp;
                            LiniaPoz tmpLinia;

                            temp = ((p.RefDO.Top + p.RefOD.Top + p.RefOD.Height) / 2) - p.RefDO.Top;
                            p.RefOD.punkty[1].X = 2;
                            p.RefOD.punkty[1].Y = p.RefOD.Height - (Int32)temp;

                            p.RefDO.punkty[0].X = 93;
                            p.RefDO.punkty[0].Y = 2;

                            tmpLinia = new LiniaPoz(strzalkaLeftRight.left);

                            tmpLinia.Top = p.RefDO.Top + p.RefDO.punkty[0].Y;
                            tmpLinia.Left = p.RefDO.Left + p.RefDO.Width - 2;

                            tmpLinia.Width = p.RefOD.Left - tmpLinia.Left;
                            tmpLinia.Height = 3;
                            

                            tmpLinia.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                            tmpLinia.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);

                            p.RefLinia1 = tmpLinia;
                            p.RefLinia2 = null;

                            panel1.Controls.Add(tmpLinia);
                            tmpLinia = null;
                        }
                    }
                }

                //linia lamana
                if (((p.RefOD.Left + p.RefOD.Width / 2) < p.RefDO.Left + (p.RefDO.Width / 2)) && ((p.RefOD.Top + p.RefOD.Height) < p.RefDO.Top))
                {
                    LiniaPion tmpPion;
                    LiniaPoz tmpPoz;

                    tmpPion = new LiniaPion(strzalkaUpDown.none);
                    tmpPion.Top = p.RefOD.Top + p.RefOD.Height;
                    tmpPion.Left = p.RefOD.Left + p.RefOD.Width / 2;
                    tmpPion.Width = 4;
                    tmpPion.Height = p.RefDO.Top - tmpPion.Top;


                    tmpPoz = new LiniaPoz(strzalkaLeftRight.right);
                    tmpPoz.Top = tmpPion.Top + tmpPion.Height - 2;
                    tmpPoz.Left = tmpPion.Left;
                    tmpPoz.Height = 4;
                    tmpPoz.Width = p.RefDO.Left - tmpPoz.Left + 92;

                    tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                    tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                    tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                    tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                    p.RefLinia1 = tmpPion;
                    p.RefLinia2 = tmpPoz;

                    panel1.Controls.Add(tmpPion);
                    panel1.Controls.Add(tmpPoz);
                    tmpPion = null;
                    tmpPoz = null;
                }
           
                if ((p.RefOD.Left + p.RefOD.Width / 2 > (p.RefDO.Left + p.RefDO.Width / 2)) && ((p.RefOD.Top + p.RefOD.Height) < p.RefDO.Top))
                {
                    LiniaPion tmpPion;
                    LiniaPoz tmpPoz;

                    tmpPion = new LiniaPion(strzalkaUpDown.none);
                    tmpPion.Top = p.RefOD.Top + p.RefOD.Height;
                    tmpPion.Left = p.RefOD.Left + p.RefOD.Width / 2;
                    tmpPion.Width = 4;
                    tmpPion.Height = p.RefDO.Top - tmpPion.Top;


                    tmpPoz = new LiniaPoz(strzalkaLeftRight.left);
                    tmpPoz.Top = tmpPion.Top + tmpPion.Height - 2;
                    tmpPoz.Left = p.RefDO.Left + (p.RefDO.Width / 2);
                    tmpPoz.Height = 4;
                    tmpPoz.Width = tmpPion.Left - tmpPoz.Left;

                    tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                    tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                    tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                    tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                    p.RefLinia1 = tmpPion;
                    p.RefLinia2 = tmpPoz;

                    panel1.Controls.Add(tmpPion);
                    panel1.Controls.Add(tmpPoz);
                    tmpPion = null;
                    tmpPoz = null;

                }
                if ((p.RefOD.Left + p.RefOD.Width / 2 > (p.RefDO.Left + p.RefDO.Width / 2)) && (p.RefOD.Top > p.RefDO.Top))
                {
                    LiniaPion tmpPion;
                    LiniaPoz tmpPoz;

                    tmpPion = new LiniaPion(strzalkaUpDown.none);
                    tmpPion.Top = p.RefDO.Top;
                    tmpPion.Left = p.RefOD.Left + p.RefOD.Width / 2;
                    tmpPion.Width = 4;
                    tmpPion.Height = p.RefOD.Top - tmpPion.Top + 4;
   

                    tmpPoz = new LiniaPoz(strzalkaLeftRight.left);
                    tmpPoz.Top = tmpPion.Top;
                    tmpPoz.Left = p.RefDO.Left + 92;
                    tmpPoz.Height = 4;
                    tmpPoz.Width = tmpPion.Left - tmpPoz.Left;

                    tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                    tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                    tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                    tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                    p.RefLinia1 = tmpPion;
                    p.RefLinia2 = tmpPoz;

                    panel1.Controls.Add(tmpPion);
                    panel1.Controls.Add(tmpPoz);

                    tmpPion = null;
                    tmpPoz = null;
                }
                
                if (((p.RefOD.Left + p.RefOD.Width / 2) < p.RefDO.Left + p.RefDO.Width / 2) && (p.RefOD.Top > p.RefDO.Top))
                {
                    LiniaPion tmpPion;
                    LiniaPoz tmpPoz;

                    tmpPion = new LiniaPion(strzalkaUpDown.none);
                    tmpPion.Top = p.RefDO.Top;// +p.RefDO.Height / 2;
                    tmpPion.Left = p.RefOD.Left + p.RefOD.Width / 2;
                    tmpPion.Width = 4;
                    tmpPion.Height = p.RefOD.Top - tmpPion.Top + 4;

                    tmpPoz = new LiniaPoz(strzalkaLeftRight.right);
                    tmpPoz.Top = tmpPion.Top;
                    tmpPoz.Left = tmpPion.Left;
                    tmpPoz.Height = 4;
                    tmpPoz.Width = p.RefDO.Left - tmpPoz.Left + 92;

                    tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                    tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                    tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                    tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                    p.RefLinia1 = tmpPion;
                    p.RefLinia2 = tmpPoz;

                    panel1.Controls.Add(tmpPion);
                    panel1.Controls.Add(tmpPoz);
                    tmpPion = null;
                    tmpPoz = null;
                }
                return;
            }
//////////////////////////////////////////////////
            

            if ((p.RefOD.GetType() != typeof(BlokDecyzyjny)) && (p.RefDO.GetType() != typeof(BlokDecyzyjny)))
            {
                if ((p.RefOD.Top + p.RefOD.Height) < p.RefDO.Top)
                {
                    if ((p.RefOD.Left <= (p.RefDO.Left + p.RefDO.Width)) && (p.RefOD.Left >= (p.RefDO.Left + ((p.RefDO.Width) / 2))))
                    {
                        double temp;
                        LiniaPion tmpLinia;

                        temp = ((p.RefOD.Left + p.RefDO.Left + p.RefDO.Width) / 2) - p.RefOD.Left;
                        p.RefOD.punkty[1].X = (Int32)temp;
                        p.RefOD.punkty[1].Y = p.RefOD.Height - 2;

                        p.RefDO.punkty[0].X = p.RefDO.Width - (Int32)temp;
                        p.RefDO.punkty[0].Y = 2;

                        tmpLinia = new LiniaPion(strzalkaUpDown.down);

                        tmpLinia.Top = p.RefOD.Top + p.RefOD.punkty[1].Y;
                        tmpLinia.Left = p.RefOD.Left + p.RefOD.punkty[1].X;

                        tmpLinia.Width = 3;
                        tmpLinia.Height = p.RefDO.Top - tmpLinia.Top;

                        tmpLinia.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpLinia.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);

                        p.RefLinia1 = tmpLinia;
                        p.RefLinia2 = null;

                        panel1.Controls.Add(tmpLinia);
                        tmpLinia = null;
                    }
                    
                    if ((p.RefDO.Left <= (p.RefOD.Left + p.RefOD.Width)) && (p.RefDO.Left >= (p.RefOD.Left + ((p.RefOD.Width) / 2))))
                    {
                        double temp;
                        LiniaPion tmpLinia;

                        temp = ((p.RefDO.Left + p.RefOD.Left + p.RefOD.Width) / 2) - p.RefDO.Left;
                        p.RefOD.punkty[1].X = p.RefDO.Width - (Int32)temp;
                        p.RefOD.punkty[1].Y = p.RefOD.Height - 2;

                        p.RefDO.punkty[0].X = (Int32)temp;
                        p.RefDO.punkty[0].Y = 2;

                        tmpLinia = new LiniaPion(strzalkaUpDown.down);

                        tmpLinia.Top = p.RefOD.Top + p.RefOD.punkty[1].Y;
                        tmpLinia.Left = p.RefDO.Left + p.RefDO.punkty[0].X;

                        tmpLinia.Width = 3;
                        tmpLinia.Height = p.RefDO.Top - tmpLinia.Top;

                        tmpLinia.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpLinia.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);

                        p.RefLinia1 = tmpLinia;
                        p.RefLinia2 = null;

                        panel1.Controls.Add(tmpLinia);
                        tmpLinia = null;
                    }
                    
                    if ((p.RefDO.Left <= (p.RefOD.Left + (p.RefOD.Width) / 2)) && (p.RefDO.Left >= p.RefOD.Left))
                    {
                        double temp;
                        LiniaPion tmpLinia;

                        temp = ((p.RefDO.Left + p.RefOD.Left + p.RefOD.Width) / 2) - p.RefDO.Left;
                        p.RefOD.punkty[1].X = p.RefDO.Width - (Int32)temp;
                        p.RefOD.punkty[1].Y = p.RefOD.Height - 2;

                        p.RefDO.punkty[0].X = (Int32)temp;
                        p.RefDO.punkty[0].Y = 2;

                        tmpLinia = new LiniaPion(strzalkaUpDown.down);

                        tmpLinia.Top = p.RefOD.Top + p.RefOD.punkty[1].Y;
                        tmpLinia.Left = p.RefDO.Left + p.RefDO.punkty[0].X;

                        tmpLinia.Width = 3;
                        tmpLinia.Height = p.RefDO.Top - tmpLinia.Top;

                        tmpLinia.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpLinia.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);

                        p.RefLinia1 = tmpLinia;
                        p.RefLinia2 = null;

                        panel1.Controls.Add(tmpLinia);
                        tmpLinia = null;
                    }

                    if ((p.RefOD.Left <= (p.RefDO.Left + (p.RefDO.Width) / 2)) && (p.RefOD.Left >= p.RefDO.Left))
                    {
                        double temp;
                        LiniaPion tmpLinia;

                        temp = ((p.RefOD.Left + p.RefDO.Left + p.RefDO.Width) / 2) - p.RefOD.Left;
                        p.RefOD.punkty[1].X = (Int32)temp;
                        p.RefOD.punkty[1].Y = p.RefOD.Height - 2;

                        p.RefDO.punkty[0].X = p.RefDO.Width - (Int32)temp;
                        p.RefDO.punkty[0].Y = 2;

                        tmpLinia = new LiniaPion(strzalkaUpDown.down);

                        tmpLinia.Top = p.RefOD.Top + p.RefOD.punkty[1].Y;
                        tmpLinia.Left = p.RefOD.Left + p.RefOD.punkty[1].X;

                        tmpLinia.Width = 3;
                        tmpLinia.Height = p.RefDO.Top - tmpLinia.Top;

                        tmpLinia.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpLinia.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);

                        p.RefLinia1 = tmpLinia;
                        p.RefLinia2 = null;
                        panel1.Controls.Add(tmpLinia);
                        tmpLinia = null;
                    }
                }
                else if (p.RefOD.Top > (p.RefDO.Top + p.RefDO.Height))
                {
                    if ((p.RefOD.Left <= (p.RefDO.Left + p.RefDO.Width)) && (p.RefOD.Left >= (p.RefDO.Left + ((p.RefDO.Width) / 2))))
                    {
                        double temp;
                        LiniaPion tmpLinia;

                        temp = ((p.RefOD.Left + p.RefDO.Left + p.RefDO.Width) / 2) - p.RefOD.Left;
                        p.RefOD.punkty[1].X = (Int32)temp;
                        p.RefOD.punkty[1].Y = 2;

                        p.RefDO.punkty[0].X = p.RefDO.Width - (Int32)temp;
                        p.RefDO.punkty[0].Y = p.RefDO.Height - 2;

                        tmpLinia = new LiniaPion(strzalkaUpDown.up);

                        tmpLinia.Top = p.RefDO.Top + p.RefDO.punkty[0].Y;
                        tmpLinia.Left = p.RefDO.Left + p.RefDO.punkty[0].X;

                        tmpLinia.Width = 3;
                        tmpLinia.Height = p.RefOD.Top - tmpLinia.Top;

                        tmpLinia.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpLinia.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);

                        p.RefLinia1 = tmpLinia;
                        p.RefLinia2 = null;

                        panel1.Controls.Add(tmpLinia);
                        tmpLinia = null;
                    }
                    
                    if ((p.RefDO.Left <= (p.RefOD.Left + p.RefOD.Width)) && (p.RefDO.Left >= (p.RefOD.Left + ((p.RefOD.Width) / 2))))
                    {
                        double temp;
                        LiniaPion tmpLinia;

                        temp = ((p.RefDO.Left + p.RefOD.Left + p.RefOD.Width) / 2) - p.RefDO.Left;
                        p.RefOD.punkty[1].X = p.RefDO.Width - (Int32)temp;
                        p.RefOD.punkty[1].Y = 2;

                        p.RefDO.punkty[0].X = (Int32)temp;
                        p.RefDO.punkty[0].Y = p.RefOD.Height - 2;

                        tmpLinia = new LiniaPion(strzalkaUpDown.up);

                        tmpLinia.Top = p.RefDO.Top + p.RefDO.punkty[0].Y;
                        tmpLinia.Left = p.RefDO.Left + p.RefDO.punkty[0].X;

                        tmpLinia.Width = 3;
                        tmpLinia.Height = p.RefOD.Top - tmpLinia.Top;

                        tmpLinia.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpLinia.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);

                        p.RefLinia1 = tmpLinia;
                        p.RefLinia2 = null;

                        panel1.Controls.Add(tmpLinia);
                        tmpLinia = null;
                    }
                    
                    if ((p.RefDO.Left <= (p.RefOD.Left + (p.RefOD.Width) / 2)) && (p.RefDO.Left >= p.RefOD.Left))
                    {
                        double temp;
                        LiniaPion tmpLinia;

                        temp = ((p.RefDO.Left + p.RefOD.Left + p.RefOD.Width) / 2) - p.RefDO.Left;
                        p.RefOD.punkty[1].X = p.RefDO.Width - (Int32)temp;
                        p.RefOD.punkty[1].Y = 2;

                        p.RefDO.punkty[0].X = (Int32)temp;
                        p.RefDO.punkty[0].Y = p.RefOD.Height - 2;

                        tmpLinia = new LiniaPion(strzalkaUpDown.up);

                        tmpLinia.Top = p.RefDO.Top + p.RefDO.punkty[0].Y;
                        tmpLinia.Left = p.RefDO.Left + p.RefDO.punkty[0].X;

                        tmpLinia.Width = 3;
                        tmpLinia.Height = p.RefOD.Top - tmpLinia.Top;

                        tmpLinia.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpLinia.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);

                        p.RefLinia1 = tmpLinia;
                        p.RefLinia2 = null;

                        panel1.Controls.Add(tmpLinia);
                        tmpLinia = null;
                    }
                    
                    if ((p.RefOD.Left <= (p.RefDO.Left + (p.RefDO.Width) / 2)) && (p.RefOD.Left >= p.RefDO.Left))
                    {
                        double temp;
                        LiniaPion tmpLinia;

                        temp = ((p.RefOD.Left + p.RefDO.Left + p.RefDO.Width) / 2) - p.RefOD.Left;
                        p.RefOD.punkty[1].X = (Int32)temp;
                        p.RefOD.punkty[1].Y = 2;

                        p.RefDO.punkty[0].X = p.RefDO.Width - (Int32)temp;
                        p.RefDO.punkty[0].Y = p.RefOD.Height - 2;

                        tmpLinia = new LiniaPion(strzalkaUpDown.up);

                        tmpLinia.Top = p.RefDO.Top + p.RefDO.punkty[0].Y;
                        tmpLinia.Left = p.RefDO.Left + p.RefDO.punkty[0].X;

                        tmpLinia.Width = 3;
                        tmpLinia.Height = p.RefOD.Top - tmpLinia.Top;

                        tmpLinia.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                        tmpLinia.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);

                        p.RefLinia1 = tmpLinia;
                        p.RefLinia2 = null;

                        panel1.Controls.Add(tmpLinia);
                        tmpLinia = null;
                    }
                }
                else if (((p.RefOD.Top + p.RefOD.Height) >= p.RefDO.Top) && (p.RefOD.Top <= (p.RefDO.Top + p.RefDO.Height)))
                {
                    if ((p.RefOD.Left + p.RefOD.Width) < p.RefDO.Left)
                    {
                        if (p.RefOD.Top <= p.RefDO.Top)
                        {
                            double temp;
                            LiniaPoz tmpLinia;

                            temp = ((p.RefOD.Top + p.RefDO.Top + p.RefDO.Height) / 2) - p.RefOD.Top;
                            p.RefOD.punkty[1].X = p.RefOD.Left + p.RefOD.Width;
                            p.RefOD.punkty[1].Y = (Int32)temp;

                            p.RefDO.punkty[0].X = 2;
                            p.RefDO.punkty[0].Y = p.RefDO.Height - (Int32)temp;

                            tmpLinia = new LiniaPoz(strzalkaLeftRight.right);

                            tmpLinia.Top = p.RefOD.Top + p.RefOD.punkty[1].Y;
                            tmpLinia.Left = p.RefOD.Left + p.RefOD.Width - 2;

                            tmpLinia.Width = p.RefDO.Left - tmpLinia.Left;
                            tmpLinia.Height = 3;

                            tmpLinia.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                            tmpLinia.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);

                            p.RefLinia1 = tmpLinia;
                            p.RefLinia2 = null;

                            panel1.Controls.Add(tmpLinia);
                            tmpLinia = null;
                        }
                        
                        if (p.RefOD.Top > p.RefDO.Top)
                        {
                            double temp;
                            LiniaPoz tmpLinia;

                            temp = ((p.RefDO.Top + p.RefOD.Top + p.RefOD.Height) / 2) - p.RefDO.Top;
                            p.RefOD.punkty[1].X = p.RefOD.Left + p.RefOD.Width;
                            p.RefOD.punkty[1].Y = p.RefOD.Height - (Int32)temp;

                            p.RefDO.punkty[0].X = 2;
                            p.RefDO.punkty[0].Y = (Int32)temp;

                            tmpLinia = new LiniaPoz(strzalkaLeftRight.right);

                            tmpLinia.Top = p.RefDO.Top + p.RefDO.punkty[0].Y;
                            tmpLinia.Left = p.RefOD.Left + p.RefOD.Width - 2;

                            tmpLinia.Width = p.RefDO.Left - tmpLinia.Left;
                            tmpLinia.Height = 3;

                            tmpLinia.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                            tmpLinia.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);

                            p.RefLinia1 = tmpLinia;
                            p.RefLinia2 = null;

                            panel1.Controls.Add(tmpLinia);
                            tmpLinia = null;
                        }
                    }
                    else if (p.RefOD.Left > (p.RefDO.Left + p.RefDO.Width))
                    {
                        if (p.RefOD.Top <= p.RefDO.Top)
                        {
                            double temp;
                            LiniaPoz tmpLinia;

                            temp = ((p.RefOD.Top + p.RefDO.Top + p.RefDO.Height) / 2) - p.RefOD.Top;
                            p.RefOD.punkty[1].X = 2;
                            p.RefOD.punkty[1].Y = (Int32)temp;

                            p.RefDO.punkty[0].X = p.RefDO.Left + p.RefDO.Width;
                            p.RefDO.punkty[0].Y = p.RefDO.Height - (Int32)temp;

                            tmpLinia = new LiniaPoz(strzalkaLeftRight.left);

                            tmpLinia.Top = p.RefOD.Top + p.RefOD.punkty[1].Y;
                            tmpLinia.Left = p.RefDO.Left + p.RefDO.Width - 2;

                            tmpLinia.Width = p.RefOD.Left - tmpLinia.Left;
                            tmpLinia.Height = 3;

                            tmpLinia.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                            tmpLinia.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);

                            p.RefLinia1 = tmpLinia;
                            p.RefLinia2 = null;

                            panel1.Controls.Add(tmpLinia);
                            tmpLinia = null;
                        }
                        
                        if (p.RefOD.Top > p.RefDO.Top)
                        {
                            double temp;
                            LiniaPoz tmpLinia;

                            temp = ((p.RefDO.Top + p.RefOD.Top + p.RefOD.Height) / 2) - p.RefDO.Top;
                            p.RefOD.punkty[1].X = 2;
                            p.RefOD.punkty[1].Y = p.RefOD.Height - (Int32)temp;

                            p.RefDO.punkty[0].X = p.RefDO.Left + p.RefDO.Width;
                            p.RefDO.punkty[0].Y = (Int32)temp;

                            tmpLinia = new LiniaPoz(strzalkaLeftRight.left);

                            tmpLinia.Top = p.RefDO.Top + p.RefDO.punkty[0].Y;
                            tmpLinia.Left = p.RefDO.Left + p.RefDO.Width - 2;

                            tmpLinia.Width = p.RefOD.Left - tmpLinia.Left;
                            tmpLinia.Height = 3;


                            tmpLinia.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                            tmpLinia.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);

                            p.RefLinia1 = tmpLinia;
                            p.RefLinia2 = null;

                            panel1.Controls.Add(tmpLinia);
                            tmpLinia = null;
                        }
                    }
                }
                
                if (((p.RefOD.Left + p.RefOD.Width) < p.RefDO.Left) && ((p.RefOD.Top + p.RefOD.Height) < p.RefDO.Top))
                {
                    LiniaPion tmpPion;
                    LiniaPoz tmpPoz;

                    tmpPion = new LiniaPion(strzalkaUpDown.none);
                    tmpPion.Top = p.RefOD.Top + p.RefOD.Height;
                    tmpPion.Left = p.RefOD.Left + p.RefOD.Width / 2;
                    tmpPion.Width = 4;
                    tmpPion.Height = (p.RefDO.Top + p.RefDO.Height / 2) - tmpPion.Top;

                    tmpPoz = new LiniaPoz(strzalkaLeftRight.right);
                    tmpPoz.Top = tmpPion.Top + tmpPion.Height - 2;
                    tmpPoz.Left = tmpPion.Left;
                    tmpPoz.Height = 4;
                    tmpPoz.Width = p.RefDO.Left - tmpPoz.Left;

                    tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                    tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                    tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                    tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                    p.RefLinia1 = tmpPion;
                    p.RefLinia2 = tmpPoz;

                    panel1.Controls.Add(tmpPion);
                    panel1.Controls.Add(tmpPoz);
                    tmpPion = null;
                    tmpPoz = null;
                }
                
                if ((p.RefOD.Left > (p.RefDO.Left + p.RefDO.Width)) && ((p.RefOD.Top + p.RefOD.Height) < p.RefDO.Top))
                {
                    LiniaPion tmpPion;
                    LiniaPoz tmpPoz;

                    tmpPion = new LiniaPion(strzalkaUpDown.none);
                    tmpPion.Top = p.RefOD.Top + p.RefOD.Height;
                    tmpPion.Left = p.RefOD.Left + p.RefOD.Width / 2;
                    tmpPion.Width = 4;
                    tmpPion.Height = (p.RefDO.Top + p.RefDO.Height / 2) - tmpPion.Top;

                    tmpPoz = new LiniaPoz(strzalkaLeftRight.left);
                    tmpPoz.Top = tmpPion.Top + tmpPion.Height - 2;
                    tmpPoz.Left = p.RefDO.Left + p.RefDO.Width;
                    tmpPoz.Height = 4;
                    tmpPoz.Width = tmpPion.Left - tmpPoz.Left;

                    tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                    tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                    tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                    tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                    p.RefLinia1 = tmpPion;
                    p.RefLinia2 = tmpPoz;

                    panel1.Controls.Add(tmpPion);
                    panel1.Controls.Add(tmpPoz);
                    tmpPion = null;
                    tmpPoz = null;
                }
                
                if ((p.RefOD.Left > (p.RefDO.Left + p.RefDO.Width)) && (p.RefOD.Top > (p.RefDO.Top + p.RefDO.Height)))
                {
                    LiniaPion tmpPion;
                    LiniaPoz tmpPoz;

                    tmpPion = new LiniaPion(strzalkaUpDown.none);
                    tmpPion.Top = p.RefDO.Top + p.RefDO.Height / 2;
                    tmpPion.Left = p.RefOD.Left + p.RefOD.Width / 2;
                    tmpPion.Width = 4;
                    tmpPion.Height = p.RefOD.Top - tmpPion.Top + 4;

                    tmpPoz = new LiniaPoz(strzalkaLeftRight.left);
                    tmpPoz.Top = tmpPion.Top;
                    tmpPoz.Left = p.RefDO.Left + p.RefDO.Width;
                    tmpPoz.Height = 4;
                    tmpPoz.Width = tmpPion.Left - tmpPoz.Left + 3;

                    tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                    tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                    tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                    tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                    p.RefLinia1 = tmpPion;
                    p.RefLinia2 = tmpPoz;

                    panel1.Controls.Add(tmpPion);
                    panel1.Controls.Add(tmpPoz);

                    tmpPion = null;
                    tmpPoz = null;
                }
                
                if (((p.RefOD.Left + p.RefOD.Width) < p.RefDO.Left) && (p.RefOD.Top > (p.RefDO.Top + p.RefDO.Height)))
                {
                    LiniaPion tmpPion;
                    LiniaPoz tmpPoz;

                    tmpPion = new LiniaPion(strzalkaUpDown.none);
                    tmpPion.Top = p.RefDO.Top + p.RefDO.Height / 2;
                    tmpPion.Left = p.RefOD.Left + p.RefOD.Width / 2;
                    tmpPion.Width = 4;
                    tmpPion.Height = p.RefOD.Top - tmpPion.Top + 4;

                    tmpPoz = new LiniaPoz(strzalkaLeftRight.right);
                    tmpPoz.Top = tmpPion.Top;
                    tmpPoz.Left = tmpPion.Left;
                    tmpPoz.Height = 4;
                    tmpPoz.Width = p.RefDO.Left - tmpPoz.Left + 3;

                    tmpPion.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                    tmpPoz.KeyDown += new KeyEventHandler(HandlerUsunPolaczenie);
                    tmpPion.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                    tmpPoz.MouseDown += new MouseEventHandler(ZaznaczPolaczenie);
                    p.RefLinia1 = tmpPion;
                    p.RefLinia2 = tmpPoz;

                    panel1.Controls.Add(tmpPion);
                    panel1.Controls.Add(tmpPoz);
                    tmpPion = null;
                    tmpPoz = null;
                }
            }
////////////////////////////////////
        }

        public void PrzesunStart(object sender, MouseEventArgs e)
        {
            WyczyscZaznaczenie();

            if (sender.GetType() == typeof(BlokSTART) ||
                sender.GetType() == typeof(BlokSTOP) ||
                sender.GetType() == typeof(BlokObliczeniowy) ||
                sender.GetType() == typeof(BlokDecyzyjny) ||
                sender.GetType() == typeof(BlokWeWy) ||
                sender.GetType() == typeof(Bloki))
            {
                zaznaczony = (Bloki)sender;
                if (symuluj == false)
                    zaznaczony.tryb = tryby.zaznaczony;

                if (polacz == true)
                {
                    if ((zaznaczony.GetType() == typeof(BlokDecyzyjny)) && (polaczOD == null))
                    {
                        if ((polaczOD == null) && (zaznaczony.GetType() != typeof(BlokSTOP)))
                        {
                            polaczOD = zaznaczony;

                            if (e.X < (zaznaczony.Width / 2))
                            {
                                blokDecTakNie = "NIE";
                            }
                            else
                            {
                                blokDecTakNie = "TAK";
                            }
                            return;
                        }
                    }
                    else
                    {
                        if ((polaczOD == null) && (zaznaczony.GetType() != typeof(BlokSTOP)))
                        {
                            polaczOD = zaznaczony;
                            return;
                        }
                        if ((polaczOD != null) && (polaczDO == null) && (zaznaczony.GetType() != typeof(BlokSTART)) && (polaczOD != polaczDO))
                        {
                            polaczDO = zaznaczony;

                            if (polaczOD.GetType() == typeof(BlokDecyzyjny))
                            {
                                switch (blokDecTakNie)
                                {
                                    case "TAK":
                                        {
                                            Polaczenie tmpPol = new Polaczenie(polaczOD, 1, polaczDO, 0, null, null);
                                            DodajPolaczenie(tmpPol);
                                            break;
                                        }
                                    case "NIE":
                                        {
                                            Polaczenie tmpPol = new Polaczenie(polaczOD, 0, polaczDO, 0, null, null);
                                            DodajPolaczenie(tmpPol);
                                            break;
                                        }
                                }
                                return;
                            }
                            else if ((polaczDO.nastepnyBlok[0] != polaczOD) || (polaczDO.GetType()==typeof(BlokDecyzyjny)))
                                {
                                    Polaczenie tmpPol = new Polaczenie(polaczOD, 0, polaczDO, 0, null, null);
                                    DodajPolaczenie(tmpPol);                                    
                                }
                        }
                    }
                    
                    polaczOD = null;
                    polaczDO = null;
                    polacz = false;
                    return;
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
            }
            else
            {
                polacz = false;

                polaczOD = null;
                polaczDO = null;
                WyczyscZaznaczenie();   
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
                if (punktKlikuNaBlok.X != e.X || punktKlikuNaBlok.Y != e.Y) //jeśli zmieniono położenie kursora
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
            if (przesun == true && zaznaczony != null)
            {
                if (punktKlikuNaBlok.X != e.X || punktKlikuNaBlok.Y != e.Y) //jeśli zmieniono położenie kursora
                {
                    zaznaczony.Left = pozK[0].Left;
                    zaznaczony.Top = pozK[0].Top;
                }
                List<Polaczenie> temp;
                Polaczenie p = new Polaczenie(null,0,zaznaczony,0,null,null);
                
                temp = ZnajdzPolaczenia(p);
                if (temp.Count > 0)
                {
                    RysujPolaczenia(p);
                }
                
                p.RefDO = null;
                p.RefOD = zaznaczony;

                temp = ZnajdzPolaczenia(p);
                if (temp.Count > 0)
                {
                    RysujPolaczenia(p);
                }

                p.RefDO = zaznaczony;
                p.RefOD = null;
                p.IndeksOD = 1;

                temp = ZnajdzPolaczenia(p);
                if (temp.Count > 0)
                {
                    RysujPolaczenia(p);
                }
                p.RefDO = null;
                p.RefOD = zaznaczony;
                p.IndeksOD = 1;

                temp = ZnajdzPolaczenia(p);
                if (temp.Count > 0)
                {
                    RysujPolaczenia(p);
                }

                zaznaczony.BringToFront();
                przesun = false;
                if (symuluj == false)
                    zaznaczony.tryb = tryby.normal;
                //zaznaczony = null;
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
            //okno = new Console();
            //okno.Show();
        }

        private void Połączenie_Click(object sender, EventArgs e)
        {
            polacz = true;
            polaczDO = null;
            polaczOD = null;
        }

        private void HandlerUsunPolaczenie(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                Polaczenie tmpPol = new Polaczenie(null, 0, null, 0, (Bloki)sender, null);
                List<Polaczenie> tmpList;
                tmpList = ZnajdzPolaczenia(tmpPol);
                
                if (tmpList.Count > 0)
                {
                    UsunPolaczenie(tmpList.First());
                }
            }
        }

        private void Symulacja(object sender, DoWorkEventArgs e)
        {
            if (tabBloki.Count == 0)
                return;

            aktualnyBlok = tabBloki[ZnajdzBlok("START")];
            aktualnyBlok.tryb = tryby.aktualny;
            bool dec = false;   //jesli false to wyszlo "NIE", jesli true to "TAK"
            
            while (aktualnyBlok.Name != "STOP" && symuluj == true)
            {
                if (symuluj == false)
                    e.Cancel = true;

                if (aktualnyBlok.typBloku == typeof(BlokObliczeniowy))
                {
                    ((BlokObliczeniowy)aktualnyBlok).Wykonaj();

                    if (aktualnyBlok.nastepnyBlok[0] == null)       //zatrzymaj jesli niema sciezki po ktorej masz isc
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                if (aktualnyBlok.typBloku == typeof(BlokWeWy))
                {
                    ((BlokWeWy)aktualnyBlok).Wykonaj();

                    if (aktualnyBlok.nastepnyBlok[0] == null)
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                if (aktualnyBlok.typBloku == typeof(BlokDecyzyjny))
                {
                    dec = ((BlokDecyzyjny)aktualnyBlok).Wykonaj();

                    if (dec == true)
                    {
                        if (aktualnyBlok.nastepnyBlok[1] == null)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                    else
                    {
                        if (aktualnyBlok.nastepnyBlok[0] == null)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }

                aktualnyBlok.tryb = tryby.normal;

                                                            //wybór następnego bloku
                if (aktualnyBlok.typBloku == typeof(BlokDecyzyjny))
                {
                    if (dec == true)
                        aktualnyBlok = aktualnyBlok.nastepnyBlok[1];
                    else
                        aktualnyBlok = aktualnyBlok.nastepnyBlok[0];
                }
                else
                    aktualnyBlok = aktualnyBlok.nastepnyBlok[0];

                aktualnyBlok.tryb = tryby.aktualny;
            }

            if (aktualnyBlok.Name == "STOP")
                symuluj = false;
        }

        private void PoSymulacji(object sender, RunWorkerCompletedEventArgs e)
        {
            for (int i = 0; i < tabBloki.Count; i++)
                tabBloki[i].tryb = tryby.normal;

            aktualnyBlok = null;
            zaznaczony = null;
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
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(PoSymulacji);

                bw.RunWorkerAsync();
            }
            else
            {
                symuluj = false;

                if (bw.IsBusy == true)
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
                pełnaToolStripMenuItem_Click(sender, e);

                symuluj = false;
                tsPracaKrokowa.Visible = false;

                if (aktualnyBlok != null)
                    aktualnyBlok.tryb = tryby.normal;

                aktualnyBlok = null;
            }
        }

        private void nastepny_Click(object sender, EventArgs e)
        {
            if (symuluj == true)
            {
                if(aktualnyBlok.GetType()==typeof(BlokDecyzyjny)) //BlokDecyzyjny nastepnyTAK, nastepnyNIE
                {
                    if (aktualnyBlok.nastepnyBlok[0] != null) 
                    {
                        aktualnyBlok.tryb = tryby.normal;
                        
                        aktualnyBlok = aktualnyBlok.nastepnyBlok[0];
                        aktualnyBlok.tryb = tryby.aktualny;
                    }
                }
                else
                {
                    if (aktualnyBlok.nastepnyBlok[0] != null)
                    {
                        aktualnyBlok.tryb = tryby.normal;

                        aktualnyBlok = aktualnyBlok.nastepnyBlok[0];
                        aktualnyBlok.tryb = tryby.aktualny;
                    }
                }
            }
        }
    }
}
