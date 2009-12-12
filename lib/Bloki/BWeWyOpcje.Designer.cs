namespace libbloki
{
    partial class BWeWyOpcje
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.txtBox = new System.Windows.Forms.TextBox();
            this.listBoxZmienne = new System.Windows.Forms.ListBox();
            this.btnAnuluj = new System.Windows.Forms.Button();
            this.btnZapisz = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.btnDodaj = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Czytaj",
            "Wypisz"});
            this.comboBox1.Location = new System.Drawing.Point(12, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(81, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // txtBox
            // 
            this.txtBox.Location = new System.Drawing.Point(113, 12);
            this.txtBox.Name = "txtBox";
            this.txtBox.Size = new System.Drawing.Size(235, 20);
            this.txtBox.TabIndex = 2;
            // 
            // listBoxZmienne
            // 
            this.listBoxZmienne.FormattingEnabled = true;
            this.listBoxZmienne.Location = new System.Drawing.Point(12, 52);
            this.listBoxZmienne.Name = "listBoxZmienne";
            this.listBoxZmienne.Size = new System.Drawing.Size(81, 186);
            this.listBoxZmienne.TabIndex = 3;
            this.listBoxZmienne.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxZmienne_MouseDoubleClick);
            // 
            // btnAnuluj
            // 
            this.btnAnuluj.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAnuluj.Location = new System.Drawing.Point(215, 245);
            this.btnAnuluj.Name = "btnAnuluj";
            this.btnAnuluj.Size = new System.Drawing.Size(75, 23);
            this.btnAnuluj.TabIndex = 21;
            this.btnAnuluj.Text = "Anuluj";
            this.btnAnuluj.UseVisualStyleBackColor = true;
            this.btnAnuluj.Click += new System.EventHandler(this.btnAnuluj_Click);
            // 
            // btnZapisz
            // 
            this.btnZapisz.Location = new System.Drawing.Point(130, 245);
            this.btnZapisz.Name = "btnZapisz";
            this.btnZapisz.Size = new System.Drawing.Size(75, 23);
            this.btnZapisz.TabIndex = 20;
            this.btnZapisz.Text = "Zapisz";
            this.btnZapisz.UseVisualStyleBackColor = true;
            this.btnZapisz.Click += new System.EventHandler(this.btnZapisz_Click);
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(113, 52);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(297, 186);
            this.listBox.TabIndex = 22;
            // 
            // btnDodaj
            // 
            this.btnDodaj.Location = new System.Drawing.Point(354, 12);
            this.btnDodaj.Name = "btnDodaj";
            this.btnDodaj.Size = new System.Drawing.Size(56, 23);
            this.btnDodaj.TabIndex = 23;
            this.btnDodaj.Text = "Dodaj";
            this.btnDodaj.UseVisualStyleBackColor = true;
            this.btnDodaj.Click += new System.EventHandler(this.Dodaj_Click);
            // 
            // BWeWyOpcje
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 276);
            this.Controls.Add(this.btnDodaj);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.btnAnuluj);
            this.Controls.Add(this.btnZapisz);
            this.Controls.Add(this.listBoxZmienne);
            this.Controls.Add(this.txtBox);
            this.Controls.Add(this.comboBox1);
            this.Name = "BWeWyOpcje";
            this.Text = "BWeWyOpcje";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox txtBox;
        private System.Windows.Forms.ListBox listBoxZmienne;
        private System.Windows.Forms.Button btnAnuluj;
        private System.Windows.Forms.Button btnZapisz;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Button btnDodaj;
    }
}