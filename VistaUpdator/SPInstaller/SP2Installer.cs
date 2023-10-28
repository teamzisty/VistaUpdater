using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace VistaUpdater.SPInstaller
{
    public partial class SP2Installer : Form
    {
        public SP2Installer()
        {
            InitializeComponent();
        }

        private void SP2Installer_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            if (!Directory.Exists("C:\\Program Files\\VistaUpdater"))
            {
                System.IO.Directory.CreateDirectory("C:\\Program Files\\VistaUpdater");
                System.IO.Directory.CreateDirectory("C:\\Program Files\\VistaUpdater\\Update");
            }

            WebClient wc1 = new WebClient();
            Uri sp1 = new Uri("http://catalog.s.download.windowsupdate.com/msdownload/update/software/svpk/2009/06/windows6.0-kb948465-x64_2eedca0bfa5ae8d1b0acf2117ddc4f15ac5183c9.exe");
            wc1.DownloadFileAsync(sp1, "C:\\Program Files\\VistaUpdater\\Update\\sp2.exe");
            wc1.DownloadFileCompleted += Wc1_DownloadFileCompleted;
            wc1.DownloadProgressChanged += Wc1_DownloadProgressChanged;
            listBox1.Items.Add("Windows Vista Service Pack 2 をダウンロードしています...");
            downloadStateText.Text = "Service Pack 2 をダウンロードしています...";

            try
            {
                if (!File.Exists("C:\\Program Files\\VistaUpdater\\Application.exe"))
                {
                    File.Copy(System.Windows.Forms.Application.ExecutablePath, "C:\\Program Files\\VistaUpdater\\Application.exe");
                }
                else
                {
                    File.Delete("C:\\Program Files\\VistaUpdater\\Application.exe");
                    File.Copy(System.Windows.Forms.Application.ExecutablePath, "C:\\Program Files\\VistaUpdater\\Application.exe");
                }

                if (!File.Exists("C:\\Program Files\\VistaUpdater\\DotNetZip.dll"))
                {
                    File.Copy(Directory.GetCurrentDirectory() + "\\DotNetZip.dll", "C:\\Program Files\\VistaUpdater\\DotNetZip.dll");
                }
                else
                {
                    File.Delete("C:\\Program Files\\VistaUpdater\\DotNetZip.dll");
                    File.Copy(Directory.GetCurrentDirectory() + "\\DotNetZip.dll", "C:\\Program Files\\VistaUpdater\\DotNetZip.dll");
                }
            } catch (Exception ex)
            {

            }
        }

        static readonly string[] SizeSuffixes =
           { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }

        private void Wc1_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            downloadStateText.Text = "Service Pack 2 をダウンロードしています... (" + SizeSuffix(e.BytesReceived) + " / " + SizeSuffix(e.TotalBytesToReceive) + ")";
            downloadState.Value = e.ProgressPercentage;
        }

        private void ChangeDownloadState(string text, int value)
        {
            downloadStateText.Text = text;
            listBox1.Items.Add(text);

            downloadState.Value = value;

        }

        private void ChangeInstallState(string text, int value)
        {
            installStateText.Text = text;
            listBox1.Items.Add(text);

            installState.Value = value;

        }

        private void Wc1_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            downloadStateText.Text = "完了";
            ChangeInstallState("Service Pack 2 をインストールしています...", 50);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "C:\\Program Files\\VistaUpdater\\Update\\sp2.exe";
            process.StartInfo.Arguments = "/quiet /nodialog /norestart";
            process.EnableRaisingEvents = true;
            process.SynchronizingObject = this;
            process.Exited += Process_Exited;
            listBox1.Items.Add("コマンドの実行: " + process.StartInfo.Arguments);
            process.Start();
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            ChangeInstallState("最終処理を実行中...", 96);
            label2.Text = "ユーザーアカウントの設定中...";
            listBox1.Items.Add("新しい仮ユーザーを作成しています...");
            try
            {
                DirectoryEntry de = new DirectoryEntry($"WinNT://{Environment.MachineName},computer");
                DirectoryEntry newUser = de.Children.Add("VistaUpdater", "user");
                newUser.Invoke("SetPassword", new object[] { "WindowsVistaUpdater.KariUser" });
                newUser.CommitChanges();
                DirectoryEntry grp = de.Children.Find("Administrators", "group");
                if (grp != null)
                {
                    grp.Invoke("Add", new object[] { newUser.Path.ToString() });
                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add("ユーザーの作成中にエラーが発生しましたが、問題はないと思われます。");
                listBox1.Items.Add("エラーメッセージ(開発者用): " + ex.Message);
            }
            listBox1.Items.Add("Shellの設定中...");
            Microsoft.Win32.RegistryKey regkey =
                Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon");
            regkey.SetValue("Shell", "C:\\Program Files\\VistaUpdater\\Application.exe", Microsoft.Win32.RegistryValueKind.String);
            regkey.SetValue("AutoAdminLogon", 1, Microsoft.Win32.RegistryValueKind.DWord);
            regkey.SetValue("DefaultUserName", "VistaUpdater", Microsoft.Win32.RegistryValueKind.String);
            regkey.SetValue("DefaultPassword", "WindowsVistaUpdater.KariUser", Microsoft.Win32.RegistryValueKind.String);
            regkey.Close();
            Microsoft.Win32.RegistryKey regkey1 =
                Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\VistaUpdater");
            regkey1.DeleteValue("SP1Installed");
            regkey1.SetValue("SP2Installed", 1, Microsoft.Win32.RegistryValueKind.QWord);
            regkey1.Close();
            Microsoft.Win32.RegistryKey regkey2 =
                Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
            regkey2.SetValue("EnableLUA", 0, Microsoft.Win32.RegistryValueKind.DWord);
            regkey2.Close();
            listBox1.Items.Add("Shellの設定が完了しました");
            listBox1.Items.Add("再起動中...");
            label2.Text = "再起動しています...";
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "shutdown.exe";
            p.StartInfo.Arguments = "/r /t 10 /c \"VistaUpdater の処理を続けるため、10秒後に再起動します。\"";
            p.Start();
            ChangeInstallState("完了", 100);
        }
    }
}
