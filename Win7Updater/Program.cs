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

                int SPInstalled = (int)regkey.GetValue("SP1Installed");
                
                if (SPInstalled == 1)
                {
                    Application.Run(new StartUpdate());
                    regkey.DeleteValue("SP1Installed");
                    regkey.Close();
                    return;
                } else
                {
                    regkey.DeleteValue("SP1Installed");
                    Application.Run(new StartUpdate());
                }

                regkey.Close();
            }
            catch (Exception ex)
            {
                int Restarted = (int)regkey.GetValue("Restarted");
                regkey.Close();

                if (Restarted == 1)
                {
                    Application.Run(new RestartedUpdate());
                    return;
                }
                else
                {
                    Application.Run(new Form1());
                }
            }
        }
    }
}
