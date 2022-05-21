using MultiThreadCalculator.Core.Builders.Literal;
using MultiThreadCalculator.Core.Builders.Operator;
using MultiThreadCalculator.Core.Expressions;
using MultiThreadCalculator.Core.Tools;

namespace MultiThreadCalculator.Core.ExpressionBuilderVisitors;

public class BuildingExpressionBuilderVisitor : IExpressionBuilderVisitor<IExpression>
{
    public IExpression Visit(NumberLiteralBuilder builder)
    {
        var value = char.GetNumericValue(builder.Literal);

        while (builder.Next is NumberLiteralBuilder numberLiteralBuilder)
        {
            value = value * 10 + char.GetNumericValue(numberLiteralBuilder.Literal);
            builder = numberLiteralBuilder;
        }

        if (builder.Next is null)
            return new LiteralExpression(value);

        if (builder.Next is not DecimalPointBuilder decimalPointBuilder)
            throw ExpressionBuilderExceptionFactory.InvalidStructure(builder);

        value += VisitDecimal(decimalPointBuilder);

        return new LiteralExpression(value);
    }

    public IExpression Visit(DecimalPointBuilder builder)
    {
        var value = VisitDecimal(builder);
        return new LiteralExpression(value);
    }

    public IExpression Visit(UnaryMinusBuilder builder)
    {
        if (!builder.IsCompleted)
            ExpressionBuilderExceptionFactory.IncompleteBuilder(builder);
        
        return new UnaryMinusExpression(builder.Wrapped!.Accept(this));
    }

    public IExpression Visit(SummationBuilder builder)
    {
        if (!builder.IsCompleted)
            ExpressionBuilderExceptionFactory.IncompleteBuilder(builder);

        var left = builder.Left!.Accept(this);
        var right = builder.Right!.Accept(this);

        return new SummationExpression(left, right);
    }

    public IExpression Visit(SubtractionBuilder builder)
    {
        if (!builder.IsCompleted)
            ExpressionBuilderExceptionFactory.IncompleteBuilder(builder);
        
        var left = builder.Left!.Accept(this);
        var right = builder.Right!.Accept(this);

        return new SubtractionExpression(left, right);
    }

    public IExpression Visit(MultiplicationBuilder builder)
    {
        var left = builder.Left!.Accept(this);
        var right = builder.Right!.Accept(this);

        return new MultiplicationExpression(left, right);
    }

    public IExpression Visit(DivisionBuilder builder)
    {
        var left = builder.Left!.Accept(this);
        var right = builder.Right!.Accept(this);

        return new DivisionExpression(left, right);
    }

    private static double VisitDecimal(DecimalPointBuilder builder)
    {
        var count = 1;
        var value = 0d;
        var current = builder.Next;

        while (current is NumberLiteralBuilder numberLiteralBuilder)
        {
            value += char.GetNumericValue(numberLiteralBuilder.Literal) * Math.Pow(10, -count);
            count++;

            current = numberLiteralBuilder.Next;
        }

        return value;
    }
}