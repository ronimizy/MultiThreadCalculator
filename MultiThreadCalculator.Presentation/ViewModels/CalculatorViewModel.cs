using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia.Threading;
using DynamicData;
using MessageBox.Avalonia.BaseWindows.Base;
using MessageBox.Avalonia.Enums;
using Microsoft.Extensions.DependencyInjection;
using MultiThreadCalculator.Application.Buffer;
using MultiThreadCalculator.Application.Models;
using MultiThreadCalculator.Application.Queues;
using MultiThreadCalculator.Application.Tools;
using MultiThreadCalculator.Core.ExpressionVisitors;
using MultiThreadCalculator.Presentation.Models;
using ReactiveUI;

namespace MultiThreadCalculator.Presentation.ViewModels;

public class CalculatorViewModel : ViewModelBase, IRoutableViewModel
{
    public CalculatorViewModel(
        IScreen hostScreen,
        IServiceProvider provider,
        ICalculatorBuffer calculatorBuffer,
        IRequestQueueConsumer consumer,
        IResponseQueuePublisher publisher,
        IFormatterExpressionVisitor expressionFormatter)
    {
        HostScreen = hostScreen;
        _calculatorBuffer = calculatorBuffer;
        _consumer = consumer;
        _publisher = publisher;
        _expressionFormatter = expressionFormatter;
        ExpressionString = string.Empty;

        _consumer.ErrorOccured += ProcessError;
        _consumer.QueueUpdated += UpdateRequests;
        _publisher.QueueUpdated += UpdateResponses;

        Requests = new ObservableCollection<CalculationRequestModel>();
        Responses = new ObservableCollection<CalculationResponseModel>();
        

        OpenSettings = ReactiveCommand.CreateFromObservable(() =>
        {
            var settings = provider.GetRequiredService<ExpressionOperationExecutionDuration>();
            var configuration = provider.GetRequiredService<RequestQueueConfiguration>();
            return HostScreen.Router.Navigate.Execute(new SettingsViewModel(HostScreen, settings, configuration));
        });
    }

    public IScreen HostScreen { get; }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();

    private readonly ICalculatorBuffer _calculatorBuffer;
    private readonly IRequestQueueConsumer _consumer;
    private readonly IResponseQueuePublisher _publisher;
    private readonly IFormatterExpressionVisitor _expressionFormatter;

    public string ExpressionString { get; private set; }
    public ObservableCollection<CalculationRequestModel> Requests { get; }
    public ObservableCollection<CalculationResponseModel> Responses { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> OpenSettings { get; }

    public void EnterCharacter(char c)
    {
        try
        {
            _calculatorBuffer.ConsumeInput(c);
            UpdateExpressionString();
        }
        catch (Exception e)
        {
            ProcessError(e);
        }
    }

    public void Pop()
    {
        _calculatorBuffer.Pop();
        UpdateExpressionString();
    }

    public void Clear()
    {
        _calculatorBuffer.Clean();
        UpdateExpressionString();
    }

    public void Evaluate()
    {
        try
        {
            var expression = _calculatorBuffer.Build();
            var request = new CalculationRequest(expression);

            _consumer.Consume(request);
            _calculatorBuffer.Clean();
            UpdateExpressionString();
        }
        catch (Exception e)
        {
            ProcessError(e);
        }
    }

    private void UpdateExpressionString()
    {
        ExpressionString = _calculatorBuffer.Format();
        this.RaisePropertyChanged(nameof(ExpressionString));
    }

    private void UpdateRequests()
    {
        Dispatcher.UIThread.Post(() =>
        {
            Requests.Clear();
            Requests.AddRange(_consumer.Reverse().Select(ToModel));
            this.RaisePropertyChanged(nameof(Requests));
        });
    }

    private void UpdateResponses()
    {
        Dispatcher.UIThread.Post(() =>
        {
            Responses.Clear();
            Responses.AddRange(_publisher.Reverse().Select(ToModel));
            this.RaisePropertyChanged(nameof(Responses));
        });
    }
    private void ProcessError(Exception exception)
    {
        Dispatcher.UIThread.Post(() =>
        {
            IMsBoxWindow<ButtonResult> message = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Error occured", exception.Message);
        
            message.Show();
        });
    }

    private CalculationRequestModel ToModel(CalculationRequest request)
    {
        _expressionFormatter.Clear();
        var expressionString = request.Expression.Accept(_expressionFormatter).ToString();

        return new CalculationRequestModel(expressionString, request.State is CalculationRequestState.Executing);
    }

    private CalculationResponseModel ToModel(CalculationResponse response)
    {
        _expressionFormatter.Clear();
        var expressionString = response.Expression.Accept(_expressionFormatter).ToString();
        var durationString = response.ExecutionDuration.ToString("c");
        
        return new CalculationResponseModel(response.Result.ToString("N"), expressionString, durationString);
    }
}