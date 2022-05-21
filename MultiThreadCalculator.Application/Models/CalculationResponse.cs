using MultiThreadCalculator.Core.Expressions;

namespace MultiThreadCalculator.Application.Models;

public class CalculationResponse
{
    public CalculationResponse(double result, IExpression expression, TimeSpan executionDuration)
    {
        Result = result;
        Expression = expression;
        ExecutionDuration = executionDuration;
    }

    public double Result { get; }
    public IExpression Expression { get; }
    public TimeSpan ExecutionDuration { get; }
}