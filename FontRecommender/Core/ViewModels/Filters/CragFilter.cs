using static FontRecommender.Core.Enums;

namespace FontRecommender.Core.ViewModels.Filters
{
    /// <summary>
    /// Filter containing Crag attributes to filter on.
    /// </summary>
    public class CragFilter
    {
        /// <summary>
        /// Name of the crag, not translated from the original language.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 3 letter code representing the Country that the Crag is in.
        /// </summary>
        public string? CountryCode { get; set; }
        /// <summary>
        /// List of unique identifiers of Tags that are attributed to the Crag.
        /// </summary>
        public List<int>? Tags { get; set; } = [];
    }

    /// <summary>
    /// Filter for sorting and returning Crags. Additional values added for offset pagination.
    /// </summary>
    public class OffsetCragFilter : CragFilter
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
    /// Filter for sorting and returning Crags. Additional values added for keyset pagination.
    /// </summary>
    public class KeysetCragFilter : CragFilter
    {
        /// <summary>
        /// Size of result set to be returned, with this page.
        /// </summary>
        public required int PageSize { get; set; }
        /// <summary>
        /// Unique identifier of the last Crag in previous page.
        /// </summary>
        public Guid? LastItem { get; set; }
    }
}
