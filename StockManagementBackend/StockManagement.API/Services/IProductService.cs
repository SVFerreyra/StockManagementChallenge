using StockManagement.API.Models;
using StockManagement.API.Models.DTOs;

namespace StockManagement.API.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
        Task<ProductResponse?> GetProductByIdAsync(int id);
        Task<ProductResponse> CreateProductAsync(ProductRequest request);
        Task<ProductResponse?> UpdateProductAsync(int id, ProductRequest request);
        Task<bool> DeleteProductAsync(int id);
        Task<FilteredProductsResponse> GetFilteredProductsAsync(int budget);
    }
}