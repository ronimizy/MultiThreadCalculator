using MultiThreadCalculator.Core.Expressions;

namespace MultiThreadCalculator.Application.Buffer;

public interface ICalculatorBuffer
{
    void ConsumeInput(char c);
    void ConsumeInput(string str);
    void Clean();
    void Pop();

    IExpression Build();
    string Format();
}