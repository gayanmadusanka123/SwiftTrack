using Microsoft.EntityFrameworkCore;

namespace WMS.Data
{
    public class WmsDbContext : DbContext
    {
        public WmsDbContext(DbContextOptions<WmsDbContext> options) : base(options)
        {
        }
    }
}
