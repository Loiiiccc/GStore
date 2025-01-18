using GStore.Data;
using GStore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(GStoreDbContext context, UserProductServices services) : ControllerBase
    {
        private readonly GStoreDbContext _context = context;
        private readonly UserProductServices services = services;

        [HttpGet]
        public async Task<IActionResult> GetCart(int codeClient)
        {
            try
            {
                var cart = await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefaultAsync(c => c.CodeClient == codeClient);

                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("/addToCart")]
        public async Task<IActionResult> AddToCart(int codeClient, int codeProduct, int quantity)
        {
            try
            {
                var cartItem = await services.AddProductToCartAsync(codeClient, codeProduct, quantity);
                return CreatedAtAction(nameof(GetCart), cartItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("/removeFromCart")]
        public async Task<IActionResult> RemoveFromCart(int codeClient, int productId)
        {
            try
            {
                await services.RemoveProductFromCartAsync(codeClient, productId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("/clearCart")]
        public async Task<IActionResult> ClearCart(int codeClient)
        {
            try
            {
                await services.ClearCartAsync(codeClient);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
