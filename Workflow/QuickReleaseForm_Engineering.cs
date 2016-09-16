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
    public partial class QuickReleaseForm_Engineering : Form
    {
        string jobNo;
        string partNo;
        int workflow_ID;
        public bool been_updated
        {
            get; set;
        }
        DateTime startedOn;
        bool closedFromButton;
        bool readOnly;
        bool hold;
        string startingStatus;
        bool firstOpen;
        bool modified;
        bool canComplete;

        public QuickReleaseForm_Engineering(string jobNo, string partNo, int workflow_ID, bool readOnly, bool hold, string startingStatus, bool canComplete)
        {
            InitializeComponent();
            this.jobNo = jobNo;
            this.partNo = partNo;
            this.workflow_ID = workflow_ID;
            this.been_updated = false;
            this.startedOn = DateTime.Now;
            this.closedFromButton = false;
            this.readOnly = readOnly;
            this.hold = hold;
            this.startingStatus = startingStatus;
            this.firstOpen = false;
            this.modified = false;
            this.canComplete = canComplete;
        }

        private void QuickReleaseForm_Load(object sender, EventArgs e)
        {
            // add set up for all combo boxes
            SetUpDropdown(question5ComboBox);
            SetUpDropdown(question4ComboBox);
            SetUpDropdown(question6ComboBox);
            SetUpDropdown(question1ComboBox);
            SetUpDropdown(question2ComboBox);
            SetUpDropdown(question3ComboBox);
            SetUpDropdown(question7ComboBox);
            SetUpDropdown(question8ComboBox);
            SetUpDropdown(question9ComboBox);

            // initialize texboxes
            jobTextBox.Text = jobNo;
            jobTextBox.Enabled = false;
            partNumberTextBox.Text = partNo;
            partNumberTextBox.Enabled = false;
            statusLabel.Text = "Status: " + startingStatus;

            // check if it exists in DB and load data
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "SELECT [Job]\n" +
                    ",[Workflow_ID]\n" +
                    ",[Person]\n" +
                    ",[Question1_Response]\n" +
                    ",[Question2_Response]\n" +
                    ",[Question3_Response]\n" +
                    ",[Question4_Response]\n" +
                    ",[Question5_Response]\n" +
                    ",[Question6_Response]\n" +
                    ",[Question7_Response]\n" +
                    ",[Question8_Response]\n" +
                    ",[Question9_Response]\n" +
                    ",[Comments]\n" +
                    ",[Status]\n" +
                    ",[Status_Subtype]\n" +
                    ",[Status_Notes]\n" +
                    ",[StartedOn]" +
                    "FROM[ATI_Workflow].[dbo].[QuickRelease_Engineering]\n" +
                    "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "'; ";

                OdbcCommand com = new OdbcCommand(query, conn);
                OdbcDataReader reader = com.ExecuteReader();

                if (reader.Read())
                {
                    question1ComboBox.SelectedItem = reader.IsDBNull(3) ? null : reader.GetString(3);
                    question2ComboBox.SelectedItem = reader.IsDBNull(4) ? null : reader.GetString(4);
                    question3ComboBox.SelectedItem = reader.IsDBNull(5) ? null : reader.GetString(5);
                    question4ComboBox.SelectedItem = reader.IsDBNull(6) ? null : reader.GetString(6);
                    question5ComboBox.SelectedItem = reader.IsDBNull(7) ? null : reader.GetString(7);
                    question6ComboBox.SelectedItem = reader.IsDBNull(8) ? null : reader.GetString(8);
                    question7ComboBox.SelectedItem = reader.IsDBNull(9) ? null : reader.GetString(9);
                    question8ComboBox.SelectedItem = reader.IsDBNull(10) ? null : reader.GetString(10);
                    question9ComboBox.SelectedItem = reader.IsDBNull(11) ? null : reader.GetString(11);

                    remarksTextBox.Text = reader.IsDBNull(12) ? "" : reader.GetString(12);

                    // check about updated status subtype dropdown
                    statusLabel.Text = "Status: " + (reader.IsDBNull(13) ? string.Empty : reader.GetString(13));

                    // check if StartedOn is NULL, this will tell you if it's the first time
                    firstOpen = reader.IsDBNull(16);
                }
            }

            // fix page to left half
            this.Left = 0;
            this.Top = 0;
            this.Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;

            // if readonly disable buttons and checkbox edits
            if (readOnly)
                DisableUserInputControls(this);

            // if on hold then make the button be "remove off Hold"
            if (hold)
            {
                holdButton.Text = "Remove Hold";

                // disable all
                DisableUserInputControls(this);

                // enable hold combo box and hold button
                holdButton.Enabled = true;
            }

            // set up all 
            if (!readOnly)
            {
                foreach (var cb in GetAllChildren(this).OfType<ComboBox>())
                    if (!cb.Name.Equals("holdComboBox"))
                        cb.SelectedIndexChanged += FormatCheckInvalidComboBox;
            }

            // add modified check
            foreach (var cb in GetAllChildren(this).OfType<ComboBox>())
                if (!cb.Name.Equals("holdComboBox"))
                    cb.SelectedIndexChanged += (object sender1, EventArgs e1) => { modified = true; };
            remarksTextBox.TextChanged += (object sender1, EventArgs e1) => { modified = true; };
        }

        private void DisableUserInputControls(Control container)
        {
            // disable all texboxes
            //disable all checkboxes
            //disable all buttons
            // disable all dropdowns

            foreach (Control c in container.Controls)
            {
                DisableUserInputControls(c);

                if (c is TextBox)
                    (c as TextBox).ReadOnly = true;
                else if (c is CheckBox)
                    (c as CheckBox).Enabled = false;
                else if (c is Button)
                    (c as Button).Enabled = false;
                else if (c is ComboBox)
                    (c as ComboBox).Enabled = false;
            }
        }

        private void SetUpDropdown(ComboBox combo_box)
        {
            // clear items
            combo_box.Items.Clear();

            combo_box.Items.Add("Yes");
            combo_box.Items.Add("No");
            combo_box.Items.Add("N/A");

            combo_box.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // if exists update, if it doesn't insert new row
        private void submitButton_Click(object sender, EventArgs e)
        {
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();
                // if it exists then run an update query
                string query =
                    "UPDATE [ATI_Workflow].[dbo].[QuickRelease_Engineering]\n" +
                    "SET\n" +
                    "[Status] = 'In Progress'\n" +
                    ",[LastUpdatedBy] = '" + Globals.userName + "'\n" +
                    ",[LastUpdatedOn] = '" + DateTime.Now + "'\n" +
                    (firstOpen ? ",[StartedBy] = '" + Globals.userName + "'\n" : string.Empty) +
                    (firstOpen ? ",[StartedOn] = '" + startedOn.ToString() + "'\n" : string.Empty) +
                    ",[Question1_Response] = " + (question1ComboBox.SelectedItem != null ? "'" + question1ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question2_Response] = " + (question2ComboBox.SelectedItem != null ? "'" + question2ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question3_Response] = " + (question3ComboBox.SelectedItem != null ? "'" + question3ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question4_Response] = " + (question4ComboBox.SelectedItem != null ? "'" + question4ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question5_Response] = " + (question5ComboBox.SelectedItem != null ? "'" + question5ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question6_Response] = " + (question6ComboBox.SelectedItem != null ? "'" + question6ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question7_Response] = " + (question7ComboBox.SelectedItem != null ? "'" + question7ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question8_Response] = " + (question8ComboBox.SelectedItem != null ? "'" + question8ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question9_Response] = " + (question9ComboBox.SelectedItem != null ? "'" + question9ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Comments] = '" + remarksTextBox.Text + "'\n" +
                    "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "'; ";

                OdbcCommand com = new OdbcCommand(query, conn);
                int rows = com.ExecuteNonQuery();

                if (rows != 1)
                    MessageBox.Show(Globals.generic_IT_error);
                else
                    been_updated = true;
            }
            closedFromButton = true;
            this.Close();
        }

        private void timestampButton_Click(object sender, EventArgs e)
        {
            remarksTextBox.Text += " " + DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString();
        }

        private void QuickReleaseForm_Engineering_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closedFromButton && !readOnly && modified)
            {
                DialogResult result = MessageBox.Show("Would you like to save the form before closing?", "Close Application", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    this.Activate();
                }
                else if (result == DialogResult.No)
                {
                    closedFromButton = false;
                }
                else
                {
                    this.submitButton_Click(new object(), new EventArgs());
                }
            }
        }

        private void completeButton_Click(object sender, EventArgs e)
        {
            // check if quality step haas been completed
            if (!canComplete)
            {
                MessageBox.Show("QA step has to be completed before ME step can be completed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // check if valid
            if (!IsComplete())
                return;

            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();
                // if it exists then run an update query
                string query =
                    "UPDATE [ATI_Workflow].[dbo].[QuickRelease_Engineering]\n" +
                    "SET\n" +
                    "[Status] = 'Completed'\n" +
                    ",[LastUpdatedBy] = '" + Globals.userName + "'\n" +
                    ",[LastUpdatedOn] = '" + DateTime.Now.ToString() + "'\n" +
                    (firstOpen ? ",[StartedBy] = '" + Globals.userName + "'\n" : string.Empty) +
                    (firstOpen ? ",[StartedOn] = '" + startedOn.ToString() + "'\n" : string.Empty) +
                    ",[CompletedBy] = '" + Globals.userName + "'\n" +
                    ",[CompletedOn] = '" + DateTime.Now.ToString() + "'\n" +
                    ",[Question1_Response] = " + (question1ComboBox.SelectedItem != null ? "'" + question1ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question2_Response] = " + (question2ComboBox.SelectedItem != null ? "'" + question2ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question3_Response] = " + (question3ComboBox.SelectedItem != null ? "'" + question3ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question4_Response] = " + (question4ComboBox.SelectedItem != null ? "'" + question4ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question5_Response] = " + (question5ComboBox.SelectedItem != null ? "'" + question5ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question6_Response] = " + (question6ComboBox.SelectedItem != null ? "'" + question6ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question7_Response] = " + (question7ComboBox.SelectedItem != null ? "'" + question7ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question8_Response] = " + (question8ComboBox.SelectedItem != null ? "'" + question8ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question9_Response] = " + (question9ComboBox.SelectedItem != null ? "'" + question9ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Comments] = '" + remarksTextBox.Text + "'\n" +
                    "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "'; ";

                OdbcCommand com = new OdbcCommand(query, conn);
                int rows = com.ExecuteNonQuery();

                if (rows != 1)
                    MessageBox.Show(Globals.generic_IT_error);
                else
                    been_updated = true;
            }
            closedFromButton = true;
            this.Close();
        }

        private void holdButton_Click(object sender, EventArgs e)
        {
            // ask for comments
            string hold_notes = string.Empty;
            string hold_type = string.Empty;
            if (!hold)
            {
                CommentDialog notes = new CommentDialog("Hold Notes", "Reason to put this item on hold:");
                notes.ShowDialog();

                if (notes.Response == CommentDialog.Response_Type.Cancel)
                {
                    MessageBox.Show("Cannot submit a hold without a note");
                    return;
                }
                else
                {
                    hold_notes = notes.Notes;
                    hold_type = notes.Hold_Type;
                }
            }

            // submt to db
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();
                // if it exists then run an update query
                string query =
                    "UPDATE [ATI_Workflow].[dbo].[QuickRelease_Engineering]\n" +
                    "SET\n" +
                    "[Status] = '" + (hold ? "In Progress" : "Hold") + "'\n" +
                    ",[Status_Subtype] = '" + (hold ? string.Empty : hold_type) + "'\n" +
                    ",[Status_Notes] = '" + (hold ? string.Empty : hold_notes) + "'\n" +
                    ",[LastUpdatedBy] = '" + Globals.userName + "'\n" +
                    ",[LastUpdatedOn] = '" + DateTime.Now + "'\n" +
                    (firstOpen ? ",[StartedBy] = '" + Globals.userName + "'\n" : string.Empty) +
                    (firstOpen ? ",[StartedOn] = '" + startedOn.ToString() + "'\n" : string.Empty) +
                    ",[Question1_Response] = " + (question1ComboBox.SelectedItem != null ? "'" + question1ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question2_Response] = " + (question2ComboBox.SelectedItem != null ? "'" + question2ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question3_Response] = " + (question3ComboBox.SelectedItem != null ? "'" + question3ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question4_Response] = " + (question4ComboBox.SelectedItem != null ? "'" + question4ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question5_Response] = " + (question5ComboBox.SelectedItem != null ? "'" + question5ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question6_Response] = " + (question6ComboBox.SelectedItem != null ? "'" + question6ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question7_Response] = " + (question7ComboBox.SelectedItem != null ? "'" + question7ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question8_Response] = " + (question8ComboBox.SelectedItem != null ? "'" + question8ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Question9_Response] = " + (question9ComboBox.SelectedItem != null ? "'" + question9ComboBox.SelectedItem.ToString() + "'" : "NULL") + "\n" +
                    ",[Comments] = '" + remarksTextBox.Text + "'\n" +
                    "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "'; ";

                OdbcCommand com = new OdbcCommand(query, conn);
                int rows = com.ExecuteNonQuery();

                if (rows != 1)
                    MessageBox.Show(Globals.generic_IT_error);
                else
                    been_updated = true;
            }

            // if not on hold then just update the status to on Hold regardless of anything else
            if (!hold)
            {
                // update the status of the workflow to Hold as well
                using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                {
                    conn.Open();

                    string query =
                        "UPDATE [ATI_Workflow].[dbo].[StatusData]\n" +
                        "SET\n" +
                        "[Workflow_Status] = 'Hold'\n" +
                        (hold_type.Equals("Engi") ? ",[Engi_Hold] = 1\n" : string.Empty) +
                        (hold_type.Equals("QA") ? ",[QA_Hold] = 1\n" : string.Empty) +
                        (hold_type.Equals("Cust") ? ",[Cust_Hold] = 1\n" : string.Empty) +
                        "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "';";

                    OdbcCommand com = new OdbcCommand(query, conn);
                    if (com.ExecuteNonQuery() != 1)
                        MessageBox.Show(Globals.generic_IT_error);
                }
            }
            // if form was already on hold then only update to In Progress if all other forms are not on hold
            else
            {
                // check if other forms are on Hold
                bool otherFormsOnHold = false;
                bool cust_hold = false;
                bool qa_hold = false;
                bool engi_hold = false;
                using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                {
                    conn.Open();

                    string query =
                        "SELECT qaT.Status_Subtype, meT.Status_Subtype, qet.Status_Subtype, qreT.Status_Subtype, qrqT.Status_Subtype\n" +
                        "FROM [ATI_Workflow].[dbo].StatusData AS wT\n" +
                        "LEFT JOIN [ATI_Workflow].[dbo].ContractReview_QA AS qaT\n" +
                        "ON qaT.Job = wT.Job AND qaT.Workflow_ID = wt.Workflow_ID\n" +
                        "LEFT JOIN [ATI_Workflow].[dbo].ContractReview_ME_QE AS meT\n" +
                        "ON meT.ContractReview_Type = 'ME' AND wT.Job = meT.job AND wT.Workflow_ID = meT.Workflow_ID\n" +
                        "LEFT JOIN [ATI_Workflow].[dbo].ContractReview_ME_QE AS qeT\n" +
                        "ON qeT.ContractReview_Type = 'QE' AND wT.Job = qeT.Job AND wT.Workflow_ID = qeT.Workflow_ID\n" +
                        "LEFT JOIN [ATI_Workflow].[dbo].QuickRelease_Engineering AS qreT\n" +
                        "ON qreT.Job = wT.Job AND qreT.Workflow_ID = wT.Workflow_ID\n" +
                        "LEFT JOIN [ATI_Workflow].[dbo].QuickRelease_Quality AS qrqT\n" +
                        "ON qrqT.Job = wT.Job AND qrqT.Workflow_ID = wT.Workflow_ID\n" +
                        "WHERE wT.Job = '" + jobNo + "' AND wT.Workflow_ID = '" + workflow_ID + "' AND\n" +
                        "\t(qaT.Status = 'Hold' OR meT.Status = 'Hold' OR qeT.Status = 'Hold' OR qreT.Status = 'Hold' OR qrqT.Status = 'Hold');";

                    OdbcCommand com = new OdbcCommand(query, conn);
                    OdbcDataReader reader = com.ExecuteReader();
                    otherFormsOnHold = reader.Read(); // if any line exists then something else must be on hold as well
                    if (otherFormsOnHold)
                    {
                        cust_hold = (reader.IsDBNull(0) ? false : reader.GetString(0).Equals("Cust")) ||
                                    (reader.IsDBNull(1) ? false : reader.GetString(1).Equals("Cust")) ||
                                    (reader.IsDBNull(2) ? false : reader.GetString(2).Equals("Cust")) ||
                                    (reader.IsDBNull(3) ? false : reader.GetString(3).Equals("Cust")) ||
                                    (reader.IsDBNull(4) ? false : reader.GetString(4).Equals("Cust"));

                        engi_hold = (reader.IsDBNull(0) ? false : reader.GetString(0).Equals("Engi")) ||
                                    (reader.IsDBNull(1) ? false : reader.GetString(1).Equals("Engi")) ||
                                    (reader.IsDBNull(2) ? false : reader.GetString(2).Equals("Engi")) ||
                                    (reader.IsDBNull(3) ? false : reader.GetString(3).Equals("Engi")) ||
                                    (reader.IsDBNull(4) ? false : reader.GetString(4).Equals("Engi"));

                        qa_hold = (reader.IsDBNull(0) ? false : reader.GetString(0).Equals("QA")) ||
                                    (reader.IsDBNull(1) ? false : reader.GetString(1).Equals("QA")) ||
                                    (reader.IsDBNull(2) ? false : reader.GetString(2).Equals("QA")) ||
                                    (reader.IsDBNull(3) ? false : reader.GetString(3).Equals("QA")) ||
                                    (reader.IsDBNull(4) ? false : reader.GetString(4).Equals("QA"));
                    }
                }


                // update the status of the workflow to Hold as well
                using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                {
                    conn.Open();

                    string query =
                        "UPDATE [ATI_Workflow].[dbo].[StatusData]\n" +
                        "SET\n" +
                        "[Workflow_Status] = '" + (otherFormsOnHold ? "Hold" : "In Progress") + "'\n" +
                        ",[Engi_Hold] = " + (engi_hold ? 1 : 0) + "\n" +
                        ",[Cust_Hold] =  " + (cust_hold ? 1 : 0) + "\n" +
                        ",[QA_Hold] =  " + (qa_hold ? 1 : 0) + "\n" +
                        "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "';";

                    OdbcCommand com = new OdbcCommand(query, conn);
                    if (com.ExecuteNonQuery() != 1)
                        MessageBox.Show(Globals.generic_IT_error);
                }
            }

            if (hold)
            {
                holdButton.Text = "Hold";

                foreach (var cb in GetAllChildren(this).OfType<ComboBox>())
                    cb.Enabled = true;

                foreach (var cb in GetAllChildren(this).OfType<Button>())
                    cb.Enabled = true;

                hold = !hold;
            }
            else
            {
                closedFromButton = true;
                this.Close();
            }
        }

        // get all controls
        public IEnumerable<Control> GetAllChildren(Control root)
        {
            var stack = new Stack<Control>();
            stack.Push(root);

            while (stack.Any())
            {
                var next = stack.Pop();
                foreach (Control child in next.Controls)
                    stack.Push(child);
                yield return next;
            }
        }

        // all drop downs must yes or n/a
        private bool IsComplete()
        {
            bool valid = true;
            foreach (var cb in GetAllChildren(this).OfType<ComboBox>())
                if ((cb.SelectedItem == null || cb.SelectedItem.ToString().Equals("")) && !cb.Name.Equals("holdComboBox"))
                {
                    errorProvider.SetError(cb, "Invalid Selection");
                    if (valid)
                        valid = false;
                }

            return valid;
        }

        private void FormatCheckInvalidComboBox(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            if (comboBox.SelectedItem == null || comboBox.SelectedItem.ToString().Equals(""))
                errorProvider.SetError(comboBox, "Invalid Selection");
            else
                errorProvider.SetError(comboBox, "");
        }
    }
}
