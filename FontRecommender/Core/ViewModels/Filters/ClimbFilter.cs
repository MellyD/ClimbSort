namespace ClimbSort.Core.ViewModels.Filters
{
    /// <summary>
    /// Filter for sorting and returning Climbs.
    /// </summary>
    public class ClimbFilter: BasicClimbFilterComponents
    {
        /// <summary>
        /// List of unique identifiers for Wall Types.
        /// </summary>
        public List<int>? WallTypeIds { get; set; } = [];
        /// <summary>
        /// List of unique identifiers for Tags. Tags determine attributes of a Climb.
        /// </summary>
        public List<int>? Tags { get; set; } = [];
    }

    /// <summary>
    /// Advanced multi-component filter for returning Climbs.
    /// </summary>
    public class AdvancedClimbFilter
    {
        /// <summary>
        /// Climb name, untranslated from original language. Separated out of individual filter due to search logic.
        /// </summary>
        public string? Name { get; set; } = default;
        /// <summary>
        /// List of individual filter components.
        /// </summary>
        public List<AdvancedClimbFilterComponent>? FilterComponents { get; set; }
    }

    /// <summary>
    /// Filter for sorting and returning Climbs. Additional values added for offset pagination.
    /// </summary>
    public class OffsetClimbFilter: ClimbFilter
    {
        /// <summary>
        /// Page number for which page in the result set is to be returned.
        /// </summary>
        public required int PageNumber { get; set; }
        /// <summary>
        /// Size of result set to be returned, with this page.
        /// </summary>
        public required int PageSize { get; set; }
    }

    /// <summary>
    /// Advanced multi-component filter for sorting and returning Climbs. Additional values added for offset pagination.
    /// </summary>
    public class OffsetAdvancedClimbFilter : AdvancedClimbFilter
    {
        /// <summary>
        /// Page number for which page in the result set is to be returned.
        /// </summary>
        public required int PageNumber { get; set; }
        /// <summary>
        /// Size of result set to be returned, with this page.
        /// </summary>
        public required int PageSize { get; set; }
    }

    /// <summary>
    /// Filter for sorting and returning Climbs. Additional values added for keyset pagination.
    /// </summary>
    public class KeysetClimbFilter: ClimbFilter
    {
        /// <summary>
        /// Size of result set to be returned, with this page.
        /// </summary>
        public required int PageSize { get; set; }
        /// <summary>
        /// Unique identifier of the last Climb in previous page.
        /// </summary>
        public Guid? LastItem { get; set; }
    }

    /// <summary>
    /// Advanced multi-component filter for sorting and returning Climbs. Additional values added for keyset pagination.
    /// </summary>
    public class KeysetAdvancedClimbFilter : AdvancedClimbFilter
    {
        /// <summary>
        /// Size of result set to be returned, with this page.
        /// </summary>
        public required int PageSize { get; set; }
        /// <summary>
        /// Unique identifier of the last Climb in previous page.
        /// </summary>
        public Guid? LastItem { get; set; }
    }

    /// <summary>
    /// Individual filter component of the advanced filter for sorting and returning Climbs. Additional values added for offset pagination.
    /// </summary>
    public class AdvancedClimbFilterComponent: BasicClimbFilterComponents
    {
        /// <summary>
        /// Unique identifier for a Wall Type.
        /// </summary>
        public int? WallTypeId { get; set; }
        /// <summary>
        /// List of unique identifiers for Tags. Tags determine attributes of a Climb/Crag.
        /// </summary>
        public List<int>? Tags { get; set; } = [];
    }

    /// <summary>
    /// Basic filter items, items chosen for being most commonly to be filtered on.
    /// </summary>
    public class BasicClimbFilterComponents
    {
        /// <summary>
        /// Climb name, untranslated from original language.
        /// </summary>
        public string? Name { get; set; } = default;
        /// <summary>
        /// The minimum integer value for what order position in the grade scale.
        /// </summary>
        public int? MinGradeScaleOrder { get; set; }
        /// <summary>
        /// The maximum integer value for what order position in the grade scale.
        /// </summary>
        public int? MaxGradeScaleOrder { get; set; }
        /// <summary>
        /// Unique identifier for a Crag.
        /// </summary>
        public Guid? CragId { get; set; } = default;
        /// <summary>
        /// Unique identifier for a Circuit.
        /// </summary>
        public Guid? CircuitId { get; set; } = default;
        /// <summary>
        /// The minimum decimal value of Rating for a Climb.
        /// </summary>
        public decimal? MinRating { get; set; }
        /// <summary>
        /// The maximum decimal value of Rating for a Climb.
        /// </summary>
        public decimal? MaxRating { get; set; }
        /// <summary>
        /// The minimum integer value of Popularity for a Climb (value is higher = more popular).
        /// </summary>
        public int? MinPopularity { get; set; }
        /// <summary>
        /// The maximum integer value of Popularity for a Climb (value is higher = more popular).
        /// </summary>
        public int? MaxPopularity { get; set; }
        /// <summary>
        /// Boolean value for whether a Climb is a sit start.
        /// </summary>
        public bool? SitStart { get; set; }
    }
}
