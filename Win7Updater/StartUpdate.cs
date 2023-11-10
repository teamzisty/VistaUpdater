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

namespace Win7Updater
{
    public partial class StartUpdate : Form
    {
        bool u1ended = false;
        bool u2ended = false;
        bool u3ended = false;
        public StartUpdate()
        {
            InitializeComponent();
        }

        Timer timer = new Timer();

        private void StartUpdate_Load(object sender, EventArgs e)
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
            } catch ( Exception) { }

            WebClient wc1 = new WebClient();
            WebClient wc2 = new WebClient();
            WebClient wc3 = new WebClient();
            Uri kb3133977 = new Uri("http://catalog.s.download.windowsupdate.com/c/msdownload/update/software/updt/2016/03/windows6.1-kb3133977-x64_7c11a96b02a1800067ce6772f6a316021cac2bfb.msu");
            Uri kb4490628 = new Uri("http://catalog.s.download.windowsupdate.com/c/msdownload/update/software/secu/2019/03/windows6.1-kb4490628-x64_d3de52d6987f7c8bdc2c015dca69eac96047c76e.msu");
            Uri kb4474419 = new Uri("http://catalog.s.download.windowsupdate.com/c/msdownload/update/software/secu/2019/09/windows6.1-kb4474419-v3-x64_b5614c6cea5cb4e198717789633dca16308ef79c.msu");
            if ((ushort)new ManagementObject("Win32_Processor.DeviceID='CPU0'")["AddressWidth"] == 32)
            {
                kb3133977 = new Uri("http://catalog.s.download.windowsupdate.com/c/msdownload/update/software/updt/2016/03/windows6.1-kb3133977-x86_077d2fdbee21af06c2d89fe594e653e8bab6da7e.msu");
                kb4490628 = new Uri("http://catalog.s.download.windowsupdate.com/c/msdownload/update/software/secu/2019/03/windows6.1-kb4490628-x86_3cdb3df55b9cd7ef7fcb24fc4e237ea287ad0992.msu");
                kb4474419 = new Uri("http://catalog.s.download.windowsupdate.com/c/msdownload/update/software/secu/2019/09/windows6.1-kb4474419-v3-x86_0f687d50402790f340087c576886501b3223bec6.msu");
            }

            wc1.DownloadFileAsync(kb3133977, "C:\\Program Files\\Win7Updater\\Update\\kb3133977.msu");
            wc1.DownloadFileCompleted += Wc1_DownloadFileCompleted;
            listBox1.Items.Add("Windows 7 用更新プログラム (KB3133977) をダウンロードしています...");
            wc2.DownloadFileAsync(kb4490628, "C:\\Program Files\\Win7Updater\\Update\\kb4490628.msu");
            wc2.DownloadFileCompleted += Wc2_DownloadFileCompleted;
            listBox1.Items.Add("2019-03 Windows 7 サービス スタック更新プログラム (KB4490628) をダウンロードしています...");
            wc3.DownloadFileAsync(kb4474419, "C:\\Program Files\\Win7Updater\\Update\\kb4474419.msu");
            wc3.DownloadFileCompleted += Wc3_DownloadFileCompleted;
            listBox1.Items.Add("2019-09 Windows 7 のセキュリティ更新プログラム (KB4474419) をダウンロードしています...");

            if (!File.Exists("C:\\Program Files\\Win7Updater\\Application.exe"))
            {
                File.Copy(System.Windows.Forms.Application.ExecutablePath, "C:\\Program Files\\Win7Updater\\Application.exe");
            }

