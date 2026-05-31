using FontRecommender.Core.Models.Generic;

namespace FontRecommender.Core.Models
{
    public class GradingSystem: BaseIdentity<int>
    {
        public required string Name { get; set; }
    }
}
