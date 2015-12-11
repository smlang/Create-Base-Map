namespace CreateBaseMap
{
    partial class OSGridReferenceUserControl
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
            this.osGrid2TextBox = new System.Windows.Forms.TextBox();
            this.osGrid1TextBox = new System.Windows.Forms.TextBox();
            this.osGrid3TextBox = new System.Windows.Forms.TextBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // osGrid2TextBox
            // 
            this.osGrid2TextBox.Location = new System.Drawing.Point(26, 0);
            this.osGrid2TextBox.MaxLength = 5;
            this.osGrid2TextBox.Name = "osGrid2TextBox";
            this.osGrid2TextBox.Size = new System.Drawing.Size(36, 20);
            this.osGrid2TextBox.TabIndex = 14;
            // 
            // osGrid1TextBox
            // 
            this.osGrid1TextBox.Location = new System.Drawing.Point(0, 0);
            this.osGrid1TextBox.MaxLength = 2;
            this.osGrid1TextBox.Name = "osGrid1TextBox";
            this.osGrid1TextBox.Size = new System.Drawing.Size(20, 20);
            this.osGrid1TextBox.TabIndex = 13;
            // 
            // osGrid3TextBox
            // 
            this.osGrid3TextBox.Location = new System.Drawing.Point(68, 0);
            this.osGrid3TextBox.MaxLength = 5;
            this.osGrid3TextBox.Name = "osGrid3TextBox";
            this.osGrid3TextBox.Size = new System.Drawing.Size(36, 20);
            this.osGrid3TextBox.TabIndex = 15;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // OSGridReferenceUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.osGrid2TextBox);
            this.Controls.Add(this.osGrid1TextBox);
            this.Controls.Add(this.osGrid3TextBox);
            this.Name = "OSGridReferenceUserControl";
            this.Size = new System.Drawing.Size(120, 20);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox osGrid2TextBox;
        private System.Windows.Forms.TextBox osGrid1TextBox;
        private System.Windows.Forms.TextBox osGrid3TextBox;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
