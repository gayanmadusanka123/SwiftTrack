namespace WMS.Model
{
    public class EquipmentCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<EquipmentModel> EquipmentModels { get; set; } 
            = new List<EquipmentModel>();
    }
}
