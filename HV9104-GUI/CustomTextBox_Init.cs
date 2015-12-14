namespace HV9104_GUI
{
        

    partial class  CustomTextBox
    {
        private System.Windows.Forms.TextBox inputBox;
        private System.Windows.Forms.Label minValueBox, maxValueBox;
        private float value;
        private bool allowSpace = false;
        private int max, min;
       
        private void InitializeComponent()
        {
            inputBox = new System.Windows.Forms.TextBox();
            minValueBox = new System.Windows.Forms.Label();
            maxValueBox = new System.Windows.Forms.Label();
            this.Size = new System.Drawing.Size(170, 54);
            //minValueBox
            minValueBox.AutoSize = true;
            minValueBox.Font = new System.Drawing.Font("Calibri (Body)", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            minValueBox.ForeColor = System.Drawing.Color.FromArgb(127, 127, 127);
            minValueBox.Location = new System.Drawing.Point((int)(this.Width * 0.06F), (this.Height - 11) / 2);
            minValueBox.Size = new System.Drawing.Size(30, 13);
            minValueBox.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            minValueBox.BackColor = System.Drawing.Color.White;
            minValueBox.Text = "" + min;
            //maxValueBox
            maxValueBox.AutoSize = true;
            maxValueBox.Font = new System.Drawing.Font("Calibri (Body)", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            maxValueBox.ForeColor = System.Drawing.Color.FromArgb(127, 127, 127);
            maxValueBox.Location = new System.Drawing.Point((int)(this.Width * 0.8F), (this.Height - 11) / 2);
            maxValueBox.Size = new System.Drawing.Size(30, 13);
            maxValueBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            maxValueBox.BackColor = System.Drawing.Color.White;
            maxValueBox.Text = "" + min;
            //inputBox           
            inputBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            inputBox.Size = new System.Drawing.Size(100, 20);
            inputBox.Location = new System.Drawing.Point((this.Width - inputBox.Width) / 2, (this.Height - 30) / 2);
            inputBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            inputBox.Font = new System.Drawing.Font("Calibri (Body)", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            inputBox.ForeColor = System.Drawing.Color.FromArgb(127, 127, 127);
            inputBox.Text = "230";
            inputBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.inputBox_KeyPress);
            //CustomTextBox
            cornerRadius = 27;
            BackColor = this.backgroundColor;
            backgroundColor = System.Drawing.Color.White;
            borderColor = System.Drawing.Color.FromArgb(140, 159, 171);
            Location = new System.Drawing.Point(92, 20);
            cornerRadius = 27;
            min = 0;
            max = 230;
            value = 5.3F;
            this.Controls.Add(maxValueBox);
            this.Controls.Add(minValueBox);
            this.Controls.Add(inputBox);
        }


    }
}
