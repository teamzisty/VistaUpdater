using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Management;
using System.IO;
using System.Reflection;
using Ionic.Zip;
using Microsoft.Win32;
using System.DirectoryServices;

namespace VistaUpdater
{
    public partial class RestartedUpdate2 : Form
    {
        public RestartedUpdate2()
        {
            InitializeComponent();
        }

        Timer timer = new Timer();

        bool u1ended = false;
        bool u2ended = false;
        bool u3ended = false;

        string output = "";
        string extractPath = "C:\\Program Files\\VistaUpdaterAfter";

        private void RestartedUpdate2_Load(object sender, EventArgs e)
        {
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            WebClient wc = new WebClient();
            WebClient wc2 = new WebClient();
            WebClient wc3 = new WebClient();
            Uri vcRedist = new Uri("http://download.microsoft.com/download/C/A/F/CAF5E118-4803-4D68-B6B5-A1772903D119/VSU4/vcredist_x86.exe");
            wc.DownloadFileAsync(vcRedist, "C:\\Program Files\\VistaUpdater\\Update\\vcredist.exe");
            listBox1.Items.Add("Visual Studio 2012 Visual C++ 再頒布可能パッケージ をダウンロードしています...");
            wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
        }


        private void p_Exited(object sender, EventArgs e)
        {
            installState.Value = 33;
            installStateText.Text = "WSUS Proxy をインストールしています... 1/2";

            listBox1.Items.Add("Visual Studio 2012 Visual C++ 再頒布可能パッケージ をインストールしました");

            System.IO.Directory.CreateDirectory("C:\\Program Files\\VistaUpdater\\Patches");
            Extract("VistaUpdater", "C:\\Program Files\\VistaUpdater\\Patches", "Resources", "wsus_proxy.zip");



            next();
        }

        private void next()
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                StreamWriter sw = new StreamWriter(ms);

                ReadOptions options = new ReadOptions();
                options.StatusMessageWriter = sw;
                ZipFile zf = ZipFile.Read("C:\\Program Files\\VistaUpdater\\Patches\\wsus_proxy.zip", options);

                zf.ExtractAll(extractPath);

                ms.Seek(0, 0);
            }
            catch (Exception)
            {

            }

            timer = new Timer();
            timer.Interval = 3000; // 3000ミリ秒（3秒）
            timer.Tick += Timer_Tick;

            // タイマーの開始
            timer.Start();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "C:\\Program Files\\VistaUpdaterAfter\\Add_wsus.cmd";

            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.EnableRaisingEvents = true;
            proc.SynchronizingObject = this;
            proc.Exited += p2_Exited;
            proc.Start();
            output = proc.StandardOutput.ReadToEnd();
        }

        public static void Extract(string nameSpace, string outDirectory, string internalFilePath, string resourceName)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            using (Stream s = assembly.GetManifestResourceStream(nameSpace + "." + (internalFilePath == "" ? "" : internalFilePath + ".") + resourceName))
            using (BinaryReader r = new BinaryReader(s))
            using (FileStream fs = new FileStream(outDirectory + "\\" + resourceName, FileMode.OpenOrCreate))
            using (BinaryWriter w = new BinaryWriter(fs))
            {
                w.Write(r.ReadBytes((int)s.Length));
            }
        }

        private void p2_Exited(object sender, EventArgs e)
        {
            installStateText.Text = "WSUS Proxy をインストールしています... 2/2";
            installState.Value = 66;

            string appName = "WSUSProxied";
            string appPath = "C:\\Program Files\\VistaUpdaterAfter\\Run_wsus.cmd";

            // レジストリキーにエントリを追加
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            key.SetValue(appName, appPath);

            installStateText.Text = "最終処理の実行中...";
            installState.Value = 100;

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
            try
            {
                if (System.IO.Directory.Exists("C:\\Program Files\\VistaUpdater\\Update") & System.IO.Directory.Exists("C:\\Program Files\\VistaUpdater\\Patches"))
                {
                    System.IO.Directory.Delete("C:\\Program Files\\VistaUpdater\\Update", true);
                    System.IO.Directory.Delete("C:\\Program Files\\VistaUpdater\\Patches", true);
                }
                else if (System.IO.Directory.Exists("C:\\Program Files\\VistaUpdater\\Patches"))
                {
                    System.IO.Directory.Delete("C:\\Program Files\\VistaUpdater\\Patches", true);
                }
                else if (System.IO.Directory.Exists("C:\\Program Files\\VistaUpdater\\Update"))
                {
                    System.IO.Directory.Delete("C:\\Program Files\\VistaUpdater\\Update", true);
                }
                else
                {
                    listBox1.Items.Add("フォルダを削除する必要はありません。スキップしました。");
                }
            } catch (Exception ex)
            {

            }
            listBox1.Items.Add("フォルダの削除に成功しました");
            listBox1.Items.Add("再起動中...");
            label2.Text = "再起動しています...";
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "shutdown.exe";
            p.StartInfo.Arguments = "/r /t 10 /c \"VistaUpdater の終了するため、10秒後に再起動します。\"";
            p.Start();
        }

        private void Wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            listBox1.Items.Add("ダウンロードに成功しました！");
            listBox1.Items.Add("インストールを開始...");
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.Arguments = "/quiet /install";
            p.EnableRaisingEvents = true;
            p.SynchronizingObject = this;
            p.StartInfo.FileName = "C:\\Program Files\\VistaUpdater\\Update\\vcredist.exe";
            p.Exited += p_Exited;

            installStateText.Text = "Visual Studio 2012 Visual C++ 再頒布可能パッケージ をインストールしています...";

            listBox1.Items.Add("Visual Studio 2012 Visual C++ 再頒布可能パッケージ をインストールしています...");
            listBox1.Items.Add("コマンドの実行: " + p.StartInfo.FileName + " " + p.StartInfo.Arguments);
            p.Start();
        }

        private void RestartedUpdate2_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
