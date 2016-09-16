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
    public partial class UserRegistrationForm : Form
    {
        public UserRegistrationForm()
        {
            InitializeComponent();
        }

        private void UserRegistrationForm_Load(object sender, EventArgs e)
        {
            firstNameTextBox.TextChanged += Update_UserNameTextBox;
            lastNameTextBox.TextChanged += Update_UserNameTextBox;

            // set up format checkers for text
            firstNameTextBox.TextChanged += FormatCheckInvalidTextBox;
            lastNameTextBox.TextChanged += FormatCheckInvalidTextBox;
            passwordTextBox.TextChanged += FormatCheckInvalidTextBox;

            // set up format checkers for check boxes
            customerServiceCheckBox.CheckedChanged += this.FormatCheckInvalidCheckBox;
            qaCheckBox.CheckedChanged += this.FormatCheckInvalidCheckBox;
            qeCheckBox.CheckedChanged += this.FormatCheckInvalidCheckBox;
            leadCheckBox.CheckedChanged += this.FormatCheckInvalidCheckBox;
            meCheckBox.CheckedChanged += this.FormatCheckInvalidCheckBox;
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            // validate form
            if (!IsFormValid())
                return;

            // check that username doesn't exist
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "SELECT *\n" +
                    "FROM[ATI_Workflow].[dbo].[UserData]\n" +
                    "WHERE userName = '" + userNameTextBox.Text + "';";

                OdbcCommand com = new OdbcCommand(query, conn);
                OdbcDataReader reader = com.ExecuteReader();

                // if row exists then user already exists, exit submission step
                if (reader.Read())
                {
                    MessageBox.Show("User already exists");
                    return;
                }
            }

            // push to db
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "INSERT INTO [ATI_Workflow].[dbo].[UserData]\n" +
                    "([userName]\n" +
                    ",[encryptedPassword]\n" +
                    ",[firstName]\n" +
                    ",[lastName]\n" +
                    ",[admin]\n" +
                    ",[CustomerService]\n" +
                    ",[QA]\n" +
                    ",[QE]\n" +
                    ",[Lead]\n" +
                    ",[ME])" +
                    "VALUES\n" +
                    "('" + userNameTextBox.Text + "'\n" +
                    ", '" + passwordTextBox.Text + "'\n" +
                    ", '" + firstNameTextBox.Text + "'\n" +
                    ", '" + lastNameTextBox.Text + "'\n" +
                    ", 0\n" +
                    "," + (customerServiceCheckBox.Checked ? 1 : 0) + "\n" +
                    "," + (qaCheckBox.Checked ? 1 : 0) + "\n" +
                    "," + (qeCheckBox.Checked ? 1 : 0) + "\n" +
                    "," + (leadCheckBox.Checked ? 1 : 0) + "\n" +
                    "," + (meCheckBox.Checked ? 1 : 0) + ");";

                OdbcCommand com = new OdbcCommand(query, conn);
                if (com.ExecuteNonQuery() != 1)
                {
                    MessageBox.Show(Globals.generic_IT_error);
                    return;
                }
                else
                    MessageBox.Show("User account succesfully created!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // close form
            this.Close();
        }

        private void Update_UserNameTextBox(object sender, EventArgs e)
        {
            TextBox tBox = (TextBox)sender;

            if (firstNameTextBox.Text.Trim().Length > 0)
                userNameTextBox.Text = firstNameTextBox.Text.ElementAt(0).ToString().ToLower() + lastNameTextBox.Text.ToString().ToLower();
            else
                userNameTextBox.Text = string.Empty;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormatCheckInvalidTextBox(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            // check for empty spaces
            if (textBox.Text.Contains(" "))
                errorProvider.SetError(textBox, "White spaces are not allowed");
            else if (textBox.Text.Length == 0)
                errorProvider.SetError(textBox, "Cannot leave empty");
            else
                errorProvider.SetError(textBox, "");
        }

        private void FormatCheckInvalidCheckBox(object sender, EventArgs e)
        {
            if (customerServiceCheckBox.Checked || qaCheckBox.Checked || qeCheckBox.Checked || leadCheckBox.Checked || meCheckBox.Checked)
                errorProvider.SetError(meCheckBox, "");
            else
                errorProvider.SetError(meCheckBox, "Selection Required");
        }

        private bool IsFormValid()
        {
            bool valid = true;

            // check textboxes
            if (firstNameTextBox.Text.Contains(" "))
            {
                errorProvider.SetError(firstNameTextBox, "White spaces are not allowed");
                valid = false;
            }
            else if (firstNameTextBox.Text.Length == 0)
            {
                errorProvider.SetError(firstNameTextBox, "Cannot leave empty");
                valid = false;
            }

            if (lastNameTextBox.Text.Contains(" "))
            {
                errorProvider.SetError(lastNameTextBox, "White spaces are not allowed");
                valid = false;
            }
            else if (lastNameTextBox.Text.Length == 0)
            {
                errorProvider.SetError(lastNameTextBox, "Cannot leave empty");
                valid = false;
            }

            if (passwordTextBox.Text.Contains(" "))
            {
                errorProvider.SetError(passwordTextBox, "White spaces are not allowed");
                valid = false;
            }
            else if (passwordTextBox.Text.Length == 0)
            {
                errorProvider.SetError(passwordTextBox, "Cannot leave empty");
                valid = false;
            }

            // check check boxes
            if (!(customerServiceCheckBox.Checked || qaCheckBox.Checked || qeCheckBox.Checked || leadCheckBox.Checked || meCheckBox.Checked))
            {
                errorProvider.SetError(meCheckBox, "Selection Required");
                valid = false;
            }


            return valid;
        }
    }
}
