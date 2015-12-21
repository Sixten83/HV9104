using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HV9104_GUI
{
    public class AdviseParentEventArgs : EventArgs
    {
        private readonly string adviseText;

        public AdviseParentEventArgs(string adviseText)
        {
            this.adviseText = adviseText;
            //Hej
        }

        public string AdviseText
        {
            get { return adviseText; }
        }
    }
}
