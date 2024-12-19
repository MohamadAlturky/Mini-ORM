using Core.Models.Filtering;

namespace Data.Sql;

public class SqlQueryBuilder
{
    // Method to generate an INSERT query
    public static string GenerateInsertQuery(string tableName, List<string> columns)
    {
        if (string.IsNullOrWhiteSpace(tableName) || columns == null || columns.Count == 0)
        {
            throw new ArgumentException("Table name and columns must be provided.");
        }

        string columnNames = string.Join(", ", columns.ConvertAll(c => "[" + c + "]"));
        string parameterPlaceholders = string.Join(", ", columns.ConvertAll(c => "@" + c));
        var sql = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterPlaceholders}) SELECT CAST(SCOPE_IDENTITY() AS INT);";
        Console.WriteLine(sql);
        return sql;
    }

    // Method to generate an UPDATE query
    public static string GenerateUpdateQuery(string tableName, List<string> columns)
    {
        string idColumn = "[Id]";
        if (string.IsNullOrWhiteSpace(tableName) || columns == null || columns.Count == 0 || string.IsNullOrWhiteSpace(idColumn))
        {
            throw new ArgumentException("Table name, columns, and ID column must be provided.");
        }

        // Constructing the SET clause
        var setClauses = new List<string>();
        foreach (var column in columns)
        {
            setClauses.Add($"[{column}] = @{column}");
        }

        string setClause = string.Join(", ", setClauses);
        var sql = $"UPDATE {tableName} SET {setClause} WHERE {idColumn} = @Id;";
        Console.WriteLine(sql);
        return sql;
    }

    // Method to generate a DELETE query
    public static string GenerateDeleteQuery(string tableName)
    {
        string idColumn = "[Id]";
        if (string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(idColumn))
        {
            throw new ArgumentException("Table name and ID column must be provided.");
        }
        var sql = $"DELETE FROM {tableName} WHERE {idColumn} = @Id;";
        Console.WriteLine(sql);
        return sql;
    }
    // Method to generate a SELECT ALL query
    public static string GenerateSelectAllQuery(string tableName, List<string> columns)
    {
        string idColumn = "[Id]";

        if (string.IsNullOrWhiteSpace(tableName) || columns == null || columns.Count == 0)
        {
            throw new ArgumentException("Table name and columns must be provided.");
        }
        string columnNames = string.Join(", ", columns.ConvertAll(c => "[" + c + "]"));

        var sql = $"SELECT {idColumn}, {columnNames} FROM {tableName};";
        Console.WriteLine(sql);
        return sql;
    }

    // Method to generate a SELECT BY ID query
    public static string GenerateSelectByIdQuery(string tableName, List<string> columns)
    {
        string idColumn = "[Id]";
        if (string.IsNullOrWhiteSpace(tableName) || columns == null || columns.Count == 0)
        {
            throw new ArgumentException("Table name and columns must be provided.");
        }
        string columnNames = string.Join(", ", columns.ConvertAll(c => "[" + c + "]"));

        var sql = $"SELECT {idColumn}, {columnNames} FROM {tableName} WHERE {idColumn} = @Id;";
        Console.WriteLine(sql);
        return sql;
    }

    // Method to generate a SELECT For Filter
    public static string GenerateFilterQuery(
        string viewName,
        List<string> columns,
        List<OrderBy> orderBy,
        List<Where> where)
    {
        if (string.IsNullOrWhiteSpace(viewName) || columns == null || columns.Count == 0)
        {
            throw new ArgumentException("View name and columns must be provided.");
        }

        // Prepare column names for the SELECT clause
        string columnNames = string.Join(", ", columns.Select(c => $"[{c}]"));

        // Prepare WHERE clause
        string whereClause = GenerateWhereClause(where);

        // Prepare ORDER BY clause
        string orderByClause = GenerateOrderByClause(orderBy);

        // Construct the final SQL query
        var sql = $"SELECT {columnNames} FROM [{viewName}]";

        if (!string.IsNullOrEmpty(whereClause))
        {
            sql += $"\n WHERE {whereClause}";
        }

        if (!string.IsNullOrEmpty(orderByClause))
        {
            sql += $"\n ORDER BY {orderByClause}";
        }

        Console.WriteLine(sql);
        return sql;
    }

    private static string GenerateWhereClause(List<Where> whereConditions)
    {
        if (whereConditions == null || whereConditions.Count == 0)
            return string.Empty;

        var conditions = whereConditions.Select(w =>
            $"((@{w.ParameterName??w.Column} IS NULL) OR ([{w.Column}] {GetWhereOperation(w)} @{w.ParameterName??w.Column}))");

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


    private static string GenerateOrderByClause(List<OrderBy> orderBys)
    {
        if (orderBys == null || orderBys.Count == 0)
            return string.Empty;

        var orderClauses = orderBys.Select(o =>
            $"[{o.Column}] {(o.DESC ? "DESC" : "ASC")}");

        return string.Join(", ", orderClauses);
    }
}
