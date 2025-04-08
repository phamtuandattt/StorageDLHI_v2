namespace StorageDLHI.App.Common.CommonGUI
{
    partial class LoadingControl
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
            this.pictureBoxLoader = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoader)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxLoader
            // 
            this.pictureBoxLoader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxLoader.Image = global::StorageDLHI.App.Properties.Resources.Dual_Ring_1x_1_0s_200px_200px;
            this.pictureBoxLoader.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxLoader.Name = "pictureBoxLoader";
            this.pictureBoxLoader.Size = new System.Drawing.Size(1264, 676);
            this.pictureBoxLoader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxLoader.TabIndex = 0;
            this.pictureBoxLoader.TabStop = false;
            // 
            // LoadingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBoxLoader);
            this.Name = "LoadingControl";
            this.Size = new System.Drawing.Size(1264, 676);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoader)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxLoader;
    }
}
