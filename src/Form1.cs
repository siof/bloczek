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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Okienka
{
    [Serializable]
    public partial class Form1 : Form
    {
        private static int numer = 0;

        private bool klik = false;
        private bool polacz = false;
        private bool ctrl = false;
        private bool przesun = false;
        private bool symuluj = false;
        private bool _zmodyfikowany = false;
        private String blokDecTakNie = "";
        private String nazwaPliku = "";

        private Bloki polaczOD = null, polaczDO = null;
        private int ile = 0;
        private int polowaX;
        private int polowaY;

        private BDOpcje bDOpcje;
        private BOOpcje bOOpcje;
        private BWeWyOpcje bWeWyOpcje;
        private Czytaj czytaj;
        private libbloki.Console console = new libbloki.Console();
        private Podglad_zmiennych podgladZmiennych;

        private IList<ParametryBloku> ParamBlokow = new List<ParametryBloku>();
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

        public bool zmodyfikowany
        {
            get { return _zmodyfikowany; }
            set { _zmodyfikowany = value; }
        }

        public Form1()
        {
            InitializeComponent();
            graph = panel1.CreateGraphics();
            numer = 0;
            podgladZmiennych = new Podglad_zmiennych(zmienne);
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
            zmodyfikowany = true;
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
            zmodyfikowany = true;
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
            zmodyfikowany = true;
        }

        private void WyczyscZaznaczenie()
        {
            if (zaznaczony != null)
            {
                if (zaznaczony.GetType() == typeof(LiniaPion) ||
                    zaznaczony.GetType() == typeof(LiniaPoz))
                {
                    Polaczenie tmpPol = new Polaczenie(null, 0,"", null, 0,"", zaznaczony, null);
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
                Polaczenie tmpPol = new Polaczenie((Bloki)sender, 0,"", null, 0,"", null, null);
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
            zmodyfikowany = true;
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
                    if (JestBlokONazwie("START")==true)
                        return;

                    BlokSTART temp = new BlokSTART();
                    
                    temp2 = (Bloki)temp;
                    temp2.typBloku = typeof(BlokSTART);
                    temp2.Name = "START";
                    temp2.nazwaBloku = "START";
                }

                if (typ == typeof(BlokSTOP))
                {
                    BlokSTOP temp = new BlokSTOP();

                    temp2 = (Bloki)temp;
                    temp2.typBloku = typeof(BlokSTOP);
                    temp2.Name = "STOP" + numer.ToString();
                    temp2.nazwaBloku = "STOP" + numer.ToString();
                }

                if (typ == typeof(BlokObliczeniowy))
                {
                    BlokObliczeniowy temp = new BlokObliczeniowy();
                    
                    temp2 = (Bloki)temp;
                    temp2.typBloku = typeof(BlokObliczeniowy);
                    temp2.listaZmiennych = zmienne;
                    temp2.MouseDoubleClick += new MouseEventHandler(WywolajBOOpcje);
                    temp2.Name = "BO" + numer.ToString();
                    temp2.nazwaBloku = "BO" + numer.ToString();
                }

                if (typ == typeof(BlokDecyzyjny))
                {
                    BlokDecyzyjny temp = new BlokDecyzyjny();
                    
                    temp2 = (Bloki)temp;
                    temp2.typBloku = typeof(BlokDecyzyjny);
                    temp2.listaZmiennych = zmienne;
                    temp2.MouseDoubleClick += new MouseEventHandler(WywolajBDOpcje);
                    temp2.Name = "BD" + numer.ToString();
                    temp2.nazwaBloku = "BD" + numer.ToString();
                }

                if (typ == typeof(BlokWeWy))
                {
                    BlokWeWy temp = new BlokWeWy(console);//
                    
                    temp2 = (Bloki)temp;
                    temp2.typBloku = typeof(BlokWeWy);
                    temp2.listaZmiennych = zmienne;
                    temp2.MouseDoubleClick += new MouseEventHandler(WywolajBWeWyOpcje);
                    temp2.Name = "BW" + numer.ToString();
                    temp2.nazwaBloku = "BW" + numer.ToString();
                }

                //globalne dla wszystkich bloków

                //if (temp2.Name == "")               //START i STOP mają własne nazwy - tylko raz mogą wystąpić w algorytmie
                //    temp2.Name = numer.ToString();      

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
                zmodyfikowany = true;
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

                Polaczenie tmpPol = new Polaczenie(p.RefOD,p.IndeksOD,p.nazwaOD,null,0,"",null,null);
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
            zmodyfikowany = true;
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
            zmodyfikowany = true;
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
            zmodyfikowany = true;
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
            Polaczenie tmpPol = new Polaczenie(null,0,"",null,0,"",(Bloki)sender,null);
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
            zmodyfikowany = true;
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
            zmodyfikowany = true;
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
                                            Polaczenie tmpPol = new Polaczenie(polaczOD, 1,polaczOD.nazwaBloku, polaczDO, 0,polaczDO.nazwaBloku, null, null);
                                            DodajPolaczenie(tmpPol);
                                            break;
                                        }
                                    case "NIE":
                                        {
                                            Polaczenie tmpPol = new Polaczenie(polaczOD, 0,polaczOD.nazwaBloku, polaczDO, 0,polaczDO.nazwaBloku, null, null);
                                            DodajPolaczenie(tmpPol);
                                            break;
                                        }
                                }
                                return;
                            }
                            else if ((polaczDO.nastepnyBlok[0] != polaczOD) || (polaczDO.GetType()==typeof(BlokDecyzyjny)))
                                {
                                    Polaczenie tmpPol = new Polaczenie(polaczOD, 0,polaczOD.nazwaBloku, polaczDO, 0,polaczDO.nazwaBloku, null, null);
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
                zmodyfikowany = true;
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
                Polaczenie p = new Polaczenie(null,0,"",zaznaczony,0,"",null,null);
                
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
                zmodyfikowany = true;
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
                Polaczenie tmpPol = new Polaczenie(null, 0,"", null, 0,"", (Bloki)sender, null);
                List<Polaczenie> tmpList;
                tmpList = ZnajdzPolaczenia(tmpPol);
                
                if (tmpList.Count > 0)
                {
                    UsunPolaczenie(tmpList.First());
                }
            }
            zmodyfikowany = true;
        }

        private void Symulacja(object sender, DoWorkEventArgs e)
        {
            if (tabBloki.Count == 0)
                return;

            aktualnyBlok = tabBloki[ZnajdzBlok("START")];
            aktualnyBlok.tryb = tryby.aktualny;
            bool dec = false;   //jesli false to wyszlo "NIE", jesli true to "TAK"
            
            while (aktualnyBlok.typBloku == typeof(BlokSTOP) && symuluj == true)
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

                    ((BlokWeWy)aktualnyBlok).Wykonaj(console);

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

            if (aktualnyBlok.typBloku == typeof(BlokSTOP))
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
                symuluj=true;

                if (console.IsDisposed == true)
                    console = new libbloki.Console();

                console.Show();
                console.richTextBox1.Clear();

                if (tabBloki.Count == 0)
                return;

                aktualnyBlok = tabBloki[ZnajdzBlok("START")];
                aktualnyBlok.tryb = tryby.aktualny;
                bool dec = false;   //jesli false to wyszlo "NIE", jesli true to "TAK"
                
                while (aktualnyBlok.typBloku != typeof(BlokSTOP) && symuluj == true)
                {
                    if (symuluj == false)
                    {
                        // e.Cancel = true;
                    }

                    if (aktualnyBlok.typBloku == typeof(BlokObliczeniowy))
                    {
                        ((BlokObliczeniowy)aktualnyBlok).Wykonaj();

                        if (aktualnyBlok.nastepnyBlok[0] == null)       //zatrzymaj jesli niema sciezki po ktorej masz isc
                        {
                           // e.Cancel = true;
                            return;
                        }
                    }

                    if (aktualnyBlok.typBloku == typeof(BlokWeWy))
                    {

                        ((BlokWeWy)aktualnyBlok).Wykonaj(console);

                        if (aktualnyBlok.nastepnyBlok[0] == null)
                        {
                           // e.Cancel = true;
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
                                //e.Cancel = true;
                                return;
                            }
                        }
                        else
                        {
                            if (aktualnyBlok.nastepnyBlok[0] == null)
                            {
                               // e.Cancel = true;
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

                if (aktualnyBlok.typBloku == typeof(BlokSTOP))
                    symuluj = false;

                for (int i = 0; i < tabBloki.Count; i++)
                    tabBloki[i].tryb = tryby.normal;

                aktualnyBlok = null;
                zaznaczony = null;
                /*
                symuluj = true;
                console.Show();
                console.richTextBox1.Clear();

                tsPracaKrokowa.Visible = false;

                bw.WorkerReportsProgress = true;
                bw.WorkerSupportsCancellation = true;
                bw.DoWork += new DoWorkEventHandler(Symulacja);
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(PoSymulacji);

                
                bw.RunWorkerAsync();
                 */
            }
            else
            {
                symuluj = false;

               // if (bw.IsBusy == true)
                   // bw.CancelAsync();
            }



            if (podgladZmiennych != null && podgladZmiennych.IsDisposed == false)
                podgladZmiennych.AktualizujListeObserwowanych();
        }

        private void krokowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabBloki.Count == 0)
                return;

            if (symuluj == false)
            {
                symuluj = true;

                if (console.IsDisposed == true)
                    console = new libbloki.Console();
               
                console.Show();
                console.richTextBox1.Clear();
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
                bool dec = false;

                if (console.IsDisposed == true)
                {
                    console = new libbloki.Console();
                    console.Show();
                }

                if (aktualnyBlok.typBloku == typeof(BlokSTOP))
                {
                    symuluj = false;
                    tsPracaKrokowa.Visible = false;

                    if (aktualnyBlok != null)
                        aktualnyBlok.tryb = tryby.normal;

                    aktualnyBlok = null;
                    return;
                }

                if (aktualnyBlok.typBloku == typeof(BlokObliczeniowy))
                {
                    ((BlokObliczeniowy)aktualnyBlok).Wykonaj();

                    if (aktualnyBlok.nastepnyBlok[0] == null)       //zatrzymaj jesli niema sciezki po ktorej masz isc
                        return;
                }

                if (aktualnyBlok.typBloku == typeof(BlokWeWy))
                {
                    ((BlokWeWy)aktualnyBlok).Wykonaj(console);

                    if (aktualnyBlok.nastepnyBlok[0] == null)
                        return;
                }

                if (aktualnyBlok.typBloku == typeof(BlokDecyzyjny))
                {
                    dec = ((BlokDecyzyjny)aktualnyBlok).Wykonaj();

                    if (dec == true)
                    {
                        if (aktualnyBlok.nastepnyBlok[1] == null)
                            return;
                    }
                    else
                    {
                        if (aktualnyBlok.nastepnyBlok[0] == null)
                            return;
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

                if (podgladZmiennych != null && podgladZmiennych.IsDisposed == false)
                    podgladZmiennych.AktualizujListeObserwowanych();
            }
        }

        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabBloki.Count > 0)
            {
                ParametryBloku pb;
                foreach (Bloki tmpBlok in tabBloki)
                {
                    pb = new ParametryBloku();
                    pb.x = tmpBlok.Left;
                    pb.y = tmpBlok.Top;
                    pb.typ = tmpBlok.GetType();
                    pb.Nazwa = tmpBlok.nazwaBloku;

                    foreach (Dzialanie tmpDzial in tmpBlok.dzialania)
                    {
                        pb.dzialania.Add(tmpDzial);
                    }

                    ParamBlokow.Add(pb);
                    pb = null;
                }
            
                FileStream plik = new FileStream("plik.xxx", FileMode.Create);

                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    bf.Serialize(plik, ParamBlokow.Count());//liczba blokow
                    foreach (ParametryBloku tmpParam in ParamBlokow)//bloki
                    {
                        bf.Serialize(plik, tmpParam);

                        bf.Serialize(plik, tmpParam.dzialania.Count());
                        foreach (Dzialanie tmpDzial in tmpParam.dzialania)//dzialnia danego bloku
                        {
                            bf.Serialize(plik, tmpDzial);
                        }
                    }
                    
                    bf.Serialize(plik, zmienne.Count());//liczba zmiennych
                    foreach (Zmienna tmpZmienna in zmienne)//zmienne
                    {
                        bf.Serialize(plik, tmpZmienna);
                    }


                    bf.Serialize(plik, Polaczenia.Count());//liczba polaczen
                    foreach (Polaczenie tmpPolaczenie in Polaczenia)
                    {
                        bf.Serialize(plik, tmpPolaczenie);
                    }

                }
                catch (SerializationException exc)
                {
                    MessageBox.Show("Nieudana serializacja: " + exc.Message);
                    throw;
                }
                finally
                {
                    ParamBlokow.Clear();
                    plik.Close();
                }
            }


        }

        private void odczytajToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tabBloki.Clear();
            this.zmienne.Clear();
            this.Polaczenia.Clear();
            panel1.Controls.Clear();
            panel1.Refresh();
            Bloki temp = new Bloki();
            int ilosc=0,iloscDzialan=0;

            ile = 0;
            FileStream plik = new FileStream("plik.xxx", FileMode.Open);

            BinaryFormatter bf = new BinaryFormatter();
            try
            {

                ilosc = (Int32) bf.Deserialize(plik);//ile blokow
                ParametryBloku tempPB;
                for (int i = 0; i < ilosc; i++)  //wczytanie/dodanie blokow
                {
                    tempPB = (ParametryBloku)bf.Deserialize(plik);

                    if (tempPB.typ == typeof(BlokSTART))
                    {
                        if (JestBlokONazwie("START"))
                            return;

                        temp = new BlokSTART();
                        temp.typBloku = typeof(BlokSTART);
                        temp.Name = "START";
                    }

                    if (tempPB.typ == typeof(BlokSTOP))
                    {
                        temp = new BlokSTOP();
                        temp.typBloku = typeof(BlokSTOP);
                        temp.Name = tempPB.Nazwa.ToString();
                        temp.nazwaBloku = tempPB.Nazwa.ToString();
                    }

                    if (tempPB.typ == typeof(BlokObliczeniowy))
                    {
                        temp = new BlokObliczeniowy();
                        temp.typBloku = typeof(BlokObliczeniowy);
                        temp.listaZmiennych = zmienne;
                        temp.Name = tempPB.Nazwa.ToString();
                        temp.nazwaBloku = tempPB.Nazwa.ToString();
                        temp.MouseDoubleClick += new MouseEventHandler(WywolajBOOpcje);
                    }

                    if (tempPB.typ == typeof(BlokDecyzyjny))
                    {
                        temp = new BlokDecyzyjny();
                        temp.typBloku = typeof(BlokDecyzyjny);
                        temp.listaZmiennych = zmienne;
                        temp.Name = tempPB.Nazwa.ToString();
                        temp.nazwaBloku = tempPB.Nazwa.ToString();
                        temp.MouseDoubleClick += new MouseEventHandler(WywolajBDOpcje);
                    }

                    if (tempPB.typ == typeof(BlokWeWy))
                    {
                        temp = new BlokWeWy(console);//
                        temp.typBloku = typeof(BlokWeWy);
                        temp.listaZmiennych = zmienne;
                        temp.Name = tempPB.Nazwa.ToString();
                        temp.nazwaBloku = tempPB.Nazwa.ToString(); ;
                        temp.MouseDoubleClick += new MouseEventHandler(WywolajBWeWyOpcje);
                    }

                    //globalne dla wszystkich bloków

                    temp.Left = temp.Left = tempPB.x;
                    temp.Top = tempPB.y;
                    temp.KeyDown += new KeyEventHandler(UsunBlok);
                    temp.MouseDown += new MouseEventHandler(PrzesunStart);
                    temp.MouseMove += new MouseEventHandler(panel1_MouseMove);
                    temp.MouseUp += new MouseEventHandler(panel1_MouseUp);

                    iloscDzialan = (Int32)bf.Deserialize(plik);
                    for (int j = 0; j < iloscDzialan; j++)
                    {
                        temp.dzialania.Add((Dzialanie)bf.Deserialize(plik));
                    }

                    tabBloki.Add(temp);
                    panel1.Controls.Add(tabBloki.Last());
                    tempPB = null;
                    temp = null;
                    ile++;
                }

                ilosc = (Int32) bf.Deserialize(plik);//ile zmiennych
                for (int i = 0; i < ilosc; i++)//wczytanie zmiennych
                {
                    zmienne.Add((Zmienna)bf.Deserialize(plik));
                }

                ilosc = (Int32)bf.Deserialize(plik);//ile polaczen
                if (ilosc > 0)
                {
                    IList<Polaczenie> tmpPolaczenia = new List<Polaczenie>();
                    for (int i = 0; i < ilosc; i++)//wczytanie polaczen
                    {
                        tmpPolaczenia.Add((Polaczenie)bf.Deserialize(plik));
                    }

                    //wypelnienie referencji w polaczeniach  refOD i refDO

                    foreach (Polaczenie tmpPolaczenie in tmpPolaczenia)
                    {
                        tmpPolaczenie.RefOD=tabBloki[ZnajdzBlok(tmpPolaczenie.nazwaOD)];
                        tmpPolaczenie.RefDO = tabBloki[ZnajdzBlok(tmpPolaczenie.nazwaDO)];
                        DodajPolaczenie(tmpPolaczenie);
                    }

                    foreach (Polaczenie tmpPolaczenie in Polaczenia)
                    {
                        RysujPolaczenie(tmpPolaczenie);
                    }
                }
            }
            catch (SerializationException exc)
            {
                MessageBox.Show("Nieudana deserializacja: " + exc.Message);
                throw;
            }
            finally
            {
                plik.Close();
            }
        }

        private void OtworzSchematPlik(String plik)
        {
            if (zmodyfikowany == true)
            {
                MessageBoxButtons msgb = MessageBoxButtons.YesNoCancel;
                DialogResult dr = MessageBox.Show("Czy zapisać zmiany?", "Czy zapisać zmiany?", msgb);
                
                if (dr == DialogResult.Cancel)
                {
                    return;
                }

                if (dr == DialogResult.Yes)
                {
                    if (nazwaPliku != "")
                    {
                        ZapiszSchematPlik(nazwaPliku);
                    }
                    else
                    {
                        saveAsToolStripMenuItem_Click(null, null);
                    }
                }
            }

            this.tabBloki.Clear();
            this.zmienne.Clear();
            this.Polaczenia.Clear();
            panel1.Controls.Clear();
            panel1.Refresh();
            Bloki temp = new Bloki();
            int ilosc = 0, iloscDzialan = 0;

            ile = 0;
            FileStream fs = new FileStream(plik, FileMode.Open);

            BinaryFormatter bf = new BinaryFormatter();
            try
            {

                ilosc = (Int32)bf.Deserialize(fs);//ile blokow
                ParametryBloku tempPB;
                for (int i = 0; i < ilosc; i++)  //wczytanie/dodanie blokow
                {
                    tempPB = (ParametryBloku)bf.Deserialize(fs);

                    if (tempPB.typ == typeof(BlokSTART))
                    {
                        if (JestBlokONazwie("START"))
                            return;

                        temp = new BlokSTART();
                        temp.typBloku = typeof(BlokSTART);
                        temp.Name = "START";
                        temp.nazwaBloku = "START";
                    }

                    if (tempPB.typ == typeof(BlokSTOP))
                    {
                        temp = new BlokSTOP();
                        temp.typBloku = typeof(BlokSTOP);
                        temp.Name = tempPB.Nazwa.ToString();
                        temp.nazwaBloku = tempPB.Nazwa.ToString();
                    }

                    if (tempPB.typ == typeof(BlokObliczeniowy))
                    {
                        temp = new BlokObliczeniowy();
                        temp.typBloku = typeof(BlokObliczeniowy);
                        temp.listaZmiennych = zmienne;
                        temp.Name = tempPB.Nazwa.ToString();
                        temp.nazwaBloku = tempPB.Nazwa.ToString();
                        temp.MouseDoubleClick += new MouseEventHandler(WywolajBOOpcje);
                    }

                    if (tempPB.typ == typeof(BlokDecyzyjny))
                    {
                        temp = new BlokDecyzyjny();
                        temp.typBloku = typeof(BlokDecyzyjny);
                        temp.listaZmiennych = zmienne;
                        temp.Name = tempPB.Nazwa.ToString();
                        temp.nazwaBloku = tempPB.Nazwa.ToString();
                        temp.MouseDoubleClick += new MouseEventHandler(WywolajBDOpcje);
                    }

                    if (tempPB.typ == typeof(BlokWeWy))
                    {
                        temp = new BlokWeWy(console);//
                        temp.typBloku = typeof(BlokWeWy);
                        temp.listaZmiennych = zmienne;
                        temp.Name = tempPB.Nazwa.ToString();
                        temp.nazwaBloku = tempPB.Nazwa.ToString(); ;
                        temp.MouseDoubleClick += new MouseEventHandler(WywolajBWeWyOpcje);
                    }

                    //globalne dla wszystkich bloków

                    temp.Left = temp.Left = tempPB.x;
                    temp.Top = tempPB.y;
                    temp.KeyDown += new KeyEventHandler(UsunBlok);
                    temp.MouseDown += new MouseEventHandler(PrzesunStart);
                    temp.MouseMove += new MouseEventHandler(panel1_MouseMove);
                    temp.MouseUp += new MouseEventHandler(panel1_MouseUp);

                    iloscDzialan = (Int32)bf.Deserialize(fs);
                    for (int j = 0; j < iloscDzialan; j++)
                    {
                        temp.dzialania.Add((Dzialanie)bf.Deserialize(fs));
                    }

                    tabBloki.Add(temp);
                    panel1.Controls.Add(tabBloki.Last());
                    tempPB = null;
                    temp = null;
                    ile++;
                }

                ilosc = (Int32)bf.Deserialize(fs);//ile zmiennych
                for (int i = 0; i < ilosc; i++)//wczytanie zmiennych
                {
                    zmienne.Add((Zmienna)bf.Deserialize(fs));
                }

                ilosc = (Int32)bf.Deserialize(fs);//ile polaczen
                if (ilosc > 0)
                {
                    IList<Polaczenie> tmpPolaczenia = new List<Polaczenie>();
                    for (int i = 0; i < ilosc; i++)//wczytanie polaczen
                    {
                        tmpPolaczenia.Add((Polaczenie)bf.Deserialize(fs));
                    }

                    //wypelnienie referencji w polaczeniach  refOD i refDO

                    foreach (Polaczenie tmpPolaczenie in tmpPolaczenia)
                    {
                        if((JestBlokONazwie(tmpPolaczenie.nazwaOD)==true) &&(JestBlokONazwie(tmpPolaczenie.nazwaDO)==true)) 
                        {
                            tmpPolaczenie.RefOD = tabBloki[ZnajdzBlok(tmpPolaczenie.nazwaOD)];
                            tmpPolaczenie.RefDO = tabBloki[ZnajdzBlok(tmpPolaczenie.nazwaDO)];
                            DodajPolaczenie(tmpPolaczenie);
                        }
                        
                    }

                    foreach (Polaczenie tmpPolaczenie in Polaczenia)
                    {
                        RysujPolaczenie(tmpPolaczenie);
                    }
                }
                zmodyfikowany = false;
            }
            catch (SerializationException exc)
            {
                MessageBox.Show("Nieudana deserializacja: " + exc.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        private void ZapiszSchematPlik(String plik)
        {
            if (tabBloki.Count > 0)
            {
                ParametryBloku pb;
                foreach (Bloki tmpBlok in tabBloki)
                {
                    pb = new ParametryBloku();
                    pb.x = tmpBlok.Left;
                    pb.y = tmpBlok.Top;
                    pb.typ = tmpBlok.GetType();
                    pb.Nazwa = tmpBlok.nazwaBloku;

                    foreach (Dzialanie tmpDzial in tmpBlok.dzialania)
                    {
                        pb.dzialania.Add(tmpDzial);
                    }

                    ParamBlokow.Add(pb);
                    pb = null;
                }

                FileStream fs = new FileStream(plik, FileMode.Create);

                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    bf.Serialize(fs, ParamBlokow.Count());//liczba blokow
                    foreach (ParametryBloku tmpParam in ParamBlokow)//bloki
                    {
                        bf.Serialize(fs, tmpParam);

                        bf.Serialize(fs, tmpParam.dzialania.Count());
                        foreach (Dzialanie tmpDzial in tmpParam.dzialania)//dzialnia danego bloku
                        {
                            bf.Serialize(fs, tmpDzial);
                        }
                    }

                    bf.Serialize(fs, zmienne.Count());//liczba zmiennych
                    foreach (Zmienna tmpZmienna in zmienne)//zmienne
                    {
                        bf.Serialize(fs, tmpZmienna);
                    }


                    bf.Serialize(fs, Polaczenia.Count());//liczba polaczen
                    foreach (Polaczenie tmpPolaczenie in Polaczenia)
                    {
                        bf.Serialize(fs, tmpPolaczenie);
                    }

                    zmodyfikowany = false;
                }
                catch (SerializationException exc)
                {
                    MessageBox.Show("Nieudana serializacja: " + exc.Message);
                    throw;
                }
                finally
                {
                    ParamBlokow.Clear();
                    fs.Close();
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                OtworzSchematPlik(openFileDialog1.FileName);
                nazwaPliku = openFileDialog1.FileName.ToString();
                saveToolStripMenuItem.Enabled = true;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nazwaPliku != "")
            {
                ZapiszSchematPlik(nazwaPliku);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (zmodyfikowany == true)
            {
                MessageBoxButtons msgb = MessageBoxButtons.YesNoCancel;
                DialogResult dr = MessageBox.Show("Czy zapisać zmiany?", "Czy zapisać zmiany?", msgb);

                if (dr == DialogResult.Cancel)
                {
                    return;
                }

                if (dr == DialogResult.Yes)
                {
                    if (nazwaPliku != "")
                    {
                        ZapiszSchematPlik(nazwaPliku);
                    }
                    else
                    {
                        saveAsToolStripMenuItem_Click(null, null);
                    }
                }
            }
            nazwaPliku = "";
            saveToolStripMenuItem.Enabled = false;
            tabBloki.Clear();
            Polaczenia.Clear();
            zmienne.Clear();
            panel1.Controls.Clear();
            panel1.Refresh();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                nazwaPliku = saveFileDialog1.FileName.ToString();
                ZapiszSchematPlik(saveFileDialog1.FileName);
                saveToolStripMenuItem.Enabled = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (zmodyfikowany == true)
            {
                MessageBoxButtons msgb = MessageBoxButtons.YesNoCancel;
                DialogResult dr = MessageBox.Show("Czy zapisać zmiany?", "Czy zapisać zmiany?", msgb);

                if (dr == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }

                if (dr == DialogResult.Yes)
                {
                    if (nazwaPliku != "")
                    {
                        ZapiszSchematPlik(nazwaPliku);
                    }
                    else
                    {
                        saveAsToolStripMenuItem_Click(null, null);
                    }
                }
            }
            //Application.Exit();
        }

        private void podgladZmiennychToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (podgladZmiennych.IsDisposed == true)
                podgladZmiennych = new Podglad_zmiennych(zmienne);
            podgladZmiennych.WczytajZmienne();
            podgladZmiennych.Show();
        }
    }
}
