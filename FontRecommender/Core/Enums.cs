namespace FontRecommender.Core
{
    public class Enums
    {
        /// <summary>
        /// Type of log being created, used to identify what severity of log should be used in a private helper method for logging.
        /// </summary>
        public enum eLogMode
        {
            Success = 1,
            AuthorizationFailure = 2,
            Failed = 3
        }
        /// <summary>
        /// Type of request being made, used to enrich logs in private helper method.
        /// </summary>
        public enum eRequestType
        {
            GET = 1,
            POST = 2,
            PUT = 3,
            DELETE = 4,
            PATCH = 5
        }
        /// <summary>
        /// Type of discipline a Grading System is associated with.
        /// </summary>
        public enum eDisciplineType
        {
            Boulder = 1,
            Sport = 2,
            Trad = 3,
            Ice = 4,
            Mixed = 5
        }
        /// <summary>
        /// Type of Coordinate pairing, used to identify what kind of Coordinates are saved against Climb/Crag/Topography.
        /// </summary>
        public enum eCoordinateType
        {
            Point = 1,
            SWPoint = 2,
            NEPoint = 3,
            TopographyLine = 4,
            CragPolygon = 5
        }
    }
}
