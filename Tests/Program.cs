using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;
using Data.Contexts;

namespace d
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Using the Set<Product>
            var productSet = new Set<Product>()
                .Where(p => p.Price > 1.00m && p.Name == "s") // Filter products with price greater than 1.00
                .OrderByASC(p => p.Name); // Order by Name ascending

            // Print results
            PrintSetProperties(productSet);
        }

        static void PrintSetProperties<T>(Set<T> set)
        {
            Console.WriteLine($"View Name: {set.ViewName}");
            Console.WriteLine("Columns: " + string.Join(", ", set.Columns));

            Console.WriteLine("Filters:");
            foreach (var whereClause in set._where)
            {
                Console.WriteLine($" - {whereClause}");
            }
            foreach (var whereClause in set._whereClause)
            {
                Console.WriteLine($" - ParameterName = {whereClause.ParameterName}");
                Console.WriteLine($" - Column = {whereClause.Column}");
                Console.WriteLine($" - ExpressionType = {whereClause.ExpressionType}");
            }

            Console.WriteLine("Order By:");
            foreach (var order in set._orderBy)
            {
                string direction = order.DESC ? "DESC" : "ASC";
                Console.WriteLine($" - {order.Column} {direction}");
            }
            Console.WriteLine("Group By:");

            foreach (var order in set._groupby)
            {
                Console.WriteLine($" - {order.Column} ");
            }

            // Note: You may want to implement similar methods for GroupBy if needed.
        }
    }
}
