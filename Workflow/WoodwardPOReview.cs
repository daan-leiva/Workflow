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
    public partial class WoodwardPOReview : Form
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

        public WoodwardPOReview(string jobNo, bool readOnly, int workflow_id, bool newJob, JobListViewer jobViewer)
        {
            InitializeComponent();
            this.jobNo = jobNo;
            this.readOnly = readOnly;
            this.newJob = newJob;
            this.workflow_id = workflow_id;
            this.type = Globals.Status_Type.Invalid;
            this.jobViewerRef = jobViewer;
            this.startTime = DateTime.Now;
            this.customer = "WOODWARD";
        }

        private void WoodwardPOReview_Load(object sender, EventArgs e)
        {
            // load label values
            if (!newJob)
            {
                jobLabel.Text = jobNo;
                jobLabel.Enabled = false;
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
                        revTextBox.Enabled = false;
                    }
                }
            }

            // disable form if it is readonly
            // if readonly disable buttons and checkbox edits
            if (readOnly)
                DisableUserInputControls(this);

            PopulateDataFromDB();

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
                    ",'Filler. Remove later'\n" +
                    ",'Filler. Remove later'\n" +
                    ",[PO_No]\n" +
                    ",[PO_Qty]\n" +
                    ",[PO_Rev]\n" +
                    ",[PO_Rev_Date]\n" +
                    ",[Description_Of_Change]\n" +
                    ",[Question1_Response]\n" +
                    ",[Question2_Response]\n" +
                    ",[Question3_Response]\n" +
                    ",[Question4_Response]\n" +
                    ",[Question5_Response]\n" +
                    ",[Question6_Response]\n" +
                    ",[Question7_Response]\n" +
                    ",[Question8_Response]\n" +
                    ",[Part_Number]\n" +
                    ",[Drawing_Rev]\n" +
                    ",[ADCN_Numbers]\n" +
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
                    "FROM[ATI_Workflow].[dbo].[Woodward_PO_Review]\n" +
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

                    if (!reader.IsDBNull(reader.GetOrdinal("Question1_Response")))
                    {
                        if (reader.GetString(reader.GetOrdinal("Question1_Response")).Equals("New"))
                        {
                            question1NewRadioButton.Checked = true;
                        }
                        else if (reader.GetString(reader.GetOrdinal("Question1_Response")).Equals("Active"))
                        {
                            question1ActiveRadioButton.Checked = true;
                        }
                        else
                        {
                            question1CompleteRadioButton.Checked = true;
                        }
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("Question2_Response")))
                    {
                        if (reader.GetBoolean(reader.GetOrdinal("Question2_Response")))
                        {
                            question2YesRadioButton.Checked = true;
                        }
                        else
                        {
                            question2NoRadioButton.Checked = true;
                        }
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("Question3_Response")))
                    {
                        if (reader.GetString(reader.GetOrdinal("Question3_Response")).Equals("Yes"))
                        {
                            question3YesRadioButton.Checked = true;
                        }
                        else if (reader.GetString(reader.GetOrdinal("Question3_Response")).Equals("No"))
                        {
                            question3NoRadioButton.Checked = true;
                        }
                        else
                        {
                            question3NARadioButton.Checked = true;
                        }
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("Question4_Response")))
                    {
                        if (reader.GetBoolean(reader.GetOrdinal("Question4_Response")))
                        {
                            question4YesRadioButton.Checked = true;
                        }
                        else
                        {
                            question4NoRadioButton.Checked = true;
                        }
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("Question5_Response")))
                    {
                        if (reader.GetBoolean(reader.GetOrdinal("Question5_Response")))
                        {
                            question5YesRadioButton.Checked = true;
                        }
                        else
                        {
                            question5NoRadioButton.Checked = true;
                        }
                    }


                    if (!reader.IsDBNull(reader.GetOrdinal("Question6_Response")))
                    {
                        if (reader.GetBoolean(reader.GetOrdinal("Question6_Response")))
                        {
                            question6YesRadioButton.Checked = true;
                        }
                        else
                        {
                            question6NoRadioButton.Checked = true;
                        }
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("Question7_Response")))
                    {
                        if (reader.GetString(reader.GetOrdinal("Question7_Response")).Equals("Yes"))
                        {
                            question7YesRadioButton.Checked = true;
                        }
                        else if (reader.GetString(reader.GetOrdinal("Question7_Response")).Equals("No"))
                        {
                            question7NoRadioButton.Checked = true;
                        }
                        else
                        {
                            question7NARadioButton.Checked = true;
                        }
                    }


                    if (!reader.IsDBNull(reader.GetOrdinal("Question8_Response")))
                    {
                        if (reader.GetBoolean(reader.GetOrdinal("Question8_Response")))
                        {
                            question8YesRadioButton.Checked = true;
                        }
                        else
                        {
                            question8NoRadioButton.Checked = true;
                        }
                    }

                    partNoLabel.Text = reader.IsDBNull(reader.GetOrdinal("Part_Number")) ? "" : reader.GetString(reader.GetOrdinal("Part_Number"));
                    partNoLabel.Enabled = false;
                    drawingRevTextBox.Text = reader.IsDBNull(reader.GetOrdinal("Drawing_Rev")) ? "" : reader.GetString(reader.GetOrdinal("Drawing_Rev"));
                    drawingRevTextBox.Enabled = false;
                    ADCNNosTextBox.Text = reader.IsDBNull(reader.GetOrdinal("ADCN_Numbers")) ? "" : reader.GetString(reader.GetOrdinal("ADCN_Numbers"));

                    // subassembly parts section
                    subassemblyPartNumber1TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Subassembly_PartNumber_1")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subassembly_PartNumber_1"));
                    subassemblyPartNumber2TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Subassembly_PartNumber_2")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subassembly_PartNumber_2"));
                    subassemblyPartNumber3TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Subassembly_PartNumber_3")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subassembly_PartNumber_3"));
                    subassemblyPartNumber4TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Subassembly_PartNumber_4")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subassembly_PartNumber_4"));
                    subassemblyPartNumber5TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Subassembly_PartNumber_5")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subassembly_PartNumber_5"));

                    drawingRev1TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Drawing_Rev_1")) ? string.Empty : reader.GetString(reader.GetOrdinal("Drawing_Rev_1"));
                    drawingRev2TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Drawing_Rev_2")) ? string.Empty : reader.GetString(reader.GetOrdinal("Drawing_Rev_2"));
                    drawingRev3TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Drawing_Rev_3")) ? string.Empty : reader.GetString(reader.GetOrdinal("Drawing_Rev_3"));
                    drawingRev4TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Drawing_Rev_4")) ? string.Empty : reader.GetString(reader.GetOrdinal("Drawing_Rev_4"));
                    drawingRev5TextBox.Text = reader.IsDBNull(reader.GetOrdinal("Drawing_Rev_5")) ? string.Empty : reader.GetString(reader.GetOrdinal("Drawing_Rev_5"));

                    if (!reader.IsDBNull(reader.GetOrdinal("Subassembly_Change_1")))
                    {
                        if (reader.GetBoolean(reader.GetOrdinal("Subassembly_Change_1")))
                        {
                            subAssemblyChange1YesRadioButton.Checked = true;
                        }
                        else
                        {
                            subAssemblyChange1NoRadioButton.Checked = true;
                        }
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("Subassembly_Change_2")))
                    {
                        if (reader.GetBoolean(reader.GetOrdinal("Subassembly_Change_2")))
                        {
                            subAssemblyChange2YesRadioButton.Checked = true;
                        }
                        else
                        {
                            subAssemblyChange2NoRadioButton.Checked = true;
                        }
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("Subassembly_Change_3")))
                    {
                        if (reader.GetBoolean(reader.GetOrdinal("Subassembly_Change_3")))
                        {
                            subAssemblyChange3YesRadioButton.Checked = true;
                        }
                        else
                        {
                            subAssemblyChange3NoRadioButton.Checked = true;
                        }
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("Subassembly_Change_4")))
                    {
                        if (reader.GetBoolean(reader.GetOrdinal("Subassembly_Change_4")))
                        {
                            subAssemblyChange4YesRadioButton.Checked = true;
                        }
                        else
                        {
                            subAssemblyChange4NoRadioButton.Checked = true;
                        }
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("Subassembly_Change_5")))
                    {
                        if (reader.GetBoolean(reader.GetOrdinal("Subassembly_Change_5")))
                        {
                            subAssemblyChange5YesRadioButton.Checked = true;
                        }
                        else
                        {
                            subAssemblyChange5NoRadioButton.Checked = true;
                        }
                    }
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

            // go to status page with job, partNo and customer
            if (partNoLabel.Text.Trim().Length == 0)
            {
                MessageBox.Show("Part number field cannot be empty");
                return;
            }

            if (jobLabel.Text.Trim().Length == 0)
            {
                MessageBox.Show("Job number field cannot be empty");
                return;
            }

            if (newJob)
            {
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
            if (question3YesRadioButton.Checked || question4YesRadioButton.Checked
                || question5YesRadioButton.Checked || question6YesRadioButton.Checked
                || question7YesRadioButton.Checked)
                type = Globals.Status_Type.ContractReview;
            else
            {
                if (question1NewRadioButton.Checked)
                    type = Globals.Status_Type.QuickRelease;
                else
                    type = Globals.Status_Type.None;
            }


            // Submit query info
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "INSERT INTO [ATI_Workflow].[dbo].[Woodward_PO_Review]\n" +
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
                    ",[Question1_Response]\n" +
                    ",[Question2_Response]\n" +
                    ",[Question3_Response]\n" +
                    ",[Question4_Response]\n" +
                    ",[Question5_Response]\n" +
                    ",[Question6_Response]\n" +
                    ",[Question7_Response]\n" +
                    ",[Question8_Response]\n" +
                    ",[Part_Number]\n" +
                    ",[Drawing_Rev]\n" +
                    ",[ADCN_Numbers]\n" +
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
                    "," + (question1NewRadioButton.Checked ? "'New'" : (question1ActiveRadioButton.Checked ? "'Active'" : (question1CompleteRadioButton.Checked ? "'Complete'" : "NULL"))) + "\n" +
                    "," + (question2YesRadioButton.Checked ? "1" : (question2NoRadioButton.Checked ? "0" : "NULL")) + "\n" +
                    "," + (question3YesRadioButton.Checked ? "'Yes'" : (question3NoRadioButton.Checked ? "'No'" : (question3NARadioButton.Checked ? "'N/A'" : "NULL"))) + "\n" +
                    "," + (question4YesRadioButton.Checked ? "1" : (question4NoRadioButton.Checked ? "0" : "NULL")) + "\n" +
                    "," + (question5YesRadioButton.Checked ? "1" : (question5NoRadioButton.Checked ? "0" : "NULL")) + "\n" +
                    "," + (question6YesRadioButton.Checked ? "1" : (question6NoRadioButton.Checked ? "0" : "NULL")) + "\n" +
                    "," + (question7YesRadioButton.Checked ? "'Yes'" : (question7NoRadioButton.Checked ? "'No'" : (question7NARadioButton.Checked ? "'N/A'" : "NULL"))) + "\n" +
                    "," + (question8YesRadioButton.Checked ? "1" : (question8NoRadioButton.Checked ? "0" : "NULL")) + "\n" +
                    ",'" + partNoLabel.Text.Trim() + "'\n" +
                    ",'" + drawingRevTextBox.Text + "'\n" +
                    ",'" + ADCNNosTextBox.Text + "'\n" +
                    ",'" + subassemblyPartNumber1TextBox.Text + "'\n" +
                    "," + (subAssemblyChange1YesRadioButton.Checked ? "1" : (subAssemblyChange1NoRadioButton.Checked ? "0" : "NULL")) + "\n" +
                    ",'" + drawingRev1TextBox.Text + "'\n" +
                    ",'" + subassemblyPartNumber2TextBox.Text + "'\n" +
                    "," + (subAssemblyChange2YesRadioButton.Checked ? "1" : (subAssemblyChange2NoRadioButton.Checked ? "0" : "NULL")) + "\n" +
                    ",'" + drawingRev2TextBox.Text + "'\n" +
                    ",'" + subassemblyPartNumber3TextBox.Text + "'\n" +
                    "," + (subAssemblyChange3YesRadioButton.Checked ? "1" : (subAssemblyChange3NoRadioButton.Checked ? "0" : "NULL")) + "\n" +
                    ",'" + drawingRev3TextBox.Text + "'\n" +
                    ",'" + subassemblyPartNumber4TextBox.Text + "'\n" +
                    "," + (subAssemblyChange4YesRadioButton.Checked ? "1" : (subAssemblyChange4NoRadioButton.Checked ? "0" : "NULL")) + "\n" +
                    ",'" + drawingRev4TextBox.Text + "'\n" +
                    ",'" + subassemblyPartNumber5TextBox.Text + "'\n" +
                    "," + (subAssemblyChange5YesRadioButton.Checked ? "1" : (subAssemblyChange5NoRadioButton.Checked ? "0" : "NULL")) + "\n" +
                    ",'" + drawingRev5TextBox.Text + "');";


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

            // checks if it is a dialog
            if (this.Modal)
            {
                this.Close();
            }
            else
            {
                // load status page
                this.Hide();
                Form statusForm = new StatusPage(jobLabel.Text.Trim(), -1, jobViewerRef);
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

            valid =
                (question1ActiveRadioButton.Checked || question1CompleteRadioButton.Checked || question1NewRadioButton.Checked) &&
                (question3YesRadioButton.Checked || question3NoRadioButton.Checked || question3NARadioButton.Checked) &&
                (question4YesRadioButton.Checked || question4NoRadioButton.Checked) &&
                (question5YesRadioButton.Checked || question5NoRadioButton.Checked) &&
                (question6YesRadioButton.Checked || question6NoRadioButton.Checked) &&
                (question7YesRadioButton.Checked || question7NoRadioButton.Checked || question7NARadioButton.Checked);

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

        private void WoodwardPOReview_FormClosing(object sender, FormClosingEventArgs e)
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
}
