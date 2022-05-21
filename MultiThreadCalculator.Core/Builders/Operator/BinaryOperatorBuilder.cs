using MultiThreadCalculator.Core.ExpressionBuilderVisitors;
using MultiThreadCalculator.Core.Tools;

namespace MultiThreadCalculator.Core.Builders.Operator;

public abstract class BinaryOperatorBuilder : IOperatorBuilder
{
    public IExpressionBuilder? Left { get; protected set; }
    public IExpressionBuilder? Right { get; protected set; }

    public bool IsCompleted => (Left?.IsCompleted ?? false) && (Right?.IsCompleted ?? false);

    public IExpressionBuilder Consume(IExpressionBuilder builder)
    {
        if (Left is null)
        {
            if (!builder.IsCompleted)
                throw ExpressionBuilderExceptionFactory.IncompleteBuilder(this);

            Left = builder;
            return this;
        }

        if (Right is null)
        {
            if (!builder.IsCompleted)
                throw ExpressionBuilderExceptionFactory.IncompleteBuilder(this);

            Right = builder;
            return this;
        }

        if (builder is ILiteralBuilder)
        {
            Right = Right.Consume(builder);
            return this;
        }

        return Consume(builder, Left, Right);
    }

    public IExpressionBuilder? Pop()
    {
        if (Right is null && Left is null)
            return null;

        if (Right is null)
            return Left;

        Right = Right.Pop();
        return this;
    }

    public abstract T Accept<T>(IExpressionBuilderVisitor<T> visitor);

    protected abstract IExpressionBuilder Consume(
        IExpressionBuilder builder,
        IExpressionBuilder left,
        IExpressionBuilder right);
}