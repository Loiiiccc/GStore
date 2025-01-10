using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public required string CodeProduct { get; set; }
        public required string Description { get; set; }
        public int Quantity { get; set; }
        [Precision(18, 4)]
        public decimal Price { get; set; }
        public bool Availiable { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
