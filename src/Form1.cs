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
        private int ile = 0;
        //private IList<BlokObliczeniowy> tabBO =new List<BlokObliczeniowy>();
        private Graphics g;
        //BlokObliczeniowy temp_BO;
        private bool przesun = false;
        //private BlokSTART przenoszony;
        private Bloki przenoszony;
        //private PictureBox pbox;
        private Type typ;
        private NaroznikLD nld;
        private NaroznikLG nlg;
        private NaroznikPD npd;
        private NaroznikPG npg;
        private int polowaX;
        private int polowaY;
        private IList<Bloki> tabBloki = new List<Bloki>();
        private Point punktKlikuNaBlok;      //punkt w którym kliknięto na blok (przeciwdziałanie przesunięciu bloku bez przesuwania kursora)

        //private Point klikoffset;

        //private Rectangle r;


        public Form1()
        {
            InitializeComponent();
            g = panel1.CreateGraphics();
            
        }

        private int ZnajdzBlok(String nazwa)
        {
            int i;
            for (i = 0; i < ile + 1; i++)
                if (tabBloki[i].blok.Name.Equals(nazwa) == true) break;
                //if (tabBO[i].Name.Equals(nazwa) == true) break;

            return i;
        }

        private void toolStripBlokObliczeniowy_Click(object sender, EventArgs e)
        {
            typ = typeof(BlokObliczeniowy);
            klik = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Click(object sender, EventArgs e)
        {
            if (klik == true)
            {
                    BlokObliczeniowy temp = new BlokObliczeniowy();
                    temp.Left = ((MouseEventArgs)e).X;
                    temp.Top = ((MouseEventArgs)e).Y;
                    temp.Name = "BO_" + numer;
                    temp.KeyDown += new KeyEventHandler(UsunBO);

                    //tabBO.Add(temp);
                    //Form1.ActiveForm.Controls.Add(tabBO.Last());
                    
                    ile++;
                if (ctrl != true)
                {
                    klik = false;
                }
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void blokObliczeniowy1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void blokObliczeniowy1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                int temp = ZnajdzBlok(((BlokObliczeniowy)sender).Name);
                //tabBO[temp].Dispose();
            }
        }

        private void UsunBO(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                //tabBO.Remove((BlokObliczeniowy)sender);
                tabBloki.RemoveAt(ZnajdzBlok(((BlokObliczeniowy)sender).Name));
                ((BlokObliczeniowy)sender).Dispose();
                ile--;
            }
        }

        private void UsunBStart(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                tabBloki.RemoveAt(ZnajdzBlok(((BlokSTART)sender).Name));
                ((BlokSTART)sender).Dispose();
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

        private void flowLayoutPanel1_Click(object sender, EventArgs e)
        {
            if (klik == true)
            {
                    BlokObliczeniowy temp = new BlokObliczeniowy();
                    temp.Left = ((MouseEventArgs)e).X;
                    temp.Top = ((MouseEventArgs)e).Y;
                    temp.Name = "BO_" + numer;
                    temp.KeyDown += new KeyEventHandler(UsunBO);

                    //tabBO.Add(temp);
                    //flowLayoutPanel1.Controls.Add(tabBO.Last());
                    
                    ile++;
                if (ctrl != true)
                {
                    klik = false;
                }
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            if (klik == true)
            {
                if (typ == typeof(BlokObliczeniowy))
                {
                    BlokObliczeniowy temp = new BlokObliczeniowy();
                    temp.Left = ((MouseEventArgs)e).X;
                    temp.Top = ((MouseEventArgs)e).Y;
                    temp.Name = "BO_" + numer;
                    temp.KeyDown += new KeyEventHandler(UsunBO);
                    temp.MouseDown += new MouseEventHandler(PrzesunStart);
                    temp.MouseMove += new MouseEventHandler(panel1_MouseMove);
                    temp.MouseUp += new MouseEventHandler(panel1_MouseUp);
                    temp.ReDrawText();

                    numer++;
                    //tabBO.Add(temp);
                    //panel1.Controls.Add(tabBO.Last());

                    /*if (ile > 0)
                    {
                        tabBO[ile - 1].prNext = tabBO[ile].Name;
                        tabBO[ile - 1].prNext_ref = tabBO[ile];
                        tabBO[ile - 1].Update();



                        foreach (BlokObliczeniowy bo in panel1.Controls)
                        {
                            if (bo.prNext.Length > 0)
                            {
                                Pen p = new Pen(Color.Black, 1);
                                p.EndCap = LineCap.ArrowAnchor;

                                g.DrawLine(p, bo.Location.X + 75, bo.Location.Y + 75, bo.prNext_ref.Location.X + 75, bo.prNext_ref.Location.Y);
                            }
                        }
                    }
                    else
                    {
                        temp_BO = (BlokObliczeniowy)panel1.Controls[0];
                    }
                    */
                    Bloki temp2 = new Bloki();
                    temp2.typBloku = typeof(BlokObliczeniowy);
                    temp2.blok = (UserControl)temp;
                    tabBloki.Add(temp2);

                    panel1.Controls.Add(tabBloki.Last().blok);

                    ile++;
                    if (ctrl != true)
                    {
                        klik = false;
                    }
                    return;
                }
                if (typ == typeof(BlokSTART))
                {
                    BlokSTART temp = new BlokSTART();
                    temp.Left = ((MouseEventArgs)e).X;
                    temp.Top = ((MouseEventArgs)e).Y;
                    temp.Name = "START_" + numer;
                    temp.KeyDown += new KeyEventHandler(UsunBStart);
                    temp.MouseDown += new MouseEventHandler(PrzesunStart);
                    temp.MouseMove += new MouseEventHandler(panel1_MouseMove);
                    temp.MouseUp += new MouseEventHandler(panel1_MouseUp);
                    //temp.KeyDown += new KeyEventHandler(UsunBO);

                    //tabBO.Add(temp);
                    Bloki temp2 = new Bloki();
                    temp2.typBloku = typeof(BlokSTART);
                    temp2.blok = (UserControl)temp;
                    tabBloki.Add(temp2);

                    panel1.Controls.Add(tabBloki.Last().blok);

                    ile++;
                    if (ctrl != true)
                    {
                        klik = false;
                    }
                }
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
            //if (ile > 0)
            //{
            //    Form1.ActiveForm.Text = temp_BO.Name;
            //    /*if (temp_BO.prNext.Length > 0)
            //    {
            //        temp_BO = temp_BO.prNext_ref;
            //    } */  
            //}
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
                ////r = new Rectangle(e.X - 10, e.Y - 10, 20, 20);
                //pbox = new PictureBox();
                //pbox.Parent = panel1;

                //Color myColor = Color.FromArgb(1, 0, 0, 0);

                //pbox.BackColor = myColor;

                ////pbox.BackgroundImage = Image.FromFile("tlo1.gif",true);

                //pbox.Width = przenoszony.Width;
                //pbox.Height = przenoszony.Height;
                ////klikoffset.X = e.X + przenoszony.Left + (przenoszony.Width)/2;  //((Control)sender).Left;
                ////klikoffset.Y = e.Y + przenoszony.Top + (przenoszony.Height)/2;   //((Control)sender).Top;
                //pbox.Left = przenoszony.Left + (przenoszony.Width) / 2;
                //pbox.Top = przenoszony.Top + (przenoszony.Height) / 2;
                //pbox.MouseMove += new MouseEventHandler(panel1_MouseMove);
                //panel1.Controls.Add(pbox);  
              
                nlg = new NaroznikLG();
                nld = new NaroznikLD();
                npg = new NaroznikPG();
                npd = new NaroznikPD();
                nlg.Parent = panel1; nlg.BackColor = Color.White;
                nld.Parent = panel1; nld.BackColor = Color.White;
                npg.Parent = panel1; npg.BackColor = Color.White;
                npd.Parent = panel1; npd.BackColor = Color.White;

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
                //pbox.Left = e.X + przenoszony.Left - (przenoszony.Width) / 2;// +klikoffset.X;
                //pbox.Top = e.Y + przenoszony.Top - (przenoszony.Height) / 2;//+klikoffset.Y;
                //pbox.Refresh();
                ////panel1.Update();
                ////r.X = e.X - 10;
                ////r.Y = e.Y - 10;
                ////g.DrawRectangle(new Pen(Color.Black),r);
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

                    nld.Left = e.X + przenoszony.blok.Left - polowaX;
                    nld.Top = e.Y + przenoszony.blok.Top + (przenoszony.blok.Height - nld.Height) - polowaY;

                    npg.Left = e.X + przenoszony.blok.Left + (przenoszony.blok.Width - npd.Width) - polowaX;
                    npg.Top = e.Y + przenoszony.blok.Top - polowaY;

                    npd.Left = e.X + przenoszony.blok.Left + (przenoszony.blok.Width - npd.Width) - polowaX;
                    npd.Top = e.Y + przenoszony.blok.Top + (przenoszony.blok.Height - npd.Height) - polowaY;

                    nlg.Refresh();
                    nld.Refresh();
                    npg.Refresh();
                    npd.Refresh();
                }
            }
        }

        private void blokMove(object sender, MouseEventArgs e)
        {
            //if (przesun == true)
            //{
            //    pbox.Left = e.X - klikoffset.X;
            //    pbox.Top = e.Y - klikoffset.Y;
            //    //pbox.Refresh();
            //    //panel1.Update();
            //    //r.X = e.X - 10;
            //    //r.Y = e.Y - 10;
            //    //g.DrawRectangle(new Pen(Color.Black),r);
            //}
        }

        public void PrzesunStop(object sender, MouseEventArgs e)
        {
            
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {                         
            if (przesun == true)
            {
                //przenoszony.Left = pbox.Left;
                //przenoszony.Top = pbox.Top;
                //przesun = false;
                //przenoszony = null;
                //panel1.Controls.Remove(pbox);
                //panel1.Refresh();
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
            typ = typeof(BlokSTART);
            klik = true;
        }

        private void toolStripBlokDecyzyjny_Click(object sender, EventArgs e)
        {

        }

        //private void panel1_MouseDown(object sender, MouseEventArgs e)
        //{
        //    pbox = new PictureBox();
        //    pbox.BackColor = Color.Black;
        //    pbox.Left = e.X + ((Control)sender).Left - 10;
        //    pbox.Top = e.Y + ((Control)sender).Top -30;
        //    klikoffset.X = e.X + ((Control)sender).Left -10;
        //    klikoffset.Y = e.Y + ((Control)sender).Top -30;
        //    pbox.MouseMove += new MouseEventHandler(panel1_MouseMove);
        //    panel1.Controls.Add(pbox);
        //}
    }
}
