using System.Text;
using MultiThreadCalculator.Core.Builders.Literal;
using MultiThreadCalculator.Core.Builders.Operator;

namespace MultiThreadCalculator.Core.ExpressionBuilderVisitors;

public class FormatterExpressionBuilderVisitor : IFormatterExpressionBuilderVisitor
{
    protected readonly StringBuilder StringBuilder = new StringBuilder();

    public void Clear()
        => StringBuilder.Clear();

    public virtual StringBuilder Visit(NumberLiteralBuilder builder)
    {
        StringBuilder.Append(builder.Literal);
        builder.Next?.Accept(this);

        return StringBuilder;
    }

    public virtual StringBuilder Visit(DecimalPointBuilder builder)
    {
        StringBuilder.Append('.');
        builder.Next?.Accept(this);

        return StringBuilder;
    }

    public virtual StringBuilder Visit(UnaryMinusBuilder builder)
    {
        StringBuilder.Append('-');

        if (builder.Wrapped is not null)
        {
            builder.Wrapped.Accept(this);
        }

        return StringBuilder;
    }

    public virtual StringBuilder Visit(SummationBuilder builder)
    {
        builder.Left?.Accept(this);
        StringBuilder.Append(" + ");
        builder.Right?.Accept(this);

        return StringBuilder;
    }

    public virtual StringBuilder Visit(SubtractionBuilder builder)
    {
        builder.Left?.Accept(this);
        StringBuilder.Append(" - ");
        builder.Right?.Accept(this);

        return StringBuilder;
    }

    public virtual StringBuilder Visit(MultiplicationBuilder builder)
    {
        builder.Left?.Accept(this);
        StringBuilder.Append(" * ");
        builder.Right?.Accept(this);

        return StringBuilder;
    }

    public virtual StringBuilder Visit(DivisionBuilder builder)
    {
        builder.Left?.Accept(this);
        StringBuilder.Append(" / ");
        builder.Right?.Accept(this);

        return StringBuilder;
    }
}