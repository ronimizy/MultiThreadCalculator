namespace MultiThreadCalculator.Core.Builders;

public interface ILiteralBuilder : IExpressionBuilder
{
    IExpressionBuilder? Next { get; }
}