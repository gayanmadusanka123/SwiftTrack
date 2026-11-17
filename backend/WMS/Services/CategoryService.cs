using Microsoft.EntityFrameworkCore;
using WMS.Data;
using WMS.DTOs;
using WMS.Interfaces;
using WMS.Model;

namespace WMS.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly WmsDbContext _context;

        public CategoryService(WmsDbContext context)
        {
            _context = context;
        }

        //GetAllCategories
        public async Task<List<EquipmentCategory>> GetAllAsync()
        {
            return await _context.EquipmentCategories
                .AsNoTracking()
                .ToListAsync();
        }

        //GetCategoryById
        public async Task <EquipmentCategory?> GetByIdAsync(int id)
        {
            return await _context.EquipmentCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        //create a new Category
        public async Task<EquipmentCategory> CreateAsync(CategoryRequest request)
        {
            var category = new EquipmentCategory
            {
                Name = request.Name.Trim(),
                Description = request.Description?.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _context.EquipmentCategories.AddAsync(category);

            await _context.SaveChangesAsync();

            return category;
        }
    }
}
