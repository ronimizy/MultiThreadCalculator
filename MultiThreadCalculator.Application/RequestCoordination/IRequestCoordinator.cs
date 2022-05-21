namespace MultiThreadCalculator.Application.RequestCoordination;

public interface IRequestCoordinator
{
    Task Start(CancellationToken cancellationToken);
}