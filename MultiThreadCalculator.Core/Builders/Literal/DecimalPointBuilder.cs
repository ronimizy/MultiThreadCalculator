using MultiThreadCalculator.Core.ExpressionBuilderVisitors;
using MultiThreadCalculator.Core.Tools;

namespace MultiThreadCalculator.Core.Builders.Literal;

public class DecimalPointBuilder : ILiteralBuilder
{
    public IExpressionBuilder? Next { get; protected set; }

    public bool IsCompleted => true;

    public IExpressionBuilder Consume(IExpressionBuilder builder)
    {
        if (builder is DecimalPointBuilder)
            throw ExpressionBuilderExceptionFactory.MoreThenOneDecimalPoint();

        if (builder is not ILiteralBuilder)
        {
            return builder.Consume(this);
        }

        Next = Next is null ? builder : Next.Consume(builder);
        return this;
    }

    public IExpressionBuilder? Pop()
    {
        if (Next is null)
            return null;

        Next = Next.Pop();
        return this;
    }

    public T Accept<T>(IExpressionBuilderVisitor<T> visitor)
        => visitor.Visit(this);

    public override string ToString()
        => "Decimal point";
}