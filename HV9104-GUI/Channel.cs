using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HV9104_GUI
{
    
    

    public class Channel
    {
        Imports.Channel     channel;
        Imports.Coupling    coupling;
        Imports.Range       voltageRange;
        short               enabled;
        public short[][]    channelBuffers;
        public ScaledData[] scaledData;        
        double              scaleFactor = 1;
        double              rms, max, min,amplitud,average;
        double[]            representation;
        public int                 index;
        short               adMaxValue;
        ushort[]            inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000 };
        
        public struct ScaledData
        {
            public double[] x;
            public double[] y;
        }


        public Channel()
        {
            this.coupling = Imports.Coupling.PS5000A_DC;
            this.voltageRange = Imports.Range.Range_20V;
            representation = new double[5];
            
        }

        public double Average
        {
            get
            {
                return this.average;
            }
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
            
        }

        private void updateRepresentation()
        {
            representation[0] = max;
            representation[1] = min;
            representation[2] = rms;
            representation[3] = amplitud;
            representation[4] = average; 

        }

        public void processFastStreamData()
        {
            double factor = ((double)inputRanges[(int)voltageRange] * scaleFactor) / adMaxValue;

            scaledData = new ScaledData[2];
            scaledData[0].y = new double[1];
            scaledData[1].y = new double[1];


            scaledData[0].y[0] = (short)(channelBuffers[0][0] * factor);
            scaledData[1].y[0] = (short)(channelBuffers[1][0] * factor);
            max = (double)scaledData[0].y[0] / 1000;
            min = (double)scaledData[1].y[0] / 1000;
            rms = max / Math.Sqrt(2);
            amplitud = max - min;
            average = (max + min) / 2;
            updateRepresentation();
           
        }

        public void processMaxMinData(int samples, int startIndex)
        {
            scaledData = new ScaledData[2];
            scaledData[0].x = new double[samples];
            scaledData[0].y = new double[samples];
            scaledData[1].x = new double[samples];
            scaledData[1].y = new double[samples];

            double maxTemp, minTemp = 0;            
            double factor = ((double)inputRanges[(int)voltageRange] * scaleFactor) / adMaxValue;
            max = -adMaxValue;
            min = adMaxValue;
            average = 0;
            int r = startIndex;
            for (; r < startIndex + samples; r++)
            {
                
                maxTemp = minTemp = channelBuffers[0][r];
                average += channelBuffers[0][r];
                if (maxTemp > max)
                    max = maxTemp;
                if (minTemp < min)
                    min = minTemp;


               
            }

            average = (average * factor)/ (r - startIndex - 1);
            average /= 1000;
            max = (max * factor) / 1000;
            min = (min * factor) / 1000;
            rms = max / Math.Sqrt(2);
            amplitud = max - min;
            updateRepresentation();

        }

        public ScaledData processData(int samples, int startIndex, double increment, double timeBase)
        {
            double maxTemp, minTemp;
            int maxSamples = (int)((10 * timeBase)/increment) + 1;
            scaledData = new ScaledData[2];
            scaledData[0].x = new double[maxSamples];
            scaledData[0].y = new double[maxSamples];
            scaledData[1].x = new double[maxSamples];
            scaledData[1].y = new double[maxSamples];
            double xStart = -(timeBase * 10) / 2;
            double xStop = (timeBase * 10) / 2;

            

            //double factor = ((double)inputRanges[(int)voltageRange] * scaleFactor ) / adMaxValue;
            double factor = ((double)inputRanges[10] * scaleFactor) / adMaxValue;
            
            max = -(double)inputRanges[(int)voltageRange] * scaleFactor;
            min = (double)inputRanges[(int)voltageRange] * scaleFactor;
            average = 0;
            int r = startIndex;
            for (; xStart <= xStop; r++)
            {
               
                scaledData[0].y[r - startIndex] = ((double)channelBuffers[0][r] * factor) / 1000;
                
                maxTemp =  minTemp = scaledData[0].y[r - startIndex];
                average += scaledData[0].y[r - startIndex]; 
                if (maxTemp > max)
                    max = maxTemp;
                if (minTemp < min)
                    min = minTemp;
                scaledData[0].x[r - startIndex] = xStart;
                
                xStart += increment;
            }

            scaledData[0].x[maxSamples - 1] = scaledData[0].x[maxSamples - 2];
            scaledData[0].y[maxSamples - 1] = scaledData[0].y[maxSamples - 2];

            double scaleToRange = (double)inputRanges[(int)voltageRange] / (double)inputRanges[10];
            max = max * scaleToRange;
            min = min * scaleToRange;

            average = average * scaleToRange / (r - startIndex - 1);
            
            rms = max / Math.Sqrt(2);
            amplitud = max - min;
            
            updateRepresentation();

            return scaledData[0];
        }

        public void setRepresentationIndex(int index)
        {
            this.index = index;
        }
        public double getRepresentation()
        {
            return this.representation[index];
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
