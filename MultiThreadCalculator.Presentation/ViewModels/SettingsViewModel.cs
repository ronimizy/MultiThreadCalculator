using System;
using System.Reactive;
using System.Threading;
using MultiThreadCalculator.Application.Tools;
using ReactiveUI;

namespace MultiThreadCalculator.Presentation.ViewModels;

public class SettingsViewModel : ViewModelBase, IRoutableViewModel
{
    private readonly ExpressionOperationExecutionDuration _duration;
    private readonly RequestQueueConfiguration _requestQueueConfiguration;

    public SettingsViewModel(
        IScreen hostScreen,
        ExpressionOperationExecutionDuration duration,
        RequestQueueConfiguration requestQueueConfiguration)
    {
        _duration = duration;
        HostScreen = hostScreen;
        _requestQueueConfiguration = requestQueueConfiguration;
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

    public double RequestQueuePollingDelay
    {
        get => _requestQueueConfiguration.QueuePollingDelay.TotalMilliseconds;
        set => _requestQueueConfiguration.QueuePollingDelay = TimeSpan.FromMilliseconds(value);
    }
}