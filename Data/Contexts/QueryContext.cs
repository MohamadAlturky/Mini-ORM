// using System.Data;
// using Core.Models;
// using Core.Models.Filtering;
// using Dapper;
// using Data.Services;
// using Data.Sql;


// namespace Data.Contexts;

// public static class QueryContext
// {
//     ///////////////
//     ///////////////
//     ///////////////

//     #region Directly
//     public static async Task<TViewResult?> Get<TViewResult>(
//         Query<TViewResult> query,
//         IQueryParameters parameters)
//        where TViewResult : IViewResult
//     {
//         string sql = QueryBuilder.GenerateFilterQuery(query);
//         using var connection = DbConnectionFactory.GetSqlServerConnection;
//         return await connection.QueryFirstOrDefaultAsync<TViewResult>(sql, parameters);
//     }

//     public static async Task<PaginatedResult<TViewResult>> List<TViewResult>(
//         Query<TViewResult> query,
//         IQueryParameters parameters)
//        where TViewResult : IViewResult
//     {
//         string sql = QueryBuilder.GenerateFilterQueryWithPaginationAndWithClause(query);
//         string countSql = QueryBuilder.GenerateFilterQueryCount(query);

//         using var connection = DbConnectionFactory.GetSqlServerConnection;

//         var items = await connection.QueryAsync<TViewResult>(sql, parameters);
//         var count = await connection.QueryFirstOrDefaultAsync<long>(countSql, parameters);

//         return new PaginatedResult<TViewResult>
//         {
//             Items = items.ToArray(),
//             TotalCount = count
//         };
//     }
//     #endregion

//     #region With Connection
//     public static async Task<TViewResult?> Get<TViewResult>(
//         IDbConnection connection,
//         Query<TViewResult> query,
//         IQueryParameters parameters,
//         IDbTransaction? transaction = null)
//        where TViewResult : IViewResult
//     {
//         string sql = QueryBuilder.GenerateFilterQuery(query);
//         return await connection.QueryFirstOrDefaultAsync<TViewResult>(sql, parameters, transaction);
//     }

//     public static async Task<PaginatedResult<TViewResult>> List<TViewResult>(
//         Query<TViewResult> query,
//         IDbConnection connection,
//         IQueryParameters parameters,
//         IDbTransaction? transaction = null)
//        where TViewResult : IViewResult
//     {
//         string sql = QueryBuilder.GenerateFilterQueryWithPaginationAndWithClause(query);
//         string countSql = QueryBuilder.GenerateFilterQueryCount(query);

//         var items = await connection.QueryAsync<TViewResult>(sql, parameters);
//         var count = await connection.QueryFirstOrDefaultAsync<long>(countSql, parameters, transaction);

//         return new PaginatedResult<TViewResult>
//         {
//             Items = items.ToArray(),
//             TotalCount = count
//         };
//     }
//     #endregion
// }