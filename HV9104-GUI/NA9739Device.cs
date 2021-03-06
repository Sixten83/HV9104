﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Timers;

namespace HV9104_GUI
{
    public class NA9739Device
    {
        // Intermediery variables
        public int duty = 800;
        public double targetPressure = 0;
        public double pressureSensorValue = 0;
        public double vacuumSensorValue = 0;
        public double regulatedVoltageValue = 0;
        public double regulatedCurrentValue = 0;

        // Transmission package variables
        public string address;
        public int regSize = 4;
        public int index = 0;
        public int csOK = 0;
        public int comSleep = 0;
        public bool timeout = false;
        public bool fullMessage = false;
        public int span;
        DateTime startMe;
        DateTime stopMe;
        System.IO.Ports.SerialPort comport;

        // Flags (Have corresponding Bool variable in PIO) - Write access
        public bool regUEnable = false;                 // false = decrease, true = increase
        public bool regUDir = false;
        public bool primaryOnRequest = false;
        public bool secondaryOnRequest = false;
        public bool overrideUMin = false;
        public bool primaryOffRequest = false;
        public bool secondaryOffRequest = false;
        public bool activateVacuumAndPressure = false;

        // Flags (Have corresponding Bool variable in PIO) - mostly Read access
        public bool vacuumSelected = false;
        public bool startCompressor = false;
        public bool startVacuumPump = false;
        public bool minUPos = false;
        public bool maxUPos = false;
        public bool commDetected = false;
        public bool earthingEngaged = false;
        public bool earthingDisengaged = false;

        // Flags Have corresponding Bool variable in PIO) - mostly Read access
        public bool fault = false;                  // W
        public bool clearFault = false;             // W
        public bool dischargeRodParked = false;     // R
        public bool emergStpKeySwClosed = false;    // R
        public bool dorrSwitchClosed = false;       // R
        public bool f2Closed = false;               // R
        public bool f1Closed = false;               // R
        public bool K1Closed = false;               // R
        public bool K2Closed = false;               // R


        // Flag arrays for coordination - usage: flagsLo = BitConverter.GetBytes(flagArray1); 
        public bool[] flagArray1 = new bool[8];
        public bool[] flagArray2 = new bool[8];
        public bool[] flagArray3 = new bool[8];
        public bool[] flagArray4 = new bool[8];

        //Message arrays
        public byte[] writeBuf = { 0x01, 0x10, 0x40, 0x00, 0x00, 0x03, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };  //{ address, fc, startRegHi, startRegLo, totRegHi, totRegLo, totBytes, flagsHi, flagsLo, dutyHi, dutyLo, targetPressureHi, targetPressureLo, checkSumHi, checkSumLo };
        public byte[] writeConfBuf = new byte[8];
        public byte[] queryBuf = { 0x01, 0x03, 0x40, 0x03, 0x00, 0x05, 0x60, 0x09 };
        public byte[] answerBuf = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        //public Byte[] readBuf = new Byte[];
        public Byte[] xmtBuf = new byte[15];
        public Byte[] rcvBuf = new Byte[12];

        // Timer to reset flags (pulse trigger in PLC)
        System.Timers.Timer T1 = new System.Timers.Timer(700);

        // Constructor initiates address and serial port
        public NA9739Device(System.IO.Ports.SerialPort comportIn)
        {
            // Update local status flags
            address = "1";
            comport = comportIn;

            // timer to reset flags
            T1.Elapsed += OnTimedEvent;
            T1.AutoReset = true;
            T1.Enabled = true;

            // Test communication
        }

        // Write routine
        public void UpdateDevice()
        {
            // Set the variable values 
            SetFlags();
            SetDuty();
            SetTargetPressure();

            //Calculate and insert the CRC checksum
            AppendCRC(writeBuf, writeBuf.Length);

            // Send it
            WriteToDevice();

            //Array.Resize(ref writeConfBuf, (comport.BytesToRead));
            //comport.Read(writeConfBuf, 0, comport.BytesToRead);
            //string h = "";
        }

