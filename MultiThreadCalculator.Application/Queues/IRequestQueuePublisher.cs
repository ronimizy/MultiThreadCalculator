using MultiThreadCalculator.Application.Models;

namespace MultiThreadCalculator.Application.Queues;

public interface IRequestQueuePublisher
{
    Task<CalculationRequest> GetRequestAsync(CancellationToken cancellationToken);
    void StartExecution(CalculationRequest request);
    void Report(CalculationRequest request);
    void Report(Exception exception);
}