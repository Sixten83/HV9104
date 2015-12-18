using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Data;


namespace HV9104_GUI
{
    class CustomCursorMenu : Form
    {
        private CustomPanel customPanel1;
        private CustomPanel customPanel3;
        private CustomPanel customPanel7;
        private CustomPanel customPanel6;
        private CustomPanel customPanel4;
        private CustomPanel customPanel5;
        private CustomPanel customPanel2;
        private Label label3;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label2;
        private Label label1;
        private int moveForm, formX, formY;
        private Panel topBorderPanel;
        public CustomButton closeButton;
        private Label hCurs1Label;
        private Label vCurs1Label;
        private Label vCurs2Label;
        private Label vDiffLabel;
        private Label hCurs2Label;
        private Label hDiffLabel;
            CustomPanel list;

            public CustomCursorMenu()
            {
               /* AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                ClientSize = new System.Drawing.Size(209, 287);
                StartPosition = FormStartPosition.Manual;
                //ControlBox = false;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                ShowInTaskbar = false;
                TopMost = true;
                list = new CustomPanel();
                list.Size = this.Size;
                list.cornerRadius = 12;
                list.backgroundColor = System.Drawing.Color.FromArgb(236, 236, 236);
                list.borderColor = System.Drawing.Color.FromArgb(166,166,166);
                this.Controls.Add(list);*/
                StartPosition = FormStartPosition.Manual;
                InitializeComponent();
                topBorderPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.topBorderPanel_MouseDown);
                topBorderPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.topBorderPanel_MouseUp);
                topBorderPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.topBorderPanel_MouseMove);
            }

           /* protected override CreateParams CreateParams
            {
                get
                {
                    const int CS_DROPSHADOW = 0x20000;
                    CreateParams cp = base.CreateParams;
                    cp.ClassStyle |= CS_DROPSHADOW;
                    return cp;
                }
            }*/

            public void updateCursorPos(double hCurs1, double hCurs2, double vCurs1, double vCurs2)
            {
                hCurs1Label.Text = "" + hCurs1;
                hCurs2Label.Text = "" + hCurs2;
                vCurs1Label.Text = "" + vCurs1;
                vCurs2Label.Text = "" + vCurs2;
                hDiffLabel.Text = "" + (hCurs1 - hCurs2);
                vDiffLabel.Text = "" + (vCurs1 - vCurs2);
                //Point startPoint = this.Owner.PointToScreen(new Point());
                if (this.Owner != null)
                {
                    Form test = this.Owner;
                    Point startPoint = test.PointToScreen(new Point());
                    
                }
            }

        private void topBorderPanel_MouseDown(object sender, MouseEventArgs e)
        { 
            moveForm = 1; 
            formX = e.X; 
            formY = e.Y;
            
        }
        private void topBorderPanel_MouseUp(object sender, MouseEventArgs e)
        { 
            moveForm = 0; 
        }
        private void topBorderPanel_MouseMove(object sender, MouseEventArgs e)
        {
            //Constrain in form??
            /*if (this.Owner != null)
            {
                Form owner = this.Owner;
                Point ownerLocation = owner.PointToScreen(new Point());
                int x = this.PointToScreen(new Point()).X;
                int y = this.PointToScreen(new Point()).Y;
                Console.WriteLine(" X:" + x + " ownerLocation.X" + ownerLocation.X);

                if (moveForm == 1 && (x > ownerLocation.X))
                {
                    this.SetDesktopLocation(MousePosition.X - formX, MousePosition.Y - formY); 
                }
            }*/

            if (moveForm == 1)
                 this.SetDesktopLocation(MousePosition.X - formX, MousePosition.Y - formY); 
        }

            private void InitializeComponent()
            {
            this.customPanel1 = new HV9104_GUI.CustomPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.customPanel7 = new HV9104_GUI.CustomPanel();
            this.vCurs1Label = new System.Windows.Forms.Label();
            this.customPanel6 = new HV9104_GUI.CustomPanel();
            this.vCurs2Label = new System.Windows.Forms.Label();
            this.customPanel4 = new HV9104_GUI.CustomPanel();
            this.hCurs1Label = new System.Windows.Forms.Label();
            this.customPanel5 = new HV9104_GUI.CustomPanel();
            this.vDiffLabel = new System.Windows.Forms.Label();
            this.customPanel2 = new HV9104_GUI.CustomPanel();
            this.hCurs2Label = new System.Windows.Forms.Label();
            this.customPanel3 = new HV9104_GUI.CustomPanel();
            this.hDiffLabel = new System.Windows.Forms.Label();
            this.topBorderPanel = new System.Windows.Forms.Panel();
            this.closeButton = new HV9104_GUI.CustomButton();
            this.customPanel1.SuspendLayout();
            this.customPanel7.SuspendLayout();
            this.customPanel6.SuspendLayout();
            this.customPanel4.SuspendLayout();
            this.customPanel5.SuspendLayout();
            this.customPanel2.SuspendLayout();
            this.customPanel3.SuspendLayout();
            this.topBorderPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // customPanel1
            // 
            this.customPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(165)))));
            this.customPanel1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel1.Controls.Add(this.label3);
            this.customPanel1.Controls.Add(this.label6);
            this.customPanel1.Controls.Add(this.label5);
            this.customPanel1.Controls.Add(this.label4);
            this.customPanel1.Controls.Add(this.label2);
            this.customPanel1.Controls.Add(this.label1);
            this.customPanel1.Controls.Add(this.customPanel7);
            this.customPanel1.Controls.Add(this.customPanel6);
            this.customPanel1.Controls.Add(this.customPanel4);
            this.customPanel1.Controls.Add(this.customPanel5);
            this.customPanel1.Controls.Add(this.customPanel2);
            this.customPanel1.Controls.Add(this.customPanel3);
            this.customPanel1.Controls.Add(this.topBorderPanel);
            this.customPanel1.CornerRadius = 20;
            this.customPanel1.IsPopUp = false;
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(600, 240);
            this.customPanel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.label3.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label3.Location = new System.Drawing.Point(21, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "VERTICAL";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.label6.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label6.Location = new System.Drawing.Point(472, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 23);
            this.label6.TabIndex = 2;
            this.label6.Text = "DIFF Δ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.label5.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label5.Location = new System.Drawing.Point(308, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 23);
            this.label5.TabIndex = 2;
            this.label5.Text = "CURSOR 2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.label4.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label4.Location = new System.Drawing.Point(170, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 23);
            this.label4.TabIndex = 2;
            this.label4.Text = "CURSOR 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.label2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label2.Location = new System.Drawing.Point(21, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "HORIZONTAL";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.label1.Location = new System.Drawing.Point(33, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "CURSORS";
            // 
            // customPanel7
            // 
            this.customPanel7.BackColor = System.Drawing.Color.Transparent;
            this.customPanel7.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(113)))), ((int)(((byte)(113)))));
            this.customPanel7.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(113)))), ((int)(((byte)(113)))));
            this.customPanel7.Controls.Add(this.vCurs1Label);
            this.customPanel7.CornerRadius = 27;
            this.customPanel7.IsPopUp = false;
            this.customPanel7.Location = new System.Drawing.Point(154, 152);
            this.customPanel7.Name = "customPanel7";
            this.customPanel7.Size = new System.Drawing.Size(120, 55);
            this.customPanel7.TabIndex = 1;
            // 
            // vCurs1Label
            // 
            this.vCurs1Label.BackColor = System.Drawing.Color.Transparent;
            this.vCurs1Label.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vCurs1Label.ForeColor = System.Drawing.Color.White;
            this.vCurs1Label.Location = new System.Drawing.Point(22, 3);
            this.vCurs1Label.Name = "vCurs1Label";
            this.vCurs1Label.Size = new System.Drawing.Size(72, 50);
            this.vCurs1Label.TabIndex = 2;
            this.vCurs1Label.Text = "121";
            this.vCurs1Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // customPanel6
            // 
            this.customPanel6.BackColor = System.Drawing.Color.Transparent;
            this.customPanel6.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(113)))), ((int)(((byte)(113)))));
            this.customPanel6.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(113)))), ((int)(((byte)(113)))));
            this.customPanel6.Controls.Add(this.vCurs2Label);
            this.customPanel6.CornerRadius = 27;
            this.customPanel6.IsPopUp = false;
            this.customPanel6.Location = new System.Drawing.Point(297, 152);
            this.customPanel6.Name = "customPanel6";
            this.customPanel6.Size = new System.Drawing.Size(120, 55);
            this.customPanel6.TabIndex = 1;
            // 
            // vCurs2Label
            // 
            this.vCurs2Label.BackColor = System.Drawing.Color.Transparent;
            this.vCurs2Label.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vCurs2Label.ForeColor = System.Drawing.Color.White;
            this.vCurs2Label.Location = new System.Drawing.Point(19, 3);
            this.vCurs2Label.Name = "vCurs2Label";
            this.vCurs2Label.Size = new System.Drawing.Size(72, 50);
            this.vCurs2Label.TabIndex = 2;
            this.vCurs2Label.Text = "121";
            this.vCurs2Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // customPanel4
            // 
            this.customPanel4.BackColor = System.Drawing.Color.Transparent;
            this.customPanel4.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            this.customPanel4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            this.customPanel4.Controls.Add(this.hCurs1Label);
            this.customPanel4.CornerRadius = 27;
            this.customPanel4.IsPopUp = false;
            this.customPanel4.Location = new System.Drawing.Point(154, 80);
            this.customPanel4.Name = "customPanel4";
            this.customPanel4.Size = new System.Drawing.Size(120, 55);
            this.customPanel4.TabIndex = 1;
            // 
            // hCurs1Label
            // 
            this.hCurs1Label.BackColor = System.Drawing.Color.Transparent;
            this.hCurs1Label.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hCurs1Label.ForeColor = System.Drawing.Color.White;
            this.hCurs1Label.Location = new System.Drawing.Point(22, 3);
            this.hCurs1Label.Name = "hCurs1Label";
            this.hCurs1Label.Size = new System.Drawing.Size(72, 50);
            this.hCurs1Label.TabIndex = 2;
            this.hCurs1Label.Text = "121";
            this.hCurs1Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // customPanel5
            // 
            this.customPanel5.BackColor = System.Drawing.Color.Transparent;
            this.customPanel5.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(113)))), ((int)(((byte)(113)))));
            this.customPanel5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(113)))), ((int)(((byte)(113)))));
            this.customPanel5.Controls.Add(this.vDiffLabel);
            this.customPanel5.CornerRadius = 27;
            this.customPanel5.IsPopUp = false;
            this.customPanel5.Location = new System.Drawing.Point(442, 152);
            this.customPanel5.Name = "customPanel5";
            this.customPanel5.Size = new System.Drawing.Size(120, 55);
            this.customPanel5.TabIndex = 1;
            // 
            // vDiffLabel
            // 
            this.vDiffLabel.BackColor = System.Drawing.Color.Transparent;
            this.vDiffLabel.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vDiffLabel.ForeColor = System.Drawing.Color.White;
            this.vDiffLabel.Location = new System.Drawing.Point(27, 3);
            this.vDiffLabel.Name = "vDiffLabel";
            this.vDiffLabel.Size = new System.Drawing.Size(72, 50);
            this.vDiffLabel.TabIndex = 2;
            this.vDiffLabel.Text = "121";
            this.vDiffLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // customPanel2
            // 
            this.customPanel2.BackColor = System.Drawing.Color.Transparent;
            this.customPanel2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            this.customPanel2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            this.customPanel2.Controls.Add(this.hCurs2Label);
            this.customPanel2.CornerRadius = 27;
            this.customPanel2.IsPopUp = false;
            this.customPanel2.Location = new System.Drawing.Point(297, 80);
            this.customPanel2.Name = "customPanel2";
            this.customPanel2.Size = new System.Drawing.Size(120, 55);
            this.customPanel2.TabIndex = 1;
            // 
            // hCurs2Label
            // 
            this.hCurs2Label.BackColor = System.Drawing.Color.Transparent;
            this.hCurs2Label.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hCurs2Label.ForeColor = System.Drawing.Color.White;
            this.hCurs2Label.Location = new System.Drawing.Point(19, 3);
            this.hCurs2Label.Name = "hCurs2Label";
            this.hCurs2Label.Size = new System.Drawing.Size(72, 50);
            this.hCurs2Label.TabIndex = 2;
            this.hCurs2Label.Text = "121";
            this.hCurs2Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // customPanel3
            // 
            this.customPanel3.BackColor = System.Drawing.Color.Transparent;
            this.customPanel3.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            this.customPanel3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            this.customPanel3.Controls.Add(this.hDiffLabel);
            this.customPanel3.CornerRadius = 27;
            this.customPanel3.IsPopUp = false;
            this.customPanel3.Location = new System.Drawing.Point(442, 80);
            this.customPanel3.Name = "customPanel3";
            this.customPanel3.Size = new System.Drawing.Size(120, 55);
            this.customPanel3.TabIndex = 1;
            // 
            // hDiffLabel
            // 
            this.hDiffLabel.BackColor = System.Drawing.Color.Transparent;
            this.hDiffLabel.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hDiffLabel.ForeColor = System.Drawing.Color.White;
            this.hDiffLabel.Location = new System.Drawing.Point(27, 3);
            this.hDiffLabel.Name = "hDiffLabel";
            this.hDiffLabel.Size = new System.Drawing.Size(72, 50);
            this.hDiffLabel.TabIndex = 2;
            this.hDiffLabel.Text = "121";
            this.hDiffLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // topBorderPanel
            // 
            this.topBorderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.topBorderPanel.Controls.Add(this.closeButton);
            this.topBorderPanel.Location = new System.Drawing.Point(15, 3);
            this.topBorderPanel.Name = "topBorderPanel";
            this.topBorderPanel.Size = new System.Drawing.Size(570, 40);
            this.topBorderPanel.TabIndex = 3;
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.closeButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.closeButton;
            this.closeButton.HoverImage = global::HV9104_GUI.Properties.Resources.closeButtonHover;
            this.closeButton.Location = new System.Drawing.Point(556, 9);
            this.closeButton.Name = "closeButton";
            this.closeButton.PressedImage = global::HV9104_GUI.Properties.Resources.closeButtonPressed;
            this.closeButton.Size = new System.Drawing.Size(15, 15);
            this.closeButton.TabIndex = 0;
            // 
            // CustomCursorMenu
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(165)))));
            this.ClientSize = new System.Drawing.Size(600, 240);
            this.ControlBox = false;
            this.Controls.Add(this.customPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomCursorMenu";
            this.ShowInTaskbar = false;
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(165)))));
            this.customPanel1.ResumeLayout(false);
            this.customPanel1.PerformLayout();
            this.customPanel7.ResumeLayout(false);
            this.customPanel7.PerformLayout();
            this.customPanel6.ResumeLayout(false);
            this.customPanel6.PerformLayout();
            this.customPanel4.ResumeLayout(false);
            this.customPanel4.PerformLayout();
            this.customPanel5.ResumeLayout(false);
            this.customPanel5.PerformLayout();
            this.customPanel2.ResumeLayout(false);
            this.customPanel2.PerformLayout();
            this.customPanel3.ResumeLayout(false);
            this.customPanel3.PerformLayout();
            this.topBorderPanel.ResumeLayout(false);
            this.ResumeLayout(false);

            }
    
    }
}
