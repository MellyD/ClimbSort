using FontRecommender.Core.Models;
using FontRecommender.Core.Models.Generic;
using FontRecommender.Core.ViewModels.Generic;

namespace FontRecommender.Core.ViewModels
{
    public class ClimbModel: ClimbSimpleModel
    {
        public required int WallTypeId { get; set; }
        public Guid? CragId { get; set; }
        public string? Description { get; set; }
        public virtual List<CoordinatesModel>? Coordinates { get; set; } = [];
    }

    public class ClimbSimpleModel
    {
        public Guid? ClimbId { get; set; }
        public required string Name { get; set; }
        public int? Popularity { get; set; }
        public string? GradeLabel { get; set; }
        public string? CragName { get; set; }
        public decimal? Rating { get; set; }
        public string? Link { get; set; }

    }
}
