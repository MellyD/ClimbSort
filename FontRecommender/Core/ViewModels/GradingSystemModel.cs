namespace FontRecommender.Core.ViewModels
{
    public class GradingSystemModel
    {
        public int GradingSystemId { get; set; }
        public required string GradingSystemName { get; set; }
        public string? DisciplineName { get; set; }
        public int? DisciplineId { get; set; }
    }
}
