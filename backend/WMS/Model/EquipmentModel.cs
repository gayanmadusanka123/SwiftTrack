using Microsoft.EntityFrameworkCore;

namespace WMS.Model
{
    public class EquipmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;

        [Precision(18, 2)]
        public decimal DailyRentalPrice { get; set; } 
        public int EquipmentCategoryId { get; set; }
        public EquipmentCategory EquipmentCategory { get; set; } = null!;
    }
}
