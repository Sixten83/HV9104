using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;


namespace HV9104_GUI
{
    public class PD1161Device
    {
        // Initialized flag. Set after device register == 1.
        public bool initComplete = false;
        public int initCtr = 0;

        // Position variables
        int microStepValue = 0;
        bool targetPositionReached = false;
        public int actualPosition = 0;

        // Range variables        
        int maxDistance = 0;
        int maxHV9126Distance = 4654950;
        int maxHV9133Distance = 4687222;

        //Message arrays
        public Byte[] xmtBuf = new Byte[9];
        public Byte[] rcvBuf = new Byte[15];

        // Communications variables
        public int address = 0;
        public int regSize = 4;
        public int index = 0;
        public int csOK = 0;
        public int comSleep = 0;
        public bool timeout = false;
        public bool fullMessage = false;
        int span;
        DateTime startMe;
        DateTime stopMe;
        System.IO.Ports.SerialPort comport;


        // Constructor takes in address value for HV9126 or HV9133
        public PD1161Device(int addIn, System.IO.Ports.SerialPort comportIn)
        {
            // HV9126 will have address 3, HV9133 will have address 4.
            address = addIn;
            comport = comportIn;

            // Set the appropriate range
            if (address == 3) { maxDistance = maxHV9126Distance; }
            if (address == 4) { maxDistance = maxHV9133Distance; }
        }

        // Interrogate the device for Init Complete affirmation  
        public bool checkCTSFlag()
        {
            // GetGlobalParameter Read instruction. Bank 2, address 1.
            xmtBuf[0] = (byte)address;
            xmtBuf[1] = 0x0A;
            xmtBuf[2] = 0x01;
            xmtBuf[3] = 0x02;
            xmtBuf[4] = 0x00;
            xmtBuf[5] = 0x00;
            xmtBuf[6] = 0x00;
            xmtBuf[7] = 0x00;

            AppendCRC();

            // Send the message
            bool crcOK = WriteToDevice();

            // Check the reply (if any??)
            if (crcOK != true)
            {
                // No answer, let's bail
                return false;
            }

            // CTS flag is true
            if (rcvBuf[7] == 1)
            {
                initComplete = true;
                return true;
            }
            else
            {
                // CTS flag is false
                initComplete = false;

                return initComplete;
            }
        }

        // Return the actual position for presentation
        public bool getTargetPositionReached()
        {
            // GAP GetAxisParameter Read instruction. Bank ?, address ?.
            xmtBuf[0] = (byte)address;
            xmtBuf[1] = 0x06;
            xmtBuf[2] = 0x08;
            xmtBuf[3] = 0x00;
            xmtBuf[4] = 0x00;
            xmtBuf[5] = 0x00;
            xmtBuf[6] = 0x00;
            xmtBuf[7] = 0x00;

            AppendCRC();

            // Send the message
            bool crcOK = WriteToDevice();

            // Check the reply (if any??)
            if (ChecksumOK() != true)
            {
                return false;
            }

            // Extract data and convert to readable value
            byte[] bArray = { rcvBuf[4], rcvBuf[5], rcvBuf[6], rcvBuf[7] };
            targetPositionReached = BitConverter.ToBoolean(bArray, 0);

            return targetPositionReached;
        }

        // Return the actual position for presentation
        public int getActualPosition()
        {
            // GAP GetAxisParameter Read instruction. Bank ?, address ?.
            xmtBuf[0] = (byte)address;
            xmtBuf[1] = 0x06;
            xmtBuf[2] = 0x01;
            xmtBuf[3] = 0x00;
            xmtBuf[4] = 0x00;
            xmtBuf[5] = 0x00;
            xmtBuf[6] = 0x00;
            xmtBuf[7] = 0x00;

            AppendCRC();

            // Send the message
            bool crcOK = WriteToDevice();

            // Check the reply (if any??)
            if (ChecksumOK() != true)
            {
                return 0;
            }

            // Extract data and convert to readable value
            byte[] bArray = { rcvBuf[7], rcvBuf[6], rcvBuf[5], rcvBuf[4] };
            int rawPosition = System.BitConverter.ToInt32(bArray, 0);

            // Format raw value to millimeter value
            actualPosition = (maxDistance - rawPosition) / 51200;

            return actualPosition;
        }


