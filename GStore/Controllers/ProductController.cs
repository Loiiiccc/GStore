using GStore.Data;
using GStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(GStoreDbContext context) : ControllerBase
    {
        private readonly GStoreDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _context.Products.Include(p => p.Category).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<Product>> GetProductByCode(string code)
        {
            var product = await _context.Products.FirstOrDefaultAsync(c => c.CodeProduct == code);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product newProduct)
        {
            if (newProduct == null)
                return BadRequest();

            _context.Products.Add(newProduct);
             await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new {id = newProduct.Id}, newProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product updateProduct)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();
            
            product.CodeProduct = updateProduct.CodeProduct;
            product.Description = updateProduct.Description;
            product.Quantity = updateProduct.Quantity;
            product.Price = updateProduct.Price;
            product.Availiable = updateProduct.Availiable;
            product.CategoryId = updateProduct.CategoryId;

            //_context.Products.Update(updateProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }



    }
}
