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
            this.testDurationLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.motorInitButton = new HV9104_GUI.CustomButton();
            this.label5 = new System.Windows.Forms.Label();
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
            this.label4 = new System.Windows.Forms.Label();
            this.customTextBox1 = new HV9104_GUI.CustomTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.customPanel5 = new HV9104_GUI.CustomPanel();
            this.label16 = new System.Windows.Forms.Label();
            this.customTextBox2 = new HV9104_GUI.CustomTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.customPanel6 = new HV9104_GUI.CustomPanel();
            this.customButton1 = new HV9104_GUI.CustomButton();
            this.onOffButton = new HV9104_GUI.CustomCheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.customPanel7 = new HV9104_GUI.CustomPanel();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.autoTestChart)).BeginInit();
            this.customPanel1.SuspendLayout();
            this.customPanel2.SuspendLayout();
            this.customPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.customPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.customPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.customPanel6.SuspendLayout();
            this.customPanel7.SuspendLayout();
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
            this.autoTestChart.Location = new System.Drawing.Point(56, 63);
            this.autoTestChart.Name = "autoTestChart";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Name = "Series1";
            series2.Points.Add(dataPoint2);
            this.autoTestChart.Series.Add(series2);
            this.autoTestChart.Size = new System.Drawing.Size(362, 217);
            this.autoTestChart.TabIndex = 0;
            this.autoTestChart.Text = "chart1";
            // 
            // customPanel1
            // 
            this.customPanel1.BackColor = System.Drawing.Color.Transparent;
            this.customPanel1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel1.Controls.Add(this.testDurationLabel);
            this.customPanel1.Controls.Add(this.label7);
            this.customPanel1.Controls.Add(this.motorInitButton);
            this.customPanel1.Controls.Add(this.label5);
            this.customPanel1.Controls.Add(this.autoTestChart);
            this.customPanel1.CornerRadius = 40;
            this.customPanel1.IsPopUp = false;
            this.customPanel1.Location = new System.Drawing.Point(1005, 266);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(483, 505);
            this.customPanel1.TabIndex = 1;
            // 
            // testDurationLabel
            // 
            this.testDurationLabel.Font = new System.Drawing.Font("Calibri", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testDurationLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.testDurationLabel.Location = new System.Drawing.Point(69, 354);
            this.testDurationLabel.Name = "testDurationLabel";
            this.testDurationLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.testDurationLabel.Size = new System.Drawing.Size(99, 97);
            this.testDurationLabel.TabIndex = 11;
            this.testDurationLabel.Text = "50";
            this.testDurationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label7.Location = new System.Drawing.Point(51, 328);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 26);
            this.label7.TabIndex = 10;
            this.label7.Text = "ELAPSED TIME";
            // 
            // motorInitButton
            // 
            this.motorInitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.motorInitButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.button;
            this.motorInitButton.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.motorInitButton.ForeColor = System.Drawing.Color.White;
            this.motorInitButton.HoverImage = global::HV9104_GUI.Properties.Resources.buttonHover;
            this.motorInitButton.Location = new System.Drawing.Point(294, 414);
            this.motorInitButton.Name = "motorInitButton";
            this.motorInitButton.PressedImage = global::HV9104_GUI.Properties.Resources.buttonPressed;
            this.motorInitButton.Size = new System.Drawing.Size(158, 57);
            this.motorInitButton.TabIndex = 7;
            this.motorInitButton.Text = "EXPORT";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label5.Location = new System.Drawing.Point(22, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 18);
            this.label5.TabIndex = 5;
            this.label5.Text = "TEST RESULTS";
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(837, 32);
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
            this.label23.Location = new System.Drawing.Point(21, 19);
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
            this.customTextBox1.Min = 5D;
            this.customTextBox1.MinimumSize = new System.Drawing.Size(170, 50);
            this.customTextBox1.Name = "customTextBox1";
            this.customTextBox1.Size = new System.Drawing.Size(170, 50);
            this.customTextBox1.TabIndex = 4;
            this.customTextBox1.TextBoxHint = "";
            this.customTextBox1.Value = 60F;
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
            this.customTextBox2.Min = 5D;
            this.customTextBox2.MinimumSize = new System.Drawing.Size(170, 50);
            this.customTextBox2.Name = "customTextBox2";
            this.customTextBox2.Size = new System.Drawing.Size(170, 50);
            this.customTextBox2.TabIndex = 5;
            this.customTextBox2.TextBoxHint = "";
            this.customTextBox2.Value = 33F;
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
            // customPanel6
            // 
            this.customPanel6.BackColor = System.Drawing.Color.Transparent;
            this.customPanel6.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel6.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel6.Controls.Add(this.customButton1);
            this.customPanel6.Controls.Add(this.onOffButton);
            this.customPanel6.Controls.Add(this.label15);
            this.customPanel6.Controls.Add(this.label6);
            this.customPanel6.CornerRadius = 40;
            this.customPanel6.IsPopUp = false;
            this.customPanel6.Location = new System.Drawing.Point(499, 266);
            this.customPanel6.Name = "customPanel6";
            this.customPanel6.Size = new System.Drawing.Size(381, 254);
            this.customPanel6.TabIndex = 7;
            // 
            // customButton1
            // 
            this.customButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customButton1.BackgroundImage = global::HV9104_GUI.Properties.Resources.button;
            this.customButton1.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customButton1.ForeColor = System.Drawing.Color.White;
            this.customButton1.HoverImage = global::HV9104_GUI.Properties.Resources.buttonHover;
            this.customButton1.Location = new System.Drawing.Point(161, 163);
            this.customButton1.Name = "customButton1";
            this.customButton1.PressedImage = global::HV9104_GUI.Properties.Resources.buttonPressed;
            this.customButton1.Size = new System.Drawing.Size(158, 57);
            this.customButton1.TabIndex = 12;
            this.customButton1.Text = "ABORT";
            // 
            // onOffButton
            // 
            this.onOffButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.onOffButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.offButton;
            this.onOffButton.CheckedHoverImage = global::HV9104_GUI.Properties.Resources.onButtonHover;
            this.onOffButton.CheckedImage = global::HV9104_GUI.Properties.Resources.onButton;
            this.onOffButton.isChecked = false;
            this.onOffButton.Location = new System.Drawing.Point(171, 78);
            this.onOffButton.Name = "onOffButton";
            this.onOffButton.Size = new System.Drawing.Size(138, 56);
            this.onOffButton.TabIndex = 10;
            this.onOffButton.Text = "customCheckBox2";
            this.onOffButton.UncheckedHoverImage = global::HV9104_GUI.Properties.Resources.offButtonHover;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label15.Location = new System.Drawing.Point(23, 93);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(124, 26);
            this.label15.TabIndex = 9;
            this.label15.Text = "START/PAUSE";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label6.Location = new System.Drawing.Point(25, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 18);
            this.label6.TabIndex = 4;
            this.label6.Text = "TEST CONTROL";
            // 
            // customPanel7
            // 
            this.customPanel7.BackColor = System.Drawing.Color.Transparent;
            this.customPanel7.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel7.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel7.Controls.Add(this.label8);
            this.customPanel7.CornerRadius = 40;
            this.customPanel7.IsPopUp = false;
            this.customPanel7.Location = new System.Drawing.Point(8, 542);
            this.customPanel7.Name = "customPanel7";
            this.customPanel7.Size = new System.Drawing.Size(467, 229);
            this.customPanel7.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label8.Location = new System.Drawing.Point(16, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(273, 18);
            this.label8.TabIndex = 5;
            this.label8.Text = "IMPULSE TESTING PARAMETERS";
            // 
            // RunView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.customPanel7);
            this.Controls.Add(this.customPanel6);
            this.Controls.Add(this.customPanel5);
            this.Controls.Add(this.customPanel4);
            this.Controls.Add(this.customPanel3);
            this.Controls.Add(this.testButton);
            this.Controls.Add(this.customPanel2);
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.customPanel5.ResumeLayout(false);
            this.customPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.customPanel6.ResumeLayout(false);
            this.customPanel6.PerformLayout();
            this.customPanel7.ResumeLayout(false);
            this.customPanel7.PerformLayout();
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
        public System.Windows.Forms.Label label5;
        private CustomPanel customPanel6;
        public System.Windows.Forms.Label label6;
        public CustomButton motorInitButton;
        public System.Windows.Forms.Label label7;
        public CustomButton customButton1;
        public CustomCheckBox onOffButton;
        public System.Windows.Forms.Label label15;
        public System.Windows.Forms.Label testDurationLabel;
        private CustomPanel customPanel7;
        public System.Windows.Forms.Label label8;
    }
}
