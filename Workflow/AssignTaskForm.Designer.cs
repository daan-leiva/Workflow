namespace Workflow
{
    partial class AssignTaskForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.step3TitleLabel = new System.Windows.Forms.Label();
            this.step4TitleLabel = new System.Windows.Forms.Label();
            this.step5TitleLabel = new System.Windows.Forms.Label();
            this.step3ComboBox = new System.Windows.Forms.ComboBox();
            this.step4ComboBox = new System.Windows.Forms.ComboBox();
            this.step5ComboBox = new System.Windows.Forms.ComboBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.submitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Task Assigner";
            // 
            // step3TitleLabel
            // 
            this.step3TitleLabel.AutoSize = true;
            this.step3TitleLabel.Location = new System.Drawing.Point(21, 80);
            this.step3TitleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.step3TitleLabel.Name = "step3TitleLabel";
            this.step3TitleLabel.Size = new System.Drawing.Size(46, 17);
            this.step3TitleLabel.TabIndex = 1;
            this.step3TitleLabel.Text = "label2";
            // 
            // step4TitleLabel
            // 
            this.step4TitleLabel.AutoSize = true;
            this.step4TitleLabel.Location = new System.Drawing.Point(21, 129);
            this.step4TitleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.step4TitleLabel.Name = "step4TitleLabel";
            this.step4TitleLabel.Size = new System.Drawing.Size(46, 17);
            this.step4TitleLabel.TabIndex = 2;
            this.step4TitleLabel.Text = "label3";
            // 
            // step5TitleLabel
            // 
            this.step5TitleLabel.AutoSize = true;
            this.step5TitleLabel.Location = new System.Drawing.Point(21, 182);
            this.step5TitleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.step5TitleLabel.Name = "step5TitleLabel";
            this.step5TitleLabel.Size = new System.Drawing.Size(46, 17);
            this.step5TitleLabel.TabIndex = 3;
            this.step5TitleLabel.Text = "label4";
            // 
            // step3ComboBox
            // 
            this.step3ComboBox.FormattingEnabled = true;
            this.step3ComboBox.Location = new System.Drawing.Point(285, 75);
            this.step3ComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.step3ComboBox.Name = "step3ComboBox";
            this.step3ComboBox.Size = new System.Drawing.Size(160, 24);
            this.step3ComboBox.TabIndex = 0;
            // 
            // step4ComboBox
            // 
            this.step4ComboBox.FormattingEnabled = true;
            this.step4ComboBox.Location = new System.Drawing.Point(285, 124);
            this.step4ComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.step4ComboBox.Name = "step4ComboBox";
            this.step4ComboBox.Size = new System.Drawing.Size(160, 24);
            this.step4ComboBox.TabIndex = 1;
            // 
            // step5ComboBox
            // 
            this.step5ComboBox.FormattingEnabled = true;
            this.step5ComboBox.Location = new System.Drawing.Point(285, 177);
            this.step5ComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.step5ComboBox.Name = "step5ComboBox";
            this.step5ComboBox.Size = new System.Drawing.Size(160, 24);
            this.step5ComboBox.TabIndex = 2;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(99, 236);
            this.closeButton.Margin = new System.Windows.Forms.Padding(4);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(100, 28);
            this.closeButton.TabIndex = 4;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(273, 236);
            this.submitButton.Margin = new System.Windows.Forms.Padding(4);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(100, 28);
            this.submitButton.TabIndex = 3;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // AssignTaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 279);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.step5ComboBox);
            this.Controls.Add(this.step4ComboBox);
            this.Controls.Add(this.step3ComboBox);
            this.Controls.Add(this.step5TitleLabel);
            this.Controls.Add(this.step4TitleLabel);
            this.Controls.Add(this.step3TitleLabel);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AssignTaskForm";
            this.Text = "AssignTaskForm";
            this.Load += new System.EventHandler(this.AssignTaskForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label step3TitleLabel;
        private System.Windows.Forms.Label step4TitleLabel;
        private System.Windows.Forms.Label step5TitleLabel;
        private System.Windows.Forms.ComboBox step3ComboBox;
        private System.Windows.Forms.ComboBox step4ComboBox;
        private System.Windows.Forms.ComboBox step5ComboBox;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button submitButton;
    }
}