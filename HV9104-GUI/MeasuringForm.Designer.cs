namespace HV9104_GUI
{
    partial class MeasuringForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel1 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(-25D, -32512D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(-25D, 32512D);
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint3 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(25D, -32512D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint4 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(25D, 32512D);
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint5 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(-25D, -32512D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint6 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(25D, -32512D);
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint7 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(-25D, 32512D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint8 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(25D, 32512D);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeasuringForm));
            this.titleBarPanel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.minimizeButton = new HV9104_GUI.CustomButton();
            this.maximizeButton = new HV9104_GUI.CustomButton();
            this.restoreDownButton = new HV9104_GUI.CustomButton();
            this.closeButton = new HV9104_GUI.CustomButton();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.acChannelPanel = new HV9104_GUI.CustomPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.acEnableCheckBox = new HV9104_GUI.CustomCheckBox();
            this.acVoltageRangeComboBox = new HV9104_GUI.CustomComboBox();
            this.dcChannelPanel = new HV9104_GUI.CustomPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.dcEnableCheckBox = new HV9104_GUI.CustomCheckBox();
            this.dcVoltageRangeComboBox = new HV9104_GUI.CustomComboBox();
            this.impulseChannelPanel = new HV9104_GUI.CustomPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.impulseEnableCheckBox = new HV9104_GUI.CustomCheckBox();
            this.impulseVoltageRangeComboBox = new HV9104_GUI.CustomComboBox();
            this.customPanel5 = new HV9104_GUI.CustomPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.triggerSetupButton = new HV9104_GUI.CustomButton();
            this.timeBaseComboBox = new HV9104_GUI.CustomComboBox();
            this.resolutionComboBox = new HV9104_GUI.CustomComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.acdcRadioButton = new HV9104_GUI.CustomRadioButton();
            this.impulseRadioButton = new HV9104_GUI.CustomRadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.customPanel1 = new HV9104_GUI.CustomPanel();
            this.chart = new HV9104_GUI.CustomChart();
            this.customPanel2 = new HV9104_GUI.CustomPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.acTitleLabel = new System.Windows.Forms.Label();
            this.titleBarPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.acChannelPanel.SuspendLayout();
            this.dcChannelPanel.SuspendLayout();
            this.impulseChannelPanel.SuspendLayout();
            this.customPanel5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.customPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.customPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleBarPanel
            // 
            this.titleBarPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.titleBarPanel.Controls.Add(this.pictureBox1);
            this.titleBarPanel.Controls.Add(this.minimizeButton);
            this.titleBarPanel.Controls.Add(this.maximizeButton);
            this.titleBarPanel.Controls.Add(this.restoreDownButton);
            this.titleBarPanel.Controls.Add(this.closeButton);
            this.titleBarPanel.Controls.Add(this.panel6);
            this.titleBarPanel.Controls.Add(this.panel2);
            this.titleBarPanel.Location = new System.Drawing.Point(1, 1);
            this.titleBarPanel.Name = "titleBarPanel";
            this.titleBarPanel.Size = new System.Drawing.Size(1918, 100);
            this.titleBarPanel.TabIndex = 13;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::HV9104_GUI.Properties.Resources.tercoLogo;
            this.pictureBox1.Location = new System.Drawing.Point(1840, 34);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(68, 56);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // minimizeButton
            // 
            this.minimizeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.minimizeButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.minimizeButton;
            this.minimizeButton.ForeColor = System.Drawing.Color.White;
            this.minimizeButton.HoverImage = global::HV9104_GUI.Properties.Resources.minimizeButtonHover;
            this.minimizeButton.Location = new System.Drawing.Point(1843, 10);
            this.minimizeButton.Name = "minimizeButton";
            this.minimizeButton.PressedImage = global::HV9104_GUI.Properties.Resources.minimizeButtonPressed;
            this.minimizeButton.Size = new System.Drawing.Size(13, 13);
            this.minimizeButton.TabIndex = 4;
            this.toolTip1.SetToolTip(this.minimizeButton, "Minimize");
            this.minimizeButton.Click += new System.EventHandler(this.minimizeButton_Click);
            // 
            // maximizeButton
            // 
            this.maximizeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.maximizeButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.maximizeButton;
            this.maximizeButton.ForeColor = System.Drawing.Color.White;
            this.maximizeButton.HoverImage = global::HV9104_GUI.Properties.Resources.maximizeButtonHover;
            this.maximizeButton.Location = new System.Drawing.Point(1869, 10);
            this.maximizeButton.Name = "maximizeButton";
            this.maximizeButton.PressedImage = global::HV9104_GUI.Properties.Resources.maximizeButtonPressed;
            this.maximizeButton.Size = new System.Drawing.Size(13, 13);
            this.maximizeButton.TabIndex = 4;
            this.toolTip1.SetToolTip(this.maximizeButton, "Maximize");
            this.maximizeButton.Visible = false;
            this.maximizeButton.Click += new System.EventHandler(this.maximizeButton_Click);
            // 
            // restoreDownButton
            // 
            this.restoreDownButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.restoreDownButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.restoreDownButton;
            this.restoreDownButton.ForeColor = System.Drawing.Color.White;
            this.restoreDownButton.HoverImage = global::HV9104_GUI.Properties.Resources.restoreDownButtonHover;
            this.restoreDownButton.Location = new System.Drawing.Point(1869, 10);
            this.restoreDownButton.Name = "restoreDownButton";
            this.restoreDownButton.PressedImage = global::HV9104_GUI.Properties.Resources.restoreDownButtonPressed;
            this.restoreDownButton.Size = new System.Drawing.Size(13, 13);
            this.restoreDownButton.TabIndex = 4;
            this.toolTip1.SetToolTip(this.restoreDownButton, "Restore down");
            this.restoreDownButton.Click += new System.EventHandler(this.restoreDownButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.closeButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.closeButton;
            this.closeButton.ForeColor = System.Drawing.Color.White;
            this.closeButton.HoverImage = global::HV9104_GUI.Properties.Resources.closeButtonHover;
            this.closeButton.Location = new System.Drawing.Point(1895, 10);
            this.closeButton.Name = "closeButton";
            this.closeButton.PressedImage = global::HV9104_GUI.Properties.Resources.closeButtonPressed;
            this.closeButton.Size = new System.Drawing.Size(13, 13);
            this.closeButton.TabIndex = 4;
            this.toolTip1.SetToolTip(this.closeButton, "Close");
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.panel6.Location = new System.Drawing.Point(1800, 10);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(2, 80);
            this.panel6.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.panel2.Location = new System.Drawing.Point(0, 98);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1920, 2);
            this.panel2.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.acChannelPanel);
            this.flowLayoutPanel1.Controls.Add(this.dcChannelPanel);
            this.flowLayoutPanel1.Controls.Add(this.impulseChannelPanel);
            this.flowLayoutPanel1.Controls.Add(this.customPanel5);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(1601, 170);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(317, 786);
            this.flowLayoutPanel1.TabIndex = 21;
            // 
            // acChannelPanel
            // 
            this.acChannelPanel.BackColor = System.Drawing.Color.Transparent;
            this.acChannelPanel.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.acChannelPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.acChannelPanel.Controls.Add(this.label10);
            this.acChannelPanel.Controls.Add(this.acEnableCheckBox);
            this.acChannelPanel.Controls.Add(this.acVoltageRangeComboBox);
            this.acChannelPanel.CornerRadius = 20;
            this.acChannelPanel.IsPopUp = false;
            this.acChannelPanel.Location = new System.Drawing.Point(3, 3);
            this.acChannelPanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.acChannelPanel.Name = "acChannelPanel";
            this.acChannelPanel.Size = new System.Drawing.Size(302, 142);
            this.acChannelPanel.TabIndex = 20;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label10.Location = new System.Drawing.Point(22, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(113, 18);
            this.label10.TabIndex = 17;
            this.label10.Text = "AC CHANNEL";
            // 
            // acEnableCheckBox
            // 
            this.acEnableCheckBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.acEnableCheckBox.BackgroundImage = global::HV9104_GUI.Properties.Resources.checkBox;
            this.acEnableCheckBox.CheckedHoverImage = global::HV9104_GUI.Properties.Resources.checkBoxCheckedHover;
            this.acEnableCheckBox.CheckedImage = global::HV9104_GUI.Properties.Resources.checkBoxChecked;
            this.acEnableCheckBox.isChecked = false;
            this.acEnableCheckBox.Location = new System.Drawing.Point(242, 62);
            this.acEnableCheckBox.Name = "acEnableCheckBox";
            this.acEnableCheckBox.Size = new System.Drawing.Size(48, 43);
            this.acEnableCheckBox.TabIndex = 19;
            this.acEnableCheckBox.Text = "acCheckBox";
            this.toolTip1.SetToolTip(this.acEnableCheckBox, "Enable ac channel");
            this.acEnableCheckBox.UncheckedHoverImage = global::HV9104_GUI.Properties.Resources.checkBoxHover;
            // 
            // acVoltageRangeComboBox
            // 
            this.acVoltageRangeComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.acVoltageRangeComboBox.BackgroundImage = global::HV9104_GUI.Properties.Resources.listButton;
            this.acVoltageRangeComboBox.HoverImage = global::HV9104_GUI.Properties.Resources.listButtonHover;
            this.acVoltageRangeComboBox.Location = new System.Drawing.Point(13, 51);
            this.acVoltageRangeComboBox.Name = "acVoltageRangeComboBox";
            this.acVoltageRangeComboBox.setCollection = new string[] {
        "0.2 kV/div",
        "0.5 kV/div",
        "1 kV/div",
        "2 kV/div",
        "5 kV/div",
        "10 kV/div",
        "20 kV/div"};
            this.acVoltageRangeComboBox.SetSelected = "20 kV/div";
            this.acVoltageRangeComboBox.Size = new System.Drawing.Size(209, 67);
            this.acVoltageRangeComboBox.TabIndex = 18;
            this.acVoltageRangeComboBox.TextBoxHint = "";
            // 
            // dcChannelPanel
            // 
            this.dcChannelPanel.BackColor = System.Drawing.Color.Transparent;
            this.dcChannelPanel.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.dcChannelPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.dcChannelPanel.Controls.Add(this.label1);
            this.dcChannelPanel.Controls.Add(this.dcEnableCheckBox);
            this.dcChannelPanel.Controls.Add(this.dcVoltageRangeComboBox);
            this.dcChannelPanel.CornerRadius = 20;
            this.dcChannelPanel.IsPopUp = false;
            this.dcChannelPanel.Location = new System.Drawing.Point(3, 158);
            this.dcChannelPanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.dcChannelPanel.Name = "dcChannelPanel";
            this.dcChannelPanel.Size = new System.Drawing.Size(302, 142);
            this.dcChannelPanel.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label1.Location = new System.Drawing.Point(22, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 18);
            this.label1.TabIndex = 17;
            this.label1.Text = "DC CHANNEL";
            // 
            // dcEnableCheckBox
            // 
            this.dcEnableCheckBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.dcEnableCheckBox.BackgroundImage = global::HV9104_GUI.Properties.Resources.checkBox;
            this.dcEnableCheckBox.CheckedHoverImage = global::HV9104_GUI.Properties.Resources.checkBoxCheckedHover;
            this.dcEnableCheckBox.CheckedImage = global::HV9104_GUI.Properties.Resources.checkBoxChecked;
            this.dcEnableCheckBox.isChecked = false;
            this.dcEnableCheckBox.Location = new System.Drawing.Point(242, 62);
            this.dcEnableCheckBox.Name = "dcEnableCheckBox";
            this.dcEnableCheckBox.Size = new System.Drawing.Size(48, 43);
            this.dcEnableCheckBox.TabIndex = 19;
            this.dcEnableCheckBox.Text = "customCheckBox1";
            this.toolTip1.SetToolTip(this.dcEnableCheckBox, "Enable dc channel");
            this.dcEnableCheckBox.UncheckedHoverImage = global::HV9104_GUI.Properties.Resources.checkBoxHover;
            // 
            // dcVoltageRangeComboBox
            // 
            this.dcVoltageRangeComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.dcVoltageRangeComboBox.BackgroundImage = global::HV9104_GUI.Properties.Resources.listButton;
            this.dcVoltageRangeComboBox.HoverImage = global::HV9104_GUI.Properties.Resources.listButtonHover;
            this.dcVoltageRangeComboBox.Location = new System.Drawing.Point(13, 51);
            this.dcVoltageRangeComboBox.Name = "dcVoltageRangeComboBox";
            this.dcVoltageRangeComboBox.setCollection = new string[] {
        "0.2 kV/div",
        "0.5 kV/div",
        "1 kV/div",
        "2 kV/div",
        "5 kV/div",
        "10 kV/div",
        "20 kV/div"};
            this.dcVoltageRangeComboBox.SetSelected = "20 kV/div";
            this.dcVoltageRangeComboBox.Size = new System.Drawing.Size(209, 67);
            this.dcVoltageRangeComboBox.TabIndex = 18;
            this.dcVoltageRangeComboBox.Text = "z";
            this.dcVoltageRangeComboBox.TextBoxHint = "";
            // 
            // impulseChannelPanel
            // 
            this.impulseChannelPanel.BackColor = System.Drawing.Color.Transparent;
            this.impulseChannelPanel.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.impulseChannelPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.impulseChannelPanel.Controls.Add(this.label2);
            this.impulseChannelPanel.Controls.Add(this.impulseEnableCheckBox);
            this.impulseChannelPanel.Controls.Add(this.impulseVoltageRangeComboBox);
            this.impulseChannelPanel.CornerRadius = 20;
            this.impulseChannelPanel.IsPopUp = false;
            this.impulseChannelPanel.Location = new System.Drawing.Point(3, 313);
            this.impulseChannelPanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.impulseChannelPanel.Name = "impulseChannelPanel";
            this.impulseChannelPanel.Size = new System.Drawing.Size(302, 142);
            this.impulseChannelPanel.TabIndex = 20;
            this.impulseChannelPanel.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label2.Location = new System.Drawing.Point(22, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 18);
            this.label2.TabIndex = 17;
            this.label2.Text = "IMPULSE CHANNEL";
            // 
            // impulseEnableCheckBox
            // 
            this.impulseEnableCheckBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.impulseEnableCheckBox.BackgroundImage = global::HV9104_GUI.Properties.Resources.checkBox;
            this.impulseEnableCheckBox.CheckedHoverImage = global::HV9104_GUI.Properties.Resources.checkBoxCheckedHover;
            this.impulseEnableCheckBox.CheckedImage = global::HV9104_GUI.Properties.Resources.checkBoxChecked;
            this.impulseEnableCheckBox.isChecked = false;
            this.impulseEnableCheckBox.Location = new System.Drawing.Point(242, 62);
            this.impulseEnableCheckBox.Name = "impulseEnableCheckBox";
            this.impulseEnableCheckBox.Size = new System.Drawing.Size(48, 43);
            this.impulseEnableCheckBox.TabIndex = 19;
            this.impulseEnableCheckBox.Text = "customCheckBox1";
            this.toolTip1.SetToolTip(this.impulseEnableCheckBox, "Enable impulse channel");
            this.impulseEnableCheckBox.UncheckedHoverImage = global::HV9104_GUI.Properties.Resources.checkBoxHover;
            // 
            // impulseVoltageRangeComboBox
            // 
            this.impulseVoltageRangeComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.impulseVoltageRangeComboBox.BackgroundImage = global::HV9104_GUI.Properties.Resources.listButton;
            this.impulseVoltageRangeComboBox.HoverImage = global::HV9104_GUI.Properties.Resources.listButtonHover;
            this.impulseVoltageRangeComboBox.Location = new System.Drawing.Point(13, 51);
            this.impulseVoltageRangeComboBox.Name = "impulseVoltageRangeComboBox";
            this.impulseVoltageRangeComboBox.setCollection = new string[] {
        "0.2 kV/div",
        "0.5 kV/div",
        "1 kV/div",
        "2 kV/div",
        "5 kV/div",
        "10 kV/div",
        "20 kV/div"};
            this.impulseVoltageRangeComboBox.SetSelected = "20 kV/div";
            this.impulseVoltageRangeComboBox.Size = new System.Drawing.Size(209, 67);
            this.impulseVoltageRangeComboBox.TabIndex = 18;
            this.impulseVoltageRangeComboBox.Text = "z";
            this.impulseVoltageRangeComboBox.TextBoxHint = "";
            // 
            // customPanel5
            // 
            this.customPanel5.BackColor = System.Drawing.Color.Transparent;
            this.customPanel5.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel5.Controls.Add(this.label6);
            this.customPanel5.Controls.Add(this.triggerSetupButton);
            this.customPanel5.Controls.Add(this.timeBaseComboBox);
            this.customPanel5.Controls.Add(this.resolutionComboBox);
            this.customPanel5.CornerRadius = 20;
            this.customPanel5.IsPopUp = false;
            this.customPanel5.Location = new System.Drawing.Point(3, 468);
            this.customPanel5.Name = "customPanel5";
            this.customPanel5.Size = new System.Drawing.Size(302, 297);
            this.customPanel5.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label6.Location = new System.Drawing.Point(22, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(183, 18);
            this.label6.TabIndex = 17;
            this.label6.Text = "COMMON CONTROLS";
            // 
            // triggerSetupButton
            // 
            this.triggerSetupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.triggerSetupButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.button;
            this.triggerSetupButton.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.triggerSetupButton.ForeColor = System.Drawing.Color.White;
            this.triggerSetupButton.HoverImage = global::HV9104_GUI.Properties.Resources.buttonHover;
            this.triggerSetupButton.Location = new System.Drawing.Point(66, 227);
            this.triggerSetupButton.Name = "triggerSetupButton";
            this.triggerSetupButton.PressedImage = global::HV9104_GUI.Properties.Resources.buttonPressed;
            this.triggerSetupButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.triggerSetupButton.Size = new System.Drawing.Size(158, 57);
            this.triggerSetupButton.TabIndex = 17;
            this.triggerSetupButton.Text = "SETUP";
            this.toolTip1.SetToolTip(this.triggerSetupButton, "Configure triggering setup");
            this.triggerSetupButton.Click += new System.EventHandler(this.triggerSetupButton_Click);
            // 
            // timeBaseComboBox
            // 
            this.timeBaseComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.timeBaseComboBox.BackgroundImage = global::HV9104_GUI.Properties.Resources.listButton;
            this.timeBaseComboBox.HoverImage = global::HV9104_GUI.Properties.Resources.listButtonHover;
            this.timeBaseComboBox.Location = new System.Drawing.Point(35, 136);
            this.timeBaseComboBox.Name = "timeBaseComboBox";
            this.timeBaseComboBox.setCollection = new string[] {
        "2 ms/Div",
        "5 ms/Div",
        "10 ms/Div"};
            this.timeBaseComboBox.SetSelected = "5 ms/Div";
            this.timeBaseComboBox.Size = new System.Drawing.Size(209, 67);
            this.timeBaseComboBox.TabIndex = 18;
            this.timeBaseComboBox.Text = "z";
            this.timeBaseComboBox.TextBoxHint = "";
            // 
            // resolutionComboBox
            // 
            this.resolutionComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.resolutionComboBox.BackgroundImage = global::HV9104_GUI.Properties.Resources.listButton;
            this.resolutionComboBox.HoverImage = global::HV9104_GUI.Properties.Resources.listButtonHover;
            this.resolutionComboBox.Location = new System.Drawing.Point(35, 53);
            this.resolutionComboBox.Name = "resolutionComboBox";
            this.resolutionComboBox.setCollection = new string[] {
        "8 Bit",
        "12 Bit"};
            this.resolutionComboBox.SetSelected = "12 Bit";
            this.resolutionComboBox.Size = new System.Drawing.Size(209, 67);
            this.resolutionComboBox.TabIndex = 18;
            this.resolutionComboBox.Text = "z";
            this.resolutionComboBox.TextBoxHint = "";
            // 
            // acdcRadioButton
            // 
            this.acdcRadioButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.acdcRadioButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.radioButton;
            this.acdcRadioButton.CheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonCheckedHover;
            this.acdcRadioButton.CheckedImage = global::HV9104_GUI.Properties.Resources.radioButtonChecked;
            this.acdcRadioButton.isChecked = true;
            this.acdcRadioButton.Location = new System.Drawing.Point(60, 79);
            this.acdcRadioButton.Name = "acdcRadioButton";
            this.acdcRadioButton.Size = new System.Drawing.Size(47, 47);
            this.acdcRadioButton.TabIndex = 15;
            this.toolTip1.SetToolTip(this.acdcRadioButton, "Use ac and dc channels as input");
            this.acdcRadioButton.UncheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonHover;
            this.acdcRadioButton.Click += new System.EventHandler(this.acdcRadioButton_Click);
            // 
            // impulseRadioButton
            // 
            this.impulseRadioButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.impulseRadioButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.radioButton;
            this.impulseRadioButton.CheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonCheckedHover;
            this.impulseRadioButton.CheckedImage = global::HV9104_GUI.Properties.Resources.radioButtonChecked;
            this.impulseRadioButton.isChecked = false;
            this.impulseRadioButton.Location = new System.Drawing.Point(175, 79);
            this.impulseRadioButton.Name = "impulseRadioButton";
            this.impulseRadioButton.Size = new System.Drawing.Size(47, 47);
            this.impulseRadioButton.TabIndex = 16;
            this.toolTip1.SetToolTip(this.impulseRadioButton, "Use impulse channel as input");
            this.impulseRadioButton.UncheckedHoverImage = global::HV9104_GUI.Properties.Resources.radioButtonHover;
            this.impulseRadioButton.Click += new System.EventHandler(this.impulseRadioButton_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Controls.Add(this.customPanel1);
            this.panel1.Controls.Add(this.customPanel2);
            this.panel1.Location = new System.Drawing.Point(1, 101);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1918, 978);
            this.panel1.TabIndex = 22;
            // 
            // customPanel1
            // 
            this.customPanel1.BackColor = System.Drawing.Color.Transparent;
            this.customPanel1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel1.Controls.Add(this.chart);
            this.customPanel1.CornerRadius = 40;
            this.customPanel1.IsPopUp = false;
            this.customPanel1.Location = new System.Drawing.Point(21, 16);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(1555, 940);
            this.customPanel1.TabIndex = 14;
            // 
            // chart
            // 
            this.chart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            chartArea1.AxisX.Interval = 5D;
            chartArea1.AxisX.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisX.Maximum = 25D;
            chartArea1.AxisX.Minimum = -25D;
            chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisY.CustomLabels.Add(customLabel1);
            chartArea1.AxisY.Interval = 6502.4D;
            chartArea1.AxisY.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisY.MajorTickMark.Enabled = false;
            chartArea1.AxisY.Maximum = 32512D;
            chartArea1.AxisY.Minimum = -32512D;
            chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            chartArea1.BorderColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Location = new System.Drawing.Point(20, 20);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Color = System.Drawing.Color.Lime;
            series1.IsVisibleInLegend = false;
            series1.Name = "acSeries";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Color = System.Drawing.Color.Red;
            series2.IsVisibleInLegend = false;
            series2.Name = "dcSeries";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series3.Color = System.Drawing.Color.RoyalBlue;
            series3.IsVisibleInLegend = false;
            series3.Name = "impulseSeries";
            series4.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            series4.BorderWidth = 2;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Color = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(159)))), ((int)(((byte)(171)))));
            series4.IsVisibleInLegend = false;
            series4.Name = "xCursor1";
            series4.Points.Add(dataPoint1);
            series4.Points.Add(dataPoint2);
            series5.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            series5.BorderWidth = 2;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Color = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(159)))), ((int)(((byte)(171)))));
            series5.IsVisibleInLegend = false;
            series5.Name = "xCursor2";
            series5.Points.Add(dataPoint3);
            series5.Points.Add(dataPoint4);
            series6.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            series6.BorderWidth = 2;
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Color = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(113)))), ((int)(((byte)(113)))));
            series6.IsVisibleInLegend = false;
            series6.Name = "yCursor1";
            series6.Points.Add(dataPoint5);
            series6.Points.Add(dataPoint6);
            series7.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            series7.BorderWidth = 2;
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Color = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(113)))), ((int)(((byte)(113)))));
            series7.IsVisibleInLegend = false;
            series7.Name = "yCursor2";
            series7.Points.Add(dataPoint7);
            series7.Points.Add(dataPoint8);
            this.chart.Series.Add(series1);
            this.chart.Series.Add(series2);
            this.chart.Series.Add(series3);
            this.chart.Series.Add(series4);
            this.chart.Series.Add(series5);
            this.chart.Series.Add(series6);
            this.chart.Series.Add(series7);
            this.chart.Size = new System.Drawing.Size(1515, 900);
            this.chart.TabIndex = 0;
            this.chart.Text = "chart1";
            // 
            // customPanel2
            // 
            this.customPanel2.BackColor = System.Drawing.Color.Transparent;
            this.customPanel2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel2.Controls.Add(this.label3);
            this.customPanel2.Controls.Add(this.label7);
            this.customPanel2.Controls.Add(this.acTitleLabel);
            this.customPanel2.Controls.Add(this.acdcRadioButton);
            this.customPanel2.Controls.Add(this.impulseRadioButton);
            this.customPanel2.CornerRadius = 20;
            this.customPanel2.IsPopUp = false;
            this.customPanel2.Location = new System.Drawing.Point(1598, 18);
            this.customPanel2.Name = "customPanel2";
            this.customPanel2.Size = new System.Drawing.Size(302, 142);
            this.customPanel2.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label3.Location = new System.Drawing.Point(161, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 29);
            this.label3.TabIndex = 18;
            this.label3.Text = "IMPULSE";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label7.Location = new System.Drawing.Point(45, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 29);
            this.label7.TabIndex = 18;
            this.label7.Text = "AC/DC";
            // 
            // acTitleLabel
            // 
            this.acTitleLabel.AutoSize = true;
            this.acTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.acTitleLabel.Location = new System.Drawing.Point(22, 17);
            this.acTitleLabel.Name = "acTitleLabel";
            this.acTitleLabel.Size = new System.Drawing.Size(57, 18);
            this.acTitleLabel.TabIndex = 17;
            this.acTitleLabel.Text = "INPUT";
            // 
            // MeasuringForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.titleBarPanel);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MeasuringForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MeasuringForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.titleBarPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.acChannelPanel.ResumeLayout(false);
            this.acChannelPanel.PerformLayout();
            this.dcChannelPanel.ResumeLayout(false);
            this.dcChannelPanel.PerformLayout();
            this.impulseChannelPanel.ResumeLayout(false);
            this.impulseChannelPanel.PerformLayout();
            this.customPanel5.ResumeLayout(false);
            this.customPanel5.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.customPanel1.ResumeLayout(false);
            this.customPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.customPanel2.ResumeLayout(false);
            this.customPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel titleBarPanel;
        public System.Windows.Forms.PictureBox pictureBox1;
        public CustomButton minimizeButton;
        public CustomButton maximizeButton;
        public CustomButton restoreDownButton;
        public CustomButton closeButton;
        public System.Windows.Forms.Panel panel6;
        public System.Windows.Forms.Panel panel2;
        public CustomPanel customPanel1;
        public CustomRadioButton acdcRadioButton;
        public CustomRadioButton impulseRadioButton;
        public CustomButton triggerSetupButton;
        public CustomPanel customPanel2;
        public System.Windows.Forms.Label acTitleLabel;
        public System.Windows.Forms.Label label7;
        public CustomPanel customPanel5;
        public System.Windows.Forms.Label label6;
        public CustomComboBox timeBaseComboBox;
        public CustomComboBox resolutionComboBox;
        public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        public CustomPanel acChannelPanel;
        public System.Windows.Forms.Label label10;
        public CustomCheckBox acEnableCheckBox;
        public CustomComboBox acVoltageRangeComboBox;
        public System.Windows.Forms.Label label3;
        public CustomPanel dcChannelPanel;
        public System.Windows.Forms.Label label1;
        public CustomCheckBox dcEnableCheckBox;
        public CustomComboBox dcVoltageRangeComboBox;
        public CustomPanel impulseChannelPanel;
        public System.Windows.Forms.Label label2;
        public CustomCheckBox impulseEnableCheckBox;
        public CustomComboBox impulseVoltageRangeComboBox;
        public System.Windows.Forms.ToolTip toolTip1;
        public CustomChart chart;
        private System.Windows.Forms.Panel panel1;
    }
}