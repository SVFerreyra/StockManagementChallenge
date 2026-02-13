using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StockManagement.API.Data;
using StockManagement.API.Models.DTOs;
using StockManagement.API.Models;
using StockManagement.API.Services;
using Xunit;

namespace StockManagement.Tests.Services
{
    public class ProductServiceTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateProduct_ShouldAddProductToDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var logger = Mock.Of<ILogger<ProductService>>();
            var service = new ProductService(context, logger);

            var request = new ProductRequest
            {
                Price = 100.50m,
                LoadDate = new DateTime(2026, 2, 12),
                Category = "PRODUNO"
            };

            // Act
            var result = await service.CreateProductAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100.50m, result.Price);
            Assert.Equal("12/02/2026", result.LoadDate);
            Assert.Equal("PRODUNO", result.Category);

            var productInDb = await context.Products.FirstOrDefaultAsync();
            Assert.NotNull(productInDb);
        }

        [Fact]
        public async Task GetFilteredProducts_ShouldReturnBestCombination()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var logger = Mock.Of<ILogger<ProductService>>();
            var service = new ProductService(context, logger);

            // Agregar productos de prueba
            context.Products.AddRange(
                new Product { Id = 1, Price = 10, LoadDate = DateTime.Now, Category = "PRODDOS" },
                new Product { Id = 2, Price = 60, LoadDate = DateTime.Now, Category = "PRODUNO" },
                new Product { Id = 3, Price = 5, LoadDate = DateTime.Now, Category = "PRODDOS" },
                new Product { Id = 4, Price = 5, LoadDate = DateTime.Now, Category = "PRODUNO" },
                new Product { Id = 5, Price = 15, LoadDate = DateTime.Now, Category = "PRODDOS" }
            );
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetFilteredProductsAsync(70);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ProductOne);
            Assert.NotNull(result.ProductTwo);
            Assert.Equal(60, result.ProductOne.Price);
            Assert.Equal(10, result.ProductTwo.Price);
            Assert.Equal(70, result.Total);
        }

        [Fact]
        public async Task GetFilteredProducts_WithInsufficientBudget_ShouldReturnMessage()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var logger = Mock.Of<ILogger<ProductService>>();
            var service = new ProductService(context, logger);

            context.Products.AddRange(
                new Product { Id = 1, Price = 100, LoadDate = DateTime.Now, Category = "PRODDOS" },
                new Product { Id = 2, Price = 100, LoadDate = DateTime.Now, Category = "PRODUNO" }
            );
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetFilteredProductsAsync(50);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.ProductOne);
            Assert.Null(result.ProductTwo);
            Assert.Contains("No se encontraron productos", result.Message);
        }

        [Fact]
        public async Task DeleteProduct_ShouldRemoveProduct()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var logger = Mock.Of<ILogger<ProductService>>();
            var service = new ProductService(context, logger);

            var product = new Product
            {
                Price = 50,
                LoadDate = DateTime.Now,
                Category = "PRODUNO"
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Act
            var result = await service.DeleteProductAsync(product.Id);

            // Assert
            Assert.True(result);
            var deletedProduct = await context.Products.FindAsync(product.Id);
            Assert.Null(deletedProduct);
        }

        [Fact]
        public async Task UpdateProduct_ShouldModifyExistingProduct()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var logger = Mock.Of<ILogger<ProductService>>();
            var service = new ProductService(context, logger);

            var product = new Product
            {
                Price = 50,
                LoadDate = DateTime.Now,
                Category = "PRODUNO"
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var updateRequest = new ProductRequest
            {
                Price = 75,
                LoadDate = new DateTime(2024, 2, 20),
                Category = "PRODDOS"
            };

            // Act
            var result = await service.UpdateProductAsync(product.Id, updateRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(75, result.Price);
            Assert.Equal("PRODDOS", result.Category);
        }
    }
}