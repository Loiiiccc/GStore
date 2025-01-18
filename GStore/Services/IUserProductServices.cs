using GStore.Models;

namespace GStore.Services
{
    public interface IUserProductServices
    {

        Task<CartItem> AddProductToCartAsync(int codeClient, int productId, int quantity);
        Task RemoveProductFromCartAsync(int codeClient, int productId);
        Task ClearCartAsync(int codeClient);
        //Task<bool> IsProductInCartAsync(Guid userId, Guid productId);
        //Task<bool> IsUserHaveCartAsync(Guid userId);
    }
}
