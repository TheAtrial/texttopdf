using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing.Printing;
using System.Diagnostics;


namespace texttopdf
{


    public partial class Form1 : Form
    {
        private System.Drawing.Font verdana10Font;
        private StreamReader reader;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog file = new OpenFileDialog())
            {
                //Show the Dialog Box & Select the File
                file.ShowDialog();
                //Assign Input FileName to TextBox
                textBox1.Text = file.FileName;
            }

            //Assign Output FileName to TextBox

            textBox2.Text = (textBox1.Text).Replace(".txt", ".pdf");



        }

        private void button2_Click(object sender, EventArgs e)
        {
            string putt = Path.GetExtension(textBox1.Text);
            if (textBox1.Text == "")
            {
                label1.Text = "Вы не указали исходный файл!";
            }
            else if (textBox2.Text == "")
            {
                label1.Text = "Конечный путь не обнаружен!";
            }
            else if (putt != ".txt")
            {
                label1.Text = "Формат файла некорректный!";
            }
            //Read the Data from Input File
            else
            {

                //string putt = Path.GetExtension(textBox1.Text);
                //MessageBox.Show(putt);
                var encoding = Encoding.GetEncoding(1251);

                StreamReader rdr = new StreamReader(textBox1.Text, System.Text.Encoding.GetEncoding("utf-8")) ;
                //MessageBox.Show(rdr.ReadToEnd());
                //Create a New instance on Document Class

                Document doc = new Document();

                //Create a New instance of PDFWriter Class for Output File

                PdfWriter.GetInstance(doc, new FileStream(textBox2.Text, FileMode.Create));

                verdana10Font = new System.Drawing.Font("Verdana", 10);

                //Open the Document

                iTextSharp.text.Font font1 = FontFactory.GetFont("c:/Windows/Fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);



                doc.Open();

                //Add the content of Text File to PDF File

                Chunk chunk = new Chunk(rdr.ReadToEnd(), font1);

                doc.Add(new Paragraph(chunk));

                //Close the Document

                doc.Close();

                 MessageBox.Show("Конвертирование выполнено успешно....");

                //Open the Converted PDF File

                System.Diagnostics.Process.Start(textBox2.Text); 
                /*
                string filename = textBox1.Text.ToString();
                //потоковая читалка
                reader = new StreamReader(filename);
                //задаем шрифт 
                verdana10Font = new System.Drawing.Font("Verdana", 10);
                //создать док-т для печати  
                PrintDocument pd = new PrintDocument();
                //добавить страницу для печати в метод  
                pd.PrintPage += new PrintPageEventHandler(this.PrintTextFileHandler);
                //вызвать метод принт  
                pd.Print();
                //закрыть читалку  
                if (reader != null)
                    reader.Close();*/
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 newForm = new Form3();
            newForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2();
            newForm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {

            string filename = textBox1.Text.ToString();
            if (filename == "")
            {
                label1.Text = "Неизвестно что печатать!";
            }
            else
            {
                PrintToASpecificPrinter();
            }


        }
        private void PrintTextFileHandler(object sender, PrintPageEventArgs ppeArgs)
        {
            //Get the Graphics object  
            Graphics g = ppeArgs.Graphics;
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            //Read margins from PrintPageEventArgs  
            float leftMargin = ppeArgs.MarginBounds.Left;
            float topMargin = ppeArgs.MarginBounds.Top;
            string line = null;
            //Calculate the lines per page on the basis of the height of the page and the height of the font  
            linesPerPage = ppeArgs.MarginBounds.Height / verdana10Font.GetHeight(g);
            //Now read lines one by one, using StreamReader  
            while (count < linesPerPage && ((line = reader.ReadLine()) != null))
            {
                //Calculate the starting position  
                yPos = topMargin + (count * verdana10Font.GetHeight(g));
                //Draw text  
                g.DrawString(line, verdana10Font, Brushes.Black, leftMargin, yPos, new StringFormat());
                //Move to next line  
                count++;
            }
            //If PrintPageEventArgs has more pages to print  
            if (line != null)
            {
                ppeArgs.HasMorePages = true;
            }
            else
            {
                ppeArgs.HasMorePages = false;
            }
        }

        public void PrintToASpecificPrinter()
        {
            using (PrintDialog printDialog = new PrintDialog())
            {
                printDialog.AllowSomePages = true;
                printDialog.AllowSelection = true;
                string fileName = textBox2.Text.ToString();
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    var StartInfo = new ProcessStartInfo();
                    StartInfo.CreateNoWindow = true;
                    StartInfo.UseShellExecute = true;
                    StartInfo.Verb = "printTo";
                    StartInfo.Arguments = "\"" + printDialog.PrinterSettings.PrinterName + "\"";
                    StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    StartInfo.FileName = fileName;

                    Process.Start(StartInfo);
                }

            }


        }

    }
}