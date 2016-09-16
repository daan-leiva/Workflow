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
    public partial class ContractReviewCheckList_QA : Form
    {
        string jobNo;
        string partNo;
        int workflow_ID;
        public bool been_updated { get; set; }
        float[] rowSizes = new float[12];
        DateTime startedOn;
        bool closedFromButton;
        bool readOnly;
        bool hold;
        string startingStatus;
        bool firstOpen;
        bool modified;

        public ContractReviewCheckList_QA(string jobNo, string partNo, int workflow_ID, bool readOnly, bool hold, string startingStatus)
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
        }

        private void ContractReviewCheckList_QA_Load(object sender, EventArgs e)
        {
            // initialize some textboxes
            jobNoTextBox.Text = jobNo;
            jobNoTextBox.Enabled = false;
            partNoTextBox.Text = partNo;
            partNoTextBox.Enabled = false;

            statusLabel.Text = "Status: " + startingStatus;

            // Set Up Combo Boxes
            SetUpComboBoxes(this);

            for (int i = 1; i < 12; i += 2)
            {
                rowSizes[i] = tableLayoutPanel1.RowStyles[i].Height;
                tableLayoutPanel1.RowStyles[i].Height = 0;
            }

            // check if it exists in DB and load data
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "SELECT\n" +
                    "'Filler'\n" +
                    ",'Filler'\n" +
                    ",'Filler'\n" +
                    ",'Filler'\n" +
                    ",'Filler'\n" +
                    ",[Question1_Response]\n" +
                    ",[Question2_Response]\n" +
                    ",[Question3_Response]\n" +
                    ",[Question4_Response]\n" +
                    ",[Question5_Response]\n" +
                    ",[Question6_Response]\n" +
                    ",[Question1_Comments]\n" +
                    ",[Question2_Comments]\n" +
                    ",[Question3_Comments]\n" +
                    ",[Question4_Comments]\n" +
                    ",[Question5_Comments]\n" +
                    ",[Question6_Comments]\n" +
                    ",[Status]\n" +
                    ",[Status_Subtype]\n" +
                    ",[Status_Notes]\n" +
                    ",[StartedOn]\n" +
                    "FROM[ATI_Workflow].[dbo].[ContractReview_QA]\n" +
                    "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "';";

                OdbcCommand com = new OdbcCommand(query, conn);
                OdbcDataReader reader = com.ExecuteReader();

                if (reader.Read())
                {
                    question1ComboBox.SelectedIndex = question1ComboBox.FindString(reader.IsDBNull(5) ? string.Empty : reader.GetString(5));
                    question2ComboBox.SelectedIndex = question2ComboBox.FindString(reader.IsDBNull(6) ? string.Empty : reader.GetString(6));
                    question3ComboBox.SelectedIndex = question3ComboBox.FindString(reader.IsDBNull(7) ? string.Empty : reader.GetString(7));
                    question4ComboBox.SelectedIndex = question4ComboBox.FindString(reader.IsDBNull(8) ? string.Empty : reader.GetString(8));
                    question5ComboBox.SelectedIndex = question5ComboBox.FindString(reader.IsDBNull(9) ? string.Empty : reader.GetString(9));
                    question6ComboBox.SelectedIndex = question6ComboBox.FindString(reader.IsDBNull(10) ? string.Empty : reader.GetString(10));

                    question1TextBox.Text = reader.IsDBNull(11) ? "" : reader.GetString(11);
                    question2TextBox.Text = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    question3TextBox.Text = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    question4TextBox.Text = reader.IsDBNull(14) ? "" : reader.GetString(14);
                    question5TextBox.Text = reader.IsDBNull(15) ? "" : reader.GetString(15);
                    question6TextBox.Text = reader.IsDBNull(16) ? "" : reader.GetString(16);

                    // check about updated status subtype dropdown
                    statusLabel.Text = "Status: " + (reader.IsDBNull(17) ? string.Empty : reader.GetString(17));

                    // check if StartedOn is NULL, this will tell you if it's the first time
                    firstOpen = reader.IsDBNull(20);
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
            foreach (var cb in GetAllChildren(this).OfType<RadioButton>())
                cb.CheckedChanged += (object sender1, EventArgs e1) => { modified = true; };
            foreach (var cb in GetAllChildren(this).OfType<TextBox>())
                cb.TextChanged += (object sender1, EventArgs e1) => { modified = true; };
            foreach (var cb in GetAllChildren(this).OfType<CheckBox>())
                cb.CheckedChanged += (object sender1, EventArgs e1) => { modified = true; };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            question1TextBox.Visible = !question1TextBox.Visible;
            if (question1TextBox.Visible == false)
            {
                tableLayoutPanel1.RowStyles[1].Height = 0;
            }
            else
                tableLayoutPanel1.RowStyles[1].Height = rowSizes[1];

        }

        private void button2_Click(object sender, EventArgs e)
        {
            question2TextBox.Visible = !question2TextBox.Visible;
            if (question2TextBox.Visible == false)
            {
                tableLayoutPanel1.RowStyles[3].Height = 0;
            }
            else
                tableLayoutPanel1.RowStyles[3].Height = rowSizes[3];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            question3TextBox.Visible = !question3TextBox.Visible;
            if (question3TextBox.Visible == false)
            {
                tableLayoutPanel1.RowStyles[5].Height = 0;
            }
            else
                tableLayoutPanel1.RowStyles[5].Height = rowSizes[5];
        }

        private void button4_Click(object sender, EventArgs e)
        {
            question4TextBox.Visible = !question4TextBox.Visible;
            if (question4TextBox.Visible == false)
            {
                tableLayoutPanel1.RowStyles[7].Height = 0;
            }
            else
                tableLayoutPanel1.RowStyles[7].Height = rowSizes[7];
        }

        private void button5_Click(object sender, EventArgs e)
        {
            question5TextBox.Visible = !question5TextBox.Visible;
            if (question5TextBox.Visible == false)
            {
                tableLayoutPanel1.RowStyles[9].Height = 0;
            }
            else
                tableLayoutPanel1.RowStyles[9].Height = rowSizes[9];
        }

        private void button6_Click(object sender, EventArgs e)
        {
            question6TextBox.Visible = !question6TextBox.Visible;
            if (question6TextBox.Visible == false)
            {
                tableLayoutPanel1.RowStyles[11].Height = 0;
            }
            else
                tableLayoutPanel1.RowStyles[11].Height = rowSizes[11];
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

        private void submitButton_Click(object sender, EventArgs e)
        {
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();
                // if it exists then run an update query
                string query =
                    "UPDATE [ATI_Workflow].[dbo].[ContractReview_QA]\n" +
                    "SET\n" +
                    "[Status] = 'In Progress'\n" +
                    ",[LastUpdatedBy] = '" + Globals.userName + "'\n" +
                    ",[LastUpdatedOn] = '" + DateTime.Now + "'\n" +
                    (firstOpen ? ",[StartedBy] = '" + Globals.userName + "'\n" : string.Empty) +
                    (firstOpen ? ",[StartedOn] = '" + startedOn.ToString() + "'\n" : string.Empty) +
                    ",[Question1_Response] = '" + (question1ComboBox.SelectedItem == null ? string.Empty : question1ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question2_Response] = '" + (question2ComboBox.SelectedItem == null ? string.Empty : question2ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question3_Response] = '" + (question3ComboBox.SelectedItem == null ? string.Empty : question3ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question4_Response] = '" + (question4ComboBox.SelectedItem == null ? string.Empty : question4ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question5_Response] = '" + (question5ComboBox.SelectedItem == null ? string.Empty : question5ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question6_Response] = '" + (question6ComboBox.SelectedItem == null ? string.Empty : question6ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question1_Comments] = '" + question1TextBox.Text + "'\n" +
                    ",[Question2_Comments] = '" + question2TextBox.Text + "'\n" +
                    ",[Question3_Comments] = '" + question3TextBox.Text + "'\n" +
                    ",[Question4_Comments] = '" + question4TextBox.Text + "'\n" +
                    ",[Question5_Comments] = '" + question5TextBox.Text + "'\n" +
                    ",[Question6_Comments] = '" + question6TextBox.Text + "'\n" +
                    "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "';";

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

        private void completeButton_Click(object sender, EventArgs e)
        {
            // check if valid
            if (!IsComplete())
                return;

            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();
                // if it exists then run an update query
                string query =
                    "UPDATE [ATI_Workflow].[dbo].[ContractReview_QA]\n" +
                    "SET\n" +
                    "[Status] = 'Completed'\n" +
                    ",[LastUpdatedBy] = '" + Globals.userName + "'\n" +
                    ",[LastUpdatedOn] = '" + DateTime.Now.ToString() + "'\n" +
                    (firstOpen ? ",[StartedBy] = '" + Globals.userName + "'\n" : string.Empty) +
                    (firstOpen ? ",[StartedOn] = '" + startedOn.ToString() + "'\n" : string.Empty) +
                    ",[CompletedBy] = '" + Globals.userName + "'\n" +
                    ",[CompletedOn] = '" + DateTime.Now.ToString() + "'\n" +
                    ",[Question1_Response] = '" + (question1ComboBox.SelectedItem == null ? string.Empty : question1ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question2_Response] = '" + (question2ComboBox.SelectedItem == null ? string.Empty : question2ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question3_Response] = '" + (question3ComboBox.SelectedItem == null ? string.Empty : question3ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question4_Response] = '" + (question4ComboBox.SelectedItem == null ? string.Empty : question4ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question5_Response] = '" + (question5ComboBox.SelectedItem == null ? string.Empty : question5ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question6_Response] = '" + (question6ComboBox.SelectedItem == null ? string.Empty : question6ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question1_Comments] = '" + question1TextBox.Text + "'\n" +
                    ",[Question2_Comments] = '" + question2TextBox.Text + "'\n" +
                    ",[Question3_Comments] = '" + question3TextBox.Text + "'\n" +
                    ",[Question4_Comments] = '" + question4TextBox.Text + "'\n" +
                    ",[Question5_Comments] = '" + question5TextBox.Text + "'\n" +
                    ",[Question6_Comments] = '" + question6TextBox.Text + "'\n" +
                    "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "';";

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

        private void ContractReviewCheckList_QA_FormClosing(object sender, FormClosingEventArgs e)
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

            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();
                // if it exists then run an update query
                string query =
                    "UPDATE [ATI_Workflow].[dbo].[ContractReview_QA]\n" +
                    "SET\n" +
                    "[Status] = '" + (hold ? "In Progress" : "Hold") + "'\n" +
                    ",[Status_Subtype] = '" + (hold ? string.Empty : hold_type) + "'\n" +
                    ",[Status_Notes] = '" + (hold ? string.Empty : hold_notes) + "'\n" +
                    ",[LastUpdatedBy] = '" + Globals.userName + "'\n" +
                    ",[LastUpdatedOn] = '" + DateTime.Now + "'\n" +
                    (firstOpen ? ",[StartedBy] = '" + Globals.userName + "'\n" : string.Empty) +
                    (firstOpen ? ",[StartedOn] = '" + startedOn.ToString() + "'\n" : string.Empty) +
                    ",[Question1_Response] = '" + (question1ComboBox.SelectedItem == null ? string.Empty : question1ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question2_Response] = '" + (question2ComboBox.SelectedItem == null ? string.Empty : question2ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question3_Response] = '" + (question3ComboBox.SelectedItem == null ? string.Empty : question3ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question4_Response] = '" + (question4ComboBox.SelectedItem == null ? string.Empty : question4ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question5_Response] = '" + (question5ComboBox.SelectedItem == null ? string.Empty : question5ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question6_Response] = '" + (question6ComboBox.SelectedItem == null ? string.Empty : question6ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question1_Comments] = '" + question1TextBox.Text + "'\n" +
                    ",[Question2_Comments] = '" + question2TextBox.Text + "'\n" +
                    ",[Question3_Comments] = '" + question3TextBox.Text + "'\n" +
                    ",[Question4_Comments] = '" + question4TextBox.Text + "'\n" +
                    ",[Question5_Comments] = '" + question5TextBox.Text + "'\n" +
                    ",[Question6_Comments] = '" + question6TextBox.Text + "'\n" +
                    "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "';";

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

        private void SetUpComboBoxes(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.GetType() == typeof(ComboBox) && !c.Name.Equals("holdComboBox"))
                {
                    ComboBox b = (ComboBox)c;
                    b.Items.Clear();
                    b.Items.Add("");
                    b.Items.Add("Completed");
                    b.Items.Add("N/A");
                    b.DropDownStyle = ComboBoxStyle.DropDownList;
                }
                else {
                    SetUpComboBoxes(c);
                }
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
