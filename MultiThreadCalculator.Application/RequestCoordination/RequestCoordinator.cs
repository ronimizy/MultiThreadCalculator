using Microsoft.Extensions.DependencyInjection;
using MultiThreadCalculator.Application.Models;
using MultiThreadCalculator.Application.Queues;
using MultiThreadCalculator.Core.Exceptions;
using MultiThreadCalculator.Core.ExpressionVisitors;

namespace MultiThreadCalculator.Application.RequestCoordination;

public class RequestCoordinator : IRequestCoordinator
{
    private readonly IServiceProvider _provider;
    private readonly IRequestQueuePublisher _publisher;
    private readonly IResponseQueueConsumer _consumer;

    public RequestCoordinator(
        IRequestQueuePublisher publisher,
        IResponseQueueConsumer consumer,
        IServiceProvider provider)
    {
        _publisher = publisher;
        _consumer = consumer;
        _provider = provider;
    }

    public async Task Start(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var request = await _publisher.GetRequestAsync(cancellationToken);

            ThreadPool.QueueUserWorkItem(state: request, callBack: state =>
            {
                if (state is not CalculationRequest r)
                    return;

                try
                {
                    _publisher.StartExecution(r);
                    var expressionEvaluator = _provider.GetRequiredService<IExpressionVisitor<TimeSpan>>();
                    var executionDuration = r.Expression.Accept(expressionEvaluator);
                    Thread.Sleep(executionDuration);

                    var result = r.Expression.Evaluate();
                    var response = new CalculationResponse(result, r.Expression, executionDuration);

                    _consumer.Consume(response);
                }
                catch (MultiThreadCalculatorException e)
                {
                    _publisher.Report(e);
                }
                catch (ArithmeticException e)
                {
                    _publisher.Report(e);
                }
                finally
                {
                    _publisher.Report(request);
                }
            });
        }
    }
}