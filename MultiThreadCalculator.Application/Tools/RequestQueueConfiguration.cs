namespace MultiThreadCalculator.Application.Tools;

public class RequestQueueConfiguration
{
    public RequestQueueConfiguration(TimeSpan queuePollingDelay)
    {
        QueuePollingDelay = queuePollingDelay;
    }

    public TimeSpan QueuePollingDelay { get; set; }
}