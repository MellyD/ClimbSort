using FontRecommender.Core.ViewModels.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FontRecommender.Data.Repository
{
    public interface IRepository<C, TEntity> where TEntity : class where C : DbContext
    {
        C FontRecommenderDBContext();
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetByIdAsync(Guid id);
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> FindAllAsQueryable(Expression<Func<TEntity, bool>> predicate);
        (IQueryable<TView> paginatedResultSet, int numberOfPages) GetPaginatedQueryable<TView>(IQueryable<TView> resultSet, OffsetPaginateInsert paginateModel) where TView : class;
        Task<OffsetPaginateView<TView>> GetOffsetPaginatedResultSet<V, TView>(IQueryable<TEntity> resultSet, Expression<Func<TEntity, V>> sortingPredicate, OffsetPaginateInsert paginateModel) where TView : class;
        Task<KeysetPaginateView<TView>> GetKeysetPaginatedResultSet<V, TView>(IQueryable<TEntity> resultSet, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, V>> sortingPredicate, Expression<Func<TEntity, Guid>> idSortingPredicate, KeysetPaginateInsert keysetPaginateInsert) where TView : class;
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
