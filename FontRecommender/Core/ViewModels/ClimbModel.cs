using FontRecommender.Core.Models;
using FontRecommender.Core.Models.Generic;
using FontRecommender.Core.ViewModels.Generic;

namespace FontRecommender.Core.ViewModels
{
    public class ClimbModel: ClimbSimpleModel
    {
        public Guid? CragId { get; set; }
        public Guid? CircuitId { get; set; }
        public string? CircuitColour { get; set; }
        public string? Description { get; set; }
        public int? CircuitNumber { get; set; }
        public bool? SitStart { get; set; }
        public string? TopoImageReference { get; set; }
        public virtual List<CoordinatesModel>? CragCoordinates { get; set; } = [];
        public virtual List<CoordinatesModel>? TopoCoordinates { get; set; } = [];
        public virtual List<CoordinatesModel>? Coordinates { get; set; } = [];
    }

    public class ClimbSimpleModel
    {
        public required int WallTypeId { get; set; }
        public Guid? ClimbId { get; set; }
        public required string Name { get; set; }
        public string? SearchName { get; set; }
        public int? Popularity { get; set; }
        public bool? Dangeroud { get; set; }
        public string? GradeLabel { get; set; }
        public int? GradeScaleOrder { get; set; }
        public string? CragName { get; set; }
        public decimal? Rating { get; set; }
        public string? Link { get; set; }
        public List<TagModel>? Tags { get; set; } = [];
    }
}
