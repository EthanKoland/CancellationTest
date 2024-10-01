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
        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        private testParameters testParameters;

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;




        public IntermediateScreen()
        {
            InitializeComponent();

            this.testParameters = new testParameters();
            this.testParameters.testType = AvailableExams.Assessment;
            this.testParameters.patientName = "Unknown";
            this.testParameters.adjustmentRatio = 1.0;
            this.testParameters.examTime = 240;

            

            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.WindowState = FormWindowState.Maximized;

            IntermediateScreen_Load();
            
        }

        public IntermediateScreen(testParameters testParameters)
        {
            InitializeComponent();
            this.testParameters = testParameters;
            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.WindowState = FormWindowState.Maximized;

            //this.groupBox1.Left = (this.Width - this.groupBox1.Width) / 2;
            //this.groupBox1.Top = (this.Height - this.groupBox1.Height) / 2;

            IntermediateScreen_Load();
        }

        private void IntermediateScreen_Load()
        {
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            WMPLib.WindowsMediaPlayer wplayer2 = new WMPLib.WindowsMediaPlayer();

            wplayer2.URL = "Sounds\\Instructions.mp3";

            this.player.SoundLocation = @"popping.wav";
            
            //Play the Instructions Mp3
            wplayer2.controls.play();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Button Clicked");

            abstractTestClass exam;

            if (this.testParameters.testType == AvailableExams.Assessment)
            {
                exam = new assesmentExam();
            }
            else if (this.testParameters.testType == AvailableExams.Practice_1)
            {
                //am = new Practice_1(this.testParameters);
                exam = new assesmentExam();
            }
            else if (this.testParameters.testType == AvailableExams.Practice_2)
            {
                //am = new Practice_2(this.testParameters);
                exam = new assesmentExam();
            }
            else
            {
                exam = new assesmentExam();
            }
            //this.wplayer.controls.play();

            //player.Stop();

            WMPLib.WindowsMediaPlayer wplayer2 = new WMPLib.WindowsMediaPlayer();

            wplayer2.URL = @"C:\Users\ethan\Desktop\Cancellation Transfer\CancellationTest\CancellationTest\Sounds\Instructions.mp3";

            SoundPlayer simpleSound = new SoundPlayer(@"CSounds\popping.wav");
            Console.WriteLine(Application.StartupPath + "\\Sounds\\popping.wav");
            //simpleSound.Play();

            this.player.SoundLocation = "popping.wav";

            //Play the player
            //this.player.Play();

            //Play the Instructions Mp3
            //wplayer2.controls.play();




            if (this.testParameters.testType != AvailableExams.Assessment)
            {
                //Tericary statement id test type is pratice1 then exam = new praticeExam1() else exam = new praticeExam2()
                praticeParent praticeParent = new praticeParent(exam, this.testParameters.adjustmentRatio, this.testParameters.examTime, this.testParameters.patientName);
                this.Hide();
                praticeParent.Show();

            }
            else
            {
                examParent examParent = new examParent(exam, this.testParameters.adjustmentRatio, this.testParameters.examTime, this.testParameters.patientName);
                this.Hide();
                examParent.Show();
            }

            //examParent examParent = new examParent(exam, this.testParameters.adjustmentRatio, this.testParameters.examTime, this.testParameters.patientName);

            //this.Hide();

            //examParent.Show();

        }

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            //this.groupBox1.SuspendLayout();
            //this.SuspendLayout();


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
            //Set the text Size - Set the font ot 8% of the screen height
            this.button1.Font = new Font("Arial", (int) Math.Round(screenHeight * 0.04));
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            //this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point((int)Math.Round(groupBoxWidth * 0.1), (int)Math.Round(groupBoxHeight * 0.4));
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size((int)Math.Round(groupBoxWidth * 0.8), (int)Math.Round(groupBoxHeight * 0.15));
            this.label3.Font = new Font("Arial", (int)Math.Round(groupBoxHeight * 0.04));
            this.label3.TabIndex = 2;
            this.label3.Text = "Please click all the complete mugs (those with no gaps).";
            // 
            // label2
            // 
            //this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point((int)Math.Round(groupBoxWidth * 0.1), (int)Math.Round(groupBoxHeight * 0.6));
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size((int)Math.Round(groupBoxWidth * 0.8), (int)Math.Round(groupBoxHeight * 0.15));
            this.label2.Font = new Font("Arial", (int)Math.Round(groupBoxHeight * 0.04));
            this.label2.TabIndex = 1;
            this.label2.Text = "Some Mugs will be complete and some will have a gap on the handle.";
            this.label2.Click += new System.EventHandler(this.label2_Click);
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
            this.label1.Click += new System.EventHandler(this.label1_Click);
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
            //this.groupBox1.ResumeLayout(false);
            //this.groupBox1.PerformLayout();
            //this.ResumeLayout(false);

        }
    }
}
