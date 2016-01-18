using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;


namespace HV9104_GUI
{
    public partial class CustomTextBox : CustomPanel
    {
        ToolTip toolTip;
        public event EventHandler<ValueChangeEventArgs> valueChangeHandler;
        bool valueOk;
        int decimals = 2;
        string decimalString = "0.00";
        bool allowText;
        bool allowDecimals = true;
        bool decimalDot;

        public CustomTextBox()
        {
            InitializeComponent();
            toolTip = new ToolTip();
            this.SizeChanged += new System.EventHandler(textBox_SizeChanged);
            this.inputBox.LostFocus += new System.EventHandler(this.lostFocus);
        }

        private void textBox_SizeChanged(object sender, EventArgs e)
        {
            inputBox.Size = new System.Drawing.Size(this.Width - CornerRadius * 3, 20);
            inputBox.Location = new System.Drawing.Point((int)(CornerRadius * 1.5F), (this.Height - 30) / 2);
            this.maxValueBox.Location = new System.Drawing.Point((int)(this.Width - maxValueBox.Width - cornerRadius / 2), (this.Height - 11) / 2);

        }

        private void lostFocus(object sender, EventArgs e)
        {
          
            if (inputBox.Text.Length != 0)
            {

                inputBox.Text = inputBox.Text.Replace('.', ',');

                float textValue = Single.Parse(inputBox.Text);
                if (textValue >= min && textValue <= max)
                {
                    value = textValue;
                    reportValueChange(inputBox.Text, (double)value);
                    inputBox.Text = inputBox.Text.Replace(',', '.');

                    this.FindForm().ActiveControl = null;
                }
                else
                    inputBox.Text = "" + value.ToString().Replace(',', '.');
                inputBox.SelectionStart = inputBox.Text.Length;
            }
        }

        public bool AllowDecimals
        {
            get
            {
                return this.allowDecimals;
            }
            set
            {
                this.allowDecimals = value;                
            }
        }

        public bool AllowText
        {
            get
            {
                return this.allowText;
            }
            set
            {
                this.allowText = value;
                if (value == true)
                {
                    minValueBox.Visible = false;
                    maxValueBox.Visible = false;
                    inputBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
                }
                else
                {
                    minValueBox.Visible = true;
                    maxValueBox.Visible = true;
                    inputBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                }
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

        private void textBox_EnabledChanged(object sender, EventArgs e)
        {
            if (this.Enabled)
            {
                this.minValueBox.Enabled = false;
                this.maxValueBox.Enabled = false;
                this.inputBox.Enabled = true;
                this.inputBox.ForeColor = Color.FromArgb(127, 127, 127);
            }
            else
            {
                this.inputBox.Enabled = false;
                this.inputBox.BackColor = Color.White;
                this.inputBox.ForeColor = Color.LightGray;
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

        public int Decimals
        {
            get
            {
                return this.decimals;
            }
            set
            {
                this.decimals = value;
                decimalString = "0.";
                for (int r = 0; r < decimals; r++)
                {
                    decimalString = decimalString + "0";
                }
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
                maxValueBox.Text = "" + max.ToString().Replace(',', '.'); 
            }
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
                minValueBox.Text = "" + min.ToString().Replace(',', '.'); 
            }
        }

        public float Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
                if (!allowText)
                    inputBox.Text = "" + this.value.ToString(decimalString).Replace(',', '.');
            }
        }

        private void inputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            valueOk = false;
            NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string groupSeparator = numberFormatInfo.NumberGroupSeparator;
            string negativeSign = numberFormatInfo.NegativeSign;
            System.Drawing.Graphics g = this.CreateGraphics();
            System.Drawing.SizeF fontSize = g.MeasureString(inputBox.Text, inputFont);

            string keyInput = e.KeyChar.ToString();
            if (!allowText)
            {
                if (inputBox.Text.Contains('.'))
                    decimalDot = true;
                else
                    decimalDot = false;
                if ((Char.IsDigit(e.KeyChar) || (Char.IsPunctuation(e.KeyChar) && !decimalDot && allowDecimals)) && (fontSize.Width < inputBox.Width - 15 || inputBox.SelectedText.Length > 0))
                {
                    if (e.KeyChar == ',')
                    {
                        e.KeyChar = '.';
                    }
                    if (Char.IsPunctuation(e.KeyChar))
                        decimalDot = true;
                    // Digits are OK
                }

               /* else if (keyInput.Equals(negativeSign) && inputBox.Text.Length > 0)
                {
                    inputBox.Text.Remove(inputBox.Text.Length - 1);// Decimal separator is OK
                }*/
                else if (e.KeyChar == '\b')
                {

                    // Backspace key is OK
                }
                else if (e.KeyChar == '\r')
                {
                    if (inputBox.Text.Length != 0)
                    {

                        inputBox.Text = inputBox.Text.Replace('.', ',');
                      
                        float textValue = Single.Parse(inputBox.Text);
                        if (textValue >= min && textValue <= max)
                        {
                            value = textValue;
                            reportValueChange(inputBox.Text, (double)value);
                            inputBox.Text = inputBox.Text.Replace(',', '.');

                            this.FindForm().ActiveControl = null;
                        }
                        else
                            inputBox.Text = "" + value.ToString().Replace(',', '.');
                        inputBox.SelectionStart = inputBox.Text.Length;
                    }
                }

                else
                {
                    // Consume this invalid key and beep
                    e.Handled = true;
                    //    MessageBeep();
                }
            }
            else
            {
                // Console.WriteLine("fontSize.Width: " + fontSize.Width + " inputBox.Width" + inputBox.Width);
                if (fontSize.Width < inputBox.Width - 15 || e.KeyChar == '\r')
                {
                    if (e.KeyChar == '\r')
                    {
                        reportValueChange(inputBox.Text, (double)value);                      
                        this.FindForm().ActiveControl = null;
                    }
                }
                else if (inputBox.SelectedText.Length > 0)
                {

                }
                else if (e.KeyChar == '\b')
                {


                }
                else
                    e.Handled = true;
            }
        }



    }




}
