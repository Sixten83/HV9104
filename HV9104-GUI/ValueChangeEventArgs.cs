using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HV9104_GUI
{
    public class ValueChangeEventArgs : EventArgs
    {
        private readonly string text;
        private readonly double value;

        public ValueChangeEventArgs(string text, double value)
        {
            this.text = text;
            this.value = value;
        }

        public string Text
        {
            get { return text; }
        }
        public double Value
        {
            get { return value; }
        }
    }
}
