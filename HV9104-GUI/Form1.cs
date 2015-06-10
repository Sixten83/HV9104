using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HV9104_GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            double[] data;
            data = new double[20]{1,2,3,4,5,4,3,2,1,2,3,4,5,4,3,2,1,2,3,4};

            //chart1.Series["Series1"].Points.Add(data);
            Random rdn = new Random();
            for (int i = -100; i < 100; i++)
            {
                chart1.Series["Series1"].Points.AddXY
                                (i, i+5);
               
            }
           // chart1.Series["Series1"].ChartType = 
            chart1.Series["Series1"].Color = Color.Red;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chart1.ChartAreas[0].AxisX.MajorTickMark.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chart1.ChartAreas[0].AxisX.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chart1.ChartAreas[0].AxisX.Interval = 20;

            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chart1.ChartAreas[0].AxisY.MajorTickMark.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chart1.ChartAreas[0].AxisY.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chart1.ChartAreas[0].AxisY.Interval = 25;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
