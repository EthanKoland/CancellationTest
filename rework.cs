using System;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Data;
using System.Collections.Generic;

namespace CancellationTest
{
    public class examFrame : Form
    {

        public examFrame()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(examFrame_KeyDown);
            this.MouseClick += new MouseEventHandler(examFrame_MouseClick);
            this.DoubleBuffered = true;
           
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ImageViewer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(11, 27);
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(screenwidth, screenheight);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ImageViewer";
            this.Text = "CancellationTest";
            this.ResumeLayout(false);

        }

    }

}
