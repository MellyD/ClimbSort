namespace ClimbSort.Core.ViewModels.Generic
{
    public class KeysetPaginateInsert
    {
        public required int PageSize { get; set; }
        public Guid? LastItem { get; set; }
    }
    public class KeysetPaginateView<TEntity> where TEntity : class
    {
        public required bool HasMore { get; set; }
        public Guid? LastItem { get; set; }
        public required List<TEntity> ResultSet { get; set; }
    }
}
