using FontRecommender.Core.Models.Static;
using System.ComponentModel.DataAnnotations;

namespace FontRecommender.Core.Models.Generic
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public required virtual TagType TagType { get; set; }
        public virtual Crag? Crag { get; set; }
        public virtual Climb? Climb { get; set; }
    }
}
