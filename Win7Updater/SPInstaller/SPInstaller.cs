using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Win7Updater.SPInstaller
{
    public partial class SPInstaller : Form
    {
        public SPInstaller()
        {
            InitializeComponent();
        }

        private void SPInstaller_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            try
            {
                if (!System.IO.Directory.Exists("C:\\Program Files\\Win7Updater"))
                {
                    System.IO.Directory.CreateDirectory("C:\\Program Files\\Win7Updater");
                    System.IO.Directory.CreateDirectory("C:\\Program Files\\Win7Updater\\Update");
                }
                else if (!System.IO.Directory.Exists("C:\\Program Files\\Win7Updater\\Update"))
                {
                    System.IO.Directory.CreateDirectory("C:\\Program Files\\Win7Updater\\Update");
                }
            }
            catch (Exception) { }

            WebClient wc1 = new WebClient();
            Uri sp1 = new Uri("http://catalog.s.download.windowsupdate.com/msdownload/update/software/svpk/2011/02/windows6.1-kb976932-x64_74865ef2562006e51d7f9333b4a8d45b7a749dab.exe");
            if ((ushort)new ManagementObject("Win32_Processor.DeviceID='CPU0'")["AddressWidth"] == 32)
            {
                sp1 = new Uri("http://catalog.s.download.windowsupdate.com/msdownload/update/software/svpk/2011/02/windows6.1-kb976932-x86_c3516bc5c9e69fee6d9ac4f981f5b95977a8a2fa.exe");
            }
            wc1.DownloadFileAsync(sp1, "C:\\Program Files\\Win7Updater\\Update\\sp1.exe");
            wc1.DownloadProgressChanged += Wc1_DownloadProgressChanged;
            wc1.DownloadFileCompleted += Wc1_DownloadFileCompleted;

            //Change Download State: Service Pack 1
            downloadStateText.Text = "Service Pack 1 をダウンロードしています...";

            listBox1.Items.Add("Windows 7 Service Pack 1 をダウンロードしています...");


            if (!File.Exists("C:\\Program Files\\Win7Updater\\Application.exe"))
            {
                File.Copy(System.Windows.Forms.Application.ExecutablePath, "C:\\Program Files\\Win7Updater\\Application.exe");
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
            downloadStateText.Text = "Service Pack 1 をダウンロードしています... (" + SizeSuffix(e.BytesReceived) + " / " + SizeSuffix(e.TotalBytesToReceive) + ")";
            downloadState.Value = e.ProgressPercentage;
        }

        private void Wc1_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                listBox1.Items.Add("Windows 7 Service Pack 1 のダウンロードに失敗...");
            } else
            {
                //Change Install State (First only)
                label2.Text = "アップデートをインストール中...";
                listBox1.Items.Add("Windows 7 Service Pack 1 のダウンロードに成功！");
                listBox1.Items.Add("Windows 7 Service Pack 1 をインストールしています...");
                downloadStateText.Text = "完了";

                //Change Install State: Service Pack 1
                installStateText.Text = "Service Pack 1 をインストールしています...";
                installState.Value = 0;

                //Process Launching: Service Pack 1
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "wusa.exe";
                p.StartInfo.Arguments = "\"C:\\Program Files\\Win7Updater\\Update\\sp1.exe\" /quiet /nodialog /norestart";
                p.EnableRaisingEvents = true;
                p.SynchronizingObject = this;
                p.Exited += p_Exited;

                listBox1.Items.Add("コマンドの実行: " + p.StartInfo.FileName + p.StartInfo.Arguments);

                p.Start();
            }
        }

        private void p_Exited(object sender, EventArgs e)
        {
            //Change Install State: Finishing
            installStateText.Text = "最終処理を実行中...";
            installState.Value = 96;

            label2.Text = "ユーザーアカウントの設定中...";
            listBox1.Items.Add("新しい仮ユーザーを作成しています...");
            try
            {
                //Create User: Win7Updater
                DirectoryEntry de = new DirectoryEntry($"WinNT://{Environment.MachineName},computer");
                DirectoryEntry newUser = de.Children.Add("Win7Updater", "user");
                newUser.Invoke("SetPassword", new object[] { "Windows7Updater.KariUser" });
                newUser.CommitChanges();
                DirectoryEntry grp = de.Children.Find("Administrators", "group");
                if (grp != null)
                {
                    grp.Invoke("Add", new object[] { newUser.Path.ToString() });
                }
            }
            catch (Exception ex)
            {
                //Error Handling
                listBox1.Items.Add("ユーザーの作成中にエラーが発生しましたが、問題はないと思われます。");
                listBox1.Items.Add("エラーメッセージ(開発者用): " + ex.Message);
            }
            listBox1.Items.Add("Shellの設定中...");

            //Shell: Enabling Auto Logon, Change Shell
            Microsoft.Win32.RegistryKey regkey =
                Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon");
            regkey.SetValue("Shell", "C:\\Program Files\\Win7Updater\\Application.exe", Microsoft.Win32.RegistryValueKind.String);
            regkey.SetValue("AutoAdminLogon", 1, Microsoft.Win32.RegistryValueKind.DWord);
            regkey.SetValue("DefaultUserName", "Win7Updater", Microsoft.Win32.RegistryValueKind.String);
            regkey.SetValue("DefaultPassword", "Windows7Updater.KariUser", Microsoft.Win32.RegistryValueKind.String);
            regkey.Close();

            //Win7Updater: Service Pack Install State
            Microsoft.Win32.RegistryKey regkey1 =
                Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Win7Updater");
            regkey1.SetValue("SP1Installed", 1, Microsoft.Win32.RegistryValueKind.DWord);
            regkey1.Close();

            //User Account Control: Disabling
            Microsoft.Win32.RegistryKey regkey2 =
                Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
            regkey2.SetValue("EnableLUA", 0, Microsoft.Win32.RegistryValueKind.DWord);
            regkey2.Close();

            listBox1.Items.Add("Shellの設定が完了しました");
            listBox1.Items.Add("再起動中...");
            label2.Text = "再起動しています...";

            //Change Install State: Completed
            installStateText.Text = "完了";
            installState.Value = 100;

            //Process Launching: shutdown
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "shutdown.exe";
            p.StartInfo.Arguments = "/r /t 10 /c \"Win7Updater の処理を続けるため、10秒後に再起動します。\"";
            p.Start();

        }
    }
}
