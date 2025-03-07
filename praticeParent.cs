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
    internal class praticeParent : examParent
    {
        

        //Sizes of the instruction mugs
        private int smallMugsize;
        private int largeMugsize;

        private Pen instructionPen = new Pen(Color.Black, 5);
        
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

        
        public praticeParent( abstractTestClass examObject, int praticeExamNum, double adjustSize = 0.5,
            int seconds = 240, string patientName = "None", int crossOutTime = -1 ) : base(examObject, adjustSize, seconds, patientName, crossOutTime)
        {

            
            

            this.smallMugsize = (int)(0.04 * this.screenwidth);
            this.largeMugsize = (int)(0.055 * this.screenwidth);

            

            this.crossOutLabel = new Label();
            this.crossOutLabel.Text = "Cross Out the full mugs";
            this.crossOutLabel.Location = new Point(0, (int)(0.1 * this.screenheight));
            //Center align the text
            this.crossOutLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.crossOutLabel.Size = new Size((int)(this.screenwidth), (int)(0.03 * this.screenwidth));
            this.crossOutLabel.Font = new Font("Arial", (int)(0.02 * this.screenwidth));

            Image smallLeftTargetImage = mugObject.getImageObject(imageTypes.TargetLeft);
            smallLeftTargetImage = ResizeImage(smallLeftTargetImage, this.smallMugsize, this.smallMugsize);

            this.smallLeftCrossedMug = new PictureBox();
            this.smallLeftCrossedMug.Image = smallLeftTargetImage;
            this.smallLeftCrossedMug.Location = new Point((int)(0.38 * this.screenwidth), (int)(0.165 * this.screenheight));
            this.smallLeftCrossedMug.Size = new Size(this.smallMugsize, this.smallMugsize);

            Image smallRightTarget = mugObject.getImageObject(imageTypes.TargetRight);
            smallRightTarget = ResizeImage(smallRightTarget, this.smallMugsize, this.smallMugsize);

            this.smallRightCrossedMug = new PictureBox();
            this.smallRightCrossedMug.Image= smallRightTarget;
            this.smallRightCrossedMug.Location = new Point((int)(0.44 * this.screenwidth), (int)(0.165 * this.screenheight));
            this.smallRightCrossedMug.Size = new Size(this.smallMugsize, this.smallMugsize);

            Image smallLeftDistractor = mugObject.getImageObject(imageTypes.DistractionLeft);
            smallLeftDistractor = ResizeImage(smallLeftDistractor, this.smallMugsize, this.smallMugsize);

            this.smallLeftMug = new PictureBox();
            this.smallLeftMug.Image = smallLeftDistractor;
            this.smallLeftMug.Location = new Point((int)(0.52 * this.screenwidth), (int)(0.165 * this.screenheight));
            this.smallLeftMug.Size = new Size(this.smallMugsize, this.smallMugsize);

            Image smallRightDistractor = mugObject.getImageObject(imageTypes.DistractionRight);
            smallRightDistractor = ResizeImage(smallRightDistractor, this.smallMugsize, this.smallMugsize);

            this.smallRightMug = new PictureBox();
            this.smallRightMug.Image = smallRightDistractor;
            this.smallRightMug.Location = new Point((int)(0.58 * this.screenwidth), (int)(0.165 * this.screenheight));
            this.smallRightMug.Size = new Size(this.smallMugsize, this.smallMugsize);

            Image leftTarget = mugObject.getImageObject(imageTypes.TargetLeft);
            leftTarget = ResizeImage(leftTarget, this.largeMugsize, this.largeMugsize);

            this.largeLeftCrossedMug = new PictureBox();
            this.largeLeftCrossedMug.Image = leftTarget;
            this.largeLeftCrossedMug.Location = new Point((int)(0.37 * this.screenwidth), (int)(0.25 * this.screenheight));
            this.largeLeftCrossedMug.Size = new Size(this.largeMugsize, this.largeMugsize);

            Image rightTarget = mugObject.getImageObject(imageTypes.TargetRight);
            rightTarget = ResizeImage(rightTarget, this.largeMugsize, this.largeMugsize);

            this.LargeRightCrossedMug = new PictureBox();
            this.LargeRightCrossedMug.Image = rightTarget;
            this.LargeRightCrossedMug.Location = new Point((int)(0.435 * this.screenwidth), (int)(0.25 * this.screenheight));
            this.LargeRightCrossedMug.Size = new Size(this.largeMugsize, this.largeMugsize);

            Image leftDistractor = mugObject.getImageObject(imageTypes.DistractionLeft);
            leftDistractor = ResizeImage(leftDistractor, this.largeMugsize, this.largeMugsize);

            this.largeLeftMug = new PictureBox();
            this.largeLeftMug.Image = leftDistractor;
            this.largeLeftMug.Location = new Point((int)(0.51 * this.screenwidth), (int)(0.25 * this.screenheight));
            this.largeLeftMug.Size = new Size(this.largeMugsize, this.largeMugsize);

            Image rightDistractor = mugObject.getImageObject(imageTypes.DistractionRight);
            rightDistractor = ResizeImage(rightDistractor, this.largeMugsize, this.largeMugsize);
            
            this.largeRightMug = new PictureBox();
            this.largeRightMug.Image = rightDistractor;
            this.largeRightMug.Location = new Point((int)(0.575 * this.screenwidth), (int)(0.25 * this.screenheight));
            this.largeRightMug.Size = new Size(this.largeMugsize, this.largeMugsize);

            this.tryThisLabel = new Label();
            this.tryThisLabel.Text = "Try This Example";
            this.tryThisLabel.Location = new Point(0, (int)(0.37 * this.screenheight));
            this.tryThisLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.tryThisLabel.Size = new Size((int)(this.screenwidth), (int)(0.03 * this.screenwidth));
            this.tryThisLabel.Font = new Font("Arial", (int)(0.02 * this.screenwidth));


            //this.Controls.Add(this.smallLeftCrossedMug);
            //this.Controls.Add(this.smallRightCrossedMug);
            this.Controls.Add(this.smallLeftMug);
            this.Controls.Add(this.smallRightMug);
            //this.Controls.Add(this.largeLeftCrossedMug);
            //this.Controls.Add(this.LargeRightCrossedMug);
            this.Controls.Add(this.largeLeftMug);
            this.Controls.Add(this.largeRightMug);
            this.Controls.Add(this.tryThisLabel);
            this.Controls.Add(this.crossOutLabel);

            //Setting the z order of the mugs
            

             
            
           
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
                //string filename = "CancellationTest_" + this.patientName + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
                //this.tracker.export(filename);
                //this.tracker.outPutImage(this.patientName, timeRemaining);
            }
            else if(e.KeyCode == Keys.H)
            {
                Console.WriteLine("Help Button Clicked");
            }
            else if (e.KeyCode == Keys.Q || e.KeyCode == Keys.Escape)
            {
                TimeSpan timeRemaining = endTime - DateTime.Now;
                tracker.endTime = DateTime.Now;
                Console.WriteLine("E Key Pressed");
                string filename = "CancellationTest_" + this.patientName + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
                //Open the export screen
                exportScreen exportScreenObj = new exportScreen(this.patientName, this.tracker, this.adjustSize);
                exportScreenObj.ShowDialog();

                System.Windows.Forms.Application.Exit();

            }
            else if (e.KeyCode == Keys.D1)
            {
                Console.WriteLine("1 Key Pressed -> Creating a Map");
                tracker.endTime = DateTime.Now;
                Export_Map map = new Export_Map(this.patientName);
                List<abstractExportClass> list = new List<abstractExportClass>();
                list.Add(map);
                this.tracker.export(list);
            }
            else if (e.KeyCode == Keys.D2)
            {
                Console.WriteLine("2 Key Pressed -> Creating a XLSX");
                tracker.endTime = DateTime.Now;
                abstractExportClass map = new Export_XLSX(this.patientName);
                List<abstractExportClass> list = new List<abstractExportClass>();
                list.Add(map);
                this.tracker.export(list);
            }
            else if (e.KeyCode == Keys.D3)
            {
                Console.WriteLine("2 Key Pressed -> Creating a Txt");
                tracker.endTime = DateTime.Now;
                abstractExportClass map = new Export_Txt(this.patientName);
                List<abstractExportClass> list = new List<abstractExportClass>();
                list.Add(map);
                this.tracker.export(list);
            }
            else if (e.KeyCode == Keys.D4)
            {
                Console.WriteLine("2 Key Pressed -> Creating a CSV");
                tracker.endTime = DateTime.Now;
                abstractExportClass map = new Export_CSV(this.patientName);
                List<abstractExportClass> list = new List<abstractExportClass>();
                list.Add(map);
                this.tracker.export(list);
            }

            Console.WriteLine("Key Pressed" + e.KeyCode);
            this.Refresh();
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

            
            base.OnPaint(e);
            e.Graphics.PageUnit = GraphicsUnit.Pixel;
            Graphics dc = e.Graphics;
            dc.PageUnit = GraphicsUnit.Pixel;

            Console.WriteLine("Override function called");

            //Cross out the small left mug
            Point p1  = new Point((int)(0.38 * this.screenwidth), (int)(0.165 * this.screenheight) + this.smallMugsize);
            Point p2 = new Point((int)(0.38 * this.screenwidth) + this.smallMugsize, (int)(0.165 * this.screenheight) );
            e.Graphics.DrawImage(mugObject.getImageObject(imageTypes.TargetLeft), p1.X, p2.Y, this.smallMugsize, this.smallMugsize);
            e.Graphics.DrawLine(this.instructionPen, p1, p2);
            

            //Cross out the small right mug
            p1 = new Point((int)(0.44 * this.screenwidth), (int)(0.165 * this.screenheight) + this.smallMugsize);
            p2 = new Point((int)(0.44 * this.screenwidth) + this.smallMugsize, (int)(0.165 * this.screenheight));
            e.Graphics.DrawImage(mugObject.getImageObject(imageTypes.TargetRight), p1.X, p2.Y, this.smallMugsize, this.smallMugsize);
            e.Graphics.DrawLine(this.instructionPen, p1, p2);
            

            //Cross out the large left mug
            p1 = new Point((int)(0.37 * this.screenwidth), (int)(0.25 * this.screenheight) + this.largeMugsize);
            p2 = new Point((int)(0.37 * this.screenwidth) + this.largeMugsize, (int)(0.25 * this.screenheight) );
            e.Graphics.DrawImage(mugObject.getImageObject(imageTypes.TargetLeft), p1.X, p2.Y, this.largeMugsize, this.largeMugsize);
            e.Graphics.DrawLine(this.instructionPen, p1, p2);

            //Cross out the large right mug
            p1 = new Point((int)(0.435 * this.screenwidth), (int)(0.25 * this.screenheight) + this.largeMugsize);
            p2 = new Point((int)(0.435 * this.screenwidth) + this.largeMugsize, (int)(0.25 * this.screenheight));
            e.Graphics.DrawImage(mugObject.getImageObject(imageTypes.TargetRight), p1.X, p2.Y, this.largeMugsize, this.largeMugsize);
            e.Graphics.DrawLine(this.instructionPen, p1, p2);
            


            

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
