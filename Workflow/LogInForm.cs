using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;

namespace Workflow
{
    public partial class LogInForm : Form
    {
        public LogInForm()
        {
            InitializeComponent();
        }

        private void LogInDialog_Load(object sender, EventArgs e)
        {
            // makeas the character for the password field be an asterix
            passwordTextBox.PasswordChar = '*';
            // set up windows user log in
            userLabel.Text = Environment.UserName;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool VerifyLogin()
        {
            if (userNameTextBox.Text.Trim().Length == 0 && passwordTextBox.Text.Trim().Length == 0)
                return false;

            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "SELECT *\n" +
                    "FROM ATI_Workflow.dbo.UserData\n" +
                    "WHERE userName = '" + userNameTextBox.Text.Trim() + "' AND encryptedPassword = '" + passwordTextBox.Text.Trim() + "';";

                OdbcCommand com = new OdbcCommand(query, conn);
                OdbcDataReader reader = com.ExecuteReader();

                return reader.Read();
            }
        }

        private void OpenListViewer()
        {
            // open job list viewer
            this.Hide();
            Form jobList = new JobListViewer();
            jobList.FormClosed += (s, args) => this.Close();
            jobList.Show();
        }

        private void fillGlobalData(string userName)
        {
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "SELECT\n" +
                    "admin\n" +
                    ",[CustomerService]\n" +
                    ",[QA]\n" +
                    ",[QE]\n" +
                    ",[Lead]\n" +
                    ",[ME]\n" +
                    "FROM [ATI_Workflow].[dbo].[UserData]\n" +
                    "WHERE userName = '" + userName + "';";

                OdbcCommand com = new OdbcCommand(query, conn);
                OdbcDataReader reader = com.ExecuteReader();

                if (reader.Read())
                {
                    Globals.userName = userName;
                    Globals.admin = reader.IsDBNull(0) ? false : reader.GetBoolean(0);
                    Globals.customerServiceAccess = reader.IsDBNull(1) ? false : reader.GetBoolean(1);
                    Globals.qaAccess = reader.IsDBNull(2) ? false : reader.GetBoolean(2);
                    Globals.qeAccess = reader.IsDBNull(3) ? false : reader.GetBoolean(3);
                    Globals.leadAccess = reader.IsDBNull(4) ? false : reader.GetBoolean(4);
                    Globals.meAccess = reader.IsDBNull(5) ? false : reader.GetBoolean(5);
                }
                else // user doesn't exist in db then no admin
                {
                    Globals.userName = userName;
                    Globals.admin = false;
                    Globals.customerServiceAccess = false;
                    Globals.qaAccess = false;
                    Globals.qeAccess = false;
                    Globals.leadAccess = false;
                    Globals.meAccess = false;
                }
            }
        }

        private void logInButton_Click(object sender, EventArgs e)
        {
            if (VerifyLogin())
            {
                // set user name in globals
                fillGlobalData(userNameTextBox.Text.Trim());

                // open job list viewer
                OpenListViewer();
            }
            else
                MessageBox.Show("Username or Password is incorrect");
        }

        private void userNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (VerifyLogin())
                {
                    // set user name in globals
                    fillGlobalData(userNameTextBox.Text.Trim());

                    // open job list viewer
                    OpenListViewer();
                }
                else
                    MessageBox.Show("Username or Password is incorrect");
            }
        }

        private void passwordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (VerifyLogin())
                {
                    // set user name in globals
                    fillGlobalData(userNameTextBox.Text.Trim());
                    // oepn up
                    OpenListViewer();
                }
                else
                    MessageBox.Show("Username or Password is incorrect");
            }
        }

        private void winLogInButton_Click(object sender, EventArgs e)
        {
            fillGlobalData(Environment.UserName);

            // open job list viewer
            OpenListViewer();
        }
    }
}