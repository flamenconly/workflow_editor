using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfApplication1.elements.shapes
{
    internal class ConnectorDrawingObject : IDrawable
    {
        public Func<Point> GetStartPoint { get; private set; }
        public Func<Point> GetEndPoint { get; private set; }

        private Brush brush { get; set; }
        private Pen pen { get; set; }

        public ConnectorDrawingObject(Func<Point> GetStartPoint, Func<Point> GetEndPoint) {

            if (GetStartPoint == null) throw new ArgumentNullException("GetStartPoint");
            if (GetEndPoint == null) throw new ArgumentNullException("GetEndPoint"); 

            this.GetEndPoint = GetEndPoint;
            this.GetStartPoint = GetStartPoint;

            brush = new SolidColorBrush(Colors.DarkGray);
            pen = new Pen(brush, 0.5);
        }

        public void OnRender(DrawingContext drawingContext)
        {
            var startPoint = GetStartPoint();
            var endPoint = GetEndPoint();

            drawingContext.DrawLine(pen, startPoint, endPoint);

        }
    }
}
