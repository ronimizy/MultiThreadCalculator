using MultiThreadCalculator.Application.Models;

namespace MultiThreadCalculator.Application.Queues;

public interface IResponseQueuePublisher : IReadOnlyCollection<CalculationResponse>
{
    event Action QueueUpdated;
}