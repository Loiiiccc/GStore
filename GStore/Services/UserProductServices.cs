using GStore.Data;
using GStore.Models;
using Microsoft.EntityFrameworkCore;

namespace GStore.Services
{
    public class UserProductServices(GStoreDbContext context) : IUserProductServices
    {
        private async Task<User>  GetUserCartAsync(int codeClient)
        {
            var user = await context.Users.Include(u => u.Carts).ThenInclude(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefaultAsync(u => u.ClientCode == codeClient);
            if (user is null)
            {
                throw new Exception("User not found");
            }
            
            return user;
        }

        public async Task<CartItem> AddProductToCartAsync(int codeClient, int productId, int quantity)
        {
            //Check if user exists
            var user = await GetUserCartAsync(codeClient);

            //Check if cart exists
            var cart = user.Carts.FirstOrDefault();
            if (cart is null)
            {
                cart = new Cart { CodeClient = codeClient };
                user.Carts.Add(cart);
                await context.SaveChangesAsync();
            }

            //Check if product exists in cart
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.CartId == cart.Id && ci.ProductId == productId);
            if (cartItem is null)
            {
                cartItem = new CartItem { CartId = cart.Id, ProductId = productId, Quantity = quantity };
                cart.CartItems.Add(cartItem);
                await context.SaveChangesAsync();
            }
            else
            {
                cartItem.Quantity += quantity;
                await context.SaveChangesAsync();
            }

            return await context.CartItems.Include(ci => ci.Product).FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.ProductId == productId)?? throw new Exception("Cart item not found");
        }

        public async Task RemoveProductFromCartAsync(int codeClient, int productId)
        {
            //Check and get user with cart and cartItem
            //var user = await context.Users.Include(u => u.Carts).ThenInclude(c => c.CartItems).FirstOrDefaultAsync(u => u.ClientCode == codeClient);
            //if (user is null)
            //{
            //    throw new Exception("User not found");
            //}
            var user = await GetUserCartAsync(codeClient);

            var cart = user.Carts.FirstOrDefault();
            if (cart is null)
            {
                throw new Exception("Cart not found for this user");
            }

            //findd item to remove
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.CartId == cart.Id && ci.ProductId == productId);
            if (cartItem is not null)
            {
                cart.CartItems.Remove(cartItem);
                await context.SaveChangesAsync();
            }

        }

        public async Task ClearCartAsync(int codeClient)
        {
            var user = await GetUserCartAsync(codeClient);

            var cart = user.Carts.FirstOrDefault();
            if (cart is null)
            {
                throw new Exception("Cart not found for this user");
            }

            foreach (var cartItem in cart.CartItems)
            {
                context.CartItems.Remove(cartItem);

            }

            await context.SaveChangesAsync();

        }
    }
}
