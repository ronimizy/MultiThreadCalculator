using MultiThreadCalculator.Core.ExpressionBuilderVisitors;

namespace MultiThreadCalculator.Core.Builders.Operator;

public class DivisionBuilder : BinaryOperatorBuilder, IPrioritizedOperatorBuilder
{
    public override T Accept<T>(IExpressionBuilderVisitor<T> visitor)
        => visitor.Visit(this);

    protected override IExpressionBuilder Consume(
        IExpressionBuilder builder,
        IExpressionBuilder left,
        IExpressionBuilder right)
    {
        return builder.Consume(this);
    }

    public override string ToString()
        => "Division";
}