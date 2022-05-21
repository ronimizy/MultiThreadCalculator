using System;
using System.Reactive;
using System.Threading;
using MultiThreadCalculator.Application.Tools;
using ReactiveUI;

namespace MultiThreadCalculator.Presentation.ViewModels;

public class SettingsViewModel : ViewModelBase, IRoutableViewModel
{
    private readonly ExpressionOperationExecutionDuration _duration;

    public SettingsViewModel(ExpressionOperationExecutionDuration duration, IScreen hostScreen)
    {
        _duration = duration;
        HostScreen = hostScreen;
    }

    public IScreen HostScreen { get; }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();

    public ReactiveCommand<Unit, Unit> CloseSettings => HostScreen.Router.NavigateBack;

    public double Literal
    {
        get => _duration.Literal;
        set => _duration.Literal = value;
    }

    public double UnaryMinus
    {
        get => _duration.UnaryMinus;
        set => _duration.UnaryMinus = value;
    }

    public double Summation
    {
        get => _duration.Summation;
        set => _duration.Summation = value;
    }

    public double Subtraction
    {
        get => _duration.Subtraction;
        set => _duration.Subtraction = value;
    }

    public double Multiplication
    {
        get => _duration.Multiplication;
        set => _duration.Multiplication = value;
    }

    public double Division
    {
        get => _duration.Division;
        set => _duration.Division = value;
    }
}