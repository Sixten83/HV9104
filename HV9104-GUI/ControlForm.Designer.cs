namespace HV9104_GUI
{
    partial class ControlForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlForm));
            this.titleBarPanel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.minimizeButton = new HV9104_GUI.CustomButton();
            this.maximizeButton = new HV9104_GUI.CustomButton();
            this.restoreDownButton = new HV9104_GUI.CustomButton();
            this.closeButton = new HV9104_GUI.CustomButton();
            this.userTitleLabel = new System.Windows.Forms.Label();
            this.modeLabel = new System.Windows.Forms.Label();
            this.modeTitleLabel = new System.Windows.Forms.Label();
            this.messageTitleLabel = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.controlFormTabController = new HV9104_GUI.CustomTabControl();
            this.customTab22 = new HV9104_GUI.CustomTab();
            this.customTab21 = new HV9104_GUI.CustomTab();
            this.titleBarPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.controlFormTabController.SuspendLayout();
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
            this.titleBarPanel.Controls.Add(this.userTitleLabel);
            this.titleBarPanel.Controls.Add(this.modeLabel);
            this.titleBarPanel.Controls.Add(this.modeTitleLabel);
            this.titleBarPanel.Controls.Add(this.messageTitleLabel);
            this.titleBarPanel.Controls.Add(this.panel6);
            this.titleBarPanel.Controls.Add(this.panel5);
            this.titleBarPanel.Controls.Add(this.panel4);
            this.titleBarPanel.Controls.Add(this.panel3);
            this.titleBarPanel.Controls.Add(this.panel2);
            this.titleBarPanel.Location = new System.Drawing.Point(1, 1);
            this.titleBarPanel.Name = "titleBarPanel";
            this.titleBarPanel.Size = new System.Drawing.Size(1918, 100);
            this.titleBarPanel.TabIndex = 12;
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
            this.minimizeButton.HoverImage = global::HV9104_GUI.Properties.Resources.minimizeButtonHover;
            this.minimizeButton.Location = new System.Drawing.Point(1843, 10);
            this.minimizeButton.Name = "minimizeButton";
            this.minimizeButton.PressedImage = global::HV9104_GUI.Properties.Resources.minimizeButtonPressed;
            this.minimizeButton.Size = new System.Drawing.Size(13, 13);
            this.minimizeButton.TabIndex = 4;
            this.minimizeButton.Click += new System.EventHandler(this.minimizeButton_Click);
            // 
            // maximizeButton
            // 
            this.maximizeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.maximizeButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.maximizeButton;
            this.maximizeButton.HoverImage = global::HV9104_GUI.Properties.Resources.maximizeButtonHover;
            this.maximizeButton.Location = new System.Drawing.Point(1869, 10);
            this.maximizeButton.Name = "maximizeButton";
            this.maximizeButton.PressedImage = global::HV9104_GUI.Properties.Resources.maximizeButtonPressed;
            this.maximizeButton.Size = new System.Drawing.Size(13, 13);
            this.maximizeButton.TabIndex = 4;
            this.maximizeButton.Visible = false;
            this.maximizeButton.Click += new System.EventHandler(this.maximizeButton_Click);
            // 
            // restoreDownButton
            // 
            this.restoreDownButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.restoreDownButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.restoreDownButton;
            this.restoreDownButton.HoverImage = global::HV9104_GUI.Properties.Resources.restoreDownButtonHover;
            this.restoreDownButton.Location = new System.Drawing.Point(1869, 10);
            this.restoreDownButton.Name = "restoreDownButton";
            this.restoreDownButton.PressedImage = global::HV9104_GUI.Properties.Resources.restoreDownButtonPressed;
            this.restoreDownButton.Size = new System.Drawing.Size(13, 13);
            this.restoreDownButton.TabIndex = 4;
            this.restoreDownButton.Click += new System.EventHandler(this.restoreDownButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.closeButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.closeButton;
            this.closeButton.HoverImage = global::HV9104_GUI.Properties.Resources.closeButtonHover;
            this.closeButton.Location = new System.Drawing.Point(1895, 10);
            this.closeButton.Name = "closeButton";
            this.closeButton.PressedImage = global::HV9104_GUI.Properties.Resources.closeButtonPressed;
            this.closeButton.Size = new System.Drawing.Size(13, 13);
            this.closeButton.TabIndex = 4;
            // 
            // userTitleLabel
            // 
            this.userTitleLabel.AutoSize = true;
            this.userTitleLabel.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.userTitleLabel.Location = new System.Drawing.Point(1445, 13);
            this.userTitleLabel.Name = "userTitleLabel";
            this.userTitleLabel.Size = new System.Drawing.Size(51, 23);
            this.userTitleLabel.TabIndex = 3;
            this.userTitleLabel.Text = "USER";
            // 
            // modeLabel
            // 
            this.modeLabel.AutoSize = true;
            this.modeLabel.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.modeLabel.Location = new System.Drawing.Point(881, 46);
            this.modeLabel.Name = "modeLabel";
            this.modeLabel.Size = new System.Drawing.Size(70, 23);
            this.modeLabel.TabIndex = 3;
            this.modeLabel.Text = "Manual";
            // 
            // modeTitleLabel
            // 
            this.modeTitleLabel.AutoSize = true;
            this.modeTitleLabel.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modeTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.modeTitleLabel.Location = new System.Drawing.Point(881, 13);
            this.modeTitleLabel.Name = "modeTitleLabel";
            this.modeTitleLabel.Size = new System.Drawing.Size(61, 23);
            this.modeTitleLabel.TabIndex = 3;
            this.modeTitleLabel.Text = "MODE";
            // 
            // messageTitleLabel
            // 
            this.messageTitleLabel.AutoSize = true;
            this.messageTitleLabel.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.messageTitleLabel.Location = new System.Drawing.Point(317, 13);
            this.messageTitleLabel.Name = "messageTitleLabel";
            this.messageTitleLabel.Size = new System.Drawing.Size(146, 23);
            this.messageTitleLabel.TabIndex = 3;
            this.messageTitleLabel.Text = "LATEST MESSAGE";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.panel6.Location = new System.Drawing.Point(1800, 10);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(2, 80);
            this.panel6.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.panel5.Location = new System.Drawing.Point(1400, 10);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(2, 80);
            this.panel5.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.panel4.Location = new System.Drawing.Point(840, 10);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(2, 80);
            this.panel4.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.panel3.Location = new System.Drawing.Point(280, 10);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(2, 80);
            this.panel3.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.panel2.Location = new System.Drawing.Point(0, 98);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1920, 2);
            this.panel2.TabIndex = 0;
            // 
            // controlFormTabController
            // 
            this.controlFormTabController.BackColor = System.Drawing.Color.White;
            this.controlFormTabController.Controls.Add(this.customTab22);
            this.controlFormTabController.Controls.Add(this.customTab21);
            this.controlFormTabController.Location = new System.Drawing.Point(1, 100);
            this.controlFormTabController.Name = "controlFormTabController";
            this.controlFormTabController.Size = new System.Drawing.Size(1918, 978);
            this.controlFormTabController.TabIndex = 11;
            // 
            // customTab22
            // 
            this.customTab22.BackColor = System.Drawing.Color.White;
            this.customTab22.BackgroundImage = global::HV9104_GUI.Properties.Resources.tabBorder;
            this.customTab22.isSelected = true;
            this.customTab22.Location = new System.Drawing.Point(400, 35);
            this.customTab22.Name = "customTab22";
            this.customTab22.SelectedIcon = global::HV9104_GUI.Properties.Resources.playSelectedIcon;
            this.customTab22.Size = new System.Drawing.Size(400, 100);
            this.customTab22.TabIndex = 0;
            this.customTab22.Text = "Run Experiment";
            this.customTab22.UnselectedIcon = global::HV9104_GUI.Properties.Resources.playIcon;
            // 
            // customTab21
            // 
            this.customTab21.BackColor = System.Drawing.Color.White;
            this.customTab21.BackgroundImage = global::HV9104_GUI.Properties.Resources.tabBorder;
            this.customTab21.isSelected = false;
            this.customTab21.Location = new System.Drawing.Point(0, 35);
            this.customTab21.Name = "customTab21";
            this.customTab21.SelectedIcon = global::HV9104_GUI.Properties.Resources.setupSelectedIcon;
            this.customTab21.Size = new System.Drawing.Size(400, 100);
            this.customTab21.TabIndex = 0;
            this.customTab21.Text = "Setup";
            this.customTab21.UnselectedIcon = global::HV9104_GUI.Properties.Resources.setupIcon;
            // 
            // ControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.titleBarPanel);
            this.Controls.Add(this.controlFormTabController);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ControlForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ControlForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.titleBarPanel.ResumeLayout(false);
            this.titleBarPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.controlFormTabController.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public CustomTabControl controlFormTabController;
        public CustomTab customTab22;
        public CustomTab customTab21;
        public System.Windows.Forms.Panel titleBarPanel;
        public System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.Panel panel4;
        public System.Windows.Forms.Panel panel5;
        public System.Windows.Forms.Panel panel6;
        public System.Windows.Forms.Label messageTitleLabel;
        public System.Windows.Forms.Label modeTitleLabel;
        public System.Windows.Forms.Label userTitleLabel;
        public CustomButton closeButton;
        public CustomButton minimizeButton;
        public CustomButton restoreDownButton;
        public CustomButton maximizeButton;
        public System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Label modeLabel;

    }
}