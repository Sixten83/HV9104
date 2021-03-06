﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
//using Excel = Microsoft.Office.Interop.Excel; 
using System.Timers;

using Timer = System.Timers.Timer;
using System.IO.Ports;
using System.IO;
using System.Threading;


namespace HV9104_GUI
{

     class Controller
    {
        MeasuringForm measuringForm;
        ControlForm controlForm;
        PicoScope picoScope;
        Channel acChannel, dcChannel, impulseChannel;
        System.Timers.Timer loopTimer, triggerTimer;
        bool fastStreamMode,streamMode;
        bool blockCaptureMode;
        //Voltage Dividers
        decimal[] acHighDividerValues = { 101.27M, 106.7M };
        decimal acLowDividerValue = 2934000;
        decimal[] dcHighDividerValues = { 280.49M, 280.21M, 280.79M };
        decimal dcLowDividerValue = 0.027997M;
        decimal[] impulseHighDividerValues = { 1.302M, 1.2714M, 1.2638M };
        decimal[] impulseLowDividerValues = { 519.498M, 513.963M, 512.21M };
        decimal impulseAttenuatorRatio = 25.1448M;

        // Class objects
        public RunView activeForm;
        public SerialPort serialPort1;
        public Updater guiUpdater;

        public NA9739Device PIO1;
        public PD1161Device HV9126;
        public PD1161Device HV9133;
        public PD1161Device activeMotor;

        // Communication flags
        public bool serialPortConnected = false;
        public bool PIO1Connected = false;
        public bool HV9126Connected = false;
        public bool HV9133Connected = false;

        // Run control variables 
        public Thread t;
        public bool quit = false;
        public bool clearToSend = false;
        public int commandPending = 0;


        public Controller()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create one form for control and one for presentation and start them
            measuringForm = new MeasuringForm();
            controlForm = new ControlForm();            
            controlForm.startMeasuringForm(measuringForm);

            // Set up the primary measuring device
            SetupPicoscope();

            // Connect some local event handlers to give access to the form controls
            ConnectEventHandlers();

            // Find a suitable COM port and connect to it. Then add a Handler to catch any replies. 
            AutoConnect();
            
            // Helper class to update the UI from another thread (avoid cross-threading exception)
            guiUpdater = new Updater(controlForm.runView);
            
            // Initialize all devices and test communication
            InitializeDevices();
            
            // If all devices are initialized have communication, start own loop for PLC, stepper motors and aux equipment.
            t = new Thread(CyclicRead);
            t.Start();
           
            // Get and present initial status info from PLC and motors
            

            // Start timed loop for Picoscope routines
            loopTimer.Start();

            // Obligatory application command 
            Application.Run(controlForm); // Måste vara sist!!!
            
        }    
       
         //***********************************************************************************************************
        //***                                    PROGRAM LOOPS                                                   *****
        //***********************************************************************************************************
        private void loopTimer_Tick(object sender, EventArgs e)
        {

            if (fastStreamMode)
                fastStream();   
            
            if(streamMode)            
                stream();
            
            if (blockCaptureMode)
                blockCapure();
                
        }
        
        // Main loop for Modbus based equipment - runs on own thread
        private void CyclicRead()
        {

            // Untill told to stop..
            while (!quit)
            {
                // Try this instead. Maybe call 2 times in a row?
                if (commandPending == 0)
                {
                    // Do regular tasks
                    PIO1.ReadFromDevice();
                    Thread.Sleep(2);
                    activeMotor.checkCTSFlag();
                    Thread.Sleep(2);
                    PIO1.UpdateDevice();
                    Thread.Sleep(2);
                    activeMotor.getActualPosition();
                    Thread.Sleep(2);
                }
                else if (commandPending == 1)
                {
                    // Do on-demand task
                    activeMotor.DecreaseGap();
                    Thread.Sleep(2);
                    commandPending = 0;
                }
                else if (commandPending == 2)
                {
                    // Do on-demand task
                    activeMotor.IncreaseGap();
                    Thread.Sleep(2);
                    commandPending = 0;
                }
                else if (commandPending == 3)
                {
                    // Do on-demand task
                    activeMotor.StopMotor();
                    Thread.Sleep(2);
                    commandPending = 0;
                }
                else if (commandPending == 4)
                {
                    // Do on-demand task
                    activeMotor.StartInit();
                    Thread.Sleep(2);
                    commandPending = 0;
                }
                else if (commandPending == 5)
                {
                    // Do on-demand task
                    int targetPos = Convert.ToInt16(this.controlForm.runView.impulseGapTextBox.Value);
                    activeMotor.MoveToPosition(targetPos);
                    Thread.Sleep(2);
                    commandPending = 0;
                }
                
                // Present the latest info
                try
                {
                    UpdateGUI();
                }
                catch
                {
                    //MessageBox.Show("Page Missing", "Page Error");
                }
            }
        }



