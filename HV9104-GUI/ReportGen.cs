using System;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Jitbit.Utils;

namespace HV9104_GUI
{
    class ReportGen
    {
        RunView runView;
        public string DatePerformed, Operator, TestObject, OtherInfo, Duration, TestVoltage, PassFailStatus, SubTitle, elapsedTimeTitleLabel, resultTestVoltageLabel, passStatusLabel, secondsUnitLabel, hvUnitLabel, passFailUnitlabel;


        //Constructor
        public ReportGen(RunView runViewIn, String modeLabelIn)
        {
            runView = runViewIn;
            DatePerformed = runView.dateTextBox.Text;
            Operator = runView.operatorTextBox.Text;
            TestObject = runView.testObjectTextBox.Text;
            OtherInfo = runView.otherTextBox.Text;
            Duration = runView.elapsedTimeLabel.Text;
            TestVoltage = runView.resultTestVoltageValueLabel.Text;// runView.testVoltageTextBox.Text;
            PassFailStatus = runView.passFailLabel.Text;
            SubTitle = modeLabelIn;

            elapsedTimeTitleLabel = runView.elapsedTimeTitleLabel.Text;
            resultTestVoltageLabel = runView.resultTestVoltageLabel.Text;
            passStatusLabel = runView.passStatusLabel.Text;
            secondsUnitLabel = runView.secondsUnitLabel.Text;
            hvUnitLabel = runView.hvUnitLabel.Text;
            passFailUnitlabel = runView.passFailUnitlabel.Text;

        }
        public void GenerateReportNow()
        {

            GenerateChartImage();

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Portable Document Format (.pdf)|*.pdf";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();
            string pdfpath = saveFileDialog1.FileName;

            //string imagepath = @"C:\Users\Terco\Desktop\"; //HEADER LOGO PATH.
            Document doc = new Document(PageSize.A4 , 36f , 36f, 36f, 10f);
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(pdfpath, FileMode.Create));
                int totalfonts = FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
                var FontColour = new BaseColor(127, 127, 127);

                //FONTS
                var Calibri18 = FontFactory.GetFont("Calibri", 18, iTextSharp.text.Font.BOLD, FontColour);
                var Calibri16 = FontFactory.GetFont("Calibri", 16, FontColour);
                var Calibri16_Bold = FontFactory.GetFont("Calibri", 15, iTextSharp.text.Font.BOLD, FontColour);
                var Calibri14 = FontFactory.GetFont("Calibri", 14, FontColour);
                var Calibri48 = FontFactory.GetFont("Calibri", 48, iTextSharp.text.Font.BOLD, FontColour);
                var Calibri12 = FontFactory.GetFont("Calibri", 12, FontColour);
                var Calibri10 = FontFactory.GetFont("Calibri", 10, FontColour);

                doc.Open();
                //content byte
                PdfContentByte cb = writer.DirectContent;

                //HEADER IMAGE
                //C:\Users\Terco\Source\Repos\HV9104\HV9104-GUI\bin\Debug\Resources\tercoLogo.png
                iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(@"C:\Users\Terco\Source\Repos\HV9104\HV9104-GUI\Resources\SplashlogoCropped.JPG");   //HEADER LOGO imagepath + "/tercoLogo.png"
                gif.ScalePercent(7.5f);
                gif.SetAbsolutePosition(36f, doc.PageSize.Height - 46f);
                doc.Add(gif);
                
                //TITLE
                Paragraph heading = new Paragraph("EXPERIMENT REPORT", Calibri18);
                heading.SpacingAfter = 20f;
                heading.Alignment = Element.ALIGN_CENTER;
                doc.Add(heading);

                //SUBTITLE
                Paragraph heading2 = new Paragraph(SubTitle, Calibri16);
                heading2.SpacingAfter = 40f;
                heading2.Alignment = Element.ALIGN_CENTER;
                doc.Add(heading2);


                PdfPTable table = new PdfPTable(2);
                table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                float[] widths = new float[] { 1f, 2f };
                table.SetWidths(widths);

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
                cell8.FixedHeight = 75f;
              
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

                Paragraph paragraphTable2 = new Paragraph();
                paragraphTable2.SpacingBefore = 35f;

                doc.Add(paragraphTable2);

