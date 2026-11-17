using Microsoft.EntityFrameworkCore;
using WMS.Model;

namespace WMS.Data
{
    public class WmsDbContext : DbContext
    {
        public WmsDbContext(DbContextOptions<WmsDbContext> options) : base(options)
        {
        }

        public DbSet<EquipmentCategory> EquipmentCategories { get; set; }
        public DbSet<EquipmentModel> EquipmentModels { get; set; }
    }
}
