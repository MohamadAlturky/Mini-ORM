namespace Core.Models.Filtering;

public interface IViewFilterSpecification
{
    string TableName { get; set; }
    List<string> Columns { get; set; }
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

public class Where
{
    public string Column { get; set; } = string.Empty;
    public string? ParameterName { get; set; }
    public WhereOperation WhereOperation { get; set; }
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