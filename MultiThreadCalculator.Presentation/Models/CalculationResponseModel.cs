namespace MultiThreadCalculator.Presentation.Models;

public class CalculationResponseModel
{
    public CalculationResponseModel(string result, string expression, string duration)
    {
        Expression = expression;
        Duration = duration;
        Result = result;
    }

    public string Result { get; }
    public string Expression { get; }
    public string Duration { get; }
}