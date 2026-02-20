using Microsoft.EntityFrameworkCore;
using prueba_cajero_api_rest.Domain.Interfaces.Repositories.Base;
using prueba_cajero_api_rest.Infrastructure.Context;

namespace prueba_cajero_api_rest.Infrastructure.Repositories.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