            timer.Tick += Timer_Tick;
            timer.Interval = 100;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (u1ended & u2ended & u3ended)
            {
                label2.Text = "アップデートをインストール中...";
                timer.Stop();

                listBox1.Items.Add("ダウンロードに成功しました！");
                listBox1.Items.Add("インストールを開始...");

                //Process Launching: KB3133977
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.Arguments = "\"C:\\Program Files\\Win7Updater\\Update\\kb3133977.msu\" /quiet /norestart";
                p.EnableRaisingEvents = true;
                p.SynchronizingObject = this;
                p.StartInfo.FileName = "wusa.exe";
                p.Exited += p_Exited;

                //Change Install State: KB3133977
                installStateText.Text = "Windows 7 用更新プログラム (KB3133977) をインストールしています...";
                installState.Value = 0;

                listBox1.Items.Add("Windows 7 用更新プログラム (KB3133977) をインストールしています...");
                listBox1.Items.Add("コマンドの実行: " + p.StartInfo.Arguments);
                p.Start();
            }
        }

        private void p_Exited(object sender, EventArgs e)
        {
            listBox1.Items.Add("Windows 7 用更新プログラム (KB3133977) のインストールが完了しました");

            //Process Launching: KB4490628
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.Arguments = "\"C:\\Program Files\\Win7Updater\\Update\\kb4490628.msu\" /quiet /norestart";
            p.EnableRaisingEvents = true;
            p.SynchronizingObject = this;
            p.StartInfo.FileName = "wusa.exe";
            p.Exited += P2_Exited;

            //Change Install State: KB4490628
            installStateText.Text = "Windows 7 サービス スタック更新プログラム (KB4490628) をインストールしています...";
            installState.Value = 33;

            listBox1.Items.Add("2019-03x64 Windows 7 サービス スタック更新プログラム (KB4490628) をインストールしています...");
            listBox1.Items.Add("コマンドの実行: " + p.StartInfo.Arguments);
            p.Start();
        }

        private void P2_Exited(object sender, EventArgs e)
        {
            listBox1.Items.Add("2019-03 Windows 7 サービス スタック更新プログラム (KB4490628) のインストールが完了しました");

            //Process Launching: KB4474419
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.Arguments = "\"C:\\Program Files\\Win7Updater\\Update\\kb4474419.msu\" /quiet /norestart";
            p.EnableRaisingEvents = true;
            p.SynchronizingObject = this;
            p.StartInfo.FileName = "wusa.exe";
            p.Exited += P3_Exited;

            //Change Install State: KB4490628
            installStateText.Text = "Windows 7 のセキュリティ更新プログラム (KB4474419) をインストールしています...";
            installState.Value = 66;

            listBox1.Items.Add("2019-09 Windows 7 のセキュリティ更新プログラム (KB4474419) をインストールしています...");
            listBox1.Items.Add("コマンドの実行: " + p.StartInfo.Arguments);
            p.Start();
        }

        private void P3_Exited(object sender, EventArgs e)
        {
            //Change Install State: Finishing
            installStateText.Text = "最終処理をを実行中...";
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

            //Win7Updater: Restart State
            Microsoft.Win32.RegistryKey regkey1 =
                Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Win7Updater");
            regkey1.SetValue("Restarted", 1, Microsoft.Win32.RegistryValueKind.QWord);
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

        private void Wc3_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                listBox1.Items.Add(e.Error.ToString());
                listBox1.Items.Add("2019-09 Windows 7 のセキュリティ更新プログラム (KB4474419) のダウンロードに失敗...");
            }
            else
            {
                listBox1.Items.Add("2019-09 Windows 7 のセキュリティ更新プログラム (KB4474419) のダウンロードに成功！");
                u3ended = true;
            }
        }

        private void Wc2_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                listBox1.Items.Add("2019-03 Windows 7 サービス スタック更新プログラム (KB4490628) のダウンロードに失敗...");
            }
            else
            {
                listBox1.Items.Add("2019-03 Windows 7 サービス スタック更新プログラム (KB4490628) のダウンロードに成功！");
                u2ended = true;
            }
        }

        private void Wc1_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                listBox1.Items.Add("Windows 7 用更新プログラム (KB3133977) のダウンロードに失敗...");
            }
            else
            {
                listBox1.Items.Add("Windows 7 用更新プログラム (KB3133977) のダウンロードに成功！");
                u1ended = true;
            }
        }
    }
}
