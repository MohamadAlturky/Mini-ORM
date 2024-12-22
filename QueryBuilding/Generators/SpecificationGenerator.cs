using System.Linq.Expressions;

namespace QueryBuilding.Contexts;

public static class SpecificationGenerator
{
    public static List<Specification> GetSpecification<TEntity>(Expression<Func<TEntity, bool>> predicate)
    {
        var specifications = new List<Specification>();

        if (predicate.Body is BinaryExpression binaryExpression)
        {
            ParseBinaryExpression(binaryExpression, specifications);
        }
        else
        {
            throw new NotSupportedException($"Unsupported expression: {predicate.Body}");
        }

        return specifications;
    }

    private static void ParseBinaryExpression(BinaryExpression binaryExpression, List<Specification> specifications)
    {
        if (binaryExpression.Left is BinaryExpression leftBinary)
        {
            ParseBinaryExpression(leftBinary, specifications);
        }
        else
        {
            specifications.Add(new Specification
            {
                Left = GetExpressionName(binaryExpression.Left),
                ExpressionType = binaryExpression.NodeType,
                Right = EvaluateExpression(binaryExpression.Right)
            });
        }

        if (binaryExpression.Right is BinaryExpression rightBinary)
        {
            ParseBinaryExpression(rightBinary, specifications);
        }
    }
    private static object EvaluateExpression(Expression expression)
    {
        var lambda = Expression.Lambda<Func<object>>(Expression.Convert(expression, typeof(object)));
        var compiledLambda = lambda.Compile();

        return compiledLambda();
    }
    private static string GetExpressionName(Expression expression)
    {
        switch (expression)
        {
            case MemberExpression memberExpression:
                return memberExpression.Member.Name;

            case ConstantExpression constantExpression:
                return constantExpression.Value?.ToString() ?? "null";

            case UnaryExpression unaryExpression:
                return GetExpressionName(unaryExpression.Operand);

            default:
                return $"Unsupported expression: {expression}";
        }
    }
}

public class Specification
{
    public string Left { get; set; } = null!;
    public ExpressionType ExpressionType { get; set; }
    public object Right { get; set; } = null!;
}
