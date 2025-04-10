using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.Common.CommonGUI
{
    public partial class LoadingForm : Form
    {
        private Timer timer;
        private int angle = 0;

        public LoadingForm()
        {
            InitializeComponent();

            // 🔧 Form setup
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;
            this.Opacity = 0.7; // semi-transparent background
            //this.TransparencyKey = Color.Black; // hides the background
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.DoubleBuffered = true;


            // 🔁 Spinner animation
            timer = new Timer();
            timer.Interval = 30;
            timer.Tick += (s, e) =>
            {
                angle += 6;
                if (angle >= 360) angle = 0;
                this.Invalidate(); // force repaint
            };
            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawSpinner(e.Graphics); // Call your spinner code
        }

        private void DrawSpinner(Graphics g)
        {
            int size = 60;
            int lineLength = 10;
            int lineWidth = 4;
            int count = 12;
            Point center = new Point(this.Width / 2, this.Height / 2);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            for (int i = 0; i < count; i++)
            {
                int alpha = (int)(255.0 * i / count);
                using (Pen pen = new Pen(Color.White, lineWidth))
                {
                    pen.Color = Color.FromArgb(alpha, pen.Color);
                    double theta = (Math.PI * 2 * (i + angle / 30.0)) / count;
                    float x1 = center.X + (float)Math.Cos(theta) * (size / 2);
                    float y1 = center.Y + (float)Math.Sin(theta) * (size / 2);
                    float x2 = center.X + (float)Math.Cos(theta) * (size / 2 + lineLength);
                    float y2 = center.Y + (float)Math.Sin(theta) * (size / 2 + lineLength);
                    g.DrawLine(pen, x1, y1, x2, y2);
                }
            }
        }

    }
}
