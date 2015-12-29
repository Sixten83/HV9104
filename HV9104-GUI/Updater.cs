using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HV9104_GUI
{
    public class Updater
    {
        public RunView activeForm;

        public Updater(RunView activeFormIn)
        {
            activeForm = activeFormIn;
        }

        // AC Voltage Input value
        public void transferVoltageInputLabel(string text1)
        {
            if (activeForm.voltageInputLabel.InvokeRequired)
            {
                activeForm.voltageInputLabel.Invoke((MethodInvoker)delegate ()
                {
                    transferVoltageInputLabel(text1);
                });
            }
            else this.activeForm.voltageInputLabel.Text = text1;
        }

        // AC Voltage Output value
        public void transferACVoltageOutputLabel(string textACVoltageOut)
        {
            if (activeForm.acValueLabel.InvokeRequired)
            {
                activeForm.acValueLabel.Invoke((MethodInvoker)delegate ()
                {
                    transferACVoltageOutputLabel(textACVoltageOut);
                });
            }
            else this.activeForm.acValueLabel.Text = textACVoltageOut;
        }

        // DC Voltage Output value
        public void transferDCVoltageOutputLabel(string textDCVoltageOut)
        {
            if (activeForm.dcValueLabel.InvokeRequired)
            {
                activeForm.dcValueLabel.Invoke((MethodInvoker)delegate ()
                {
                    transferDCVoltageOutputLabel(textDCVoltageOut);
                });
            }
            else this.activeForm.dcValueLabel.Text = textDCVoltageOut;
        }

        // Impulse Voltage Output value
        public void transferImpulseVoltageOutputLabel(string textImpulseVoltageOut)
        {
            if (activeForm.impulseValueLabel.InvokeRequired)
            {
                activeForm.impulseValueLabel.Invoke((MethodInvoker)delegate ()
                {
                    transferImpulseVoltageOutputLabel(textImpulseVoltageOut);
                });
            }
            else this.activeForm.impulseValueLabel.Text = textImpulseVoltageOut;
        }

        // Current value
        public void transferCurrentInputLabel(string text2)
        {
            if (activeForm.currentInputLabel.InvokeRequired)
            {
                activeForm.currentInputLabel.Invoke((MethodInvoker)delegate ()
                {
                    transferCurrentInputLabel(text2);
                });
            }
            else this.activeForm.currentInputLabel.Text = text2;
        }

        // Pressure value
        public void transferPressureTextbox(string text3)
        {
            if (activeForm.pressureTextBox.InvokeRequired)
            {
                activeForm.pressureTextBox.Invoke((MethodInvoker)delegate ()
                {
                    transferPressureTextbox(text3);
                });
            }
            else this.activeForm.pressureTextBox.Text = text3;
        }

        //// Comport
        //public void transferTextBoxActiveComPort(string textActiveComPort)
        //{
        //    if (activeForm.TextBoxActiveComPort.InvokeRequired)
        //    {
        //        activeForm.TextBoxActiveComPort.Invoke((MethodInvoker)delegate ()
        //        {
        //            transferTextBoxActiveComPort(textActiveComPort);
        //        });
        //    }
        //    else this.activeForm.TextBoxActiveComPort.Text = textActiveComPort;
        //}

        // Impulse Sphere Gap position
        public void transferImpulseGapLabel(string text11)
        {
            if (activeForm.impulseGapLabel.InvokeRequired)
            {
                activeForm.impulseGapLabel.Invoke((MethodInvoker)delegate ()
                {
                    transferImpulseGapLabel(text11);
                });
            }
            else this.activeForm.impulseGapLabel.Text = text11;
        }

        //// Calculated High Voltage Value
        //public void transferLabel47(string text47)
        //{
        //    if (activeForm.label47.InvokeRequired)
        //    {
        //        activeForm.label47.Invoke((MethodInvoker)delegate ()
        //        {
        //            transferLabel47(text47);
        //        });
        //    }
        //    else this.activeForm.label47.Text = text47;
        //}

        //// Fault
        //public void transferLabel36(string text36)
        //{
        //    if (activeForm.label36.InvokeRequired)
        //    {
        //        activeForm.label36.Invoke((MethodInvoker)delegate ()
        //        {
        //            transferLabel36(text36);
        //        });
        //    }
        //    else this.activeForm.label36.Text = text36;
        //}

        //// Earthing switch
        //public void transferLabel37(string text37)
        //{
        //    if (activeForm.label37.InvokeRequired)
        //    {
        //        activeForm.label37.Invoke((MethodInvoker)delegate ()
        //        {
        //            transferLabel37(text37);
        //        });
        //    }
        //    else this.activeForm.label37.Text = text37;
        //}

        //// Discharge rod
        //public void transferLabel38(string text38)
        //{
        //    if (activeForm.label38.InvokeRequired)
        //    {
        //        activeForm.label38.Invoke((MethodInvoker)delegate ()
        //        {
        //            transferLabel38(text38);
        //        });
        //    }
        //    else this.activeForm.label38.Text = text38;
        //}

        //// CTS Flag
        //public void transferCTSFlag(string textCTSFlag)
        //{
        //    if (activeForm.CTSFlag.InvokeRequired)
        //    {
        //        activeForm.CTSFlag.Invoke((MethodInvoker)delegate ()
        //        {
        //            transferCTSFlag(textCTSFlag);
        //        });
        //    }
        //    else this.activeForm.CTSFlag.Text = textCTSFlag;
        //}

        //// Em Stop + Key switch
        //public void transferLabel39(string text39)
        //{
        //    if (activeForm.label39.InvokeRequired)
        //    {
        //        activeForm.label39.Invoke((MethodInvoker)delegate ()
        //        {
        //            transferLabel39(text39);
        //        });
        //    }
        //    else this.activeForm.label39.Text = text39;
        //}

        //// Door switch(es)
        //public void transferLabel40(string text40)
        //{
        //    if (activeForm.label40.InvokeRequired)
        //    {
        //        activeForm.label40.Invoke((MethodInvoker)delegate ()
        //        {
        //            transferLabel40(text40);
        //        });
        //    }
        //    else this.activeForm.label40.Text = text40;
        //}

        //// F2 + K1
        //public void transferLabel41(string text41)
        //{
        //    if (activeForm.label41.InvokeRequired)
        //    {
        //        activeForm.label41.Invoke((MethodInvoker)delegate ()
        //        {
        //            transferLabel41(text41);
        //        });
        //    }
        //    else this.activeForm.label41.Text = text41;
        //}

        //// F1 + K2
        //public void transferLabel42(string text42)
        //{
        //    if (activeForm.label42.InvokeRequired)
        //    {
        //        activeForm.label42.Invoke((MethodInvoker)delegate ()
        //        {
        //            transferLabel42(text42);
        //        });
        //    }
        //    else this.activeForm.label42.Text = text42;
        //}

        //// Voltage parked
        //public void transferLabel43(string text43)
        //{
        //    if (activeForm.label43.InvokeRequired)
        //    {
        //        activeForm.label43.Invoke((MethodInvoker)delegate ()
        //        {
        //            transferLabel43(text43);
        //        });
        //    }
        //    else this.activeForm.label43.Text = text43;
        //}

        // High Voltage symbol
        //public void transferHVPic(bool HVPicIn)
        //{
        //    if (activeForm.pictureBox1.Visible.InvokeRequired)
        //    {
        //        activeForm.pictureBox1.Visible.Invoke((MethodInvoker)delegate()
        //        {
        //            transferHVPic(HVPicIn);
        //        });
        //    }
        //    else this.activeForm.pictureBox1.Visible = HVPicIn;
        //}

    }
}
