namespace MakeGeoRefBaseMap
{
    partial class MakeGeoRefBaseMapForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.osGrid1TextBox = new System.Windows.Forms.TextBox();
            this.osGrid2TextBox = new System.Windows.Forms.TextBox();
            this.osGrid3TextBox = new System.Windows.Forms.TextBox();
            this.createButton = new System.Windows.Forms.Button();
            this.widthComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lengthComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.ppkmTextBox = new System.Windows.Forms.TextBox();
            this.aboutButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "SW OS Grid Reference";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // osGrid1TextBox
            // 
            this.osGrid1TextBox.Location = new System.Drawing.Point(133, 6);
            this.osGrid1TextBox.MaxLength = 2;
            this.osGrid1TextBox.Name = "osGrid1TextBox";
            this.osGrid1TextBox.Size = new System.Drawing.Size(20, 20);
            this.osGrid1TextBox.TabIndex = 3;
            this.osGrid1TextBox.Text = "SJ";
            this.osGrid1TextBox.Enter += new System.EventHandler(this.ClearError);
            // 
            // osGrid2TextBox
            // 
            this.osGrid2TextBox.Location = new System.Drawing.Point(159, 6);
            this.osGrid2TextBox.MaxLength = 2;
            this.osGrid2TextBox.Name = "osGrid2TextBox";
            this.osGrid2TextBox.Size = new System.Drawing.Size(20, 20);
            this.osGrid2TextBox.TabIndex = 4;
            this.osGrid2TextBox.Enter += new System.EventHandler(this.ClearError);
            // 
            // osGrid3TextBox
            // 
            this.osGrid3TextBox.Location = new System.Drawing.Point(185, 6);
            this.osGrid3TextBox.MaxLength = 2;
            this.osGrid3TextBox.Name = "osGrid3TextBox";
            this.osGrid3TextBox.Size = new System.Drawing.Size(20, 20);
            this.osGrid3TextBox.TabIndex = 5;
            this.osGrid3TextBox.Enter += new System.EventHandler(this.ClearError);
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(133, 112);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(75, 23);
            this.createButton.TabIndex = 99;
            this.createButton.Text = "Download";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // widthComboBox
            // 
            this.widthComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.widthComboBox.DisplayMember = "5";
            this.widthComboBox.FormattingEnabled = true;
            this.widthComboBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.widthComboBox.Location = new System.Drawing.Point(133, 59);
            this.widthComboBox.MaxLength = 1;
            this.widthComboBox.Name = "widthComboBox";
            this.widthComboBox.Size = new System.Drawing.Size(32, 21);
            this.widthComboBox.Sorted = true;
            this.widthComboBox.TabIndex = 10;
            this.widthComboBox.Text = "5";
            this.widthComboBox.ValueMember = "5";
            this.widthComboBox.Enter += new System.EventHandler(this.ClearError);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(69, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Width (km)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lengthComboBox
            // 
            this.lengthComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.lengthComboBox.DisplayMember = "5";
            this.lengthComboBox.FormattingEnabled = true;
            this.lengthComboBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.lengthComboBox.Location = new System.Drawing.Point(133, 32);
            this.lengthComboBox.MaxLength = 1;
            this.lengthComboBox.Name = "lengthComboBox";
            this.lengthComboBox.Size = new System.Drawing.Size(32, 21);
            this.lengthComboBox.Sorted = true;
            this.lengthComboBox.TabIndex = 7;
            this.lengthComboBox.Text = "5";
            this.lengthComboBox.ValueMember = "5";
            this.lengthComboBox.Enter += new System.EventHandler(this.ClearError);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Length (km)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "tif";
            this.saveFileDialog.Filter = "TIFF Files|*.tif";
            this.saveFileDialog.Title = "Save Georef Base Map";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(58, 89);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 100;
            this.label7.Text = "Pixels per km";
            // 
            // ppkmTextBox
            // 
            this.ppkmTextBox.Location = new System.Drawing.Point(133, 86);
            this.ppkmTextBox.MaxLength = 4;
            this.ppkmTextBox.Name = "ppkmTextBox";
            this.ppkmTextBox.Size = new System.Drawing.Size(40, 20);
            this.ppkmTextBox.TabIndex = 98;
            this.ppkmTextBox.Text = "1000";
            this.ppkmTextBox.Enter += new System.EventHandler(this.ClearError);
            // 
            // aboutButton
            // 
            this.aboutButton.BackColor = System.Drawing.Color.Red;
            this.aboutButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.aboutButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.aboutButton.Location = new System.Drawing.Point(12, 112);
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(21, 23);
            this.aboutButton.TabIndex = 101;
            this.aboutButton.TabStop = false;
            this.aboutButton.Text = "?";
            this.aboutButton.UseVisualStyleBackColor = false;
            this.aboutButton.Click += new System.EventHandler(this.aboutButton_Click);
            // 
            // MakeGeoRefBaseMapForm
            // 
            this.AcceptButton = this.createButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 148);
            this.Controls.Add(this.aboutButton);
            this.Controls.Add(this.widthComboBox);
            this.Controls.Add(this.ppkmTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lengthComboBox);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.osGrid2TextBox);
            this.Controls.Add(this.osGrid1TextBox);
            this.Controls.Add(this.osGrid3TextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(228, 174);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(228, 174);
            this.Name = "MakeGeoRefBaseMapForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Make Georef Base Map";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox osGrid1TextBox;
        private System.Windows.Forms.TextBox osGrid2TextBox;
        private System.Windows.Forms.TextBox osGrid3TextBox;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.ComboBox widthComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox lengthComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox ppkmTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button aboutButton;
    }
}

