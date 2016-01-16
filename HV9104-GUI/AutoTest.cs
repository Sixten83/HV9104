using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using System.Windows.Forms.DataVisualization.Charting;
using Timer = System.Windows.Forms.Timer;


namespace HV9104_GUI
{
    public class AutoTest : INotifyPropertyChanged
    {
        

        // Source for the actual values + controls
        RunView runView;
        DashBoardView dashboardView;
        NA9739Device PIO1;
        PD1161Device HV9126;
        PD1161Device HV9133;
        Chart autoTestChart;
        MeasuringForm measuringForm;
        // Timer
        public Timer sampleTimer;
        public Timer impulseRoutineTimer;
        public Timer triggerTimeoutTimer;
        
        // 
        public DateTime startTime;
        public DateTime currentTime;
        private DateTime pauseTime;
        public TimeSpan elapsedTime;

        //Measurement Values
        Channel acChannel;
        Channel dcChannel;
        Channel impChannel;


        // Experiment variables
        public string testType;
        public string voltageType;
        public int voltageTypeIndex = 0;
        public string measurementType;
        public int duration = 60;
        public double targetVoltage;
        public double actualTestVoltage;
        public double toleranceHigh;
        public double toleranceLow;

        ArrayList list = new ArrayList();
        private Timer updateVoltageOutputValuesTimer;

        public bool testIsWithstand = true;
        public int impulseLevels;
        public int impulsePerLevel;
        public bool isPaused;
        public double sampleRate = 0.5;
        public int acMax = 100;
        public int dcMax = 120;
        public int impMax = 120;

        // Report variables
        public DateTime testDate;
        public string testOperatorName = "";
        public string testObjectName = "";
        public string testOtherInfo = "";

        // Run variables
        public bool pause = false;
        public bool quit = false;

        // Chart line value holders
        double[] xArray;
        double[] yArray;
        private bool isRunning;
        private double testVoltageToleranceHigh;
        private double testVoltageToleranceLow;
        private bool parking;
        private bool abortRegulation;
        private double tolerance;
        private int sampleNumber;
        public List<double> xList = new List<double>();
        public List<double> yList = new List<double>();
        public List<bool> breakdownList = new List<bool>();
        private double simElapsedTime;
        private Timer logoGrowEffectTimer;
        private Timer logoShrinkEffectTimer;

        // Delta Voltage
        private double actualACVoltage;
        private double dcVal;
        private double actualImpulseVoltage;
        private double dvAC;
        private double acLast;
        private double dvDC;
        private double dcLast;
        private double dvImp;
        private double impLast;
        private double voltageDiff;
        private double lastVoltage;
        List<double> voltageList = new List<double>();

        // Delta Current 
        private double currentDiff;
        private double actualCurrent;
        private double lastCurrent;
        List<double> currentList = new List<double>();

        // Breakdown
        private double voltageFlashoverLimit = 10;
        private double currentFlashoverLimit = 3;
        private bool flashoverDetected;
        private double dVdt;
        private double dIdt;
        private bool aborting;
        private double actualTestVoltageMax;
        private double actualDCVoltage;
        private double impulseLimitMax;
        private double impulseLimitMin;
        private double previousRegulatedVoltageValue;
        private double previousTestVoltage;
        private bool outputDead = false;
        private DateTime outputDeadStartTime;
        private TimeSpan outputDeadTimeSpan;
        private double impulseRange;
        private List<double> impulseTargetVoltageList;
        private double impulseVoltageStep;
        private double nextImpulseVoltage;
        private double lastImpulseVoltage;
        private bool levelsFinished;
        private double impulseTargetVoltage;
        private int remainingImpulseThisLevel;
        private bool levelClear;
        private bool inBounds;
        private bool noBreakdown;
        private bool breakdownOccurred;
        internal bool triggerRequest = false;
        private bool triggerRequestValue;
        private bool triggerFailed;
        private int failedTriggercount;
        internal Channel.ScaledData impulseData;
        private double[] dataX;
        private double[] dataY;
        private double nextGap;
       
        private bool pauseRegulation;
        private double lastTestVoltageMax = 0;
        private bool acquiringGap;
        private bool gapAcquired;
        private double targetGap;
        private int destination;
        private bool triggerAttempted;
        private object bla;


