namespace DnDMapper
{
    partial class Form1
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
            this.mapPane = new DnDMapper.MapPane();
            this.mapsList = new DnDMapper.MapsList();
            this.topMenu = new DnDMapper.TopMenu();
            this.SuspendLayout();
            // 
            // mapPane
            // 
            this.mapPane.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mapPane.Location = new System.Drawing.Point(70, 59);
            this.mapPane.Name = "mapPane";
            this.mapPane.Size = new System.Drawing.Size(500, 449);
            this.mapPane.TabIndex = 2;
            // 
            // mapsList
            // 
            this.mapsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mapsList.Location = new System.Drawing.Point(36, 28);
            this.mapsList.Name = "mapsList";
            this.mapsList.Size = new System.Drawing.Size(492, 458);
            this.mapsList.TabIndex = 1;
            // 
            // topMenu
            // 
            this.topMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topMenu.Location = new System.Drawing.Point(9, 9);
            this.topMenu.Margin = new System.Windows.Forms.Padding(0);
            this.topMenu.Name = "topMenu";
            this.topMenu.Size = new System.Drawing.Size(486, 443);
            this.topMenu.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(877, 779);
            this.Controls.Add(this.mapPane);
            this.Controls.Add(this.mapsList);
            this.Controls.Add(this.topMenu);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private TopMenu topMenu;
        private MapsList mapsList;
        private MapPane mapPane;
    }
}

