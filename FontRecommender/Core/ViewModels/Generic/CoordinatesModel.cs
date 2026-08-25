using static ClimbSort.Core.Enums;

namespace ClimbSort.Core.ViewModels.Generic
{
    /// <summary>
    /// View model for an individual set of Coordinates.
    /// </summary>
    public class CoordinatesModel
    {
        /// <summary>
        /// Value denoting the type of the coordinate pairing.
        /// </summary>
        public required eCoordinateType CoordinateType { get; set; }
        /// <summary>
        /// Longitude or X value of the coordinate pairing.
        /// </summary>
        public required double Longitude { get; set; }
        /// <summary>
        /// Latitude or Y value of the coordinate pairing.
        /// </summary>
        public required double Latitude { get; set; }
    }
}
