using MultiThreadCalculator.Core.ExpressionVisitors;

namespace MultiThreadCalculator.Core.Expressions;

public class DivisionExpression : IExpression
{
    public DivisionExpression(IExpression divisible, IExpression divisor)
    {
        Divisible = divisible;
        Divisor = divisor;
    }

    public IExpression Divisible { get; }
    public IExpression Divisor { get; }

    public double Evaluate()
    {
        if (Divisor.Evaluate() is 0)
            throw new DivideByZeroException();
        
        return Divisible.Evaluate() / Divisor.Evaluate();
    }

    public T Accept<T>(IExpressionVisitor<T> visitor)
        => visitor.Visit(this);
}