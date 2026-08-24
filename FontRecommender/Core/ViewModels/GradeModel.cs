using FontRecommender.Core.Models;

namespace FontRecommender.Core.ViewModels
{
    /// <summary>
    /// View model of an individual Grade in a Grading System.
    /// </summary>
    public class GradeModel
    {
        /// <summary>
        /// Unique identifier of the Grade.
        /// </summary>
        public int GradeId { get; set; }
        /// <summary>
        /// Label for the individual Grade.
        /// </summary>
        public required string GradeLabel { get; set; }
        /// <summary>
        /// Unique identifier of the Grading System the Grade belongs to.
        /// </summary>
        public required int GradingSystemId { get; set; }
        /// <summary>
        /// Name of the Grading System the Grade belongs to.
        /// </summary>
        public required string GradingSystemName { get; set; }
        /// <summary>
        /// The order position of the Grade in the Grading System.
        /// </summary>
        public required int ScaleOrder { get; set; }
        /// <summary>
        /// The decimal value denoting the minimum rank position of the Grade in our system. Used for creating relational links between Grading Systems.
        /// </summary>
        public decimal MinDifficultyRank { get; set; }
        /// <summary>
        /// The decimal value denoting the maximum rank position of the Grade in our system. Used for creating relational links between Grading Systems.
        /// </summary>
        public decimal MaxDifficultyRank { get; set; }
        /// <summary>
        /// The decimal value denoting the mean rank position of the Grade in our system. Used for creating relational links between Grading Systems.
        /// </summary>
        public decimal? MeanDifficultyRank { get; set; }
    }
}
