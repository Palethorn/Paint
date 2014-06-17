using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Paint
{
    class Pencil : IGraphic
    {
        Pen p;
        List<Point> points;
        public void init(System.Drawing.Point p1, System.Drawing.Pen p)
        {
            this.p = p;
            points = new List<Point>();
            points.Add(p1);
        }

        public void redraw(System.Drawing.Graphics g)
        {
            int i = 0;
            for(i = 0; i < points.Count - 1 && points.Count >= 2; i++)
            {
                g.DrawLine(p, points[i], points[i + 1]);
            }
        }

        public void update(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            points.Add(p2);
        }

        public void changeFill(System.Drawing.Color c)
        {
            
        }

        public bool isClosedForm()
        {
            return false;
        }

        public System.Drawing.Rectangle getBounds()
        {
            return new System.Drawing.Rectangle();
        }

        public IGraphic create()
        {
            return new Pencil();
        }
    }
}
