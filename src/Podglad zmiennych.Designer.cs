namespace Okienka
{
    partial class Podglad_zmiennych
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
            this.lbZmienne = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbObserwowane = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbZmienne
            // 
            this.lbZmienne.FormattingEnabled = true;
            this.lbZmienne.Location = new System.Drawing.Point(12, 42);
            this.lbZmienne.Name = "lbZmienne";
            this.lbZmienne.Size = new System.Drawing.Size(120, 381);
            this.lbZmienne.TabIndex = 0;
            this.lbZmienne.DoubleClick += new System.EventHandler(this.lbZmienne_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "kliknij 2 razy aby dodac do \r\nobserwowanych zmiennych";
            // 
            // lbObserwowane
            // 
            this.lbObserwowane.FormattingEnabled = true;
            this.lbObserwowane.HorizontalScrollbar = true;
            this.lbObserwowane.Location = new System.Drawing.Point(161, 42);
            this.lbObserwowane.Name = "lbObserwowane";
            this.lbObserwowane.Size = new System.Drawing.Size(300, 381);
            this.lbObserwowane.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(253, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Obserwowane zmienne:";
            // 
            // Podglad_zmiennych
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 435);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbObserwowane);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbZmienne);
            this.Name = "Podglad_zmiennych";
            this.Text = "Podglad zmiennych";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbZmienne;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbObserwowane;
        private System.Windows.Forms.Label label2;

    }
}