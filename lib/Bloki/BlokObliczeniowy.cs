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
    public partial class BlokObliczeniowy : UserControl
    {
        private BFopcje frmOpcje;
        Graphics graph = null;
        private String[] lval = new String[5];
        private int[] rval_a = new int[5];
        private int[] rval_b = new int[5];
        private String[] dzial = new String[5];

        private Point klikoffset;
        private bool klik;

        /*public BlokObliczeniowy prNext_ref
        {
            get { return next_ref; }
            set { next_ref = value; }
        }

        public String prNext
        {
            get { return next; }
            set { next = value; }
        }*/

        public String[] prlval
        {
            get { return lval;}
            set { lval = value;}
        }
        public int[] prrval_a
        {
            get { return rval_a; }
            set { rval_a = value; }
        }
        public int[] prrval_b
        {
            get { return rval_b; }
            set { rval_b = value; }
        }
        public String[] prdzial
        {
            get { return dzial; }
            set { dzial = value; }
        }
        public BlokObliczeniowy()
        {
            InitializeComponent();
            for (int i = 0; i < 5; i++)
            {
                prlval[i] = "";
                prdzial[i] = "+";
            }
            /*for (int i = 0; i < 2; i++)
                for (int j = 0; j < 5; j++)
                    prrval[i,j] = new Int32();*/
        }

        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {

        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            Font fnt = new Font("Verdana", 16);
            Graphics g = pe.Graphics;
            Pen pn = new Pen(Color.Brown,2);
            Rectangle rect = new Rectangle(1, 1, 150, 75);
            g.DrawRectangle(pn, rect); 
            g.FillRectangle(new SolidBrush(Color.Wheat),rect);
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {

        }

        private void UserControl1_Click(object sender, EventArgs e)
        {

        }

        private void UserControl1_Resize(object sender, EventArgs e)
        {

        }

        private void UserControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            frmOpcje = new BFopcje(this);
            frmOpcje.ShowDialog(this);

            Pen pn = new Pen(Color.Brown, 2);
            Rectangle rect = new Rectangle(1, 1, 150, 75);
            graph.DrawRectangle(pn, rect);
            graph.FillRectangle(new SolidBrush(Color.Wheat), rect);
            Font fnt = new Font("Verdana", 8);
            
            int i = 2;
            if (prlval[0].ToString().Length > 0)
            {

                graph.DrawString(prlval[0] + " = " + prrval_a[0].ToString() + " " + prdzial[0] + " " + prrval_b[0].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
            if (prlval[1].Length > 0)
            {
                graph.DrawString(prlval[1] + " = " + prrval_a[1].ToString() + " " + prdzial[1] + " " + prrval_b[1].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
            if (prlval[2].Length > 0)
            {
                graph.DrawString(prlval[2] + " = " + prrval_a[2].ToString() + " " + prdzial[2] + " " + prrval_b[2].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
            if (prlval[3].Length > 0)
            {
                graph.DrawString(prlval[3] + " = " + prrval_a[3].ToString() + " " + prdzial[3] + " " + prrval_b[3].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
            if (prlval[4].Length > 0)
            {
                graph.DrawString(prlval[4] + " = " + prrval_a[4].ToString() + " " + prdzial[4] + " " + prrval_a[4].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
        }
        /*
        private void BlokObliczeniowy_MouseDown(object sender, MouseEventArgs e)
        {
            klik = true;
            klikoffset.X = e.X;
            klikoffset.Y = e.Y;
        }
        
        private void BlokObliczeniowy_MouseUp(object sender, MouseEventArgs e)
        {
            klik = false;

            Pen pn = new Pen(Color.Brown, 2);
            Rectangle rect = new Rectangle(1, 1, 150, 75);
            graph.DrawRectangle(pn, rect);
            graph.FillRectangle(new SolidBrush(Color.Wheat), rect);
            Font fnt = new Font("Verdana", 8);

            int i = 2;
            if (prlval[0].ToString().Length > 0)
            {

                graph.DrawString(prlval[0] + " = " + prrval_a[0].ToString() + " " + prdzial[0] + " " + prrval_b[0].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
            if (prlval[1].Length > 0)
            {
                graph.DrawString(prlval[1] + " = " + prrval_a[1].ToString() + " " + prdzial[1] + " " + prrval_b[1].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
            if (prlval[2].Length > 0)
            {
                graph.DrawString(prlval[2] + " = " + prrval_a[2].ToString() + " " + prdzial[2] + " " + prrval_b[2].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
            if (prlval[3].Length > 0)
            {
                graph.DrawString(prlval[3] + " = " + prrval_a[3].ToString() + " " + prdzial[3] + " " + prrval_b[3].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
            if (prlval[4].Length > 0)
            {
                graph.DrawString(prlval[4] + " = " + prrval_a[4].ToString() + " " + prdzial[4] + " " + prrval_a[4].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
        }

        private void BlokObliczeniowy_MouseMove(object sender, MouseEventArgs e)
        {
            if (klik)
            {
                this.Left = e.X + this.Left - klikoffset.X;
                this.Top = e.Y + this.Top - klikoffset.Y;
            }
        }
        
        private void BlokObliczeniowy_Move(object sender, EventArgs e)
        {
            Pen pn = new Pen(Color.Brown, 2);
            Rectangle rect = new Rectangle(1, 1, 150, 75);
            graph.DrawRectangle(pn, rect);
            graph.FillRectangle(new SolidBrush(Color.Wheat), rect);
            Font fnt = new Font("Verdana", 8);

            int i = 2;
            if (prlval[0].ToString().Length > 0)
            {

                graph.DrawString(prlval[0] + " = " + prrval_a[0].ToString() + " " + prdzial[0] + " " + prrval_b[0].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
            if (prlval[1].Length > 0)
            {
                graph.DrawString(prlval[1] + " = " + prrval_a[1].ToString() + " " + prdzial[1] + " " + prrval_b[1].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
            if (prlval[2].Length > 0)
            {
                graph.DrawString(prlval[2] + " = " + prrval_a[2].ToString() + " " + prdzial[2] + " " + prrval_b[2].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
            if (prlval[3].Length > 0)
            {
                graph.DrawString(prlval[3] + " = " + prrval_a[3].ToString() + " " + prdzial[3] + " " + prrval_b[3].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
            if (prlval[4].Length > 0)
            {
                graph.DrawString(prlval[4] + " = " + prrval_a[4].ToString() + " " + prdzial[4] + " " + prrval_a[4].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
        }*/
        public void ReDrawText()
        {
            if (graph != null)
                graph.Dispose();
            graph = CreateGraphics();
            Pen pn = new Pen(Color.Brown, 2);
            Rectangle rect = new Rectangle(1, 1, 150, 75);
            graph.DrawRectangle(pn, rect);
            graph.FillRectangle(new SolidBrush(Color.Wheat), rect);
            Font fnt = new Font("Verdana", 8);

            int i = 2;
            if (prlval[0].ToString().Length > 0)
            {

                graph.DrawString(prlval[0] + " = " + prrval_a[0].ToString() + " " + prdzial[0] + " " + prrval_b[0].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
            if (prlval[1].Length > 0)
            {
                graph.DrawString(prlval[1] + " = " + prrval_a[1].ToString() + " " + prdzial[1] + " " + prrval_b[1].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
            if (prlval[2].Length > 0)
            {
                graph.DrawString(prlval[2] + " = " + prrval_a[2].ToString() + " " + prdzial[2] + " " + prrval_b[2].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
            if (prlval[3].Length > 0)
            {
                graph.DrawString(prlval[3] + " = " + prrval_a[3].ToString() + " " + prdzial[3] + " " + prrval_b[3].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
            if (prlval[4].Length > 0)
            {
                graph.DrawString(prlval[4] + " = " + prrval_a[4].ToString() + " " + prdzial[4] + " " + prrval_a[4].ToString(), fnt, new SolidBrush(Color.Black), 20, i);
                i += 15;
            }
        }
    }
}
