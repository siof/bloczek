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
    public partial class BlokWeWy : UserControl
    {
        private Point klikoffset;
        private bool klik;

        public BlokWeWy()
        {
            InitializeComponent();
        }

        private void BlokWeWy_MouseDown(object sender, MouseEventArgs e)
        {
            klik = true;
            klikoffset.X = e.X;
            klikoffset.Y = e.Y;
        }

        private void BlokWeWy_MouseUp(object sender, MouseEventArgs e)
        {
            klik = false;
        }

        private void BlokWeWy_MouseMove(object sender, MouseEventArgs e)
        {
            if (klik)
            {
                this.Left = e.X + this.Left - klikoffset.X;
                this.Top = e.Y + this.Top - klikoffset.Y;
            }
        }
    }
}
