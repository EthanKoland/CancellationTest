﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Windows.Forms;
using System.Windows.Media.TextFormatting;
using CancellationTest;
using OfficeOpenXml;

namespace CancellationTest
{
    internal class praticeParent : Form
    {
        private IContainer components;
        private MainMenu ImgMenu = null;
        private Graphics g = null;

        //A reference map to cacluate the interactions
        private int[,] bitMap = null;

        private int curPtindex = 0;
        private Pen GreenPen = null;
        private Pen BluePen = null;
        private Pen RedPen = null;
        private Pen CrossPen = null;
        private Point CurPt;
        private Point[] Cath2DPts = null;
        private Point[] CirPts = null;
        private int MaxPts = 0;

        private string patientName;

        public int adjustedScreenWidth { get; private set; }
        public int adjustedScreenHeight { get; private set; }

        //Inorder to standardize the screen size a variable is declared to adjust the size of the screen
        private double adjustSize = 1.0;

        //The none adjustsize of the screen
        private int screenwidth = 960;
        //Total screen size in 575
        private int screenheight = 540;
        private int verticalOffset = 0;

        //Sizes of the instruction mugs
        private int smallMugsize;
        private int largeMugsize;

        //Vars that are declared in class init
        private int numberOfHorizontalGrids; // a variable that defines the number of grids in the horizontal direction
        private int numberOfVerticalGrids; // a variable that defines the number of grids in the vertical direction


        //Storage of the lines seperating the cells
        private int[] horizontalLines;
        private int[] verticalLines;

        //Refrences for the images
        private List<imageObject> images = new List<imageObject>();

        //Action tracker
        private actionTracker tracker;
        private abstractTestClass localExamObject;

        private DateTime endTime;

        private Label timeLabel;
        private Label currentTimeLabel;
        private Button helpButton;
        private Label crossOutLabel;
        private Label tryThisLabel;

        private PictureBox smallLeftCrossedMug;
        private PictureBox smallRightCrossedMug;
        private PictureBox largeLeftCrossedMug;
        private PictureBox LargeRightCrossedMug;

        private PictureBox smallLeftMug;
        private PictureBox smallRightMug;
        private PictureBox largeLeftMug;
        private PictureBox largeRightMug;

        private Point centerScreen;
        private Point minBoundry;
        private Point maxBoundry;


