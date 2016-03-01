using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace HV9104_GUI
{
    public class CustomStatusIndicator : Control
    {
         Image trueImage, falseImage;
         bool status;

         public CustomStatusIndicator()
        {
            this.Size = new System.Drawing.Size(20, 20);
            this.Name = "";
            this.BackColor = System.Drawing.Color.FromArgb(236, 236, 236);
           
        }

         public bool IsTrue
         {
             set
             {
                 if(value != status)
                 { 
                    status = value;
                    this.Invalidate();
                 }
                 else
                     status = value;
             }
             get
             {
                 return status;
             }
         }

         public Image FalseImage
         {
             get
             {
                 return this.falseImage;
             }
             set
             {
                 this.falseImage = value;
             }
         }

         public Image TrueImage
         {
             get
             {
                 return this.trueImage;
             }
             set
             {
                 this.trueImage = value;
             }
         }

         // Override the OnPaint method to draw the background image and the text. 
         protected override void OnPaint(PaintEventArgs e)
         {
             if (this.status && this.trueImage != null)
                 e.Graphics.DrawImage(this.trueImage, new Rectangle(0, 0, trueImage.Width, trueImage.Height));
             else if (!this.status && this.falseImage != null)
                 e.Graphics.DrawImage(this.falseImage, new Rectangle(0, 0, falseImage.Width, falseImage.Height));            

             else
             {
                 Bitmap bmp = new Bitmap(Width, Height);
                 Graphics g = Graphics.FromImage(bmp);
                 g.FillEllipse(new SolidBrush(Color.FromArgb(141, 158, 166)), 0, 0, bmp.Width, bmp.Height);                
                 g.Dispose();
                 e.Graphics.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
             }

             base.OnPaint(e);
         }
    }
}
