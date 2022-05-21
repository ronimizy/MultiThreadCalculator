using MultiThreadCalculator.Core.ExpressionVisitors;

namespace MultiThreadCalculator.Core.Expressions;

public interface IExpression
{
    double Evaluate();
    T Accept<T>(IExpressionVisitor<T> visitor);
}