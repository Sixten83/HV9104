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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 0D);
            this.autoTestChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.customPanel1 = new HV9104_GUI.CustomPanel();
            this.testStatusLabel = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.onOffAutoButton = new HV9104_GUI.CustomCheckBox();
            this.abortAutoTestButton = new HV9104_GUI.CustomButton();
            this.passStatusLabel = new System.Windows.Forms.Label();
            this.passFailLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.secondsUnitLabel = new System.Windows.Forms.Label();
            this.testDurationLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.createReportButton = new HV9104_GUI.CustomButton();
            this.exportValuesButton = new HV9104_GUI.CustomButton();
            this.testButton = new System.Windows.Forms.Button();
            this.WithstandRadioButton = new HV9104_GUI.CustomRadioButton();
            this.DisruptiveRadioButton = new HV9104_GUI.CustomRadioButton();
            this.label21 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.customPanel2 = new HV9104_GUI.CustomPanel();
            this.voltageComboBox = new HV9104_GUI.CustomComboBox();
            this.customPanel3 = new HV9104_GUI.CustomPanel();
            this.otherTextBox = new System.Windows.Forms.TextBox();
            this.otherLabel = new System.Windows.Forms.Label();
            this.operatorTextBox = new System.Windows.Forms.TextBox();
            this.testObjectTextBox = new System.Windows.Forms.TextBox();
            this.operatorLabel = new System.Windows.Forms.Label();
            this.testObjectLabel = new System.Windows.Forms.Label();
            this.dateLabel = new System.Windows.Forms.Label();
            this.dateTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.customPanel4 = new HV9104_GUI.CustomPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.testDurationTextBox = new HV9104_GUI.CustomTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.customPanel5 = new HV9104_GUI.CustomPanel();
            this.label16 = new System.Windows.Forms.Label();
            this.testVoltageTextBox = new HV9104_GUI.CustomTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.customPanel7 = new HV9104_GUI.CustomPanel();
            this.impPerLevelLabel = new System.Windows.Forms.Label();
            this.voltLevelsLabel = new System.Windows.Forms.Label();
            this.impulsePerLevelComboBox = new HV9104_GUI.CustomComboBox();
            this.voltageLevelsComboBox = new HV9104_GUI.CustomComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.customPanel8 = new HV9104_GUI.CustomPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.impulseOutputComboBox = new HV9104_GUI.CustomComboBox();
            this.impulseTitleLabel = new System.Windows.Forms.Label();
            this.impulseValueLabel = new System.Windows.Forms.Label();
            this.customPanel9 = new HV9104_GUI.CustomPanel();
            this.label17 = new System.Windows.Forms.Label();
            this.dcOutputComboBox = new HV9104_GUI.CustomComboBox();
            this.dcTitleLabel = new System.Windows.Forms.Label();
            this.dcValueLabel = new System.Windows.Forms.Label();
            this.customPanel10 = new HV9104_GUI.CustomPanel();
            this.acOutputComboBox = new HV9104_GUI.CustomComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.acTitleLabel = new System.Windows.Forms.Label();
            this.acValueLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.autoTestChart)).BeginInit();
            this.customPanel1.SuspendLayout();
            this.customPanel2.SuspendLayout();
            this.customPanel3.SuspendLayout();
            this.customPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.customPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.customPanel7.SuspendLayout();
            this.customPanel8.SuspendLayout();
            this.customPanel9.SuspendLayout();
            this.customPanel10.SuspendLayout();
            this.SuspendLayout();
            // 
            // autoTestChart
            // 
            this.autoTestChart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            chartArea1.AxisX.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisY.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisY.MajorTickMark.Enabled = false;
            chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            chartArea1.BorderColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.autoTestChart.ChartAreas.Add(chartArea1);
            this.autoTestChart.Location = new System.Drawing.Point(64, 50);
            this.autoTestChart.Name = "autoTestChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Name = "Series1";
            series1.Points.Add(dataPoint1);
            this.autoTestChart.Series.Add(series1);
            this.autoTestChart.Size = new System.Drawing.Size(756, 301);
            this.autoTestChart.TabIndex = 0;
            this.autoTestChart.Text = "chart1";
            // 
            // customPanel1
            // 
            this.customPanel1.BackColor = System.Drawing.Color.Transparent;
            this.customPanel1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel1.Controls.Add(this.testStatusLabel);
            this.customPanel1.Controls.Add(this.label15);
            this.customPanel1.Controls.Add(this.onOffAutoButton);
            this.customPanel1.Controls.Add(this.abortAutoTestButton);
            this.customPanel1.Controls.Add(this.passStatusLabel);
            this.customPanel1.Controls.Add(this.passFailLabel);
            this.customPanel1.Controls.Add(this.label7);
            this.customPanel1.Controls.Add(this.secondsUnitLabel);
            this.customPanel1.Controls.Add(this.testDurationLabel);
            this.customPanel1.Controls.Add(this.label5);
            this.customPanel1.Controls.Add(this.autoTestChart);
            this.customPanel1.CornerRadius = 40;
            this.customPanel1.IsPopUp = false;
            this.customPanel1.Location = new System.Drawing.Point(511, 266);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(886, 505);
            this.customPanel1.TabIndex = 1;
            // 
            // testStatusLabel
            // 
            this.testStatusLabel.AutoSize = true;
            this.testStatusLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testStatusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.testStatusLabel.Location = new System.Drawing.Point(513, 454);
            this.testStatusLabel.Name = "testStatusLabel";
            this.testStatusLabel.Size = new System.Drawing.Size(103, 19);
            this.testStatusLabel.TabIndex = 17;
            this.testStatusLabel.Text = "EVALUATING...\r";
            this.testStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.testStatusLabel.Visible = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label15.Location = new System.Drawing.Point(23, 421);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(124, 26);
            this.label15.TabIndex = 9;
            this.label15.Text = "START/PAUSE";
            // 
            // onOffAutoButton
            // 
            this.onOffAutoButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.onOffAutoButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.offButton;
            this.onOffAutoButton.CheckedHoverImage = global::HV9104_GUI.Properties.Resources.onButtonHover;
            this.onOffAutoButton.CheckedImage = global::HV9104_GUI.Properties.Resources.onButton;
            this.onOffAutoButton.isChecked = false;
            this.onOffAutoButton.Location = new System.Drawing.Point(153, 406);
            this.onOffAutoButton.Name = "onOffAutoButton";
            this.onOffAutoButton.Size = new System.Drawing.Size(138, 56);
            this.onOffAutoButton.TabIndex = 10;
            this.onOffAutoButton.Text = "customCheckBox2";
            this.onOffAutoButton.UncheckedHoverImage = global::HV9104_GUI.Properties.Resources.offButtonHover;
            // 
            // abortAutoTestButton
            // 
            this.abortAutoTestButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.abortAutoTestButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.button;
            this.abortAutoTestButton.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.abortAutoTestButton.ForeColor = System.Drawing.Color.White;
            this.abortAutoTestButton.HoverImage = global::HV9104_GUI.Properties.Resources.buttonHover;
            this.abortAutoTestButton.Location = new System.Drawing.Point(678, 406);
            this.abortAutoTestButton.Name = "abortAutoTestButton";
            this.abortAutoTestButton.PressedImage = global::HV9104_GUI.Properties.Resources.buttonPressed;
            this.abortAutoTestButton.Size = new System.Drawing.Size(158, 57);
            this.abortAutoTestButton.TabIndex = 12;
            this.abortAutoTestButton.Text = "ABORT";
            // 
            // passStatusLabel
            // 
            this.passStatusLabel.AutoSize = true;
            this.passStatusLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passStatusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.passStatusLabel.Location = new System.Drawing.Point(483, 367);
            this.passStatusLabel.Name = "passStatusLabel";
            this.passStatusLabel.Size = new System.Drawing.Size(162, 26);
            this.passStatusLabel.TabIndex = 15;
            this.passStatusLabel.Text = "PASS/FAIL STATUS";
            // 
            // passFailLabel
            // 
            this.passFailLabel.AutoSize = true;
            this.passFailLabel.Font = new System.Drawing.Font("Calibri", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passFailLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.passFailLabel.Location = new System.Drawing.Point(483, 385);
            this.passFailLabel.Name = "passFailLabel";
            this.passFailLabel.Size = new System.Drawing.Size(162, 78);
            this.passFailLabel.TabIndex = 16;
            this.passFailLabel.Text = "PASS";
            this.passFailLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.passFailLabel.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label7.Location = new System.Drawing.Point(321, 367);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 26);
            this.label7.TabIndex = 10;
            this.label7.Text = "ELAPSED TIME";
            // 
            // secondsUnitLabel
            // 
            this.secondsUnitLabel.AutoSize = true;
            this.secondsUnitLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.secondsUnitLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.secondsUnitLabel.Location = new System.Drawing.Point(353, 454);
            this.secondsUnitLabel.Name = "secondsUnitLabel";
            this.secondsUnitLabel.Size = new System.Drawing.Size(71, 19);
            this.secondsUnitLabel.TabIndex = 12;
            this.secondsUnitLabel.Text = "SECONDS";
            // 
            // testDurationLabel
            // 
            this.testDurationLabel.Font = new System.Drawing.Font("Calibri", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testDurationLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.testDurationLabel.Location = new System.Drawing.Point(339, 384);
            this.testDurationLabel.Name = "testDurationLabel";
            this.testDurationLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.testDurationLabel.Size = new System.Drawing.Size(99, 80);
            this.testDurationLabel.TabIndex = 11;
            this.testDurationLabel.Text = "50";
            this.testDurationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label5.Location = new System.Drawing.Point(25, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 18);
            this.label5.TabIndex = 5;
            this.label5.Text = "TEST CONTROL";
            // 
            // createReportButton
            // 
            this.createReportButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.createReportButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.button;
            this.createReportButton.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createReportButton.ForeColor = System.Drawing.Color.White;
            this.createReportButton.HoverImage = global::HV9104_GUI.Properties.Resources.buttonHover;
            this.createReportButton.Location = new System.Drawing.Point(40, 406);
            this.createReportButton.Name = "createReportButton";
            this.createReportButton.PressedImage = global::HV9104_GUI.Properties.Resources.buttonPressed;
            this.createReportButton.Size = new System.Drawing.Size(158, 57);
            this.createReportButton.TabIndex = 14;
            this.createReportButton.Text = "CREATE REPORT";
            // 
            // exportValuesButton
            // 
            this.exportValuesButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.exportValuesButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.button;
            this.exportValuesButton.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportValuesButton.ForeColor = System.Drawing.Color.White;
            this.exportValuesButton.HoverImage = global::HV9104_GUI.Properties.Resources.buttonHover;
            this.exportValuesButton.Location = new System.Drawing.Point(236, 406);
            this.exportValuesButton.Name = "exportValuesButton";
            this.exportValuesButton.PressedImage = global::HV9104_GUI.Properties.Resources.buttonPressed;
            this.exportValuesButton.Size = new System.Drawing.Size(158, 57);
            this.exportValuesButton.TabIndex = 13;
            this.exportValuesButton.Text = "EXPORT VALUES";
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(315, 17);
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
            this.WithstandRadioButton.Location = new System.Drawing.Point(34, 63);
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
            this.DisruptiveRadioButton.Location = new System.Drawing.Point(34, 119);
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
            this.label21.Location = new System.Drawing.Point(87, 72);
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
            this.label20.Location = new System.Drawing.Point(87, 128);
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
            this.customPanel2.Location = new System.Drawing.Point(10, 32);
            this.customPanel2.Name = "customPanel2";
            this.customPanel2.Size = new System.Drawing.Size(465, 200);
            this.customPanel2.TabIndex = 3;
            // 
            // voltageComboBox
            // 
            this.voltageComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.voltageComboBox.Location = new System.Drawing.Point(292, 64);
            this.voltageComboBox.MaximumSize = new System.Drawing.Size(400, 60);
            this.voltageComboBox.MinimumSize = new System.Drawing.Size(120, 45);
            this.voltageComboBox.Name = "voltageComboBox";
            this.voltageComboBox.setCollection = new string[] {
        "AC",
        "DC",
        "Imp"};
            this.voltageComboBox.SetSelected = "AC";
            this.voltageComboBox.Size = new System.Drawing.Size(139, 45);
            this.voltageComboBox.TabIndex = 4;
            this.voltageComboBox.Text = "customComboBox1";
            this.voltageComboBox.TextBoxHint = "";
            // 
            // customPanel3
            // 
            this.customPanel3.BackColor = System.Drawing.Color.Transparent;
            this.customPanel3.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel3.Controls.Add(this.otherTextBox);
            this.customPanel3.Controls.Add(this.otherLabel);
            this.customPanel3.Controls.Add(this.operatorTextBox);
            this.customPanel3.Controls.Add(this.createReportButton);
            this.customPanel3.Controls.Add(this.testObjectTextBox);
            this.customPanel3.Controls.Add(this.exportValuesButton);
            this.customPanel3.Controls.Add(this.operatorLabel);
            this.customPanel3.Controls.Add(this.testObjectLabel);
            this.customPanel3.Controls.Add(this.dateLabel);
            this.customPanel3.Controls.Add(this.dateTextBox);
            this.customPanel3.Controls.Add(this.label1);
            this.customPanel3.CornerRadius = 40;
            this.customPanel3.IsPopUp = false;
            this.customPanel3.Location = new System.Drawing.Point(1427, 266);
            this.customPanel3.Name = "customPanel3";
            this.customPanel3.Size = new System.Drawing.Size(432, 505);
            this.customPanel3.TabIndex = 4;
            // 
            // otherTextBox
            // 
            this.otherTextBox.Location = new System.Drawing.Point(161, 245);
            this.otherTextBox.Multiline = true;
            this.otherTextBox.Name = "otherTextBox";
            this.otherTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.otherTextBox.Size = new System.Drawing.Size(222, 135);
            this.otherTextBox.TabIndex = 16;
            // 
            // otherLabel
            // 
            this.otherLabel.AutoSize = true;
            this.otherLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otherLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.otherLabel.Location = new System.Drawing.Point(35, 245);
            this.otherLabel.Name = "otherLabel";
            this.otherLabel.Size = new System.Drawing.Size(69, 26);
            this.otherLabel.TabIndex = 15;
            this.otherLabel.Text = "OTHER";
            // 
            // operatorTextBox
            // 
            this.operatorTextBox.Location = new System.Drawing.Point(161, 131);
            this.operatorTextBox.Multiline = true;
            this.operatorTextBox.Name = "operatorTextBox";
            this.operatorTextBox.Size = new System.Drawing.Size(222, 26);
            this.operatorTextBox.TabIndex = 14;
            // 
            // testObjectTextBox
            // 
            this.testObjectTextBox.Location = new System.Drawing.Point(161, 177);
            this.testObjectTextBox.Multiline = true;
            this.testObjectTextBox.Name = "testObjectTextBox";
            this.testObjectTextBox.Size = new System.Drawing.Size(222, 48);
            this.testObjectTextBox.TabIndex = 13;
            // 
            // operatorLabel
            // 
            this.operatorLabel.AutoSize = true;
            this.operatorLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.operatorLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.operatorLabel.Location = new System.Drawing.Point(35, 131);
            this.operatorLabel.Name = "operatorLabel";
            this.operatorLabel.Size = new System.Drawing.Size(102, 26);
            this.operatorLabel.TabIndex = 12;
            this.operatorLabel.Text = "OPERATOR";
            // 
            // testObjectLabel
            // 
            this.testObjectLabel.AutoSize = true;
            this.testObjectLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testObjectLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.testObjectLabel.Location = new System.Drawing.Point(35, 177);
            this.testObjectLabel.Name = "testObjectLabel";
            this.testObjectLabel.Size = new System.Drawing.Size(120, 26);
            this.testObjectLabel.TabIndex = 11;
            this.testObjectLabel.Text = "TEST OBJECT";
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.dateLabel.Location = new System.Drawing.Point(35, 85);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(55, 26);
            this.dateLabel.TabIndex = 10;
            this.dateLabel.Text = "DATE";
            // 
            // dateTextBox
            // 
            this.dateTextBox.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTextBox.Location = new System.Drawing.Point(161, 85);
            this.dateTextBox.Multiline = true;
            this.dateTextBox.Name = "dateTextBox";
            this.dateTextBox.Size = new System.Drawing.Size(222, 26);
            this.dateTextBox.TabIndex = 7;
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
            this.customPanel4.Controls.Add(this.testDurationTextBox);
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
            // testDurationTextBox
            // 
            this.testDurationTextBox.AllowDecimals = true;
            this.testDurationTextBox.AllowText = false;
            this.testDurationTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.testDurationTextBox.BackgroundColor = System.Drawing.Color.White;
            this.testDurationTextBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(159)))), ((int)(((byte)(171)))));
            this.testDurationTextBox.CornerRadius = 25;
            this.testDurationTextBox.Decimals = 0;
            this.testDurationTextBox.IsPopUp = false;
            this.testDurationTextBox.Location = new System.Drawing.Point(21, 183);
            this.testDurationTextBox.Max = 1800D;
            this.testDurationTextBox.MaximumSize = new System.Drawing.Size(400, 50);
            this.testDurationTextBox.Min = 5D;
            this.testDurationTextBox.MinimumSize = new System.Drawing.Size(170, 50);
            this.testDurationTextBox.Name = "testDurationTextBox";
            this.testDurationTextBox.Size = new System.Drawing.Size(170, 50);
            this.testDurationTextBox.TabIndex = 4;
            this.testDurationTextBox.TextBoxHint = "";
            this.testDurationTextBox.Value = 60F;
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
            this.customPanel5.Controls.Add(this.testVoltageTextBox);
            this.customPanel5.Controls.Add(this.label3);
            this.customPanel5.Controls.Add(this.pictureBox3);
            this.customPanel5.CornerRadius = 40;
            this.customPanel5.IsPopUp = false;
            this.customPanel5.Location = new System.Drawing.Point(255, 266);
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
            // testVoltageTextBox
            // 
            this.testVoltageTextBox.AllowDecimals = true;
            this.testVoltageTextBox.AllowText = false;
            this.testVoltageTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.testVoltageTextBox.BackgroundColor = System.Drawing.Color.White;
            this.testVoltageTextBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(159)))), ((int)(((byte)(171)))));
            this.testVoltageTextBox.CornerRadius = 25;
            this.testVoltageTextBox.Decimals = 2;
            this.testVoltageTextBox.IsPopUp = false;
            this.testVoltageTextBox.Location = new System.Drawing.Point(24, 183);
            this.testVoltageTextBox.Max = 400D;
            this.testVoltageTextBox.MaximumSize = new System.Drawing.Size(400, 50);
            this.testVoltageTextBox.Min = 5D;
            this.testVoltageTextBox.MinimumSize = new System.Drawing.Size(170, 50);
            this.testVoltageTextBox.Name = "testVoltageTextBox";
            this.testVoltageTextBox.Size = new System.Drawing.Size(170, 50);
            this.testVoltageTextBox.TabIndex = 5;
            this.testVoltageTextBox.TextBoxHint = "";
            this.testVoltageTextBox.Value = 33F;
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
            // customPanel7
            // 
            this.customPanel7.BackColor = System.Drawing.Color.Transparent;
            this.customPanel7.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel7.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel7.Controls.Add(this.impPerLevelLabel);
            this.customPanel7.Controls.Add(this.voltLevelsLabel);
            this.customPanel7.Controls.Add(this.impulsePerLevelComboBox);
            this.customPanel7.Controls.Add(this.voltageLevelsComboBox);
            this.customPanel7.Controls.Add(this.label8);
            this.customPanel7.Controls.Add(this.testButton);
            this.customPanel7.CornerRadius = 40;
            this.customPanel7.IsPopUp = false;
            this.customPanel7.Location = new System.Drawing.Point(8, 542);
            this.customPanel7.Name = "customPanel7";
            this.customPanel7.Size = new System.Drawing.Size(471, 229);
            this.customPanel7.TabIndex = 8;
            // 
            // impPerLevelLabel
            // 
            this.impPerLevelLabel.AutoSize = true;
            this.impPerLevelLabel.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.impPerLevelLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.impPerLevelLabel.Location = new System.Drawing.Point(14, 159);
            this.impPerLevelLabel.Name = "impPerLevelLabel";
            this.impPerLevelLabel.Size = new System.Drawing.Size(188, 29);
            this.impPerLevelLabel.TabIndex = 9;
            this.impPerLevelLabel.Text = "IMPULSES / LEVEL";
            // 
            // voltLevelsLabel
            // 
            this.voltLevelsLabel.AutoSize = true;
            this.voltLevelsLabel.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.voltLevelsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.voltLevelsLabel.Location = new System.Drawing.Point(14, 101);
            this.voltLevelsLabel.Name = "voltLevelsLabel";
            this.voltLevelsLabel.Size = new System.Drawing.Size(241, 29);
            this.voltLevelsLabel.TabIndex = 8;
            this.voltLevelsLabel.Text = "TOTAL VOLTAGE LEVELS";
            // 
            // impulsePerLevelComboBox
            // 
            this.impulsePerLevelComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.impulsePerLevelComboBox.Location = new System.Drawing.Point(266, 151);
            this.impulsePerLevelComboBox.MaximumSize = new System.Drawing.Size(400, 60);
            this.impulsePerLevelComboBox.MinimumSize = new System.Drawing.Size(120, 45);
            this.impulsePerLevelComboBox.Name = "impulsePerLevelComboBox";
            this.impulsePerLevelComboBox.setCollection = new string[] {
        "AC",
        "DC",
        "Impulse"};
            this.impulsePerLevelComboBox.SetSelected = "AC";
            this.impulsePerLevelComboBox.Size = new System.Drawing.Size(184, 45);
            this.impulsePerLevelComboBox.TabIndex = 7;
            this.impulsePerLevelComboBox.Text = "voltageLevelsComboBox";
            this.impulsePerLevelComboBox.TextBoxHint = "";
            // 
            // voltageLevelsComboBox
            // 
            this.voltageLevelsComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.voltageLevelsComboBox.Location = new System.Drawing.Point(266, 93);
            this.voltageLevelsComboBox.MaximumSize = new System.Drawing.Size(400, 60);
            this.voltageLevelsComboBox.MinimumSize = new System.Drawing.Size(120, 45);
            this.voltageLevelsComboBox.Name = "voltageLevelsComboBox";
            this.voltageLevelsComboBox.setCollection = new string[] {
        "AC",
        "DC",
        "Impulse"};
            this.voltageLevelsComboBox.SetSelected = "AC";
            this.voltageLevelsComboBox.Size = new System.Drawing.Size(184, 45);
            this.voltageLevelsComboBox.TabIndex = 6;
            this.voltageLevelsComboBox.Text = "voltageLevelsComboBox";
            this.voltageLevelsComboBox.TextBoxHint = "";
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
            // customPanel8
            // 
            this.customPanel8.BackColor = System.Drawing.Color.Transparent;
            this.customPanel8.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel8.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel8.Controls.Add(this.label9);
            this.customPanel8.Controls.Add(this.impulseOutputComboBox);
            this.customPanel8.Controls.Add(this.impulseTitleLabel);
            this.customPanel8.Controls.Add(this.impulseValueLabel);
            this.customPanel8.CornerRadius = 40;
            this.customPanel8.IsPopUp = false;
            this.customPanel8.Location = new System.Drawing.Point(1414, 32);
            this.customPanel8.Name = "customPanel8";
            this.customPanel8.Size = new System.Drawing.Size(444, 200);
            this.customPanel8.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Calibri", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label9.Location = new System.Drawing.Point(217, 91);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 58);
            this.label9.TabIndex = 6;
            this.label9.Text = "kV";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // impulseOutputComboBox
            // 
            this.impulseOutputComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.impulseOutputComboBox.Location = new System.Drawing.Point(294, 97);
            this.impulseOutputComboBox.MaximumSize = new System.Drawing.Size(400, 60);
            this.impulseOutputComboBox.MinimumSize = new System.Drawing.Size(120, 45);
            this.impulseOutputComboBox.Name = "impulseOutputComboBox";
            this.impulseOutputComboBox.setCollection = new string[] {
        "Pos",
        "Neg"};
            this.impulseOutputComboBox.SetSelected = "Pos";
            this.impulseOutputComboBox.Size = new System.Drawing.Size(140, 45);
            this.impulseOutputComboBox.TabIndex = 3;
            this.impulseOutputComboBox.Text = "customComboBox1";
            this.impulseOutputComboBox.TextBoxHint = "";
            // 
            // impulseTitleLabel
            // 
            this.impulseTitleLabel.AutoSize = true;
            this.impulseTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.impulseTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.impulseTitleLabel.Location = new System.Drawing.Point(20, 14);
            this.impulseTitleLabel.Name = "impulseTitleLabel";
            this.impulseTitleLabel.Size = new System.Drawing.Size(201, 18);
            this.impulseTitleLabel.TabIndex = 2;
            this.impulseTitleLabel.Text = "IMPULSE PEAK OUTPUT";
            // 
            // impulseValueLabel
            // 
            this.impulseValueLabel.Font = new System.Drawing.Font("Calibri", 60F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.impulseValueLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.impulseValueLabel.Location = new System.Drawing.Point(11, 63);
            this.impulseValueLabel.Name = "impulseValueLabel";
            this.impulseValueLabel.Size = new System.Drawing.Size(231, 95);
            this.impulseValueLabel.TabIndex = 1;
            this.impulseValueLabel.Text = "0.0";
            this.impulseValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // customPanel9
            // 
            this.customPanel9.BackColor = System.Drawing.Color.Transparent;
            this.customPanel9.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel9.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel9.Controls.Add(this.label17);
            this.customPanel9.Controls.Add(this.dcOutputComboBox);
            this.customPanel9.Controls.Add(this.dcTitleLabel);
            this.customPanel9.Controls.Add(this.dcValueLabel);
            this.customPanel9.CornerRadius = 40;
            this.customPanel9.IsPopUp = false;
            this.customPanel9.Location = new System.Drawing.Point(953, 32);
            this.customPanel9.Name = "customPanel9";
            this.customPanel9.Size = new System.Drawing.Size(444, 200);
            this.customPanel9.TabIndex = 10;
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("Calibri", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label17.Location = new System.Drawing.Point(217, 91);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(77, 58);
            this.label17.TabIndex = 5;
            this.label17.Text = "kV";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dcOutputComboBox
            // 
            this.dcOutputComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.dcOutputComboBox.Location = new System.Drawing.Point(294, 97);
            this.dcOutputComboBox.MaximumSize = new System.Drawing.Size(400, 60);
            this.dcOutputComboBox.MinimumSize = new System.Drawing.Size(120, 45);
            this.dcOutputComboBox.Name = "dcOutputComboBox";
            this.dcOutputComboBox.setCollection = new string[] {
        "avg",
        "max",
        "min",
        "pk-pk"};
            this.dcOutputComboBox.SetSelected = "avg";
            this.dcOutputComboBox.Size = new System.Drawing.Size(140, 45);
            this.dcOutputComboBox.TabIndex = 3;
            this.dcOutputComboBox.Text = "z";
            this.dcOutputComboBox.TextBoxHint = "";
            // 
            // dcTitleLabel
            // 
            this.dcTitleLabel.AutoSize = true;
            this.dcTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dcTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.dcTitleLabel.Location = new System.Drawing.Point(17, 14);
            this.dcTitleLabel.Name = "dcTitleLabel";
            this.dcTitleLabel.Size = new System.Drawing.Size(105, 18);
            this.dcTitleLabel.TabIndex = 2;
            this.dcTitleLabel.Text = "DC OUTPUT";
            // 
            // dcValueLabel
            // 
            this.dcValueLabel.Font = new System.Drawing.Font("Calibri", 60F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dcValueLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.dcValueLabel.Location = new System.Drawing.Point(8, 62);
            this.dcValueLabel.Name = "dcValueLabel";
            this.dcValueLabel.Size = new System.Drawing.Size(234, 96);
            this.dcValueLabel.TabIndex = 1;
            this.dcValueLabel.Text = "0.0";
            this.dcValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // customPanel10
            // 
            this.customPanel10.BackColor = System.Drawing.Color.Transparent;
            this.customPanel10.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel10.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel10.Controls.Add(this.acOutputComboBox);
            this.customPanel10.Controls.Add(this.label10);
            this.customPanel10.Controls.Add(this.acTitleLabel);
            this.customPanel10.Controls.Add(this.acValueLabel);
            this.customPanel10.CornerRadius = 40;
            this.customPanel10.IsPopUp = false;
            this.customPanel10.Location = new System.Drawing.Point(492, 32);
            this.customPanel10.Name = "customPanel10";
            this.customPanel10.Size = new System.Drawing.Size(444, 200);
            this.customPanel10.TabIndex = 11;
            // 
            // acOutputComboBox
            // 
            this.acOutputComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.acOutputComboBox.Location = new System.Drawing.Point(294, 98);
            this.acOutputComboBox.MaximumSize = new System.Drawing.Size(400, 60);
            this.acOutputComboBox.MinimumSize = new System.Drawing.Size(120, 45);
            this.acOutputComboBox.Name = "acOutputComboBox";
            this.acOutputComboBox.setCollection = new string[] {
        "rms",
        "max",
        "min",
        "pk-pk"};
            this.acOutputComboBox.SetSelected = "rms";
            this.acOutputComboBox.Size = new System.Drawing.Size(140, 45);
            this.acOutputComboBox.TabIndex = 3;
            this.acOutputComboBox.Text = "customComboBox1";
            this.acOutputComboBox.TextBoxHint = "";
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Calibri", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label10.Location = new System.Drawing.Point(217, 91);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 58);
            this.label10.TabIndex = 4;
            this.label10.Text = "kV";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // acTitleLabel
            // 
            this.acTitleLabel.AutoSize = true;
            this.acTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.acTitleLabel.Location = new System.Drawing.Point(16, 14);
            this.acTitleLabel.Name = "acTitleLabel";
            this.acTitleLabel.Size = new System.Drawing.Size(103, 18);
            this.acTitleLabel.TabIndex = 2;
            this.acTitleLabel.Text = "AC OUTPUT";
            // 
            // acValueLabel
            // 
            this.acValueLabel.Font = new System.Drawing.Font("Calibri", 60F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acValueLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.acValueLabel.Location = new System.Drawing.Point(6, 55);
            this.acValueLabel.Margin = new System.Windows.Forms.Padding(0);
            this.acValueLabel.Name = "acValueLabel";
            this.acValueLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.acValueLabel.Size = new System.Drawing.Size(236, 111);
            this.acValueLabel.TabIndex = 1;
            this.acValueLabel.Text = "0.0";
            this.acValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // RunView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.customPanel8);
            this.Controls.Add(this.customPanel9);
            this.Controls.Add(this.customPanel10);
            this.Controls.Add(this.customPanel7);
            this.Controls.Add(this.customPanel5);
            this.Controls.Add(this.customPanel4);
            this.Controls.Add(this.customPanel3);
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
            this.customPanel4.ResumeLayout(false);
            this.customPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.customPanel5.ResumeLayout(false);
            this.customPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.customPanel7.ResumeLayout(false);
            this.customPanel7.PerformLayout();
            this.customPanel8.ResumeLayout(false);
            this.customPanel8.PerformLayout();
            this.customPanel9.ResumeLayout(false);
            this.customPanel9.PerformLayout();
            this.customPanel10.ResumeLayout(false);
            this.customPanel10.PerformLayout();
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
        private System.Windows.Forms.TextBox dateTextBox;
        public System.Windows.Forms.Label label1;
        private CustomPanel customPanel4;
        private CustomPanel customPanel5;
        private CustomTextBox testDurationTextBox;
        public System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private CustomTextBox testVoltageTextBox;
        public System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox3;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label label16;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label label7;
        public CustomButton abortAutoTestButton;
        public CustomCheckBox onOffAutoButton;
        public System.Windows.Forms.Label label15;
        public System.Windows.Forms.Label testDurationLabel;
        private CustomPanel customPanel7;
        public System.Windows.Forms.Label label8;
        public System.Windows.Forms.Label secondsUnitLabel;
        public CustomButton createReportButton;
        public CustomButton exportValuesButton;
        public System.Windows.Forms.Label impPerLevelLabel;
        public System.Windows.Forms.Label voltLevelsLabel;
        public CustomComboBox impulsePerLevelComboBox;
        public CustomComboBox voltageLevelsComboBox;
        public CustomPanel customPanel8;
        public System.Windows.Forms.Label label9;
        public CustomComboBox impulseOutputComboBox;
        public System.Windows.Forms.Label impulseTitleLabel;
        public System.Windows.Forms.Label impulseValueLabel;
        public CustomPanel customPanel9;
        public System.Windows.Forms.Label label17;
        public CustomComboBox dcOutputComboBox;
        public System.Windows.Forms.Label dcTitleLabel;
        public System.Windows.Forms.Label dcValueLabel;
        public CustomPanel customPanel10;
        public CustomComboBox acOutputComboBox;
        public System.Windows.Forms.Label label10;
        public System.Windows.Forms.Label acTitleLabel;
        public System.Windows.Forms.Label acValueLabel;
        private System.Windows.Forms.TextBox testObjectTextBox;
        public System.Windows.Forms.Label operatorLabel;
        public System.Windows.Forms.Label testObjectLabel;
        public System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.TextBox operatorTextBox;
        public System.Windows.Forms.Label testStatusLabel;
        public System.Windows.Forms.Label passStatusLabel;
        public System.Windows.Forms.Label passFailLabel;
        private System.Windows.Forms.TextBox otherTextBox;
        public System.Windows.Forms.Label otherLabel;
    }
}
