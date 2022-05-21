using MultiThreadCalculator.Core.ExpressionBuilderVisitors;

namespace MultiThreadCalculator.Core.Builders.Operator;

public class SubtractionBuilder : BinaryOperatorBuilder
{
    public override T Accept<T>(IExpressionBuilderVisitor<T> visitor)
        => visitor.Visit(this);

    protected override IExpressionBuilder Consume(
        IExpressionBuilder builder,
        IExpressionBuilder left,
        IExpressionBuilder right)
    {
        if (builder is IPrioritizedOperatorBuilder)
        {
            Right = right.Consume(builder);
            return this;
        }

        return builder.Consume(this);
    }

    public override string ToString()
        => "Subtraction";
}