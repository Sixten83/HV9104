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
         

        public CustomTextBox()
        {

           
            InitializeComponent();
            toolTip = new ToolTip();
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
                for(int r = 0 ;r <decimals; r++)
                {
                    decimalString = decimalString + "0";
                }
            }
        }

        public int Max
        {
            get
            {
                return this.max;
            }
            set
            {
                this.max = value;
                maxValueBox.Text = "" + max;
            }
        }

        public int Min
        {
            get
            {
                return this.min;
            }
            set
            {
                this.min = value;
                minValueBox.Text = "" + min;
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


            string keyInput = e.KeyChar.ToString();

            if ((Char.IsDigit(e.KeyChar) || Char.IsPunctuation(e.KeyChar)) && inputBox.Text.Length < 5)
            {
                 if (e.KeyChar == ',')
            {
                 e.KeyChar = '.';
                 
               
            }
                // Digits are OK
            }
           
            else if (keyInput.Equals(negativeSign) && inputBox.Text.Length > 0)
            {
                inputBox.Text.Remove(inputBox.Text.Length - 1);// Decimal separator is OK
            }
            else if (e.KeyChar == '\b')
            {
                // Backspace key is OK
            }
            //    else if ((ModifierKeys & (Keys.Control | Keys.Alt)) != 0) 
            //    { 
            //     // Let the edit control handle control and alt key combinations 
            //    } 
            else if (this.allowSpace && e.KeyChar == ' ')
            {

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
                        
                        this.Parent.Focus();
                    }
                    else
                        inputBox.Text = "" + value;
                }                
            }
            
            else
            {
                // Consume this invalid key and beep
                e.Handled = true;
                //    MessageBeep();
            }
        }

        
       
    }



    
}
