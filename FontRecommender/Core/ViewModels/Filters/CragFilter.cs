using static FontRecommender.Core.Enums;

namespace FontRecommender.Core.ViewModels.Filters
{
    public class CragFilter
    {
        public string? Name { get; set; }
        public string? CountryCode { get; set; }
        public List<eTag>? Tags { get; set; } = [];
    }

    public class OffsetCragFilter : CragFilter
    {
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
    }

    public class KeysetCragFilter : CragFilter
    {
        public required int PageSize { get; set; }
        public Guid? LastItem { get; set; }
    }
}
