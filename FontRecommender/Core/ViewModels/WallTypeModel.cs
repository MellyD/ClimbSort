namespace ClimbSort.Core.ViewModels
{
    /// <summary>
    /// View model of the Wall Type, the type of angle or shape of the rock face.
    /// </summary>
    public class WallTypeModel
    {
        /// <summary>
        /// Unique identifier of the Wall Type.
        /// </summary>
        public int WallTypeId { get; set; }
        /// <summary>
        /// Description for the Wall Type.
        /// </summary>
        public required string WallTypeDescription { get; set; }
    }
}
