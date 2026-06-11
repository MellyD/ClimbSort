using System.ComponentModel.DataAnnotations;
using static FontRecommender.Core.Enums;

namespace FontRecommender.Core.Models.Generic
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public required eTag TagType { get; set; }
        public virtual Crag? Crag { get; set; }
        public virtual Climb? Climb { get; set; }
    }
}
