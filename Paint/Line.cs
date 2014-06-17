using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Paint
{
    class Line : IGraphic
    {
        Point p1, p2;
        Pen p;
        public void init(Point p1, Pen p)
        {
            this.p = p;
            this.p1 = p1;
        }
        public void update(Point p1, Point p2)
        {
            this.p2 = p2;
            this.p1 = p1;
        }
        public void changeFill(Color c)
        {
        }
        public IGraphic create()
        {
            return new Line();
        }
        public void redraw(Graphics g)
        {
            g.DrawLine(p, p1, p2);
        }
        public bool isClosedForm()
        {
            return false;
        }
        public System.Drawing.Rectangle getBounds()
        {
            return new System.Drawing.Rectangle();
        }
    }
}
