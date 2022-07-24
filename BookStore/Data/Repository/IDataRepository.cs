using BookStore.Models;

namespace BookStore.Data.Repository
{
    public interface IDataRepository<TEntity>
    {
        public Task<IEnumerable<TEntity>> GetAll();

        public Task<TEntity> Get(Guid id);

        public Task Add(TEntity entity);

        public Task Delete(TEntity entity);

        public Task Delete(Guid id);

        public Task Update(TEntity entity);

    }
}
