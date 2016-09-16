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
    public partial class AssignTaskForm : Form
    {
        string jobNo;
        int workflow_ID;
        Globals.Status_Type type;

        public AssignTaskForm(string jobNo, int workflow_ID)
        {
            InitializeComponent();

            this.jobNo = jobNo;
            this.workflow_ID = workflow_ID;

            // load type
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "SELECT [Type]\n" +
                    "FROM[ATI_Workflow].[dbo].[StatusData]\n" +
                    "WHERE Job = '" + jobNo + "' AND Workflow_ID = " + workflow_ID + ";\n";

                OdbcCommand com = new OdbcCommand(query, conn);
                OdbcDataReader reader = com.ExecuteReader();

                if (reader.Read() && !reader.IsDBNull(0))
                {
                    string type_str = reader.GetString(0);
                    if (type_str.Equals("Contract Review"))
                        type = Globals.Status_Type.ContractReview;
                    else if (type_str.Equals("Quick Release"))
                        type = Globals.Status_Type.QuickRelease;
                    else if (type_str.Equals("None"))
                        type = Globals.Status_Type.None;
                    else
                    {
                        MessageBox.Show("Cannot task sheet. Contact IT support");
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Cannot task sheet. Contact IT support");
                    this.Close();
                }
            }
        }

        private void AssignTaskForm_Load(object sender, EventArgs e)
        {
            // populate Combo boxes
            PopulateComboBox(this.step3ComboBox);
            PopulateComboBox(this.step4ComboBox);
            PopulateComboBox(this.step5ComboBox);

            // base on type rename labels and hide unnecesaries
            if (type == Globals.Status_Type.ContractReview)
            {
                step3TitleLabel.Text = "ME Contract Review Check List Form";
                step4TitleLabel.Text = "QA Contract Review Check List Form";
                step5TitleLabel.Text = "QE Contract Review Check List Form";
            }
            else if (type == Globals.Status_Type.QuickRelease)
            {
                step3TitleLabel.Text = "Engineering Quick Release Form";
                step4TitleLabel.Text = "Quality Quick Release Form";

                // hide step 5, not needed for quick release
                step5TitleLabel.Hide();
                step5ComboBox.Hide();
            }

            // load old assigns if any
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "SELECT\n" +
                    "quickRelEngT.AssignedTo AS QuickRelease_Engineering_UserName\n" +
                    ",quickRelQuaT.AssignedTo AS QuickRelease_Quality_UserName\n" +
                    ",contractReviewMET.AssignedTo AS ContractReview_ME_UserName\n" +
                    ",contractReviewQAT.AssignedTo AS ContractReview_QA_UserName\n" +
                    ",contractReviewQET.AssignedTo AS ContractReview_QE_UserName\n" +
                    "FROM [ATI_Workflow].[dbo].[StatusData] AS statusT\n" +
                    "LEFT JOIN[ATI_Workflow].[dbo].[Honda_PO_Review] AS hondaPOT\n" +
                    "ON hondaPOT.Job = statusT.Job AND hondaPOT.Workflow_ID = statusT.Workflow_ID\n" +
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
                    "WHERE statusT.Job = '" + jobNo + "' AND statusT.Workflow_ID = '" + workflow_ID + "';";

                OdbcCommand com = new OdbcCommand(query, conn);
                OdbcDataReader reader = com.ExecuteReader();
                if (reader.Read())
                {
                    switch (type)
                    {
                        case Globals.Status_Type.ContractReview:
                            if (!reader.IsDBNull(reader.GetOrdinal("ContractReview_ME_UserName")))
                                step3ComboBox.SelectedIndex = step3ComboBox.FindString(reader.GetString(reader.GetOrdinal("ContractReview_ME_UserName")));
                            if (!reader.IsDBNull(reader.GetOrdinal("ContractReview_QA_UserName")))
                                step4ComboBox.SelectedIndex = step4ComboBox.FindString(reader.GetString(reader.GetOrdinal("ContractReview_QA_UserName")));
                            if (!reader.IsDBNull(reader.GetOrdinal("ContractReview_QE_UserName")))
                                step5ComboBox.SelectedIndex = step5ComboBox.FindString(reader.GetString(reader.GetOrdinal("ContractReview_QE_UserName")));
                            break;
                        case Globals.Status_Type.QuickRelease:
                            if (!reader.IsDBNull(reader.GetOrdinal("QuickRelease_Engineering_UserName")))
                                step3ComboBox.SelectedIndex = step3ComboBox.FindString(reader.GetString(reader.GetOrdinal("QuickRelease_Engineering_UserName")));
                            if (!reader.IsDBNull(reader.GetOrdinal("QuickRelease_Quality_UserName")))
                                step4ComboBox.SelectedIndex = step4ComboBox.FindString(reader.GetString(reader.GetOrdinal("QuickRelease_Quality_UserName")));
                            break;
                    }
                }
            }
        }

        private void PopulateComboBox(ComboBox c)
        {
            c.Items.Clear();

            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "SELECT [userName]\n" +
                    "FROM[ATI_Workflow].[dbo].[UserData];";
                OdbcCommand com = new OdbcCommand(query, conn);
                OdbcDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                        c.Items.Add(reader.GetString(0));
                }
            }

            c.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (type == Globals.Status_Type.ContractReview)
            {
                // update ME table
                if (step3ComboBox.SelectedIndex > -1)
                {
                    using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                    {
                        conn.Open();

                        string query =
                            "UPDATE [ATI_Workflow].[dbo].[ContractReview_ME_QE]\n" +
                            "SET [AssignedTo] = '" + step3ComboBox.SelectedItem.ToString() + "'\n" +
                            ",[AssignedOn] = '" + DateTime.Now.ToString() + "'\n" +
                            ",[Status] = 'Assigned'\n" +
                            "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "' AND ContractReview_Type = 'ME';";

                        OdbcCommand com = new OdbcCommand(query, conn);
                        if (com.ExecuteNonQuery() != 1)
                            MessageBox.Show(Globals.generic_IT_error);
                    }
                }

                // update QA table
                if (step4ComboBox.SelectedIndex > -1)
                {
                    using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                    {
                        conn.Open();

                        string query =
                            "UPDATE [ATI_Workflow].[dbo].[ContractReview_QA]\n" +
                            "SET [AssignedTo] = '" + step4ComboBox.SelectedItem.ToString() + "'\n" +
                            ",[AssignedOn] = '" + DateTime.Now.ToString() + "'\n" +
                            ",[Status] = 'Assigned'\n" +
                            "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "';";

                        OdbcCommand com = new OdbcCommand(query, conn);
                        if (com.ExecuteNonQuery() != 1)
                            MessageBox.Show(Globals.generic_IT_error);
                    }
                }

                // update QE table
                if (step5ComboBox.SelectedIndex > -1)
                {
                    using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                    {
                        conn.Open();

                        string query =
                            "UPDATE [ATI_Workflow].[dbo].[ContractReview_ME_QE]\n" +
                            "SET [AssignedTo] = '" + step5ComboBox.SelectedItem.ToString() + "'\n" +
                            ",[AssignedOn] = '" + DateTime.Now.ToString() + "'\n" +
                            ",[Status] = 'Assigned'\n" +
                            "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "' AND ContractReview_Type = 'QE';";

                        OdbcCommand com = new OdbcCommand(query, conn);
                        if (com.ExecuteNonQuery() != 1)
                            MessageBox.Show(Globals.generic_IT_error);
                    }
                }


            }
            else if (type == Globals.Status_Type.QuickRelease)
            {
                // update quick release engineering table
                if (step3ComboBox.SelectedIndex > -1)
                {
                    using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                    {
                        conn.Open();

                        string query =
                            "UPDATE [ATI_Workflow].[dbo].[QuickRelease_Engineering]\n" +
                            "SET [AssignedTo] = '" + step3ComboBox.SelectedItem.ToString() + "'\n" +
                            ",[AssignedOn] = '" + DateTime.Now.ToString() + "'\n" +
                            ",[Status] = 'Assigned'\n" +
                            "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "';";

                        OdbcCommand com = new OdbcCommand(query, conn);
                        if (com.ExecuteNonQuery() != 1)
                            MessageBox.Show(Globals.generic_IT_error);
                    }
                }

                // update quick release quality table
                if (step4ComboBox.SelectedIndex > -1)
                {
                    using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
                    {
                        conn.Open();

                        string query =
                            "UPDATE [ATI_Workflow].[dbo].[QuickRelease_Quality]\n" +
                            "SET [AssignedTo] = '" + step4ComboBox.SelectedItem.ToString() + "'\n" +
                            ",[AssignedOn] = '" + DateTime.Now.ToString() + "'\n" +
                            ",[Status] = 'Assigned'\n" +
                            "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "';";

                        OdbcCommand com = new OdbcCommand(query, conn);
                        if (com.ExecuteNonQuery() != 1)
                            MessageBox.Show(Globals.generic_IT_error);
                    }
                }
            }

            // update assignment table
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "UPDATE [ATI_Workflow].[dbo].[AssignmentInfo]\n" +
                    "SET[LastUpdatedBy] = '" + Globals.userName + "'\n" +
                    ",[LastUpdatedOn] = '" + DateTime.Now.ToString() + "'\n" +
                    ",[Status] = 'Completed'\n" +
                    "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "';";

                OdbcCommand com = new OdbcCommand(query, conn);

                if (com.ExecuteNonQuery() != 1)
                    MessageBox.Show(Globals.generic_IT_error);
            }

            this.Close();
        }
    }
}
