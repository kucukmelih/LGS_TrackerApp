namespace LGS_Tracker
{
    partial class StudentExamViewerForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvExams = new System.Windows.Forms.DataGridView();
            this.dgvDetails = new System.Windows.Forms.DataGridView();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.chartPerformance = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnDelete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartPerformance)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(451, 44);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(260, 51);
            this.lblTitle.TabIndex = 6;
            this.lblTitle.Text = "Exam History";
            // 
            // dgvExams
            // 
            this.dgvExams.ColumnHeadersHeight = 40;
            this.dgvExams.Location = new System.Drawing.Point(118, 119);
            this.dgvExams.MultiSelect = false;
            this.dgvExams.Name = "dgvExams";
            this.dgvExams.ReadOnly = true;
            this.dgvExams.RowHeadersWidth = 72;
            this.dgvExams.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvExams.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvExams.Size = new System.Drawing.Size(569, 317);
            this.dgvExams.TabIndex = 7;
            // 
            // dgvDetails
            // 
            this.dgvDetails.ColumnHeadersHeight = 40;
            this.dgvDetails.Location = new System.Drawing.Point(693, 119);
            this.dgvDetails.Name = "dgvDetails";
            this.dgvDetails.ReadOnly = true;
            this.dgvDetails.RowHeadersWidth = 72;
            this.dgvDetails.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvDetails.Size = new System.Drawing.Size(369, 317);
            this.dgvDetails.TabIndex = 8;
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExport.Location = new System.Drawing.Point(775, 754);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(248, 58);
            this.btnExport.TabIndex = 9;
            this.btnExport.Text = "Export to CSV";
            // 
            // btnBack
            // 
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnBack.Location = new System.Drawing.Point(153, 754);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(248, 58);
            this.btnBack.TabIndex = 10;
            this.btnBack.Text = "Go Back";
            // 
            // chartPerformance
            // 
            chartArea1.Name = "ChartArea1";
            this.chartPerformance.ChartAreas.Add(chartArea1);
            this.chartPerformance.Location = new System.Drawing.Point(153, 453);
            this.chartPerformance.Name = "chartPerformance";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "SuccessRate";
            this.chartPerformance.Series.Add(series1);
            this.chartPerformance.Size = new System.Drawing.Size(870, 268);
            this.chartPerformance.TabIndex = 12;
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(463, 754);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(248, 58);
            this.btnDelete.TabIndex = 22;
            this.btnDelete.Text = "Delete";
            // 
            // StudentExamViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 836);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.chartPerformance);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.dgvExams);
            this.Controls.Add(this.dgvDetails);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnBack);
            this.Name = "StudentExamViewerForm";
            this.Text = "StudentExamViewerForm";
            this.Load += new System.EventHandler(this.StudentExamViewerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartPerformance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvExams;
        private System.Windows.Forms.DataGridView dgvDetails;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPerformance;
        private System.Windows.Forms.Button btnDelete;
    }
}