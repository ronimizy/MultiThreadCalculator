using MultiThreadCalculator.Core.ExpressionVisitors;

namespace MultiThreadCalculator.Core.Expressions;

public class SubtractionExpression : IExpression
{
    public SubtractionExpression(IExpression left, IExpression right)
    {
        Left = left;
        Right = right;
    }

    public IExpression Left { get; }
    public IExpression Right { get; }
    
    public double Evaluate()
        => Left.Evaluate() - Right.Evaluate();

    public T Accept<T>(IExpressionVisitor<T> visitor)
        => visitor.Visit(this);
}