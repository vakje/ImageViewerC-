using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsC_app
{
    internal class Program
    {
        [STAThread]
        public static void Main (string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           

            if(args.Length == 0)
            {
              
               Application.Run(new Form1());
            }
            else if(args.Length == 1)
            {
                
                string dirpath = args[0];
               
                Application.Run(new Form1(dirpath));
            }
        }
    }
}
