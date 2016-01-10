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
    public partial class ControlForm : Form
    {
        public MeasuringForm measuringForm;
        public DashBoardView dashboardView;
        public SetupView setupView;
        public RunView runView;
        int formX, formY;
        bool moveForm;
        private int cnt;

        public ControlForm()
        {
            InitializeComponent();
            if(Screen.AllScreens.Length <= 2)
                this.Location = Screen.AllScreens[1].WorkingArea.Location;
           
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
            this.controlFormTabController.addPage(runView = new RunView(), 1);
            this.controlFormTabController.addPage(dashboardView  = new DashBoardView(), 0);
            this.controlFormTabController.addPage(setupView = new SetupView(), 2);
        }

        //Creates a shadow around the form
        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
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
            moveForm = false;
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


        public void startMeasuringForm(MeasuringForm measuringForm)
        {
            this.measuringForm = measuringForm;
            measuringForm.Show();
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

        private void controlFormTabController_Paint(object sender, PaintEventArgs e)
        {

        }


        
        // Dashboard selected
        private void customTab22_Click(object sender, EventArgs e)
        {
           
        }

        // Setup view selected
        private void customTab21_Click(object sender, EventArgs e)
        {

        }

        private void runExperimentTab_Click(object sender, EventArgs e)
        {

        }

        private void restoreDownButton_Click(object sender, EventArgs e)
        {
            Point lastLocation = this.Location;            
            this.WindowState = FormWindowState.Normal;
            this.Location = new Point(lastLocation.X + 20, lastLocation.Y + 20);
            this.restoreDownButton.Visible = false;
            this.maximizeButton.Visible = true;
            
        }


    }
}
