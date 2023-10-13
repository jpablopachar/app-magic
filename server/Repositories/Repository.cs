using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Specifications;

namespace server.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        internal DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task Create(T entity)
        {
            await _dbSet.AddAsync(entity);

            await Save();
        }

        public async Task<T?> Get(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (!tracked) query = query.AsNoTracking();

            if (filter != null) query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null) query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.ToListAsync();
        }

        public PagedList<T> GetAllPaged(Parameters parameters, Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null) query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return PagedList<T>.ToPagedList(query, parameters.PageNumber, parameters.PageSize);
        }

        public Task Remove(T entity)
        {
            _dbSet.Remove(entity);

            return Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}