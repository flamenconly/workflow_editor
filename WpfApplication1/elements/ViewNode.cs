using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfApplication1.elements
{
    public class ViewNode : ViewObject
    {
        public List<ViewEdge> InEdges { get; private set; } = new List<ViewEdge>();

        public List<ViewEdge> OutEdges { get; private set; } = new List<ViewEdge>();

        public IDrawable DrawingShape { get; set; } = new RectangleDrawingObject();

        public ViewNode(ViewableDataObject UserData) {
            if (UserData == null) throw new ArgumentNullException("UserData");
            this.UserData = UserData;
        }

        #region Overrides

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            DrawingShape?.OnRender(drawingContext);
        }

        #endregion

        #region Drawing Purposes

        #endregion

        private class RectangleDrawingObject : IDrawable
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
}
