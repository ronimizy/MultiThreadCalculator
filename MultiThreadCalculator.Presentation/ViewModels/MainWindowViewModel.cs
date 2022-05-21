using System;
using Microsoft.Extensions.DependencyInjection;
using MultiThreadCalculator.Application.Buffer;
using MultiThreadCalculator.Application.Queues;
using MultiThreadCalculator.Core.ExpressionVisitors;
using ReactiveUI;

namespace MultiThreadCalculator.Presentation.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IScreen
    {
        private readonly IServiceProvider _provider;

        public MainWindowViewModel(IServiceProvider provider)
        {
            Router = new RoutingState();
            _provider = provider;

            Router.Navigate.Execute(GetCalculatorViewModel(provider));
        }

        public RoutingState Router { get; }

        public CalculatorViewModel GetCalculatorViewModel(IServiceProvider provider)
        {
            return new CalculatorViewModel
            (
                hostScreen: this,
                provider: provider,
                calculatorBuffer: _provider.GetRequiredService<ICalculatorBuffer>(),
                consumer: _provider.GetRequiredService<IRequestQueueConsumer>(),
                publisher: _provider.GetRequiredService<IResponseQueuePublisher>(),
                expressionFormatter: _provider.GetRequiredService<IFormatterExpressionVisitor>()
            );
        }
    }
}