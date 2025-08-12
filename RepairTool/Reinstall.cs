using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace RepairTool
{
    public partial class Reinstall : Form
    {
        public static bool dlRecovery = false;
        public static string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\VistaUpdater-Recovery\\" + "VistaUpdater.exe";
        public Reinstall(bool downloading = false)
        {
            dlRecovery = downloading;

            InitializeComponent();
        }

        private void Reinstall_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            if (!dlRecovery)
            {
                listBox1.Items.Add("VistaUpdater を実行しています...");
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "C:\\Program Files\\VistaUpdater\\Application.exe";
                listBox1.Items.Add("コマンド: " + p.StartInfo.FileName);
                p.Start();
                listBox1.Items.Add("VistaUpdater 回復ツールのプロセスを完了しました。");
            } else
            {
                WebClient webClient = new WebClient();
                Uri uri = new Uri("http://vistaupdater.net/tools/VistaUpdater.exe");
                webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
                webClient.DownloadFileAsync(uri, filePath);
            }
        }

        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                listBox1.Items.Add("VistaUpdater を実行しています...");
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\VistaUpdater-Recovery\\" + "VistaUpdater.exe";
                listBox1.Items.Add("コマンド: " + p.StartInfo.FileName);
                p.Start();
                listBox1.Items.Add("VistaUpdater 回復ツールのプロセスを完了しました。");
            }
            catch (Exception)
            {
                listBox1.Items.Add("VistaUpdater 回復ツールのプロセスを完了できませんでした。");
            }
        }
    }
}