        // Insert autotraf motor speed into transmission array. Duty range is 0-1000, representing 0.0% - 100.0% (0x0 - 0x3E8)
        public void SetDuty()
        {
            // convert int to bytes
            byte[] dutyBytes = BitConverter.GetBytes(duty);
            writeBuf[9] = dutyBytes[1];
            writeBuf[10] = dutyBytes[0];
        }

        // Insert Target Pressure into transmission array. Pressure range is -1 to 6 bar, ST3424 AI(0V = 0x0, 2V = 0x0333, 5V = 0x07FF, 10V = 0xFFFF = 6Bar = -1Bar)
        public void SetTargetPressure()
        {
            double fraction = 0;
            UInt16 tpOutput = 0;

            // Set Vacuum/Pressure Flag
            if (targetPressure >= 0)
            {
                // Pressure selected
                vacuumSelected = false;
                fraction = targetPressure / 6;  // 6Bar = 0x0FFF
            }
            else
            {
                // Vacuum selected
                vacuumSelected = true;
                fraction = Convert.ToSingle(targetPressure) * -1;  // 6Bar = 0x0FFF
            }

            //Convert target pressure into value 0 - 65535 for 0 to 6Bar, or for 0 to (-)1Bar;
            tpOutput = Convert.ToUInt16(fraction * 0x0FFF);

            // convert int to bytes
            byte[] dutyBytes = BitConverter.GetBytes(tpOutput);
            writeBuf[11] = dutyBytes[1];
            writeBuf[12] = dutyBytes[0];
        }

        // Get the actual flag values, convert to 2 Bytes and insert into transmission array  
        public void SetFlags()
        {
            flagArray1[0] = regUEnable;             // W
            flagArray1[1] = regUDir;                // W
            flagArray1[2] = primaryOnRequest;       // W
            flagArray1[3] = secondaryOnRequest;     // W
            flagArray1[4] = overrideUMin;           // W
            flagArray1[5] = primaryOffRequest;      // W                       
            flagArray1[6] = secondaryOffRequest;    // W
            //flagArray1[7] =                       // W

            flagArray2[0] = activateVacuumAndPressure; // W
            flagArray2[1] = vacuumSelected;         // W
            flagArray2[2] = startCompressor;        // W
            flagArray2[3] = startVacuumPump;        // W
            //flagArray2[4] =                       // W
            //flagArray2[5] =                       // W
            flagArray2[6] = commDetected;           // W
            flagArray2[7] = clearFault;             // W

            //flagArray3[0] = fault;                  // R
            //flagArray3[1] = earthingEngaged;        // R
            //flagArray3[2] = dischargeRodParked;     // R
            //flagArray3[3] = emergStpKeySwClosed;    // R
            //flagArray3[4] = dorrSwitchClosed;       // R
            //flagArray3[5] = f2Closed;               // R
            //flagArray3[6] = K1Closed;               // R
            //flagArray3[7] = K2Closed;

            //flagArray4[0] = minUPos;                // R 
            //flagArray4[1] = maxUPos;                // R
            //flagArray4[2] = false;                      // 
            //flagArray4[3] = false;                      // 
            //flagArray4[4] = false;                      // 
            //flagArray4[5] = false;                      // 
            //flagArray4[6] = false;                      // 
            //flagArray4[7] = false;                      // 

            // Convert all flags to Bytes and enter in transmission array
            byte hiByte = 0;
            byte loByte = 0;

            for (int i = 7; i >= 0; i--)
            {
                loByte <<= 1;
                if (flagArray1[i]) loByte |= 1;
                hiByte <<= 1;
                if (flagArray2[i]) hiByte |= 1;
            }
            writeBuf[7] = hiByte;
            writeBuf[8] = loByte;
        }

