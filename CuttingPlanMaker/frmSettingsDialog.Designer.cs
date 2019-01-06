namespace CuttingPlanMaker
{
    partial class frmSettingsDialog
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSawBladeKerf = new System.Windows.Forms.TextBox();
            this.cbDrawUnused = new System.Windows.Forms.CheckBox();
            this.ddlOrientation = new System.Windows.Forms.ComboBox();
            this.cbAutoRecalc = new System.Windows.Forms.CheckBox();
            this.tbPartPaddingWidth = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbPartPaddingLength = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpTargetDate = new System.Windows.Forms.DateTimePicker();
            this.tbClientAddr = new System.Windows.Forms.TextBox();
            this.tbClientNr = new System.Windows.Forms.TextBox();
            this.tbClientName = new System.Windows.Forms.TextBox();
            this.tbJobId = new System.Windows.Forms.TextBox();
            this.tbProjectName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbIncludePaddingOnReports = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(431, 264);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(512, 264);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Saw kerf:";
            // 
            // tbSawBladeKerf
            // 
            this.tbSawBladeKerf.Location = new System.Drawing.Point(127, 19);
            this.tbSawBladeKerf.Name = "tbSawBladeKerf";
            this.tbSawBladeKerf.Size = new System.Drawing.Size(39, 20);
            this.tbSawBladeKerf.TabIndex = 2;
            this.tbSawBladeKerf.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbDrawUnused
            // 
            this.cbDrawUnused.AutoSize = true;
            this.cbDrawUnused.Location = new System.Drawing.Point(264, 34);
            this.cbDrawUnused.Name = "cbDrawUnused";
            this.cbDrawUnused.Size = new System.Drawing.Size(118, 17);
            this.cbDrawUnused.TabIndex = 14;
            this.cbDrawUnused.Text = "Draw unused stock";
            this.cbDrawUnused.UseVisualStyleBackColor = true;
            // 
            // ddlOrientation
            // 
            this.ddlOrientation.FormattingEnabled = true;
            this.ddlOrientation.Items.AddRange(new object[] {
            "Horizontal",
            "Vertical"});
            this.ddlOrientation.Location = new System.Drawing.Point(280, 73);
            this.ddlOrientation.Name = "ddlOrientation";
            this.ddlOrientation.Size = new System.Drawing.Size(109, 21);
            this.ddlOrientation.TabIndex = 13;
            // 
            // cbAutoRecalc
            // 
            this.cbAutoRecalc.AutoSize = true;
            this.cbAutoRecalc.Location = new System.Drawing.Point(264, 16);
            this.cbAutoRecalc.Name = "cbAutoRecalc";
            this.cbAutoRecalc.Size = new System.Drawing.Size(95, 17);
            this.cbAutoRecalc.TabIndex = 10;
            this.cbAutoRecalc.Text = "Auto - Repack";
            this.cbAutoRecalc.UseVisualStyleBackColor = true;
            // 
            // tbPartPaddingWidth
            // 
            this.tbPartPaddingWidth.Location = new System.Drawing.Point(127, 73);
            this.tbPartPaddingWidth.Name = "tbPartPaddingWidth";
            this.tbPartPaddingWidth.Size = new System.Drawing.Size(39, 20);
            this.tbPartPaddingWidth.TabIndex = 9;
            this.tbPartPaddingWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(213, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Orientation:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Part padding (width):";
            // 
            // tbPartPaddingLength
            // 
            this.tbPartPaddingLength.Location = new System.Drawing.Point(127, 45);
            this.tbPartPaddingLength.Name = "tbPartPaddingLength";
            this.tbPartPaddingLength.Size = new System.Drawing.Size(39, 20);
            this.tbPartPaddingLength.TabIndex = 8;
            this.tbPartPaddingLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Part padding (length):";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpTargetDate);
            this.groupBox1.Controls.Add(this.tbClientAddr);
            this.groupBox1.Controls.Add(this.tbClientNr);
            this.groupBox1.Controls.Add(this.tbClientName);
            this.groupBox1.Controls.Add(this.tbJobId);
            this.groupBox1.Controls.Add(this.tbProjectName);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(575, 166);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Project details";
            // 
            // dtpTargetDate
            // 
            this.dtpTargetDate.Location = new System.Drawing.Point(334, 43);
            this.dtpTargetDate.Name = "dtpTargetDate";
            this.dtpTargetDate.Size = new System.Drawing.Size(200, 20);
            this.dtpTargetDate.TabIndex = 11;
            // 
            // tbClientAddr
            // 
            this.tbClientAddr.Location = new System.Drawing.Point(87, 127);
            this.tbClientAddr.Name = "tbClientAddr";
            this.tbClientAddr.Size = new System.Drawing.Size(341, 20);
            this.tbClientAddr.TabIndex = 10;
            // 
            // tbClientNr
            // 
            this.tbClientNr.Location = new System.Drawing.Point(87, 104);
            this.tbClientNr.Name = "tbClientNr";
            this.tbClientNr.Size = new System.Drawing.Size(149, 20);
            this.tbClientNr.TabIndex = 9;
            // 
            // tbClientName
            // 
            this.tbClientName.Location = new System.Drawing.Point(87, 82);
            this.tbClientName.Name = "tbClientName";
            this.tbClientName.Size = new System.Drawing.Size(241, 20);
            this.tbClientName.TabIndex = 8;
            // 
            // tbJobId
            // 
            this.tbJobId.Location = new System.Drawing.Point(87, 43);
            this.tbJobId.Name = "tbJobId";
            this.tbJobId.Size = new System.Drawing.Size(115, 20);
            this.tbJobId.TabIndex = 7;
            // 
            // tbProjectName
            // 
            this.tbProjectName.Location = new System.Drawing.Point(87, 20);
            this.tbProjectName.Name = "tbProjectName";
            this.tbProjectName.Size = new System.Drawing.Size(341, 20);
            this.tbProjectName.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(261, 46);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "Target Date:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 130);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Client Address:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 107);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Client Tel nr.:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 85);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Client Name:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(40, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Job ID:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Project Name:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbIncludePaddingOnReports);
            this.groupBox2.Controls.Add(this.tbSawBladeKerf);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cbDrawUnused);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.ddlOrientation);
            this.groupBox2.Controls.Add(this.tbPartPaddingLength);
            this.groupBox2.Controls.Add(this.cbAutoRecalc);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tbPartPaddingWidth);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 184);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(401, 107);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Processing settings";
            // 
            // cbIncludePaddingOnReports
            // 
            this.cbIncludePaddingOnReports.AccessibleDescription = "";
            this.cbIncludePaddingOnReports.AutoSize = true;
            this.cbIncludePaddingOnReports.Location = new System.Drawing.Point(218, 52);
            this.cbIncludePaddingOnReports.Name = "cbIncludePaddingOnReports";
            this.cbIncludePaddingOnReports.Size = new System.Drawing.Size(158, 17);
            this.cbIncludePaddingOnReports.TabIndex = 15;
            this.cbIncludePaddingOnReports.Text = "Include Padding on Reports";
            this.cbIncludePaddingOnReports.UseVisualStyleBackColor = true;
            // 
            // frmSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 299);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmSettingsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuration settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSawBladeKerf;
        private System.Windows.Forms.CheckBox cbDrawUnused;
        private System.Windows.Forms.ComboBox ddlOrientation;
        private System.Windows.Forms.CheckBox cbAutoRecalc;
        private System.Windows.Forms.TextBox tbPartPaddingWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbPartPaddingLength;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpTargetDate;
        private System.Windows.Forms.TextBox tbClientAddr;
        private System.Windows.Forms.TextBox tbClientNr;
        private System.Windows.Forms.TextBox tbClientName;
        private System.Windows.Forms.TextBox tbJobId;
        private System.Windows.Forms.TextBox tbProjectName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbIncludePaddingOnReports;
    }
}