namespace Aplikacia_Motion_Detect.UI.Views.MainWindow
{
    partial class VideoDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoDisplay));
            this.axVideoDisplayControl1 = new AxDTKVideoCapLib.AxVideoDisplayControl();
            ((System.ComponentModel.ISupportInitialize)(this.axVideoDisplayControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // axVideoDisplayControl1
            // 
            this.axVideoDisplayControl1.Enabled = true;
            this.axVideoDisplayControl1.Location = new System.Drawing.Point(0, 0);
            this.axVideoDisplayControl1.Name = "axVideoDisplayControl1";
            this.axVideoDisplayControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axVideoDisplayControl1.OcxState")));
            this.axVideoDisplayControl1.Size = new System.Drawing.Size(1000, 440);
            this.axVideoDisplayControl1.TabIndex = 0;
            // 
            // VideoDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axVideoDisplayControl1);
            this.Name = "VideoDisplay";
            this.Size = new System.Drawing.Size(1000, 440);
            ((System.ComponentModel.ISupportInitialize)(this.axVideoDisplayControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxDTKVideoCapLib.AxVideoDisplayControl axVideoDisplayControl1;
    }
}
