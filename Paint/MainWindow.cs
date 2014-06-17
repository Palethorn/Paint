using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Paint
{
    public partial class MainWindow : Form
    {
        Bitmap bitmap;
        Graphics g;
        Pen pen;
        Point p1;
        Point p2;
        IGraphic graphic;
        List<IGraphic> graphics;
        public MainWindow()
        {
            InitializeComponent();
            bitmap = new Bitmap(canvas.Width, canvas.Height);
            g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);
            canvas.BackgroundImage = bitmap;
            pen = new Pen(new SolidBrush(Color.Red), 2);
            canvas.MouseDown += getFirstPoint;

            rectangleTool.Click += setRectangleTool;
            ellipseTool.Click += setEllipseTool;
            lineTool.Click += setLineTool;
            fillBucketTool.Click += setFillBucketTool;
            pencilTool.Click += setPencilTool;

            graphics = new List<IGraphic>();
            canvas.Paint += paint;

            openToolStripMenuItem.Click += new EventHandler((o, a) =>
            {
                OpenFileDialog opendialog = new OpenFileDialog();
                opendialog.ShowDialog();
                try
                {
                    bitmap = new Bitmap(opendialog.FileName);
                    canvas.BackgroundImage = bitmap;
                }
                catch
                { 
                    // Pogresan file
                }
                canvas.Invalidate();
            });
            saveToolStripMenuItem.Click += new EventHandler((o, a) =>
            {
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.ShowDialog();
                try
                {
                    bitmap.Save(savedialog.FileName);
                }
                catch
                {
                    // Pogresan file
                }
            });
            exitToolStripMenuItem.Click += new EventHandler((o, a) =>
            {
                Application.Exit();
            });

            lineToolStripMenuItem.Click += setLineTool;
            ellipseToolStripMenuItem.Click += setEllipseTool;
            rectangleToolStripMenuItem.Click += setRectangleTool;
            fillBucketToolStripMenuItem.Click += setFillBucketTool;
            pencilToolStripMenuItem.Click += setPencilTool;

            strokeInput.TextChanged += changePen;
            strokeInput.GotFocus += showTrackBar;
            strokeTrackBar.LostFocus += hideTrackBar;
            strokeTrackBar.ValueChanged += changeStroke;
            canvas.MouseDown += hideTrackBar;
            colorInput.TextChanged += changePen;

            colorPreview.BackColor = pen.Color;

            setPencilTool(null, null);
        }
        public void changePen(object sender, EventArgs args)
        {
            short width = 0;
            if (Int16.TryParse(strokeInput.Text, out width))
            {
                int r = 0, gr = 0, b = 0;
                SolidBrush brush;
                if(colorInput.Text.Length % 6 == 0)
                {
                    r = Int32.Parse(colorInput.Text.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    gr = Int32.Parse(colorInput.Text.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    b = Int32.Parse(colorInput.Text.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    brush = new SolidBrush(Color.FromArgb(255, r, gr, b));
                }
                else
                {
                    brush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
                }
                colorPreview.BackColor = brush.Color;
                pen = new Pen(brush, width);
            }
        }
        void getFirstPoint(object sender, MouseEventArgs args)
        {
            p1 = args.Location;
            graphics.Add(graphic);
            graphic.init(p1, pen);
            canvas.MouseMove += updateSecondPoint;
            canvas.MouseUp += releaseMouse;
        }
        void updateSecondPoint(object sender, MouseEventArgs args)
        {
            p2 = args.Location;
            graphic.update(p1, p2);
            graphic.redraw(g);
            canvas.Invalidate();
        }
        void releaseMouse(object sender, EventArgs args)
        {
            canvas.MouseMove -= updateSecondPoint;
            canvas.MouseUp -= releaseMouse;
            graphic = graphic.create();
        }
        public void setRectangleTool(object sender, EventArgs args)
        {
            canvas.MouseUp -= setFill;
            canvas.MouseDown -= getFirstPoint;
            canvas.MouseDown += getFirstPoint;
            graphic = new Rectangle();
        }
        public void setEllipseTool(object sender, EventArgs args)
        {
            canvas.MouseUp -= setFill;
            canvas.MouseDown -= getFirstPoint;
            canvas.MouseDown += getFirstPoint;
            graphic = new Ellipse();
        }
        public void setLineTool(object sender, EventArgs args)
        {
            canvas.MouseUp -= setFill;
            canvas.MouseDown -= getFirstPoint;
            canvas.MouseDown += getFirstPoint;
            graphic = new Line();
        }
        public void setPencilTool(object sender, EventArgs args)
        {
            canvas.MouseUp -= setFill;
            canvas.MouseDown -= getFirstPoint;
            canvas.MouseDown += getFirstPoint;
            graphic = new Pencil();
        }
        public void setFillBucketTool(object sender, EventArgs args)
        {
            graphic = null;
            canvas.MouseDown -= getFirstPoint;
            canvas.MouseUp -= releaseMouse;
            canvas.MouseMove -= updateSecondPoint;
            canvas.MouseDown -= setFill;
            canvas.MouseDown += setFill;
        }
        public void showTrackBar(object sender, EventArgs args)
        {
            strokeTrackBar.Visible = true;
        }
        public void hideTrackBar(object sender, EventArgs args)
        {
            strokeTrackBar.Visible = false;
        }
        public void changeStroke(object sender, EventArgs args)
        {
            strokeInput.Text = strokeTrackBar.Value.ToString();
        }
        public void setFill(object sender, MouseEventArgs args)
        {
            int i = 0;
            Point tmp = args.Location;
            for (i = graphics.Count - 1; i >= 0; i--)
            {
                if (graphics[i].isClosedForm())
                {
                    System.Drawing.Rectangle rect = graphics[i].getBounds();
                    if (rect.X <= tmp.X && (rect.X + rect.Width) >= tmp.X && rect.Y <= tmp.Y && (rect.Y + rect.Height) >= tmp.Y)
                    {
                        int r = 0, gr = 0, b = 0;
                        Color c;
                        if (colorInput.Text.Length % 6 == 0)
                        {
                            r = Int32.Parse(colorInput.Text.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                            gr = Int32.Parse(colorInput.Text.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                            b = Int32.Parse(colorInput.Text.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                            c = Color.FromArgb(255, r, gr, b);
                        }
                        else
                        {
                            c = Color.FromArgb(255, 0, 0, 0);
                        }
                        graphics[i].changeFill(c);
                        paint(null, null);
                        canvas.Invalidate();
                        canvas.Update();
                        return;
                    }
                }
            }
        }
        public void paint(object sender, PaintEventArgs args)
        {
            g.Clear(Color.White);
            foreach (IGraphic graf in graphics)
            {
                graf.redraw(g);
            }
        }
    }
}