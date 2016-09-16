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
    public partial class ContractReviewCheckList_QE : Form
    {
        string jobNo;
        string partNo;
        int workflow_ID;
        public bool been_updated { get; set; }
        DateTime startedOn;
        bool closedFromButton;
        bool readOnly;
        bool hold;
        string startingStatus;
        bool firstOpen;
        bool canComplete;
        bool modified;

        public ContractReviewCheckList_QE(string jobNo, string partNo, int workflow_ID, bool readOnly, bool hold, string startingStatus, bool canComplete)
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
            this.canComplete = canComplete;
            this.modified = false;
        }

        //Button Events that open and close a textbox
        #region buttonClicks
        private void button1_Click(object sender, EventArgs e)
        {
            question1TextBox.Visible = !question1TextBox.Visible;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            question2TextBox.Visible = !question2TextBox.Visible;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            question3TextBox.Visible = !question3TextBox.Visible;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            question4TextBox.Visible = !question4TextBox.Visible;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            question5TextBox.Visible = !question5TextBox.Visible;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            question6TextBox.Visible = !question6TextBox.Visible;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            question7TextBox.Visible = !question7TextBox.Visible;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            question8TextBox.Visible = !question8TextBox.Visible;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            question9TextBox.Visible = !question9TextBox.Visible;

        }

        private void button10_Click(object sender, EventArgs e)
        {
            question10TextBox.Visible = !question10TextBox.Visible;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            question11TextBox.Visible = !question11TextBox.Visible;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            question12TextBox.Visible = !question12TextBox.Visible;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            question13TextBox.Visible = !question13TextBox.Visible;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            question14TextBox.Visible = !question14TextBox.Visible;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            question15TextBox.Visible = !question15TextBox.Visible;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            question16TextBox.Visible = !question16TextBox.Visible;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            question17TextBox.Visible = !question17TextBox.Visible;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            question18TextBox.Visible = !question18TextBox.Visible;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            question19TextBox.Visible = !question19TextBox.Visible;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            question20TextBox.Visible = !question20TextBox.Visible;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            question21TextBox.Visible = !question21TextBox.Visible;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            question22TextBox.Visible = !question22TextBox.Visible;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            question23TextBox.Visible = !question23TextBox.Visible;
        }

        private void button24_Click(object sender, EventArgs e)
        {
            question24TextBox.Visible = !question24TextBox.Visible;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            question25TextBox.Visible = !question25TextBox.Visible;
        }

        private void button26_Click(object sender, EventArgs e)
        {
            question26TextBox.Visible = !question26TextBox.Visible;
        }

        private void button27_Click(object sender, EventArgs e)
        {
            question27TextBox.Visible = !question27TextBox.Visible;
        }

        private void button28_Click(object sender, EventArgs e)
        {
            question28TextBox.Visible = !question28TextBox.Visible;
        }

        private void button29_Click(object sender, EventArgs e)
        {
            question29TextBox.Visible = !question29TextBox.Visible;
        }

        private void button30_Click(object sender, EventArgs e)
        {
            question30TextBox.Visible = !question30TextBox.Visible;
        }

        private void button31_Click(object sender, EventArgs e)
        {
            question31TextBox.Visible = !question31TextBox.Visible;
        }

        private void button32_Click(object sender, EventArgs e)
        {
            question32TextBox.Visible = !question32TextBox.Visible;
        }

        private void button33_Click(object sender, EventArgs e)
        {
            question33TextBox.Visible = !question33TextBox.Visible;
        }

        private void button34_Click(object sender, EventArgs e)
        {
            question34TextBox.Visible = !question34TextBox.Visible;
        }

        private void button35_Click(object sender, EventArgs e)
        {
            question35TextBox.Visible = !question35TextBox.Visible;
        }

        private void button36_Click(object sender, EventArgs e)
        {
            question36TextBox.Visible = !question36TextBox.Visible;
        }

        private void button37_Click(object sender, EventArgs e)
        {
            question37TextBox.Visible = !question37TextBox.Visible;
        }

        private void button38_Click(object sender, EventArgs e)
        {
            question38TextBox.Visible = !question38TextBox.Visible;

        }

        private void button39_Click(object sender, EventArgs e)
        {
            question39TextBox.Visible = !question39TextBox.Visible;

        }
        #endregion

        //Make all rows 0 to hide rows
        private void ContractReviewCheckList_QE_Load(object sender, EventArgs e)
        {
            for (int i = 1; i < 79; i += 2)
            {
                tableLayoutPanel1.RowStyles[i].Height = 0;
            }

            // initialize some textboxes
            jobNoTextBox.Text = jobNo;
            jobNoTextBox.Enabled = false;
            partNoTextBox.Text = partNo;
            partNoTextBox.Enabled = false;
            statusLabel.Text = "Status: " + startingStatus;

            // Set Up Combo Boxes
            SetUpComboBoxes(this);

            // check if it exists in DB and load data
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();

                string query =
                    "SELECT\n" +
                    "'Filler'\n" +
                    ",'Filler'\n" +
                    ",[Initial]\n" +
                    ",[PO_Date]\n" +
                    ",[Planning]\n" +
                    ",[Question1_Response]\n" +
                    ",[Question2_Response]\n" +
                    ",[Question3_Response]\n" +
                    ",[Question4_Response]\n" +
                    ",[Question5_Response]\n" +
                    ",[Question6_Response]\n" +
                    ",[Question7_Response]\n" +
                    ",[Question8_Response]\n" +
                    ",[Question9_Response]\n" +
                    ",[Question10_Response]\n" +
                    ",[Question11_Response]\n" +
                    ",[Question12_Response]\n" +
                    ",[Question13_Response]\n" +
                    ",[Question14_Response]\n" +
                    ",[Question15_Response]\n" +
                    ",[Question16_Response]\n" +
                    ",[Question17_Response]\n" +
                    ",[Question18_Response]\n" +
                    ",[Question19_Response]\n" +
                    ",[Question20_Response]\n" +
                    ",[Question21_Response]\n" +
                    ",[Question22_Response]\n" +
                    ",[Question23_Response]\n" +
                    ",[Question24_Response]\n" +
                    ",[Question25_Response]\n" +
                    ",[Question26_Response]\n" +
                    ",[Question27_Response]\n" +
                    ",[Question28_Response]\n" +
                    ",[Question29_Response]\n" +
                    ",[Question30_Response]\n" +
                    ",[Question31_Response]\n" +
                    ",[Question32_Response]\n" +
                    ",[Question33_Response]\n" +
                    ",[Question34_Response]\n" +
                    ",[Question35_Response]\n" +
                    ",[Question36_Response]\n" +
                    ",[Question37_Response]\n" +
                    ",[Question38_Response]\n" +
                    ",[Question39_Response]\n" +
                    ",[Question1_Comments]\n" +
                    ",[Question2_Comments]\n" +
                    ",[Question3_Comments]\n" +
                    ",[Question4_Comments]\n" +
                    ",[Question5_Comments]\n" +
                    ",[Question6_Comments]\n" +
                    ",[Question7_Comments]\n" +
                    ",[Question8_Comments]\n" +
                    ",[Question9_Comments]\n" +
                    ",[Question10_Comments]\n" +
                    ",[Question11_Comments]\n" +
                    ",[Question12_Comments]\n" +
                    ",[Question13_Comments]\n" +
                    ",[Question14_Comments]\n" +
                    ",[Question15_Comments]\n" +
                    ",[Question16_Comments]\n" +
                    ",[Question17_Comments]\n" +
                    ",[Question18_Comments]\n" +
                    ",[Question19_Comments]\n" +
                    ",[Question20_Comments]\n" +
                    ",[Question21_Comments]\n" +
                    ",[Question22_Comments]\n" +
                    ",[Question23_Comments]\n" +
                    ",[Question24_Comments]\n" +
                    ",[Question25_Comments]\n" +
                    ",[Question26_Comments]\n" +
                    ",[Question27_Comments]\n" +
                    ",[Question28_Comments]\n" +
                    ",[Question29_Comments]\n" +
                    ",[Question30_Comments]\n" +
                    ",[Question31_Comments]\n" +
                    ",[Question32_Comments]\n" +
                    ",[Question33_Comments]\n" +
                    ",[Question34_Comments]\n" +
                    ",[Question35_Comments]\n" +
                    ",[Question36_Comments]\n" +
                    ",[Question37_Comments]\n" +
                    ",[Question38_Comments]\n" +
                    ",[Question39_Comments]\n" +
                    ",[Status]\n" +
                    ",[Status_Subtype]\n" +
                    ",[Status_Notes]\n" +
                    ",[StartedOn]\n" +
                    "FROM[ATI_Workflow].[dbo].[ContractReview_ME_QE]\n" +
                    "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "' AND ContractReview_Type = 'QE';";

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
                    question7ComboBox.SelectedIndex = question7ComboBox.FindString(reader.IsDBNull(11) ? string.Empty : reader.GetString(11));
                    question8ComboBox.SelectedIndex = question8ComboBox.FindString(reader.IsDBNull(12) ? string.Empty : reader.GetString(12));
                    question9ComboBox.SelectedIndex = question9ComboBox.FindString(reader.IsDBNull(13) ? string.Empty : reader.GetString(13));
                    question10ComboBox.SelectedIndex = question10ComboBox.FindString(reader.IsDBNull(14) ? string.Empty : reader.GetString(14));
                    question11ComboBox.SelectedIndex = question11ComboBox.FindString(reader.IsDBNull(15) ? string.Empty : reader.GetString(15));
                    question12ComboBox.SelectedIndex = question12ComboBox.FindString(reader.IsDBNull(16) ? string.Empty : reader.GetString(16));
                    question13ComboBox.SelectedIndex = question13ComboBox.FindString(reader.IsDBNull(17) ? string.Empty : reader.GetString(17));
                    question14ComboBox.SelectedIndex = question14ComboBox.FindString(reader.IsDBNull(18) ? string.Empty : reader.GetString(18));
                    question15ComboBox.SelectedIndex = question15ComboBox.FindString(reader.IsDBNull(19) ? string.Empty : reader.GetString(19));
                    question16ComboBox.SelectedIndex = question16ComboBox.FindString(reader.IsDBNull(20) ? string.Empty : reader.GetString(20));
                    question17ComboBox.SelectedIndex = question17ComboBox.FindString(reader.IsDBNull(21) ? string.Empty : reader.GetString(21));
                    question18ComboBox.SelectedIndex = question18ComboBox.FindString(reader.IsDBNull(22) ? string.Empty : reader.GetString(22));
                    question19ComboBox.SelectedIndex = question19ComboBox.FindString(reader.IsDBNull(23) ? string.Empty : reader.GetString(23));
                    question20ComboBox.SelectedIndex = question20ComboBox.FindString(reader.IsDBNull(24) ? string.Empty : reader.GetString(24));
                    question21ComboBox.SelectedIndex = question21ComboBox.FindString(reader.IsDBNull(25) ? string.Empty : reader.GetString(25));
                    question22ComboBox.SelectedIndex = question22ComboBox.FindString(reader.IsDBNull(26) ? string.Empty : reader.GetString(26));
                    question23ComboBox.SelectedIndex = question23ComboBox.FindString(reader.IsDBNull(27) ? string.Empty : reader.GetString(27));
                    question24ComboBox.SelectedIndex = question24ComboBox.FindString(reader.IsDBNull(28) ? string.Empty : reader.GetString(28));
                    question25ComboBox.SelectedIndex = question25ComboBox.FindString(reader.IsDBNull(29) ? string.Empty : reader.GetString(29));
                    question26ComboBox.SelectedIndex = question26ComboBox.FindString(reader.IsDBNull(30) ? string.Empty : reader.GetString(30));
                    question27ComboBox.SelectedIndex = question27ComboBox.FindString(reader.IsDBNull(31) ? string.Empty : reader.GetString(31));
                    question28ComboBox.SelectedIndex = question28ComboBox.FindString(reader.IsDBNull(32) ? string.Empty : reader.GetString(32));
                    question29ComboBox.SelectedIndex = question29ComboBox.FindString(reader.IsDBNull(33) ? string.Empty : reader.GetString(33));
                    question30ComboBox.SelectedIndex = question30ComboBox.FindString(reader.IsDBNull(34) ? string.Empty : reader.GetString(34));
                    question31ComboBox.SelectedIndex = question31ComboBox.FindString(reader.IsDBNull(35) ? string.Empty : reader.GetString(35));
                    question32ComboBox.SelectedIndex = question32ComboBox.FindString(reader.IsDBNull(36) ? string.Empty : reader.GetString(36));
                    question33ComboBox.SelectedIndex = question33ComboBox.FindString(reader.IsDBNull(37) ? string.Empty : reader.GetString(37));
                    question34ComboBox.SelectedIndex = question34ComboBox.FindString(reader.IsDBNull(38) ? string.Empty : reader.GetString(38));
                    question35ComboBox.SelectedIndex = question35ComboBox.FindString(reader.IsDBNull(39) ? string.Empty : reader.GetString(39));
                    question36ComboBox.SelectedIndex = question36ComboBox.FindString(reader.IsDBNull(40) ? string.Empty : reader.GetString(40));
                    question37ComboBox.SelectedIndex = question37ComboBox.FindString(reader.IsDBNull(41) ? string.Empty : reader.GetString(41));
                    question38ComboBox.SelectedIndex = question38ComboBox.FindString(reader.IsDBNull(42) ? string.Empty : reader.GetString(42));
                    question39ComboBox.SelectedIndex = question39ComboBox.FindString(reader.IsDBNull(43) ? string.Empty : reader.GetString(43));

                    question1TextBox.Text = reader.IsDBNull(44) ? "" : reader.GetString(44);
                    question2TextBox.Text = reader.IsDBNull(45) ? "" : reader.GetString(45);
                    question3TextBox.Text = reader.IsDBNull(46) ? "" : reader.GetString(46);
                    question4TextBox.Text = reader.IsDBNull(47) ? "" : reader.GetString(47);
                    question5TextBox.Text = reader.IsDBNull(48) ? "" : reader.GetString(48);
                    question6TextBox.Text = reader.IsDBNull(49) ? "" : reader.GetString(49);
                    question7TextBox.Text = reader.IsDBNull(50) ? "" : reader.GetString(50);
                    question8TextBox.Text = reader.IsDBNull(51) ? "" : reader.GetString(51);
                    question9TextBox.Text = reader.IsDBNull(52) ? "" : reader.GetString(52);
                    question10TextBox.Text = reader.IsDBNull(53) ? "" : reader.GetString(53);
                    question11TextBox.Text = reader.IsDBNull(54) ? "" : reader.GetString(54);
                    question12TextBox.Text = reader.IsDBNull(55) ? "" : reader.GetString(55);
                    question13TextBox.Text = reader.IsDBNull(56) ? "" : reader.GetString(56);
                    question14TextBox.Text = reader.IsDBNull(57) ? "" : reader.GetString(57);
                    question15TextBox.Text = reader.IsDBNull(58) ? "" : reader.GetString(58);
                    question16TextBox.Text = reader.IsDBNull(59) ? "" : reader.GetString(59);
                    question17TextBox.Text = reader.IsDBNull(60) ? "" : reader.GetString(60);
                    question18TextBox.Text = reader.IsDBNull(61) ? "" : reader.GetString(61);
                    question19TextBox.Text = reader.IsDBNull(62) ? "" : reader.GetString(62);
                    question20TextBox.Text = reader.IsDBNull(63) ? "" : reader.GetString(63);
                    question21TextBox.Text = reader.IsDBNull(64) ? "" : reader.GetString(64);
                    question22TextBox.Text = reader.IsDBNull(65) ? "" : reader.GetString(65);
                    question23TextBox.Text = reader.IsDBNull(66) ? "" : reader.GetString(66);
                    question24TextBox.Text = reader.IsDBNull(67) ? "" : reader.GetString(67);
                    question25TextBox.Text = reader.IsDBNull(68) ? "" : reader.GetString(68);
                    question26TextBox.Text = reader.IsDBNull(69) ? "" : reader.GetString(69);
                    question27TextBox.Text = reader.IsDBNull(70) ? "" : reader.GetString(70);
                    question28TextBox.Text = reader.IsDBNull(71) ? "" : reader.GetString(71);
                    question29TextBox.Text = reader.IsDBNull(72) ? "" : reader.GetString(72);
                    question30TextBox.Text = reader.IsDBNull(73) ? "" : reader.GetString(73);
                    question31TextBox.Text = reader.IsDBNull(74) ? "" : reader.GetString(74);
                    question32TextBox.Text = reader.IsDBNull(75) ? "" : reader.GetString(75);
                    question33TextBox.Text = reader.IsDBNull(76) ? "" : reader.GetString(76);
                    question34TextBox.Text = reader.IsDBNull(77) ? "" : reader.GetString(77);
                    question35TextBox.Text = reader.IsDBNull(78) ? "" : reader.GetString(78);
                    question36TextBox.Text = reader.IsDBNull(79) ? "" : reader.GetString(79);
                    question37TextBox.Text = reader.IsDBNull(80) ? "" : reader.GetString(80);
                    question38TextBox.Text = reader.IsDBNull(81) ? "" : reader.GetString(81);
                    question39TextBox.Text = reader.IsDBNull(82) ? "" : reader.GetString(82);

                    // check about updated status subtype dropdown
                    statusLabel.Text = "Status: " + (reader.IsDBNull(83) ? string.Empty : reader.GetString(83));

                    // check if StartedOn is NULL, this will tell you if it's the first time
                    firstOpen = reader.IsDBNull(86);
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

        //textBox events that change text of Button
        //If there is text then Button will read 'View/edit'
        //If no text Button will read 'Create Note'
        #region textChangeQE
        private void textBox8_TextChanged_1(object sender, EventArgs e)
        {
            if (question1TextBox.Text == "")
            {
                button1.Text = "Create Note";

            }
            else
                button1.Text = "View/Edit";
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (question2TextBox.Text == "")
            {
                button2.Text = "Create Note";

            }
            else
                button2.Text = "View/Edit";
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (question3TextBox.Text == "")
            {
                button3.Text = "Create Note";

            }
            else
                button3.Text = "View/Edit";
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (question4TextBox.Text == "")
            {
                button4.Text = "Create Note";

            }
            else
                button4.Text = "View/Edit";
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if (question5TextBox.Text == "")
            {
                button5.Text = "Create Note";

            }
            else
                button5.Text = "View/Edit";
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            if (question6TextBox.Text == "")
            {
                button6.Text = "Create Note";

            }
            else
                button6.Text = "View/Edit";
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            if (question7TextBox.Text == "")
            {
                button7.Text = "Create Note";

            }
            else
                button7.Text = "View/Edit";
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            if (question8TextBox.Text == "")
            {
                button8.Text = "Create Note";

            }
            else
                button8.Text = "View/Edit";
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            if (question9TextBox.Text == "")
            {
                button9.Text = "Create Note";

            }
            else
                button9.Text = "View/Edit";
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            if (question10TextBox.Text == "")
            {
                button10.Text = "Create Note";

            }
            else
                button10.Text = "View/Edit";
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            if (question11TextBox.Text == "")
            {
                button11.Text = "Create Note";

            }
            else
                button11.Text = "View/Edit";
        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {
            if (question12TextBox.Text == "")
            {
                button12.Text = "Create Note";

            }
            else
                button12.Text = "View/Edit";
        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {
            if (question13TextBox.Text == "")
            {
                button13.Text = "Create Note";

            }
            else
                button13.Text = "View/Edit";
        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            if (question14TextBox.Text == "")
            {
                button14.Text = "Create Note";

            }
            else
                button14.Text = "View/Edit";
        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {
            if (question15TextBox.Text == "")
            {
                button15.Text = "Create Note";

            }
            else
                button15.Text = "View/Edit";
        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {
            if (question16TextBox.Text == "")
            {
                button16.Text = "Create Note";

            }
            else
                button16.Text = "View/Edit";
        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            if (question17TextBox.Text == "")
            {
                button17.Text = "Create Note";

            }
            else
                button17.Text = "View/Edit";
        }

        private void textBox25_TextChanged(object sender, EventArgs e)
        {
            if (question18TextBox.Text == "")
            {
                button18.Text = "Create Note";

            }
            else
                button18.Text = "View/Edit";
        }

        private void textBox26_TextChanged(object sender, EventArgs e)
        {
            if (question19TextBox.Text == "")
            {
                button19.Text = "Create Note";

            }
            else
                button19.Text = "View/Edit";
        }

        private void textBox27_TextChanged(object sender, EventArgs e)
        {
            if (question20TextBox.Text == "")
            {
                button20.Text = "Create Note";

            }
            else
                button20.Text = "View/Edit";
        }

        private void textBox28_TextChanged(object sender, EventArgs e)
        {
            if (question21TextBox.Text == "")
            {
                button21.Text = "Create Note";

            }
            else
                button21.Text = "View/Edit";
        }

        private void textBox29_TextChanged(object sender, EventArgs e)
        {
            if (question22TextBox.Text == "")
            {
                button22.Text = "Create Note";

            }
            else
                button22.Text = "View/Edit";
        }

        private void textBox30_TextChanged(object sender, EventArgs e)
        {
            if (question23TextBox.Text == "")
            {
                button23.Text = "Create Note";

            }
            else
                button23.Text = "View/Edit";
        }

        private void textBox31_TextChanged(object sender, EventArgs e)
        {
            if (question24TextBox.Text == "")
            {
                button24.Text = "Create Note";

            }
            else
                button24.Text = "View/Edit";
        }

        private void textBox32_TextChanged(object sender, EventArgs e)
        {
            if (question25TextBox.Text == "")
            {
                button25.Text = "Create Note";

            }
            else
                button25.Text = "View/Edit";
        }

        private void textBox33_TextChanged(object sender, EventArgs e)
        {
            if (question26TextBox.Text == "")
            {
                button26.Text = "Create Note";

            }
            else
                button26.Text = "View/Edit";
        }

        private void textBox34_TextChanged(object sender, EventArgs e)
        {
            if (question27TextBox.Text == "")
            {
                button27.Text = "Create Note";

            }
            else
                button27.Text = "View/Edit";
        }

        private void textBox35_TextChanged(object sender, EventArgs e)
        {
            if (question28TextBox.Text == "")
            {
                button28.Text = "Create Note";

            }
            else
                button28.Text = "View/Edit";
        }

        private void textBox36_TextChanged(object sender, EventArgs e)
        {
            if (question29TextBox.Text == "")
            {
                button29.Text = "Create Note";

            }
            else
                button29.Text = "View/Edit";
        }

        private void textBox37_TextChanged(object sender, EventArgs e)
        {
            if (question30TextBox.Text == "")
            {
                button30.Text = "Create Note";

            }
            else
                button30.Text = "View/Edit";
        }

        private void textBox38_TextChanged(object sender, EventArgs e)
        {
            if (question31TextBox.Text == "")
            {
                button31.Text = "Create Note";

            }
            else
                button31.Text = "View/Edit";
        }

        private void textBox39_TextChanged(object sender, EventArgs e)
        {
            if (question32TextBox.Text == "")
            {
                button32.Text = "Create Note";

            }
            else
                button32.Text = "View/Edit";
        }

        private void textBox40_TextChanged(object sender, EventArgs e)
        {
            if (question33TextBox.Text == "")
            {
                button33.Text = "Create Note";

            }
            else
                button33.Text = "View/Edit";
        }

        private void textBox41_TextChanged(object sender, EventArgs e)
        {
            if (question34TextBox.Text == "")
            {
                button34.Text = "Create Note";

            }
            else
                button34.Text = "View/Edit";
        }

        private void textBox42_TextChanged(object sender, EventArgs e)
        {
            if (question35TextBox.Text == "")
            {
                button35.Text = "Create Note";

            }
            else
                button35.Text = "View/Edit";
        }

        private void textBox43_TextChanged(object sender, EventArgs e)
        {
            if (question36TextBox.Text == "")
            {
                button36.Text = "Create Note";

            }
            else
                button36.Text = "View/Edit";
        }

        private void textBox44_TextChanged(object sender, EventArgs e)
        {
            if (question37TextBox.Text == "")
            {
                button37.Text = "Create Note";

            }
            else
                button37.Text = "View/Edit";
        }

        private void textBox45_TextChanged(object sender, EventArgs e)
        {
            if (question38TextBox.Text == "")
            {
                button38.Text = "Create Note";

            }
            else
                button38.Text = "View/Edit";
        }

        private void textBox46_TextChanged(object sender, EventArgs e)
        {
            if (question39TextBox.Text == "")
            {
                button39.Text = "Create Note";

            }
            else
                button39.Text = "View/Edit";
        }
        #endregion

        private void submitButton_Click(object sender, EventArgs e)
        {
            using (OdbcConnection conn = new OdbcConnection(Globals.odbc_connection_string))
            {
                conn.Open();
                string query =
                    "UPDATE [ATI_Workflow].[dbo].[ContractReview_ME_QE]\n" +
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
                    ",[Question7_Response] = '" + (question7ComboBox.SelectedItem == null ? string.Empty : question7ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question8_Response] = '" + (question8ComboBox.SelectedItem == null ? string.Empty : question8ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question9_Response] = '" + (question9ComboBox.SelectedItem == null ? string.Empty : question9ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question10_Response] = '" + (question10ComboBox.SelectedItem == null ? string.Empty : question10ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question11_Response] = '" + (question11ComboBox.SelectedItem == null ? string.Empty : question11ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question12_Response] = '" + (question12ComboBox.SelectedItem == null ? string.Empty : question12ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question13_Response] = '" + (question13ComboBox.SelectedItem == null ? string.Empty : question13ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question14_Response] = '" + (question14ComboBox.SelectedItem == null ? string.Empty : question14ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question15_Response] = '" + (question15ComboBox.SelectedItem == null ? string.Empty : question15ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question16_Response] = '" + (question16ComboBox.SelectedItem == null ? string.Empty : question16ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question17_Response] = '" + (question17ComboBox.SelectedItem == null ? string.Empty : question17ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question18_Response] = '" + (question18ComboBox.SelectedItem == null ? string.Empty : question18ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question19_Response] = '" + (question19ComboBox.SelectedItem == null ? string.Empty : question19ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question20_Response] = '" + (question20ComboBox.SelectedItem == null ? string.Empty : question20ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question21_Response] = '" + (question21ComboBox.SelectedItem == null ? string.Empty : question21ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question22_Response] = '" + (question22ComboBox.SelectedItem == null ? string.Empty : question22ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question23_Response] = '" + (question23ComboBox.SelectedItem == null ? string.Empty : question23ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question24_Response] = '" + (question24ComboBox.SelectedItem == null ? string.Empty : question24ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question25_Response] = '" + (question25ComboBox.SelectedItem == null ? string.Empty : question25ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question26_Response] = '" + (question26ComboBox.SelectedItem == null ? string.Empty : question26ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question27_Response] = '" + (question27ComboBox.SelectedItem == null ? string.Empty : question27ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question28_Response] = '" + (question28ComboBox.SelectedItem == null ? string.Empty : question28ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question29_Response] = '" + (question29ComboBox.SelectedItem == null ? string.Empty : question29ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question30_Response] = '" + (question30ComboBox.SelectedItem == null ? string.Empty : question30ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question31_Response] = '" + (question31ComboBox.SelectedItem == null ? string.Empty : question31ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question32_Response] = '" + (question32ComboBox.SelectedItem == null ? string.Empty : question32ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question33_Response] = '" + (question33ComboBox.SelectedItem == null ? string.Empty : question33ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question34_Response] = '" + (question34ComboBox.SelectedItem == null ? string.Empty : question34ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question35_Response] = '" + (question35ComboBox.SelectedItem == null ? string.Empty : question35ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question36_Response] = '" + (question36ComboBox.SelectedItem == null ? string.Empty : question36ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question37_Response] = '" + (question37ComboBox.SelectedItem == null ? string.Empty : question37ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question38_Response] = '" + (question38ComboBox.SelectedItem == null ? string.Empty : question38ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question39_Response] = '" + (question39ComboBox.SelectedItem == null ? string.Empty : question39ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question1_Comments] = '" + question1TextBox.Text + "'\n" +
                    ",[Question2_Comments] = '" + question2TextBox.Text + "'\n" +
                    ",[Question3_Comments] = '" + question3TextBox.Text + "'\n" +
                    ",[Question4_Comments] = '" + question4TextBox.Text + "'\n" +
                    ",[Question5_Comments] = '" + question5TextBox.Text + "'\n" +
                    ",[Question6_Comments] = '" + question6TextBox.Text + "'\n" +
                    ",[Question7_Comments] = '" + question7TextBox.Text + "'\n" +
                    ",[Question8_Comments] = '" + question8TextBox.Text + "'\n" +
                    ",[Question9_Comments] = '" + question9TextBox.Text + "'\n" +
                    ",[Question10_Comments] = '" + question10TextBox.Text + "'\n" +
                    ",[Question11_Comments] = '" + question11TextBox.Text + "'\n" +
                    ",[Question12_Comments] = '" + question12TextBox.Text + "'\n" +
                    ",[Question13_Comments] = '" + question13TextBox.Text + "'\n" +
                    ",[Question14_Comments] = '" + question14TextBox.Text + "'\n" +
                    ",[Question15_Comments] = '" + question15TextBox.Text + "'\n" +
                    ",[Question16_Comments] = '" + question16TextBox.Text + "'\n" +
                    ",[Question17_Comments] = '" + question17TextBox.Text + "'\n" +
                    ",[Question18_Comments] = '" + question8TextBox.Text + "'\n" +
                    ",[Question19_Comments] = '" + question19TextBox.Text + "'\n" +
                    ",[Question20_Comments] = '" + question20TextBox.Text + "'\n" +
                    ",[Question21_Comments] = '" + question21TextBox.Text + "'\n" +
                    ",[Question22_Comments] = '" + question22TextBox.Text + "'\n" +
                    ",[Question23_Comments] = '" + question23TextBox.Text + "'\n" +
                    ",[Question24_Comments] = '" + question24TextBox.Text + "'\n" +
                    ",[Question25_Comments] = '" + question25TextBox.Text + "'\n" +
                    ",[Question26_Comments] = '" + question26TextBox.Text + "'\n" +
                    ",[Question27_Comments] = '" + question27TextBox.Text + "'\n" +
                    ",[Question28_Comments] = '" + question28TextBox.Text + "'\n" +
                    ",[Question29_Comments] = '" + question29TextBox.Text + "'\n" +
                    ",[Question30_Comments] = '" + question30TextBox.Text + "'\n" +
                    ",[Question31_Comments] = '" + question31TextBox.Text + "'\n" +
                    ",[Question32_Comments] = '" + question32TextBox.Text + "'\n" +
                    ",[Question33_Comments] = '" + question33TextBox.Text + "'\n" +
                    ",[Question34_Comments] = '" + question34TextBox.Text + "'\n" +
                    ",[Question35_Comments] = '" + question35TextBox.Text + "'\n" +
                    ",[Question36_Comments] = '" + question36TextBox.Text + "'\n" +
                    ",[Question37_Comments] = '" + question37TextBox.Text + "'\n" +
                    ",[Question38_Comments] = '" + question38TextBox.Text + "'\n" +
                    ",[Question39_Comments] = '" + question39TextBox.Text + "'\n" +
                    "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "' AND ContractReview_Type = 'QE';";

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
            // check if quality step haas been completed
            if (!canComplete)
            {
                MessageBox.Show("QA step has to be completed before QE checklist can be completed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    "UPDATE [ATI_Workflow].[dbo].[ContractReview_ME_QE]\n" +
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
                    ",[Question7_Response] = '" + (question7ComboBox.SelectedItem == null ? string.Empty : question7ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question8_Response] = '" + (question8ComboBox.SelectedItem == null ? string.Empty : question8ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question9_Response] = '" + (question9ComboBox.SelectedItem == null ? string.Empty : question9ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question10_Response] = '" + (question10ComboBox.SelectedItem == null ? string.Empty : question10ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question11_Response] = '" + (question11ComboBox.SelectedItem == null ? string.Empty : question11ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question12_Response] = '" + (question12ComboBox.SelectedItem == null ? string.Empty : question12ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question13_Response] = '" + (question13ComboBox.SelectedItem == null ? string.Empty : question13ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question14_Response] = '" + (question14ComboBox.SelectedItem == null ? string.Empty : question14ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question15_Response] = '" + (question15ComboBox.SelectedItem == null ? string.Empty : question15ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question16_Response] = '" + (question16ComboBox.SelectedItem == null ? string.Empty : question16ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question17_Response] = '" + (question17ComboBox.SelectedItem == null ? string.Empty : question17ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question18_Response] = '" + (question18ComboBox.SelectedItem == null ? string.Empty : question18ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question19_Response] = '" + (question19ComboBox.SelectedItem == null ? string.Empty : question19ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question20_Response] = '" + (question20ComboBox.SelectedItem == null ? string.Empty : question20ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question21_Response] = '" + (question21ComboBox.SelectedItem == null ? string.Empty : question21ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question22_Response] = '" + (question22ComboBox.SelectedItem == null ? string.Empty : question22ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question23_Response] = '" + (question23ComboBox.SelectedItem == null ? string.Empty : question23ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question24_Response] = '" + (question24ComboBox.SelectedItem == null ? string.Empty : question24ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question25_Response] = '" + (question25ComboBox.SelectedItem == null ? string.Empty : question25ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question26_Response] = '" + (question26ComboBox.SelectedItem == null ? string.Empty : question26ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question27_Response] = '" + (question27ComboBox.SelectedItem == null ? string.Empty : question27ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question28_Response] = '" + (question28ComboBox.SelectedItem == null ? string.Empty : question28ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question29_Response] = '" + (question29ComboBox.SelectedItem == null ? string.Empty : question29ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question30_Response] = '" + (question30ComboBox.SelectedItem == null ? string.Empty : question30ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question31_Response] = '" + (question31ComboBox.SelectedItem == null ? string.Empty : question31ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question32_Response] = '" + (question32ComboBox.SelectedItem == null ? string.Empty : question32ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question33_Response] = '" + (question33ComboBox.SelectedItem == null ? string.Empty : question33ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question34_Response] = '" + (question34ComboBox.SelectedItem == null ? string.Empty : question34ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question35_Response] = '" + (question35ComboBox.SelectedItem == null ? string.Empty : question35ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question36_Response] = '" + (question36ComboBox.SelectedItem == null ? string.Empty : question36ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question37_Response] = '" + (question37ComboBox.SelectedItem == null ? string.Empty : question37ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question38_Response] = '" + (question38ComboBox.SelectedItem == null ? string.Empty : question38ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question39_Response] = '" + (question39ComboBox.SelectedItem == null ? string.Empty : question39ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question1_Comments] = '" + question1TextBox.Text + "'\n" +
                    ",[Question2_Comments] = '" + question2TextBox.Text + "'\n" +
                    ",[Question3_Comments] = '" + question3TextBox.Text + "'\n" +
                    ",[Question4_Comments] = '" + question4TextBox.Text + "'\n" +
                    ",[Question5_Comments] = '" + question5TextBox.Text + "'\n" +
                    ",[Question6_Comments] = '" + question6TextBox.Text + "'\n" +
                    ",[Question7_Comments] = '" + question7TextBox.Text + "'\n" +
                    ",[Question8_Comments] = '" + question8TextBox.Text + "'\n" +
                    ",[Question9_Comments] = '" + question9TextBox.Text + "'\n" +
                    ",[Question10_Comments] = '" + question10TextBox.Text + "'\n" +
                    ",[Question11_Comments] = '" + question11TextBox.Text + "'\n" +
                    ",[Question12_Comments] = '" + question12TextBox.Text + "'\n" +
                    ",[Question13_Comments] = '" + question13TextBox.Text + "'\n" +
                    ",[Question14_Comments] = '" + question14TextBox.Text + "'\n" +
                    ",[Question15_Comments] = '" + question15TextBox.Text + "'\n" +
                    ",[Question16_Comments] = '" + question16TextBox.Text + "'\n" +
                    ",[Question17_Comments] = '" + question17TextBox.Text + "'\n" +
                    ",[Question18_Comments] = '" + question8TextBox.Text + "'\n" +
                    ",[Question19_Comments] = '" + question19TextBox.Text + "'\n" +
                    ",[Question20_Comments] = '" + question20TextBox.Text + "'\n" +
                    ",[Question21_Comments] = '" + question21TextBox.Text + "'\n" +
                    ",[Question22_Comments] = '" + question22TextBox.Text + "'\n" +
                    ",[Question23_Comments] = '" + question23TextBox.Text + "'\n" +
                    ",[Question24_Comments] = '" + question24TextBox.Text + "'\n" +
                    ",[Question25_Comments] = '" + question25TextBox.Text + "'\n" +
                    ",[Question26_Comments] = '" + question26TextBox.Text + "'\n" +
                    ",[Question27_Comments] = '" + question27TextBox.Text + "'\n" +
                    ",[Question28_Comments] = '" + question28TextBox.Text + "'\n" +
                    ",[Question29_Comments] = '" + question29TextBox.Text + "'\n" +
                    ",[Question30_Comments] = '" + question30TextBox.Text + "'\n" +
                    ",[Question31_Comments] = '" + question31TextBox.Text + "'\n" +
                    ",[Question32_Comments] = '" + question32TextBox.Text + "'\n" +
                    ",[Question33_Comments] = '" + question33TextBox.Text + "'\n" +
                    ",[Question34_Comments] = '" + question34TextBox.Text + "'\n" +
                    ",[Question35_Comments] = '" + question35TextBox.Text + "'\n" +
                    ",[Question36_Comments] = '" + question36TextBox.Text + "'\n" +
                    ",[Question37_Comments] = '" + question37TextBox.Text + "'\n" +
                    ",[Question38_Comments] = '" + question38TextBox.Text + "'\n" +
                    ",[Question39_Comments] = '" + question39TextBox.Text + "'\n" +
                    "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "' AND ContractReview_Type = 'QE';";

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

        private void ContractReviewCheckList_QE_FormClosing(object sender, FormClosingEventArgs e)
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
                    "UPDATE [ATI_Workflow].[dbo].[ContractReview_ME_QE]\n" +
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
                    ",[Question7_Response] = '" + (question7ComboBox.SelectedItem == null ? string.Empty : question7ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question8_Response] = '" + (question8ComboBox.SelectedItem == null ? string.Empty : question8ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question9_Response] = '" + (question9ComboBox.SelectedItem == null ? string.Empty : question9ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question10_Response] = '" + (question10ComboBox.SelectedItem == null ? string.Empty : question10ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question11_Response] = '" + (question11ComboBox.SelectedItem == null ? string.Empty : question11ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question12_Response] = '" + (question12ComboBox.SelectedItem == null ? string.Empty : question12ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question13_Response] = '" + (question13ComboBox.SelectedItem == null ? string.Empty : question13ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question14_Response] = '" + (question14ComboBox.SelectedItem == null ? string.Empty : question14ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question15_Response] = '" + (question15ComboBox.SelectedItem == null ? string.Empty : question15ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question16_Response] = '" + (question16ComboBox.SelectedItem == null ? string.Empty : question16ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question17_Response] = '" + (question17ComboBox.SelectedItem == null ? string.Empty : question17ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question18_Response] = '" + (question18ComboBox.SelectedItem == null ? string.Empty : question18ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question19_Response] = '" + (question19ComboBox.SelectedItem == null ? string.Empty : question19ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question20_Response] = '" + (question20ComboBox.SelectedItem == null ? string.Empty : question20ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question21_Response] = '" + (question21ComboBox.SelectedItem == null ? string.Empty : question21ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question22_Response] = '" + (question22ComboBox.SelectedItem == null ? string.Empty : question22ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question23_Response] = '" + (question23ComboBox.SelectedItem == null ? string.Empty : question23ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question24_Response] = '" + (question24ComboBox.SelectedItem == null ? string.Empty : question24ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question25_Response] = '" + (question25ComboBox.SelectedItem == null ? string.Empty : question25ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question26_Response] = '" + (question26ComboBox.SelectedItem == null ? string.Empty : question26ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question27_Response] = '" + (question27ComboBox.SelectedItem == null ? string.Empty : question27ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question28_Response] = '" + (question28ComboBox.SelectedItem == null ? string.Empty : question28ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question29_Response] = '" + (question29ComboBox.SelectedItem == null ? string.Empty : question29ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question30_Response] = '" + (question30ComboBox.SelectedItem == null ? string.Empty : question30ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question31_Response] = '" + (question31ComboBox.SelectedItem == null ? string.Empty : question31ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question32_Response] = '" + (question32ComboBox.SelectedItem == null ? string.Empty : question32ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question33_Response] = '" + (question33ComboBox.SelectedItem == null ? string.Empty : question33ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question34_Response] = '" + (question34ComboBox.SelectedItem == null ? string.Empty : question34ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question35_Response] = '" + (question35ComboBox.SelectedItem == null ? string.Empty : question35ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question36_Response] = '" + (question36ComboBox.SelectedItem == null ? string.Empty : question36ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question37_Response] = '" + (question37ComboBox.SelectedItem == null ? string.Empty : question37ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question38_Response] = '" + (question38ComboBox.SelectedItem == null ? string.Empty : question38ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question39_Response] = '" + (question39ComboBox.SelectedItem == null ? string.Empty : question39ComboBox.SelectedItem.ToString()) + "'\n" +
                    ",[Question1_Comments] = '" + question1TextBox.Text + "'\n" +
                    ",[Question2_Comments] = '" + question2TextBox.Text + "'\n" +
                    ",[Question3_Comments] = '" + question3TextBox.Text + "'\n" +
                    ",[Question4_Comments] = '" + question4TextBox.Text + "'\n" +
                    ",[Question5_Comments] = '" + question5TextBox.Text + "'\n" +
                    ",[Question6_Comments] = '" + question6TextBox.Text + "'\n" +
                    ",[Question7_Comments] = '" + question7TextBox.Text + "'\n" +
                    ",[Question8_Comments] = '" + question8TextBox.Text + "'\n" +
                    ",[Question9_Comments] = '" + question9TextBox.Text + "'\n" +
                    ",[Question10_Comments] = '" + question10TextBox.Text + "'\n" +
                    ",[Question11_Comments] = '" + question11TextBox.Text + "'\n" +
                    ",[Question12_Comments] = '" + question12TextBox.Text + "'\n" +
                    ",[Question13_Comments] = '" + question13TextBox.Text + "'\n" +
                    ",[Question14_Comments] = '" + question14TextBox.Text + "'\n" +
                    ",[Question15_Comments] = '" + question15TextBox.Text + "'\n" +
                    ",[Question16_Comments] = '" + question16TextBox.Text + "'\n" +
                    ",[Question17_Comments] = '" + question17TextBox.Text + "'\n" +
                    ",[Question18_Comments] = '" + question8TextBox.Text + "'\n" +
                    ",[Question19_Comments] = '" + question19TextBox.Text + "'\n" +
                    ",[Question20_Comments] = '" + question20TextBox.Text + "'\n" +
                    ",[Question21_Comments] = '" + question21TextBox.Text + "'\n" +
                    ",[Question22_Comments] = '" + question22TextBox.Text + "'\n" +
                    ",[Question23_Comments] = '" + question23TextBox.Text + "'\n" +
                    ",[Question24_Comments] = '" + question24TextBox.Text + "'\n" +
                    ",[Question25_Comments] = '" + question25TextBox.Text + "'\n" +
                    ",[Question26_Comments] = '" + question26TextBox.Text + "'\n" +
                    ",[Question27_Comments] = '" + question27TextBox.Text + "'\n" +
                    ",[Question28_Comments] = '" + question28TextBox.Text + "'\n" +
                    ",[Question29_Comments] = '" + question29TextBox.Text + "'\n" +
                    ",[Question30_Comments] = '" + question30TextBox.Text + "'\n" +
                    ",[Question31_Comments] = '" + question31TextBox.Text + "'\n" +
                    ",[Question32_Comments] = '" + question32TextBox.Text + "'\n" +
                    ",[Question33_Comments] = '" + question33TextBox.Text + "'\n" +
                    ",[Question34_Comments] = '" + question34TextBox.Text + "'\n" +
                    ",[Question35_Comments] = '" + question35TextBox.Text + "'\n" +
                    ",[Question36_Comments] = '" + question36TextBox.Text + "'\n" +
                    ",[Question37_Comments] = '" + question37TextBox.Text + "'\n" +
                    ",[Question38_Comments] = '" + question38TextBox.Text + "'\n" +
                    ",[Question39_Comments] = '" + question39TextBox.Text + "'\n" +
                    "WHERE Job = '" + jobNo + "' AND Workflow_ID = '" + workflow_ID + "' AND ContractReview_Type = 'QE';";

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
