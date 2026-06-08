using AutoMapper;
using AutoMapper.QueryableExtensions;
using FontRecommender.Core.ViewModels.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace FontRecommender.Data.Repository
{
    public class Repository<C, TEntity> : IRepository<C, TEntity> where TEntity : class where C : DbContext
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly C _context;
        private readonly IMapper _mapper;

        public Repository(C context, IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _mapper = mapper;
        }
        public C FontRecommenderDBContext()
        {
            try
            {
                return _context;

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the database context.", ex);
            }
        }
        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return _dbSet;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all entities.", ex);
            }
        }
        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            try
            {
                TEntity? entity = await _dbSet.FindAsync(id) ?? throw new KeyNotFoundException($"Could not find a {typeof(TEntity).Name} instance with id {id}");
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the entity by ID.", ex);
            }
        }
        public async Task<TEntity> GetByIdAsync(int id)
        {
            try
            {
                TEntity? entity = await _dbSet.FindAsync(id) ?? throw new KeyNotFoundException($"Could not find a {typeof(TEntity).Name} instance with id {id}");
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the entity by ID.", ex);
            }
        }
        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await _dbSet.FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while finding the entity.", ex);
            }
        }
        public async Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await _dbSet.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while finding the entities.", ex);
            }
        }
        public IQueryable<TEntity> FindAllAsQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return _dbSet.Where(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while finding the entities.", ex);
            }
        }
        public (IQueryable<TView> paginatedResultSet, int numberOfPages) GetPaginatedQueryable<TView>(IQueryable<TView> resultSet, OffsetPaginateInsert paginateModel) where TView : class
        {
            try
            {
                int numberOfPages = resultSet.Count() / paginateModel.PageSize;
                int remainingPages = resultSet.Count() % paginateModel.PageSize;
                numberOfPages = remainingPages > 0 ? numberOfPages + 1 : numberOfPages;
                if (!resultSet.Any())
                {
                    numberOfPages = 1;
                }

                if (paginateModel.PageSize > 100 || paginateModel.PageSize < 1)
                {
                    throw new ArgumentOutOfRangeException("Page size must be between 1 and 100.");
                }
                else if (paginateModel.PageNumber <= 0 || paginateModel.PageNumber > numberOfPages)
                {
                    throw new ArgumentOutOfRangeException($"Page number must be between 1 and {numberOfPages}.");
                }

                IQueryable<TView> paginatedResultSet = resultSet.Skip((paginateModel.PageNumber - 1) * paginateModel.PageSize).Take(paginateModel.PageSize);

                return (paginatedResultSet, numberOfPages);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while finding the entities.", ex);
            }
        }

        public async Task<OffsetPaginateView<TView>> GetOffsetPaginatedResultSet<V, TView>(IQueryable<TEntity> resultSet, Expression<Func<TEntity, V>> sortingPredicate, OffsetPaginateInsert paginateModel) where TView : class
        {
            try
            {
                if (!resultSet.Any())
                {
                    OffsetPaginateView<TView> emptyResult = new OffsetPaginateView<TView>
                    {
                        PageSize = paginateModel.PageSize,
                        PageNumber = paginateModel.PageNumber,
                        ResultSet = [],
                    };
                    return emptyResult;
                }
                (IQueryable<TEntity> paginatedResultSet, int numberOfPages) = GetPaginatedQueryable(resultSet.OrderByDescending(sortingPredicate), paginateModel);
                List<TView> mappedResultSet = [];
                if (typeof(TEntity) != typeof(TView))
                {
                    mappedResultSet = await paginatedResultSet.ProjectTo<TView>(_mapper.ConfigurationProvider).ToListAsync();
                }
                else
                {
                    mappedResultSet = await resultSet.Cast<TView>().ToListAsync();
                }

                return new OffsetPaginateView<TView>
                {
                    PageSize = paginateModel.PageSize,
                    PageNumber = paginateModel.PageNumber,
                    ResultSet = mappedResultSet
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the paginated result set.", ex);
            }
        }

        public async Task<KeysetPaginateView<TView>> GetKeysetPaginatedResultSet<V, TView>(IQueryable<TEntity> resultSet, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, V>> sortingPredicate, Expression<Func<TEntity, Guid>> idSortingPredicate, KeysetPaginateInsert keysetPaginateInsert) where TView : class
        {
            try
            {
                if (keysetPaginateInsert.LastItem.HasValue)
                {
                    resultSet = resultSet.Where(predicate);
                }
                IQueryable<TEntity> paginatedResultSet = resultSet.OrderByDescending(sortingPredicate).ThenByDescending(idSortingPredicate).Take(keysetPaginateInsert.PageSize);

                List<TView> mappedResultSet = [];
                if (typeof(TEntity) != typeof(TView))
                {
                    mappedResultSet = await paginatedResultSet.ProjectTo<TView>(_mapper.ConfigurationProvider).ToListAsync();
                }
                else
                {
                    mappedResultSet = await resultSet.Cast<TView>().ToListAsync();
                }
                KeysetPaginateView<TView> paginateResult = new KeysetPaginateView<TView>
                {
                    HasMore = mappedResultSet.Count == keysetPaginateInsert.PageSize,
                    LastItem = mappedResultSet.Count > 0 ? idSortingPredicate.Compile().Invoke(resultSet.Last()) : null,
                    ResultSet = mappedResultSet
                };
                return paginateResult;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the keyset paginated result set.", ex);
            }
        }

        public async Task<TEntity?> FindInMemoryAsync(Func<TEntity, bool> predicate)
        {
            return await Task.FromResult(
                _dbSet
                    .AsEnumerable()
                    .FirstOrDefault(predicate));
        }

        public async Task CreateAsync(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new InvalidOperationException($"The {typeof(TEntity).Name} entity to create cannot be null.");
                }
                else
                {
                    await _dbSet.AddAsync(entity);
                    await _context.SaveChangesAsync();
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while attempting to create entity.", ex);
            }
        }
        public async Task UpdateAsync(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new InvalidOperationException($"The {typeof(TEntity).Name} entity to update cannot be null.");
                }
                else
                {
                    _dbSet.Update(entity);
                    await _context.SaveChangesAsync();
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while attempting to update entity.", ex);
            }
        }
        public async Task DeleteAsync(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new InvalidOperationException($"The {typeof(TEntity).Name} entity to delete cannot be null.");
                }
                else
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while attempting to delete entity.", ex);
            }
        }
    }
}
