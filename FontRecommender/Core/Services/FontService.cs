using AutoMapper;
using FontRecommender.Core.Interfaces;
using FontRecommender.Core.Models;
using FontRecommender.Core.Models.Generic;
using FontRecommender.Core.ViewModels;
using FontRecommender.Core.ViewModels.Filters;
using FontRecommender.Core.ViewModels.Generic;
using FontRecommender.Data;
using FontRecommender.Data.Repository;
using Microsoft.Data.SqlClient;
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

        public async Task<List<ClimbSimpleModel>> AdvancedGetClimbs(AdvancedClimbFilter filter)
        {
            try
            {
                List<ClimbSimpleModel> returnModels = [];

                if (filter.FilterComponents == null || filter.FilterComponents.Count == 0)
                {
                    IEnumerable<ClimbSimpleModel> models = await GetClimbs(_mapper.Map<AdvancedClimbFilter, ClimbFilter>(filter));
                    return models.ToList();
                }

                foreach(AdvancedClimbFilterComponent component in filter.FilterComponents)
                {
                    IQueryable<Climb> climbs = _climbRepo.FindAllAsQueryable(
                        s => (string.IsNullOrEmpty(filter.Name) || s.Name.StartsWith(filter.Name)) &&
                        (component.MinGradeScaleOrder == null || (s.Grade != null && s.Grade.ScaleOrder >= component.MinGradeScaleOrder)) &&
                        (component.MaxGradeScaleOrder == null || (s.Grade != null && s.Grade.ScaleOrder <= component.MaxGradeScaleOrder)) &&
                        (component.CragId == null || (s.Crag != null && s.Crag.Id == component.CragId)) &&
                        (component.MinRating == null || (s.Rating != null && s.Rating >= component.MinRating)) &&
                        (component.MaxRating == null || (s.Rating != null && s.Rating <= component.MaxRating)) &&
                        (component.MinPopularity == null || (s.Popularity != null && s.Popularity >= component.MinPopularity)) &&
                        (component.MaxPopularity == null || (s.Popularity != null && s.Popularity <= component.MaxPopularity)) &&
                        (component.WallTypeId == null || s.WallType.Id == component.WallTypeId)
                        );

                    IEnumerable<ClimbSimpleModel> models = _mapper.Map<IEnumerable<Climb>, IEnumerable<ClimbSimpleModel>>(climbs.ToList());
                    returnModels.AddRange(models);
                }
                return returnModels;
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
                IQueryable<Climb> climbs = _climbRepo.FindAllAsQueryable(
                    s => (string.IsNullOrEmpty(filter.Name) || s.Name.StartsWith(filter.Name)) &&
                    (filter.MinGradeScaleOrder == null || (s.Grade != null && s.Grade.ScaleOrder >= filter.MinGradeScaleOrder)) &&
                    (filter.MaxGradeScaleOrder == null || (s.Grade != null && s.Grade.ScaleOrder <= filter.MaxGradeScaleOrder)) &&
                    (filter.CragId == null || (s.Crag != null && s.Crag.Id == filter.CragId)) &&
                    (filter.MinRating == null || (s.Rating != null && s.Rating >= filter.MinRating)) &&
                    (filter.MaxRating == null || (s.Rating != null && s.Rating <= filter.MaxRating)) &&
                    (filter.MinPopularity == null || (s.Popularity != null && s.Popularity >= filter.MinPopularity)) &&
                    (filter.MaxPopularity == null || (s.Popularity != null && s.Popularity <= filter.MaxPopularity))
                    );

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

        public async Task<IEnumerable<CragSimpleModel>> GetCrags(CragFilter filter)
        {
            try
            {
                IQueryable<Crag> crags = _cragRepo.FindAllAsQueryable(c =>
                    (filter.Name == null || c.Name.StartsWith(filter.Name)) &&
                    (filter.CountryCode == null || c.CountryCode == filter.CountryCode));
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
    }
}
