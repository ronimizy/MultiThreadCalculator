using MultiThreadCalculator.Core.ExpressionBuilderVisitors;

namespace MultiThreadCalculator.Core.Builders.Operator;

public class SummationBuilder : BinaryOperatorBuilder
{
    public override T Accept<T>(IExpressionBuilderVisitor<T> visitor)
        => visitor.Visit(this);

    protected override IExpressionBuilder Consume(
        IExpressionBuilder builder,
        IExpressionBuilder left,
        IExpressionBuilder right)
    {
        Right = right.Consume(builder);
        return this;
    }

    public override string ToString()
        => "Summation";
}