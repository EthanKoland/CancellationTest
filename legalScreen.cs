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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();

            int screenWidth = 640;
            int screenHeight = 360;

            int groupBoxWidth = (int)Math.Round(screenWidth * 0.6);
            int groupBoxHeight = (int)Math.Round(screenHeight * 0.8);

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

            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(screenWidth, screenHeight);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "Some legal stuff";
            this.textBox1.Font = new Font("Arial", (int)Math.Round(screenHeight * 0.04));
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = ScrollBars.Vertical;
            this.textBox1.WordWrap = true;

            //Add the controls to the form
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
