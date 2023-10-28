using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Win7Updater
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
     Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Win7Updater");
            regkey1.SetValue("Version", FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion, Microsoft.Win32.RegistryValueKind.String);
            regkey1.Close();

            System.OperatingSystem os = System.Environment.OSVersion;
            if (os.ServicePack == "Service Pack 1")
            {
                new StartUpdate().ShowDialog();
                this.Hide();
            } else
            {
                new SPInstaller.SPInstaller().ShowDialog();
                this.Hide();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (System.Environment.OSVersion.Version.Major.ToString() + "." + System.Environment.OSVersion.Version.Minor.ToString() != "6.1")
            {
                this.Hide();
                MessageBox.Show("結果: 不合格\nOS は Windows 7 で動作していません", "互換性チェック - Win7Updater", MessageBoxButtons.OK, MessageBoxIcon.Question);
                Application.Exit();
            }

            label3.Text = "Win7Updater Version " + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
        }
    }
}