        //***********************************************************************************************************
        //***                                     PICOSCOPE AND CHANNELS SETUP                                   ****
        //***********************************************************************************************************
        public void SetupPicoscope()
        {
            picoScope = new PicoScope();
            uint status = picoScope.openDevice();
            if (status != Imports.PICO_OK)
            {
                Console.WriteLine("Unable to open device");
            }
            else
                this.controlForm.messageLabel.Text = "Measuring device opened successfully";
            picoScope.setACChannel(acChannel = new Channel());
            picoScope.setDCChannel(dcChannel = new Channel());
            picoScope.setImpulseChannel(impulseChannel = new Channel());
            acChannel.DividerRatio = (double)((acHighDividerValues[0] + acLowDividerValue) / acHighDividerValues[0]) / 1000;
            dcChannel.DividerRatio = (double)((dcHighDividerValues[0] + dcLowDividerValue) / dcLowDividerValue) / 1000;
            impulseChannel.DividerRatio = (double)(impulseAttenuatorRatio * (impulseHighDividerValues[0] + impulseLowDividerValues[0]) / impulseHighDividerValues[0]) / 1000;
            Console.WriteLine("impulseChannel.DividerRatio" + (int)impulseChannel.DividerRatio);
            picoScope.setTriggerChannel(Imports.Channel.ChannelA);
            picoScope.Resolution = Imports.DeviceResolution.PS5000A_DR_12BIT;
            picoScope.setFastStreamDataBuffer();
            fastStreamMode = true;
            loopTimer = new Timer(10);
            loopTimer.Elapsed += new ElapsedEventHandler(this.loopTimer_Tick);
   
            triggerTimer = new Timer(3000);
            triggerTimer.Elapsed += new ElapsedEventHandler(this.triggerTimer_Tick);

            this.measuringForm.chart.cursorMenu.setScaleFactor(acChannel.getScaleFactor(), acChannel.DCOffset);
        }


        public void fastStream()
        {
            if (picoScope._autoStop)
            {
                if (picoScope._overflow == 0)
                {
                    acChannel.processFastStreamData();
                    //this.controlForm.runView.acValueLabel.Text = ;
                    guiUpdater.transferACVoltageOutputLabel("" + acChannel.getRepresentation().ToString("0.0").Replace(',', '.'));
                    dcChannel.processFastStreamData();
                    //this.controlForm.runView.dcValueLabel.Text = "" + dcChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                    guiUpdater.transferDCVoltageOutputLabel("" + dcChannel.getRepresentation().ToString("0.0").Replace(',', '.'));
                }
                else
                    autoSetVoltageRange();
                picoScope.streamStarted = false;
                picoScope._autoStop = false;
            }
            else if (!picoScope.streamStarted)
            {
                picoScope.startFastStreaming();
            }
            else if (!picoScope._autoStop && picoScope.streamStarted)
            {
                picoScope.getFastStreamValues();
            } 
        }
         
