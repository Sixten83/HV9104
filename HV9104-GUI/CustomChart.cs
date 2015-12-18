using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.Windows.Input;

namespace HV9104_GUI
{
    public class CustomChart : Chart
    {
        public ChartArea chartArea1;
        Series acSeries;
        Series dcSeries;
        Series impulseSeries;
        Series xCursor1;
        Series xCursor2;
        Series yCursor1;
        Series yCursor2;
        bool x1InPos, x2InPos, y1InPos, y2InPos, pressed, cursorMenuDisplayed;
        CustomCursorMenu cursorMenu;

        public CustomChart()
        {
            chartArea1 = new ChartArea();

            acSeries = new Series();
            dcSeries = new Series();
            impulseSeries = new Series();
            xCursor1 = new Series();
            xCursor2 = new Series();
            yCursor1 = new Series();
            yCursor2 = new Series();

            


            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            chartArea1.AxisX.Interval = 5D;
            chartArea1.AxisX.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisX.Maximum = 25D;
            chartArea1.AxisX.Minimum = -25D;
            chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisY.Interval = 4D;
            chartArea1.AxisY.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisY.MajorTickMark.Enabled = false;
            chartArea1.AxisY.Maximum = 20D;
            chartArea1.AxisY.Minimum = -20D;
            chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            chartArea1.BorderColor = System.Drawing.Color.Transparent;            
            chartArea1.Name = "ChartArea1";
     //       this.ChartAreas.Add(chartArea1);
            this.Location = new System.Drawing.Point(20, 13);
            this.Name = "chart1";

            acSeries.ChartArea = "ChartArea1";
            acSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            acSeries.IsVisibleInLegend = false;
            acSeries.Name = "acSeries";

            dcSeries.ChartArea = "ChartArea1";
            dcSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            dcSeries.IsVisibleInLegend = false;
            dcSeries.Name = "dcSeries";

            impulseSeries.ChartArea = "ChartArea1";
            impulseSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            impulseSeries.IsVisibleInLegend = false;
            impulseSeries.Name = "impulseSeries";


            xCursor1.ChartArea = "ChartArea1";
            xCursor1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            xCursor1.IsVisibleInLegend = false;
            xCursor1.Name = "xCursor1";
            xCursor1.Points.Add(new DataPoint(chartArea1.AxisX.Minimum, chartArea1.AxisY.Minimum));
            xCursor1.Points.Add(new DataPoint(chartArea1.AxisX.Minimum, chartArea1.AxisY.Maximum));
            xCursor1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            xCursor1.BorderWidth = 2;
            xCursor1.Color = System.Drawing.Color.FromArgb(140,159,171);
            xCursor2.ChartArea = "ChartArea1";
            xCursor2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            xCursor2.IsVisibleInLegend = false;
            xCursor2.Name = "xCursor2";
            xCursor2.Points.Add(new DataPoint(chartArea1.AxisX.Maximum, chartArea1.AxisY.Minimum));
            xCursor2.Points.Add(new DataPoint(chartArea1.AxisX.Maximum, chartArea1.AxisY.Maximum));
            xCursor2.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            xCursor2.BorderWidth = 2;
            xCursor2.Color = System.Drawing.Color.FromArgb(140, 159, 171);
            yCursor1.ChartArea = "ChartArea1";
            yCursor1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            yCursor1.IsVisibleInLegend = false;
            yCursor1.Name = "yCursor1";
            yCursor1.Points.Add(new DataPoint(chartArea1.AxisX.Minimum, chartArea1.AxisY.Minimum));
            yCursor1.Points.Add(new DataPoint(chartArea1.AxisX.Maximum, chartArea1.AxisY.Minimum));
            yCursor1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            yCursor1.BorderWidth = 2;
            yCursor1.Color = System.Drawing.Color.FromArgb(118,113,113);
            yCursor2.ChartArea = "ChartArea1";
            yCursor2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            yCursor2.IsVisibleInLegend = false;
            yCursor2.Name = "yCursor2";
            yCursor2.Points.Add(new DataPoint(chartArea1.AxisX.Minimum, chartArea1.AxisY.Maximum));
            yCursor2.Points.Add(new DataPoint(chartArea1.AxisX.Maximum, chartArea1.AxisY.Maximum));
            yCursor2.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            yCursor2.BorderWidth = 2;
            yCursor2.Color = System.Drawing.Color.FromArgb(118, 113, 113);
          /* this.Series.Add(acSeries);
           this.Series.Add(dcSeries);
           this.Series.Add(impulseSeries);
            this.Series.Add(xCursor1);
            this.Series.Add(xCursor2);
            this.Series.Add(yCursor1);
            this.Series.Add(yCursor2);*/
            this.Size = new System.Drawing.Size(864, 708);
            this.TabIndex = 0;
            this.Text = "chart1";

            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chart_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart_MouseUp);
            this.MouseLeave += new System.EventHandler(this.chart_MouseLeave);
            
            
        }

