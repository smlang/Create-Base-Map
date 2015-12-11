namespace CreateBaseMap
{
    partial class MapSettingsUserControl
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
            this.selectOcadFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.selectOcadFileButton = new System.Windows.Forms.Button();
            this.selectOcadFileTextBox = new System.Windows.Forms.TextBox();
            this.nextButton = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.osGridReferenceLabel = new System.Windows.Forms.Label();
            this.selectOcadFileLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // selectOcadFileDialog
            // 
            this.selectOcadFileDialog.DefaultExt = "*.ocd";
            this.selectOcadFileDialog.Filter = "OCAD9|*.ocd";
            this.selectOcadFileDialog.Title = "OCAD File with MDOC Street Symbols";
            // 
            // selectOcadFileButton
            // 
            this.selectOcadFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.errorProvider.SetIconAlignment(this.selectOcadFileButton, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.selectOcadFileButton.Location = new System.Drawing.Point(107, 23);
            this.selectOcadFileButton.Name = "selectOcadFileButton";
            this.selectOcadFileButton.Size = new System.Drawing.Size(109, 23);
            this.selectOcadFileButton.TabIndex = 3;
            this.selectOcadFileButton.Text = "&Select OCAD File...";
            this.selectOcadFileButton.UseVisualStyleBackColor = true;
            this.selectOcadFileButton.Click += new System.EventHandler(this.loadOcadButton_Click);
            // 
            // selectOcadFileTextBox
            // 
            this.selectOcadFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectOcadFileTextBox.Enabled = false;
            this.selectOcadFileTextBox.Location = new System.Drawing.Point(3, 25);
            this.selectOcadFileTextBox.Name = "selectOcadFileTextBox";
            this.selectOcadFileTextBox.Size = new System.Drawing.Size(98, 20);
            this.selectOcadFileTextBox.TabIndex = 2;
            // 
            // nextButton
            // 
            this.nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nextButton.Location = new System.Drawing.Point(141, 312);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(75, 23);
            this.nextButton.TabIndex = 9;
            this.nextButton.Text = "&Next >";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // osGridReferenceLabel
            // 
            this.osGridReferenceLabel.AutoSize = true;
            this.osGridReferenceLabel.Location = new System.Drawing.Point(3, 49);
            this.osGridReferenceLabel.Name = "osGridReferenceLabel";
            this.osGridReferenceLabel.Size = new System.Drawing.Size(137, 13);
            this.osGridReferenceLabel.TabIndex = 4;
            this.osGridReferenceLabel.Text = "Centre (OS Grid Reference)";
            // 
            // osGridReferenceUserControl
            // 
            this.osGridReferenceUserControl.Location = new System.Drawing.Point(3, 65);
            this.osGridReferenceUserControl.Name = "osGridReferenceUserControl";
            this.osGridReferenceUserControl.Size = new System.Drawing.Size(120, 20);
            this.osGridReferenceUserControl.TabIndex = 5;
            // 
            // selectOcadFileLabel
            // 
            this.selectOcadFileLabel.AutoSize = true;
            this.selectOcadFileLabel.Location = new System.Drawing.Point(3, 7);
            this.selectOcadFileLabel.Name = "selectOcadFileLabel";
            this.selectOcadFileLabel.Size = new System.Drawing.Size(215, 13);
            this.selectOcadFileLabel.TabIndex = 1;
            this.selectOcadFileLabel.Text = "Scale, Symbols, Colours (Source OCAD File)";
            // 
            // MapSettingsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.selectOcadFileLabel);
            this.Controls.Add(this.osGridReferenceLabel);
            this.Controls.Add(this.osGridReferenceUserControl);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.selectOcadFileTextBox);
            this.Controls.Add(this.selectOcadFileButton);
            this.Name = "MapSettingsUserControl";
            this.Size = new System.Drawing.Size(220, 338);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button selectOcadFileButton;
        private System.Windows.Forms.TextBox selectOcadFileTextBox;
        internal System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label selectOcadFileLabel;
        private System.Windows.Forms.Label osGridReferenceLabel;
        private OSGridReferenceUserControl osGridReferenceUserControl;
        private System.Windows.Forms.OpenFileDialog selectOcadFileDialog;
    }
}
