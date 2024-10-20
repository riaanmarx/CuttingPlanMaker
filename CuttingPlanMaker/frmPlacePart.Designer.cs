
namespace CuttingPlanMaker
{
    partial class frmPlacePart
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbParts = new System.Windows.Forms.ComboBox();
            this.cbBoards = new System.Windows.Forms.ComboBox();
            this.tbLenOffset = new System.Windows.Forms.TextBox();
            this.tbWidOffset = new System.Windows.Forms.TextBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Part:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Board:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Length-offset:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Width-offset:";
            // 
            // cbParts
            // 
            this.cbParts.FormattingEnabled = true;
            this.cbParts.Location = new System.Drawing.Point(99, 4);
            this.cbParts.Name = "cbParts";
            this.cbParts.Size = new System.Drawing.Size(271, 21);
            this.cbParts.TabIndex = 1;
            // 
            // cbBoards
            // 
            this.cbBoards.FormattingEnabled = true;
            this.cbBoards.Location = new System.Drawing.Point(99, 31);
            this.cbBoards.Name = "cbBoards";
            this.cbBoards.Size = new System.Drawing.Size(271, 21);
            this.cbBoards.TabIndex = 1;
            // 
            // tbLenOffset
            // 
            this.tbLenOffset.Location = new System.Drawing.Point(99, 64);
            this.tbLenOffset.Name = "tbLenOffset";
            this.tbLenOffset.Size = new System.Drawing.Size(100, 20);
            this.tbLenOffset.TabIndex = 2;
            // 
            // tbWidOffset
            // 
            this.tbWidOffset.Location = new System.Drawing.Point(99, 96);
            this.tbWidOffset.Name = "tbWidOffset";
            this.tbWidOffset.Size = new System.Drawing.Size(100, 20);
            this.tbWidOffset.TabIndex = 3;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(332, 94);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // frmPlacePart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 140);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.tbWidOffset);
            this.Controls.Add(this.tbLenOffset);
            this.Controls.Add(this.cbBoards);
            this.Controls.Add(this.cbParts);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmPlacePart";
            this.Text = "frmPlacePart";
            this.Load += new System.EventHandler(this.frmPlacePart_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbParts;
        private System.Windows.Forms.ComboBox cbBoards;
        private System.Windows.Forms.TextBox tbLenOffset;
        private System.Windows.Forms.TextBox tbWidOffset;
        private System.Windows.Forms.Button btnApply;
    }
}