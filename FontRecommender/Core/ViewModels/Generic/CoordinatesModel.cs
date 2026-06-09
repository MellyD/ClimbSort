using static FontRecommender.Core.Enums;

namespace FontRecommender.Core.ViewModels.Generic
{
    public class CoordinatesModel
    {
        public required eCoordinateType CoordinateType { get; set; }
        public required double Longitude { get; set; }
        public required double Latitude { get; set; }
    }
}
