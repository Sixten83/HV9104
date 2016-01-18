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
    
    class CustomDropDownList : Form
    {

        CustomPanel list;
        List<Label> listMembers;
        public event EventHandler<AdviseParentEventArgs> AdviseParent;

        public CustomDropDownList()
        {
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            
            StartPosition = FormStartPosition.Manual;
            ControlBox = false;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            ShowInTaskbar = false;
         
            this.LostFocus += new System.EventHandler(this.lostFocus);
            list = new CustomPanel();
            list.Size = this.Size;
            list.cornerRadius = 12;
            list.backgroundColor = System.Drawing.Color.White;
            list.borderColor = System.Drawing.Color.FromArgb(143, 200, 232);
            this.Controls.Add(list);
            list.MouseEnter += new System.EventHandler(this._OnMouseEnter);
            list.MouseLeave += new System.EventHandler(this._OnMouseLeave);
            list.MouseDown += new System.Windows.Forms.MouseEventHandler(this._OnMouseDown);
            
        }

        public void addListMembers(List<String> member)
        {
            int r = 0;
            listMembers = new List<Label>();
            int fontSize = 20;
            int newHeight= 24;
            foreach (String s in member)
            {
                Label listMember = new Label();
                listMember.Name = "listMember" + r+1;
                listMember.BorderStyle = BorderStyle.None;
                listMember.Font = new System.Drawing.Font("Calibri (Body)", fontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                listMember.ForeColor = System.Drawing.Color.FromArgb(127, 127, 127);
                listMember.Size = new System.Drawing.Size(200, fontSize * 2);
                listMember.AutoSize = false;
                listMember.Location = new System.Drawing.Point((int)(this.Width * 0.05F), listMember.Height * r + 12);
                listMember.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                
                listMember.BackColor = Color.Transparent;
                listMember.Text = s;
                this.Controls.Add(listMember);
                listMember.MouseDown += new System.Windows.Forms.MouseEventHandler(this._OnMouseDown);
                listMember.MouseEnter += new System.EventHandler(this._OnMouseEnter);
                listMember.MouseLeave += new System.EventHandler(this._OnMouseLeave);
                listMembers.Add(listMember);             
                list.Controls.Add(listMember);
                r++;
                newHeight += fontSize * 2;
            }
            ClientSize = new System.Drawing.Size(this.Width, newHeight);
            list.Size = this.Size;
        }

        public void _OnMouseDown(object sender, MouseEventArgs e)
        {
            if (sender.GetType() == typeof(Label))
            {
                Label l = (Label)sender;
                OneSetLabel(l.Text);
              
            }
            this.Close();
        }


        protected virtual void OneSetLabel(string text)
        {
            EventHandler<AdviseParentEventArgs> handler = AdviseParent;
            if (handler != null)
            {
                AdviseParentEventArgs e = new AdviseParentEventArgs(text);
                handler(this, e);
            }
        }
        

        private void _OnMouseLeave(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Label))
            {
                Label l = (Label)sender;

                l.ForeColor = System.Drawing.Color.FromArgb(127, 127, 127);
            }
                //list.borderColor = System.Drawing.Color.FromArgb(140, 159, 171);
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        private void _OnMouseEnter(object sender, EventArgs e)
        {

            if (sender.GetType() == typeof(Label))
            {
                Label l = (Label)sender;

                l.ForeColor = System.Drawing.Color.FromArgb(143, 200, 232);
            }
            //list.borderColor = System.Drawing.Color.FromArgb(143, 200, 232);
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        //Override OnPaintBackground method to make form truly transparent
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //empty implementation
        }

        //Closes the list if user clicks/tabs outside listarea
        private void lostFocus(object sender, EventArgs e)
        {
            
            this.Close();
        }


        
    }
}
