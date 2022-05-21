namespace MultiThreadCalculator.Core.Exceptions;

public abstract class MultiThreadCalculatorException : Exception
{
    protected MultiThreadCalculatorException(string? message) : base(message) { }
}