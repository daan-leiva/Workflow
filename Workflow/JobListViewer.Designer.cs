namespace Workflow
{
    partial class JobListViewer
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
            this.components = new System.ComponentModel.Container();
            this.jobTextBox = new System.Windows.Forms.TextBox();
            this.partNoTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.startDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.rowsLabel = new System.Windows.Forms.Label();
            this.endDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.createJobButton = new System.Windows.Forms.Button();
            this.tastkListButton = new System.Windows.Forms.Button();
            this.inProgressRadioButton = new System.Windows.Forms.RadioButton();
            this.completedRadioButton = new System.Windows.Forms.RadioButton();
            this.anyStatusRadioButton = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.holdStatusRadioButton = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.anyHotRadioButton = new System.Windows.Forms.RadioButton();
            this.yesHotRadioButton = new System.Windows.Forms.RadioButton();
            this.noHotRadioButton = new System.Windows.Forms.RadioButton();
            this.holdComboBox = new System.Windows.Forms.ComboBox();
            this.createJobCustomersComboBox = new System.Windows.Forms.ComboBox();
            this.registerUserButton = new System.Windows.Forms.Button();
            this.editUserButton = new System.Windows.Forms.Button();
            this.currentUserLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.permissionsLabel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.workflowRadioButton = new System.Windows.Forms.RadioButton();
            this.formsDetailRadioButton = new System.Windows.Forms.RadioButton();
            this.newHotRadioButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // jobTextBox
            // 
            this.jobTextBox.Location = new System.Drawing.Point(147, 108);
            this.jobTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.jobTextBox.Name = "jobTextBox";
            this.jobTextBox.Size = new System.Drawing.Size(271, 23);
            this.jobTextBox.TabIndex = 2;
            // 
            // partNoTextBox
            // 
            this.partNoTextBox.Location = new System.Drawing.Point(147, 81);
            this.partNoTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.partNoTextBox.Name = "partNoTextBox";
            this.partNoTextBox.Size = new System.Drawing.Size(271, 23);
            this.partNoTextBox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(100, 113);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 17);
            this.label5.TabIndex = 39;
            this.label5.Text = "Job:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(75, 86);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 38;
            this.label4.Text = "Part No:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 59);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 17);
            this.label3.TabIndex = 37;
            this.label3.Text = "Customer:";
            // 
            // startDateTimePicker
            // 
            this.startDateTimePicker.Location = new System.Drawing.Point(147, 135);
            this.startDateTimePicker.Margin = new System.Windows.Forms.Padding(4);
            this.startDateTimePicker.Name = "startDateTimePicker";
            this.startDateTimePicker.Size = new System.Drawing.Size(271, 23);
            this.startDateTimePicker.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 140);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 17);
            this.label7.TabIndex = 43;
            this.label7.Text = "Created Betweem:";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1645, 31);
            this.label1.TabIndex = 45;
            this.label1.Text = "Workflow List";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeColumns = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(15, 231);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(1615, 303);
            this.dataGridView.TabIndex = 11;
            this.dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellContentClick);
            this.dataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellDoubleClick);
            // 
            // rowsLabel
            // 
            this.rowsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rowsLabel.AutoSize = true;
            this.rowsLabel.Location = new System.Drawing.Point(1537, 542);
            this.rowsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.rowsLabel.Name = "rowsLabel";
            this.rowsLabel.Size = new System.Drawing.Size(58, 17);
            this.rowsLabel.TabIndex = 47;
            this.rowsLabel.Text = "Rows: 0";
            // 
            // endDateTimePicker
            // 
            this.endDateTimePicker.Location = new System.Drawing.Point(427, 135);
            this.endDateTimePicker.Margin = new System.Windows.Forms.Padding(4);
            this.endDateTimePicker.Name = "endDateTimePicker";
            this.endDateTimePicker.Size = new System.Drawing.Size(271, 23);
            this.endDateTimePicker.TabIndex = 4;
            // 
            // createJobButton
            // 
            this.createJobButton.Location = new System.Drawing.Point(427, 53);
            this.createJobButton.Margin = new System.Windows.Forms.Padding(4);
            this.createJobButton.Name = "createJobButton";
            this.createJobButton.Size = new System.Drawing.Size(139, 28);
            this.createJobButton.TabIndex = 5;
            this.createJobButton.Text = "Create Job";
            this.createJobButton.UseVisualStyleBackColor = true;
            this.createJobButton.Click += new System.EventHandler(this.createJobButton_Click);
            // 
            // tastkListButton
            // 
            this.tastkListButton.Location = new System.Drawing.Point(16, 191);
            this.tastkListButton.Margin = new System.Windows.Forms.Padding(4);
            this.tastkListButton.Name = "tastkListButton";
            this.tastkListButton.Size = new System.Drawing.Size(124, 28);
            this.tastkListButton.TabIndex = 6;
            this.tastkListButton.Text = "Task List";
            this.tastkListButton.UseVisualStyleBackColor = true;
            this.tastkListButton.Click += new System.EventHandler(this.tastkListButton_Click);
            // 
            // inProgressRadioButton
            // 
            this.inProgressRadioButton.AutoSize = true;
            this.inProgressRadioButton.Location = new System.Drawing.Point(16, 10);
            this.inProgressRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.inProgressRadioButton.Name = "inProgressRadioButton";
            this.inProgressRadioButton.Size = new System.Drawing.Size(98, 21);
            this.inProgressRadioButton.TabIndex = 0;
            this.inProgressRadioButton.TabStop = true;
            this.inProgressRadioButton.Text = "In Progress";
            this.inProgressRadioButton.UseVisualStyleBackColor = true;
            // 
            // completedRadioButton
            // 
            this.completedRadioButton.AutoSize = true;
            this.completedRadioButton.Location = new System.Drawing.Point(125, 10);
            this.completedRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.completedRadioButton.Name = "completedRadioButton";
            this.completedRadioButton.Size = new System.Drawing.Size(93, 21);
            this.completedRadioButton.TabIndex = 1;
            this.completedRadioButton.TabStop = true;
            this.completedRadioButton.Text = "Completed";
            this.completedRadioButton.UseVisualStyleBackColor = true;
            // 
            // anyStatusRadioButton
            // 
            this.anyStatusRadioButton.Location = new System.Drawing.Point(300, 10);
            this.anyStatusRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.anyStatusRadioButton.Name = "anyStatusRadioButton";
            this.anyStatusRadioButton.Size = new System.Drawing.Size(68, 21);
            this.anyStatusRadioButton.TabIndex = 3;
            this.anyStatusRadioButton.TabStop = true;
            this.anyStatusRadioButton.Text = "Any";
            this.anyStatusRadioButton.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.holdStatusRadioButton);
            this.panel1.Controls.Add(this.inProgressRadioButton);
            this.panel1.Controls.Add(this.anyStatusRadioButton);
            this.panel1.Controls.Add(this.completedRadioButton);
            this.panel1.Location = new System.Drawing.Point(1002, 122);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(367, 43);
            this.panel1.TabIndex = 7;
            // 
            // holdStatusRadioButton
            // 
            this.holdStatusRadioButton.AutoSize = true;
            this.holdStatusRadioButton.Location = new System.Drawing.Point(231, 10);
            this.holdStatusRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.holdStatusRadioButton.Name = "holdStatusRadioButton";
            this.holdStatusRadioButton.Size = new System.Drawing.Size(55, 21);
            this.holdStatusRadioButton.TabIndex = 2;
            this.holdStatusRadioButton.TabStop = true;
            this.holdStatusRadioButton.Text = "Hold";
            this.holdStatusRadioButton.UseVisualStyleBackColor = true;
            this.holdStatusRadioButton.CheckedChanged += new System.EventHandler(this.holdStatusRadioButton_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1013, 110);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 17);
            this.label2.TabIndex = 56;
            this.label2.Text = "Status";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1398, 110);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 17);
            this.label6.TabIndex = 58;
            this.label6.Text = "Hot?";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.newHotRadioButton);
            this.panel2.Controls.Add(this.anyHotRadioButton);
            this.panel2.Controls.Add(this.yesHotRadioButton);
            this.panel2.Controls.Add(this.noHotRadioButton);
            this.panel2.Location = new System.Drawing.Point(1384, 122);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(246, 43);
            this.panel2.TabIndex = 57;
            // 
            // anyHotRadioButton
            // 
            this.anyHotRadioButton.AutoSize = true;
            this.anyHotRadioButton.Location = new System.Drawing.Point(186, 10);
            this.anyHotRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.anyHotRadioButton.Name = "anyHotRadioButton";
            this.anyHotRadioButton.Size = new System.Drawing.Size(50, 21);
            this.anyHotRadioButton.TabIndex = 2;
            this.anyHotRadioButton.TabStop = true;
            this.anyHotRadioButton.Text = "Any";
            this.anyHotRadioButton.UseVisualStyleBackColor = true;
            // 
            // yesHotRadioButton
            // 
            this.yesHotRadioButton.AutoSize = true;
            this.yesHotRadioButton.Location = new System.Drawing.Point(9, 10);
            this.yesHotRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.yesHotRadioButton.Name = "yesHotRadioButton";
            this.yesHotRadioButton.Size = new System.Drawing.Size(50, 21);
            this.yesHotRadioButton.TabIndex = 0;
            this.yesHotRadioButton.TabStop = true;
            this.yesHotRadioButton.Text = "Yes";
            this.yesHotRadioButton.UseVisualStyleBackColor = true;
            // 
            // noHotRadioButton
            // 
            this.noHotRadioButton.AutoSize = true;
            this.noHotRadioButton.Location = new System.Drawing.Point(132, 10);
            this.noHotRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.noHotRadioButton.Name = "noHotRadioButton";
            this.noHotRadioButton.Size = new System.Drawing.Size(44, 21);
            this.noHotRadioButton.TabIndex = 1;
            this.noHotRadioButton.TabStop = true;
            this.noHotRadioButton.Text = "No";
            this.noHotRadioButton.UseVisualStyleBackColor = true;
            // 
            // holdComboBox
            // 
            this.holdComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.holdComboBox.FormattingEnabled = true;
            this.holdComboBox.Location = new System.Drawing.Point(1268, 169);
            this.holdComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.holdComboBox.Name = "holdComboBox";
            this.holdComboBox.Size = new System.Drawing.Size(71, 24);
            this.holdComboBox.TabIndex = 8;
            // 
            // createJobCustomersComboBox
            // 
            this.createJobCustomersComboBox.FormattingEnabled = true;
            this.createJobCustomersComboBox.Location = new System.Drawing.Point(147, 54);
            this.createJobCustomersComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.createJobCustomersComboBox.Name = "createJobCustomersComboBox";
            this.createJobCustomersComboBox.Size = new System.Drawing.Size(271, 24);
            this.createJobCustomersComboBox.TabIndex = 0;
            // 
            // registerUserButton
            // 
            this.registerUserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.registerUserButton.Location = new System.Drawing.Point(1365, 69);
            this.registerUserButton.Margin = new System.Windows.Forms.Padding(4);
            this.registerUserButton.Name = "registerUserButton";
            this.registerUserButton.Size = new System.Drawing.Size(127, 28);
            this.registerUserButton.TabIndex = 61;
            this.registerUserButton.Text = "Register User";
            this.registerUserButton.UseVisualStyleBackColor = true;
            this.registerUserButton.Click += new System.EventHandler(this.registerUserButton_Click);
            // 
            // editUserButton
            // 
            this.editUserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editUserButton.Location = new System.Drawing.Point(1500, 69);
            this.editUserButton.Margin = new System.Windows.Forms.Padding(4);
            this.editUserButton.Name = "editUserButton";
            this.editUserButton.Size = new System.Drawing.Size(127, 28);
            this.editUserButton.TabIndex = 62;
            this.editUserButton.Text = "Edit User";
            this.editUserButton.UseVisualStyleBackColor = true;
            this.editUserButton.Click += new System.EventHandler(this.editUserButton_Click);
            // 
            // currentUserLabel
            // 
            this.currentUserLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.currentUserLabel.AutoSize = true;
            this.currentUserLabel.Location = new System.Drawing.Point(1427, 25);
            this.currentUserLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.currentUserLabel.Name = "currentUserLabel";
            this.currentUserLabel.Size = new System.Drawing.Size(74, 17);
            this.currentUserLabel.TabIndex = 64;
            this.currentUserLabel.Text = "fLastname";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1335, 25);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 17);
            this.label8.TabIndex = 63;
            this.label8.Text = "Current User:";
            // 
            // permissionsLabel
            // 
            this.permissionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.permissionsLabel.AutoSize = true;
            this.permissionsLabel.Location = new System.Drawing.Point(1425, 42);
            this.permissionsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.permissionsLabel.Name = "permissionsLabel";
            this.permissionsLabel.Size = new System.Drawing.Size(74, 17);
            this.permissionsLabel.TabIndex = 68;
            this.permissionsLabel.Text = "fLastname";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1339, 42);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 17);
            this.label9.TabIndex = 67;
            this.label9.Text = "Permissions:";
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // workflowRadioButton
            // 
            this.workflowRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.workflowRadioButton.AutoSize = true;
            this.workflowRadioButton.Location = new System.Drawing.Point(1383, 203);
            this.workflowRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.workflowRadioButton.Name = "workflowRadioButton";
            this.workflowRadioButton.Size = new System.Drawing.Size(123, 21);
            this.workflowRadioButton.TabIndex = 9;
            this.workflowRadioButton.TabStop = true;
            this.workflowRadioButton.Text = "Workflow Detail";
            this.workflowRadioButton.UseVisualStyleBackColor = true;
            // 
            // formsDetailRadioButton
            // 
            this.formsDetailRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.formsDetailRadioButton.AutoSize = true;
            this.formsDetailRadioButton.Location = new System.Drawing.Point(1529, 203);
            this.formsDetailRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.formsDetailRadioButton.Name = "formsDetailRadioButton";
            this.formsDetailRadioButton.Size = new System.Drawing.Size(98, 21);
            this.formsDetailRadioButton.TabIndex = 10;
            this.formsDetailRadioButton.TabStop = true;
            this.formsDetailRadioButton.Text = "Form Detail";
            this.formsDetailRadioButton.UseVisualStyleBackColor = true;
            // 
            // newHotRadioButton
            // 
            this.newHotRadioButton.AutoSize = true;
            this.newHotRadioButton.Location = new System.Drawing.Point(69, 10);
            this.newHotRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.newHotRadioButton.Name = "newHotRadioButton";
            this.newHotRadioButton.Size = new System.Drawing.Size(53, 21);
            this.newHotRadioButton.TabIndex = 3;
            this.newHotRadioButton.TabStop = true;
            this.newHotRadioButton.Text = "New";
            this.newHotRadioButton.UseVisualStyleBackColor = true;
            // 
            // JobListViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1645, 569);
            this.Controls.Add(this.formsDetailRadioButton);
            this.Controls.Add(this.workflowRadioButton);
            this.Controls.Add(this.permissionsLabel);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.currentUserLabel);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.editUserButton);
            this.Controls.Add(this.registerUserButton);
            this.Controls.Add(this.createJobCustomersComboBox);
            this.Controls.Add(this.holdComboBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tastkListButton);
            this.Controls.Add(this.createJobButton);
            this.Controls.Add(this.endDateTimePicker);
            this.Controls.Add(this.rowsLabel);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.startDateTimePicker);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.jobTextBox);
            this.Controls.Add(this.partNoTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1661, 607);
            this.Name = "JobListViewer";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "JobListViewer";
            this.Load += new System.EventHandler(this.JobListViewer_Load);
            this.VisibleChanged += new System.EventHandler(this.JobListViewer_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox jobTextBox;
        private System.Windows.Forms.TextBox partNoTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker startDateTimePicker;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label rowsLabel;
        private System.Windows.Forms.DateTimePicker endDateTimePicker;
        private System.Windows.Forms.Button createJobButton;
        private System.Windows.Forms.Button tastkListButton;
        private System.Windows.Forms.RadioButton inProgressRadioButton;
        private System.Windows.Forms.RadioButton completedRadioButton;
        private System.Windows.Forms.RadioButton anyStatusRadioButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton yesHotRadioButton;
        private System.Windows.Forms.RadioButton noHotRadioButton;
        private System.Windows.Forms.RadioButton anyHotRadioButton;
        private System.Windows.Forms.RadioButton holdStatusRadioButton;
        private System.Windows.Forms.ComboBox holdComboBox;
        private System.Windows.Forms.ComboBox createJobCustomersComboBox;
        private System.Windows.Forms.Button registerUserButton;
        private System.Windows.Forms.Button editUserButton;
        private System.Windows.Forms.Label currentUserLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label permissionsLabel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.RadioButton workflowRadioButton;
        private System.Windows.Forms.RadioButton formsDetailRadioButton;
        private System.Windows.Forms.RadioButton newHotRadioButton;
    }
}