using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Workflow
{
    public partial class CommentDialog : Form
    {
        public string Notes { get; set; }
        public Response_Type Response { get; set; }
        public string Hold_Type;
        public enum Response_Type { Ok, Cancel };
        public string prompt;

        public CommentDialog(string title, string prompt)
        {
            InitializeComponent();

            this.prompt = prompt.Trim();
            this.Text = title.Trim();
        }

        private void CommentDialog_Load(object sender, EventArgs e)
        {
            promptLabel.Text = prompt.Trim();
            holdComboBox.Items.Add("");
            holdComboBox.Items.Add("Engi");
            holdComboBox.Items.Add("QA");
            holdComboBox.Items.Add("Cust");
            holdComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (textBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Cannot leave Text Box empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if(holdComboBox.SelectedItem == null || holdComboBox.SelectedItem.ToString().Length == 0)
            {
                MessageBox.Show("Cannot leave Hold Type Box empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Notes = textBox.Text;
            Response = Response_Type.Ok;
            Hold_Type = holdComboBox.SelectedItem.ToString();

            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Response = Response_Type.Cancel;

            this.Close();
        }
    }
}
