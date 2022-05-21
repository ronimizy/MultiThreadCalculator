using MultiThreadCalculator.Core.Builders;

namespace MultiThreadCalculator.Core.Factories;

public interface IExpressionBuilderFactory
{
    IExpressionBuilder Create(string expression);
    IExpressionBuilder Create(string expression, IExpressionBuilder? previous);

    IExpressionBuilder Create(char c, IExpressionBuilder? previous);
}