namespace Aplikacia_Motion_Detect.UI.Views.MotionZones
{
    partial class VideoDisplayMotionZones
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoDisplayMotionZones));
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
            this.axVideoDisplayControl1.Size = new System.Drawing.Size(900, 600);
            this.axVideoDisplayControl1.TabIndex = 0;
            this.axVideoDisplayControl1.VideoMouseDown += new AxDTKVideoCapLib._IVideoDisplayControlEvents_VideoMouseDownEventHandler(this.axVideoDisplayControl1_VideoMouseDown);
            this.axVideoDisplayControl1.VideoMouseUp += new AxDTKVideoCapLib._IVideoDisplayControlEvents_VideoMouseUpEventHandler(this.axVideoDisplayControl1_VideoMouseUp);
            // 
            // VideoDisplayMotionZones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axVideoDisplayControl1);
            this.Name = "VideoDisplayMotionZones";
            this.Size = new System.Drawing.Size(900, 600);
            ((System.ComponentModel.ISupportInitialize)(this.axVideoDisplayControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxDTKVideoCapLib.AxVideoDisplayControl axVideoDisplayControl1;
    }
}
