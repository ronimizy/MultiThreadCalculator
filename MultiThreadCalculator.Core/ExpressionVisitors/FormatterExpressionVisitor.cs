using System.Text;
using MultiThreadCalculator.Core.Expressions;

namespace MultiThreadCalculator.Core.ExpressionVisitors;

public class FormatterExpressionVisitor : IFormatterExpressionVisitor
{
    private readonly StringBuilder _stringBuilder = new StringBuilder();
    
    public void Clear()
        => _stringBuilder.Clear();

    public StringBuilder Visit(LiteralExpression expression)
    {
        _stringBuilder.Append(expression.Value);
        return _stringBuilder;
    }

    public StringBuilder Visit(UnaryMinusExpression expression)
    {
        _stringBuilder.Append('-');
        expression.Wrapped.Accept(this);
        return _stringBuilder;
    }

    public StringBuilder Visit(SummationExpression expression)
    {
        expression.Left.Accept(this);
        _stringBuilder.Append('+');
        expression.Right.Accept(this);
        
        return _stringBuilder;
    }

    public StringBuilder Visit(SubtractionExpression expression)
    {
        expression.Left.Accept(this);
        _stringBuilder.Append('-');
        expression.Right.Accept(this);
        
        return _stringBuilder;
    }

    public StringBuilder Visit(MultiplicationExpression expression)
    {
        expression.Left.Accept(this);
        _stringBuilder.Append('*');
        expression.Right.Accept(this);
        
        return _stringBuilder;
    }

    public StringBuilder Visit(DivisionExpression expression)
    {
        expression.Divisible.Accept(this);
        _stringBuilder.Append('/');
        expression.Divisor.Accept(this);
        
        return _stringBuilder;
    }
}