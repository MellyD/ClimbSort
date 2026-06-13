using static FontRecommender.Core.Enums;

namespace FontRecommender.Core.ViewModels.Filters
{
    public class CragFilter
    {
        public string? Name { get; set; }
        public string? CountryCode { get; set; }
        public List<eTag>? Tags { get; set; } = [];
    }
}
