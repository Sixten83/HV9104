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
    public class CustomComboBox : Control
    {
        private Color focusColor, lostFocusColor;
        Image backgroundImage, hoverImage;
        public bool hover, clicked;
        List<String> listMembers;
        Label selectedMember;
        String[] test;
        ToolTip toolTip;
        public event EventHandler<ValueChangeEventArgs> valueChangeHandler;
       

        public CustomComboBox()
        {
            focusColor = System.Drawing.Color.FromArgb(143, 200, 232);
            lostFocusColor = System.Drawing.Color.FromArgb(140, 159, 171);
            Size = new System.Drawing.Size(209, 67);         
            
            this.BackColor = System.Drawing.Color.FromArgb(236, 236, 236);
            
            listMembers = new List<String>();

            selectedMember = new Label();
            selectedMember.Font = new System.Drawing.Font("Calibri (Body)", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            selectedMember.ForeColor = System.Drawing.Color.FromArgb(127, 127, 127);

            selectedMember.Size = new System.Drawing.Size(200, 40);
            selectedMember.Location = new System.Drawing.Point((this.Width - selectedMember.Width) / 2, 12);
            selectedMember.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            selectedMember.BackColor = Color.Transparent;
            this.Controls.Add(selectedMember);
            selectedMember.MouseEnter += new System.EventHandler(this._OnMouseEnter);
            selectedMember.MouseLeave += new System.EventHandler(this._OnMouseLeave);
            selectedMember.MouseDown += new System.Windows.Forms.MouseEventHandler(this._OnMouseDown);
            toolTip = new ToolTip();
        }

        public string TextBoxHint
        {
            get
            {
                return toolTip.GetToolTip(this);
            }
            set
            {
                toolTip.SetToolTip(this, value);
                foreach (Control c in this.Controls)
                {
                    toolTip.SetToolTip(c, value);
                }
            }
        }
       
        public String[] setCollection
        {
            get
            {
                return this.test;
            }
            set
            {
                this.test = value;
                listMembers.Clear();
                foreach(String s in test)
                {
                    addListMembers(s);
                }
                selectedMember.Text = listMembers[0];
            }
        }

        public Image BackgroundImage
        {
            get
            {
                return this.backgroundImage;
            }
            set
            {
                this.backgroundImage = value;
            }
        }

        public Image HoverImage
        {
            get
            {
                return this.hoverImage;
            }
            set
            {
                this.hoverImage = value;
            }
        }

        public void setSelectedMember(String member)
        {
            selectedMember.Text = member;
        }

        public void addListMembers(String member)
        {
            listMembers.Add(member);
        }

        //Tries to get the images from the resources
        public Image setImage(String file)
        {
            try
            {

                return Image.FromFile(file);


            }
            catch (System.IO.FileNotFoundException)
            {
                //Draws a representation of the "image" if the image can't be found 
                Bitmap bmp = new Bitmap(Width, Height);
                Graphics g = Graphics.FromImage(bmp);
                int cornerRadius = 12;
                int borderThickness = 2;
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(lostFocusColor);
              
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillEllipse(myBrush, new Rectangle(0, 0, cornerRadius * 2, cornerRadius * 2));
                g.FillEllipse(myBrush, new Rectangle(this.Width - cornerRadius * 2, 0, cornerRadius * 2, cornerRadius * 2));
                g.FillEllipse(myBrush, new Rectangle(0, this.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2));
                g.FillEllipse(myBrush, new Rectangle(this.Width - cornerRadius * 2, this.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2));
                g.FillRectangle(myBrush, new Rectangle(cornerRadius, 0, this.Width - cornerRadius * 2, this.Height));
                g.FillRectangle(myBrush, new Rectangle(0, cornerRadius, this.Width, this.Height - cornerRadius * 2));

                myBrush.Color = Color.White;

                g.FillEllipse(myBrush, new Rectangle(borderThickness, borderThickness, 2 * (cornerRadius - borderThickness), 2 * (cornerRadius - borderThickness)));
                g.FillEllipse(myBrush, new Rectangle(this.Width - cornerRadius * 2 + borderThickness, 0 + borderThickness, 2 * (cornerRadius - borderThickness), 2 * (cornerRadius - borderThickness)));
                g.FillEllipse(myBrush, new Rectangle(borderThickness, this.Height - cornerRadius * 2 + borderThickness, 2 * (cornerRadius - borderThickness), 2 * (cornerRadius - borderThickness)));
                g.FillEllipse(myBrush, new Rectangle(this.Width - cornerRadius * 2 + borderThickness, this.Height - cornerRadius * 2 + borderThickness, 2 * (cornerRadius - borderThickness), 2 * (cornerRadius - borderThickness)));
                g.FillRectangle(myBrush, new Rectangle(cornerRadius + borderThickness, borderThickness, this.Width - 2 * (cornerRadius + borderThickness), this.Height - 2 * borderThickness));
                g.FillRectangle(myBrush, new Rectangle(borderThickness, cornerRadius + borderThickness, this.Width - 2 * borderThickness, this.Height - 2 * (cornerRadius + borderThickness)));

                myBrush.Dispose();
                g.Dispose();

                return bmp;

                


            }

        }

        // When the mouse button is pressed, set the "pressed" flag to true  
        // and invalidate the form to cause a repaint.  The .NET Compact Framework  
        // sets the mouse capture automatically. 
        protected override void OnMouseDown(MouseEventArgs e)
        {
            showDropDownlist();
            base.OnMouseDown(e);
        }

        

        private void _OnMouseDown(object sender, MouseEventArgs e)
        {

            showDropDownlist();
            base.OnMouseDown(e);
        }
         
        private void _OnMouseEnter(object sender, EventArgs e)
        {
            showHoverState();
            base.OnMouseEnter(e);
        }

        private void _OnMouseLeave(object sender, EventArgs e)
        {
            showNeutralState();
            base.OnMouseLeave(e);
        }

       
        
        // When the mouse is entering the button area, set the "hover" flag 
        // and invalidate to redraw the button in the unpressed state.  
        protected override void OnMouseEnter(EventArgs e)
        {
            showHoverState();
            base.OnMouseEnter(e);
        }

        // When the mouse is leaving the button area, reset the "hover" flag 
        // and invalidate to redraw the button in the unpressed state. 
        protected override void OnMouseLeave(EventArgs e)
        {
            showNeutralState();
            base.OnMouseLeave(e);
        }

        private void showDropDownlist()
        {
            clicked = true;
            selectedMember.ForeColor = focusColor;
            Point startPoint = this.PointToScreen(new Point());
            CustomDropDownList list = new CustomDropDownList();
            list.Location = new Point(startPoint.X, startPoint.Y + this.Height);
            list.LostFocus += new System.EventHandler(this.lostFocus);
            list.addListMembers(listMembers);
            list.AdviseParent += new EventHandler<AdviseParentEventArgs>(frm2_AdviseParent);
            list.Show();
            //list.Owner = this.;
            this.Invalidate();

        }

        private void frm2_AdviseParent(object sender, AdviseParentEventArgs e)
        {
            selectedMember.Text = e.AdviseText;
            reportValueChange(selectedMember.Text,0);
        }

        protected virtual void reportValueChange(string text, double value)
        {
            EventHandler<ValueChangeEventArgs> handler = valueChangeHandler;
            if (handler != null)
            {
                ValueChangeEventArgs e = new ValueChangeEventArgs(text, value);
                handler(this, e);
            }
        }

        private void showHoverState()
        {
            if (!hover)
            {
                hover = true;
                selectedMember.ForeColor = focusColor;
                this.Invalidate();
            }

        }

        private void showNeutralState()
        {
            if (!clicked) 
            {
                hover = false;
                selectedMember.ForeColor = System.Drawing.Color.FromArgb(127, 127, 127);
                this.Invalidate();
            }

        }

        //Closes the list if user clicks/tabs outside listarea
        private void lostFocus(object sender, EventArgs e)
        {
         
            clicked = false;
            hover = false;
            selectedMember.ForeColor = System.Drawing.Color.FromArgb(127, 127, 127);
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if ((this.hover || clicked) && this.hoverImage != null)
                e.Graphics.DrawImage(this.hoverImage, new Rectangle(0, 0, hoverImage.Width, hoverImage.Height));
            else if (!this.hover && this.backgroundImage != null)
                e.Graphics.DrawImage(this.backgroundImage, new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height));
            else
            {
                //Draws a representation of the "image" if the image can't be found 
                Bitmap bmp = new Bitmap(Width, Height);
                Graphics g = Graphics.FromImage(bmp);
                int cornerRadius = 12;
                int borderThickness = 2;
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(lostFocusColor);

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillEllipse(myBrush, new Rectangle(0, 0, cornerRadius * 2, cornerRadius * 2));
                g.FillEllipse(myBrush, new Rectangle(this.Width - cornerRadius * 2, 0, cornerRadius * 2, cornerRadius * 2));
                g.FillEllipse(myBrush, new Rectangle(0, this.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2));
                g.FillEllipse(myBrush, new Rectangle(this.Width - cornerRadius * 2, this.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2));
                g.FillRectangle(myBrush, new Rectangle(cornerRadius, 0, this.Width - cornerRadius * 2, this.Height));
                g.FillRectangle(myBrush, new Rectangle(0, cornerRadius, this.Width, this.Height - cornerRadius * 2));

                myBrush.Color = Color.White;

                g.FillEllipse(myBrush, new Rectangle(borderThickness, borderThickness, 2 * (cornerRadius - borderThickness), 2 * (cornerRadius - borderThickness)));
                g.FillEllipse(myBrush, new Rectangle(this.Width - cornerRadius * 2 + borderThickness, 0 + borderThickness, 2 * (cornerRadius - borderThickness), 2 * (cornerRadius - borderThickness)));
                g.FillEllipse(myBrush, new Rectangle(borderThickness, this.Height - cornerRadius * 2 + borderThickness, 2 * (cornerRadius - borderThickness), 2 * (cornerRadius - borderThickness)));
                g.FillEllipse(myBrush, new Rectangle(this.Width - cornerRadius * 2 + borderThickness, this.Height - cornerRadius * 2 + borderThickness, 2 * (cornerRadius - borderThickness), 2 * (cornerRadius - borderThickness)));
                g.FillRectangle(myBrush, new Rectangle(cornerRadius + borderThickness, borderThickness, this.Width - 2 * (cornerRadius + borderThickness), this.Height - 2 * borderThickness));
                g.FillRectangle(myBrush, new Rectangle(borderThickness, cornerRadius + borderThickness, this.Width - 2 * borderThickness, this.Height - 2 * (cornerRadius + borderThickness)));

                myBrush.Dispose();
                g.Dispose();
                e.Graphics.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            }
            base.OnPaint(e);
        }
    }
}
