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
    
    
    public partial class BlokSTART : Bloki
    {
        
        //private tryby _tryb;

        //public tryby tryb
        //{
        //    get { return _tryb; }
        //    set
        //    {
        //        _tryb = value;
        //        Rectangle rect = new Rectangle(1, 1, 150, 75);
        //        PaintEventArgs pe = new PaintEventArgs(graph, rect);
        //        this.OnPaint(pe);
        //    }
        //}

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
            tryb = tryby.normal;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Font fnt = new Font("Verdana", 16);
            Graphics g = pe.Graphics;
            SolidBrush brush = new SolidBrush(Color.Black);
            switch (tryb)
            {
                case tryby.normal: brush.Color = Color.Wheat ; break;
                case tryby.zaznaczony: brush.Color = Color.Orange; break;
                case tryby.aktualny: brush.Color = Color.Red; break;
            }

            Pen pn = new Pen(Color.Brown, 2);
            Rectangle rect = new Rectangle(1, 1, 150, 75);
            g.DrawEllipse(pn, rect);
            g.FillEllipse(brush, rect);
            g.DrawString("START", fnt, new SolidBrush(Color.Black), 40, 25);

            pn.Dispose();
            brush.Dispose();
            g = null;
        }

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

        private void BlokSTART_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void BlokSTART_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle rect = new Rectangle(1, 1, 150, 75);
            PaintEventArgs pe = new PaintEventArgs(graph, rect);
            this.OnPaint(pe);
        }
    }
}
