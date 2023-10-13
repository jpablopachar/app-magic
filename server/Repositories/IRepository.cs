using System.Linq.Expressions;
using server.Specifications;

namespace server.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task Create(T entity);

        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

        PagedList<T> GetAllPaged(Parameters parameters, Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

        Task<T?> Get(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null);

        Task Remove(T entity);

        Task Save();
    }
}