        // Write values to device
        public bool WriteToDevice()
        {
            string[] str = { "", "" };
            fullMessage = false;
            timeout = false;

            try
            {
                //comport.DiscardOutBuffer();
                // Write to device
                comport.Write(xmtBuf, 0, xmtBuf.Length);
                startMe = DateTime.Now;
                System.Threading.Thread.Sleep(30);  //8
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
            Array.Resize(ref rcvBuf, (comport.BytesToRead));

            try
            {
                // Read the in buffer 
                comport.Read(rcvBuf, 0, comport.BytesToRead);

                // Depending on calling function, validate the reply
                if (ChecksumOK())
                {
                    // Confirm transmission
                    return true;
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show("No answer on bus at address " & Integer.Parse(xmtBuf(0)) & ".", "Read error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                return false;
            }
            return false;
        }

        // Instruct device to carry out init/calibration procedure - NEEDS ADRESSES!!
        public bool StartInit()
        {
            //Set the Global Parameter to call Init() function in device
            xmtBuf[0] = (byte)address;
            xmtBuf[1] = 0x81;                   // 0x81 or 129 = Run application
            xmtBuf[2] = 0x01;                   // 1 = Run from address, 0 = Run from actual PC
            xmtBuf[3] = 0x00;                   // Dont care
            xmtBuf[4] = 0x00;
            xmtBuf[5] = 0x00;
            xmtBuf[6] = 0x00;
            xmtBuf[7] = 0x07;                   // Address value

            AppendCRC();

            //// Send and confirm with CRC
            bool crcOK = WriteToDevice();

            // Check the reply (if any??)
            if (ChecksumOK() != true)
            {
                return false;
            }

            return true;
        }

        // Move motor to absolute position. Internal routines in motor controller fix regulation
        public bool MoveToPosition(int posSetPoint)
        {
            // No running untill initialized
            if (initComplete != true)
            {
                return false;
            }

            // Convert user input to microstep
            microStepValue = posSetPoint * 51200;
            int gap = maxDistance - microStepValue;

            // Convert int to bytes
            byte[] gapBytes = { 0x00, 0x00, 0x00, 0x00 };
            gapBytes = BitConverter.GetBytes(gap);

            // Move To Position - Send to motor (MVP ABS 0, <VALUE IN bank 2, address 5>)
            xmtBuf[0] = (byte)address;
            xmtBuf[1] = 0x04;                   // 4 = MVP Move to position
            xmtBuf[2] = 0x00;                   // 0 = Absolute, 1 = Relative.
            xmtBuf[3] = 0x00;                   // Bank 0 always
            xmtBuf[4] = gapBytes[3];
            xmtBuf[5] = gapBytes[2];
            xmtBuf[6] = gapBytes[1];
            xmtBuf[7] = gapBytes[0];

            AppendCRC();

            // Send and confirm with CRC
            bool crcOK = WriteToDevice();

            // Check the reply (if any??)
            if (ChecksumOK() != true)
            {
                return false;
            }

            return true;
        }

        // Simple routine to move motor in one direction. Called directly.
        public bool DecreaseGap()
        {
            // No running untill initialized
            if (initComplete != true)
            {
                return false;
            }

            // Send ROR instruction to motor - Alex's appplication
            xmtBuf[0] = (byte)address;
            xmtBuf[1] = 0x81;                       // 0x81 eller 129 = Run application
            xmtBuf[2] = 1;                          // Type = 1: Run from specific address 
            xmtBuf[3] = 0x00;                       // Dont care
            xmtBuf[4] = 0x00;
            xmtBuf[5] = 0x00;
            xmtBuf[6] = 0x00;
            xmtBuf[7] = 0x0F;                       // Address to run from
            AppendCRC();

            // Send and confirm with CRC
            bool crcOK = WriteToDevice();

            // Check the reply (if any??)
            if (ChecksumOK() != true)
            {
                return false;
            }
            return true;
        }

        // Simple routine to move motor in one direction. Called directly.
        public bool IncreaseGap()
        {
            // No running untill initialized
            if (initComplete != true)
            {
                return false;
            }

            // Send ROL instruction to motor - Alex's appplication
            xmtBuf[0] = (byte)address;
            xmtBuf[1] = 0x81;                       // 0x81 eller 129 = Run application
            xmtBuf[2] = 1;                          // Type = 1: Run from specific address 
            xmtBuf[3] = 0x00;                       // Dont care
            xmtBuf[4] = 0x00;
            xmtBuf[5] = 0x00;
            xmtBuf[6] = 0x00;
            xmtBuf[7] = 0x2D;                       // Address to run from
            AppendCRC();

            // Send and confirm with CRC
            bool crcOK = WriteToDevice();

            // Check the reply (if any??)
            if (ChecksumOK() != true)
            {
                return false;
            }
            return true;
        }

        // Stop the motor
        public bool StopMotor()
        {
            // Send STP instruction to motor - Alex's appplication
            xmtBuf[0] = (byte)address;
            xmtBuf[1] = 0x81;                       // 0x81 = Run application
            xmtBuf[2] = 1;                          // Type = 1: Run from specific address 
            xmtBuf[3] = 0x00;                       // Dont care
            xmtBuf[4] = 0x00;
            xmtBuf[5] = 0x00;
            xmtBuf[6] = 0x00;
            xmtBuf[7] = 0x31;                       // Address to run from (holds stop instruction)
            AppendCRC();

            // Send and confirm with CRC
            bool crcOK = WriteToDevice();

            // Check the reply (if any??)
            if (ChecksumOK() != true)
            {
                return false;
            }
            return true;
        }

        // Calculate 8-bit addition CRC (Different from standard modbus) and append into transmission array
        public void AppendCRC()
        {
            // returns the MODBUS CRC of the lbuf first bytes of "buf" buffer (buf is a global array of bytes)
            int crc = 0;

            for (int csi = 0; csi <= 7; csi++)
            {
                crc = crc + xmtBuf[csi];
            }
            byte[] tempByte = BitConverter.GetBytes(crc);
            xmtBuf[8] = tempByte[0];
        }

        // Only reinstated for CheckSumOK
        public int ModRTU_CRC(byte[] bufIn, int bufLen)
        {
            // returns the MODBUS CRC of the lbuf first bytes of "buf" buffer (buf is a global array of bytes)
            int crc = 0;

            for (int csi = 0; csi <= 7; csi++)
            {
                crc = crc + bufIn[csi];
            }
            byte[] tempByte = BitConverter.GetBytes(crc);
            return tempByte[0];
        }

        // Validate received CRC
        public bool ChecksumOK()
        {
            try
            {
                // Validate reply
                if (rcvBuf.Length > 3)
                {
                    // Calculate new checksum from reply values & Swap the checksum byte order to be able to compare
                    string rawChecksum = ModRTU_CRC(rcvBuf, (rcvBuf.Length - 2)).ToString("X2");
                    // Retreive the checksum in the reply
                    string receivedChecksum = rcvBuf[rcvBuf.Length - 1].ToString("X2");

                    if ((rawChecksum == receivedChecksum))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}