        // Contructor
        public AutoTest(RunView runViewIn, DashBoardView dashboardViewIn, MeasuringForm measuringFormIn, NA9739Device PIO1In, PD1161Device HV9126In, PD1161Device HV9133In, Channel acChannelIn, Channel dcChannelIn, Channel impulseChannelIn)
        {
            runView = runViewIn;
            dashboardView = dashboardViewIn;
            autoTestChart = runView.autoTestChart;
            PIO1 = PIO1In;
            HV9126 = HV9126In;
            HV9133 = HV9133In;
            acChannel = acChannelIn;
            dcChannel = dcChannelIn;
            impChannel = impulseChannelIn;
            measuringForm = measuringFormIn;

            // Timer to update the chart and hold the testVoltage in bounds during AC/DC tests
            sampleTimer = new Timer();
            sampleTimer.Tick += new EventHandler(this.sampleTimer_Tick);

            // Timer to retreive the latest HV values
            updateVoltageOutputValuesTimer = new Timer();
            updateVoltageOutputValuesTimer.Tick += new EventHandler(updateVoltageOutputValuesTimer_Tick);
            updateVoltageOutputValuesTimer.Interval = 50;
            updateVoltageOutputValuesTimer.Enabled = true;
            updateVoltageOutputValuesTimer.Start();

            // Timer to update the chart and hold the testVoltage in bounds during impulse tests
            impulseRoutineTimer = new Timer();
            impulseRoutineTimer.Tick += new EventHandler(this.impulseRoutineTimer_Tick);
            impulseRoutineTimer.Interval = 800;
            //impulseRoutineTimer.Enabled = true;

            // Timer to timeout a trigger attempt
            triggerTimeoutTimer = new Timer();
            triggerTimeoutTimer.Tick += new EventHandler(this.triggerTimeoutTimer_Tick);
            triggerTimeoutTimer.Interval = 2000;
            //triggerTimeoutTimer.Enabled = true;

            // Update the chart, but only if we are connected
            if (PIO1.K2Closed)
            { 
                xList.Add((double)0);
                yList.Add((double)0);
                UpdateChart();
            }
        }

        // Impulse has been triggered, but no response after 2 seconds
        private void triggerTimeoutTimer_Tick(object sender, EventArgs e)
        {
            //Stop me
            triggerTimeoutTimer.Stop();

            // Set flag to indicate failure to overseeing routine
            triggerFailed = true;

            // Increment a counter 
            failedTriggercount += 1;

            if(failedTriggercount > 5)
            {
                // Notify of problem - Check Batteries?
                abortRegulation = true;
                AbortTest();
            }
        }

        // Refresh the voltage output values (every 10ms)
        private void updateVoltageOutputValuesTimer_Tick(object sender, EventArgs e)
        {
            // Gets the value of the pre-set ac voltage type.
            actualACVoltage = acChannel.getRepresentation();
            runView.acValueLabel.Text = actualACVoltage.ToString("0.0");
            actualDCVoltage = dcChannel.getRepresentation();
            runView.dcValueLabel.Text = actualDCVoltage.ToString("0.0");
            actualImpulseVoltage = impChannel.getRepresentation();
            runView.impulseValueLabel.Text = actualImpulseVoltage.ToString("0.0");
           
            // Set the regulation value
            if (runView.voltageComboBox.SetSelected == "AC") actualTestVoltage = Convert.ToDouble(runView.acValueLabel.Text);
            else if (runView.voltageComboBox.SetSelected == "DC") actualTestVoltage = Convert.ToDouble(runView.dcValueLabel.Text);
            else if (runView.voltageComboBox.SetSelected == "Imp") actualTestVoltage = Convert.ToDouble(runView.dcValueLabel.Text)*0.93;

            // Update the max value if Disruptive Discharge test
            if (!testIsWithstand)
            {
                runView.elapsedTimeLabel.Text = sampleRate.ToString("0.0");
  
                actualTestVoltageMax = actualTestVoltage;
                runView.resultTestVoltageValueLabel.Text = lastTestVoltageMax.ToString("0.0");
                lastTestVoltageMax = actualTestVoltageMax;
       
            }
            
            // Calculate the voltage derivitive
            //voltageDiff = actualVoltage - lastVoltage;
            //dVdt = voltageDiff / sampleRate;
            //if (dVdt != 0) voltageList.Add(dVdt);
            //lastVoltage = actualVoltage;

            // Calculate the current derivitive
            actualCurrent = PIO1.regulatedCurrentValue;
            currentDiff = actualCurrent - lastCurrent;
            dIdt = currentDiff / sampleRate;
            if(dIdt != 0)currentList.Add(dIdt);
            lastCurrent = actualCurrent;

            // Compare with the preset limit
            if ((currentDiff > currentFlashoverLimit)) // (voltageDiff > voltageFlashoverLimit) || 
            {
                flashoverDetected = true;
            }         
        }

