using Microsoft.AspNetCore.Mvc;
using WMS.DTOs;
using WMS.Interfaces;

namespace WMS.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if(category == null)
            {
                return NotFound(new
                {
                    message = "Category not found"
                });
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryRequest request)
        {
            var category = await _categoryService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = category.Id },
                category
            );
        }
    }
}
