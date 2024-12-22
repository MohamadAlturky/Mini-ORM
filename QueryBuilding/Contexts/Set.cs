using System.Data;
using Dapper;
using System.Linq.Expressions;
using QueryBuilding.Generators;
using QueryBuilding.Models;

namespace QueryBuilding.Contexts;

public class Set<T>
{
    private readonly List<Expression<Func<T, bool>>> _where = [];
    private readonly List<Where> _whereClause = [];
    private readonly List<OrderBy> _orderBy = [];
    private int? _count;
    private long? _skip;
    private readonly List<GroupBy> _groupby = [];
    private string _viewName = $"dbo.[{typeof(T).Name}]";
    private readonly List<Column> _columns = PropertyNameGenerator.GetClassPropertyNames<T>()
        .Select(e => new Column() { Name = e }).ToList();
    public Set<T> Where(Expression<Func<T, bool>> action)
    {
        _where.Add(action);
        foreach (Specification item in SpecificationGenerator.GetSpecification(action))
        {
            // System.Console.WriteLine(JsonSerializer.Serialize(item));
            _whereClause.Add
            (
                new()
                {
                    Column = item.Left,
                    ExpressionType = SqlServerQueryGenerator.GetWhereExpressionType(item.ExpressionType),
                    Value = item.Right
                }
            );

        }
        return this;
    }
    public Set<T> OrderBy(Expression<Func<T, object>> action)
    {
        _orderBy.Add(new()
        {
            ColumnName = PropertyNameGenerator.GetPropertyName(action),
            OrderByType = OrderByType.ASC
        });
        return this;
    }
    public Set<T> Like(Expression<Func<T, bool>> action)
    {
        foreach (Specification item in SpecificationGenerator.GetSpecification(action))
        {
            _whereClause.Add
            (
                new()
                {
                    Column = item.Left,
                    ExpressionType = WhereExpressionType.Like,
                    Value = item.Right
                }
            );

        }
        return this;
    }
    public Set<T> OrderByDescending(Expression<Func<T, object>> action)
    {
        _orderBy.Add(new()
        {
            ColumnName = PropertyNameGenerator.GetPropertyName(action),
            OrderByType = OrderByType.DESC

        });
        return this;
    }
    // public Set<T> GroupBy(Expression<Func<T, object>> action)
    // {
    //     _groupby.Add(new()
    //     {
    //         ColumnName = PropertyNameGenerator.GetPropertyName(action)
    //     });
    //     return this;
    // }
    public Set<T> Take(int? count)
    {
        _count = count;
        return this;
    }
    public Set<T> Skip(long? skip)
    {
        _skip = skip;
        return this;
    }
    public (string sql, Dictionary<string, object> parameters) Sql()
    {
        return SqlServerQueryGenerator.GenerateSql(new Query()
        {
            Columns = _columns,
            GroupBy = _groupby,
            OrderBy = _orderBy,
            Skip = _skip,
            TableName = _viewName,
            Take = _count,
            Where = _whereClause
        });
    }
    public Set<T>  From(string sql)
    {
        _viewName = sql;
        return this;
    }
    public async Task<List<T>> ToList(IDbConnection connection)
    {
        var generated = SqlServerQueryGenerator.GenerateSql(new Query()
        {
            Columns = _columns,
            GroupBy = _groupby,
            OrderBy = _orderBy,
            Skip = _skip,
            TableName = _viewName,
            Take = _count,
            Where = _whereClause
        });
        System.Console.WriteLine(generated.sql);
        foreach (var item in generated.parameters)
        {
            System.Console.WriteLine($"{item.Key} = {item.Value}");
        }
        var result = await connection.QueryAsync<T>(generated.sql, generated.parameters);
        return result.ToList();
    }
    public async Task<long> Count(IDbConnection connection)
    {
        var generated = SqlServerQueryGenerator.GenerateCountSql(new Query()
        {
            Columns = _columns,
            GroupBy = _groupby,
            OrderBy = _orderBy,
            Skip = _skip,
            TableName = _viewName,
            Take = _count,
            Where = _whereClause
        });
        System.Console.WriteLine(generated.sql);
        foreach (var item in generated.parameters)
        {
            System.Console.WriteLine($"{item.Key} = {item.Value}");
        }
        var result = await connection.QueryFirstOrDefaultAsync<long>(generated.sql, generated.parameters);
        return result;
    }
    public static Set<T> Query()
    {
        return new Set<T>();
    }
}
