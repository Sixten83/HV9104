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
    public class CustomPopUp : Form
    {
        private Label titleLabel;
        public CustomButton okButton;
        private CustomPanel customPanel1;
        private int formX, formY;
        private IContainer components;
        private Label textLabel;
        private bool moveForm;

        public CustomPopUp()
        {

            InitializeComponent();
            addHandlers(customPanel1);
            addHandlers(titleLabel);
        }

        public string InfoText
        {
            set
            {
                this.textLabel.Text = value;
            }
            get
            {
                return this.textLabel.Text;
            }
        }

        public string TitleText
        {
            set
            {
                this.titleLabel.Text = value;
            }
            get
            {
                return this.titleLabel.Text;
            }
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
            this.okButton = new HV9104_GUI.CustomButton();
            this.textLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.customPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // customPanel1
            // 
            this.customPanel1.BackColor = System.Drawing.Color.Transparent;
            this.customPanel1.BackgroundColor = System.Drawing.Color.White;
            this.customPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.customPanel1.Controls.Add(this.okButton);
            this.customPanel1.Controls.Add(this.textLabel);
            this.customPanel1.Controls.Add(this.titleLabel);
            this.customPanel1.CornerRadius = 30;
            this.customPanel1.IsPopUp = true;
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(250, 225);
            this.customPanel1.TabIndex = 0;
            // 
            // okButton
            // 
            this.okButton.BackColor = System.Drawing.Color.White;
            this.okButton.BackgroundImage = global::HV9104_GUI.Properties.Resources.button;
            this.okButton.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okButton.ForeColor = System.Drawing.Color.White;
            this.okButton.HoverImage = global::HV9104_GUI.Properties.Resources.buttonHover;
            this.okButton.Location = new System.Drawing.Point(39, 141);
            this.okButton.Name = "okButton";
            this.okButton.PressedImage = global::HV9104_GUI.Properties.Resources.buttonPressed;
            this.okButton.Size = new System.Drawing.Size(158, 57);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // textLabel
            // 
            this.textLabel.AutoSize = true;
            this.textLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.textLabel.Location = new System.Drawing.Point(23, 95);
            this.textLabel.Name = "textLabel";
            this.textLabel.Size = new System.Drawing.Size(202, 26);
            this.textLabel.TabIndex = 6;
            this.textLabel.Text = "No data to export!";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.titleLabel.Location = new System.Drawing.Point(25, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(103, 18);
            this.titleLabel.TabIndex = 6;
            this.titleLabel.Text = "Export Chart";
            // 
            // CustomPopUp
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(165)))));
            this.ClientSize = new System.Drawing.Size(250, 225);
            this.ControlBox = false;
            this.Controls.Add(this.customPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomPopUp";
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

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}