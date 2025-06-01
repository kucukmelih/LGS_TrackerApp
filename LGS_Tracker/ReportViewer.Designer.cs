namespace LGS_Tracker
{
    partial class ReportViewer
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox cmbStudent;
        private System.Windows.Forms.CheckedListBox chkExams;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnBack;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cmbStudent = new System.Windows.Forms.ComboBox();
            this.chkExams = new System.Windows.Forms.CheckedListBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbStudent
            // 
            this.cmbStudent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.cmbStudent.FormattingEnabled = true;
            this.cmbStudent.Location = new System.Drawing.Point(466, 112);
            this.cmbStudent.Margin = new System.Windows.Forms.Padding(4);
            this.cmbStudent.Name = "cmbStudent";
            this.cmbStudent.Size = new System.Drawing.Size(274, 39);
            this.cmbStudent.TabIndex = 0;
            this.cmbStudent.Visible = false;
            // 
            // chkExams
            // 
            this.chkExams.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.chkExams.FormattingEnabled = true;
            this.chkExams.Location = new System.Drawing.Point(328, 187);
            this.chkExams.Margin = new System.Windows.Forms.Padding(4);
            this.chkExams.Name = "chkExams";
            this.chkExams.Size = new System.Drawing.Size(548, 446);
            this.chkExams.TabIndex = 1;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGenerate.Location = new System.Drawing.Point(328, 662);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(4);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(248, 75);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "Export to PDF";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnBack
            // 
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnBack.Location = new System.Drawing.Point(630, 662);
            this.btnBack.Margin = new System.Windows.Forms.Padding(4);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(248, 75);
            this.btnBack.TabIndex = 3;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // ReportViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 836);
            this.Controls.Add(this.cmbStudent);
            this.Controls.Add(this.chkExams);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnBack);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ReportViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generate Exam Report";
            this.Load += new System.EventHandler(this.ReportViewerForm_Load);
            this.ResumeLayout(false);

        }
    }
}
