using GStore.Data;
using GStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(GStoreDbContext context) : ControllerBase
    {
        private readonly GStoreDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return Ok(await _context.Categories.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<Category>> GetCategoryByCode(string code)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryCode == code);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory(Category newCategory)
        {
            if (newCategory == null)
                return BadRequest();

            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoryById), new { id = newCategory.Id }, newCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Category UpdateCategory)
        {
            var category = await _context.Categories.FindAsync(UpdateCategory.Id);
            if (category == null)
                return NotFound();

            category.CategoryCode = UpdateCategory.CategoryCode;
            category.CategoryName = UpdateCategory.CategoryName;
            //_context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
