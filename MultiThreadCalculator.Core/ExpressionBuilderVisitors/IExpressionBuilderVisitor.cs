using MultiThreadCalculator.Core.Builders.Literal;
using MultiThreadCalculator.Core.Builders.Operator;

namespace MultiThreadCalculator.Core.ExpressionBuilderVisitors;

public interface IExpressionBuilderVisitor<out T>
{
    T Visit(NumberLiteralBuilder builder);
    T Visit(DecimalPointBuilder builder);

    T Visit(UnaryMinusBuilder builder);

    T Visit(SummationBuilder builder);
    T Visit(SubtractionBuilder builder);
    
    T Visit(MultiplicationBuilder builder);
    T Visit(DivisionBuilder builder);
}