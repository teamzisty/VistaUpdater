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
    public partial class RestartedUpdate : Form
    {
        bool u1ended = false;
        bool u2ended = false;
        bool u3ended = false;
        public RestartedUpdate()
        {
            InitializeComponent();
        }

        Timer timer = new Timer();

        private void StartUpdate_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            WebClient wc1 = new WebClient();
            Uri kb3125574 = new Uri("http://catalog.s.download.windowsupdate.com/d/msdownload/update/software/updt/2016/05/windows6.1-kb3125574-v4-x64_2dafb1d203c8964239af3048b5dd4b1264cd93b9.msu");
            if ((ushort)new ManagementObject("Win32_Processor.DeviceID='CPU0'")["AddressWidth"] == 32)
            {
                kb3125574 = new Uri("http://catalog.s.download.windowsupdate.com/d/msdownload/update/software/updt/2016/05/windows6.1-kb3125574-v4-x86_ba1ff5537312561795cc04db0b02fbb0a74b2cbd.msu");
            }
            wc1.DownloadFileAsync(kb3125574, "C:\\Program Files\\Win7Updater\\Update\\kb3125574.msu");
            wc1.DownloadFileCompleted += Wc1_DownloadFileCompleted;
            listBox1.Items.Add("Windows 7 用更新プログラム (KB3125574) をダウンロードしています...");
        }
            
        private void p_Exited(object sender, EventArgs e)
        {
            listBox1.Items.Add("システムを元に戻しています...");
            label2.Text = "最終処理の実行中...";
            listBox1.Items.Add("Shell を復元中...");

            //Change Install State: Finishing
            installStateText.Text = "最終処理を実行中...";
            installState.Value = 96;


            //Shell: Disable Auto Logon, Reset Shell
            Microsoft.Win32.RegistryKey regkey =
                  Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
            regkey.SetValue("Shell", "explorer.exe", Microsoft.Win32.RegistryValueKind.String);
            regkey.SetValue("AutoAdminLogon", 0, Microsoft.Win32.RegistryValueKind.DWord);
            regkey.DeleteValue("DefaultUserName");
            regkey.DeleteValue("DefaultPassword");
            regkey.Close();
            listBox1.Items.Add("Shell の復元が完了しました");
            listBox1.Items.Add("ユーザーアカウント: 'Win7Updater' を削除中...");

            //Delete User: Win7Updater
            DirectoryEntry localDirectory = new DirectoryEntry($"WinNT://{Environment.MachineName},computer");
            DirectoryEntries users = localDirectory.Children;
            DirectoryEntry user = users.Find("Win7Updater");
            users.Remove(user);
            listBox1.Items.Add("ユーザーアカウント: 'Win7Updater' を削除しました");
            listBox1.Items.Add("Win7Updater の設定を削除、ユーザーアカウント制御を有効にしています...");

            //Win7Updater: Remove State
            Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Win7Updater");

            //User Account Control: Enabling
            Microsoft.Win32.RegistryKey regkey2 =
                Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
            regkey2.SetValue("EnableLUA", 1, Microsoft.Win32.RegistryValueKind.DWord);

            listBox1.Items.Add("設定の削除とユーザーアカウント制御を有効にしました");
            listBox1.Items.Add("フォルダを削除中...");


            //Delete Directory
            if (System.IO.Directory.Exists("C:\\Program Files\\Win7Updater\\Update") & System.IO.Directory.Exists("C:\\Program Files\\Win7Updater\\Patches"))
            {
                System.IO.Directory.Delete("C:\\Program Files\\Win7Updater\\Update", true);
                System.IO.Directory.Delete("C:\\Program Files\\Win7Updater\\Patches", true);
            }
            else if (System.IO.Directory.Exists("C:\\Program Files\\Win7Updater\\Update"))
            {
                System.IO.Directory.Delete("C:\\Program Files\\Win7Updater\\Update", true);
            }
            else
            {
                //Skipped
                listBox1.Items.Add("フォルダを削除する必要はありません。スキップしました。");
            }


            listBox1.Items.Add("フォルダの削除に成功しました");
            listBox1.Items.Add("再起動中...");
            label2.Text = "再起動しています...";

            //Change Install State: Completed
            installStateText.Text = "完了";
            installState.Value = 100;

            //Process Launching: shutdown
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "shutdown.exe";
            p.StartInfo.Arguments = "/r /t 10 /c \"Win7Updater の処理を終了するため、10秒後に再起動します。\"";
            p.Start();
        }

        private void Wc1_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                listBox1.Items.Add("Windows 7 for x64-Based Systems 用更新プログラム (KB3125574) のダウンロードに失敗...");
            }
            else
            {
                listBox1.Items.Add("Windows 7 for x64-Based Systems 用更新プログラム (KB3125574) のダウンロードに成功！");

                label2.Text = "アップデートをインストール中...";
                timer.Stop();

                listBox1.Items.Add("ダウンロードに成功しました！");
                listBox1.Items.Add("インストールを開始...");

                //Process Launching: KB3125574 (Optimize SSU / SP2)
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.Arguments = "\"C:\\Program Files\\Win7Updater\\Update\\kb3125574.msu\" /quiet /norestart";
                p.EnableRaisingEvents = true;
                p.SynchronizingObject = this;
                p.StartInfo.FileName = "wusa.exe";
                p.Exited += p_Exited;

                //Change Install State: KB3125574
                installStateText.Text = "Windows 7 用更新プログラム (KB3125574) をインストールしています...";
                installState.Value = 0;

                listBox1.Items.Add("Windows 7 用更新プログラム (KB3125574) をインストールしています...");
                listBox1.Items.Add("コマンドの実行: " + p.StartInfo.Arguments);
                p.Start();
            }
        }
    }
}
