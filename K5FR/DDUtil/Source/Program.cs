using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace DataDecoder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
//            Debug.Assert(false);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Setup());
        }
    }
}