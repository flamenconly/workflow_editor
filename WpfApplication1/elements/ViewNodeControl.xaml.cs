﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApplication1.elements.adorner;

namespace WpfApplication1.elements
{
    /// <summary>
    /// Interaction logic for ViewNodeControl.xaml
    /// </summary>
    public partial class ViewNodeControl : UserControl
    {
        public ViewNodeControl()
        {
            InitializeComponent();
        }

        public delegate void OnIsSelectedChanged(ViewNodeControl sender, bool IsSelected);
        public static event OnIsSelectedChanged IsSelectedChanged;

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ViewNodeControl), new FrameworkPropertyMetadata("no title",
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,(dependencyObject,args) => {
                    dependencyObject.SetValue(TitleProperty, args.NewValue);
                }));

        public Icon Icon
        {
            get { return (Icon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Icon), typeof(ViewNodeControl), new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (dp,args) => {
                    dp.SetValue(IconProperty, args.NewValue);
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

        #region Overrides
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            var adornerLayer = AdornerLayer.GetAdornerLayer(this);

            if (HighlightAdorner != null ) {
                adornerLayer.Add(HighlightAdorner);
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
                SetSelected();
            }
        }

        private void SetSelected() {
            var adornerLayer = AdornerLayer.GetAdornerLayer(this);

            if (this.IsSelected)
            {
                if (LinkAdorner != null)
                {
                    adornerLayer.Add(LinkAdorner);
                }
            }
            else {
                if (LinkAdorner != null)
                {
                    adornerLayer.Remove(LinkAdorner);
                }
            }

            IsSelectedChanged?.Invoke(this, this.IsSelected);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (dragStart.HasValue && e.LeftButton == MouseButtonState.Pressed && e.Handled == false)
            {
                var canvasPosition = e.GetPosition((Parent as Canvas));
                Canvas.SetLeft(this, canvasPosition.X - dragStart.Value.X);
                Canvas.SetTop(this, canvasPosition.Y - dragStart.Value.Y);
            }
        }

        #endregion

    }
}
