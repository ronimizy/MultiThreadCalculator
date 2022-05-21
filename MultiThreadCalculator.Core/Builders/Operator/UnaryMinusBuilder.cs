using MultiThreadCalculator.Core.ExpressionBuilderVisitors;

namespace MultiThreadCalculator.Core.Builders.Operator;

public class UnaryMinusBuilder : IExpressionBuilder
{
    public IExpressionBuilder? Wrapped { get; private set; }

    public bool IsCompleted => Wrapped is not null && Wrapped.IsCompleted;

    public IExpressionBuilder Consume(IExpressionBuilder builder)
    {
        if (builder is IOperatorBuilder)
        {
            return builder.Consume(this);
        }
        
        Wrapped = Wrapped is null ? builder : Wrapped.Consume(builder);
        return this;
    }

    public IExpressionBuilder? Pop()
    {
        if (Wrapped is null)
            return null;

        Wrapped = Wrapped.Pop();
        return this;
    }

    public T Accept<T>(IExpressionBuilderVisitor<T> visitor)
        => visitor.Visit(this);

    public override string ToString()
        => "Unary minus";
}