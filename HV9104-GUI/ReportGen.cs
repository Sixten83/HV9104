using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace HV9104_GUI
{
    class ReportGen
    {


        SaveFileDialog sfd;

        public void GenerateChartImage(Chart chartIn)
        {
            Chart chart1 = new Chart();
            chart1.SaveImage("test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg); //filnamn på charten. Just nu skrivs den över för varje "export"
        }


        public void GeneratePdf()
        {
            /*
            saveFileDialog1.ShowDialog();
            string fn = saveFileDialog1.FileName;
            saveFileDialog1.Filter = "*.tex";
            saveFileDialog1.DefaultExt = "tex";
            */


            string filename = sfd.FileName;//@"C:\Users\Terco\Desktop\text.tex"; 
            //filename = filename.Replace(@"\", "/");
            Process p1 = new Process();
            p1.StartInfo.FileName = @"C:\Program Files (x86)\MiKTeX 2.9\miktex\bin\xelatex.exe"; //xelatex är typsättaren
            p1.StartInfo.Arguments = filename;
            p1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p1.StartInfo.RedirectStandardOutput = true;
            p1.StartInfo.UseShellExecute = false;

            p1.Start();
           // try
            //{
                var output = p1.StandardOutput.ReadToEnd();
            //}
            //catch (IOException e)
            //{
            //    string ex = e.ToString();
            //}

            p1.WaitForExit();

        }

        public void GenerateTex(string testType, string date, string operatorName, string testObject, string otherInfo, string duration, string testVoltage, string statusPassFail)
        {
            sfd = new SaveFileDialog();
            sfd.Filter = "(*.tex)|*.tex";
            sfd.ShowDialog();

            string path = sfd.FileName; //@"C:\Users\Terco\Desktop\WindowsFormsApplication2\WindowsFormsApplication2\bin\Debug\text.tex";
            //if (!File.Exists(path))
            // {
            using (StreamWriter sw = File.CreateText(path))
            {
                //#####################################
                //Packages and definitions
                //#####################################
                sw.WriteLine("\\documentclass{article}");
                sw.WriteLine("\\usepackage{graphicx}");
                sw.WriteLine("\\usepackage{a4wide}");
                sw.WriteLine("\\usepackage{amsmath}");
                sw.WriteLine("\\usepackage{fancyhdr}");
                sw.WriteLine("\\pagestyle{fancy}");
                sw.WriteLine("\\usepackage{xcolor}");
                sw.WriteLine("\\usepackage{color}");
                sw.WriteLine("\\usepackage[bottom=0.50cm, top=2.5cm]{geometry}");
                sw.WriteLine("\\setlength\\parindent{0pt}");
                sw.WriteLine("\\definecolor{textcolor}{RGB}{127,127,127}");
                sw.WriteLine("\\renewcommand{\\labelenumi}{\\alph{enumi}.} ");
                sw.WriteLine("\\usepackage{fontspec}");
                sw.WriteLine("\\setmainfont{Calibri}");
                sw.WriteLine("\\newcommand{\\sizeone}{\\fontsize{48pt}{20pt}\\selectfont} %48pt");
                sw.WriteLine("\\newcommand{\\sizetwo}{\\fontsize{16pt}{20pt}\\selectfont} % 16pt");
                sw.WriteLine("\\newcommand{\\sizethree}{\\fontsize{12pt}{20pt}\\selectfont} % 12pt");
                sw.WriteLine("\\title{\\vspace{-2.0cm}EXPERIMENT REPORT \\\\ " + testType + " } "); // EXPERIMENT REPORT är statisk, DC WITHSTAND TEST = dynamisk, hämta variabel.
                sw.WriteLine("\\date{} ");
                sw.WriteLine("\\geometry{headheight = 0.6in}");
                sw.WriteLine("\\fancypagestyle{firstpage}{\\fancyhf{}\\fancyhead[R]{\\includegraphics[height=0.5in, keepaspectratio=true]{Splashlogo}}}");
                sw.WriteLine("\\fancypagestyle{plain}{\\fancyhf{}\\fancyhead[L]{\\includegraphics[height=0.5in, keepaspectratio=true]{Splashlogo}}}");

                //#####################################
                //Begin document {preamble}
                //#####################################
                sw.WriteLine("\\begin{document}");
                sw.WriteLine("\\color{textcolor}");
                sw.WriteLine("\\maketitle");
                sw.WriteLine("{\\large");
                //sw.WriteLine("\\begin{center}");
                sw.WriteLine("\\begin{tabular}{l p{10cm}}");
                sw.WriteLine("DATE PERFORMED: & {0} \\\\ ", date);
                sw.WriteLine("OPERATOR: & {0} \\\\ ", operatorName);
                sw.WriteLine("TEST OBJECT: & {0} \\\\", testObject);
                sw.WriteLine("OTHER INFO: & {0}", otherInfo);
                sw.WriteLine("\\end{tabular}");
                //sw.WriteLine("\\end{center}}");
                sw.WriteLine("}");

                //#####################################
                //SECTION 1
                //#####################################

                //sw.WriteLine("\\begin{center}");
                sw.WriteLine("\\section*{EXPERIMENT RESULTS}");
                //sw.WriteLine("\\end{center}");
                sw.WriteLine("\\begin{figure}[h]");
                sw.WriteLine("\\begin{center}");
                sw.WriteLine("\\includegraphics[width=0.5\\textwidth]{test}");
                sw.WriteLine("\\caption{Measured data}");
                sw.WriteLine("\\end{center}");
                sw.WriteLine("\\end{figure}");
                sw.WriteLine("\\begin{center}");
                sw.WriteLine("\\begin{tabular}{c c c c c}");
                sw.WriteLine("{\\sizetwo \\textbf{ DURATION} }& & \\sizetwo \\textbf{TEST VOLTAGE} & & \\sizetwo \\textbf{PASS/FAIL STATUS} \\\\");
                sw.WriteLine("& & \\\\");
                //sw.WriteLine(" \\sizeone \\textbf{{0}} &  &",label10.Text);
                sw.WriteLine("\\sizeone \\textbf{");
                sw.WriteLine("{0}", duration);
                sw.WriteLine("} & &");

                // sw.WriteLine("{\\sizeone \\textbf{ {0}}} & & ");
                sw.WriteLine("\\sizeone \\textbf{");
                sw.WriteLine("{0}", testVoltage);
                sw.WriteLine("} & &");

                //sw.WriteLine("{\\sizeone  \\textbf{ {0}}} \\\\");
                sw.WriteLine("\\sizeone \\textbf{");
                sw.WriteLine("{0}", statusPassFail);
                sw.WriteLine("} \\\\");

                sw.WriteLine("{\\sizethree Seconds }& &{\\sizethree kV} &&  \\\\");
                sw.WriteLine("& &\\footnotesize{ $ \\pm 15 \\%$}& & \\footnotesize{e 46 seconds} \\\\");
                sw.WriteLine("\\end{tabular}");
                sw.WriteLine("\\end{center}");
                sw.WriteLine("\\end{document}");



                // }

            }

        }
    }
}
