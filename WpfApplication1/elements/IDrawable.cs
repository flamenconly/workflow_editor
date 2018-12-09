using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApplication1.elements
{
    public interface IDrawable
    {
        void OnRender(DrawingContext drawingContext);
    }
}
