using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace HV9104_GUI
{
    public class CustomPanel : Panel
    {

            public int borderThickness,cornerRadius;
            public Color backgroundColor, borderColor,fontColor;
            bool isPopUp;    

            public CustomPanel()
            {
                this.BackColor = System.Drawing.Color.Transparent;
                Label titleLabel = new Label();
                cornerRadius = 40;
                borderThickness = 2;
                borderColor = System.Drawing.Color.FromArgb(166, 166, 166);
                backgroundColor = System.Drawing.Color.FromArgb(236, 236, 236);
                fontColor = System.Drawing.Color.FromArgb(127, 127, 127);

                titleLabel.AutoSize = true;
                titleLabel.Font = new System.Drawing.Font("Calibri (Body)", 14.6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
                titleLabel.ForeColor = fontColor;
                titleLabel.Location = new System.Drawing.Point(this.Width / 8, this.Height / 8);
                titleLabel.Size = new System.Drawing.Size(30, 13);
                titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                //titleLabel.BackColor = backgroundColor;
                titleLabel.Text = "";
                this.Controls.Add(titleLabel);
                
                
            }

            public bool IsPopUp
            {
                get
                {
                    return this.isPopUp;
                }
                set
                {
                    this.isPopUp = value;
                }
            }

            public Color BackgroundColor
            {
                get
                {
                    return this.backgroundColor;
                }
                set
                {
                    this.backgroundColor = value;
                }
            }

            public Color BorderColor
            {
                get
                {
                    return this.borderColor;
                }
                set
                {
                    this.borderColor = value;
                }
            }

            public int CornerRadius
            {
                get
                {
                    return this.cornerRadius;
                }
                set
                {
                    this.cornerRadius = value;
                }
            }




            protected override void OnPaint(PaintEventArgs e)
            {

                Bitmap bmp = new Bitmap(Width, Height);
                Graphics formGraphics = Graphics.FromImage(bmp);

                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(borderColor);
             
                formGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                formGraphics.FillEllipse(myBrush, new Rectangle(0, 0, cornerRadius * 2, cornerRadius * 2));
                formGraphics.FillEllipse(myBrush, new Rectangle(this.Width - cornerRadius * 2, 0, cornerRadius * 2, cornerRadius * 2));
                formGraphics.FillEllipse(myBrush, new Rectangle(0, this.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2));
                formGraphics.FillEllipse(myBrush, new Rectangle(this.Width - cornerRadius * 2, this.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2));
                formGraphics.FillRectangle(myBrush, new Rectangle(cornerRadius, 0, this.Width - cornerRadius * 2, this.Height));
                formGraphics.FillRectangle(myBrush, new Rectangle(0, cornerRadius, this.Width, this.Height - cornerRadius * 2));

                myBrush.Color = backgroundColor;

                formGraphics.FillEllipse(myBrush, new Rectangle(borderThickness, borderThickness, 2 * (cornerRadius - borderThickness), 2 * (cornerRadius - borderThickness)));
                formGraphics.FillEllipse(myBrush, new Rectangle(this.Width - cornerRadius * 2 + borderThickness, 0 + borderThickness, 2 * (cornerRadius - borderThickness), 2 * (cornerRadius - borderThickness)));
                formGraphics.FillEllipse(myBrush, new Rectangle(borderThickness, this.Height - cornerRadius * 2 + borderThickness, 2 * (cornerRadius - borderThickness), 2 * (cornerRadius - borderThickness)));
                formGraphics.FillEllipse(myBrush, new Rectangle(this.Width - cornerRadius * 2 + borderThickness, this.Height - cornerRadius * 2 + borderThickness, 2 * (cornerRadius - borderThickness), 2 * (cornerRadius - borderThickness)));
                formGraphics.FillRectangle(myBrush, new Rectangle(cornerRadius + borderThickness, borderThickness, this.Width - 2 * (cornerRadius + borderThickness), this.Height - 2 * borderThickness));
                formGraphics.FillRectangle(myBrush, new Rectangle(borderThickness, cornerRadius + borderThickness, this.Width - 2 * borderThickness, this.Height - 2 * (cornerRadius + borderThickness)));
                
                if(isPopUp)
                {
                    myBrush.Color = Color.FromArgb(236,236,236);
                    Pen myPen = new Pen(borderColor, borderThickness);

                    formGraphics.FillEllipse(myBrush, new Rectangle(borderThickness, borderThickness, 2 * (cornerRadius - borderThickness), 2 * (cornerRadius - borderThickness)));
                    formGraphics.FillEllipse(myBrush, new Rectangle(this.Width - cornerRadius * 2 + borderThickness, 0 + borderThickness, 2 * (cornerRadius - borderThickness), 2 * (cornerRadius - borderThickness)));
                    formGraphics.FillRectangle(myBrush, new Rectangle(cornerRadius + borderThickness, borderThickness, this.Width - 2 * (cornerRadius + borderThickness), cornerRadius + borderThickness));
                    formGraphics.FillRectangle(myBrush, new Rectangle(borderThickness, cornerRadius + borderThickness, this.Width - 2 * borderThickness, cornerRadius));//this.Height));
                    formGraphics.DrawLine(myPen, new Point(0, cornerRadius * 2), new Point(Width, cornerRadius * 2));
                }                    

                myBrush.Dispose();
                formGraphics.Dispose();
                
                e.Graphics.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            }

                       
    }
}
