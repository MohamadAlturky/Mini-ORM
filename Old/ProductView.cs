// using Core.Models.Filtering;

// namespace Tests;

// public class ProductView : IView
// {
//     public int Id { get; set; }
//     public string Name { get; set; } = string.Empty;
//     public string Description { get; set; } = string.Empty;
//     public int Category { get; set; }
// }

// public class ProductFilterSpecification : IViewFilterSpecification
// {
//     public string TableName { get; set; } = nameof(Product); // The name of the table/view.
//     public List<ColumnClause> Columns { get; set; } = new List<ColumnClause>
//         {
//             new() { Column = nameof(Product.Id), As = nameof(Product.Id)},
//             new() { Column = nameof(Product.Name), As = nameof(Product.Name)},
//             new() { Column = nameof(Product.Description), As = nameof(Product.Description)},
//             new() { Column = nameof(Product.Category)}
//         }; // Columns available for selection.

//     public List<OrderBy> OrderBy { get; set; }
//     = [
//         new()
//         {
//             Column = nameof(Product.Id),
//             DESC = true
//         },
//         new()
//         {
//             Column = nameof(Product.Category),
//             DESC = false
//         },
//     ]; // Sorting criteria.

//     public List<Where> Where { get; set; }
//     = [
//         new()
//         {
//             Column = nameof(Product.Category),
//             ParameterName = nameof(ProductFilter.Category),
//             WhereOperation = WhereOperation.BiggerOrEqual
//         },
//         new()
//         {
//             Column = nameof(Product.Name),
//             ParameterName = nameof(ProductFilter.NameLike),
//             WhereOperation = WhereOperation.Like
//         },
//     ]; // Filtering conditions.

//     // You can add methods to build queries based on these properties if needed.
// }
// public class ProductFilter : IViewFilter
// {
//     public int? Category { get; set; }
//     public string? NameLike { get; set; }
//     public long? Start { get; set; }
//     public long? End { get; set; }
// }