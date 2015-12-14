using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HV9104_GUI
{
    public partial class MeasuringForm : Form
    {

        private int formX, formY;
        private bool moveForm;
        public TriggerWindow triggerWindow;

        public MeasuringForm()
        {
            InitializeComponent();
           // this.Location = Screen.AllScreens[0].WorkingArea.Location;


            triggerWindow = new TriggerWindow();

            titleBarPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleBarPanel_MouseDown);
            titleBarPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.titleBarPanel_MouseUp);
            titleBarPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.titleBarPanel_MouseMove);
            titleBarPanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.titleBarPanel_MouseDoubleClick);

            foreach (Control c in titleBarPanel.Controls)
            {
                if (c.GetType() != typeof(CustomButton))
                {
                    c.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleBarPanel_MouseDown);
                    c.MouseUp += new System.Windows.Forms.MouseEventHandler(this.titleBarPanel_MouseUp);
                    c.MouseMove += new System.Windows.Forms.MouseEventHandler(this.titleBarPanel_MouseMove);
                    c.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.titleBarPanel_MouseDoubleClick);
                }

            }
        }

        private void titleBarPanel_MouseDown(object sender, MouseEventArgs e)
        {
            moveForm = true;
            if (sender == titleBarPanel)
            {
                formX = e.X;
                formY = e.Y;
            }
            else
            {
                Control temp = (Control)sender;
                formX = e.X + temp.Location.X;
                formY = e.Y + temp.Location.Y;
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

        private void titleBarPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {

                this.WindowState = FormWindowState.Maximized;
                this.maximizeButton.Visible = false;
                this.restoreDownButton.Visible = true;
            }
            else
            {

                Point lastLocation = this.Location;
                this.WindowState = FormWindowState.Normal;
                this.Location = new Point(lastLocation.X + 20, lastLocation.Y + 20);
                this.restoreDownButton.Visible = false;
                this.maximizeButton.Visible = true;
            }
        }

        private void minimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void maximizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.maximizeButton.Visible = false;
            this.restoreDownButton.Visible = true;
        }

        private void restoreDownButton_Click(object sender, EventArgs e)
        {
            Point lastLocation = this.Location;
            Console.WriteLine(this.Location);
            this.WindowState = FormWindowState.Normal;
            this.Location = new Point(lastLocation.X + 20, lastLocation.Y + 20);
            this.restoreDownButton.Visible = false;
            this.maximizeButton.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TriggerWindow popUp = new TriggerWindow();
            popUp.Owner = this;
            popUp.Show();
        }

        private void acdcRadioButton_Click(object sender, EventArgs e)
        {
            if(acdcRadioButton.isChecked)
            {
                acChannelPanel.Visible = true;
                dcChannelPanel.Visible = true;
                impulseChannelPanel.Visible = false;
            }
        }

        private void impulseRadioButton_Click(object sender, EventArgs e)
        {
            if (impulseRadioButton.isChecked)
            {
                acChannelPanel.Visible = false;
                dcChannelPanel.Visible = false;
                impulseChannelPanel.Visible = true;
            }
        }

        private void triggerSetupButton_Click(object sender, EventArgs e)
        {
            this.triggerWindow.Show();
        }

        

      
    }
}
