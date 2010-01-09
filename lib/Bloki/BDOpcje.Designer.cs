namespace libbloki
{
    partial class BDOpcje
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
            this.btnDodaj = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.listBoxZmienne = new System.Windows.Forms.ListBox();
            this.txtBoxP = new System.Windows.Forms.TextBox();
            this.btnAnuluj = new System.Windows.Forms.Button();
            this.btnZapisz = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.txtBoxL = new System.Windows.Forms.TextBox();
            this.btnNaDol = new System.Windows.Forms.Button();
            this.btnDoGory = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDodaj
            // 
            this.btnDodaj.Location = new System.Drawing.Point(354, 12);
            this.btnDodaj.Name = "btnDodaj";
            this.btnDodaj.Size = new System.Drawing.Size(56, 23);
            this.btnDodaj.TabIndex = 28;
            this.btnDodaj.Text = "Dodaj";
            this.btnDodaj.UseVisualStyleBackColor = true;
            this.btnDodaj.Click += new System.EventHandler(this.btnDodaj_Click);
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(113, 52);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(266, 186);
            this.listBox.TabIndex = 27;
            this.listBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox_KeyDown);
            // 
            // listBoxZmienne
            // 
            this.listBoxZmienne.FormattingEnabled = true;
            this.listBoxZmienne.Location = new System.Drawing.Point(12, 52);
            this.listBoxZmienne.Name = "listBoxZmienne";
            this.listBoxZmienne.Size = new System.Drawing.Size(81, 186);
            this.listBoxZmienne.TabIndex = 26;
            this.listBoxZmienne.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxZmienne_MouseDoubleClick);
            // 
            // txtBoxP
            // 
            this.txtBoxP.Location = new System.Drawing.Point(194, 12);
            this.txtBoxP.Name = "txtBoxP";
            this.txtBoxP.Size = new System.Drawing.Size(154, 20);
            this.txtBoxP.TabIndex = 25;
            // 
            // btnAnuluj
            // 
            this.btnAnuluj.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAnuluj.Location = new System.Drawing.Point(215, 245);
            this.btnAnuluj.Name = "btnAnuluj";
            this.btnAnuluj.Size = new System.Drawing.Size(75, 23);
            this.btnAnuluj.TabIndex = 30;
            this.btnAnuluj.Text = "Anuluj";
            this.btnAnuluj.UseVisualStyleBackColor = true;
            this.btnAnuluj.Click += new System.EventHandler(this.btnAnuluj_Click);
            // 
            // btnZapisz
            // 
            this.btnZapisz.Location = new System.Drawing.Point(130, 245);
            this.btnZapisz.Name = "btnZapisz";
            this.btnZapisz.Size = new System.Drawing.Size(75, 23);
            this.btnZapisz.TabIndex = 29;
            this.btnZapisz.Text = "Zapisz";
            this.btnZapisz.UseVisualStyleBackColor = true;
            this.btnZapisz.Click += new System.EventHandler(this.btnZapisz_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "=",
            ">",
            "<",
            "<>",
            ">=",
            "<="});
            this.comboBox2.Location = new System.Drawing.Point(144, 12);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(44, 21);
            this.comboBox2.TabIndex = 31;
            this.comboBox2.Text = "Dzial";
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "",
            "AND",
            "OR"});
            this.comboBox3.Location = new System.Drawing.Point(12, 12);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(39, 21);
            this.comboBox3.TabIndex = 32;
            this.comboBox3.Visible = false;
            // 
            // txtBoxL
            // 
            this.txtBoxL.Location = new System.Drawing.Point(57, 12);
            this.txtBoxL.Name = "txtBoxL";
            this.txtBoxL.Size = new System.Drawing.Size(81, 20);
            this.txtBoxL.TabIndex = 33;
            // 
            // btnNaDol
            // 
            this.btnNaDol.Location = new System.Drawing.Point(385, 173);
            this.btnNaDol.Name = "btnNaDol";
            this.btnNaDol.Size = new System.Drawing.Size(25, 23);
            this.btnNaDol.TabIndex = 35;
            this.btnNaDol.Text = "\\/";
            this.btnNaDol.UseVisualStyleBackColor = true;
            this.btnNaDol.Click += new System.EventHandler(this.btnNaDol_Click);
            // 
            // btnDoGory
            // 
            this.btnDoGory.Location = new System.Drawing.Point(385, 89);
            this.btnDoGory.Name = "btnDoGory";
            this.btnDoGory.Size = new System.Drawing.Size(25, 23);
            this.btnDoGory.TabIndex = 34;
            this.btnDoGory.Text = "/\\";
            this.btnDoGory.UseVisualStyleBackColor = true;
            this.btnDoGory.Click += new System.EventHandler(this.btnDoGory_Click);
            // 
            // BDOpcje
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 276);
            this.Controls.Add(this.btnNaDol);
            this.Controls.Add(this.btnDoGory);
            this.Controls.Add(this.txtBoxL);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.btnAnuluj);
            this.Controls.Add(this.btnZapisz);
            this.Controls.Add(this.btnDodaj);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.listBoxZmienne);
            this.Controls.Add(this.txtBoxP);
            this.Name = "BDOpcje";
            this.Text = "BDOpcje";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDodaj;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.ListBox listBoxZmienne;
        private System.Windows.Forms.TextBox txtBoxP;
        private System.Windows.Forms.Button btnAnuluj;
        private System.Windows.Forms.Button btnZapisz;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.TextBox txtBoxL;
        private System.Windows.Forms.Button btnNaDol;
        private System.Windows.Forms.Button btnDoGory;

    }
}