using CancellationTest.Properties;
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
        private System.Windows.Forms.Button increaseAspectButton;
        private System.Windows.Forms.Button decreaseAspectButton;
        private System.Windows.Forms.TextBox currentRatioBox;
        private System.Windows.Forms.TextBox currentAspectRatioBox;
        private System.Windows.Forms.GroupBox a4GroupBox;
        private System.Windows.Forms.PictureBox a4ImagePictureBox;
#if DEBUG
        private System.Windows.Forms.Panel debugLeftBar;
        private System.Windows.Forms.Panel debugRightBar;
        private System.Windows.Forms.Panel debugTopBar;
        private System.Windows.Forms.Panel debugBottomBar;
#endif

        private Image a4Image = Resources.A4_ScalePaper;
        public double selectedAspectRatio { get; private set; }

        public screenSizeAdjustment()
        {
            
            //Sample Test Parameters used for testing the page
            testParameters testParameters = new testParameters();
            testParameters.testType = AvailableExams.Assessment;
            testParameters.patientName = "Unknown";
            testParameters.adjustmentRatio = 1.0;
            testParameters.aspectRatio = 16.0 / 9.0;
            testParameters.examTime = 240;

            this.m_testParameters = testParameters;
            this.selectedAspectRatio = testParameters.aspectRatio;

            

            frameSetup();
        }

        public screenSizeAdjustment(testParameters testParameters)
        {
            this.screenSize = testParameters.adjustmentRatio;
            this.m_testParameters = testParameters;
            this.selectedAspectRatio = testParameters.aspectRatio > 0 ? testParameters.aspectRatio : (double)Screen.PrimaryScreen.Bounds.Width / Screen.PrimaryScreen.Bounds.Height;

            frameSetup();

        }

        public void frameSetup()
        {
            Console.WriteLine((int)(100 * Screen.PrimaryScreen.WorkingArea.Width / Screen.PrimaryScreen.Bounds.Width));
            Console.WriteLine(this.DeviceDpi);

            Console.WriteLine("Screen Width: " + Screen.PrimaryScreen.Bounds.Width);

#if !DEBUG
            this.TopMost = true;
#endif
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.screenSize = this.m_testParameters.adjustmentRatio > 0 ? this.m_testParameters.adjustmentRatio : 1.0;
            this.selectedAspectRatio = this.selectedAspectRatio > 0 ? this.selectedAspectRatio : ((double)Screen.PrimaryScreen.Bounds.Width / Screen.PrimaryScreen.Bounds.Height);

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

        private void increaseAspectButton_Click(object sender, EventArgs e)
        {
            this.selectedAspectRatio += 0.05;
            adjust();
        }

        private void decreaseAspectButton_Click(object sender, EventArgs e)
        {
            this.selectedAspectRatio = Math.Max(1.0, this.selectedAspectRatio - 0.05);
            adjust();
        }

        private void adjust()
        {
            Console.WriteLine("g" + Screen.PrimaryScreen.Bounds.Width + " " + Screen.PrimaryScreen.Bounds.Height);

            Console.WriteLine("screenSize: " + this.screenSize);
            Console.WriteLine("Current Window Width: " + this.Width);
            Console.WriteLine("Current Window Height: " + this.Height);
            currentRatioBox.Text = this.screenSize.ToString("0.00");
            currentAspectRatioBox.Text = this.selectedAspectRatio.ToString("0.00");

            Rectangle contentBounds = AspectRatioLayout.GetContentBounds(this.ClientSize, this.selectedAspectRatio);

            int imageWidth = (int)(contentBounds.Width / a4WidthRatio * this.screenSize);
            int imageHeight = (int)(contentBounds.Height / a4HeightRatio * this.screenSize);
            Console.WriteLine("Image Width: " + imageWidth);
            Console.WriteLine("Image Height: " + imageHeight);
            Console.WriteLine("Difference: " + Screen.PrimaryScreen.Bounds.Height + " " + this.Height + " " + this.Height / Screen.PrimaryScreen.Bounds.Height);
            Console.WriteLine("Difference: " + Screen.PrimaryScreen.Bounds.Width + " " + this.Width + " " + this.Width / Screen.PrimaryScreen.Bounds.Width);
            
            if (imageWidth > 0 && imageHeight > 0)
            {
                Image oldImage = this.a4ImagePictureBox.Image;
                this.a4ImagePictureBox.Width = imageWidth;
                this.a4ImagePictureBox.Height = imageHeight;
                this.a4ImagePictureBox.Image = ResizeImage(this.a4Image, imageWidth, imageHeight);

                if (oldImage != null && oldImage != this.a4Image)
                {
                    oldImage.Dispose();
                }
            }

            this.a4ImagePictureBox.Left = contentBounds.Left + (contentBounds.Width - this.a4ImagePictureBox.Width) / 2;
            this.a4ImagePictureBox.Top = contentBounds.Top + (contentBounds.Height - this.a4ImagePictureBox.Height) / 2 - this.currentRatioBox.Height - this.okayButton.Height;

            this.decreaseButton.Top = contentBounds.Bottom - (int)(contentBounds.Height * 0.18);
            this.decreaseButton.Left = contentBounds.Left + (int)(contentBounds.Width * 0.22);

            this.currentRatioBox.Top = this.decreaseButton.Top;
            this.currentRatioBox.Left = contentBounds.Left + (int)(contentBounds.Width * 0.42);

            this.increaseButton.Top = this.decreaseButton.Top;
            this.increaseButton.Left = contentBounds.Left + (int)(contentBounds.Width * 0.62);

            this.decreaseAspectButton.Top = contentBounds.Bottom - (int)(contentBounds.Height * 0.28);
            this.decreaseAspectButton.Left = this.decreaseButton.Left;

            this.currentAspectRatioBox.Top = this.decreaseAspectButton.Top;
            this.currentAspectRatioBox.Left = this.currentRatioBox.Left;

            this.increaseAspectButton.Top = this.decreaseAspectButton.Top;
            this.increaseAspectButton.Left = this.increaseButton.Left;

            this.okayButton.Top = contentBounds.Bottom - (int)(contentBounds.Height * 0.08);
            this.okayButton.Left = contentBounds.Left + (int)(contentBounds.Width * 0.45);

#if DEBUG
            this.debugLeftBar.Bounds = new Rectangle(0, 0, contentBounds.Left, this.ClientSize.Height);
            this.debugRightBar.Bounds = new Rectangle(contentBounds.Right, 0, this.ClientSize.Width - contentBounds.Right, this.ClientSize.Height);
            this.debugTopBar.Bounds = new Rectangle(contentBounds.Left, 0, contentBounds.Width, contentBounds.Top);
            this.debugBottomBar.Bounds = new Rectangle(contentBounds.Left, contentBounds.Bottom, contentBounds.Width, this.ClientSize.Height - contentBounds.Bottom);
#endif

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Console.WriteLine("textBox1_Enter");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("screenSize: " + currentRatioBox.Text);
        }

        private void okayButton_Click_1(object sender, EventArgs e)
        {
            this.m_testParameters.adjustmentRatio = this.screenSize;
            this.m_testParameters.aspectRatio = this.selectedAspectRatio;

            this.Close();
        }

        private void InitializeComponent()
        {
            this.okayButton = new System.Windows.Forms.Button();
            this.increaseButton = new System.Windows.Forms.Button();
            this.decreaseButton = new System.Windows.Forms.Button();
            this.increaseAspectButton = new System.Windows.Forms.Button();
            this.decreaseAspectButton = new System.Windows.Forms.Button();
            this.currentRatioBox = new System.Windows.Forms.TextBox();
            this.currentAspectRatioBox = new System.Windows.Forms.TextBox();
            this.a4GroupBox = new System.Windows.Forms.GroupBox();
            this.a4ImagePictureBox = new System.Windows.Forms.PictureBox();
#if DEBUG
            this.debugLeftBar = new System.Windows.Forms.Panel();
            this.debugRightBar = new System.Windows.Forms.Panel();
            this.debugTopBar = new System.Windows.Forms.Panel();
            this.debugBottomBar = new System.Windows.Forms.Panel();
#endif

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
            this.currentRatioBox.ReadOnly = true;
            this.currentRatioBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);

            //
            // currentAspectRatioBox
            //
            this.currentAspectRatioBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.currentAspectRatioBox.Top = (int)(screenHeight * 0.7);
            this.currentAspectRatioBox.Left = (int)(screenWidth * 0.45);
            this.currentAspectRatioBox.Name = "textBoxAspect";
            this.currentAspectRatioBox.Size = new System.Drawing.Size(adjustWidth(185), adjustHeight(62));
            this.currentAspectRatioBox.TabIndex = 6;
            this.currentAspectRatioBox.ReadOnly = true;

            //
            // increaseAspectButton
            //
            this.increaseAspectButton.BackColor = System.Drawing.Color.SeaGreen;
            this.increaseAspectButton.Top = (int)(screenHeight * 0.7);
            this.increaseAspectButton.Left = (int)(screenWidth * 0.6);
            this.increaseAspectButton.Name = "buttonAspectPlus";
            this.increaseAspectButton.Size = new System.Drawing.Size((int)(screenWidth * 0.1), (int)(0.08 * screenHeight));
            this.increaseAspectButton.TabIndex = 7;
            this.increaseAspectButton.Text = "Aspect +";
            this.increaseAspectButton.UseVisualStyleBackColor = false;
            this.increaseAspectButton.Click += new System.EventHandler(this.increaseAspectButton_Click);

            //
            // decreaseAspectButton
            //
            this.decreaseAspectButton.BackColor = System.Drawing.Color.IndianRed;
            this.decreaseAspectButton.Top = (int)(screenHeight * 0.7);
            this.decreaseAspectButton.Left = (int)(screenWidth * 0.3);
            this.decreaseAspectButton.Name = "buttonAspectMinus";
            this.decreaseAspectButton.Size = new System.Drawing.Size((int)(screenWidth * 0.1), (int)(0.08 * screenHeight));
            this.decreaseAspectButton.TabIndex = 8;
            this.decreaseAspectButton.Text = "Aspect -";
            this.decreaseAspectButton.UseVisualStyleBackColor = false;
            this.decreaseAspectButton.Click += new System.EventHandler(this.decreaseAspectButton_Click);
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
            // a4ImagePictureBox
            //
            
            this.a4ImagePictureBox.Image = ResizeImage(this.a4Image, (int)(this.Width / a4WidthRatio * this.screenSize), (int)(this.Height / a4HeightRatio * this.screenSize));
            
            //this.a4ImagePictureBox.Image = re

            //Resize the image to match the screen size
            this.a4ImagePictureBox.Location = new System.Drawing.Point((this.Width - this.a4ImagePictureBox.Width) / 2, 
                (this.Height - this.a4ImagePictureBox.Height) / 2 - adjustHeight(62) - this.okayButton.Height);



            // 
            // screenSizeAdjustment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            //this.ClientSize = new System.Drawing.Size(adjustWidth(1496), adjustHeight(861));
#if DEBUG
            this.debugLeftBar.BackColor = Color.Yellow;
            this.debugRightBar.BackColor = Color.Yellow;
            this.debugTopBar.BackColor = Color.Yellow;
            this.debugBottomBar.BackColor = Color.Yellow;
            this.Controls.Add(this.debugLeftBar);
            this.Controls.Add(this.debugRightBar);
            this.Controls.Add(this.debugTopBar);
            this.Controls.Add(this.debugBottomBar);
#endif
            this.Controls.Add(this.a4GroupBox);
            this.Controls.Add(this.currentRatioBox);
            this.Controls.Add(this.currentAspectRatioBox);
            this.Controls.Add(this.decreaseButton);
            this.Controls.Add(this.increaseButton);
            this.Controls.Add(this.decreaseAspectButton);
            this.Controls.Add(this.increaseAspectButton);
            this.Controls.Add(this.okayButton);
            this.Controls.Add(this.a4ImagePictureBox);
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

        public Image ResizeImage(Image image, int width, int height)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image), "Source image is null. Verify that A4_ScalePaper is loaded from resources.");
            }

            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than 0.");
            }

            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height), "Height must be greater than 0.");
            }


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

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.a4ImagePictureBox != null)
            {
                adjust();
            }
        }
    }


}
