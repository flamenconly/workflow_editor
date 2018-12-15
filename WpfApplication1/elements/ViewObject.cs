using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using WpfApplication1.elements.shapes;

namespace WpfApplication1.elements
{
    public abstract class ViewObject : UIElement,ISelectable
    {
        public ViewableDataObject UserData
        {
            get; protected set;
        }

        public bool IsDraggable { get; set; } = true;

        public bool IsSelectable { get; set; }

        public bool IsSelected { get; set; }

        public Adorner SelectionAdorner { get; set; }

        shapes.IDrawable DrawingShape { get; set; }

        public bool DisableSelectionAdorner()
        {
            if (SelectionAdorner == null) return false;

            AdornerLayer.GetAdornerLayer(this).Remove(SelectionAdorner);
            return true;
        }

        public bool EnableSelectionAdorner()
        {
            if (SelectionAdorner == null) return false;

            AdornerLayer.GetAdornerLayer(this).Add(SelectionAdorner);
            return true;

        }
    }
}
