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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint4 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 0D);
            this.autoTestChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.customPanel1 = new HV9104_GUI.CustomPanel();
            this.testButton = new System.Windows.Forms.Button();
            this.WithstandRadioButton = new HV9104_GUI.CustomRadioButton();
            this.DisruptiveRadioButton = new HV9104_GUI.CustomRadioButton();
            this.label21 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.customPanel2 = new HV9104_GUI.CustomPanel();
            this.voltageComboBox = new HV9104_GUI.CustomComboBox();
            this.customPanel3 = new HV9104_GUI.CustomPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.customPanel4 = new HV9104_GUI.CustomPanel();
            this.customPanel5 = new HV9104_GUI.CustomPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.customTextBox1 = new HV9104_GUI.CustomTextBox();
            this.customTextBox2 = new HV9104_GUI.CustomTextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.autoTestChart)).BeginInit();
            this.customPanel1.SuspendLayout();
            this.customPanel2.SuspendLayout();
            this.customPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.customPanel4.SuspendLayout();
            this.customPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // autoTestChart
            // 
            this.autoTestChart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            chartArea4.AxisX.LineColor = System.Drawing.Color.Transparent;
            chartArea4.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            chartArea4.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea4.AxisX.MajorTickMark.Enabled = false;
            chartArea4.AxisX.MinorGrid.LineColor = System.Drawing.Color.Transparent;
            chartArea4.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea4.AxisY.LineColor = System.Drawing.Color.Transparent;
            chartArea4.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            chartArea4.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea4.AxisY.MajorTickMark.Enabled = false;
            chartArea4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            chartArea4.BorderColor = System.Drawing.Color.Transparent;
            chartArea4.Name = "ChartArea1";
            this.autoTestChart.ChartAreas.Add(chartArea4);
            this.autoTestChart.Location = new System.Drawing.Point(23, 24);
            this.autoTestChart.Name = "autoTestChart";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series4.Name = "Series1";
            series4.Points.Add(dataPoint4);
            this.autoTestChart.Series.Add(series4);
            this.autoTestChart.Size = new System.Drawing.Size(285, 184);
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
            this.customPanel1.Location = new System.Drawing.Point(551, 507);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(341, 263);
            this.customPanel1.TabIndex = 1;
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(1237, 422);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(118, 51);
            this.testButton.TabIndex = 2;
            this.testButton.Text = "addPoints";
            this.testButton.UseVisualStyleBackColor = true;
            // 
            // WithstandRadioButton
            // 
            this.WithstandRadioButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.WithstandRadioButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.radioButton;
            this.WithstandRadioButton.CheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonCheckedHover;
            this.WithstandRadioButton.CheckedImage = global::HV9104_GUI.Properties.Resources.radioButtonChecked;
            this.WithstandRadioButton.isChecked = true;
            this.WithstandRadioButton.Location = new System.Drawing.Point(272, 49);
            this.WithstandRadioButton.Name = "WithstandRadioButton";
            this.WithstandRadioButton.Size = new System.Drawing.Size(47, 47);
            this.WithstandRadioButton.TabIndex = 2;
            this.WithstandRadioButton.Text = "customRadioButton1";
            this.WithstandRadioButton.UncheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonHover;
            // 
            // DisruptiveRadioButton
            // 
            this.DisruptiveRadioButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.DisruptiveRadioButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.radioButton;
            this.DisruptiveRadioButton.CheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonCheckedHover;
            this.DisruptiveRadioButton.CheckedImage = global::HV9104_GUI.Properties.Resources.radioButtonChecked;
            this.DisruptiveRadioButton.isChecked = false;
            this.DisruptiveRadioButton.Location = new System.Drawing.Point(272, 102);
            this.DisruptiveRadioButton.Name = "DisruptiveRadioButton";
            this.DisruptiveRadioButton.Size = new System.Drawing.Size(47, 47);
            this.DisruptiveRadioButton.TabIndex = 2;
            this.DisruptiveRadioButton.Text = "customRadioButton1";
            this.DisruptiveRadioButton.UncheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonHover;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label21.Location = new System.Drawing.Point(35, 58);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(116, 29);
            this.label21.TabIndex = 2;
            this.label21.Text = "Withstand";
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
            this.label20.Location = new System.Drawing.Point(35, 111);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(214, 29);
            this.label20.TabIndex = 2;
            this.label20.Text = "Disruptive Discharge";
            // 
            // customPanel2
            // 
            this.customPanel2.BackColor = System.Drawing.Color.Transparent;
            this.customPanel2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel2.Controls.Add(this.voltageComboBox);
            this.customPanel2.Controls.Add(this.label20);
            this.customPanel2.Controls.Add(this.label23);
            this.customPanel2.Controls.Add(this.label21);
            this.customPanel2.Controls.Add(this.DisruptiveRadioButton);
            this.customPanel2.Controls.Add(this.WithstandRadioButton);
            this.customPanel2.CornerRadius = 40;
            this.customPanel2.IsPopUp = false;
            this.customPanel2.Location = new System.Drawing.Point(3, 13);
            this.customPanel2.Name = "customPanel2";
            this.customPanel2.Size = new System.Drawing.Size(365, 233);
            this.customPanel2.TabIndex = 3;
            // 
            // voltageComboBox
            // 
            this.voltageComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.voltageComboBox.Location = new System.Drawing.Point(40, 163);
            this.voltageComboBox.MaximumSize = new System.Drawing.Size(400, 60);
            this.voltageComboBox.MinimumSize = new System.Drawing.Size(120, 45);
            this.voltageComboBox.Name = "voltageComboBox";
            this.voltageComboBox.setCollection = new string[] {
        "AC",
        "DC",
        "Impulse"};
            this.voltageComboBox.SetSelected = "AC";
            this.voltageComboBox.Size = new System.Drawing.Size(184, 45);
            this.voltageComboBox.TabIndex = 4;
            this.voltageComboBox.Text = "customComboBox1";
            this.voltageComboBox.TextBoxHint = "";
            // 
            // customPanel3
            // 
            this.customPanel3.BackColor = System.Drawing.Color.Transparent;
            this.customPanel3.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel3.Controls.Add(this.pictureBox1);
            this.customPanel3.Controls.Add(this.textBox1);
            this.customPanel3.Controls.Add(this.label1);
            this.customPanel3.CornerRadius = 40;
            this.customPanel3.IsPopUp = false;
            this.customPanel3.Location = new System.Drawing.Point(1005, 15);
            this.customPanel3.Name = "customPanel3";
            this.customPanel3.Size = new System.Drawing.Size(1011, 231);
            this.customPanel3.TabIndex = 4;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(248, 59);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(679, 122);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(303, 119);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(318, 20);
            this.textBox1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label1.Location = new System.Drawing.Point(23, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "REPORT INFO";
            // 
            // customPanel4
            // 
            this.customPanel4.BackColor = System.Drawing.Color.Transparent;
            this.customPanel4.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel4.Controls.Add(this.label4);
            this.customPanel4.Controls.Add(this.customTextBox1);
            this.customPanel4.Controls.Add(this.label2);
            this.customPanel4.Controls.Add(this.pictureBox2);
            this.customPanel4.CornerRadius = 40;
            this.customPanel4.IsPopUp = false;
            this.customPanel4.Location = new System.Drawing.Point(3, 266);
            this.customPanel4.Name = "customPanel4";
            this.customPanel4.Size = new System.Drawing.Size(224, 254);
            this.customPanel4.TabIndex = 5;
            // 
            // customPanel5
            // 
            this.customPanel5.BackColor = System.Drawing.Color.Transparent;
            this.customPanel5.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel5.Controls.Add(this.label16);
            this.customPanel5.Controls.Add(this.customTextBox2);
            this.customPanel5.Controls.Add(this.label3);
            this.customPanel5.Controls.Add(this.pictureBox3);
            this.customPanel5.CornerRadius = 40;
            this.customPanel5.IsPopUp = false;
            this.customPanel5.Location = new System.Drawing.Point(251, 266);
            this.customPanel5.Name = "customPanel5";
            this.customPanel5.Size = new System.Drawing.Size(224, 254);
            this.customPanel5.TabIndex = 6;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::HV9104_GUI.Properties.Resources.StopWatchGrey;
            this.pictureBox2.Location = new System.Drawing.Point(51, 53);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(111, 115);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::HV9104_GUI.Properties.Resources.lightningBoltGrey;
            this.pictureBox3.Location = new System.Drawing.Point(51, 53);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(111, 115);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label2.Location = new System.Drawing.Point(21, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "TEST DURATION";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label3.Location = new System.Drawing.Point(20, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "TEST VOLTAGE";
            // 
            // customTextBox1
            // 
            this.customTextBox1.AllowDecimals = true;
            this.customTextBox1.AllowText = false;
            this.customTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customTextBox1.BackgroundColor = System.Drawing.Color.White;
            this.customTextBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(159)))), ((int)(((byte)(171)))));
            this.customTextBox1.CornerRadius = 25;
            this.customTextBox1.Decimals = 0;
            this.customTextBox1.IsPopUp = false;
            this.customTextBox1.Location = new System.Drawing.Point(21, 183);
            this.customTextBox1.Max = 1800D;
            this.customTextBox1.MaximumSize = new System.Drawing.Size(400, 50);
            this.customTextBox1.Min = 0D;
            this.customTextBox1.MinimumSize = new System.Drawing.Size(170, 50);
            this.customTextBox1.Name = "customTextBox1";
            this.customTextBox1.Size = new System.Drawing.Size(170, 50);
            this.customTextBox1.TabIndex = 4;
            this.customTextBox1.TextBoxHint = "";
            this.customTextBox1.Value = 60F;
            // 
            // customTextBox2
            // 
            this.customTextBox2.AllowDecimals = true;
            this.customTextBox2.AllowText = false;
            this.customTextBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customTextBox2.BackgroundColor = System.Drawing.Color.White;
            this.customTextBox2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(159)))), ((int)(((byte)(171)))));
            this.customTextBox2.CornerRadius = 25;
            this.customTextBox2.Decimals = 2;
            this.customTextBox2.IsPopUp = false;
            this.customTextBox2.Location = new System.Drawing.Point(24, 183);
            this.customTextBox2.Max = 400D;
            this.customTextBox2.MaximumSize = new System.Drawing.Size(400, 50);
            this.customTextBox2.Min = 0D;
            this.customTextBox2.MinimumSize = new System.Drawing.Size(170, 50);
            this.customTextBox2.Name = "customTextBox2";
            this.customTextBox2.Size = new System.Drawing.Size(170, 50);
            this.customTextBox2.TabIndex = 5;
            this.customTextBox2.TextBoxHint = "";
            this.customTextBox2.Value = 33F;
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Calibri", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(159)))), ((int)(((byte)(171)))));
            this.label16.Location = new System.Drawing.Point(145, 53);
            this.label16.Margin = new System.Windows.Forms.Padding(0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(49, 41);
            this.label16.TabIndex = 6;
            this.label16.Text = "kV";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Calibri", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(159)))), ((int)(((byte)(171)))));
            this.label4.Location = new System.Drawing.Point(156, 53);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 41);
            this.label4.TabIndex = 7;
            this.label4.Text = "s";
            // 
            // RunView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.customPanel5);
            this.Controls.Add(this.customPanel4);
            this.Controls.Add(this.customPanel3);
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
            this.customPanel3.ResumeLayout(false);
            this.customPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.customPanel4.ResumeLayout(false);
            this.customPanel4.PerformLayout();
            this.customPanel5.ResumeLayout(false);
            this.customPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataVisualization.Charting.Chart autoTestChart;
        public CustomPanel customPanel1;
        public System.Windows.Forms.Button testButton;
        public CustomRadioButton WithstandRadioButton;
        public CustomRadioButton DisruptiveRadioButton;
        public System.Windows.Forms.Label label21;
        public System.Windows.Forms.Label label23;
        public System.Windows.Forms.Label label20;
        public CustomPanel customPanel2;
        public CustomComboBox voltageComboBox;
        private CustomPanel customPanel3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.Label label1;
        private CustomPanel customPanel4;
        private CustomPanel customPanel5;
        private CustomTextBox customTextBox1;
        public System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private CustomTextBox customTextBox2;
        public System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox3;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label label16;
    }
}
