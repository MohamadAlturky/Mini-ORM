using System.Linq.Expressions;
using System.Reflection;

namespace QueryBuilding.Generators;

public static class PropertyNameGenerator
{
    public static List<string> GetClassPropertyNames<T>()
    {
        return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Select(prop => prop.Name)
                        .ToList();
    }
    public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
    {
        if (expression.Body is MemberExpression member)
        {
            return member.Member.Name;
        }
        else if (expression.Body is UnaryExpression unary)
        {
            if (unary.Operand is MemberExpression operandMember)
            {
                return operandMember.Member.Name;
            }
        }
        throw new ArgumentException("Invalid expression format");
    }
}
