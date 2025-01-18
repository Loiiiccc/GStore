using System.ComponentModel.DataAnnotations;

namespace GStore.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public float TotalCost { get; set; }
        //public List<Product>? ListProduct { get; set; }

        public int CodeClient { get; set; }
        public User Client { get; set; }

        public List<CartItem> CartItems { get; set; }

    }
}
