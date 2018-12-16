using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApplication1.elements.adorner
{
    internal class LinkAdorner : Adorner
    {
        
        private Pen LinkDashLinePen { get; set; }
        private Brush LinkDashLineBrush { get; set; } = Brushes.DarkGray;

        private Brush AdornerBrush { get; set; } = Brushes.Aquamarine;
        private Pen AdornerPen { get; set; }

        public LinkAdorner(UIElement adornedElement) : base(adornedElement) {

            Width = 10;
            Height = 10;

            LinkDashLinePen = new Pen(LinkDashLineBrush, 1) {
                DashCap = PenLineCap.Flat,
                DashStyle = DashStyles.Dash,
                EndLineCap = PenLineCap.Triangle
            };

            AdornerPen = new Pen(AdornerBrush, 1);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var adornedRect = new Rect(AdornedElement.RenderSize);

            drawingContext.DrawRectangle(AdornerBrush, AdornerPen, 
                new Rect(
                new Point(((adornedRect.TopRight.X - adornedRect.TopLeft.X) / 2-(Width/2)), adornedRect.Top-Height)
                ,new Size(Width, Height)));

        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            CaptureMouse();
            DragDrop.DoDragDrop(this, new ViewEdgePreview() { StartNode = (AdornedElement as ViewNodeControl)}, DragDropEffects.Link);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            
            base.OnDragOver(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            ReleaseMouseCapture();
            InvalidateVisual();
        }

    }
}
