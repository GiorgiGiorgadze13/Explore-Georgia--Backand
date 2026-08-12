namespace DiscoverGeorgia.API.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PlaceId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}