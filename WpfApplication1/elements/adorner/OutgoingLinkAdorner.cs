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
    internal class OutgoingLinkAdorner : LinkAdorner
    {

        private bool _isHovered;

        protected bool IsHovered { get { return _isHovered; } set { _isHovered = value; } }

        public OutgoingLinkAdorner(UIElement adornedElement) : base(adornedElement) {

            Height = 3;

            AdornerPen = new Pen(AdornerBrush, 1);
            AdornerHoverPen = new Pen(AdornerHoverBrush, 1);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var adornedRect = new Rect(AdornedElement.RenderSize);
            Width = adornedRect.Width/2;

            var startPoint = new Point(adornedRect.TopLeft.X, adornedRect.Bottom - Height + 5);
            var midPoint = new Point(((adornedRect.TopRight.X - adornedRect.TopLeft.X) / 2 - (Width / 2)), startPoint.Y + 10);
            var endPoint = new Point(adornedRect.TopRight.X, startPoint.Y);

            var geometry = new PathGeometry(new PathFigureCollection() {
                new PathFigure(startPoint,
                new List<PathSegment>(){ new ArcSegment() {
                    Point = new Point(10,100),
                    SweepDirection = SweepDirection.Counterclockwise,
                    Size = new Size(adornedRect.Size.Width,10),
                    RotationAngle = 45,
                    IsSmoothJoin =true } }
                ,false)
            }
            );

            drawingContext.DrawGeometry(IsHovered ? AdornerHoverBrush : AdornerBrush,
                IsHovered ? AdornerHoverPen : AdornerPen, geometry);

        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            IsHovered = true;
            InvalidateVisual();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            IsHovered = false;
            InvalidateVisual();
        }

    }
}
