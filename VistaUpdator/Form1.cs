using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using VistaUpdater.SPInstaller;

namespace VistaUpdater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // メインボタンのクリック処理
        private void button1_Click(object sender, EventArgs e)
        {
            // バージョン情報をレジストリに保存
            using (var regkey1 = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\VistaUpdater"))
            {
                regkey1.SetValue("Version", FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion, Microsoft.Win32.RegistryValueKind.String);
            }

            if (check_sp.Checked)
            {
                Hide();
                new StartUpdate().Show();
                return;
            }

            var os = Environment.OSVersion;
            if (os.ServicePack == "")
            {
                Hide();
                new SP1Installer().ShowDialog();
            }
            else if (os.ServicePack == "Service Pack 1")
            {
                Hide();
                new SP2Installer().ShowDialog();
            }
            else
            {
                Hide();
                new StartUpdate().Show();
            }
        }

        // フォームロード時の初期化
        private void Form1_Load(object sender, EventArgs e)
        {
            MaximumSize = Size;
            MinimumSize = Size;

            using (var regkey1 = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\VistaUpdater"))
            {
                regkey1.SetValue("InstallRoot", 1, Microsoft.Win32.RegistryValueKind.DWord);
            }

            if ($"{Environment.OSVersion.Version.Major}.{Environment.OSVersion.Version.Minor}" != "6.0")
            {
                Hide();
                MessageBox.Show("結果: 不合格\nOS は Windows Vista で動作していません", "互換性チェック - VistaUpdater", MessageBoxButtons.OK, MessageBoxIcon.Question);
                Application.Exit();
            }

            label3.Text = "VistaUpdater Version " + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
        }

        // Service Pack確認チェックボックス
        private void check_sp_CheckStateChanged(object sender, EventArgs e)
        {
            if (check_sp.Checked)
            {
                var result = MessageBox.Show("VistaUpdaterは、Service Packを確認しません。VistaUpdaterの処理に失敗する可能性があります。", "警告 - VistaUpdater", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    check_sp.Checked = false;
                }
            }
        }

        // InstallRoot設定チェックボックス
        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            using (var regkey1 = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\VistaUpdater"))
            {
                regkey1.SetValue("InstallRoot", checkBox1.Checked ? 1 : 0, Microsoft.Win32.RegistryValueKind.DWord);
            }
        }
    }
}
