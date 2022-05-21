using System.Collections;
using System.Collections.Concurrent;
using MultiThreadCalculator.Application.Models;

namespace MultiThreadCalculator.Application.Queues;

public class ResponseQueue : IResponseQueueConsumer, IResponseQueuePublisher
{
    private readonly ConcurrentQueue<CalculationResponse> _queue;

    public ResponseQueue()
    {
        _queue = new ConcurrentQueue<CalculationResponse>();
    }

    public event Action? QueueUpdated;

    public int Count => _queue.Count;

    public void Consume(CalculationResponse response)
    {
        _queue.Enqueue(response);
        OnQueueUpdated();
    }

    public IEnumerator<CalculationResponse> GetEnumerator()
        => _queue.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    protected virtual void OnQueueUpdated()
        => QueueUpdated?.Invoke();
}