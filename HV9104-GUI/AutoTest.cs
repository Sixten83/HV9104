using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
    
using System.Windows.Forms.DataVisualization.Charting;
using Timer = System.Windows.Forms.Timer;


namespace HV9104_GUI
{
    public class AutoTest
    {
        // Source for the actual values + controls
        RunView runView;
        DashBoardView dashboardView;
        NA9739Device PIO1;
        PD1161Device HV9126;
        PD1161Device HV9133;
        Chart autoTestChart;

        // Timer
        Timer sampleTimer;
        //Timer impTestTimer;
        
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


        // Contructor
        public AutoTest(RunView runViewIn, DashBoardView dashboardViewIn, NA9739Device PIO1In, PD1161Device HV9126In, PD1161Device HV9133In, Channel acChannelIn, Channel dcChannelIn, Channel impulseChannelIn)
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

            // Timer to update the chart and hold the testVoltage in bounds
            sampleTimer = new Timer();
            sampleTimer.Tick += new EventHandler(this.sampleTimer_Tick);

            // Timer to retreive the latest HV values
            updateVoltageOutputValuesTimer = new Timer();
            updateVoltageOutputValuesTimer.Tick += new EventHandler(updateVoltageOutputValuesTimer_Tick);
            updateVoltageOutputValuesTimer.Interval = 10;
            updateVoltageOutputValuesTimer.Enabled = true;
            updateVoltageOutputValuesTimer.Start();

            // Update the chart, but only if we are connected
            if (PIO1.K2Closed)
            { 
                xList.Add((double)0);
                yList.Add((double)0);
                UpdateChart();
            }
        }

        // Refresh the voltage output values (every 10ms)
        private void updateVoltageOutputValuesTimer_Tick(object sender, EventArgs e)
        {
            // Gets the value of the pre-set ac voltage type.
            actualACVoltage = acChannel.getRepresentation();
            runView.acValueLabel.Text = actualACVoltage.ToString("0.0");
            actualDCVoltage = dcChannel.getRepresentation();
            runView.dcValueLabel.Text = dcVal.ToString("0.0");
            actualImpulseVoltage = impChannel.getRepresentation();
            runView.impulseValueLabel.Text = actualImpulseVoltage.ToString("0.0");
           
            // Set the regulation value
            if (runView.voltageComboBox.SetSelected == "AC") actualTestVoltage = Convert.ToDouble(runView.acValueLabel.Text);
            else if (runView.voltageComboBox.SetSelected == "DC") actualTestVoltage = Convert.ToDouble(runView.dcValueLabel.Text);
            else if (runView.voltageComboBox.SetSelected == "Imp") actualTestVoltage = Convert.ToDouble(runView.impulseValueLabel.Text);

            // Update the max value if Disruptive Discharge test
            if (!testIsWithstand)
            {
                runView.elapsedTimeLabel.Text = sampleRate.ToString();
                if(actualTestVoltage > actualTestVoltageMax)
                {
                    actualTestVoltageMax = actualTestVoltage;
                    runView.resultTestVoltageValueLabel.Text = actualTestVoltage.ToString();
                }
                    
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

        // The timed event where everything is updated
        private void sampleTimer_Tick(object sender, EventArgs e)
        {

            // Update the chart
            if((!flashoverDetected)&&(!parking)&&(PIO1.K2Closed)) UpdateChart();

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
            isRunning = false;
            isPaused = false;
            parking = false;
            flashoverDetected = false;
            abortRegulation = false;
            aborting = false;

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
                // Impulse disruptive discharge



                // Get levels info
                impulseLevels = (int)runView.impulseVoltageLevelsTextBox.Value;

                // Get impulses/level info
                impulsePerLevel = (int)runView.impPerLevelTextBox.Value;

                // Get min/max levels

                // Create level array

                // Run impulse disruptive routine
            }
            else
            {
                
                // AC Withstand, DC Withstand: Run up to testVoltage, hold for duration, then park. Quit if flashover (voltage outside of bounds).
                // AC Disruptive, DC Disruptive discharge: testVoltage = maxVoltage, duration = 1s, trafSpeed = slow(180). 
                // Run up to testVoltage. Quit if flashover (inCurrent spikes).
                // Note last voltage before flashover

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
            }
            

            

            // Clear the arraylists and reset the sample counter
            xList.Clear();
            yList.Clear();
            sampleNumber = 0;

            // Connect the power
            PIO1.closePrimary();
            Thread.Sleep(2000);
            PIO1.closeSecondary();

            return true;

        }

        private void RunACDCDisruptive()
        {
            throw new NotImplementedException();
        }


        // Automated voltage set routine
        public void GoToVoltageAuto(double valueIn)
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

            voltageType = runView.voltageComboBox.SetSelected;
                
            // Set tolerances
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
                return;
            }
            Thread.Sleep(500);
            // Continue until found or precess aborted 
            while (((error < toleranceLo) || (error > toleranceHi)) && (runView.onOffAutoButton.isChecked) && (!abortRegulation))
            {
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

                        if ((error <= 3) && (error >= -3))
                        {
                            styr = 57 + (int)integral;
                        }
                        else
                        {
                            styr = (int)((error * -k) + 60 + integral);
                        }
                        // Voltage low, increase
                        if (!testIsWithstand) styr = 120;
                        PIO1.increaseVoltage(styr);
                    }
                    else if (error > toleranceLo)
                    {

                        if ((error <= 3) && (error >= -3))
                        {
                            styr = 57 + (int)integral;
                        }
                        else
                        {
                            styr = (int)((error * k) + 60 + integral);
                        }
                        // Voltage high, decrease
                        PIO1.decreaseVoltage(styr);
                    }

                    Thread.Sleep(10);
                    previousError = error;
                }
                //else
                //{F
                //    // Stop transformer
                //    PIO1.StopTransformerMotor();
                   
                //    // Try to close K2 by opening K1 to refresh
                //    PIO1.openSecondary();
                //    PIO1.openPrimary();
                //    Thread.Sleep(1000);
          
                //    // Then attempt to close again
                //    PIO1.closePrimary();
                //    Thread.Sleep(1000);
                //    PIO1.closeSecondary();
                //    Thread.Sleep(1000);

                //    //// If it worked
                //    //if (PIO1.K2Closed)
                //    //{
                //    //    // Restart sampling
                //    //    sampleTimer.Start();
                //    //}
                //    //// If it didn't work
                //    //else
                //    //{
                //    //    // Give up
                //    //    AbortTest();
                //    //    CleanUpAfterTest();
                //    //}
                //}
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
