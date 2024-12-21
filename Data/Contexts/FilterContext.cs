// using System.Data;
// using Core.Models;
// using Core.Models.Filtering;
// using Dapper;
// using Data.Services;
// using Data.Sql;

// namespace Data.Contexts;

// public static class FilterContext
// {
//     ///////////////
//     ///
//     ///////////////



//     #region from sql
//     public static async Task<TView?> Get<TView>(
//         string sql,
//         object? parameters = null) where TView : IView
//     {
//         using var connection = DbConnectionFactory.GetSqlServerConnection;
//         return await connection.QueryFirstOrDefaultAsync<TView>(sql, parameters);
//     }

//     public static async Task<IEnumerable<TView>> List<TView>(
//         string sql,
//         object? parameters = null) where TView : IView
//     {
//         using var connection = DbConnectionFactory.GetSqlServerConnection;
//         return await connection.QueryAsync<TView>(sql, parameters);
//     }
//     #endregion
//     ///
//     ///
//     ///
//     ///
//     ///
//     ///
//     #region from sql with connection
//     public static async Task<TView?> Get<TView>(
//         IDbConnection connection,
//         string sql,
//         object? parameters = null)
//     {
//         return await connection.QueryFirstOrDefaultAsync<TView>(sql, parameters);
//     }

//     public static async Task<IEnumerable<TView>> List<TView>(
//         IDbConnection connection,
//         string sql,
//         object? parameters = null)
//     {
//         return await connection.QueryAsync<TView>(sql, parameters);
//     }
//     #endregion
//     ///
//     ///
//     ///
//     ///
//     ///
//     ///

//     #region Directly
//     public static async Task<TView?> Get<TView, TViewFilter>(IViewFilterSpecification viewFilter)
//        where TView : IView
//        where TViewFilter : IViewFilter
//     {
//         string sql = SqlQueryBuilder.GenerateFilterQuery(
//             viewFilter.TableName,
//             viewFilter.Columns,
//             viewFilter.OrderBy,
//             viewFilter.Where);
//         using var connection = DbConnectionFactory.GetSqlServerConnection;
//         return await connection.QueryFirstOrDefaultAsync<TView>(sql, viewFilter);
//     }
//     public static async Task<IEnumerable<TView>> List<TView, TViewFilter, TViewFilterSpecification>(
//         IViewFilterSpecification viewFilterSpecification,
//         IViewFilter viewFilter)
//         where TView : IView
//         where TViewFilter : IViewFilter
//         where TViewFilterSpecification : IViewFilterSpecification
//     {
//         string sql = SqlQueryBuilder.GenerateFilterQuery(
//             viewFilterSpecification.TableName,
//             viewFilterSpecification.Columns,
//             viewFilterSpecification.OrderBy,
//             viewFilterSpecification.Where);
//         using var connection = DbConnectionFactory.GetSqlServerConnection;
//         return await connection.QueryAsync<TView>(sql, viewFilter);
//     }
//     public static async Task<IEnumerable<TView>> Filter<TView, TViewFilter, TViewFilterSpecification>(
//         IViewFilterSpecification viewFilterSpecification,
//         IViewFilter viewFilter)
//         where TView : IView
//         where TViewFilter : IViewFilter
//         where TViewFilterSpecification : IViewFilterSpecification
//     {
//         string sql = SqlQueryBuilder.GenerateFilterQueryWithPaginationAndWithClause(
//             viewFilterSpecification.TableName,
//             viewFilterSpecification.Columns,
//             viewFilterSpecification.OrderBy,
//             viewFilterSpecification.Where);
//         using var connection = DbConnectionFactory.GetSqlServerConnection;
//         return await connection.QueryAsync<TView>(sql, viewFilter);
//     }
//     #endregion
//     ///
//     ///
//     ///
//     ///
//     ///
//     ///
//     // #region Directly With connection
//     // public static async Task<TView?> Get<TKey, TView>(
//     //     IDbConnection connection,
//     //     TKey id)
//     // where TView : IView<TKey>, new()
//     // {
//     //     TView model = new() { Id = id };
//     //     string sql = SqlQueryBuilder.GenerateSelectByIdQuery(model.TableName, model.ColumnsWithOutId);
//     //     return await connection.QueryFirstOrDefaultAsync<TView>(sql, new { Id = model.Id });
//     // }
//     // public static async Task<IEnumerable<TView>> List<TView>(IDbConnection connection) where TView : IView, new()
//     // {
//     //     TView model = new();
//     //     string sql = SqlQueryBuilder.GenerateSelectAllQuery(model.TableName, model.ColumnsWithOutId);
//     //     return await connection.QueryAsync<TView>(sql);
//     // }
//     // #endregion

//     #region Procedure
//     public static async Task<T> ExecuteProcedureSingleAsync<T>(
//         string procedureName,
//         object? parameters = null)
//     {
//         using var connection = DbConnectionFactory.GetSqlServerConnection;
//         return await connection.QuerySingleAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
//     }

//     public static async Task<T> ExecuteProcedureSingleAsync<T>(
//         IDbConnection connection,
//         string procedureName,
//         object? parameters = null,
//         IDbTransaction? transaction = null)
//     {
//         return await connection.QuerySingleAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
//     }
//     public static async Task<IEnumerable<T>> ExecuteProcedureAsync<T>(
//         string procedureName,
//         object? parameters = null)
//     {
//         using var connection = DbConnectionFactory.GetSqlServerConnection;
//         return await connection.QueryAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
//     }

//     public static async Task<IEnumerable<T>> ExecuteProcedureAsync<T>(
//         IDbConnection connection,
//         string procedureName,
//         object? parameters = null,
//         IDbTransaction? transaction = null)
//     {
//         return await connection.QueryAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
//     }
//     #endregion



//     #region Function
//     public static async Task<T> ExecuteFunctionSingleAsync<T>(
//         string functionName,
//         object? parameters = null)
//     {
//         using var connection = DbConnectionFactory.GetSqlServerConnection;
//         var result = await connection.QuerySingleAsync<T>(functionName, parameters, commandType: CommandType.Text);
//         return result;
//     }

//     public static async Task<T> ExecuteFunctionSingleAsync<T>(
//         IDbConnection connection,
//         string functionName,
//         object? parameters = null,
//         IDbTransaction? transaction = null)
//     {
//         var result = await connection.QuerySingleAsync<T>(functionName, parameters, commandType: CommandType.Text, transaction: transaction);
//         return result;
//     }
//     public static async Task<IEnumerable<T>> ExecuteFunctionAsync<T>(
//         string functionName,
//         object? parameters = null)
//     {
//         using var connection = DbConnectionFactory.GetSqlServerConnection;
//         var result = await connection.QueryAsync<T>(functionName, parameters, commandType: CommandType.Text);
//         return result;
//     }

//     public static async Task<IEnumerable<T>> ExecuteFunctionAsync<T>(
//         IDbConnection connection,
//         string functionName,
//         object? parameters = null,
//         IDbTransaction? transaction = null)
//     {
//         var result = await connection.QueryAsync<T>(functionName, parameters, commandType: CommandType.Text, transaction: transaction);
//         return result;
//     }

//     #endregion
// }