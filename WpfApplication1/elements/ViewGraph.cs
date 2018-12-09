using Microsoft.Msagl.Drawing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections;

namespace WpfApplication1.elements
{
    public class ViewGraph : Canvas
    {
        private MouseButtonEventHandler mouseDown;
        private MouseButtonEventHandler mouseUp;

        private MouseEventHandler mouseMove;
        private Action<UIElement> enableDrag;
        private Action<UIElement> disableDrag;

        private Point? dragStart;

        static ViewGraph()
        {
            DefaultStyleKeyProperty.
                OverrideMetadata(typeof(ViewGraph),
                new FrameworkPropertyMetadata(typeof(ViewGraph))
                );
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            ViewObjectCollectionChangedAction = (sender, args) => {
                switch (args.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        OnNewElementsAdded(args.NewItems);
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        OnOldElementsRemove(args.OldItems);
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                        OnOldElementsRemove(args.OldItems);
                        OnNewElementsAdded(args.NewItems);
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                        OnClearNodes();
                        break;
                }
            };

            mouseDown = (sender, args) => {
                var element = sender as ViewObject;
                if (element == null) return;
                if (element.IsDraggable == false) return;
                
                dragStart = args.GetPosition(element);
                element.CaptureMouse();
            };

            mouseUp = (sender, args) =>
            {
                var element = sender as UIElement;
                if (element == null) return;

                dragStart = null;
                element.ReleaseMouseCapture();
            };

            mouseMove = (sender, args) =>
            {
                if (dragStart.HasValue && args.LeftButton == MouseButtonState.Pressed)
                {
                    var element = sender as UIElement;
                    if (element == null) return;

                    var canvasPosition = args.GetPosition(this);
                    SetLeft(element, canvasPosition.X - dragStart.Value.X);
                    SetTop(element, canvasPosition.Y - dragStart.Value.Y);

                }
            };

            enableDrag = (element) => {
                element.MouseDown += mouseDown;
                element.MouseUp += mouseUp;
                element.MouseMove += mouseMove;
            };

            disableDrag = (element) => {
                element.MouseDown -= mouseDown;
                element.MouseUp -= mouseUp;
                element.MouseMove -= mouseMove;
            };

        }

        private void OnClearNodes()
        {
            Children.Clear();
        }

        private void OnOldElementsRemove(IList oldItems)
        {
            foreach (var item in oldItems)
            {
                var node = item as ViewObject;
                Children.Remove(node);
                enableDrag(node);
            }
        }

        private void OnNewElementsAdded(IList newItems)
        {
            foreach (var item in newItems)
            {
                var node = item as ViewObject;
                Children.Add(node);
                disableDrag(node);
            }
        }
        
        #region Dependency Properties
        static System.Collections.Specialized.NotifyCollectionChangedEventHandler ViewObjectCollectionChangedAction;
        
        private static void deregisterNodes(ObservableCollection<ViewNode> Nodes)
        {
            if (Nodes != null) {
                Nodes.CollectionChanged -= ViewObjectCollectionChangedAction;
            }
        }

        private static void deregisterEdges(ObservableCollection<ViewEdge> Edges)
        {
            if (Edges != null)
            {
                Edges.CollectionChanged -= ViewObjectCollectionChangedAction;
            }
        }

        private static void registerNodes(ObservableCollection<ViewNode> Nodes)
        {
            if (Nodes != null) {
                Nodes.CollectionChanged += ViewObjectCollectionChangedAction;
            }
        }

        private static void registerEdges(ObservableCollection<ViewEdge> Edges)
        {
            if (Edges != null)
            {
                Edges.CollectionChanged += ViewObjectCollectionChangedAction;
            }
        }

        // Using a DependencyProperty as the backing store for Edge.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EdgeProperty =
            DependencyProperty.Register("Edges", typeof(ObservableCollection<ViewEdge>), typeof(ViewGraph),
                new FrameworkPropertyMetadata(new ObservableCollection<ViewEdge>(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnEdgesPropertyChangedCallback));

        private static void OnEdgesPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs baseValue)
        {
            d.SetValue(EdgeProperty, baseValue.NewValue);
            if (baseValue.OldValue != null)
            {
                deregisterEdges(baseValue.OldValue as ObservableCollection<ViewEdge>);
            }

            if (baseValue.NewValue != null)
            {
                registerEdges(baseValue.NewValue as ObservableCollection<ViewEdge>);
            }

        }

        // Using a DependencyProperty as the backing store for Nodes.  This enables animation, styling, binding, etc...

        public static readonly DependencyProperty NodesProperty =
            DependencyProperty.Register("Nodes", 
                typeof(ObservableCollection<ViewNode>), 
                typeof(ViewGraph), 
                new FrameworkPropertyMetadata(new ObservableCollection<ViewNode>(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnNodesPropertyChangedCallback));

        private static void OnNodesPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs baseValue)
        {
            
            d.SetValue(NodesProperty,baseValue.NewValue);

            if (baseValue.OldValue != null) {
                deregisterNodes(baseValue.OldValue as ObservableCollection<ViewNode>);
            }

            if (baseValue.NewValue != null) {
                registerNodes(baseValue.NewValue as ObservableCollection<ViewNode>);
            }

        }

        public ObservableCollection<ViewNode> Nodes
        {
            get { return (ObservableCollection<ViewNode>)GetValue(NodesProperty); }
            set
            {
                SetValue(NodesProperty, value);
            }
        }

        public ObservableCollection<ViewEdge> Edges
        {
            get { return (ObservableCollection<ViewEdge>)GetValue(EdgeProperty); }
            set {
                SetValue(EdgeProperty, value);
            }
        }

        

        #endregion
    }
}
