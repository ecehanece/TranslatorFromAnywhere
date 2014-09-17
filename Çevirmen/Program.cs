using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using HtmlAgilityPack;
using System.Net;
using System.Web;
using System.Diagnostics;
using System.Runtime.InteropServices;



namespace WindowsFormsApplication10
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        

        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

           
            Application.Run(new Form1());

            

        }


       
    }
}
