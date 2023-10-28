namespace Win7Updater.SPInstaller
{
    partial class SPInstaller
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPInstaller));
            this.installStateText = new System.Windows.Forms.Label();
            this.installState = new System.Windows.Forms.ProgressBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.downloadStateText = new System.Windows.Forms.Label();
            this.downloadState = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // installStateText
            // 
            this.installStateText.AutoSize = true;
            this.installStateText.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.installStateText.Location = new System.Drawing.Point(12, 242);
            this.installStateText.Name = "installStateText";
            this.installStateText.Size = new System.Drawing.Size(63, 20);
            this.installStateText.TabIndex = 27;
            this.installStateText.Text = "準備中...";
            // 
            // installState
            // 
            this.installState.Location = new System.Drawing.Point(9, 267);
            this.installState.Name = "installState";
            this.installState.Size = new System.Drawing.Size(493, 33);
            this.installState.TabIndex = 26;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(520, 92);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 25;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("メイリオ", 10.75F);
            this.label2.Location = new System.Drawing.Point(5, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(235, 23);
            this.label2.TabIndex = 24;
            this.label2.Text = "アップデートをダウンロード中...";
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("メイリオ", 10F);
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(8, 308);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(493, 184);
            this.listBox1.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("メイリオ", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(3, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(245, 31);
            this.label1.TabIndex = 22;
            this.label1.Text = "処理を実行しています...";
            // 
            // downloadStateText
            // 
            this.downloadStateText.AutoSize = true;
            this.downloadStateText.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.downloadStateText.Location = new System.Drawing.Point(12, 172);
            this.downloadStateText.Name = "downloadStateText";
            this.downloadStateText.Size = new System.Drawing.Size(63, 20);
            this.downloadStateText.TabIndex = 29;
            this.downloadStateText.Text = "準備中...";
            // 
            // downloadState
            // 
            this.downloadState.Location = new System.Drawing.Point(8, 195);
            this.downloadState.Name = "downloadState";
            this.downloadState.Size = new System.Drawing.Size(493, 33);
            this.downloadState.TabIndex = 28;
            // 
            // SPInstaller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 504);
            this.ControlBox = false;
            this.Controls.Add(this.downloadStateText);
            this.Controls.Add(this.downloadState);
            this.Controls.Add(this.installStateText);
            this.Controls.Add(this.installState);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SPInstaller";
            this.Text = "Win7Updater - Service Pack Installer - インストール中...";
            this.Load += new System.EventHandler(this.SPInstaller_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label installStateText;
        private System.Windows.Forms.ProgressBar installState;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label downloadStateText;
        private System.Windows.Forms.ProgressBar downloadState;
    }
}