using ClimbSort.Core.Models;
using ClimbSort.Core.Models.Generic;
using ClimbSort.Core.ViewModels.Generic;

namespace ClimbSort.Core.ViewModels
{
    /// <summary>
    /// View model for viewing Climb information. This version being the more detailed model.
    /// </summary>
    public class ClimbModel: ClimbSimpleModel
    {
        /// <summary>
        /// Unique identifier for a Crag.
        /// </summary>
        public Guid? CragId { get; set; }
        /// <summary>
        /// Unique identifier for a Circuit.
        /// </summary>
        public Guid? CircuitId { get; set; }
        /// <summary>
        /// Colour of a Circuit.
        /// </summary>
        public string? CircuitColour { get; set; }
        /// <summary>
        /// Description of the Climb, offering any extra necessary information on the Climb (eg. approach info).
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// The indentifying number for which position the Climb is in the corresponding Circuit.
        /// </summary>
        public int? CircuitNumber { get; set; }
        /// <summary>
        /// Whether the Climb is a sit start or not.
        /// </summary>
        public bool? SitStart { get; set; }
        /// <summary>
        /// Url link for the climb image.
        /// </summary>
        public string? TopoImageReference { get; set; }
        /// <summary>
        /// List of coordinates for the corresponding Crag.
        /// </summary>
        public virtual List<CoordinatesModel>? CragCoordinates { get; set; } = [];
        /// <summary>
        /// List of pixel coordinates for the corresponding Topography.
        /// </summary>
        public virtual List<CoordinatesModel>? TopoCoordinates { get; set; } = [];
        /// <summary>
        /// List of coordinates for the Climb.
        /// </summary>
        public virtual List<CoordinatesModel>? Coordinates { get; set; } = [];
    }

    /// <summary>
    /// More simple view model for viewing Climb information.
    /// </summary>
    public class ClimbSimpleModel
    {
        /// <summary>
        /// Unique identifier for the Wall Type.
        /// </summary>
        public required int WallTypeId { get; set; }
        /// <summary>
        /// Unique identifier for the Climb.
        /// </summary>
        public Guid? ClimbId { get; set; }
        /// <summary>
        /// Name of the Climb, in original language.
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// More searchable version of the name of the Climb (replacing special characters).
        /// </summary>
        public string? SearchName { get; set; }
        /// <summary>
        /// Integer value denoting the popularity of the Climb (higher = more popular).
        /// </summary>
        public int? Popularity { get; set; }
        /// <summary>
        /// Lable name of the Grade of the Climb.
        /// </summary>
        public string? GradeLabel { get; set; }
        /// <summary>
        /// The order of the Grade of the CLimb in the scale of the Grading system.
        /// </summary>
        public int? GradeScaleOrder { get; set; }
        /// <summary>
        /// Name of the Crag that the Climb is located in.
        /// </summary>
        public string? CragName { get; set; }
        /// <summary>
        /// Decimal value denoting the Rating of the Climb out of 5.
        /// </summary>
        public decimal? Rating { get; set; }
        /// <summary>
        /// Url link of the Climb in the corresponding, highest info Climb repository website.
        /// </summary>
        public string? Link { get; set; }
        /// <summary>
        /// List of Tag view models, holding information of Climb attributes.
        /// </summary>
        public List<TagModel>? Tags { get; set; } = [];
    }
}
