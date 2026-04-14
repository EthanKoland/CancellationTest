using CancellationTest.Properties;
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
    public partial class legalScreen : Form
    {

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label licenseLabel;
        private System.Windows.Forms.Label byLabel;
        private System.Windows.Forms.PictureBox byPicture;
        private System.Windows.Forms.Label ncLabel;
        private System.Windows.Forms.PictureBox ncPicture;
        private System.Windows.Forms.Label ndLabel;
        private System.Windows.Forms.PictureBox ndPicture;

        private System.Windows.Forms.Label copyLabel;
        private System.Windows.Forms.Label newUsersLabel;
        private System.Windows.Forms.Label comLabels;

        private System.Windows.Forms.Label contactLabel;

        public legalScreen()
        {
            this.StartPosition = FormStartPosition.CenterScreen;

            // Optional: Set the form title
            this.Text = "Help Screen";

            this.ClientSize = new Size(640, 360);


            formLoad();

            this.TopMost = true;
        }

        private void formLoad()
        {

            System.Resources.ResourceManager resources = new System.Resources.ResourceManager("CancellationTest.Properties.Resources", typeof(Resources).Assembly);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.licenseLabel = new System.Windows.Forms.Label();
            this.byLabel = new System.Windows.Forms.Label();
            this.byPicture = new System.Windows.Forms.PictureBox();
            this.ncLabel = new System.Windows.Forms.Label();
            this.ncPicture = new System.Windows.Forms.PictureBox();
            this.ndLabel = new System.Windows.Forms.Label();
            this.ndPicture = new System.Windows.Forms.PictureBox();

            this.copyLabel = new System.Windows.Forms.Label();
            this.newUsersLabel = new System.Windows.Forms.Label();
            this.comLabels = new System.Windows.Forms.Label();

            this.contactLabel = new System.Windows.Forms.Label();

            int screenWidth = 640;
            int screenHeight = 360;

            int groupBoxWidth = (int)Math.Round(screenWidth * 0.6);
            int groupBoxHeight = (int)Math.Round(screenHeight * 0.8);

            //this.titleLabel.BackColor = System.Drawing.Color.Purple;
            this.titleLabel.Size = new System.Drawing.Size((int)Math.Round(screenWidth * 0.9), (int)Math.Round(screenHeight * 0.1));
            this.titleLabel.Location = new System.Drawing.Point((int)Math.Round(screenWidth * 0.05), (int)Math.Round(screenHeight * 0.01));
            this.titleLabel.Text = "CENT software and manual licence information";
            this.titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            AdjustFontSizeToFit(titleLabel);

            //this.licenseLabel.BackColor = System.Drawing.Color.Green;
            this.licenseLabel.Size = new System.Drawing.Size((int)Math.Round(screenWidth * 0.9), (int)Math.Round(screenHeight * 0.15));
            this.licenseLabel.Location = new System.Drawing.Point((int)Math.Round(screenWidth * 0.05), (int)Math.Round(screenHeight * 0.1));
            this.licenseLabel.Text = "This license enables reusers to copy and distribute the material in any medium or format in unadapted form only, for non-commercial purposes only, and only so long as attribution is given to the creator. CC BY-NC-ND includes the following elements:";
            this.licenseLabel.TextAlign = ContentAlignment.MiddleLeft;

            //this.byLabel.BackColor = System.Drawing.Color.Green;
            this.byLabel.Size = new System.Drawing.Size((int)Math.Round(screenWidth * 0.7), (int)Math.Round(screenHeight * 0.05));
            this.byLabel.Location = new System.Drawing.Point((int)Math.Round(screenWidth * 0.15), (int)Math.Round(screenHeight * 0.27));
            this.byLabel.Text = "BY: credit must be given to the creator";
            this.byLabel.TextAlign = ContentAlignment.MiddleLeft;

            System.Drawing.Size elementSize = new  System.Drawing.Size((int)Math.Round(screenWidth * 0.05), (int)Math.Round(screenWidth * 0.05));
            this.byPicture.Image = (Image)(new Bitmap((System.Drawing.Image)(resources.GetObject("credit")), elementSize));
            //this.byPicture.BackColor = System.Drawing.Color.Orange;
            this.byPicture.Size = elementSize;
            this.byPicture.Location = new System.Drawing.Point((int)Math.Round(screenWidth * 0.1), (int)Math.Round(screenHeight * 0.25));
            this.byPicture.Name = "pictureBox1";
            this.byPicture.TabStop = false;

            this.ncPicture.Image = (Image)(new Bitmap((System.Drawing.Image)(resources.GetObject("noncom")), elementSize));
            //this.ncPicture.BackColor = System.Drawing.Color.Orange;
            this.ncPicture.Size = elementSize;
            this.ncPicture.Location = new System.Drawing.Point((int)Math.Round(screenWidth * 0.1), (int)Math.Round(screenHeight * 0.35));
            this.ncPicture.Name = "pictureBox1";
            this.ncPicture.TabStop = false;

            //this.ncLabel.BackColor = System.Drawing.Color.Green;
            this.ncLabel.Size = new System.Drawing.Size((int)Math.Round(screenWidth * 0.7), (int)Math.Round(screenHeight * 0.05));
            this.ncLabel.Location = new System.Drawing.Point((int)Math.Round(screenWidth * 0.15), (int)Math.Round(screenHeight * 0.37));
            this.ncLabel.Text = "NC: Only non-commercial uses of the work are permitted.";
            this.ncLabel.TextAlign = ContentAlignment.MiddleLeft;

            this.ndPicture.Image = (Image)(new Bitmap((System.Drawing.Image)(resources.GetObject("adaptions")), elementSize));
            //this.ndPicture.BackColor = System.Drawing.Color.Orange;
            this.ndPicture.Size = elementSize;
            this.ndPicture.Location = new System.Drawing.Point((int)Math.Round(screenWidth * 0.1), (int)Math.Round(screenHeight * 0.45));
            this.ndPicture.Name = "pictureBox1";
            this.ndPicture.TabStop = false;

            //this.ndLabel.BackColor = System.Drawing.Color.Green;
            this.ndLabel.Size = new System.Drawing.Size((int)Math.Round(screenWidth * 0.7), (int)Math.Round(screenHeight * 0.05));
            this.ndLabel.Location = new System.Drawing.Point((int)Math.Round(screenWidth * 0.15), (int)Math.Round(screenHeight * 0.47));
            this.ndLabel.Text = "ND: No derivatives or adaptations of the work are permitted.";
            this.ndLabel.TextAlign = ContentAlignment.MiddleLeft;

            //this.copyLabel.BackColor = System.Drawing.Color.Green;
            this.copyLabel.Size = new System.Drawing.Size((int)Math.Round(screenWidth * 0.9), (int)Math.Round(screenHeight * 0.1));
            this.copyLabel.Location = new System.Drawing.Point((int)Math.Round(screenWidth * 0.05), (int)Math.Round(screenHeight * 0.55));
            this.copyLabel.Text = "Please note that copy and distribution of this software and associated manual is ONLY allowed on a per user basis. Any person wishing to use the CENT measure must apply for their own licence.";
            this.copyLabel.TextAlign = ContentAlignment.MiddleLeft;

            //this.newUsersLabel.BackColor = System.Drawing.Color.Green;
            this.newUsersLabel.Size = new System.Drawing.Size((int)Math.Round(screenWidth * 0.9), (int)Math.Round(screenHeight * 0.05));
            this.newUsersLabel.Location = new System.Drawing.Point((int)Math.Round(screenWidth * 0.05), (int)Math.Round(screenHeight * 0.65));
            this.newUsersLabel.Text = "Any new user must register via our website: https://expresslicensing.uea.ac.uk/";
            this.newUsersLabel.TextAlign = ContentAlignment.MiddleLeft;

            //this.comLabels.BackColor = System.Drawing.Color.Green;
            this.comLabels.Size = new System.Drawing.Size((int)Math.Round(screenWidth * 0.9), (int)Math.Round(screenHeight * 0.1));
            this.comLabels.Location = new System.Drawing.Point((int)Math.Round(screenWidth * 0.05), (int)Math.Round(screenHeight * 0.7));
            this.comLabels.Text = "Commercial users, users funded by industry or work involving translation may be allowed following registration via our website and payment of the required fees: https://expresslicensing.uea.ac.uk/";
            this.comLabels.TextAlign = ContentAlignment.MiddleLeft;

            this.button1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.button1.Location = new System.Drawing.Point((int)Math.Round(screenWidth * 0.4), (int)Math.Round(screenHeight * 0.825));
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size((int)Math.Round(screenWidth * 0.2), (int)Math.Round(screenHeight * 0.1));
            this.button1.TabIndex = 3;
            this.button1.Text = "OK";
            //Set the text Size - Set the font ot 8% of the screen height
            this.button1.Font = new Font("Arial", (int)Math.Round(screenHeight * 0.04));
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);

            //this.contactLabel.BackColor = System.Drawing.Color.Green;
            this.contactLabel.Size = new System.Drawing.Size((int)Math.Round(screenWidth * 0.5), (int)Math.Round(screenHeight * 0.15));
            this.contactLabel.Location = new System.Drawing.Point((int)Math.Round(screenWidth * 0.0025), (int)Math.Round(screenHeight * 0.825));
            this.contactLabel.Text = "Contact for queries:\r\nDr. Stephanie Rossit\r\nSchool of Psychology, University of East Anglia\r\ne-mail: s.rossit@uea.ac.uk Tel. +44 01603 591674";
            this.contactLabel.TextAlign = ContentAlignment.MiddleLeft;
            

            this.Controls.Add(this.button1);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.licenseLabel);
            this.Controls.Add(this.byPicture);
            this.Controls.Add(this.byLabel);
            this.Controls.Add(this.ncPicture);
            this.Controls.Add(this.ncLabel);
            this.Controls.Add(this.ndPicture);
            this.Controls.Add(this.ndLabel);
            this.Controls.Add(this.copyLabel);
            this.Controls.Add(this.newUsersLabel);
            this.Controls.Add(this.comLabels);
            this.Controls.Add(this.contactLabel);

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AdjustFontSizeToFit(Label label)
        {
            // Start with a reasonably large font size
            float fontSize = 10.0f;
            Font testFont = new Font(label.Font.FontFamily, fontSize, label.Font.Style);

            // Measure the text size
            Size textSize = TextRenderer.MeasureText(label.Text, testFont);

            // Increase the font size until the text no longer fits within the label's boundaries
            while (textSize.Width < label.Width && textSize.Height < label.Height)
            {
                fontSize += 0.5f;
                testFont = new Font(label.Font.FontFamily, fontSize, label.Font.Style);
                textSize = TextRenderer.MeasureText(label.Text, testFont);
            }

            // Set the font to the largest size that fits within the boundaries
            label.Font = new Font(label.Font.FontFamily, fontSize - 0.5f, label.Font.Style);
        }
    }
}
