using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Paint
{
    interface IGraphic
    {
        void init(Point p1, Pen p);
        void redraw(Graphics g);
        void update(Point p1, Point p2);
        void changeFill(Color c);
        bool isClosedForm();
        System.Drawing.Rectangle getBounds();
        IGraphic create();
    }
}
