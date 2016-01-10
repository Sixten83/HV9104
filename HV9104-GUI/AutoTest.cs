using System;
using System.Collections;
using System.Collections.Generic;
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
        Timer acDcSampleTimer;
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
        private double actualACVoltage;
        private double actualDCVoltage;
        private double actualImpVoltage;

        // Experiment variables
        public string testType;
        public string voltageType;
        public int voltageTypeIndex = 0;
        public string measurementType;
        public int duration = 60;
        public double testVoltage;
        public double actualVoltage;
        public double toleranceHigh;
        public double toleranceLow;

        ArrayList list = new ArrayList();
        private Timer updateValueTimer;
        

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
        private double maxTestVoltage;
        private double minTestVoltage;
        private bool parking;
        private bool abortRegulation;
        private double tolerance;
        private int sampleNumber;
        ArrayList xList = new ArrayList();
        ArrayList yList = new ArrayList();

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

            acDcSampleTimer = new Timer();
            acDcSampleTimer.Tick += new EventHandler(this.updateTimer_Tick);

            updateValueTimer = new Timer();
            updateValueTimer.Tick += new EventHandler(updateValueTimer_Tick);
            updateValueTimer.Interval = 10;
            updateValueTimer.Enabled = true;
            updateValueTimer.Start();

            // Update the chart
            xList.Add((double)0);
            yList.Add((double)0);
            UpdateChart();
        }

        private void updateValueTimer_Tick(object sender, EventArgs e)
        {

            // Gets the value of the pre-set ac voltage type. 
            runView.acValueLabel.Text = acChannel.getRepresentation().ToString("0.0");
            runView.dcValueLabel.Text = dcChannel.getRepresentation().ToString("0.0");
            runView.impulseValueLabel.Text = impChannel.getRepresentation().ToString("0.0");
           
            // Set the regulation value
            if (runView.voltageComboBox.SetSelected == "AC") actualVoltage = Convert.ToDouble(runView.acValueLabel.Text);
            else if (runView.voltageComboBox.SetSelected == "DC") actualVoltage = Convert.ToDouble(runView.dcValueLabel.Text);
            else if (runView.voltageComboBox.SetSelected == "Imp") actualVoltage = Convert.ToDouble(runView.impulseValueLabel.Text);


        }

        // The timed event where everything is updated
        private void updateTimer_Tick(object sender, EventArgs e)
        {

            // Update the chart
            UpdateChart();

            // In bounds
            if ((actualVoltage < maxTestVoltage) && (actualVoltage > minTestVoltage))
            {
                // First time here
                if (!isRunning)
                {
                    // Set flag and start timer
                    isRunning = true;
                    startTime = DateTime.Now;
                    runView.testStatusLabel.Text = "EVALUATING" + System.Environment.NewLine + "PLEASE WAIT";
                }
                else
                {
                    // Returning loop, normal test in progress
                    currentTime = DateTime.Now;
                    elapsedTime = currentTime - startTime;
                    runView.testDurationLabel.Text = elapsedTime.Seconds.ToString();

                    // Time is up!
                    if (elapsedTime.Seconds > duration)
                    {
                        // Stop updating the timer
                        runView.testDurationLabel.Text = duration.ToString();

                        // Present the status
                        runView.testStatusLabel.Visible = false;
                        runView.passFailLabel.Text = "PASS";
                        runView.passFailLabel.Visible = true;

                        // First park the transformer before we stop taking results
                        if (!PIO1.minUPos)
                        {
                            if (!parking)
                            {
                                PIO1.ParkTransformer();
                                parking = true;
                            }
                        }
                    }                
                }
            }

            // We are running the test but the voltage is too high
            if ((isRunning) && (actualVoltage > maxTestVoltage))
            {
                // Abort the test
                AbortTest();
            }
            // We are running the test but the voltage is too low
            else if ((isRunning) && (actualVoltage < minTestVoltage) && (!parking))
            {
                // Have we strayed or is it a flashover?
                if (actualVoltage > 5)
                {
                    // Strayed low, abort the test
                    AbortTest();
                }
                else
                {
                    // 
                    FailTest();
                }              
            }

            // Finished successfully, and parked.
            if ((parking) && (PIO1.minUPos))
            {
                // Success!
                Thread.Sleep(1000);
                // Update the chart
                UpdateChart();
                PassTest();
            }            
        }

        // Successfully completed test
        private void PassTest()
        {
            acDcSampleTimer.Stop();
            isRunning = false;
            isPaused = false;
            parking = false;

            // Disconnect the power
            PIO1.openPrimary();

            runView.onOffAutoButton.isChecked = false;
            runView.onOffAutoButton.Invalidate();

            // Update the chart
            Thread.Sleep(2000);
            UpdateChart();

        }

        // Failed test
        private void FailTest()
        {
            acDcSampleTimer.Stop();
            isPaused = false;
            runView.onOffAutoButton.isChecked = false;
            runView.onOffAutoButton.Invalidate();

            // Disconnect the power
            PIO1.openPrimary();

            // Drive the voltage down to zero
            PIO1.ParkTransformer();

            // Present the status
            runView.passFailLabel.Text = "FAIL";
            runView.passFailLabel.Visible = true;
            runView.testStatusLabel.Visible = false;
            runView.testStatusLabel.Invalidate();
        }

        // Aborted test. Stop the test and reset
        internal void AbortTest()
        {
            acDcSampleTimer.Stop();
            isRunning = false;
            isPaused = false;
            runView.onOffAutoButton.isChecked = false;
            runView.onOffAutoButton.Invalidate();

            // Disconnect the power
            PIO1.openPrimary();

            Thread.Sleep(200);

            // Drive the voltage down to zero
            PIO1.ParkTransformer();

            // Present the status
            runView.passFailLabel.Text = "VOID";
            runView.passFailLabel.Visible = true;
            runView.testStatusLabel.Visible = false;
            runView.testStatusLabel.Invalidate();
        }

        // Run the test
        public bool StartTest()
        {
            double msDuration;
            int requiredElements;
            double intSampleRate;
            int msSampleRate;

            // Clear previous flags
            abortRegulation = false;

            runView.testStatusLabel.Visible = true;
            runView.passFailLabel.Visible = false;
            runView.testStatusLabel.Invalidate();
            runView.passFailLabel.Invalidate();
            runView.testDurationLabel.Text = "0";

            // Get test type
            if (runView.WithstandRadioButton.isChecked)
            {
                // Withstand test. Run normal routine
               
                // Get user input and convert to usable values
                duration = Convert.ToInt32(runView.testDurationTextBox.Value);
                sampleRate = Convert.ToDouble(runView.sampleRateTextBox.Value);
                intSampleRate = sampleRate * 1000;
                msSampleRate = Convert.ToInt32(intSampleRate);

                // Set up timer parameters and start
                acDcSampleTimer.Interval = msSampleRate;
                acDcSampleTimer.Enabled = true;
                acDcSampleTimer.Start();

                // Get test voltage parameters and set tolerances
                testVoltage = Convert.ToDouble(runView.testVoltageTextBox.Value);
                tolerance = testVoltage * (runView.toleranceTextBox.Value / 100);
                maxTestVoltage = testVoltage + tolerance;
                minTestVoltage = testVoltage - tolerance;

            }
            else
            {
                // Disruptive discharge
                if(runView.voltageComboBox.SetSelected == "Imp")
                {
                    // Impulse disruptive discharge

                    // Get setup info 1, 2, 3 stage. 

                    // Set Max value

                    // Get levels info

                    // Get impulses/level info

                    // Create level array

                    // Run impulse disruptive routine
                }
                else
                {
                    // AC/DC disruptive discharge.

                    // Get setup info 1, 2, 3 stage. 

                    // Get user input and convert to usable values
                    duration = 0;
                    sampleRate = Convert.ToDouble(runView.sampleRateTextBox.Value);
                    intSampleRate = sampleRate * 1000;
                    msSampleRate = Convert.ToInt32(intSampleRate);

                    // Set up timer parameters and start
                    acDcSampleTimer.Interval = msSampleRate;
                    acDcSampleTimer.Enabled = true;
                    acDcSampleTimer.Start();

                    // Set targetVoltage to Stage Max
                    testVoltage = Convert.ToDouble(runView.testVoltageTextBox.Value);       // set to x-Stage max when disruptive discharge selected
                    tolerance = testVoltage * (runView.toleranceTextBox.Value / 100);
                    maxTestVoltage = testVoltage + tolerance;
                    minTestVoltage = testVoltage - tolerance;

                    // Set trafSpeed to quite slow

                    // Run normal routine

                }

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

        // Temporarily stop the timer
        internal void PauseTest()
        {
            // If we haven't found the voltage yet, stop searching
            if (!isRunning)
            {
                acDcSampleTimer.Stop();
                isPaused = true;
                abortRegulation = true;
                PIO1.openSecondary();
            }
            else
            {
                // Do not allow a pause (user must abort)
                runView.onOffAutoButton.isChecked = true;
            }

        }

        // Automated voltage set routine
        public void GoToVoltageAuto(double valueIn)
        {
            
            Thread regUAutoThread = new Thread(RegulateVoltage);
            regUAutoThread.Start();
            runView.testStatusLabel.Text = "SEARCHING" + Environment.NewLine + "PLEASE WAIT";
            runView.testStatusLabel.Visible = true;
            runView.testStatusLabel.Invalidate();
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

            // Continue until found or precess aborted 
            while (((error <= toleranceLo) || (error >= toleranceHi)) && (runView.onOffAutoButton.isChecked) && (!abortRegulation))
            {
                // Run only if we have connected the voltage
                if (PIO1.K2Closed) { 

                    error = actualVoltage - targetVoltage;

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
                    else
                    {
                        // In bounds. We should only make it here once
                        PIO1.StopTransformerMotor();
                    }

                    Thread.Sleep(10);
                    previousError = error;
                }
            }

            // In bounds. We should only make it here once
            PIO1.StopTransformerMotor();
            abortRegulation = true;
        }

        internal void UpdateChart()
        {
            xList.Add((double)sampleNumber);
            yList.Add((double)actualVoltage);

            sampleNumber += 1;
            xArray = (Double[])xList.ToArray(typeof(double));
            yArray = (Double[])yList.ToArray(typeof(double));
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
    }
}
