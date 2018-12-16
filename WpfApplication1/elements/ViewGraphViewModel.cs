using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApplication1.elements.adorner;
using WpfApplication1.elements.shapes;

namespace WpfApplication1.elements
{
    public class ViewGraphViewModel : BindableBase
    {
        private ObservableCollection<UIElement> _nodes = new ObservableCollection<UIElement>();
        private ObservableCollection<ViewEdge> _edges = new ObservableCollection<ViewEdge>();

        public ObservableCollection<UIElement> NodesList {
            get { return _nodes; }
            set { SetProperty(ref _nodes, value); }
        }
        
        public ObservableCollection<ViewEdge> EdgesList
        {
            get {return _edges;}
            set { SetProperty(ref _edges, value); }
            
        }

        public ViewNode AddNode(ViewNode viewNode)
        {
            NodesList.Add(viewNode);
            return viewNode;
        }

        public ViewEdge AddEdge(ViewEdge viewEdge)
        {
            EdgesList.Add(viewEdge);
            return viewEdge;
        }

        private void LinkAction(object sender, DragEventArgs args)
        {
            if (args.Data.GetDataPresent(typeof(ViewEdgePreview)))
            {
                var viewEdgePreview = args.Data.GetData(typeof(ViewEdgePreview)) as ViewEdgePreview;

                if (viewEdgePreview.StartNode == null) return;
                if (viewEdgePreview.DropPosition.HasValue == false) return;

                var canvas = (viewEdgePreview.StartNode.Parent as Canvas);

                foreach (FrameworkElement child in canvas.Children)
                {

                    var top = Canvas.GetTop(child);
                    var left = Canvas.GetLeft(child);
                    var bottom = top + child.DesiredSize.Height;
                    var right = left + child.DesiredSize.Width;


                    var boundingBox = new Rect(
                        new Point(left,top),new Size(child.DesiredSize.Width,child.DesiredSize.Height)
                        );

                    if (boundingBox.Contains(viewEdgePreview.DropPosition.Value))
                    {

                        var newEdge = new ViewEdge();
                        newEdge.Connect(viewEdgePreview.StartNode, (child as ViewNodeControl));
                        EdgesList.Add(newEdge);
                        return;
                    }
                }
            }
        }

        public void InitMockup() {

            var viewNode = new ViewNodeControl()
            { Width = 100, Height = 50, Title = "First",AllowDrop=true };

            viewNode.HighlightAdorner = new HighlightAdorner(viewNode);
            viewNode.LinkAdorner = new LinkAdorner(viewNode);
            viewNode.Drop += LinkAction;

            viewNode.IsSelectedChanged += (node, isSelected) => {

                if (isSelected)
                {
                    NodesList.ToList().ForEach(n => { if (n.Equals(node) == false) { (n as ISelectable).IsSelected = false; } });
                }

            };

            NodesList = new ObservableCollection<UIElement>();
            NodesList.Add(viewNode);

            viewNode = new ViewNodeControl() { Width = 100, Height = 50, Title = "Second",AllowDrop=true };
            viewNode.HighlightAdorner = new HighlightAdorner(viewNode);
            viewNode.LinkAdorner = new LinkAdorner(viewNode);
            viewNode.Drop += LinkAction;

            viewNode.IsSelectedChanged += (node, isSelected) => {

                if (isSelected)
                {
                    NodesList.ToList().ForEach(n => { if (n.Equals(node) == false) { (n as ISelectable).IsSelected = false; } });
                }

            };
            NodesList.Add(viewNode);
        }
    }
}
