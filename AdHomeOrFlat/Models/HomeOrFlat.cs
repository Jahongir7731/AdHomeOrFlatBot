namespace AdHomeOrFlat.Models
{
    public class HomeOrFlat
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? FileId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? RegionName { get; set; }
        public string? DistrictName { get; set; }
    }
}
