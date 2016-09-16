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
using System.Data.SqlClient;

namespace Workflow
{
    public partial class StatusPage : Form
    {
        private BindingSource bindingSource = new BindingSource();
        string jobNo;
        int initialWorkflow; // marks the workflow to start at, if -1 then start at index 0 or no index if empty
        JobListViewer jobViewerRef;

        Globals.Status_Type type;
        string partNo;
        string customer;

        public StatusPage(string jobNo, int initial_Workflow_ID, JobListViewer jobViewer)
        {
            InitializeComponent();
            this.jobNo = jobNo;
            this.jobViewerRef = jobViewer;
            this.initialWorkflow = initial_Workflow_ID;
            this.type = Globals.Status_Type.Invalid;

            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "SELECT uT.Job, uT.Part_Number, uT.Customer\n" +
                    "FROM(\n" +
                    "SELECT jT.Job, jT.Part_Number, jT.Customer\n" +
                    "FROM PRODUCTION.dbo.Job AS jT\n" +
                    "UNION\n" +
                    "SELECT wJ.Job, wJ.Part_Number, wJ.Customer\n" +
                    "FROM ATI_Workflow.dbo.Job AS wJ\n" +
                    ") AS uT\n" +
                    "WHERE Job = '" + jobNo + "';";

                OdbcCommand com = new OdbcCommand(query, conn);
                OdbcDataReader reader = com.ExecuteReader();
                if (reader.Read())
                {
                    this.partNo = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    this.customer = reader.IsDBNull(2) ? "" : reader.GetString(2);
                }
            }
        }

        private void StatusPage_Load(object sender, EventArgs e)
        {
            // set up datagridview
            workflowDataGridView.DataSource = bindingSource;
            workflowDataGridView.ReadOnly = true;
            workflowDataGridView.MultiSelect = false;

            // set up lead labels
            jobNoLabel.Text = jobNo;
            partNoLabel.Text = partNo;
            customerLabel.Text = customer;
            currentUserLabel.Text = Globals.userName;

            // write permissions label            
            if (Globals.admin)
                permissionsLabel.Text = "admin";
            else
            {
                List<string> permissions = new List<string>();
                if (Globals.customerServiceAccess)
                    permissions.Add("Customer Service");
                if (Globals.qaAccess)
                    permissions.Add("QA");
                if (Globals.qeAccess)
                    permissions.Add("QE");
                if (Globals.meAccess)
                    permissions.Add("ME");
                permissionsLabel.Text = string.Join(", ", permissions.ToArray());

                if (permissionsLabel.Text.Length == 0)
                    permissionsLabel.Text = "Read Only";
            }

            // update layout shape
            UpdateLayout();

            // set up detail events double click
            step1Details.DoubleClick += this.step1_DoubleClick;
            step2Details.DoubleClick += this.step2_DoubleClick;
            step3Details.DoubleClick += this.step3_DoubleClick;
            step4Details.DoubleClick += this.step4_DoubleClick;
            step5Details.DoubleClick += this.step5_DoubleClick;
            step1Label.DoubleClick += this.step1_DoubleClick;
            step2Label.DoubleClick += this.step2_DoubleClick;
            step3Label.DoubleClick += this.step3_DoubleClick;
            step4Label.DoubleClick += this.step4_DoubleClick;
            step5Label.DoubleClick += this.step5_DoubleClick;

            // add workflow datagridview data
            UpdateWorkflowDataGridViewFromSQL();

            if (workflowDataGridView.Rows.Count > 0)
            {
                // check if initial workflow is in list
                int index = IsItemInDataGridviewColumn(workflowDataGridView, 1, initialWorkflow.ToString());
                if (index != -1)
                {
                    workflowDataGridView.Rows[index].Selected = true;
                    workflowDataGridView_CellClick(new object(), new DataGridViewCellEventArgs(0, index));
                }
                // if workflow isn't available then activate first workflow and load info
                else
                {
                    workflowDataGridView.Rows[0].Selected = true;
                    workflowDataGridView_CellClick(new object(), new DataGridViewCellEventArgs(0, 0));
                }
            }
            else
            {
                completeButton.Enabled = false;
            }
        }

