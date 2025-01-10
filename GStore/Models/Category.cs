using System.ComponentModel.DataAnnotations;

namespace GStore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public required string CategoryCode { get; set; }
        public required string CategoryName { get; set; }

        //public Product Products { get; set; }
    }
}
