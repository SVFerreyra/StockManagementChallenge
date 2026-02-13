using System.ComponentModel.DataAnnotations;

namespace StockManagement.API.Models.DTOs
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }

    public class ProductRequest
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Price { get; set; }

        [Required]
        public DateTime LoadDate { get; set; }

        [Required]
        [RegularExpression("^(PRODUNO|PRODDOS)$", ErrorMessage = "La categoría debe ser PRODUNO o PRODDOS")]
        public string Category { get; set; } = string.Empty;
    }

    public class ProductResponse
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string LoadDate { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }

    public class FilteredProductsRequest
    {
        [Required]
        [Range(1, 1000000, ErrorMessage = "El monto debe estar entre 1 y 1.000.000")]
        public int Budget { get; set; }
    }

    public class FilteredProductsResponse
    {
        public ProductResponse? ProductOne { get; set; }
        public ProductResponse? ProductTwo { get; set; }
        public decimal Total { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}