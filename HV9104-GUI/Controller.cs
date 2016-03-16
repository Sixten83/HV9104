﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms.Integration;
using System.IO;
using System.Net.NetworkInformation;
using Jitbit.Utils;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using System.Text;


namespace HV9104_GUI
{

    class Controller
    {
        MeasuringForm measuringForm;
        ControlForm controlForm;
        PicoScope picoScope;
        Channel acChannel, dcChannel, impulseChannel;
        Channel.ScaledData data;
        
        //Timers
        System.Windows.Forms.Timer loopTimer, triggerTimer;
        
        //PicoScope mode flags
        bool fastStreamMode, streamMode;
        bool blockCaptureMode;

        //Voltage Dividers
        decimal[]   acDefaultHighDividerValues = { 101.27M, 106.7M };        
        decimal[]   acHighDividerValues = { 101.27M, 106.7M };
        decimal     acLowDividerValue = 2934000;        
        decimal[]   dcDefaultHighDividerValues = { 280.49M, 280.21M, 280.79M };
        decimal     dcDefaultLowDividerValue = 0.027997M;
        decimal[]   dcHighDividerValues = { 280.49M, 280.21M, 280.79M };
        decimal     dcLowDividerValue = 0.027997M;
        decimal[]   impulseDefaultHighDividerValues = { 1.302M, 1.2714M, 1.2638M };
        decimal[]   impulseDefaultLowDividerValues = { 519.498M, 513.963M, 512.21M };
        decimal[]   impulseHighDividerValues = { 1.302M, 1.2714M, 1.2638M };
        decimal[]   impulseLowDividerValues = { 519.498M, 513.963M, 512.21M };
        decimal     impulseAttenuatorRatio = 25.1448M;
       
        //Composer PLayer
        public ComposerPLayer.UserControl1 player;
        public ElementHost ctrlHost;
        System.Windows.Forms.Timer composerTimer;

        //Check thin client communication
        private string adress;
        private System.Windows.Forms.Timer thinClientTimer;
        private System.Windows.Forms.Timer thinClientWatchDogTimer;
        private bool thinClientConnected = true; 

        // Class objects
        public DashBoardView activeForm;
        public SerialPort serialPort1;
        public Updater guiUpdater;
        public AutoTest autoTest;

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
        public bool searchingGap = false;
        private bool abortRegulation = true;
        public int trafSpeed = 600;
        public double maxVoltage = 235;
        private double targetVoltage;
        private string cnt;
        int setupNr = 0;
        int setupNrLast = 100;
        System.Timers.Timer parkTimer = new System.Timers.Timer(300);
        private bool initializeMotor;
        private string motorState = "NOT INITIALIZED";

        // presentation
        string selectedVoltage;
        string selectedMeasType;

        // Limits
        public int acInputMax = 230;
        public double acOutputMax = 140;
        public double dcOutputMax = 140;
        public double impOutputMax = 140;
        private double qualifier;
        private int acTypeIndex = 0;
        private int dcTypeIndex = 0;
        private int impTypeIndex = 0;
        private int acAutoTypeIndex = 0;
        private int acAutoOutputMax = 100;
        private int dcAutoTypeIndex = 0;
        private int dcAutoOutputMax = 140;
        private int impAutoTypeIndex = 0;
        private int impAutoOutputMax;

        // Output controls
        ReportGen report;
        private bool upDownRequest;

        public Controller()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            // Create one form for control and one for presentation and start them
            measuringForm = new MeasuringForm();
            controlForm = new ControlForm();
            controlForm.startMeasuringForm(measuringForm);

            // Set up the primary measuring device
            SetupPicoscope();

            //Composer PLayer
            SetupComposerPlayer();

            // Find a suitable COM port and connect to it. Then add a Handler to catch any replies. 
            AutoConnect();

            // Initialize all devices and test communication
            InitializeDevices();

            // Instantiate the class for autorun procedures(after initializing the argument object)
            autoTest = new AutoTest(controlForm.runView, controlForm.dashboardView, measuringForm, PIO1, HV9126, HV9133, acChannel, dcChannel, impulseChannel);

            // Connect some local event handlers to give access to the form controls
            ConnectEventHandlers();

            // If all devices are initialized have communication, start own loop for PLC, stepper motors and aux equipment.
            t = new Thread(CyclicRead);
            t.Start();

            // Start timed loop for Picoscope routines
            loopTimer.Start();

            // Get and present initial status info from PLC and motors
            Thread.Sleep(200);
            InitializeDbView();

            //Setup for Thin Client Communication test
            initCommunicationTest();


            // Obligatory application command 
            System.Windows.Forms.Application.Run(controlForm); // Måste vara sist!!!

        }



