using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
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

        private void button1_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.RegistryKey regkey1 =
                 Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\VistaUpdater");
            regkey1.SetValue("Version", FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion, Microsoft.Win32.RegistryValueKind.String);
            regkey1.Close();

            if (check_sp.Checked)
            {
                StartUpdate su = new StartUpdate();
                this.Hide();
                su.Show();

                return;
            }
            System.OperatingSystem os = System.Environment.OSVersion;
            if (os.ServicePack == "")
            {
                SP1Installer sp1 = new SP1Installer();
                this.Hide();
                sp1.ShowDialog();
            }
            else if (os.ServicePack == "Service Pack 1")
            {
                SP2Installer sp2 = new SP2Installer();
                this.Hide();
                sp2.ShowDialog();
            }
            else
            {
                StartUpdate su = new StartUpdate();
                this.Hide();
                su.Show();
            }

            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            Microsoft.Win32.RegistryKey regkey1 =
     Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\VistaUpdater");
            regkey1.SetValue("InstallRoot", 1, Microsoft.Win32.RegistryValueKind.DWord);
            regkey1.Close();

            if (System.Environment.OSVersion.Version.Major.ToString() + "." + System.Environment.OSVersion.Version.Minor.ToString() != "6.0")
            {
                this.Hide();
                MessageBox.Show("結果: 不合格\nOS は Windows Vista で動作していません", "互換性チェック - VistaUpdater", MessageBoxButtons.OK, MessageBoxIcon.Question);
                Application.Exit();
            }

            label3.Text = "VistaUpdater Version " + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
        }

        private void check_sp_CheckStateChanged(object sender, EventArgs e)
        {
            if (check_sp.Checked)
            {
                DialogResult result = MessageBox.Show("VistaUpdaterは、Service Packを確認しません。VistaUpdaterの処理に失敗する可能性があります。", "警告 - VistaUpdater", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    check_sp.Checked = false;
                }
            }
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Microsoft.Win32.RegistryKey regkey1 =
    Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\VistaUpdater");
                regkey1.SetValue("InstallRoot", 1, Microsoft.Win32.RegistryValueKind.DWord);
                regkey1.Close();
            } else
            {
                Microsoft.Win32.RegistryKey regkey1 =
Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\VistaUpdater");
                regkey1.SetValue("InstallRoot", 0, Microsoft.Win32.RegistryValueKind.DWord);
                regkey1.Close();
            }
        }
    }
}
