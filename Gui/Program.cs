using System;

using System.Windows.Forms;

namespace GenArt
{
    static class Program
    {
        /// <summary>
        /// main entry point
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
