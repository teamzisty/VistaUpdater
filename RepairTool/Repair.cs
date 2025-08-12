using System;
using System.DirectoryServices;
using System.Windows.Forms;

namespace RepairTool
{
    public partial class Repair : Form
    {
        public Repair()
        {
            InitializeComponent();
        }

        private void Repair_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            listBox1.Items.Add("システムを元に戻しています...");
            label2.Text = "最終処理の実行中...";
            RestoreShell();
            RemoveUserAccount();
            RemoveSettingsAndEnableUAC();
            DeleteUpdaterFolders();
            listBox1.Items.Add("再起動中...");
            label2.Text = "再起動しています...";
            RestartSystem();
        }

        private void RestoreShell()
        {
            listBox1.Items.Add("Shell を復元中...");
            try
            {
                using (var regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true))
                {
                    regkey.SetValue("Shell", "explorer.exe", Microsoft.Win32.RegistryValueKind.String);
                    regkey.SetValue("AutoAdminLogon", 0, Microsoft.Win32.RegistryValueKind.DWord);
                    regkey.DeleteValue("DefaultUserName", false);
                    regkey.DeleteValue("DefaultPassword", false);
                }
                listBox1.Items.Add("Shell の復元が完了しました");
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"Shell の復元に失敗: {ex.Message}");
            }
        }

        private void RemoveUserAccount()
        {
            listBox1.Items.Add("ユーザーアカウント: 'VistaUpdater' を削除中...");
            try
            {
                DirectoryEntry localDirectory = new DirectoryEntry($"WinNT://{Environment.MachineName},computer");
                DirectoryEntries users = localDirectory.Children;
                DirectoryEntry user = users.Find("VistaUpdater");
                users.Remove(user);
                listBox1.Items.Add("ユーザーアカウント: 'VistaUpdater' を削除しました");
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"ユーザーアカウント削除失敗: {ex.Message}");
            }
        }

        private void RemoveSettingsAndEnableUAC()
        {
            listBox1.Items.Add("VistaUpdater の設定を削除、ユーザーアカウント制御を有効にしています...");
            try
            {
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\\VistaUpdater");
                using (var regkey2 = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"))
                {
                    regkey2.SetValue("EnableLUA", 1, Microsoft.Win32.RegistryValueKind.DWord);
                }
                listBox1.Items.Add("設定の削除とユーザーアカウント制御を有効にしました");
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"設定削除/UAC有効化失敗: {ex.Message}");
            }
        }

        private void DeleteUpdaterFolders()
        {
            listBox1.Items.Add("フォルダを削除中...");
            try
            {
                string basePath = "C:\\Program Files\\VistaUpdater";
                string updatePath = basePath + "\\Update";
                string patchesPath = basePath + "\\Patches";
                bool updateExists = System.IO.Directory.Exists(updatePath);
                bool patchesExists = System.IO.Directory.Exists(patchesPath);
                bool baseExists = System.IO.Directory.Exists(basePath);

                if (updateExists && patchesExists && baseExists)
                {
                    System.IO.Directory.Delete(updatePath, true);
                    System.IO.Directory.Delete(patchesPath, true);
                    System.IO.Directory.Delete(basePath, true);
                }
                else if (patchesExists)
                {
                    System.IO.Directory.Delete(patchesPath, true);
                }
                else if (updateExists)
                {
                    System.IO.Directory.Delete(updatePath, true);
                }
                else if (baseExists)
                {
                    System.IO.Directory.Delete(basePath, true);
                }
                else
                {
                    listBox1.Items.Add("フォルダを削除する必要はありません。スキップしました。");
                }
                listBox1.Items.Add("フォルダの削除に成功しました");
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"フォルダ削除失敗: {ex.Message}");
            }
        }

        private void RestartSystem()
        {
            try
            {
                var p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "shutdown.exe";
                p.StartInfo.Arguments = "/r /t 10 /c \"VistaUpdater 修復ツールの処理を終了するため、再起動する必要があります。\"";
                p.Start();
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"再起動コマンド失敗: {ex.Message}");
            }
        }
    }
}
