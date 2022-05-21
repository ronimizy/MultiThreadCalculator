using MultiThreadCalculator.Application.Models;

namespace MultiThreadCalculator.Application.Queues;

public interface IResponseQueueConsumer
{
    void Consume(CalculationResponse response);
}