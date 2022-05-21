using System.Text;

namespace MultiThreadCalculator.Core.ExpressionBuilderVisitors;

public interface IFormatterExpressionBuilderVisitor : IExpressionBuilderVisitor<StringBuilder>
{
    void Clear();
}