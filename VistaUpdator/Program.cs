using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using VistaUpdater.SPInstaller;

namespace VistaUpdater
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
            Microsoft.Win32.RegistryKey regkey1 =
                 Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\VistaUpdater", false);
            if (regkey1 == null)
            {
                Application.Run(new Form1());
                return;
            }
            try
            {
                long r = (long)regkey1.GetValue("SP1Installed");
                if (r == 1)
                {
                    Application.Run(new SP2Installer());
                    return;
                }
                long d = (long)regkey1.GetValue("SP2Installed");
                if (d == 1)
                {
                    Application.Run(new StartUpdate());
                    return;
                }
            }
            catch (Exception)
            {
                try
                {
                    long s = (long)regkey1.GetValue("Restarted");
                    regkey1.Close();
                    if (s == 1)
                    {
                        Application.Run(new RestartedUpdate());
                        return;
                    }
                    else if (s == 2)
                    {
                        Application.Run(new RestartedUpdate2());
                        return;
                    }
                    else if (s == 3)
                    {
                        Application.Run(new RestartedUpdate3());
                        return;
                    }
                    else if (s == 4)
                    {
                        Application.Run(new Final());
                        return;
                    }
                }
                catch (Exception)
                {

                }

                Application.Run(new StartUpdate());
                return;
            }

        }
    }
}
