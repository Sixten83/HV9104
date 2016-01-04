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
    public class IncrementWindow : Form
    {
        private CustomButton closeButton;
        private Label titleLabel;
        private CustomButton increaseButton;
        private CustomButton decreaseButton;
        private CustomTextBox triggerLevelTextBox;
        private CustomButton cancelButton;
        public CustomButton okButton;
        private CustomPanel customPanel1;
        private int formX, formY;
        private IContainer components;
        private bool moveForm;

        public IncrementWindow()
        {

            InitializeComponent();
            addHandlers(customPanel1);
            addHandlers(titleLabel);
        }

        private void addHandlers(Control control)
        {
            control.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleBarPanel_MouseDown);
            control.MouseUp += new System.Windows.Forms.MouseEventHandler(this.titleBarPanel_MouseUp);
            control.MouseMove += new System.Windows.Forms.MouseEventHandler(this.titleBarPanel_MouseMove);
        }
        private void titleBarPanel_MouseDown(object sender, MouseEventArgs e)
        {

            if (sender == customPanel1 && e.Y < customPanel1.CornerRadius * 2)
            {
                formX = e.X;
                formY = e.Y;
                moveForm = true;
            }
            else if (e.Y < customPanel1.CornerRadius * 2)
            {
                Control temp = (Control)sender;
                formX = e.X + temp.Location.X;
                formY = e.Y + temp.Location.Y;
                moveForm = true;
            }

        }
        private void titleBarPanel_MouseUp(object sender, MouseEventArgs e)
        {
            moveForm = false;

        }
        private void titleBarPanel_MouseMove(object sender, MouseEventArgs e)
        {

            if (moveForm)
                this.SetDesktopLocation(MousePosition.X - formX, MousePosition.Y - formY);
        }

        private void InitializeComponent()
        {
            this.customPanel1 = new HV9104_GUI.CustomPanel();
            this.increaseButton = new HV9104_GUI.CustomButton();
            this.decreaseButton = new HV9104_GUI.CustomButton();
            this.triggerLevelTextBox = new HV9104_GUI.CustomTextBox();
            this.cancelButton = new HV9104_GUI.CustomButton();
            this.okButton = new HV9104_GUI.CustomButton();
            this.titleLabel = new System.Windows.Forms.Label();
            this.closeButton = new HV9104_GUI.CustomButton();
            this.customPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // customPanel1
            // 
            this.customPanel1.BackColor = System.Drawing.Color.Transparent;
            this.customPanel1.BackgroundColor = System.Drawing.Color.White;
            this.customPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel1.Controls.Add(this.increaseButton);
            this.customPanel1.Controls.Add(this.decreaseButton);
            this.customPanel1.Controls.Add(this.triggerLevelTextBox);
            this.customPanel1.Controls.Add(this.cancelButton);
            this.customPanel1.Controls.Add(this.okButton);
            this.customPanel1.Controls.Add(this.titleLabel);
            this.customPanel1.Controls.Add(this.closeButton);
            this.customPanel1.CornerRadius = 30;
            this.customPanel1.IsPopUp = true;
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(500, 260);
            this.customPanel1.TabIndex = 0;
            // 
            // increaseButton
            // 
            this.increaseButton.BackColor = System.Drawing.Color.White;
            this.increaseButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.upButton;
            this.increaseButton.HoverImage = global::HV9104_GUI.Properties.Resources.upButton;
            this.increaseButton.Location = new System.Drawing.Point(364, 97);
            this.increaseButton.Name = "increaseButton";
            this.increaseButton.PressedImage = global::HV9104_GUI.Properties.Resources.upButtonPressed;
            this.increaseButton.Size = new System.Drawing.Size(61, 61);
            this.increaseButton.TabIndex = 10;
            // 
            // decreaseButton
            // 
            this.decreaseButton.BackColor = System.Drawing.Color.White;
            this.decreaseButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.downButton;
            this.decreaseButton.HoverImage = global::HV9104_GUI.Properties.Resources.downButtonHover;
            this.decreaseButton.Location = new System.Drawing.Point(75, 97);
            this.decreaseButton.Name = "decreaseButton";
            this.decreaseButton.PressedImage = global::HV9104_GUI.Properties.Resources.downButtonPressed;
            this.decreaseButton.Size = new System.Drawing.Size(61, 61);
            this.decreaseButton.TabIndex = 11;
            // 
            // triggerLevelTextBox
            // 
            this.triggerLevelTextBox.BackColor = System.Drawing.Color.White;
            this.triggerLevelTextBox.BackgroundColor = System.Drawing.Color.White;
            this.triggerLevelTextBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(159)))), ((int)(((byte)(171)))));
            this.triggerLevelTextBox.CornerRadius = 27;
            this.triggerLevelTextBox.Decimals = 2;
            this.triggerLevelTextBox.IsPopUp = false;
            this.triggerLevelTextBox.Location = new System.Drawing.Point(165, 101);
            this.triggerLevelTextBox.Max = 230;
            this.triggerLevelTextBox.Min = 0;
            this.triggerLevelTextBox.Name = "triggerLevelTextBox";
            this.triggerLevelTextBox.Size = new System.Drawing.Size(169, 54);
            this.triggerLevelTextBox.TabIndex = 9;
            this.triggerLevelTextBox.TextBoxHint = "";
            this.triggerLevelTextBox.Value = 200F;
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.White;
            this.cancelButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.button;
            this.cancelButton.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.HoverImage = global::HV9104_GUI.Properties.Resources.buttonHover;
            this.cancelButton.Location = new System.Drawing.Point(244, 176);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.PressedImage = global::HV9104_GUI.Properties.Resources.buttonPressed;
            this.cancelButton.Size = new System.Drawing.Size(158, 57);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "CANCEL";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.BackColor = System.Drawing.Color.White;
            this.okButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.button;
            this.okButton.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okButton.ForeColor = System.Drawing.Color.White;
            this.okButton.HoverImage = global::HV9104_GUI.Properties.Resources.buttonHover;
            this.okButton.Location = new System.Drawing.Point(65, 176);
            this.okButton.Name = "okButton";
            this.okButton.PressedImage = global::HV9104_GUI.Properties.Resources.buttonPressed;
            this.okButton.Size = new System.Drawing.Size(158, 57);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "OK";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.titleLabel.Location = new System.Drawing.Point(25, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(230, 18);
            this.titleLabel.TabIndex = 6;
            this.titleLabel.Text = "INCREMENT SETUP (V/step)";
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.closeButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.closeButton;
            this.closeButton.HoverImage = global::HV9104_GUI.Properties.Resources.closeButtonHover;
            this.closeButton.Location = new System.Drawing.Point(456, 25);
            this.closeButton.Name = "closeButton";
            this.closeButton.PressedImage = global::HV9104_GUI.Properties.Resources.closeButtonPressed;
            this.closeButton.Size = new System.Drawing.Size(13, 13);
            this.closeButton.TabIndex = 5;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // IncrementWindow
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(165)))));
            this.ClientSize = new System.Drawing.Size(500, 260);
            this.ControlBox = false;
            this.Controls.Add(this.customPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "IncrementWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(165)))));
            this.customPanel1.ResumeLayout(false);
            this.customPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }


    }
}
