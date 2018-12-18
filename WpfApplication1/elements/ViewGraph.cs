using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        public DragEventHandler DropAction { get; set; } = (object sender, DragEventArgs args) =>
        {
            if (args.Data.GetDataPresent(typeof(ViewEdgePreview)))
            {
                var viewEdgePreview = args.Data.GetData(typeof(ViewEdgePreview)) as ViewEdgePreview;

                if (viewEdgePreview.StartNode == null) return;
                if (viewEdgePreview.DropPosition.HasValue == false) return;

                var canvas = (viewEdgePreview.StartNode.Parent as Canvas);

                var frameworkHelper = FrameworkHelper.GetInstance();

                foreach (FrameworkElement child in canvas.Children)
                {
                    var boundingBox = frameworkHelper.DetermineBoundingBoxOfUIItemWithinCanvas(child);

                    if (frameworkHelper.IsPositionWithinBoundingBox(viewEdgePreview.DropPosition.Value, boundingBox)) {
                        // Found our child
                        break;
                    }

                }
            }
        };

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

        protected override Size MeasureOverride(Size constraint)
        {
            base.MeasureOverride(constraint);
            var desiredSize = new Size(100d,100d);

            foreach (UIElement child in Children)
            {
                var left = GetLeft(child);
                var top = GetTop(child);

                if (double.IsNaN(left)) left = 1;
                if (double.IsNaN(top)) top = 1;

                desiredSize = new Size(
                    Math.Max(desiredSize.Width, left + child.DesiredSize.Width),
                    Math.Max(desiredSize.Height, top + child.DesiredSize.Height));
            }
            return desiredSize;
        }

        private void OnClearNodes()
        {
            Children.Clear();
        }

        private void OnOldElementsRemove(IList oldItems)
        {
            foreach (var item in oldItems)
            {
                var node = item as ViewNode;
                Children.Remove(node.UIElement);
            }
        }

        private void OnNewElementsAdded(IList newItems)
        {
            foreach (var item in newItems)
            {
                var node = item as ViewNode;
                Children.Add(node.UIElement);
            }
        }
        
        #region Dependency Properties
        static System.Collections.Specialized.NotifyCollectionChangedEventHandler ViewObjectCollectionChangedAction;
        
        private static void DeregisterNodes(ObservableCollection<ViewNode> Nodes)
        {
            if (Nodes != null) {
                Nodes.CollectionChanged -= ViewObjectCollectionChangedAction;
            }
        }

        private static void RegisterNodes(ObservableCollection<ViewNode> Nodes)
        {
            if (Nodes != null) {
                Nodes.CollectionChanged += ViewObjectCollectionChangedAction;
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
                DeregisterNodes(baseValue.OldValue as ObservableCollection<ViewNode>);
            }

            if (baseValue.NewValue != null) {
                RegisterNodes(baseValue.NewValue as ObservableCollection<ViewNode>);
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

        #endregion
    }
}
