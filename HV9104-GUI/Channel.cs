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
        double              dividerRatio = 1;
        double              rms, max, min,amplitud,average;
        double[]            representation;
        public int                 index;
        short               adMaxValue;
        ushort[]            inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000 };
        double[][]          incrementValues;
        int                 incrementIndex;
        float               dcOffset = 0;
        int                 polarity = 1;

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
            setIncrementValues();
        }

        public int rangeToVolt()
        {
            return inputRanges[(int)voltageRange]  /1000;
        }

        public int Polarity
        {
            set
            {
                this.polarity = value;
            }
            get
            {
                return this.polarity;
            }
        }

        public float DCOffset
        {   
            set
            {
                this.dcOffset = value;
            }
            get
            {
                return this.dcOffset;
            }
        }

        public int IncrementIndex
        {
            set
            {    
                this.incrementIndex = value;
            }
        }

        private void setIncrementValues()
        {
            incrementValues = new double[9][];

            //if (e.Text.Equals("2 ms/Div"))0,0001
                
                int r = 0;
                decimal increment;

                incrementValues[0] = new double[1600];
                //X values for 2 ms/Div
                
                for(increment = -10; increment < 10 ; increment += 0.0125m )
                {
                    incrementValues[0][r++] = (double)increment;
                }

               
                incrementValues[1] = new double[1600];
                //X values for 5 ms/Div
                for (increment = -25, r = 0; increment < 25; increment += 0.03125m)
                {
                    incrementValues[1][r++] = (double)increment;
                }

                incrementValues[2] = new double[1600];
                //X values for 10 ms/Div
                for (increment = -50, r = 0; increment < 50; increment += 0.0625m)
                {
                    incrementValues[2][r++] = (double)increment;
                }

                incrementValues[3] = new double[1000];
                //X values for 200 ns/Div
                for (increment = -1, r = 0; increment < 1; increment += 0.002m)
                {
                    incrementValues[3][r++] = (double)increment;
                }

                incrementValues[4] = new double[2500];
                //X values for 500 ns/Div
                for (increment = -2.5m, r = 0; increment < 2.5m; increment += 0.002m)
                {
                    incrementValues[4][r++] = (double)increment;
                }

                incrementValues[5] = new double[5000];
                //X values for 1 us/Div
                for (increment = -5, r = 0; increment < 5; increment += 0.002m)
                {
                    incrementValues[5][r++] = (double)increment;
                }

                incrementValues[6] = new double[10000];
                //X values for 2 us/Div
                for (increment = -10, r = 0; increment < 10; increment += 0.002m)
                {
                    incrementValues[6][r++] = (double)increment;
                }

                incrementValues[7] = new double[25000];
                //X values for 5 us/Div
                for (increment = -25, r = 0; increment < 25; increment += 0.002m)
                {
                    incrementValues[7][r++] = (double)increment;
                }

                incrementValues[8] = new double[50000];
                //X values for 10 us/Div
                for (increment = -50, r = 0; increment < 50; increment += 0.002m)
                {
                    incrementValues[8][r++] = (double)increment;
                }
         
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

        public double getScaleFactor()
        {
            return (((double)inputRanges[(int)voltageRange] * dividerRatio) / adMaxValue) / 1000;
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
            double factor = (((double)inputRanges[(int)voltageRange] * dividerRatio) / adMaxValue) / 1000;

            scaledData = new ScaledData[2];
            scaledData[0].y = new double[1];
            scaledData[1].y = new double[1];


            scaledData[0].y[0] = (double)(channelBuffers[0][0] * factor);
            scaledData[1].y[0] = (double)(channelBuffers[1][0] * factor);
            max = (double)scaledData[0].y[0] - 1 * dcOffset;
            min = (double)scaledData[1].y[0] - 1 * dcOffset;
            rms = max / Math.Sqrt(2);
            amplitud = max - min;
            average = (max + min) / 2;
            updateRepresentation();
           
        }

        public void processMaxMinData(int samples, int startIndex)
        {
            scaledData = new ScaledData[2];            
            scaledData[0].y = new double[samples];
            double factor = (((double)inputRanges[(int)voltageRange] * dividerRatio) / adMaxValue) / 1000;
            Array.Copy(channelBuffers[0], startIndex, scaledData[0].y, 0, samples);
            average = factor * scaledData[0].y.Sum() / samples;
            max = factor * scaledData[0].y.Max() - 1 * dcOffset;
            min = factor * scaledData[0].y.Min() - 1 * dcOffset;
            rms = max / Math.Sqrt(2);
            amplitud = max - min;
            updateRepresentation();
        }

        

        public ScaledData processData(int samples, int startIndex)
        {
            scaledData = new ScaledData[2];
            scaledData[0].x = new double[samples];
            scaledData[0].y = new double[samples];

            double factor = (((double)inputRanges[(int)voltageRange] * dividerRatio) / adMaxValue) / 1000;            
            Array.Copy(channelBuffers[0], startIndex, scaledData[0].y, 0, samples);
            Array.Copy(incrementValues[incrementIndex], 0, scaledData[0].x, 0, samples);
            average = factor * scaledData[0].y.Sum() / samples;
            max = factor * scaledData[0].y.Max() - 1 * dcOffset;
            min = factor * scaledData[0].y.Min() - 1 * dcOffset;
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


        public double DividerRatio
        {
            set
            {
                this.dividerRatio = value;
            }
            get
            {
                return this.dividerRatio;
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