        public void addPoints(String serie, Channel.ScaledData scaledData, int points)
        {

            for (int r = 0; r < points; r++)
            {
                this.Series[serie].Points.AddXY(scaledData.x[r], scaledData.y[r]);
                //Console.WriteLine(" x:" + scaledData.x[r] + " y: " + scaledData.y[r]);
            }
            
        }

        public void setVoltsPerDiv(double voltsPerDiv)
        {
            this.Series["yCursor1"].Points[0].YValues[0] *= voltsPerDiv / this.ChartAreas[0].AxisY.Interval;
            this.Series["yCursor1"].Points[1].YValues[0] *= voltsPerDiv / this.ChartAreas[0].AxisY.Interval;
            this.Series["yCursor2"].Points[0].YValues[0] *= voltsPerDiv / this.ChartAreas[0].AxisY.Interval;
            this.Series["yCursor2"].Points[1].YValues[0] *= voltsPerDiv / this.ChartAreas[0].AxisY.Interval;
            this.ChartAreas[0].AxisY.Interval = voltsPerDiv;
            this.ChartAreas[0].AxisY.Maximum = voltsPerDiv * 5;
            this.ChartAreas[0].AxisY.Minimum = -voltsPerDiv * 5;
            this.Series["xCursor1"].Points[0].YValues[0] = this.ChartAreas[0].AxisY.Minimum;
            this.Series["xCursor1"].Points[1].YValues[0] = this.ChartAreas[0].AxisY.Maximum;
            this.Series["xCursor2"].Points[0].YValues[0] = this.ChartAreas[0].AxisY.Minimum;
            this.Series["xCursor2"].Points[1].YValues[0] = this.ChartAreas[0].AxisY.Maximum;
         
        }

        public void setTimePerDiv(double timePerDiv)
        {
            this.Series["xCursor1"].Points[0].XValue *= timePerDiv / this.ChartAreas[0].AxisX.Interval;
            this.Series["xCursor1"].Points[1].XValue *= timePerDiv / this.ChartAreas[0].AxisX.Interval;
            this.Series["xCursor2"].Points[0].XValue *= timePerDiv / this.ChartAreas[0].AxisX.Interval;
            this.Series["xCursor2"].Points[1].XValue *= timePerDiv / this.ChartAreas[0].AxisX.Interval;
            this.ChartAreas[0].AxisX.Interval = timePerDiv;
            this.ChartAreas[0].AxisX.Maximum = timePerDiv * 5;
            this.ChartAreas[0].AxisX.Minimum = -timePerDiv * 5;
            this.Series["yCursor1"].Points[0].XValue = this.ChartAreas[0].AxisX.Minimum;
            this.Series["yCursor1"].Points[1].XValue = this.ChartAreas[0].AxisX.Maximum;
            this.Series["yCursor2"].Points[0].XValue = this.ChartAreas[0].AxisX.Minimum;
            this.Series["yCursor2"].Points[1].XValue = this.ChartAreas[0].AxisX.Maximum;
        }

        

        private void chart_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
          
            if (x1InPos || x2InPos || y1InPos || y2InPos)
                pressed = true;

