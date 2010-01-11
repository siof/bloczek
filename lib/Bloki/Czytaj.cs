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
        int nrElTab;

        public Czytaj()
        {
            InitializeComponent();
        }

        public Czytaj(IList<Zmienna> usr, int ind, int nrEl)
        {
            InitializeComponent();
            listaZmiennych = usr;
            this.index = ind;
            nrElTab = nrEl;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listaZmiennych == null)
            {
                MessageBox.Show("Czytaj: OK: brak listy zmiennych");
                return;
            }

            if (index < 0)
            {
                MessageBox.Show("Czytaj: OK: nrElTab ujemny:" + index.ToString());
                return;
            }

            if (index  > listaZmiennych.Count - 1)
            {
                MessageBox.Show("Czytaj: OK: index poza lista zmiennych");
                return;
            }

            if (listaZmiennych[index].tablica == true)
            {
                if (nrElTab < 0)
                {
                    MessageBox.Show("Czytaj: OK: nrElTab ujemny" + nrElTab.ToString());
                    return;
                }

                if (nrElTab > listaZmiennych[index].wartosci.Count - 1)
                {
                    MessageBox.Show("Czytaj: OK: nrElTab poza lista wartosci");
                    return;
                }

                listaZmiennych[index].wartosci[nrElTab] = maskedTextBox1.Text.ToString();

            }
            else
                listaZmiennych[index].wartosc = maskedTextBox1.Text.ToString();

            Close();
        }
    }
}
