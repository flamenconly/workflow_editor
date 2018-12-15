using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfApplication1.elements.adorner;

namespace WpfApplication1.elements
{
    public class ViewNode : ViewObject
    {
        /// <summary>
        /// Edges which are connecting Inputs into the node
        /// </summary>
        public List<ViewEdge> InEdges { get; private set; } = new List<ViewEdge>();

        /// <summary>
        /// Edges which are connecting this Nodes with others
        /// </summary>
        public List<ViewEdge> OutEdges { get; private set; } = new List<ViewEdge>();

        public Point IncomingConnectorLocation { get; set; } = new Point(1,1);

        public Point OutgoingConnectorLocation { get; set; } = new Point(100, 100);

        public ViewNode(ViewableDataObject UserData) {
            if (UserData == null) throw new ArgumentNullException("UserData");
            this.UserData = UserData;
            SelectionAdorner = new HighlightAdorner(this);

        }


        #region Drawing Purposes

        #endregion

        
    }
}
