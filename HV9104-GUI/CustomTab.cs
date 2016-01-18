using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace HV9104_GUI
{
    public class CustomTab : Control
    {
        Image tabBorderImage, tabSelectedIcon, tabUnselectedIcon;
        bool selected, hover;

        public CustomTab()
        {
            this.Size = new System.Drawing.Size(400, 100);
            this.BackColor = System.Drawing.Color.White;            
            selected = true;
        }

        public Image BackgroundImage
        {
            get
            {
                return this.tabBorderImage;
            }
            set
            {
                this.tabBorderImage = value;
            }
        }

        public Image SelectedIcon
        {
            get
            {
                return this.tabSelectedIcon;
            }
            set
            {
                this.tabSelectedIcon = value;
            }
        }

        public Image UnselectedIcon
        {
            get
            {
                return this.tabUnselectedIcon;
            }
            set
            {
                this.tabUnselectedIcon = value;
            }
        }



        public bool isSelected
        {
            set
            {
                selected = value;
            }
            get
            {
                return selected;
            }
        }


        // When the mouse button is pressed, set the "pressed" flag to true  
        // and invalidate the form to cause a repaint.  The .NET Compact Framework  
        // sets the mouse capture automatically. 
        protected override void OnMouseDown(MouseEventArgs e)
        {
            //Checks if the Parent contains more radioButtons and deselects them.  
            foreach (Control c in this.Parent.Controls)
            {
                if (c.GetType() == typeof(CustomTab))
                {
                    CustomTab tab = (CustomTab)c;
                    tab.isSelected = false;
                    tab.Invalidate();
                }
            }
            this.Focus();
            this.selected = true;
            this.Invalidate();
            this.Parent.Invalidate();
            base.OnMouseDown(e);

        }


        // When the mouse is entering the button area, set the "hover" flag 
        // and invalidate to redraw the button in the unpressed state.  
        protected override void OnMouseEnter(EventArgs e)
        {
            if (!hover && !selected)
            {
                this.hover = true;
                this.Invalidate();
            }
            base.OnMouseEnter(e);
        }

        // When the mouse is leaving the button area, reset the "hover" flag 
        // and invalidate to redraw the button in the unpressed state. 
        protected override void OnMouseLeave(EventArgs e)
        {
            if (hover)
            {
                this.hover = false;
                this.Invalidate();
            }
            base.OnMouseLeave(e);
        }



        // Override the OnPaint method to draw the background image and the text. 
        protected override void OnPaint(PaintEventArgs e)
        {


            Color fontColor = new Color();

            if (this.selected && this.tabBorderImage != null)
            {

                e.Graphics.DrawImage(this.tabBorderImage, new Rectangle(0, 0, tabBorderImage.Width, tabBorderImage.Height));
                if (this.tabSelectedIcon != null)
                    e.Graphics.DrawImage(this.tabSelectedIcon, new Rectangle(20, (this.Height - tabSelectedIcon.Width) / 2, tabSelectedIcon.Width, tabSelectedIcon.Height));
                fontColor = System.Drawing.Color.FromArgb(143, 200, 232);

            }
            else if (!this.selected && this.hover && this.tabSelectedIcon != null)
            {
                if (this.tabSelectedIcon != null)
                {
                    e.Graphics.DrawImage(this.tabSelectedIcon, new Rectangle(20, (this.Height - tabSelectedIcon.Width) / 2, tabSelectedIcon.Width, tabSelectedIcon.Height));
                }
                fontColor = System.Drawing.Color.FromArgb(143, 200, 232);
                

            }
            else if (!this.selected && !this.hover && this.tabUnselectedIcon != null)
            {
                if (this.tabUnselectedIcon != null)
                    e.Graphics.DrawImage(this.tabUnselectedIcon, new Rectangle(20, (this.Height - tabUnselectedIcon.Width) / 2, tabUnselectedIcon.Width, tabUnselectedIcon.Height));
                fontColor = Color.FromArgb(141, 158, 166);
             
            }
            else
            {
                Bitmap bmp = new Bitmap(Width, Height);
                Graphics g = Graphics.FromImage(bmp);
                int cornerRadius = 40;
                int borderThickness = 2;
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(Color.FromArgb(141, 158, 166));

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillEllipse(myBrush, new Rectangle(0, 0, cornerRadius * 2, cornerRadius * 2));
                g.FillEllipse(myBrush, new Rectangle(this.Width - cornerRadius * 2, 0, cornerRadius * 2, cornerRadius * 2));
                g.FillRectangle(myBrush, new Rectangle(cornerRadius, 0, this.Width - cornerRadius * 2, this.Height));
                g.FillRectangle(myBrush, new Rectangle(0, cornerRadius, this.Width, this.Height));

                myBrush.Color = Color.White;

                g.FillEllipse(myBrush, new Rectangle(borderThickness, borderThickness, 2 * (cornerRadius - borderThickness), 2 * (cornerRadius - borderThickness)));
                g.FillEllipse(myBrush, new Rectangle(this.Width - cornerRadius * 2 + borderThickness, 0 + borderThickness, 2 * (cornerRadius - borderThickness), 2 * (cornerRadius - borderThickness)));
                g.FillRectangle(myBrush, new Rectangle(cornerRadius + borderThickness, borderThickness, this.Width - 2 * (cornerRadius + borderThickness), this.Height - 2 * borderThickness));
                g.FillRectangle(myBrush, new Rectangle(borderThickness, cornerRadius + borderThickness, this.Width - 2 * borderThickness, this.Height));

                myBrush.Dispose();
                g.Dispose();
                e.Graphics.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));

            }

            Bitmap bmp2 = new Bitmap(this.Width, this.Height);
            Graphics g2 = Graphics.FromImage(bmp2);
            g2.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            g2.DrawString(this.Text,
                     new System.Drawing.Font("Calibri (Body)", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0))),
                       new System.Drawing.SolidBrush(fontColor),
                     59 + 40,
                     (this.ClientSize.Height - 28) / 2);
            g2.Dispose();
            e.Graphics.DrawImage(bmp2, new Rectangle(0, 0, bmp2.Width, bmp2.Height));

            base.OnPaint(e);
            
        }

    }
}
