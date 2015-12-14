using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace HV9104_GUI
{
    class CustomSlideButton : Control
    {

        Image uncheckedImage, checkedImage, checkedHoverImage, uncheckedHoverImage;
        bool check, hover;

        public CustomSlideButton()
        {
            this.Size = new System.Drawing.Size(138, 56);
            this.Name = "";
            this.BackColor = System.Drawing.Color.FromArgb(236, 236, 236);
            uncheckedImage = setImage("Resources/offButton.png");
            checkedImage = setImage("Resources/onButton.png");
            uncheckedHoverImage = setImage("Resources/offButtonHover.png");
            checkedHoverImage = setImage("Resources/onButtonHover.png");

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
                int cornerRadius = 12;
                int borderThickness = 2;
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(Color.FromArgb(141, 158, 166));

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

                myBrush.Color = Color.FromArgb(141, 158, 166);

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillEllipse(myBrush, new Rectangle(0, 0, cornerRadius * 2, cornerRadius * 2));
                g.FillEllipse(myBrush, new Rectangle(this.Width / 2  - cornerRadius * 2, 0, cornerRadius * 2, cornerRadius * 2));
                g.FillEllipse(myBrush, new Rectangle(0, this.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2));
                g.FillEllipse(myBrush, new Rectangle(this.Width / 2  - cornerRadius * 2, this.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2));
                g.FillRectangle(myBrush, new Rectangle(cornerRadius, 0, this.Width / 2  - cornerRadius * 2, this.Height));
                g.FillRectangle(myBrush, new Rectangle(0, cornerRadius, this.Width / 2 , this.Height - cornerRadius * 2));


                g.DrawString("OFF",
                     new System.Drawing.Font("Calibri (Body)", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0))),
                       myBrush,
                     (this.ClientSize.Width) / 2,
                     (this.ClientSize.Height-28) / 2);

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
            else if (!this.check && !this.hover && this.uncheckedImage != null)
                e.Graphics.DrawImage(this.uncheckedImage, new Rectangle(0, 0, uncheckedImage.Width, uncheckedImage.Height));
            else if ((this.uncheckedHoverImage != null))
                e.Graphics.DrawImage(this.uncheckedHoverImage, new Rectangle(0, 0, uncheckedHoverImage.Width, uncheckedHoverImage.Height));

            else
            {
                Bitmap bmp = new Bitmap(this.Width, this.Height);
                Graphics g = Graphics.FromImage(bmp);
                g.FillRectangle(new SolidBrush(Color.Black), 0, 0, bmp.Width, bmp.Height);
                g.Dispose();
            }

            base.OnPaint(e);
        }

    }
}
