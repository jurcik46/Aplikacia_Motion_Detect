namespace Aplikacia_Motion_Detec.UI.Views.MainWindow
{
    partial class VideoDisplayControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoDisplayControl));
            this.axVideoDisplayControl1 = new AxDTKVideoCapLib.AxVideoDisplayControl();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.axVideoDisplayControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // axVideoDisplayControl1
            // 
            this.axVideoDisplayControl1.Enabled = true;
            this.axVideoDisplayControl1.Location = new System.Drawing.Point(0, 0);
            this.axVideoDisplayControl1.Name = "axVideoDisplayControl1";
            this.axVideoDisplayControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axVideoDisplayControl1.OcxState")));
            this.axVideoDisplayControl1.Size = new System.Drawing.Size(1081, 670);
            this.axVideoDisplayControl1.TabIndex = 0;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // VideoDisplayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axVideoDisplayControl1);
            this.Name = "VideoDisplayControl";
            this.Size = new System.Drawing.Size(1081, 670);
            ((System.ComponentModel.ISupportInitialize)(this.axVideoDisplayControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxDTKVideoCapLib.AxVideoDisplayControl axVideoDisplayControl1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
