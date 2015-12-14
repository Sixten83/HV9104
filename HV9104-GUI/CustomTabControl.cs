using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace HV9104_GUI
{
    public class CustomTabControl : Panel
    {
        List<Panel> tabPages;
  
        public CustomTabControl()
        {
            this.BackColor = System.Drawing.Color.White;
            this.Size = new System.Drawing.Size(1910, 950);
            tabPages = new List<Panel>();
            this.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.Control_Added);
            
        }

        public void addPage(UserControl usrContrl, int pagenr)
        {
            if (tabPages[pagenr] != null)
                tabPages[pagenr].Controls.Add(usrContrl);
     
        }

        private void Control_Added(object sender, System.Windows.Forms.ControlEventArgs e)
        {
            if(e.Control.GetType() == typeof(CustomTab))
            {
                Panel tabPage = new Panel();
                
                tabPage.Size = new System.Drawing.Size(1875,800);
                tabPage.Location = new System.Drawing.Point((this.Width-tabPage.Width)/2, 150);
                tabPage.BackColor = this.BackColor;
                tabPages.Add(tabPage);
                //tabPage.Controls.Add(new RunView());
                this.Controls.Add(tabPage);
               
            }            
            
        }

       

        protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap bmp = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(bmp);

            int r = 0;
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(CustomTab))
                {
                    CustomTab cstmTab = (CustomTab)c;
                        
                    if (cstmTab.isSelected)
                    {
                        g.DrawLine(new Pen(Color.FromArgb(141, 158, 166), 2), new Point(0, 135 + 1), new Point(cstmTab.Location.X + 2, 135 + 1));
                        g.DrawLine(new Pen(Color.FromArgb(141, 158, 166), 2), new Point(cstmTab.Location.X + cstmTab.Width - 2, 135 + 1), new Point(Width, 135 + 1));
                           tabPages[r].Visible = true;
                    }
                    else
                        tabPages[r].Visible = false;
                    r++;
                }
            }

            g.Dispose();
            e.Graphics.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));

            base.OnPaint(e);
        }
    }
}
