using FontRecommender.Core.ViewModels;
using FontRecommender.Core.ViewModels.Filters;
using FontRecommender.Core.ViewModels.Generic;

namespace FontRecommender.Core.Interfaces
{
    public interface IFontService
    {
        Task<IEnumerable<ClimbSimpleModel>> GetClimbs(AdvancedClimbFilter filter);
        Task<IEnumerable<ClimbSimpleModel>> GetClimbs(ClimbFilter filter);
        Task<KeysetPaginateView<TView>> GetAllClimbsKeysetPaginated<TView>(KeysetClimbFilter filters) where TView: class;
        Task<KeysetPaginateView<TView>> GetAllClimbsKeysetPaginated<TView>(KeysetAdvancedClimbFilter filters) where TView : class;
        Task<OffsetPaginateView<TView>> GetClimbsOffsetPaginated<TView>(OffsetClimbFilter filters) where TView : class;
        Task<OffsetPaginateView<TView>> GetClimbsOffsetPaginated<TView>(OffsetAdvancedClimbFilter filters) where TView : class;
        Task<ClimbModel> GetClimb(Guid climbId);
        Task<Guid> CreateClimb(ClimbModel model);
        Task<Guid> UpdateClimb(ClimbModel model);
        Task<bool> DeleteClimb(Guid climbId);
        Task<IEnumerable<CragSimpleModel>> GetCrags(CragFilter filter);
        Task<KeysetPaginateView<TView>> GetCragsKeysetPaginated<TView>(KeysetCragFilter filter) where TView : class;
        Task<CragModel> GetCrag(Guid cragId);
        Task<Guid> CreateCrag(CragModel model);
        Task<Guid> UpdateCrag(CragModel model);
        Task<bool> DeleteCrag(Guid cragId);
        Task<IEnumerable<TagModel>> GetTags(bool forClimbs);
        IEnumerable<GradingSystemModel> GetGradingSystems();
        Task<IEnumerable<GradeModel>> GetGradesForSystem(int gradingSystemId);
        IEnumerable<WallTypeModel> GetWallTypes();
    }
}
