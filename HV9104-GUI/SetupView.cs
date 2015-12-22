using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HV9104_GUI
{
    public partial class SetupView : UserControl
    {
        public SetupView()
        {
            InitializeComponent();
        }

        

        private void dcStage1RadioButton_Click(object sender, EventArgs e)
        {
            this.dcDivder2TextBox.Visible = false;
            this.dcDivder3TextBox.Visible = false;
            this.dcDividerPanel.Invalidate();
        }

        private void dcStage2RadioButton_Click(object sender, EventArgs e)
        {
            this.dcDivder2TextBox.Visible = true;
            this.dcDivder3TextBox.Visible = false;
            this.dcDividerPanel.Invalidate();
        }

        private void dcStage3RadioButton_Click(object sender, EventArgs e)
        {
            this.dcDivder2TextBox.Visible = true;
            this.dcDivder3TextBox.Visible = true;
            this.dcDividerPanel.Invalidate();
        }

        private void impulseStage1RadioButton_Click(object sender, EventArgs e)
        {
            this.impulseDivder1TextBox.Visible = true;
            this.impulseDivder2TextBox.Visible = false;
            this.impulseDivder3TextBox.Visible = false;
            this.impulseDividerPanel.Invalidate();
        }

        private void impulseStage2RadioButton_Click(object sender, EventArgs e)
        {
            this.impulseDivder1TextBox.Visible = true;
            this.impulseDivder2TextBox.Visible = true;
            this.impulseDivder3TextBox.Visible = false;
            this.impulseDividerPanel.Invalidate();
        }

        private void impulseStage3RadioButton_Click(object sender, EventArgs e)
        {
            this.impulseDivder1TextBox.Visible = true;
            this.impulseDivder2TextBox.Visible = true;
            this.impulseDivder3TextBox.Visible = true;
            this.impulseDividerPanel.Invalidate();
        }

       
    }
}