        public praticeParent( abstractTestClass examObject, double adjustSize = 0.5,
            int seconds = 240, string patientName = "None")
        {

            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.screenheight = Screen.PrimaryScreen.Bounds.Height;
            this.screenwidth = Screen.PrimaryScreen.Bounds.Width;
            //this.screenwidth = this.Width;
            //this.screenheight = this.Height;

            //Assign the parameter values to the variables
            this.adjustSize = adjustSize * ((double)this.screenwidth / 1920.0); ;
            this.patientName = patientName;

            this.smallMugsize = (int)(0.04 * this.screenwidth);
            this.largeMugsize = (int)(0.055 * this.screenwidth);

            this.BackColor = Color.DarkGray;
            
            

            this.localExamObject = examObject;
            this.tracker = new actionTracker(this.localExamObject);


            this.endTime = DateTime.Now.AddSeconds(seconds);

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = (1000); // 10 seconds in milliseconds
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            //The timer 
            this.timeLabel = new Label();
            this.timeLabel.Text = "Time:";
            this.timeLabel.Location = new Point((int)(0.8*this.screenwidth), 5);
            this.timeLabel.Size = new Size((int)(0.05 * this.screenwidth ), 40);
            this.timeLabel.Font = new Font("Arial", 24);
            this.timeLabel.ForeColor = Color.DimGray;

            this.currentTimeLabel = new Label();
            this.currentTimeLabel.Text = "00:00";
            this.currentTimeLabel.Location = new Point((int)(0.9 * this.screenwidth), 5);
            this.currentTimeLabel.Size = new Size((int)(0.05 * this.screenwidth), 40);
            this.currentTimeLabel.Font = new Font("Arial", 24);
            this.currentTimeLabel.ForeColor = Color.DeepSkyBlue;

            this.helpButton = new Button();
            this.helpButton.Text = "?";
            this.helpButton.Location = new Point((int)(0.95 * this.screenwidth), 5);
            this.helpButton.Size = new Size(40, 40);
            this.helpButton.Font = new Font("Arial", 24);
            this.helpButton.ForeColor = Color.WhiteSmoke;
            this.helpButton.BackColor = Color.DeepSkyBlue;

            this.crossOutLabel = new Label();
            this.crossOutLabel.Text = "Cross Out the full mugs";
            this.crossOutLabel.Location = new Point(0, (int)(0.1 * this.screenheight));
            //Center align the text
            this.crossOutLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.crossOutLabel.Size = new Size((int)(this.screenwidth), (int)(0.03 * this.screenwidth));
            this.crossOutLabel.Font = new Font("Arial", (int)(0.02 * this.screenwidth));

            Image smallLeftTargetImage = imageObject.getImageObject(imageTypes.TargetLeft);
            smallLeftTargetImage = ResizeImage(smallLeftTargetImage, this.smallMugsize, this.smallMugsize);

            this.smallLeftCrossedMug = new PictureBox();
            this.smallLeftCrossedMug.Image = smallLeftTargetImage;
            this.smallLeftCrossedMug.Location = new Point((int)(0.35 * this.screenwidth), (int)(0.165 * this.screenheight));
            this.smallLeftCrossedMug.Size = new Size(this.smallMugsize, this.smallMugsize);

            Image smallRightTarget = imageObject.getImageObject(imageTypes.TargetRight);
            smallRightTarget = ResizeImage(smallRightTarget, this.smallMugsize, this.smallMugsize);

            this.smallRightCrossedMug = new PictureBox();
            this.smallRightCrossedMug.Image= smallRightTarget;
            this.smallRightCrossedMug.Location = new Point((int)(0.4 * this.screenwidth), (int)(0.165 * this.screenheight));
            this.smallRightCrossedMug.Size = new Size(this.smallMugsize, this.smallMugsize);

            Image smallLeftDistractor = imageObject.getImageObject(imageTypes.DistractionLeft);
            smallLeftDistractor = ResizeImage(smallLeftDistractor, this.smallMugsize, this.smallMugsize);

            this.smallLeftMug = new PictureBox();
            this.smallLeftMug.Image = smallLeftDistractor;
            this.smallLeftMug.Location = new Point((int)(0.5 * this.screenwidth), (int)(0.165 * this.screenheight));
            this.smallLeftMug.Size = new Size(this.smallMugsize, this.smallMugsize);

            Image smallRightDistractor = imageObject.getImageObject(imageTypes.DistractionRight);
            smallRightDistractor = ResizeImage(smallRightDistractor, this.smallMugsize, this.smallMugsize);

            this.smallRightMug = new PictureBox();
            this.smallRightMug.Image = smallRightDistractor;
            this.smallRightMug.Location = new Point((int)(0.55 * this.screenwidth), (int)(0.165 * this.screenheight));
            this.smallRightMug.Size = new Size(this.smallMugsize, this.smallMugsize);

            Image leftTarget = imageObject.getImageObject(imageTypes.TargetLeft);
            leftTarget = ResizeImage(leftTarget, this.largeMugsize, this.largeMugsize);

            this.largeLeftCrossedMug = new PictureBox();
            this.largeLeftCrossedMug.Image = leftTarget;
            this.largeLeftCrossedMug.Location = new Point((int)(0.33 * this.screenwidth), (int)(0.25 * this.screenheight));
            this.largeLeftCrossedMug.Size = new Size(this.largeMugsize, this.largeMugsize);

            Image rightTarget = imageObject.getImageObject(imageTypes.TargetRight);
            rightTarget = ResizeImage(rightTarget, this.largeMugsize, this.largeMugsize);

            this.LargeRightCrossedMug = new PictureBox();
            this.LargeRightCrossedMug.Image = rightTarget;
            this.LargeRightCrossedMug.Location = new Point((int)(0.4 * this.screenwidth), (int)(0.25 * this.screenheight));
            this.LargeRightCrossedMug.Size = new Size(this.largeMugsize, this.largeMugsize);

            Image leftDistractor = imageObject.getImageObject(imageTypes.DistractionLeft);
            leftDistractor = ResizeImage(leftDistractor, this.largeMugsize, this.largeMugsize);

            this.largeLeftMug = new PictureBox();
            this.largeLeftMug.Image = leftDistractor;
            this.largeLeftMug.Location = new Point((int)(0.5 * this.screenwidth), (int)(0.25 * this.screenheight));
            this.largeLeftMug.Size = new Size(this.largeMugsize, this.largeMugsize);

            Image rightDistractor = imageObject.getImageObject(imageTypes.DistractionRight);
            rightDistractor = ResizeImage(rightDistractor, this.largeMugsize, this.largeMugsize);
            
            this.largeRightMug = new PictureBox();
            this.largeRightMug.Image = rightDistractor;
            this.largeRightMug.Location = new Point((int)(0.55 * this.screenwidth), (int)(0.25 * this.screenheight));
            this.largeRightMug.Size = new Size(this.largeMugsize, this.largeMugsize);

            this.tryThisLabel = new Label();
            this.tryThisLabel.Text = "Try This Example";
            this.tryThisLabel.Location = new Point(0, (int)(0.37 * this.screenheight));
            this.tryThisLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.tryThisLabel.Size = new Size((int)(this.screenwidth), (int)(0.03 * this.screenwidth));
            this.tryThisLabel.Font = new Font("Arial", (int)(0.02 * this.screenwidth));






            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.currentTimeLabel);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.smallLeftCrossedMug);
            this.Controls.Add(this.smallRightCrossedMug);
            this.Controls.Add(this.smallLeftMug);
            this.Controls.Add(this.smallRightMug);
            this.Controls.Add(this.largeLeftCrossedMug);
            this.Controls.Add(this.LargeRightCrossedMug);
            this.Controls.Add(this.largeLeftMug);
            this.Controls.Add(this.largeRightMug);
            this.Controls.Add(this.tryThisLabel);


