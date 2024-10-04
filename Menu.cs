using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CancellationTest
{
    public struct testParameters
    {
        public AvailableExams testType;
        public string patientName;
        public double adjustmentRatio;
        public int examTime;
        public int crossOutTime;
    }
    public partial class Menu : Form
    {

        private testParameters testParameters;

        private DateTime OrginPoint = new DateTime(2020, 1, 1, 0, 0, 0);
        private DateTime MaxTime = new DateTime(2020, 1, 1, 1, 0, 0);
        private DateTime MinTime = new DateTime(2020, 1, 1, 0, 0, 5);
        private DateTime dateTimePicker1_startingPoint = new DateTime(2020, 1, 1, 0, 4, 0);
        private DateTime dateTimePicker_PreviousTime;

        private bool dateTimePicker_Navigating = false;
        private TimeSpan dateTimePicker_timeIncrement = new TimeSpan(0, 0, 5);

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label Patient;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button button3;


        public Menu()
        {
            InitializeComponent();
            this.testParameters = new testParameters();
            this.testParameters.testType = AvailableExams.Assessment;
            this.testParameters.patientName = "Unknown";
            this.testParameters.adjustmentRatio = 1.0;
            this.testParameters.examTime = 240;
            this.testParameters.crossOutTime = -1;

            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            //move the groupbox to the center of the form
           


            this.groupBox1.Left = (this.Width - this.groupBox1.Width) / 2;
            this.groupBox1.Top = (this.Height - this.groupBox1.Height) / 2;

            Console.WriteLine("Screen Width " + this.Width);
            Console.WriteLine("Screen Heigth " + this.Height);
        }

        public Menu(testParameters testParameters)
        {
            InitializeComponent();
            this.testParameters = testParameters;

            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.groupBox1.Left = (this.Width - this.groupBox1.Width) / 2;
            this.groupBox1.Top = (this.Height - this.groupBox1.Height) / 2;
        }


        private void loadTest(object sender, EventArgs e)
        {
            

            //Set the right exam type in the exam parameters
            if (comboBox1.SelectedIndex == 0)
            {
                this.testParameters.testType = AvailableExams.Practice_1;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                this.testParameters.testType = AvailableExams.Practice_2;
            }
            else
            {
                this.testParameters.testType = AvailableExams.Assessment;
            }


            this.testParameters.examTime = (int)(dateTimePicker1.Value - OrginPoint).TotalSeconds;
            Console.WriteLine("Exam Time: " + this.testParameters.examTime);


            //Check if a patient Name has been entered else use unknown
            if (textBox1.Text != "")
            {
                this.testParameters.patientName = textBox1.Text;
            }
            else
            {
                this.testParameters.patientName = "Unknown";
            }

            //If the cross out check box is checked then set the cross out time to 5 seconds
            this.testParameters.crossOutTime = this.checkBox1.Checked ? 5 : -1;

            IntermediateScreen intermediateScreen = new IntermediateScreen(this.testParameters);
            this.Hide();

            intermediateScreen.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            screenSizeAdjustment screenSizeAdjustment = new screenSizeAdjustment(this.testParameters);
            this.Hide();

            screenSizeAdjustment.ShowDialog();
            this.testParameters.adjustmentRatio = screenSizeAdjustment.screenSize;

            this.Show();
            Console.WriteLine("Screen Size: " + this.testParameters.adjustmentRatio);
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {
            if (!dateTimePicker_Navigating)
            {
                /* First set the navigating flag to true so this method doesn't get called again while updating */
                dateTimePicker_Navigating = true;

                
                if(dateTimePicker_PreviousTime == new DateTime(2020, 1, 1, 0, 0, 55))
                {
                    Console.WriteLine("Previous Time" + dateTimePicker_PreviousTime.ToString() + " " + dateTimePicker1.Value.ToString() + " " + dateTimePicker1.Value.Hour + " " + dateTimePicker1.Value.Minute + " " + dateTimePicker1.Value.Second);
                    Console.WriteLine("First Time");
                }

                //Detecting a wrap around
                
                if (dateTimePicker_PreviousTime.Minute == 0 &&
                    dateTimePicker1.Value.Minute == 59)
                {
                    decreaseTime();
                }
                else if (dateTimePicker_PreviousTime.Minute == 59 &&
                    dateTimePicker1.Value.Minute == 0)
                {
                    increaseTime();
                }
                //If the difference is less than the previous time, then the user wants decrement the time
                else if (dateTimePicker_PreviousTime < dateTimePicker1.Value)
                {
                    increaseTime();
                }
                else
                {
                    decreaseTime();
                }

                
                //Check that 
                dateTimePicker_Navigating = false;
                dateTimePicker_PreviousTime = dateTimePicker1.Value;
            }
            
        }

        private void decreaseTime()
        {
            DateTime tempDT = dateTimePicker_PreviousTime.Subtract(dateTimePicker_timeIncrement);

            //Check if the time is greater than the maximum time -> set to maximum time
            if (tempDT <= MinTime)
            {
                dateTimePicker1.Value = MinTime;
            }
            else
            {
                dateTimePicker1.Value = tempDT;
            }
        }

        private void increaseTime()
        {
            DateTime tempDT = dateTimePicker_PreviousTime.Add(dateTimePicker_timeIncrement);

            //Check if the time is less than the minimum time -> set to minimum time
            if (dateTimePicker1.Value >= MaxTime)
            {
                dateTimePicker1.Value = MaxTime;
            }
            else
            {
                dateTimePicker1.Value = tempDT;
            }

            
        }

        private int percentScreenWidth(double n)
        {
            return (int)Math.Round((this.Width * n) / 100);
        }

        private int percentScreenHeight(double n)
        {
            return (int)Math.Round((this.Height * n) / 100);
        }

        private void InitializeComponent()
        {
            
            
            
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Patient = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            //this.groupBox1.SuspendLayout();
            //this.groupBox5.SuspendLayout();
            //this.groupBox4.SuspendLayout();
            //this.groupBox3.SuspendLayout();
            //((System.ComponentModel.ISupportInitialize)(this.dateTimePicker1)).BeginInit();
            //this.groupBox2.SuspendLayout();
            //this.SuspendLayout();
            // 
            // a4GroupBox
            // 
            this.groupBox1.BackColor = System.Drawing.Color.LightGray;
            
            this.groupBox1.Location = new System.Drawing.Point(relativeScreenWidth(0.070), relativeScreenHeight(0.042));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(relativeScreenWidth(0.7), relativeScreenHeight(0.60));
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // currentRatioBox
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.textBox1.Location = new System.Drawing.Point(relativeScreenWidth(0.125), relativeScreenHeight(0.021));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(relativeScreenWidth(0.15), relativeScreenHeight(0.072));
            this.textBox1.TabIndex = 7;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox5.Controls.Add(this.button3);
            this.groupBox5.Location = new System.Drawing.Point(relativeScreenWidth(0.004), relativeScreenHeight(0.478));
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(relativeScreenWidth(0.69), relativeScreenHeight(0.106));
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            // 
            // decreaseButton
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.button3.Location = new System.Drawing.Point(relativeScreenWidth(0.25), relativeScreenHeight(0.0289));
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(relativeScreenWidth(0.16), relativeScreenHeight(0.048));
            this.button3.TabIndex = 0;
            this.button3.Text = "Adjust Screen size";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox4.Controls.Add(this.checkBox1);
            this.groupBox4.Location = new System.Drawing.Point(relativeScreenWidth(0.004), relativeScreenHeight(0.369));
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(relativeScreenWidth(0.320), relativeScreenHeight(0.1));
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(relativeScreenWidth(0.007), relativeScreenHeight(0.016));
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(relativeScreenWidth(0.248), relativeScreenHeight(0.038));
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Invisable Cancellation Condition";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            
            this.groupBox3.Location = new System.Drawing.Point(relativeScreenWidth(0.342), relativeScreenHeight(0.369));
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(relativeScreenWidth(0.347), relativeScreenHeight(0.100));
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.dateTimePicker1.Location = new System.Drawing.Point(312, 19);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(relativeScreenWidth(0.132), relativeScreenHeight(0.072));
            this.dateTimePicker1.TabIndex = 10;
            this.dateTimePicker1.CustomFormat = "mm:ss";
            this.dateTimePicker1.Format = DateTimePickerFormat.Custom;
            //Set the current time ot 4:00
            this.dateTimePicker1.Value = dateTimePicker1_startingPoint;
            this.dateTimePicker_PreviousTime = dateTimePicker1_startingPoint;
            this.dateTimePicker1.ShowUpDown = true;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePickerStart_ValueChanged);
            
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(relativeScreenWidth(0.004), relativeScreenHeight(0.015));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(relativeScreenWidth(0.163), relativeScreenHeight(0.082));
            this.label1.TabIndex = 9;
            this.label1.Text = "Maximum Time for excersize in seconds";
            // 
            // increaseButton
            // 
            this.button2.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(relativeScreenWidth(0.566), relativeScreenHeight(0.020));
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(relativeScreenWidth(0.11), relativeScreenHeight(0.073));
            this.button2.TabIndex = 3;
            this.button2.Text = "Start";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.loadTest);
            // 
            // okayButton
            // 
            this.button1.BackColor = System.Drawing.Color.DimGray;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(relativeScreenWidth(0.448), relativeScreenHeight(0.020));
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(relativeScreenWidth(0.108), relativeScreenHeight(0.073));
            this.button1.TabIndex = 2;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Patient
            // 
            this.Patient.AutoSize = true;
            this.Patient.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Patient.Location = new System.Drawing.Point(relativeScreenWidth(0.004), relativeScreenHeight(0.026));
            //this.Patient.Location = new System.Drawing.Point(6, 22);
            this.Patient.Name = "Patient";
            this.Patient.Size = new System.Drawing.Size(relativeScreenWidth(.12), relativeScreenHeight(0.063));
            this.Patient.TabIndex = 1;
            this.Patient.Text = "Patient";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Location = new System.Drawing.Point(relativeScreenWidth(0.004), relativeScreenHeight(0.1));
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(relativeScreenWidth(0.69), relativeScreenHeight(0.258));
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(relativeScreenWidth(0.0065), relativeScreenHeight(0.030));
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(relativeScreenWidth(0.40), relativeScreenHeight(0.20));
            this.comboBox1.TabIndex = 0;
            this.comboBox1.Items.AddRange(new String[] {
                "Pratice Test 1",
                "Pratice Test 2",
                "Assessment" });
            this.comboBox1.SelectedIndex = 2;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            //Prevent the combo box from being edited
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            

            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(relativeScreenWidth(0.86), relativeScreenHeight(0.715));
            this.Controls.Add(this.groupBox1);
            this.Name = "Menu";
            this.Text = "Cancellation Test Menu";
            this.Load += new System.EventHandler(this.Menu_Load);


            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.Patient);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox3.Controls.Add(this.dateTimePicker1);
            this.groupBox3.Controls.Add(this.label1);
            //this.groupBox1.ResumeLayout(false);
            //this.groupBox1.PerformLayout();
            //this.groupBox5.ResumeLayout(false);
            //this.groupBox4.ResumeLayout(false);
            //this.groupBox4.PerformLayout();
            //this.groupBox3.ResumeLayout(false);
            //((System.ComponentModel.ISupportInitialize)(this.dateTimePicker1)).EndInit();
            //this.groupBox2.ResumeLayout(false);
            //this.ResumeLayout(false);

        }

        private int relativeScreenHeight(double n)
        {
            return (int)Math.Round(Screen.PrimaryScreen.Bounds.Height * n * 1.25);
        }

        private int relativeScreenWidth(double n)
        {
            //Multiple the screen width by the percentage
            return (int)Math.Round(Screen.PrimaryScreen.Bounds.Width * n * 1.25);    
        }
    }
}
