using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace ScreenShifter
{
    public partial class FormPreview : Form
    {

        public delegate void SimpleDelegate(Image context);

        public SimpleDelegate extra;

        public FormPreview(Image img, SimpleDelegate extra)
        {
            InitializeComponent();
            this.extra = extra;
            pictureBox.Image = img;
            toolStripComboBoxShapeType.Items.AddRange(Enum.GetValues(typeof(ShapeType)).Cast<ShapeType>().Select(t => t.ToString()).ToArray());
            toolStripComboBoxShapeType.SelectedIndex = 0;
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        enum ShapeType
        {
            Pen,
            Line,
            Circle,
            Rectangle,
            Polygon
        }

        struct Shape
        {
            public GraphicsPath path;
            public bool edge;
            public Color edgeColour;
            public int edgeWidth;
            public bool fill;
            public Color fillColour;
        }

        GraphicsPath path = new GraphicsPath();
        Point lastPoint = Point.Empty;
        Color edge = Color.Black;
        Color fill = Color.Red;

        List<Shape> shapes = new List<Shape>();

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = e.Location;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (lastPoint == Point.Empty)
                return;
            switch (GetCurrentShapeType())
            {
                case ShapeType.Pen:
                case ShapeType.Polygon:
                    path.AddLine(lastPoint, e.Location);
                    lastPoint = e.Location;
                    break;

                case ShapeType.Line:
                    path.Reset();
                    path.AddLine(lastPoint, e.Location);
                    break;

                case ShapeType.Circle:
                    path.Reset();
                    path.AddEllipse(Math.Min(lastPoint.X, e.Location.X), Math.Min(lastPoint.Y, e.Location.Y), Math.Abs(lastPoint.X - e.Location.X), Math.Abs(lastPoint.Y - e.Location.Y));
                    break;

                case ShapeType.Rectangle:
                    path.Reset();
                    path.AddRectangle(new Rectangle(Math.Min(lastPoint.X, e.Location.X), Math.Min(lastPoint.Y, e.Location.Y), Math.Abs(lastPoint.X - e.Location.X), Math.Abs(lastPoint.Y - e.Location.Y)));
                    break;
            }
            pictureBox.Invalidate();
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (GetCurrentShapeType() == ShapeType.Polygon)
            {
                path.CloseFigure();
                pictureBox.Invalidate();
            }
            if (toolStripMenuItemEdgeEnabled.Checked || toolStripMenuItemFillEnabled.Checked)
            {
                var s = new Shape();
                s.edge = toolStripMenuItemEdgeEnabled.Checked;
                s.edgeColour = Color.FromArgb(toint(toolStripTextBoxEdgeAlpha.Text, 255), edge);
                s.edgeWidth = toint(toolStripTextBoxEdgeWidth.Text, 3);
                s.fill = toolStripMenuItemFillEnabled.Checked;
                s.fillColour = Color.FromArgb(toint(toolStripTextBoxFillAlpha.Text, 50), fill);
                s.path = (GraphicsPath)path.Clone();
                shapes.Add(s);
            }
            lastPoint = Point.Empty;
            path.Reset();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var s in shapes)
                paint(e.Graphics, s.path, s.edge, s.edgeColour, s.edgeWidth, s.fill, s.fillColour);

            paint(e.Graphics, path, toolStripMenuItemEdgeEnabled.Checked, Color.FromArgb(toint(toolStripTextBoxEdgeAlpha.Text, 255), edge), toint(toolStripTextBoxEdgeWidth.Text, 3), toolStripMenuItemFillEnabled.Checked, Color.FromArgb(toint(toolStripTextBoxFillAlpha.Text, 50), fill));
        }

        private void paint(Graphics g, GraphicsPath path, bool edge, Color edgeColour, int edgeWidth, bool fill, Color fillColour)
        {
            if (edge)
                g.DrawPath(new Pen(edgeColour, edgeWidth), path);
            if (fill)
                g.FillPath(new SolidBrush(fillColour), path);
        }

        private ShapeType GetCurrentShapeType()
        {
            return (ShapeType)Enum.Parse(typeof(ShapeType), toolStripComboBoxShapeType.Text);
        }

        private void toolStripButtonUndo_Click(object sender, EventArgs e)
        {
            if (shapes.Count <= 0)
                return;
            shapes.RemoveAt(shapes.Count - 1);
            pictureBox.Invalidate();
        }

        private void toolStripMenuItemEdgeFill_Click(object sender, EventArgs e)
        {
            toolStripMenuItemEdgeEnabled.Checked = !toolStripMenuItemEdgeEnabled.Checked;
        }

        private void toolStripMenuItemFillEnabled_Click(object sender, EventArgs e)
        {
            toolStripMenuItemFillEnabled.Checked = !toolStripMenuItemFillEnabled.Checked;
        }

        private int toint(string s, int def)
        {
            try
            {
                Convert.ToInt32(s);
            }
            catch (Exception) { }
            return def;
        }

        private void toolStripMenuItemEdgeFillColour_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = edge;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                edge = colorDialog1.Color;
            }
        }

        private void toolStripMenuItemFillColour_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = fill;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                fill = colorDialog1.Color;
            }
        }

        private void toolStripButtonUpload_Click(object sender, EventArgs e)
        {
            var img = (Image)pictureBox.Image.Clone();
            using (var g = Graphics.FromImage(img))
            {
                foreach (var s in shapes)
                    paint(g, s.path, s.edge, s.edgeColour, s.edgeWidth, s.fill, s.fillColour);
            }
            extra(img);
            this.Close();
            this.Dispose();
        }

        private void FormPreview_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (pictureBox.Image != null)
                pictureBox.Image.Dispose();
        }

    }
}
