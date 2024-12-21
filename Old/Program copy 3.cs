// using System;
// using System.Linq.Expressions;

// public static class PropertyNameExtractor
// {
//     /// <summary>
//     /// Gets the name of the property from the provided lambda expression.
//     /// </summary>
//     /// <typeparam name="T">The type of the entity.</typeparam>
//     /// <param name="expression">The lambda expression representing the property.</param>
//     /// <returns>The name of the property as a string.</returns>
//     public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
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

// public class Person
// {
//     public string Name { get; set; }
//     public string Name1 { get; set; }
//     public string Name2{ get; set; }
//     public string Name3 { get; set; }
//     public int Id { get; set; }
// }

// // class Program
// // {
// //     static void Main()
// //     {
// //         string propertyName = PropertyNameExtractor.GetPropertyName<Person>(p => p.Name);
// //         Console.WriteLine(propertyName); // Output: Name
// //         propertyName = PropertyNameExtractor.GetPropertyName<Person>(p => p.Name1);
// //         Console.WriteLine(propertyName); // Output: Name
// //         propertyName = PropertyNameExtractor.GetPropertyName<Person>(p => p.Name2);
// //         Console.WriteLine(propertyName); // Output: Name
// //         propertyName = PropertyNameExtractor.GetPropertyName<Person>(p => p.Id);
// //         Console.WriteLine(propertyName); // Output: Name
// //     }
// // }
