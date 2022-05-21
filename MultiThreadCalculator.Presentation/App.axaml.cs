using System;
using System.Threading;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MultiThreadCalculator.Application.Buffer;
using MultiThreadCalculator.Application.ExpressionVisitors;
using MultiThreadCalculator.Application.Queues;
using MultiThreadCalculator.Application.RequestCoordination;
using MultiThreadCalculator.Application.Tools;
using MultiThreadCalculator.Core.ExpressionBuilderVisitors;
using MultiThreadCalculator.Core.Expressions;
using MultiThreadCalculator.Core.ExpressionVisitors;
using MultiThreadCalculator.Core.Factories;
using MultiThreadCalculator.Presentation.ViewModels;
using MultiThreadCalculator.Presentation.Views;
using ReactiveUI;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace MultiThreadCalculator.Presentation
{
    public partial class App : Avalonia.Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            Name = "Calculator";

            var collection = new ServiceCollection();
            collection.UseMicrosoftDependencyResolver();

            collection.AddTransient<ICalculatorBuffer, CalculatorBuffer>();
            collection.AddTransient<IExpressionBuilderVisitor<IExpression>, BuildingExpressionBuilderVisitor>();
            collection.AddTransient<IFormatterExpressionBuilderVisitor, FormatterExpressionBuilderVisitor>();
            collection.AddSingleton<IExpressionBuilderFactory, ExpressionBuilderFactory>();
            collection.AddSingleton<IFormatterExpressionVisitor, FormatterExpressionVisitor>();
            collection.AddSingleton<IExpressionVisitor<TimeSpan>, DurationExpressionVisitor>();

            collection.AddSingleton(new ExpressionOperationExecutionDuration
            {
                UnaryMinus = 0.25,
                Summation = 1,
                Subtraction = 1,
                Multiplication = 1,
                Division = 1,
            });

            collection.AddSingleton<IRequestCoordinator, RequestCoordinator>();

            collection.AddTransient<IViewFor<CalculatorViewModel>, CalculatorView>();
            collection.AddTransient<IViewFor<SettingsViewModel>, SettingsView>();

            var configuration = new RequestQueueConfiguration(TimeSpan.FromMilliseconds(500));
            var requestQueue = new RequestQueue(configuration);
            collection.AddSingleton<IRequestQueueConsumer>(requestQueue);
            collection.AddSingleton<IRequestQueuePublisher>(requestQueue);

            var responseQueue = new ResponseQueue();
            collection.AddSingleton<IResponseQueueConsumer>(responseQueue);
            collection.AddSingleton<IResponseQueuePublisher>(responseQueue);

            collection.AddTransient<MainWindowViewModel>();

            _serviceProvider = collection.BuildServiceProvider();
            _serviceProvider.UseMicrosoftDependencyResolver();

            var coordinator = _serviceProvider.GetRequiredService<IRequestCoordinator>();
            coordinator.Start(CancellationToken.None);
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = _serviceProvider.GetRequiredService<MainWindowViewModel>(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}