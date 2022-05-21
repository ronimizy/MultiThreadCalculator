using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using MultiThreadCalculator.Presentation.ViewModels;

namespace MultiThreadCalculator.Presentation.Views;

public partial class CalculatorView : ReactiveUserControl<CalculatorViewModel>
{
    public CalculatorView()
    {
        InitializeComponent();
    }

    private void Clear_OnClick(object? sender, RoutedEventArgs e)
        => ViewModel?.Clear();

    private void Pop_OnClick(object? sender, RoutedEventArgs e)
        => ViewModel?.Pop();

    private void Evaluate_OnClick(object? sender, RoutedEventArgs e)
        => ViewModel?.Evaluate();

    private void Character_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button { Tag: string tag })
            return;

        ViewModel?.EnterCharacter(tag.Single());
        Focus();
    }

    protected override void OnInitialized()
        => Focus();

    protected override void OnTextInput(TextInputEventArgs e)
    {
        if (e.Text is not { Length: 1 })
            return;

        var c = e.Text[0];

        if (char.IsDigit(c))
            ViewModel?.EnterCharacter(c);

        if (c is '.' or '+' or '-' or '*' or '/')
            ViewModel?.EnterCharacter(c);
        
        if (c is '=')
            ViewModel?.Evaluate();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key is Key.Delete or Key.Back)
            ViewModel?.Pop();

        if (e.Key is Key.Escape)
            ViewModel?.Clear();

        if (e.Key is Key.Enter)
            ViewModel?.Evaluate();
    }
}