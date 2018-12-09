using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplication1.elements
{
    public class ViewEdge : ViewObject
    {
        public ViewNode StartNode { get; set; }

        public ViewNode EndNode { get; set; }
        
        public ViewEdge(ViewableDataObject UserData) {
            if (UserData == null) throw new ArgumentNullException("UserData");

        }

        #region Overrides



        #endregion
    }
}
