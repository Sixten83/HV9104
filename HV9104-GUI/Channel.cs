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
        Imports.Channel             channel;
        Imports.Coupling            coupling;
        Imports.Range               voltageRange;        
        Imports.ThresholdDirection  triggerType;
        short                       triggerLevel;
        short                       enabled;
        public short[][]            channelBuffers;
        public ScaledData[]         scaledData;        
        double                      dividerRatio = 1;
        double                      rms, max, min,amplitud,average;
        double[]                    representation;
        public int                  index;
        short                       adMaxValue;
        ushort[]                    inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000};
        double[]                    incrementValues;
        int                         incrementIndex;
        float                       dcOffset = 0;
        int                         polarity = 1;
        bool                        voltageAutoRange;
        int                         maxSamples = 100000;
        int                         stage = 1;
        string                      name;
        bool                        isAutoRangeable = true;        
        double[]                    voltsPerDiv = { 0.4, 1, 2, 4, 10, 20, 40};
        string[]                    selectibleVoltageList;


        public struct ScaledData
        {
            public double[] x;
            public double[] y;
        }


        public Channel()
        {
            this.coupling = Imports.Coupling.PS5000A_DC;
            this.voltageRange = Imports.Range.Range_1V;
            representation = new double[5];
            setIncrementValues();
            
        }

        public bool isSWOverFlow()
        {
            double factor = (((double)inputRanges[(int)voltageRange] * dividerRatio) / adMaxValue) / 1000;
            double rawMax = ((max + 1 * dcOffset) / factor);
            rawMax *= dividerRatio / (10 * (double)stage);

            if (rawMax > adMaxValue)
                return true;
            else
                return false;
        }
        
        public string[] getSelectibleVoltageList()
        {
            int i = 0 ,r = 0;

            if (isAutoRangeable)
            {
                selectibleVoltageList = new string[8];
                selectibleVoltageList[i++] = "Auto";
            }
            else
                selectibleVoltageList = new string[7];

            selectibleVoltageList[i++] = (voltsPerDiv[r++] * stage).ToString("0.0").Replace(',', '.') + " kV/Div";

            for (; i < selectibleVoltageList.Length; i++)
            {
                selectibleVoltageList[i] = (voltsPerDiv[r] * stage).ToString("0.").Replace(',', '.') + " kV/Div";
                r++;
            }
               
            return selectibleVoltageList;
        }

        public bool IsAutoRangeable
        {
            set
            {
                this.isAutoRangeable = value;
            }
            get
            {
                return this.isAutoRangeable;
            }
        }

        public string Name
        {
            set
            {
                this.name = value;
            }
            get
            {
                return this.name;
            }
        }

        public int Stage
        {
            set
            {
                this.stage = value;
            }
            get
            {
                return this.stage;
            }
        }

        public bool VoltageAutoRange
        {
            set
            {
                this.voltageAutoRange = value;
            }
            get
            {
                return this.voltageAutoRange;
            }
        }

        public double getRawMaxRatio()
        {
            double factor = (((double)inputRanges[(int)voltageRange] * dividerRatio) / adMaxValue) / 1000;
            double chartScaler = dividerRatio / (10 * (double)stage);
            return (chartScaler * (max + 1 * dcOffset) / factor) / adMaxValue;
        }

        public Imports.ThresholdDirection TriggerType
        {
            set
            {
                this.triggerType = value;
            }
            get
            {
                return this.triggerType;
            }
        }

        public double rangeToVolt()
        {
            return (double)inputRanges[(int)voltageRange]  /1000;
        }

        public short TriggerLevel
        {
            set
            {
                this.triggerLevel = value;
            }
            get
            {
                return this.triggerLevel;
            }
        }        

        public double triggerLevelkV()
        {
            double factor = (((double)inputRanges[(int)voltageRange] * dividerRatio) / adMaxValue) / 1000;
            double kVTriggerLevel = triggerLevel * factor - 1 * dcOffset * (float)dividerRatio;
            return kVTriggerLevel;
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

        private void setIncrementValues()
        {

            incrementValues = new double[maxSamples];
            int r = 0;

            for (; r < maxSamples; r++)
            {
                incrementValues[r] = r;
            }
         
        }       

        public double getScaleFactor()
        {
            double chartScaler = dividerRatio / (10 * (double)stage);
            return (((double)inputRanges[(int)voltageRange] * dividerRatio) / adMaxValue) / (1000 * chartScaler);
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
            max = (double)scaledData[0].y[0] - 1 * dcOffset * (float)dividerRatio;
            min = (double)scaledData[1].y[0] - 1 * dcOffset * (float)dividerRatio;
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
            max = factor * scaledData[0].y.Max() - 1 * dcOffset * (float)dividerRatio;
            min = factor * scaledData[0].y.Min() - 1 * dcOffset * (float)dividerRatio;
            rms = max / Math.Sqrt(2);
            amplitud = max - min;
            updateRepresentation();
        }       

        public ScaledData processData(int samples, int startIndex)
        {

            scaledData = new ScaledData[2];
            scaledData[0].y = new double[samples];
            scaledData[0].x = new double[samples];
            
            double factor = (((double)inputRanges[(int)voltageRange] * dividerRatio) / adMaxValue) / 1000;
            double chartScaler = dividerRatio / (10 * (double)stage);
            
            int r = startIndex;
            int i = 0;

            for (; r < startIndex + samples; r++)
            {
                scaledData[0].y[i++] = (double)channelBuffers[0][r] * chartScaler;     
            }
            
            Array.Copy(incrementValues, 0, scaledData[0].x, 0, samples);

            average = factor * scaledData[0].y.Sum() / (samples * chartScaler);
            max = factor * scaledData[0].y.Max() / chartScaler - 1 * dcOffset * (float)dividerRatio;
            min = factor * scaledData[0].y.Min() / chartScaler - 1 * dcOffset * (float)dividerRatio;    
                
            
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
