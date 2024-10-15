using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace CancellationTest
{
    public partial class helpScreen : Form
    {
        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
        System.Media.SoundPlayer player = new System.Media.SoundPlayer();


        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;

        public helpScreen()
        {
            //this.Size = new Size(640, 360);

            // Set the start position to center the form on the screen
            this.StartPosition = FormStartPosition.CenterScreen;

            // Optional: Set the form title
            this.Text = "Help Screen";


            formLoad();

            this.TopMost = true;
           



            wplayer.URL = "Sounds\\Instructions.mp3";


            //Play the Instructions Mp3
            wplayer.controls.play();

        }

        private void formLoad()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            //this.groupBox1.SuspendLayout();
            //this.SuspendLayout();


            int screenWidth = 640;
            int screenHeight = 360;

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
            this.groupBox1.Location = new System.Drawing.Point(0,0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(screenWidth, screenHeight);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // okayButton
            // 
            this.button1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.button1.Location = new System.Drawing.Point((int)Math.Round(screenWidth * 0.4), (int)Math.Round(screenHeight * 0.8));
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size((int)Math.Round(screenWidth * 0.2), (int)Math.Round(screenHeight * 0.15));
            this.button1.TabIndex = 3;
            this.button1.Text = "OK";
            //Set the text Size - Set the font ot 8% of the screen height
            this.button1.Font = new Font("Arial", (int)Math.Round(screenHeight * 0.04));
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            //this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point((int)Math.Round(screenWidth * 0.1), (int)Math.Round(screenHeight * 0.4));
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size((int)Math.Round(screenWidth * 0.8), (int)Math.Round(screenHeight * 0.15));
            this.label3.Font = new Font("Arial", (int)Math.Round(screenHeight * 0.04));
            this.label3.TabIndex = 2;
            this.label3.Text = "Please click all the complete mugs (those with no gaps).";
            // 
            // label2
            // 
            //this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point((int)Math.Round(screenWidth * 0.1), (int)Math.Round(screenHeight * 0.6));
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size((int)Math.Round(screenWidth * 0.8), (int)Math.Round(screenHeight * 0.15));
            this.label2.Font = new Font("Arial", (int)Math.Round(screenHeight * 0.04));
            this.label2.TabIndex = 1;
            this.label2.Text = "Some Mugs will be complete and some will have a gap on the handle.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point((int)Math.Round(screenWidth * 0.1), (int)Math.Round(screenHeight * 0.2));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size((int)Math.Round(screenWidth * 0.8), (int)Math.Round(screenHeight * 0.15));
            this.label1.Font = new Font("Arial", (int)Math.Round(screenHeight * 0.04));
            this.label1.TabIndex = 0;
            this.label1.Text = "You will see a screen with lots of mugs on.";
            
            // 
            // IntermediateScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(640, 360);
            this.Controls.Add(this.groupBox1);
            this.Name = "IntermediateScreen";
            this.Text = "Form1";
            //this.groupBox1.ResumeLayout(false);
            //this.groupBox1.PerformLayout();
            //this.ResumeLayout(false);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Button Clicked");

            this.wplayer.controls.stop();






            this.Hide();

        }
    }
}