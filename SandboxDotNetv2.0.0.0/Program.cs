using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SandboxDotNetv2._0._0._0
{
   public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }
    }
}
