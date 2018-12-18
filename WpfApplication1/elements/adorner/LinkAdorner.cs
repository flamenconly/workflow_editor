using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApplication1.elements.adorner
{ 
        internal class LinkAdorner : Adorner
        {

            protected Pen LinkDashLinePen { get; set; }
            protected Brush LinkDashLineBrush { get; set; } = Brushes.DarkGray;

            protected Brush AdornerBrush { get; set; } = Brushes.DarkBlue;
            protected Pen AdornerPen { get; set; }

            protected Brush AdornerHoverBrush { get; set; } = Brushes.LightBlue;
            protected Pen AdornerHoverPen { get; set; }

            protected bool IsHovered { get; set; }

            public LinkAdorner(UIElement adornedElement) : base(adornedElement)
            {

                Height = 2;

                AdornerPen = new Pen(AdornerBrush, 1);
                AdornerHoverPen = new Pen(AdornerHoverBrush, 1);
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);

                var adornedRect = new Rect(AdornedElement.RenderSize);
                Width = adornedRect.Width / 2;

                drawingContext.DrawRectangle(
                    IsHovered ? AdornerHoverBrush : AdornerBrush,
                    IsHovered ? AdornerHoverPen : AdornerPen,
                    new Rect(
                    new Point(((adornedRect.TopRight.X - adornedRect.TopLeft.X) / 2 - (Width / 2)), adornedRect.Bottom - Height + 5)
                    , new Size(Width, Height)));
            }

            protected override void OnMouseDown(MouseButtonEventArgs e)
            {
                base.OnMouseDown(e);

                CaptureMouse();
                DragDrop.DoDragDrop(this, new ViewEdgePreview() { StartNode = (AdornedElement as ViewNodeControl) }, DragDropEffects.Link);
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
