namespace CreateBaseMap
{
    partial class DownloadForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadForm));
            this.imagesCheckBox = new System.Windows.Forms.CheckBox();
            this.osmCheckBox = new System.Windows.Forms.CheckBox();
            this.centreLabel = new System.Windows.Forms.Label();
            this.heightLabel = new System.Windows.Forms.Label();
            this.widthTextBox = new System.Windows.Forms.TextBox();
            this.widthLabel = new System.Windows.Forms.Label();
            this.heightTextBox = new System.Windows.Forms.TextBox();
            this.widthLabel2 = new System.Windows.Forms.Label();
            this.heightLabel2 = new System.Windows.Forms.Label();
            this.downloadButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.overwriteCheckBox = new System.Windows.Forms.CheckBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.destinationfolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // imagesCheckBox
            // 
            this.imagesCheckBox.AutoSize = true;
            this.imagesCheckBox.Location = new System.Drawing.Point(25, 90);
            this.imagesCheckBox.Name = "imagesCheckBox";
            this.imagesCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.imagesCheckBox.Size = new System.Drawing.Size(121, 17);
            this.imagesCheckBox.TabIndex = 4;
            this.imagesCheckBox.Text = "Background Images";
            this.imagesCheckBox.UseVisualStyleBackColor = true;
            this.imagesCheckBox.CheckedChanged += new System.EventHandler(this.imagesOrOsmCheckBox_CheckedChanged);
            // 
            // osmCheckBox
            // 
            this.osmCheckBox.AutoSize = true;
            this.osmCheckBox.Location = new System.Drawing.Point(70, 113);
            this.osmCheckBox.Name = "osmCheckBox";
            this.osmCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.osmCheckBox.Size = new System.Drawing.Size(76, 17);
            this.osmCheckBox.TabIndex = 5;
            this.osmCheckBox.Text = "OSM Data";
            this.osmCheckBox.UseVisualStyleBackColor = true;
            this.osmCheckBox.CheckedChanged += new System.EventHandler(this.imagesOrOsmCheckBox_CheckedChanged);
            // 
            // centreLabel
            // 
            this.centreLabel.AutoSize = true;
            this.centreLabel.Location = new System.Drawing.Point(24, 15);
            this.centreLabel.Name = "centreLabel";
            this.centreLabel.Size = new System.Drawing.Size(101, 13);
            this.centreLabel.TabIndex = 17;
            this.centreLabel.Text = "Centre of Download";
            // centreOSGridReferenceUserControl
            this.centreOSGridReferenceUserControl.Location = new System.Drawing.Point(131, 12);
            // 
            // heightLabel
            // 
            this.heightLabel.AutoSize = true;
            this.heightLabel.Location = new System.Drawing.Point(87, 67);
            this.heightLabel.Name = "heightLabel";
            this.heightLabel.Size = new System.Drawing.Size(38, 13);
            this.heightLabel.TabIndex = 25;
            this.heightLabel.Text = "Height";
            this.heightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // widthTextBox
            // 
            this.widthTextBox.Location = new System.Drawing.Point(131, 38);
            this.widthTextBox.MaxLength = 2;
            this.widthTextBox.Name = "widthTextBox";
            this.widthTextBox.Size = new System.Drawing.Size(20, 20);
            this.widthTextBox.TabIndex = 2;
            this.widthTextBox.Text = "4";
            // 
            // widthLabel
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.Location = new System.Drawing.Point(90, 41);
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size(35, 13);
            this.widthLabel.TabIndex = 23;
            this.widthLabel.Text = "Width";
            this.widthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // heightTextBox
            // 
            this.heightTextBox.Location = new System.Drawing.Point(131, 64);
            this.heightTextBox.MaxLength = 2;
            this.heightTextBox.Name = "heightTextBox";
            this.heightTextBox.Size = new System.Drawing.Size(20, 20);
            this.heightTextBox.TabIndex = 3;
            this.heightTextBox.Text = "4";
            // 
            // widthLabel2
            // 
            this.widthLabel2.AutoSize = true;
            this.widthLabel2.Location = new System.Drawing.Point(153, 41);
            this.widthLabel2.Name = "widthLabel2";
            this.widthLabel2.Size = new System.Drawing.Size(21, 13);
            this.widthLabel2.TabIndex = 26;
            this.widthLabel2.Text = "km";
            this.widthLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // heightLabel2
            // 
            this.heightLabel2.AutoSize = true;
            this.heightLabel2.Location = new System.Drawing.Point(153, 67);
            this.heightLabel2.Name = "heightLabel2";
            this.heightLabel2.Size = new System.Drawing.Size(21, 13);
            this.heightLabel2.TabIndex = 27;
            this.heightLabel2.Text = "km";
            this.heightLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // downloadButton
            // 
            this.downloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadButton.Enabled = false;
            this.downloadButton.Location = new System.Drawing.Point(161, 159);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(75, 23);
            this.downloadButton.TabIndex = 8;
            this.downloadButton.Text = "&Download";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(80, 159);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // overwriteCheckBox
            // 
            this.overwriteCheckBox.AutoSize = true;
            this.overwriteCheckBox.Location = new System.Drawing.Point(12, 136);
            this.overwriteCheckBox.Name = "overwriteCheckBox";
            this.overwriteCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.overwriteCheckBox.Size = new System.Drawing.Size(134, 17);
            this.overwriteCheckBox.TabIndex = 6;
            this.overwriteCheckBox.Text = "Overwrite Existing Files";
            this.overwriteCheckBox.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // destinationfolderBrowserDialog
            // 
            this.destinationfolderBrowserDialog.Description = "Select destination folder for downloaded items";
            // 
            // DownloadForm
            // 
            this.AcceptButton = this.downloadButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(244, 189);
            this.Controls.Add(this.centreOSGridReferenceUserControl);
            this.Controls.Add(this.overwriteCheckBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.downloadButton);
            this.Controls.Add(this.imagesCheckBox);
            this.Controls.Add(this.osmCheckBox);
            this.Controls.Add(this.centreLabel);
            this.Controls.Add(this.heightLabel2);
            this.Controls.Add(this.widthLabel2);
            this.Controls.Add(this.widthTextBox);
            this.Controls.Add(this.heightLabel);
            this.Controls.Add(this.widthLabel);
            this.Controls.Add(this.heightTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(260, 228);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(260, 228);
            this.Name = "DownloadForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Download";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox imagesCheckBox;
        private System.Windows.Forms.CheckBox osmCheckBox;
        private OSGridReferenceUserControl centreOSGridReferenceUserControl;
        private System.Windows.Forms.Label centreLabel;
        private System.Windows.Forms.Label heightLabel;
        private System.Windows.Forms.TextBox widthTextBox;
        private System.Windows.Forms.Label widthLabel;
        private System.Windows.Forms.TextBox heightTextBox;
        private System.Windows.Forms.Label widthLabel2;
        private System.Windows.Forms.Label heightLabel2;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckBox overwriteCheckBox;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.FolderBrowserDialog destinationfolderBrowserDialog;
    }
}