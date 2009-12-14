using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace libbloki
{
    public partial class Czytaj : Form
    {
        String bWeWy;

        public Czytaj()
        {
            InitializeComponent();
        }

        public Czytaj(String usr)
        {
            InitializeComponent();
            bWeWy = usr;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bWeWy = maskedTextBox1.Text.ToString();
            Close();
        }
    }
}
