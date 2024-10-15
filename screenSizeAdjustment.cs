using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CancellationTest
{
    public partial class screenSizeAdjustment : Form
    {

        //Ratio of 40" 16:9 Screen to A4 Paper
        private double a4WidthRatio = 88.6 / 29.7;
        private double a4HeightRatio = 49.8/21.0;

        public double screenSize { get; private set; }

        private testParameters m_testParameters;

        private System.Windows.Forms.Button okayButton;
        private System.Windows.Forms.Button increaseButton;
        private System.Windows.Forms.Button decreaseButton;
        private System.Windows.Forms.TextBox currentRatioBox;
        private System.Windows.Forms.GroupBox a4GroupBox;
        private System.Windows.Forms.PictureBox a4Image;

        public screenSizeAdjustment()
        {
            
            //Sample Test Parameters used for testing the page
            testParameters testParameters = new testParameters();
            testParameters.testType = AvailableExams.Assessment;
            testParameters.patientName = "Unknown";
            testParameters.adjustmentRatio = 1.0;
            testParameters.examTime = 240;

            this.m_testParameters = testParameters;


            frameSetup();
        }

        public screenSizeAdjustment(testParameters testParameters)
        {
            this.screenSize = testParameters.adjustmentRatio;
            this.m_testParameters = testParameters;

            frameSetup();

        }

        public void frameSetup()
        {
            Console.WriteLine((int)(100 * Screen.PrimaryScreen.WorkingArea.Width / Screen.PrimaryScreen.Bounds.Width));
            Console.WriteLine(this.DeviceDpi);

            Console.WriteLine("Screen Width: " + Screen.PrimaryScreen.Bounds.Width);

            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.screenSize = 1.0;

            InitializeComponent(); 

            
            //this.currentRatioBox.Enter += new EventHandler(textBox1_Enter);
            this.currentRatioBox.ReadOnly = true;

            

            

            //adjust();
            
        }



        private void increaseButton_Click(object sender, EventArgs e)
        {
            this.screenSize += 0.01;
            adjust();
        }

        private void decreaseButton_Click(object sender, EventArgs e)
        {
            this.screenSize -= 0.01;
            adjust();
        }

        private void adjust()
        {
            //this.a4GroupBox.Show();

            Console.WriteLine("g" + Screen.PrimaryScreen.Bounds.Width + " " + Screen.PrimaryScreen.Bounds.Height);

            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            
            Console.WriteLine("screenSize: " + this.screenSize);
            Console.WriteLine("Current Window Width: " + this.Width);
            Console.WriteLine("Current Window Height: " + this.Height);
            currentRatioBox.Text = this.screenSize.ToString();
            //this.a4GroupBox.Width = (int)(this.screenSize * a4WidthRatio * 100);
            //this.a4GroupBox.Height = (int)(this.screenSize * a4HeightRatio * 100);

            //this.a4GroupBox.Left = (this.Width - this.a4GroupBox.Width) / 2;
            //this.a4GroupBox.Top = (this.Height - this.a4GroupBox.Height) / 2 - this.currentRatioBox.Height - this.okayButton.Height;

            //this.a4Image.Width = (int)(this.screenSize * a4WidthRatio * 100);
            //this.a4Image.Height = (int)(this.screenSize * a4HeightRatio * 100);
            //this.a4Image.Image = ResizeImage(Image.FromFile("A4_ScalePaper.jpg"), this.a4Image.Width, this.a4Image.Height);

            int imageWidth = (int)(this.Width/a4WidthRatio * this.screenSize);
            int imageHeight = (int)(this.Height/a4HeightRatio * this.screenSize);
            Console.WriteLine("Image Width: " + imageWidth);
            Console.WriteLine("Image Height: " + imageHeight);
            Console.WriteLine("Difference: " + Screen.PrimaryScreen.Bounds.Height + " " + this.Height + " " + this.Height / Screen.PrimaryScreen.Bounds.Height);
            Console.WriteLine("Difference: " + Screen.PrimaryScreen.Bounds.Width + " " + this.Width + " " + this.Width / Screen.PrimaryScreen.Bounds.Width);
            //this.a4Image.Image = ResizeImage(Image.FromFile("A4_ScalePaper.jpg"), imageWidth, imageHeight);
            this.a4Image.Width = imageWidth;
            this.a4Image.Height = imageHeight;
            this.a4Image.Image = ResizeImage(Image.FromFile("A4_ScalePaper.jpg"), imageWidth, imageHeight);

            this.a4Image.Left = (this.Width - this.a4Image.Width) / 2;
            this.a4Image.Top = (this.Height - this.a4Image.Height) / 2 - this.currentRatioBox.Height - this.okayButton.Height;

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Console.WriteLine("textBox1_Enter");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //this.screenSize = Convert.ToDouble(currentRatioBox.Text);
            Console.WriteLine("screenSize: " + currentRatioBox.Text);    
            adjust();
        }

        private void okayButton_Click_1(object sender, EventArgs e)
        {
            this.m_testParameters.adjustmentRatio = this.screenSize;
            this.Close();
        }

        private void InitializeComponent()
        {
            this.okayButton = new System.Windows.Forms.Button();
            this.increaseButton = new System.Windows.Forms.Button();
            this.decreaseButton = new System.Windows.Forms.Button();
            this.currentRatioBox = new System.Windows.Forms.TextBox();
            this.a4GroupBox = new System.Windows.Forms.GroupBox();
            this.a4Image = new System.Windows.Forms.PictureBox();

            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            int testScreenWidth = 1518;
            int testScreenHeight = 917;

            double adjsutmentWidthRatio = (double)screenWidth / testScreenWidth;
            double adjsutmentHeightRatio = (double)screenHeight / testScreenHeight;
   
            //Lambda functino to adjust the width and height of the form
            Func<int, int> adjustWidth = (int width) => (int)(width * adjsutmentWidthRatio);
            Func<int, int> adjustHeight = (int height) => (int)(height * adjsutmentHeightRatio);
        


            Console.WriteLine("screenWidth: " + screenWidth);
            Console.WriteLine("screenHeight: " + screenHeight);



            //this.SuspendLayout();
            // 
            // okayButton
            // 
            this.okayButton.BackColor = System.Drawing.Color.DeepSkyBlue;
            
            //this.okayButton.Location = new System.Drawing.Point(adjustWidth(674), adjustHeight(808));
            this.okayButton.Top = (int)(screenHeight * 0.9);
            this.okayButton.Left = (int)(screenWidth * 0.45);
            this.okayButton.Name = "button1";
            this.okayButton.Size = new System.Drawing.Size((int)(screenWidth * 0.1), (int)(0.08 * screenHeight));
            this.okayButton.TabIndex = 0;
            this.okayButton.Text = "Okay";
            this.okayButton.UseVisualStyleBackColor = false;
            this.okayButton.Click += new System.EventHandler(this.okayButton_Click_1);
            // 
            // increaseButton
            // 
            this.increaseButton.BackColor = System.Drawing.Color.DarkGreen;
            //this.increaseButton.Location = new System.Drawing.Point(adjustWidth(870), adjustHeight(740));
            this.increaseButton.Top = (int)(screenHeight * 0.8);
            this.increaseButton.Left = (int)(screenWidth * 0.6);
            this.increaseButton.Name = "button2";
            this.increaseButton.Size = new System.Drawing.Size((int)(screenWidth * 0.1), (int)(0.08 * screenHeight));
            this.increaseButton.TabIndex = 1;
            this.increaseButton.Text = "Increase";
            this.increaseButton.UseVisualStyleBackColor = false;
            this.increaseButton.Click += new System.EventHandler(this.increaseButton_Click);
            // 
            // decreaseButton
            // 
            this.decreaseButton.BackColor = System.Drawing.Color.DarkRed;
            //this.decreaseButton.Location = new System.Drawing.Point(adjustWidth(583), adjustHeight(740));
            this.decreaseButton.Top = (int)(screenHeight * 0.8);
            this.decreaseButton.Left = (int)(screenWidth * 0.3);
            this.decreaseButton.Name = "button3";
            this.decreaseButton.Size = new System.Drawing.Size((int)(screenWidth * 0.1), (int)(0.08 * screenHeight));
            this.decreaseButton.TabIndex = 2;
            this.decreaseButton.Text = "Decrease";
            this.decreaseButton.UseVisualStyleBackColor = false;
            this.decreaseButton.Click += new System.EventHandler(this.decreaseButton_Click);
            // 
            // currentRatioBox
            // 
            this.currentRatioBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            //this.currentRatioBox.Location = new System.Drawing.Point(adjustWidth(679), adjustHeight(740));
            this.currentRatioBox.Top = (int)(screenHeight * 0.8);
            this.currentRatioBox.Left = (int)(screenWidth * 0.45);
            this.currentRatioBox.Name = "textBox1";
            this.currentRatioBox.Size = new System.Drawing.Size(adjustWidth(185), adjustHeight(62));
            this.currentRatioBox.TabIndex = 3;
            this.currentRatioBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // a4GroupBox
            // 
            this.a4GroupBox.BackColor = System.Drawing.Color.WhiteSmoke;

            //this.a4GroupBox.Location = new System.Drawing.Point(adjustWidth(479), adjustHeight(217));
            this.a4GroupBox.Width = (int)(this.Width / a4WidthRatio * this.screenSize);
            this.a4GroupBox.Height = (int)(this.Height / a4HeightRatio * this.screenSize);
            //this.a4GroupBox.Location = new System.Drawing.Point((this.Width - this.a4GroupBox.Width) / 2, 
                //(this.Height - this.a4GroupBox.Height) / 2 - adjustHeight(62) - this.okayButton.Height);
            
            //this.a4GroupBox.Left = (int)(screenWidth * 0.4);
            //this.a4GroupBox.Top = (int)(screenHeight * 0.3);
            //this.a4GroupBox.Location = new System.Drawing.Point(960,512);
            this.a4GroupBox.Name = "groupBox1";
            //this.a4GroupBox.Width = (int)(100);
            //this.a4GroupBox.Height = (int)(100);
            this.a4GroupBox.TabIndex = 4;
            this.a4GroupBox.TabStop = false;
            this.a4GroupBox.Text = "A4 Paper";
            this.a4GroupBox.Hide();
            //
            // a4Image
            //
            Image a4paper = Image.FromFile("A4_ScalePaper.jpg");
            this.a4Image.Image = ResizeImage(a4paper, (int)(this.Width / a4WidthRatio * this.screenSize), (int)(this.Height / a4HeightRatio * this.screenSize));
            
            //this.a4Image.Image = re

            //Resize the image to match the screen size
            this.a4Image.Location = new System.Drawing.Point((this.Width - this.a4Image.Width) / 2, 
                (this.Height - this.a4Image.Height) / 2 - adjustHeight(62) - this.okayButton.Height);



            // 
            // screenSizeAdjustment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            //this.ClientSize = new System.Drawing.Size(adjustWidth(1496), adjustHeight(861));
            this.Controls.Add(this.a4GroupBox);
            this.Controls.Add(this.currentRatioBox);
            this.Controls.Add(this.decreaseButton);
            this.Controls.Add(this.increaseButton);
            this.Controls.Add(this.okayButton);
            this.Controls.Add(this.a4Image);
            this.Name = "screenSizeAdjustment";
            this.Text = "screenSizeAdjustment";
            Console.WriteLine("screenWidth: " + this.Width);
            Console.WriteLine("screenHeight: " + this.Height);
            Console.WriteLine("A4 Ratio: " + a4WidthRatio + " " + a4HeightRatio);
            Console.WriteLine("Screen Size" + this.screenSize);
            this.Load += new EventHandler(initalLoadCalculations);
            //this.ResumeLayout(false);
            //this.PerformLayout();

        }

        private Image ResizeImage(Image image, int width, int height)
        {
            // Create a new bitmap with the desired dimensions
            Bitmap resizedImage = new Bitmap(width, height);

            // Use a graphics object to draw the resized image
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                // Set the interpolation mode for high quality resizing
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                // Draw the original image onto the new bitmap with the new dimensions
                graphics.DrawImage(image, 0, 0, width, height);
            }

            Console.WriteLine("Resized Image Width: " + resizedImage.Width);
            Console.WriteLine("Resized Image Height: " + resizedImage.Height);

            return resizedImage;

        }

        private void initalLoadCalculations(object sender, EventArgs e)
        {
            // Adjust the size of the GroupBox based on the screen size and a scale factor
            adjust();
        }
    }


}
