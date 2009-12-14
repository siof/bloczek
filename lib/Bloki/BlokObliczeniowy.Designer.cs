namespace libbloki
{
    partial class BlokObliczeniowy
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txt = new System.Windows.Forms.Label();
            this.txtHint = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // txt
            // 
            this.txt.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.txt.BackColor = System.Drawing.Color.Wheat;
            this.txt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txt.ForeColor = System.Drawing.Color.Black;
            this.txt.Location = new System.Drawing.Point(10, 10);
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(132, 57);
            this.txt.TabIndex = 0;
            this.txt.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.txt.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txt_MouseMove);
            this.txt.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txt_MouseDoubleClick);
            this.txt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txt_MouseDown);
            this.txt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txt_MouseUp);
            // 
            // BlokObliczeniowy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.txt);
            this.Name = "BlokObliczeniowy";
            this.Size = new System.Drawing.Size(152, 77);
            this.txtHint.SetToolTip(this, "tooltip\r\naaaaa");
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label txt;
        private System.Windows.Forms.ToolTip txtHint;

    }
}
