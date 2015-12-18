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
            SINGLE_STAGE,
            TWO_STAGE,
            THREE_STAGE
        }

        public enum TimeBase : uint
        {             
            Time_5_msDiv,
            Time_10_msDiv,
            Time_20_msDiv,
            Time_50_msDiv,
            Time_100_msDiv
        }

        public enum CH : uint
        {
            AC_CHANNEL,
            DC_CHANNEL,
            IMPULSE_CHANNEL
        }

        

    }
}
