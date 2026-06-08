using FontRecommender.Core.Models.Generic;

namespace FontRecommender.Core.Models
{
    public class Climb: BaseIdentity<Guid>
    {
        public virtual Grade? Grade { get; set; }
        public virtual required WallType WallType { get; set; }
        public int? Popularity { get; set; }
        public required string Name { get; set; }
        public virtual Crag? Crag { get; set; }
        public decimal? Rating { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public virtual List<Coordinates>? Coordinates { get; } = [];
        public virtual Topography? Topography { get; } = default;
    }
}
