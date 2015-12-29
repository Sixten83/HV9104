using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace HV9104_GUI
{
    public class CustomButton : Control
    {
        Image backgroundImage, pressedImage, hoverImage;
        String defaultImagePath;
        bool pressed = false;
        bool hover = false;


        public CustomButton()
        {
            //this.BackColor = System.Drawing.Color.Transparent;
            this.Size = new System.Drawing.Size(400, 100);
            this.BackColor = System.Drawing.Color.FromArgb(236, 236, 236);
            this.ForeColor = Color.White;        
           
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

                myBrush.Dispose();
                g.Dispose();

                return bmp;


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

        public Image PressedImage
        {
            get
            {
                return this.pressedImage;
            }
            set
            {
                this.pressedImage = value;
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

     
        // When the mouse button is pressed, set the "pressed" flag to true  
        // and invalidate the form to cause a repaint.  The .NET Compact Framework  
        // sets the mouse capture automatically. 
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.pressed = true;
            this.Invalidate();
            base.OnMouseDown(e);
        }

        // When the mouse is released, reset the "pressed" flag 
        // and invalidate to redraw the button in the unpressed state. 
        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.pressed = false;
            this.Invalidate();
            base.OnMouseUp(e);
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

           
            if (this.pressed && this.pressedImage != null)
                e.Graphics.DrawImage(this.pressedImage, new Rectangle(0, 0, pressedImage.Width, pressedImage.Height));
            else if ((this.hover && this.hoverImage != null))
                e.Graphics.DrawImage(this.hoverImage, new Rectangle(0, 0, hoverImage.Width, hoverImage.Height));
            else if (this.backgroundImage != null)
                e.Graphics.DrawImage(this.backgroundImage, new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height));
            else
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

                myBrush.Dispose();
                g.Dispose();
                e.Graphics.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            }



            // Draw the text if there is any. 
            if (this.Text.Length > 0)
            {
                SizeF size = e.Graphics.MeasureString(this.Text, this.Font);

                // Center the text inside the client area of the PictureButton.
                e.Graphics.DrawString(this.Text,
                    this.Font,
                    new SolidBrush(this.ForeColor),
                    (this.ClientSize.Width - size.Width) / 2,
                    (this.ClientSize.Height - size.Height) / 2);
            }

            base.OnPaint(e);
        }
    }
}
