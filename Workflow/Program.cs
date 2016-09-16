using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Workflow
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string jobString = 297795.ToString();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
          //  Application.Run(new ContractReviewCheckList_QA(jobString, 001));
            //Application.Run(new ContractReviewCheckList_ME(jobString,001));
            Application.Run(new LogInForm());
        }
    }
}