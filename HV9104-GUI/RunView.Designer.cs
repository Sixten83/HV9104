namespace HV9104_GUI
{
    partial class RunView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 0D);
            this.autoTestChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.customPanel1 = new HV9104_GUI.CustomPanel();
            this.testButton = new System.Windows.Forms.Button();
            this.dcDisruptiveRadioButton = new HV9104_GUI.CustomRadioButton();
            this.impulseDisruptiveRadioButton = new HV9104_GUI.CustomRadioButton();
            this.label25 = new System.Windows.Forms.Label();
            this.acWithstandRadioButton = new HV9104_GUI.CustomRadioButton();
            this.label24 = new System.Windows.Forms.Label();
            this.impulseWithstandRadioButton = new HV9104_GUI.CustomRadioButton();
            this.acDisruptiveRadioButton = new HV9104_GUI.CustomRadioButton();
            this.label21 = new System.Windows.Forms.Label();
            this.dcWithstandRadioButton = new HV9104_GUI.CustomRadioButton();
            this.label26 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.customPanel2 = new HV9104_GUI.CustomPanel();
            ((System.ComponentModel.ISupportInitialize)(this.autoTestChart)).BeginInit();
            this.customPanel1.SuspendLayout();
            this.customPanel2.SuspendLayout();
            this.SuspendLayout();
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
            this.autoTestChart.Location = new System.Drawing.Point(24, 23);
            this.autoTestChart.Name = "autoTestChart";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Name = "Series1";
            series2.Points.Add(dataPoint2);
            this.autoTestChart.Series.Add(series2);
            this.autoTestChart.Size = new System.Drawing.Size(376, 265);
            this.autoTestChart.TabIndex = 0;
            this.autoTestChart.Text = "chart1";
            // 
            // customPanel1
            // 
            this.customPanel1.BackColor = System.Drawing.Color.Transparent;
            this.customPanel1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel1.Controls.Add(this.autoTestChart);
            this.customPanel1.CornerRadius = 40;
            this.customPanel1.IsPopUp = false;
            this.customPanel1.Location = new System.Drawing.Point(660, 452);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(431, 328);
            this.customPanel1.TabIndex = 1;
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(536, 729);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(118, 51);
            this.testButton.TabIndex = 2;
            this.testButton.Text = "addPoints";
            this.testButton.UseVisualStyleBackColor = true;
            // 
            // dcDisruptiveRadioButton
            // 
            this.dcDisruptiveRadioButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.dcDisruptiveRadioButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.radioButton;
            this.dcDisruptiveRadioButton.CheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonCheckedHover;
            this.dcDisruptiveRadioButton.CheckedImage = global::HV9104_GUI.Properties.Resources.radioButtonChecked;
            this.dcDisruptiveRadioButton.isChecked = false;
            this.dcDisruptiveRadioButton.Location = new System.Drawing.Point(615, 120);
            this.dcDisruptiveRadioButton.Name = "dcDisruptiveRadioButton";
            this.dcDisruptiveRadioButton.Size = new System.Drawing.Size(47, 47);
            this.dcDisruptiveRadioButton.TabIndex = 2;
            this.dcDisruptiveRadioButton.Text = "customRadioButton1";
            this.dcDisruptiveRadioButton.UncheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonHover;
            // 
            // impulseDisruptiveRadioButton
            // 
            this.impulseDisruptiveRadioButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.impulseDisruptiveRadioButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.radioButton;
            this.impulseDisruptiveRadioButton.CheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonCheckedHover;
            this.impulseDisruptiveRadioButton.CheckedImage = global::HV9104_GUI.Properties.Resources.radioButtonChecked;
            this.impulseDisruptiveRadioButton.isChecked = false;
            this.impulseDisruptiveRadioButton.Location = new System.Drawing.Point(1009, 120);
            this.impulseDisruptiveRadioButton.Name = "impulseDisruptiveRadioButton";
            this.impulseDisruptiveRadioButton.Size = new System.Drawing.Size(47, 47);
            this.impulseDisruptiveRadioButton.TabIndex = 2;
            this.impulseDisruptiveRadioButton.Text = "customRadioButton1";
            this.impulseDisruptiveRadioButton.UncheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonHover;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label25.Location = new System.Drawing.Point(701, 81);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(199, 29);
            this.label25.TabIndex = 2;
            this.label25.Text = "Impulse Withstand";
            // 
            // acWithstandRadioButton
            // 
            this.acWithstandRadioButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.acWithstandRadioButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.radioButton;
            this.acWithstandRadioButton.CheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonCheckedHover;
            this.acWithstandRadioButton.CheckedImage = global::HV9104_GUI.Properties.Resources.radioButtonChecked;
            this.acWithstandRadioButton.isChecked = false;
            this.acWithstandRadioButton.Location = new System.Drawing.Point(287, 67);
            this.acWithstandRadioButton.Name = "acWithstandRadioButton";
            this.acWithstandRadioButton.Size = new System.Drawing.Size(47, 47);
            this.acWithstandRadioButton.TabIndex = 2;
            this.acWithstandRadioButton.Text = "customRadioButton1";
            this.acWithstandRadioButton.UncheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonHover;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label24.Location = new System.Drawing.Point(362, 133);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(247, 29);
            this.label24.TabIndex = 2;
            this.label24.Text = "DC Disruptive Discharge";
            // 
            // impulseWithstandRadioButton
            // 
            this.impulseWithstandRadioButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.impulseWithstandRadioButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.radioButton;
            this.impulseWithstandRadioButton.CheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonCheckedHover;
            this.impulseWithstandRadioButton.CheckedImage = global::HV9104_GUI.Properties.Resources.radioButtonChecked;
            this.impulseWithstandRadioButton.isChecked = false;
            this.impulseWithstandRadioButton.Location = new System.Drawing.Point(1009, 67);
            this.impulseWithstandRadioButton.Name = "impulseWithstandRadioButton";
            this.impulseWithstandRadioButton.Size = new System.Drawing.Size(47, 47);
            this.impulseWithstandRadioButton.TabIndex = 2;
            this.impulseWithstandRadioButton.Text = "customRadioButton1";
            this.impulseWithstandRadioButton.UncheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonHover;
            // 
            // acDisruptiveRadioButton
            // 
            this.acDisruptiveRadioButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.acDisruptiveRadioButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.radioButton;
            this.acDisruptiveRadioButton.CheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonCheckedHover;
            this.acDisruptiveRadioButton.CheckedImage = global::HV9104_GUI.Properties.Resources.radioButtonChecked;
            this.acDisruptiveRadioButton.isChecked = false;
            this.acDisruptiveRadioButton.Location = new System.Drawing.Point(287, 120);
            this.acDisruptiveRadioButton.Name = "acDisruptiveRadioButton";
            this.acDisruptiveRadioButton.Size = new System.Drawing.Size(47, 47);
            this.acDisruptiveRadioButton.TabIndex = 2;
            this.acDisruptiveRadioButton.Text = "customRadioButton1";
            this.acDisruptiveRadioButton.UncheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonHover;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label21.Location = new System.Drawing.Point(35, 81);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(116, 29);
            this.label21.TabIndex = 2;
            this.label21.Text = "Withstand";
            // 
            // dcWithstandRadioButton
            // 
            this.dcWithstandRadioButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.dcWithstandRadioButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.radioButton;
            this.dcWithstandRadioButton.CheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonCheckedHover;
            this.dcWithstandRadioButton.CheckedImage = global::HV9104_GUI.Properties.Resources.radioButtonChecked;
            this.dcWithstandRadioButton.isChecked = false;
            this.dcWithstandRadioButton.Location = new System.Drawing.Point(615, 67);
            this.dcWithstandRadioButton.Name = "dcWithstandRadioButton";
            this.dcWithstandRadioButton.Size = new System.Drawing.Size(47, 47);
            this.dcWithstandRadioButton.TabIndex = 2;
            this.dcWithstandRadioButton.Text = "customRadioButton1";
            this.dcWithstandRadioButton.UncheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonHover;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label26.Location = new System.Drawing.Point(701, 133);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(302, 29);
            this.label26.TabIndex = 2;
            this.label26.Text = "Impulse  Disruptive Discharge";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label4.Location = new System.Drawing.Point(362, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 29);
            this.label4.TabIndex = 2;
            this.label4.Text = "DC Withstand";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label23.Location = new System.Drawing.Point(21, 18);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(97, 18);
            this.label23.TabIndex = 2;
            this.label23.Text = "TEST TYPE";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label20.Location = new System.Drawing.Point(35, 133);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(246, 29);
            this.label20.TabIndex = 2;
            this.label20.Text = "AC Disruptive Discharge";
            // 
            // customPanel2
            // 
            this.customPanel2.BackColor = System.Drawing.Color.Transparent;
            this.customPanel2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel2.Controls.Add(this.label20);
            this.customPanel2.Controls.Add(this.label23);
            this.customPanel2.Controls.Add(this.label4);
            this.customPanel2.Controls.Add(this.label26);
            this.customPanel2.Controls.Add(this.dcWithstandRadioButton);
            this.customPanel2.Controls.Add(this.label21);
            this.customPanel2.Controls.Add(this.acDisruptiveRadioButton);
            this.customPanel2.Controls.Add(this.impulseWithstandRadioButton);
            this.customPanel2.Controls.Add(this.label24);
            this.customPanel2.Controls.Add(this.acWithstandRadioButton);
            this.customPanel2.Controls.Add(this.label25);
            this.customPanel2.Controls.Add(this.impulseDisruptiveRadioButton);
            this.customPanel2.Controls.Add(this.dcDisruptiveRadioButton);
            this.customPanel2.CornerRadius = 40;
            this.customPanel2.IsPopUp = false;
            this.customPanel2.Location = new System.Drawing.Point(3, 13);
            this.customPanel2.Name = "customPanel2";
            this.customPanel2.Size = new System.Drawing.Size(1088, 197);
            this.customPanel2.TabIndex = 3;
            // 
            // RunView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.customPanel2);
            this.Controls.Add(this.testButton);
            this.Controls.Add(this.customPanel1);
            this.Name = "RunView";
            this.Size = new System.Drawing.Size(1875, 800);
            ((System.ComponentModel.ISupportInitialize)(this.autoTestChart)).EndInit();
            this.customPanel1.ResumeLayout(false);
            this.customPanel1.PerformLayout();
            this.customPanel2.ResumeLayout(false);
            this.customPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataVisualization.Charting.Chart autoTestChart;
        public CustomPanel customPanel1;
        public System.Windows.Forms.Button testButton;
        public CustomRadioButton dcDisruptiveRadioButton;
        public CustomRadioButton impulseDisruptiveRadioButton;
        public System.Windows.Forms.Label label25;
        public CustomRadioButton acWithstandRadioButton;
        public System.Windows.Forms.Label label24;
        public CustomRadioButton impulseWithstandRadioButton;
        public CustomRadioButton acDisruptiveRadioButton;
        public System.Windows.Forms.Label label21;
        public CustomRadioButton dcWithstandRadioButton;
        public System.Windows.Forms.Label label26;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label label23;
        public System.Windows.Forms.Label label20;
        public CustomPanel customPanel2;
    }
}
