using System.Collections;
using System.Collections.Concurrent;
using MultiThreadCalculator.Application.Models;
using MultiThreadCalculator.Application.Tools;

namespace MultiThreadCalculator.Application.Queues;

public class RequestQueue : IRequestQueueConsumer, IRequestQueuePublisher
{
    private readonly RequestQueueConfiguration _configuration;
    private readonly ConcurrentQueue<CalculationRequest> _queue;
    private readonly ConcurrentQueue<CalculationRequest> _untriagedRequests;

    public RequestQueue(RequestQueueConfiguration configuration)
    {
        _configuration = configuration;
        _queue = new ConcurrentQueue<CalculationRequest>();
        _untriagedRequests = new ConcurrentQueue<CalculationRequest>();
    }

    public event Action? QueueUpdated;
    public event Action<Exception>? ErrorOccured;

    public void Consume(CalculationRequest request)
    {
        _untriagedRequests.Enqueue(request);
        _queue.Enqueue(request);
    }

    public async Task<CalculationRequest> GetRequestAsync(CancellationToken cancellationToken)
    {
        CalculationRequest? request;

        while (!_untriagedRequests.TryDequeue(out request))
        {
            await Task.Delay(_configuration.QueuePollingDelay, cancellationToken);
        }

        return request;
    }

    public void StartExecution(CalculationRequest request)
    {
        request.State = CalculationRequestState.Executing;
        OnQueueUpdated();
    }

    public void Report(CalculationRequest request)
    {
        request.State = CalculationRequestState.Completed;
        OnQueueUpdated();
    }

    public void Report(Exception exception)
        => OnErrorOccured(exception);

    public IEnumerator<CalculationRequest> GetEnumerator()
    {
        IEnumerable<CalculationRequest> GetEnumerable()
            => _queue.Where(r => r.State is not CalculationRequestState.Completed);

        return GetEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    protected virtual void OnQueueUpdated()
        => QueueUpdated?.Invoke();

    protected virtual void OnErrorOccured(Exception obj)
        => ErrorOccured?.Invoke(obj);
}