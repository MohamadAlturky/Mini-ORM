using System.Linq.Expressions;
using System.Reflection;
using Core.Models.Filtering;

namespace Data.Contexts;

public class Set<T>
{
    public readonly List<Expression<Func<T, bool>>> _where = [];
    public readonly List<Where> _whereClause = [];
    public readonly List<OrderBy> _orderBy = [];
    public readonly List<GroupBy> _groupby = [];
    public readonly string ViewName = typeof(T).Name;
    public readonly List<string> Columns = _getPropertyNames();
    public Set<T> Where(Expression<Func<T, bool>> action)
    {
        _where.Add(action);
        foreach (Specification item in SpecificationBuilder.GetSpecification(action))
        {
        _whereClause.Add
        (
            new ()
            {
                Column = item.Left,
                ExpressionType = item.ExpressionType,
                ParameterName = item.Right.ToString()
            }            
        );    
        }
        return this;
    }
    public Set<T> OrderByASC(Expression<Func<T, object>> action)
    {
        _orderBy.Add(new()
        {
            Column = _getPropertyName(action),
            DESC = false
        });
        return this;
    }
    public Set<T> OrderByDESC(Expression<Func<T, object>> action)
    {
        _orderBy.Add(new()
        {
            Column = _getPropertyName(action),
            DESC = true
        });
        return this;
    }
    public Set<T> GroupBy(Expression<Func<T, object>> action)
    {
        _groupby.Add(new()
        {
            Column = _getPropertyName(action)
        });
        return this;
    }
    public static List<string> _getPropertyNames()
    {
        // Use reflection to get the properties of type T
        return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Select(prop => prop.Name)
                        .ToList();
    }
    private string _getPropertyName(Expression<Func<T, object>> expression)
    {
        // Extract the body of the expression
        if (expression.Body is MemberExpression member)
        {
            return member.Member.Name;
        }
        else if (expression.Body is UnaryExpression unary)
        {
            // Handle cases where the property is a value type
            if (unary.Operand is MemberExpression operandMember)
            {
                return operandMember.Member.Name;
            }
        }

        throw new ArgumentException("Invalid expression format");
    }
}

public static class SpecificationBuilder
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
                Left = GetExpressionValue(binaryExpression.Left),
                Operator = GetOperator(binaryExpression.NodeType),
                ExpressionType = binaryExpression.NodeType,
                Right = GetExpressionValue(binaryExpression.Right)
            });
        }

        if (binaryExpression.Right is BinaryExpression rightBinary)
        {
            ParseBinaryExpression(rightBinary, specifications);
        }
    }

    private static string GetExpressionValue(Expression expression)
    {
        switch (expression)
        {
            case MemberExpression memberExpression:
                return memberExpression.Member.Name;

            case ConstantExpression constantExpression:
                return constantExpression.Value?.ToString() ?? "null";

            case UnaryExpression unaryExpression:
                return GetExpressionValue(unaryExpression.Operand);

            default:
                return $"Unsupported expression: {expression}";
        }
    }

    private static string GetOperator(ExpressionType expressionType)
    {
        return expressionType switch
        {
            ExpressionType.Equal => "==",
            ExpressionType.NotEqual => "!=",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            ExpressionType.AndAlso => "&&",
            ExpressionType.OrElse => "||",
            _ => throw new NotSupportedException($"Unsupported operator: {expressionType}")
        };
    }
}

public class Specification
{
    public string Left { get; set; }
    public string Operator { get; set; }
    public ExpressionType ExpressionType { get; set; }
    public object Right { get; set; }
}
