using FontRecommender.Core.Models.Generic;
using static FontRecommender.Core.Enums;

namespace FontRecommender.Core.Models
{
    public class GradingSystem: BaseIdentity<int>
    {
        public required string Name { get; set; }
        public required eDisciplineType Discipline { get; set; }
    }
}
