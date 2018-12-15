﻿using System;
using System.Collections.Generic;
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
using WpfApplication1.elements;
using WpfApplication1.elements.adorner;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewGraphViewModel= new elements.ViewGraphViewModel();
        }

        public elements.ViewGraphViewModel ViewGraphViewModel
        {
            get;
            set;
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var viewNode = new ViewNodeControl()
            { Width = 100, Height = 100, Title = "First"};
            viewNode.HighlightAdorner = new HighlightAdorner(viewNode);
            viewNode.LinkAdorner = new LinkAdorner(viewNode);
            ViewGraphViewModel.NodesList = new System.Collections.ObjectModel.ObservableCollection<UIElement>();
            ViewGraphViewModel.NodesList.Add(viewNode);

            viewNode = new ViewNodeControl() { Width = 100, Height = 100, Title = "Second"};
            viewNode.HighlightAdorner = new HighlightAdorner(viewNode);
            viewNode.LinkAdorner = new LinkAdorner(viewNode);
            ViewGraphViewModel.NodesList.Add(viewNode);
        }

        private class NodeViewDataObject : elements.ViewableDataObject
        {
            public string Title
            {
                get;

                set;
            }

            public string Tooltip
            {
                get;

                set;
            }
        }
    }
}
