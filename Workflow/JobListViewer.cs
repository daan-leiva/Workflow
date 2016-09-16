using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Collections.Generic;

namespace Workflow
{
    public partial class JobListViewer : Form
    {
        private BindingSource bindingSource = new BindingSource();

        public JobListViewer()
        {
            InitializeComponent();
        }

        private void JobListViewer_Load(object sender, EventArgs e)
        {
            // set date time picker range to between a year from now and today
            startDateTimePicker.Value = DateTimePicker.MinimumDateTime;
            endDateTimePicker.Value = DateTime.Now;

            // set up hold list
            holdComboBox.Items.Add("Any");
            holdComboBox.Items.Add("Cust");
            holdComboBox.Items.Add("Engi");
            holdComboBox.Items.Add("QA");
            holdComboBox.SelectedIndex = holdComboBox.FindString("Any");
            holdComboBox.Enabled = false;
            holdComboBox.Visible = false;

            // add event handlers for updating the datagridview
            startDateTimePicker.ValueChanged += updateDataGridView;
            endDateTimePicker.ValueChanged += updateDataGridView;
            createJobCustomersComboBox.SelectedIndexChanged += updateDataGridView;
            partNoTextBox.TextChanged += updateDataGridView;
            jobTextBox.TextChanged += updateDataGridView;
            holdComboBox.SelectedIndexChanged += updateDataGridView;

            // set up datagridview
            dataGridView.DataSource = bindingSource;
            dataGridView.ReadOnly = true;
            dataGridView.MultiSelect = false;

            updateDataGridView(new object(), new EventArgs());

            inProgressRadioButton.CheckedChanged += updateDataGridView;
            completedRadioButton.CheckedChanged += updateDataGridView;
            holdStatusRadioButton.CheckedChanged += updateDataGridView;
            anyStatusRadioButton.CheckedChanged += updateDataGridView;

            yesHotRadioButton.CheckedChanged += updateDataGridView;
            noHotRadioButton.CheckedChanged += updateDataGridView;
            newHotRadioButton.CheckedChanged += updateDataGridView;
            anyHotRadioButton.CheckedChanged += updateDataGridView;

            inProgressRadioButton.Checked = true;
            anyHotRadioButton.Checked = true;

            workflowRadioButton.Checked = true;
            workflowRadioButton.CheckedChanged += this.updateDataGridView;
            formsDetailRadioButton.CheckedChanged += this.updateDataGridView;

            // set up user data
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
            }

            // only admin can edit users
            if (Globals.admin)
            {
                registerUserButton.Visible = true;
                registerUserButton.Enabled = true;
                editUserButton.Visible = true;
                editUserButton.Enabled = true;
            }
            else
            {
                registerUserButton.Visible = false;
                registerUserButton.Enabled = false;
                editUserButton.Visible = false;
                editUserButton.Enabled = false;
            }


            // load into comboBox
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "SELECT DISTINCT uT.Customer\n" +
                    "FROM\n" +
                    "(\n" +
                    "SELECT jT.Customer\n" +
                    "FROM PRODUCTION.dbo.Job AS jT\n" +
                    "UNION\n" +
                    "SELECT wJt.Customer\n" +
                    "FROM ATI_Workflow.dbo.Job AS wjT\n" +
                    ") AS uT\n" +
                    "ORDER BY uT.Customer;";

                OdbcCommand com = new OdbcCommand(query, conn);
                OdbcDataReader reader = com.ExecuteReader();

