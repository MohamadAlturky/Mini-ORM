using System.Data;
using Core.Models;
using Dapper;
using Data.Services;
using Data.Sql;

namespace Data.Contexts;

public static class QueriesContext
{
    ///////////////
    ///
    ///////////////



    #region from sql
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
    #endregion
    ///
    ///
    ///
    ///
    ///
    ///
    #region from sql with connection
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
    #endregion
    ///
    ///
    ///
    ///
    ///
    ///

    #region Directly
    public static async Task<TTable?> Get<TKey, TTable>(TKey id)
    where TTable : ITable<TKey>, new()
    {
        TTable model = new() { Id = id };
        string sql = SqlQueryBuilder.GenerateSelectByIdQuery(model.TableName, model.ColumnsWithOutId);
        using var connection = DbConnectionFactory.GetSqlServerConnection;
        return await connection.QueryFirstOrDefaultAsync<TTable>(sql, new { Id = model.Id });
    }
    public static async Task<IEnumerable<TTable>> List<TTable>() where TTable : ITable, new()
    {
        TTable model = new();
        string sql = SqlQueryBuilder.GenerateSelectAllQuery(model.TableName, model.ColumnsWithOutId);
        using var connection = DbConnectionFactory.GetSqlServerConnection;
        return await connection.QueryAsync<TTable>(sql);
    }
    #endregion
    ///
    ///
    ///
    ///
    ///
    ///
    #region Directly With connection
    public static async Task<TTable?> Get<TKey, TTable>(
        IDbConnection connection,
        TKey id)
    where TTable : ITable<TKey>, new()
    {
        TTable model = new() { Id = id };
        string sql = SqlQueryBuilder.GenerateSelectByIdQuery(model.TableName, model.ColumnsWithOutId);
        return await connection.QueryFirstOrDefaultAsync<TTable>(sql, new { Id = model.Id });
    }
    public static async Task<IEnumerable<TTable>> List<TTable>(IDbConnection connection) where TTable : ITable, new()
    {
        TTable model = new();
        string sql = SqlQueryBuilder.GenerateSelectAllQuery(model.TableName, model.ColumnsWithOutId);
        return await connection.QueryAsync<TTable>(sql);
    }
    #endregion

    #region Procedure
    public static async Task<T> ExecuteProcedureSingleAsync<T>(
        string procedureName,
        object? parameters = null)
    {
        using var connection = DbConnectionFactory.GetSqlServerConnection;
        return await connection.QuerySingleAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
    }

    public static async Task<T> ExecuteProcedureSingleAsync<T>(
        IDbConnection connection,
        string procedureName,
        object? parameters = null,
        IDbTransaction? transaction = null)
    {
        return await connection.QuerySingleAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
    }
    public static async Task<IEnumerable<T>> ExecuteProcedureAsync<T>(
        string procedureName,
        object? parameters = null)
    {
        using var connection = DbConnectionFactory.GetSqlServerConnection;
        return await connection.QueryAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
    }

    public static async Task<IEnumerable<T>> ExecuteProcedureAsync<T>(
        IDbConnection connection,
        string procedureName,
        object? parameters = null,
        IDbTransaction? transaction = null)
    {
        return await connection.QueryAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
    }
    #endregion



    #region Function
    public static async Task<T> ExecuteFunctionSingleAsync<T>(
        string functionName,
        object? parameters = null)
    {
        using var connection = DbConnectionFactory.GetSqlServerConnection;
        var result = await connection.QuerySingleAsync<T>(functionName, parameters, commandType: CommandType.Text);
        return result;
    }

    public static async Task<T> ExecuteFunctionSingleAsync<T>(
        IDbConnection connection,
        string functionName,
        object? parameters = null,
        IDbTransaction? transaction = null)
    {
        var result = await connection.QuerySingleAsync<T>(functionName, parameters, commandType: CommandType.Text, transaction: transaction);
        return result;
    }
    public static async Task<IEnumerable<T>> ExecuteFunctionAsync<T>(
        string functionName,
        object? parameters = null)
    {
        using var connection = DbConnectionFactory.GetSqlServerConnection;
        var result = await connection.QueryAsync<T>(functionName, parameters, commandType: CommandType.Text);
        return result;
    }

    public static async Task<IEnumerable<T>> ExecuteFunctionAsync<T>(
        IDbConnection connection,
        string functionName,
        object? parameters = null,
        IDbTransaction? transaction = null)
    {
        var result = await connection.QueryAsync<T>(functionName, parameters, commandType: CommandType.Text, transaction: transaction);
        return result;
    }

    #endregion
}