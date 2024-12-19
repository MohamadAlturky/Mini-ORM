using System.Data;
using Core.Models;
using Dapper;
using Data.Services;
using Data.Sql;

namespace Data.Contexts;

public static class QueriesContext
{
    ////////////
    ///      ///
    ////////////

    public static async Task<TTable?> Get<TTable>(
        string sql,
        object? parameters = null) where TTable : ITable
    {
        using var connection = DbConnectionFactory.GetSqlServerConnection;
        return await connection.QueryFirstOrDefaultAsync<TTable>(sql, parameters);
    }

    public static async Task<IEnumerable<TTable>> List<TTable>(
        string sql,
        object? parameters = null) where TTable : ITable
    {
        using var connection = DbConnectionFactory.GetSqlServerConnection;
        return await connection.QueryAsync<TTable>(sql, parameters);
    }
    ///
    ///
    ///
    ///
    ///
    ///
    public static async Task<TTable?> Get<TTable>(
        IDbConnection connection,
        string sql,
        object? parameters = null)
    {
        return await connection.QueryFirstOrDefaultAsync<TTable>(sql, parameters);
    }

    public static async Task<IEnumerable<TTable>> List<TTable>(
        IDbConnection connection,
        string sql,
        object? parameters = null)
    {
        return await connection.QueryAsync<TTable>(sql, parameters);
    }

    ///
    ///
    ///
    ///
    ///
    ///
    public static async Task<TTable?> Get<TKey, TTable>(TKey id)
    where TTable : ITable<TKey>, new()
    {
        TTable model = new() { Id = id };
        string sql = SqlQueryBuilder.GenerateSelectByIdQuery(model.TableName, model.ColumnsWithOutId);
        using var connection = DbConnectionFactory.GetSqlServerConnection;
        return await connection.QueryFirstOrDefaultAsync<TTable>(sql, new { Id = model.Id });
    }
    public static async Task<IEnumerable<TTable>> List<TTable>() where TTable : ITable,new()
    {
        TTable model = new();
        string sql = SqlQueryBuilder.GenerateSelectAllQuery(model.TableName, model.ColumnsWithOutId);
        using var connection = DbConnectionFactory.GetSqlServerConnection;
        return await connection.QueryAsync<TTable>(sql);
    }
    ///
    ///
    ///
    ///
    ///
    ///
    public static async Task<TTable?> Get<TKey, TTable>(
        IDbConnection connection,
        TKey id)
    where TTable : ITable<TKey>, new()
    {
        TTable model = new() { Id = id };
        string sql = SqlQueryBuilder.GenerateSelectByIdQuery(model.TableName, model.ColumnsWithOutId);
        return await connection.QueryFirstOrDefaultAsync<TTable>(sql, new { Id = model.Id });
    }
    public static async Task<IEnumerable<TTable>> List<TTable>(IDbConnection connection) where TTable : ITable,new()
    {
        TTable model = new();
        string sql = SqlQueryBuilder.GenerateSelectAllQuery(model.TableName, model.ColumnsWithOutId);
        return await connection.QueryAsync<TTable>(sql);
    }
}