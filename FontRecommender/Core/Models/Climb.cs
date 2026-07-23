using FontRecommender.Core.Models.Generic;
using FontRecommender.Core.Models.Static;

namespace FontRecommender.Core.Models
{
    public class Climb: BaseIdentity<Guid>
    {
        public virtual Grade? Grade { get; set; }
        public virtual required WallType WallType { get; set; }
        public int? Popularity { get; set; }
        public required string Name { get; set; }
        public string? SearchName { get; set; }
        public virtual Crag? Crag { get; set; }
        public virtual Circuit? Circuit { get; set; }
        public int? CircuitNumber { get; set; }
        public bool? SitStart { get; set; } = false;
        public decimal? Rating { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public virtual List<Coordinates> Coordinates { get; } = [];
        public virtual List<Tag> Tags { get; } = [];
        public virtual Topography? Topography { get; } = default;
    }
}
