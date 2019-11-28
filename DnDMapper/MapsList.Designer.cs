namespace DnDMapper
{
    partial class MapsList
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
            this.lstBxMaps = new System.Windows.Forms.ListBox();
            this.pnlMapInfo = new System.Windows.Forms.Panel();
            this.btnDelMap = new System.Windows.Forms.Button();
            this.btnEditMap = new System.Windows.Forms.Button();
            this.btnNewMap = new System.Windows.Forms.Button();
            this.picBxPreview = new System.Windows.Forms.PictureBox();
            this.pnlMapInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBxPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // lstBxMaps
            // 
            this.lstBxMaps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstBxMaps.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstBxMaps.FormattingEnabled = true;
            this.lstBxMaps.ItemHeight = 20;
            this.lstBxMaps.Items.AddRange(new object[] {
            "map 1",
            "map 2"});
            this.lstBxMaps.Location = new System.Drawing.Point(3, 3);
            this.lstBxMaps.Margin = new System.Windows.Forms.Padding(2);
            this.lstBxMaps.Name = "lstBxMaps";
            this.lstBxMaps.Size = new System.Drawing.Size(417, 784);
            this.lstBxMaps.TabIndex = 0;
            // 
            // pnlMapInfo
            // 
            this.pnlMapInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMapInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.pnlMapInfo.Controls.Add(this.btnDelMap);
            this.pnlMapInfo.Controls.Add(this.btnEditMap);
            this.pnlMapInfo.Controls.Add(this.btnNewMap);
            this.pnlMapInfo.Controls.Add(this.picBxPreview);
            this.pnlMapInfo.Location = new System.Drawing.Point(383, 3);
            this.pnlMapInfo.Margin = new System.Windows.Forms.Padding(2);
            this.pnlMapInfo.Name = "pnlMapInfo";
            this.pnlMapInfo.Size = new System.Drawing.Size(414, 789);
            this.pnlMapInfo.TabIndex = 1;
            // 
            // btnDelMap
            // 
            this.btnDelMap.Font = new System.Drawing.Font("Lucida Console", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelMap.Location = new System.Drawing.Point(3, 561);
            this.btnDelMap.Name = "btnDelMap";
            this.btnDelMap.Size = new System.Drawing.Size(260, 70);
            this.btnDelMap.TabIndex = 3;
            this.btnDelMap.Text = "Delete Map";
            this.btnDelMap.UseVisualStyleBackColor = true;
            // 
            // btnEditMap
            // 
            this.btnEditMap.Font = new System.Drawing.Font("Lucida Console", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditMap.Location = new System.Drawing.Point(3, 485);
            this.btnEditMap.Name = "btnEditMap";
            this.btnEditMap.Size = new System.Drawing.Size(260, 70);
            this.btnEditMap.TabIndex = 2;
            this.btnEditMap.Text = "Edit Map";
            this.btnEditMap.UseVisualStyleBackColor = true;
            // 
            // btnNewMap
            // 
            this.btnNewMap.Font = new System.Drawing.Font("Lucida Console", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewMap.Location = new System.Drawing.Point(3, 409);
            this.btnNewMap.Name = "btnNewMap";
            this.btnNewMap.Size = new System.Drawing.Size(260, 70);
            this.btnNewMap.TabIndex = 1;
            this.btnNewMap.Text = "New Map";
            this.btnNewMap.UseVisualStyleBackColor = true;
            this.btnNewMap.Click += new System.EventHandler(this.btnNewMap_Click);
            // 
            // picBxPreview
            // 
            this.picBxPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.picBxPreview.Location = new System.Drawing.Point(3, 3);
            this.picBxPreview.Name = "picBxPreview";
            this.picBxPreview.Size = new System.Drawing.Size(408, 400);
            this.picBxPreview.TabIndex = 0;
            this.picBxPreview.TabStop = false;
            // 
            // MapsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMapInfo);
            this.Controls.Add(this.lstBxMaps);
            this.Name = "MapsList";
            this.Size = new System.Drawing.Size(800, 800);
            this.Resize += new System.EventHandler(this.MapsList_Resize);
            this.pnlMapInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBxPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstBxMaps;
        private System.Windows.Forms.Panel pnlMapInfo;
        private System.Windows.Forms.PictureBox picBxPreview;
        private System.Windows.Forms.Button btnNewMap;
        private System.Windows.Forms.Button btnDelMap;
        private System.Windows.Forms.Button btnEditMap;
    }
}
