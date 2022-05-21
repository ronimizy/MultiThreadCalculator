using MultiThreadCalculator.Core.Builders;
using MultiThreadCalculator.Core.Builders.Literal;
using MultiThreadCalculator.Core.Builders.Operator;
using MultiThreadCalculator.Core.Tools;

namespace MultiThreadCalculator.Core.Factories;

public class ExpressionBuilderFactory : IExpressionBuilderFactory
{
    public IExpressionBuilder Create(string expression)
        => Create(expression, null);

    public IExpressionBuilder Create(string expression, IExpressionBuilder? previous)
    {
        var resultingBuilder = expression
            .Where(c => c is not ' ')
            .Aggregate<char, IExpressionBuilder?>(previous, (current, c) => Create(c, current));

        if (resultingBuilder is null)
        {
            throw ExpressionBuilderExceptionFactory.NotAnExpression(expression);
        }

        return resultingBuilder;
    }

    public IExpressionBuilder Create(char c, IExpressionBuilder? previous)
    {
        IExpressionBuilder builder = c switch
        {
            '+' => new SummationBuilder(),
            '-' when previous is null => new UnaryMinusBuilder(),
            '-' => new SubtractionBuilder(),
            '*' => new MultiplicationBuilder(),
            '/' => new DivisionBuilder(),
            >= '0' and <= '9' => new NumberLiteralBuilder(c),
            '.' => new DecimalPointBuilder(),
            _ => throw ExpressionBuilderExceptionFactory.InvalidCharacter(c),
        };

        return previous is null ? builder : previous.Consume(builder);
    }
}