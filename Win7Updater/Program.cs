using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Win7Updater
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Microsoft.Win32.RegistryKey regkey =
                 Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Win7Updater", false);
            if (regkey == null)
            {
                Application.Run(new Form1());
                return;
            }

            try
            {
                Microsoft.Win32.RegistryKey regkey1 =
        Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Win7Updater");
                long SPInstalled = (long)regkey1.GetValue("SP1Installed");

                if (SPInstalled == 1)
                {
                    regkey1.DeleteValue("SP1Installed");
                    Application.Run(new StartUpdate());
                    regkey1.Close();
                    return;
                }

                regkey1.Close();
            }
            catch (Exception)
            {
                Microsoft.Win32.RegistryKey regkey1 =
Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Win7Updater");
                long Restarted = (long)regkey1.GetValue("Restarted");
                regkey1.Close();

                if (Restarted == 1)
                {
                    Application.Run(new RestartedUpdate());
                    return;
                } else
                {
                    Application.Run(new Form1());
                }
            }
        }
    }
}
