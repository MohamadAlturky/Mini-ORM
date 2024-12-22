using System.Text;
using Microsoft.Data.SqlClient;
using QueryBuilding.Contexts;

using var connection = new SqlConnection("Server=DESKTOP-OO326C9\\SQLEXPRESS01;Database=Test;User id=sa; Password=A@123456789; MultipleActiveResultSets=true;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Trusted_Connection=false");
connection.Open();

int? id = 2;
id = null;
byte? category = 1;
// category = null;
string? name = "%a%";
// name = null;

var query = DataTable<Product>
        .Query()
        .Where
        (
            e => e.Id == id && 
            e.Category == category 
        )
        .Like(e=>e.Name == name)
        .OrderBy(e=>e.Id)
        .OrderByDescending(e=>e.Name);

List<Product> result = await query.ToList(connection);

var jsonResult = SampleClass.ConvertToJson(result);
Console.WriteLine(jsonResult);

// foreach (var item in result)
// {
//     System.Console.WriteLine($"");
//     System.Console.WriteLine($"{nameof(item.Name)} = {item.Name}");
//     System.Console.WriteLine($"{nameof(item.Description)} = {item.Description}");
//     System.Console.WriteLine($"{nameof(item.Id)} = {item.Id}");
//     System.Console.WriteLine($"{nameof(item.Category)} = {item.Category}");
//     System.Console.WriteLine($"");

// }

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Category { get; set; }
}

public static class SampleClass
{

    public static string ConvertToJson(List<Product> items)
    {
        var sb = new StringBuilder();
        sb.AppendLine("[");

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            sb.AppendLine("  {");
            sb.AppendLine($"    \"Name\": \"{EscapeJson(item.Name)}\",");
            sb.AppendLine($"    \"Description\": \"{EscapeJson(item.Description)}\",");
            sb.AppendLine($"    \"Id\": {item.Id},");
            sb.AppendLine($"    \"Category\": \"{EscapeJson(item.Category.ToString())}\"");
            sb.Append("  }");

            if (i < items.Count - 1)
            {
                sb.AppendLine(",");
            }
        }

        sb.AppendLine();
        sb.AppendLine("]");

        return sb.ToString();
    }

    private static string EscapeJson(string value)
    {
        if (value == null) return "null";

        return value.Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\b", "\\b")
                    .Replace("\f", "\\f")
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r")
                    .Replace("\t", "\\t");
    }
}

public static class DataTable<T>
{
    public static Set<T> Query()
    {
        return new Set<T>();
    }
}