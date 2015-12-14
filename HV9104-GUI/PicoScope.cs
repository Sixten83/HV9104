using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HV9104_GUI

{
    class PicoScope
    {
        private short handle;
        private bool powerSupplyConnected = true;
        private Imports.DeviceResolution resolution;
        private Imports.Channel triggerChannel;
        private Imports.ps5000aBlockReady _callbackDelegate;
        
        private short triggerLevel;
        private Imports.ThresholdDirection triggerType;
        public Channel[] channels;   
        private short[][] buffers;
        public bool _autoStop;
        public uint _trigAt = 0;
        private uint _startIndex;
        private bool _ready = false;
        private bool _readyBlock = false;
        private int _sampleCount;
        private uint preTrigger;
        private short _trig = 0;
        public bool streamStarted = false;
        private int bufferSize = 1024 * 100;//1024 * 100; /*  *100 is to make sure buffer large enough */
        uint streamingSamples = 4000;
        uint streamingInterval = 25;
        private Imports.ReportedTimeUnits streamingIntervalUnits = Imports.ReportedTimeUnits.MicroSeconds;
        uint blockSamples = 2500;
        uint blockTimeBase = 1;
        short maxADValue = 32512;
        
       

        public PicoScope()
        {
            channels = new Channel[3];
            for (uint r = 0; r < 3; r++)
            {
                channels[r] = new Channel();
                channels[r].ChannelName = Imports.Channel.ChannelA + r;
                channels[r].setChannelBuffers(bufferSize);
            }
          
           buffers = new short[5][];

           triggerChannel = Imports.Channel.ChannelA;
           triggerLevel = 1000;
           triggerType = Imports.ThresholdDirection.Rising;
           
          /* channels[2].Enabled = 1;
           channels[2].Coupling = Imports.Coupling.PS5000A_DC;
           channels[2].VoltageRange = Imports.Range.Range_20V;
           Imports.SetChannel(handle, channels[2].ChannelName, channels[2].Enabled, channels[2].Coupling, channels[2].VoltageRange, 0);*/
           
        }

        public void setACChannel(Channel acChannel)
        {
            channels[0] = acChannel;
            channels[0].ChannelName = Imports.Channel.ChannelA + r;
            channels[0].setChannelBuffers(bufferSize);
        }


        public uint PreTrigger
        {
            set
            {
                this.preTrigger = value;
            }
            get
            {
                return this.preTrigger;
            }
            
        }

        public void stopStreaming()
        {
            Imports.Stop(handle);
        }

        public short getADMaxValue()
        {
            return maxADValue;
             
        }

        public void setTriggerChannel(Imports.Channel triggerChannel)
        {
            uint status;
            this.triggerChannel = triggerChannel;
            status = Imports.SetSimpleTrigger(handle, 1, this.triggerChannel, this.triggerLevel, this.triggerType, 0, 0);
            Console.WriteLine("TriggerChannel Status : {0} ", status);
        }

        public void setTriggerLevel(short triggerLevel)
        {
            uint status;
            this.triggerLevel = triggerLevel;
            status = Imports.SetSimpleTrigger(handle, 1, this.triggerChannel, this.triggerLevel, this.triggerType, 0, 0);
            Console.WriteLine("TriggerLevel Status : {0} ", status);
        }

        public void setTriggerType(Imports.ThresholdDirection triggerType)
        {
            uint status;
            this.triggerType = triggerType;
            status = Imports.SetSimpleTrigger(handle, 1, this.triggerChannel, this.triggerLevel, this.triggerType, 0, 0);
            Console.WriteLine("TriggerType Status : {0} ", status);
        }


        public void setFastStreamDataBuffer()
        {
               uint status;
               for (int ch = 0; ch < 4; ch += 2)
               {
                   buffers[ch] = new short[bufferSize];
                   buffers[ch + 1] = new short[bufferSize];
                   status = Imports.SetDataBuffers(handle, (Imports.Channel)(ch / 2), buffers[ch], buffers[ch + 1], bufferSize, 0, Imports.RatioMode.Aggregate);
                   Console.WriteLine("setFastStreamDataBuffer Status : {0} ", status);
               }
            
        }

        public void startFastStreaming()
        {

            uint status;

            streamStarted = true;
            _sampleCount = 0;
            _startIndex = 0;
            _trigAt = 0;
            _autoStop = false;
            status = Imports.RunStreaming(handle, ref streamingInterval, streamingIntervalUnits, preTrigger, streamingSamples - preTrigger, 1, streamingSamples, Imports.RatioMode.Aggregate, (uint)bufferSize);
                Console.WriteLine("RunStreaming Status : {0} ", status);
          if (_autoStop)
                Imports.Stop(handle);
        }

        public void getFastStreamValues()
        {
            uint status;
            status = Imports.GetStreamingLatestValues(handle, fastStreamingCallback, IntPtr.Zero);
        }

        public void fastStreamingCallback(short handle,
                                int noOfSamples,
                                uint startIndex,
                                short overflow,
                                uint triggerAt,
                                short triggered,
                                short autoStop,
                                IntPtr pVoid)
        {
            // used for streaming
            _sampleCount = noOfSamples;
            _startIndex = startIndex;
            _autoStop = autoStop != 0;

            _ready = true;

            // flags to show if & where a trigger has occurred
            _trig = triggered;

            if (_trig != 0)
                _trigAt = triggerAt;

            if (_sampleCount != 0)
            {
                for (uint ch = 0; ch < 2 * 2; ch += 2)
                {
                   Array.Copy(buffers[ch], _startIndex, channels[ch / 2].channelBuffers[0], _startIndex, _sampleCount); //max
                   Array.Copy(buffers[ch + 1], _startIndex, channels[ch / 2].channelBuffers[1], _startIndex, _sampleCount);//min
                }
            }

        }

        
        public void setStreamDataBuffers()
        {
            uint status;
            for (int ch = 0; ch < 2; ch++)
            {
                buffers[ch] = new short[bufferSize];               
                status = Imports.SetDataBuffer(handle, (Imports.Channel)(ch), buffers[ch], bufferSize, 0, Imports.RatioMode.None);
                Console.WriteLine("DataBuffers Status : {0} ", status);
            }

        }

        public void startStreaming()
        {

             uint status;

             streamStarted = true;
             _sampleCount = 0;
             _startIndex = 0;
            _trigAt = 0;
            _autoStop = false;
            for (int ch = 0; ch < 2; ch++)
            {
                status = Imports.RunStreaming(handle, ref streamingInterval, streamingIntervalUnits, preTrigger, streamingSamples - preTrigger, 1, 1, Imports.RatioMode.None, (uint)bufferSize);
            }
            if (_autoStop)
                Imports.Stop(handle);
        }

  
        public void getStreamValues()
        {
            uint status;
            status = Imports.GetStreamingLatestValues(handle, streamingCallback, IntPtr.Zero);
        }

        

        //Callback funktion for Streaming
        public void streamingCallback(short handle,
                                int noOfSamples,
                                uint startIndex,
                                short overflow,
                                uint triggerAt,
                                short triggered,
                                short autoStop,
                                IntPtr pVoid)
        {
            // used for streaming
            _sampleCount = noOfSamples;
            _startIndex = startIndex;
            _autoStop = autoStop != 0;
           
            _ready = true;
           
            // flags to show if & where a trigger has occurred
            _trig = triggered;
            
            if(_trig != 0)
               _trigAt = triggerAt;
            
                
            if (_sampleCount != 0 )
            {
               
         
                        //Array.Copy(buffers[0], _startIndex, appBuffers[0], _startIndex, _sampleCount); //max
                        //Array.Copy(buffers[0 + 1], _startIndex, appBuffers[0 + 1], _startIndex, _sampleCount);//min
            }
              
        }
       

        //Data buffer setup for captureing Lightning Impulse 
        public void setBlockDataBuffer()
        {
            uint status;

            buffers[4] = new short[bufferSize];
            status = Imports.SetDataBuffer(handle, Imports.Channel.ChannelC, buffers[4], bufferSize, 0, Imports.RatioMode.None);
          
            Console.WriteLine("BlockDataBuffers Status : {0} ", status);

        }

        public void startBlock()
        {
            int timeInterval =0;
            int maxSamples = 0;
            uint status;
            while ((status = Imports.GetTimebase(handle, blockTimeBase, (int)blockSamples, out timeInterval, out maxSamples, 0)) != 0)
            {
                Console.WriteLine("Timebase selection\n" + status);
                blockTimeBase++;

            }
            Console.WriteLine("timeInterval:" + timeInterval + " maxSamples" + maxSamples + " blockTimeBase:" + blockTimeBase);

            _readyBlock = false;
            _callbackDelegate = BlockCallback;
          
            int timeIndisposed;
            status = Imports.RunBlock(handle, 0, (int)blockSamples, blockTimeBase, out timeIndisposed, 0, _callbackDelegate, IntPtr.Zero);
            Console.WriteLine("RunBlock Status:" + status);
        }

        public uint getBlockValues()
        {
            short overflow;
            uint status = 1;

                       
            Imports.Stop(handle);

            if (_readyBlock)
            {
                status = Imports.GetValues(handle, 0, ref blockSamples, 1, Imports.DownSamplingMode.None, 0, out overflow);
                if (status == (short)Imports.PICO_OK)
                {
                    Array.Copy(buffers[4], 0, channels[2].channelBuffers[0], 0, blockSamples);
                }
            }

            return status;
        }

        

        private void BlockCallback(short handle, short status, IntPtr pVoid)
        {
            // flag to say done reading data
            if (status != (short)Imports.PICO_CANCELLED)
                _readyBlock = true;
        }

        public void setChannelVoltageRange(int index, Imports.Range voltageRange)
        {
            uint status;
            channels[index].VoltageRange = voltageRange;
            status = Imports.SetChannel(handle, channels[index].ChannelName, channels[index].Enabled, channels[index].Coupling, channels[index].VoltageRange, 0);
            Console.WriteLine("VoltageRange Status: {0}", status);     
        }

        public void setCouplingType(int index, Imports.Coupling coupling)
        {
            uint status;
            channels[index].Coupling = coupling;
            status = Imports.SetChannel(handle, channels[index].ChannelName, channels[index].Enabled, channels[index].Coupling, channels[index].VoltageRange, 0);
            Console.WriteLine("Coupling Status: {0}", status); 
        }

        public void enableChannel(int index)
        {
            uint status;
            channels[index].Enabled = 1;
            status = Imports.SetChannel(handle, channels[index].ChannelName, channels[index].Enabled, channels[index].Coupling, channels[index].VoltageRange, 0);
            Console.WriteLine("Enable Status: {0}", status); 
        }

        public void disableChannel(int index)
        {
            uint status;
            channels[index].Enabled = 0;
            status = Imports.SetChannel(handle, channels[index].ChannelName, channels[index].Enabled, channels[index].Coupling, channels[index].VoltageRange, 0);
            Console.WriteLine("Disable Status: {0}", status); 
        }

        public Imports.DeviceResolution Resolution
        {
            set
            {
                this.resolution = value;
                uint status;

                status = Imports.SetDeviceResolution(handle, resolution);
                Console.WriteLine("DeviceResolution Status: {0}", status);
                status = Imports.MaximumValue(handle, out maxADValue);
                Console.WriteLine("MaximumValue Status: {0}", status);
            }
            get
            {
                return this.resolution;
            }
        }

        public short Handle
        {
            get
            {
                return handle;
            }
        }

        public void startSignalGen(float frequency)
        {
            uint status;
            status = Imports.SetSigGenBuiltIn(handle, 2000000, 4000000, Imports.WaveType.PS5000A_SQUARE, frequency, frequency, 0, 1, Imports.SweepType.PS5000A_UP, Imports.ExtraOperations.PS5000A_ES_OFF, 0, 0, 0, Imports.SigGenTrigSource.PS5000A_SIGGEN_NONE, 0);
            Console.WriteLine("SetSigGenBuiltIn Status:" + status);
        
        }

        public void openDevice() 
        {
            
            uint status = Imports.OpenUnit(out handle, null, Imports.DeviceResolution.PS5000A_DR_8BIT);
            
        
            if (status != Imports.PICO_OK && handle != 0)
            {
                status = Imports.ChangePowerSource(handle, status);
                powerSupplyConnected = false;
            }
            
            if (status != Imports.PICO_OK)
            {
                Console.WriteLine("Unable to open device");
            }
        
        }

        public void closeDevice()
        {
            Imports.CloseUnit(handle);
            handle = 0;
        }

    }
}
