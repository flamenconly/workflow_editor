using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfApplication1.elements.shapes
{
    internal class RectangleDrawingObject : IDrawable
    {
        public Rect Shape { get; set; }
        public Brush Brush { get; set; }
        public Pen Pen { get; set; }

        public RectangleDrawingObject()
        {
            Shape = new Rect(10, 10, 100, 100);
            Brush = new SolidColorBrush(Colors.Black);
            Pen = new Pen(Brush, 1d);
        }

        public void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Brush, Pen, Shape);
        }
    }
}
