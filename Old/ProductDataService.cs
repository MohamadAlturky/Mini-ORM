// using Data.Contexts;
// namespace Tests;
// public class ProductDataService
// {
//     public async Task<int> CreateProduct(Product product)
//     {
//         return await CommandsContext.Create<int, Product>(product);
//     }

//     public async Task<Product?> GetProduct(int id)
//     {
//         return await QueriesContext.Get<int, Product>(id);
//     }

//     public async Task<IEnumerable<Product>> GetAllProducts()
//     {
//         return await QueriesContext.List<Product>();
//     }

//     public async Task UpdateProduct(Product product)
//     {
//         await CommandsContext.Update(product);
//     }

//     public async Task DeleteProduct(int id)
//     {
//         await CommandsContext.Delete(new Product { Id = id });
//     }
// }
