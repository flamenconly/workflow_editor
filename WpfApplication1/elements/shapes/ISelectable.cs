using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace WpfApplication1.elements.shapes
{
    interface ISelectable
    {
        bool IsSelectable { get; set; }
        bool IsSelected { get; set; }

        Adorner SelectionAdorner { get; set; }

        bool EnableSelectionAdorner();

        bool DisableSelectionAdorner();
    }
}
