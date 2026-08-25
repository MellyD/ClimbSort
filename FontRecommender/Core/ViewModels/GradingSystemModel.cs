namespace ClimbSort.Core.ViewModels
{
    /// <summary>
    /// View model of a Grading System, a system by which to categorise the difficulty of a Climb.
    /// </summary>
    public class GradingSystemModel
    {
        /// <summary>
        /// Unique identifier of the Grading System.
        /// </summary>
        public int GradingSystemId { get; set; }
        /// <summary>
        /// Name of the Grading System.
        /// </summary>
        public required string GradingSystemName { get; set; }
        /// <summary>
        /// Name of the discipline for which the Grading System is linked to.
        /// </summary>
        public string? DisciplineName { get; set; }
        /// <summary>
        /// Unique identifier of the discipline for which the Grading System is linked to.
        /// </summary>
        public int? DisciplineId { get; set; }
    }
}
