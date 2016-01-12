namespace HV9104_GUI
{
    partial class ReportForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 0D);
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.panel1 = new System.Windows.Forms.Panel();
            this.passFailLabel = new System.Windows.Forms.Label();
            this.passFailUnitlabel = new System.Windows.Forms.Label();
            this.passStatusLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.secondsUnitLabel = new System.Windows.Forms.Label();
            this.elapsedTimeLabel = new System.Windows.Forms.Label();
            this.autoTestChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label16 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.testTimeLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.autoTestChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.pictureBox3);
            this.panel1.Controls.Add(this.testTimeLabel);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.passFailLabel);
            this.panel1.Controls.Add(this.passFailUnitlabel);
            this.panel1.Controls.Add(this.passStatusLabel);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.secondsUnitLabel);
            this.panel1.Controls.Add(this.elapsedTimeLabel);
            this.panel1.Controls.Add(this.autoTestChart);
            this.panel1.Location = new System.Drawing.Point(29, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(765, 868);
            this.panel1.TabIndex = 0;
            // 
            // passFailLabel
            // 
            this.passFailLabel.AutoSize = true;
            this.passFailLabel.Font = new System.Drawing.Font("Calibri", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passFailLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.passFailLabel.Location = new System.Drawing.Point(446, 622);
            this.passFailLabel.Margin = new System.Windows.Forms.Padding(0);
            this.passFailLabel.Name = "passFailLabel";
            this.passFailLabel.Size = new System.Drawing.Size(123, 59);
            this.passFailLabel.TabIndex = 24;
            this.passFailLabel.Text = "PASS";
            this.passFailLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.passFailLabel.Visible = false;
            // 
            // passFailUnitlabel
            // 
            this.passFailUnitlabel.AutoSize = true;
            this.passFailUnitlabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passFailUnitlabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.passFailUnitlabel.Location = new System.Drawing.Point(467, 688);
            this.passFailUnitlabel.Name = "passFailUnitlabel";
            this.passFailUnitlabel.Size = new System.Drawing.Size(79, 19);
            this.passFailUnitlabel.TabIndex = 25;
            this.passFailUnitlabel.Text = "PASS / FAIL";
            this.passFailUnitlabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // passStatusLabel
            // 
            this.passStatusLabel.AutoSize = true;
            this.passStatusLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passStatusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.passStatusLabel.Location = new System.Drawing.Point(448, 594);
            this.passStatusLabel.Name = "passStatusLabel";
            this.passStatusLabel.Size = new System.Drawing.Size(118, 26);
            this.passStatusLabel.TabIndex = 23;
            this.passStatusLabel.Text = "TEST STATUS";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label7.Location = new System.Drawing.Point(127, 591);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 26);
            this.label7.TabIndex = 20;
            this.label7.Text = "ELAPSED TIME";
            // 
            // secondsUnitLabel
            // 
            this.secondsUnitLabel.AutoSize = true;
            this.secondsUnitLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.secondsUnitLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.secondsUnitLabel.Location = new System.Drawing.Point(159, 685);
            this.secondsUnitLabel.Name = "secondsUnitLabel";
            this.secondsUnitLabel.Size = new System.Drawing.Size(71, 19);
            this.secondsUnitLabel.TabIndex = 22;
            this.secondsUnitLabel.Text = "SECONDS";
            this.secondsUnitLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // elapsedTimeLabel
            // 
            this.elapsedTimeLabel.Font = new System.Drawing.Font("Calibri", 44.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.elapsedTimeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.elapsedTimeLabel.Location = new System.Drawing.Point(145, 608);
            this.elapsedTimeLabel.Name = "elapsedTimeLabel";
            this.elapsedTimeLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.elapsedTimeLabel.Size = new System.Drawing.Size(99, 80);
            this.elapsedTimeLabel.TabIndex = 21;
            this.elapsedTimeLabel.Text = "50";
            this.elapsedTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoTestChart
            // 
            this.autoTestChart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            chartArea2.AxisX.LineColor = System.Drawing.Color.Transparent;
            chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            chartArea2.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea2.AxisX.MajorTickMark.Enabled = false;
            chartArea2.AxisX.MinorGrid.LineColor = System.Drawing.Color.Transparent;
            chartArea2.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea2.AxisY.LineColor = System.Drawing.Color.Transparent;
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            chartArea2.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea2.AxisY.MajorTickMark.Enabled = false;
            chartArea2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            chartArea2.BorderColor = System.Drawing.Color.Transparent;
            chartArea2.Name = "ChartArea1";
            this.autoTestChart.ChartAreas.Add(chartArea2);
            this.autoTestChart.Location = new System.Drawing.Point(42, 219);
            this.autoTestChart.Name = "autoTestChart";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Name = "Series1";
            series2.Points.Add(dataPoint2);
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.autoTestChart.Series.Add(series2);
            this.autoTestChart.Size = new System.Drawing.Size(676, 301);
            this.autoTestChart.TabIndex = 19;
            this.autoTestChart.Text = "chart1";
            title2.Name = "Title1";
            this.autoTestChart.Titles.Add(title2);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Calibri", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(159)))), ((int)(((byte)(171)))));
            this.label4.Location = new System.Drawing.Point(467, 59);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 41);
            this.label4.TabIndex = 31;
            this.label4.Text = "s";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::HV9104_GUI.Properties.Resources.StopWatchGrey;
            this.pictureBox2.Location = new System.Drawing.Point(361, 53);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(111, 115);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 26;
            this.pictureBox2.TabStop = false;
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Calibri", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(159)))), ((int)(((byte)(171)))));
            this.label16.Location = new System.Drawing.Point(136, 53);
            this.label16.Margin = new System.Windows.Forms.Padding(0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(49, 41);
            this.label16.TabIndex = 30;
            this.label16.Text = "kV";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::HV9104_GUI.Properties.Resources.lightningBoltGrey;
            this.pictureBox3.Location = new System.Drawing.Point(42, 53);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(111, 115);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 27;
            this.pictureBox3.TabStop = false;
            // 
            // testTimeLabel
            // 
            this.testTimeLabel.AutoSize = true;
            this.testTimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testTimeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.testTimeLabel.Location = new System.Drawing.Point(332, 24);
            this.testTimeLabel.Name = "testTimeLabel";
            this.testTimeLabel.Size = new System.Drawing.Size(140, 18);
            this.testTimeLabel.TabIndex = 28;
            this.testTimeLabel.Text = "TEST DURATION";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label3.Location = new System.Drawing.Point(15, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 18);
            this.label3.TabIndex = 29;
            this.label3.Text = "TEST VOLTAGE";
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1322, 914);
            this.Controls.Add(this.panel1);
            this.Name = "ReportForm";
            this.Text = "ReportForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.autoTestChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Label passFailLabel;
        public System.Windows.Forms.Label passFailUnitlabel;
        public System.Windows.Forms.Label passStatusLabel;
        public System.Windows.Forms.Label label7;
        public System.Windows.Forms.Label secondsUnitLabel;
        public System.Windows.Forms.Label elapsedTimeLabel;
        public System.Windows.Forms.DataVisualization.Charting.Chart autoTestChart;
        public System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox2;
        public System.Windows.Forms.Label label16;
        private System.Windows.Forms.PictureBox pictureBox3;
        public System.Windows.Forms.Label testTimeLabel;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Panel panel1;
    }
}