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
        delegate void Del(object sender, EventArgs e);
        Del d;
        public class View3D
        {
            public class Point3D
            {
                public double X;
                public double Y;
                public double Z;

                public Point3D(int x, int y, int z)
                {
                    X = x;
                    Y = y;
                    Z = z;
                }

                public Point3D(float x, float y, float z)
                {
                    X = (double)x;
                    Y = (double)y;
                    Z = (double)z;
                }

                public Point3D(double x, double y, double z)
                {
                    X = x;
                    Y = y;
                    Z = z;
                }

                public Point3D()
                {
                }

                public override string ToString()
                {
                    return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
                }
            }

            public static Point3D RotateX(Point3D point3D, double degrees)
            {
                double cDegrees = (Math.PI * degrees) / 180.0f;
                double x = (point3D.X * Math.Cos(cDegrees)) + (point3D.Z * Math.Sin(cDegrees));
                double z = (point3D.X * -Math.Sin(cDegrees)) + (point3D.Z * Math.Cos(cDegrees));

                return new Point3D(x, point3D.Y, z);
            }

            public static Point3D RotateY(Point3D point3D, double degrees)
            {
                double cDegrees = (Math.PI * degrees) / 180.0;
                double y = (point3D.Y * Math.Cos(cDegrees)) + (point3D.Z * Math.Sin(cDegrees));
                double z = (point3D.Y * -Math.Sin(cDegrees)) + (point3D.Z * Math.Cos(cDegrees));

                return new Point3D(point3D.X, y, z);
            }

           

            public static Point3D Translate(Point3D points3D, Point3D oldOrigin, Point3D newOrigin)
            {
                Point3D difference = new Point3D(newOrigin.X - oldOrigin.X, newOrigin.Y - oldOrigin.Y, newOrigin.Z - oldOrigin.Z);
                points3D.X += difference.X;
                points3D.Y += difference.Y;
                points3D.Z += difference.Z;
                return points3D;
            }

      
            public static Point3D[] RotateX(Point3D[] points3D, double degrees)
            {
                for (int i = 0; i < points3D.Length; i++)
                {
                    points3D[i] = RotateX(points3D[i], degrees);
                }
                return points3D;
            }

            public static Point3D[] RotateY(Point3D[] points3D, double degrees)
            {
                for (int i = 0; i < points3D.Length; i++)
                {
                    points3D[i] = RotateY(points3D[i], degrees);
                }
                return points3D;
            }

            public static Point3D[] Translate(Point3D[] points3D, Point3D oldOrigin, Point3D newOrigin)
            {
                for (int i = 0; i < points3D.Length; i++)
                {
                    points3D[i] = Translate(points3D[i], oldOrigin, newOrigin);
                }
                return points3D;
            }

        }
        public static View3D.Point3D[] XYZPoints(int width, int height, int depth)
        {
            View3D.Point3D[] verts = new View3D.Point3D[4];

            verts[0] = new View3D.Point3D(0, 0, 0);
            verts[1] = new View3D.Point3D(0, height, 0);
            verts[2] = new View3D.Point3D(width, 0, 0);
            verts[3] = new View3D.Point3D(0, 0, depth);

            return verts;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            d = button1_Click;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            pictureBox1.Image = new Bitmap(pictureBox1.Width,pictureBox1.Height);
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
                int XAngle = Convert.ToInt32(trackBar1.Value);
                int YAngle = Convert.ToInt32(trackBar2.Value);
                int Size = Convert.ToInt32(textBox7.Text);
                PointF[] point3D = new PointF[4];
                View3D.Point3D point0 = checkBox1.Checked ? new View3D.Point3D(0, 0, 0) : new View3D.Point3D(x, y, z);
                View3D.Point3D[] xyzPoints = XYZPoints((int)Size, (int)Size, (int)Size);

                View3D.Point3D cubeOrigin = new View3D.Point3D((int)(Size / 2), (int)(Size / 2), (int)(Size / 2));
                xyzPoints = View3D.Translate(xyzPoints, cubeOrigin, point0);
                xyzPoints = View3D.RotateX(xyzPoints, XAngle);
                xyzPoints = View3D.RotateY(xyzPoints, YAngle);

                xyzPoints = View3D.Translate(xyzPoints, point0, cubeOrigin);
                for (int i = 0; i < point3D.Length; i++)
                {
                    point3D[i].X = ((XSize / 2) + (((float)xyzPoints[i].X * dist) / ((float)xyzPoints[i].Z + dist)));
                    point3D[i].Y = (YSize / 2) - (((float)xyzPoints[i].Y * dist) / ((float)xyzPoints[i].Z + dist));

                }

                g.DrawLine(Pens.Red, point3D[0], point3D[1]);
                g.DrawLine(Pens.Green, point3D[0], point3D[2]);
                g.DrawLine(Pens.Blue, point3D[0], point3D[3]);
           
                pictureBox1.Refresh();
            }
            stopwatch.Stop();
            label1.Text = "Runtime: " + stopwatch.Elapsed.ToString();
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
                
                int x = Convert.ToInt32(textBox1.Text);
                int y = Convert.ToInt32(textBox3.Text);
                int z = Convert.ToInt32(textBox2.Text);
                int XSize = Convert.ToInt32(textBox4.Text);
                int YSize = Convert.ToInt32(textBox5.Text);
                int dist = Convert.ToInt32(textBox6.Text);
                int R = Convert.ToInt32(textBox7.Text);

                float sx = (XSize / 2) + ((x * dist) / (z + dist));
                float sy = (YSize / 2) - ((y * dist) / (z + dist));
                int zMove = z + R;
                float ax = (XSize / 2) + ((x * dist) / (zMove + dist));
                float ay = (YSize / 2) - ((y * dist) / (zMove + dist));
                zMove = z - R;
                float bx = (XSize / 2) + ((x * dist) / (zMove + dist));
                float by = (YSize / 2) - ((y * dist) / (zMove + dist));

                int xMove = x - R;
                float cx = (XSize / 2) + ((xMove * dist) / (z + dist));
                float cy = (YSize / 2) - ((y * dist) / (z + dist));
                xMove = x + R;
                float dx = (XSize / 2) + ((xMove * dist) / (z + dist));
                float dy = (YSize / 2) - ((y * dist) / (z + dist));
                int yMove = y + R;
                float ex = (XSize / 2) + ((x * dist) / (z + dist));
                float ey = (YSize / 2) - ((yMove * dist) / (z + dist));
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
            label1.Text = "Runtime: "+stopwatch.Elapsed.ToString();
        }
        public static View3D.Point3D[] CubePoints(int width, int height, int depth)
        {
            View3D.Point3D[] verts = new View3D.Point3D[24];

            verts[0] = new View3D.Point3D(0, 0, 0);
            verts[1] = new View3D.Point3D(0, height, 0);
            verts[2] = new View3D.Point3D(width, height, 0);
            verts[3] = new View3D.Point3D(width, 0, 0);


            verts[4] = new View3D.Point3D(0, 0, depth);
            verts[5] = new View3D.Point3D(0, height, depth);
            verts[6] = new View3D.Point3D(width, height, depth);
            verts[7] = new View3D.Point3D(width, 0, depth);


            verts[8] = new View3D.Point3D(0, 0, 0);
            verts[9] = new View3D.Point3D(0, 0, depth);
            verts[10] = new View3D.Point3D(0, height, depth);
            verts[11] = new View3D.Point3D(0, height, 0);

            verts[12] = new View3D.Point3D(width, 0, 0);
            verts[13] = new View3D.Point3D(width, 0, depth);
            verts[14] = new View3D.Point3D(width, height, depth);
            verts[15] = new View3D.Point3D(width, height, 0);

        
            verts[16] = new View3D.Point3D(0, height, 0);
            verts[17] = new View3D.Point3D(0, height, depth);
            verts[18] = new View3D.Point3D(width, height, depth);
            verts[19] = new View3D.Point3D(width, height, 0);


            verts[20] = new View3D.Point3D(0, 0, 0);
            verts[21] = new View3D.Point3D(0, 0, depth);
            verts[22] = new View3D.Point3D(width, 0, depth);
            verts[23] = new View3D.Point3D(width, 0, 0);

            return verts;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            d = new Del(button4_Click);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
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
                int Size = Convert.ToInt32(textBox7.Text);
                int XAngle = Convert.ToInt32(trackBar1.Value);
                int YAngle = Convert.ToInt32(trackBar2.Value)+90;
                int notRotatedX = x;
                int notRotatedY = y;
                PointF[] point3D = new PointF[24];
                
                View3D.Point3D point0 =checkBox1.Checked ? new View3D.Point3D(0, 0, 0) : new View3D.Point3D(x, y, z);
                View3D.Point3D[] cubePoints = CubePoints(Size,Size,Size);
                View3D.Point3D cubeOrigin = new View3D.Point3D((int)(Size / 2),(int)(Size / 2), (int)(Size / 2));
                cubePoints = View3D.Translate(cubePoints, cubeOrigin, point0);
                cubePoints = View3D.RotateX(cubePoints,XAngle);
                cubePoints = View3D.RotateY(cubePoints, YAngle); 
          
                cubePoints = View3D.Translate(cubePoints, point0, cubeOrigin);
                for (int i = 0; i < point3D.Length; i++)
                {
                    point3D[i].X = ((XSize / 2) + (((float)cubePoints[i].X * dist) / ((float)cubePoints[i].Z+ dist)));
                    point3D[i].Y = (YSize / 2) - (((float)cubePoints[i].Y * dist) / ((float)cubePoints[i].Z + dist));
                    
                }

                g.DrawLine(Pens.Red, point3D[0], point3D[1]);
                g.DrawLine(Pens.Red, point3D[1], point3D[2]);
                g.DrawLine(Pens.Red, point3D[2], point3D[3]);
                g.DrawLine(Pens.Red, point3D[3], point3D[0]);

                g.DrawLine(Pens.Black, point3D[4], point3D[5]);
                g.DrawLine(Pens.Black, point3D[5], point3D[6]);
                g.DrawLine(Pens.Black, point3D[6], point3D[7]);
                g.DrawLine(Pens.Black, point3D[7], point3D[4]);

                g.DrawLine(Pens.Black, point3D[8], point3D[9]);
                g.DrawLine(Pens.Black, point3D[9], point3D[10]);
                g.DrawLine(Pens.Black, point3D[10], point3D[11]);
                g.DrawLine(Pens.Black, point3D[11], point3D[8]);

                g.DrawLine(Pens.Black, point3D[12], point3D[13]);
                g.DrawLine(Pens.Black, point3D[13], point3D[14]);
                g.DrawLine(Pens.Black, point3D[14], point3D[15]);
                g.DrawLine(Pens.Black, point3D[15], point3D[12]);

                g.DrawLine(Pens.Green, point3D[16], point3D[17]);
                g.DrawLine(Pens.Green, point3D[17], point3D[18]);
                g.DrawLine(Pens.Green, point3D[18], point3D[19]);
                g.DrawLine(Pens.Green, point3D[19], point3D[16]);

                g.DrawLine(Pens.Black, point3D[20], point3D[21]);
                g.DrawLine(Pens.Black, point3D[21], point3D[22]);
                g.DrawLine(Pens.Black, point3D[22], point3D[23]);
                g.DrawLine(Pens.Black, point3D[23], point3D[20]);

                pictureBox1.Refresh();
            }
            stopwatch.Stop();
            label1.Text = "Runtime: " + stopwatch.Elapsed.ToString();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            d(sender, e);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            d(sender, e);
        }
    }
}
