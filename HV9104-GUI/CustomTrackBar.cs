using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace HV9104_GUI
{
    public class CustomTrackBar : Panel
    {
        Image[] backgroundImage, pressedImage, hoverImage;
        double position;
        int buttonRadius = 30;
        int start = 30, end = 370;
        PictureBox pictureBox;
        bool pressed, leftOver, leftUnder;
        int x = 1, y = 0;
        double max = 10, min = 0;
        public event EventHandler<ValueChangeEventArgs> valueChangeHandler;
        Orientation orientation = Orientation.HORIZONTAL;


        public CustomTrackBar()
        {
            this.Size = new System.Drawing.Size(400, 60);
            this.pictureBox = new PictureBox();
            this.pictureBox.Size = new System.Drawing.Size(60, 60);
            this.pictureBox.Location = new Point((this.Width - buttonRadius * 2) / 2, 0);
            this.pictureBox.BackColor = Color.Transparent;
            this.Controls.Add(pictureBox);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            this.pictureBox.MouseLeave += new System.EventHandler(this.pictureBox_MouseLeave);
            this.pictureBox.MouseHover += new System.EventHandler(this.pictureBox_MouseHover);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(trackBar_MouseUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(trackBar_MouseDown);
            this.SizeChanged += new System.EventHandler(trackBar_SizeChanged);
            this.MaximumSize = new System.Drawing.Size(800, 60);
            this.MinimumSize = new System.Drawing.Size(200, 60);
            backgroundImage = new Image[2];
            pressedImage = new Image[2];
            hoverImage = new Image[2];
            backgroundImage[0] = global::HV9104_GUI.Properties.Resources.slider;
            this.backgroundImage[1] = CopyAndRotateImage(backgroundImage[0]);
            hoverImage[0] = global::HV9104_GUI.Properties.Resources.sliderHover;
            this.hoverImage[1] = CopyAndRotateImage(hoverImage[0]);
            pressedImage[0] = global::HV9104_GUI.Properties.Resources.SliderPressed;
            this.pressedImage[1] = CopyAndRotateImage(pressedImage[0]);
            this.pictureBox.BackgroundImage = backgroundImage[0];
        }

        public enum Orientation
        {
            VERTICAL,
            HORIZONTAL
        }

        public Orientation Layout
        {
            get
            {
                return orientation;
            }
            set
            {
                this.orientation = value;
                changeOrientation();
            }
        }

        private void changeOrientation()
        {
            if (orientation == Orientation.HORIZONTAL)
            {
                x = 1;
                y = 0;
                this.MaximumSize = new System.Drawing.Size(800, 60);
                this.MinimumSize = new System.Drawing.Size(200, 60);
                this.Size = new System.Drawing.Size(400, 60);
                this.pictureBox.Location = new Point((this.Width - buttonRadius * 2) / 2, 0);

            }
            else
            {
                x = 0;
                y = 1;
                this.MaximumSize = new System.Drawing.Size(60, 800);
                this.MinimumSize = new System.Drawing.Size(60, 200);
                this.Size = new System.Drawing.Size(60, 400);
                this.pictureBox.Location = new Point(0, (this.Height - buttonRadius * 2) / 2);

            }
            this.pictureBox.BackgroundImage = backgroundImage[y];
            this.Invalidate();
        }

        public double Min
        {
            get
            {
                return this.min;
            }
            set
            {
                this.min = value;
            }
        }

        public double Max
        {
            get
            {
                return this.max;
            }
            set
            {
                this.max = value;
            }
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

        private void trackBar_SizeChanged(object sender, EventArgs e)
        {
            start = buttonRadius;
            end = x * (this.Width - buttonRadius) + y * (this.Height - buttonRadius);
            pictureBox.Location = new Point(x * (this.Width / 2 - buttonRadius), y * (this.Height / 2 - buttonRadius));
        }

        private void trackBar_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int mousePos = x * e.X + y * e.Y;
            if (mousePos >= start && mousePos <= end)
            {
                pictureBox.Location = new Point(x * (mousePos - buttonRadius), y * (mousePos - buttonRadius));
                this.Invalidate();
            }
            else if (mousePos < start)
            {
                pictureBox.Location = new Point(0, 0);
                this.Invalidate();
            }
            else if (mousePos > end)
            {
                pictureBox.Location = new Point(x * (this.Width - buttonRadius * 2), y * (this.Height - buttonRadius * 2));
                this.Invalidate();
            }
        }

        private void trackBar_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            position = (double)((max - min) * (x * (pictureBox.Location.X + buttonRadius - start) + y * (end - pictureBox.Location.Y - buttonRadius)) / (end - start));
            position += min;
            //Send info to subscriber
            reportValueChange(position.ToString(), position);

        }



        private void pictureBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pressed = true;
            if (pressedImage != null)
                pictureBox.BackgroundImage = pressedImage[y];
        }

        private void pictureBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            pressed = false;
            this.Invalidate();
            position = (double)((max - min) * (x * (pictureBox.Location.X + buttonRadius - start) + y * (end - pictureBox.Location.Y - buttonRadius)) / (end - start));
            position += min;
            //Send info to subscriber
            reportValueChange(position.ToString(), position);
            if (HoverImage != null)
                pictureBox.BackgroundImage = hoverImage[y];


        }

        private void pictureBox_MouseHover(object sender, System.EventArgs e)
        {
            if (HoverImage != null)
                pictureBox.BackgroundImage = hoverImage[y];
        }

        private void pictureBox_MouseLeave(object sender, System.EventArgs e)
        {
            pressed = false;
            if (backgroundImage != null)
                pictureBox.BackgroundImage = backgroundImage[y];
        }

        private void pictureBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int mousePos = x * (e.X + pictureBox.Location.X) + y * (e.Y + pictureBox.Location.Y);
            if (pressed)
            {

                if (mousePos >= start && mousePos <= end)
                {
                    pictureBox.Location = new Point(x * (mousePos - 30), y * (mousePos - 30));
                    leftOver = false;
                    leftUnder = false;

                }
                else if (mousePos < 0 && !leftUnder)
                {
                    leftUnder = true;
                    leftOver = false;
                    pictureBox.Location = new Point(0, 0);
                    this.Invalidate();
                }
                else if (mousePos > (x * this.Width + y * this.Height) && !leftOver)
                {
                    leftOver = true;
                    leftUnder = false;
                    pictureBox.Location = new Point(x * (this.Width - buttonRadius * 2), y * (this.Height - buttonRadius * 2));
                    this.Invalidate();
                }
            }

        }

        public Image BackgroundImage
        {
            get
            {
                return this.backgroundImage[0];
            }
            set
            {
                this.backgroundImage[0] = value;
                this.backgroundImage[1] = CopyAndRotateImage(value);
                this.pictureBox.BackgroundImage = backgroundImage[y];
            }
        }

        protected Bitmap CopyAndRotateImage(Image source)
        {

            Bitmap bmp = new Bitmap(source.Width, source.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height));

            g.Dispose();
            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            return bmp;
        }

        public Image HPressedImage
        {
            get
            {
                return this.pressedImage[0];
            }
            set
            {
                this.pressedImage[0] = value;
                this.pressedImage[1] = CopyAndRotateImage(value);
            }
        }

        public Image HoverImage
        {
            get
            {
                return this.hoverImage[0];
            }
            set
            {
                this.hoverImage[0] = value;
                this.hoverImage[1] = CopyAndRotateImage(value);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            Bitmap bmp = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(bmp);
            int cornerRadius = 7;
            int borderThickness = 2;
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(Color.FromArgb(141, 158, 166));
            LinearGradientBrush linGrBrush = new LinearGradientBrush(
                                                                           new Point(0, 10),
                                                                           new Point(this.Width, 10),
                                                                           Color.FromArgb(177, 196, 208),   // Opaque red
                                                                           Color.FromArgb(141, 158, 166));  // Opaque blue
            if (orientation == Orientation.HORIZONTAL)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillEllipse(myBrush, new Rectangle(buttonRadius - cornerRadius - borderThickness, buttonRadius - cornerRadius - borderThickness, cornerRadius * 2 + borderThickness * 2, cornerRadius * 2 + borderThickness * 2));
                g.FillEllipse(myBrush, new Rectangle(end - cornerRadius - borderThickness, buttonRadius - cornerRadius - borderThickness, cornerRadius * 2 + borderThickness * 2, cornerRadius * 2 + borderThickness * 2));
                g.FillRectangle(myBrush, new Rectangle(start, buttonRadius - cornerRadius - borderThickness, x * this.Width + y * this.Height - buttonRadius * 2, cornerRadius * 2 + 2 * borderThickness));
                myBrush.Color = Color.White;
                g.FillEllipse(myBrush, new Rectangle(buttonRadius - cornerRadius, buttonRadius - cornerRadius, cornerRadius * 2, cornerRadius * 2));
                g.FillEllipse(myBrush, new Rectangle(end - cornerRadius, buttonRadius - cornerRadius, cornerRadius * 2, cornerRadius * 2));
                g.FillRectangle(myBrush, new Rectangle(start, buttonRadius - cornerRadius, x * this.Width + y * this.Height - buttonRadius * 2, cornerRadius * 2));

                g.FillEllipse(linGrBrush, new Rectangle(buttonRadius - cornerRadius, buttonRadius - cornerRadius, cornerRadius * 2, cornerRadius * 2));
                g.FillRectangle(linGrBrush, new Rectangle(start, buttonRadius - cornerRadius, x * pictureBox.Location.X + y * pictureBox.Location.Y - start + buttonRadius, cornerRadius * 2));
            }
            else
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillEllipse(myBrush, new Rectangle(buttonRadius - cornerRadius - borderThickness, buttonRadius - cornerRadius - borderThickness, cornerRadius * 2 + borderThickness * 2, cornerRadius * 2 + borderThickness * 2));
                g.FillEllipse(myBrush, new Rectangle(buttonRadius - cornerRadius - borderThickness, end - cornerRadius - borderThickness, cornerRadius * 2 + borderThickness * 2, cornerRadius * 2 + borderThickness * 2));
                g.FillRectangle(myBrush, new Rectangle(buttonRadius - cornerRadius - borderThickness, start, cornerRadius * 2 + 2 * borderThickness, this.Height - buttonRadius * 2));
                myBrush.Color = Color.White;
                g.FillEllipse(myBrush, new Rectangle(buttonRadius - cornerRadius, buttonRadius - cornerRadius, cornerRadius * 2, cornerRadius * 2));
                g.FillEllipse(myBrush, new Rectangle(buttonRadius - cornerRadius, end - cornerRadius, cornerRadius * 2, cornerRadius * 2));
                g.FillRectangle(myBrush, new Rectangle(buttonRadius - cornerRadius, start, cornerRadius * 2, this.Height - buttonRadius * 2));

                g.FillEllipse(linGrBrush, new Rectangle(buttonRadius - cornerRadius, end - cornerRadius, cornerRadius * 2, cornerRadius * 2));

                g.FillRectangle(linGrBrush, new Rectangle(buttonRadius - cornerRadius, pictureBox.Location.Y + buttonRadius, cornerRadius * 2, end - pictureBox.Location.Y - buttonRadius));

            }

            linGrBrush.Dispose();
            myBrush.Dispose();
            g.Dispose();

            e.Graphics.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));

            base.OnPaint(e);
        }

    }


}
