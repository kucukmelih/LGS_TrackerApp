namespace LGS_Tracker
{
    partial class OCRandPDFInputForm
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
            this.btnSelectPDF = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnSaveToDB = new System.Windows.Forms.Button();
            this.lblStudent = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.cmbStudent = new System.Windows.Forms.ComboBox();
            this.lblSelectedFile = new System.Windows.Forms.Label();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSelectPDF
            // 
            this.btnSelectPDF.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSelectPDF.Location = new System.Drawing.Point(695, 312);
            this.btnSelectPDF.Name = "btnSelectPDF";
            this.btnSelectPDF.Size = new System.Drawing.Size(268, 58);
            this.btnSelectPDF.TabIndex = 10;
            this.btnSelectPDF.Text = "Upload PDF";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(292, 64);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(593, 51);
            this.lblTitle.TabIndex = 11;
            this.lblTitle.Text = "OCR and PDF Data Import Panel";
            // 
            // btnBack
            // 
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnBack.Location = new System.Drawing.Point(156, 698);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(268, 58);
            this.btnBack.TabIndex = 14;
            this.btnBack.Text = "Back";
            // 
            // btnSaveToDB
            // 
            this.btnSaveToDB.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSaveToDB.Location = new System.Drawing.Point(760, 698);
            this.btnSaveToDB.Name = "btnSaveToDB";
            this.btnSaveToDB.Size = new System.Drawing.Size(268, 58);
            this.btnSaveToDB.TabIndex = 15;
            this.btnSaveToDB.Text = "Save Exam";
            // 
            // lblStudent
            // 
            this.lblStudent.AutoSize = true;
            this.lblStudent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStudent.Location = new System.Drawing.Point(257, 162);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.Size = new System.Drawing.Size(109, 32);
            this.lblStudent.TabIndex = 16;
            this.lblStudent.Text = "Student:";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDescription.Location = new System.Drawing.Point(203, 234);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(221, 32);
            this.lblDescription.TabIndex = 18;
            this.lblDescription.Text = "Exam Description:";
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtDescription.Location = new System.Drawing.Point(441, 229);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(522, 39);
            this.txtDescription.TabIndex = 17;
            // 
            // cmbStudent
            // 
            this.cmbStudent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.cmbStudent.FormattingEnabled = true;
            this.cmbStudent.Location = new System.Drawing.Point(441, 157);
            this.cmbStudent.Name = "cmbStudent";
            this.cmbStudent.Size = new System.Drawing.Size(522, 39);
            this.cmbStudent.TabIndex = 19;
            // 
            // lblSelectedFile
            // 
            this.lblSelectedFile.AutoSize = true;
            this.lblSelectedFile.Location = new System.Drawing.Point(162, 389);
            this.lblSelectedFile.Name = "lblSelectedFile";
            this.lblSelectedFile.Size = new System.Drawing.Size(0, 25);
            this.lblSelectedFile.TabIndex = 21;
            // 
            // txtOutput
            // 
            this.txtOutput.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtOutput.Location = new System.Drawing.Point(156, 443);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(872, 213);
            this.txtOutput.TabIndex = 22;
            // 
            // OCRandPDFInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 836);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.lblSelectedFile);
            this.Controls.Add(this.cmbStudent);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblStudent);
            this.Controls.Add(this.btnSaveToDB);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnSelectPDF);
            this.Name = "OCRandPDFInputForm";
            this.Text = "OCRandPDFInputForm";
            this.Load += new System.EventHandler(this.OCRandPDFInputForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectPDF;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnSaveToDB;
        private System.Windows.Forms.Label lblStudent;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.ComboBox cmbStudent;
        private System.Windows.Forms.Label lblSelectedFile;
        private System.Windows.Forms.TextBox txtOutput;
    }
}