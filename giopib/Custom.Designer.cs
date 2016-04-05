namespace giopib
{
    partial class Custom
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
            this.PBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // PBar
            // 
            this.PBar.Location = new System.Drawing.Point(21, 184);
            this.PBar.Maximum = 12;
            this.PBar.Name = "PBar";
            this.PBar.Size = new System.Drawing.Size(135, 28);
            this.PBar.TabIndex = 0;
            // 
            // Custom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 459);
            this.Controls.Add(this.PBar);
            this.Name = "Custom";
            this.Text = "Custom";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar PBar;
    }
}