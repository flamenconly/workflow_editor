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
using WpfApplication1.elements.shapes;
using WpfApplication1.elements.adorner;
using System.Windows.Documents;

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

            // Mousedown Implementation
            mouseDown = (sender, args) => {
                var element = sender as UIElement;
                if (element == null) return;

                dragStart = args.GetPosition(element);
                element.CaptureMouse();
            };

            // Mouseup Implementation
            mouseUp = (sender, args) =>
            {
                var element = sender as UIElement;
                if (element == null) return;

                if (dragStart.HasValue)
                {
                    //Was in drag mode
                    dragStart = null;
                    element.ReleaseMouseCapture();
                }

            };

            // Mousemove Implementation
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
                var node = item as UIElement;
                Children.Remove(node);
            }
        }

        private void OnNewElementsAdded(IList newItems)
        {
            foreach (var item in newItems)
            {
                var node = item as UIElement;
                Children.Add(node);
            }
        }
        
        #region Dependency Properties
        static System.Collections.Specialized.NotifyCollectionChangedEventHandler ViewObjectCollectionChangedAction;
        
        private static void deregisterNodes(ObservableCollection<UIElement> Nodes)
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

        private static void registerNodes(ObservableCollection<UIElement> Nodes)
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
        public static readonly DependencyProperty EdgesProperty =
            DependencyProperty.Register("Edges", typeof(ObservableCollection<ViewEdge>), typeof(ViewGraph),
                new FrameworkPropertyMetadata(new ObservableCollection<ViewEdge>(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnEdgesPropertyChangedCallback));

        private static void OnEdgesPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs baseValue)
        {
            d.SetValue(EdgesProperty, baseValue.NewValue);
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
                typeof(ObservableCollection<UIElement>), 
                typeof(ViewGraph), 
                new FrameworkPropertyMetadata(new ObservableCollection<UIElement>(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnNodesPropertyChangedCallback));

        private static void OnNodesPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs baseValue)
        {
            
            d.SetValue(NodesProperty,baseValue.NewValue);

            if (baseValue.OldValue != null) {
                deregisterNodes(baseValue.OldValue as ObservableCollection<UIElement>);
            }

            if (baseValue.NewValue != null) {
                registerNodes(baseValue.NewValue as ObservableCollection<UIElement>);
            }

        }

        public ObservableCollection<UIElement> Nodes
        {
            get { return (ObservableCollection<UIElement>)GetValue(NodesProperty); }
            set
            {
                SetValue(NodesProperty, value);
            }
        }

        public ObservableCollection<ViewEdge> Edges
        {
            get { return (ObservableCollection<ViewEdge>)GetValue(EdgesProperty); }
            set {
                SetValue(EdgesProperty, value);
            }
        }

        

        #endregion
    }
}
