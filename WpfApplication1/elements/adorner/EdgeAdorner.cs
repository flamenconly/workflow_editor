using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfApplication1.elements.adorner
{
    internal class EdgeAdorner : Adorner
    {
        public ViewNode Target { get; set; }

        public ViewNode Source { get; set; }

        public EdgeAdorner(UIElement adornedElement) : base(adornedElement) {

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }
    }
}
