using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Windows.Forms;
using System.DirectoryServices;

namespace VistaUpdater
{
    public partial class StartUpdate : Form
    {
        // ダウンロード完了フラグ
        bool u1ended = false, u2ended = false, u3ended = false, u4ended = false, u5ended = false, u6ended = false, u7ended = false, u8ended = false;
        Timer timer = new Timer();
        Timer timer2 = new Timer();

        public StartUpdate()
        {
            InitializeComponent();
        }

        private void StartUpdate_Load(object sender, EventArgs e)
        {
            MaximumSize = Size;
            MinimumSize = Size;
            bool installRoot = true;
            try
            {
                using (var regkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\VistaUpdater"))
                {
                    var rootValue = regkey.GetValue("InstallRoot")?.ToString();
                    if (rootValue == "0") installRoot = false;
                }
            }
            catch (Exception ex) { Console.WriteLine("エラー: " + ex.Message); }

            try
            {
                Directory.CreateDirectory("C:\\Program Files\\VistaUpdater");
                Directory.CreateDirectory("C:\\Program Files\\VistaUpdater\\Update");
            }
            catch (Exception ex) { Console.WriteLine("エラー: " + ex.Message); }

            // ダウンロード処理
            var uris = GetUpdateUris();
            var wc1 = new WebClient();
            wc1.DownloadFileAsync(uris[0], "C:\\Program Files\\VistaUpdater\\Update\\kb3205638.msu");
            wc1.DownloadFileCompleted += Wc1_DownloadFileCompleted;
            ChangeDownloadState("Windows Vista 用セキュリティ更新プログラム (KB3205638) をダウンロードしています...", 16);
            var wc2 = new WebClient();
            wc2.DownloadFileAsync(uris[1], "C:\\Program Files\\VistaUpdater\\Update\\kb4012583.msu");
            wc2.DownloadFileCompleted += Wc2_DownloadFileCompleted;
            ChangeDownloadState("Windows Vista 用セキュリティ更新プログラム (KB4012583) をダウンロードしています...", 32);
            var wc3 = new WebClient();
            wc3.DownloadFileAsync(uris[2], "C:\\Program Files\\VistaUpdater\\Update\\kb4015195.msu");
            wc3.DownloadFileCompleted += Wc3_DownloadFileCompleted;
            ChangeDownloadState("Windows Vista 用セキュリティ更新プログラム (KB4015195) をダウンロードしています...", 48);
            var wc4 = new WebClient();
            wc4.DownloadFileAsync(uris[3], "C:\\Program Files\\VistaUpdater\\Update\\kb4015380.msu");
            wc4.DownloadFileCompleted += Wc4_DownloadFileCompleted;
            ChangeDownloadState("Windows Vista 用セキュリティ更新プログラム (KB4015380) をダウンロードしています...", 64);
            var wc5 = new WebClient();
            wc5.DownloadFileAsync(uris[4], "C:\\Program Files\\VistaUpdater\\Update\\kb971512.msu");
            wc5.DownloadFileCompleted += Wc5_DownloadFileCompleted;
            ChangeDownloadState("Windows Vista 用の更新プログラム (KB971512) をダウンロードしています...", 96);
            var wc6 = new WebClient();
            wc6.DownloadFileAsync(uris[5], "C:\\Program Files\\VistaUpdater\\Update\\kb2117917.msu");
            wc6.DownloadFileCompleted += Wc6_DownloadFileCompleted;
            ChangeDownloadState("Windows Vista 用プラットフォーム更新プログラム補足 (KB2117917) をダウンロードしています...", 100);

            if (installRoot)
            {
                var wc7 = new WebClient();
                var wc8 = new WebClient();
                var wc9 = new WebClient();
                wc7.DownloadFileAsync(new Uri("http://vistaupdater.net/tools/WindowsRoot.sst"), "C:\\Program Files\\VistaUpdater\\Update\\WindowsRoot.sst");
                wc7.DownloadFileCompleted += Wc7_DownloadFileCompleted;
                ChangeDownloadState("ルート証明書(1番目) をダウンロードしています...", 100);
                wc8.DownloadFileAsync(new Uri("http://vistaupdater.net/tools/WindowsIntermediate.sst"), "C:\\Program Files\\VistaUpdater\\Update\\WindowsIntermediate.sst");
                wc8.DownloadFileCompleted += Wc8_DownloadFileCompleted;
                ChangeDownloadState("ルート証明書(2番目) をダウンロードしています...", 100);
                wc9.DownloadFileAsync(new Uri("http://vistaupdater.net/tools/RootUpdater.exe"), "C:\\Program Files\\VistaUpdater\\Update\\RootUpdater.exe");
                wc9.DownloadFileCompleted += Wc9_DownloadFileCompleted;
                ChangeDownloadState("証明書更新ツール をダウンロードしています...", 100);
            }

            try
            {
                var appExe = "C:\\Program Files\\VistaUpdater\\Application.exe";
                if (!File.Exists(appExe))
                {
                    File.Copy(Application.ExecutablePath, appExe);
                }
                else
                {
                    File.Delete(appExe);
                    File.Copy(Application.ExecutablePath, appExe);
                }
            }
            catch (Exception ex) { Console.WriteLine("エラー: " + ex.Message); }

            timer.Tick += Timer_Tick;
            timer.Interval = 100;
            timer.Start();
        }

        // 更新プログラムのURLを取得
        private Uri[] GetUpdateUris()
        {
            Uri kb3205638 = new Uri("http://catalog.s.download.windowsupdate.com/d/msdownload/update/software/secu/2016/11/windows6.0-kb3205638-x64_a52aaa009ee56ca941e21a6009c00bc4c88cbb7c.msu");
            Uri kb4012583 = new Uri("http://catalog.s.download.windowsupdate.com/c/msdownload/update/software/secu/2017/02/windows6.0-kb4012583-x64_f63c9a85aa877d86c886e432560fdcfad53b752d.msu");
            Uri kb4015195 = new Uri("http://catalog.s.download.windowsupdate.com/d/msdownload/update/software/secu/2017/03/windows6.0-kb4015195-x64_2e310724d86b6a43c5ae8ec659685dd6cfb28ba4.msu");
            Uri kb4015380 = new Uri("http://catalog.s.download.windowsupdate.com/c/msdownload/update/software/secu/2017/03/windows6.0-kb4015380-x64_959aedbe0403d160be89f4dac057e2a0cd0c6d40.msu");
            Uri kb971512 = new Uri("http://catalog.s.download.windowsupdate.com/msdownload/update/software/updt/2009/10/windows6.0-kb971512-x64_0b329b985437c6c572529e5fd0042b9d54aeaa0c.msu");
            Uri kb2117917 = new Uri("http://catalog.s.download.windowsupdate.com/msdownload/update/software/updt/2011/02/windows6.0-kb2117917-x64_655a21758801e9648702791d7bf30f81b58884b3.msu");
            if ((ushort)new ManagementObject("Win32_Processor.DeviceID='CPU0'")["AddressWidth"] == 32)
            {
                kb3205638 = new Uri("http://catalog.s.download.windowsupdate.com/d/msdownload/update/software/secu/2016/11/windows6.0-kb3205638-x86_e2211e9a6523061972decd158980301fc4c32a47.msu");
                kb4012583 = new Uri("http://catalog.s.download.windowsupdate.com/c/msdownload/update/software/secu/2017/02/windows6.0-kb4012583-x86_1887cb5393b62cbd2dbb6a6ff6b136e809a2fbd0.msu");
                kb4015195 = new Uri("http://catalog.s.download.windowsupdate.com/d/msdownload/update/software/secu/2017/03/windows6.0-kb4015195-x86_eb045e0144266b20b615f29fa581c4001ebb7852.msu");
                kb4015380 = new Uri("http://catalog.s.download.windowsupdate.com/d/msdownload/update/software/secu/2017/03/windows6.0-kb4015380-x86_3f3548db24cf61d6f47d2365c298d739e6cb069a.msu");
                kb971512 = new Uri("http://catalog.s.download.windowsupdate.com/msdownload/update/software/updt/2009/10/windows6.0-kb971512-x86_370c3e41e1c161ddce29676e9273e4b8bb7ba3eb.msu");
                kb2117917 = new Uri("http://catalog.s.download.windowsupdate.com/msdownload/update/software/updt/2011/02/windows6.0-kb2117917-x86_370435d9efa6643c44d6506666b1960d56cf673a.msu");
            }
            return new[] { kb3205638, kb4012583, kb4015195, kb4015380, kb971512, kb2117917 };
        }

        private void Wc9_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            timer2.Tick += Timer2_Tick;
            timer2.Interval = 100;
            timer2.Start();
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            if (u7ended & u8ended)
            {
                timer2.Stop();
                listBox1.Items.Add("ルート証明書(1番目) をインストールしています...");
                Process process = new Process();
                process.StartInfo.FileName = "C:\\Program Files\\VistaUpdater\\Update\\RootUpdater.exe";
                process.StartInfo.Arguments = "-l \"C:\\Program Files\\VistaUpdater\\Update\\WindowsRoot.sst\"";
                process.EnableRaisingEvents = true;
                process.SynchronizingObject = this;
                process.Exited += Root1_Exited;
                process.Start();
            }
        }

