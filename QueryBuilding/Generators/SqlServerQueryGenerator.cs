using System.Linq.Expressions;
using QueryBuilding.Models;

namespace QueryBuilding.Generators;

public static class SqlServerQueryGenerator
{
    public static (string sql, Dictionary<string, object> parameters) GenerateSql(Query query)
    {
        if (query is null)
            throw new ArgumentNullException(nameof(query));

        var sql = $"SELECT {GetColumns(query.Columns)} FROM {query.TableName}";
        var parameters = new Dictionary<string, object>();

        // Add WHERE conditions if any
        if (query.Where.Any())
        {
            sql += $" WHERE {GetWhereConditions(query.Where, parameters)}";
        }

        // Add GROUP BY if any
        if (query.GroupBy.Any())
        {
            sql += $" GROUP BY {GetGroupBy(query.GroupBy)}";
        }

        // Add ORDER BY if any
        if (query.OrderBy.Any())
        {
            sql += $" ORDER BY {GetOrderBy(query.OrderBy)}";
        }
        // Add SKIP and TAKE for pagination
        if (query.Skip.HasValue)
        {
            sql += $" OFFSET @skip ROWS";
            parameters["@skip"] = query.Skip.Value;
        }

        if (query.Take.HasValue)
        {
            sql += $" FETCH NEXT @take ROWS ONLY";
            parameters["@take"] = query.Take.Value;
        }
        return (sql, parameters);
    }
public static (string sql, Dictionary<string, object> parameters) GenerateCountSql(Query query)
    {
        if (query is null)
            throw new ArgumentNullException(nameof(query));

        var sql = $"SELECT COUNT(*) FROM {query.TableName}";
        var parameters = new Dictionary<string, object>();

        // Add WHERE conditions if any
        if (query.Where.Any())
        {
            sql += $" WHERE {GetWhereConditions(query.Where, parameters)}";
        }

        // Add GROUP BY if any
        if (query.GroupBy.Any())
        {
            sql += $" GROUP BY {GetGroupBy(query.GroupBy)}";
        }

        // Add ORDER BY if any
        if (query.OrderBy.Any())
        {
            sql += $" ORDER BY {GetOrderBy(query.OrderBy)}";
        }
        return (sql, parameters);
    }

    private static string GetColumns(List<Column> columns)
    {
        return columns.Count == 0 ? "*" : string.Join(", ", columns.Select(c => $"[{c.Name}]"));
    }

    private static string GetWhereConditions(List<Where> whereConditions, Dictionary<string, object> parameters)
    {
        var conditions = new List<string>();
        
        for (int i = 0; i < whereConditions.Count; i++)
        {
            var w = whereConditions[i];
            var paramName = $"@p{i + 1}"; // Create distinct parameter names
            conditions.Add($"[{w.Column}] {GetSqlOperator(w.ExpressionType)} {paramName}");
            parameters[paramName] = w.Value; // Add parameter to dictionary
        }

        return string.Join(" AND ", conditions);
    }

    private static string GetSqlOperator(ExpressionType expressionType)
    {
        return expressionType switch
        {
            ExpressionType.Equal => "=",
            ExpressionType.NotEqual => "<>",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            ExpressionType.AndAlso => "AND",
            ExpressionType.OrElse => "OR",
            _ => throw new NotSupportedException($"Operator {expressionType} is not supported.")
        };
    }

    private static string GetGroupBy(List<GroupBy> groupBy)
    {
        return string.Join(", ", groupBy.Select(g => $"[{g.ColumnName}]"));
    }

    private static string GetOrderBy(List<OrderBy> orderBy)
    {
        var orderByClauses = orderBy.Select(o => $"[{o.ColumnName}] {o.OrderByType}");
        return string.Join(", ", orderByClauses);
    }
}
