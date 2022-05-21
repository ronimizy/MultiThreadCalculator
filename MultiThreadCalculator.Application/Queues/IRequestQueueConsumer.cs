using MultiThreadCalculator.Application.Models;

namespace MultiThreadCalculator.Application.Queues;

public interface IRequestQueueConsumer : IEnumerable<CalculationRequest>
{
    event Action QueueUpdated;
    event Action<Exception> ErrorOccured; 
    void Consume(CalculationRequest request);
}