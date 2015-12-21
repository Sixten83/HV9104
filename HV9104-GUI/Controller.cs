using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace HV9104_GUI
{

     class Controller
    {
        MeasuringForm measuringForm;
        ControlForm controlForm;
        PicoScope picoScope;
        Channel acChannel, dcChannel, impulseChannel;
        Timer loopTimer, triggerTimer;
        bool fastStream,stream;
        bool blockCapture;
        //Voltage Dividers
        decimal[] acHighDividerValues = { 101.27M, 106.7M };
        decimal acLowDividerValue = 2934000;
        decimal[] dcHighDividerValues = { 280.49M, 280.21M, 280.79M };
        decimal dcLowDividerValue = 0.027997M;
        decimal[] impulseHighDividerValues = { 1.302M, 1.2714M, 1.2638M };
        decimal[] impulseLowDividerValues = { 519.498M, 513.963M, 512.21M };
        decimal impulseAttenuatorRatio = 25.1448M; 
        
        public Controller()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            measuringForm = new MeasuringForm();
            controlForm = new ControlForm();            
            controlForm.startMeasuringForm(measuringForm);

            //***********************************************************************************************************
            //***                                     PICOSCOPE AND CHANNELS SETUP                                   ****
            //***********************************************************************************************************

            picoScope = new PicoScope();            
            uint status = picoScope.openDevice();
            if (status != Imports.PICO_OK)
            {
                Console.WriteLine("Unable to open device");
            }
            picoScope.setACChannel(acChannel = new Channel());
            picoScope.setDCChannel(dcChannel = new Channel());
            picoScope.setImpulseChannel(impulseChannel = new Channel());
            acChannel.DividerRatio = (double)((acHighDividerValues[0] + acLowDividerValue) / acHighDividerValues[0]) / 1000;
            dcChannel.DividerRatio = (double)((dcHighDividerValues[0] + dcLowDividerValue) / dcLowDividerValue) / 1000;
            impulseChannel.DividerRatio = (double)(impulseAttenuatorRatio * (impulseHighDividerValues[0] + impulseLowDividerValues[0]) / impulseLowDividerValues[0]) / 1000;
            picoScope.setTriggerChannel(Imports.Channel.ChannelA);
            picoScope.Resolution = Imports.DeviceResolution.PS5000A_DR_12BIT;
            picoScope.setFastStreamDataBuffer();
            fastStream = true;
            loopTimer = new Timer();
            loopTimer.Tick += new System.EventHandler(this.loopTimer_Tick);
            loopTimer.Interval = 10;
            triggerTimer = new Timer();
            triggerTimer.Tick += new System.EventHandler(this.triggerTimer_Tick);
            triggerTimer.Interval = 3000;
            this.measuringForm.chart.cursorMenu.setScaleFactor(acChannel.getScaleFactor(), acChannel.DCOffset);
            
            /////////////////
            this.measuringForm.triggerWindow.okButton.Click += new System.EventHandler(this.triggerMenuOkButton_Click);
            this.measuringForm.closeButton.Click += new System.EventHandler(this.formsCloseButton_Click);
            this.controlForm.closeButton.Click += new System.EventHandler(this.formsCloseButton_Click);

            //***********************************************************************************************************
            //***                                     SETUP VIEW EVENT LISTENERS                                     ****
            //***********************************************************************************************************
            //Mode selection listeners
            this.controlForm.setupView.manualModeRadioButton.Click += new System.EventHandler(manualModeRadiobutton_Click);
            this.controlForm.setupView.acWithstandRadioButton.Click += new System.EventHandler(acWithstandRadioButton_Click);
            this.controlForm.setupView.acDisruptiveRadioButton.Click += new System.EventHandler(acDisruptiveRadioButton_Click);
            this.controlForm.setupView.dcWithstandRadioButton.Click += new System.EventHandler(dcWithstandRadioButton_Click);
            this.controlForm.setupView.dcDisruptiveRadioButton.Click += new System.EventHandler(dcDisruptiveRadioButton_Click);
            this.controlForm.setupView.impulseWithstandRadioButton.Click += new System.EventHandler(impulseWithstandRadioButton_Click);
            this.controlForm.setupView.impulseDisruptiveRadioButton.Click += new System.EventHandler(impulseDisruptiveRadioButton_Click);
            //Stage setup listeners
            this.controlForm.setupView.acCheckBox.Click += new System.EventHandler(acCheckBox_Click);
            this.controlForm.setupView.acStage1RadioButton.Click += new System.EventHandler(acStage1RadioButton_Click);
            this.controlForm.setupView.acStage2RadioButton.Click += new System.EventHandler(acStage2RadioButton_Click);
            this.controlForm.setupView.acStage3RadioButton.Click += new System.EventHandler(acStage3RadioButton_Click);
            this.controlForm.setupView.dcCheckBox.Click += new System.EventHandler(dcCheckBox_Click);
            this.controlForm.setupView.dcStage1RadioButton.Click += new System.EventHandler(dcStage1RadioButton_Click);
            this.controlForm.setupView.dcStage2RadioButton.Click += new System.EventHandler(dcStage2RadioButton_Click);
            this.controlForm.setupView.dcStage3RadioButton.Click += new System.EventHandler(dcStage3RadioButton_Click);
            this.controlForm.setupView.impulseCheckBox.Click += new System.EventHandler(impulseCheckBox_Click);
            this.controlForm.setupView.impulseStage1RadioButton.Click += new System.EventHandler(impulseStage1RadioButton_Click);
            this.controlForm.setupView.impulseStage2RadioButton.Click += new System.EventHandler(impulseStage2RadioButton_Click);
            this.controlForm.setupView.impulseStage3RadioButton.Click += new System.EventHandler(impulseStage3RadioButton_Click);
            //***********************************************************************************************************
            //***                                     RUN VIEW EVENT LISTENERS                                       ****
            //***********************************************************************************************************
            //Output Representation Listeners
            this.controlForm.runView.acOutputComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(acOutputComboBox_valueChange);
            this.controlForm.runView.dcOutputComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(dcOutputComboBox_valueChange);
            this.controlForm.runView.impulseOutputComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(impulseOutputComboBox_valueChange);         
            //POWER Listeners
            this.controlForm.runView.onOffButton.Click += new System.EventHandler(onOffButton_Click);
            this.controlForm.runView.pauseButton.Click += new System.EventHandler(pauseButton_Click);
            this.controlForm.runView.parkCheckBox.Click += new System.EventHandler(parkCheckBox_Click);
            this.controlForm.runView.overrideCheckBox.Click += new System.EventHandler(overrideCheckBox_Click);
            //Regulated Voltage Type Listeners            
            this.controlForm.runView.inputVoltageRadioButton.Click += new System.EventHandler(inputVoltageRadioButton_Click);
            this.controlForm.runView.acOutputRadioButton.Click += new System.EventHandler(acVoltageRadioButton_Click);
            this.controlForm.runView.dcVoltageRadioButton.Click += new System.EventHandler(dcVoltageRadioButton_Click);
            //Set Voltage Listeners            
            this.controlForm.runView.decreaseRegulatedVoltageButton.MouseDown += new System.Windows.Forms.MouseEventHandler(decreaseRegulatedVoltageButton_Down);
            this.controlForm.runView.decreaseRegulatedVoltageButton.MouseUp+= new System.Windows.Forms.MouseEventHandler(decreaseRegulatedVoltageButton_Up);
            this.controlForm.runView.regulatedVoltageTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(regulatedVoltageTextBox_valueChange);
            this.controlForm.runView.increaseRegulatedVoltageButton.MouseDown += new System.Windows.Forms.MouseEventHandler(increaseRegulatedVoltageButton_Down);
            this.controlForm.runView.increaseRegulatedVoltageButton.MouseUp += new System.Windows.Forms.MouseEventHandler(increaseRegulatedVoltageButton_Up);
            this.controlForm.runView.incrementButton.Click += new System.EventHandler(incrementButton_Click);
            this.controlForm.runView.voltageRegulationRepresentationComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(voltageRegulationRepresentationComboBox_valueChange);
            //Impuse Trigger Control Listeners            
            this.controlForm.runView.triggerButton.Click += new System.EventHandler(triggerButton_Click);
            this.controlForm.runView.choppingCheckBox.Click += new System.EventHandler(choppingCheckBox_Click);
            this.controlForm.runView.decreaseChoppingTimeButton.MouseDown += new System.Windows.Forms.MouseEventHandler(decreaseChoppingTimeButton_Down);
            this.controlForm.runView.decreaseChoppingTimeButton.MouseUp += new System.Windows.Forms.MouseEventHandler(decreaseChoppingTimeButton_Up);
            this.controlForm.runView.choppingTimeTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(choppingTimeTextBox_valueChange);
            this.controlForm.runView.increaseChoppingTimeButton.MouseDown += new System.Windows.Forms.MouseEventHandler(increaseImpulseGapButton_Down);
            this.controlForm.runView.increaseChoppingTimeButton.MouseUp += new System.Windows.Forms.MouseEventHandler(increaseChoppingTimeButton_Up);
            //Measuring Sphere Gap Listeners
            this.controlForm.runView.decreaseMeasuringGapButton.MouseDown += new System.Windows.Forms.MouseEventHandler(decreaseMeasureeGap_Down);
            this.controlForm.runView.decreaseMeasuringGapButton.MouseUp += new System.Windows.Forms.MouseEventHandler(decreaseMeasureGap_Up);
            this.controlForm.runView.measuringGapTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(measureGapTextBox_valueChange);
            this.controlForm.runView.increaseMeasuringGapButton.MouseDown += new System.Windows.Forms.MouseEventHandler(increaseMeasureGapButton_Down);
            this.controlForm.runView.increaseMeasuringGapButton.MouseUp += new System.Windows.Forms.MouseEventHandler(increaseMeasureGapButton_Up);
            //Impulse Sphere Gap Listeners
            this.controlForm.runView.decreaseImpulseGapButton.MouseDown += new System.Windows.Forms.MouseEventHandler(decreaseImpulseGap_Down);
            this.controlForm.runView.decreaseImpulseGapButton.MouseUp += new System.Windows.Forms.MouseEventHandler(decreaseImpulseGap_Up);
            this.controlForm.runView.impulseGapTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(impulseGapTextBox_valueChange);
            this.controlForm.runView.increaseImpulseGapButton.MouseDown += new System.Windows.Forms.MouseEventHandler(increaseImpulseGapButton_Down);
            this.controlForm.runView.increaseImpulseGapButton.MouseUp += new System.Windows.Forms.MouseEventHandler(increaseImpulseGapButton_Up);
            //Pressure Control Listeners
            this.controlForm.runView.decreasePressureButton.MouseDown += new System.Windows.Forms.MouseEventHandler(decreasePressureButton_Down);
            this.controlForm.runView.decreasePressureButton.MouseUp += new System.Windows.Forms.MouseEventHandler(decreasePressureButton_Up);
            this.controlForm.runView.pressureTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(pressureTextBox_valueChange);
            this.controlForm.runView.increasePressureButton.MouseDown += new System.Windows.Forms.MouseEventHandler(increasePressureButton_Down);
            this.controlForm.runView.increasePressureButton.MouseUp += new System.Windows.Forms.MouseEventHandler(increasePressureButton_Up);

            //***********************************************************************************************************
            //***                                  MEASURING FORM EVENT LISTENERS                                   *****
            //***********************************************************************************************************
            //Input Listeners
            this.measuringForm.acdcRadioButton.Click += new System.EventHandler(acdcRadioButton_Click);
            this.measuringForm.impulseRadioButton.Click += new System.EventHandler(impulseRadioButton_Click);
            //AC Channel Listeners
            this.measuringForm.acVoltageRangeComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(acVoltageRangeComboBox_valueChange);
            this.measuringForm.acEnableCheckBox.Click += new System.EventHandler(acEnableCheckBox_Click);
            //DC Channel Listeners
            this.measuringForm.dcVoltageRangeComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(dcVoltageRangeComboBox_valueChange);
            this.measuringForm.dcEnableCheckBox.Click += new System.EventHandler(dcEnableCheckBox_Click);
            //Impulse Channel Listeners
            this.measuringForm.impulseVoltageRangeComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(impulseVoltageRangeComboBox_valueChange);
            this.measuringForm.impulseEnableCheckBox.Click += new System.EventHandler(impulseEnableCheckBox_Click);
            //Common Controls Listeners
            this.measuringForm.resolutionComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(resolutionComboBox_valueChange);
            this.measuringForm.timeBaseComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(timeBaseComboBox_valueChange);
            this.measuringForm.triggerSetupButton.Click += new System.EventHandler(triggerSetupButton_Click);

            //***********************************************************************************************************
            //***                                  CURSOR MENU EVENT LISTENERS                                      *****
            //***********************************************************************************************************
            this.measuringForm.chart.cursorMenu.acChannelRadioButton.Click += new System.EventHandler(this.acChannelRadioButton_Click);
            this.measuringForm.chart.cursorMenu.dcChannelRadioButton.Click += new System.EventHandler(this.dcChannelRadioButton_Click);
 
            loopTimer.Start();
            Application.Run(controlForm); // Måste vara sist!!!
            
        }    
       
         //***********************************************************************************************************
        //***                                    PROGRAM LOOP                                                   *****
        //***********************************************************************************************************
        private void loopTimer_Tick(object sender, EventArgs e)
        {
           
            if(fastStream)
            {
                if (picoScope._autoStop)
                {
                    if (picoScope._overflow == 0)
                    {
                        acChannel.processFastStreamData();
                        this.controlForm.runView.acValueLabel.Text = "" + acChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                        dcChannel.processFastStreamData();
                        this.controlForm.runView.dcValueLabel.Text = "" + dcChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                        
                    }
                    else
                        autoSetVoltageRange();
                    picoScope.streamStarted = false;
                    picoScope._autoStop = false;
                }
                else if(!picoScope.streamStarted)
                {
                   picoScope.startFastStreaming();
                }
                else if (!picoScope._autoStop && picoScope.streamStarted)
                {
                    picoScope.getFastStreamValues();
                }                
            }    
            if(stream)
            {
                if (picoScope._autoStop)
                {
                    int trigAt = (int)picoScope._trigAt;
                    this.measuringForm.chart.Series.SuspendUpdates();
                        if(this.measuringForm.acEnableCheckBox.isChecked)
                        {
                           // Console
                            this.measuringForm.chart.Series["acSeries"].Points.Clear();
                            Channel.ScaledData data = acChannel.processData(1000, trigAt);
                           
                            this.measuringForm.chart.Series["acSeries"].Points.DataBindXY(data.x,data.y);
                           
                            
                            this.controlForm.runView.acValueLabel.Text = "" + acChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                        }
                        else
                        {
                            acChannel.processMaxMinData(1600, trigAt);
                            this.controlForm.runView.acValueLabel.Text = "" + acChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                        }

                        if (this.measuringForm.dcEnableCheckBox.isChecked)
                        {
                            this.measuringForm.chart.Series["dcSeries"].Points.Clear();
                            Channel.ScaledData data = dcChannel.processData(1000, trigAt);
                            this.measuringForm.chart.Series["dcSeries"].Points.DataBindXY(data.x, data.y);
                            this.controlForm.runView.dcValueLabel.Text = "" + dcChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                        }
                        else
                        {
                            dcChannel.processMaxMinData(1600, trigAt);
                            this.controlForm.runView.dcValueLabel.Text = "" + dcChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                        }
                        this.measuringForm.chart.Series.ResumeUpdates();
                        this.measuringForm.chart.Series.Invalidate();
                       
                    picoScope.streamStarted = false;
                    picoScope._autoStop = false;
                }
                else if (!picoScope.streamStarted)
                {
                    picoScope.startStreaming();
                }
                else if (!picoScope._autoStop && picoScope.streamStarted)
                {
                    picoScope.getStreamValues();
                }    
            }
            if (blockCapture)
            {
                if(picoScope.getBlockValues() == 0)
                {
                    triggerTimer.Stop();
                    rebootStreaming();
                    if (this.measuringForm.impulseRadioButton.isChecked)
                    {
                        this.measuringForm.chart.Series["impulseSeries"].Points.Clear();
                        Channel.ScaledData data = impulseChannel.processData((int)picoScope.BlockSamples, 0);
                        this.measuringForm.chart.Series["impulseSeries"].Points.DataBindXY(data.x, data.y);
                        this.controlForm.runView.impulseValueLabel.Text = "" + impulseChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                    }
                    else
                    {
                        impulseChannel.processMaxMinData((int)picoScope.BlockSamples, 0);
                        this.controlForm.runView.impulseValueLabel.Text = "" + impulseChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                    }
                }
            }
        }

        //***********************************************************************************************************
        //***                                    PICOSCOPE RUTINES                                              *****
        //***********************************************************************************************************
        private void triggerTimer_Tick(object sender, EventArgs e)
        {
            triggerTimer.Stop();
            rebootStreaming();
        }

         public void rebootStreaming()
        {
            //Stop streaming
            picoScope.stopStreaming();
            blockCapture = false;
            picoScope.enableChannel(0);
            picoScope.enableChannel(1);
            picoScope.disableChannel(2);
            picoScope.setTriggerChannel(Imports.Channel.ChannelA);                       
            picoScope.setTriggerLevel(1000);
            picoScope.setTriggerType(Imports.ThresholdDirection.Rising);
            if (this.measuringForm.acEnableCheckBox.isChecked || this.measuringForm.dcEnableCheckBox.isChecked)
            {
                picoScope.setStreamDataBuffers();
                stream = true;
            }
            else
            {
                picoScope.setFastStreamDataBuffer();
                fastStream = true;
            }
            picoScope.streamStarted = false;
            picoScope._autoStop = false;
        }

        public void pauseStream()
        {
            //fastStream = false;
            picoScope.stopStreaming();
        }

        public void rebootStream()
        {

            picoScope.streamStarted = false;
            picoScope._autoStop = false;
           // fastStream = true;
        }

        public void autoSetVoltageRange()
        {
            pauseStream();
            int ch = (int)picoScope._overflow & 1;
            if(ch > 0)
            {
                acChannel.VoltageRange++;
                picoScope.setChannelVoltageRange(0, acChannel.VoltageRange);
    
            }

            ch = (int)picoScope._overflow & 2;
            if (ch > 0)
            {
                dcChannel.VoltageRange++;
                picoScope.setChannelVoltageRange(1, dcChannel.VoltageRange);
            }
            rebootStream();
            
        }

        //***********************************************************************************************************
        //***                                    MEASURING FORM EVENT HANDLERS                                  *****
        //***********************************************************************************************************

        private void acdcRadioButton_Click(object sender, EventArgs e)
        {
            picoScope.TimePerDivision = 5;
            this.measuringForm.chart.setVoltsPerDiv(6502.4);
            this.measuringForm.chart.setTimePerDiv(5);
            acChannel.IncrementIndex = 1;
            dcChannel.IncrementIndex = 1;  
            this.measuringForm.chart.Series["impulseSeries"].Points.Clear();
            this.measuringForm.chart.cursorMenu.acChannelRadioButton.isChecked = true;
            this.measuringForm.chart.cursorMenu.dcChannelRadioButton.isChecked = false;
            this.measuringForm.chart.cursorMenu.setScaleFactor(acChannel.getScaleFactor(), acChannel.DCOffset);
            this.measuringForm.chart.cursorMenu.resizeUp();
            this.measuringForm.chart.updateCursorMenu();
            this.measuringForm.timeBaseComboBox.setCollection = new string[] {
        "2 ms/Div",
        "5 ms/Div",
        "10 ms/Div"};
            this.measuringForm.timeBaseComboBox.SetSelected = "5 ms/Div";
        }

        private void impulseRadioButton_Click(object sender, EventArgs e)
        {
            stream = false;
            picoScope.setFastStreamDataBuffer();
            picoScope.TimePerDivision = 0.5;
            this.measuringForm.chart.setVoltsPerDiv(6502.4);
            this.measuringForm.chart.setTimePerDiv(0.5);
            impulseChannel.IncrementIndex = 4;  
            this.measuringForm.acEnableCheckBox.isChecked = false;
            this.measuringForm.dcEnableCheckBox.isChecked = false;
            this.measuringForm.chart.Series["acSeries"].Points.Clear();
            this.measuringForm.chart.Series["dcSeries"].Points.Clear();
            fastStream = true;
            this.measuringForm.chart.cursorMenu.setScaleFactor(impulseChannel.getScaleFactor(), impulseChannel.DCOffset);
            this.measuringForm.chart.cursorMenu.resizeDown();
            this.measuringForm.chart.updateCursorMenu();
            this.measuringForm.timeBaseComboBox.setCollection = new string[] {
        "200 ns/Div",
        "500 ns/Div",
        "1 us/Div",
        "2 us/Div",
        "5 us/Div",
        "10 us/Div"};
            this.measuringForm.timeBaseComboBox.SetSelected = "500 ns/Div";
           
        }   
    
       
        private void acVoltageRangeComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            pauseStream();            
            picoScope.setChannelVoltageRange(0, (Imports.Range)e.Value + 4);
            rebootStream();

            if (this.measuringForm.chart.cursorMenu.acChannelRadioButton.isChecked)
            {
                this.measuringForm.chart.cursorMenu.setScaleFactor(acChannel.getScaleFactor(), acChannel.DCOffset);
                this.measuringForm.chart.updateCursorMenu();
            }
        }


        private void acEnableCheckBox_Click(object sender, EventArgs e)
        {
            
            if (this.measuringForm.acEnableCheckBox.isChecked)
            {             
                fastStream = false;
                if(!stream)
                    picoScope.setStreamDataBuffers();
                stream = true;
            }
            if (!this.measuringForm.acEnableCheckBox.isChecked)
            {
                this.measuringForm.chart.Series["acSeries"].Points.Clear();
            }

        }

         private void dcVoltageRangeComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            pauseStream();
            picoScope.setChannelVoltageRange(1, (Imports.Range)e.Value + 4);
            rebootStream();

            if (this.measuringForm.chart.cursorMenu.dcChannelRadioButton.isChecked)
            {
                this.measuringForm.chart.cursorMenu.setScaleFactor(dcChannel.getScaleFactor(), dcChannel.DCOffset);
                this.measuringForm.chart.updateCursorMenu();
            }
            
        }


        private void dcEnableCheckBox_Click(object sender, EventArgs e)
        {
            
            if (this.measuringForm.dcEnableCheckBox.isChecked)
            {

                fastStream = false;
                if (!stream)
                    picoScope.setStreamDataBuffers();
                stream = true;
            }
            if (!this.measuringForm.dcEnableCheckBox.isChecked)
            {
                this.measuringForm.chart.Series["dcSeries"].Points.Clear();
            }
        }


          
        private void impulseVoltageRangeComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            
            pauseStream();
            picoScope.setChannelVoltageRange(2, (Imports.Range)e.Value + 4);            
            rebootStream();
            this.measuringForm.chart.cursorMenu.setScaleFactor(dcChannel.getScaleFactor(), dcChannel.DCOffset);
            this.measuringForm.chart.updateCursorMenu();
            
        }

        private void impulseEnableCheckBox_Click(object sender, EventArgs e)
        {


        }

        private void resolutionComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            pauseStream();
            picoScope.Resolution = (Imports.DeviceResolution)e.Value;
            rebootStream();
        }

        private void timeBaseComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            if(this.measuringForm.acdcRadioButton.isChecked)
            {
                this.measuringForm.chart.Series["acSeries"].Points.Clear();
                this.measuringForm.chart.Series["dcSeries"].Points.Clear();
                if (e.Text.Equals("2 ms/Div"))
                {
                    picoScope.StreamingInterval = 12500;
                    picoScope.TimePerDivision = 2;
                    acChannel.IncrementIndex = 0;
                    dcChannel.IncrementIndex = 0;
                    this.measuringForm.chart.setTimePerDiv(2);
                   
                }
                else if(e.Text.Equals("5 ms/Div"))
                {
                    picoScope.StreamingInterval = 31250;
                    picoScope.TimePerDivision = 5;
                    acChannel.IncrementIndex = 1;
                    dcChannel.IncrementIndex = 1;
                    this.measuringForm.chart.setTimePerDiv(5);
                }
                else
                {
                    picoScope.StreamingInterval = 62500;
                    picoScope.TimePerDivision = 10;
                    acChannel.IncrementIndex = 2;
                    dcChannel.IncrementIndex = 2;
                    this.measuringForm.chart.setTimePerDiv(10);
                }               
               
            }
            else
            {
                if (e.Text.Equals("200 ns/Div"))
                {
                    picoScope.BlockSamples = 1000;
                    picoScope.TimePerDivision = 0.2;
                    impulseChannel.IncrementIndex = 3;                    
                    this.measuringForm.chart.setTimePerDiv(0.2);
                   
                }
                else if(e.Text.Equals("500 ns/Div"))
                {
                    picoScope.BlockSamples = 2500;
                    picoScope.TimePerDivision = 0.5;
                    impulseChannel.IncrementIndex = 4;  
                    this.measuringForm.chart.setTimePerDiv(0.5);
                }
                else if(e.Text.Equals("1 us/Div"))
                {
                    picoScope.BlockSamples = 5000;
                    picoScope.TimePerDivision = 1;
                    impulseChannel.IncrementIndex = 5;  
                    this.measuringForm.chart.setTimePerDiv(1);
                }
                else if(e.Text.Equals("2 us/Div"))
                {
                    picoScope.BlockSamples = 10000;
                    picoScope.TimePerDivision = 2;
                    impulseChannel.IncrementIndex = 6;  
                    this.measuringForm.chart.setTimePerDiv(2);
                }
                else if(e.Text.Equals("5 us/Div"))
                {
                    picoScope.BlockSamples = 25000;
                    picoScope.TimePerDivision = 5;
                    impulseChannel.IncrementIndex = 7;  
                    this.measuringForm.chart.setTimePerDiv(5);
                }
                else if(e.Text.Equals("10 us/Div"))
                {
                    picoScope.BlockSamples = 50000;
                    picoScope.TimePerDivision = 10;
                    impulseChannel.IncrementIndex = 8;  
                    this.measuringForm.chart.setTimePerDiv(10);
                }
            }
            this.measuringForm.chart.updateCursorMenu();
        }
         
        private void triggerSetupButton_Click(object sender, EventArgs e)
        {


        }


        //***********************************************************************************************************
        //***                                     RUN VIEW EVENT HANDLERS                                       *****
        //***********************************************************************************************************

         private void acOutputComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
             //representation index: 0 = Vmax, 1 = Vmin, 2 = Vrms, 3 = vpk-vpk, 4 = Vavg
             int[] selection = { 2, 0, 1, 3 };  
             acChannel.setRepresentationIndex(selection[(int)e.Value]);

        }

          private void dcOutputComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            //representation index: 0 = Vmax, 1 = Vmin, 2 = Vrms, 3 = vpk-vpk, 4 = Vavg
              int[] selection = {4,0,1,3};              
              dcChannel.setRepresentationIndex(selection[(int)e.Value]);

        }

          private void impulseOutputComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            //representation index: 0 = Vmax, 1 = Vmin, 2 = Vrms, 3 = vpk-vpk, 4 = Vavg
            int[] selection = { 0, 1};
            int[] polarity = { 1, -1 };  
            impulseChannel.setRepresentationIndex(selection[(int)e.Value]);
            impulseChannel.Polarity = polarity[(int)e.Value];
        }


        private void onOffButton_Click(object sender, EventArgs e)
        {
            

        }

        private void pauseButton_Click(object sender, EventArgs e)
        {


        }

        private void parkCheckBox_Click(object sender, EventArgs e)
        {


        }

        private void overrideCheckBox_Click(object sender, EventArgs e)
        {


        }
       
        private void inputVoltageRadioButton_Click(object sender, EventArgs e)
        {


        }

        private void acVoltageRadioButton_Click(object sender, EventArgs e)
        {


        }

        private void dcVoltageRadioButton_Click(object sender, EventArgs e)
        {


        }

        private void decreaseRegulatedVoltageButton_Down(object sender, MouseEventArgs e)
        {


        }

        private void decreaseRegulatedVoltageButton_Up(object sender, MouseEventArgs e)
        {


        }



        private void regulatedVoltageTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {


        }

        private void increaseRegulatedVoltageButton_Down(object sender, MouseEventArgs e)
        {


        }

        private void increaseRegulatedVoltageButton_Up(object sender, MouseEventArgs e)
        {


        }


        private void incrementButton_Click(object sender, EventArgs e)
        {


        }

        private void voltageRegulationRepresentationComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {


        }
 
         
        private void triggerButton_Click(object sender, EventArgs e)
        {
            
            //Stop streaming mode
            stream = false;
            fastStream = false;
            picoScope.stopStreaming();
            triggerTimer.Stop();
            //RunBLock mode
            blockCapture = true;
            //Disable Channel A,B,D Enable Channel C
            picoScope.disableChannel(0);
            picoScope.disableChannel(1);
            picoScope.enableChannel(2);
            picoScope.setDCoffset(2, -1 * impulseChannel.Polarity * impulseChannel.rangeToVolt() * 0.8f);              
            //Set databuffer
            picoScope.setBlockDataBuffer();            
            //Set trigger Channel/Level/Type
            picoScope.setTriggerChannel(Imports.Channel.ChannelC);
            short level = (short)(-1 * impulseChannel.Polarity * picoScope.getADMaxValue() * 4 / 5 + 1000);
            Console.WriteLine("Level status {0}", level); 
            picoScope.setTriggerLevel(level);
            //picoScope.setTriggerLevel(1000);
            picoScope.setTriggerType(Imports.ThresholdDirection.Rising);
            //Setup Trigger / Chopping time
            picoScope.setupSignalGen(100000);
            //Start Block
            picoScope.startBlock();
            //Trigger Signal gen
            picoScope.triggerSignalGen();
            //Start watchDog
            triggerTimer.Start();
            
            

        }

        private void choppingCheckBox_Click(object sender, EventArgs e)
        {


        }

        private void decreaseChoppingTimeButton_Down(object sender, MouseEventArgs e)
        {


        }

        private void decreaseChoppingTimeButton_Up(object sender, MouseEventArgs e)
        {


        }



        private void choppingTimeTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {


        }

        private void increaseChoppingTimeButton_Down(object sender, MouseEventArgs e)
        {


        }

        private void increaseChoppingTimeButton_Up(object sender, MouseEventArgs e)
        {


        }


        private void decreaseMeasureeGap_Down(object sender, MouseEventArgs e)
        {


        }

        private void decreaseMeasureGap_Up(object sender, MouseEventArgs e)
        {


        }



        private void measureGapTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {


        }

        private void increaseMeasureGapButton_Down(object sender, MouseEventArgs e)
        {


        }

        private void increaseMeasureGapButton_Up(object sender, MouseEventArgs e)
        {


        }


        private void decreaseImpulseGap_Down(object sender, MouseEventArgs e)
        {


        }

        private void decreaseImpulseGap_Up(object sender, MouseEventArgs e)
        {


        }



        private void impulseGapTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {


        }

        private void increaseImpulseGapButton_Down(object sender, MouseEventArgs e)
        {


        }

        private void increaseImpulseGapButton_Up(object sender, MouseEventArgs e)
        {


        }

        private void decreasePressureButton_Down(object sender, MouseEventArgs e)
        {


        }

        private void decreasePressureButton_Up(object sender, MouseEventArgs e)
        {


        }

        private void pressureTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {


        }

        private void increasePressureButton_Down(object sender, MouseEventArgs e)
        {


        }

        private void increasePressureButton_Up(object sender, MouseEventArgs e)
        {


        }


        //***********************************************************************************************************
        //***                                     SETUP VIEW EVENT HANDLERS                                     *****
        //***********************************************************************************************************
        private void acCheckBox_Click(object sender, EventArgs e)
        {
            if(this.controlForm.setupView.acCheckBox.isChecked)
            {
                fastStream = true;
            }

        }

        private void acStage1RadioButton_Click(object sender, EventArgs e)
        {


        }

        private void acStage2RadioButton_Click(object sender, EventArgs e)
        {


        }
        private void acStage3RadioButton_Click(object sender, EventArgs e)
        {


        }

        private void dcCheckBox_Click(object sender, EventArgs e)
        {


        }

        private void dcStage1RadioButton_Click(object sender, EventArgs e)
        {


        }

        private void dcStage2RadioButton_Click(object sender, EventArgs e)
        {


        }

        private void dcStage3RadioButton_Click(object sender, EventArgs e)
        {


        }


        private void impulseCheckBox_Click(object sender, EventArgs e)
        {


        }

        private void impulseStage1RadioButton_Click(object sender, EventArgs e)
        {


        }

        private void impulseStage2RadioButton_Click(object sender, EventArgs e)
        {
            

        }
        private void impulseStage3RadioButton_Click(object sender, EventArgs e)
        {

            
        }


        private void disableForControls()
        {

        }

        private void manualModeRadiobutton_Click(object sender, EventArgs e)
        {

            this.controlForm.modeLabel.Text = "Manual mode";
        }

        private void acWithstandRadioButton_Click(object sender, EventArgs e)
        {
            disableForControls();
            this.controlForm.modeLabel.Text = "AC Withstand Voltage Test";
        }

        private void acDisruptiveRadioButton_Click(object sender, EventArgs e)
        {
            disableForControls();
            this.controlForm.modeLabel.Text = "AC Disruptive Discharge Voltage Test";
        }

        private void dcWithstandRadioButton_Click(object sender, EventArgs e)
        {
            disableForControls();
            this.controlForm.modeLabel.Text = "DC Withstand Voltage Test";
        }

        private void dcDisruptiveRadioButton_Click(object sender, EventArgs e)
        {
            disableForControls();
            this.controlForm.modeLabel.Text = "DC Disruptive Discharge Voltage Test";
        }

        private void impulseWithstandRadioButton_Click(object sender, EventArgs e)
        {
            disableForControls();
            this.controlForm.modeLabel.Text = "Impulse Withstand Voltage Test";
        }

        private void impulseDisruptiveRadioButton_Click(object sender, EventArgs e)
        {
            disableForControls();
            this.controlForm.modeLabel.Text = "Impulse Disruptive Discharge Voltage Test";
        }

        private void acChannelRadioButton_Click(object sender, EventArgs e)
        {
            this.measuringForm.chart.cursorMenu.setScaleFactor(acChannel.getScaleFactor(), acChannel.DCOffset);
            this.measuringForm.chart.updateCursorMenu();
        }

        private void dcChannelRadioButton_Click(object sender, EventArgs e)
        {
            this.measuringForm.chart.cursorMenu.setScaleFactor(dcChannel.getScaleFactor(), dcChannel.DCOffset);
            this.measuringForm.chart.updateCursorMenu();
        }

        //User want's to save the trigger setup information
        private void triggerMenuOkButton_Click(object sender, EventArgs e)
        {
            this.measuringForm.triggerWindow.Hide();            
        }

        private void formsCloseButton_Click(object sender, EventArgs e)
        {
            loopTimer.Stop();
            loopTimer.Dispose();
            picoScope.closeDevice();
            this.measuringForm.Close();
            this.controlForm.Close();
        }
    }
}
