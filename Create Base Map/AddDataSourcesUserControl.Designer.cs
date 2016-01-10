namespace CreateBaseMap
{
    partial class AddDataSourcesUserControl
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
            this.saveButton = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.previousButton = new System.Windows.Forms.Button();
            this.FileListBox = new System.Windows.Forms.ListBox();
            this.fileListContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addLocalFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadAddFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveOcadFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.fileListContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(142, 312);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 19;
            this.saveButton.Text = "&Save >";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // previousButton
            // 
            this.previousButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.previousButton.CausesValidation = false;
            this.previousButton.Location = new System.Drawing.Point(61, 312);
            this.previousButton.Name = "previousButton";
            this.previousButton.Size = new System.Drawing.Size(75, 23);
            this.previousButton.TabIndex = 18;
            this.previousButton.Text = "&Previous <";
            this.previousButton.UseVisualStyleBackColor = true;
            this.previousButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.previousButton_MouseClick);
            // 
            // FileListBox
            // 
            this.FileListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileListBox.ContextMenuStrip = this.fileListContextMenuStrip;
            this.FileListBox.FormattingEnabled = true;
            this.FileListBox.Location = new System.Drawing.Point(3, 3);
            this.FileListBox.Name = "FileListBox";
            this.FileListBox.ScrollAlwaysVisible = true;
            this.FileListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.FileListBox.Size = new System.Drawing.Size(214, 303);
            this.FileListBox.Sorted = true;
            this.FileListBox.TabIndex = 20;
            // 
            // fileListContextMenuStrip
            // 
            this.fileListContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLocalFilesToolStripMenuItem,
            this.downloadAddFilesToolStripMenuItem,
            this.removeFilesToolStripMenuItem});
            this.fileListContextMenuStrip.Name = "fileListContextMenuStrip";
            this.fileListContextMenuStrip.Size = new System.Drawing.Size(202, 70);
            this.fileListContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.fileListContextMenuStrip_Opening);
            // 
            // addLocalFilesToolStripMenuItem
            // 
            this.addLocalFilesToolStripMenuItem.Name = "addLocalFilesToolStripMenuItem";
            this.addLocalFilesToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.addLocalFilesToolStripMenuItem.Text = "Add Local Files...";
            this.addLocalFilesToolStripMenuItem.Click += new System.EventHandler(this.addLocalFilesToolStripMenuItem_Click);
            // 
            // downloadAddFilesToolStripMenuItem
            // 
            this.downloadAddFilesToolStripMenuItem.Name = "downloadAddFilesToolStripMenuItem";
            this.downloadAddFilesToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.downloadAddFilesToolStripMenuItem.Text = "Download && Add Files...";
            this.downloadAddFilesToolStripMenuItem.Click += new System.EventHandler(this.downloadAddFilesToolStripMenuItem_Click);
            // 
            // removeFilesToolStripMenuItem
            // 
            this.removeFilesToolStripMenuItem.Name = "removeFilesToolStripMenuItem";
            this.removeFilesToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.removeFilesToolStripMenuItem.Text = "Remove Files";
            this.removeFilesToolStripMenuItem.Click += new System.EventHandler(this.removeFilesToolStripMenuItem_Click);
            // 
            // selectFileDialog
            // 
            this.selectFileDialog.DefaultExt = "*.osm";
            this.selectFileDialog.Filter = "Common files|*.osm;*.gml;*.ocd;*.tif|OSM files|*.osm|OS GML files|*.gml|OCAD9 files|*" +
    ".ocd|GeoTiff files|*.tif|All files|*.*";
            this.selectFileDialog.Multiselect = true;
            this.selectFileDialog.Title = "Select OSM, OS, OCAD9 or GeoTiff Files";
            // 
            // saveOcadFileDialog
            // 
            this.saveOcadFileDialog.DefaultExt = "*.ocd";
            this.saveOcadFileDialog.Filter = "OCAD9 files|*.ocd";
            this.saveOcadFileDialog.Title = "Save OCAD9 File";
            // 
            // AddDataSourcesUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.FileListBox);
            this.Controls.Add(this.previousButton);
            this.Controls.Add(this.saveButton);
            this.Name = "AddDataSourcesUserControl";
            this.Size = new System.Drawing.Size(220, 338);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.fileListContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider;
        internal System.Windows.Forms.Button saveButton;
        internal System.Windows.Forms.Button previousButton;
        private System.Windows.Forms.ContextMenuStrip fileListContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addLocalFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadAddFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFilesToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog selectFileDialog;
        internal System.Windows.Forms.ListBox FileListBox;
        private System.Windows.Forms.SaveFileDialog saveOcadFileDialog;
    }
}