        // The timed event which controls impulse tests
        private void impulseRoutineTimer_Tick(object sender, EventArgs e)
        {
            
            // Do we have the resource? If not, then bail.
           if (impulseTargetVoltageList == null) return;
           else if (impulseTargetVoltageList.Count > 0) levelsFinished = false;


            //////// GAP CALCULATION and ACQUIREMENT
            // Calculate the next gap
            destination = CalculateDestination(impulseTargetVoltageList[0]);

            // Move sphere to correct gap
            if ((!acquiringGap) && (HV9126.actualPosition != destination))
            {
                pauseRegulation = true;

                // Mo to the position
                HV9126.MoveToPosition((int)nextGap);

                // set a flag so we don't call AcquireGap every time
                acquiringGap = true;

                return;
            }
    
            // If gap has been acquired
            if(HV9126.actualPosition == destination)
            {
                acquiringGap = false;
                gapAcquired = true;


                pauseRegulation = false;
            }




            UpdateChart();



            //////// ABORT ROUTINES
            // Abort button has been clicked. VOID label and aborting flag was set in AbortTest()
            if ((aborting) && (!PIO1.minUPos))
            {
                // Stop regulation the transformer
                abortRegulation = true;
                
                // Do not continue in this method
                return;
            }
            else if ((aborting) && (PIO1.minUPos))
            {
                // Tidy up flags etc
                CleanUpAfterTest();

                // Do not continue in this method
                return;
            }




            // Check for bounds - IN
            if (inBounds)
            {
                pauseRegulation = true;
                
                //Check for levelsFinished
                if (!levelsFinished)
                {

                    // Set the new target voltage - regulation should occur automatically as the voltage is now out of bounds
                    impulseTargetVoltage = impulseTargetVoltageList[0];
                    remainingImpulseThisLevel = impulsePerLevel;
                    levelClear = false;
                    

                    if (!levelClear) 
                    {
                        if (!triggerAttempted)
                        {
                            // reset impulseVoltageValue and previous failure flag
                            actualImpulseVoltage = 0;
                            triggerFailed = false;

                            // Trigger
                            TriggerRequest = true;
                            triggerAttempted = true;

                            // Start a timeouttimer
                            triggerTimeoutTimer.Start();

                            // Update the status to show we are waiting for trigger result
                            runView.passFailLabel.Text = "TRIG";
                            runView.passFailLabel.Invalidate();
                        }
                        else if ((triggerAttempted) && (!triggerFailed))
                        {
                            // Check to see if there was an impulse received - this may take > 2 seconds

                            // Still waiting, do we have a result?
                            if (actualImpulseVoltage > 0)
                            {

                                // Result! Stop the timeouttimer
                                triggerTimeoutTimer.Stop();

                                // update the status 
                                //runView.passFailLabel.Text = "EVAL";
                                //runView.passFailLabel.Invalidate();

                                //////// if so, Update list with x(index nr) and y(voltage value)
                                xList.Add(xList.Count + 1);

                                yList.Add(actualImpulseVoltage);

                                //////// Analyse graph for breakdown - if so, Set breakdownOccurred = true;
                                AnalyzeImpulseCurve();

                                //////// Update bool breakdownList = breakdownOccurred 
                                breakdownList.Add(breakdownOccurred);

                                //////// Update graph - red or blue points - red for breakdown
                                //UpdateImpulseChart();

                                //////// Decrement remainingImpulseThisLevel;
                                remainingImpulseThisLevel -= 1;

                            }
                            else
                            {
                                // No result yet, keep waiting
                            }
                        }
                        else if ((TriggerRequest) && (triggerFailed))
                        {                         
                            // Finalization - call PASS/FAIL/VOID finishing routine - ADD CONDITIONS!!!
                            VoidImpTest();
                        }
                        

                        //////// Impulse complete, check for more on this level - if last, set levelClear flag
                        if (remainingImpulseThisLevel == 0)
                        {
                            levelClear = true;
                            // Reset the failed trigger counter
                            failedTriggercount = 0;
                        }
                        
                    }
                    else if (levelClear)
                    {
                        // Level finished, check for more - if none, set levelsFinished flag, if some, Remove top element
                        if (impulseTargetVoltageList.Count > 1)
                        {
                            gapAcquired = false;
                            impulseTargetVoltageList.RemoveAt(0);
                        }
                        else
                            levelsFinished = true;
                    }
                                        
                }
                else if(levelsFinished)
                {
                    // No breakdown means we don't have high enough voltage capability to test the test object
                    if (!breakdownOccurred) PassImpTest();
                    else if (breakdownOccurred) FailImpTest();
                }
            }
            else if (!inBounds)
            {
                // Out of bounds but still working
                if (!parking)
                {
                    // Levels not finished, do nothing but wait to get inBounds
                    pauseRegulation = false;
                }
                // Out of bounds, Levels finished
                else if (parking)
                {
                    // Finished and Parking
                    if (!PIO1.minUPos)
                    {
                        // Wait
                    }
                    // Finished, Parking and reached zero
                    else if (PIO1.minUPos)
                    {
                        // Tidy up flags etc
                        CleanUpAfterImpTest();
                    }
                }
            }
        }

        private void CleanUpAfterImpTest()
        {
            impulseRoutineTimer.Stop();
            parking = false;
            flashoverDetected = false;
            abortRegulation = false;
            inBounds = false;
            pauseRegulation = false;
            aborting = false;
            breakdownOccurred = false;
            levelClear = false;
            levelsFinished = false;
            breakdownOccurred = false;
            TriggerRequest = false;
            triggerFailed = false;
            triggerRequest = false;

            // Disconnect the power
            PIO1.openSecondary();
            Thread.Sleep(1500);
            PIO1.openPrimary();

            // Enable/Disable buttons
            runView.onOffAutoButton.Enabled = true;
            runView.abortAutoTestButton.Enabled = false;
        }

        private void FailImpTest()
        {
            abortRegulation = true;

            // Present the status
            runView.passFailLabel.Text = "FAIL";
            runView.passFailLabel.Visible = true;
            runView.passFailLabel.Invalidate();

            // Park the transformer
            PIO1.ParkTransformer();

            // Reset the Start button
            runView.onOffAutoButton.isChecked = false;
            runView.onOffAutoButton.Invalidate();
        }