            this.Controls.Add(this.crossOutLabel);
             
            this.KeyPreview = true;
            this.centerScreen = new Point(this.screenwidth / 2, this.screenheight / 2); 
            

            int frameX = (int)(examObject.screenWidth * this.adjustSize);
            int frameY = (int)(examObject.screenHeight * this.adjustSize);

            this.minBoundry = new Point((this.screenwidth - frameX)/2, (this.screenheight - frameY)/2);
            this.maxBoundry = new Point((this.screenwidth + frameX) / 2, (this.screenheight + frameY) / 2);

            //Creating a new bitMap

            InitializeComponent();
            this.KeyDown += new KeyEventHandler(examParent_KeyDown);
            this.MouseClick += new MouseEventHandler(ImageViewer_MouseClick);
            this.DoubleBuffered = true;
            GreenPen = new Pen(Color.LightGreen, 1);
            BluePen = new Pen(Color.LightBlue, 1);
            RedPen = new Pen(Color.Red, 1);
            CrossPen = new Pen(Color.Black, (float)(this.screenheight * 0.01));
           
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            TimeSpan timeRemaining = endTime - DateTime.Now;

            //If the time is less than 0 then stop the timer
            if (timeRemaining.TotalSeconds < 0)
            {
                ((System.Windows.Forms.Timer)sender).Stop();
                this.currentTimeLabel.Text = "0:00";

                return;
            }

            int totalSeconds = (int)Math.Round(timeRemaining.TotalSeconds);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            string modifedSeconds = seconds < 10 ? "0" + seconds : "" + seconds;

