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

namespace RepairTool
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.RegistryKey registryKey =
                 Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\VistaUpdater", false);
            if (registryKey == null)
            {
                DialogResult result = MessageBox.Show("VistaUpdater がインストールされていない可能性があります。\n本当に続行しますか？", "VistaUpdater | 修復ツール", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes) {
                    new RepairSelector().Show();
                    this.Hide();
                }
                else Application.Exit();
            }
            else
            {
                new RepairSelector().Show();
                this.Hide();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            if (System.Environment.OSVersion.Version.Major.ToString() + "." + System.Environment.OSVersion.Version.Minor.ToString() != "6.0")
            {
                this.Hide();
                MessageBox.Show("結果: 不合格\nOS は Windows Vista で動作していません", "互換性チェック - VistaUpdater", MessageBoxButtons.OK, MessageBoxIcon.Question);
                Application.Exit();
            }

            label3.Text = "VU RepairTool Version " + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
        }
    }
}
