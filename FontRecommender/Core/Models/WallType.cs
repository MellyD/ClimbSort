using FontRecommender.Core.Models.Generic;

namespace FontRecommender.Core.Models
{
    public class WallType: BaseIdentity<int>
    {
        public required string Description { get; set; }
    }
}
