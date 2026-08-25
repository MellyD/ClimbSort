using ClimbSort.Core.Models.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClimbSort.Core.Models.Static
{
    public class TagType
    {
        [Key]
        public int Id { get; set; }
        public required string Description { get; set; }
        public virtual List<Tag> Tags { get; } = [];
    }
}