        private void PassImpTest()
        {
            abortRegulation = true;

            // Present the status
            runView.passFailLabel.Text = "PASS";
            runView.passFailLabel.Visible = true;
            runView.passFailLabel.Invalidate();

            // Park the transformer
            PIO1.ParkTransformer();

            // Reset the Start button
            runView.onOffAutoButton.isChecked = false;
            runView.onOffAutoButton.Invalidate();
        }

        private void VoidImpTest()
        {
            abortRegulation = true;

            // Present the status
            runView.passFailLabel.Text = "FAIL";
            runView.passFailLabel.Visible = true;
            runView.passFailLabel.Invalidate();

            // Park the transformer
            PIO1.ParkTransformer();

            // Reset the Start button
            runView.onOffAutoButton.isChecked = false;
            runView.onOffAutoButton.Invalidate();
        }




        // Automated voltage set routine for Impulse
        public void GoToImpulseVoltageAuto()
        {
            // Create a new thread so as not to disturb the other routines
            Thread regUAutoImpThread = new Thread(RegulateImpulseVoltage);
            regUAutoImpThread.Start();
        }




        // Voltage regulation for impulse test only
        private void RegulateImpulseVoltage()
        {
            // Set some tolerances (we aren't perfect)
            double targetVoltage;
            double toleranceHi = 0.18;
            double toleranceLo = -0.18;

            // Pd Variables - if needed?
            float P = 0;
            float k = 4;
            float d = 0;
            double error = 10;
            double previousError = 0;
            double integral = 0;
            int intCnt = 0;
            int styr = 30;

            // Protection setup
            outputDead = false;
            previousTestVoltage = actualTestVoltage;
            previousRegulatedVoltageValue = PIO1.regulatedVoltageValue;

            // Wait here a bit for K2 to close
            Thread.Sleep(500);

            // Continue until targetVoltage found or process aborted 
            while ((runView.onOffAutoButton.isChecked) && (!abortRegulation)) // ((error < toleranceLo) || (error > toleranceHi)) && 
            {

                // Pause station if needed
               if (pauseRegulation)
                {
                    PIO1.StopTransformerMotor();
                    Thread.Sleep(300);
                }
               else if(!pauseRegulation)
               {
                    // Firstly, some protection for when Input voltage is changing but output is not
                    if ((PIO1.regulatedVoltageValue != previousRegulatedVoltageValue) && (actualTestVoltage == previousTestVoltage))
                    {
                        // Set flag and note time of first entry
                        if (!outputDead)
                        {
                            // Do this once
                            outputDeadStartTime = DateTime.Now;
                            outputDead = true;
                        }
                        else
                        {
                            // Output still not following input
                            outputDeadTimeSpan = DateTime.Now - outputDeadStartTime;

                            // Have we waited long enough?
                            if (outputDeadTimeSpan.Seconds >= 8)
                            {
                                // Shut down, something is wrong
                                abortRegulation = true;
                                //AbortTest();
                                return;
                            }

                        }
                    }
                    // Output is following input voltage
                    else
                    {
                        // Make sure the flag is reset
                        outputDead = false;
                    }


                    // actualTestVoltage is set in the output values timer as dc * 0.92
                    // Get the latest target voltage
                    targetVoltage = impulseTargetVoltageList[0];

                    // Calculate the error
                    error = actualTestVoltage - targetVoltage;
                
                    // If we are in bounds, stop the transformer and set a flag for the trigger routine
                    if ((error > toleranceLo) && (error < toleranceHi))
                    {
                        PIO1.StopTransformerMotor();
                        inBounds = true;
                    }
                    else
                    {
                        // Not in bounds, do your thing
                        inBounds = false;

                        // Run only if we have connected the voltage
                        if (PIO1.K2Closed)
                        {
                            if (error == previousError)
                            {
                                integral += 0.1;
                            }
                            else
                            {
                                integral = 0;
                            }

                            // Call the appropriate instruction
                            if (error < toleranceHi)
                            {
                                // Voltage low, set the reaction
                                if ((error <= 5) && (error >= -5))
                                {
                                    styr = 57 + (int)integral;
                                }
                                else
                                {
                                    styr = (int)((error * -k) + 60 + integral);
                                }
                            
                                // On Disruptive Discharge test, speed is set to slow - used in AC/DC, don't know if needed here
                                // if (!testIsWithstand) styr = 120;
                            
                                // Increase
                                PIO1.increaseVoltage(styr);
                            }
                            else if (error > toleranceLo)
                            {
                                // Voltage high, set the reaction
                                if ((error <= 3) && (error >= -3))
                                {
                                    styr = 57 + (int)integral;
                                }
                                else
                                {
                                    styr = (int)((error * k) + 60 + integral);
                                }
                                // Decrease
                                PIO1.decreaseVoltage(styr);
                            }

                            Thread.Sleep(10);
                            previousError = error;
                        }
                        else
                        {
                            // K2 is not connected, Stop transformer
                            PIO1.StopTransformerMotor();
                        }
                    }
                }

            }

            // Abort or OnOff(OFF) has been called
            PIO1.StopTransformerMotor();
            abortRegulation = true;

        }








        // Look at the latest impulse curve values and determine if a breakdown has occurred
        private void AnalyzeImpulseCurve()
        {
            
            
            // values from measuringForm.chart: (we may need to wait here until they arrive)
            dataX = impulseData.x;
            dataY = impulseData.y;

            

            //for (int i = 0; i < dataY.Length-1; i++)
            //{

            //    // Calculate the rate of change for a block

               

            //}
            //impulsechart.Series["impulseSeries"].Points.DataBindXY(data.x, data.y);
        }

