using ClimbSort.Core.Models.Generic;
using static ClimbSort.Core.Enums;

namespace ClimbSort.Core.Models
{
    public class Grade: BaseIdentity<int>
    {
        public required string GradeLabel { get; set; }
        public required virtual GradingSystem GradingSystem { get; set; }
        public required int ScaleOrder { get; set; }
        public decimal MinDifficultyRank { get; set; }
        public decimal MaxDifficultyRank { get; set; }
        public decimal? MeanDifficultyRank { get; set; }
    }
}
