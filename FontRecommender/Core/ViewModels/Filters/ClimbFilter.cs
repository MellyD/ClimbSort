using static FontRecommender.Core.Enums;

namespace FontRecommender.Core.ViewModels.Filters
{
    public class ClimbFilter: BasicClimbFilterComponents
    {
        public List<int>? WallTypeIds { get; set; } = [];
        public List<int>? Tags { get; set; } = [];
    }

    public class AdvancedClimbFilter
    {
        public string? Name { get; set; } = default;
        public List<AdvancedClimbFilterComponent>? FilterComponents { get; set; }
    }

    public class OffsetClimbFilter: ClimbFilter
    {
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
    }

    public class OffsetAdvancedClimbFilter : AdvancedClimbFilter
    {
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
    }

    public class KeysetClimbFilter: ClimbFilter
    {
        public required int PageSize { get; set; }
        public Guid? LastItem { get; set; }
    }

    public class KeysetAdvancedClimbFilter : AdvancedClimbFilter
    {
        public required int PageSize { get; set; }
        public Guid? LastItem { get; set; }
    }

    public class AdvancedClimbFilterComponent: BasicClimbFilterComponents
    {
        public int? WallTypeId { get; set; }
        public List<int>? Tags { get; set; } = [];
    }

    public class BasicClimbFilterComponents
    {
        public string? Name { get; set; } = default;
        public int? MinGradeScaleOrder { get; set; }
        public int? MaxGradeScaleOrder { get; set; }
        public Guid? CragId { get; set; } = default;
        public Guid? CircuitId { get; set; } = default;
        public decimal? MinRating { get; set; }
        public decimal? MaxRating { get; set; }
        public int? MinPopularity { get; set; }
        public int? MaxPopularity { get; set; }
        public bool? SitStart { get; set; }
    }
}
