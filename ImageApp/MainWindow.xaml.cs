using ImageApp.ViewModels;
using System.Windows;

namespace ImageApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}