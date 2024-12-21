using System.Linq.Expressions;

namespace QueryBuilding.Models;


public enum OrderByType
{
    ASC,
    DESC
}
public class GroupBy 
{
    public string ColumnName { get; set; } = null!;
}
public class OrderBy
{
    public string ColumnName { get; set; } = string.Empty;
    public OrderByType OrderByType { get; set; }
}
public class Column
{
    public string Name { get; set; } = null!;
}
public class Where
{
    public string Column { get; set; } = string.Empty;
    public ExpressionType ExpressionType { get; set; }
    public object Value { get; set; } = null!;
}

public class Query
{
    public List<Column> Columns { get; set; } = [];
    public string TableName { get; set; } = null!;
    public List<OrderBy> OrderBy { get; set; } = [];
    public List<Where> Where { get; set; } = [];
    public List<GroupBy> GroupBy { get; set; } = [];
    public long? Take { get; set; }
    public long? Skip { get; set; }
}

