using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HV9104_GUI
{
    

    class Channel
    {
        Imports.Channel     channel;
        Imports.Coupling    coupling;
        Imports.Range       voltageRange;
        short               enabled;
        public short[][]    channelBuffers;

        public Channel()
        {
            this.coupling = Imports.Coupling.PS5000A_DC;
            this.voltageRange = Imports.Range.Range_5V;
        }

        public void setChannelBuffers(int bufferSize)
        {
            channelBuffers = new short[2][];
            channelBuffers[0] = new short[bufferSize];
            channelBuffers[1] = new short[bufferSize];
        }

        public Imports.Channel ChannelName
        {
            set
            {
                this.channel = value;
            }
            get
            {
                return this.channel;
            }
        }

        public Imports.Coupling Coupling
        {
            set
            {
                this.coupling = value;
            }
            get
            {
                return this.coupling;
            }
        }

        public Imports.Range VoltageRange
        {
            set
            {
                this.voltageRange = value;
            }
            get
            {
                return this.voltageRange;
            }
        }

        public short Enabled
        {
            set
            {
                this.enabled = value;
            }
            get
            {
                return this.enabled;
            }
        }



    }
}