        // Write values to device
        public bool WriteToDevice()
        {
            string[] str = { "", "" };
            fullMessage = false;
            timeout = false;

            try
            {
                comport.Write(writeBuf, 0, writeBuf.Length);
                System.Threading.Thread.Sleep(17);
                startMe = DateTime.Now;

            }
            catch (Exception ex)
            {
                // MessageBox.Show(("Write to device at address " + (int.Parse(xmtBuf[0]) + (" failed." + ("\r\n" + "Check physical connections. Check parameters.")))), "Write error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            // Wait here for full message or timeout
            while (!fullMessage && !timeout)
            {
                stopMe = DateTime.Now;
                span = stopMe.Subtract(startMe).Milliseconds;
                if ((span > comSleep))
                {
                    // waited long enough, move on
                    timeout = true;
                }
            }

            // Clear the buffer for next attempt
            Array.Resize(ref writeConfBuf, (comport.BytesToRead));

            try
            {
                // Read the in buffer 
                comport.Read(writeConfBuf, 0, comport.BytesToRead);
                // Validate the response
                if ((writeConfBuf[0] == writeBuf[0]) && (writeConfBuf[1] == writeBuf[1]) && (writeConfBuf[2] == writeBuf[2]) && (writeConfBuf[3] == writeBuf[3]) && (writeConfBuf[4] == writeBuf[4]) && (writeConfBuf[5] == writeBuf[5]))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show("No answer on bus at address " & Integer.Parse(xmtBuf(0)) & ".", "Read error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                return false;
            }
            return true;
        }

        // Retreive the vacuum and pressure sensor values 
        public bool ReadFromDevice()
        {
            // Create the read message. 6 registers read from address 0x4000.
            string[] str = { "", "" };
            fullMessage = false;
            timeout = false;

            try
            {
                // Write to device
                comport.Write(queryBuf, 0, queryBuf.Length);
                startMe = DateTime.Now;
                System.Threading.Thread.Sleep(16);
            }
            catch (Exception ex)
            {
                // MessageBox.Show(("Write to device at address " + (int.Parse(xmtBuf[0]) + (" failed." + ("\r\n" + "Check physical connections. Check parameters.")))), "Write error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            // Wait here for full message or timeout
            while (!fullMessage && !timeout)
            {
                if ((span > comSleep))
                {
                    // waited long enough, move on
                    timeout = true;
                }
                //stopMe = DateTime.Now;
                span = DateTime.Now.Subtract(startMe).Milliseconds;
            }



            try
            {
                // Clear the buffer for next attempt
                Array.Resize(ref answerBuf, (comport.BytesToRead));
                // Read from the buffer
                comport.Read(answerBuf, 0, answerBuf.Length);
            }
            catch (Exception ex)
            {
                // MessageBox.Show("No answer on bus at address " & Integer.Parse(xmtBuf(0)) & ".", "Read error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                return false;
            }

            // Validate reply
            if (ChecksumOK())
            {
                FormatReply();
                csOK = (csOK + 1);
                return true;
            }
            else
            {
                return false;
            }
        }

        // After successful ReadFromDevice, extract raw data, format into readable values and update the appropriate variables
        public void FormatReply()
        {
            if (answerBuf.Length > 8)
            {
                // Flags, convert to bool array and set the writeable flags answerBuf[4] 
                byte bArray0 = answerBuf[8];
                byte bArray1 = answerBuf[7];
                //byte bArray2 = answerBuf[6];
                //byte bArray3 = answerBuf[5];

                bool[] bits1 = new bool[8];
                bool[] bits2 = new bool[8];
                //bool[] bits3 = new bool[8];
                //bool[] bits4 = new bool[8];

                for (int bIndx = 0; bIndx <= 7; bIndx++)
                {
                    bits1[bIndx] = (bArray0 & (1 << bIndx - 1)) != 0;
                    bits2[bIndx] = (bArray1 & (1 << bIndx - 1)) != 0;
                    //bits3[bIndx] = (bArray2 & (1 << bIndx - 1)) != 0;
                    //bits4[bIndx] = (bArray3 & (1 << bIndx - 1)) != 0;
                }

                // Most of these not to be used here. To be set from regulation routines in Controller Class. 
                // Maybe add PIO status variables to hold these for comparison?

                fault = bits1[0];                       // R
                earthingEngaged = bits1[1];             // R
                dischargeRodParked = bits1[2];          // R
                emergStpKeySwClosed = bits1[3];         // R
                dorrSwitchClosed = bits1[4];            // R
                f2Closed = bits1[5];                    // R
                K1Closed = bits1[6];                    // R
                K2Closed = bits1[7];                    // R

                minUPos = bits2[0];                     // R
                maxUPos = bits2[1];                     // R
                //unused = bits2[2];                    // R
                //unused = bits2[3];                    // R
                //unused = bits2[4];                    // R
                //unused = bits2[5];                    // R
                //unused = bits2[6];                    // R
                //unused = bits2[7];                    // R

                //regUEnable = bits3[0];                // W
                //regUDir = bits3[1];                   // W
                //primaryOnRequest = bits3[2];          // W
                //secondaryOnRequest = bits3[3];        // W
                //overrideUMin = bits3[4];              // W
                //primaryOffRequest = bits3[5];         // W
                //secondaryOffRequest = bits3[6];       // W
                //unused = bits3[7];                    // W

                //activateVacuumAndPressure = bits[0];  // W
                //vacuumSelected = bits4[1];            // W
                //startCompressor = bits4[2];           // W
                //startVacuumPump = bits4[3];           // W
                //unused = bits4[4];                    // W
                //unused = bits4[5];                    // W
                //commDetected = bits4[6];              // W
                //clearFault = bits4[7];                // W

                // Duty cycle value HiByte and LoByte
                //byte[] bArray4 = { answerBuf[8], answerBuf[7] };

                // Vacuum and Pressure extraction
                double vacPressLimit = 0;
                // Are we extracting vacuum or pressure?
                if (vacuumSelected)
                {
                    vacPressLimit = -1;
                }
                else
                {
                    vacPressLimit = 6;
                }

                // Test values - can be erased after final testing
                //answerBuf[12] = 0xFF;   //0x07FF = 5V = 50%
                //answerBuf[11] = 0x07;
                //answerBuf[14] = 0x33;   //0x0333 = 2V = 20%
                //answerBuf[13] = 0x03;

                // VacuumPressure setpoint value HiByte and LoByte 
                //byte[] bArray5 = { answerBuf[10], answerBuf[9] };
                //short vPSetpoint = BitConverter.ToInt16(bArray5, 0);
                //double formattedVPSetpoint = (double)vPSetpoint / (double)0x0FFF;
                //double vacuumPressureSetpoint = Math.Round(formattedVPSetpoint * vacPressLimit, 1, MidpointRounding.ToEven);

                // Pressure sensor value HiByte and LoByte mashed, converted to double and rounded to 1 d.p.
                byte[] bArray6 = { answerBuf[4], answerBuf[3] };
                short pSensRaw = BitConverter.ToInt16(bArray6, 0);
                double formattedpSensRaw = (double)pSensRaw / (double)0x0FFF;
                pressureSensorValue = Math.Round(formattedpSensRaw * 6, 1, MidpointRounding.ToEven);

                // Vacuum sensor value HiByte and LoByte mashed, converted to double and rounded to 1 d.p. 
                byte[] bArray7 = { answerBuf[6], answerBuf[5] };
                short vSensRaw = BitConverter.ToInt16(bArray7, 0);
                double formattedvSensRaw = (double)vSensRaw / (double)0x0FFF;
                vacuumSensorValue = Math.Round(formattedvSensRaw * -1, 1, MidpointRounding.ToEven);

                // Regulated Voltage value HiByte and LoByte mashed, converted to double and rounded to 2 d.p. 
                // y = 16,833x - 181,37
                // x = y + 181,37 / 16,833
                // volt = vltSensRaw + 181,37 / 16,833;
                byte[] bArray8 = { answerBuf[10], answerBuf[9] };
                short vltSensRaw = BitConverter.ToInt16(bArray8, 0);
                double formattedvltSensRaw = 0;

                // Need some different formatting in the beginning
                if (vltSensRaw == 0)
                {
                    formattedvltSensRaw = 0;
                }
                else if (vltSensRaw == 1)
                {
                    formattedvltSensRaw = 1.0;
                }
                else if (vltSensRaw == 2)
                {
                    formattedvltSensRaw = 5.0;
                }
                else if (vltSensRaw == 3)
                {
                    formattedvltSensRaw = 6.0;
                }
                else if (vltSensRaw == 4)
                {
                    formattedvltSensRaw = 6.5;
                }
                else if (vltSensRaw == 5)
                {
                    formattedvltSensRaw = 6.8;
                }
                else if (vltSensRaw == 6)
                {
                    formattedvltSensRaw = 7.8;
                }
                else if (vltSensRaw == 7)
                {
                    formattedvltSensRaw = 7.1;
                }
                else if (vltSensRaw == 7)
                {
                    formattedvltSensRaw = 7.1;
                }
                else if (vltSensRaw == 8)
                {
                    formattedvltSensRaw = 7.2;
                }
                else if (vltSensRaw == 9)
                {
                    formattedvltSensRaw = 7.4;
                }
                else
                {
                    formattedvltSensRaw = (vltSensRaw + 181.37) / 16.833;
                }

                regulatedVoltageValue = Math.Round(formattedvltSensRaw, 1, MidpointRounding.ToEven);

                // Regulated Current value HiByte and LoByte mashed, converted to double and rounded to 2 d.p.
                // y = 0,0085x - 0,0151
                // current = (currentSensraw * 0.0085) - 0.0151; 
                byte[] bArray9 = { answerBuf[12], answerBuf[11] };
                short currSensRaw = BitConverter.ToInt16(bArray9, 0);
                double formattedcurrSensRaw = (currSensRaw * 0.0085) - 0.0151;
                regulatedCurrentValue = Math.Round(formattedcurrSensRaw, 2, MidpointRounding.ToEven);
            }
        }

        // Validate received CRC
        public bool ChecksumOK()
        {
            try
            {
                // Validate reply
                if (answerBuf.Length > 3)
                {
                    //Calculate new checksum from reply values & Swap the checksum byte order to be able to compare
                    string rawChecksum = ModRTU_CRC(answerBuf, (answerBuf.Length - 2)).ToString("X4");
                    string calculatedChecksum = (rawChecksum.Substring(2, 2) + (" " + rawChecksum.Substring(0, 2)));

                    // Retreive the checksum in the reply
                    string receivedChecksum = (answerBuf[answerBuf.Length - 2].ToString("X2") + (" " + answerBuf[answerBuf.Length - 1].ToString("X2")));

                    if ((calculatedChecksum == receivedChecksum))
                    {
                        return true;
                    }
                    else
                    {
                        // Invalid data
                        return false;
                    }
                }
                else
                {
                    // No data
                    return false;
                }
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        // Append the CRC
        public Byte[] AppendCRC(Byte[] inBuf, int inBufLen)
        {
            //Calculate CRC - calls local function CRC
            UInt16 strCRC = ModRTU_CRC(inBuf, inBufLen - 2);
            byte[] crcBytes = BitConverter.GetBytes(strCRC);

            //Split/transform and insert into array
            inBuf[inBufLen - 1] = crcBytes[1];
            inBuf[inBufLen - 2] = crcBytes[0];

            return inBuf;
        }

        // Calculate the MODBUS RTU CRC
        UInt16 ModRTU_CRC(byte[] buf, int len)
        {
            UInt16 crc = 0xFFFF;
            for (int pos = 0; pos < len; pos++)
            {
                crc ^= (UInt16)buf[pos];          // XOR byte into least sig. byte of crc
                for (int i = 8; i != 0; i--)
                {    // Loop over each bit
                    if ((crc & 0x0001) != 0)
                    {      // If the LSB is set
                        crc >>= 1;                    // Shift right and XOR 0xA001
                        crc ^= 0xA001;
                    }
                    else                            // Else LSB is not set
                        crc >>= 1;                    // Just shift right
                }
            }
            // Note, this number has low and high bytes swapped, so use it accordingly (or swap bytes)
            return crc;
        }

        // Increase the voltage output
        internal void increaseVoltage(int speedIn)
        {
            // Set REG_U_DIR to increase
            regUDir = true;

            // Connect motor
            regUEnable = true;

            // Set speed
            duty = speedIn;

            // Prepare array
            SetFlags();
            SetDuty();
            AppendCRC(writeBuf, writeBuf.Length);

            // Send instruction to device
            WriteToDevice();
        }

        // Increase the voltage output
        internal void decreaseVoltage(int speedIn)
        {
            // Set REG_U_DIR to increase
            regUDir = false;

            // Connect motor
            regUEnable = true;

            // Set speed
            duty = speedIn;

            // Prepare array
            SetFlags();
            SetDuty();
            AppendCRC(writeBuf, writeBuf.Length);

            // Send instruction to device
            WriteToDevice();
        }

        // Stop the transformer motor
        internal void StopTransformerMotor()
        {
            // Disconnect motor - Set REG_U_ENABLE to true
            regUEnable = false;

            // Set speed to 0
            duty = 0;

            // Prepare array
            SetFlags();
            SetDuty();
            AppendCRC(writeBuf, writeBuf.Length);

            // Send instruction to device
            WriteToDevice();
        }

        // Open K1
        internal void openPrimary()
        {
            // Set variable. Operates on rising edge
            primaryOffRequest = true;

            // Prepare array
            SetFlags();
            AppendCRC(writeBuf, writeBuf.Length);
            T1.Start();

        }

        // Close K1
        internal void closePrimary()
        {
            // Set variable. Operates on rising edge
            primaryOnRequest = true;

            // Prepare array
            SetFlags();
            AppendCRC(writeBuf, writeBuf.Length);
            T1.Start();
        }

        // Open K2
        internal void openSecondary()
        {
            // Set variable. Operates on rising edge
            secondaryOffRequest = true;

            // Prepare array
            SetFlags();
            AppendCRC(writeBuf, writeBuf.Length);
            T1.Start();
        }

        // Close K2
        internal void closeSecondary()
        {
            // Set variable. Operates on rising edge 
            secondaryOnRequest = true;

            // Prepare array
            SetFlags();
            AppendCRC(writeBuf, writeBuf.Length);
            T1.Start();
        }

        // After time elapsed, reset the request flags. PLC operates on positive flank only.
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            primaryOnRequest = false;
            primaryOffRequest = false;
            secondaryOnRequest = false;
            secondaryOffRequest = false;
            T1.Stop();
        }

        internal void ChangePressure(double targetPressureIn)
        {
            activateVacuumAndPressure = true;
            targetPressure = targetPressureIn + 0.05;        // Trim in the value
            SetFlags();
            SetTargetPressure();
            AppendCRC(writeBuf, writeBuf.Length);
            WriteToDevice();
        }

        // Return a Vac or Pressure value based on the value of target pressure
        // Needs to be improved!!
        internal string getPressure()
        {
            if (targetPressure >= 0)
            {
                return pressureSensorValue.ToString();
            }
            else
            {
                return vacuumSensorValue.ToString();
            }
        }

        internal void DecreasePressure()
        {
            // Prepare flags 
            targetPressure = 0;
            activateVacuumAndPressure = false;
            startVacuumPump = true;
            startCompressor = false;

            // Prepare array
            SetFlags();
            AppendCRC(writeBuf, writeBuf.Length);

            // Send instruction to device
            WriteToDevice();

            // Reset variabel 
            activateVacuumAndPressure = false;

        }

        internal void IncreasePressure()
        {
            // Reset variabel - not running automatically 
            targetPressure = 0;
            activateVacuumAndPressure = false;
            startVacuumPump = false;
            startCompressor = true;

            // Prepare array
            SetFlags();

            AppendCRC(writeBuf, writeBuf.Length);

            // Send instruction to device
            WriteToDevice();

        }

        internal void stopVacPressure()
        {
            // Reset variabel - not running automatically 
            startVacuumPump = false;
            startCompressor = false;

            // Prepare array
            SetFlags();

            AppendCRC(writeBuf, writeBuf.Length);

            // Send instruction to device
            WriteToDevice();
        }

        // Anyone out there to talk to?
        public bool CheckConnection()
        {
            ReadFromDevice();
            return true;
        }

        internal void closeVacuumValve()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

}
