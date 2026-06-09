namespace FontRecommender.Core.ViewModels.Filters
{
    public class ClimbFilter
    {
        public string? Name { get; set; } = default;
        public int? MinGradeScaleOrder { get; set; }
        public int? MaxGradeScaleOrder { get; set; }
        public Guid? CragId { get; set; } = default;
        public List<int>? WallTypeIds { get; set; } = [];
        public decimal? MinRating { get; set; }
        public decimal? MaxRating { get; set; }
        public int? MinPopularity { get; set; }
        public int? MaxPopularity { get; set; }
    }
}
