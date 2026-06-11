using FontRecommender.Core.Models.Generic;

namespace FontRecommender.Core.Models
{
    public class Topography: BaseIdentity<Guid>
    {
        public string? FileReference { get; set; }
        public virtual required Climb Climb { get; set; }
        public virtual List<Coordinates>? Coordinates { get; } = [];
    }
}
