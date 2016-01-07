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
        // Source for the actual voltage value
        RunView runView;
        
        // Experiment variables
        public string testType;
        public string voltageType;
        public int duration;
        public double testVoltage;
        public int impulseLevels;
        public int impulsePerLevel;
        public string runStatus;
        public int sampleRate = 500;

        // Report variables
        public DateTime testDate;
        public string testOperatorName = "";
        public string testObjectName = "";
        public string testOtherInfo = "";

        // Chart line value holders
        double[] xArray;
        double[] yArray;

        // Contructor
        public AutoTest(RunView runViewIn)
        {
            runView = runViewIn;
        }

        // Sets the array sizes according to the user defined test parameters
        public void ResizeArrays()
        {

        }
    }
}
