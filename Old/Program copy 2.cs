// using System;
// using System.Collections.Generic;
// using System.Linq.Expressions;
// using System.Reflection;

// public class Entity
// {
//     public int? Id { get; set; }
//     public string Name { get; set; }
// }

// public static class ColumnExtractor
// {
//     public static List<string> GetPropertyNames<T>()
//     {
//         // Use reflection to get the properties of type T
//         return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
//                         .Select(prop => prop.Name)
//                         .ToList();
//     }

// }

// // class Program
// // {
// //     public class SampleClass
// //     {
// //         public int Id { get; set; }
// //         public string Name { get; set; }
// //     }

// //     static void Main()
// //     {
// //         var propertyNames = ColumnExtractor.GetPropertyNames<SampleClass>();
// //         Console.WriteLine(string.Join(", ", propertyNames));

// //     }
// // }
