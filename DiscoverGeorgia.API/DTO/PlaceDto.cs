namespace DiscoverGeorgia.API.DTOs
{
    public class PlaceDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string Region { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsDisabledFriendly { get; set; }
    }
}