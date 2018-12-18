using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using WpfApplication1.elements.adorner;

namespace WpfApplication1.elements
{
    /// <summary>
    /// Interaction logic for ViewNodeControl.xaml
    /// </summary>
    public partial class ViewNodeControl : ContentControl
    {
        public ViewNodeControl()
        {
            InitializeComponent();
        }

        #region Properties

        public delegate void OnIsSelectedChanged(ViewNodeControl sender, bool IsSelected);
        public event OnIsSelectedChanged IsSelectedChanged;

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public System.Windows.Point GetOutgoingEdgePosition() {

            var y = Canvas.GetTop(this)+DesiredSize.Height;
            var x = (Canvas.GetLeft(this)) + DesiredSize.Width / 2;
            return new System.Windows.Point(x, y);
        }

        public System.Windows.Point GetIncomingEdgePosition() {

            var y = Canvas.GetTop(this);
            var x = (Canvas.GetLeft(this)) + DesiredSize.Width / 2;
            return new System.Windows.Point(x, y);

        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ViewNodeControl), new FrameworkPropertyMetadata("no title",
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,(dependencyObject,args) => {
                    dependencyObject.SetValue(TitleProperty, args.NewValue);
                }));

        public Adorner HighlightAdorner
        {
            get { return (Adorner)GetValue(HighlightAdornerProperty); }
            set { SetValue(HighlightAdornerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HiglightAdorner.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightAdornerProperty =
            DependencyProperty.Register("HighlightAdorner", typeof(Adorner), typeof(ViewNodeControl), new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (dp,args)=> {
                    dp.SetValue(HighlightAdornerProperty, args.NewValue);
                }));


        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; SetSelected(); }
        }

        public Adorner LinkAdorner
        {
            get { return (Adorner)GetValue(LinkAdornerProperty); }
            set { SetValue(LinkAdornerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LinkAdorner.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LinkAdornerProperty =
            DependencyProperty.Register("LinkAdorner", typeof(Adorner), typeof(ViewNodeControl), new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (dp,args)=> {
                    dp.SetValue(LinkAdornerProperty, args.NewValue);
                }));

        #endregion

        #region Overrides

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            if (HighlightAdorner != null) {
                AddAdornerSafely(HighlightAdorner);               
            }

        }
        
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            var adornerLayer = AdornerLayer.GetAdornerLayer(this);
            if (HighlightAdorner != null) {

                adornerLayer.Remove(HighlightAdorner);
            }
        }

        private System.Windows.Point? dragStart;
        private int clickStart;

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            dragStart = e.GetPosition(this);
            CaptureMouse();

            clickStart = e.Timestamp;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            //Was in drag mode
            dragStart = null;
            ReleaseMouseCapture();

            // Was it a click?
            if (e.Timestamp - clickStart < 200)
            {
                IsSelected = !IsSelected;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (dragStart.HasValue && e.LeftButton == MouseButtonState.Pressed && e.Handled == false)
            {
                var canvasPosition = e.GetPosition((Parent as Canvas));
                Canvas.SetLeft(this, canvasPosition.X - dragStart.Value.X);
                Canvas.SetTop(this, canvasPosition.Y - dragStart.Value.Y);

                (Parent as Canvas).InvalidateMeasure(); // Inform a possible ScrollViewer
            }
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);

            var adornerLayer = AdornerLayer.GetAdornerLayer(this);

            if (HighlightAdorner != null)
            {
                AddAdornerSafely(HighlightAdorner);
            }

            // If we are trying to link Nodes
            if (e.Data.GetDataPresent(typeof(ViewEdgePreview)))
            {
                var dataObject = e.Data.GetData(typeof(ViewEdgePreview)) as ViewEdgePreview;
                dataObject.DropPosition = e.GetPosition(Parent as FrameworkElement);
            }
        }

        protected override void OnDragLeave(DragEventArgs e)
        {
            base.OnDragLeave(e);
            var adornerLayer = AdornerLayer.GetAdornerLayer(this);
            if (HighlightAdorner != null)
            {

                adornerLayer.Remove(HighlightAdorner);
            }
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            if (Parent != null) {
                LinkAdorner = new OutgoingLinkAdorner(this);
                AddAdornerSafely(LinkAdorner);
            }
        }

        #endregion

        #region Private Methods


        private void SetSelected() {
            IsSelectedChanged?.Invoke(this, IsSelected);
        }

        private void AddAdornerSafely(Adorner adorner)
        {

            var adornerLayer = AdornerLayer.GetAdornerLayer(this);
            var alreadyExistingAdorner = adornerLayer.GetAdorners(this);

            if (alreadyExistingAdorner == null ||
                alreadyExistingAdorner.Count(c => c.Equals(adorner)) == 0)
                adornerLayer.Add(adorner);
        }

        #endregion

    }
}
