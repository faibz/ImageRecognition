namespace DistributedSystems.Client
{
    partial class ImageSenderForm
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
            this.directorySearcher1 = new System.DirectoryServices.DirectorySearcher();
            this.selectImageButton = new System.Windows.Forms.Button();
            this.FilePickerDialog = new System.Windows.Forms.OpenFileDialog();
            this.imageTagsPoller = new System.Windows.Forms.Timer(this.components);
            this.tagsLabel = new System.Windows.Forms.Label();
            this.tagDataGrid = new System.Windows.Forms.DataGridView();
            this.tagsPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.tagDataGrid)).BeginInit();
            this.tagsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // directorySearcher1
            // 
            this.directorySearcher1.ClientTimeout = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerPageTimeLimit = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerTimeLimit = System.TimeSpan.Parse("-00:00:01");
            // 
            // selectImageButton
            // 
            this.selectImageButton.Location = new System.Drawing.Point(98, 229);
            this.selectImageButton.Name = "selectImageButton";
            this.selectImageButton.Size = new System.Drawing.Size(128, 23);
            this.selectImageButton.TabIndex = 0;
            this.selectImageButton.Text = "Select an image";
            this.selectImageButton.UseVisualStyleBackColor = true;
            this.selectImageButton.Click += new System.EventHandler(this.SelectImageButton_Click);
            // 
            // FilePickerDialog
            // 
            this.FilePickerDialog.Tag = "";
            // 
            // tagsLabel
            // 
            this.tagsLabel.AutoSize = true;
            this.tagsLabel.Location = new System.Drawing.Point(13, 13);
            this.tagsLabel.Name = "tagsLabel";
            this.tagsLabel.Size = new System.Drawing.Size(278, 13);
            this.tagsLabel.TabIndex = 1;
            this.tagsLabel.Text = "Retrieving tags... They will show below as they are found.";
            this.tagsLabel.Visible = false;
            // 
            // tagDataGrid
            // 
            this.tagDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tagDataGrid.Location = new System.Drawing.Point(3, 3);
            this.tagDataGrid.Name = "tagDataGrid";
            this.tagDataGrid.Size = new System.Drawing.Size(269, 439);
            this.tagDataGrid.TabIndex = 4;
            // 
            // tagsPanel
            // 
            this.tagsPanel.Controls.Add(this.tagDataGrid);
            this.tagsPanel.Location = new System.Drawing.Point(16, 29);
            this.tagsPanel.Name = "tagsPanel";
            this.tagsPanel.Size = new System.Drawing.Size(275, 445);
            this.tagsPanel.TabIndex = 3;
            this.tagsPanel.Visible = false;
            // 
            // ImageSenderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 485);
            this.Controls.Add(this.selectImageButton);
            this.Controls.Add(this.tagsPanel);
            this.Controls.Add(this.tagsLabel);
            this.Name = "ImageSenderForm";
            this.Text = "Test Client";
            ((System.ComponentModel.ISupportInitialize)(this.tagDataGrid)).EndInit();
            this.tagsPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.DirectoryServices.DirectorySearcher directorySearcher1;
        private System.Windows.Forms.Button selectImageButton;
        private System.Windows.Forms.OpenFileDialog FilePickerDialog;
        private System.Windows.Forms.Timer imageTagsPoller;
        private System.Windows.Forms.Label tagsLabel;
        private System.Windows.Forms.DataGridView tagDataGrid;
        private System.Windows.Forms.Panel tagsPanel;
    }
}

