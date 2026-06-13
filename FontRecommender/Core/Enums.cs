namespace FontRecommender.Core
{
    public class Enums
    {
        public enum eLogMode
        {
            Success = 1,
            AuthorizationFailure = 2,
            Failed = 3
        }
        public enum eRequestType
        {
            GET = 1,
            POST = 2,
            PUT = 3,
            DELETE = 4,
            PATCH = 5
        }
        public enum eDisciplineType
        {
            Boulder = 1,
            Sport = 2,
            Trad = 3,
            Ice = 4,
            Mixed = 5
        }
        public enum eDifficultyConsensus
        {
            Soft = 1,
            Accurate = 2,
            Hard = 3,
            Unrepeated = 4
        }
        public enum eCoordinateType
        {
            Point = 1,
            SWPoint = 2,
            NEPoint = 3,
            TopographyLine = 4,
            CragPolygon = 5
        }
        public enum eTag
        {
            Popular = 1,
            BeginnerFriendly = 2,
            FamilyFriendly = 3,
            DryFast = 4
        }
    }
}
