using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace Paint
{
    class Ellipse : IGraphic
    {
        System.Drawing.Rectangle rect;
        Pen p;
        SolidBrush brush;
        public void init(Point p1, Pen p)
        {
            brush = (SolidBrush)p.Brush;
            rect = new System.Drawing.Rectangle(p1, new Size(0, 0));
            this.p = p;
        }

        public void update(Point p1, Point p2)
        {
            if (p2.X < p1.X)
            {
                p2.X ^= p1.X;
                p1.X ^= p2.X;
                p2.X ^= p1.X;
            }
            if (p2.Y < p1.Y)
            {
                p2.Y ^= p1.Y;
                p1.Y ^= p2.Y;
                p2.Y ^= p1.Y;
            }
            rect.Location = p1;
            int deltaX = p2.X - p1.X;
            int deltaY = p2.Y - p1.Y;
            int width = deltaX > 0 ? deltaX : -deltaX;
            int height = deltaY > 0 ? deltaY : -deltaY;
            Size size = new Size(width, height);
            rect.Size = size;
        }

        public void changeFill(Color c)
        {
            brush = new SolidBrush(c);
        }
        public IGraphic create()
        {
            return new Ellipse();
        }
        public void redraw(Graphics g)
        {
            if (rect != null)
            {
                g.DrawEllipse(p, rect);
                if (brush != null)
                {
                    g.FillEllipse(this.brush, rect);
                }
            }
        }
        public bool isClosedForm()
        {
            return true;
        }
        public System.Drawing.Rectangle getBounds()
        {
            return rect;
        }
    }
}
