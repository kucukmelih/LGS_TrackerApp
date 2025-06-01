namespace LGS_Tracker
{
    partial class AdminForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnStudentList = new System.Windows.Forms.Button();
            this.btnAddExam = new System.Windows.Forms.Button();
            this.btnViewAllResults = new System.Windows.Forms.Button();
            this.btnOCRPDF = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnReport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(342, 190);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(489, 51);
            this.lblTitle.TabIndex = 8;
            this.lblTitle.Text = "Admin Panel - Home Page";
            // 
            // btnStudentList
            // 
            this.btnStudentList.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnStudentList.Location = new System.Drawing.Point(408, 266);
            this.btnStudentList.Name = "btnStudentList";
            this.btnStudentList.Size = new System.Drawing.Size(352, 58);
            this.btnStudentList.TabIndex = 9;
            this.btnStudentList.Text = "Student List";
            // 
            // btnAddExam
            // 
            this.btnAddExam.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddExam.Location = new System.Drawing.Point(408, 346);
            this.btnAddExam.Name = "btnAddExam";
            this.btnAddExam.Size = new System.Drawing.Size(352, 58);
            this.btnAddExam.TabIndex = 10;
            this.btnAddExam.Text = "Add Exam to Student";
            // 
            // btnViewAllResults
            // 
            this.btnViewAllResults.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnViewAllResults.Location = new System.Drawing.Point(408, 426);
            this.btnViewAllResults.Name = "btnViewAllResults";
            this.btnViewAllResults.Size = new System.Drawing.Size(352, 58);
            this.btnViewAllResults.TabIndex = 11;
            this.btnViewAllResults.Text = "View All Exam Results";
            // 
            // btnOCRPDF
            // 
            this.btnOCRPDF.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnOCRPDF.Location = new System.Drawing.Point(408, 584);
            this.btnOCRPDF.Name = "btnOCRPDF";
            this.btnOCRPDF.Size = new System.Drawing.Size(352, 58);
            this.btnOCRPDF.TabIndex = 14;
            this.btnOCRPDF.Text = "Data Entry via PDF / OCR";
            // 
            // btnLogout
            // 
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogout.Location = new System.Drawing.Point(408, 658);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(352, 58);
            this.btnLogout.TabIndex = 15;
            this.btnLogout.Text = "Log Out";
            this.btnLogout.UseVisualStyleBackColor = false;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMessage.Location = new System.Drawing.Point(73, 62);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(119, 32);
            this.lblMessage.TabIndex = 16;
            this.lblMessage.Text = "Welcome";
            // 
            // btnReport
            // 
            this.btnReport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnReport.Location = new System.Drawing.Point(408, 507);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(352, 58);
            this.btnReport.TabIndex = 17;
            this.btnReport.Text = "Download PDF";
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 836);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnStudentList);
            this.Controls.Add(this.btnAddExam);
            this.Controls.Add(this.btnViewAllResults);
            this.Controls.Add(this.btnOCRPDF);
            this.Controls.Add(this.btnLogout);
            this.Name = "AdminForm";
            this.Text = "AdminForm";
            this.Load += new System.EventHandler(this.AdminForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnStudentList;
        private System.Windows.Forms.Button btnAddExam;
        private System.Windows.Forms.Button btnViewAllResults;
        private System.Windows.Forms.Button btnOCRPDF;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnReport;
    }
}