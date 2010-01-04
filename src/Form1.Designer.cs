namespace Okienka
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tsBloki = new System.Windows.Forms.ToolStrip();
            this.dodajBlokStart = new System.Windows.Forms.ToolStripButton();
            this.dodajBlokStop = new System.Windows.Forms.ToolStripButton();
            this.dodajBlokObliczeniowy = new System.Windows.Forms.ToolStripButton();
            this.dodajBlokDecyzyjny = new System.Windows.Forms.ToolStripButton();
            this.dodajBlokWeWy = new System.Windows.Forms.ToolStripButton();
            this.Połączenie = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zapiszKodŹródłowyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zapiszKodŹródłowyJakoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.symulacjaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pełnaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.krokowaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dodatkoweToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.podgladZmiennychToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsPracaKrokowa = new System.Windows.Forms.ToolStrip();
            this.nastepny = new System.Windows.Forms.ToolStripButton();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tsBloki.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tsPracaKrokowa.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsBloki
            // 
            this.tsBloki.Dock = System.Windows.Forms.DockStyle.Left;
            this.tsBloki.ImageScalingSize = new System.Drawing.Size(29, 20);
            this.tsBloki.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dodajBlokStart,
            this.dodajBlokStop,
            this.dodajBlokObliczeniowy,
            this.dodajBlokDecyzyjny,
            this.dodajBlokWeWy,
            this.Połączenie});
            this.tsBloki.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.tsBloki.Location = new System.Drawing.Point(0, 24);
            this.tsBloki.Name = "tsBloki";
            this.tsBloki.Size = new System.Drawing.Size(34, 520);
            this.tsBloki.TabIndex = 3;
            this.tsBloki.Text = "Bloki";
            // 
            // dodajBlokStart
            // 
            this.dodajBlokStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.dodajBlokStart.Image = ((System.Drawing.Image)(resources.GetObject("dodajBlokStart.Image")));
            this.dodajBlokStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dodajBlokStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dodajBlokStart.Name = "dodajBlokStart";
            this.dodajBlokStart.Size = new System.Drawing.Size(31, 24);
            this.dodajBlokStart.Text = "Blok Start";
            this.dodajBlokStart.Click += new System.EventHandler(this.dodajBlokStart_Click);
            // 
            // dodajBlokStop
            // 
            this.dodajBlokStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.dodajBlokStop.Image = ((System.Drawing.Image)(resources.GetObject("dodajBlokStop.Image")));
            this.dodajBlokStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dodajBlokStop.Name = "dodajBlokStop";
            this.dodajBlokStop.Size = new System.Drawing.Size(31, 24);
            this.dodajBlokStop.Text = "Blok Stop";
            this.dodajBlokStop.Click += new System.EventHandler(this.dodajBlokStop_Click);
            // 
            // dodajBlokObliczeniowy
            // 
            this.dodajBlokObliczeniowy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.dodajBlokObliczeniowy.Image = ((System.Drawing.Image)(resources.GetObject("dodajBlokObliczeniowy.Image")));
            this.dodajBlokObliczeniowy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dodajBlokObliczeniowy.Name = "dodajBlokObliczeniowy";
            this.dodajBlokObliczeniowy.Size = new System.Drawing.Size(31, 24);
            this.dodajBlokObliczeniowy.Text = "Blok Obliczeniowy";
            this.dodajBlokObliczeniowy.Click += new System.EventHandler(this.dodajBlokObliczeniowy_Click);
            // 
            // dodajBlokDecyzyjny
            // 
            this.dodajBlokDecyzyjny.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.dodajBlokDecyzyjny.Image = ((System.Drawing.Image)(resources.GetObject("dodajBlokDecyzyjny.Image")));
            this.dodajBlokDecyzyjny.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dodajBlokDecyzyjny.Name = "dodajBlokDecyzyjny";
            this.dodajBlokDecyzyjny.Size = new System.Drawing.Size(31, 24);
            this.dodajBlokDecyzyjny.Text = "Blok Decyzyjny";
            this.dodajBlokDecyzyjny.Click += new System.EventHandler(this.dodajBlokDecyzyjny_Click);
            // 
            // dodajBlokWeWy
            // 
            this.dodajBlokWeWy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.dodajBlokWeWy.Image = ((System.Drawing.Image)(resources.GetObject("dodajBlokWeWy.Image")));
            this.dodajBlokWeWy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dodajBlokWeWy.Name = "dodajBlokWeWy";
            this.dodajBlokWeWy.Size = new System.Drawing.Size(31, 24);
            this.dodajBlokWeWy.Text = "Blok Wejścia/Wyjścia";
            this.dodajBlokWeWy.Click += new System.EventHandler(this.dodajBlokWeWy_Click);
            // 
            // Połączenie
            // 
            this.Połączenie.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Połączenie.Image = ((System.Drawing.Image)(resources.GetObject("Połączenie.Image")));
            this.Połączenie.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Połączenie.Name = "Połączenie";
            this.Połączenie.Size = new System.Drawing.Size(31, 24);
            this.Połączenie.Text = "toolStripButton6";
            this.Połączenie.ToolTipText = "Połączenie";
            this.Połączenie.Click += new System.EventHandler(this.Połączenie_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(34, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(726, 520);
            this.panel1.TabIndex = 0;
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.Click += new System.EventHandler(this.panel1_Click);
            this.panel1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panel1_Scroll);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.symulacjaToolStripMenuItem,
            this.dodatkoweToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(792, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.zapiszKodŹródłowyToolStripMenuItem,
            this.zapiszKodŹródłowyJakoToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(34, 20);
            this.fileToolStripMenuItem.Text = "&Plik";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.newToolStripMenuItem.Text = "&Nowy";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.openToolStripMenuItem.Text = "&Otwórz schemat";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(191, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.saveToolStripMenuItem.Text = "&Zapisz schemat";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.saveAsToolStripMenuItem.Text = "Z&apisz schemat jako";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // zapiszKodŹródłowyToolStripMenuItem
            // 
            this.zapiszKodŹródłowyToolStripMenuItem.Name = "zapiszKodŹródłowyToolStripMenuItem";
            this.zapiszKodŹródłowyToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.zapiszKodŹródłowyToolStripMenuItem.Text = "Zapisz kod źródłowy";
            this.zapiszKodŹródłowyToolStripMenuItem.Visible = false;
            // 
            // zapiszKodŹródłowyJakoToolStripMenuItem
            // 
            this.zapiszKodŹródłowyJakoToolStripMenuItem.Name = "zapiszKodŹródłowyJakoToolStripMenuItem";
            this.zapiszKodŹródłowyJakoToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.zapiszKodŹródłowyJakoToolStripMenuItem.Text = "Zapisz kod źródłowy jako";
            this.zapiszKodŹródłowyJakoToolStripMenuItem.Visible = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(191, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // symulacjaToolStripMenuItem
            // 
            this.symulacjaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pełnaToolStripMenuItem,
            this.krokowaToolStripMenuItem});
            this.symulacjaToolStripMenuItem.Name = "symulacjaToolStripMenuItem";
            this.symulacjaToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.symulacjaToolStripMenuItem.Text = "Symulacja";
            // 
            // pełnaToolStripMenuItem
            // 
            this.pełnaToolStripMenuItem.Name = "pełnaToolStripMenuItem";
            this.pełnaToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.pełnaToolStripMenuItem.Text = "Pełna";
            this.pełnaToolStripMenuItem.Click += new System.EventHandler(this.pełnaToolStripMenuItem_Click);
            // 
            // krokowaToolStripMenuItem
            // 
            this.krokowaToolStripMenuItem.Name = "krokowaToolStripMenuItem";
            this.krokowaToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.krokowaToolStripMenuItem.Text = "Krokowa";
            this.krokowaToolStripMenuItem.Click += new System.EventHandler(this.krokowaToolStripMenuItem_Click);
            // 
            // dodatkoweToolStripMenuItem
            // 
            this.dodatkoweToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.podgladZmiennychToolStripMenuItem});
            this.dodatkoweToolStripMenuItem.Name = "dodatkoweToolStripMenuItem";
            this.dodatkoweToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.dodatkoweToolStripMenuItem.Text = "Dodatkowe";
            // 
            // podgladZmiennychToolStripMenuItem
            // 
            this.podgladZmiennychToolStripMenuItem.Name = "podgladZmiennychToolStripMenuItem";
            this.podgladZmiennychToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.podgladZmiennychToolStripMenuItem.Text = "Podglad zmiennych";
            this.podgladZmiennychToolStripMenuItem.Click += new System.EventHandler(this.podgladZmiennychToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 544);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(792, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(109, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // tsPracaKrokowa
            // 
            this.tsPracaKrokowa.Dock = System.Windows.Forms.DockStyle.Right;
            this.tsPracaKrokowa.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nastepny});
            this.tsPracaKrokowa.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.tsPracaKrokowa.Location = new System.Drawing.Point(760, 24);
            this.tsPracaKrokowa.Name = "tsPracaKrokowa";
            this.tsPracaKrokowa.Size = new System.Drawing.Size(32, 520);
            this.tsPracaKrokowa.TabIndex = 4;
            this.tsPracaKrokowa.Text = "pracaKrokowa";
            this.tsPracaKrokowa.Visible = false;
            // 
            // nastepny
            // 
            this.nastepny.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.nastepny.Image = ((System.Drawing.Image)(resources.GetObject("nastepny.Image")));
            this.nastepny.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.nastepny.Name = "nastepny";
            this.nastepny.Size = new System.Drawing.Size(29, 20);
            this.nastepny.Text = "Nastepny";
            this.nastepny.Click += new System.EventHandler(this.nastepny_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tsPracaKrokowa);
            this.Controls.Add(this.tsBloki);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.tsBloki.ResumeLayout(false);
            this.tsBloki.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tsPracaKrokowa.ResumeLayout(false);
            this.tsPracaKrokowa.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsBloki;
        private System.Windows.Forms.ToolStripButton dodajBlokObliczeniowy;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton dodajBlokStart;
        private System.Windows.Forms.ToolStripButton dodajBlokStop;
        private System.Windows.Forms.ToolStripButton dodajBlokDecyzyjny;
        private System.Windows.Forms.ToolStripButton dodajBlokWeWy;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripButton Połączenie;
        private System.Windows.Forms.ToolStripMenuItem symulacjaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pełnaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem krokowaToolStripMenuItem;
        private System.Windows.Forms.ToolStrip tsPracaKrokowa;
        private System.Windows.Forms.ToolStripButton nastepny;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem zapiszKodŹródłowyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zapiszKodŹródłowyJakoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dodatkoweToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem podgladZmiennychToolStripMenuItem;
    }
}

