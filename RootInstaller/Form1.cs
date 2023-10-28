using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace RootInstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            WebClient wc1 = new WebClient();
            WebClient wc2 = new WebClient();
            WebClient wc3 = new WebClient();
            Uri rootCert = new Uri("http://vistaupdater.net/tools/WindowsRoot.sst");
            Uri intermediateCert = new Uri("http://vistaupdater.net/tools/WindowsIntermediate.sst");
            Uri rootUpdater = new Uri("http://vistaupdater.net/tools/RootUpdater.exe");

            try
            {
                System.IO.Directory.CreateDirectory("C:\\Program Files\\VistaUpdater\\CertInstaller");
            }
            catch (Exception ex)
            {
                Console.WriteLine("エラー: " + ex.Message);
            }

            wc1.DownloadFileAsync(rootCert, "C:\\Program Files\\VistaUpdater\\CertInstaller\\WindowsRoot.sst");

            wc2.DownloadFileAsync(intermediateCert, "C:\\Program Files\\VistaUpdater\\CertInstaller\\WindowsIntermediate.sst");

            wc3.DownloadFileAsync(rootUpdater, "C:\\Program Files\\VistaUpdater\\CertInstaller\\RootUpdater.exe");
            wc3.DownloadFileCompleted += Wc3_DownloadFileCompleted;
        }

        private void Wc3_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = "C:\\Program Files\\VistaUpdater\\CertInstaller\\RootUpdater.exe";
            process.StartInfo.Arguments = "-l \"C:\\Program Files\\VistaUpdater\\CertInstaller\\WindowsRoot.sst\"";
            process.Start();

            Process process2 = new Process();
            process2.StartInfo.FileName = "C:\\Program Files\\VistaUpdater\\CertInstaller\\RootUpdater.exe";
            process2.StartInfo.Arguments = "-l \"C:\\Program Files\\VistaUpdater\\CertInstaller\\WindowsIntermediate.sst\"";
            process2.Start();

            MessageBox.Show("成功", "VistaUpdater", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
