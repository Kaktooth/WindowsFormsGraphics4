using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsGraphics4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            using (var g = Graphics.FromImage(pictureBox1.Image))
            {
                Brush b = Brushes.Black;
                Pen p = Pens.Black;

                int x = Convert.ToInt32(textBox1.Text);
                int y = Convert.ToInt32(textBox3.Text);
                int z = Convert.ToInt32(textBox2.Text);
                int XSize = Convert.ToInt32(textBox4.Text);
                int YSize = Convert.ToInt32(textBox5.Text);
                int dist = Convert.ToInt32(textBox6.Text);

                float sx = (XSize / 2) + ((x * dist) / (z - dist));
                float sy = (YSize / 2) - ((y * dist) / (z - dist));


                int XAngle = Convert.ToInt32(textBox7.Text);
                int YAngle = Convert.ToInt32(textBox8.Text);

              

                float rotatedX = 0;
                float rotatedY = 0;
                if (XAngle != 0)
                {
                    rotatedX = sx * (float)Math.Cos(XAngle) - sy * (float)Math.Sin(XAngle);
                }
                else
                {
                    rotatedX = sx;
                }

                if (YAngle != 0)
                {
                    rotatedY = sx * (float)Math.Sin(YAngle) + sy * (float)Math.Cos(YAngle);
                }
                else
                {
                    rotatedY = sx;
                }


                g.DrawRectangle(p, rotatedX, rotatedY, XSize, YSize);
                pictureBox1.Refresh();
            }
            stopwatch.Stop();
            label1.Text = stopwatch.Elapsed.ToString();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            using (var g = Graphics.FromImage(pictureBox1.Image))
            {
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
              
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            using (var g = Graphics.FromImage(pictureBox1.Image))
            {
                Brush b = Brushes.Black;
                Pen p = Pens.Black;
                int R = 80;
                int height = 40;
                int x = Convert.ToInt32(textBox1.Text);
                int y = Convert.ToInt32(textBox3.Text);
                int z = Convert.ToInt32(textBox2.Text);
                int XSize = Convert.ToInt32(textBox4.Text);
                int YSize = Convert.ToInt32(textBox5.Text);
                int dist = Convert.ToInt32(textBox6.Text);

                float sx = (XSize / 2) + ((x * dist) / (z - dist));
                float sy = (YSize / 2) - ((y * dist) / (z - dist));
                int zMove = z + R;
                float ax = (XSize / 2) + ((x * dist) / (zMove - dist));
                float ay = (YSize / 2) - ((y * dist) / (zMove - dist));
                zMove = z - R;
                float bx = (XSize / 2) + ((x * dist) / (zMove - dist));
                float by = (YSize / 2) - ((y * dist) / (zMove - dist));

                int xMove = x - R;
                float cx = (XSize / 2) + ((xMove * dist) / (z - dist));
                float cy = (YSize / 2) - ((y * dist) / (z - dist));
                xMove = x + R;
                float dx = (XSize / 2) + ((xMove * dist) / (z - dist));
                float dy = (YSize / 2) - ((y * dist) / (z - dist));
                int yMove = y + R;
                float ex = (XSize / 2) + ((x * dist) / (z - dist));
                float ey = (YSize / 2) - ((yMove * dist) / (z - dist));
                // g.FillRectangle(b, sx, sy, 2, 2);
                //g.DrawLine(p, sx, sy, ax, ay); 
                //g.DrawLine(p, sx, sy, bx, by);
                //g.DrawLine(p, sx, sy, cx, cy);
                //g.DrawLine(p, sx, sy, dx, dy);

                g.DrawLine(p, bx, by, dx, dy);
                g.DrawLine(p, bx, by, cx, cy);
                g.DrawLine(p, cx, cy, dx,dy);

                g.DrawLine(p, bx, by, ex, ey);
                g.DrawLine(p, cx, cy, ex, ey);
                g.DrawLine(p, dx, dy, ex, ey);
                pictureBox1.Refresh();
            }
            stopwatch.Stop();
            label1.Text = stopwatch.Elapsed.ToString();
        }
    }
}
