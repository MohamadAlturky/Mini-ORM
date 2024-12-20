using Core.Models.Filtering;

namespace Data.Sql;

public class QueryBuilder
{
    // Method to generate a SELECT For Filter
    public static string GenerateFilterQuery<TViewResult>(
        Query<TViewResult> query) where TViewResult : IViewResult
    {
        if (string.IsNullOrWhiteSpace(query.From) || query.Columns == null || query.Columns.Count == 0)
        {
            throw new ArgumentException("View name and columns must be provided.");
        }

        // Prepare column names for the SELECT clause
        string columnNames = string.Join(", ", query.Columns.Select(c => $"[{c.Column}] AS {c.As ?? c.Column}"));

        // Prepare WHERE clause
        string whereClause = GenerateWhereClause(query.Where);

        // Prepare GROUP BY clause
        string groupByClause = GenerateGroupByClause(query.GroupBy);

        // Prepare ORDER BY clause
        string orderByClause = GenerateOrderByClause(query.OrderBy);

        // Construct the final SQL query
        var sql = $"SELECT {columnNames} FROM {query.From}";

        if (!string.IsNullOrEmpty(whereClause))
        {
            sql += $"\n WHERE {whereClause}";
        }

        if (!string.IsNullOrEmpty(groupByClause))
        {
            sql += $"\n GROUP BY {groupByClause}";
        }

        if (!string.IsNullOrEmpty(orderByClause))
        {
            sql += $"\n ORDER BY {orderByClause}";
        }

        Console.WriteLine(sql);
        return sql;
    }

    // Method to generate a SELECT query with pagination using WITH clause
    public static string GenerateFilterQueryWithPaginationAndWithClause<TViewResult>(
        Query<TViewResult> query) where TViewResult : IViewResult
    {
        if (string.IsNullOrWhiteSpace(query.From) || query.Columns == null || query.Columns.Count == 0)
        {
            throw new ArgumentException("View name and columns must be provided.");
        }

        // Prepare column names for the SELECT clause
        string columnNames = string.Join(", ", query.Columns.Select(c => $"[{c.Column}] AS {c.As ?? c.Column}"));

        // Prepare WHERE clause
        string whereClause = GenerateWhereClause(query.Where);

        // Prepare GROUP BY clause
        string groupByClause = GenerateGroupByClause(query.GroupBy);

        // Prepare ORDER BY clause
        string orderByClause = GenerateOrderByClause(query.OrderBy);

        if (string.IsNullOrEmpty(orderByClause))
        {
            throw new ArgumentException("ORDER BY clause is required for pagination.");
        }

        // Construct the final SQL query using WITH clause
        var sql = $@"
        WITH FilteredData AS (
            SELECT {columnNames}, ROW_NUMBER() OVER (ORDER BY {orderByClause}) AS RowNum
            FROM {query.From}
            {(string.IsNullOrEmpty(whereClause) ? "" : $"WHERE {whereClause}")}
            {(string.IsNullOrEmpty(groupByClause) ? "" : $"GROUP BY {groupByClause}")}
        )
        SELECT {columnNames}
        FROM FilteredData
        WHERE ((@Start IS NULL OR @End IS NULL) OR (RowNum BETWEEN @Start AND @End))";

        Console.WriteLine(sql);
        return sql;
    }
// Method to generate a SELECT query with pagination using WITH clause
    public static string GenerateFilterQueryCount<TViewResult>(
        Query<TViewResult> query) where TViewResult : IViewResult
    {
        if (string.IsNullOrWhiteSpace(query.From) || query.Columns == null || query.Columns.Count == 0)
        {
            throw new ArgumentException("View name and columns must be provided.");
        }

        // Prepare column names for the SELECT clause
        string columnNames = string.Join(", ", query.Columns.Select(c => $"[{c.Column}] AS {c.As ?? c.Column}"));

        // Prepare WHERE clause
        string whereClause = GenerateWhereClause(query.Where);

        // Prepare GROUP BY clause
        string groupByClause = GenerateGroupByClause(query.GroupBy);

        // Prepare ORDER BY clause
        string orderByClause = GenerateOrderByClause(query.OrderBy);

        if (string.IsNullOrEmpty(orderByClause))
        {
            throw new ArgumentException("ORDER BY clause is required for pagination.");
        }

        // Construct the final SQL query using WITH clause
        var sql = $@"
        WITH FilteredData AS (
            SELECT {columnNames}, ROW_NUMBER() OVER (ORDER BY {orderByClause}) AS RowNum
            FROM {query.From}
            {(string.IsNullOrEmpty(whereClause) ? "" : $"WHERE {whereClause}")}
            {(string.IsNullOrEmpty(groupByClause) ? "" : $"GROUP BY {groupByClause}")}
        )
        SELECT COUNT(*)
        FROM FilteredData
        WHERE ((@Start IS NULL OR @End IS NULL) OR (RowNum BETWEEN @Start AND @End))";

        Console.WriteLine(sql);
        return sql;
    }

    private static string GenerateWhereClause(List<Where> whereConditions)
    {
        if (whereConditions == null || whereConditions.Count == 0)
            return string.Empty;

        var conditions = whereConditions.Select(w =>
            $"((@{w.ParameterName ?? w.Column} IS NULL) OR ([{w.Column}] {GetWhereOperation(w)} @{w.ParameterName ?? w.Column}))");

        return string.Join(" AND ", conditions);
    }

    private static string GetWhereOperation(Where where)
    {
        return where.WhereOperation switch
        {
            WhereOperation.Equals => "=",
            WhereOperation.NotEquals => "<>",
            WhereOperation.Bigger => ">",
            WhereOperation.BiggerOrEqual => ">=",
            WhereOperation.Smaller => "<",
            WhereOperation.SmallerOrEqual => "<=",
            WhereOperation.Like => "LIKE",
            WhereOperation.In => "IN",
            _ => throw new NotSupportedException($"The operation '{where.WhereOperation}' is not supported.")
        };
    }

    private static string GenerateGroupByClause(List<GroupBy> groupBys)
    {
        if (groupBys == null || groupBys.Count == 0)
            return string.Empty;

        var groupClauses = groupBys.Select(g => $"[{g.Column}]");
        
        return string.Join(", ", groupClauses);
    }
    private static string GenerateOrderByClause(List<OrderBy> orderBys)
    {
        if (orderBys == null || orderBys.Count == 0)
            return string.Empty;

        var orderClauses = orderBys.Select(o =>
            $"[{o.Column}] {(o.DESC ? "DESC" : "ASC")}");

        return string.Join(", ", orderClauses);
    }
}
