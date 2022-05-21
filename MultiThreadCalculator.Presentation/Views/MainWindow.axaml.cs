using Avalonia.ReactiveUI;
using MultiThreadCalculator.Presentation.ViewModels;

namespace MultiThreadCalculator.Presentation.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}