            this.currentTimeLabel.Text = "" + minutes + ":" + modifedSeconds;
        }

        void ImageViewer_MouseClick(object sender, MouseEventArgs e)
        {


            //Figure out if the click point is within the boundry
            if(e.X < this.minBoundry.X || e.X > this.maxBoundry.X || e.Y < this.minBoundry.Y || e.Y > this.maxBoundry.Y)
            {
                return;
            }   

            this.CurPt = adjustedClickPoint(new Point(e.X, e.Y));


            // Check if the ooint correlates to any of the images
            int imageID = this.localExamObject.checkClick(this.CurPt);
            Console.WriteLine("Image ID : " + imageID);



            if (imageID != 0)
           {

                int halfWidth = this.localExamObject.imageList[imageID - 1].width / 2;
                int halfHeight = this.localExamObject.imageList[imageID - 1].height / 2;

                Point imageCenter = this.localExamObject.imageList[imageID - 1].imageCenter;

                clickAction action = new clickAction();
                action.clickPoint = new Point(imageCenter.X + halfWidth, imageCenter.Y + halfHeight);
                action.ImageID = imageID;
                action.timeOfClick = DateTime.Now;

                Console.WriteLine("Image Clicked : " + imageID);
                imageObject clickedImage = this.localExamObject.imageList[imageID - 1];
                clickedImage.setClicked(0, adjustSize);

                action.isCrossed = clickedImage.isClicked;

                Console.WriteLine("Image cliscked State" + clickedImage.isClicked);

                this.tracker.addAction(action);
           }

                
            Console.WriteLine("Image ID : " + imageID);

            Console.WriteLine("ImageCenter for index 1 : " + this.localExamObject.imageList[0].imageCenter.X + " " + this.localExamObject.imageList[0].imageCenter.Y);



            this.Refresh();

        }

        void examParent_MouseEvent(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Mouse Event");

            //The pos
        }

        void examParent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                //pnlwhichImg.Text = " image " + curimg;
                this.Refresh();
            }
            else if (e.KeyCode == Keys.E)
            {
                TimeSpan timeRemaining = endTime - DateTime.Now;
                Console.WriteLine("E Key Pressed");
                string filename = "CancellationTest_" + this.patientName + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
                this.tracker.export(filename);
                this.tracker.outPutImage(this.patientName, timeRemaining);
            }
            else if (e.KeyCode == Keys.Oemplus)
            {
                increaseAdjustSize();
                Console.WriteLine("Adjust Size : " + this.adjustSize);
            }
            else if (e.KeyCode == Keys.OemMinus)
            {
                decreaseAdjustSize();
                Console.WriteLine("Adjust Size : " + this.adjustSize);
            }
            else if(e.KeyCode == Keys.H)
            {
                Console.WriteLine("Help Button Clicked");
            }
            else if(e.KeyCode == Keys.Q)
            {
                System.Windows.Forms.Application.Exit();

            }
            Console.WriteLine("Key Pressed" + e.KeyCode);
            this.Refresh();
        }


        //draw customized mouse pointer
        private Cursor crossCursor(Pen pen, int x, int y)
        {
            var pic = new Bitmap(x, y);
            Graphics gr = Graphics.FromImage(pic);

            var pathX = new GraphicsPath();
            var pathY = new GraphicsPath();
            pathX.AddLine(0, y / 2, x, y / 2);
            pathY.AddLine(x / 2, 0, x / 2, y);
            gr.DrawPath(pen, pathX);
            gr.DrawPath(pen, pathY);
            gr.DrawArc(pen, 2, 2, x - 5, y - 5, 0, 360);

            Console.WriteLine("X : " + x + " Y : " + y);

            IntPtr ptr = pic.GetHicon();
            var c = new Cursor(ptr);
            return c;
        }

        private void bitMapValidation(Graphics g)
        {
            for (int i = 0; i < this.screenwidth; i++)
            {
                for (int j = 0; j < this.screenheight; j++)
                {
                    Point tempPoi = new Point(i, j);
                    Point adjustedPoint = adjustedClickPoint(tempPoi);
                    int imageID = this.localExamObject.checkClick(adjustedPoint);
                    if (imageID != 0)
                    {
                        Console.WriteLine("Image ID : " + imageID);
                        g.DrawRectangle(Pens.Black, i, j, 1, 1);
                    }

                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            //re-adjust the size of cursor
            //Cursor = crossCursor(Pens.WhiteSmoke, Brushes.WhiteSmoke, Bounds.Width, Bounds.Height);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 255), 5);
            Cursor = crossCursor(blackPen, 50, 50);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int i;
            base.OnPaint(e);
            e.Graphics.PageUnit = GraphicsUnit.Pixel;
            Graphics dc = e.Graphics;
            dc.PageUnit = GraphicsUnit.Pixel;

            //localExamObject.drawHeader(dc, endTime);

            //bitMapValidation(dc);


            //Draw the images in the list
            foreach (imageObject img in localExamObject.imageList)
            {
                
                int halfWidth = (int)Math.Round(img.width * adjustSize) / 2;
                int halfHeight = (int)Math.Round(img.height * adjustSize) / 2;

                int adjustedHeight = (int)Math.Round(img.height * adjustSize);
                int adjustedWidth = (int)Math.Round(img.width * adjustSize);

                int adjustedX = (int)Math.Round(img.imageCenter.X * adjustSize + this.minBoundry.X);
                int adjustedY = (int)Math.Round(img.imageCenter.Y * adjustSize + this.minBoundry.Y);

                e.Graphics.DrawImage(imageObject.getImageObject(img.imageType), adjustedX - halfWidth, adjustedY - halfHeight, adjustedWidth, adjustedHeight);
                
                if(img.isClicked)
                {
                    int x1 = adjustedX - adjustedWidth/2;

                    int y1 = adjustedY - adjustedHeight / 2;


                    int x2 = x1 + adjustedWidth;

                    int y2 = y1 + adjustedHeight;

                    e.Graphics.DrawLine(this.CrossPen, x1, y1, x2, y2);
                    
                }


                e.Graphics.DrawRectangle(Pens.Black, adjustedX - halfWidth, adjustedY - halfWidth, adjustedWidth, adjustedHeight);
            }



            

            Console.WriteLine("Min Boundry : " + this.minBoundry.X + " " + this.minBoundry.Y);
            Console.WriteLine("Max Boundry : " + this.maxBoundry.X + " " + this.maxBoundry.Y);

            dc.DrawRectangle(RedPen, this.minBoundry.X, this.minBoundry.Y, this.maxBoundry.X - this.minBoundry.X, this.maxBoundry.Y - this.minBoundry.Y);
            dc.DrawRectangle(RedPen, 900, 500, 120, 80);
        }

        private void Draw2DPoint(Graphics dc, Pen pen, Point pt)
        {
            dc.DrawLine(pen, pt.X - 7, pt.Y, pt.X + 7, pt.Y);
            dc.DrawLine(pen, pt.X, pt.Y - 7, pt.X, pt.Y + 7);
        }


        private void increaseAdjustSize()
        {
            this.adjustSize += 0.01;

            //int adjusX = (int)Math.Round(screenwidth * 0.01);
            //int adjusY = (int)Math.Round(screenheight * 0.01);

            //this.minBoundry = new Point(Math.Max(this.minBoundry.X - adjusX, 0), Math.Max(this.minBoundry.Y - adjusY, 0));
            //this.maxBoundry = new Point(Math.Min(this.maxBoundry.X + adjusX, screenwidth), Math.Min(this.maxBoundry.Y + adjusY, screenheight));

            int frameX = (int)(localExamObject.screenWidth * this.adjustSize);
            int frameY = (int)(localExamObject.screenHeight * this.adjustSize);

            this.minBoundry = new Point((this.screenwidth - frameX) / 2, (this.screenheight - frameY) / 2);
            this.maxBoundry = new Point((this.screenwidth + frameX) / 2, (this.screenheight + frameY) / 2);

            this.CrossPen = new Pen(Color.Black, (float)(frameY * 0.01));
    
        }

        private void decreaseAdjustSize()
        {
            this.adjustSize -= 0.01;

            int frameX = (int)(localExamObject.screenWidth * this.adjustSize);
            int frameY = (int)(localExamObject.screenHeight * this.adjustSize);

            this.minBoundry = new Point((this.screenwidth - frameX) / 2, (this.screenheight - frameY) / 2);
            this.maxBoundry = new Point((this.screenwidth + frameX) / 2, (this.screenheight + frameY) / 2);

            this.CrossPen = new Pen(Color.Black, (float)(frameY * 0.01));
        }

        private Point adjustedClickPoint(Point clickPoint)
        {
            
            Point newPoint = new Point((int)Math.Round(clickPoint.X / adjustSize) - this.minBoundry.X,
                                        (int)Math.Round(clickPoint.Y / adjustSize) - this.minBoundry.Y);
            Console.WriteLine("Adjusted Point : " + newPoint.X + " " + newPoint.Y);
            
            return newPoint;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ImageViewer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(11, 27);
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(screenwidth, screenheight + verticalOffset);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ImageViewer";
            this.Text = "CancellationTest";
            this.ResumeLayout(false);

        }

        public static Image ResizeImage(Image image, int maxWidth, int maxHeight)
        {
            // Calculate the new dimensions while maintaining the aspect ratio
            int originalWidth = image.Width;
            int originalHeight = image.Height;
            float ratioX = (float)maxWidth / originalWidth;
            float ratioY = (float)maxHeight / originalHeight;
            float ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(originalWidth * ratio);
            int newHeight = (int)(originalHeight * ratio);

            // Create a new bitmap with the new dimensions
            Bitmap resizedImage = new Bitmap(newWidth, newHeight);

            // Use a graphics object to draw the resized image
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return resizedImage;
        }
    }

}