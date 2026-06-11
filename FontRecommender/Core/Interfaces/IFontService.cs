using FontRecommender.Core.ViewModels;
using FontRecommender.Core.ViewModels.Filters;

namespace FontRecommender.Core.Interfaces
{
    public interface IFontService
    {
        Task<List<ClimbSimpleModel>> AdvancedGetClimbs(AdvancedClimbFilter filter);
        Task<IEnumerable<ClimbSimpleModel>> GetClimbs(ClimbFilter filter);
        Task<ClimbModel> GetClimb(Guid climbId);
        Task<Guid> CreateClimb(ClimbModel model);
        Task<Guid> UpdateClimb(ClimbModel model);
        Task<bool> DeleteClimb(Guid climbId);
        Task<IEnumerable<CragSimpleModel>> GetCrags(CragFilter filter);
        Task<CragModel> GetCrag(Guid cragId);
        Task<Guid> CreateCrag(CragModel model);
        Task<Guid> UpdateCrag(CragModel model);
        Task<bool> DeleteCrag(Guid cragId);
    }
}
