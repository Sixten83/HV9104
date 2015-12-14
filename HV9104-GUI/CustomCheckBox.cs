using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace HV9104_GUI
{
    public class CustomCheckBox : Control
    {

        Image backgroundImage, checkedImage, checkedHoverImage, uncheckedHoverImage;
        bool check, hover;

        public CustomCheckBox()
        {
            this.Size = new System.Drawing.Size(48, 43);
            this.Name = "";
            this.BackColor = System.Drawing.Color.FromArgb(236, 236, 236);
           
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

        public Image CheckedImage
        {
            get
            {
                return this.checkedImage;
            }
            set
            {
                this.checkedImage = value;
            }
        }

        public Image CheckedHoverImage
        {
            get
            {
                return this.checkedHoverImage;
            }
            set
            {
                this.checkedHoverImage = value;
            }
        }

        public Image UncheckedHoverImage
        {
            get
            {
                return this.uncheckedHoverImage;
            }
            set
            {
                this.uncheckedHoverImage = value;
            }
        }

        public bool isChecked
        {
            set
            {
                check = value;
            }
            get
            {
                return check;
            }
        }

        


        public Image setImage(String file)
        {
            try
            {

                return Image.FromFile(file);


            }
            catch (System.IO.FileNotFoundException)
            {

                Bitmap bmp = new Bitmap(Width, Height);
                Graphics g = Graphics.FromImage(bmp);
                g.FillRectangle(new SolidBrush(Color.White), 0, 0, bmp.Width, bmp.Height);
                g.DrawRectangle(new Pen(Color.FromArgb(141, 158, 166),2), 1, 1, bmp.Width-2, bmp.Height-2);
                g.DrawLine(new Pen(Color.FromArgb(141, 158, 166),2),new Point(8,25),new Point(19,37));
                g.DrawLine(new Pen(Color.FromArgb(141, 158, 166),2), new Point(19, 37), new Point(41, 5));
                g.Dispose();

                return bmp;


            }

        }

        // When the mouse button is pressed, set the "pressed" flag to true  
        // and invalidate the form to cause a repaint.  The .NET Compact Framework  
        // sets the mouse capture automatically. 
        protected override void OnMouseDown(MouseEventArgs e)
        {
            //Checks if the Parent contains more radioButtons and deselects them.  
            if (this.check)
                this.check = false;
            else
                this.check = true;
            this.Invalidate();
            base.OnMouseDown(e);

        }


        // When the mouse is entering the button area, set the "hover" flag 
        // and invalidate to redraw the button in the unpressed state.  
        protected override void OnMouseEnter(EventArgs e)
        {
            this.hover = true;
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        // When the mouse is leaving the button area, reset the "hover" flag 
        // and invalidate to redraw the button in the unpressed state. 
        protected override void OnMouseLeave(EventArgs e)
        {
            this.hover = false;
            this.Invalidate();
            base.OnMouseLeave(e);
        }



        // Override the OnPaint method to draw the background image and the text. 
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.check && !this.hover && this.checkedImage != null)
                e.Graphics.DrawImage(this.checkedImage, new Rectangle(0, 0, checkedImage.Width, checkedImage.Height));
            else if (this.check && this.hover && this.checkedImage != null)
                e.Graphics.DrawImage(this.checkedHoverImage, new Rectangle(0, 0, checkedHoverImage.Width, checkedHoverImage.Height));
            else if (!this.check && !this.hover && this.backgroundImage != null)
                e.Graphics.DrawImage(this.backgroundImage, new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height));
            else if ((this.uncheckedHoverImage != null))
                e.Graphics.DrawImage(this.uncheckedHoverImage, new Rectangle(0, 0, uncheckedHoverImage.Width, uncheckedHoverImage.Height));

            else
            {
                Bitmap bmp = new Bitmap(Width, Height);
                Graphics g = Graphics.FromImage(bmp);
                g.FillRectangle(new SolidBrush(Color.White), 0, 0, bmp.Width, bmp.Height);
                g.DrawRectangle(new Pen(Color.FromArgb(141, 158, 166), 2), 1, 1, bmp.Width - 2, bmp.Height - 2);
                g.DrawLine(new Pen(Color.FromArgb(141, 158, 166), 2), new Point(8, 25), new Point(19, 37));
                g.DrawLine(new Pen(Color.FromArgb(141, 158, 166), 2), new Point(19, 37), new Point(41, 5));
                g.Dispose();
                e.Graphics.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            }

            base.OnPaint(e);
        }

    }
}
