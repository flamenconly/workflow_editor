using System.Windows;
using WpfApplication1.elements;

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
            DataContext = ViewGraphViewModel= new ViewGraphViewModel(viewer);
        }

        public elements.ViewGraphViewModel ViewGraphViewModel
        {
            get;
            set;
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewGraphViewModel.InitMockup();
        }

        
    }
}
