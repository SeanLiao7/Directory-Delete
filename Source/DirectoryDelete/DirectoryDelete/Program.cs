using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace DirectoryDelete
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main ( )
        {
            var isCreate = false;
            var processName = Process.GetCurrentProcess ( ).ProcessName;

            using ( var mutex = new Mutex ( true, processName, out isCreate ) )
            {
                if ( isCreate )
                {
                    Application.EnableVisualStyles ( );
                    Application.SetCompatibleTextRenderingDefault ( false );
                    Application.Run ( new DirectoryDeleteForm ( ) );
                }
                else
                    MessageBox.Show ( "程式重複開啟！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification );
            }
        }
    }
}