                createJobCustomersComboBox.Items.Add("All");
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                        createJobCustomersComboBox.Items.Add(reader.GetString(0));
                }
            }

            // set up customer combo box
            createJobCustomersComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            createJobCustomersComboBox.SelectedIndexChanged += this.CheckValidComboBox;
        }

        private void updateDataGridView(object sender, EventArgs e)
        {
            if (workflowRadioButton.Checked)
            {
                string filter = string.Empty;

                if (inProgressRadioButton.Checked)
                    filter += " AND statusT.Workflow_Status = 'In Progress'";
                else if (completedRadioButton.Checked)
                    filter += " AND statusT.Workflow_Status = 'Completed'";
                else if (holdStatusRadioButton.Checked)
                {
                    filter += " AND statusT.Workflow_Status = 'Hold'";
                    if (holdComboBox.SelectedItem.Equals("QA"))
                        filter += " AND statusT.QA_Hold = 1";
                    else if (holdComboBox.SelectedItem.Equals("Engi"))
                        filter += " AND statusT.Engi_Hold = 1";
                    else if (holdComboBox.SelectedItem.Equals("Cust"))
                        filter += " AND statusT.Cust_Hold = 1";
                }

                if (yesHotRadioButton.Checked)
                    filter += " AND statusT.Hot = '1'";
                else if (noHotRadioButton.Checked)
                    filter += " AND statusT.Hot = '0'";
                else if (newHotRadioButton.Checked)
                    filter += " AND genericPoT.Question4_Response = 'Yes'";

                string query =
                    "SELECT uT.Job, uT.Customer, uT.Part_Number AS 'Part Number', statusT.Workflow_ID AS 'Workflow ID', statusT.Workflow_Status AS 'Status', statusT.Type, (CASE WHEN statusT.Hot = 1 THEN 'Yes' WHEN statusT.Hot IS NULL THEN '' ELSE 'No' END) AS 'Hot', genericPoT.Question4_Response AS 'New Part',\n" +
                    "statusT.CreatedOn AS 'Created On',\n" +
                    "(SELECT MAX(mDate) FROM(VALUES(meT.LastUpdatedOn), (qeT.LastUpdatedOn), (qaT.LastUpdatedOn), (qreT.LastUpdatedOn), (qrqT.LastUpdatedOn), (statusT.CreatedOn)) AS AllDates(mDate)) AS 'Last Updated On',\n" +
                    "DATEDIFF(day, (SELECT MAX(mDate) FROM(VALUES(meT.LastUpdatedOn), (qeT.LastUpdatedOn), (qaT.LastUpdatedOn), (qreT.LastUpdatedOn), (qrqT.LastUpdatedOn), (statusT.CreatedOn)) AS AllDates(mDate)), GETDATE()) AS 'Days Inactive',\n" +
                    "STUFF(COALESCE(', ' + (CASE WHEN assignT.Status <> 'Completed' THEN 'ASSIGN' ELSE NULL END), '') + \n" +
                    "COALESCE(', ' + (CASE WHEN (statusT.Type = 'Contract Review' AND meT.Status = 'Completed' AND qeT.Status = 'Completed' AND qaT.Status = 'Completed' AND statusT.Workflow_Status <> 'Completed') OR\n" +
                                                "(statusT.Type = 'Quick Release' AND qret.Status = 'Completed' AND qrqt.Status = 'Completed' AND statusT.Workflow_Status <> 'Completed') OR\n" +
                                                "(statusT.Type = 'None' AND statusT.Workflow_Status <> 'Completed') THEN 'APPROVAL' ELSE NULL END), '') +\n" +
                    "COALESCE(', ' + (CASE WHEN (statusT.Type = 'Contract Review' AND meT.Status <> 'Completed') OR (statusT.Type = 'Quick Release' AND qret.Status <> 'Completed') THEN 'ME' ELSE NULL END), '') +\n" +
                    "COALESCE(', ' + (CASE WHEN (statusT.Type = 'Contract Review' AND qeT.Status <> 'Completed') THEN 'QE' ELSE NULL END), '') +\n" +
                    "COALESCE(', ' + (CASE WHEN (statusT.Type = 'Contract Review' AND qaT.Status <> 'Completed') OR (statusT.Type = 'Quick Release' AND qrqT.Status <> 'Completed') THEN 'QA' ELSE NULL END), '')\n" +
                    ", 1, 2, '') AS 'Incomplete Forms'\n" +
                    "FROM\n" +
                    "(SELECT Job, Customer, Part_Number\n" +
                    "FROM PRODUCTION.dbo.Job\n" +
                    "UNION\n" +
                    "SELECT wJ.Job, wJ.Customer, wJ.Part_Number\n" +
                    "FROM ATI_Workflow.dbo.Job AS wJ\n" +
                    ") AS uT\n" +
                    "LEFT JOIN ATI_Workflow.dbo.StatusData AS statusT\n" +
                    "ON statusT.Job = uT.Job\n" +
                    "LEFT JOIN[ATI_Workflow].[dbo].ContractReview_ME_QE AS meT\n" +
                    "ON meT.ContractReview_Type = 'ME' AND statusT.Job = meT.job AND statusT.Workflow_ID = meT.Workflow_ID\n" +
                    "LEFT JOIN [ATI_Workflow].[dbo].ContractReview_ME_QE AS qeT\n" +
                    "ON qeT.ContractReview_Type = 'QE' AND statusT.Job = qeT.Job AND statusT.Workflow_ID = qeT.Workflow_ID\n" +
                    "LEFT JOIN [ATI_Workflow].[dbo].[ContractReview_QA] AS qaT\n" +
                    "ON qaT.job = statusT.Job AND qaT.Workflow_ID = statusT.Workflow_ID\n" +
                    "LEFT JOIN [ATI_Workflow].[dbo].QuickRelease_Engineering AS qreT\n" +
                    "ON qreT.Job = statusT.Job AND qreT.Workflow_ID = statusT.Workflow_ID\n" +
                    "LEFT JOIN [ATI_Workflow].[dbo].QuickRelease_Quality AS qrqT\n" +
                    "ON qrqT.Job = statusT.Job AND qrqT.Workflow_ID = statusT.Workflow_ID\n" +
                    "LEFT JOIN [ATI_Workflow].[dbo].AssignmentInfo AS assignT\n" +
                    "ON assignT.Job = statusT.Job AND assignT.Workflow_ID = statusT.Workflow_ID\n" +
                    "LEFT JOIN [ATI_Workflow].[dbo].Generic_PO_Review AS genericPoT\n" +
                    "ON genericPoT.Job = statusT.Job AND genericPoT.Workflow_ID = statusT.Workflow_ID\n" +
                    "WHERE uT.Job LIKE '%" + jobTextBox.Text.Trim() + "%' AND uT.Part_Number LIKE '%" + partNoTextBox.Text.Trim() + "%'\n" +
                    "AND uT.Customer LIKE '%" + (createJobCustomersComboBox.SelectedItem == null || createJobCustomersComboBox.SelectedItem.ToString().Contains("All") ? string.Empty : createJobCustomersComboBox.SelectedItem.ToString()) + "%'\n" +
                    "AND (statusT.CreatedOn >= CONVERT(DATETIME, '" + startDateTimePicker.Value.ToString() + "') AND statusT.CreatedOn <= CONVERT(DATETIME, '" + endDateTimePicker.Value.ToString() + "')" + " OR statusT.Job IS NULL)" + filter + ";";

                // clear datagridview
                dataGridView.Columns.Clear();
                dataGridView.Refresh();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Globals.binding_connection_string);

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                bindingSource.DataSource = dt;

                DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
                btnColumn.HeaderText = "Create Workflow";
                btnColumn.Text = "Create Workflow";
                btnColumn.UseColumnTextForButtonValue = true;

                this.dataGridView.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                this.dataGridView.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                this.dataGridView.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                dataGridView.Columns.Add(btnColumn);

                // update Rows label
                rowsLabel.Text = "Rows: " + dt.Rows.Count;
            }
            else if(formsDetailRadioButton.Checked)
            {
                string filter = string.Empty;

                if (inProgressRadioButton.Checked)
                    filter += " AND (uT.Status = 'In Progress' OR uT.Status = 'Unassigned' OR uT.Status = 'Assigned')";
                else if (completedRadioButton.Checked)
                    filter += " AND uT.Status = 'Completed'";
                else if(holdStatusRadioButton.Checked)
                    filter += " AND uT.Status LIKE '%Hold%'";

                if (yesHotRadioButton.Checked)
                    filter += " AND statusT.Hot = '1'";
                else if (noHotRadioButton.Checked)
                    filter += " AND statusT.Hot = '0'";

                string query =
                    "SELECT ut.Job, jT.Customer, jt.Part_Number AS 'Part Number', ut.Workflow_ID AS 'Workflow ID', ut.[Form Type], AssignedTo AS 'Assigned To', (CASE WHEN uT.Status_Subtype IS NULL OR LEN(uT.Status_Subtype) = 0 THEN ut.Status ELSE uT.Status + ' - ' +  ut.Status_Subtype END) AS 'Status',(CASE WHEN statusT.Hot = 1 THEN 'Yes' ELSE 'No' END) AS 'Hot?'\n" +
                    "FROM(\n" +
                    "SELECT cr_ME_QE.Job, cr_ME_QE.Workflow_ID, cr_ME_QE.ContractReview_Type + ' Contract Review' AS 'Form Type', cr_ME_QE.AssignedTo, cr_ME_QE.Status, cr_ME_QE.Status_Subtype\n" +
                    "FROM ATI_Workflow.dbo.ContractReview_ME_QE AS cr_ME_QE\n" +
                    "UNION\n" +
                    "SELECT cr_QA.Job, cr_QA.Workflow_ID, 'QA Contract Review' AS 'Form Type', cr_QA.AssignedTo, cr_QA.Status, cr_QA.Status_Subtype\n" +
                    "FROM ATI_Workflow.dbo.ContractReview_QA AS cr_QA\n" +
                    "UNION\n" +
                    "SELECT q_E.Job, q_E.Workflow_ID, 'ME Quick Release' AS 'Form Type', q_E.AssignedTo, q_E.Status, q_E.Status_Subtype\n" +
                    "FROM ATI_Workflow.dbo.QuickRelease_Engineering AS q_E\n" +
                    "UNION\n" +
                    "SELECT q_A.Job, q_A.Workflow_ID, 'QA Quick Release' AS 'Form Type', q_A.AssignedTo, q_A.Status, q_A.Status_Subtype\n" +
                    "FROM ATI_Workflow.dbo.QuickRelease_Quality AS q_A\n" +
                    ") AS uT\n" +
                    "LEFT JOIN\n" +
                    "(\n" +
                    "SELECT Job, Part_Number, j.Customer\n" +
                    "FROM PRODUCTION.dbo.Job AS j\n" +
                    "UNION\n" +
                    "SELECT wb.Job, wb.Part_Number, wb.Customer\n" +
                    "FROM ATI_Workflow.dbo.Job AS wb\n" +
                    ") AS jT\n" +
                    "ON jT.Job = uT.Job\n" +
                    "LEFT JOIN ATI_Workflow.dbo.StatusData AS statusT\n" +
                    "ON statusT.Job = uT.Job AND statusT.Workflow_ID = ut.Workflow_ID\n" +
                    "WHERE jT.Job LIKE '%" + jobTextBox.Text + "%' AND jT.Part_Number LIKE '%" + partNoTextBox.Text + "%'\n" +
                    "AND Customer LIKE '%" + (createJobCustomersComboBox.SelectedItem == null || createJobCustomersComboBox.SelectedItem.ToString().Contains("All") ? string.Empty : createJobCustomersComboBox.SelectedItem.ToString()) + "%'\n" +
                    filter;

                // clear datagridview
                dataGridView.Columns.Clear();
                dataGridView.Refresh();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Globals.binding_connection_string);

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                bindingSource.DataSource = dt;

                // update Rows label
                rowsLabel.Text = "Rows: " + dt.Rows.Count;
            }
        }

        private void CheckValidComboBox(object sender, EventArgs e)
        {
            if (createJobCustomersComboBox.SelectedItem == null)
                errorProvider.SetError(createJobCustomersComboBox, "Need to select customer");
            else
                errorProvider.SetError(createJobCustomersComboBox, "");
        }

        private void createJobButton_Click(object sender, EventArgs e)
        {
            if (createJobCustomersComboBox.SelectedItem == null || createJobCustomersComboBox.SelectedItem.ToString().Length == 0 || createJobCustomersComboBox.SelectedItem.ToString().Contains("All"))
            {
                MessageBox.Show("Invalid selection of customer");
                return;
            }

            // only customer service (and admints) can create jobs
            if (!(Globals.admin || Globals.customerServiceAccess))
            {
                MessageBox.Show("Only customer service personel can create jobs", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // check that customer combo box has been selected
            if (createJobCustomersComboBox.SelectedItem == null)
            {
                errorProvider.SetError(createJobCustomersComboBox, "Need to select customer");
                return;
            }

            /*if (createJobCustomersComboBox.SelectedItem.ToString().Equals("HONDA AERO"))
            {
                // launch honda PO
                this.Hide();
                Form hondaPOForm = new HondaPOReview("-1", false, 101, true, this);
                hondaPOForm.FormClosed += (s, args) => this.Close();
                hondaPOForm.Show();
            }
            else if(createJobCustomersComboBox.SelectedItem.ToString().Equals("ROLLS"))
            {
                // launch rolls PO
                this.Hide();
                Form rollsPOForm = new RollsRoycePOReview("-1", false, 101, true, this);
                rollsPOForm.FormClosed += (s, args) => this.Close();
                rollsPOForm.Show();
            }
            else if (createJobCustomersComboBox.SelectedItem.ToString().Equals("WOODWARD"))
            {
                // launch woodward PO
                this.Hide();
                Form woodwardPOForm = new WoodwardPOReview("-1", false, 101, true, this);
                woodwardPOForm.FormClosed += (s, args) => this.Close();
                woodwardPOForm.Show();
            }
            else if (createJobCustomersComboBox.SelectedItem.ToString().Equals("EATON"))
            {
                // launch eaton PO
                this.Hide();
                Form eatonPOForm = new EatonPOReview("-1", false, 101, true, this);
                eatonPOForm.FormClosed += (s, args) => this.Close();
                eatonPOForm.Show();
            }
            else if (createJobCustomersComboBox.SelectedItem.ToString().Equals("ABLE"))
            {
                // launch able PO
                this.Hide();
                Form ablePOForm = new AblePOReview("-1", false, 101, true, this);
                ablePOForm.FormClosed += (s, args) => this.Close();
                ablePOForm.Show();
            }
            if (createJobCustomersComboBox.SelectedItem.ToString().Equals("WENCOR"))
            {
                // launch wencor PO
                this.Hide();
                Form wencorPOForm = new WencorPOReview("-1", false, 101, true, this);
                wencorPOForm.FormClosed += (s, args) => this.Close();
                wencorPOForm.Show();
            }
            else
            {*/
            // launch generic PO
            this.Hide();
            Form genericPOForm = new GenericPOReview("-1", false, 101, true, this, createJobCustomersComboBox.SelectedItem.ToString());
            genericPOForm.FormClosed += (s, args) => this.Close();
            genericPOForm.Show();
            //}
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // check that the cell is valid
            if (e == null || e.RowIndex == -1)
                return;
            if (dataGridView.Rows[e.RowIndex].Cells[0].Value == null || dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString().Trim().Length == 0)
                return;

            if (workflowRadioButton.Checked)
            {
                // launch status page and pass job, partno and customer as arguments
                this.Hide();
                int workflow = -1;
                Form statusPage = new StatusPage(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString(), dataGridView.Rows[e.RowIndex].Cells[3].Value == null ? -1 : (int.TryParse(dataGridView.Rows[e.RowIndex].Cells[3].Value.ToString(), out workflow) ? workflow : -1), this);
                statusPage.FormClosed += (s, args) => this.Close();
                statusPage.Show();
            }
            else if(formsDetailRadioButton.Checked)
            {
                // launch status page and pass job, partno and customer as arguments
                this.Hide();
                int workflow = -1;
                Form statusPage = new StatusPage(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString(), dataGridView.Rows[e.RowIndex].Cells[3].Value == null ? -1 : (int.TryParse(dataGridView.Rows[e.RowIndex].Cells[3].Value.ToString(), out workflow) ? workflow : -1), this);
                statusPage.FormClosed += (s, args) => this.Close();
                statusPage.Show();
            }
        }

        private void tastkListButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form jobList = new TaskViewer(this);
            jobList.FormClosed += (s, args) => this.Close();
            jobList.Show();
        }

        private void JobListViewer_Shown(object sender, EventArgs e)
        {
            endDateTimePicker.Value = DateTime.Now;
            updateDataGridView(new object(), new EventArgs());
        }

        private void holdStatusRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (holdStatusRadioButton.Checked)
            {
                holdComboBox.Visible = true;
                holdComboBox.Enabled = true;
            }
            else
            {
                holdComboBox.Visible = false;
                holdComboBox.Enabled = false;
            }
        }

        private void registerUserButton_Click(object sender, EventArgs e)
        {
            UserRegistrationForm userRegistration = new UserRegistrationForm();
            userRegistration.ShowDialog();
        }

        private void editUserButton_Click(object sender, EventArgs e)
        {
            EditUserForm userEdit = new EditUserForm();
            userEdit.ShowDialog();
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                if (dataGridView.Rows[e.RowIndex].Cells[0].Value == null || dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString().Trim().Length == 0 ||
                    dataGridView.Rows[e.RowIndex].Cells[1].Value == null || dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString().Trim().Length == 0)
                {
                    return;
                }

                // only customer service (and admints) can create wrokflow
                if (!(Globals.admin || Globals.customerServiceAccess))
                {
                    MessageBox.Show("Only customer service personel can create workflows", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //  LAUNCH PO
                string jobNo = dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
                bool readonlyPO = false;

                // find next workflow ID on the list
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
                        string currentIdStr = reader.GetString(0);
                        int currentIdint = int.Parse(currentIdStr);
                        next_ID = currentIdint + 1;
                    }
                }

                bool newJob = false;
                string customer = dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString().Trim();

                GenericPOReview poReview = new GenericPOReview(jobNo, readonlyPO, next_ID, newJob, this, customer);
                poReview.FormClosed += (s, args) => this.Close();
                poReview.Show();

                // TODO - Launch status viewer for that PO/customer

            }
        }
    }
}
