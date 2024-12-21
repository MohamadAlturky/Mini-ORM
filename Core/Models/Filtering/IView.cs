using System.Linq.Expressions;

namespace Core.Models.Filtering;

public interface IViewFilterSpecification
{
    string TableName { get; set; }
    List<ColumnClause> Columns { get; set; }
    List<OrderBy> OrderBy { get; set; }
    List<Where> Where { get; set; }
}
public interface IViewFilter
{
    long? Start { get; set; }
    long? End { get; set; }
}

public interface IView { }

public class OrderBy
{
    public string Column { get; set; } = string.Empty;
    public bool DESC { get; set; }
}
public class ColumnClause
{
    public string Column { get; set; } = null!;
    public string? As { get; set; }

}
public class Where
{
    public string Column { get; set; } = string.Empty;
    //public string? ParameterName { get; set; }
    //public WhereOperation WhereOperation { get; set; }
    public ExpressionType ExpressionType { get; set; }
    public object Value { get; set; }
}

public enum WhereOperation
{
    Equals,
    NotEquals,
    Bigger,
    BiggerOrEqual,
    Smaller,
    SmallerOrEqual,
    Like,
    In
}

public interface IViewResult { }
public class Query<TViewResult> 
    where TViewResult : IViewResult
{
    public List<ColumnClause> Columns { get; set; } = [];
    public string From { get; set; } = null!;
    public List<OrderBy> OrderBy { get; set; } = [];
    public List<Where> Where { get; set; } = [];
    public List<GroupBy> GroupBy { get; set; } = [];
}

public class GroupBy 
{
    public string Column { get; set; } = null!;
}
public interface IQueryParameters 
{
    long? Start { get; set; }
    long? End { get; set; }
}