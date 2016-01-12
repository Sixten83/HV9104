using System;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HV9104_GUI
{
    class ReportGen
    {
        RunView runView;
        public string DatePerformed, Operator, TestObject, OtherInfo, Duration, TestVoltage, PassFailStatus, SubTitle;

        public void GenerateReportNow(RunView runViewIn, String modeLabelIn)
        {
            runView = runViewIn;
            DatePerformed = runView.dateTextBox.Text;
            Operator = runView.operatorTextBox.Text;
            TestObject = runView.testObjectTextBox.Text;
            OtherInfo = runView.otherTextBox.Text;
            Duration = runView.elapsedTimeLabel.Text;
            TestVoltage = runView.testVoltageTextBox.Text;
            PassFailStatus = runView.passFailLabel.Text;
            SubTitle = modeLabelIn;

            GenerateChartImage();

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Portable Document Format (.pdf)|*.pdf";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();
            string pdfpath = saveFileDialog1.FileName;

            //string imagepath = @"C:\Users\Terco\Desktop\"; //HEADER LOGO PATH.
            Document doc = new Document();
            try
            {
                PdfWriter.GetInstance(doc, new FileStream(pdfpath, FileMode.Create));
                int totalfonts = FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
                var FontColour = new BaseColor(127, 127, 127);

                //FONTS
                var Calibri18 = FontFactory.GetFont("Calibri", 18, iTextSharp.text.Font.BOLD, FontColour);
                var Calibri16 = FontFactory.GetFont("Calibri", 16, FontColour);
                var Calibri16_Bold = FontFactory.GetFont("Calibri", 15, iTextSharp.text.Font.BOLD, FontColour);
                var Calibri14 = FontFactory.GetFont("Calibri", 14, FontColour);
                var Calibri48 = FontFactory.GetFont("Calibri", 48, iTextSharp.text.Font.BOLD, FontColour);
                var Calibri12 = FontFactory.GetFont("Calibri", 12, FontColour);

                doc.Open();
                //HEADER IMAGE
                iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(@"C:\Users\Terco\Source\Repos\HV9104\HV9104-GUI\bin\Debug\Resources\tercoLogo.png");   //HEADER LOGO imagepath + "/tercoLogo.png"
                gif.ScalePercent(50f);
                gif.SetAbsolutePosition(36f, doc.PageSize.Height - 36f);
                doc.Add(gif);
                
                //TITLE
                Paragraph heading = new Paragraph("EXPERIMENT REPORT", Calibri18);
                heading.SpacingAfter = 20f;
                heading.Alignment = Element.ALIGN_CENTER;
                doc.Add(heading);

                //SUBTITLE
                Paragraph heading2 = new Paragraph(SubTitle, Calibri16);
                heading2.SpacingAfter = 50f;
                heading2.Alignment = Element.ALIGN_CENTER;
                doc.Add(heading2);


                PdfPTable table = new PdfPTable(2);
                table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                PdfPCell cell1 = new PdfPCell(new Phrase("DATE PERFORMED:", Calibri14));
                PdfPCell cell2 = new PdfPCell(new Phrase(DatePerformed, Calibri14)); //dyn
                PdfPCell cell3 = new PdfPCell(new Phrase("OPERATOR:", Calibri14));
                PdfPCell cell4 = new PdfPCell(new Phrase(Operator, Calibri14));//dyn
                PdfPCell cell5 = new PdfPCell(new Phrase("TEST OBJECT:", Calibri14));
                PdfPCell cell6 = new PdfPCell(new Phrase(TestObject, Calibri14));//dyn
                PdfPCell cell7 = new PdfPCell(new Phrase("OTHER INFO:", Calibri14));
                PdfPCell cell8 = new PdfPCell(new Phrase(OtherInfo, Calibri14));//dyn

                cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell8.Border = iTextSharp.text.Rectangle.NO_BORDER;


                table.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1);
                table.AddCell(cell2);
                table.AddCell(cell3);
                table.AddCell(cell4);
                table.AddCell(cell5);
                table.AddCell(cell6);
                table.AddCell(cell7);
                table.AddCell(cell8);
                doc.Add(table);
                //Experiment results
                Paragraph heading3 = new Paragraph("EXPERIMENT RESULTS", Calibri16_Bold);
                heading3.SpacingAfter = 20f;
                heading3.SpacingBefore = 20f;
                heading3.Alignment = Element.ALIGN_CENTER;
                doc.Add(heading3);

                //chart image

                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(@"C:\Users\Terco\Desktop\PDFgeneration\chart.jpg"); //CHART PATH.
                jpg.ScaleToFit(420f, 500f);
                jpg.Border = iTextSharp.text.Rectangle.BOX;
                //jpg.BorderColor = Color.YELLOW;
                jpg.BorderWidth = 0.5f;
                jpg.Alignment = Element.ALIGN_CENTER;
                doc.Add(jpg);


                PdfPTable table2 = new PdfPTable(3);
                PdfPCell cell9 = new PdfPCell(new Phrase("DURATION", Calibri16_Bold));
                PdfPCell cell10 = new PdfPCell(new Phrase("TEST VOLTAGE", Calibri16_Bold)); //dyn
                PdfPCell cell11 = new PdfPCell(new Phrase("PASS/FAIL STATUS", Calibri16_Bold));
                PdfPCell cell12 = new PdfPCell(new Phrase(Duration, Calibri48));//dyn
                PdfPCell cell13 = new PdfPCell(new Phrase(TestVoltage, Calibri48));
                PdfPCell cell14 = new PdfPCell(new Phrase(PassFailStatus, Calibri48));//dyn
                PdfPCell cell15 = new PdfPCell(new Phrase("SECONDS", Calibri12));
                PdfPCell cell16 = new PdfPCell(new Phrase("kV", Calibri12));//dyn
                PdfPCell cell17 = new PdfPCell(new Phrase("", Calibri14));
                PdfPCell cell18 = new PdfPCell(new Phrase("", Calibri12));//dyn
                PdfPCell cell19 = new PdfPCell(new Phrase("+/- 15%", Calibri12));
                PdfPCell cell20 = new PdfPCell(new Phrase("e 46 seconds", Calibri12));//dyn


                cell9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell10.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell13.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell14.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell15.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell16.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell17.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell18.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell19.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell20.Border = iTextSharp.text.Rectangle.NO_BORDER;

                cell9.HorizontalAlignment = 1;
                cell10.HorizontalAlignment = 1;
                cell11.HorizontalAlignment = 1;
                cell12.HorizontalAlignment = 1;
                cell13.HorizontalAlignment = 1;
                cell14.HorizontalAlignment = 1;
                cell15.HorizontalAlignment = 1;
                cell16.HorizontalAlignment = 1;
                cell17.HorizontalAlignment = 1;
                cell18.HorizontalAlignment = 1;
                cell19.HorizontalAlignment = 1;
                cell20.HorizontalAlignment = 1;
                //cell11.Width = 2f;

                cell9.HorizontalAlignment = 1;
                table2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cell9);
                table2.AddCell(cell10);
                table2.AddCell(cell11);
                table2.AddCell(cell12);
                table2.AddCell(cell13);
                table2.AddCell(cell14);
                table2.AddCell(cell15);
                table2.AddCell(cell16);
                table2.AddCell(cell17);
                table2.AddCell(cell18);
                table2.AddCell(cell19);
                table2.AddCell(cell20);


                doc.Add(table2);

            }

            catch (Exception ex)
            {

            }

            finally
            {
                doc.Close();
            }



        }
        //private void GetInfo()
        //{
        //    DatePerformed = textBox1.Text;
        //    Operator = textBox2.Text;
        //    TestObject = textBox3.Text;
        //    OtherInfo = textBox4.Text;
        //    Duration = textBox5.Text;
        //    TestVoltage = textBox6.Text;
        //    PassFailStatus = textBox7.Text;
        //    SubTitle = label8.Text;

        //}

        private void GenerateChartImage()
        {
            runView.autoTestChart.SaveImage(@"C:\Users\Terco\Desktop\PDFgeneration\chart.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    GenerateChartImage();
        //    GetInfo();
        //    GenerateReport();

        //}

        private void label2_Click(object sender, EventArgs e)
        {

        }






    }
}