        private void UpdateWorkflowDataGridViewFromSQL()
        {
            string query =
                    "SELECT Job, Workflow_ID AS 'ID', Type, Workflow_Status AS 'Status', (CASE WHEN Hot = 1 THEN 'Yes' ELSE 'No' END) AS 'Hot'\n" +
                    "FROM [ATI_Workflow].[dbo].[StatusData]\n" +
                    "WHERE Job = '" + jobNo + "';";

            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Globals.binding_connection_string);

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            bindingSource.DataSource = dt;
        }

        private int IsItemInDataGridviewColumn(DataGridView gridView, int colIndex, string item)
        {
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                if (gridView.Rows[i].Cells[colIndex].Value.ToString().Equals(item))
                    return i;
            }

            return -1;
        }

        private void UpdateLayout()
        {
            // disable the forms that are not needed for this type
            if (type == Globals.Status_Type.None) // only needs PO Form
            {
                // Show step 1
                step1Label.Show();
                step1Details.Show();
                // Hide step 2-4
                step2Label.Hide();
                step2Details.Hide();
                step3Label.Hide();
                step3Details.Hide();
                step4Label.Hide();
                step4Details.Hide();
                step5Label.Hide();
                step5Details.Hide();
            }
            else if (type == Globals.Status_Type.ContractReview)
            {
                // Show step 1 - 4
                step1Label.Show();
                step1Details.Show();
                step2Label.Show();
                step2Details.Show();
                step3Label.Show();
                step3Details.Show();
                step4Label.Show();
                step4Details.Show();
                step5Label.Show();
                step5Details.Show();

                // rename steps 2-5
                step2Label.Text = "Assign Workflow";
                step3Label.Text = "Contract Review Check List QA";
                step4Label.Text = "Contract Review Check List ME";
                step5Label.Text = "Contract Review Check List QE";
            }
            else if (type == Globals.Status_Type.QuickRelease)
            {
                // Show step 1 - 4
                step1Label.Show();
                step1Details.Show();
                step2Label.Show();
                step2Details.Show();
                step3Label.Show();
                step3Details.Show();
                step4Label.Show();
                step4Details.Show();

                // Hide step 5
                step5Label.Hide();
                step5Details.Hide();

                // rename steps 2-4
                step2Label.Text = "Assign Workflow";
                step3Label.Text = "Quick Release QA";
                step4Label.Text = "Quick Release ME";
            }
            else if (type == Globals.Status_Type.Invalid)
            {
                // Hide step 1 - 5
                step1Label.Hide();
                step1Details.Hide();
                step2Label.Hide();
                step2Details.Hide();
                step3Label.Hide();
                step3Details.Hide();
                step4Label.Hide();
                step4Details.Hide();
                step5Label.Hide();
                step5Details.Hide();
            }
        }

        private void step1_DoubleClick(object sender, EventArgs e)
        {
            /*if (customer.Equals("HONDA AERO"))
            {
                Form hondaPo = new HondaPOReview(jobNo, true, int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()), false, null);
                hondaPo.ShowDialog();
            }
            else if (customer.Equals("ROLLS"))
            {
                Form rollsPo = new RollsRoycePOReview(jobNo, true, int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()), false, null);
                rollsPo.ShowDialog();
            }
            else if (customer.Equals("WOODWARD"))
            {
                Form woodwardPO = new WoodwardPOReview(jobNo, true, int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()), false, null);
                woodwardPO.ShowDialog();
            }
            else if (customer.Equals("EATON"))
            {
                Form eatonPO = new EatonPOReview(jobNo, true, int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()), false, null);
                eatonPO.ShowDialog();
            }
            else if (customer.Equals("ABLE"))
            {
                Form ablePO = new AblePOReview(jobNo, true, int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()), false, null);
                ablePO.ShowDialog();
            }
            else if (customer.Equals("WENCOR"))
            {
                Form wencorPO = new WencorPOReview(jobNo, true, int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()), false, null);
                wencorPO.ShowDialog();
            }
            else
            {*/
                Form genericPO = new GenericPOReview(jobNo, true, int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()), false, null, customer);
                genericPO.ShowDialog();
            //}
        }

        private void step2_DoubleClick(object sender, EventArgs e)
        {
            if (!(Globals.admin || Globals.leadAccess))
            {
                MessageBox.Show("Only leads can assign tasks", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // open form to assign tasks
            Form assignTastkForm = new AssignTaskForm(jobNo, int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()));
            assignTastkForm.ShowDialog();

            UpdateFromDB(int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()));
        }

        private void step3_DoubleClick(object sender, EventArgs e)
        {
            bool updated = false;
            bool readOnly = false;
            bool onHold = step3Status.Text.Trim().Contains("Hold");

            if (type == Globals.Status_Type.ContractReview)
            {
                readOnly = (step3Status.Text.Trim().Equals("Completed") || !Globals.qaAccess) && !Globals.admin;

                ContractReviewCheckList_QA contractReviewQA = new ContractReviewCheckList_QA(jobNo, partNo, int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()), readOnly, onHold, step3Status.Text);
                contractReviewQA.ShowDialog();
                updated = contractReviewQA.been_updated;
            }
            else if (type == Globals.Status_Type.QuickRelease)
            {
                readOnly = (step3Status.Text.Trim().Equals("Completed") || !Globals.qaAccess) && !Globals.admin;

                QuickReleaseForm_Quality quickReleaseForm = new QuickReleaseForm_Quality(jobNo, partNo, int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()), readOnly, onHold, step3Status.Text);
                quickReleaseForm.ShowDialog();
                updated = quickReleaseForm.been_updated;
            }

            UpdateFromDB(int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()));
            int index = workflowDataGridView.SelectedRows[0].Index;
            UpdateWorkflowDataGridViewFromSQL();
            workflowDataGridView.Rows[index].Selected = true;
            workflowDataGridView_CellClick(new object(), new DataGridViewCellEventArgs(0, index));
        }

        private void step4_DoubleClick(object sender, EventArgs e)
        {
            bool updated = false;
            bool readOnly = false;
            bool canComplete = false;
            bool onHold = step4Status.Text.Trim().Contains("Hold");

            if (type == Globals.Status_Type.ContractReview)
            {
                readOnly = (step4Status.Text.Trim().Equals("Completed") || !Globals.meAccess) && !Globals.admin;
                canComplete = step3Status.Text.Equals("Completed") || Globals.admin;

                ContractReviewCheckList_ME contractReviewME = new ContractReviewCheckList_ME(jobNo, partNo, int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()), readOnly, onHold, step4Status.Text, canComplete);
                contractReviewME.ShowDialog();
                updated = contractReviewME.been_updated;
            }
            else if (type == Globals.Status_Type.QuickRelease)
            {
                readOnly = (step4Status.Text.Trim().Equals("Completed") || !Globals.meAccess) && !Globals.admin;
                canComplete = step3Status.Text.Equals("Completed") || Globals.admin;

                QuickReleaseForm_Engineering quickReleaseForm = new QuickReleaseForm_Engineering(jobNo, partNo, int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()), readOnly, onHold, step4Status.Text, canComplete);
                quickReleaseForm.ShowDialog();
                updated = quickReleaseForm.been_updated;
            }

            UpdateFromDB(int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()));
            int index = workflowDataGridView.SelectedRows[0].Index;
            UpdateWorkflowDataGridViewFromSQL();
            workflowDataGridView.Rows[index].Selected = true;
            workflowDataGridView_CellClick(new object(), new DataGridViewCellEventArgs(0, index));
        }

        private void step5_DoubleClick(object sender, EventArgs e)
        {
            bool updated = false;
            bool readOnly = false;
            bool canComplete = false;
            bool onHold = step5Status.Text.Trim().Contains("Hold");

            if (type == Globals.Status_Type.ContractReview)
            {
                readOnly = (step3Status.Text.Trim().Equals("Completed") || !Globals.qeAccess) && !Globals.admin;
                canComplete = step3Status.Text.Equals("Completed") || Globals.admin;

                ContractReviewCheckList_QE contractReviewQE = new ContractReviewCheckList_QE(jobNo, partNo, int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()), readOnly, onHold, step5Status.Text, canComplete);
                contractReviewQE.ShowDialog();
                updated = contractReviewQE.been_updated;
            }

            UpdateFromDB(int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()));
            int index = workflowDataGridView.SelectedRows[0].Index;
            UpdateWorkflowDataGridViewFromSQL();
            workflowDataGridView.Rows[index].Selected = true;
            workflowDataGridView_CellClick(new object(), new DataGridViewCellEventArgs(0, index));
        }

        private void StatusPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            // open job list viewer
            e.Cancel = true;
            this.Hide();
            jobViewerRef.Show();
        }

        private void UpdateFromDB(int workflowID)
        {
            // once layout is updated pull info
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "SELECT\n" +
                    "statusT.[Job]\n" +
                    ",statusT.Type\n" +
                    ",statusT.[Hot] AS status_hot\n" +
                    ",statusT.[Workflow_Status] AS status_status\n" +
                    ",statusT.[Engi_Hold] AS status_engi_hold\n" +
                    ",statusT.[Cust_Hold] AS status_cust_hold\n" +
                    ",statusT.[QA_Hold] AS status_qa_hold\n" +

                    ",hondaPOT.Status AS Honda_POReview_Status\n" +
                    ",hondaPOT.LastUpdatedBy AS Honda_POReview_UserName\n" +
                    ",hondaPOT.LastUpdatedOn AS Honda_POReview_TimeStamp\n" +
                    ",hondaPOT.PO_No AS Honda_POReview_PO_No\n" +
                    ",hondaPOT.Description_Of_Change AS Honda_POReview_Description_Of_Change\n" +

                    ",eatonPOT.Status AS Eaton_POReview_Status\n" +
                    ",eatonPOT.LastUpdatedBy AS Eaton_POReview_UserName\n" +
                    ",eatonPOT.LastUpdatedOn AS Eaton_POReview_TimeStamp\n" +
                    ",eatonPOT.PO_No AS Eaton_POReview_PO_No\n" +
                    ",eatonPOT.Description_Of_Change AS Eaton_POReview_Description_Of_Change\n" +

                    ",rollsPOT.Status AS Rolls_POReview_Status\n" +
                    ",rollsPOT.LastUpdatedBy AS Rolls_POReview_UserName\n" +
                    ",rollsPOT.LastUpdatedOn AS Rolls_POReview_TimeStamp\n" +
                    ",rollsPOT.PO_No AS Rolls_POReview_PO_No\n" +
                    ",rollsPOT.Description_Of_Change AS Rolls_POReview_Description_Of_Change\n" +

                    ",woodwardPOT.Status AS Woodward_POReview_Status\n" +
                    ",woodwardPOT.LastUpdatedBy AS Woodward_POReview_UserName\n" +
                    ",woodwardPOT.LastUpdatedOn AS Woodward_POReview_TimeStamp\n" +
                    ",woodwardPOT.PO_No AS Woodward_POReview_PO_No\n" +
                    ",woodwardPOT.Description_Of_Change AS Woordward_POReview_Description_Of_Change\n" +

                    ",ablePOT.Status AS Able_POReview_Status\n" +
                    ",ablePOT.LastUpdatedBy AS Able_POReview_UserName\n" +
                    ",ablePOT.LastUpdatedOn AS Able_POReview_TimeStamp\n" +
                    ",ablePOT.PO_No AS Able_POReview_PO_No\n" +
                    ",ablePOT.Description_Of_Change AS Able_POReview_Description_Of_Change\n" +

                    ",wencorPOT.Status AS Wencor_POReview_Status\n" +
                    ",wencorPOT.LastUpdatedBy AS Wencor_POReview_UserName\n" +
                    ",wencorPOT.LastUpdatedOn AS Wencor_POReview_TimeStamp\n" +
                    ",wencorPOT.PO_No AS Wencor_POReview_PO_No\n" +
                    ",wencorPOT.Description_Of_Change AS Wencor_POReview_Description_Of_Change\n" +

                    ",genericPOT.Status AS Generic_POReview_Status\n" +
                    ",genericPOT.LastUpdatedBy AS Generic_POReview_UserName\n" +
                    ",genericPOT.LastUpdatedOn AS Generic_POReview_TimeStamp\n" +
                    ",genericPOT.PO_No AS Generic_POReview_PO_No\n" +
                    ",genericPOT.Description_Of_Change AS Generic_POReview_Description_Of_Change\n" +

                    ",assignmentT.Status AS Assignment_Status\n" +
                    ",assignmentT.LastUpdatedBy AS Assignment_UserName\n" +
                    ",assignmentT.LastUpdatedOn AS Assignment_TimeStamp\n" +

                    ",quickRelEngT.Status AS QuickRelease_Engineering_Status\n" +
                    ",quickRelEngT.Status_Subtype AS QuickRelease_Engineering_Status_Subtype\n" +
                    ",quickRelEngT.Status_Notes AS QuickRelease_Engineering_Status_Notes\n" +
                    ",quickRelEngT.LastUpdatedBy AS QuickRelease_Engineering_UserName\n" +
                    ",quickRelEngT.LastUpdatedOn AS QuickRelease_Engineering_TimeStamp\n" +
                    ",quickRelEngT.AssignedTo AS QuickRelease_Engineering_AssignedTo\n" +
                    ",quickRelEngT.CompletedOn AS QuickRelease_Engineering_CompletedOn\n" +
                    ",quickRelEngT.CompletedBy AS QuickRelease_Engineering_CompletedBy\n" +

                    ",quickRelQuaT.Status AS QuickRelease_Quality_Status\n" +
                    ",quickRelQuaT.Status_Subtype AS QuickRelease_Quality_Status_Subtype\n" +
                    ",quickRelQuaT.Status_Notes AS QuickRelease_Quality_Status_Notes\n" +
                    ",quickRelQuaT.LastUpdatedBy AS QuickRelease_Quality_UserName\n" +
                    ",quickRelQuaT.LastUpdatedOn AS QuickRelease_Quality_TimeStamp\n" +
                    ",quickRelQuaT.AssignedTo AS QuickRelease_Quality_AssignedTo\n" +
                    ",quickRelQuaT.CompletedOn AS QuickRelease_Quality_CompletedOn\n" +
                    ",quickRelQuaT.CompletedBy AS QuickRelease_Quality_CompletedBy\n" +

                    ",contractReviewMET.Status AS ContractReview_ME_Status\n" +
                    ",contractReviewMET.Status_Subtype AS ContractReview_ME_Status_Subtype\n" +
                    ",contractReviewMET.Status_Notes AS ContractReview_ME_Status_Notes\n" +
                    ",contractReviewMET.LastUpdatedBy AS ContractReview_ME_UserName\n" +
                    ",contractReviewMET.LastUpdatedOn AS ContractReview_ME_TimeStamp\n" +
                    ",contractReviewMET.AssignedTo AS ContractReview_ME_AssignedTO\n" +
                    ",contractReviewMET.CompletedOn AS ContractReview_ME_CompletedOn\n" +
                    ",contractReviewMET.CompletedBy AS ContractReview_ME_CompletedBy\n" +

                    ",contractReviewQAT.Status AS ContractReview_QA_Status\n" +
                    ",contractReviewQAT.Status_Subtype AS ContractReview_QA_Status_Subtype\n" +
                    ",contractReviewQAT.Status_Notes AS ContractReview_QA_Status_Notes\n" +
                    ",contractReviewQAT.LastUpdatedBy AS ContractReview_QA_UserName\n" +
                    ",contractReviewQAT.LastUpdatedOn AS ContractReview_QA_TimeStamp\n" +
                    ",contractReviewQAT.AssignedTo AS ContractReview_QA_AssignedTo\n" +
                    ",contractReviewQAT.CompletedOn AS ContractReview_QA_CompletedOn\n" +
                    ",contractReviewQAT.CompletedBy AS ContractReview_QA_CompletedBy\n" +

                    ",contractReviewQET.Status AS ContractReview_QE_Status\n" +
                    ",contractReviewQET.Status_Subtype AS ContractReview_QE_Status_Subtype\n" +
                    ",contractReviewQET.Status_Notes AS ContractReview_QE_Status_Notes\n" +
                    ",contractReviewQET.LastUpdatedBy AS ContractReview_QE_UserName\n" +
                    ",contractReviewQET.LastUpdatedOn AS ContractReview_QE_TimeStamp\n" +
                    ",contractReviewQET.AssignedTo AS ContractReview_QE_AssignedTo\n" +
                    ",contractReviewQET.CompletedOn AS ContractReview_QE_CompletedOn\n" +
                    ",contractReviewQET.CompletedBy AS ContractReview_QE_CompletedBy\n" +

                    "FROM[ATI_Workflow].[dbo].[StatusData] AS statusT\n" +
                    "LEFT JOIN[ATI_Workflow].[dbo].[Honda_PO_Review] AS hondaPOT\n" +
                    "ON hondaPOT.Job = statusT.Job AND hondaPOT.Workflow_ID = statusT.Workflow_ID\n" +
                    "LEFT JOIN[ATI_Workflow].[dbo].[Eaton_PO_Review] AS eatonPOT\n" +
                    "ON eatonPOT.Job = statusT.Job AND eatonPOT.Workflow_ID = statusT.Workflow_ID\n" +
                    "LEFT JOIN[ATI_Workflow].[dbo].[RollsRoyce_PO_Review] AS rollsPOT\n" +
                    "ON rollsPOT.Job = statusT.Job AND rollsPOT.Workflow_ID = statusT.Workflow_ID\n" +
                    "LEFT JOIN[ATI_Workflow].[dbo].[Woodward_PO_Review] AS woodwardPOT\n" +
                    "ON woodwardPOT.Job = statusT.Job AND woodwardPOT.Workflow_ID = statusT.Workflow_ID\n" +
                    "LEFT JOIN[ATI_Workflow].[dbo].[Able_PO_Review] AS ablePOT\n" +
                    "ON ablePOT.Job = statusT.Job AND ablePOT.Workflow_ID = statusT.Workflow_ID\n" +
                    "LEFT JOIN[ATI_Workflow].[dbo].[Wencor_PO_Review] AS wencorPOT\n" +
                    "ON wencorPOT.Job = statusT.Job AND wencorPOT.Workflow_ID = statusT.Workflow_ID\n" +
                    "LEFT JOIN[ATI_Workflow].[dbo].[Generic_PO_Review] AS genericPOT\n" +
                    "ON genericPOT.Job = statusT.Job AND genericPOT.Workflow_ID = statusT.Workflow_ID\n" +
                    "LEFT JOIN [ATI_Workflow].[dbo].QuickRelease_Engineering AS quickRelEngT\n" +
                    "On quickRelEngT.job = statusT.Job AND quickRelEngT.Workflow_ID = statusT.Workflow_ID\n" +
                    "LEFT JOIN [ATI_Workflow].[dbo].QuickRelease_Quality AS quickRelQuaT\n" +
                    "On quickRelQuaT.job = statusT.Job AND quickRelQuaT.Workflow_ID = statusT.Workflow_ID\n" +
                    "LEFT JOIN [ATI_Workflow].[dbo].[ContractReview_ME_QE] AS contractReviewMET\n" +
                    "ON contractReviewMET.ContractReview_Type = 'ME' AND contractReviewMET.Job = statusT.Job AND contractReviewMET.Workflow_ID = statusT.Workflow_ID\n" +
                    "LEFT JOIN [ATI_Workflow].[dbo].ContractReview_QA AS contractReviewQAT\n" +
                    "ON contractReviewQAT.Job = statusT.Job AND contractReviewQAT.Workflow_ID = statusT.Workflow_ID\n" +
                    "LEFT JOIN [ATI_Workflow].[dbo].[ContractReview_ME_QE] AS contractReviewQET\n" +
                    "ON contractReviewQET.ContractReview_Type = 'QE' AND contractReviewQET.Job = statusT.Job AND contractReviewQET.Workflow_ID = statusT.Workflow_ID\n" +
                    "LEFT JOIN [ATI_Workflow].[dbo].[AssignmentInfo] AS assignmentT\n" +
                    "ON assignmentT.Job = statusT.Job AND assignmentT.Workflow_ID = statusT.Workflow_ID\n" +
                    "WHERE statusT.Job = '" + jobNo + "' AND statusT.Workflow_ID = '" + workflowID + "';";

                OdbcCommand com = new OdbcCommand(query, conn);
                OdbcDataReader reader = com.ExecuteReader();

                try
                {
                    if (reader.Read())
                    {
                        // double check that type matches current type
                        // find index of type
                        int type_index = reader.GetOrdinal("Type");

                        // update layout first
                        if (!reader.IsDBNull(type_index))
                        {
                            if (reader.GetString(type_index).Equals("Contract Review"))
                            {
                                type = Globals.Status_Type.ContractReview;
                            }
                            else if (reader.GetString(type_index).Equals("Quick Release"))
                            {
                                this.type = Globals.Status_Type.QuickRelease;
                            }
                            else if (reader.GetString(type_index).Equals("None"))
                            {
                                this.type = Globals.Status_Type.None;
                            }
                            UpdateLayout();
                        }

                        // first fill in PO which all will have
                        /*if (customer.Equals("HONDA AERO"))
                        {
                            this.step1UserName.Text = reader.IsDBNull(reader.GetOrdinal("Honda_POReview_UserName")) ? "Username" : reader.GetString(reader.GetOrdinal("Honda_POReview_UserName"));
                            this.step1TimeStamp.Text = reader.IsDBNull(reader.GetOrdinal("Honda_POReview_TimeStamp")) ? "TimeStamp" : reader.GetDateTime(reader.GetOrdinal("Honda_POReview_TimeStamp")).ToString(@"MM\/dd\/yyyy hh:mm tt");
                            this.step1Status.Text = reader.IsDBNull(reader.GetOrdinal("Honda_POReview_Status")) ? "Status" : reader.GetString(reader.GetOrdinal("Honda_POReview_Status"));
                            poNoLabel.Text = reader.IsDBNull(reader.GetOrdinal("Honda_PoReview_PO_No")) ? "" : reader.GetString(reader.GetOrdinal("Honda_PoReview_PO_No"));
                            descriptionOfChangeLabel.Text = reader.IsDBNull(reader.GetOrdinal("Honda_PoReview_Description_Of_Change")) ? "" : reader.GetString(reader.GetOrdinal("Honda_PoReview_Description_Of_Change"));
                        }
                        else if (customer.Equals("EATON"))
                        {
                            this.step1UserName.Text = reader.IsDBNull(reader.GetOrdinal("Eaton_POReview_UserName")) ? "Username" : reader.GetString(reader.GetOrdinal("Eaton_POReview_UserName"));
                            this.step1TimeStamp.Text = reader.IsDBNull(reader.GetOrdinal("Eaton_POReview_TimeStamp")) ? "TimeStamp" : reader.GetDateTime(reader.GetOrdinal("Eaton_POReview_TimeStamp")).ToString(@"MM\/dd\/yyyy hh:mm tt");
                            this.step1Status.Text = reader.IsDBNull(reader.GetOrdinal("Eaton_POReview_Status")) ? "Status" : reader.GetString(reader.GetOrdinal("Eaton_POReview_Status"));
                            poNoLabel.Text = reader.IsDBNull(reader.GetOrdinal("Eaton_PoReview_PO_No")) ? "" : reader.GetString(reader.GetOrdinal("Eaton_PoReview_PO_No"));
                            descriptionOfChangeLabel.Text = reader.IsDBNull(reader.GetOrdinal("Eaton_PoReview_Description_Of_Change")) ? "" : reader.GetString(reader.GetOrdinal("Eaton_PoReview_Description_Of_Change"));
                        }
                        else if (customer.Equals("ROLLS"))
                        {
                            this.step1UserName.Text = reader.IsDBNull(reader.GetOrdinal("Rolls_POReview_UserName")) ? "Username" : reader.GetString(reader.GetOrdinal("Rolls_POReview_UserName"));
                            this.step1TimeStamp.Text = reader.IsDBNull(reader.GetOrdinal("Rolls_POReview_TimeStamp")) ? "TimeStamp" : reader.GetDateTime(reader.GetOrdinal("Rolls_POReview_TimeStamp")).ToString(@"MM\/dd\/yyyy hh:mm tt");
                            this.step1Status.Text = reader.IsDBNull(reader.GetOrdinal("Rolls_POReview_Status")) ? "Status" : reader.GetString(reader.GetOrdinal("Rolls_POReview_Status"));
                            poNoLabel.Text = reader.IsDBNull(reader.GetOrdinal("Rolls_PoReview_PO_No")) ? "" : reader.GetString(reader.GetOrdinal("Rolls_PoReview_PO_No"));
                            descriptionOfChangeLabel.Text = reader.IsDBNull(reader.GetOrdinal("Rolls_PoReview_Description_Of_Change")) ? "" : reader.GetString(reader.GetOrdinal("Rolls_PoReview_Description_Of_Change"));
                        }
                        else if (customer.Equals("WOODWARD"))
                        {
                            this.step1UserName.Text = reader.IsDBNull(reader.GetOrdinal("Woodward_POReview_UserName")) ? "Username" : reader.GetString(reader.GetOrdinal("Woodward_POReview_UserName"));
                            this.step1TimeStamp.Text = reader.IsDBNull(reader.GetOrdinal("Woodward_POReview_TimeStamp")) ? "TimeStamp" : reader.GetDateTime(reader.GetOrdinal("Woodward_POReview_TimeStamp")).ToString(@"MM\/dd\/yyyy hh:mm tt");
                            this.step1Status.Text = reader.IsDBNull(reader.GetOrdinal("Woodward_POReview_Status")) ? "Status" : reader.GetString(reader.GetOrdinal("Woodward_POReview_Status"));
                            poNoLabel.Text = reader.IsDBNull(reader.GetOrdinal("Woodward_PoReview_PO_No")) ? "" : reader.GetString(reader.GetOrdinal("Woodward_PoReview_PO_No"));
                            descriptionOfChangeLabel.Text = reader.IsDBNull(reader.GetOrdinal("Woodward_PoReview_Description_Of_Change")) ? "" : reader.GetString(reader.GetOrdinal("Woodward_PoReview_Description_Of_Change"));
                        }
                        else if (customer.Equals("ABLE"))
                        {
                            this.step1UserName.Text = reader.IsDBNull(reader.GetOrdinal("Able_POReview_UserName")) ? "Username" : reader.GetString(reader.GetOrdinal("Able_POReview_UserName"));
                            this.step1TimeStamp.Text = reader.IsDBNull(reader.GetOrdinal("Able_POReview_TimeStamp")) ? "TimeStamp" : reader.GetDateTime(reader.GetOrdinal("Able_POReview_TimeStamp")).ToString(@"MM\/dd\/yyyy hh:mm tt");
                            this.step1Status.Text = reader.IsDBNull(reader.GetOrdinal("Able_POReview_Status")) ? "Status" : reader.GetString(reader.GetOrdinal("Able_POReview_Status"));
                            poNoLabel.Text = reader.IsDBNull(reader.GetOrdinal("Able_PoReview_PO_No")) ? "" : reader.GetString(reader.GetOrdinal("Able_PoReview_PO_No"));
                            descriptionOfChangeLabel.Text = reader.IsDBNull(reader.GetOrdinal("Able_PoReview_Description_Of_Change")) ? "" : reader.GetString(reader.GetOrdinal("Able_PoReview_Description_Of_Change"));
                        }
                        else if (customer.Equals("WENCOR"))
                        {
                            this.step1UserName.Text = reader.IsDBNull(reader.GetOrdinal("Wencor_POReview_UserName")) ? "Username" : reader.GetString(reader.GetOrdinal("Wencor_POReview_UserName"));
                            this.step1TimeStamp.Text = reader.IsDBNull(reader.GetOrdinal("Wencor_POReview_TimeStamp")) ? "TimeStamp" : reader.GetDateTime(reader.GetOrdinal("Wencor_POReview_TimeStamp")).ToString(@"MM\/dd\/yyyy hh:mm tt");
                            this.step1Status.Text = reader.IsDBNull(reader.GetOrdinal("Wencor_POReview_Status")) ? "Status" : reader.GetString(reader.GetOrdinal("Wencor_POReview_Status"));
                            poNoLabel.Text = reader.IsDBNull(reader.GetOrdinal("Wencor_PoReview_PO_No")) ? "" : reader.GetString(reader.GetOrdinal("Wencor_PoReview_PO_No"));
                            descriptionOfChangeLabel.Text = reader.IsDBNull(reader.GetOrdinal("Wencor_PoReview_Description_Of_Change")) ? "" : reader.GetString(reader.GetOrdinal("Wencor_PoReview_Description_Of_Change"));
                        }
                        else
                        {*/
                            this.step1UserName.Text = reader.IsDBNull(reader.GetOrdinal("Generic_POReview_UserName")) ? "Username" : reader.GetString(reader.GetOrdinal("Generic_POReview_UserName"));
                            this.step1TimeStamp.Text = reader.IsDBNull(reader.GetOrdinal("Generic_POReview_TimeStamp")) ? "TimeStamp" : reader.GetDateTime(reader.GetOrdinal("Generic_POReview_TimeStamp")).ToString(@"MM\/dd\/yyyy hh:mm tt");
                            this.step1Status.Text = reader.IsDBNull(reader.GetOrdinal("Generic_POReview_Status")) ? "Status" : reader.GetString(reader.GetOrdinal("Generic_POReview_Status"));
                            poNoLabel.Text = reader.IsDBNull(reader.GetOrdinal("Generic_PoReview_PO_No")) ? "" : reader.GetString(reader.GetOrdinal("Generic_PoReview_PO_No"));
                            step1NotesTitle.Text = reader.IsDBNull(reader.GetOrdinal("Generic_PoReview_Description_Of_Change")) ? "" : reader.GetString(reader.GetOrdinal("Generic_PoReview_Description_Of_Change"));
                        //}

                        // load status data
                        this.hotLabel.Text = reader.IsDBNull(reader.GetOrdinal("status_hot")) ? "--" : (reader.GetBoolean(reader.GetOrdinal("status_hot")) ? "Yes" : "No");
                        this.workflowStatusLabel.Text = reader.IsDBNull(reader.GetOrdinal("status_status")) ? "--" : reader.GetString(reader.GetOrdinal("status_status"));

                        // if hold then add hold types
                        if (this.workflowStatusLabel.Text.Contains("Hold"))
                        {
                            List<string> hold_types = new List<string>();
                            if (reader.IsDBNull(reader.GetOrdinal("status_engi_hold")) ? false : reader.GetBoolean(reader.GetOrdinal("status_engi_hold")))
                                hold_types.Add("Engi");
                            if (reader.IsDBNull(reader.GetOrdinal("status_cust_hold")) ? false : reader.GetBoolean(reader.GetOrdinal("status_cust_hold")))
                                hold_types.Add("Cust");
                            if (reader.IsDBNull(reader.GetOrdinal("status_qa_hold")) ? false : reader.GetBoolean(reader.GetOrdinal("status_qa_hold")))
                                hold_types.Add("QA");
                            this.workflowStatusLabel.Text += " - " + string.Join(", ", hold_types.ToArray());

                            if (type == Globals.Status_Type.ContractReview  || type == Globals.Status_Type.QuickRelease)
                            {
                                if(type == Globals.Status_Type.ContractReview)
                                {
                                    if ((reader.IsDBNull(reader.GetOrdinal("ContractReview_QA_Status")) ? "" : reader.GetString(reader.GetOrdinal("ContractReview_QA_Status"))).Contains("Hold"))
                                    {
                                        step1NotesTitle.Show();
                                        step1NotesTitle.Text = "QA Hold Notes:";
                                        step1Notes.Show();
                                        step1Notes.Text = reader.IsDBNull(reader.GetOrdinal("ContractReview_QA_Status_Notes")) ? "" : reader.GetString(reader.GetOrdinal("ContractReview_QA_Status_Notes"));
                                    }
                                    else
                                    {
                                        step1NotesTitle.Hide();
                                        step1Notes.Hide();
                                    }

                                    if ((reader.IsDBNull(reader.GetOrdinal("ContractReview_ME_Status")) ? "" : reader.GetString(reader.GetOrdinal("ContractReview_ME_Status"))).Contains("Hold"))
                                    {
                                        step2NotesTitle.Show();
                                        step2NotesTitle.Text = "ME Hold Notes:";
                                        step2Notes.Show();
                                        step2Notes.Text = reader.IsDBNull(reader.GetOrdinal("ContractReview_ME_Status_Notes")) ? "" : reader.GetString(reader.GetOrdinal("ContractReview_ME_Status_Notes"));
                                    }
                                    else
                                    {
                                        step2NotesTitle.Hide();
                                        step2Notes.Hide();
                                    }

                                    if ((reader.IsDBNull(reader.GetOrdinal("ContractReview_QE_Status")) ? "" : reader.GetString(reader.GetOrdinal("ContractReview_QE_Status"))).Contains("Hold"))
                                    {
                                        step3NotesTitle.Show();
                                        step3NotesTitle.Text = "QE Hold Notes:";
                                        step3Notes.Show();
                                        step3Notes.Text = reader.IsDBNull(reader.GetOrdinal("ContractReview_QE_Status_Notes")) ? "" : reader.GetString(reader.GetOrdinal("ContractReview_QE_Status_Notes"));
                                    }
                                    else
                                    {
                                        step3NotesTitle.Hide();
                                        step3Notes.Hide();
                                    }
                                }
                                else
                                {
                                    if ((reader.IsDBNull(reader.GetOrdinal("QuickRelease_Quality_Status")) ? "" : reader.GetString(reader.GetOrdinal("QuickRelease_Quality_Status"))).Contains("Hold"))
                                    {
                                        step1NotesTitle.Show();
                                        step1NotesTitle.Text = "QA Hold Notes:";
                                        step1Notes.Show();
                                        step1Notes.Text = reader.IsDBNull(reader.GetOrdinal("QuickRelease_Quality_Status_Notes")) ? "" : reader.GetString(reader.GetOrdinal("QuickRelease_Quality_Status_Notes"));
                                    }
                                    else
                                    {
                                        step1NotesTitle.Hide();
                                        step1Notes.Hide();
                                    }

                                    if ((reader.IsDBNull(reader.GetOrdinal("QuickRelease_Engineering_Status")) ? "" : reader.GetString(reader.GetOrdinal("QuickRelease_Engineering_Status"))).Contains("Hold"))
                                    {
                                        step2NotesTitle.Show();
                                        step2NotesTitle.Text = "ME Hold Notes:";
                                        step2Notes.Show();
                                        step2Notes.Text = reader.IsDBNull(reader.GetOrdinal("QuickRelease_Engineering_Status_Notes")) ? "" : reader.GetString(reader.GetOrdinal("QuickRelease_Engineering_Status_Notes"));
                                    }
                                    else
                                    {
                                        step2NotesTitle.Hide();
                                        step2Notes.Hide();
                                    }

                                    step3NotesTitle.Hide();
                                    step3Notes.Hide();
                                }
                            }
                        }
                        else // hide everything hold related
                        {
                            step1NotesTitle.Hide();
                            step1Notes.Hide();
                            step2NotesTitle.Hide();
                            step2Notes.Hide();
                            step3NotesTitle.Hide();
                            step3Notes.Hide();
                        }


                        // load info accordingly based on type
                        switch (this.type)
                        {
                            case Globals.Status_Type.ContractReview:
                                // step 2
                                this.step2UserName.Text = "Last Updated By: " + (reader.IsDBNull(reader.GetOrdinal("Assignment_Status")) ? "" : reader.GetString(reader.GetOrdinal("Assignment_UserName")));
                                this.step2TimeStamp.Text = "Last Updated On: " + (reader.IsDBNull(reader.GetOrdinal("Assignment_TimeStamp")) ? "" : reader.GetDateTime(reader.GetOrdinal("Assignment_TimeStamp")).ToString(@"MM\/dd\/yyyy hh:mm tt"));
                                this.step2Status.Text = reader.IsDBNull(reader.GetOrdinal("Assignment_Status")) ? "" : reader.GetString(reader.GetOrdinal("Assignment_Status"));
                                // step 3
                                this.step3UserName.Text = "Last Updated By: " + (reader.IsDBNull(reader.GetOrdinal("ContractReview_QA_Status")) ? "" : reader.GetString(reader.GetOrdinal("ContractReview_QA_UserName")));
                                this.step3TimeStamp.Text = "Last Updated On: " + (reader.IsDBNull(reader.GetOrdinal("ContractReview_QA_TimeStamp")) ? "" : reader.GetDateTime(reader.GetOrdinal("ContractReview_QA_TimeStamp")).ToString(@"MM\/dd\/yyyy hh:mm tt"));
                                this.step3Status.Text = reader.IsDBNull(reader.GetOrdinal("ContractReview_QA_Status")) ? "" : reader.GetString(reader.GetOrdinal("ContractReview_QA_Status"));
                                if (step3Status.Text.Equals("Hold"))
                                    this.step3Status.Text += " - " + (reader.IsDBNull(reader.GetOrdinal("ContractReview_QA_Status_Subtype")) ? "" : reader.GetString(reader.GetOrdinal("ContractReview_QA_Status_Subtype")));
                                this.step3AssignedToLabel.Text = "Assigned To: " + (reader.IsDBNull(reader.GetOrdinal("ContractReview_QA_AssignedTo")) ? "" : reader.GetString(reader.GetOrdinal("ContractReview_QA_AssignedTo")));
                                step3CompletedOnLabel.Text = "Completed On: " + (reader.IsDBNull(reader.GetOrdinal("ContractReview_QA_CompletedOn")) || !step3Status.Text.Equals("Completed") ? "" : reader.GetDateTime(reader.GetOrdinal("ContractReview_QA_CompletedOn")).ToString(@"MM\/dd\/yyyy hh:mm tt"));
                                step3CompletedByLabel.Text = "Completed By: " + (reader.IsDBNull(reader.GetOrdinal("ContractReview_QA_CompletedBy")) || !step3Status.Text.Equals("Completed") ? "" : reader.GetString(reader.GetOrdinal("ContractReview_QA_CompletedBy")));
                                // step 4
                                this.step4UserName.Text = "Last Updated By: " + (reader.IsDBNull(reader.GetOrdinal("ContractReview_ME_UserName")) ? "" : reader.GetString(reader.GetOrdinal("ContractReview_ME_UserName")));
                                this.step4TimeStamp.Text = "Last Updated On: " + (reader.IsDBNull(reader.GetOrdinal("ContractReview_ME_TimeStamp")) ? "" : reader.GetDateTime(reader.GetOrdinal("ContractReview_ME_TimeStamp")).ToString(@"MM\/dd\/yyyy hh:mm tt"));
                                this.step4Status.Text = reader.IsDBNull(reader.GetOrdinal("ContractReview_ME_Status")) ? "" : reader.GetString(reader.GetOrdinal("ContractReview_ME_Status"));
                                if (step4Status.Text.Equals("Hold"))
                                    this.step4Status.Text += " - " + (reader.IsDBNull(reader.GetOrdinal("ContractReview_ME_Status_Subtype")) ? "" : reader.GetString(reader.GetOrdinal("ContractReview_ME_Status_Subtype")));
                                this.step4AssignedToLabel.Text = "Assigned To: " + (reader.IsDBNull(reader.GetOrdinal("ContractReview_ME_AssignedTo")) ? "" : reader.GetString(reader.GetOrdinal("ContractReview_ME_AssignedTo")));
                                step4CompletedOnLabel.Text = "Completed On: " + (reader.IsDBNull(reader.GetOrdinal("ContractReview_ME_CompletedOn")) || !step4Status.Text.Equals("Completed") ? "" : reader.GetDateTime(reader.GetOrdinal("ContractReview_ME_CompletedOn")).ToString(@"MM\/dd\/yyyy hh:mm tt"));
                                step4CompletedByLabel.Text = "Completed By: " + (reader.IsDBNull(reader.GetOrdinal("ContractReview_ME_CompletedBy")) || !step4Status.Text.Equals("Completed") ? "" : reader.GetString(reader.GetOrdinal("ContractReview_ME_CompletedBy")));
                                // step 5
                                this.step5UserName.Text = "Last Updated By: " + (reader.IsDBNull(reader.GetOrdinal("ContractReview_QE_UserName")) ? "" : reader.GetString(reader.GetOrdinal("ContractReview_QE_UserName")));
                                this.step5TimeStamp.Text = "Last Updated On: " + (reader.IsDBNull(reader.GetOrdinal("ContractReview_QE_TimeStamp")) ? "" : reader.GetDateTime(reader.GetOrdinal("ContractReview_QE_TimeStamp")).ToString(@"MM\/dd\/yyyy hh:mm tt"));
                                this.step5Status.Text = reader.IsDBNull(reader.GetOrdinal("ContractReview_QE_Status")) ? "" : reader.GetString(reader.GetOrdinal("ContractReview_QE_Status"));
                                if (step5Status.Text.Equals("Hold"))
                                    this.step5Status.Text += " - " + (reader.IsDBNull(reader.GetOrdinal("ContractReview_QE_Status_Subtype")) ? "" : reader.GetString(reader.GetOrdinal("ContractReview_QE_Status_Subtype")));
                                this.step5AssignedToLabel.Text = "Assigned To: " + (reader.IsDBNull(reader.GetOrdinal("ContractReview_QE_AssignedTo")) ? "" : reader.GetString(reader.GetOrdinal("ContractReview_QE_AssignedTo")));
                                step5CompletedOnLabel.Text = "Completed On: " + (reader.IsDBNull(reader.GetOrdinal("ContractReview_QE_CompletedOn")) || !step5Status.Text.Equals("Completed") ? "" : reader.GetDateTime(reader.GetOrdinal("ContractReview_QE_CompletedOn")).ToString(@"MM\/dd\/yyyy hh:mm tt"));
                                step5CompletedByLabel.Text = "Completed By: " + (reader.IsDBNull(reader.GetOrdinal("ContractReview_QE_CompletedBy")) || !step5Status.Text.Equals("Completed") ? "" : reader.GetString(reader.GetOrdinal("ContractReview_QE_CompletedBy")));
                                // check if everything is Completedd
                                if ((this.step3Status.Text.Equals("Completed") && this.step4Status.Text.Equals("Completed") && this.step5Status.Text.Equals("Completed")) && !this.workflowStatusLabel.Text.Equals("Completed"))
                                    completeButton.Enabled = true;
                                else
                                    completeButton.Enabled = false;
                                break;
                            case Globals.Status_Type.QuickRelease:
                                // step 2
                                this.step2UserName.Text = "Last Updated By: " + (reader.IsDBNull(reader.GetOrdinal("Assignment_Status")) ? "" : reader.GetString(reader.GetOrdinal("Assignment_UserName")));
                                this.step2TimeStamp.Text = "Last Updated On: " + (reader.IsDBNull(reader.GetOrdinal("Assignment_TimeStamp")) ? "" : reader.GetDateTime(reader.GetOrdinal("Assignment_TimeStamp")).ToString(@"MM\/dd\/yyyy hh:mm tt"));
                                this.step2Status.Text = reader.IsDBNull(reader.GetOrdinal("Assignment_Status")) ? "" : reader.GetString(reader.GetOrdinal("Assignment_Status"));
                                // step 3
                                this.step3UserName.Text = "Last Updated By: " + (reader.IsDBNull(reader.GetOrdinal("QuickRelease_Quality_UserName")) ? "" : reader.GetString(reader.GetOrdinal("QuickRelease_Quality_UserName")));
                                this.step3TimeStamp.Text = "Last Updated On: " + (reader.IsDBNull(reader.GetOrdinal("QuickRelease_Quality_TimeStamp")) ? "" : reader.GetDateTime(reader.GetOrdinal("QuickRelease_Quality_TimeStamp")).ToString(@"MM\/dd\/yyyy hh:mm tt"));
                                this.step3Status.Text = reader.IsDBNull(reader.GetOrdinal("QuickRelease_Quality_Status")) ? "" : reader.GetString(reader.GetOrdinal("QuickRelease_Quality_Status"));
                                if (step3Status.Text.Equals("Hold"))
                                    this.step3Status.Text += " - " + (reader.IsDBNull(reader.GetOrdinal("QuickRelease_Quality_Status_Subtype")) ? "" : reader.GetString(reader.GetOrdinal("QuickRelease_Quality_Status_Subtype")));
                                this.step3AssignedToLabel.Text = "Assigned To: " + (reader.IsDBNull(reader.GetOrdinal("QuickRelease_Quality_AssignedTo")) ? "" : reader.GetString(reader.GetOrdinal("QuickRelease_Quality_AssignedTo")));
                                step3CompletedOnLabel.Text = "Completed On: " + (reader.IsDBNull(reader.GetOrdinal("QuickRelease_Quality_CompletedOn")) || !step3Status.Text.Equals("Completed") ? "" : reader.GetDateTime(reader.GetOrdinal("QuickRelease_Quality_CompletedOn")).ToString(@"MM\/dd\/yyyy hh:mm tt"));
                                step3CompletedByLabel.Text = "Completed By: " + (reader.IsDBNull(reader.GetOrdinal("QuickRelease_Quality_CompletedBy")) || !step3Status.Text.Equals("Completed") ? "" : reader.GetString(reader.GetOrdinal("QuickRelease_Quality_CompletedBy")));
                                // step 4
                                this.step4UserName.Text = "Last Updated By: " + (reader.IsDBNull(reader.GetOrdinal("QuickRelease_Engineering_UserName")) ? "" : reader.GetString(reader.GetOrdinal("QuickRelease_Engineering_UserName")));
                                this.step4TimeStamp.Text = "Last Updated On: " + (reader.IsDBNull(reader.GetOrdinal("QuickRelease_Engineering_TimeStamp")) ? "" : reader.GetDateTime(reader.GetOrdinal("QuickRelease_Engineering_TimeStamp")).ToString(@"MM\/dd\/yyyy hh:mm tt"));
                                this.step4Status.Text = reader.IsDBNull(reader.GetOrdinal("QuickRelease_Engineering_Status")) ? "" : reader.GetString(reader.GetOrdinal("QuickRelease_Engineering_Status"));
                                if (step4Status.Text.Equals("Hold"))
                                    this.step4Status.Text += " - " + (reader.IsDBNull(reader.GetOrdinal("QuickRelease_Engineering_Status_Subtype")) ? "" : reader.GetString(reader.GetOrdinal("QuickRelease_Engineering_Status_Subtype")));
                                this.step4AssignedToLabel.Text = "Assigned To: " + (reader.IsDBNull(reader.GetOrdinal("QuickRelease_Engineering_AssignedTo")) ? "" : reader.GetString(reader.GetOrdinal("QuickRelease_Engineering_AssignedTo")));
                                step3CompletedOnLabel.Text = "Completed On: " + (reader.IsDBNull(reader.GetOrdinal("QuickRelease_Engineering_CompletedOn")) || !step4Status.Text.Equals("Completed") ? "" : reader.GetDateTime(reader.GetOrdinal("QuickRelease_Engineering_CompletedOn")).ToString(@"MM\/dd\/yyyy hh:mm tt"));
                                step3CompletedByLabel.Text = "Completed By: " + (reader.IsDBNull(reader.GetOrdinal("QuickRelease_Engineering_CompletedBy")) || !step4Status.Text.Equals("Completed") ? "" : reader.GetString(reader.GetOrdinal("QuickRelease_Engineering_CompletedBy")));
                                // check if everything is completed
                                if ((this.step3Status.Text.Equals("Completed") && this.step4Status.Text.Equals("Completed")) && !this.workflowStatusLabel.Text.Equals("Completed"))
                                    completeButton.Enabled = true;
                                else
                                    completeButton.Enabled = false;
                                break;
                            case Globals.Status_Type.None:
                                completeButton.Enabled = true;
                                break;
                        }

                        // do label colors
                        step2Label.BackColor = step2Status.Text.Contains("Complete") ? SystemColors.MenuHighlight : Color.Red;
                        step3Label.BackColor = step3Status.Text.Contains("Complete") ? SystemColors.MenuHighlight : Color.Red;
                        step4Label.BackColor = step4Status.Text.Contains("Complete") ? SystemColors.MenuHighlight : Color.Red;
                        step5Label.BackColor = step5Status.Text.Contains("Complete") ? SystemColors.MenuHighlight : Color.Red;
                    }
                    else
                    {
                        MessageBox.Show("Error updating layout. Please contact IT support.");
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show("Error getting type data from DB Server. Please contact IT suport.");
                }
                catch (OdbcException)
                {
                    MessageBox.Show("Error connecting to database. Please contact IT support.");
                }
            }
        }

        // submits a new workflow to the db with no
        private void createWorkflowButton_Click(object sender, EventArgs e)
        {
            // only customer service (and admints) can create wrokflow
            if (!(Globals.admin || Globals.customerServiceAccess))
            {
                MessageBox.Show("Only customer service personel can create workflows", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // check that every other workflow is completed
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "SELECT *\n" +
                    "FROM ATI_Workflow.dbo.StatusData\n" +
                    "WHERE Job = '" + jobNo + "' AND Workflow_Status <> 'Completed';";

                OdbcCommand com = new OdbcCommand(query, conn);
                OdbcDataReader reader = com.ExecuteReader();

                // if more than one 
                if (reader.Read())
                {
                    if (MessageBox.Show("There are other incomplete workflows. Are you sure you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        return;
                    }
                }
            }

            Globals.Status_Type tempType = Globals.Status_Type.Invalid;
            int next_ID = 101;

            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                // first get next Worflow ID
                string query =
                    "SELECT MAX(Workflow_ID)\n" +
                    "FROM ATI_Workflow.dbo.StatusData\n" +
                    "WHERE Job = '" + jobNo + "';";

                OdbcCommand comm = new OdbcCommand(query, conn);
                OdbcDataReader reader = comm.ExecuteReader();

                // check if you can load one row
                if (reader.Read() && !reader.IsDBNull(0))
                {
                    // increase the workflow ID by 1
                    next_ID = int.Parse(reader.GetString(0)) + 1;
                }
            }

            /*if (customer.Equals("HONDA AERO"))
            {
                HondaPOReview poReview = new HondaPOReview(jobNo, false, next_ID, false, null);
                poReview.ShowDialog();
                tempType = poReview.type;
            }
            else if (customer.Equals("ROLLS"))
            {
                RollsRoycePOReview poReview = new RollsRoycePOReview(jobNo, false, next_ID, false, null);
                poReview.ShowDialog();
                tempType = poReview.type;
            }
            else if (customer.Equals("WOODWARD"))
            {
                WoodwardPOReview poReview = new WoodwardPOReview(jobNo, false, next_ID, false, null);
                poReview.ShowDialog();
                tempType = poReview.type;
            }
            else if (customer.Equals("EATON"))
            {
                EatonPOReview poReview = new EatonPOReview(jobNo, false, next_ID, false, null);
                poReview.ShowDialog();
                tempType = poReview.type;
            }
            else if (customer.Equals("ABLE"))
            {
                AblePOReview poReview = new AblePOReview(jobNo, false, next_ID, false, null);
                poReview.ShowDialog();
                tempType = poReview.type;
            }
            else if (customer.Equals("WENCOR"))
            {
                WencorPOReview poReview = new WencorPOReview(jobNo, false, next_ID, false, null);
                poReview.ShowDialog();
                tempType = poReview.type;
            }
            else
            {*/
                GenericPOReview poReview = new GenericPOReview(jobNo, false, next_ID, false, null, customer);
                poReview.ShowDialog();
                tempType = poReview.type;
            //}

            // in case form was closed prematurely
            if (tempType == Globals.Status_Type.Invalid)
                return;

            // add new workflow to list and select
            UpdateWorkflowDataGridViewFromSQL();
            int index = IsItemInDataGridviewColumn(workflowDataGridView, 1, next_ID.ToString());
            workflowDataGridView.Rows[index].Selected = true;
            type = tempType;
            UpdateLayout();
            workflowDataGridView_CellClick(new object(), new DataGridViewCellEventArgs(0, index));
        }

        private void completeButton_Click(object sender, EventArgs e)
        {
            if (!(Globals.admin || Globals.qeAccess || Globals.leadAccess))
            {
                MessageBox.Show("Only QE, leads and admin can complete a workflow");
                return;
            }

            // check that every workflow before this one was completed
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "SELECT *\n" +
                    "FROM [ATI_Workflow].[dbo].[StatusData]\n" +
                    "WHERE Job = '" + jobNoLabel.Text.Trim() + "' AND Workflow_Status <> 'Completed'  AND Workflow_ID < " + workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString() + ";";

                OdbcCommand com = new OdbcCommand(query, conn);
                if (com.ExecuteReader().Read())
                {
                    MessageBox.Show("You have to complete all previous workflows before closing this one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            // update status of workflow to show this
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "UPDATE [ATI_Workflow].[dbo].[StatusData]\n" +
                    "SET[Workflow_Status] = 'Completed'\n" +
                    "WHERE Job = '" + jobNoLabel.Text.Trim() + "' AND Workflow_ID = '" + workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString() + "';";

                OdbcCommand com = new OdbcCommand(query, conn);
                if (com.ExecuteNonQuery() != 1)
                    MessageBox.Show(Globals.generic_IT_error);
            }

            UpdateFromDB(int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()));
            int index = workflowDataGridView.SelectedRows[0].Index;
            UpdateWorkflowDataGridViewFromSQL();
            workflowDataGridView.Rows[index].Selected = true;
            workflowDataGridView_CellClick(new object(), new DataGridViewCellEventArgs(0, index));
        }

        private void workflowDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (workflowDataGridView.SelectedRows[0].Cells[1].Value != null)
                    UpdateFromDB(int.Parse(workflowDataGridView.SelectedRows[0].Cells[1].Value.ToString()));
            }
        }
    }
}