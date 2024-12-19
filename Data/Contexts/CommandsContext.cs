using System.Data;
using Core.Models;
using Dapper;
using Data.Services;
using Data.Sql;

namespace Data.Contexts;

public static class CommandsContext
{
    ////////////
    ///      ///
    ////////////

    public static async Task<TKey?> Create<TKey>(
            string sql,
            object? parameters = null)
    {
        using var connection = DbConnectionFactory.GetSqlServerConnection;
        return await connection.ExecuteScalarAsync<TKey>(sql, parameters);
    }

    public static async Task Update(
        string sql,
        object? parameters = null)
    {
        using var connection = DbConnectionFactory.GetSqlServerConnection;
        await connection.ExecuteAsync(sql, parameters);
    }

    public static async Task Delete(
        string sql,
        object? parameters = null)
    {
        using var connection = DbConnectionFactory.GetSqlServerConnection;
        await connection.ExecuteAsync(sql, parameters);
    }
    ///
    ///
    ///
    ///
    ///
    ///
    public static async Task<TKey?> Create<TKey>(
        IDbConnection connection,
        string sql,
        object? parameters = null,
        IDbTransaction? transaction = null)
    {
        return await connection.ExecuteScalarAsync<TKey>(sql, parameters, transaction);
    }

    public static async Task Update(
        IDbConnection connection,
        string sql,
        object? parameters = null,
        IDbTransaction? transaction = null)
    {
        await connection.ExecuteAsync(sql, parameters, transaction);
    }

    public static async Task Delete(
        IDbConnection connection,
        string sql,
        object? parameters = null,
        IDbTransaction? transaction = null)
    {
        await connection.ExecuteAsync(sql, parameters, transaction);
    }

    ///
    ///
    ///
    ///
    ///
    ///
    public static async Task<TKey?> Create<TKey, TTable>(TTable model) where TTable : ITable<TKey>
    {
        string sql = SqlQueryBuilder.GenerateInsertQuery(model.TableName, model.ColumnsWithOutId);
        using var connection = DbConnectionFactory.GetSqlServerConnection;
        return await connection.ExecuteScalarAsync<TKey>(sql, model);
    }

    public static async Task Update<TTable>(TTable model) where TTable : ITable
    {
        string sql = SqlQueryBuilder.GenerateUpdateQuery(model.TableName, model.ColumnsWithOutId);
        using var connection = DbConnectionFactory.GetSqlServerConnection;
        await connection.ExecuteAsync(sql, model);
    }

    public static async Task Delete<TTable>(TTable model) where TTable : ITable
    {
        string sql = SqlQueryBuilder.GenerateDeleteQuery(model.TableName);
        using var connection = DbConnectionFactory.GetSqlServerConnection;
        await connection.ExecuteAsync(sql, model);
    }
    ///
    ///
    ///
    ///
    ///
    ///
    public static async Task<TKey?> Create<TKey, TTable>(
        IDbConnection connection,
        TTable model,
        IDbTransaction? transaction = null) where TTable : ITable<TKey>
    {
        string sql = SqlQueryBuilder.GenerateInsertQuery(model.TableName, model.ColumnsWithOutId);
        return await connection.ExecuteScalarAsync<TKey>(sql, model, transaction);
    }

    public static async Task Update<TTable>(
        IDbConnection connection,
        TTable model,
        IDbTransaction? transaction = null) where TTable : ITable
    {
        string sql = SqlQueryBuilder.GenerateUpdateQuery(model.TableName, model.ColumnsWithOutId);
        await connection.ExecuteAsync(sql, model, transaction);
    }

    public static async Task Delete<TTable>(
        IDbConnection connection,
        TTable model,
        IDbTransaction? transaction = null) where TTable : ITable
    {
        string sql = SqlQueryBuilder.GenerateDeleteQuery(model.TableName);
        await connection.ExecuteAsync(sql, model, transaction);
    }
}