            if (pressed && !cursorMenuDisplayed)
            {
                Point startPoint = this.PointToScreen(new Point());
                if (cursorMenu == null)
                {
                    cursorMenu = new CustomCursorMenu();
                    cursorMenu.Location = new Point(startPoint.X + this.Width - cursorMenu.Width, startPoint.Y + this.Height - cursorMenu.Height);
                    cursorMenu.updateCursorPos(this.Series["xCursor1"].Points[0].XValue, this.Series["xCursor2"].Points[0].XValue, this.Series["yCursor1"].Points[0].YValues[0], this.Series["yCursor2"].Points[0].YValues[0]);
                    cursorMenu.Owner = FindForm();
                    cursorMenu.closeButton.Click += new System.EventHandler(this.cursorMenuCloseButton_Click);
                }
                cursorMenu.Location = new Point(startPoint.X + this.Width - cursorMenu.Width, startPoint.Y + this.Height - cursorMenu.Height);
                cursorMenu.updateCursorPos(this.Series["xCursor1"].Points[0].XValue, this.Series["xCursor2"].Points[0].XValue, this.Series["yCursor1"].Points[0].YValues[0], this.Series["yCursor2"].Points[0].YValues[0]);
                  
                cursorMenu.Show();
                cursorMenuDisplayed = true;
           

            }
           
        }

        private void cursorMenuCloseButton_Click(object sender, EventArgs e)
        {
            cursorMenu.Hide();
            cursorMenuDisplayed = false;
        }

        private void chart_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
           
            pressed = x1InPos = x2InPos = y1InPos = y2InPos = false;
            
        }
        
        private void chart_MouseLeave(object sender, System.EventArgs e)
        {
           
            pressed = x1InPos = x2InPos = y1InPos = y2InPos = false;
            
        }

        private void chart_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
           

            double mouseX = 0;
            double mouseY = 0;
            try
            {
                mouseX = this.ChartAreas[0].AxisX.PixelPositionToValue((double)e.X);
                mouseY = this.ChartAreas[0].AxisY.PixelPositionToValue((double)e.Y);
            }
            catch (ArgumentException)
            {

            }
            catch(InvalidOperationException)
            {

            }

            double xMin = this.ChartAreas[0].AxisX.Minimum;
            double xMax = this.ChartAreas[0].AxisX.Maximum;
            double yMin = this.ChartAreas[0].AxisY.Minimum;
            double yMax = this.ChartAreas[0].AxisY.Maximum;

            if (xMin <= mouseX && mouseX <= xMax && yMin <= mouseY && mouseY <= yMax)
            {
                double xPremises = this.ChartAreas[0].AxisX.Interval / 10;
                double yPremises = this.ChartAreas[0].AxisY.Interval / 10;
                
                if (!x2InPos && !y1InPos && !y2InPos && Math.Abs(mouseX - this.Series["xCursor1"].Points[0].XValue) < xPremises || (x1InPos && pressed))
                {
                    x1InPos = true;
                    this.Cursor = System.Windows.Forms.Cursors.VSplit;
                  
                    if (pressed)
                    {
                        this.Series["xCursor1"].Points[0].XValue = mouseX;
                        this.Series["xCursor1"].Points[1].XValue = mouseX;
                        cursorMenu.updateCursorPos(this.Series["xCursor1"].Points[0].XValue, this.Series["xCursor2"].Points[0].XValue, this.Series["yCursor1"].Points[0].YValues[0], this.Series["yCursor2"].Points[0].YValues[0]);
                    }
                }
                else if (!x1InPos && !y1InPos && !y2InPos && Math.Abs(mouseX - this.Series["xCursor2"].Points[0].XValue) < xPremises || (x2InPos && pressed))
                {                   
                    x2InPos = true;
                    this.Cursor = System.Windows.Forms.Cursors.VSplit;
 
                    if (pressed)
                    {
                        this.Series["xCursor2"].Points[0].XValue = mouseX;
                        this.Series["xCursor2"].Points[1].XValue = mouseX;
                        cursorMenu.updateCursorPos(this.Series["xCursor1"].Points[0].XValue, this.Series["xCursor2"].Points[0].XValue, this.Series["yCursor1"].Points[0].YValues[0], this.Series["yCursor2"].Points[0].YValues[0]);
                    }
                }
                else if (!x1InPos && !x2InPos && !y2InPos && Math.Abs(mouseY - this.Series["yCursor1"].Points[0].YValues[0]) < yPremises || (y1InPos && pressed))
                {
                    y1InPos = true;
                    this.Cursor = System.Windows.Forms.Cursors.HSplit;

                    if (pressed)
                    {
                        this.Series["yCursor1"].Points[0].SetValueXY(xMin, mouseY);
                        this.Series["yCursor1"].Points[1].SetValueXY(xMax, mouseY);
                        cursorMenu.updateCursorPos(this.Series["xCursor1"].Points[0].XValue, this.Series["xCursor2"].Points[0].XValue, this.Series["yCursor1"].Points[0].YValues[0], this.Series["yCursor2"].Points[0].YValues[0]);
                        this.Invalidate();
                    }
                }
                else if (!x1InPos && !x2InPos && !y1InPos && Math.Abs(mouseY - this.Series["yCursor2"].Points[0].YValues[0]) < yPremises || (y2InPos && pressed))
                {
                    y2InPos = true;
                    this.Cursor = System.Windows.Forms.Cursors.HSplit;
             
                    if (pressed)
                    {
                        this.Series["yCursor2"].Points[0].SetValueXY(xMin, mouseY);
                        this.Series["yCursor2"].Points[1].SetValueXY(xMax, mouseY);
                        cursorMenu.updateCursorPos(this.Series["xCursor1"].Points[0].XValue, this.Series["xCursor2"].Points[0].XValue, this.Series["yCursor1"].Points[0].YValues[0], this.Series["yCursor2"].Points[0].YValues[0]);
                        this.Invalidate();
                    }
                }

                else
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    x1InPos = x2InPos = y1InPos = y2InPos = false;
                }
            }
            else
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                x1InPos = x2InPos = y1InPos = y2InPos = false;
            }
            
        }
    }
}
