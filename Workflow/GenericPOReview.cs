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
using System.Net.Mail;

namespace Workflow
{
    public partial class GenericPOReview : Form
    {
        private string jobNo;
        private bool readOnly;
        private bool newJob;
        private int workflow_id;
        public Globals.Status_Type type { get; set; }
        public string partNo;
        JobListViewer jobViewerRef;
        DateTime startTime;
        string customer;
        bool closedFromButton;
        bool modified;

        public GenericPOReview(string jobNo, bool readOnly, int workflow_id, bool newJob, JobListViewer jobViewer, string customer)
        {
            InitializeComponent();
            this.jobNo = jobNo;
            this.readOnly = readOnly;
            this.newJob = newJob;
            this.workflow_id = workflow_id;
            this.type = Globals.Status_Type.Invalid;
            this.jobViewerRef = jobViewer;
            this.startTime = DateTime.Now;
            this.customer = customer;
            this.closedFromButton = false;
            this.modified = false;
        }

        private void GenericPOReview_Load(object sender, EventArgs e)
        {
            // load label values
            if (!newJob)
            {
                jobLabel.Text = jobNo;
                jobLabel.Enabled = false;
            }
            else // autonumber job
            {
                try
                {
                    using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                    {
                        conn.Open();

                        string query =
                            "SELECT\n" +
                            "MAX(CONVERT(NUMERIC, CASE\n" +
                            "WHEN ISNUMERIC(Job) = 1 THEN Job\n" +
                            "WHEN CHARINDEX('R', Job) <> 0 THEN SUBSTRING(Job, 0, CHARINDEX('R', Job))\n" +
                            "WHEN CHARINDEX('-', Job) <> 0 THEN SUBSTRING(Job, 0, CHARINDEX('-', Job))\n" +
                            "WHEN CHARINDEX('C', Job) <> 0 THEN SUBSTRING(Job, 0, CHARINDEX('C', Job))\n" +
                            "WHEN CHARINDEX('A', Job) <> 0 THEN SUBSTRING(Job, 0, CHARINDEX('A', Job))\n" +
                            "END))\n" +
                            "FROM\n" +
                            "(\n" +
                            "SELECT Job, Customer, Part_Number\n" +
                            "FROM PRODUCTION.dbo.Job\n" +
                            "UNION\n" +
                            "SELECT wJ.Job, wJ.Customer, wJ.Part_Number\n" +
                            "FROM ATI_Workflow.dbo.Job AS wJ\n" +
                            ") AS uT\n" +
                            "WHERE Job <> '275927' AND CHARINDEX('D', Job) = 0 AND CHARINDEX('E', Job) = 0\n" +
                            "AND ISNUMERIC(\n" +
                            "CASE\n" +
                            "WHEN ISNUMERIC(Job) = 1 THEN Job\n" +
                            "WHEN CHARINDEX('R', Job) <> 0 THEN SUBSTRING(Job, 0, CHARINDEX('R', Job))\n" +
                            "WHEN CHARINDEX('-', Job) <> 0 THEN SUBSTRING(Job, 0, CHARINDEX('-', Job))\n" +
                            "WHEN CHARINDEX('C', Job) <> 0 THEN SUBSTRING(Job, 0, CHARINDEX('C', Job))\n" +
                            "WHEN CHARINDEX('A', Job) <> 0 THEN SUBSTRING(Job, 0, CHARINDEX('A', Job))\n" +
                            "END\n" +
                            ") = 1";

                        OdbcCommand com = new OdbcCommand(query, conn);
                        OdbcDataReader reader = com.ExecuteReader();

                        if (reader.Read())
                        {
                            jobLabel.Text = (reader.GetInt32(0) + 1).ToString();
                        }
                        else
                        {
                            jobLabel.Text = "10001";
                        }
                    }
                }
                catch (Exception ec)
                {
                    MessageBox.Show(ec.Message);
                }
            }

            initiatedByTextBox.Text = Globals.userName;
            initiatedByTextBox.Enabled = false;

            if (!newJob)
            {
                // query for part number and load
                using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                {
                    conn.Open();

                    string query =
                        "SELECT uT.Part_Number, uT.Rev\n" +
                        "FROM\n" +
                        "(SELECT pjT.Part_Number, pjT.Rev, pjT.Job\n" +
                        "FROM Production.dbo.Job AS pjT\n" +
                        "UNION\n" +
                        "SELECT ajT.Part_Number, ajT.Rev, ajT.Job\n" +
                        "FROM ATI_Workflow.dbo.Job AS ajT\n" +
                        ") AS uT\n" +
                        "WHERE uT.Job = '" + jobLabel.Text.Trim() + "';";

                    OdbcCommand com = new OdbcCommand(query, conn);
                    OdbcDataReader reader = com.ExecuteReader();

                    if (reader.Read())
                    {
                        partNoLabel.Text = reader.GetString(reader.GetOrdinal("Part_Number"));
                        partNoLabel.Enabled = false;

                        revTextBox.Text = reader.IsDBNull(reader.GetOrdinal("Rev")) ? "" : reader.GetString(reader.GetOrdinal("Rev"));
                    }
                }
            }

            // set up combo boxes
            question1ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            question2ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            question3ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            question4ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            question5ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            question6ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            question7ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            subAssemblyChange1ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            subAssemblyChange2ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            subAssemblyChange3ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            subAssemblyChange4ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            subAssemblyChange5ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            question1ComboBox.Items.Add("");
            question1ComboBox.Items.Add("New (Not Started)");
            question1ComboBox.Items.Add("Active (Parts in WPI)");
            question1ComboBox.Items.Add("Completed (Parts in Stock)");

            question2ComboBox.Items.Add("");
            question2ComboBox.Items.Add("Yes");
            question2ComboBox.Items.Add("No");

            question3ComboBox.Items.Add("");
            question3ComboBox.Items.Add("Yes");
            question3ComboBox.Items.Add("No");
            question3ComboBox.Items.Add("N/A");

            question4ComboBox.Items.Add("");
            question4ComboBox.Items.Add("Yes");
            question4ComboBox.Items.Add("No");

            question5ComboBox.Items.Add("");
            question5ComboBox.Items.Add("Yes");
            question5ComboBox.Items.Add("No");

            question6ComboBox.Items.Add("");
            question6ComboBox.Items.Add("Yes");
            question6ComboBox.Items.Add("No");
            question6ComboBox.Items.Add("N/A");

            question7ComboBox.Items.Add("");
            question7ComboBox.Items.Add("Yes");
            question7ComboBox.Items.Add("No");

            subAssemblyChange1ComboBox.Items.Add("N/A");
            subAssemblyChange1ComboBox.Items.Add("Yes");
            subAssemblyChange1ComboBox.Items.Add("No");
            subAssemblyChange1ComboBox.SelectedIndex = subAssemblyChange1ComboBox.FindString("N/A");

            subAssemblyChange2ComboBox.Items.Add("N/A");
            subAssemblyChange2ComboBox.Items.Add("Yes");
            subAssemblyChange2ComboBox.Items.Add("No");
            subAssemblyChange2ComboBox.SelectedIndex = subAssemblyChange2ComboBox.FindString("N/A");

            subAssemblyChange3ComboBox.Items.Add("N/A");
            subAssemblyChange3ComboBox.Items.Add("Yes");
            subAssemblyChange3ComboBox.Items.Add("No");
            subAssemblyChange3ComboBox.SelectedIndex = subAssemblyChange3ComboBox.FindString("N/A");

            subAssemblyChange4ComboBox.Items.Add("N/A");
            subAssemblyChange4ComboBox.Items.Add("Yes");
            subAssemblyChange4ComboBox.Items.Add("No");
            subAssemblyChange4ComboBox.SelectedIndex = subAssemblyChange4ComboBox.FindString("N/A");

            subAssemblyChange5ComboBox.Items.Add("N/A");
            subAssemblyChange5ComboBox.Items.Add("Yes");
            subAssemblyChange5ComboBox.Items.Add("No");
            subAssemblyChange5ComboBox.SelectedIndex = subAssemblyChange5ComboBox.FindString("N/A");

            // disable form if it is readonly
            // if readonly disable buttons and checkbox edits
            if (readOnly)
                DisableUserInputControls(this);

            // set up all
            if (!readOnly)
            {
                foreach (var cb in GetAllChildren(this).OfType<ComboBox>())
                    if (!cb.Name.Contains("subAssemblyChange"))
                        cb.SelectedIndexChanged += FormatCheckInvalidComboBox;

                foreach (var cb in GetAllChildren(this).OfType<TextBox>())
                    if (!cb.Name.Contains("subAssembly") && !cb.Name.Contains("comment") && !cb.Name.Contains("poNoTextBox") && !cb.Name.Contains("poQtyTextBox") && !cb.Name.Contains("jobLabel"))
                        cb.TextChanged += FormatCheckInvalidTextBox;

                foreach (var cb in GetAllChildren(this).OfType<TextBox>())
                    if (cb.Name.Contains("poNoTextBox") || cb.Name.Contains("poQtyTextBox") || cb.Name.Contains("jobLabel"))
                        cb.TextChanged += FormatCheckInvalidNumericTextBox;
            }

            PopulateDataFromDB();

            // add modified check
            foreach (var cb in GetAllChildren(this).OfType<RadioButton>())
                cb.CheckedChanged += (object sender1, EventArgs e1) => { modified = true; };
            foreach (var cb in GetAllChildren(this).OfType<TextBox>())
                cb.TextChanged += (object sender1, EventArgs e1) => { modified = true; };
            foreach (var cb in GetAllChildren(this).OfType<CheckBox>())
                cb.CheckedChanged += (object sender1, EventArgs e1) => { modified = true; };
            foreach (var cb in GetAllChildren(this).OfType<ComboBox>())
                cb.SelectedIndexChanged += (object sender1, EventArgs e1) => { modified = true; };

            // fix page to left half
            this.Left = 0;
            this.Top = 0;
            this.Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
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
                else if (c is RadioButton)
                    (c as RadioButton).Enabled = false;
            }
        }

