using MultiThreadCalculator.Core.Expressions;

namespace MultiThreadCalculator.Core.ExpressionVisitors;

public interface IExpressionVisitor<out T>
{
    T Visit(LiteralExpression expression);

    T Visit(UnaryMinusExpression expression);

    T Visit(SummationExpression expression);
    
    T Visit(SubtractionExpression expression);
    
    T Visit(MultiplicationExpression expression);
    
    T Visit(DivisionExpression expression);
}