using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScreenShifter
{
    public partial class FormSelection : Form
    {

        public delegate void SimpleDelegate(Image context);

        public SimpleDelegate extra;

        public FormSelection(SimpleDelegate extra)
        {
            InitializeComponent();
            this.extra = extra;
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        private void FormSelection_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            Rectangle r = Utility.GetRectangleFromScreen(true);
            this.Top = r.Top;
            this.Left = r.Left;
            this.Size = new Size(r.Width, r.Height);
            this.TopMost = true;

            pictureBox1.Image = Utility.GetImageFromScreen(r);
            pictureBox1.Dock = DockStyle.Fill;
        }

        private void FormSelection_Shown(object sender, EventArgs e)
        {
            this.BringToFront();
            this.Focus();
            this.Activate();
        }

        private void Bye()
        {
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
            this.Close();
            this.Dispose();
        }

        private void FormSelection_KeyPress(object sender, KeyPressEventArgs e)
        {
            Bye();
        }

        private void FormSelection_Deactivate(object sender, EventArgs e)
        {
            Bye();
        }

        Point start;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                Bye();
                return;
            }
            start = e.Location;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            pictureBox1.Refresh();
            using (var g = pictureBox1.CreateGraphics())
            {
                Rectangle rc = new Rectangle(Math.Min(start.X, e.Location.X), Math.Min(start.Y, e.Location.Y), Math.Abs(start.X - e.Location.X), Math.Abs(start.Y - e.Location.Y));
                using (var pen = new Pen(Settings.Instance.SelectionColor, 3))
                {
                    g.DrawRectangle(pen, rc);
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            //using (var g = Graphics.FromImage(pictureBox1.Image))
            //{
                Rectangle rc = new Rectangle(Math.Min(start.X, e.Location.X), Math.Min(start.Y, e.Location.Y), Math.Abs(start.X - e.Location.X), Math.Abs(start.Y - e.Location.Y));
                var img = ((Bitmap)pictureBox1.Image).Clone(rc, pictureBox1.Image.PixelFormat);
                extra(img);
            //}

            Bye();
        }

    }
}
