using FontRecommender.Core.Models.Generic;

namespace FontRecommender.Core.Models.Static
{
    public class WallType: BaseIdentity<int>
    {
        public required string Description { get; set; }
    }
}
