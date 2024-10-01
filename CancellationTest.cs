using System;
using System.Collections.Generic;
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

    public class ImageViewer : Form
    {
        


        public ImageViewer()
        {
            //Assign the parameter values to the variables
            
        }

        


        [STAThread]
        static void Main()
        {
            abstractTestClass test = new  assesmentExam();
            abstractTestClass p1 = new praticeExam1();
            abstractTestClass p2 = new praticeExam2();
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new examParent(test, 1.0));
            //Application.Run(new examParent(p1, 1.0));
            //Application.Run(new praticeParent(p2,1.0));
            //Application.Run(new screenSizeAdjustment());
            Application.Run(new Menu());
            //Application.Run(new IntermediateScreen());


            //Testing DPIUtil
           


            //Get the current screen scale

        }

        
    }

    
}