        private int CalculateDestination(double impulseTargetVoltageIn)
        {
            //nextGap = (0.0001 * Math.Pow(impulseTargetVoltageIn, 2)  ) + (0.1824 * impulseTargetVoltageIn) + 3.5905;
            nextGap = (0.0005 * Math.Pow(impulseTargetVoltageIn, 2)) + (0.326 * impulseTargetVoltageIn) + 3.9868;
            nextGap += 10;
            return (int)nextGap;
        }

        // Trigger request property in order to fire event in Controller Class
        public bool TriggerRequest
        {
            get
            {
                return this.triggerRequestValue;
            }

            set
            {
                if (value != this.triggerRequestValue)
                {
                    this.triggerRequestValue = value;
                    OnPropertyChanged("triggerRequest");
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;


        // The timed event where everything is updated during AC/DC tests
        private void sampleTimer_Tick(object sender, EventArgs e)
        {

            // Update the chart
            if((!flashoverDetected)&&(!parking)&&(PIO1.K2Closed)&&(PIO1.regulatedVoltageValue > 3)) UpdateChart();

            // Finished successfully, and parked.
            if ((aborting) && (PIO1.minUPos))
            {
                // Success!
                Thread.Sleep(1000);

                // Tidy up flags etc
                CleanUpAfterTest();

                return;
            }
            else if ((aborting) && (!PIO1.minUPos))
            {
                // Drive the voltage down to zero
                PIO1.ParkTransformer();

                return;
            }

            // The dVdt or dIdt calculations indicate a breakdown has occured
            if (flashoverDetected)
            {
                // If first time here
                if (!parking)
                {
                    // Abort the regulation routine
                    abortRegulation = true;

                    // Call park, reset variables m.m.
                    FailTest();
                    
                    // Set a flag so we don't come back in next time
                    parking = true;
                }
            }

            // Ths is what we do when we have found the targetVoltage - In bounds
            if ((actualTestVoltage < testVoltageToleranceHigh) && (actualTestVoltage > testVoltageToleranceLow))
            {
                // First time here
                if (!isRunning)
                {
                    // Set flag and start timer
                    isRunning = true;
                    startTime = DateTime.Now;
                    runView.passFailLabel.Text = "EVAL";
                    runView.passFailLabel.Invalidate();
                }
                else
                {
                    // Returning loop, normal test in progress
                    simElapsedTime += sampleRate;

                    // Update the UI label on whole numbers only
                    if((simElapsedTime - Math.Truncate(simElapsedTime)) == 0) runView.elapsedTimeLabel.Text = simElapsedTime.ToString();

                    // Time is up!
                     if (simElapsedTime > duration)
                    {
                        // Stop updating the timer
                        runView.elapsedTimeLabel.Text = duration.ToString();
                        
                        // First park the transformer before we stop taking results
                        if (!PIO1.minUPos)
                        {
                            if (!parking)
                            {
                                // Abort any regulation
                                abortRegulation = true;
                                
                                // Park the transformer
                                PIO1.ParkTransformer();

                                // Call park, reset variables m.m.
                                PassTest();

                                // Set a flag so we don't come back in next time
                                parking = true;
                            }
                        }
                    }                
                }
            }

            // We found the testVoltage but now the voltage is too high
            if ((isRunning) && (actualTestVoltage > testVoltageToleranceHigh))
            {
                // Drive the voltage down to zero
                PIO1.ParkTransformer();

                // Abort the test
                AbortTest();

                // Set aborting flag
                aborting = true;
            }
            
            // We found the testVoltage but now the voltage is too low, and it's not beacuse we are parking
            else if ((isRunning) && (actualTestVoltage < testVoltageToleranceLow) && (!parking))
            {
                // Drive the voltage down to zero
                PIO1.ParkTransformer();

                // Abort the test
                AbortTest();

                // Set aborting flag
                aborting = true;

            }

            // Finished successfully, and parked.
            if ((parking) && (PIO1.minUPos))
            {

                // Update the chart
                if (PIO1.K2Closed) UpdateChart();

                // Tidy up flags etc
                CleanUpAfterTest();
            }
            else if ((parking) && (!PIO1.minUPos))
            {
                // Update the chart
                if(PIO1.K2Closed)UpdateChart();
            }
        }

        // Successfully completed test
        private void PassTest()
        {
            abortRegulation = true;

            // Present the status
            runView.passFailLabel.Text = "PASS";
            runView.passFailLabel.Visible = true;
            runView.passFailLabel.Invalidate();
  
            // Park the transformer
            PIO1.ParkTransformer();

            // Reset the Start button
            runView.onOffAutoButton.isChecked = false;
            runView.onOffAutoButton.Invalidate();
        }

        // Failed test
        private void FailTest()
        {
           
            // Present the status
            runView.passFailLabel.Text = "FAIL";
            runView.passFailLabel.Visible = true;
            runView.passFailLabel.Invalidate();

            Thread.Sleep(1000);
            
            // Drive the voltage down to zero
            PIO1.ParkTransformer();

            // We have had breakdown, and opened the contactors, nothing to see here
            // Dont stop the timer or we won't know when we have parked and we won't clean up
            //sampleTimer.Stop();

            // Reset the start button
            runView.onOffAutoButton.isChecked = false;
            runView.onOffAutoButton.Invalidate();
        }

        // Aborted test. Stop the test and reset
        internal void AbortTest()
        {
            aborting = true;
            abortRegulation = true;

            // Drive the voltage down to zero
            PIO1.ParkTransformer();

            // Present the status
            runView.passFailLabel.Text = "VOID";
            runView.passFailLabel.Visible = true;
            //runView.testStatusLabel.Visible = false;
            runView.passFailLabel.Invalidate();

            // Reset the start button
            runView.onOffAutoButton.isChecked = false;
            runView.onOffAutoButton.Invalidate();
        }

        // Stop but keep the chart running
        internal void SoftAbortTest()
        {
        }
        
        // Temporarily stop the timer
        internal void PauseTest()
        {
        }

        // Resume the test after pausing
        internal bool ResumeTest()
        {
            // Clear previous flags
            abortRegulation = false;

            // Reset the flag to be able to pause again
            isPaused = false;

            // Connect the output power
            PIO1.overrideUMin = true;
            PIO1.closeSecondary();
            
            // Resume updating chart
            sampleTimer.Start();

            return true;
        }

        // Finally
        internal void CleanUpAfterTest()
        {
            sampleTimer.Stop();
            impulseRoutineTimer.Stop();
            isRunning = false;
            isPaused = false;
            parking = false;
            flashoverDetected = false;
            abortRegulation = false;
            pauseRegulation = false;
            aborting = false;
            breakdownOccurred = false;

            // Disconnect the power
            PIO1.openSecondary();
            Thread.Sleep(1500);
            PIO1.openPrimary();

            // Enable/Disable buttons
            runView.onOffAutoButton.Enabled = true;
            runView.abortAutoTestButton.Enabled = false;
        }

        // Run the test
        public bool StartTest()
        {
            double intSampleRate;
            int msSampleRate;

            // Clear previous flags
            abortRegulation = false;
            inBounds = false;

            // Enable/Disable buttons
            runView.onOffAutoButton.Enabled = false;
            runView.abortAutoTestButton.Enabled = true;

            // Set status label value
            runView.passFailLabel.Text = "EVAL";
            runView.passFailLabel.Visible = true;
            runView.passFailLabel.Invalidate();

            // Set elapsed time and label value
            simElapsedTime = 0;
            runView.elapsedTimeLabel.Text = "0";
            runView.elapsedTimeLabel.Invalidate();

            //Disruptive discharge
            actualTestVoltageMax = 0;
            //runView.resultTestVoltageValue.Text = maxActualVoltage.ToString();

            // Clear the derivitive logs
            voltageList.Clear();
            currentList.Clear();

            // Get rid of logo
            ShrinkLogo();

            // Disruptive discharge
            if(runView.voltageComboBox.SetSelected == "Imp")
            {
                // Impulse testing - How to identify flashover?? Cycle each point in impulse curve (measurementForm)to get derivitive value. If dv/dt > breakdownLimit, we have had a breakdown.

                // Impulse Withstand: levels = 1(testVoltage), impulses per level = user defined (usually 1-5). Set gap distance to prevent spontaneous breakdown in gap. Run up to level[0] = testVoltage. 
                // Trigger impulse. If impulsePeak < 20%, impulse generation is successful, if 0.95 x DC, then no breakdown has occurred. 
                // Update graph. Set LineType to individual points. Blue = no breakdown Red = breakdown. Repeat (impulsesPerLevel).

                // Impulse disruptive discharge: testVoltage = maxVoltage. Create array with levelsArray[levels] up to max.
                // Check for stability at each level? *Do not stop regulation!   

                // Disable any unwanted timers
                sampleTimer.Enabled = false;

                // Get levels info
                impulseLevels = (int)runView.impulseVoltageLevelsTextBox.Value;

                // Get impulses/level info
                impulsePerLevel = (int)runView.impPerLevelTextBox.Value;

                // Get min/max levels
                impulseLimitMax = runView.maxImpulseVoltageTextBox.Value;
                impulseLimitMin = runView.minImpulseVoltageTextBox.Value;

                // Create level list and fill it
                impulseRange = impulseLimitMax - impulseLimitMin;
                impulseVoltageStep = impulseRange / impulseLevels;
                impulseTargetVoltageList = new List<double>();

                nextImpulseVoltage = impulseLimitMin;

                while (nextImpulseVoltage <= impulseLimitMax)
                {
                    impulseTargetVoltageList.Add(nextImpulseVoltage);
                    nextImpulseVoltage += impulseVoltageStep;
                }

                // Run impulse disruptive routine
                triggerTimeoutTimer.Enabled = true;
                impulseRoutineTimer.Enabled = true;
                impulseRoutineTimer.Start();

                // Clear the arraylists and reset the sample counter
                xList.Clear();
                yList.Clear();
                sampleNumber = 0;

                // Connect the power
                PIO1.closePrimary();
                Thread.Sleep(2000);
                PIO1.closeSecondary();

                // Start regulation routine but Pause regulation untill the Gap is found
                GoToImpulseVoltageAuto();
                pauseRegulation = true;
            }
            else
            {

                // AC Withstand, DC Withstand: Run up to testVoltage, hold for duration, then park. Quit if flashover (voltage outside of bounds).
                // AC Disruptive, DC Disruptive discharge: testVoltage = maxVoltage, duration = 1s, trafSpeed = slow(180). 
                // Run up to testVoltage. Quit if flashover (inCurrent spikes).
                // Note last voltage before flashover

                // Disable any unwanted timers
                triggerTimeoutTimer.Enabled = false;
                impulseRoutineTimer.Enabled = false;

                // Reset some flags
                failedTriggercount = 0;
           

                // Get user input and convert to usable values
                duration = Convert.ToInt32(runView.testDurationTextBox.Value);
                sampleRate = Convert.ToDouble(runView.sampleRateTextBox.Value);
                intSampleRate = sampleRate * 1000;
                msSampleRate = Convert.ToInt32(intSampleRate);

                // Set up timer parameters and start
                sampleTimer.Interval = msSampleRate;
                sampleTimer.Enabled = true;
                sampleTimer.Start();

                // Set targetVoltage to Stage Max
                targetVoltage = Convert.ToDouble(runView.testVoltageTextBox.Value);       // already set to 1/2/3-Stage max when disruptive discharge selected
                tolerance = targetVoltage * (runView.toleranceTextBox.Value / 100);
                testVoltageToleranceHigh = targetVoltage + tolerance;
                testVoltageToleranceLow = targetVoltage - tolerance;

                // Clear the arraylists and reset the sample counter
                xList.Clear();
                yList.Clear();
                sampleNumber = 0;

                // Connect the power
                PIO1.closePrimary();
                Thread.Sleep(2000);
                PIO1.closeSecondary();

                // Start regulation routine but Pause regulation untill the Gap is found
                GoToVoltageAuto();
            }

            return true;

        }

        private void RunACDCDisruptive()
        {
            throw new NotImplementedException();
        }


        // Automated voltage set routine
        public void GoToVoltageAuto()
        {
            
            Thread regUAutoThread = new Thread(RegulateVoltage);
            regUAutoThread.Start();
           
        }

        // Set dynamic bounds from user input, then start the auto process
        private void RegulateVoltage()
        {
            // Set some tolerances (we aren't perfect)
            double targetVoltage = runView.testVoltageTextBox.Value;
            double toleranceHi = 0.18;
            double toleranceLo = -0.18;

            // Variable to hold our selectable measured voltage value           
            double uActual = 0;

            // Pd Variables - if needed?
            float P = 0;
            float k = 7;
            float d = 0;
            double error = 10;
            double previousError = 0;
            double integral = 0;
            int intCnt = 0;
            int styr = 30;

            // Protection setup
            //outputDead = false;
            //previousTestVoltage = actualTestVoltage;
            //previousRegulatedVoltageValue = PIO1.regulatedVoltageValue;

            // Set tolerances
            voltageType = runView.voltageComboBox.SetSelected;

            if (voltageType == "AC") 
            {
                toleranceHi = 0.2;
                toleranceLo = -0.2;
            }
            else if (voltageType == "DC")
            {
                toleranceHi = 0.15;
                toleranceLo = -0.15;
            }
            else if (voltageType == "Imp")
            {
                toleranceHi = 0.15;
                toleranceLo = -0.15;
            }
            else
            {
                // Shut down, something is wrong
                abortRegulation = true;
                AbortTest();
                return;
            }

            // Wait here a bit for K2 to close
            Thread.Sleep(500);

                // Continue until targetVoltage found or process aborted 
                while (((error < toleranceLo) || (error > toleranceHi)) && (runView.onOffAutoButton.isChecked) && (!abortRegulation))
                {
                // Firstly, some protection for when Input voltage is changing but output is not
                if ((PIO1.regulatedVoltageValue != previousRegulatedVoltageValue) && (actualTestVoltage == previousTestVoltage))
                {
                    // Set flag and note time of first entry
                    if (!outputDead)
                    {
                        // Do this once
                        outputDeadStartTime = DateTime.Now;
                        outputDead = true;
                    }
                    else
                    {
                        // Output still not following input
                        outputDeadTimeSpan = DateTime.Now - outputDeadStartTime;

                        // Have we waited long enough?
                        if (outputDeadTimeSpan.Seconds >= 5)
                        {
                            // Shut down, something is wrong
                            abortRegulation = true;
                            AbortTest();
                            return;
                        }

                    }
                }
                //Output is following input voltage
                else
                {
                    // Make sure the flag is reset
                    outputDead = false;
                }

                    // Run only if we have connected the voltage
                    if (PIO1.K2Closed)
                    {

                        error = actualTestVoltage - targetVoltage;

                        if (error == previousError)
                        {
                            integral += 0.1;
                        }
                        else
                        {
                            integral = 0;
                        }

                        // Call the appropriate instruction
                        if (error < toleranceHi)
                        {
                            // Voltage low, set the reaction
                            if ((error <= 3) && (error >= -3))
                            {
                                styr = 57 + (int)integral;
                            }
                            else
                            {
                                styr = (int)((error * -k) + 60 + integral);
                            }
                        
                            // On AC/DC Disruptive Discharge test, speed is set to slow
                            if (!testIsWithstand) styr = 120;
                            
                            // Increase
                            PIO1.increaseVoltage(styr);

                        }
                        else if (error > toleranceLo)
                        {
                            // Voltage high, set the reaction
                            if ((error <= 3) && (error >= -3))
                            {
                                styr = 57 + (int)integral;
                            }
                            else
                            {
                                styr = (int)((error * k) + 60 + integral);
                            }
                            
                            
                            // Decrease
                            PIO1.decreaseVoltage(styr);

                        }

                        Thread.Sleep(10);
                        previousError = error;
                    }
                    else
                    {
                        // K2 is not connected, Stop transformer but don't abort
                        PIO1.StopTransformerMotor();
                    }
                }

                // In bounds. We should only make it here once
                PIO1.StopTransformerMotor();
                abortRegulation = true;
              
        }

        internal void UpdateChart()
        {
            xList.Add((double)sampleNumber);
            yList.Add((double)actualTestVoltage);

            sampleNumber += 1;
            xArray = (Double[])xList.ToArray();
            yArray = (Double[])yList.ToArray();
            autoTestChart.Series["Series1"].Points.Clear();
            //There are two ways to add points 
            //1) Add points one by one with the AddXY method 
            //runView.autoTestChart.Series["Series1"].Points.AddXY(x[r], y[r]);
            //2) by using databind and adding all the point at once
            autoTestChart.Series.SuspendUpdates();
            autoTestChart.Series["Series1"].Points.DataBindXY(xArray, yArray);

            //If you want 10Div * 10Div
            autoTestChart.ChartAreas[0].AxisX.Interval = (int)((xArray.Max() - xArray.Min()) / 10);
            autoTestChart.ChartAreas[0].AxisY.Interval = (int)((yArray.Max() - yArray.Min()) / 10);
            autoTestChart.Series.ResumeUpdates();
        }

        internal void GrowLogo()
        {
            runView.dynamicLogoPictureBox.Width = 0;
            runView.dynamicLogoPictureBox.Height = 0;
            runView.dynamicLogoPictureBox.Visible = true;

            logoGrowEffectTimer = new Timer();
            logoGrowEffectTimer.Tick += new EventHandler(logoGrowEffect_Tick);
            logoGrowEffectTimer.Interval = 1;
            logoGrowEffectTimer.Enabled = true;
            logoGrowEffectTimer.Start();

        }

        internal void ShrinkLogo()
        {
            logoShrinkEffectTimer = new Timer();
            logoShrinkEffectTimer.Tick += new EventHandler(logoShrinkEffect_Tick);
            logoShrinkEffectTimer.Interval = 1;
            logoShrinkEffectTimer.Enabled = true;
            logoShrinkEffectTimer.Start();
           
        }

        private void logoShrinkEffect_Tick(object sender, EventArgs e)
        {
            int dx = 0;
            int dy = 0;

            if (runView.dynamicLogoPictureBox.Width > 1) runView.dynamicLogoPictureBox.Width -= 15;
            if (runView.dynamicLogoPictureBox.Height > 1) runView.dynamicLogoPictureBox.Height -= 10;

            if (runView.dynamicLogoPictureBox.Location.X > 62) dx = 20;
            if (runView.dynamicLogoPictureBox.Location.Y > 50) dy = 10;

            runView.dynamicLogoPictureBox.Location = new Point(runView.dynamicLogoPictureBox.Left -= dx, runView.dynamicLogoPictureBox.Top -= dy);

            if ((runView.dynamicLogoPictureBox.Width <= 1) && (runView.dynamicLogoPictureBox.Height <= 1) && (runView.dynamicLogoPictureBox.Location.X <= 200) && (runView.dynamicLogoPictureBox.Location.Y <= 150))
            {
                logoShrinkEffectTimer.Stop();
                runView.dynamicLogoPictureBox.Visible = false;
                runView.autoTestChart.Visible = true;
                runView.autoTestChart.Invalidate();
            }
        }

        private void logoGrowEffect_Tick(object sender, EventArgs e)
        {
            int dx = 0;
            int dy = 0;

            if (runView.dynamicLogoPictureBox.Width < 756) runView.dynamicLogoPictureBox.Width += 8;
            if (runView.dynamicLogoPictureBox.Height < 301) runView.dynamicLogoPictureBox.Height += 6;

            if (runView.dynamicLogoPictureBox.Location.X > 62) dx = 2;
            if (runView.dynamicLogoPictureBox.Location.Y > 50) dy = 1;

            runView.dynamicLogoPictureBox.Location = new Point(runView.dynamicLogoPictureBox.Left -= dx, runView.dynamicLogoPictureBox.Top -= dy);

            if ((runView.dynamicLogoPictureBox.Width >= 756) && (runView.dynamicLogoPictureBox.Height >= 301) && (runView.dynamicLogoPictureBox.Location.X <= 100) && (runView.dynamicLogoPictureBox.Location.Y <= 50))
            {
                logoGrowEffectTimer.Stop();
            }
        }

        
    }
}
