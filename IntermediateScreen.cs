using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace CancellationTest
{
    public partial class IntermediateScreen : Form
    {

        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
        
        private testParameters testParameters;


        //----------------------Form Components----------------------//
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;




        public IntermediateScreen()
        {
            InitializeComponent();

            //Sample test parameters are create to be used for debuging this page. These are not normally within the program path
            this.testParameters = new testParameters();
            this.testParameters.testType = AvailableExams.Assessment;
            this.testParameters.patientName = "Unknown";
            this.testParameters.adjustmentRatio = 1.0;
            this.testParameters.examTime = 240;

            IntermediateScreen_Load();
            
        }

        public IntermediateScreen(testParameters testParameters)
        {
            InitializeComponent();
            this.testParameters = testParameters;
            
            IntermediateScreen_Load();
        }

        private void IntermediateScreen_Load()
        {
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            //Play the Instructions Mp3
            wplayer.URL = "Sounds\\Instructions.mp3";
            wplayer.controls.play();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            this.wplayer.controls.stop();

            //Create the exam object to be used in the examParent outside of the if statement for persistence
            abstractTestClass exam;

            if (this.testParameters.testType == AvailableExams.Assessment)
            {
                exam = new assesmentExam();
            }
            else if (this.testParameters.testType == AvailableExams.Practice_1)
            {
                exam = new praticeExam1();
                
            }
            else if (this.testParameters.testType == AvailableExams.Practice_2)
            {
                exam = new praticeExam2();
            }
            else
            {
                exam = new assesmentExam();
            }
            

            //If the test is not an assessment, then we need to create a practiceParent object because the only other option our practice tests
            if (this.testParameters.testType != AvailableExams.Assessment)
            {
                int praticeNum = this.testParameters.testType == AvailableExams.Practice_1 ? 1 : 2;
                praticeParent praticeParent = new praticeParent(exam, praticeNum, this.testParameters.adjustmentRatio, this.testParameters.examTime, this.testParameters.patientName, this.testParameters.crossOutTime);
                this.Hide();
                praticeParent.Show();

            }
            else
            {
                examParent examParent = new examParent(exam, this.testParameters.adjustmentRatio, this.testParameters.examTime, this.testParameters.patientName, this.testParameters.crossOutTime);
                this.Hide();
                examParent.Show();
            }

            
        }

        private void InitializeComponent()
        {

            //Creating the form elements
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();


            //A variable is used to store the screen width and height to reduce the number of calls and increase readability
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            int groupBoxWidth = (int)Math.Round(screenWidth * 0.6);
            int groupBoxHeight = (int)Math.Round(screenHeight * 0.8);

            // 
            // a4GroupBox
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point((int) Math.Round(screenWidth * 0.2), (int) Math.Round(screenHeight * 0.1));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(groupBoxWidth, groupBoxHeight);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // okayButton
            // 
            this.button1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.button1.Location = new System.Drawing.Point((int) Math.Round(groupBoxWidth * 0.4), (int) Math.Round(groupBoxHeight * 0.8));
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size((int) Math.Round(groupBoxWidth * 0.2), (int) Math.Round(groupBoxHeight * 0.15));
            this.button1.TabIndex = 3;
            this.button1.Text = "OK";
            this.button1.Font = new Font("Arial", (int) Math.Round(screenHeight * 0.04));
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point((int)Math.Round(groupBoxWidth * 0.1), (int)Math.Round(groupBoxHeight * 0.4));
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size((int)Math.Round(groupBoxWidth * 0.8), (int)Math.Round(groupBoxHeight * 0.15));
            this.label3.Font = new Font("Arial", (int)Math.Round(groupBoxHeight * 0.04));
            this.label3.TabIndex = 2;
            this.label3.Text = "Some Mugs will be complete and some will have a gap on the handle";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point((int)Math.Round(groupBoxWidth * 0.1), (int)Math.Round(groupBoxHeight * 0.6));
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size((int)Math.Round(groupBoxWidth * 0.8), (int)Math.Round(groupBoxHeight * 0.15));
            this.label2.Font = new Font("Arial", (int)Math.Round(groupBoxHeight * 0.04));
            this.label2.TabIndex = 1;
            this.label2.Text = "Please click all the complete mugs (those with no gaps).";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point((int)Math.Round(groupBoxWidth * 0.1), (int)Math.Round(groupBoxHeight * 0.2));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size((int)Math.Round(groupBoxWidth * 0.8), (int)Math.Round(groupBoxHeight * 0.15));
            this.label1.Font = new Font("Arial", (int)Math.Round(groupBoxHeight * 0.04));
            this.label1.TabIndex = 0;
            this.label1.Text = "You will see a screen with lots of mugs on.";
            // 
            // IntermediateScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox1);
            this.Name = "IntermediateScreen";
            this.Text = "Form1";

        }
    }
}
