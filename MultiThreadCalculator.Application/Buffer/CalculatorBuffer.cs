using MultiThreadCalculator.Application.Tools;
using MultiThreadCalculator.Core.Builders;
using MultiThreadCalculator.Core.ExpressionBuilderVisitors;
using MultiThreadCalculator.Core.Expressions;
using MultiThreadCalculator.Core.Factories;

namespace MultiThreadCalculator.Application.Buffer;

public class CalculatorBuffer : ICalculatorBuffer
{
    private readonly IExpressionBuilderVisitor<IExpression> _expressionBuildingVisitor;
    private readonly IFormatterExpressionBuilderVisitor _formatterVisitor;
    private readonly IExpressionBuilderFactory _factory;
    private IExpressionBuilder? _builder;

    public CalculatorBuffer(
        IExpressionBuilderFactory factory,
        IExpressionBuilderVisitor<IExpression> expressionBuildingVisitor,
        IFormatterExpressionBuilderVisitor formatterVisitor)
    {
        _factory = factory;
        _expressionBuildingVisitor = expressionBuildingVisitor;
        _formatterVisitor = formatterVisitor;
    }

    public void ConsumeInput(char c)
    {
        _builder = _factory.Create(c, _builder);
    }

    public void ConsumeInput(string str)
    {
        _builder = _factory.Create(str, _builder);
    }

    public void Clean()
    {
        _builder = null;
    }

    public void Pop()
    {
        _builder = _builder?.Pop();
    }

    public IExpression Build()
    {
        if (_builder is null)
            throw CalculatorBufferExceptionFactory.EmptyBuffer();

        return _builder.Accept(_expressionBuildingVisitor);
    }

    public string Format()
    {
        _formatterVisitor.Clear();
        return _builder?.Accept(_formatterVisitor).ToString() ?? string.Empty;
    }

    public override string ToString()
        => Format();
}