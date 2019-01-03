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
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(201, 204);
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
            this.btnCancel.Location = new System.Drawing.Point(282, 204);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Saw kerf:";
            // 
            // tbSawBladeKerf
            // 
            this.tbSawBladeKerf.Location = new System.Drawing.Point(123, 11);
            this.tbSawBladeKerf.Name = "tbSawBladeKerf";
            this.tbSawBladeKerf.Size = new System.Drawing.Size(39, 20);
            this.tbSawBladeKerf.TabIndex = 2;
            this.tbSawBladeKerf.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbDrawUnused
            // 
            this.cbDrawUnused.AutoSize = true;
            this.cbDrawUnused.Location = new System.Drawing.Point(123, 141);
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
            this.ddlOrientation.Location = new System.Drawing.Point(123, 114);
            this.ddlOrientation.Name = "ddlOrientation";
            this.ddlOrientation.Size = new System.Drawing.Size(109, 21);
            this.ddlOrientation.TabIndex = 13;
            // 
            // cbAutoRecalc
            // 
            this.cbAutoRecalc.AutoSize = true;
            this.cbAutoRecalc.Location = new System.Drawing.Point(123, 91);
            this.cbAutoRecalc.Name = "cbAutoRecalc";
            this.cbAutoRecalc.Size = new System.Drawing.Size(109, 17);
            this.cbAutoRecalc.TabIndex = 10;
            this.cbAutoRecalc.Text = "Auto - recalculate";
            this.cbAutoRecalc.UseVisualStyleBackColor = true;
            // 
            // tbPartPaddingWidth
            // 
            this.tbPartPaddingWidth.Location = new System.Drawing.Point(123, 65);
            this.tbPartPaddingWidth.Name = "tbPartPaddingWidth";
            this.tbPartPaddingWidth.Size = new System.Drawing.Size(39, 20);
            this.tbPartPaddingWidth.TabIndex = 9;
            this.tbPartPaddingWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(56, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Orientation:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Part padding (width):";
            // 
            // tbPartPaddingLength
            // 
            this.tbPartPaddingLength.Location = new System.Drawing.Point(123, 37);
            this.tbPartPaddingLength.Name = "tbPartPaddingLength";
            this.tbPartPaddingLength.Size = new System.Drawing.Size(39, 20);
            this.tbPartPaddingLength.TabIndex = 8;
            this.tbPartPaddingLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Part padding (length):";
            // 
            // frmSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 239);
            this.Controls.Add(this.cbDrawUnused);
            this.Controls.Add(this.ddlOrientation);
            this.Controls.Add(this.cbAutoRecalc);
            this.Controls.Add(this.tbPartPaddingWidth);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbPartPaddingLength);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbSawBladeKerf);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmSettingsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuration settings";
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}