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
        public short[][]    scaledData;
        double              scaleFactor;
        double              rms, max, min,amplitud;
        short               adMaxValue;

        public Channel()
        {
            this.coupling = Imports.Coupling.PS5000A_DC;
            this.voltageRange = Imports.Range.Range_5V;
        }

        public double RMS
        {
            get
            {
                return this.rms;
            }
        }

        public double Max
        {
            get
            {
                return this.max;
            }
        }

        public double Min
        {
            get
            {
                return this.min;
            }
        }

        public double Amplitude
        {
            get
            {
                return this.amplitud;
            }
        }

        public short ADMaxValue
        {
            set
            {
                this.adMaxValue = value;
            }
        }

        public void setChannelBuffers(int bufferSize)
        {
            channelBuffers = new short[2][];
            channelBuffers[0] = new short[bufferSize];
            channelBuffers[1] = new short[bufferSize];
            scaledData = new short[2][];
            scaledData[0] = new short[bufferSize];
            scaledData[1] = new short[bufferSize];
        }

        public void processFastStreamData()
        {
            double factor = (double)(double.Parse(voltageRange.ToString("d")) * scaleFactor) / adMaxValue;

            scaledData[0][0] = (short)(channelBuffers[0][0] * factor);
            scaledData[1][0] = (short)(channelBuffers[1][0] * factor);
            max = scaledData[0][0];
            min = scaledData[1][0];
            rms = max / Math.Sqrt(2);
            amplitud = max - min;
        }

        public void processData(int samples)
        {
            double maxTemp, minTemp;

            double factor = (double)(double.Parse(voltageRange.ToString("d")) * scaleFactor) / adMaxValue;

            for (int r = 0; r < samples; r++)
            {
                scaledData[0][r] = (short)(channelBuffers[0][r] * factor);
                maxTemp =  minTemp = scaledData[0][r];
                
                if (maxTemp > max)
                    max = maxTemp;
                if (minTemp < min)
                    min = minTemp;

            }
            
            rms = max / Math.Sqrt(2);
            amplitud = max - min;
        }

        public double ScaleFactor
        {
            set
            {
                this.scaleFactor = value;
            }
            get
            {
                return this.scaleFactor;
            }
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
