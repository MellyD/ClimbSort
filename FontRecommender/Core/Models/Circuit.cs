using ClimbSort.Core.Models.Generic;

namespace ClimbSort.Core.Models
{
    public class Circuit: BaseIdentity<Guid>
    {
        public required string Colour { get; set; }
        public virtual Grade? Grade { get; set; }
        public bool? Beginner { get; set; }
        public bool? Dangerous { get; set; }
        public virtual List<Coordinates> Coordinates { get; } = [];
        public virtual List<Climb> Climbs { get; } = [];
    }
}
