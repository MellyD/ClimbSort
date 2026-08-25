using ClimbSort.Core.Models.Generic;
using static ClimbSort.Core.Enums;

namespace ClimbSort.Core.Models
{
    public class GradingSystem: BaseIdentity<int>
    {
        public required string Name { get; set; }
        public required eDisciplineType Discipline { get; set; }
    }
}
