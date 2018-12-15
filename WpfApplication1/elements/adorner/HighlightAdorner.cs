using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WpfApplication1.elements.adorner
{
    internal class HighlightAdorner : Adorner
    {
        public Brush Brush { get; set; }

        public Pen Pen { get; set; }
        
        public HighlightAdorner(UIElement adornedElement) : base(adornedElement) {
            Brush = Brushes.Blue;
            Pen = new Pen(Brush, 1);

            Loaded += (sender, args) => {

                DoubleAnimation doubleAnimation = new DoubleAnimation() {
                    From = 0.0, To = 1.0,
                    Duration = new Duration(TimeSpan.FromSeconds(1))
                };

                var storyBoard = new Storyboard();
                storyBoard.Children.Add(doubleAnimation);
                Storyboard.SetTarget(storyBoard, this);
                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(OpacityProperty));

                storyBoard.Begin(this);

            };

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Rect adornedElementRect = new Rect(AdornedElement.RenderSize);
            
            drawingContext.DrawLine(Pen, adornedElementRect.TopLeft, adornedElementRect.TopRight);
            drawingContext.DrawLine(Pen, adornedElementRect.TopRight, adornedElementRect.BottomRight);
            drawingContext.DrawLine(Pen, adornedElementRect.TopLeft, adornedElementRect.BottomLeft);
            drawingContext.DrawLine(Pen, adornedElementRect.BottomLeft, adornedElementRect.BottomRight);
        }
    }
}
