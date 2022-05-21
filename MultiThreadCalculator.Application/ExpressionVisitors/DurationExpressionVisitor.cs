using MultiThreadCalculator.Application.Tools;
using MultiThreadCalculator.Core.Expressions;
using MultiThreadCalculator.Core.ExpressionVisitors;

namespace MultiThreadCalculator.Application.ExpressionVisitors;

public class DurationExpressionVisitor : IExpressionVisitor<TimeSpan>
{
    private readonly ExpressionOperationExecutionDuration _duration;

    public DurationExpressionVisitor(ExpressionOperationExecutionDuration duration)
    {
        _duration = duration;
    }

    public TimeSpan Visit(LiteralExpression expression)
        => TimeSpan.FromSeconds(_duration.Literal);

    public TimeSpan Visit(UnaryMinusExpression expression)
    {
        return TimeSpan.FromSeconds(_duration.UnaryMinus) + expression.Wrapped.Accept(this);
    }

    public TimeSpan Visit(SummationExpression expression)
    {
        return TimeSpan.FromSeconds(_duration.Summation) +
               expression.Left.Accept(this) +
               expression.Right.Accept(this);
    }

    public TimeSpan Visit(SubtractionExpression expression)
    {
        return TimeSpan.FromSeconds(_duration.Subtraction) +
               expression.Left.Accept(this) +
               expression.Right.Accept(this);
    }

    public TimeSpan Visit(MultiplicationExpression expression)
    {
        return TimeSpan.FromSeconds(_duration.Multiplication) +
               expression.Left.Accept(this) +
               expression.Right.Accept(this);
    }

    public TimeSpan Visit(DivisionExpression expression)
    {
        return TimeSpan.FromSeconds(_duration.Division) +
               expression.Divisible.Accept(this) +
               expression.Divisor.Accept(this);
    }
}