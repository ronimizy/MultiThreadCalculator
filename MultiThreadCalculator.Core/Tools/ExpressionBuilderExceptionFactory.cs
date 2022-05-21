using MultiThreadCalculator.Core.Builders;
using MultiThreadCalculator.Core.Exceptions;

namespace MultiThreadCalculator.Core.Tools;

public static class ExpressionBuilderExceptionFactory
{
    public static Exception MoreThenOneDecimalPoint()
        => throw new ExpressionBuilderException("Number literal cannot contain more than one decimal point.");

    public static Exception IncompleteBuilder(IExpressionBuilder builder)
        => throw new ExpressionBuilderException($"{builder} operation is not complete.");

    public static Exception InvalidStructure(IExpressionBuilder builder)
        => throw new ExpressionBuilderException($"{builder} was at invalid position");

    public static Exception InvalidCharacter(char c)
        => throw new ExpressionBuilderException($"Character {c} is not a valid expression character.");

    public static Exception NotAnExpression(string str)
        => throw new ExpressionBuilderException($"String {str} does not contain an expression");
}