using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using static System.Net.Mime.MediaTypeNames;
using Ionic.Zip;
using System.Reflection;
using System.Diagnostics;
using System.Management;

namespace VistaUpdater
{
    public partial class RestartedUpdate3 : Form
    {
        public RestartedUpdate3()
        {
            InitializeComponent();
        }

        string output = "";
        string extractPath = "C:\\Program Files\\VistaUpdater\\Patches\\WindowsUpdate_Patch";

        private void RestartedUpdate3_Load(object sender, EventArgs e)
        {
                this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            System.IO.Directory.CreateDirectory("C:\\Program Files\\VistaUpdater\\Patches");
            listBox1.Items.Add("Windows Update パッチをインストールしています... 1/2");
            Extract("VistaUpdater", "C:\\Program Files\\VistaUpdater\\Patches", "Resources", "Vista_SHA2_WUC.zip");
            Extract("VistaUpdater", "C:\\Program Files\\VistaUpdater\\Patches", "Resources", "PsExec.exe");
            try
            {
                MemoryStream ms = new MemoryStream();
                StreamWriter sw = new StreamWriter(ms);

                ReadOptions options = new ReadOptions();
                options.StatusMessageWriter = sw;
                ZipFile zf = ZipFile.Read("C:\\Program Files\\VistaUpdater\\Patches\\Vista_SHA2_WUC.zip", options);

                zf.ExtractAll(extractPath);

                ms.Seek(0, 0);
            } catch (Exception)
            {
                
            }

            update();
        }

        private void update()
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            if ((ushort)new ManagementObject("Win32_Processor.DeviceID='CPU0'")["AddressWidth"] == 32)
            {
                proc.StartInfo.FileName = "C:\\Program Files\\VistaUpdater\\Patches\\WindowsUpdate_Patch\\32\\NSudoLC.exe";
            }
            else
            {
                proc.StartInfo.FileName = "C:\\Program Files\\VistaUpdater\\Patches\\WindowsUpdate_Patch\\64\\NSudoLC.exe";
            }
            proc.StartInfo.Arguments = " -U:t \"" + extractPath + "\\Install_WUC.cmd\"";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.EnableRaisingEvents = true;
            proc.SynchronizingObject = this;
            proc.Exited += p_Exited;
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

        private void p_Exited(object sender, EventArgs e)
        {
            var ProcessExited = false;
            for(int i = 0; i < double.PositiveInfinity; i++) {
                Process[] pname = Process.GetProcessesByName("cmd");
                if (pname.Length == 0 & ProcessExited == false)
                {
                    ProcessExited = true;
                    listBox1.Items.Add("Windows Update パッチのインストールを続行するために、再起動します。");
                    label2.Text = "最終処理の実行中...";
                    listBox1.Items.Add("Shellの設定中...");
                    Microsoft.Win32.RegistryKey regkey1 =
                        Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\VistaUpdater");
                    regkey1.SetValue("Restarted", 4, Microsoft.Win32.RegistryValueKind.QWord);
                    regkey1.Close();
                    listBox1.Items.Add("Shellの設定が完了しました");
                    listBox1.Items.Add("再起動中...");
                    label2.Text = "再起動しています...";
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = "shutdown.exe";
                    p.StartInfo.Arguments = "/r /t 0 /c \"VistaUpdater の処理を続行するため、再起動します。\"";
                    p.Start();
                }
            }
        }

        private void RestartedUpdate3_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
