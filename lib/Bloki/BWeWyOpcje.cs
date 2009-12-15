﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace libbloki
{
    public partial class BWeWyOpcje : Form
    {
        BlokWeWy BWeWy;
        IList<Działanie> dodaneDzialania = new List<Działanie>();

        public BWeWyOpcje()
        {
            InitializeComponent();
        }

        public BWeWyOpcje(BlokWeWy blok)
        {
            InitializeComponent();

            BWeWy = blok;
            String temp = "";

            for (int i = 0; i < BWeWy.dzialania.Count; i++)
            {
                temp = BWeWy.dzialania[i].dzialanie1 + " : ";

                if (BWeWy.dzialania[i].srodekZmienna == true)
                    temp += BWeWy.znacznikZmiennej + BWeWy.dzialania[i].srodek + BWeWy.znacznikZmiennej;
                else
                    temp += BWeWy.dzialania[i].srodek;

                listBox.Items.Add(temp);
            }

            if (BWeWy.listaZmiennych != null)
            {
                //czytaj liste zmiennych i dodaj do combo/list boxow
                for (int i = 0; i < BWeWy.listaZmiennych.Count; i++)
                {
                    temp = BWeWy.listaZmiennych[i].nazwa.ToString();
                    listBoxZmienne.Items.Add(temp);
                }
            }
        }

        private void listBoxZmienne_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (((ListBox)sender).SelectedItem == null)
                return;

            if (comboBox1.SelectedText == "Czytaj")
            {
                txtBox.Text = BWeWy.znacznikZmiennej + listBoxZmienne.SelectedItem.ToString() + BWeWy.znacznikZmiennej;
            }
            else
            {
                txtBox.Text += " " + BWeWy.znacznikZmiennej + listBoxZmienne.SelectedItem.ToString() + BWeWy.znacznikZmiennej + " ";
            }
        }

        private void Dodaj_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                return;

            String temp;
            Działanie noweDzialanie = new Działanie();

            temp = comboBox1.SelectedItem.ToString() + " : " + txtBox.Text;

            if (comboBox1.SelectedItem.ToString() == "Czytaj" && BWeWy.SprawdzCzyIstniejeZmienna(txtBox.Text) == false)
            {
                noweDzialanie.nowaZmienna = true;
                noweDzialanie.lewa = txtBox.Text;
                if (comboBox2.SelectedItem != null)
                    noweDzialanie.dodatkowe = comboBox2.SelectedItem.ToString();
                else
                    noweDzialanie.dodatkowe = "String";
            }

            noweDzialanie.dzialanie1 = comboBox1.SelectedItem.ToString();
            noweDzialanie.srodek = txtBox.Text;

            listBox.Items.Add(temp);
            BWeWy.dzialania.Add(noweDzialanie);
            dodaneDzialania.Add(noweDzialanie);
        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            //z racji że na bierząco aktualizuje działania to musze je usunąć z listy
            for (int i = 0; i < dodaneDzialania.Count; i++)
            {
                BWeWy.dzialania.Remove(dodaneDzialania[i]);
            }

            dodaneDzialania.Clear(); //na wszelki wypadek

            BWeWyOpcje.ActiveForm.Close();
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            //z racji że na bierząco aktualizuje dzialania to nie trzeba nic robić
            //poza wyświetleniem działań na bloku

            BWeWy.DodajNoweZmienne();
            BWeWy.ReDrawText();

            dodaneDzialania.Clear(); //na wszelki wypadek

            BWeWyOpcje.ActiveForm.Close();
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    BWeWy.dzialania.RemoveAt(listBox.SelectedIndex);
                    listBox.Items.Remove(listBox.SelectedItem);
                }
            }
        }
    }
}
