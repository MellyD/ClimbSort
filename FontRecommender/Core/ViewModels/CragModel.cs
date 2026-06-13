using FontRecommender.Core.Models.Generic;
using FontRecommender.Core.ViewModels.Generic;

namespace FontRecommender.Core.ViewModels
{
    public class CragModel: CragSimpleModel
    {
        public virtual List<ClimbSimpleModel> Climbs { get; set; } = [];
        public virtual List<CoordinatesModel> Coordinates { get; set; } = [];
    }

    public class CragSimpleModel
    {
        public Guid? CragId { get; set; }
        public string? SearchName { get; set; }
        public required string Name { get; set; }
        public required string CountryCode { get; set; }
        public List<TagModel>? Tags { get; set; } = [];
    }
}
