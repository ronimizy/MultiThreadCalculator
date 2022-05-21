using Avalonia.ReactiveUI;
using MultiThreadCalculator.Presentation.ViewModels;

namespace MultiThreadCalculator.Presentation.Views;

public partial class SettingsView : ReactiveUserControl<SettingsViewModel>
{
    public SettingsView()
    {
        InitializeComponent();
    }
}