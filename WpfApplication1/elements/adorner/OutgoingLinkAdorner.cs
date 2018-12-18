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
        
        public OutgoingLinkAdorner(UIElement adornedElement) : base(adornedElement) {

            Height = 2;

            AdornerPen = new Pen(AdornerBrush, 1);
            AdornerHoverPen = new Pen(AdornerHoverBrush, 1);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var adornedRect = new Rect(AdornedElement.RenderSize);
            Width = adornedRect.Width/2;
            
            drawingContext.DrawRectangle(
                IsHovered? AdornerHoverBrush: AdornerBrush, 
                IsHovered? AdornerHoverPen : AdornerPen, 
                new Rect(
                new Point(((adornedRect.TopRight.X - adornedRect.TopLeft.X) / 2-(Width/2)), adornedRect.Bottom-Height+5)
                ,new Size(Width, Height)));
        }

    }
}
