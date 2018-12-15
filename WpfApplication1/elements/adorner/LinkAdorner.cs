using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApplication1.elements.adorner
{
    internal class LinkAdorner : Adorner
    {
        public LinkAdorner(UIElement adornedElement) : base(adornedElement) { }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var brush = Brushes.Aquamarine;
            var adornedRect = new Rect(AdornedElement.RenderSize);

            drawingContext.DrawRectangle(brush, new Pen(brush, 1), new Rect(
                new Point(((adornedRect.TopRight.X - adornedRect.TopLeft.X) / 2), adornedRect.Top-10)
                ,new Size(10, 10)));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton == MouseButtonState.Pressed) {

                e.Handled = true;
            }
        }

    }
}
