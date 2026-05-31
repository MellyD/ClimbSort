using FontRecommender.Core.Models.Generic;

namespace FontRecommender.Core.Models
{
    public class Crag: BaseIdentity<Guid>
    {
        public required string Name { get; set; }
        public required string CountryCode { get; set; }
        public virtual List<Coordinates> Coordinates { get; } = [];
        public virtual List<Climb> Climbs { get; } = [];
    }
}
