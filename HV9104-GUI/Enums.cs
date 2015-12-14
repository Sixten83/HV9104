using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HV9104_GUI
{
    public class Enums
    {
        public enum Stage: uint
        {
            Single_Stage,
            Two_Stage,
            Three_Stage
        }

        public enum TimeBase : uint
        {             
            Time_5_msDiv = 5,
            Time_10_msDiv,
            Time_20_msDiv,
            Time_50_msDiv,
            Time_100_msDiv
        }       

    }
}
