namespace LGS_Tracker
{
    partial class StudentForm
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
            this.btnAddExam = new System.Windows.Forms.Button();
            this.btnViewResults = new System.Windows.Forms.Button();
            this.btnExportPDF = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(330, 236);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(510, 51);
            this.lblTitle.TabIndex = 6;
            this.lblTitle.Text = "Student Panel - Home Page";
            // 
            // btnAddExam
            // 
            this.btnAddExam.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddExam.Location = new System.Drawing.Point(446, 314);
            this.btnAddExam.Name = "btnAddExam";
            this.btnAddExam.Size = new System.Drawing.Size(274, 60);
            this.btnAddExam.TabIndex = 7;
            this.btnAddExam.Text = "Add New Exam";
            // 
            // btnViewResults
            // 
            this.btnViewResults.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnViewResults.Location = new System.Drawing.Point(446, 397);
            this.btnViewResults.Name = "btnViewResults";
            this.btnViewResults.Size = new System.Drawing.Size(274, 60);
            this.btnViewResults.TabIndex = 8;
            this.btnViewResults.Text = "View Exam History";
            // 
            // btnExportPDF
            // 
            this.btnExportPDF.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExportPDF.Location = new System.Drawing.Point(446, 479);
            this.btnExportPDF.Name = "btnExportPDF";
            this.btnExportPDF.Size = new System.Drawing.Size(274, 60);
            this.btnExportPDF.TabIndex = 10;
            this.btnExportPDF.Text = "Download PDF Report";
            // 
            // btnLogout
            // 
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogout.Location = new System.Drawing.Point(446, 558);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(274, 60);
            this.btnLogout.TabIndex = 11;
            this.btnLogout.Text = "Log Out";
            this.btnLogout.UseVisualStyleBackColor = false;
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.Location = new System.Drawing.Point(74, 49);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(119, 32);
            this.lblWelcome.TabIndex = 12;
            this.lblWelcome.Text = "Welcome";
            // 
            // StudentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 836);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnAddExam);
            this.Controls.Add(this.btnViewResults);
            this.Controls.Add(this.btnExportPDF);
            this.Controls.Add(this.btnLogout);
            this.Name = "StudentForm";
            this.Text = "StudentForm";
            this.Load += new System.EventHandler(this.StudentForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnAddExam;
        private System.Windows.Forms.Button btnViewResults;
        private System.Windows.Forms.Button btnExportPDF;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblWelcome;
    }
}