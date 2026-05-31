namespace FontRecommender.Core.ViewModels.Generic
{
    public class OffsetPaginateInsert
    {
        public required int PageSize { get; set; }
        public required int PageNumber { get; set; }
    }

    public class OffsetPaginateView<TEntity> where TEntity : class
    {
        public required int PageSize { get; set; }
        public required int PageNumber { get; set; }
        public required List<TEntity> ResultSet { get; set; }
    }
}
