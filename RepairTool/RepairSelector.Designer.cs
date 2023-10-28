namespace RepairTool
{
    partial class RepairSelector
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
            this.ReInstallVU = new System.Windows.Forms.Button();
            this.UpdateReinstall = new System.Windows.Forms.LinkLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ReInstallVU
            // 
            this.ReInstallVU.Font = new System.Drawing.Font("メイリオ", 14F);
            this.ReInstallVU.Location = new System.Drawing.Point(164, 160);
            this.ReInstallVU.Margin = new System.Windows.Forms.Padding(4);
            this.ReInstallVU.Name = "ReInstallVU";
            this.ReInstallVU.Size = new System.Drawing.Size(200, 75);
            this.ReInstallVU.TabIndex = 0;
            this.ReInstallVU.Text = "VistaUpdaterを\r\n再インストール";
            this.toolTip1.SetToolTip(this.ReInstallVU, "インストール済みのVistaUpdaterを再インストールします。");
            this.ReInstallVU.UseVisualStyleBackColor = true;
            this.ReInstallVU.Click += new System.EventHandler(this.ReInstallVU_Click);
            // 
            // UpdateReinstall
            // 
            this.UpdateReinstall.AutoSize = true;
            this.UpdateReinstall.Location = new System.Drawing.Point(4, 271);
            this.UpdateReinstall.Name = "UpdateReinstall";
            this.UpdateReinstall.Size = new System.Drawing.Size(300, 18);
            this.UpdateReinstall.TabIndex = 1;
            this.UpdateReinstall.TabStop = true;
            this.UpdateReinstall.Text = "VistaUpdaterをアップデートして再インストールする";
            this.toolTip1.SetToolTip(this.UpdateReinstall, "VistaUpdaterをインターネットからダウンロードして、再インストールします。\r\n(アップデートがある場合、ファイルが壊れている場合に推奨)");
            this.UpdateReinstall.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.UpdateReinstall_LinkClicked);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("メイリオ", 14F);
            this.button1.Location = new System.Drawing.Point(164, 77);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(200, 75);
            this.button1.TabIndex = 2;
            this.button1.Text = "PCを元の状態へ戻す";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 289);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "VistaUpdater v*.*.*.*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 307);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "VU Repair Tool v*.*.*.*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("メイリオ", 16F);
            this.label3.Location = new System.Drawing.Point(178, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 33);
            this.label3.TabIndex = 5;
            this.label3.Text = "修復方法を選択";
            // 
            // RepairSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 330);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.UpdateReinstall);
            this.Controls.Add(this.ReInstallVU);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RepairSelector";
            this.Text = "VistaUpdater | 回復ツール - オプションを選択";
            this.Load += new System.EventHandler(this.RepairSelector_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ReInstallVU;
        private System.Windows.Forms.LinkLabel UpdateReinstall;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}