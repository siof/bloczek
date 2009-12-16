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
        IList<Zmienna> listaZmiennych;
        int index;

        public Czytaj()
        {
            InitializeComponent();
        }

        public Czytaj(IList<Zmienna> usr, int ind)
        {
            InitializeComponent();
            listaZmiennych = usr;
            this.index = ind;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listaZmiennych[index].wartosc = maskedTextBox1.Text.ToString();
            Close();
        }
    }
}
