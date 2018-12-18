using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;


namespace WpfApplication1.elements
{
    public class ViewGraphViewModel : BindableBase
    {
        private readonly ViewGraph UIElement;

        private ObservableCollection<ViewNode> _nodes = new ObservableCollection<ViewNode>();

        public ObservableCollection<ViewNode> NodesList {
            get { return _nodes; }
            set { SetProperty(ref _nodes, value); }
        }

        public ViewGraphViewModel(ViewGraph element) {
            UIElement = element;
        }

        public ViewNode AddNode(ViewNode viewNode)
        {
            NodesList.Add(viewNode);
            return viewNode;
        }

        public void InitMockup() {
            NodesList.Add(new ViewNode(UIElement));
            NodesList.Add(new ViewNode(UIElement));
        }
    }
}
