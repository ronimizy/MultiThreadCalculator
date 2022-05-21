namespace MultiThreadCalculator.Presentation.Models;

public class CalculationRequestModel
{
    public CalculationRequestModel(string expression, bool isExecuting)
    {
        Expression = expression;
        IsExecuting = isExecuting;
    }

    public string Expression { get; }
    public bool IsExecuting { get; }
}