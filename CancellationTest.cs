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

            //Application.Run(new examParent(test,0.75));
            //Application.Run(new examParent(p1, 1.0));
            //Application.Run(new praticeParent(p1,1, 0.75));
            //.Run(new screenSizeAdjustment());
            Application.Run(new Menu());
            //Application.Run(new IntermediateScreen());
            //Application.Run(new helpScreen());
            //Application.Run(new exportScreen("123456789", 1.0));
            //Application.Run(new legalScreen());


        }

    }

    internal static class ResourceMedia
    {
        private static readonly List<string> TempFiles = new List<string>();

        static ResourceMedia()
        {
            AppDomain.CurrentDomain.ProcessExit += (_, __) => CleanupTempFiles();
        }

        internal static string GetTempMediaFile(string resourceName, string extension)
        {
            object resource = Properties.Resources.ResourceManager.GetObject(resourceName, Properties.Resources.Culture);
            byte[] bytes = resource as byte[];
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }

            string tempPath = Path.Combine(Path.GetTempPath(), $"CancellationTest_{resourceName}_{Guid.NewGuid():N}{extension}");
            File.WriteAllBytes(tempPath, bytes);

            lock (TempFiles)
            {
                TempFiles.Add(tempPath);
            }

            return tempPath;
        }

        internal static Stream GetMediaStream(string resourceName)
        {
            object resource = Properties.Resources.ResourceManager.GetObject(resourceName, Properties.Resources.Culture);
            byte[] bytes = resource as byte[];
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }

            return new MemoryStream(bytes);
        }

        private static void CleanupTempFiles()
        {
            lock (TempFiles)
            {
                foreach (string file in TempFiles)
                {
                    try
                    {
                        if (File.Exists(file))
                        {
                            File.Delete(file);
                        }
                    }
                    catch
                    {
                    }
                }

                TempFiles.Clear();
            }
        }
    }

    internal static class AspectRatioLayout
    {
        internal static Rectangle GetContentBounds(Size hostSize, double aspectRatio)
        {
            if (aspectRatio <= 0 || double.IsNaN(aspectRatio) || double.IsInfinity(aspectRatio))
            {
                aspectRatio = (double)hostSize.Width / Math.Max(1, hostSize.Height);
            }

            int contentWidth = hostSize.Width;
            int contentHeight = (int)Math.Round(contentWidth / aspectRatio);

            if (contentHeight > hostSize.Height)
            {
                contentHeight = hostSize.Height;
                contentWidth = (int)Math.Round(contentHeight * aspectRatio);
            }

            int x = (hostSize.Width - contentWidth) / 2;
            int y = (hostSize.Height - contentHeight) / 2;

            return new Rectangle(x, y, contentWidth, contentHeight);
        }
    }

    
}







