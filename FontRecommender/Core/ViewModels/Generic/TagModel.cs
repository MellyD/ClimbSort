
namespace FontRecommender.Core.ViewModels.Generic
{
    /// <summary>
    /// View model of a Tag, representing an attribute of a Climb or a Crag.
    /// </summary>
    public class TagModel
    {
        /// <summary>
        /// Identifying name of the Tag.
        /// </summary>
        public required string TagName { get; set; }
        /// <summary>
        /// Unique identifier of the Tag.
        /// </summary>
        public required int TagId { get; set; }
    }
}
