using FontRecommender.Core.Models.Generic;
using FontRecommender.Core.ViewModels.Generic;

namespace FontRecommender.Core.ViewModels
{
    /// <summary>
    /// View model holding all information of the Crag.
    /// </summary>
    public class CragModel: CragSimpleModel
    {
        /// <summary>
        /// List of Climb view models that are within this Crag.
        /// </summary>
        public virtual List<ClimbSimpleModel> Climbs { get; set; } = [];
        /// <summary>
        /// List of coordinates for the Crag
        /// </summary>
        public virtual List<CoordinatesModel> Coordinates { get; set; } = [];
    }

    /// <summary>
    /// Simple view model holding all information of the Crag.
    /// </summary>
    public class CragSimpleModel
    {
        /// <summary>
        /// Unique identifier of the Crag.
        /// </summary>
        public Guid? CragId { get; set; }
        /// <summary>
        /// Name of the crag, with special characters replaced for easier searching. Not translated from original language.
        /// </summary>
        public string? SearchName { get; set; }
        /// <summary>
        /// Name of the crag, not translated from original language.
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// 3 character code of the Country the Crag is in.
        /// </summary>
        public required string CountryCode { get; set; }
        /// <summary>
        /// List of Tag view models, holding information of Crag attributes.
        /// </summary>
        public List<TagModel>? Tags { get; set; } = [];
    }
}
