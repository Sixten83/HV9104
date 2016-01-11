using System;
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
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace HV9104_GUI
{

     class Controller
    {
        MeasuringForm measuringForm;
        ControlForm controlForm;
        PicoScope picoScope;
        Channel acChannel, dcChannel, impulseChannel;
        System.Windows.Forms.Timer loopTimer, triggerTimer;
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

            // Initialize all devices and test communication
            InitializeDevices();

            // Instantiate the class for autorun procedures
            autoTest = new AutoTest(controlForm.runView, controlForm.dashboardView, PIO1, HV9126, HV9133, acChannel, dcChannel, impulseChannel);

            // If all devices are initialized have communication, start own loop for PLC, stepper motors and aux equipment.
            t = new Thread(CyclicRead);
            t.Start();
            
            // Start timed loop for Picoscope routines
            loopTimer.Start();

            // Get and present initial status info from PLC and motors
            Thread.Sleep(200);
            InitializeDbView();

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
                this.measuringForm.chart.Series.SuspendUpdates();

                if ((int)(picoScope._overflow & 1) == 0 || !acChannel.VoltageAutoRange)
                {
                    if (this.measuringForm.acEnableCheckBox.isChecked)
                    {
                        // Console
                        this.measuringForm.chart.Series["acSeries"].Points.Clear();
                        Channel.ScaledData data = acChannel.processData(1600, trigAt, 400);

                        this.measuringForm.chart.Series["acSeries"].Points.DataBindXY(data.x, data.y);
                        this.controlForm.dashboardView.acValueLabel.Text = "" + acChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                    }
                    else
                    {
                        acChannel.processMaxMinData(1600, trigAt);
                        this.controlForm.dashboardView.acValueLabel.Text = "" + acChannel.getRepresentation().ToString("0.0").Replace(',', '.');
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
                        this.measuringForm.chart.Series["dcSeries"].Points.Clear();
                        Channel.ScaledData data = dcChannel.processData(1600, trigAt, 400);
                        this.measuringForm.chart.Series["dcSeries"].Points.DataBindXY(data.x, data.y);
                        this.controlForm.dashboardView.dcValueLabel.Text = "" + dcChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                    }
                    else
                    {
                        dcChannel.processMaxMinData(1600, trigAt);
                        this.controlForm.dashboardView.dcValueLabel.Text = "" + dcChannel.getRepresentation().ToString("0.0").Replace(',', '.');
                    }
                }
                else
                    autoSetVoltageRange(dcChannel);

                if ((int)(picoScope._overflow & 2) == 0 && dcChannel.getRawMaxRatio() < 0.3 && dcChannel.VoltageAutoRange)
                {
                    autoSetVoltageRange(dcChannel);
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
            if(picoScope._overflow != 0)
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

            this.controlForm.runExperimentTab.Click += new System.EventHandler(runExperimentTab_Click);
            this.controlForm.dashboardTab.Click += new System.EventHandler(dashboardTab_Click);
            this.controlForm.setupTab.Click += new System.EventHandler(setupTab_Click);

            //***********************************************************************************************************
            //***                                     SETUP VIEW EVENT LISTENERS                                     ****
            //***********************************************************************************************************

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
            this.controlForm.runView.voltageLevelsTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(voltageLevelsTextBox_valueChange);
            this.controlForm.runView.impPerLevelTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(impPerLevelTextBox_valueChange);
            this.controlForm.runView.abortAutoTestButton.Click += new System.EventHandler(abortAutoTestButton_Click);
            this.controlForm.runView.voltageComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(autoTestVoltageComboBox_valueChange);
            this.controlForm.runView.acOutputComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(acOutputAutoComboBox_valueChange);
            this.controlForm.runView.dcOutputComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(dcOutputAutoComboBox_valueChange);
            this.controlForm.runView.impulseOutputComboBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(impulseOutputAutoComboBox_valueChange);
            this.controlForm.runView.createReportButton.Click += new System.EventHandler(createReportButton_Click);
           
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
            this.measuringForm.impulseEnableCheckBox.Click += new System.EventHandler(impulseEnableCheckBox_Click);
            this.measuringForm.impulsePreTriggerTextBox.valueChangeHandler += new EventHandler<ValueChangeEventArgs>(impulsePreTriggerTextBox_valueChange);
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
        //***************************
        // Get relevant values and create a report 
        private void createReportButton_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }


        private void GenerateReport()
        {
            report.GenerateChartImage(controlForm.runView.autoTestChart); //chart --> .jpg
            report.GenerateTex(controlForm.modeLabel.Text, controlForm.runView.dateTextBox.Text, controlForm.runView.operatorTextBox.Text, controlForm.runView.testObjectTextBox.Text, controlForm.runView.otherTextBox.Text, controlForm.runView.testDurationLabel.Text, controlForm.runView.testVoltageTextBox.Value.ToString(), controlForm.runView.passFailLabel.Text); //Generate .Tex file
            report.GeneratePdf(); //Generate final .pdf file.
        }




        // Voltage measurement type has been changed in auto test page
        private void autoTestMeasTypeComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            // Set the test norification text 
            GetTestType();

        }

        SaveFileDialog sfd;

        public void GenerateChartImage(Chart chartIn)
        {
            Chart chart1 = new Chart();
            chart1.SaveImage("test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg); //filnamn på charten. Just nu skrivs den över för varje "export"
        }


        public void GeneratePdf()
        {
            /*
            saveFileDialog1.ShowDialog();
            string fn = saveFileDialog1.FileName;
            saveFileDialog1.Filter = "*.tex";
            saveFileDialog1.DefaultExt = "tex";
            */


            string filename = sfd.FileName;//@"C:\Users\Terco\Desktop\text.tex"; 
            //filename = filename.Replace(@"\", "/");
            Process p1 = new Process();
            p1.StartInfo.FileName = @"C:\Program Files (x86)\MiKTeX 2.9\miktex\bin\xelatex.exe"; //xelatex är typsättaren
            p1.StartInfo.Arguments = filename;
            p1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p1.StartInfo.RedirectStandardOutput = true;
            p1.StartInfo.UseShellExecute = false;

            p1.Start();
            // try
            //{
            var output = p1.StandardOutput.ReadToEnd();
            //}
            //catch (IOException e)
            //{
            //    string ex = e.ToString();
            //}

            p1.WaitForExit();

        }

        public void GenerateTex(string testType, string date, string operatorName, string testObject, string otherInfo, string duration, string testVoltage, string statusPassFail)
        {
            sfd = new SaveFileDialog();
            sfd.Filter = "(*.tex)|*.tex";
            sfd.ShowDialog();

            string path = sfd.FileName; //@"C:\Users\Terco\Desktop\WindowsFormsApplication2\WindowsFormsApplication2\bin\Debug\text.tex";
            //if (!File.Exists(path))
            // {
            using (StreamWriter sw = File.CreateText(path))
            {
                //#####################################
                //Packages and definitions
                //#####################################
                sw.WriteLine("\\documentclass{article}");
                sw.WriteLine("\\usepackage{graphicx}");
                sw.WriteLine("\\usepackage{a4wide}");
                sw.WriteLine("\\usepackage{amsmath}");
                sw.WriteLine("\\usepackage{fancyhdr}");
                sw.WriteLine("\\pagestyle{fancy}");
                sw.WriteLine("\\usepackage{xcolor}");
                sw.WriteLine("\\usepackage{color}");
                sw.WriteLine("\\usepackage[bottom=0.50cm, top=2.5cm]{geometry}");
                sw.WriteLine("\\setlength\\parindent{0pt}");
                sw.WriteLine("\\definecolor{textcolor}{RGB}{127,127,127}");
                sw.WriteLine("\\renewcommand{\\labelenumi}{\\alph{enumi}.} ");
                sw.WriteLine("\\usepackage{fontspec}");
                sw.WriteLine("\\setmainfont{Calibri}");
                sw.WriteLine("\\newcommand{\\sizeone}{\\fontsize{48pt}{20pt}\\selectfont} %48pt");
                sw.WriteLine("\\newcommand{\\sizetwo}{\\fontsize{16pt}{20pt}\\selectfont} % 16pt");
                sw.WriteLine("\\newcommand{\\sizethree}{\\fontsize{12pt}{20pt}\\selectfont} % 12pt");
                sw.WriteLine("\\title{\\vspace{-2.0cm}EXPERIMENT REPORT \\\\ " + testType + " } "); // EXPERIMENT REPORT är statisk, DC WITHSTAND TEST = dynamisk, hämta variabel.
                sw.WriteLine("\\date{} ");
                sw.WriteLine("\\geometry{headheight = 0.6in}");
                sw.WriteLine("\\fancypagestyle{firstpage}{\\fancyhf{}\\fancyhead[R]{\\includegraphics[height=0.5in, keepaspectratio=true]{Splashlogo}}}");
                sw.WriteLine("\\fancypagestyle{plain}{\\fancyhf{}\\fancyhead[L]{\\includegraphics[height=0.5in, keepaspectratio=true]{Splashlogo}}}");

                //#####################################
                //Begin document {preamble}
                //#####################################
                sw.WriteLine("\\begin{document}");
                sw.WriteLine("\\color{textcolor}");
                sw.WriteLine("\\maketitle");
                sw.WriteLine("{\\large");
                //sw.WriteLine("\\begin{center}");
                sw.WriteLine("\\begin{tabular}{l p{10cm}}");
                sw.WriteLine("DATE PERFORMED: & {0} \\\\ ", date);
                sw.WriteLine("OPERATOR: & {0} \\\\ ", operatorName);
                sw.WriteLine("TEST OBJECT: & {0} \\\\", testObject);
                sw.WriteLine("OTHER INFO: & {0}", otherInfo);
                sw.WriteLine("\\end{tabular}");
                //sw.WriteLine("\\end{center}}");
                sw.WriteLine("}");

                //#####################################
                //SECTION 1
                //#####################################

                //sw.WriteLine("\\begin{center}");
                sw.WriteLine("\\section*{EXPERIMENT RESULTS}");
                //sw.WriteLine("\\end{center}");
                sw.WriteLine("\\begin{figure}[h]");
                sw.WriteLine("\\begin{center}");
                sw.WriteLine("\\includegraphics[width=0.5\\textwidth]{test.jpg}");
                sw.WriteLine("\\caption{Measured data}");
                sw.WriteLine("\\end{center}");
                sw.WriteLine("\\end{figure}");
                sw.WriteLine("\\begin{center}");
                sw.WriteLine("\\begin{tabular}{c c c c c}");
                sw.WriteLine("{\\sizetwo \\textbf{ DURATION} }& & \\sizetwo \\textbf{TEST VOLTAGE} & & \\sizetwo \\textbf{PASS/FAIL STATUS} \\\\");
                sw.WriteLine("& & \\\\");
                //sw.WriteLine(" \\sizeone \\textbf{{0}} &  &",label10.Text);
                sw.WriteLine("\\sizeone \\textbf{");
                sw.WriteLine("{0}", duration);
                sw.WriteLine("} & &");

                // sw.WriteLine("{\\sizeone \\textbf{ {0}}} & & ");
                sw.WriteLine("\\sizeone \\textbf{");
                sw.WriteLine("{0}", testVoltage);
                sw.WriteLine("} & &");

                //sw.WriteLine("{\\sizeone  \\textbf{ {0}}} \\\\");
                sw.WriteLine("\\sizeone \\textbf{");
                sw.WriteLine("{0}", statusPassFail);
                sw.WriteLine("} \\\\");

                sw.WriteLine("{\\sizethree Seconds }& &{\\sizethree kV} &&  \\\\");
                sw.WriteLine("& &\\footnotesize{ $ \\pm 15 \\%$}& & \\footnotesize{e 46 seconds} \\\\");
                sw.WriteLine("\\end{tabular}");
                sw.WriteLine("\\end{center}");
                sw.WriteLine("\\end{document}");



                // }

            }

        }

        //***********************************************************************************************************
        //***                                  RUNVIEW EVENT HANDLERS                                          *****
        //***********************************************************************************************************

        // Experiment Start/Pause
        private void onOffAutoButton_Click(object sender, EventArgs e)
        {
            if (controlForm.runView.onOffAutoButton.isChecked)
            {

                bool ready = autoTest.StartTest();
                if (ready)
                {
                    autoTest.GoToVoltageAuto(autoTest.testVoltage);
                }
            }
            else
            {
                autoTest.PauseTest();
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
            this.measuringForm.chart.cursorMenu.setScaleFactor(impulseChannel.getScaleFactor(), impulseChannel.DCOffset * impulseChannel.DividerRatio);
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
            this.measuringForm.timeBaseComboBox.SetSelected = "500 ns/Div";
           
        }   
    
       
        private void acVoltageRangeComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            pauseStream();
            if (e.Value == 0)
                acChannel.VoltageAutoRange = true;
            else
                acChannel.VoltageAutoRange = false;
            picoScope.setChannelVoltageRange(0, (Imports.Range)e.Value + 3);
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
            picoScope.setDCoffset(2, (float)(-1 * impulseChannel.Polarity * impulseChannel.rangeToVolt() * 0.8f));
            rebootStream();
            this.measuringForm.chart.cursorMenu.setScaleFactor(impulseChannel.getScaleFactor(), impulseChannel.DCOffset * impulseChannel.DividerRatio);
            this.measuringForm.chart.updateCursorMenu();
            
        }

        private void impulseEnableCheckBox_Click(object sender, EventArgs e)
        {
            
        }

         private void impulsePreTriggerTextBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            picoScope.BlockPreTrigger = (double)(e.Value / 100); 
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
                else if (e.Text.Equals("20 us/Div"))
                {
                    picoScope.BlockSamples = 100000;
                    picoScope.TimePerDivision = 20;
                    impulseChannel.IncrementIndex = 9;
                    this.measuringForm.chart.setTimePerDiv(20);
                }
            }
            this.measuringForm.chart.updateCursorMenu();
        }
         
        private void triggerSetupButton_Click(object sender, EventArgs e)
        {


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
            if((controlForm.dashboardView.trafSpeedTextBox.Value <= controlForm.dashboardView.trafSpeedTextBox.Max) && (controlForm.dashboardView.trafSpeedTextBox.Value >= controlForm.dashboardView.trafSpeedTextBox.Min))
            { 
                trafSpeed = (int)controlForm.dashboardView.trafSpeedTextBox.Value * 10;
            }
        }

        // Voltage ON/OFF Switch
        private void onOffButton_Click(object sender, EventArgs e)
        {
            // Connect K1
            if(this.controlForm.dashboardView.onOffButton.isChecked)
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
                
                // Now disconnect K1
                OpenPrimaryRequest();

                // If Park is selected
                if (this.controlForm.dashboardView.parkCheckBox.isChecked)
                {
                    // Drive the voltage down to zero
                    PIO1.ParkTransformer();
                }

                // Write a reminder to use discharge rod
                this.controlForm.messageLabel.Text = "Important! Always use the Discharge Rod to discharge components when entering the HV area.";
            }
        }

        // Voltage ON/OFF Switch
        private void onOffSecButton_Click(object sender, EventArgs e)
        {
            // Connect K2
            if (this.controlForm.dashboardView.onOffSecButton.isChecked)
            {
                CloseSecondaryRequest(this.controlForm.dashboardView.overrideCheckBox.isChecked);
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

                error =  uActual - targetVoltage;

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

            // *Not needed if we regulate against the already converted UI presentation value 

        }
 
        // Create a signal to trigger an impulse voltage
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
            picoScope.setDCoffset(2, -1 * (float)(impulseChannel.Polarity * impulseChannel.rangeToVolt()) * 0.8f);              
            //Set databuffer
            picoScope.setBlockDataBuffer();            
            //Set trigger Channel/Level/Type
            picoScope.setTriggerChannel(Imports.Channel.ChannelC);


            //Setup Trigger / Chopping time - IF ENABLED!!! TO BE CHECKED!!!!
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

        }

        private void choppingCheckBox_Click(object sender, EventArgs e)
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
            searchingGap = true;

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
        //***                                  RUNVIEW EVENT HANDLERS                                          *****
        //***********************************************************************************************************   
        
        // Voltage type has been changed in auto test page
        private void autoTestVoltageComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            // Set the test notification text
            UpdateRnTestVoltageMax();
            GetTestType();
        }

        private void impulseOutputAutoComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            impAutoTypeIndex = (int)e.Value;
            SetImpOutputType(impAutoTypeIndex);
            UpdateRnTestVoltageMax();
            GetTestType();
        }

        private void dcOutputAutoComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            dcAutoTypeIndex = (int)e.Value;
            SetDCOutputType(dcAutoTypeIndex);
            UpdateRnTestVoltageMax();
            GetTestType();
        }

        private void acOutputAutoComboBox_valueChange(object sender, ValueChangeEventArgs e)
        {
            acAutoTypeIndex = (int)e.Value;
            SetACOutputType(acAutoTypeIndex);
            UpdateRnTestVoltageMax();
            GetTestType();
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

        private void testWithstandRadioButton_Click(object sender, EventArgs e)
        {
            disableForControls();
            UpdateRnTestVoltageMax();
            GetTestType();
        }

        private void testDisruptiveRadioButton_Click(object sender, EventArgs e)
        {
            disableForControls();
            UpdateRnTestVoltageMax();
            GetTestType();

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

            report = new ReportGen();

            //if (!activeMotor.initComplete)
            //{
            //    InitMotorRequest();
            //}


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
                    controlForm.runView.voltageLevelsTextBox.Value = 20;
                    controlForm.runView.impPerLevelTextBox.Value = 1;
                }
                else
                {
                    controlForm.runView.testVoltageTextBox.Value = 67;
                    controlForm.runView.voltageLevelsTextBox.Value = 1;
                    controlForm.runView.impPerLevelTextBox.Value = 5;
                }
                controlForm.runView.testVoltageTextBox.Invalidate();
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
                controlForm.runView.testVoltageTextBox.Max = impOutputMax;   
            }
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
            controlForm.dashboardView.statusLabelUmin.Text = PIO1.minUPos.ToString();
            controlForm.dashboardView.statusLabelUmin.Invalidate();
            controlForm.dashboardView.statusLabelEarthingengaged.Text = PIO1.earthingEngaged.ToString();
            controlForm.dashboardView.statusLabelEarthingengaged.Invalidate();
            controlForm.dashboardView.statusLabelDischargeRodParked.Text = PIO1.dischargeRodParked.ToString();
            controlForm.dashboardView.statusLabelDischargeRodParked.Invalidate();
            controlForm.dashboardView.statuslabelEmStopKeySwClosed.Text = PIO1.emergStpKeySwClosed.ToString();
            controlForm.dashboardView.statusLabelDoorClosed.Text = PIO1.dorrSwitchClosed.ToString();
            controlForm.dashboardView.statusLabelK1F2Closed.Text = PIO1.K1Closed.ToString();
            controlForm.dashboardView.statusLabelK2F1Closed.Text = PIO1.K2Closed.ToString();

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

            }
            else if (setupNr == 200)
            {
                controlForm.dashboardView.activeSetupPictureBox.Image = Properties.Resources._2_stageAC;
                controlForm.dashboardView.activeSetupPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                controlForm.dashboardView.activeSetupPictureBox.Refresh();

            }
            else if (setupNr == 300)
            {
                controlForm.dashboardView.activeSetupPictureBox.Image = Properties.Resources._3_stageAC;
                controlForm.dashboardView.activeSetupPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                controlForm.dashboardView.activeSetupPictureBox.Refresh();
            }
            else if (setupNr == 110)
            {
                controlForm.dashboardView.activeSetupPictureBox.Image = Properties.Resources._1_stageDC;
                controlForm.dashboardView.activeSetupPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                controlForm.dashboardView.activeSetupPictureBox.Refresh();
            }
            else if (setupNr == 120)
            {
                controlForm.dashboardView.activeSetupPictureBox.Image = Properties.Resources._2_stageDC;
                controlForm.dashboardView.activeSetupPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                controlForm.dashboardView.activeSetupPictureBox.Refresh();
            }
            else if (setupNr == 130)
            {
                controlForm.dashboardView.activeSetupPictureBox.Image = Properties.Resources._3_stageDC;
                controlForm.dashboardView.activeSetupPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                controlForm.dashboardView.activeSetupPictureBox.Refresh();
            }
            else if (setupNr == 111)
            {
                controlForm.dashboardView.activeSetupPictureBox.Image = Properties.Resources._1_stageImp;
                controlForm.dashboardView.activeSetupPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                controlForm.dashboardView.activeSetupPictureBox.Refresh();
            }
            else if (setupNr == 112)
            {
                controlForm.dashboardView.activeSetupPictureBox.Image = Properties.Resources._2_stageImp;
                controlForm.dashboardView.activeSetupPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                controlForm.dashboardView.activeSetupPictureBox.Refresh();
            }
            else if (setupNr == 113)
            {
                controlForm.dashboardView.activeSetupPictureBox.Image = Properties.Resources._3_stageImp;
                controlForm.dashboardView.activeSetupPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                controlForm.dashboardView.activeSetupPictureBox.Refresh();
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

            if (!searchingGap)
            {
                if (activeMotor.initComplete)
                {
                    // At rest and 
                    return "       READY     ";
                }
                else
                {
                           
                    return "NOT INITIALIZED";
                }
            }
            else
            { 
                int presentedValue = Convert.ToInt16(controlForm.dashboardView.impulseGapLabel.Text);

                if ((controlForm.dashboardView.impulseGapTextBox.Value != presentedValue) && (presentedValue != 90))
                {
                    return "   SEARCHING...";

                }
                else
                {
                    searchingGap = false;
                    return "       READY     ";
                }
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
            searchingGap = true;

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
            string selectedVoltage = controlForm.runView.voltageComboBox.SetSelected;
            string selectedMeasType;

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
