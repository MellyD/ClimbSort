using System.ComponentModel.DataAnnotations;
using static FontRecommender.Core.Enums;

namespace FontRecommender.Core.Models.Generic
{
    public class Coordinates
    {
        [Key]
        public int Id { get; set; }
        public virtual Climb? Climb { get; set; }
        public virtual Crag? Crag { get; set; }
        public virtual Topography? Topography { get; set; }
        public virtual Circuit? Circuit { get; set; }
        public required eCoordinateType CoordinateType { get; set; }
        public required double Longitude { get; set; }
        public required double Latitude { get; set; }
    }
}
