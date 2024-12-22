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
            var whereConditions = GetWhereConditions(query.Where, parameters);
            if (whereConditions != string.Empty)
            {

                sql += $" WHERE {whereConditions}";
            }
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
            var whereConditions = GetWhereConditions(query.Where, parameters);
            if (whereConditions != string.Empty)
            {

                sql += $" WHERE {whereConditions}";
            }
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
        List<string> conditions = [];

        for (int i = 0; i < whereConditions.Count; i++)
        {
            var w = whereConditions[i];
            if (w.Value is not null)
            {
                var paramName = $"@p{i + 1}"; // Create distinct parameter names
                conditions.Add($"[{w.Column}] {GetSqlOperator(w.ExpressionType)} {paramName}");
                parameters[paramName] = w.Value; // Add parameter to dictionary
            }
        }
        if (conditions.Count == 0)
        {
            return string.Empty;
        }
        return string.Join(" AND ", conditions);
    }

    private static string GetSqlOperator(WhereExpressionType expressionType)
    {
        return expressionType switch
        {
            WhereExpressionType.Equal => "=",
            WhereExpressionType.NotEqual => "<>",
            WhereExpressionType.GreaterThan => ">",
            WhereExpressionType.GreaterThanOrEqual => ">=",
            WhereExpressionType.LessThan => "<",
            WhereExpressionType.LessThanOrEqual => "<=",
            WhereExpressionType.AndAlso => "AND",
            WhereExpressionType.OrElse => "OR",
            WhereExpressionType.Like => "LIKE",
            _ => throw new NotSupportedException($"Operator {expressionType} is not supported.")
        };
    }
    public static WhereExpressionType GetWhereExpressionType(ExpressionType expressionType, bool isLike = false)
    {
        if(isLike)
        {
            return WhereExpressionType.Like;
        }
        return expressionType switch
        {
            ExpressionType.Equal => WhereExpressionType.Equal,
            ExpressionType.NotEqual => WhereExpressionType.NotEqual,
            ExpressionType.GreaterThan => WhereExpressionType.GreaterThan,
            ExpressionType.GreaterThanOrEqual => WhereExpressionType.GreaterThanOrEqual,
            ExpressionType.LessThan => WhereExpressionType.LessThan,
            ExpressionType.LessThanOrEqual => WhereExpressionType.LessThanOrEqual,
            ExpressionType.AndAlso => WhereExpressionType.AndAlso,
            ExpressionType.OrElse => WhereExpressionType.OrElse,
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
