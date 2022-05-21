using MultiThreadCalculator.Core.ExpressionBuilderVisitors;

namespace MultiThreadCalculator.Core.Builders.Literal;

public class NumberLiteralBuilder : ILiteralBuilder
{
    public NumberLiteralBuilder(char literal)
    {
        Literal = literal;
    }

    public IExpressionBuilder? Next { get; private set; }

    public char Literal { get; }

    public bool IsCompleted => true;

    public IExpressionBuilder Consume(IExpressionBuilder builder)
    {
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
        => "Number literal";
}