                PdfPTable table2 = new PdfPTable(3);
                PdfPCell cell9 = new PdfPCell(new Phrase(elapsedTimeTitleLabel, Calibri16_Bold));
                PdfPCell cell10 = new PdfPCell(new Phrase(resultTestVoltageLabel, Calibri16_Bold)); //dyn
                PdfPCell cell11 = new PdfPCell(new Phrase(passStatusLabel, Calibri16_Bold));
                PdfPCell cell12 = new PdfPCell(new Phrase(Duration, Calibri48));//dyn
                PdfPCell cell13 = new PdfPCell(new Phrase(TestVoltage, Calibri48));
                PdfPCell cell14 = new PdfPCell(new Phrase(PassFailStatus, Calibri48));//dyn
                PdfPCell cell15 = new PdfPCell(new Phrase(secondsUnitLabel, Calibri12));
                PdfPCell cell16 = new PdfPCell(new Phrase(hvUnitLabel, Calibri12));//dyn
                PdfPCell cell17 = new PdfPCell(new Phrase(passFailUnitlabel, Calibri12));
                PdfPCell cell18 = new PdfPCell(new Phrase("", Calibri12));//dyn
                PdfPCell cell19 = new PdfPCell(new Phrase(" ", Calibri12));
                PdfPCell cell20 = new PdfPCell(new Phrase(" ", Calibri12));//dyn


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

                //PdfPTable table3 = new PdfPTable(3);
                //PdfPCell c0 = new PdfPCell(new Phrase(" ", Calibri14));
                //PdfPCell c1 = new PdfPCell(new Phrase("APPROVED BY", Calibri14));
                //PdfPCell c2 = new PdfPCell(new Phrase("DATE", Calibri14));
                //PdfPCell c3 = new PdfPCell(new Phrase("STAMP", Calibri14));

                //c1.HorizontalAlignment = 1;
                //c2.HorizontalAlignment = 1;
                //c3.HorizontalAlignment = 1;
                //c0.HorizontalAlignment = 1;
                //c0.FixedHeight = 75f;

                //c1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //c2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //c3.Border = iTextSharp.text.Rectangle.NO_BORDER;

                //table3.HorizontalAlignment = 1;
                //table3.AddCell(c1);
                //table3.AddCell(c2);
                //table3.AddCell(c3);
                //table3.AddCell(c0);
                //table3.AddCell(" ");
                //table3.AddCell(" ");
                //table3.SpacingBefore = 30f;
                //doc.Add(table3);

                Paragraph paragraphTable3 = new Paragraph();
                paragraphTable3.SpacingBefore = 110f;

                doc.Add(paragraphTable2);
                Chunk chunk1 = new Chunk("APPROVED BY:____________________________  DATE:______________________ ",Calibri10);
                doc.Add(paragraphTable3);
                doc.Add(chunk1);

                //draw box
                cb.Rectangle(doc.PageSize.Width - 200f, 24f, 150f, 150f);
                cb.SetColorStroke(FontColour);
                cb.Stroke();

                //text box
             
                cb.BeginText();
                BaseFont bf = BaseFont.CreateFont("C:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                
                cb.SetFontAndSize(bf,10);
                cb.SetColorFill(FontColour);

                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "OFFICIAL STAMP", doc.PageSize.Width - 125f, 160f, 0f);
                cb.EndText();

            }

            catch (Exception ex)
            {

            }

            finally
            {
                doc.Close();
            }

            System.Diagnostics.Process.Start(pdfpath);

        }
        

        private void GenerateChartImage()
        {
            runView.autoTestChart.SaveImage(@"C:\Users\Terco\Desktop\PDFgeneration\chart.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }


        public void ExportValues(double[] xIn, double[] yIn)
        {

            double[] x = new double[xIn.Length-1];
            double[] y = new double[yIn.Length - 1];

            for(int i = 0; i<xIn.Length-1;i++)
            {
                x[i] = xIn[i];
                y[i] = yIn[i];
            }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Comma-Separated Value (.csv) | *.csv ";
            saveFileDialog1.Title = "Save file";
            saveFileDialog1.ShowDialog();
            var finalPath = saveFileDialog1.FileName;

            if (finalPath != "")
            {
                var myExport = new CsvExport();
                myExport.AddRow();
                myExport["DATE"] = DatePerformed;
                myExport["OPERATOR"] = Operator;
                myExport["TEST OBJECT"] = TestObject;
                myExport["OTHER INFO"] = OtherInfo;
                for (int i = 0; i < 100; i++)
                {
                    myExport.AddRow();
                    myExport["X"] = x[i].ToString();
                    myExport["Y"] = y[i].ToString();

                }

                //var desiredPath =  @"C:\Users\Terco\Desktop\test.csv"; //ChooseFolder();
                //var finalPath = UniqueFileName(desiredPath);

                myExport.ExportToFile(finalPath);
            }



        }






    }
}
