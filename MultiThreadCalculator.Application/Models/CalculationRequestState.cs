namespace MultiThreadCalculator.Application.Models;

public enum CalculationRequestState
{
    Awaiting = 1,
    Executing = 2,
    Completed = 3,
}