         public void stream()
        {
            if (picoScope._autoStop)
            {
                int trigAt = (int)picoScope._trigAt;
                this.measuringForm.chart.Series.SuspendUpdates();
                if (this.measuringForm.acEnableCheckBox.isChecked)
                {
                    // Console
                    this.measuringForm.chart.Series["acSeries"].Points.Clear();
                    Channel.ScaledData data = acChannel.processData(1600, trigAt, 400);

                    this.measuringForm.chart.Series["acSeries"].Points.DataBindXY(data.x, data.y);
                    //this.controlForm.runView.acValueLabel.Text = "" + acChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                    guiUpdater.transferACVoltageOutputLabel("" + acChannel.getRepresentation().ToString("0.0").Replace(',', '.'));
                }
                else
                {
                    acChannel.processMaxMinData(1600, trigAt);
                    //this.controlForm.runView.acValueLabel.Text = "" + acChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                    guiUpdater.transferACVoltageOutputLabel("" + acChannel.getRepresentation().ToString("0.0").Replace(',', '.'));
                }

                if (this.measuringForm.dcEnableCheckBox.isChecked)
                {
                    this.measuringForm.chart.Series["dcSeries"].Points.Clear();
                    Channel.ScaledData data = dcChannel.processData(1600, trigAt, 400);
                    this.measuringForm.chart.Series["dcSeries"].Points.DataBindXY(data.x, data.y);
                    //this.controlForm.runView.dcValueLabel.Text = "" + dcChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                    guiUpdater.transferDCVoltageOutputLabel("" + dcChannel.getRepresentation().ToString("0.0").Replace(',', '.'));
                }
                else
                {
                    dcChannel.processMaxMinData(1600, trigAt);
                    //this.controlForm.runView.dcValueLabel.Text = "" + dcChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                    guiUpdater.transferDCVoltageOutputLabel("" + dcChannel.getRepresentation().ToString("0.0").Replace(',', '.'));
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
        public void blockCapure()
        {
            if (picoScope.getBlockValues() == 0)
            {
                triggerTimer.Stop();
                rebootStreaming();

                if (this.measuringForm.impulseRadioButton.isChecked)
                {
                    this.measuringForm.chart.Series["impulseSeries"].Points.Clear();
                    Channel.ScaledData data = impulseChannel.processData((int)picoScope.BlockSamples, 0, 2500);
                    this.measuringForm.chart.Series["impulseSeries"].Points.DataBindXY(data.x, data.y);
                    //this.controlForm.runView.impulseValueLabel.Text = "" + impulseChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                    guiUpdater.transferImpulseVoltageOutputLabel("" + impulseChannel.getRepresentation().ToString("0.0").Replace(',', '.'));
                 
                }
                else
                {
                    impulseChannel.processMaxMinData((int)picoScope.BlockSamples, 0);
                    //this.controlForm.runView.impulseValueLabel.Text = "" + impulseChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                    guiUpdater.transferImpulseVoltageOutputLabel("" + impulseChannel.getRepresentation().ToString("0.0").Replace(',', '.'));
                }
            }
        }
         
         private void triggerTimer_Tick(object sender, EventArgs e)
        {
            triggerTimer.Stop();
            rebootStreaming();
        }


         public void rebootStreaming()
         {
            //Stop streaming
            picoScope.stopStreaming();
            blockCaptureMode = false;
            picoScope.enableChannel(0);
            picoScope.enableChannel(1);
            picoScope.disableChannel(2);
            picoScope.setTriggerChannel(Imports.Channel.ChannelA);                       
            picoScope.setTriggerLevel(1000);
            picoScope.setTriggerType(Imports.ThresholdDirection.Rising);
            if (this.measuringForm.acEnableCheckBox.isChecked || this.measuringForm.dcEnableCheckBox.isChecked)
            {
                picoScope.setStreamDataBuffers();
                streamMode = true;
            }
            else
            {
                picoScope.setFastStreamDataBuffer();
                fastStreamMode = true;
            }
            picoScope.streamStarted = false;
            picoScope._autoStop = false;
        }

        public void pauseStream()
        {            
            picoScope.stopStreaming();
        }

        public void rebootStream()
        {
            picoScope.streamStarted = false;
            picoScope._autoStop = false;           
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
        //***                                     EVENT HANDLER CONNECTIONS                                      ****
        //***********************************************************************************************************
        public void ConnectEventHandlers()
        {
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
            this.controlForm.runView.decreaseRegulatedVoltageButton.MouseUp += new System.Windows.Forms.MouseEventHandler(decreaseRegulatedVoltageButton_Up);
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
           // this.controlForm.runView.decreaseMeasuringGapButton.MouseDown += new System.Windows.Forms.MouseEventHandler(decreaseMeasureeGap_Down);
            //this.controlForm.runView.decreaseMeasuringGapButton.MouseUp += new System.Windows.Forms.MouseEventHandler(decreaseMeasureGap_Up);
            //this.controlForm.runView.measuringGapTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(measureGapTextBox_valueChange);
            //this.controlForm.runView.increaseMeasuringGapButton.MouseDown += new System.Windows.Forms.MouseEventHandler(increaseMeasureGapButton_Down);
            //this.controlForm.runView.increaseMeasuringGapButton.MouseUp += new System.Windows.Forms.MouseEventHandler(increaseMeasureGapButton_Up);
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
            streamMode = false;
            picoScope.setFastStreamDataBuffer();
            picoScope.TimePerDivision = 0.5;
            this.measuringForm.chart.setVoltsPerDiv(6502.4);
            this.measuringForm.chart.setTimePerDiv(0.5);
            impulseChannel.IncrementIndex = 4;  
            this.measuringForm.acEnableCheckBox.isChecked = false;
            this.measuringForm.dcEnableCheckBox.isChecked = false;
            this.measuringForm.chart.Series["acSeries"].Points.Clear();
            this.measuringForm.chart.Series["dcSeries"].Points.Clear();
            fastStreamMode = true;
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
                fastStreamMode = false;
                if(!streamMode)
                    picoScope.setStreamDataBuffers();
                streamMode = true;
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

                fastStreamMode = false;
                if (!streamMode)
                    picoScope.setStreamDataBuffers();
                streamMode = true;
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


        // Voltage ON/OFF Switch
        private void onOffButton_Click(object sender, EventArgs e)
        {

            if(this.controlForm.runView.onOffButton.isChecked)
            {
                ClosePrimaryRequest();
                CloseSecondaryRequest(this.controlForm.runView.overrideCheckBox.isChecked);
            }
            else
            {
                OpenSecondaryRequest();
                OpenPrimaryRequest();
            }
            

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

            DecreaseVoltageRequest(650);

        }

        private void decreaseRegulatedVoltageButton_Up(object sender, MouseEventArgs e)
        {

            PIO1.StopTransformerMotor();

        }

        private void regulatedVoltageTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {

            // RunTo voltage value
            GoToVoltage(controlForm.runView.regulatedVoltageTextBox.Value);

        }

        // Automated voltage set routine
        private void GoToVoltage(float value)
        {
            Thread regUThread = new Thread(RegulateVoltage);
            regUThread.Start();

        
        }

        private void RegulateVoltage()
        {
            // Set some tolerances (we aren't perfect)
            float targetVoltage = controlForm.runView.regulatedVoltageTextBox.Value;
            double toleranceHi = 0.16;
            double toleranceLo = -0.16;
            
            // Variable to hold our selectable measured voltage value           
            double uActual = 0;

            // Pd Variables - if needed?
            float P = 0;
            float k = 0;
            float d = 0;
            double error = 10;
            double previousError = 0;
            double integral = 0;
            int intCnt = 0;
            int styr = 30;

            while (((error <= toleranceLo) || (error >= toleranceHi)) && (controlForm.runView.onOffButton.isChecked))
            {

                // Get the value to regulate agianst
                if (controlForm.runView.inputVoltageRadioButton.isChecked)
                {
                    //uActual = (float)PIO1.regulatedVoltageValue;
                    uActual = Convert.ToDouble(controlForm.runView.voltageInputLabel.Text);
                }
                else if (controlForm.runView.acOutputRadioButton.isChecked)
                {
                    //uActual = Convert.ToDouble(controlForm.runView.acValueLabel.Text);
                    uActual = picoScope.channels[0].Average;
                }
                else if (controlForm.runView.dcVoltageRadioButton.isChecked)
                {
                    uActual = Convert.ToDouble(controlForm.runView.dcValueLabel.Text);
                }
                else
                {
                    // Shut down, something is wrong
                }

                error =  uActual - targetVoltage;


                if(error == previousError)
                {
                    integral += 0.25;
                }
                else
                {
                    if(integral >= 0.1)
                    {
                        integral -= 0.1;
                    }
                }

                // Call the appropriate instruction
                if (error < toleranceHi)
                {
                    styr = (int)((error * -4) + 60 + integral);

                    // Voltage low, increase
                    IncreaseVoltageRequest(styr);

                }
                else if (error > toleranceLo)
                {
                    styr = (int)((error * 4) + 60 + integral);

                    // Voltage high, decrease
                    DecreaseVoltageRequest(styr);

                }
                else
                {
                    // In bounds. We should only make it here once
                    StopTransformerMotorRequest();

                }
                Thread.Sleep(50);
                previousError = error;
            }

            // In bounds. We should only make it here once
            StopTransformerMotorRequest();
            string what = "";
        }

        private void increaseRegulatedVoltageButton_Down(object sender, MouseEventArgs e)
        {

            IncreaseVoltageRequest(650);

        }

        private void increaseRegulatedVoltageButton_Up(object sender, MouseEventArgs e)
        {

            PIO1.StopTransformerMotor();

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
            streamMode = false;
            fastStreamMode = false;
            picoScope.stopStreaming();
            triggerTimer.Stop();
            //RunBLock mode
            blockCaptureMode = true;
            //Disable Channel A,B,D Enable Channel C
            picoScope.disableChannel(0);
            picoScope.disableChannel(1);
            picoScope.enableChannel(2);
            picoScope.setDCoffset(2, -1 * impulseChannel.Polarity * impulseChannel.rangeToVolt() * 0.8f);              
            //Set databuffer
            picoScope.setBlockDataBuffer();            
            //Set trigger Channel/Level/Type
            picoScope.setTriggerChannel(Imports.Channel.ChannelC);            
            //Setup Trigger / Chopping time
            picoScope.setupSignalGen(11000);
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

            StopMotorRequest();

        }



        private void measureGapTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {


        }

        private void increaseMeasureGapButton_Down(object sender, MouseEventArgs e)
        {


        }

        private void increaseMeasureGapButton_Up(object sender, MouseEventArgs e)
        {

            StopMotorRequest();

        }


        private void decreaseImpulseGap_Down(object sender, MouseEventArgs e)
        {

            DecreaseGapRequest();

        }

        private void decreaseImpulseGap_Up(object sender, MouseEventArgs e)
        {

            StopMotorRequest();

        }



        private void impulseGapTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {

            RunToPosRequest(this.controlForm.runView.impulseGapTextBox.Text);

        }

        private void increaseImpulseGapButton_Down(object sender, MouseEventArgs e)
        {

            IncreaseGapRequest();

        }

        private void increaseImpulseGapButton_Up(object sender, MouseEventArgs e)
        {

            StopMotorRequest();

        }

        private void decreasePressureButton_Down(object sender, MouseEventArgs e)
        {

            DecreasePressureRequest();

        }

        private void decreasePressureButton_Up(object sender, MouseEventArgs e)
        {

            StopPressureRequest();

        }

        private void pressureTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {

            SetPressureRequest(controlForm.runView.pressureTextBox.Value);

        }

        private void increasePressureButton_Down(object sender, MouseEventArgs e)
        {

            IncreasePressureRequest();

        }

        private void increasePressureButton_Up(object sender, MouseEventArgs e)
        {

            StopPressureRequest();

        }


        //***********************************************************************************************************
        //***                                     SETUP VIEW EVENT HANDLERS                                     *****
        //***********************************************************************************************************
        private void acCheckBox_Click(object sender, EventArgs e)
        {
            if(this.controlForm.setupView.acCheckBox.isChecked)
            {
                fastStreamMode = true;
            }

        }

        private void acStage1RadioButton_Click(object sender, EventArgs e)
        {
            this.controlForm.setupView.acDivder1TextBox.Value = (float)acHighDividerValues[0];
            acChannel.DividerRatio = (double)((acHighDividerValues[0] + acLowDividerValue) / acHighDividerValues[0]) / 1000;
            

        }

        private void acStage2RadioButton_Click(object sender, EventArgs e)
        {
            this.controlForm.setupView.acDivder1TextBox.Value = (float)acHighDividerValues[1];
            acChannel.DividerRatio = (double)((acHighDividerValues[1] + acLowDividerValue) / acHighDividerValues[1]) / 1000;
           
        }
        private void acStage3RadioButton_Click(object sender, EventArgs e)
        {
            this.controlForm.setupView.acDivder1TextBox.Value = (float)acHighDividerValues[1];
            acChannel.DividerRatio = (double)((acHighDividerValues[1] + acLowDividerValue) / acHighDividerValues[1]) / 1000;
            
        }

        private void dcCheckBox_Click(object sender, EventArgs e)
        {


        }

        private void dcStage1RadioButton_Click(object sender, EventArgs e)
        {

            dcChannel.DividerRatio = (double)((dcHighDividerValues[0] + dcLowDividerValue) / dcLowDividerValue) / 1000;
           

        }

        private void dcStage2RadioButton_Click(object sender, EventArgs e)
        {
            dcChannel.DividerRatio = (double)((dcHighDividerValues[0] + dcHighDividerValues[1] + dcLowDividerValue) / dcLowDividerValue) / 1000;

        }

        private void dcStage3RadioButton_Click(object sender, EventArgs e)
        {

            dcChannel.DividerRatio = (double)((dcHighDividerValues.Sum() + dcLowDividerValue) / dcLowDividerValue) / 1000;
        }


        private void impulseCheckBox_Click(object sender, EventArgs e)
        {
            
        }

        private void impulseStage1RadioButton_Click(object sender, EventArgs e)
        {

            this.controlForm.setupView.impulseLowDivderTextBox.Value = (float)impulseLowDividerValues[0];
            impulseChannel.DividerRatio = (double)(impulseAttenuatorRatio * (impulseHighDividerValues[0] + impulseLowDividerValues[0]) / impulseHighDividerValues[0]) / 1000;
        }

        private void impulseStage2RadioButton_Click(object sender, EventArgs e)
        {
            decimal highDivider = 1 / (1 / impulseHighDividerValues[0] + 1 / impulseHighDividerValues[1]);
            this.controlForm.setupView.impulseLowDivderTextBox.Value = (float)impulseLowDividerValues[1];
            impulseChannel.DividerRatio = (double)(impulseAttenuatorRatio * (highDivider + impulseLowDividerValues[1]) / highDivider) / 1000;
        }
        private void impulseStage3RadioButton_Click(object sender, EventArgs e)
        {
            decimal highDivider = 1 / (1 / impulseHighDividerValues[0] + 1 / impulseHighDividerValues[1] + 1 / impulseHighDividerValues[2]);
            this.controlForm.setupView.impulseLowDivderTextBox.Value = (float)impulseLowDividerValues[2];
            impulseChannel.DividerRatio = (double)(impulseAttenuatorRatio * (highDivider + impulseLowDividerValues[2]) / highDivider) / 1000;
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


        // Set up the new devices. Should maybe return bool to notify of init problems
        public void InitializeDevices()
        {
            // Create new device objects
            PIO1 = new NA9739Device(serialPort1);
            HV9126 = new PD1161Device(3, serialPort1);
            HV9133 = new PD1161Device(4, serialPort1);
            activeMotor = HV9126;

            if (!activeMotor.initComplete)
            {
                InitMotorRequest();
            }


            //// Check communication - see who is connected 
            //FindDevices();
        }

        // Who is connected?
        //public void FindDevices()
        //{
        //    if (PIO1.CheckConnection()){ PIO1Connected = true; }
        //    if (HV9133.checkCTSFlag()) { HV9133Connected = true; }
        //    if (HV9126.checkCTSFlag()) { HV9126Connected = true; }

        //    // Select a Sphere Gap
        //    if (HV9126Connected)
        //    {
        //        activeMotor = HV9126;
        //    }
        //    else
        //    {
        //        if (HV9133Connected)
        //        {
        //            activeMotor = HV9133;
        //        }
        //        else
        //        {
        //            // No one is connected
        //            activeMotor = null;
        //        }
        //    }
        //}

        


               

        // Add all updated variables to this...
        public void UpdateGUI()
        {
            double voltVal = 0;

            // Voltage and Current
            guiUpdater.transferVoltageInputLabel(PIO1.regulatedVoltageValue.ToString("0.0"));
            guiUpdater.transferCurrentInputLabel(PIO1.regulatedCurrentValue.ToString("0.00"));


            // Status flags
            //guiUpdater.transferTextBox6(PIO1.getPressure());
            //guiUpdater.transferLabel36(PIO1.fault.ToString());
            //guiUpdater.transferLabel37(PIO1.earthingEngaged.ToString());
            //guiUpdater.transferLabel38(PIO1.dischargeRodParked.ToString());
            //guiUpdater.transferLabel39(PIO1.emergStpKeySwClosed.ToString());
            //guiUpdater.transferLabel40(PIO1.dorrSwitchClosed.ToString());
            //guiUpdater.transferLabel41(PIO1.K1Closed.ToString());
            //guiUpdater.transferLabel42(PIO1.K2Closed.ToString());
            //guiUpdater.transferLabel43(PIO1.minUPos.ToString());
            //guiUpdater.transferCTSFlag(activeMotor.initComplete.ToString());

            // Ratio-calculated High Voltage value
            //voltVal = (((double)PIO1.regulatedVoltageValue * 45) / 100);
            //guiUpdater.transferLabel47(voltVal.ToString("00.00"));

            //if ((voltVal >= 5) && (PIO1.K2Closed))
            //{
            //    activeForm.pictureBox1.Visible = true;
            //}
            //else
            //{
            //    activeForm.pictureBox1.Visible = false;
            //}

            // Motor status
            //activeForm.label53.Visible = activeMotor.initComplete;
            guiUpdater.transferImpulseGapLabel(activeMotor.actualPosition.ToString());

        }

        // Voltage connection control
        public void ClosePrimaryRequest()
        {
            PIO1.closePrimary();
        }

        // Voltage connection control
        public void CloseSecondaryRequest(bool overRideIn)
        {
            PIO1.overrideUMin = overRideIn;
            PIO1.closeSecondary();
        }

        // Voltage connection control
        public void OpenPrimaryRequest()
        {
            PIO1.openPrimary();
        }

        // Voltage connection control
        public void OpenSecondaryRequest()
        {
            PIO1.openSecondary();
        }

        // Voltage level control
        public void IncreaseVoltageRequest(int speedValIn)
        {
            PIO1.increaseVoltage(speedValIn);
        }

        // Voltage level control
        public void DecreaseVoltageRequest(int speedValIn)
        {
            PIO1.decreaseVoltage(speedValIn);
        }

        // Voltage level control
        public void StopTransformerMotorRequest()
        {
            PIO1.StopTransformerMotor();
        }

        public void ChangeActiveMotorRequest(int selectedMotorIn)
        {
            if (selectedMotorIn == 0)
            {
                activeMotor = HV9126;
            }
            else
            {
                activeMotor = HV9133;
            }
        }

        //
        public void DecreaseGapRequest()
        {
            //activeMotor.DecreaseGap();
            commandPending = 1;
        }

        public void IncreaseGapRequest()
        {
            //activeMotor.IncreaseGap();
            commandPending = 2;
        }

        // 
        public void StopMotorRequest()
        {
            //activeMotor.StopMotor();
            commandPending = 3;
        }

        // Active stepper motor initialization
        public void InitMotorRequest()
        {
            //activeMotor.StartInit();
            commandPending = 4;

        }

        public void RunToPosRequest(string posIn)
        {
            if (activeMotor.initComplete != true)
            {
                return;
            }

            commandPending = 5;

        }

        //
        public void IncreasePressureRequest()
        {
            PIO1.startCompressor = true; // should be done in devices own increase pressure routine
            PIO1.UpdateDevice();
        }

        //
        public void DecreasePressureRequest()
        {
            PIO1.startVacuumPump = true; // should be done in devices own decrease pressure routine
            PIO1.UpdateDevice();
        }

        //
        public void SetPressureRequest(double presValIn)
        {
            double presVal = presValIn / 10;
            PIO1.ChangePressure(presVal);
        }

        public void StopPressureRequest()
        {
            PIO1.stopVacPressure();
        }

        public string GetPressureRequest()
        {
            string retrievedPress = PIO1.getPressure();
            return retrievedPress;
        }



        // Serial port communications
        public bool AutoConnect()
        {
            // Communication variables
            int Baud = 38400;
            Parity Parity = System.IO.Ports.Parity.None;
            int DataBits = 8;
            StopBits StopBits = System.IO.Ports.StopBits.One;

            // Search active comports for adapter
            string portName = getAdapterPort();
            if (portName == "Not Found")
            {
                return false;
            }

            // Create a new serial port instance
            serialPort1 = new SerialPort(portName);
        
            // Assign communication variables to the port
            serialPort1.PortName = portName;
            serialPort1.BaudRate = Baud;
            serialPort1.Parity = Parity;
            serialPort1.DataBits = DataBits;
            serialPort1.StopBits = StopBits;

            //  Set the read/write timeouts
            serialPort1.ReadTimeout = 1000;
            serialPort1.WriteTimeout = 1000;
            //serialPort1.ReceivedBytesThreshold = 9;

            // Test the connection
            try
            {
                serialPort1.Close();
                serialPort1.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(("Com port " + (portName + " could not be opened.")), "Name Entry Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //return 0;
            }

            if (serialPort1.IsOpen)
            {
                serialPortConnected = true;

                //// Present the active ComPort
                // guiUpdater.transferTextBoxActiveComPort(portName);
                // "Modbus live on COM19"
            }

            return serialPortConnected;
        }

        // Look through COM ports for a connected device
        public string getAdapterPort()
        {
            try
            {
                List<string> tList = new List<string>();
                foreach (string s in SerialPort.GetPortNames())
                {
                    tList.Add(s);
                }

                return tList[0];
            }
            catch
            {
                return "Not Found";

            }
        }
    }
}