        private void Root1_Exited(object sender, EventArgs e)
        {
            listBox1.Items.Add("ルート証明書(1番目) のインストールが完了しました");
            listBox1.Items.Add("ルート証明書(2番目) をインストールしています...");
            Process process2 = new Process();
            process2.StartInfo.FileName = "C:\\Program Files\\VistaUpdater\\Update\\RootUpdater.exe";
            process2.StartInfo.Arguments = "-l \"C:\\Program Files\\VistaUpdater\\Update\\WindowsIntermediate.sst\"";
            process2.Start();
            listBox1.Items.Add("ルート証明書(2番目) のインストールが完了しました");
        }

        private void Wc8_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            u8ended = true;
        }

        private void Wc7_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            u7ended = true;
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (u1ended & u2ended & u3ended & u4ended & u5ended & u6ended)
            {
                label2.Text = "アップデートをインストール中...";
                timer.Stop();
                ChangeDownloadState("ダウンロードに成功しました！", 100);
                ChangeInstallState("インストールを開始...", 0);
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.Arguments = "\"C:\\Program Files\\VistaUpdater\\Update\\kb3205638.msu\" /quiet /norestart";
                p.EnableRaisingEvents = true;
                p.SynchronizingObject = this;
                p.StartInfo.FileName = "wusa.exe";
                p.Exited += p_Exited;
                ChangeInstallState("Windows Vista 用セキュリティ更新プログラム (KB3205638) をインストールしています...", 8);
                listBox1.Items.Add("コマンドの実行: " + p.StartInfo.FileName + " " + p.StartInfo.Arguments);
                p.Start();
            }
        }

        private void p_Exited(object sender, EventArgs e)
        {
            ChangeInstallState("Windows Vista 用セキュリティ更新プログラム (KB3205638) のインストールが完了しました", 16);
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.Arguments = "\"C:\\Program Files\\VistaUpdater\\Update\\kb4012583.msu\" /quiet /norestart";
            p.EnableRaisingEvents = true;
            p.SynchronizingObject = this;
            p.StartInfo.FileName = "wusa.exe";
            p.Exited += P2_Exited;
            ChangeInstallState("Windows Vista 用セキュリティ更新プログラム (KB4012583) をインストールしています...", 16);
            listBox1.Items.Add("コマンドの実行: " + p.StartInfo.FileName + " " + p.StartInfo.Arguments);
            p.Start();
        }

        private void P2_Exited(object sender, EventArgs e)
        {
            ChangeInstallState("Windows Vista 用セキュリティ更新プログラム (KB4012583) のインストールが完了しました", 32);
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.Arguments = "\"C:\\Program Files\\VistaUpdater\\Update\\kb4015195.msu\" /quiet /norestart";
            p.EnableRaisingEvents = true;
            p.SynchronizingObject = this;
            p.StartInfo.FileName = "wusa.exe";
            p.Exited += P3_Exited;
            ChangeInstallState("Windows Vista 用セキュリティ更新プログラム (KB4015195) をインストールしています...", 32);
            listBox1.Items.Add("コマンドの実行: " + p.StartInfo.FileName + " " + p.StartInfo.Arguments);
            p.Start();
        }

        private void P3_Exited(object sender, EventArgs e)
        {
            ChangeInstallState("Windows Vista 用セキュリティ更新プログラム (KB4015195) のインストールが完了しました", 48);
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.Arguments = "\"C:\\Program Files\\VistaUpdater\\Update\\kb4015380.msu\" /quiet /norestart";
            p.EnableRaisingEvents = true;
            p.SynchronizingObject = this;
            p.StartInfo.FileName = "wusa.exe";
            p.Exited += P4_Exited;
            ChangeInstallState("Windows Vista 用セキュリティ更新プログラム (KB4015380) をインストールしています...", 48);
            listBox1.Items.Add("コマンドの実行: " + p.StartInfo.FileName + " " + p.StartInfo.Arguments);
            p.Start();
        }

        private void P4_Exited(object sender, EventArgs e)
        {
            ChangeInstallState("Windows Vista 用セキュリティ更新プログラム (KB4015380) のインストールが完了しました", 48);
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.Arguments = "\"C:\\Program Files\\VistaUpdater\\Update\\kb971512.msu\" /quiet /norestart";
            p.EnableRaisingEvents = true;
            p.SynchronizingObject = this;
            p.StartInfo.FileName = "wusa.exe";
            p.Exited += P5_Exited;
            ChangeInstallState("Windows Vista 用セキュリティ更新プログラム (KB971512) をインストールしています...", 48);
            listBox1.Items.Add("コマンドの実行: " + p.StartInfo.FileName + " " + p.StartInfo.Arguments);
            p.Start();
        }

        private void P5_Exited(object sender, EventArgs e)
        {
            ChangeInstallState("Windows Vista 用セキュリティ更新プログラム (KB971512) のインストールが完了しました", 64);
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.Arguments = "\"C:\\Program Files\\VistaUpdater\\Update\\kb2117917.msu\" /quiet /norestart";
            p.EnableRaisingEvents = true;
            p.SynchronizingObject = this;
            p.StartInfo.FileName = "wusa.exe";
            p.Exited += P6_Exited;
            ChangeInstallState("Windows Vista 用プラットフォーム更新プログラム補足 (KB2117917) をインストールしています...", 64);
            listBox1.Items.Add("コマンドの実行: " + p.StartInfo.FileName + " " + p.StartInfo.Arguments);
            p.Start();
        }

        private void P6_Exited(object sender, EventArgs e)
        {
            ChangeInstallState("Windows Vista 用プラットフォーム更新プログラム補足 (KB2117917) のインストールが完了しました", 80);
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
                listBox1.Items.Add("エラー: " + ex.Message);
                Console.WriteLine("エラー: " + ex.Message);
            }
            try
            {
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
                regkey1.SetValue("Restarted", 1, Microsoft.Win32.RegistryValueKind.QWord);
                regkey1.Close();
            }
            catch (Exception ex)
            {
                listBox1.Items.Add("エラー: " + ex.Message);
                Console.WriteLine("エラー: " + ex.Message);
            }
            try
            {
                Microsoft.Win32.RegistryKey regkey =
    Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\VistaUpdater");
                regkey.DeleteValue("SP2Installed");
                regkey.DeleteValue("SP1Installed");
                regkey.Close();
            }
            catch (Exception) { }
            try
            {
                Microsoft.Win32.RegistryKey regkey2 =
                    Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
                regkey2.SetValue("EnableLUA", 0, Microsoft.Win32.RegistryValueKind.DWord);
                regkey2.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("エラー: " + ex.Message);
            }
            try
            {
                listBox1.Items.Add("Shellの設定が完了しました");
                listBox1.Items.Add("再起動中...");
                label2.Text = "再起動しています...";
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "shutdown.exe";
                p.StartInfo.Arguments = "/r /t 10 /c \"VistaUpdater の処理を続けるため、10秒後に再起動します。\"";
                p.Start();
                ChangeInstallState("完了", 100);
            }
            catch (Exception ex)
            {
                Console.WriteLine("エラー: " + ex.Message);
            }
        }

        private void Wc6_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                listBox1.Items.Add("Windows Vista 用プラットフォーム更新プログラム補足 (KB2117917) のダウンロードに失敗..." + e.Error);
            }
            else
            {
                listBox1.Items.Add("Windows Vista 用プラットフォーム更新プログラム補足 (KB2117917) のダウンロードに成功！");
                u6ended = true;
            }
        }

        private void Wc5_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                listBox1.Items.Add("Windows Vista 用の更新プログラム (KB971512) のダウンロードに失敗..." + e.Error);
            }
            else
            {
                listBox1.Items.Add("Windows Vista 用の更新プログラム (KB971512) のダウンロードに成功！");
                u5ended = true;
            }
        }

        private void Wc4_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                listBox1.Items.Add("Windows Vista 用セキュリティ更新プログラム (KB4015380) のダウンロードに失敗..." + e.Error);
            }
            else
            {
                listBox1.Items.Add("Windows Vista 用セキュリティ更新プログラム (KB4015380) のダウンロードに成功！");
                u4ended = true;
            }
        }

        private void Wc3_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                listBox1.Items.Add("Windows Vista 用セキュリティ更新プログラム (KB4015195) のダウンロードに失敗...");
            }
            else
            {
                listBox1.Items.Add("Windows Vista 用セキュリティ更新プログラム (KB4015195) のダウンロードに成功！");
                u3ended = true;
            }
        }

        private void Wc2_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                listBox1.Items.Add("Windows Vista 用セキュリティ更新プログラム (KB4012583) のダウンロードに失敗...");
            }
            else
            {
                listBox1.Items.Add("Windows Vista 用セキュリティ更新プログラム (KB4012583) のダウンロードに成功！");
                u2ended = true;
            }
        }

        private void Wc1_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                listBox1.Items.Add("Windows Vista 用セキュリティ更新プログラム (KB3205638) のダウンロードに失敗...");
            }
            else
            {
                listBox1.Items.Add("Windows Vista 用セキュリティ更新プログラム (KB3205638) のダウンロードに成功！");
                u1ended = true;
            }
        }
    }
}
