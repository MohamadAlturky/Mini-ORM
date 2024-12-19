namespace Core.Models;

public interface ITable<TKey> : ITable
{
    TKey Id { get; set; }
}

public interface ITable
{
    string TableName { get; set; }
    List<string> ColumnsWithOutId { get; set; }
}