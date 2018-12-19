using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfApplication1.elements.adorner;

namespace WpfApplication1.elements
{
    public class ViewNode : BindableBase
    {
        private Adorner _incomingLinkConnectorAdorner;
        private Adorner _outgoingLinkConnectorAdorner;

        public Adorner IncomingLinkConnectorAdorner { get { return _incomingLinkConnectorAdorner; } set { SetProperty(ref _incomingLinkConnectorAdorner, value); } }

        public Adorner OutgoingLinkConnectorAdorner { get { return _outgoingLinkConnectorAdorner; } set { SetProperty(ref _outgoingLinkConnectorAdorner, value); } }

        private string _imageSource;

        public string ImageSource
        {
            get { return _imageSource; }
            set { SetProperty(ref _imageSource,value); }
        }

        public ViewNodeControl UIElement { get; protected set; }

        public ViewGraph UIElementParent { get; protected set; }

        /// <summary>
        /// Edges which are connecting Inputs into the node
        /// </summary>
        public List<ViewNode> IncomingLinks { get; private set; } = new List<ViewNode>();

        /// <summary>
        /// Edges which are connecting this Nodes with others
        /// </summary>
        public List<ViewNode> OutgoingLinks { get; private set; } = new List<ViewNode>();

        public Point IncomingConnectorLocation { get; set; } = new Point(1,1);

        public Point OutgoingConnectorLocation { get; set; } = new Point(100, 100);

        public ViewNode(ViewGraph parent) {
            if (parent == null) throw new ArgumentNullException("parent");

            UIElementParent = parent;
            UIElement = new ViewNodeControl() {
                DataContext = this,
                Width = 50,
                Height = 50,
                AllowDrop = true
            };

            UIElement.HighlightAdorner = new HighlightAdorner(UIElement);
            UIElement.OutgoingLinkAdorner = new OutgoingLinkAdorner(UIElement) { };
            
            UIElement.Drop += parent.DropAction;

            ImageSource = "pack://application:,,,/WpfApplication1;component/Resources/outline_input_black_18dp.png";



        }

    }
}
