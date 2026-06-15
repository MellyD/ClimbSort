using AutoMapper;
using FontRecommender.Core.Interfaces;
using FontRecommender.Core.Models;
using FontRecommender.Core.Models.Generic;
using FontRecommender.Core.ViewModels;
using FontRecommender.Core.ViewModels.Filters;
using FontRecommender.Core.ViewModels.Generic;
using FontRecommender.Data;
using FontRecommender.Data.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FontRecommender.Core.Services
{
    public class FontService : IFontService
    {
        private readonly IRepository<FontRecommendationDBContext, Climb> _climbRepo;
        private readonly IRepository<FontRecommendationDBContext, Crag> _cragRepo;
        private readonly IRepository<FontRecommendationDBContext, Grade> _gradeRepo;
        private readonly IRepository<FontRecommendationDBContext, GradingSystem> _gradingSystemRepo;
        private readonly IRepository<FontRecommendationDBContext, Topography> _topographyRepo;
        private readonly IRepository<FontRecommendationDBContext, WallType> _wallTypeRepo;
        private readonly IRepository<FontRecommendationDBContext, Coordinates> _coordinatesRepo;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public FontService(
            IRepository<FontRecommendationDBContext, Climb> climbRepo,
            IRepository<FontRecommendationDBContext, Crag> cragRepo,
            IRepository<FontRecommendationDBContext, Grade> gradeRepo,
            IRepository<FontRecommendationDBContext, GradingSystem> gradingSystemRepo,
            IRepository<FontRecommendationDBContext, Topography> topographyRepo,
            IRepository<FontRecommendationDBContext, WallType> wallTypeRepo,
            IRepository<FontRecommendationDBContext, Coordinates> coordinatesRepo,
            IMapper mapper,
            ILogger logger)
        {
            _climbRepo = climbRepo;
            _cragRepo = cragRepo;
            _gradeRepo = gradeRepo;
            _gradingSystemRepo = gradingSystemRepo;
            _topographyRepo = topographyRepo;
            _wallTypeRepo = wallTypeRepo;
            _coordinatesRepo = coordinatesRepo;
            _mapper = mapper;
            _logger = logger;
        }

        #region Climb Methods
        public async Task<IEnumerable<ClimbSimpleModel>> GetClimbs(AdvancedClimbFilter filter)
        {
            try
            {
                IQueryable<Climb> climbs = await GetAdvancedClimbsAsQueryable(filter);
                IEnumerable<ClimbSimpleModel> models = _mapper.Map<IEnumerable<Climb>, IEnumerable<ClimbSimpleModel>>(climbs.ToList());

                return models;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to get climbs, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ClimbSimpleModel>> GetClimbs(ClimbFilter filter)
        {
            try
            {
                IQueryable<Climb> climbs = await GetClimbsAsQueryable(filter);
                IEnumerable<ClimbSimpleModel> models = _mapper.Map<IEnumerable<Climb>, IEnumerable<ClimbSimpleModel>>(climbs.ToList());

                models = models.Where(m => filter.WallTypeIds != null && filter.WallTypeIds.Count != 0 && filter.WallTypeIds.Contains(m.WallTypeId));

                return models;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to get climbs, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        public async Task<KeysetPaginateView<TView>> GetAllClimbsKeysetPaginated<TView>(KeysetClimbFilter filters) where TView : class
        {
            try
            {
                IQueryable<Climb> climbs = await GetClimbsAsQueryable(filters);
                KeysetPaginateView<TView> paginatedResultSet = await KeysetPaginateGetTask<TView>(filters.PageSize, filters.LastItem, climbs);
                return paginatedResultSet;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to get climbs, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        public async Task<OffsetPaginateView<TView>> GetClimbsOffsetPaginated<TView>(OffsetClimbFilter filters) where TView : class
        {
            try
            {
                IQueryable<Climb> climbs = await GetClimbsAsQueryable(filters);
                OffsetPaginateView<TView> offsetView = await OffsetPaginateGetTask<TView>(filters.PageNumber, filters.PageSize, climbs);
                return offsetView;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to get climbs, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        public async Task<KeysetPaginateView<TView>> GetAllClimbsKeysetPaginated<TView>(KeysetAdvancedClimbFilter filters) where TView : class
        {
            try
            {
                IQueryable<Climb> climbs = await GetAdvancedClimbsAsQueryable(filters);
                KeysetPaginateView<TView> paginatedResultSet = await KeysetPaginateGetTask<TView>(filters.PageSize, filters.LastItem, climbs);
                return paginatedResultSet;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to get climbs, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        public async Task<OffsetPaginateView<TView>> GetClimbsOffsetPaginated<TView>(OffsetAdvancedClimbFilter filters) where TView : class
        {
            try
            {
                IQueryable<Climb> climbs = await GetAdvancedClimbsAsQueryable(filters);
                OffsetPaginateView<TView> offsetView = await OffsetPaginateGetTask<TView>(filters.PageNumber, filters.PageSize, climbs);
                return offsetView;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to get climbs, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        public async Task<ClimbModel> GetClimb(Guid climbId)
        {
            try
            {
                Climb climb = await _climbRepo.GetByIdAsync(climbId) ?? throw new KeyNotFoundException($"Failed to fetch climb by Id: {climbId}");

                ClimbModel model = _mapper.Map<Climb, ClimbModel>(climb);
                return model;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to fetch climb, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        public async Task<Guid> CreateClimb(ClimbModel model)
        {
            try
            {
                Climb climb = new()
                {
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    WallType = await _wallTypeRepo.GetByIdAsync(model.WallTypeId) ?? throw new KeyNotFoundException($"Failed to fetch wall type by Id: {model.WallTypeId}"),
                    Description = model.Description,
                    Link = model.Link,
                    Popularity = model.Popularity,
                    Rating = model.Rating
                };

                if (model.GradeLabel != null && model.GradeLabel != climb.Grade?.GradeLabel)
                    climb.Grade = await _gradeRepo.FindAsync(g => g.GradeLabel.ToLower() == model.GradeLabel.ToLower()) ?? throw new KeyNotFoundException($"Failed to fetch grade by Label: {model.GradeLabel}");
                if (model.CragId != null)
                    climb.Crag = await _cragRepo.GetByIdAsync((Guid)model.CragId) ?? throw new KeyNotFoundException($"Failed to fetch crag by Id: {model.CragId}");

                await _climbRepo.CreateAsync(climb);

                if (model.Coordinates != null && model.Coordinates.Count > 0)
                {

                    foreach (CoordinatesModel coordsModel in model.Coordinates)
                    {
                        Coordinates coords = new()
                        {
                            Climb = climb,
                            CoordinateType = coordsModel.CoordinateType,
                            Latitude = coordsModel.Latitude,
                            Longitude = coordsModel.Longitude
                        };
                        await _coordinatesRepo.CreateAsync(coords);
                    }
                }
                return climb.Id;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to create climb, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        public async Task<Guid> UpdateClimb(ClimbModel model)
        {
            try
            {
                if (model.ClimbId == null)
                    throw new InvalidOperationException("Cannot update climb without Id.");
                Climb climb = await _climbRepo.GetByIdAsync((Guid)model.ClimbId) ?? throw new KeyNotFoundException($"Failed to fetch climb by Id: {model.ClimbId}");
                climb.Name = model.Name;
                climb.Description = model.Description;
                climb.Link = model.Link;
                climb.Popularity = model.Popularity;
                climb.Rating = model.Rating;
                climb.ModifiedDate = DateTime.Now;

                if (model.WallTypeId != climb.WallType.Id)
                    climb.WallType = await _wallTypeRepo.GetByIdAsync(model.WallTypeId) ?? throw new KeyNotFoundException($"Failed to fetch wall type by Id: {model.WallTypeId}");
                if (model.GradeLabel != null && model.GradeLabel != climb.Grade?.GradeLabel)
                    climb.Grade = await _gradeRepo.FindAsync(g => g.GradeLabel.ToLower() == model.GradeLabel.ToLower()) ?? throw new KeyNotFoundException($"Failed to fetch grade by Label: {model.GradeLabel}");
                if (model.CragId != null && model.CragId != climb.Crag?.Id)
                    climb.Crag = await _cragRepo.GetByIdAsync((Guid)model.CragId) ?? throw new KeyNotFoundException($"Failed to fetch crag by Id: {model.CragId}");

                if (model.Coordinates != null && model.Coordinates.Count > 0)
                {
                    foreach (CoordinatesModel coordsModel in model.Coordinates)
                    {
                        Coordinates? existingCoords = await _coordinatesRepo.FindAsync(c => c.Climb != null && c.Climb.Id == climb.Id && c.CoordinateType == coordsModel.CoordinateType);
                        if (existingCoords != null)
                        {
                            if (existingCoords.Latitude == coordsModel.Latitude && existingCoords.Longitude == coordsModel.Longitude)
                                continue;
                            else
                            {
                                existingCoords.Latitude = coordsModel.Latitude;
                                existingCoords.Longitude = coordsModel.Longitude;
                                await _coordinatesRepo.UpdateAsync(existingCoords);
                            }
                        }
                        else
                        {
                            Coordinates newCoords = new()
                            {
                                Climb = climb,
                                CoordinateType = coordsModel.CoordinateType,
                                Latitude = coordsModel.Latitude,
                                Longitude = coordsModel.Longitude
                            };
                            await _coordinatesRepo.CreateAsync(newCoords);
                        }
                    }
                }

                await _climbRepo.UpdateAsync(climb);
                return climb.Id;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to update climb, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteClimb(Guid climbId)
        {
            try
            {
                Climb climb = await _climbRepo.GetByIdAsync(climbId) ?? throw new KeyNotFoundException($"Failed to fetch climb by Id: {climbId}");
                List<Coordinates> coords = climb.Coordinates ?? [];

                await _climbRepo.DeleteAsync(climb);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to delete climb, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }
        private async Task<KeysetPaginateView<TView>> KeysetPaginateGetTask<TView>(int pageSize, Guid? lastItem, IQueryable<Climb> climbs) where TView : class
        {
            DateTime? lastItemModifiedAt = null;
            KeysetPaginateInsert paginateInsert = new() { PageSize = pageSize, LastItem = lastItem };
            if (paginateInsert.LastItem.HasValue)
            {
                Climb lastClimb = await _climbRepo.GetByIdAsync(paginateInsert.LastItem.Value);
                lastItemModifiedAt = lastClimb.ModifiedDate;
            }
            KeysetPaginateView<TView> paginatedResultSet = await _climbRepo.GetKeysetPaginatedResultSet<DateTime, TView>(
                climbs,
                s => s.ModifiedDate < lastItemModifiedAt,
                s => s.ModifiedDate,
                s => s.Id,
                paginateInsert);
            return paginatedResultSet;
        }

        private async Task<OffsetPaginateView<TView>> OffsetPaginateGetTask<TView>(int pageNumber, int pageSize, IQueryable<Climb> climbs) where TView : class
        {
            OffsetPaginateInsert paginateInsert = new() { PageNumber = pageNumber, PageSize = pageSize };
            OffsetPaginateView<TView> paginateResultset = await _climbRepo.GetOffsetPaginatedResultSet<DateTime, TView>(climbs, s => s.ModifiedDate, paginateInsert);

            return paginateResultset;
        }

        private async Task<IQueryable<Climb>> GetClimbsAsQueryable(ClimbFilter filter)
        {
            IQueryable<Climb> climbs = _climbRepo.FindAllAsQueryable(
                s => (string.IsNullOrEmpty(filter.Name) || (s.SearchName != null ? s.SearchName.StartsWith(filter.Name) : s.Name.StartsWith(filter.Name))) &&
                (filter.MinGradeScaleOrder == null || (s.Grade != null && s.Grade.ScaleOrder >= filter.MinGradeScaleOrder)) &&
                (filter.MaxGradeScaleOrder == null || (s.Grade != null && s.Grade.ScaleOrder <= filter.MaxGradeScaleOrder)) &&
                (filter.CragId == null || (s.Crag != null && s.Crag.Id == filter.CragId)) &&
                (filter.CircuitId == null || (s.Circuit != null && s.Circuit.Id == filter.CircuitId)) &&
                (filter.MinRating == null || (s.Rating != null && s.Rating >= filter.MinRating)) &&
                (filter.MaxRating == null || (s.Rating != null && s.Rating <= filter.MaxRating)) &&
                (filter.MinPopularity == null || (s.Popularity != null && s.Popularity >= filter.MinPopularity)) &&
                (filter.MaxPopularity == null || (s.Popularity != null && s.Popularity <= filter.MaxPopularity)) &&
                (filter.SitStart == null || s.SitStart == filter.SitStart) &&
                (filter.Dangerous == null || s.Dangerous == filter.Dangerous)
                );
            climbs = climbs.Where(c =>
                                c.Tags
                                    .Where(t => filter.Tags == null || filter.Tags.Contains(t.TagType))
                                    .Select(t => t.TagType)
                                    .Distinct()
                                    .Count() == (filter.Tags != null ? filter.Tags.Count : 0));

            return climbs;
        }

        private async Task<IQueryable<Climb>> GetAdvancedClimbsAsQueryable(AdvancedClimbFilter filter)
        {
            IQueryable<Climb>? combinedQuery = null;

            if (filter.FilterComponents == null || filter.FilterComponents.Count == 0)
            {
                IQueryable<Climb> climbs = await GetClimbsAsQueryable(_mapper.Map<AdvancedClimbFilter, ClimbFilter>(filter));
                return climbs;
            }

            foreach (var component in filter.FilterComponents)
            {
                IQueryable<Climb> climbs = _climbRepo.FindAllAsQueryable(
                    s => (string.IsNullOrEmpty(filter.Name) || (s.SearchName != null ? s.SearchName.StartsWith(filter.Name) : s.Name.StartsWith(filter.Name))) &&
                    (component.MinGradeScaleOrder == null || (s.Grade != null && s.Grade.ScaleOrder >= component.MinGradeScaleOrder)) &&
                    (component.MaxGradeScaleOrder == null || (s.Grade != null && s.Grade.ScaleOrder <= component.MaxGradeScaleOrder)) &&
                    (component.CragId == null || (s.Crag != null && s.Crag.Id == component.CragId)) &&
                    (component.CircuitId == null || (s.Circuit != null && s.Circuit.Id == component.CircuitId)) &&
                    (component.MinRating == null || (s.Rating != null && s.Rating >= component.MinRating)) &&
                    (component.MaxRating == null || (s.Rating != null && s.Rating <= component.MaxRating)) &&
                    (component.MinPopularity == null || (s.Popularity != null && s.Popularity >= component.MinPopularity)) &&
                    (component.MaxPopularity == null || (s.Popularity != null && s.Popularity <= component.MaxPopularity)) &&
                    (component.WallTypeId == null || s.WallType.Id == component.WallTypeId) &&
                    (component.SitStart == null || s.SitStart == component.SitStart) &&
                    (component.Dangerous == null || s.Dangerous == component.Dangerous)
                    );

                if (component.Tags?.Any() == true)
                {
                    foreach (var tag in component.Tags)
                    {
                        climbs = climbs.Where(c =>
                            c.Tags.Any(t => t.TagType == tag));
                    }
                }

                combinedQuery = combinedQuery == null
                    ? climbs
                    : combinedQuery.Union(climbs);
            }

            if (combinedQuery != null)
                return combinedQuery;
            else
                throw new KeyNotFoundException("Failed to fetch any climbs that match the filter");
        }
        #endregion

        #region Crag Methods
        public async Task<IEnumerable<CragSimpleModel>> GetCrags(CragFilter filter)
        {
            try
            {
                IQueryable<Crag> crags = _cragRepo.FindAllAsQueryable(c =>
                    (filter.Name == null || c.Name.StartsWith(filter.Name)) &&
                    (filter.CountryCode == null || c.CountryCode == filter.CountryCode)
                    );
                crags = crags.Where(c =>
                                    c.Tags
                                        .Where(t => filter.Tags == null || filter.Tags.Contains(t.TagType))
                                        .Select(t => t.TagType)
                                        .Distinct()
                                        .Count() == (filter.Tags != null ? filter.Tags.Count : 0));
                IEnumerable<CragSimpleModel> models = _mapper.Map<IEnumerable<Crag>, IEnumerable<CragSimpleModel>>(crags.ToList());
                return models;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to get crags, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        public async Task<CragModel> GetCrag(Guid cragId)
        {
            try
            {
                Crag crag = await _cragRepo.GetByIdAsync(cragId) ?? throw new KeyNotFoundException($"Failed to fetch crag by Id: {cragId}");
                CragModel model = _mapper.Map<Crag, CragModel>(crag);
                return model;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to fetch crag, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        public async Task<Guid> CreateCrag(CragModel model)
        {
            try
            {
                Crag crag = new()
                {
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    CountryCode = model.CountryCode
                };

                await _cragRepo.CreateAsync(crag);

                if (model.Coordinates != null && model.Coordinates.Count > 0)
                {
                    foreach (CoordinatesModel coordsModel in model.Coordinates)
                    {
                        Coordinates coords = new()
                        {
                            Crag = crag,
                            CoordinateType = coordsModel.CoordinateType,
                            Latitude = coordsModel.Latitude,
                            Longitude = coordsModel.Longitude
                        };
                        await _coordinatesRepo.CreateAsync(coords);
                    }
                }

                return crag.Id;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to create crag, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        public async Task<Guid> UpdateCrag(CragModel model)
        {
            try
            {
                if (model.CragId == null)
                    throw new InvalidOperationException("Cannot update crag without providing an Id");
                Crag crag = await _cragRepo.GetByIdAsync((Guid)model.CragId) ?? throw new KeyNotFoundException($"Failed to fetch crag by Id: {model.CragId}");
                crag.Name = model.Name;
                crag.CountryCode = model.CountryCode;
                crag.ModifiedDate = DateTime.Now;
                if (model.Coordinates != null && model.Coordinates.Count > 0)
                {
                    foreach (CoordinatesModel coordsModel in model.Coordinates)
                    {
                        Coordinates? existingCoords = await _coordinatesRepo.FindAsync(c => c.Crag != null && c.Crag.Id == crag.Id && c.CoordinateType == coordsModel.CoordinateType);
                        if (existingCoords != null)
                        {
                            if (existingCoords.Latitude == coordsModel.Latitude && existingCoords.Longitude == coordsModel.Longitude)
                                continue;
                            else
                            {
                                existingCoords.Latitude = coordsModel.Latitude;
                                existingCoords.Longitude = coordsModel.Longitude;
                                await _coordinatesRepo.UpdateAsync(existingCoords);
                            }
                        }
                        else
                        {
                            Coordinates newCoords = new()
                            {
                                Crag = crag,
                                CoordinateType = coordsModel.CoordinateType,
                                Latitude = coordsModel.Latitude,
                                Longitude = coordsModel.Longitude
                            };
                            await _coordinatesRepo.CreateAsync(newCoords);
                        }
                    }
                }
                await _cragRepo.UpdateAsync(crag);
                return crag.Id;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to update crag, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteCrag(Guid cragId)
        {
            try
            {
                Crag crag = await _cragRepo.GetByIdAsync(cragId) ?? throw new KeyNotFoundException($"Failed to fetch crag by Id: {cragId}");
                List<Coordinates> coords = crag.Coordinates;
                await _cragRepo.DeleteAsync(crag);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to delete crag, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }
        #endregion

        #region Static Variable Methods
        public IEnumerable<GradingSystemModel> GetGradingSystems()
        {
            try
            {
                IEnumerable<GradingSystem> gradingSystems = _gradingSystemRepo.GetAll() ?? throw new KeyNotFoundException("Failed to fetch grading systems.");
                IEnumerable<GradingSystemModel> models = _mapper.Map<IEnumerable<GradingSystem>, IEnumerable<GradingSystemModel>>(gradingSystems);

                return models;
            }
            catch(Exception ex)
            {
                _logger.Error("Failed to get grading sytems, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<GradeModel>> GetGradesForSystem(int gradingSystemId)
        {
            try
            {
                IEnumerable<Grade> grades = await _gradeRepo.FindAllAsync(g => g.GradingSystem.Id == gradingSystemId) ?? throw new KeyNotFoundException($"Failed to fetch grades for grading system by GradingSystemId: {gradingSystemId}");
                IEnumerable<GradeModel> models = _mapper.Map<IEnumerable<Grade>, IEnumerable<GradeModel>>(grades);

                return models;
            }
            catch(Exception ex)
            {
                _logger.Error("Failed to get grades for grading system, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        public IEnumerable<WallTypeModel> GetWallTypes()
        {
            try
            {
                IEnumerable<WallType> wallTypes = _wallTypeRepo.GetAll() ?? throw new KeyNotFoundException("Failed to fetch wall types.");
                IEnumerable<WallTypeModel> models = _mapper.Map<IEnumerable<WallType>, IEnumerable<WallTypeModel>>(wallTypes);

                return models;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to get wall types, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }
        #endregion
    }
}