        private void PopulateDataFromDB()
        {
            // check if this contract review exists on DB
            // load if so
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "SELECT [Job]\n" +
                    ",[Workflow_ID]\n" +
                    ",[PO_No]\n" +
                    ",[PO_Qty]\n" +
                    ",[PO_Rev]\n" +
                    ",[PO_Rev_Date]\n" +
                    ",[Comments]\n" +
                    ",[Description_Of_Change]\n" +
                    ",[Question1_Response]\n" +
                    ",[Question2_Response]\n" +
                    ",[Question3_Response]\n" +
                    ",[Question4_Response]\n" +
                    ",[Question5_Response]\n" +
                    ",[Question6_Response]\n" +
                    ",[Question7_Response]\n" +
                    ",[Part_Number]\n" +
                    ",[Drawing_Rev]\n" +
                    ",[Subassembly_PartNumber_1]\n" +
                    ",[Subassembly_Change_1]\n" +
                    ",[Drawing_Rev_1]\n" +
                    ",[Subassembly_PartNumber_2]\n" +
                    ",[Subassembly_Change_2]\n" +
                    ",[Drawing_Rev_2]\n" +
                    ",[Subassembly_PartNumber_3]\n" +
                    ",[Subassembly_Change_3]\n" +
                    ",[Drawing_Rev_3]\n" +
                    ",[Subassembly_PartNumber_4]\n" +
                    ",[Subassembly_Change_4]\n" +
                    ",[Drawing_Rev_4]\n" +
                    ",[Subassembly_PartNumber_5]\n" +
                    ",[Subassembly_Change_5]\n" +
                    ",[Drawing_Rev_5]\n" +
                    "FROM[ATI_Workflow].[dbo].[Generic_PO_Review]\n" +
                    "WHERE Job = '" + jobLabel.Text.Trim() + "' AND Workflow_ID = '" + workflow_id + "';";

                OdbcCommand com = new OdbcCommand(query, conn);
                OdbcDataReader reader = com.ExecuteReader();

                // if row exists populate DB
                if (reader.Read())
                {
                    poNoTextBox.Text = reader.IsDBNull(reader.GetOrdinal("PO_No")) ? "" : reader.GetString(reader.GetOrdinal("PO_No"));
                    poQtyTextBox.Text = reader.IsDBNull(reader.GetOrdinal("PO_Qty")) ? "" : reader.GetString(reader.GetOrdinal("PO_Qty"));
                    revTextBox.Text = reader.IsDBNull(reader.GetOrdinal("PO_Rev")) ? "" : reader.GetString(reader.GetOrdinal("PO_Rev"));
                    revDateTextBox.Text = reader.IsDBNull(reader.GetOrdinal("PO_Rev_Date")) ? "" : reader.GetString(reader.GetOrdinal("PO_Rev_Date"));
                    descriptionOfChangeTextBox.Text = reader.IsDBNull(reader.GetOrdinal("Description_Of_Change")) ? "" : reader.GetString(reader.GetOrdinal("Description_Of_Change"));
                    commentsTextBox.Text = reader.IsDBNull(reader.GetOrdinal("Comments")) ? "" : reader.GetString(reader.GetOrdinal("Comments"));

                    question1ComboBox.SelectedIndex = question1ComboBox.FindString(reader.IsDBNull(reader.GetOrdinal("Question1_Response")) ? string.Empty : reader.GetString(reader.GetOrdinal("Question1_Response")));
                    question2ComboBox.SelectedIndex = question2ComboBox.FindString(reader.IsDBNull(reader.GetOrdinal("Question2_Response")) ? string.Empty : reader.GetString(reader.GetOrdinal("Question2_Response")));
                    question3ComboBox.SelectedIndex = question3ComboBox.FindString(reader.IsDBNull(reader.GetOrdinal("Question3_Response")) ? string.Empty : reader.GetString(reader.GetOrdinal("Question3_Response")));
                    question4ComboBox.SelectedIndex = question4ComboBox.FindString(reader.IsDBNull(reader.GetOrdinal("Question4_Response")) ? string.Empty : reader.GetString(reader.GetOrdinal("Question4_Response")));
                    question5ComboBox.SelectedIndex = question5ComboBox.FindString(reader.IsDBNull(reader.GetOrdinal("Question5_Response")) ? string.Empty : reader.GetString(reader.GetOrdinal("Question5_Response")));
                    question6ComboBox.SelectedIndex = question6ComboBox.FindString(reader.IsDBNull(reader.GetOrdinal("Question6_Response")) ? string.Empty : reader.GetString(reader.GetOrdinal("Question6_Response")));
                    question7ComboBox.SelectedIndex = question7ComboBox.FindString(reader.IsDBNull(reader.GetOrdinal("Question7_Response")) ? string.Empty : reader.GetString(reader.GetOrdinal("Question7_Response")));

                    partNoLabel.Text = reader.IsDBNull(reader.GetOrdinal("Part_Number")) ? "" : reader.GetString(reader.GetOrdinal("Part_Number"));
                    drawingRevTextBox.Text = reader.IsDBNull(reader.GetOrdinal("Drawing_Rev")) ? "" : reader.GetString(reader.GetOrdinal("Drawing_Rev"));

                    // subassembly parts section
                    subAssemblyPartNumber1TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Subassembly_PartNumber_1")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subassembly_PartNumber_1"));
                    subAssemblyPartNumber2TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Subassembly_PartNumber_2")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subassembly_PartNumber_2"));
                    subAssemblyPartNumber3TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Subassembly_PartNumber_3")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subassembly_PartNumber_3"));
                    subAssemblyPartNumber4TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Subassembly_PartNumber_4")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subassembly_PartNumber_4"));
                    subAssemblyPartNumber5TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Subassembly_PartNumber_5")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subassembly_PartNumber_5"));

                    subAssemblyDrawingRev1TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Drawing_Rev_1")) ? string.Empty : reader.GetString(reader.GetOrdinal("Drawing_Rev_1"));
                    subAssemblyDrawingRev2TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Drawing_Rev_2")) ? string.Empty : reader.GetString(reader.GetOrdinal("Drawing_Rev_2"));
                    subAssemblyDrawingRev3TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Drawing_Rev_3")) ? string.Empty : reader.GetString(reader.GetOrdinal("Drawing_Rev_3"));
                    subAssemblyDrawingRev4TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Drawing_Rev_4")) ? string.Empty : reader.GetString(reader.GetOrdinal("Drawing_Rev_4"));
                    subAssemblyDrawingRev5TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Drawing_Rev_5")) ? string.Empty : reader.GetString(reader.GetOrdinal("Drawing_Rev_5"));

                    subAssemblyChange1ComboBox.SelectedIndex = subAssemblyChange1ComboBox.FindString(reader.IsDBNull(reader.GetOrdinal("Subassembly_Change_1")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subassembly_Change_1")));
                    subAssemblyChange2ComboBox.SelectedIndex = subAssemblyChange2ComboBox.FindString(reader.IsDBNull(reader.GetOrdinal("Subassembly_Change_2")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subassembly_Change_2")));
                    subAssemblyChange3ComboBox.SelectedIndex = subAssemblyChange3ComboBox.FindString(reader.IsDBNull(reader.GetOrdinal("Subassembly_Change_3")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subassembly_Change_3")));
                    subAssemblyChange4ComboBox.SelectedIndex = subAssemblyChange4ComboBox.FindString(reader.IsDBNull(reader.GetOrdinal("Subassembly_Change_4")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subassembly_Change_4")));
                    subAssemblyChange5ComboBox.SelectedIndex = subAssemblyChange5ComboBox.FindString(reader.IsDBNull(reader.GetOrdinal("Subassembly_Change_5")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subassembly_Change_5")));

                }
            }
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (readOnly)
                return;

            if (!IsComplete())
            {
                MessageBox.Show("Form has not been filled out correctly");
                return;
            }

            if (newJob)
            {
                // warn user if job does not match next max value
                using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                {
                    conn.Open();

                    string query =
                        "SELECT\n" +
                        "MAX(CONVERT(NUMERIC, CASE\n" +
                        "WHEN ISNUMERIC(Job) = 1 THEN Job\n" +
                        "WHEN CHARINDEX('R', Job) <> 0 THEN SUBSTRING(Job, 0, CHARINDEX('R', Job))\n" +
                        "WHEN CHARINDEX('-', Job) <> 0 THEN SUBSTRING(Job, 0, CHARINDEX('-', Job))\n" +
                        "WHEN CHARINDEX('C', Job) <> 0 THEN SUBSTRING(Job, 0, CHARINDEX('C', Job))\n" +
                        "WHEN CHARINDEX('A', Job) <> 0 THEN SUBSTRING(Job, 0, CHARINDEX('A', Job))\n" +
                        "END))\n" +
                        "FROM\n" +
                        "(\n" +
                        "SELECT Job, Customer, Part_Number\n" +
                        "FROM PRODUCTION.dbo.Job\n" +
                        "UNION\n" +
                        "SELECT wJ.Job, wJ.Customer, wJ.Part_Number\n" +
                        "FROM ATI_Workflow.dbo.Job AS wJ\n" +
                        ") AS uT\n" +
                        "WHERE Job <> '275927' AND CHARINDEX('D', Job) = 0 AND CHARINDEX('E', Job) = 0\n" +
                        "AND ISNUMERIC(\n" +
                        "CASE\n" +
                        "WHEN ISNUMERIC(Job) = 1 THEN Job\n" +
                        "WHEN CHARINDEX('R', Job) <> 0 THEN SUBSTRING(Job, 0, CHARINDEX('R', Job))\n" +
                        "WHEN CHARINDEX('-', Job) <> 0 THEN SUBSTRING(Job, 0, CHARINDEX('-', Job))\n" +
                        "WHEN CHARINDEX('C', Job) <> 0 THEN SUBSTRING(Job, 0, CHARINDEX('C', Job))\n" +
                        "WHEN CHARINDEX('A', Job) <> 0 THEN SUBSTRING(Job, 0, CHARINDEX('A', Job))\n" +
                        "END\n" +
                        ") = 1";

                    OdbcCommand com = new OdbcCommand(query, conn);
                    OdbcDataReader reader = com.ExecuteReader();

                    if (reader.Read())
                    {
                        if (!jobLabel.Text.Equals((reader.GetInt32(0) + 1).ToString()))
                        {
                            DialogResult result = MessageBox.Show("Warning: This job number does not match the next number in the job sequence. Continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (result == DialogResult.No)
                            {
                                return;
                            }
                        }
                    }
                }

                bool jobExists = true;
                // if job is new then make sure the job number hasn't been used
                using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                {
                    conn.Open();

                    string query =
                        "SELECT *\n" +
                        "FROM (SELECT pJT.Job\n" +
                        "FROM PRODUCTION.dbo.Job as pJT\n" +
                        "UNION\n" +
                        "SELECT wJT.Job\n" +
                        "FROM ATI_Workflow.dbo.Job wJT) AS uT\n" +
                        "WHERE Job = '" + jobLabel.Text.Trim() + "';";

                    OdbcCommand com = new OdbcCommand(query, conn);
                    OdbcDataReader reader = com.ExecuteReader();


                    jobExists = reader.Read();
                }

                // if it doesn't exist then insert it into Job db
                if (!jobExists)
                {
                    using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                    {
                        conn.Open();

                        string query =
                            "INSERT INTO [ATI_Workflow].[dbo].[Job]\n" +
                            "([Job]\n" +
                            ",[Part_Number]\n" +
                            ",[Customer])\n" +
                            "VALUES\n" +
                            "('" + jobLabel.Text.Trim() + "'\n" +
                            ",'" + partNoLabel.Text.Trim() + "'\n" +
                            ",'" + customer + "');";

                        OdbcCommand com = new OdbcCommand(query, conn);
                        int rows = com.ExecuteNonQuery();
                        if (rows != 1)
                            MessageBox.Show(Globals.generic_IT_error);
                    }
                }
                else
                {
                    MessageBox.Show("Job number already exists. Cannot create a duplicate job.");
                    return;
                }

            }

            // calculate the type of form to create after
            if (question1ComboBox.SelectedItem.ToString().Equals("Yes") || question2ComboBox.SelectedItem.ToString().Equals("Yes")
                || question3ComboBox.SelectedItem.ToString().Equals("Yes") || question4ComboBox.SelectedItem.ToString().Equals("Yes")
                || question5ComboBox.SelectedItem.ToString().Equals("Yes") || question6ComboBox.SelectedItem.ToString().Equals("Yes"))
                type = Globals.Status_Type.ContractReview;
            else
            {
                if (question1ComboBox.SelectedItem.ToString().Equals("New (Not Started)"))
                    type = Globals.Status_Type.QuickRelease;
                else
                    type = Globals.Status_Type.None;
            }


            // Submit query info
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "INSERT INTO [ATI_Workflow].[dbo].[Generic_PO_Review]\n" +
                    "([Job]\n" +
                    ",[Workflow_ID]\n" +
                    ",[Status]\n" +
                    ",[AvailableOn]\n" +
                    ",[StartedBy]\n" +
                    ",[StartedOn]\n" +
                    ",[CompletedBy]\n" +
                    ",[CompletedOn]\n" +
                    ",[LastUpdatedBy]\n" +
                    ",[LastUpdatedOn]\n" +
                    ",[PO_No]\n" +
                    ",[PO_Qty]\n" +
                    ",[PO_Rev]\n" +
                    ",[PO_Rev_Date]\n" +
                    ",[Description_Of_Change]\n" +
                    ",[Comments]\n" +
                    ",[Question1_Response]\n" +
                    ",[Question2_Response]\n" +
                    ",[Question3_Response]\n" +
                    ",[Question4_Response]\n" +
                    ",[Question5_Response]\n" +
                    ",[Question6_Response]\n" +
                    ",[Question7_Response]\n" +
                    ",[Part_Number]\n" +
                    ",[Drawing_Rev]\n" +
                    ",[Subassembly_PartNumber_1]\n" +
                    ",[Subassembly_Change_1]\n" +
                    ",[Drawing_Rev_1]\n" +
                    ",[Subassembly_PartNumber_2]\n" +
                    ",[Subassembly_Change_2]\n" +
                    ",[Drawing_Rev_2]\n" +
                    ",[Subassembly_PartNumber_3]\n" +
                    ",[Subassembly_Change_3]\n" +
                    ",[Drawing_Rev_3]\n" +
                    ",[Subassembly_PartNumber_4]\n" +
                    ",[Subassembly_Change_4]\n" +
                    ",[Drawing_Rev_4]\n" +
                    ",[Subassembly_PartNumber_5]\n" +
                    ",[Subassembly_Change_5]\n" +
                    ",[Drawing_Rev_5])\n" +
                    "VALUES\n" +
                    "('" + jobLabel.Text.Trim() + "'\n" +
                    ",'" + workflow_id + "'\n" +
                    ",'Completed'\n" +
                    ",'" + startTime.ToString() + "'\n" +
                    ",'" + Globals.userName + "'\n" +
                    ",'" + startTime.ToString() + "'\n" +
                    ",'" + Globals.userName + "'\n" +
                    ",'" + DateTime.Now.ToString() + "'\n" +
                    ",'" + Globals.userName + "'\n" +
                    ",'" + DateTime.Now.ToString() + "'\n" +
                    ",'" + poNoTextBox.Text + "'\n" +
                    ",'" + poQtyTextBox.Text + "'\n" +
                    ",'" + revTextBox.Text + "'\n" +
                    ",'" + revDateTextBox.Text + "'\n" +
                    ",'" + descriptionOfChangeTextBox.Text + "'\n" +
                    ",'" + commentsTextBox.Text + "'\n" +
                    ",'" + question1ComboBox.SelectedItem.ToString() + "'\n" +
                    ",'" + question2ComboBox.SelectedItem.ToString() + "'\n" +
                    ",'" + question3ComboBox.SelectedItem.ToString() + "'\n" +
                    ",'" + question4ComboBox.SelectedItem.ToString() + "'\n" +
                    ",'" + question5ComboBox.SelectedItem.ToString() + "'\n" +
                    ",'" + question6ComboBox.SelectedItem.ToString() + "'\n" +
                    ",'" + question7ComboBox.SelectedItem.ToString() + "'\n" +
                    ",'" + partNoLabel.Text.Trim() + "'\n" +
                    ",'" + drawingRevTextBox.Text + "'\n" +
                    ",'" + subAssemblyPartNumber1TextBox.Text + "'\n" +
                    ",'" + subAssemblyChange1ComboBox.SelectedItem.ToString() + "'\n" +
                    ",'" + subAssemblyDrawingRev1TextBox.Text + "'\n" +
                    ",'" + subAssemblyPartNumber2TextBox.Text + "'\n" +
                    ",'" + subAssemblyChange2ComboBox.SelectedItem.ToString() + "'\n" +
                    ",'" + subAssemblyDrawingRev2TextBox.Text + "'\n" +
                    ",'" + subAssemblyPartNumber3TextBox.Text + "'\n" +
                    ",'" + subAssemblyChange3ComboBox.SelectedItem.ToString() + "'\n" +
                    ",'" + subAssemblyDrawingRev3TextBox.Text + "'\n" +
                    ",'" + subAssemblyPartNumber4TextBox.Text + "'\n" +
                    ",'" + subAssemblyChange4ComboBox.SelectedItem.ToString() + "'\n" +
                    ",'" + subAssemblyDrawingRev4TextBox.Text + "'\n" +
                    ",'" + subAssemblyPartNumber5TextBox.Text + "'\n" +
                    ",'" + subAssemblyChange5ComboBox.SelectedItem.ToString() + "'\n" +
                    ",'" + subAssemblyDrawingRev5TextBox.Text + "');";


                OdbcCommand com = new OdbcCommand(query, conn);

                if (com.ExecuteNonQuery() != 1)
                    MessageBox.Show(Globals.generic_IT_error);
            }

            // submit new status row
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string type_str = string.Empty;
                if (type == Globals.Status_Type.ContractReview)
                    type_str = "Contract Review";
                else if (type == Globals.Status_Type.QuickRelease)
                    type_str = "Quick Release";
                else if (type == Globals.Status_Type.None)
                    type_str = "None";

                string query =
                    "INSERT INTO [ATI_Workflow].[dbo].[StatusData]\n" +
                    "([Job]\n" +
                    ",[Workflow_ID]\n" +
                    ",[Type]\n" +
                    ",[JobCreator_UserName]\n" +
                    ",[Hot]\n" +
                    ",[Workflow_Status]\n" +
                    ",[CreatedOn])\n" +
                    "VALUES\n" +
                    "('" + jobLabel.Text.Trim() + "'\n" +
                    ",'" + workflow_id + "'\n" +
                    ",'" + type_str + "'\n" +
                    ",'" + Globals.userName + "'\n" +
                    "," + (hotCheckBox.Checked ? 1 : 0) + "\n" +
                    ",'In Progress'\n" +
                    ",'" + DateTime.Now.ToString() + "');";

                OdbcCommand com = new OdbcCommand(query, conn);

                if (com.ExecuteNonQuery() != 1)
                    MessageBox.Show(Globals.generic_IT_error);
            }

            // based on the type submit new rows for the corresponding type
            if (type == Globals.Status_Type.ContractReview)
            {
                // Assignment step query
                using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                {
                    conn.Open();

                    string query =
                        "INSERT INTO [ATI_Workflow].[dbo].[AssignmentInfo]\n" +
                        "([Job]\n" +
                        ",[Workflow_ID]\n" +
                        ",[LastUpdatedBy]\n" +
                        ",[LastUpdatedOn]\n" +
                        ",[Status])\n" +
                        "VALUES\n" +
                        "('" + jobLabel.Text.Trim() + "'\n" +
                        ",'" + workflow_id + "'\n" +
                        ",'" + Globals.userName + "'\n" +
                        ",'" + DateTime.Now.ToString() + "'\n" +
                        ",'Incomplete');";

                    OdbcCommand com = new OdbcCommand(query, conn);
                    if (com.ExecuteNonQuery() != 1)
                        MessageBox.Show(Globals.generic_IT_error);
                }

                // submit ME query
                using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                {
                    conn.Open();

                    string query =
                        "INSERT INTO [ATI_Workflow].[dbo].[ContractReview_ME_QE]\n" +
                        "([Job]\n" +
                        ",[Workflow_ID]\n" +
                        ",[ContractReview_Type]\n" +
                        ",[Status]\n" +
                        ",[AvailableOn]\n" +
                        ",[LastUpdatedBy]\n" +
                        ",[LastUpdatedOn])\n" +
                        "VALUES\n" +
                        "('" + jobLabel.Text.Trim() + "'\n" +
                          ",'" + workflow_id + "'\n" +
                          ",'ME'\n" +
                          ",'Unassigned'\n" +
                          ",'" + DateTime.Now.ToString() + "'\n" +
                          ",'" + Globals.userName + "'\n" +
                          ",'" + DateTime.Now.ToString() + "');";

                    OdbcCommand com = new OdbcCommand(query, conn);
                    if (com.ExecuteNonQuery() != 1)
                        MessageBox.Show(Globals.generic_IT_error);
                }

                // submit QE query
                using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                {
                    conn.Open();

                    string query =
                        "INSERT INTO [ATI_Workflow].[dbo].[ContractReview_ME_QE]\n" +
                        "([Job]\n" +
                        ",[Workflow_ID]\n" +
                        ",[ContractReview_Type]\n" +
                        ",[Status]\n" +
                        ",[AvailableOn]\n" +
                        ",[LastUpdatedBy]\n" +
                        ",[LastUpdatedOn])\n" +
                        "VALUES\n" +
                        "('" + jobLabel.Text.Trim() + "'\n" +
                          ",'" + workflow_id + "'\n" +
                          ",'QE'\n" +
                          ",'Unassigned'\n" +
                          ",'" + DateTime.Now.ToString() + "'\n" +
                          ",'" + Globals.userName + "'\n" +
                          ",'" + DateTime.Now.ToString() + "');";

                    OdbcCommand com = new OdbcCommand(query, conn);
                    if (com.ExecuteNonQuery() != 1)
                        MessageBox.Show(Globals.generic_IT_error);
                }

                // submit QA query
                using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                {
                    conn.Open();

                    string query =
                        "INSERT INTO [ATI_Workflow].[dbo].[ContractReview_QA]\n" +
                        "([Job]\n" +
                        ",[Workflow_ID]\n" +
                        ",[Status]\n" +
                        ",[AvailableOn]\n" +
                        ",[LastUpdatedBy]\n" +
                        ",[LastUpdatedOn])\n" +
                        "VALUES\n" +
                        "('" + jobLabel.Text.Trim() + "'\n" +
                          ",'" + workflow_id + "'\n" +
                          ",'Unassigned'\n" +
                          ",'" + DateTime.Now.ToString() + "'\n" +
                          ",'" + Globals.userName + "'\n" +
                          ",'" + DateTime.Now.ToString() + "');";

                    OdbcCommand com = new OdbcCommand(query, conn);
                    if (com.ExecuteNonQuery() != 1)
                        MessageBox.Show(Globals.generic_IT_error);
                }
            }
            else if (type == Globals.Status_Type.QuickRelease)
            {
                // Assignment step query
                using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                {
                    conn.Open();

                    string query =
                        "INSERT INTO [ATI_Workflow].[dbo].[AssignmentInfo]\n" +
                        "([Job]\n" +
                        ",[Workflow_ID]\n" +
                        ",[LastUpdatedBy]\n" +
                        ",[LastUpdatedOn]\n" +
                        ",[Status])\n" +
                        "VALUES\n" +
                        "('" + jobLabel.Text.Trim() + "'\n" +
                        ",'" + workflow_id + "'\n" +
                        ",'" + Globals.userName + "'\n" +
                        ",'" + DateTime.Now.ToString() + "'\n" +
                        ",'Incomplete');";

                    OdbcCommand com = new OdbcCommand(query, conn);
                    if (com.ExecuteNonQuery() != 1)
                        MessageBox.Show(Globals.generic_IT_error);
                }

                // engineering quick release
                using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                {
                    conn.Open();

                    string query =
                        "INSERT INTO [ATI_Workflow].[dbo].[QuickRelease_Engineering]\n" +
                        "([Job]\n" +
                        ",[Workflow_ID]\n" +
                        ",[Status]\n" +
                        ",[AvailableOn]\n" +
                        ",[LastUpdatedBy]\n" +
                        ",[LastUpdatedOn])\n" +
                        "VALUES\n" +
                        "('" + jobLabel.Text.Trim() + "'\n" +
                        ", '" + workflow_id + "'\n" +
                        ", 'Unassigned'\n" +
                        ", '" + DateTime.Now.ToString() + "'\n" +
                        ", '" + Globals.userName + "'\n" +
                        ", '" + DateTime.Now.ToString() + "')";

                    OdbcCommand com = new OdbcCommand(query, conn);
                    if (com.ExecuteNonQuery() != 1)
                        MessageBox.Show(Globals.generic_IT_error);
                }

                // quality quick release
                using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                {
                    conn.Open();

                    string query =
                        "INSERT INTO [ATI_Workflow].[dbo].[QuickRelease_Quality]\n" +
                        "([Job]\n" +
                        ",[Workflow_ID]\n" +
                        ",[Status]\n" +
                        ",[AvailableOn]\n" +
                        ",[LastUpdatedBy]\n" +
                        ",[LastUpdatedOn])\n" +
                        "VALUES\n" +
                        "('" + jobLabel.Text.Trim() + "'\n" +
                        ", '" + workflow_id + "'\n" +
                        ", 'Unassigned'\n" +
                        ", '" + DateTime.Now.ToString() + "'\n" +
                        ", '" + Globals.userName + "'\n" +
                        ", '" + DateTime.Now.ToString() + "')";

                    OdbcCommand com = new OdbcCommand(query, conn);
                    if (com.ExecuteNonQuery() != 1)
                        MessageBox.Show(Globals.generic_IT_error);
                }
            }

            // send new part email
            SendEmailForNewPart();

            closedFromButton = true;
            // checks if it is a dialog
            if (this.Modal)
            {
                this.Close();
            }
            else
            {
                // load status page
                this.Hide();

                int next_id = -1;

                if (workflow_id > 100)
                    next_id = workflow_id;
                Form statusForm = new StatusPage(jobLabel.Text.Trim(), next_id, jobViewerRef);
                statusForm.FormClosed += (s, args) => this.Close();
                statusForm.Show();
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
            // content combobox
            foreach (var cb in GetAllChildren(this).OfType<ComboBox>())
                if ((cb.SelectedItem == null || cb.SelectedItem.ToString().Equals("")) && !cb.Name.Contains("subAssemblyChange"))
                {
                    errorProvider.SetError(cb, "Invalid Selection");
                    if (valid)
                        valid = false;
                }

            // content non zero
            foreach (var cb in GetAllChildren(this).OfType<TextBox>())
                if (cb.Text.Trim().Length == 0 && !cb.Name.Contains("subAssembly") && !cb.Name.Contains("comment") && !cb.Name.Contains("poNoTextBox") && !cb.Name.Contains("poQtyTextBox") && !cb.Name.Contains("jobLabel"))
                {
                    errorProvider.SetError(cb, "Invalid Content");
                    if (valid)
                        valid = false;
                }

            // content numeric
            int dummy = 0;
            foreach (var cb in GetAllChildren(this).OfType<TextBox>())
                if ((cb.Text.Trim().Length == 0 || !int.TryParse(cb.Text, out dummy)) && (cb.Name.Contains("poNoTextBox") || cb.Name.Contains("poQtyTextBox") || cb.Name.Contains("jobLabel")))
                {
                    errorProvider.SetError(cb, "Invalid content");
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

        private void FormatCheckInvalidTextBox(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text.Trim().Length == 0)
                errorProvider.SetError(textBox, "Invalid Content");
            else
                errorProvider.SetError(textBox, "");
        }

        private void FormatCheckInvalidNumericTextBox(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int dummy = 0;

            if (textBox.Text.Trim().Length == 0 || !int.TryParse(textBox.Text, out dummy))
                errorProvider.SetError(textBox, "Invalid Content");
            else
                errorProvider.SetError(textBox, "");
        }

        private void GenericPOReview_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closedFromButton && !readOnly && modified)
            {
                DialogResult result = MessageBox.Show("You will lose all progress if you close the form now. Continue?", "Close Application", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    this.Activate();
                }
                else if (result == DialogResult.Yes)
                {
                    closedFromButton = false;

                    if (jobViewerRef != null)
                    {
                        e.Cancel = true;
                        // open job list viewer
                        this.Hide();
                        jobViewerRef.Show();
                    }
                }
            }
            else
            {
                if (jobViewerRef != null)
                {
                    e.Cancel = true;
                    // open job list viewer
                    this.Hide();
                    jobViewerRef.Show();
                }
            }
        }

        private void question7ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (question7ComboBox.SelectedItem != null && question7ComboBox.SelectedItem.ToString().Equals("Yes") && !readOnly)
            {
                MessageBox.Show("Identify as ITAR in the JobBoss Material Record", "Important", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void SendEmailForNewPart()
        {
            List<string> emails = new List<string>();

            emails.Add("dleiva@absolutetechnologies.com");


            MailMessage mail = new MailMessage("dleiva@absolutetechnologies.com", "dleiva@absolutetechnologies.com");
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.google.com";
            mail.Subject = "this is a test email.";
            mail.Body = "this is my test email body";
            client.Send(mail);

        }
    }
}
