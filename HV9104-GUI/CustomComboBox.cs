using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Data;
using System.Drawing.Drawing2D;



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
            Size = new System.Drawing.Size(200, 60);
            this.MinimumSize = new System.Drawing.Size(145, 45);
            this.MaximumSize = new System.Drawing.Size(400, 60);

            this.BackColor = System.Drawing.Color.FromArgb(236, 236, 236);

            listMembers = new List<String>();

            selectedMember = new Label();
            selectedMember.Font = new System.Drawing.Font("Calibri (Body)", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            selectedMember.ForeColor = System.Drawing.Color.FromArgb(127, 127, 127);

            selectedMember.Size = new System.Drawing.Size(this.Width, this.Height);
            selectedMember.Location = new System.Drawing.Point((int)(this.Width * 0.05F), 0);
            selectedMember.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            selectedMember.AutoSize = false;
            selectedMember.BackColor = Color.Transparent;
            this.Controls.Add(selectedMember);
            selectedMember.MouseEnter += new System.EventHandler(this._OnMouseEnter);
            selectedMember.MouseLeave += new System.EventHandler(this._OnMouseLeave);
            selectedMember.MouseDown += new System.Windows.Forms.MouseEventHandler(this._OnMouseDown);
            toolTip = new ToolTip();
            this.SizeChanged += new System.EventHandler(comboBox_SizeChanged);
        }

        private void comboBox_SizeChanged(object sender, EventArgs e)
        {
            selectedMember.Size = new System.Drawing.Size(this.Width, this.Height);
            selectedMember.Location = new System.Drawing.Point((int)(this.Width * 0.05F), 0);
        }

        public void setSelected(int index)
        {
            selectedMember.Text = listMembers[index];
        }

        public string SetSelected
        {
            set
            {
                selectedMember.Text = value;
            }
            get
            {
                return selectedMember.Text;
            }
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

        
        public void addListMembers(String member)
        {
            listMembers.Add(member);
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
            double index = 0;
            foreach (String s in listMembers)
            {
                
                if (s.Equals(selectedMember.Text))
                    break;
                index++;
            }
            reportValueChange(selectedMember.Text,index);
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
                selectedMember.ForeColor = System.Drawing.Color.FromArgb(1
                    , 127, 127);
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

            {
                //Draws a representation of the "image" if the image can't be found 
                Bitmap bmp = new Bitmap(Width, Height);
                Graphics g = Graphics.FromImage(bmp);
                int cornerRadius = 12;
                int borderThickness = 2;
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(lostFocusColor);
                PointF point1 = new PointF(this.Width * 0.9F, this.Height / 3);
                PointF point2 = new PointF(point1.X - 2 * point1.Y, point1.Y);
                PointF point3 = new PointF(point1.X - point1.Y, point1.Y * 2);
                PointF[] trianglePoints = { point1, point2, point3 };
                PointF[] cornerPoints = { point1, point2, point3, point1, point2 };
                Pen pen = new Pen(lostFocusColor, 3);

                FillMode newFillMode = FillMode.Winding;

                if (hover || clicked)
                    myBrush.Color = focusColor;


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
                myBrush.Color = focusColor;

                myBrush.Color = lostFocusColor;

                if (hover || clicked)
                    pen.Color = focusColor;

                g.FillPolygon(myBrush, trianglePoints, newFillMode);
                g.DrawLines(pen, cornerPoints);
                myBrush.Dispose();

                g.Dispose();
                e.Graphics.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            }
            base.OnPaint(e);
        }
    }
}
