using MultiThreadCalculator.Core.ExpressionVisitors;

namespace MultiThreadCalculator.Core.Expressions;

public class LiteralExpression : IExpression
{
    public LiteralExpression(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public double Evaluate()
        => Value;

    public T Accept<T>(IExpressionVisitor<T> visitor)
        => visitor.Visit(this);
}