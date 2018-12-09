using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.elements
{
    public class ViewGraphViewModel : BindableBase
    {
        private ObservableCollection<ViewNode> _nodes = new ObservableCollection<ViewNode>();
        private ObservableCollection<ViewEdge> _edges = new ObservableCollection<ViewEdge>();

        public ObservableCollection<ViewNode> NodesList {
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
    }
}
