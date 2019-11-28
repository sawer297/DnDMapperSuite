namespace DnDMapper
{
    partial class TopMenu
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
            this.pnlLogo = new System.Windows.Forms.Panel();
            this.pnlBtns = new System.Windows.Forms.Panel();
            this.btnSessions = new System.Windows.Forms.Button();
            this.btnMaps = new System.Windows.Forms.Button();
            this.pnlLogo.SuspendLayout();
            this.pnlBtns.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLogo
            // 
            this.pnlLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.pnlLogo.Controls.Add(this.pnlBtns);
            this.pnlLogo.Location = new System.Drawing.Point(3, 3);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(905, 763);
            this.pnlLogo.TabIndex = 0;
            // 
            // pnlBtns
            // 
            this.pnlBtns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlBtns.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBtns.Controls.Add(this.btnSessions);
            this.pnlBtns.Controls.Add(this.btnMaps);
            this.pnlBtns.Location = new System.Drawing.Point(0, 383);
            this.pnlBtns.Name = "pnlBtns";
            this.pnlBtns.Size = new System.Drawing.Size(451, 380);
            this.pnlBtns.TabIndex = 0;
            // 
            // btnSessions
            // 
            this.btnSessions.Font = new System.Drawing.Font("Lucida Console", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSessions.ForeColor = System.Drawing.Color.Red;
            this.btnSessions.Location = new System.Drawing.Point(3, 192);
            this.btnSessions.Name = "btnSessions";
            this.btnSessions.Size = new System.Drawing.Size(445, 185);
            this.btnSessions.TabIndex = 1;
            this.btnSessions.Text = "Sessions";
            this.btnSessions.UseVisualStyleBackColor = true;
            // 
            // btnMaps
            // 
            this.btnMaps.Font = new System.Drawing.Font("Lucida Console", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMaps.Location = new System.Drawing.Point(3, 3);
            this.btnMaps.Name = "btnMaps";
            this.btnMaps.Size = new System.Drawing.Size(445, 185);
            this.btnMaps.TabIndex = 0;
            this.btnMaps.Text = "Maps";
            this.btnMaps.UseVisualStyleBackColor = true;
            this.btnMaps.Click += new System.EventHandler(this.btnMaps_Click);
            // 
            // TopMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlLogo);
            this.Name = "TopMenu";
            this.Size = new System.Drawing.Size(911, 769);
            this.Resize += new System.EventHandler(this.TopMenu_Resize);
            this.pnlLogo.ResumeLayout(false);
            this.pnlBtns.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.Panel pnlBtns;
        private System.Windows.Forms.Button btnSessions;
        private System.Windows.Forms.Button btnMaps;
    }
}
