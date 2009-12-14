namespace libbloki
{
    partial class BlokWeWy
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
            this.txt.BackColor = System.Drawing.Color.Wheat;
            this.txt.Location = new System.Drawing.Point(20, 10);
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(134, 57);
            this.txt.TabIndex = 0;
            this.txt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txt.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txt_MouseMove);
            this.txt.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txt_MouseDoubleClick);
            this.txt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txt_MouseDown);
            this.txt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txt_MouseUp);
            // 
            // BlokWeWy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.txt);
            this.Name = "BlokWeWy";
            this.Size = new System.Drawing.Size(174, 77);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label txt;
        private System.Windows.Forms.ToolTip txtHint;
    }
}
