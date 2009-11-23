using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace libbloki
{
    public partial class BlokSTART : UserControl
    {
        Graphics graph;
        //private Point klikoffset;
        //private bool klik;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        public BlokSTART()
        {
            InitializeComponent();

            graph = CreateGraphics();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Font fnt = new Font("Verdana", 16);
            Graphics g = pe.Graphics;
            Pen pn = new Pen(Color.Brown, 2);
            Rectangle rect = new Rectangle(1, 1, 150, 75);
            g.DrawEllipse(pn, rect);
            g.FillEllipse(new SolidBrush(Color.Wheat), rect);
            g.DrawString("START", fnt, new SolidBrush(Color.Black), 40, 25);
        }

        //private void BlokSTART_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (klik)
        //    {
        //        this.Left = e.X + this.Left - klikoffset.X;
        //        this.Top = e.Y + this.Top - klikoffset.Y;
        //    }
        //}

        //private void BlokSTART_MouseUp(object sender, MouseEventArgs e)
        //{
        //    klik = false;
        //}

        //private void BlokSTART_MouseDown(object sender, MouseEventArgs e)
        //{
        //    klik = true;
        //    klikoffset.X = e.X;
        //    klikoffset.Y = e.Y;
        //}

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // przezroczyste tlo
        }


        protected override void OnMove(EventArgs e)
        {
            //RecreateHandle();
            //this.OnPaint(this);
             Rectangle rect = new Rectangle(1, 1, 150, 75);
             PaintEventArgs pe = new PaintEventArgs(graph, rect);
             this.OnPaint(pe);
        }
    }
}
