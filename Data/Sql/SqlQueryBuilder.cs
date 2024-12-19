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
}