        //***********************************************************************************************************
        //***                                    PROGRAM LOOPS                                                   *****
        //***********************************************************************************************************
        private void loopTimer_Tick(object sender, EventArgs e)
        {

            if (fastStreamMode)
                fastStream();

            if (streamMode)
                stream();

            if (blockCaptureMode)
                blockCapure();

            // Present the latest info
            try
            {
                UpdateGUI();
                CheckLimits();

            }
            catch
            {
                //MessageBox.Show("Page Missing", "Page Error");
            }

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
                    Thread.Sleep(20);
                    commandPending = 0;
                }
                else if (commandPending == 2)
                {
                    // Do on-demand task
                    activeMotor.IncreaseGap();
                    Thread.Sleep(20);
                    commandPending = 0;
                }
                else if (commandPending == 3)
                {
                    // Do on-demand task
                    activeMotor.StopMotor();
                    Thread.Sleep(20);
                    commandPending = 0;
                }
                else if (commandPending == 4)
                {
                    // Do on-demand task
                    activeMotor.StartInit();
                    Thread.Sleep(20);
                    commandPending = 0;
                }
                else if (commandPending == 5)
                {
                    // Do on-demand task
                    int targetPos = Convert.ToInt16(this.controlForm.dashboardView.impulseGapTextBox.Value);
                    activeMotor.MoveToPosition(targetPos);
                    Thread.Sleep(20);
                    commandPending = 0;
                }
            }
        }

        //***********************************************************************************************************
        //***                                     Composer Player Setup                                          ****
        //***********************************************************************************************************

        public void SetupComposerPlayer()
        {
            player = new ComposerPLayer.UserControl1();
            ctrlHost = new ElementHost();
            ctrlHost.Dock = DockStyle.Fill;
            ctrlHost.Child = player;
            this.controlForm.setupView.ComposerPlayerPanel.Controls.Add(ctrlHost);
            composerTimer = new System.Windows.Forms.Timer();
            this.composerTimer.Tick += new System.EventHandler(this.composerTimer_Tick);
            composerTimer.Interval = 50;
            player.modelLoaded = false;
            player.sumLoad = 0;
            composerTimer.Start();
        }

        private void composerTimer_Tick(object sender, EventArgs e)
        {
            if (player.modelLoaded)
            { 
                player.hideMenus();
                this.controlForm.setupView.ComposerPlayerPanel.Visible = true;
                composerTimer.Stop();
            } 
               
        }

        //***********************************************************************************************************
        //***                                     Thin Client Communication Test                                 ****
        //***********************************************************************************************************

        private void initCommunicationTest()
        {
            thinClientTimer = new System.Windows.Forms.Timer();
            thinClientTimer.Interval = 1000;
            thinClientTimer.Tick += new System.EventHandler(this.thinClientTimer_Tick);
            
            thinClientWatchDogTimer = new System.Windows.Forms.Timer();
            thinClientWatchDogTimer.Interval = 3000;
            thinClientWatchDogTimer.Tick += new System.EventHandler(thinClientWatchDog_Tick);

            try
            {

                StreamReader sr = new StreamReader("Resources/IP.txt", System.Text.Encoding.Default);
                using (sr)
                {   
                    adress = sr.ReadToEnd();                   
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                adress = "192.168.17.18";
                System.IO.File.WriteAllText("Resources/IP.txt", adress);                
            }

            thinClientTimer.Start();
        }

        //Loop timer for thin client timer
        public void thinClientTimer_Tick(object sender, EventArgs e)
        {
            if(thinClientConnected)
            {
                thinClientConnected = false;
                Thread pingThread = new Thread(new ThreadStart(pingThinClient));
                pingThread.Start();
                thinClientWatchDogTimer.Start();
            }
        }
        
        //Watchdog timer for thin client
        public void thinClientWatchDog_Tick(Object sender, EventArgs e)
        {
            closeApplication();
        }

        //Ping the thin client to make sure the communications is ok!
        public void pingThinClient()
        {            
            Ping pingSender = new Ping();
            PingReply reply = pingSender.Send(adress);

            if (reply.Status == IPStatus.Success)
            {
                thinClientWatchDogTimer.Stop();
                thinClientConnected = true;                
            }
            else
            {
                Console.WriteLine(reply.Status);
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
            acChannel.DividerRatio = (double)((acDefaultHighDividerValues[0] + acLowDividerValue) / acDefaultHighDividerValues[0]) / 1000;
            dcChannel.DividerRatio = (double)((dcDefaultHighDividerValues[0] + dcDefaultLowDividerValue) / dcDefaultLowDividerValue) / 1000;
            impulseChannel.DividerRatio = (double)(impulseAttenuatorRatio * (impulseDefaultHighDividerValues[0] + impulseDefaultLowDividerValues[0]) / impulseDefaultHighDividerValues[0]) / 1000;
            Console.WriteLine("impulseChannel.DividerRatio" + impulseChannel.DividerRatio);
            picoScope.setTriggerChannel(Imports.Channel.ChannelA);
            picoScope.Resolution = Imports.DeviceResolution.PS5000A_DR_12BIT;
            picoScope.setFastStreamDataBuffer();
            fastStreamMode = true;

            loopTimer = new System.Windows.Forms.Timer();
            loopTimer.Tick += new EventHandler(this.loopTimer_Tick);
            loopTimer.Interval = 10;
            loopTimer.Enabled = true;

            triggerTimer = new System.Windows.Forms.Timer();
            triggerTimer.Tick += new EventHandler(this.triggerTimer_Tick);
            triggerTimer.Interval = 3000;
            triggerTimer.Enabled = true;

            this.measuringForm.chart.cursorMenu.setScaleFactor(acChannel.getScaleFactor(), acChannel.DCOffset);
            this.measuringForm.chart.setTimePerDiv(50000);
        }


        public void fastStream()
        {
            if (picoScope._autoStop)
            {

                if ((int)(picoScope._overflow & 1) == 0 || !acChannel.VoltageAutoRange)
                {
                    acChannel.processFastStreamData();
                    this.controlForm.dashboardView.acValueLabel.Text = "" + acChannel.getRepresentation().ToString("0.0").Replace(',', '.');

                }
                else
                    autoSetVoltageRange(acChannel);

                if ((int)(picoScope._overflow & 1) == 0 && acChannel.getRawMaxRatio() < 0.3 && acChannel.VoltageAutoRange)
                {
                    autoSetVoltageRange(acChannel);
                }


                if ((int)(picoScope._overflow & 2) == 0 || !dcChannel.VoltageAutoRange)
                {
                    dcChannel.processFastStreamData();
                    this.controlForm.dashboardView.dcValueLabel.Text = "" + dcChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                }
                else
                    autoSetVoltageRange(dcChannel);

                if ((int)(picoScope._overflow & 2) == 0 && dcChannel.getRawMaxRatio() < 0.3 && dcChannel.VoltageAutoRange)
                {
                    autoSetVoltageRange(dcChannel);
                }
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
                
                if ((int)(picoScope._overflow & 1) == 0 || !acChannel.VoltageAutoRange)
                {
                    if (this.measuringForm.acEnableCheckBox.isChecked)
                    {
                        // Console
                       
                        Channel.ScaledData data = acChannel.processData(50000, trigAt);
                        this.measuringForm.chart.updateChart("acSeries", data, 50000);                      
                        this.controlForm.dashboardView.acValueLabel.Text = "" + acChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                        this.measuringForm.acStatusLabel.Text = "Enabled";
                    }
                    else
                    {
                        acChannel.processMaxMinData(50000, trigAt);
                        this.controlForm.dashboardView.acValueLabel.Text = "" + acChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                        this.measuringForm.acStatusLabel.Text = "Disabeld";
                    }
                }
                else
                    autoSetVoltageRange(acChannel);

                if ((int)(picoScope._overflow & 1) == 0 && acChannel.getRawMaxRatio() < 0.3 && acChannel.VoltageAutoRange)
                {
                    autoSetVoltageRange(acChannel);
                }

                if ((int)(picoScope._overflow & 2) == 0 || !dcChannel.VoltageAutoRange)
                {
                    if (this.measuringForm.dcEnableCheckBox.isChecked)
                    {

                        Channel.ScaledData data = dcChannel.processData(50000, trigAt);
                        this.measuringForm.chart.updateChart("dcSeries", data, 50000);
                        this.controlForm.dashboardView.dcValueLabel.Text = "" + dcChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                        this.measuringForm.dcStatusLabel.Text = "Enabled";
                    }
                    else
                    {
                        dcChannel.processMaxMinData(50000, trigAt);
                        this.controlForm.dashboardView.dcValueLabel.Text = "" + dcChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                        this.measuringForm.dcStatusLabel.Text = "Disabeld";
                    }
                }
                else
                    autoSetVoltageRange(dcChannel);

                if ((int)(picoScope._overflow & 2) == 0 && dcChannel.getRawMaxRatio() < 0.3 && dcChannel.VoltageAutoRange)
                {
                    autoSetVoltageRange(dcChannel);
                }


               
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
                   
                    data = impulseChannel.processData((int)picoScope.BlockSamples, 0);
                    autoTest.impulseData = data;
                    this.measuringForm.chart.updateChart("impulseSeries", data, (int)picoScope.BlockSamples);
                    this.controlForm.dashboardView.impulseValueLabel.Text = "" + impulseChannel.getRepresentation().ToString("0.0").Replace(',', '.');

                }
                else
                {
                    impulseChannel.processMaxMinData((int)picoScope.BlockSamples, 0);
                    this.controlForm.dashboardView.impulseValueLabel.Text = "" + impulseChannel.getRepresentation().ToString("0.0").Replace(',', '.');
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

        public void autoSetVoltageRange(Channel channel)
        {
            pauseStream();
            if (picoScope._overflow != 0)
            {

                if ((int)channel.VoltageRange < 10)
                {
                    channel.VoltageRange++;
                    picoScope.setChannelVoltageRange((int)channel.ChannelName, channel.VoltageRange);
                }
            }
            else
            {

                if ((int)channel.VoltageRange > 4)
                {
                    channel.VoltageRange--;
                    picoScope.setChannelVoltageRange((int)channel.ChannelName, channel.VoltageRange);
                }

            }

            if (this.measuringForm.chart.cursorMenu.acChannelRadioButton.isChecked && !this.measuringForm.impulseRadioButton.isChecked)
            {
                this.measuringForm.chart.cursorMenu.setScaleFactor(acChannel.getScaleFactor(), acChannel.DCOffset);
                this.measuringForm.chart.updateCursorMenu();
                
            }
            else if (this.measuringForm.chart.cursorMenu.dcChannelRadioButton.isChecked && !this.measuringForm.impulseRadioButton.isChecked)
            {
                this.measuringForm.chart.cursorMenu.setScaleFactor(dcChannel.getScaleFactor(), dcChannel.DCOffset);
                this.measuringForm.chart.updateCursorMenu();
               
            }

            if(channel == acChannel)
                this.measuringForm.acScaleLabel.Text = channel.rangeToVolt() + " kV/Div";
            else
                this.measuringForm.dcScaleLabel.Text = channel.rangeToVolt() + " kV/Div";
           
            rebootStream();
        }

        //***********************************************************************************************************
        //***                                     EVENT HANDLER CONNECTIONS                                      ****
        //***********************************************************************************************************
        public void ConnectEventHandlers()
        {
            /////////////////
            this.measuringForm.closeButton.Click += new System.EventHandler(this.formsCloseButton_Click);
            this.controlForm.closeButton.Click += new System.EventHandler(this.formsCloseButton_Click);

            this.controlForm.runExperimentTab.Click += new System.EventHandler(runExperimentTab_Click);
            this.controlForm.dashboardTab.Click += new System.EventHandler(dashboardTab_Click);
            this.controlForm.setupTab.Click += new System.EventHandler(setupTab_Click);

            //***********************************************************************************************************
            //***                                     SETUP VIEW EVENT LISTENERS                                     ****
            //***********************************************************************************************************

            //Stage setup listeners
            this.controlForm.setupView.acCheckBox.Click += new System.EventHandler(acCheckBox_Click);
            this.controlForm.setupView.acDivderDefaultButton.Click += new System.EventHandler(acDivderDefaultButton_Click);
            this.controlForm.setupView.acStage1RadioButton.Click += new System.EventHandler(acStage1RadioButton_Click);
            this.controlForm.setupView.acStage2RadioButton.Click += new System.EventHandler(acStage2RadioButton_Click);
            this.controlForm.setupView.acStage3RadioButton.Click += new System.EventHandler(acStage3RadioButton_Click);
            this.controlForm.setupView.acDivder1TextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(acDivder1TextBox_valueChange);
            this.controlForm.setupView.dcCheckBox.Click += new System.EventHandler(dcCheckBox_Click);
            this.controlForm.setupView.dcDivderDefaultButton.Click += new System.EventHandler(dcDivderDefaultButton_Click);
            this.controlForm.setupView.dcStage1RadioButton.Click += new System.EventHandler(dcStage1RadioButton_Click);
            this.controlForm.setupView.dcStage2RadioButton.Click += new System.EventHandler(dcStage2RadioButton_Click);
            this.controlForm.setupView.dcStage3RadioButton.Click += new System.EventHandler(dcStage3RadioButton_Click);
            this.controlForm.setupView.dcDivder1TextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(dcDivder1TextBox_valueChange);
            this.controlForm.setupView.dcDivder2TextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(dcDivder2TextBox_valueChange);
            this.controlForm.setupView.dcDivder3TextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(dcDivder3TextBox_valueChange);
            this.controlForm.setupView.impulseCheckBox.Click += new System.EventHandler(impulseCheckBox_Click);
            this.controlForm.setupView.impulseDivderDefaultButton.Click += new System.EventHandler(impulseDivderDefaultButton_Click);
            this.controlForm.setupView.impulseStage1RadioButton.Click += new System.EventHandler(impulseStage1RadioButton_Click);
            this.controlForm.setupView.impulseStage2RadioButton.Click += new System.EventHandler(impulseStage2RadioButton_Click);
            this.controlForm.setupView.impulseStage3RadioButton.Click += new System.EventHandler(impulseStage3RadioButton_Click);
            this.controlForm.setupView.impulseLowDivderTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(impulseLowDivderTextBox_valueChange);
            this.controlForm.setupView.impulseDivder1TextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(impulseDivder1TextBox_valueChange);
            this.controlForm.setupView.impulseDivder2TextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(impulseDivder2TextBox_valueChange);
            this.controlForm.setupView.impulseDivder3TextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(impulseDivder3TextBox_valueChange);
            //***********************************************************************************************************
            //***                                     DASHBOARD VIEW EVENT LISTENERS                                       ****
            //***********************************************************************************************************
            // 

            //Output Representation Listeners
            this.controlForm.dashboardView.acOutputComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(acOutputComboBox_valueChange);
            this.controlForm.dashboardView.dcOutputComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(dcOutputComboBox_valueChange);
            this.controlForm.dashboardView.impulseOutputComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(impulseOutputComboBox_valueChange);
            //POWER Listeners
            this.controlForm.dashboardView.onOffButton.Click += new System.EventHandler(onOffButton_Click);
            this.controlForm.dashboardView.onOffSecButton.Click += new System.EventHandler(onOffSecButton_Click);
            this.controlForm.dashboardView.parkCheckBox.Click += new System.EventHandler(parkCheckBox_Click);
            this.controlForm.dashboardView.overrideCheckBox.Click += new System.EventHandler(overrideCheckBox_Click);
            //Regulated Voltage Type Listeners            
            this.controlForm.dashboardView.acInputRadioButton.Click += new System.EventHandler(inputVoltageRadioButton_Click);
            this.controlForm.dashboardView.acOutputRadioButton.Click += new System.EventHandler(acVoltageRadioButton_Click);
            this.controlForm.dashboardView.dcOutputRadioButton.Click += new System.EventHandler(dcVoltageRadioButton_Click);
            //Set Voltage Listeners            
            this.controlForm.dashboardView.decreaseRegulatedVoltageButton.MouseDown += new System.Windows.Forms.MouseEventHandler(decreaseRegulatedVoltageButton_Down);
            this.controlForm.dashboardView.decreaseRegulatedVoltageButton.MouseUp += new System.Windows.Forms.MouseEventHandler(decreaseRegulatedVoltageButton_Up);
            this.controlForm.dashboardView.regulatedVoltageTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(regulatedVoltageTextBox_valueChange);
            this.controlForm.dashboardView.increaseRegulatedVoltageButton.MouseDown += new System.Windows.Forms.MouseEventHandler(increaseRegulatedVoltageButton_Down);
            this.controlForm.dashboardView.increaseRegulatedVoltageButton.MouseUp += new System.Windows.Forms.MouseEventHandler(increaseRegulatedVoltageButton_Up);
            this.controlForm.dashboardView.abortRegulationButton.Click += new System.EventHandler(abortRegulationButtonButton_Click);
            //this.controlForm.dashboardView.voltageRegulationRepresentationComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(voltageRegulationRepresentationComboBox_valueChange);
            //Impuse Trigger Control Listeners            
            this.controlForm.dashboardView.triggerButton.Click += new System.EventHandler(triggerButton_Click);
            this.controlForm.dashboardView.choppingCheckBox.Click += new System.EventHandler(choppingCheckBox_Click);
            this.controlForm.dashboardView.decreaseChoppingTimeButton.MouseDown += new System.Windows.Forms.MouseEventHandler(decreaseChoppingTimeButton_Down);
            this.controlForm.dashboardView.decreaseChoppingTimeButton.MouseUp += new System.Windows.Forms.MouseEventHandler(decreaseChoppingTimeButton_Up);
            this.controlForm.dashboardView.choppingTimeTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(choppingTimeTextBox_valueChange);
            this.controlForm.dashboardView.increaseChoppingTimeButton.MouseDown += new System.Windows.Forms.MouseEventHandler(increaseChoppingTimeButton_Down);
            this.controlForm.dashboardView.increaseChoppingTimeButton.MouseUp += new System.Windows.Forms.MouseEventHandler(increaseChoppingTimeButton_Up);
            //Measuring Sphere Gap Listeners
            // this.controlForm.runView.decreaseMeasuringGapButton.MouseDown += new System.Windows.Forms.MouseEventHandler(decreaseMeasureeGap_Down);
            //this.controlForm.runView.decreaseMeasuringGapButton.MouseUp += new System.Windows.Forms.MouseEventHandler(decreaseMeasureGap_Up);
            this.controlForm.dashboardView.trafSpeedTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(trafSpeedTextBox_valueChange);
            this.controlForm.dashboardView.trafSpeedTrackBar.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(trafSpeedTrackBar_valueChange);
            //this.controlForm.runView.increaseMeasuringGapButton.MouseDown += new System.Windows.Forms.MouseEventHandler(increaseMeasureGapButton_Down);
            //this.controlForm.runView.increaseMeasuringGapButton.MouseUp += new System.Windows.Forms.MouseEventHandler(increaseMeasureGapButton_Up);
            //Sphere Gap Listeners
            this.controlForm.dashboardView.decreaseImpulseGapButton.MouseDown += new System.Windows.Forms.MouseEventHandler(decreaseImpulseGap_Down);
            this.controlForm.dashboardView.decreaseImpulseGapButton.MouseUp += new System.Windows.Forms.MouseEventHandler(decreaseImpulseGap_Up);
            this.controlForm.dashboardView.impulseGapTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(impulseGapTextBox_valueChange);
            this.controlForm.dashboardView.increaseImpulseGapButton.MouseDown += new System.Windows.Forms.MouseEventHandler(increaseImpulseGapButton_Down);
            this.controlForm.dashboardView.increaseImpulseGapButton.MouseUp += new System.Windows.Forms.MouseEventHandler(increaseImpulseGapButton_Up);
            this.controlForm.dashboardView.impulseSelectedRadioButton.Click += new System.EventHandler(impulseSelectedRadioButton_Click);
            this.controlForm.dashboardView.measuringSelectedRadioButton.Click += new System.EventHandler(measuringSelectedRadioButton_Click);
            this.controlForm.dashboardView.motorInitButton.Click += new System.EventHandler(motorInitButtonButton_Click);
            //Pressure Control Listeners
            this.controlForm.dashboardView.decreasePressureButton.MouseDown += new System.Windows.Forms.MouseEventHandler(decreasePressureButton_Down);
            this.controlForm.dashboardView.decreasePressureButton.MouseUp += new System.Windows.Forms.MouseEventHandler(decreasePressureButton_Up);
            this.controlForm.dashboardView.pressureTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(pressureTextBox_valueChange);
            this.controlForm.dashboardView.increasePressureButton.MouseDown += new System.Windows.Forms.MouseEventHandler(increasePressureButton_Down);
            this.controlForm.dashboardView.increasePressureButton.MouseUp += new System.Windows.Forms.MouseEventHandler(increasePressureButton_Up);
            this.controlForm.dashboardView.compressorPowerCheckBox.Click += new System.EventHandler(compressorPowerCheckBox_Click);
            this.controlForm.dashboardView.vacuumPowerCheckBox.Click += new System.EventHandler(vacuumPowerCheckBox_Click);

            //***********************************************************************************************************
            //***                                  RUNVIEW EVENT LISTENERS                                          *****
            //***********************************************************************************************************


            //Mode selection listeners
            this.controlForm.runView.WithstandRadioButton.Click += new System.EventHandler(testWithstandRadioButton_Click);
            this.controlForm.runView.DisruptiveRadioButton.Click += new System.EventHandler(testDisruptiveRadioButton_Click);
            this.controlForm.runView.onOffAutoButton.Click += new System.EventHandler(onOffAutoButton_Click);
            this.controlForm.runView.testDurationTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(testDurationTextBox_valueChange);
            this.controlForm.runView.testVoltageTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(testVoltageTextBox_valueChange);
            this.controlForm.runView.impulseVoltageLevelsTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(voltageLevelsTextBox_valueChange);
            this.controlForm.runView.impPerLevelTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(impPerLevelTextBox_valueChange);
            this.controlForm.runView.abortAutoTestButton.Click += new System.EventHandler(abortAutoTestButton_Click);
            this.controlForm.runView.voltageComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(autoTestVoltageComboBox_valueChange);
            this.controlForm.runView.acOutputComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(acOutputAutoComboBox_valueChange);
            this.controlForm.runView.dcOutputComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(dcOutputAutoComboBox_valueChange);
            this.controlForm.runView.impulseOutputComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(impulseOutputAutoComboBox_valueChange);
            this.controlForm.runView.createReportButton.Click += new System.EventHandler(createReportButton_Click);
            this.controlForm.runView.exportValuesButton.Click += new System.EventHandler(exportValuesButton_Click);
            this.controlForm.runView.impulseLimitsButton.Click += new System.EventHandler(impulseLimitsButton_Click);
            this.controlForm.runView.impulseParametersButton.Click += new System.EventHandler(impulseParametersButton_Click);
            this.controlForm.runView.dynamicLogoPictureBox.Click += new System.EventHandler(dynamicLogoPictureBox_Click);
            //this.controlForm.runView.impulseLimitsButton.Enter += new System.EventHandler(impulseLimitsButton_Enter);

            this.autoTest.PropertyChanged += triggerRequest_PropertyChanged;




            //this.controlForm.runView.measurementTypeComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(autoTestMeasTypeComboBox_valueChange);

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
            //this.measuringForm.impulseEnableCheckBox.Click += new System.EventHandler(impulseEnableCheckBox_Click);
            this.measuringForm.impulsePreTriggerTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(impulsePreTriggerTextBox_valueChange);
            //Common Controls Listeners
            this.measuringForm.resolutionComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(resolutionComboBox_valueChange);
            this.measuringForm.timeBaseComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(timeBaseComboBox_valueChange);
            this.measuringForm.exportChartButton.Click += new System.EventHandler(exportChartButton_Click);

            //***********************************************************************************************************
            //***                                  CURSOR MENU EVENT LISTENERS                                      *****
            //***********************************************************************************************************
            this.measuringForm.chart.cursorMenu.acChannelRadioButton.Click += new System.EventHandler(this.acChannelRadioButton_Click);
            this.measuringForm.chart.cursorMenu.dcChannelRadioButton.Click += new System.EventHandler(this.dcChannelRadioButton_Click);

        }


        // 
        private void trafSpeedTrackBar_valueChange(object sender, ValueChangeEventArgs e)
        {
            controlForm.dashboardView.trafSpeedTextBox.Value = (float)e.Value;            
            
        }

        private void dynamicLogoPictureBox_Click(object sender, EventArgs e)
        {
            controlForm.runView.dynamicLogoPictureBox.Visible = false;
        }

        // TriggerRequest has been received from autotest
        private void triggerRequest_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // If triggerrequest = true, trigger an impulse. else ignore, we are just resetting
            if (autoTest.TriggerRequest)
            {

                measuringForm.chart.Series["impulseSeries"].Points.Clear();

                // Make sure we can see the whole puls
                SetChartTimeBase("2 us/Div");

                //Set up the measuringForm chart INTERIM VALUES - TO BE SET DYNAMICALLY!!!!!!!
                if (PIO1.regulatedVoltageValue < 25) SetImpulseChartVoltageRange(2);
                else if ((PIO1.regulatedVoltageValue >= 25) && (PIO1.regulatedVoltageValue < 60)) SetImpulseChartVoltageRange(3);
                else if ((PIO1.regulatedVoltageValue >= 60) && (PIO1.regulatedVoltageValue < 180)) SetImpulseChartVoltageRange(4);
                else if ((PIO1.regulatedVoltageValue >= 25) && (PIO1.regulatedVoltageValue < 80)) SetImpulseChartVoltageRange(5);


                // Shoot
                TriggerImpulse();
            }
        }

        private void impulseParametersButton_Click(object sender, EventArgs e)
        {
            controlForm.runView.impulseLimitPanel.Visible = true;
        }


        // Impulse parameter control panel 
        private void impulseLimitsButton_Click(object sender, EventArgs e)
        {
            controlForm.runView.impulseLimitPanel.Visible = false;
        }


        // Voltage measurement type has been changed in auto test page
        private void autoTestMeasTypeComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            // Set the test norification text 
            GetTestType();
        }


        //***********************************************************************************************************
        //***                                  RUNVIEW EVENT HANDLERS                                          *****
        //***********************************************************************************************************


        // Withstand test type selected
        private void testWithstandRadioButton_Click(object sender, EventArgs e)
        {
            disableForControls();
            autoTest.testIsWithstand = true;
            UpdateRnTestVoltageMax();
            GetTestType();
            UpdateResultLabels();
        }

        // Disruptive Discharge test type selected
        private void testDisruptiveRadioButton_Click(object sender, EventArgs e)
        {
            disableForControls();
            autoTest.testIsWithstand = false;
            UpdateRnTestVoltageMax();
            GetTestType();
            UpdateResultLabels();
        }

        // Voltage type has been changed in auto test page
        private void autoTestVoltageComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            // Set the test notification text
            UpdateRnTestVoltageMax();
            GetTestType();
            UpdateResultLabels();
            if (e.Text == "Imp")
            {
                ImpulseDisplaySelected();
            }
            else
                acdcDisplaySelected();
        }

        private void impulseOutputAutoComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            impAutoTypeIndex = (int)e.Value;
            SetImpOutputType(impAutoTypeIndex);
            UpdateRnTestVoltageMax();
            GetTestType();
            UpdateResultLabels();
        }

        private void dcOutputAutoComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            dcAutoTypeIndex = (int)e.Value;
            SetDCOutputType(dcAutoTypeIndex);
            UpdateRnTestVoltageMax();
            GetTestType();
            UpdateResultLabels();
        }

        private void acOutputAutoComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            acAutoTypeIndex = (int)e.Value;
            SetACOutputType(acAutoTypeIndex);
            UpdateRnTestVoltageMax();
            GetTestType();
            UpdateResultLabels();
        }

        // Experiment Start button
        private void onOffAutoButton_Click(object sender, EventArgs e)
        {
            if (controlForm.runView.onOffAutoButton.isChecked)
            {
                // Start request and is not in paused state
                if (!autoTest.isPaused)
                {
                    // Restarting after a failed test? - Reset osc so it doesn't get stuck at incorrect range
                    if ((PIO1.minUPos) && (Convert.ToDouble(controlForm.runView.acValueLabel.Text) > 5))
                    {
                        SetACVoltageRange(2);
                        Thread.Sleep(500);
                        SetACVoltageRange(0);
                    }


                    // Initialize test parameters and connect power
                    autoTest.StartTest();
                }
                // Start is On, but in paused state
                else
                {
                    // Reset flags and reconnect power
                    bool resumeReady = autoTest.StartTest(); autoTest.ResumeTest();

                    if (resumeReady)
                    {
                        // Regulate to testVoltage
                        autoTest.GoToVoltageAuto();
                    }
                }

            }
            else
            {
                // Search has been been started but targetVoltage not reached 
                autoTest.PauseTest();

                // Set label text to warn for trip on resume at high voltages
                controlForm.messageLabel.Text = "Attention! Resuming at high voltages can cause overcurrent protection to trip.";
            }

        }

        // Stop experiment
        private void abortAutoTestButton_Click(object sender, EventArgs e)
        {
            autoTest.AbortTest();

        }

        // Voltage to test at
        private void testVoltageTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {

        }

        // length of time at test voltage
        private void testDurationTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {

        }

        // Ammoint of impulse levels to run
        private void voltageLevelsTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {

        }
        // Impulses to run at each level
        private void impPerLevelTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {

        }

        // Get relevant values and create a report 
        private void createReportButton_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }

        // Export report to pdf
        public void GenerateReport()
        {
            if (controlForm.runView.voltageComboBox.SetSelected == "Imp")
            {
                // Set a few special variables
                autoTest.PrepareImpulseReport();
            }
            ReportGen latestreport = new ReportGen(controlForm.runView, measuringForm, controlForm.modeLabel.Text);
            latestreport.GenerateReportNow();
        }

        // Export to CSV
        private void exportValuesButton_Click(object sender, EventArgs e)
        {
            ReportGen latestreport = new ReportGen(controlForm.runView, measuringForm, controlForm.modeLabel.Text);
            // Check for null list !!!!!!!!!!!!!!!!!!

            if (controlForm.runView.voltageComboBox.SetSelected == "Imp")
            {
                
                autoTest.impulseData = data;
                latestreport.ExportValues(autoTest.impulseData.x, autoTest.impulseData.y);
            }
            else
            {
                latestreport.ExportValues(autoTest.xList.ToArray(), autoTest.yList.ToArray());
            }
           
        }


        //***********************************************************************************************************
        //***                                    CONTROL FORM EVENT HANDLERS                                  *****
        //***********************************************************************************************************

        // DashboardView selected
        private void dashboardTab_Click(object sender, EventArgs e)
        {
            InitializeDbView();

        }

        // RunView selected
        private void runExperimentTab_Click(object sender, EventArgs e)
        {
            InitializeRnView();

        }

        // SetupView selected
        private void setupTab_Click(object sender, EventArgs e)
        {
            controlForm.modeLabel.Text = "Setup/Config";
        }


        //***********************************************************************************************************
        //***                                    MEASURING FORM EVENT HANDLERS                                  *****
        //***********************************************************************************************************
        private void acdcRadioButton_Click(object sender, EventArgs e)
        {
            if (this.measuringForm.acdcRadioButton.isChecked)
            {
                // Set the parameters
                acdcDisplaySelected();
            }
        }

        private void impulseRadioButton_Click(object sender, EventArgs e)
        {

            if (this.measuringForm.impulseRadioButton.isChecked)
            {
                // Set the parameters
                ImpulseDisplaySelected();                
            }

        }

        // AC/DC selected to display in chart
        public void acdcDisplaySelected()
        {
            this.measuringForm.impulseStatusLabel.Text = "Disabled";
            this.measuringForm.acdcRadioButton.isChecked = true;
            this.measuringForm.impulseRadioButton.isChecked = false;
            measuringForm.impulseRadioButton.Invalidate();
            measuringForm.acdcRadioButton.Invalidate();
            picoScope.TimePerDivision = 5;
            picoScope.StreamingInterval = 1000;
            this.measuringForm.chart.setVoltsPerDiv(6502.4);
            this.measuringForm.chart.setTimePerDiv(50000);
            acChannel.IncrementIndex = 1;
            dcChannel.IncrementIndex = 1;
            this.measuringForm.acChannelPanel.Visible = true;
            this.measuringForm.dcChannelPanel.Visible = true;
            this.measuringForm.impulseChannelPanel.Visible = false;
            this.measuringForm.chart.Series["impulseSeries"].Points.Clear();
            this.measuringForm.chart.cursorMenu.acChannelRadioButton.isChecked = true;
            this.measuringForm.chart.cursorMenu.dcChannelRadioButton.isChecked = false;
            this.measuringForm.chart.cursorMenu.setScaleFactor(acChannel.getScaleFactor(), acChannel.DCOffset);
            this.measuringForm.chart.cursorMenu.setTimeScaleFactor(50000, picoScope.TimePerDivision);
            this.measuringForm.chart.cursorMenu.resizeUp();
            this.measuringForm.chart.updateCursorMenu();
            this.measuringForm.timeBaseComboBox.setCollection = new string[] {
                                                                                "2 ms/Div",
                                                                                "5 ms/Div",
                                                                                "10 ms/Div"};
            this.measuringForm.timeBaseComboBox.SetSelected = "5 ms/Div";
            this.measuringForm.timeBaseLabel.Text = "5 ms/Div";
            picoScope.TimeBaseUnit = "ms";
            
        }

        // Impulse selected to display in chart
        public void ImpulseDisplaySelected()
        {

            // We dont always select impulse by clicking the radiobutton, show it as selected.
            this.measuringForm.impulseStatusLabel.Text = "Enabled";
            this.measuringForm.acStatusLabel.Text = "Disabled";
            this.measuringForm.dcStatusLabel.Text = "Disabled";
            this.measuringForm.acdcRadioButton.isChecked = false;
            this.measuringForm.impulseRadioButton.isChecked = true;
            measuringForm.impulseRadioButton.Invalidate();
            measuringForm.acdcRadioButton.Invalidate();
            this.measuringForm.acChannelPanel.Visible = false;
            this.measuringForm.dcChannelPanel.Visible = false;
            this.measuringForm.impulseChannelPanel.Visible = true;
            streamMode = false;
            picoScope.stopStreaming();
            picoScope.setFastStreamDataBuffer();
            picoScope.TimePerDivision = 2;
            this.measuringForm.chart.setVoltsPerDiv(6502.4);
            this.measuringForm.chart.setTimePerDiv(10000);
            picoScope.BlockSamples = 10000;
            impulseChannel.IncrementIndex = 6;
            this.measuringForm.acEnableCheckBox.isChecked = false;
            this.measuringForm.dcEnableCheckBox.isChecked = false;
            this.measuringForm.chart.Series["acSeries"].Points.Clear();
            this.measuringForm.chart.Series["dcSeries"].Points.Clear();

            this.measuringForm.chart.cursorMenu.setScaleFactor(impulseChannel.getScaleFactor(), impulseChannel.DCOffset * impulseChannel.DividerRatio);
            this.measuringForm.chart.cursorMenu.setTimeScaleFactor(10000, picoScope.TimePerDivision);
            this.measuringForm.chart.cursorMenu.resizeDown();
            this.measuringForm.chart.updateCursorMenu();
            this.measuringForm.timeBaseComboBox.setCollection = new string[] {
                                                                                "200 ns/Div",
                                                                                "500 ns/Div",
                                                                                "1 us/Div",
                                                                                "2 us/Div",
                                                                                "5 us/Div",
                                                                                "10 us/Div",
                                                                                "20 us/Div"};
            this.measuringForm.timeBaseComboBox.SetSelected = "2 us/Div";
            this.measuringForm.timeBaseLabel.Text = "2 us/Div";
            picoScope.streamStarted = false;
            picoScope._autoStop = false;
            fastStreamMode = true;
            picoScope.TimeBaseUnit = "us";

        }

        private void acVoltageRangeComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            SetACVoltageRange(e.Value);
        }

        private void SetACVoltageRange(double acVoltageRangeIn)
        {
            pauseStream();
            if (acVoltageRangeIn == 0)
                acChannel.VoltageAutoRange = true;
            else
                acChannel.VoltageAutoRange = false;
            picoScope.setChannelVoltageRange(0, (Imports.Range)acVoltageRangeIn + 3);
            rebootStream();


            if (this.measuringForm.chart.cursorMenu.acChannelRadioButton.isChecked)
            {
                this.measuringForm.chart.cursorMenu.setScaleFactor(acChannel.getScaleFactor(), acChannel.DCOffset);
                this.measuringForm.chart.updateCursorMenu();
            }
            this.measuringForm.acScaleLabel.Text = acChannel.rangeToVolt() + "kV/Div";
        }

        private void acEnableCheckBox_Click(object sender, EventArgs e)
        {

            if (this.measuringForm.acEnableCheckBox.isChecked)
            {
                fastStreamMode = false;
                if (!streamMode)
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
            if (e.Value == 0)
                dcChannel.VoltageAutoRange = true;
            else
                dcChannel.VoltageAutoRange = false;

            picoScope.setChannelVoltageRange(1, (Imports.Range)e.Value + 3);
            rebootStream();

            if (this.measuringForm.chart.cursorMenu.dcChannelRadioButton.isChecked)
            {
                this.measuringForm.chart.cursorMenu.setScaleFactor(dcChannel.getScaleFactor(), dcChannel.DCOffset);
                this.measuringForm.chart.updateCursorMenu();
            }
            this.measuringForm.dcScaleLabel.Text = dcChannel.rangeToVolt() + "kV/Div";

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

            SetImpulseChartVoltageRange(e.Value);
        }


        private void SetImpulseChartVoltageRange(double rawImpulseVoltageRange)
        {

            pauseStream();
            picoScope.setChannelVoltageRange(2, (Imports.Range)rawImpulseVoltageRange + 4);
            picoScope.setDCoffset(2, (float)(-1 * impulseChannel.Polarity * impulseChannel.rangeToVolt() * 0.8f));
            rebootStream();
            this.measuringForm.chart.cursorMenu.setScaleFactor(impulseChannel.getScaleFactor(), impulseChannel.DCOffset * impulseChannel.DividerRatio);
            this.measuringForm.chart.updateCursorMenu();
            this.measuringForm.impulseScaleLabel.Text = impulseChannel.rangeToVolt() + "kV/Div";
        }


        private void impulseEnableCheckBox_Click(object sender, EventArgs e)
        {

        }


        private void impulsePreTriggerTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            picoScope.BlockPreTrigger = (double)(e.Value / 100);
            this.measuringForm.preTriggerLabel.Text = e.Value + " %";
        }

        private void resolutionComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            pauseStream();
            picoScope.Resolution = (Imports.DeviceResolution)e.Value;
            this.measuringForm.resolutionLabel.Text = e.Text;
            rebootStream();
        }

        private void timeBaseComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            SetChartTimeBase(e.Text);
        }

        private void SetChartTimeBase(string timeBaseValueIn)
        {
            this.measuringForm.timeBaseLabel.Text = timeBaseValueIn;
            if (this.measuringForm.acdcRadioButton.isChecked)
            {
                this.measuringForm.chart.Series["acSeries"].Points.Clear();
                this.measuringForm.chart.Series["dcSeries"].Points.Clear();
                picoScope.TimeBaseUnit = "ms";
                if (timeBaseValueIn.Equals("2 ms/Div"))
                {
                    picoScope.StreamingInterval = 400;
                    picoScope.TimePerDivision = 2;

                }
                else if (timeBaseValueIn.Equals("5 ms/Div"))
                {
                    picoScope.StreamingInterval = 1000;
                    picoScope.TimePerDivision = 5;
                }
                else
                {
                    picoScope.StreamingInterval = 2000;
                    picoScope.TimePerDivision = 10;
                    
                }
                this.measuringForm.chart.setTimePerDiv(50000);
                this.measuringForm.chart.cursorMenu.setTimeScaleFactor(50000, picoScope.TimePerDivision);

            }
            else
            {
                if (timeBaseValueIn.Equals("200 ns/Div"))
                {
                    picoScope.BlockSamples = 1000;
                    picoScope.TimePerDivision = 200;
                    picoScope.TimeBaseUnit = "ns";

                }
                else if (timeBaseValueIn.Equals("500 ns/Div"))
                {
                    picoScope.BlockSamples = 2500;
                    picoScope.TimePerDivision = 500;
                    picoScope.TimeBaseUnit = "ns";
                }
                else if (timeBaseValueIn.Equals("1 us/Div"))
                {
                    picoScope.BlockSamples = 5000;
                    picoScope.TimePerDivision = 1;
                    picoScope.TimeBaseUnit = "us";
                }
                else if (timeBaseValueIn.Equals("2 us/Div"))
                {
                    picoScope.BlockSamples = 10000;
                    picoScope.TimePerDivision = 2;
                    picoScope.TimeBaseUnit = "us";
                }
                else if (timeBaseValueIn.Equals("5 us/Div"))
                {
                    picoScope.BlockSamples = 25000;
                    picoScope.TimePerDivision = 5;
                    picoScope.TimeBaseUnit = "us";
                }
                else if (timeBaseValueIn.Equals("10 us/Div"))
                {
                    picoScope.BlockSamples = 50000;
                    picoScope.TimePerDivision = 10;
                    picoScope.TimeBaseUnit = "us";
                }
                else if (timeBaseValueIn.Equals("20 us/Div"))
                {
                    picoScope.BlockSamples = 100000;
                    picoScope.TimePerDivision = 20;
                    picoScope.TimeBaseUnit = "us";
                }
                this.measuringForm.chart.setTimePerDiv(picoScope.BlockSamples);
                this.measuringForm.chart.cursorMenu.setTimeScaleFactor(picoScope.BlockSamples, picoScope.TimePerDivision);
            }
            this.measuringForm.chart.updateCursorMenu();
        }

        private void exportChartButton_Click(object sender, EventArgs e)
        {            
            ExportValues();
        }       

        private bool isDataInitilized(Channel channel)
        {
           
            if (channel.scaledData != null)
                return true;
            else
                return false;
        }

        private void ExportValues()
        {

            pauseStream();
            string Operator = this.controlForm.runView.operatorTextBox.Text;
            string[] channelNames;
            bool collectImpulseData = this.measuringForm.impulseRadioButton.isChecked & isDataInitilized(impulseChannel);
            bool collectACData = this.measuringForm.acEnableCheckBox.isChecked & isDataInitilized(acChannel);
            bool collectDCData = this.measuringForm.dcEnableCheckBox.isChecked & isDataInitilized(dcChannel);
            int noChannels = 0;
            double[] x;
            double[][] y;

            if (collectImpulseData)
            {
                channelNames = new string[1];
                channelNames[0] = "Impulse Channel";
                noChannels = 1;
                x = impulseChannel.scaledData[0].x;
                y = new double[noChannels][];
                y[0] = impulseChannel.scaledData[0].y;               
            }
            else if (collectACData && collectDCData)
            {
                channelNames = new string[2];
                channelNames[0] = "AC Channel";
                channelNames[1] = "DC Channel";
                noChannels = 2;
                x = acChannel.scaledData[0].x;
                y = new double[noChannels][];
                y[0] = acChannel.scaledData[0].y;
                y[1] = dcChannel.scaledData[0].y;
            }
            else if (collectACData)
            {
                channelNames = new string[1];
                channelNames[0] = "AC Channel";
                noChannels = 1;
                x = acChannel.scaledData[0].x;
                y = new double[noChannels][];
                y[0] = acChannel.scaledData[0].y;
                Console.WriteLine("ACLENGHT" + acChannel.scaledData[0].y.Length);
            }
            else if (collectDCData)
            {
                channelNames = new string[1];
                channelNames[0] = "DC Channel";
                noChannels = 1;
                x = dcChannel.scaledData[0].x;
                y = new double[noChannels][];                
                y[0] = dcChannel.scaledData[0].y;
            }
            else
            {
                CustomPopUp popUp = new CustomPopUp();
                popUp.Owner = this.measuringForm;
                popUp.ShowDialog();
                rebootStream();
                return;
            }
            

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Comma-Separated Value (.csv) | *.csv ";
            saveFileDialog1.Title = "Save file";
            saveFileDialog1.ShowDialog();
            var finalPath = saveFileDialog1.FileName;

            if (finalPath != "")
            {
                var myExport = new CsvExport();
                DateTime todaysDateRaw = DateTime.Now;
                string todaysDate = todaysDateRaw.ToString(" d MMMM yyyy, HH:mm");
                myExport.AddRow();
                myExport["Time"] = picoScope.TimeBaseUnit;

                for (int r = 0; r < noChannels; r++)
                {
                    myExport[channelNames[r]] = "Volts(kV)";
                }
                myExport["OPERATOR"] = Operator;
                myExport["DATE"] = todaysDate;
                

                for (int i = 0; i < x.Length; i++)
                {
                    myExport.AddRow();
                    myExport["Time"] = (((double)i / (double)x.Length) * (picoScope.TimePerDivision * 10));
                    for (int r = 0; r < noChannels; r++)
                    {
                        myExport[channelNames[r]] = y[r][i].ToString();
                    }
                }
               
                myExport.ExportToFile(finalPath);
            }

            rebootStream();
        }

        //***********************************************************************************************************
        //***                                     DASHBOARD VIEW EVENT HANDLERS                                 *****
        //***********************************************************************************************************

        private void acOutputComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            acTypeIndex = (int)e.Value;
            SetACOutputType(acTypeIndex);
            SetMaxAC();
        }

        private void dcOutputComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            dcTypeIndex = (int)e.Value;
            SetDCOutputType(dcTypeIndex);
            SetMaxDC();
        }

        private void impulseOutputComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            impTypeIndex = (int)e.Value;
            SetImpOutputType(impTypeIndex);
        }

        // Update the transformer motor speed parameter
        private void trafSpeedTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            if ((controlForm.dashboardView.trafSpeedTextBox.Value <= controlForm.dashboardView.trafSpeedTextBox.Max) && (controlForm.dashboardView.trafSpeedTextBox.Value >= controlForm.dashboardView.trafSpeedTextBox.Min))
            {
                trafSpeed = (int)controlForm.dashboardView.trafSpeedTextBox.Value * 10;
                controlForm.dashboardView.trafSpeedTrackBar.setPosition(((float)e.Value - 10 )/ 80);
                //controlForm.dashboardView.trafSpeedTrackBar.AutoScrollPosition = trafSpeed;
            }
        }



        //System.Timers.Timer T2 = new System.Timers.Timer(200);

        // Voltage ON/OFF Switch
        private void onOffButton_Click(object sender, EventArgs e)
        {
            // Connect K1
            if (this.controlForm.dashboardView.onOffButton.isChecked)
            {
                ClosePrimaryRequest();
            }
            // Disconnect K1
            else
            {
                // First disconnect K2
                OpenSecondaryRequest();
                this.controlForm.dashboardView.onOffSecButton.isChecked = false;
                this.controlForm.dashboardView.onOffSecButton.Invalidate();



                // If Park is selected
                if ((this.controlForm.dashboardView.parkCheckBox.isChecked) && (!PIO1.minUPos))
                {
                    this.controlForm.dashboardView.onOffButton.isChecked = true;
                    this.controlForm.dashboardView.onOffButton.Invalidate();

                    // Drive the voltage down to zero
                    PIO1.ParkTransformer();

                    // Monitor for zero position in timer
                    parkTimer.Enabled = true;
                    parkTimer.Start();
                    parkTimer.Elapsed += ParkTimer_Elapsed;
                }
                else
                {
                    // Now disconnect K1
                    OpenPrimaryRequest();
                }


                // Write a reminder to use discharge rod
                this.controlForm.messageLabel.Text = "Important! Always use the Discharge Rod to discharge components when entering the HV area.";
            }
        }

        // Used to clean up after park
        private void ParkTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (PIO1.minUPos)
            {
                this.controlForm.dashboardView.onOffButton.isChecked = false;
                this.controlForm.dashboardView.onOffButton.Invalidate();
                parkTimer.Stop();
            }
        }

        // Voltage ON/OFF Switch
        private void onOffSecButton_Click(object sender, EventArgs e)
        {
            // Connect K2
            if (this.controlForm.dashboardView.onOffSecButton.isChecked)
            {
                CloseSecondaryRequest(this.controlForm.dashboardView.overrideCheckBox.isChecked);
                this.controlForm.dashboardView.overrideCheckBox.isChecked = false;                
            }
            // Disconnect K2
            else
            {
                OpenSecondaryRequest();
            }
        }

        // The following event handlers should set variables/flags for later evaluation but to save time we check the control status isChecked property directly
        private void parkCheckBox_Click(object sender, EventArgs e) { }
        private void overrideCheckBox_Click(object sender, EventArgs e) { }

        // We want to regulate the AC input value
        private void inputVoltageRadioButton_Click(object sender, EventArgs e)
        {
            // Set max regulation value
            UpdateDbRegulatedVoltageMax();
        }

        // We want to regulate the AC output value
        private void acVoltageRadioButton_Click(object sender, EventArgs e)
        {
            // Set max regulation value
            SetMaxAC();
            UpdateDbRegulatedVoltageMax();
        }

        // We want to regulate the DC output value
        private void dcVoltageRadioButton_Click(object sender, EventArgs e)
        {
            // Set max regulation value
            SetMaxDC();
            UpdateDbRegulatedVoltageMax();
        }

        // Manual voltage decrease
        private void decreaseRegulatedVoltageButton_Down(object sender, MouseEventArgs e)
        {

            trafSpeed = (int)controlForm.dashboardView.trafSpeedTextBox.Value * 10;
            DecreaseVoltageRequest(trafSpeed);

        }

        // Stop manual voltage decrease
        private void decreaseRegulatedVoltageButton_Up(object sender, MouseEventArgs e)
        {

            PIO1.StopTransformerMotor();

        }

        // Manual voltage increase
        private void increaseRegulatedVoltageButton_Down(object sender, MouseEventArgs e)
        {

            trafSpeed = (int)controlForm.dashboardView.trafSpeedTextBox.Value * 10;
            IncreaseVoltageRequest(trafSpeed);

        }

        // Stop manual voltage increase
        private void increaseRegulatedVoltageButton_Up(object sender, MouseEventArgs e)
        {

            PIO1.StopTransformerMotor();

        }

        // Abort an active auto voltage regulation attempt
        private void abortRegulationButtonButton_Click(object sender, EventArgs e)
        {
            abortRegulation = true;
        }

        // A value has been entered against which to regulate the voltage automatically
        private void regulatedVoltageTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            // Reset any abort commands
            abortRegulation = false;

            // RunTo voltage value
            GoToVoltage(controlForm.dashboardView.regulatedVoltageTextBox.Value);

        }

        // Automated voltage set routine
        private void GoToVoltage(double valueIn)
        {
            targetVoltage = valueIn;
            Thread regUThread = new Thread(RegulateVoltage);
            regUThread.Start();

        }

        // Set dynamic bounds from user input, then start the auto process
        private void RegulateVoltage()
        {
            // Set some tolerances (we aren't perfect)
            targetVoltage = controlForm.dashboardView.regulatedVoltageTextBox.Value;
            double toleranceHi = 0.18;
            double toleranceLo = -0.18;

            // Variable to hold our selectable measured voltage value           
            double uActual = 0;

            // Pd Variables - if needed?
            float P = 0;
            float k = 4;
            float d = 0;
            double error = 10;
            double previousError = 0;
            double integral = 0;
            int intCnt = 0;
            int styr = 30;

            // Continue until found or precess aborted 
            while (((error <= toleranceLo) || (error >= toleranceHi)) && (controlForm.dashboardView.onOffButton.isChecked) && (!abortRegulation))
            {

                // Get the value to regulate agianst
                if (controlForm.dashboardView.acInputRadioButton.isChecked)
                {
                    //uActual = (float)PIO1.regulatedVoltageValue;
                    uActual = Convert.ToDouble(controlForm.dashboardView.voltageInputLabel.Text);
                }
                else if ((controlForm.dashboardView.acOutputRadioButton.isChecked) && (PIO1.K2Closed))
                {
                    uActual = Convert.ToDouble(controlForm.dashboardView.acValueLabel.Text);
                    //uActual = acChannel.getRepresentation();

                    toleranceHi = 0.2;
                    toleranceLo = -0.2;
                }
                else if ((controlForm.dashboardView.dcOutputRadioButton.isChecked) && (PIO1.K2Closed))
                {
                    uActual = Convert.ToDouble(controlForm.dashboardView.dcValueLabel.Text);
                    //uActual = dcChannel.getRepresentation();

                    toleranceHi = 0.15;
                    toleranceLo = -0.15;
                }
                else
                {
                    // Shut down, something is wrong
                    abortRegulation = true;
                    return;
                }

                error = uActual - targetVoltage;

                if (error == previousError)
                {
                    integral += 0.1;
                }
                else
                {
                    //if(integral >= 0.1)
                    //{
                    integral = 0;
                    // }
                }


                // Call the appropriate instruction
                if (error < toleranceHi)
                {

                    if ((error <= 4) && (error >= -4))
                    {
                        styr = 57 + (int)integral;
                    }
                    else
                    {
                        styr = (int)((error * -k) + 60 + integral);
                    }
                    // Voltage low, increase
                    IncreaseVoltageRequest(styr);
                }
                else if (error > toleranceLo)
                {

                    if ((error <= 4) && (error >= -4))
                    {
                        styr = 57 + (int)integral;
                    }
                    else
                    {
                        styr = (int)((error * k) + 60 + integral);
                    }
                    // Voltage high, decrease
                    DecreaseVoltageRequest(styr);
                }
                else
                {
                    // In bounds. We should only make it here once
                    StopTransformerMotorRequest();
                }

                Thread.Sleep(10);
                previousError = error;
            }

            // In bounds. We should only make it here once
            StopTransformerMotorRequest();
            abortRegulation = true;
        }

        // A voltage qualifier (rma, min, max) to regulate against has been selected  
        private void voltageRegulationRepresentationComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            // *Not needed since we regulate against the already converted UI presentation value 
        }

        // Create a signal to trigger an impulse voltage
        private void triggerButton_Click(object sender, EventArgs e)
        {
          
            TriggerImpulse();
        }

        public void TriggerImpulse()
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
            picoScope.setDCoffset(2, -1 * (float)(impulseChannel.Polarity * impulseChannel.rangeToVolt()) * 0.8f);
            this.measuringForm.impulseOffsetLabel.Text = this.impulseChannel.DCOffset + "kV";
            //Set databuffer
            picoScope.setBlockDataBuffer();
            //Set trigger Channel/Level/Type
            picoScope.setTriggerChannel(Imports.Channel.ChannelC);

            //Setup Trigger / Chopping time
            if (controlForm.dashboardView.choppingCheckBox.isChecked)
            {
                int index = (int)((float)(1000 * controlForm.dashboardView.choppingTimeTextBox.Value) / 100);

                picoScope.setupSignalGen(index);
            }
            else
            {
                picoScope.setupSignalGen(0);
            }

            //Start Block
            picoScope.startBlock();
            //Trigger Signal gen
            picoScope.triggerSignalGen();
            //Start watchDog
            triggerTimer.Start();

            autoTest.TriggerRequest = false;
        }

        // Chopping checkbox clicked
        private void choppingCheckBox_Click(object sender, EventArgs e)
        {
            // Enable/disable chopping control buttons
            InitChoppingButtons();
        }

        // Enable/disable chopping control buttons
        private void InitChoppingButtons()
        {
            if (controlForm.dashboardView.choppingCheckBox.isChecked)
            {
                controlForm.dashboardView.choppingTimeTextBox.Enabled = true;
                controlForm.dashboardView.decreaseChoppingTimeButton.Enabled = true;
                controlForm.dashboardView.increaseChoppingTimeButton.Enabled = true;
            }
            else
            {
                controlForm.dashboardView.choppingTimeTextBox.Enabled = false;
                controlForm.dashboardView.decreaseChoppingTimeButton.Enabled = false;
                controlForm.dashboardView.increaseChoppingTimeButton.Enabled = false;
            }
        }

        private void decreaseChoppingTimeButton_Down(object sender, MouseEventArgs e)
        {
            // Verify the input value
            if (controlForm.dashboardView.choppingTimeTextBox.Value > controlForm.dashboardView.choppingTimeTextBox.Min)
            {
                // Increase the delay time by one
                controlForm.dashboardView.choppingTimeTextBox.Value -= 0.1F;
            }

        }

        private void decreaseChoppingTimeButton_Up(object sender, MouseEventArgs e)
        {


        }



        private void choppingTimeTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {


        }

        private void increaseChoppingTimeButton_Down(object sender, MouseEventArgs e)
        {
            // Verify the input value
            if (controlForm.dashboardView.choppingTimeTextBox.Value < controlForm.dashboardView.choppingTimeTextBox.Max)
            {
                // Increase the delay time by one
                controlForm.dashboardView.choppingTimeTextBox.Value += 0.1F;
            }


        }

        private void increaseChoppingTimeButton_Up(object sender, MouseEventArgs e)
        {


        }

        // Sphere Gap 
        private void motorInitButtonButton_Click(object sender, EventArgs e)
        {
            InitMotorRequest();
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

            RunToPosRequest(this.controlForm.dashboardView.impulseGapTextBox.Text);

        }

        private void increaseImpulseGapButton_Down(object sender, MouseEventArgs e)
        {

            IncreaseGapRequest();

        }

        private void increaseImpulseGapButton_Up(object sender, MouseEventArgs e)
        {

            StopMotorRequest();

        }

        // HV9133 Measuring Sphere Gap selected
        private void measuringSelectedRadioButton_Click(object sender, EventArgs e)
        {

            activeMotor = HV9133;

        }

        // HV9125 Impulse Sphere Gap selected
        private void impulseSelectedRadioButton_Click(object sender, EventArgs e)
        {

            activeMotor = HV9126;

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

            SetPressureRequest(controlForm.dashboardView.pressureTextBox.Value);

        }

        private void increasePressureButton_Down(object sender, MouseEventArgs e)
        {

            IncreasePressureRequest();

        }

        private void increasePressureButton_Up(object sender, MouseEventArgs e)
        {

            StopPressureRequest();

        }

        // Manual control of the compressor power outlet
        private void vacuumPowerCheckBox_Click(object sender, EventArgs e)
        {
            EnergizeVacuumPumpOutputRequest();
        }

        // Manual control of the vacuum pump power outlet
        private void compressorPowerCheckBox_Click(object sender, EventArgs e)
        {
            EnergizeCompressorOutputRequest();
        }


        //***********************************************************************************************************
        //***                                     SETUP VIEW EVENT HANDLERS                                     *****
        //***********************************************************************************************************
        private void acCheckBox_Click(object sender, EventArgs e)
        {
            if (this.controlForm.setupView.acCheckBox.isChecked)
            {
                fastStreamMode = true;               
            }

        }

        private void acDivderDefaultButton_Click(object sender, EventArgs e)
        {
            acHighDividerValues[0] = acDefaultHighDividerValues[0];
            acHighDividerValues[1] = acDefaultHighDividerValues[1];

            if (this.controlForm.setupView.acStage1RadioButton.isChecked)
            { 
                this.controlForm.setupView.acDivder1TextBox.Value = (float)acDefaultHighDividerValues[0];
                acChannel.DividerRatio = (double)((acDefaultHighDividerValues[0] + acLowDividerValue) / acDefaultHighDividerValues[0]) / 1000;
            }
            else
            {
                this.controlForm.setupView.acDivder1TextBox.Value = (float)acDefaultHighDividerValues[1];
                acChannel.DividerRatio = (double)((acDefaultHighDividerValues[1] + acLowDividerValue) / acDefaultHighDividerValues[1]) / 1000;
            }
        
        }

        private void acDivder1TextBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            if(this.controlForm.setupView.acStage1RadioButton.isChecked)
            {
                acHighDividerValues[0] = (decimal)e.Value;
                acChannel.DividerRatio = (double)((acHighDividerValues[0] + acLowDividerValue) / acHighDividerValues[0]) / 1000;
            }
            else
            {
                acHighDividerValues[1] = (decimal)e.Value;
                acChannel.DividerRatio = (double)((acHighDividerValues[1] + acLowDividerValue) / acHighDividerValues[1]) / 1000;
            }
        }

        private void acStage1RadioButton_Click(object sender, EventArgs e)
        {
            this.controlForm.setupView.acDivder1TextBox.Value = (float)acHighDividerValues[0];
            acChannel.DividerRatio = (double)((acHighDividerValues[0] + acLowDividerValue) / acHighDividerValues[0]) / 1000;

            controlForm.setupView.dcCheckBox.Enabled = true;
            controlForm.setupView.impulseCheckBox.Enabled = true;
        }        

        private void acStage2RadioButton_Click(object sender, EventArgs e)
        {
            this.controlForm.setupView.acDivder1TextBox.Value = (float)acHighDividerValues[1];
            acChannel.DividerRatio = (double)((acHighDividerValues[1] + acLowDividerValue) / acHighDividerValues[1]) / 1000;

            controlForm.setupView.dcCheckBox.isChecked = false;
            controlForm.setupView.dcCheckBox.Enabled = false;
            controlForm.setupView.impulseCheckBox.isChecked = false;
            controlForm.setupView.impulseCheckBox.Enabled = false;

        }

        private void acStage3RadioButton_Click(object sender, EventArgs e)
        {
            this.controlForm.setupView.acDivder1TextBox.Value = (float)acHighDividerValues[1];
            acChannel.DividerRatio = (double)((acHighDividerValues[1] + acLowDividerValue) / acHighDividerValues[1]) / 1000;

            controlForm.setupView.dcCheckBox.isChecked = false;
            controlForm.setupView.dcCheckBox.Enabled = false;
            controlForm.setupView.impulseCheckBox.isChecked = false;
            controlForm.setupView.impulseCheckBox.Enabled = false;
        }

        private void dcCheckBox_Click(object sender, EventArgs e)
        {


        }

        private void dcDivderDefaultButton_Click(object sender, EventArgs e)
        {
            if (this.controlForm.setupView.dcStage1RadioButton.isChecked)
            {                
                dcChannel.DividerRatio = (double)((dcDefaultHighDividerValues[0] + dcDefaultLowDividerValue) / dcDefaultLowDividerValue) / 1000;                
            }
            else if (this.controlForm.setupView.dcStage2RadioButton.isChecked)
            {               
                dcChannel.DividerRatio = (double)((dcDefaultHighDividerValues[0] + dcDefaultHighDividerValues[1] + dcDefaultLowDividerValue) / dcDefaultLowDividerValue) / 1000;
            }
            else
            {                
                dcChannel.DividerRatio = (double)((dcDefaultHighDividerValues.Sum() + dcDefaultLowDividerValue) / dcDefaultLowDividerValue) / 1000;
            }
           
            this.controlForm.setupView.dcDivder1TextBox.Value = (float)dcDefaultHighDividerValues[0];
            this.controlForm.setupView.dcDivder2TextBox.Value = (float)dcDefaultHighDividerValues[1];
            this.controlForm.setupView.dcDivder3TextBox.Value = (float)dcDefaultHighDividerValues[2];
        }

        private void calcDCDividerRatio()
        {
            if(this.controlForm.setupView.dcStage1RadioButton.isChecked)
            {
                 dcChannel.DividerRatio = (double)((dcHighDividerValues[0] + dcLowDividerValue) / dcLowDividerValue) / 1000;
            }
            else if(this.controlForm.setupView.dcStage2RadioButton.isChecked)
            {               
                dcChannel.DividerRatio = (double)((dcHighDividerValues[0] + dcHighDividerValues[1] + dcLowDividerValue) / dcLowDividerValue) / 1000;
            }
            else
            {               
                 dcChannel.DividerRatio = (double)((dcHighDividerValues.Sum() + dcLowDividerValue) / dcLowDividerValue) / 1000;
            }
        }

        private void dcDivder1TextBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            dcHighDividerValues[0] = (decimal)e.Value;
            calcDCDividerRatio();           
        }

        private void dcDivder2TextBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            dcHighDividerValues[1] = (decimal)e.Value;
            calcDCDividerRatio();
        }

        private void dcDivder3TextBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            dcHighDividerValues[2] = (decimal)e.Value;            
            calcDCDividerRatio();
        }

        private void dcStage1RadioButton_Click(object sender, EventArgs e)
        {
            dcChannel.DividerRatio = (double)((dcHighDividerValues[0] + dcLowDividerValue) / dcLowDividerValue) / 1000;
            controlForm.setupView.impulseCheckBox.Enabled = true;
        }
        

        private void dcStage2RadioButton_Click(object sender, EventArgs e)
        {
            dcChannel.DividerRatio = (double)((dcHighDividerValues[0] + dcHighDividerValues[1] + dcLowDividerValue) / dcLowDividerValue) / 1000;
            controlForm.setupView.impulseCheckBox.isChecked = false;
            controlForm.setupView.impulseCheckBox.Enabled = false;
        }

        private void dcStage3RadioButton_Click(object sender, EventArgs e)
        {

            dcChannel.DividerRatio = (double)((dcHighDividerValues.Sum() + dcLowDividerValue) / dcLowDividerValue) / 1000;
            controlForm.setupView.impulseCheckBox.isChecked = false;
            controlForm.setupView.impulseCheckBox.Enabled = false;
        }


        private void impulseCheckBox_Click(object sender, EventArgs e)
        {

        }

        private void impulseDivderDefaultButton_Click(object sender, EventArgs e)
        {
            for (int r = 0; r < impulseDefaultLowDividerValues.Length; r++)
            {
                impulseLowDividerValues[r] = impulseDefaultLowDividerValues[r];
                impulseHighDividerValues[r] = impulseDefaultHighDividerValues[r];
            }

            if (this.controlForm.setupView.impulseStage1RadioButton.isChecked)
            {
                this.controlForm.setupView.impulseLowDivderTextBox.Value = (float)impulseDefaultLowDividerValues[0];
                impulseChannel.DividerRatio = (double)(impulseAttenuatorRatio * (impulseDefaultHighDividerValues[0] + impulseDefaultLowDividerValues[0]) / impulseDefaultHighDividerValues[0]) / 1000;
            }
            else if (this.controlForm.setupView.impulseStage2RadioButton.isChecked)
            {
                decimal highDivider = 1 / (1 / impulseDefaultHighDividerValues[0] + 1 / impulseDefaultHighDividerValues[1]);
                this.controlForm.setupView.impulseLowDivderTextBox.Value = (float)impulseDefaultLowDividerValues[1];
                impulseChannel.DividerRatio = (double)(impulseAttenuatorRatio * (highDivider + impulseDefaultLowDividerValues[1]) / highDivider) / 1000;
            }
            else
            {
                decimal highDivider = 1 / (1 / impulseDefaultHighDividerValues[0] + 1 / impulseDefaultHighDividerValues[1] + 1 / impulseDefaultHighDividerValues[2]);
                this.controlForm.setupView.impulseLowDivderTextBox.Value = (float)impulseDefaultLowDividerValues[2];
                impulseChannel.DividerRatio = (double)(impulseAttenuatorRatio * (highDivider + impulseDefaultLowDividerValues[2]) / highDivider) / 1000;
            }

            this.controlForm.setupView.impulseDivder1TextBox.Value = (float)impulseDefaultHighDividerValues[0];
            this.controlForm.setupView.impulseDivder2TextBox.Value = (float)impulseDefaultHighDividerValues[1];
            this.controlForm.setupView.impulseDivder3TextBox.Value = (float)impulseDefaultHighDividerValues[2];
        }

        private void calcImpulseDividerRatio()
        {
            if(this.controlForm.setupView.impulseStage1RadioButton.isChecked)
            {
                impulseChannel.DividerRatio = (double)(impulseAttenuatorRatio * (impulseHighDividerValues[0] + impulseLowDividerValues[0]) / impulseHighDividerValues[0]) / 1000;
            }
            else if(this.controlForm.setupView.impulseStage2RadioButton.isChecked)
            {               
                decimal highDivider = 1 / (1 / impulseHighDividerValues[0] + 1 / impulseHighDividerValues[1]);                
                impulseChannel.DividerRatio = (double)(impulseAttenuatorRatio * (highDivider + impulseLowDividerValues[1]) / highDivider) / 1000;
            }
            else
            {               
                decimal highDivider = 1 / (1 / impulseHighDividerValues[0] + 1 / impulseHighDividerValues[1] + 1 / impulseHighDividerValues[2]);
                impulseChannel.DividerRatio = (double)(impulseAttenuatorRatio * (highDivider + impulseLowDividerValues[2]) / highDivider) / 1000;
            }
        }

        private void impulseLowDivderTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            if(this.controlForm.setupView.impulseStage1RadioButton.isChecked)
            {                
                impulseLowDividerValues[0] = (decimal)e.Value;             
            }
            else if(this.controlForm.setupView.impulseStage2RadioButton.isChecked)
            {
                impulseLowDividerValues[1] = (decimal)e.Value;             
            }
            else
            {
                impulseLowDividerValues[2] = (decimal)e.Value;             
            }

            calcImpulseDividerRatio();
        }        

        private void impulseDivder1TextBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            impulseHighDividerValues[0] = (decimal)e.Value;
            calcImpulseDividerRatio();
        }

        private void impulseDivder2TextBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            impulseHighDividerValues[1] = (decimal)e.Value;
            calcImpulseDividerRatio();
        }

        private void impulseDivder3TextBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            impulseHighDividerValues[2] = (decimal)e.Value;
            calcImpulseDividerRatio();
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
            
        }

        private void formsCloseButton_Click(object sender, EventArgs e)
        {
            closeApplication();            
        }

        public void closeApplication()
        {
            loopTimer.Stop();
            loopTimer.Dispose();
            picoScope.stopStreaming();
            picoScope.closeDevice();
            try
            {
                autoTest.sampleTimer.Stop();
                autoTest.sampleTimer.Dispose();
                autoTest.triggerTimeoutTimer.Stop();
                autoTest.triggerTimeoutTimer.Dispose();
                autoTest.impulseRoutineTimer.Stop();
                autoTest.impulseRoutineTimer.Dispose();
                autoTest.updateVoltageOutputValuesTimer.Stop();
                autoTest.updateVoltageOutputValuesTimer.Dispose();
                autoTest.logoGrowEffectTimer.Stop();
                autoTest.logoGrowEffectTimer.Dispose();
                autoTest.logoShrinkEffectTimer.Stop();
                autoTest.logoShrinkEffectTimer.Dispose();

            }
            catch (System.NullReferenceException ex)
            {
                // MessageBox.Show(ex.Message);

            }

            this.measuringForm.Close();
            this.controlForm.Close();

            if (System.Windows.Forms.Application.MessageLoop)
            {
                // WinForms app
                System.Windows.Forms.Application.Exit();
                Environment.Exit(0);
            }
            else
            {
                // Console app
                System.Environment.Exit(1);
            }
        }


        // Set up the new devices. Should maybe return bool to notify of init problems
        public void InitializeDevices()
        {
            // Create new device objects
            PIO1 = new NA9739Device(serialPort1);
            HV9126 = new PD1161Device(3, serialPort1);
            HV9133 = new PD1161Device(4, serialPort1);
            activeMotor = HV9126;

            report = new ReportGen(controlForm.runView, measuringForm, controlForm.modeLabel.Text);

            //// Check communication - see who is connected (Not Implemented)
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
        
        // Set the controls to the correct states and values. called on entry
        private void InitializeRnView()
        {

            Thread.Sleep(200);
       
            // Top info strip notice
            GetTestType();

            // Set the voltage representations (rms, pk ....) for each voltage
            SetACOutputType(acAutoTypeIndex);
            SetDCOutputType(dcAutoTypeIndex);
            SetImpOutputType(impAutoTypeIndex);

            // Update the textbox max value
            UpdateRnTestVoltageMax();

            // Fill date textbox with today's date
            DateTime todaysDateRaw = DateTime.Now;
            string todaysDate = todaysDateRaw.ToString(" d MMMM yyyy, HH:mm");
            controlForm.runView.dateTextBox.Text = todaysDate;
            
            // Hide the chart temporarily
            controlForm.runView.autoTestChart.Visible = false;
            controlForm.runView.autoTestChart.Invalidate();

            // Run in the cool Terco logo
            autoTest.GrowLogo();
        }

        private void UpdateRnTestVoltageMax()
        {
            if (controlForm.runView.voltageComboBox.SetSelected == "AC")
            {
                // Update the max value
                SetAutoMaxAC();

                // Update the textbox value
                if (controlForm.runView.DisruptiveRadioButton.isChecked)
                {
                    controlForm.runView.testVoltageTextBox.Value = acAutoOutputMax;
                }
                else
                {
                    controlForm.runView.testVoltageTextBox.Value = 33;
                }
                controlForm.runView.testVoltageTextBox.Invalidate();

            }
            else if (controlForm.runView.voltageComboBox.SetSelected == "DC")
            {
                // Update the max value
                SetAutoMaxDC();

                // Update the textbox value
                if (controlForm.runView.DisruptiveRadioButton.isChecked)
                {
                    controlForm.runView.testVoltageTextBox.Value = dcAutoOutputMax;
                }
                else
                {
                    controlForm.runView.testVoltageTextBox.Value = 20;
                }
                controlForm.runView.testVoltageTextBox.Invalidate();

            }
            else if (controlForm.runView.voltageComboBox.SetSelected == "Imp")
            {
                // Update the max value
                SetAutoMaxImp();

                // Update the textbox value
                if (controlForm.runView.DisruptiveRadioButton.isChecked)
                {
                    controlForm.runView.testVoltageTextBox.Value = impAutoOutputMax;
                    controlForm.runView.impulseVoltageLevelsTextBox.Value = 20;
                    controlForm.runView.impPerLevelTextBox.Value = 1;
                }
                else
                {
                    controlForm.runView.testVoltageTextBox.Value = 67;
                    controlForm.runView.impulseVoltageLevelsTextBox.Value = 5;
                    controlForm.runView.impPerLevelTextBox.Value = 3;
                }
                controlForm.runView.testVoltageTextBox.Invalidate();
                controlForm.runView.impulseStepSizeTextBox.Invalidate();
                controlForm.runView.impulseStartVoltageTextBox.Invalidate();
            }
          
        }

        public void SetAutoMaxAC()
        {
            // Limit values
            if (acAutoTypeIndex == 0) acAutoOutputMax = (setupNr / 100) * 100;
            if (acAutoTypeIndex == 1) acAutoOutputMax = (setupNr / 100) * 140;

            if (controlForm.runView.voltageComboBox.SetSelected == "AC")
            {
                controlForm.runView.testVoltageTextBox.Max = acAutoOutputMax;   
            }
        }

        private void SetAutoMaxDC()
        {
            // Limit values
            if (dcAutoTypeIndex == 0) dcAutoOutputMax = ((setupNr - 100) / 10) * 140;
            if (dcAutoTypeIndex == 1) dcAutoOutputMax = ((setupNr - 100) / 10) * 140;

            if (controlForm.runView.voltageComboBox.SetSelected == "DC")
            {
                controlForm.runView.testVoltageTextBox.Max = dcOutputMax; 
            }
        }

        private void SetAutoMaxImp()
        {
            // Limit values
            if (impAutoTypeIndex == 0) impAutoOutputMax = (setupNr - 110) * 140;
            if (impAutoTypeIndex == 1) impAutoOutputMax = (setupNr - 110) * 140;

            if (controlForm.runView.voltageComboBox.SetSelected == "Imp")
            {
                if (controlForm.setupView.impulseCheckBox.isChecked)
                {
                    controlForm.runView.testVoltageTextBox.Max = impOutputMax;
                    controlForm.runView.impulseStepSizeTextBox.Max = impOutputMax;
                    controlForm.runView.impulseStartVoltageTextBox.Max = impOutputMax - 5;
                } 
            }

            controlForm.runView.elapsedTimeTitleLabel.Text = "REMAINING";
            controlForm.runView.elapsedTimeTitleLabel.Invalidate();
            controlForm.runView.secondsUnitLabel.Text = "THIS LEVEL";
            controlForm.runView.secondsUnitLabel.Invalidate();
            controlForm.runView.resultTestVoltageLabel.Text = "NEXT TARGET";
            controlForm.runView.resultTestVoltageLabel.Invalidate();
        }

        // Before we start, make sure the UI correctly represents the PLC and Motor status. Called on startup and on entry
        private void InitializeDbView()
        {

            Thread.Sleep(200);

            // Top info strip notice
            controlForm.modeLabel.Text = "Manual Operation";

            // Check K1 and K2 to see if they've been left open
            if (PIO1.K1Closed)
            {
                // K1 has been left open. Reflect this in the UI
                controlForm.dashboardView.onOffButton.isChecked = true;
                controlForm.dashboardView.onOffButton.Invalidate();
            }
            if (PIO1.K2Closed)
            {
                // K1 has been left open. Reflect this in the UI
                controlForm.dashboardView.onOffSecButton.isChecked = true;
                controlForm.dashboardView.onOffButton.Invalidate();
            }

            // Set the voltage representations (rms, pk ....) for each voltage
            SetACOutputType(acTypeIndex);
            SetDCOutputType(dcTypeIndex);
            SetImpOutputType(impTypeIndex);

            // Get Regulated Voltage Control max based on selected setup and selected reference


            // Update the textbox max value
            UpdateDbRegulatedVoltageMax();

        }

        private void UpdateDbRegulatedVoltageMax()
        {
            if (controlForm.dashboardView.acInputRadioButton.isChecked)
            {
                controlForm.dashboardView.regulatedVoltageTextBox.Max = acInputMax;
            }
            else if (controlForm.dashboardView.acOutputRadioButton.isChecked)
            {
                SetMaxAC();

            }
            else if (controlForm.dashboardView.dcOutputRadioButton.isChecked)
            {
                SetMaxDC();
                controlForm.dashboardView.regulatedVoltageTextBox.Max = dcOutputMax;
            }
        }

        public void SetMaxAC()
        {
            // Limit values
            if (acTypeIndex == 0) acOutputMax = (setupNr / 100) * 100;
            if (acTypeIndex == 1) acOutputMax = (setupNr / 100) * 140;

            if (controlForm.dashboardView.acOutputRadioButton.isChecked)
            {
                controlForm.dashboardView.regulatedVoltageTextBox.Max = acOutputMax;
                controlForm.dashboardView.regulatedVoltageTextBox.Invalidate();
            }
        }

        private void SetMaxDC()
        {
            // Limit values
            if (dcTypeIndex == 0) dcOutputMax = ((setupNr - 100) / 10) * 140;
            if (dcTypeIndex == 1) dcOutputMax = ((setupNr - 100) / 10) * 140;

            if (controlForm.dashboardView.dcOutputRadioButton.isChecked)
            {
                controlForm.dashboardView.regulatedVoltageTextBox.Max = dcOutputMax;
                controlForm.dashboardView.regulatedVoltageTextBox.Invalidate();
            }
        }

        private void UpdateResultLabels()
        {

            if ((autoTest.testIsWithstand) && (controlForm.runView.voltageComboBox.SetSelected != "Imp"))
            {
                controlForm.runView.resultTestVoltageLabel.Text = "TEST VOLTAGE";
                controlForm.runView.elapsedTimeTitleLabel.Text = "ELAPSED TIME";
                controlForm.runView.hvUnitLabel.Text = "kV" + selectedVoltage + selectedMeasType;
                controlForm.runView.secondsUnitLabel.Text = "SECONDS";

            }
            else if ((!autoTest.testIsWithstand) && (controlForm.runView.voltageComboBox.SetSelected != "Imp"))
            {
                controlForm.runView.resultTestVoltageLabel.Text = "INCEPTION";
                controlForm.runView.elapsedTimeTitleLabel.Text = "SAMPLE TIME";
                controlForm.runView.hvUnitLabel.Text = "kV" + selectedVoltage + selectedMeasType;
                controlForm.runView.secondsUnitLabel.Text = "SECONDS";
            }
            else if (controlForm.runView.voltageComboBox.SetSelected == "Imp")
            {
                controlForm.runView.resultTestVoltageLabel.Text = "NEXT TARGET";
                controlForm.runView.elapsedTimeTitleLabel.Text = "REMAINING";
                controlForm.runView.hvUnitLabel.Text = "kVDC"; //+ selectedVoltage + selectedMeasType;
                controlForm.runView.secondsUnitLabel.Text = "THIS LEVEL";
            }

            controlForm.runView.resultTestVoltageLabel.Invalidate();
            controlForm.runView.elapsedTimeTitleLabel.Invalidate();
            controlForm.runView.elapsedTimeTitleLabel.Invalidate();
            controlForm.runView.secondsUnitLabel.Invalidate();
        }

        // Present all values in the UI
        public void UpdateGUI()
        {
            // Voltage and Current
            if ((PIO1.minUPos) && (!PIO1.K2Closed) && (PIO1.regulatedVoltageValue > 8))
            {
                // With K2 open (no load) when parked, there can be some scrap voltage in the transformer. Don't show it. 
                controlForm.dashboardView.voltageInputLabel.Text = "0.0";
                controlForm.dashboardView.currentInputLabel.Text = "0.00";
            }
            else
            {
                // When loading or not parked, the value is correct. Present it.
                controlForm.dashboardView.voltageInputLabel.Text = PIO1.regulatedVoltageValue.ToString("0.0").Replace(',', '.'); ;
                controlForm.dashboardView.currentInputLabel.Text = PIO1.regulatedCurrentValue.ToString("0.00").Replace(',', '.'); ;
            }

            // Pressure
            controlForm.dashboardView.pressureLabel.Text = PIO1.getPressure();

            // Status flags
            //guiUpdater.transferLabel36(PIO1.fault.ToString());
            controlForm.dashboardView.statusIndicatorUmin.IsTrue = PIO1.minUPos;           
            controlForm.dashboardView.statusIndicatorEarthingengaged.IsTrue = PIO1.earthingEngaged;          
            controlForm.dashboardView.statusIndicatorDischargeRodParked.IsTrue = PIO1.dischargeRodParked;           
            controlForm.dashboardView.statusIndicatorEmStopKeySwClosed.IsTrue = PIO1.emergStpKeySwClosed;
            controlForm.dashboardView.statusIndicatorDoorClosed.IsTrue = PIO1.dorrSwitchClosed;
            controlForm.dashboardView.statusIndicatorK1F2Closed.IsTrue = PIO1.K1Closed;
            controlForm.dashboardView.statusIndicatorK2F1Closed.IsTrue = PIO1.K2Closed;

            // Auto voltage regulation status
            controlForm.dashboardView.statusLabelAutoRegVoltage.Visible = !abortRegulation;

            // Warning High Voltage image
            if ((PIO1.regulatedVoltageValue >= 1) && (PIO1.K2Closed))
            {
                controlForm.dashboardView.statusPictureBoxHVPresent.Visible = true;
                controlForm.dashboardView.dischargePictureBox.Visible = false;  
            }
            else
            {
                controlForm.dashboardView.statusPictureBoxHVPresent.Visible = false;
                controlForm.dashboardView.dischargePictureBox.Visible = true; 
            }

            // Active motor info
            controlForm.dashboardView.impulseGapLabel.Text = activeMotor.actualPosition.ToString();
            controlForm.dashboardView.statusLabelActiveMotorInitialized.Text = GetMotorStatus();
            

            // Active setup presentation
            GetActiveSetup();

        }

        // Evaluate the user input and present the current setup image
        private void GetActiveSetup()
        {
            setupNr = 0;

            if (controlForm.setupView.acCheckBox.isChecked)
            {
                if (controlForm.setupView.acStage1RadioButton.isChecked)
                {
                    setupNr += 100;

                }
                else if (controlForm.setupView.acStage2RadioButton.isChecked)
                {
                    setupNr += 200;
                }
                else if (controlForm.setupView.acStage3RadioButton.isChecked)
                {
                    setupNr += 300;
                }
            }
            if (controlForm.setupView.dcCheckBox.isChecked)
            {
                if (controlForm.setupView.dcStage1RadioButton.isChecked)
                {
                    setupNr += 10;

                }
                else if (controlForm.setupView.dcStage2RadioButton.isChecked)
                {
                    setupNr += 20;
                }
                else if (controlForm.setupView.dcStage3RadioButton.isChecked)
                {
                    setupNr += 30;
                }
            }
            if (controlForm.setupView.impulseCheckBox.isChecked)
            {
                if (controlForm.setupView.impulseStage1RadioButton.isChecked)
                {
                    setupNr += 1;

                }
                else if (controlForm.setupView.impulseStage2RadioButton.isChecked)
                {
                    setupNr += 2;
                }
                else if (controlForm.setupView.impulseStage3RadioButton.isChecked)
                {
                    setupNr += 3;
                }
            }

            if(setupNr == 100)
            {
                // Setup image
                controlForm.dashboardView.activeSetupPictureBox.Image = Properties.Resources._1_stageAC;
                controlForm.dashboardView.activeSetupPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                controlForm.dashboardView.activeSetupPictureBox.Refresh(); 
                
                //Setup composer model
                if(setupNr != setupNrLast)
                {
                    setupNrLast = setupNr;
                    player.modelLoaded = false;
                    player.sumLoad = 0;
                    int i = player.sumLoad;
                    player.changeModel("AC-1-Stage.smg");
                    this.controlForm.setupView.ComposerPlayerPanel.Visible = false;
                    composerTimer.Start();
                }

            }
            else if (setupNr == 200)
            {
                controlForm.dashboardView.activeSetupPictureBox.Image = Properties.Resources._2_stageAC;
                controlForm.dashboardView.activeSetupPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                controlForm.dashboardView.activeSetupPictureBox.Refresh();
                //Setup composer model
                if (setupNr != setupNrLast)
                {
                    setupNrLast = setupNr;
                    player.modelLoaded = false;
                    player.sumLoad = 0;
                    int i = player.sumLoad;
                    player.changeModel("AC-2-Stage.smg");
                    this.controlForm.setupView.ComposerPlayerPanel.Visible = false;
                    composerTimer.Start();
                }
            }
            else if (setupNr == 300)
            {
                controlForm.dashboardView.activeSetupPictureBox.Image = Properties.Resources._3_stageAC;
                controlForm.dashboardView.activeSetupPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                controlForm.dashboardView.activeSetupPictureBox.Refresh();
                //Setup composer model
                if (setupNr != setupNrLast)
                {
                    setupNrLast = setupNr;
                    player.modelLoaded = false;
                    player.sumLoad = 0;
                    int i = player.sumLoad;
                    player.changeModel("AC-3-Stage.smg");
                    this.controlForm.setupView.ComposerPlayerPanel.Visible = false;
                    composerTimer.Start();
                }
            }
            else if (setupNr == 110)
            {
                controlForm.dashboardView.activeSetupPictureBox.Image = Properties.Resources._1_stageDC;
                controlForm.dashboardView.activeSetupPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                controlForm.dashboardView.activeSetupPictureBox.Refresh();
                //Setup composer model
                if (setupNr != setupNrLast)
                {
                    setupNrLast = setupNr;
                    player.modelLoaded = false;
                    player.sumLoad = 0;
                    int i = player.sumLoad;
                    player.changeModel("DC-1-Stage.smg");
                    this.controlForm.setupView.ComposerPlayerPanel.Visible = false;
                    composerTimer.Start();
                }
            }
            else if (setupNr == 120)
            {
                controlForm.dashboardView.activeSetupPictureBox.Image = Properties.Resources._2_stageDC;
                controlForm.dashboardView.activeSetupPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                controlForm.dashboardView.activeSetupPictureBox.Refresh();
                //Setup composer model
                if (setupNr != setupNrLast)
                {
                    setupNrLast = setupNr;
                    player.modelLoaded = false;
                    player.sumLoad = 0;
                    int i = player.sumLoad;
                    player.changeModel("DC-2-Stage.smg");
                    this.controlForm.setupView.ComposerPlayerPanel.Visible = false;
                    composerTimer.Start();
                }
            }
            else if (setupNr == 130)
            {
                controlForm.dashboardView.activeSetupPictureBox.Image = Properties.Resources._3_stageDC;
                controlForm.dashboardView.activeSetupPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                controlForm.dashboardView.activeSetupPictureBox.Refresh();
                //Setup composer model
                if (setupNr != setupNrLast)
                {
                    setupNrLast = setupNr;
                    player.modelLoaded = false;
                    player.sumLoad = 0;
                    int i = player.sumLoad;
                    player.changeModel("DC-3-Stage.smg");
                    this.controlForm.setupView.ComposerPlayerPanel.Visible = false;
                    composerTimer.Start();
                }
            }
            else if (setupNr == 111)
            {
                controlForm.dashboardView.activeSetupPictureBox.Image = Properties.Resources._1_stageImp;
                controlForm.dashboardView.activeSetupPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                controlForm.dashboardView.activeSetupPictureBox.Refresh();
                //Setup composer model
                if (setupNr != setupNrLast)
                {
                    setupNrLast = setupNr;
                    player.modelLoaded = false;
                    player.sumLoad = 0;
                    int i = player.sumLoad;
                    player.changeModel("Impulse-1-Stage.smg");
                    this.controlForm.setupView.ComposerPlayerPanel.Visible = false;
                    composerTimer.Start();
                }
            }
            else if (setupNr == 112)
            {
                controlForm.dashboardView.activeSetupPictureBox.Image = Properties.Resources._2_stageImp;
                controlForm.dashboardView.activeSetupPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                controlForm.dashboardView.activeSetupPictureBox.Refresh();
                //Setup composer model
                if (setupNr != setupNrLast)
                {
                    setupNrLast = setupNr;
                    player.modelLoaded = false;
                    player.sumLoad = 0;
                    int i = player.sumLoad;
                    player.changeModel("Impulse-2-Stage.smg");
                    this.controlForm.setupView.ComposerPlayerPanel.Visible = false;
                    composerTimer.Start();
                }
            }
            else if (setupNr == 113)
            {
                controlForm.dashboardView.activeSetupPictureBox.Image = Properties.Resources._3_stageImp;
                controlForm.dashboardView.activeSetupPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                controlForm.dashboardView.activeSetupPictureBox.Refresh();
                //Setup composer model
                if (setupNr != setupNrLast)
                {
                    setupNrLast = setupNr;
                    player.modelLoaded = false;
                    player.sumLoad = 0;
                    int i = player.sumLoad;
                    player.changeModel("Impulse-3-Stage.smg");
                    this.controlForm.setupView.ComposerPlayerPanel.Visible = false;
                    composerTimer.Start();
                }
            }
            else
            {
                controlForm.dashboardView.activeSetupPictureBox.Image = Properties.Resources.tercoLogo;
                controlForm.dashboardView.activeSetupPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                controlForm.dashboardView.activeSetupPictureBox.Refresh();
            }

        }

        // periodically check to see that nothing weird or dangerous is happening
        private void CheckLimits()
        {
        //    // Make sure the voltage output is within limits
        //    if (PIO1.regulatedVoltageValue >= 235)
        //    {
        //        // Stop the transformer
        //        if(PIO1.regulatedVoltageValue >= maxVoltage)
        //        {
        //            PIO1.StopTransformerMotor();
        //        }


            //        // Notify max voltage has been reached..
            //    }

        }

        // Return a status text string for the active motor
        private string GetMotorStatus()
        {
            if (motorState == "READY")
            {
                
                // How to get out of here?
                if ((activeMotor.initComplete) && (searchingGap))
                {
                    motorState = "SEARCHING";
                }
                else if(!activeMotor.initComplete)
                {
                    motorState = "NOT INITIALIZED";
                }
               
                return "       READY     ";
            }

            else if (motorState == "SEARCHING")
            {
                if ((activeMotor.actualPosition == controlForm.dashboardView.impulseGapTextBox.Value)|| (upDownRequest))
                {
                    searchingGap = false;
                    upDownRequest = false;
                    motorState = "READY";
                }

                return "   SEARCHING...";
            }

            else if (motorState == "INITIALIZING")
            {
                if (activeMotor.initComplete)
                {
                    motorState = "READY";
                    initializeMotor = false;
                }

                return "  INITIALIZING   ";
               
            }

            else if (motorState == "NOT INITIALIZED")
            {
                if (initializeMotor)
                {
                    motorState = "INITIALIZING";
                }
                else if (activeMotor.initComplete)
                {
                    motorState = "READY";
                }

                return "NOT INITIALIZED";
            }
            
            else
            {
                return "       ERROR      ";
            }
          
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

            // If overridden or voltage parked
            if (((PIO1.minUPos) || (PIO1.overrideUMin)) && (PIO1.K1Closed))
            {
                PIO1.closeSecondary();
            }
            else
            {
                // Add additional notification here
                this.controlForm.dashboardView.onOffSecButton.isChecked = false;
                this.controlForm.dashboardView.onOffSecButton.Invalidate();
            }

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
            upDownRequest = true;
        }

        public void IncreaseGapRequest()
        {
            //activeMotor.IncreaseGap();
            commandPending = 2;
            upDownRequest = true;
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
            initializeMotor = true;

        }

        public void RunToPosRequest(string posIn)
        {
            if (activeMotor.initComplete != true)
            {
                return;
            }

            commandPending = 5;
            searchingGap = true;
        }

        //
        public void IncreasePressureRequest()
        {
            PIO1.startCompressor = true; // should be done in devices own increase pressure routine

        }

        //
        public void DecreasePressureRequest()
        {
            PIO1.startVacuumPump = true; // should be done in devices own decrease pressure routine

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

        // Manually control the 230V outputs
        private void EnergizeCompressorOutputRequest()
        {

            if (controlForm.dashboardView.compressorPowerCheckBox.isChecked)
            {
                PIO1.startCompressor = true;
            }
            else
            {
                PIO1.startCompressor = false;
            }
               
        }

        // Manually control the 230V outputs
        private void EnergizeVacuumPumpOutputRequest()
        {
            if (controlForm.dashboardView.vacuumPowerCheckBox.isChecked)
            {
                PIO1.startVacuumPump = true;
            }
            else
            {
                PIO1.startVacuumPump = false;
            }
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
        
        // A test type change has been fired by change or entering form
        public void GetTestType()
        {
            selectedVoltage = controlForm.runView.voltageComboBox.SetSelected;
 

            if (selectedVoltage == "AC")
            {
                selectedMeasType = "(" + controlForm.runView.acOutputComboBox.SetSelected + ")";
            }
            else if (selectedVoltage == "DC")
            {
                selectedMeasType = "(" + controlForm.runView.dcOutputComboBox.SetSelected + ")";
                
            }
            else
            {
                selectedMeasType = "";
            }

            
            if (controlForm.runView.WithstandRadioButton.isChecked)
            {
                this.controlForm.modeLabel.Text = selectedVoltage + selectedMeasType + " Withstand Voltage Test";
            }
            else if (controlForm.runView.DisruptiveRadioButton.isChecked)
            {
                this.controlForm.modeLabel.Text = selectedVoltage + selectedMeasType  + " Disruptive Discharge Voltage Test";
            }
            else
            {
                this.controlForm.modeLabel.Text = selectedVoltage + " No Test Selected";
            }

            
        }

        // Set the voltage type to call when asking for a value
        private void SetACOutputType(int indexIn)
        {
            //representation index: 0 = Vmax, 1 = Vmin, 2 = Vrms, 3 = vpk-vpk, 4 = Vavg
            int[] selection = { 2, 0, 1, 3 };
            acChannel.setRepresentationIndex(selection[indexIn]);
        }

        // Set the voltage type to call when asking for a value
        private void SetDCOutputType(int indexIn)
        {
            //representation index: 0 = Vmax, 1 = Vmin, 2 = Vrms, 3 = vpk-vpk, 4 = Vavg
            int[] selection = { 4, 0, 1, 3 };
            dcChannel.setRepresentationIndex(selection[indexIn]);  
        }

        // Set the voltage type to call when asking for a value
        private void SetImpOutputType(int indexIn)
        {
            //representation index: 0 = Vmax, 1 = Vmin, 2 = Vrms, 3 = vpk-vpk, 4 = Vavg
            int[] selection = { 0, 1 };
            int[] polarity = { 1, -1 };
            impulseChannel.setRepresentationIndex(selection[indexIn]);
            impulseChannel.Polarity = polarity[indexIn];

            // Limit values
           impOutputMax = (setupNr-110) * 140 * polarity[indexIn];
        }

    }
}
