using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfApplication1.elements.shapes;

namespace WpfApplication1.elements
{
    public class ViewEdge : ViewObject
    {
        public ViewNodeControl StartNode { get; private set; }

        public ViewNodeControl EndNode { get; private set; }

        public Point? DropPosition { get; set; }

        public void Connect(ViewNodeControl startNode, ViewNodeControl endNode) {
            if (startNode == null) throw new ArgumentNullException("StartNode");
            if (endNode == null) throw new ArgumentException("EndNode");
            StartNode = startNode;
            EndNode = endNode;
            Drawable = new ConnectorDrawingObject(StartNode.GetOutgoingEdgePosition, EndNode.GetIncomingEdgePosition);
        }

        public shapes.IDrawable Drawable { get; set; }

        #region Overrides

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Drawable?.OnRender(drawingContext);
        }


        #endregion
    }
}
