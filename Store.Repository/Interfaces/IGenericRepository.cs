using Store.Data.Entites;
using Store.Repository.Specefication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Interfaces
{
    public interface IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey? id);
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<IReadOnlyList<TEntity>> GetAllNoTrackingAsync();
        Task<TEntity> GetBySpecificatiobIdAsync(ISpecefication<TEntity> speces);
        Task<IReadOnlyList<TEntity>> GetAllwithspecificationAsync(ISpecefication<TEntity> speces);
        Task<int> GetCountwithSpecificationAsync(ISpecefication<TEntity> speces);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

    }
}
