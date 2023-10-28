using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace VistaUpdater
{
    public partial class Final : Form
    {
        public Final()
        {
            InitializeComponent();
        }

        private void Final_Load(object sender, EventArgs e)
        {
                listBox1.Items.Add("Windows Update パッチをインストールしています... 2/2");

            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "C:\\Program Files\\VistaUpdater\\Patches\\WindowsUpdate_Patch\\Patch_WUC.cmd";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.EnableRaisingEvents = true;
            proc.SynchronizingObject = this;
            proc.Exited += p_Exited;
            proc.Start();
        }

        private void p_Exited(object sender, EventArgs e)
        {
            listBox1.Items.Add("Windows Update パッチのインストールが完了しました");
            listBox1.Items.Add("システムを元に戻しています...");
            label2.Text = "最終処理の実行中...";
            listBox1.Items.Add("Shell を復元中...");
            Microsoft.Win32.RegistryKey regkey =
                  Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
            regkey.SetValue("Shell", "explorer.exe", Microsoft.Win32.RegistryValueKind.String);
            regkey.SetValue("AutoAdminLogon", 0, Microsoft.Win32.RegistryValueKind.DWord);
            regkey.DeleteValue("DefaultUserName");
            regkey.DeleteValue("DefaultPassword");
            regkey.Close();
            listBox1.Items.Add("Shell の復元が完了しました");
            listBox1.Items.Add("ユーザーアカウント: 'VistaUpdater' を削除中...");
            DirectoryEntry localDirectory = new DirectoryEntry($"WinNT://{Environment.MachineName},computer");
            DirectoryEntries users = localDirectory.Children;
            DirectoryEntry user = users.Find("VistaUpdater");
            users.Remove(user);
            listBox1.Items.Add("ユーザーアカウント: 'VistaUpdater' を削除しました");
            listBox1.Items.Add("VistaUpdater の設定を削除、ユーザーアカウント制御を有効にしています...");
            Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\VistaUpdater");
            Microsoft.Win32.RegistryKey regkey2 =
                Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
            regkey2.SetValue("EnableLUA", 1, Microsoft.Win32.RegistryValueKind.DWord);
            listBox1.Items.Add("設定の削除とユーザーアカウント制御を有効にしました");
            listBox1.Items.Add("フォルダを削除中...");
            if (System.IO.Directory.Exists("C:\\Program Files\\VistaUpdater\\Update") & System.IO.Directory.Exists("C:\\Program Files\\VistaUpdater\\Patches"))
            {
                System.IO.Directory.Delete("C:\\Program Files\\VistaUpdater\\Update", true);
                System.IO.Directory.Delete("C:\\Program Files\\VistaUpdater\\Patches", true);
            } else if (System.IO.Directory.Exists("C:\\Program Files\\VistaUpdater\\Patches"))
            {
                System.IO.Directory.Delete("C:\\Program Files\\VistaUpdater\\Patches", true);
            } else if (System.IO.Directory.Exists("C:\\Program Files\\VistaUpdater\\Update"))
            {
                System.IO.Directory.Delete("C:\\Program Files\\VistaUpdater\\Update", true);
            } else
            {
                listBox1.Items.Add("フォルダを削除する必要はありません。スキップしました。");
            }
            listBox1.Items.Add("フォルダの削除に成功しました");
            listBox1.Items.Add("再起動中...");
            label2.Text = "再起動しています...";
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "shutdown.exe";
            p.StartInfo.Arguments = "/r /t 10 /c \"VistaUpdater の終了するため、10秒後に再起動します。\"";
            p.Start();
        }

        private void Final_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
