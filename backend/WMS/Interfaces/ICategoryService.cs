using WMS.DTOs;
using WMS.Model;

namespace WMS.Interfaces
{
    public interface ICategoryService
    {
        Task<List<EquipmentCategory>> GetAllAsync();
        Task<EquipmentCategory?> GetByIdAsync(int id);
        Task<EquipmentCategory> CreateAsync(CategoryRequest request);
    }
}
