using MultiThreadCalculator.Core.Expressions;

namespace MultiThreadCalculator.Application.Models;

public class CalculationRequest
{
    public CalculationRequest(IExpression expression)
    {
        Expression = expression;
        State = CalculationRequestState.Awaiting;
    }

    public IExpression Expression { get; }
    public CalculationRequestState State { get; set; }
}