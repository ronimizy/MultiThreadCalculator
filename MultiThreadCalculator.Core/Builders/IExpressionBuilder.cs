using MultiThreadCalculator.Core.ExpressionBuilderVisitors;

namespace MultiThreadCalculator.Core.Builders;

public interface IExpressionBuilder
{
    bool IsCompleted { get; }

    IExpressionBuilder Consume(IExpressionBuilder builder);

    IExpressionBuilder? Pop();

    T Accept<T>(IExpressionBuilderVisitor<T> visitor);
}