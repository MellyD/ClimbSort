using ClimbSort.Core.Models.Generic;

namespace ClimbSort.Core.Models.Static
{
    public class WallType: BaseIdentity<int>
    {
        public required string Description { get; set; }
    }
}
