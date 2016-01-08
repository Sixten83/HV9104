using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer =  System.Windows.Forms.Timer;

namespace HV9104_GUI
{
    public class AutoTest
    {
        // Source for the actual values + controls
        RunView runView;
        NA9739Device PIO1;
        PD1161Device HV9126;
        PD1161Device HV9133;

        // Timer
        Timer updateTimer;
        
        // 
        public DateTime startTime;
        public DateTime currentTime;
        private DateTime pauseTime;
        public TimeSpan elapsedTime;

        // Experiment variables
        public string testType;
        public string voltageType;
        public int duration = 60;
        public double testVoltage;
        public double actualVoltage;
        public double toleranceHigh;
        public double toleranceLow;

        public int impulseLevels;
        public int impulsePerLevel;
        public bool isPaused;
        public double sampleRate = 0.5;

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


        // Contructor
        public AutoTest(RunView runViewIn, NA9739Device PIO1In, PD1161Device HV9126In, PD1161Device HV9133In)
        {
            runView = runViewIn;
            PIO1 = PIO1In;
            HV9126 = HV9126In;
            HV9133 = HV9133In;
            updateTimer = new Timer();
            updateTimer.Tick += new EventHandler(this.updateTimer_Tick);
        }

        // The timed event where everything is updated
        private void updateTimer_Tick(object sender, EventArgs e)
        {
            // Get latest value
            actualVoltage = Convert.ToDouble(runView.acValueLabel.Text);

            // In bounds
            if ((actualVoltage < maxTestVoltage) && (actualVoltage > minTestVoltage))
            {
                // First time here, set flag and start timer
                if (!isRunning)
                {
                    isRunning = true;
                    startTime = DateTime.Now;
                }
                else
                {
                    // Returning loop, normal test in progress
                    currentTime = DateTime.Now;
                    elapsedTime = currentTime - startTime;
                    runView.testDurationLabel.Text = elapsedTime.Seconds.ToString();

                    if (elapsedTime.Seconds >= duration)
                    {
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
            if((isRunning) && (actualVoltage > maxTestVoltage))
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
                PassTest();
            }

            // 
            runView.testStatusLabel.Visible = isRunning;
        }

        // Successfully completed test
        private void PassTest()
        {
            updateTimer.Stop();
            isRunning = false;
            isPaused = false;
            parking = false;

            // Present the status
            runView.testStatusLabel.Text = "PASS";
        }

        // Failed test
        private void FailTest()
        {
            updateTimer.Stop();
            isPaused = false;
            runView.onOffAutoButton.isChecked = false;

            // Drive the voltage down to zero
            PIO1.ParkTransformer();

            // Present the status
            runView.testStatusLabel.Text = "FAIL";
        }

        // Aborted test. Stop the test and reset
        internal void AbortTest()
        {
            updateTimer.Stop();
            isPaused = false;
            runView.onOffAutoButton.isChecked = false;

            // Drive the voltage down to zero
            PIO1.ParkTransformer();

            // Present the status
            runView.testStatusLabel.Text = "VOID";
        }

        // Run the test
        public void StartTest()
        {
            double msDuration;
            int requiredElements;
            double intSampleRate;
            int msSampleRate;

            // Get user input and convert to usable values
            duration = Convert.ToInt32(runView.testDurationTextBox.Text);
            sampleRate = Convert.ToDouble(runView.sampleRateTextBox.Text);
            intSampleRate = sampleRate * 1000;
            msSampleRate = Convert.ToInt32(intSampleRate);

            // Set up timer parameters and start
            updateTimer.Interval = msSampleRate;
            updateTimer.Enabled = true;
            updateTimer.Start();

            // Set up test voltage variables
            testVoltage = Convert.ToDouble(runView.testVoltageTextBox.Text);
            maxTestVoltage = testVoltage + toleranceHigh;
            minTestVoltage = testVoltage + toleranceLow;

            // Sample rate[ms], test duration[s]. How many elements required?
            msDuration = duration * 1000;
            requiredElements = (int)(sampleRate * msDuration);

            // Declare the new arrays
            xArray = new double[requiredElements];
            yArray = new double[requiredElements];
        }

        // Temporarily stop the timer
        internal void PauseTest()
        {
            // If we haven't found the voltage yet, stop searching
            if (!isRunning)
            {
                updateTimer.Stop();
                isPaused = true;
            }
            else
            {
                // Do not allow a pause (user must abort)
                runView.onOffAutoButton.isChecked = true;
            }

        }
    }
}
