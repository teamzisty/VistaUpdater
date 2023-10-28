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
            Microsoft.Win32.RegistryKey regkey1 =
                 Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Win7Updater", false);
            if (regkey1 == null)
            {
                Application.Run(new Form1());
                return;
            }
            long s = (long)regkey1.GetValue("Restarted");
            regkey1.Close();
            if (s == 1)
            {
                Application.Run(new RestartedUpdate());
            }
        }
    }
}
