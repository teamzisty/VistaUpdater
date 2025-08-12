using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Management;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace VistaUpdater
{
    public partial class RestartedUpdate : Form
    {
        public RestartedUpdate()
        {
            InitializeComponent();
        }

        private void RestartedUpdate_Load(object sender, EventArgs e)
        {
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo("C:\\Program Files\\Internet Explorer\\iexplore.exe");
            if (myFileVersionInfo.FileVersion.Contains("9.0"))
            {
                GoNext();
            }
            else
            {
                label2.Text = "Internet Explorer 9 をインストールしています...";
                Uri ie9 = new Uri("http://catalog.s.download.windowsupdate.com/msdownload/update/software/uprl/2011/03/wu-ie9-windowsvista-x64_f599c02e7e1ea8a4e1029f0e49418a8be8416367.exe");
                if ((ushort)new ManagementObject("Win32_Processor.DeviceID='CPU0'")["AddressWidth"] == 32)
                {
                    ie9 = new Uri("http://catalog.s.download.windowsupdate.com/msdownload/update/software/uprl/2011/05/wu-ie9-windowsvista-x86_90e3e90e964c2769a008cbf924eefdc42413dd52.exe");
                }
                WebClient wc = new WebClient();
                wc.DownloadFileAsync(ie9, "C:\\Program Files\\VistaUpdater\\Update\\ie9.exe");
                ChangeDownloadState("Windows Vista 用 Windows Internet Explorer 9 をダウンロードしています...", 25);
                wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
            }
        }
        private void ChangeDownloadState(string text, int value)
        {
            listBox1.Items.Add(text);
        }

        private void ChangeInstallState(string text, int value)
        {
            installStateText.Text = text;
            listBox1.Items.Add(text);

            installState.Value = value;

        }


        private void Wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            ChangeDownloadState("Windows Vista 用 Windows Internet Explorer 9 をダウンロードしました", 50);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "C:\\Program Files\\VistaUpdater\\Update\\ie9.exe";
            process.StartInfo.Arguments = "/quiet /norestart";
            process.EnableRaisingEvents = true;
            process.SynchronizingObject = this;
            process.Exited += Process_Exited;
            ChangeInstallState("Windows Vista 用 Windows Internet Explorer 9 をインストールしています...", 25);
            listBox1.Items.Add("コマンドの実行: " + process.StartInfo.FileName + " " + process.StartInfo.Arguments);
            process.Start();
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            listBox1.Items.Add("Windows Vista 用 Windows Internet Explorer 9 をインストールしました");

            Uri ie9lp = new Uri("http://catalog.s.download.windowsupdate.com/msdownload/update/software/updt/2011/03/ie9-langpack-windowsvista-x64-jpn_331d32d2b458301c359cb95b639425ff2dbaf2a1.exe");
            if ((ushort)new ManagementObject("Win32_Processor.DeviceID='CPU0'")["AddressWidth"] == 32)
            {
                ie9lp = new Uri("http://catalog.s.download.windowsupdate.com/msdownload/update/software/updt/2011/03/ie9-langpack-windowsvista-x86-jpn_8400e4ecbbb1fb4b40e37ec852f2324e7da0adda.exe");
            }
            WebClient wc = new WebClient();
            wc.DownloadFileAsync(ie9lp, "C:\\Program Files\\VistaUpdater\\Update\\ie9lp.exe");
            ChangeDownloadState("Windows Vista 用 Windows Internet Explorer 9 言語パックをダウンロードしています...", 25);
            wc.DownloadFileCompleted += Wc3_DownloadFileCompleted;
        }

        private void Wc3_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            ChangeDownloadState("Windows Vista 用 Windows Internet Explorer 9 言語パック をダウンロードしました", 50);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "C:\\Program Files\\VistaUpdater\\Update\\ie9lp.exe";
            process.StartInfo.Arguments = "/quiet /norestart";
            process.EnableRaisingEvents = true;
            process.SynchronizingObject = this;
            process.Exited += Process3_Exited;
            ChangeInstallState("Windows Vista 用 Windows Internet Explorer 9 言語パック をインストールしています...", 25);
            listBox1.Items.Add("コマンドの実行: " + process.StartInfo.FileName + " " + process.StartInfo.Arguments);
            process.Start();
        }

        private void Process3_Exited(object sender, EventArgs e)
        {
            GoNext();
        }

        private void GoNext()
        {
            label2.Text = "更新プログラムをインストールしています...";

            Uri kb4014661 = new Uri("http://catalog.s.download.windowsupdate.com/d/msdownload/update/software/secu/2017/04/ie9-windows6.0-kb4014661-x64_b0fd6aaf25578ac22e771e9420fb691845cc3c90.msu");
            if ((ushort)new ManagementObject("Win32_Processor.DeviceID='CPU0'")["AddressWidth"] == 32)
            {
                kb4014661 = new Uri("http://catalog.s.download.windowsupdate.com/d/msdownload/update/software/secu/2017/04/ie9-windows6.0-kb4014661-x86_41c4885409c1e7712495cda5067389e9d58be7e7.msu");
            }
            WebClient wc = new WebClient();
            wc.DownloadFileAsync(kb4014661, "C:\\Program Files\\VistaUpdater\\Update\\kb4014661.msu");
            ChangeDownloadState("Windows Vista 用 Internet Explorer 9 の累積的なセキュリティ更新プログラム (KB4014661) をダウンロードしています...", 75);
            wc.DownloadFileCompleted += Wc2_DownloadFileCompleted;
        }

        private void Wc2_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            ChangeDownloadState("Windows Vista 用 Internet Explorer 9 の累積的なセキュリティ更新プログラム (KB4014661) をダウンロードしました", 100);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.Arguments = "\"C:\\Program Files\\VistaUpdater\\Update\\kb4014661.msu\" /quiet /norestart";
            process.EnableRaisingEvents = true;
            process.SynchronizingObject = this;
            process.StartInfo.FileName = "wusa.exe";
            process.Exited += Process2_Exited;
            ChangeInstallState("Windows Vista 用 Internet Explorer 9 の累積的なセキュリティ更新プログラム (KB4014661) をインストールしています...", 75);
            listBox1.Items.Add("コマンドの実行: " + process.StartInfo.FileName + " " + process.StartInfo.Arguments);
            process.Start();
        }

        private void Process2_Exited(object sender, EventArgs e)
        {
            ChangeInstallState("Windows Vista 用 Internet Explorer 9 の累積的なセキュリティ更新プログラム (KB4014661) をインストールしました", 95);
            ChangeInstallState("最終処理を実行中...", 95);
            label2.Text = "最終処理の実行中...";
            listBox1.Items.Add("Shellの設定中...");
            Microsoft.Win32.RegistryKey regkey1 =
                Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\VistaUpdater");
            regkey1.SetValue("Restarted", 2, Microsoft.Win32.RegistryValueKind.QWord);
            regkey1.Close();
            listBox1.Items.Add("Shellの設定が完了しました");
            listBox1.Items.Add("再起動中...");
            label2.Text = "再起動しています...";
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "shutdown.exe";
            p.StartInfo.Arguments = "/r /t 10 /c \"VistaUpdater の処理を続行するため、10秒後に再起動します。\"";
            p.Start();
            ChangeInstallState("完了", 100);
        }

        private void RestartedUpdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
