﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApplication1.elements.adorner
{
    internal class OutgoingLinkAdorner : LinkAdorner
    {

        private bool _isHovered;

        protected bool IsHovered { get { return _isHovered; } set { _isHovered = value; } }

        public OutgoingLinkAdorner(UIElement adornedElement) : base(adornedElement) {

            Height = 3;

            AdornerPen = new Pen(AdornerBrush, 3);
            AdornerHoverPen = new Pen(AdornerHoverBrush, 3);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var adornedRect = new Rect(AdornedElement.RenderSize);
            Width = adornedRect.Width/4;

            var startPoint = new Point(adornedRect.TopLeft.X+Width, adornedRect.Bottom + 5);
            var endPoint = new Point(adornedRect.TopRight.X-Width, startPoint.Y);

            var ellipse = new ArcSegment()
            {
                Point = endPoint,
                Size = new Size(adornedRect.Width*0.5, adornedRect.Width*.25),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = false,
                IsSmoothJoin = true
            };

            var pathFigure = new PathFigure(startPoint,new List<PathSegment>(){ellipse}, false){ IsFilled = false};

            var geometry = new PathGeometry(
                new PathFigureCollection() {
                    pathFigure
                }
            );
            
            drawingContext.DrawGeometry(IsHovered ? AdornerHoverBrush : AdornerBrush,
                IsHovered ? AdornerHoverPen : AdornerPen, geometry);

        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            CaptureMouse();
            DragDrop.DoDragDrop(this, new ViewEdgePreview() { StartNode = (AdornedElement as ViewNodeControl) }, DragDropEffects.Link);
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
