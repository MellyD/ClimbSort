namespace FontRecommender.Core.Models.Generic
{
    public class BaseIdentity<T>
    {
        public required T Id { get; set; }
        public required DateTime CreatedDate { get; set; }
        public required DateTime ModifiedDate { get; set; }
    }
}
