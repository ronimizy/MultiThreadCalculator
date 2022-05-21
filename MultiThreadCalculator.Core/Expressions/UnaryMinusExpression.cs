using MultiThreadCalculator.Core.ExpressionVisitors;

namespace MultiThreadCalculator.Core.Expressions;

public class UnaryMinusExpression : IExpression
{
    public UnaryMinusExpression(IExpression wrapped)
    {
        Wrapped = wrapped;
    }

    public IExpression Wrapped { get; }

    public double Evaluate()
        => -Wrapped.Evaluate();

    public T Accept<T>(IExpressionVisitor<T> visitor)
        => visitor.Visit(this);
}