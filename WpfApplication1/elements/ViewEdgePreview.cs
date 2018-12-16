using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplication1.elements
{
    internal class ViewEdgePreview
    {
        /// <summary>
        /// The node with the outgoing connection
        /// </summary>
        public ViewNodeControl StartNode { get; set; }

        /// <summary>
        /// The node with the incoming connection
        /// </summary>
        public ViewNodeControl EndNode { get; set; }

        /// <summary>
        /// The position withing the parent control where the user tries to connect
        /// </summary>
        public Point? DropPosition { get; set; }

    }
}
