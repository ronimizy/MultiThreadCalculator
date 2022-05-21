using System.Text;

namespace MultiThreadCalculator.Core.ExpressionVisitors;

public interface IFormatterExpressionVisitor : IExpressionVisitor<StringBuilder>
{
    void Clear();
}