namespace libbloki
{
    partial class BOOpcje
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
            this.btnZapisz = new System.Windows.Forms.Button();
            this.btnAnuluj = new System.Windows.Forms.Button();
            this.btnDodaj = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.listBoxZmienne = new System.Windows.Forms.ListBox();
            this.txtL = new System.Windows.Forms.TextBox();
            this.txtS = new System.Windows.Forms.TextBox();
            this.txtP = new System.Windows.Forms.TextBox();
            this.cbDzialanie = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnZapisz
            // 
            this.btnZapisz.Location = new System.Drawing.Point(130, 245);
            this.btnZapisz.Name = "btnZapisz";
            this.btnZapisz.Size = new System.Drawing.Size(75, 23);
            this.btnZapisz.TabIndex = 0;
            this.btnZapisz.Text = "Zapisz";
            this.btnZapisz.UseVisualStyleBackColor = true;
            this.btnZapisz.Click += new System.EventHandler(this.btnZapisz_Click);
            // 
            // btnAnuluj
            // 
            this.btnAnuluj.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAnuluj.Location = new System.Drawing.Point(215, 245);
            this.btnAnuluj.Name = "btnAnuluj";
            this.btnAnuluj.Size = new System.Drawing.Size(75, 23);
            this.btnAnuluj.TabIndex = 1;
            this.btnAnuluj.Text = "Anuluj";
            this.btnAnuluj.UseVisualStyleBackColor = true;
            this.btnAnuluj.Click += new System.EventHandler(this.btnAnuluj_Click);
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
            this.listBox.Size = new System.Drawing.Size(297, 186);
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
            // txtL
            // 
            this.txtL.Location = new System.Drawing.Point(71, 12);
            this.txtL.Name = "txtL";
            this.txtL.Size = new System.Drawing.Size(59, 20);
            this.txtL.TabIndex = 29;
            // 
            // txtS
            // 
            this.txtS.Location = new System.Drawing.Point(158, 12);
            this.txtS.Name = "txtS";
            this.txtS.Size = new System.Drawing.Size(72, 20);
            this.txtS.TabIndex = 30;
            // 
            // txtP
            // 
            this.txtP.Location = new System.Drawing.Point(282, 12);
            this.txtP.Name = "txtP";
            this.txtP.Size = new System.Drawing.Size(66, 20);
            this.txtP.TabIndex = 31;
            // 
            // cbDzialanie
            // 
            this.cbDzialanie.FormattingEnabled = true;
            this.cbDzialanie.Items.AddRange(new object[] {
            "",
            "+",
            "-",
            "*",
            "/"});
            this.cbDzialanie.Location = new System.Drawing.Point(236, 12);
            this.cbDzialanie.Name = "cbDzialanie";
            this.cbDzialanie.Size = new System.Drawing.Size(40, 21);
            this.cbDzialanie.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(136, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = ":=";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "int",
            "double",
            "String"});
            this.comboBox2.Location = new System.Drawing.Point(12, 12);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(53, 21);
            this.comboBox2.TabIndex = 34;
            // 
            // BOOpcje
            // 
            this.AcceptButton = this.btnZapisz;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnAnuluj;
            this.ClientSize = new System.Drawing.Size(422, 276);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbDzialanie);
            this.Controls.Add(this.txtP);
            this.Controls.Add(this.txtS);
            this.Controls.Add(this.txtL);
            this.Controls.Add(this.btnDodaj);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.listBoxZmienne);
            this.Controls.Add(this.btnAnuluj);
            this.Controls.Add(this.btnZapisz);
            this.Name = "BOOpcje";
            this.Text = "opcje";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnZapisz;
        private System.Windows.Forms.Button btnAnuluj;
        private System.Windows.Forms.Button btnDodaj;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.ListBox listBoxZmienne;
        private System.Windows.Forms.TextBox txtL;
        private System.Windows.Forms.TextBox txtS;
        private System.Windows.Forms.TextBox txtP;
        private System.Windows.Forms.ComboBox cbDzialanie;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox2;
    }
}