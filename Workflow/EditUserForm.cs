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
    public partial class EditUserForm : Form
    {
        public EditUserForm()
        {
            InitializeComponent();
        }

        private void EditUserForm_Load(object sender, EventArgs e)
        {
            // load user names
            userNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "SELECT [userName]\n" +
                    "FROM[ATI_Workflow].[dbo].[UserData]";

                OdbcCommand com = new OdbcCommand(query, conn);
                OdbcDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                        userNameComboBox.Items.Add(reader.GetString(0));
                }
            }

            // set up format checkers for text
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
            if (userNameComboBox.SelectedItem == null)
            {
                MessageBox.Show("Invalid username");
                return;
            }

            // validate form
            if (!IsFormValid())
                return;

            // push to db
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "UPDATE [ATI_Workflow].[dbo].[UserData]\n" +
                    "SET\n" +
                    "[encryptedPassword] = '" + passwordTextBox.Text + "'\n" +
                    ",[CustomerService] = " + (customerServiceCheckBox.Checked ? 1 : 0) + "\n" +
                    ",[QA] = " + (qaCheckBox.Checked ? 1 : 0) + "\n" +
                    ",[QE] = " + (qeCheckBox.Checked ? 1 : 0) + "\n" +
                    ",[Lead] = " + (leadCheckBox.Checked ? 1 : 0) + "\n" +
                    ",[ME] = " + (meCheckBox.Checked ? 1 : 0) + "\n" +
                    "WHERE userName = '" + userNameComboBox.SelectedItem.ToString() + "';";

                OdbcCommand com = new OdbcCommand(query, conn);
                if (com.ExecuteNonQuery() != 1)
                {
                    MessageBox.Show(Globals.generic_IT_error);
                    return;
                }
                else
                    MessageBox.Show("User account succesfully updated!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (userNameComboBox.SelectedItem == null)
            {
                MessageBox.Show("Invalid username");
                return;
            }

            // double check with user
            if (MessageBox.Show("If you proceed the user account will be permanently deleted. Are you sure you want to proceed?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                // push to db
                using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                {
                    conn.Open();

                    string query =
                        "DELETE FROM [ATI_Workflow].[dbo].[UserData]\n" +
                        "WHERE userName = '" + userNameComboBox.SelectedItem.ToString() + "';";

                    OdbcCommand com = new OdbcCommand(query, conn);
                    if (com.ExecuteNonQuery() != 1)
                    {
                        MessageBox.Show(Globals.generic_IT_error);
                        return;
                    }
                    else
                        MessageBox.Show("User account succesfully deleted!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.Close();
            }


        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void userNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userNameComboBox.SelectedItem == null)
                return;

            // load info
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "SELECT\n" +
                    "[firstName]\n" +
                    ",[lastName]\n" +
                    ",[CustomerService]\n" +
                    ",[QA]\n" +
                    ",[QE]\n" +
                    ",[Lead]\n" +
                    ",[ME]\n" +
                    "FROM[ATI_Workflow].[dbo].[UserData]\n" +
                    "WHERE userName = '" + userNameComboBox.SelectedItem.ToString() + "';";

                OdbcCommand com = new OdbcCommand(query, conn);
                OdbcDataReader reader = com.ExecuteReader();

                if (reader.Read())
                {
                    firstNameTextBox.Text = reader.GetString(0);
                    lastNameTextBox.Text = reader.GetString(1);
                    customerServiceCheckBox.Checked = reader.GetBoolean(2);
                    qaCheckBox.Checked = reader.GetBoolean(3);
                    qeCheckBox.Checked = reader.GetBoolean(4);
                    leadCheckBox.Checked = reader.GetBoolean(5);
                    meCheckBox.Checked = reader.GetBoolean(6);
                }
            }
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