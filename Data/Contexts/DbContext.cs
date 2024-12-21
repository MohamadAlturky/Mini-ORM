// using System.Linq.Expressions;
// using System.Reflection;
// using Core.Models.Filtering;

// namespace Data.Contexts;

// public class Set<T>
// {
//     public static Set<T> Create()
//     {
//         return new Set<T>();
//     } 
//     public readonly List<Expression<Func<T, bool>>> _where = [];
//     public readonly List<Where> _whereClause = [];
//     public readonly List<OrderBy> _orderBy = [];
//     public readonly List<GroupBy> _groupby = [];
//     public readonly string ViewName = typeof(T).Name;
//     public readonly List<string> Columns = _getPropertyNames();
//     public Set<T> Where(Expression<Func<T, bool>> action)
//     {
//         _where.Add(action);
//         foreach (Specification item in QueryBuilding.Contexts.SpecificationGenerator.GetSpecification(action))
//         {
//         _whereClause.Add
//         (
//             new ()
//             {
//                 Column = item.Left,
//                 ExpressionType = item.ExpressionType,
//                 Value = item.Right
//             }            
//         );    
//         }
//         return this;
//     }
//     public Set<T> OrderByASC(Expression<Func<T, object>> action)
//     {
//         _orderBy.Add(new()
//         {
//             Column = _getPropertyName(action),
//             DESC = false
//         });
//         return this;
//     }
//     public Set<T> OrderByDESC(Expression<Func<T, object>> action)
//     {
//         _orderBy.Add(new()
//         {
//             Column = _getPropertyName(action),
//             DESC = true
//         });
//         return this;
//     }
//     public Set<T> GroupBy(Expression<Func<T, object>> action)
//     {
//         _groupby.Add(new()
//         {
//             Column = _getPropertyName(action)
//         });
//         return this;
//     }
//     public static List<string> _getPropertyNames()
//     {
//         // Use reflection to get the properties of type T
//         return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
//                         .Select(prop => prop.Name)
//                         .ToList();
//     }
//     private string _getPropertyName(Expression<Func<T, object>> expression)
//     {
//         // Extract the body of the expression
//         if (expression.Body is MemberExpression member)
//         {
//             return member.Member.Name;
//         }
//         else if (expression.Body is UnaryExpression unary)
//         {
//             // Handle cases where the property is a value type
//             if (unary.Operand is MemberExpression operandMember)
//             {
//                 return operandMember.Member.Name;
//             }
//         }

//         throw new ArgumentException("Invalid expression format");
//     }
// }
