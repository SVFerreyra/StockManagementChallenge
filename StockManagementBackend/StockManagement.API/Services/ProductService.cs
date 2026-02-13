using Microsoft.EntityFrameworkCore;
using StockManagement.API.Data;
using StockManagement.API.Models;
using StockManagement.API.Models.DTOs;
using System.Globalization;

namespace StockManagement.API.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ApplicationDbContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _context.Products
                .OrderByDescending(p => p.LoadDate)
                .ToListAsync();

            return products.Select(MapToResponse);
        }

        public async Task<ProductResponse?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product == null ? null : MapToResponse(product);
        }

        public async Task<ProductResponse> CreateProductAsync(ProductRequest request)
        {
            var product = new Product
            {
                Price = request.Price,
                LoadDate = request.LoadDate,
                Category = request.Category
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Producto creado con ID: {ProductId}", product.Id);
            return MapToResponse(product);
        }

        public async Task<ProductResponse?> UpdateProductAsync(int id, ProductRequest request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return null;

            product.Price = request.Price;
            product.LoadDate = request.LoadDate;
            product.Category = request.Category;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Producto actualizado con ID: {ProductId}", product.Id);
            return MapToResponse(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Producto eliminado con ID: {ProductId}", id);
            return true;
        }

        public async Task<FilteredProductsResponse> GetFilteredProductsAsync(int budget)
        {
            // Obtener todos los productos por categoría
            var productosUno = await _context.Products
                .Where(p => p.Category == "PRODUNO" && p.Price <= budget)
                .OrderByDescending(p => p.Price)
                .ToListAsync();

            var productosDos = await _context.Products
                .Where(p => p.Category == "PRODDOS" && p.Price <= budget)
                .OrderByDescending(p => p.Price)
                .ToListAsync();

            // Si no hay productos de alguna categoría
            if (!productosUno.Any() || !productosDos.Any())
            {
                return new FilteredProductsResponse
                {
                    Message = "No se encontraron productos de ambas categorías que cumplan con el presupuesto."
                };
            }

            // Encontrar la mejor combinación
            Product? bestProductOne = null;
            Product? bestProductTwo = null;
            decimal bestTotal = 0;

            foreach (var prodUno in productosUno)
            {
                foreach (var prodDos in productosDos)
                {
                    decimal total = prodUno.Price + prodDos.Price;

                    // Debe ser menor o igual al presupuesto
                    if (total <= budget)
                    {
                        // Buscar la combinación más cercana al presupuesto
                        if (total > bestTotal)
                        {
                            bestTotal = total;
                            bestProductOne = prodUno;
                            bestProductTwo = prodDos;
                        }
                    }
                }
            }

            if (bestProductOne == null || bestProductTwo == null)
            {
                return new FilteredProductsResponse
                {
                    Message = "No se encontró una combinación de productos que no exceda el presupuesto."
                };
            }

            _logger.LogInformation(
                "Productos filtrados - Budget: {Budget}, Total: {Total}, PRODUNO: {ProdUno}, PRODDOS: {ProdDos}",
                budget, bestTotal, bestProductOne.Id, bestProductTwo.Id);

            return new FilteredProductsResponse
            {
                ProductOne = MapToResponse(bestProductOne),
                ProductTwo = MapToResponse(bestProductTwo),
                Total = bestTotal,
                Message = "Productos encontrados exitosamente."
            };
        }

        private ProductResponse MapToResponse(Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Price = product.Price,
                LoadDate = product.LoadDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Category = product.Category
            };
        }
    }
}