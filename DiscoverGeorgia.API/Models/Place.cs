namespace DiscoverGeorgia.API.Models
{
    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? NameEn { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? DescriptionEn { get; set; }
        public int CategoryId { get; set; }
        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? CityId { get; set; }
public int? RegionId { get; set; }
    }
}