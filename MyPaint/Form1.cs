using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPaint
{
    public partial class Form1 : Form
    {
        Color color;
        Bitmap snap, temp;
        Point[] points = new Point[3];
        string select = "";

        bool isClick = false;
        bool line = false;
        bool rect = false;
        bool ellip = false;
        bool triangle = false;

        int X = 0;
        int Y = 0;
        int X1 = 0;
        int Y1 = 0;
        int count = 0;

        public Form1()
        {
            InitializeComponent();

            this.Text = "MyPaint";
            snap = new Bitmap(pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height);
            temp = (Bitmap)snap.Clone();
            color = Color.Black;

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            isClick = true;
            X = e.X;
            Y = e.Y;
            temp = (Bitmap)snap.Clone();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isClick)
            {
                X1 = e.X;
                Y1 = e.Y;
                pictureBox1.Invalidate();
                pictureBox1.Update();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isClick = false;
            snap = (Bitmap)temp.Clone();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (line)
                select = "Line";
            if (rect)
                select = "Rectangle";
            if (ellip)
                select = "Ellipse";
            if (triangle)
                select = "Triangle";
            temp = (Bitmap)snap.Clone();
            Graphics g = Graphics.FromImage(temp);
            g.SmoothingMode = SmoothingMode.HighQuality;
            Pen pen = new Pen(color, (float)numericUpDown1.Value);

            switch (select)
            {
                case "Line":
                    if (temp != null)
                        g.DrawLine(pen, new Point(X, Y), new Point(X1, Y1));
                    break;
                case "Rectangle":
                    Rectangle rectangle;
                    if (X > X1 && Y > Y1)
                        rectangle = new Rectangle(X1, Y1, Math.Abs(X - X1), Math.Abs(Y - Y1));
                    else if (X < X1 && Y > Y1)
                        rectangle = new Rectangle(X, Y1, Math.Abs(X - X1), Math.Abs(Y - Y1));
                    else if (X > X1 && Y < Y1)
                        rectangle = new Rectangle(X1, Y, Math.Abs(X - X1), Math.Abs(Y - Y1));
                    else
                        rectangle = new Rectangle(X, Y, Math.Abs(X - X1), Math.Abs(Y - Y1));
                    if (temp != null)
                        g.DrawRectangle(pen, rectangle);
                    break;
                case "Ellipse":
                    if (X > X1 && Y > Y1)
                        rectangle = new Rectangle(X1, Y1, Math.Abs(X - X1), Math.Abs(Y - Y1));
                    else if (X < X1 && Y > Y1)
                        rectangle = new Rectangle(X, Y1, Math.Abs(X - X1), Math.Abs(Y - Y1));
                    else if (X > X1 && Y < Y1)
                        rectangle = new Rectangle(X1, Y, Math.Abs(X - X1), Math.Abs(Y - Y1));
                    else
                        rectangle = new Rectangle(X, Y, Math.Abs(X - X1), Math.Abs(Y - Y1));
                    if (temp != null)
                        g.DrawEllipse(pen, rectangle);
                    break;
                case "Triangle":
                    g.DrawPolygon(pen, points);
                    points = new Point[3];
                    count = 0;
                    break;
                default:
                    break;
            }

            pen.Dispose();
            e.Graphics.DrawImageUnscaled(temp, 0, 0);
            g.Dispose();

        }


        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            line = true;
            rect = false;
            ellip = false;
            triangle = false;
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rect = true;
            line = false;
            ellip = false;
            triangle = false;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Cross;
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void ellipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ellip = true;
            line = false;
            rect = false;
            triangle = false;
        }

        private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            triangle = true;
            ellip = false;
            line = false;
            rect = false;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (triangle)
            {
                if (count <= 2)
                {
                    points[count] = new Point(e.X, e.Y);
                    count++;
                }
                if (count == 3)
                {
                    pictureBox1.Invalidate();
                    pictureBox1.Update();
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            if (c.ShowDialog() == DialogResult.OK)
            {
                color = c.Color;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);
            pictureBox1.Image = null;
            snap = new Bitmap(pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height);
            temp = (Bitmap)snap.Clone();
            g.Dispose();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "JPEG Images (*.jpg)|*.jpg|GIF Images (*.gif)|*.gif|" +
                "BITMAPS (*.bmp)|*.bmp|PNG Images (*.png)|*.png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                string str = fileName.Remove(0, fileName.Length - 3);
                switch (str)
                {
                    case "jpg":
                        snap.Save(fileName, ImageFormat.Jpeg);
                        break;
                    case "gif":
                        snap.Save(fileName, ImageFormat.Gif);
                        break;
                    case "bmp":
                        snap.Save(fileName, ImageFormat.Bmp);
                        break;
                    case "png":
                        snap.Save(fileName, ImageFormat.Png);
                        break;
                    default:
                        break;
                }
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);
            snap = new Bitmap(pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height);
            temp = (Bitmap)snap.Clone();
            g.Dispose();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPEG Images (*.jpg)|*.jpg|GIF Images (*.gif)|*.gif|" +
                "BITMAPS (*.bmp)|*.bmp|PNG Images (*.png)|*.png";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                snap = (Bitmap)pictureBox1.Image;
                temp = (Bitmap)snap.Clone();
            }
        }
    }

    class LinePoints
    {
        public Point point1;
        public Point point2;

        public LinePoints(Point p1, Point p2)
        {
            point1 = p1;
            point2 = p2;
        }
    }
}
