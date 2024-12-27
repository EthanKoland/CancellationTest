using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CancellationTest
{
    public partial class exportScreen : Form
    {
        private string patient_ID;
        private double adjustSize;
        private actionTracker tracker;

        private int screenHeight = 480;
        private int screenWidth = 640;
        
        private System.Windows.Forms.GroupBox comboGroupBox;
        private System.Windows.Forms.CheckBox checkBox_Map;
        private System.Windows.Forms.CheckBox checkBox_Txt;
        private System.Windows.Forms.CheckBox checkBox_Excel;
        private System.Windows.Forms.CheckBox checkBox_CSV;
        private System.Windows.Forms.CheckBox checkBox_PDF;
        private System.Windows.Forms.CheckBox checkBox_IMG;

        private System.Windows.Forms.Button button_Exit;
        private System.Windows.Forms.Button button_Export;
        private System.Windows.Forms.Button button_Menu;
        private System.Windows.Forms.Label label_Title;


        public exportScreen(string patient_ID, actionTracker tracker, double adjustSize = 1.0 )
        {
            this.adjustSize = adjustSize;
            this.patient_ID = patient_ID;
            this.tracker = tracker;

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Export Screen";
            this.TopMost = true;
            this.ClientSize = new System.Drawing.Size(screenWidth, screenHeight);
            this.MinimizeBox = false;
            this.MaximizeBox = false;



            this.comboGroupBox = new System.Windows.Forms.GroupBox();
            this.checkBox_Map = new System.Windows.Forms.CheckBox();
            this.checkBox_Txt = new System.Windows.Forms.CheckBox();
            this.checkBox_Excel = new System.Windows.Forms.CheckBox();
            this.checkBox_CSV = new System.Windows.Forms.CheckBox();
            this.checkBox_PDF = new System.Windows.Forms.CheckBox();
            this.checkBox_IMG = new System.Windows.Forms.CheckBox();
            this.button_Exit = new System.Windows.Forms.Button();
            this.button_Menu = new System.Windows.Forms.Button();
            this.button_Export = new System.Windows.Forms.Button();
            this.label_Title = new System.Windows.Forms.Label();

            this.comboGroupBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.comboGroupBox.Controls.Add(this.label_Title);
            this.comboGroupBox.Controls.Add(this.checkBox_Map);
            this.comboGroupBox.Controls.Add(this.checkBox_Txt);
            this.comboGroupBox.Controls.Add(this.checkBox_Excel);
            this.comboGroupBox.Controls.Add(this.checkBox_CSV);
            this.comboGroupBox.Controls.Add(this.checkBox_PDF);
            this.comboGroupBox.Controls.Add(this.checkBox_IMG);
            //this.comboGroupBox.Controls.Add(this.button_Menu);
            this.comboGroupBox.Controls.Add(this.button_Exit);
            this.comboGroupBox.Controls.Add(this.button_Export);
            this.comboGroupBox.Location = new System.Drawing.Point((int)(screenWidth * 0.05), (int)(screenHeight * 0.05));
            this.comboGroupBox.Size = new System.Drawing.Size((int)(screenWidth * 0.9), (int)(screenHeight * 0.9));
            this.Controls.Add(this.comboGroupBox);

            this.label_Title.AutoSize = true;
            this.label_Title.Location = new System.Drawing.Point(25,25);
            this.label_Title.Name = "labelTitle";
            this.label_Title.Size = new System.Drawing.Size((int)Math.Round(screenWidth * 0.8), (int)Math.Round(screenHeight * 0.15));
            this.label_Title.Font = new Font("Arial", (int)Math.Round(screenHeight * 0.04));
            this.label_Title.TabIndex = 0;
            this.label_Title.Text = "Export Test for Patient: " + this.patient_ID;

            this.checkBox_Map.AutoSize = true;
            this.checkBox_Map.Location = new System.Drawing.Point((int)(screenWidth * 0.1), (int)(screenHeight * 0.2));
            this.checkBox_Map.Name = "checkBox_Map";
            this.checkBox_Map.Size = new System.Drawing.Size((int)(screenWidth * 0.8), (int)(screenHeight * 0.15));
            this.checkBox_Map.Font = new Font("Arial", (int)Math.Round(screenHeight * 0.04));
            this.checkBox_Map.TabIndex = 1;
            this.checkBox_Map.Text = "Export Map Data";

            this.checkBox_Txt.AutoSize = true;
            this.checkBox_Txt.Location = new System.Drawing.Point((int)(screenWidth * 0.1), (int)(screenHeight * 0.3));
            this.checkBox_Txt.Name = "checkBox_Txt";
            this.checkBox_Txt.Size = new System.Drawing.Size((int)(screenWidth * 0.8), (int)(screenHeight * 0.15));
            this.checkBox_Txt.Font = new Font("Arial", (int)Math.Round(screenHeight * 0.04));
            this.checkBox_Txt.TabIndex = 2;
            this.checkBox_Txt.Text = "Export as Txt File";

            this.checkBox_Excel.AutoSize = true;
            this.checkBox_Excel.Location = new System.Drawing.Point((int)(screenWidth * 0.1), (int)(screenHeight * 0.4));
            this.checkBox_Excel.Name = "checkBox_Excel";
            this.checkBox_Excel.Size = new System.Drawing.Size((int)(screenWidth * 0.8), (int)(screenHeight * 0.15));
            this.checkBox_Excel.Font = new Font("Arial", (int)Math.Round(screenHeight * 0.04));
            this.checkBox_Excel.TabIndex = 3;
            this.checkBox_Excel.Text = "Export as Excel File";

            this.checkBox_CSV.AutoSize = true;
            this.checkBox_CSV.Location = new System.Drawing.Point((int)(screenWidth * 0.1), (int)(screenHeight * 0.5));
            this.checkBox_CSV.Name = "checkBox_CSV";
            this.checkBox_CSV.Size = new System.Drawing.Size((int)(screenWidth * 0.8), (int)(screenHeight * 0.15));
            this.checkBox_CSV.Font = new Font("Arial", (int)Math.Round(screenHeight * 0.04));
            this.checkBox_CSV.TabIndex = 4;
            this.checkBox_CSV.Text = "Export as CSV File";

            this.checkBox_PDF.AutoSize = true;
            this.checkBox_PDF.Location = new System.Drawing.Point((int)(screenWidth * 0.1), (int)(screenHeight * 0.6));
            this.checkBox_PDF.Name = "checkBox_PDF";
            this.checkBox_PDF.Size = new System.Drawing.Size((int)(screenWidth * 0.8), (int)(screenHeight * 0.15));
            this.checkBox_PDF.Font = new Font("Arial", (int)Math.Round(screenHeight * 0.04));
            this.checkBox_PDF.TabIndex = 5;
            this.checkBox_PDF.Text = "Export as PDF File - In Dev";

            this.checkBox_IMG.AutoSize = true;
            this.checkBox_IMG.Location = new System.Drawing.Point((int)(screenWidth * 0.1), (int)(screenHeight * 0.7));
            this.checkBox_IMG.Name = "checkBox_IMG";
            this.checkBox_IMG.Size = new System.Drawing.Size((int)(screenWidth * 0.8), (int)(screenHeight * 0.15));
            this.checkBox_IMG.Font = new Font("Arial", (int)Math.Round(screenHeight * 0.04));
            this.checkBox_IMG.TabIndex = 6;
            this.checkBox_IMG.Text = "Export as Image File - In Dev";

            this.button_Export.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.button_Export.Location = new System.Drawing.Point((int)(screenWidth * 0), (int)(screenHeight * 0.8));
            this.button_Export.Name = "button_Export";
            this.button_Export.Size = new System.Drawing.Size((int)(screenWidth * 0.2), (int)(screenHeight * 0.11));
            this.button_Export.TabIndex = 7;
            this.button_Export.Text = "Export";
            this.button_Export.Click += new System.EventHandler(this.export);

            this.button_Exit.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.button_Exit.Location = new System.Drawing.Point((int)(screenWidth * 0.3), (int)(screenHeight * 0.8));
            this.button_Exit.Name = "button_Exit";
            this.button_Exit.Size = new System.Drawing.Size((int)(screenWidth * 0.2), (int)(screenHeight * 0.1));
            this.button_Exit.TabIndex = 8;
            this.button_Exit.Text = "Exit";
            this.button_Exit.Click += new System.EventHandler(this.exit);

            this.button_Menu.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.button_Menu.Location = new System.Drawing.Point((int)(screenWidth * 0.6), (int)(screenHeight * 0.8));
            this.button_Menu.Name = "button_Menu";
            this.button_Menu.Size = new System.Drawing.Size((int)(screenWidth * 0.2), (int)(screenHeight * 0.1));
            this.button_Menu.TabIndex = 9;
            this.button_Menu.Text = "Home";
            this.button_Menu.Click += new System.EventHandler(this.back);





        }

        private void export(object sender, EventArgs e)
        {
            List<abstractExportClass> exportList = new List<abstractExportClass>();

            if (this.checkBox_IMG != null) 
            { 
                Console.WriteLine("Exporting as Image");
            }
            if (this.checkBox_PDF != null)
            {
                Console.WriteLine("Exporting as PDF");
            }
            if (this.checkBox_CSV != null)
            {
                Console.WriteLine("Exporting as CSV");
                exportList.Add(new Export_CSV(this.patient_ID));
            }
            if (this.checkBox_Excel != null)
            {
                Console.WriteLine("Exporting as Excel");
                exportList.Add(new Export_XLSX(this.patient_ID));
            }
            if (this.checkBox_Txt != null)
            {
                Console.WriteLine("Exporting as Txt");
                exportList.Add(new Export_Txt(this.patient_ID));
            }
            if (this.checkBox_Map != null)
            {
                Console.WriteLine("Exporting as Map");
                exportList.Add(new Export_Map(this.patient_ID));
            }

            this.tracker.export(exportList);


        }

        private void exit(object sender, EventArgs e)
        {
            this.Close();
        }

        private void back(object sender, EventArgs e)
        {
            // Create a new instance of the testParameters class and set its properties
            testParameters parms = new testParameters
            {
                testType = AvailableExams.Assessment,
                patientName = this.patient_ID,
                adjustmentRatio = this.adjustSize,
                examTime = 240,
                crossOutTime = -1
            };

            // Create a new instance of the Menu form with the parameters
            Menu menu = new Menu(parms);

            // Show the Menu form
            Application.Run(menu);

            // Close the current form
            this.Hide();
        }


    }
}
