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

            protected Brush AdornerBrush { get; set; } = Brushes.CornflowerBlue;
            protected Pen AdornerPen { get; set; }

            protected Brush AdornerHoverBrush { get; set; } = Brushes.LightBlue;
            protected Pen AdornerHoverPen { get; set; }

            public LinkAdorner(UIElement adornedElement) : base(adornedElement)
            {

                Height = 2;

                AdornerPen = new Pen(AdornerBrush, 1);
                AdornerHoverPen = new Pen(AdornerHoverBrush, 1);
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
