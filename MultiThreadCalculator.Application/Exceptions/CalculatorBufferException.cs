using MultiThreadCalculator.Core.Exceptions;

namespace MultiThreadCalculator.Application.Exceptions;

public class CalculatorBufferException : MultiThreadCalculatorException
{
    public CalculatorBufferException(string? message) : base(message) { }
}