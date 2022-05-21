using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiThreadCalculator.Core.ExpressionBuilderVisitors;
using MultiThreadCalculator.Core.Expressions;
using MultiThreadCalculator.Core.Factories;
using NUnit.Framework;

namespace MultiThreadCalculator.Tests;

public class ExpressionBuilderTests
{
    public record struct ExpressionBuilderTestCase(string Expression, double Result);

    private IExpressionBuilderFactory _expressionBuilderFactory = null!;
    private IExpressionBuilderVisitor<StringBuilder> _formatterVisitor = null!;
    private IExpressionBuilderVisitor<IExpression> _buildingVisitor = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        _expressionBuilderFactory = new ExpressionBuilderFactory();
    }

    [SetUp]
    public void IterationSetup()
    {
        _formatterVisitor = new BracedFormatterExpressionBuilderVisitor();
        _buildingVisitor = new BuildingExpressionBuilderVisitor();
    }

    [Test]
    [TestCaseSource(nameof(TestCases))]
    public void ExpressionBuildingTest_ExpressionStringParsed_WithSameFormatting_WithSameResult(
        ExpressionBuilderTestCase testCase)
    {
        var (expressionString, result) = testCase;

        var expressionBuilder = _expressionBuilderFactory.Create(expressionString);
        var formattedExpressionBuilder = expressionBuilder.Accept(_formatterVisitor).ToString();
        var expression = expressionBuilder.Accept(_buildingVisitor);

        Console.WriteLine(formattedExpressionBuilder);

        Assert.AreEqual(result, expression.Evaluate(), 0.000001, "Invalid structure.");
        CollectionAssert.AreEqual(expressionString, RemoveBraces(formattedExpressionBuilder), "Invalid formatting.");
    }

    private static IEnumerable<char> RemoveBraces(string str)
        => str.Where(c => c is not '(' and not ')');

    public static IEnumerable<ExpressionBuilderTestCase> TestCases()
    {
        yield return new ExpressionBuilderTestCase("1 + 2", 3);
        yield return new ExpressionBuilderTestCase("1 - 3 + 4", 2);
        yield return new ExpressionBuilderTestCase("-1 + 3", 2);
        yield return new ExpressionBuilderTestCase("2 * 4", 8);
        yield return new ExpressionBuilderTestCase("4 / 2", 2);
        yield return new ExpressionBuilderTestCase("2.5 * 2 + 1", 6);
        yield return new ExpressionBuilderTestCase("-2.25 + 23 - 3 * 6 + 2 + 4 / 2", 6.75);
        yield return new ExpressionBuilderTestCase("5 * 9 / 4 + 3 * 2 - 1 + 2 / 4 - 3.234 + 12 * 8 / 14 + 12 / 2 * 4", 44.3731428);
        yield return new ExpressionBuilderTestCase("36.75 + 28.94 * 19.35 - 2.84 + 4.44 + 38.14 - 38.3 - 13.53", 584.6490000000001D);
        yield return new ExpressionBuilderTestCase("52.61 * 29.22 + 39.15 - 23.99 - 5.62 + 60.03 * 6.45 + 3.88", 1937.8777000000002D);
        yield return new ExpressionBuilderTestCase("12.23 + 38.4 / 56.41 * 7.8 / 5 - 24.92", 12.23 + 38.4 / 56.41 * 7.8 / 5 - 24.92);
        yield return new ExpressionBuilderTestCase("32.76 * 28.88 / 34.85 / 6.27 + 8.88 - 8.13 * 0.69 - 5.81 * 16.68 - 44.09", -133.40067086213642D);
        yield return new ExpressionBuilderTestCase("40.59 + 40.51 / 68.92 - 34.76 + 36.35 * 12.24 * 38.2 + 11.66 + 88.8", 17102.974582936742D);
        yield return new ExpressionBuilderTestCase("65.59 - 31.11 + 89.61 / 3.27 * 0 - 3.47 + 27.36 / 10.67 * 62.72", 191.83654170571697D);
        yield return new ExpressionBuilderTestCase("33.43 * 35.49 / 21.75 + 48.3 / 34.44", 55.950976955424736D);
        yield return new ExpressionBuilderTestCase("5.82 * 21.75 * 0 / 11.96 / 1.69", 0D);
        yield return new ExpressionBuilderTestCase("47.43 / 19.13", 2.4793518034500783D);
        yield return new ExpressionBuilderTestCase("1.19 - 34.73 / 19.37 + 48.81 / 40.12 + 49.42 * 1.14 - 30.95 * 26.02 * 4.05", -3204.5895286338455D);
        yield return new ExpressionBuilderTestCase("25.91 - 31.29 * 1.58 * 51.31 - 25.85 * 5.11 - 55.58 * 38.94 * 77.27 - 16.22", -169893.394946D);
        yield return new ExpressionBuilderTestCase("50.95 / 1.86 * 62.47 + 15.63 + 41.24 - 8.68 + 16.89", 1776.2877956989248D);
        yield return new ExpressionBuilderTestCase("35.29 + 12.16 / 19.48 / 43.41 + 55.81 - 6.7", 84.41437986591721D);
        yield return new ExpressionBuilderTestCase("19.64 * 15.68 + 42.26 * 87.82", 4019.2283999999995D);
        yield return new ExpressionBuilderTestCase("47.97 * 6.28 + 52.05 * 84.16 - 57.5 * 1.17 + 2.85", 4617.3546D);
        yield return new ExpressionBuilderTestCase("25.61 - 3.57 - 1.95 / 11.71 + 5.33 + 46.87 - 22.84 + 20.8 - 19.26 - 23.29", 29.48347566182749D);
        yield return new ExpressionBuilderTestCase("52.51 + 11.65 * 4.68 - 13.51", 93.52199999999999D);
        yield return new ExpressionBuilderTestCase("79.84 * 12.02 * 38.7 / 7.78 * 3.19 * 0 + 2.87 * 22.6", 64.86200000000001D);
        yield return new ExpressionBuilderTestCase("0.9 / 15.72 - 33.41 / 20.93 / 47.88 - 18.32 - 0.39", -18.686087132787797D);
        yield return new ExpressionBuilderTestCase("6.78 * 45.16 + 8.12 * 10.3 / 27.38", 309.239438422206D);
        yield return new ExpressionBuilderTestCase("38.59 + 0.94 - 15.89 * 39.82 + 40.12 + 13.1", -539.9898000000001D);
        yield return new ExpressionBuilderTestCase("0.16 - 3.65 - 16.62 / 27.78 - 9.19", -13.27827213822894D);
        yield return new ExpressionBuilderTestCase("37.52 - 6.22 - 42.67 - 0.67 + 16.07 + 62.13 * 12.3 - 59.39 / 71.63", 767.3998781236912D);
        yield return new ExpressionBuilderTestCase("0.52 / 7.48 - 40.57 - 0.99 - 3.16 + 2.24 * 3.62 * 13.19", 62.304590716577536D);
        yield return new ExpressionBuilderTestCase("53.79 * 76.52 - 1.35 + 6.84 + 13.62 / 26.8", 4122.009008955224D);
        yield return new ExpressionBuilderTestCase("21.7 + 13.5 + 23.87 * 35.2 / 17.32 + 22.98 + 7.39 * 55.31", 515.4326782909931D);
        yield return new ExpressionBuilderTestCase("5.09 - 37.26 + 5.16 - 41.27 * 52.31 + 92.13 / 54.88 / 39.08 / 16.05", -2185.8410235608335D);
        yield return new ExpressionBuilderTestCase("12.77 - 22.11 + 4.99 + 40.86 - 49.07 / 24.57", 34.512849002849D);
        yield return new ExpressionBuilderTestCase("0 - 19.99 + 41.78 / 11.03 - 6.27 * 1.97 - 6.31 - 6.33 / 1.62", -38.77145609281085D);
        yield return new ExpressionBuilderTestCase("20.84 + 41.98 + 1.44 + 27.7 + 36.96 - 41.54 - 11.07 + 11.4 + 5.53", 93.24000000000001D);
        yield return new ExpressionBuilderTestCase("12.39 * 1.7 * 17.87 - 10.9", 365.49581D);
        yield return new ExpressionBuilderTestCase("4.64 - 5.08 / 33.28 + 56.19 * 3.79 / 86.83 - 53.67", -46.73003453365994D);
        yield return new ExpressionBuilderTestCase("49.09 * 7.57 / 16.07 - 22.18 - 66.02 / 43.29 * 45.8", -68.9033730446736D);
        yield return new ExpressionBuilderTestCase("0.81 * 42.94 - 83.39 + 8.22 - 0.74 + 11.23 / 33.5 * 5.11 / 0.95 * 6.82", -28.831106017282018D);
        yield return new ExpressionBuilderTestCase("40.06 / 2 - 4.59 - 13.48 + 35.96", 40.06 / 2 - 4.59 - 13.48 + 35.96);
        yield return new ExpressionBuilderTestCase("14.65 + 59.08 - 6.03 * 38.04 / 30.08 * 38.55 / 8.52", 39.226370945085414D);
        yield return new ExpressionBuilderTestCase("34.8 + 17.44 + 15.16 - 5.55 - 0.91 * 71.17 - 3.01", -5.92470000000001D);
        yield return new ExpressionBuilderTestCase("43.76 * 1 - 0.84 * 43.9", 6.884D);
        yield return new ExpressionBuilderTestCase("6.72 / 3.56 - 57.56 / 0.81 * 11.68 + 8.52 - 0.63 / 13.54 * 3.58 / 31.29", -819.598670730504D);
        yield return new ExpressionBuilderTestCase("18.87 + 0.29 / 4.12 / 36.29 * 4.02 + 72.33 * 0.4 + 37.91", 85.7197972214122D);
        yield return new ExpressionBuilderTestCase("45.16 * 4.79 * 19.31 + 7.45 + 25.37 * 7.11", 4364.900383999999D);
        yield return new ExpressionBuilderTestCase("6.78 * 23.52 + 59.49 * 9.56 - 73.44 * 0.02 - 63.46 / 16.23 * 1.4", 721.2471396179915D);
        yield return new ExpressionBuilderTestCase("4.94 - 50.69 + 7.6 - 21.14 - 37.8 * 35.11 * 22.33 * 51.83", -1536064.0487961997D);
        yield return new ExpressionBuilderTestCase("33.76 - 43.95 * 9.01 + 24.73 * 35.17 * 1.2 + 0.24", 681.7154200000001D);
        yield return new ExpressionBuilderTestCase("9.52 / 55.22 * 31.6 * 66.71 + 69.22 * 20.19 + 50.5", 1811.4799550162984D);
        yield return new ExpressionBuilderTestCase("17.62 / 10.35 / 11.11 + 57.98 + 26.52 + 24.53 + 64.86 - 9.35", 164.69323271457583D);
        yield return new ExpressionBuilderTestCase("67.61 - 2.74 + 4.47 - 1.62 + 2.56 + 68.44 * 42.11 / 51.72 * 17.7 + 3.94", 1060.5221786542922D);
        yield return new ExpressionBuilderTestCase("34.25 / 10.67 + 79.6 * 9.6 / 8.03 * 41.88 * 57.33 + 27.39", 228515.42963103313D);
        yield return new ExpressionBuilderTestCase("42.56 - 70.61 * 34.84 / 22.79", -65.38437911364633D);
        yield return new ExpressionBuilderTestCase("10.23 * 14.12 / 46.46 * 42.52 * 3.71 * 82.02 * 35.15 / 4.95 - 5.42 / 11.97", 285652.1501322184D);
        yield return new ExpressionBuilderTestCase("5.01 + 2.56 - 20.39 / 2.36 / 89 * 28.3 - 14.49 / 12.87 + 23.7", 27.39685392592553D);
        yield return new ExpressionBuilderTestCase("31.17 - 77.79 - 61.53 + 83.93 / 51.36", -106.51584890965732D);
        yield return new ExpressionBuilderTestCase("60.66 - 9.74 + 27.44 / 17.69", 52.4711588468061D);
        yield return new ExpressionBuilderTestCase("39.88 * 1.26 * 1.21 / 4.91 * 12.61 / 64.37 / 24.91 + 25.39 + 28.21 - 3.26", 50.43738395825853D);
        yield return new ExpressionBuilderTestCase("16.29 - 52.9 - 11.48 / 1.07 + 31.89 / 4.99 + 10.54 + 6.71 - 0.95 + 6.74", -17.90819039949057D);
        yield return new ExpressionBuilderTestCase("4.58 - 0.17 * 1.99 * 58.6 + 2.29", -12.95438D);
        yield return new ExpressionBuilderTestCase("30.2 - 60.53 / 7.62 - 17.19", 5.066430446194222D);
        yield return new ExpressionBuilderTestCase("16.06 + 15.8 + 48.03 + 5.3 - 12.3 - 38.85 + 1.41", 35.449999999999996D);
        yield return new ExpressionBuilderTestCase("1.29 / 1.65 - 3.51 / 9.7 + 25.87", 26.28996251171509D);
        yield return new ExpressionBuilderTestCase("45.02 + 34.35 + 7.97 + 18.11 + 5.81", 111.26D);
        yield return new ExpressionBuilderTestCase("8.92 - 58.02 / 1.27 / 24.98 - 54.86 - 7.94", -55.70886466653638D);
        yield return new ExpressionBuilderTestCase("1.3 + 12.73 + 38.68 + 53.52", 106.23D);
        yield return new ExpressionBuilderTestCase("28.45 + 3.14 - 31.03 + 25.46 + 22.29", 48.31D);
        yield return new ExpressionBuilderTestCase("24.56 - 0 + 13.66 - 10.62 + 21.51 / 8.02 - 3.23 - 12.36 / 59.68", 26.8449403301398D);
        yield return new ExpressionBuilderTestCase("16.43 + 54.24 + 24.48 / 10.11 + 3.2 * 6.71", 94.56336498516322D);
        yield return new ExpressionBuilderTestCase("4.68 / 7.47 / 10.76 * 9.95 / 31.06", 0.018652395231435488D);
        yield return new ExpressionBuilderTestCase("10.13 * 60.49 - 30.04 * 65.83 - 11.26 - 10.71 + 0.01", -1386.7295D);
        yield return new ExpressionBuilderTestCase("13.28 / 2.4 - 13.96 / 18.24 / 4.5 / 60.3 / 1.59 - 51.76", -46.22844058504865D);
        yield return new ExpressionBuilderTestCase("43.96 * 3.82 / 49.75 - 24.79", -21.41457889447236D);
        yield return new ExpressionBuilderTestCase("7.74 + 34.87 + 10.89 + 4.34 - 25 - 7.8 * 11.31 / 1.1 / 1.56", -18.569090909090896D);
    }
}