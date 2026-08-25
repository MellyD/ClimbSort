using System.ComponentModel.DataAnnotations;

namespace ClimbSort.Core.Models.Generic
{
    public class BaseIdentity<T>
    {
        [Key]
        public required T Id { get; set; }
        public required DateTime CreatedDate { get; set; }
        public required DateTime ModifiedDate { get; set; }
    }
}
