using FontRecommender.Core.Models.Generic;
using static FontRecommender.Core.Enums;

namespace FontRecommender.Core.Models
{
    public class Grade: BaseIdentity<int>
    {
        public required string GradeLabel { get; set; }
        public required virtual GradingSystem GradingSystem { get; set; }
        public required eDisciplineType Discipline {  get; set; }
        public required int ScaleOrder { get; set; }
        public decimal MinDifficultyRank { get; set; }
        public decimal MaxDifficultyRank { get; set; }
        public decimal? MeanDifficultyRank { get; set; }
        public virtual List<Climb> Climbs { get; } = [];
    }
}
