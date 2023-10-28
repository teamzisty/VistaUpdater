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
    public partial class RepairSelector : Form
    {
        public RepairSelector()
        {
            InitializeComponent();
        }

        private void RepairSelector_Load(object sender, EventArgs e)
        {
            Microsoft.Win32.RegistryKey regkey1 =
    Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\VistaUpdater");
            string key = regkey1.GetValue("Version").ToString();
            regkey1.Close();

            label1.Text = "VistaUpdater v" + key;
            label2.Text = "VU RepairTool Version " + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
        }

        private void ReInstallVU_Click(object sender, EventArgs e)
        {
            new Reinstall().ShowDialog();
            this.Hide();
        }

        private void UpdateReinstall_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Reinstall(true).ShowDialog();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Repair().ShowDialog();
            this.Hide();
        }
    }
}
