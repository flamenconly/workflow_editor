using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplication1.elements
{
    public abstract class ViewObject : UIElement
    {
        public ViewableDataObject UserData
        {
            get; protected set;
        }

        public bool IsDraggable { get; set; } = true;

        shapes.IDrawable DrawingShape { get; set; }
    }
}
