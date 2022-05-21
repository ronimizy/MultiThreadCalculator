using System.Text;
using MultiThreadCalculator.Core.Builders.Operator;

namespace MultiThreadCalculator.Core.ExpressionBuilderVisitors;

public class BracedFormatterExpressionBuilderVisitor : FormatterExpressionBuilderVisitor
{
    public override StringBuilder Visit(UnaryMinusBuilder builder)
    {
        StringBuilder.Append('(');
        base.Visit(builder);
        StringBuilder.Append(')');

        return StringBuilder;
    }

    public override StringBuilder Visit(MultiplicationBuilder builder)
    {
        StringBuilder.Append('(');
        base.Visit(builder);
        StringBuilder.Append(')');

        return StringBuilder;
    }

    public override StringBuilder Visit(DivisionBuilder builder)
    {
        StringBuilder.Append('(');
        base.Visit(builder);
        StringBuilder.Append(')');

        return StringBuilder;
    }
}