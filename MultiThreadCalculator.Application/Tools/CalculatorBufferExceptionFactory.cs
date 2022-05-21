using MultiThreadCalculator.Application.Exceptions;

namespace MultiThreadCalculator.Application.Tools;

public static class CalculatorBufferExceptionFactory
{
    public static Exception EmptyBuffer()
        => new CalculatorBufferException("Buffer has no input.");
}