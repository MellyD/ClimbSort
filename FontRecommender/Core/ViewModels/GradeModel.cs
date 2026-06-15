using FontRecommender.Core.Models;

namespace FontRecommender.Core.ViewModels
{
    public class GradeModel
    {
        public int GradeId { get; set; }
        public required string GradeLabel { get; set; }
        public required int GradingSystemId { get; set; }
        public required string GradingSystemName { get; set; }
        public required int ScaleOrder { get; set; }
        public decimal MinDifficultyRank { get; set; }
        public decimal MaxDifficultyRank { get; set; }
        public decimal? MeanDifficultyRank { get; set; }
    }
}
