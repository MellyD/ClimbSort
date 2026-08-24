using AutoMapper;
using FontRecommender.Core.Interfaces;
using FontRecommender.Core.Models;
using FontRecommender.Core.Models.Generic;
using FontRecommender.Core.Models.Static;
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
    /// <summary>
    /// Service class that provides methods for managing climbs, crags, grades, grading systems, wall types, and coordinates in the FontRecommender application.
    /// </summary>
    public class FontService : IFontService
    {
        private readonly IRepository<FontRecommendationDBContext, Climb> _climbRepo;
        private readonly IRepository<FontRecommendationDBContext, Crag> _cragRepo;
        private readonly IRepository<FontRecommendationDBContext, Grade> _gradeRepo;
        private readonly IRepository<FontRecommendationDBContext, GradingSystem> _gradingSystemRepo;
        private readonly IRepository<FontRecommendationDBContext, WallType> _wallTypeRepo;
        private readonly IRepository<FontRecommendationDBContext, Coordinates> _coordinatesRepo;
        private readonly IRepository<FontRecommendationDBContext, Tag> _tagRepo;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public FontService(
            IRepository<FontRecommendationDBContext, Climb> climbRepo,
            IRepository<FontRecommendationDBContext, Crag> cragRepo,
            IRepository<FontRecommendationDBContext, Grade> gradeRepo,
            IRepository<FontRecommendationDBContext, GradingSystem> gradingSystemRepo,
            IRepository<FontRecommendationDBContext, WallType> wallTypeRepo,
            IRepository<FontRecommendationDBContext, Coordinates> coordinatesRepo,
            IRepository<FontRecommendationDBContext, Tag> tagRepo,
            IMapper mapper,
            ILogger logger)
        {
            _climbRepo = climbRepo;
            _cragRepo = cragRepo;
            _gradeRepo = gradeRepo;
            _gradingSystemRepo = gradingSystemRepo;
            _wallTypeRepo = wallTypeRepo;
            _coordinatesRepo = coordinatesRepo;
            _tagRepo = tagRepo;
            _mapper = mapper;
            _logger = logger;
        }

        //All methods for CRUD operations on climbs. Most of the explanations for the logic are in this section, as the crag methods are very similar and follow the same logic.
        #region Climb Methods
        /// <summary>
        /// Returns a list of climbs based on the provided filter, with the filter being an advanced filter that can combine multiple conditional filters.
        /// This is to allow a more complex and customised list of climbs.
        /// The filter can include various criteria such as name, grade, crag, circuit, rating, popularity, sit start, and tags. 
        /// The method retrieves the climbs from the repository, maps them to ClimbSimpleModel objects, and returns the result.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns> IEnumerable of the climb simple view models. </returns>
        public async Task<IEnumerable<ClimbSimpleModel>> GetClimbs(AdvancedClimbFilter filter)
        {
            try
            {
                IQueryable<Climb> climbs = await GetAdvancedClimbsAsQueryable(filter); // Using the advanced filter to get climbs as IQueryable from private helper method.
                IEnumerable<ClimbSimpleModel> models = _mapper.Map<IEnumerable<Climb>, IEnumerable<ClimbSimpleModel>>(climbs.ToList()); // Mapping the climbs to the simple view model.

                return models;
            }
            // Using the exception handling to log the error and rethrow it to be handled by the calling method. Mainly to indicate more explicitly the location of the error.
            catch (Exception ex)
            {
                _logger.Error("Failed to get climbs, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Returns a list of climbs based on the provided filter. 
        /// The filter can include various criteria such as name, grade, crag, circuit, rating, popularity, sit start, and tags. 
        /// The method retrieves the climbs from the repository, maps them to ClimbSimpleModel objects, and returns the result.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns> IEnumerable of the climb simple view models. </returns>
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

        /// <summary>
        /// This method retrieves a paginated list of climbs based on the provided keyset filter.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="filters"></param>
        /// <returns> List of climb view models along with the relevant keyset pagination info. </returns>
        public async Task<KeysetPaginateView<TView>> GetAllClimbsKeysetPaginated<TView>(KeysetClimbFilter filters) where TView : class
        {
            try
            {
                IQueryable<Climb> climbs = await GetClimbsAsQueryable(filters);

                // Using the private helper method to get the appropriately paginated result set, already mapped to requested view model by method.
                KeysetPaginateView<TView> paginatedResultSet = await KeysetPaginateGetTask<TView>(filters.PageSize, filters.LastItem, climbs); 
                return paginatedResultSet;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to get climbs, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// This method retrieves a paginated list of climbs based on the provided offset filter.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="filters"></param>
        /// <returns> List of climb view models along with the relevant offset pagination info. </returns>
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

        /// <summary>
        /// This method retrieves a paginated list of climbs based on the provided advanced keyset filter, which allows for more complex filtering criteria.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="filters"></param>
        /// <returns> List of climb view models along with the relevant keyset pagination info. </returns>
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

        /// <summary>
        /// This method retrieves a paginated list of climbs based on the provided advanced offset filter, which allows for more complex filtering criteria.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="filters"></param>
        /// <returns> List of climb view models along with the relevant offset pagination info. </returns>
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

        /// <summary>
        /// This method retrieves a climb by its unique identifier (climbId) and returns the corresponding ClimbModel.
        /// </summary>
        /// <param name="climbId"></param>
        /// <returns> View model containing the relevant climb information. </returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ClimbModel> GetClimb(Guid climbId)
        {
            try
            {
                //Using the KeyNotFoundException as an indicator of failure to fetch the climb by Id. Caught by controller and returned as a 404 response.
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

        /// <summary>
        /// This method creates a new climb based on the provided ClimbModel. It initializes a new Climb entity, sets its properties, and saves it to the repository.
        /// </summary>
        /// <param name="model"></param>
        /// <returns> Guid of the climb created. </returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<Guid> CreateClimb(ClimbModel model)
        {
            try
            {
                //Building the model manually rather than using AutoMapper to allow for more control over the creation process and to handle potential null values and exceptions more explicitly.
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

                //Since the Grade is technically unnecessary for the Climb, we check first if the GradeLabel is provided before attempting to fetch the Grade from the repository. If not provided, the Grade will remain null.
                if (model.GradeLabel != null)
                    //If, however, the GradeLabel is provided but does not match any existing Grade in the repository, we throw a KeyNotFoundException to indicate that the provided GradeLabel is invalid and cannot be associated with the new climb.
                    climb.Grade = await _gradeRepo.FindAsync(g => g.GradeLabel.ToLower() == model.GradeLabel.ToLower()) ?? throw new KeyNotFoundException($"Failed to fetch grade by Label: {model.GradeLabel}");

                //Same is done with the crag.
                if (model.CragId != null)
                    climb.Crag = await _cragRepo.GetByIdAsync((Guid)model.CragId) ?? throw new KeyNotFoundException($"Failed to fetch crag by Id: {model.CragId}");

                //Creation is initiated and awaited to ensure that the climb is saved to the repository before proceeding to create any associated coordinates.
                await _climbRepo.CreateAsync(climb);

                //If the model contains any coordinates, we iterate through them and create a new Coordinates entity for each one, associating it with the newly created climb. Each coordinate is then saved to the repository.
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
                return climb.Id; //Id of created climb is returned.
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to create climb, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// This method updates an existing climb based on the provided ClimbModel. It retrieves the climb from the repository, updates its properties, and saves the changes back to the repository.
        /// </summary>
        /// <param name="model"></param>
        /// <returns> Guid of the climb updated. </returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<Guid> UpdateClimb(ClimbModel model)
        {
            try
            {
                //If the Id isn't provided, we can't find a climb to update.
                if (model.ClimbId == null)
                    throw new InvalidOperationException("Cannot update climb without Id.");

                //We fetch the climb, if it doesn't match an existing climb, we throw an exception.
                Climb climb = await _climbRepo.GetByIdAsync((Guid)model.ClimbId) ?? throw new KeyNotFoundException($"Failed to fetch climb by Id: {model.ClimbId}");

                //Everything is manually updated as a mapper transformation would cause issues and wouldn't allow for fetches.
                climb.Name = model.Name;
                climb.Description = model.Description;
                climb.Link = model.Link;
                climb.Popularity = model.Popularity;
                climb.Rating = model.Rating;
                climb.ModifiedDate = DateTime.Now; //Modified time is updated for record keeping and pagination purposes.

                //We check if fetch items are different from the existing climb's, and only then do we perform a fetch. To save on unnecessary querying of the database.
                if (model.WallTypeId != climb.WallType.Id)
                    climb.WallType = await _wallTypeRepo.GetByIdAsync(model.WallTypeId) ?? throw new KeyNotFoundException($"Failed to fetch wall type by Id: {model.WallTypeId}");
                if (model.GradeLabel != null && model.GradeLabel != climb.Grade?.GradeLabel)
                    climb.Grade = await _gradeRepo.FindAsync(g => g.GradeLabel.ToLower() == model.GradeLabel.ToLower()) ?? throw new KeyNotFoundException($"Failed to fetch grade by Label: {model.GradeLabel}");
                if (model.CragId != null && model.CragId != climb.Crag?.Id)
                    climb.Crag = await _cragRepo.GetByIdAsync((Guid)model.CragId) ?? throw new KeyNotFoundException($"Failed to fetch crag by Id: {model.CragId}");

                //We iterate through the provided coordinates, and then check if they already exist in the database (by checking that type of coordinate for this climb). If they do, we update them, if not, we create them.
                //This allows for a more flexible update process where coordinates can be added or updated as needed.
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

                //Climb is updated and then the Id is returned.
                await _climbRepo.UpdateAsync(climb);
                return climb.Id;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to update climb, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// This method deletes a climb based on the provided climbId. It retrieves the climb from the repository and deletes it, along with any associated coordinates.
        /// </summary>
        /// <param name="climbId"></param>
        /// <returns> Bool indicating deletion success, mainly for the purpose of avoiding void Task return errors. </returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<bool> DeleteClimb(Guid climbId)
        {
            try
            {
                //Climb is fetched and the coordinates loaded to ensure that they are also deleted when the climb is deleted. If the climb doesn't exist, we throw an exception.
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

        /// <summary>
        /// This method is a private helper method used internally by the service to perform the actual pagination logic for climbs.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="pageSize"></param>
        /// <param name="lastItem"></param>
        /// <param name="climbs"></param>
        /// <returns> View model with appropriate keyset pagination info for climbs. </returns>
        private async Task<KeysetPaginateView<TView>> KeysetPaginateGetTask<TView>(int pageSize, Guid? lastItem, IQueryable<Climb> climbs) where TView : class
        {
            //Since we are ordering by ModifiedDate (for keyset pagination specifically), we have to fetch the ModifiedDate of the last item in order to use it as a reference point for the next page of results.
            //If the last item is null, we simply return the first page of results.
            DateTime? lastItemModifiedAt = null;
            KeysetPaginateInsert paginateInsert = new() { PageSize = pageSize, LastItem = lastItem }; //We populate the insert model with the provided page size and last item.
            //If the last item is provided, we fetch it and set the lastItemModifiedAt to its ModifiedDate. This will be used in the query to fetch the next set of results.
            if (paginateInsert.LastItem.HasValue)
            {
                Climb lastClimb = await _climbRepo.GetByIdAsync(paginateInsert.LastItem.Value);
                lastItemModifiedAt = lastClimb.ModifiedDate;
            }
            //The set of climbs is then fetched using the last known item and the page size, and the result is returned as a KeysetPaginateView<TView> object.
            KeysetPaginateView<TView> paginatedResultSet = await _climbRepo.GetKeysetPaginatedResultSet<DateTime, TView>(
                climbs,
                s => s.ModifiedDate < lastItemModifiedAt,
                s => s.ModifiedDate,
                s => s.Id,
                paginateInsert);
            return paginatedResultSet;
        }

        /// <summary>
        /// This method is a private helper method used internally by the service to perform the actual offset pagination logic for climbs.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="climbs"></param>
        /// <returns> View model with appropriate offset pagination info for climbs. </returns>
        private async Task<OffsetPaginateView<TView>> OffsetPaginateGetTask<TView>(int pageNumber, int pageSize, IQueryable<Climb> climbs) where TView : class
        {
            //We populate the insert model with the provided page number and page size. This will be used in the query to fetch the appropriate set of results.
            OffsetPaginateInsert paginateInsert = new() { PageNumber = pageNumber, PageSize = pageSize };
            //The view models are then fetched (mapped by method from climbs queried) using the page number and page size provided.
            OffsetPaginateView<TView> paginateResultset = await _climbRepo.GetOffsetPaginatedResultSet<DateTime, TView>(climbs, s => s.ModifiedDate, paginateInsert);

            return paginateResultset;
        }

        /// <summary>
        /// This method retrieves climbs from the repository based on the provided ClimbFilter and returns them as an IQueryable(Climb).
        /// </summary>
        /// <param name="filter"></param>
        /// <returns> IQueryable containing the filtered climbs. </returns>
        private async Task<IQueryable<Climb>> GetClimbsAsQueryable(ClimbFilter filter)
        {
            //Filter is applied to the query predicate in classic fashion (checking if filter item is provided with a null check)
            IQueryable<Climb> climbs = _climbRepo.FindAllAsQueryable(
                s => (string.IsNullOrEmpty(filter.Name) || (s.SearchName != null ? s.SearchName.StartsWith(filter.Name) : s.Name.StartsWith(filter.Name))) &&
                (filter.MinGradeScaleOrder == null || (s.Grade != null && s.Grade.ScaleOrder >= filter.MinGradeScaleOrder)) &&
                (filter.MaxGradeScaleOrder == null || (s.Grade != null && s.Grade.ScaleOrder <= filter.MaxGradeScaleOrder)) &&
                (filter.CragId == null || (s.Crag != null && s.Crag.Id == filter.CragId)) &&
                (filter.CircuitId == null || (s.Circuit != null && s.Circuit.Id == filter.CircuitId)) &&
                (filter.MinRating == null || (s.Rating != null && s.Rating >= filter.MinRating)) && //Min rating is used as a "greater than" filter
                (filter.MaxRating == null || (s.Rating != null && s.Rating <= filter.MaxRating)) && //Max rating is used as a "less than" filter
                (filter.MinPopularity == null || (s.Popularity != null && s.Popularity >= filter.MinPopularity)) &&
                (filter.MaxPopularity == null || (s.Popularity != null && s.Popularity <= filter.MaxPopularity)) &&
                (filter.SitStart == null || s.SitStart == filter.SitStart)
                );
            //Tags are filtered after the fact as the filtering is more complex and requires a distinct count of the tags to ensure that all provided tags are present on the climb.
            climbs = climbs.Where(c =>
                                c.Tags
                                    .Where(t => filter.Tags == null || filter.Tags.Contains(t.TagType.Id))
                                    .Select(t => t.TagType)
                                    .Distinct()
                                    .Count() == (filter.Tags != null ? filter.Tags.Count : 0));

            return climbs;
        }

        /// <summary>
        /// This method retrieves climbs from the repository based on the provided AdvancedClimbFilter and returns them as an IQueryable(Climb).
        /// </summary>
        /// <param name="filter"></param>
        /// <returns> IQueryable containing the filtered climbs. </returns>
        /// <exception cref="KeyNotFoundException"></exception>
        private async Task<IQueryable<Climb>> GetAdvancedClimbsAsQueryable(AdvancedClimbFilter filter)
        {
            //IQueryable is initialised early to allow for the combination of multiple filter components into a single query.
            //This is done using the Union method, which combines the results of multiple queries into a single result set.
            IQueryable<Climb>? combinedQuery = null;

            //If no filter components are provided, we simply return the climbs based on the basic filter criteria.
            if (filter.FilterComponents == null || filter.FilterComponents.Count == 0)
            {
                IQueryable<Climb> climbs = await GetClimbsAsQueryable(_mapper.Map<AdvancedClimbFilter, ClimbFilter>(filter));
                return climbs;
            }

            //For each filter component provided, we build a query based on the component's criteria and combine it with the existing combinedQuery using the Union method.
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
                    (component.SitStart == null || s.SitStart == component.SitStart)
                    );

                if (component.Tags?.Any() == true)
                {
                    foreach (var tag in component.Tags)
                    {
                        climbs = climbs.Where(c =>
                            c.Tags.Any(t => t.TagType.Id == tag));
                    }
                }

                combinedQuery = combinedQuery == null
                    ? climbs
                    : combinedQuery.Union(climbs);
            }

            //If the query is successfully built, we return it. If not, we throw an exception indicating that no climbs matching the filter were found.
            if (combinedQuery != null)
                return combinedQuery;
            else
                throw new KeyNotFoundException("Failed to fetch any climbs that match the filter");
        }
        #endregion

        //Most of the crag methods follow the same methodology as the climb methods, with the exception of the advanced filter methods, which are not implemented for crags as they are not needed at this time (there aren't that many crags).
        #region Crag Methods
        /// <summary>
        /// This method retrieves a list of crags based on the provided CragFilter. The filter can include various criteria such as name, country code, and tags.
        /// The method retrieves the crags from the repository, maps them to CragSimpleModel objects, and returns the result.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns> IEnumerable of a simplified view model of crags. </returns>
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
                                        .Where(t => filter.Tags == null || filter.Tags.Contains(t.TagType.Id))
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

        /// <summary>
        /// This method retrieves a paginated list of crags based on the provided keyset filter. The filter can include various criteria such as name, country code, and tags.
        /// Pagination is performed using keyset pagination, which allows for efficient retrieval of large datasets by using the last item of the previous page as a reference point for the next page.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="filter"></param>
        /// <returns> Returns a view model with appropriate keyset pagination info for crags. Filtered based on provided filter. </returns>
        public async Task<KeysetPaginateView<TView>> GetCragsKeysetPaginated<TView>(KeysetCragFilter filter) where TView : class
        {
            try
            {
                IQueryable<Crag> crags = GetCragsAsQueryable(filter);
                KeysetPaginateView<TView> paginatedResultSet = await KeysetPaginateGetTask<TView>(filter.PageSize, filter.LastItem, crags);
                return paginatedResultSet;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to get crags, failed in service method. Ex: {ex}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// This method is a private helper method used internally by the service to perform the actual pagination logic for crags.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="pageSize"></param>
        /// <param name="lastItem"></param>
        /// <param name="crags"></param>
        /// <returns> View model with appropriate keyset pagination info for crags. </returns>
        private async Task<KeysetPaginateView<TView>> KeysetPaginateGetTask<TView>(int pageSize, Guid? lastItem, IQueryable<Crag> crags) where TView : class
        {
            DateTime? lastItemModifiedAt = null;
            KeysetPaginateInsert paginateInsert = new() { PageSize = pageSize, LastItem = lastItem };
            if (paginateInsert.LastItem.HasValue)
            {
                Crag lastClimb = await _cragRepo.GetByIdAsync(paginateInsert.LastItem.Value);
                lastItemModifiedAt = lastClimb.ModifiedDate;
            }
            KeysetPaginateView<TView> paginatedResultSet = await _cragRepo.GetKeysetPaginatedResultSet<DateTime, TView>(
                crags,
                s => s.ModifiedDate < lastItemModifiedAt,
                s => s.ModifiedDate,
                s => s.Id,
                paginateInsert);
            return paginatedResultSet;
        }

        /// <summary>
        /// This method retrieves crags from the repository based on the provided KeysetCragFilter and returns them as an IQueryable(Crag).
        /// </summary>
        /// <param name="filter"></param>
        /// <returns> IQueryable of crags, filtered based on provided filter. </returns>
        private IQueryable<Crag> GetCragsAsQueryable(KeysetCragFilter filter)
        {
            IQueryable<Crag> crags = _cragRepo.FindAllAsQueryable(c =>
                (filter.Name == null || c.Name.StartsWith(filter.Name)) &&
                (filter.CountryCode == null || c.CountryCode == filter.CountryCode)
                );
            crags = crags.Where(c =>
                                c.Tags
                                    .Where(t => filter.Tags == null || filter.Tags.Contains(t.TagType.Id))
                                    .Select(t => t.TagType)
                                    .Distinct()
                                    .Count() == (filter.Tags != null ? filter.Tags.Count : 0));

            return crags;
        }

        /// <summary>
        /// This method retrieves a crag by its unique identifier (cragId) and returns the corresponding view model.
        /// </summary>
        /// <param name="cragId"></param>
        /// <returns> View model of the crag requested. </returns>
        /// <exception cref="KeyNotFoundException"></exception>
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

        /// <summary>
        /// This method creates a new crag based on the provided CragModel. It initializes a new Crag entity, sets its properties, and saves it to the repository.
        /// </summary>
        /// <param name="model"></param>
        /// <returns> Guid of the created crag. </returns>
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

        /// <summary>
        /// This method updates an existing crag based on the provided CragModel. It retrieves the crag from the repository, updates its properties, and saves the changes back to the repository.
        /// </summary>
        /// <param name="model"></param>
        /// <returns> Guid of the updated crag. </returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
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

        /// <summary>
        /// This method deletes a crag based on the provided cragId. It retrieves the crag from the repository and deletes it, along with any associated coordinates.
        /// </summary>
        /// <param name="cragId"></param>
        /// <returns> Bool indicating success. Mostly there to avoid void task return errors. </returns>
        /// <exception cref="KeyNotFoundException"></exception>
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

        //The static variable methods are similar again, however, they only have get methods as they are static and don't require creation, updating, or deletion.
        //A future version of the application may include the ability to add new grading systems, grades, or wall types, but for now, they are static and only need to be retrieved as any addition of such would require an admin.
        #region Static Variable Methods
        /// <summary>
        /// This method retrieves a list of Tags for either Climbs or Crags depending on the boolean provided.
        /// </summary>
        /// <param name="forClimbs"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TagModel>> GetTags(bool forClimbs)
        {
            IQueryable<Tag>? tags = null;
            if (forClimbs)
                tags = _tagRepo.FindAllAsQueryable(t => t.Climb != null);

            else
                tags = _tagRepo.FindAllAsQueryable(t => t.Crag != null);

            IEnumerable<TagModel> models = _mapper.Map<IEnumerable<Tag>, IEnumerable<TagModel>>(tags.ToList());

            return models;
        }

        /// <summary>
        /// This method retrieves a list of grading systems from the repository and maps them to GradingSystemModel objects. It returns the result as an IEnumerable(GradingSystemModel).
        /// </summary>
        /// <returns> IEnumerable of the grading system view models. </returns>
        /// <exception cref="KeyNotFoundException"></exception>
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

        /// <summary>
        /// This method retrieves a list of grades associated with a specific grading system, identified by the provided gradingSystemId. 
        /// It fetches the grades from the repository, maps them to GradeModel view model, and returns the result.
        /// </summary>
        /// <param name="gradingSystemId"></param>
        /// <returns> IEnumerable of the grade view models. </returns>
        /// <exception cref="KeyNotFoundException"></exception>
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

        /// <summary>
        /// This method retrieves a list of wall types from the repository and maps them to WallTypeModel view models. It returns the result as an IEnumerable(WallTypeModel).
        /// </summary>
        /// <returns> IEnumerable of the wall type view models. </returns>
        /// <exception cref="KeyNotFoundException"></exception>
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
