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
    public partial class TaskViewer : Form
    {
        private BindingSource bindingSource = new BindingSource();
        JobListViewer jobViewerRef;

        public TaskViewer(JobListViewer jobViewer)
        {
            InitializeComponent();
            this.jobViewerRef = jobViewer;
        }

        private void TaskViewer_Load(object sender, EventArgs e)
        {
            // load info into labels
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "SELECT\n" +
                    "[firstName]\n" +
                    ",[lastName]\n" +
                    "FROM[ATI_Workflow].[dbo].[UserData]\n" +
                    "WHERE userName = '" + Globals.userName + "';";

                OdbcCommand com = new OdbcCommand(query, conn);
                OdbcDataReader reader = com.ExecuteReader();

                if (reader.Read())
                {
                    firstNameLabel.Text = reader.GetString(0);
                    lastNameLabel.Text = reader.GetString(1);
                    userNameLabel.Text = Globals.userName;
                }

                // set up datagridview
                dataGridView.DataSource = bindingSource;
                dataGridView.ReadOnly = true;
                dataGridView.MultiSelect = true;
            }

            {
                // load tasks assigned that aren't complete
                string query =
                    "SELECT ut.Job, ut.Workflow_ID AS 'Workflow ID', ut.[Form Assigned], AssignedTo AS 'Assigned To', ut.Status\n" +
                    "FROM(\n" +
                    "SELECT cr_ME_QE.Job, cr_ME_QE.Workflow_ID, cr_ME_QE.ContractReview_Type + ' Contract Review' AS 'Form Assigned', cr_ME_QE.AssignedTo, cr_ME_QE.Status\n" +
                    "FROM ATI_Workflow.dbo.ContractReview_ME_QE AS cr_ME_QE\n" +
                    "UNION\n" +
                    "SELECT cr_QA.Job, cr_QA.Workflow_ID, 'QA Contract Review' AS 'Form Assigned', cr_QA.AssignedTo, cr_QA.Status\n" +
                    "FROM ATI_Workflow.dbo.ContractReview_QA AS cr_QA\n" +
                    "UNION\n" +
                    "SELECT q_E.Job, q_E.Workflow_ID, 'ME Quick Release' AS 'Form Assigned', q_E.AssignedTo, q_E.Status\n" +
                    "FROM ATI_Workflow.dbo.QuickRelease_Engineering AS q_E\n" +
                    "UNION\n" +
                    "SELECT q_A.Job, q_A.Workflow_ID, 'QA Quick Release' AS 'Form Assigned', q_A.AssignedTo, q_A.Status\n" +
                    "FROM ATI_Workflow.dbo.QuickRelease_Quality AS q_A\n" +
                    ") AS uT\n" +
                    "LEFT JOIN\n" +
                    "(\n" +
                    "SELECT Job, Part_Number\n" +
                    "FROM PRODUCTION.dbo.Job AS j\n" +
                    "UNION\n" +
                    "SELECT wb.Job, wb.Part_Number\n" +
                    "FROM ATI_Workflow.dbo.Job AS wb\n" +
                    ") AS jT\n" +
                    "ON jT.Job = uT.Job\n" +
                    "WHERE AssignedTo = '" + Globals.userName + "' AND Status <> 'Completed';";

                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Globals.binding_connection_string);

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                bindingSource.DataSource = dt;

                // update Rows label
                tasksAssignedLabel.Text = dt.Rows.Count.ToString();
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            // open job list viewer
            this.Hide();
            jobViewerRef.Show();
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // check that the cell is valid
            if (e == null)
                return;
            if (dataGridView.Rows[e.RowIndex].Cells[0].Value == null || dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString().Trim().Length == 0)
                return;

            // launch status page and pass job, partno and customer as arguments
            this.Hide();
            Form statusPage = new StatusPage(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString(), dataGridView.Rows[e.RowIndex].Cells[1].Value == null ? -1 : int.Parse(dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString()), jobViewerRef);
            statusPage.FormClosed += (s, args) => this.Close();
            statusPage.Show();
        }

        private void TaskViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            // open job list viewer
            this.Hide();
            jobViewerRef.Show();
        }
    }
}
