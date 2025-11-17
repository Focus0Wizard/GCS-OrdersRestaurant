using Microsoft.EntityFrameworkCore;
using Restaurant.Context;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;

namespace Restaurant.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly MyDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(MyDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(short id)
        {
            var entity = await _dbSet.FindAsync(id);
            var prop = typeof(T).GetProperty("Estado");
            if (prop != null && entity != null)
            {
                var estado = (sbyte?)prop.GetValue(entity);
                if (estado != 1)
                    return null;
            }
            return entity;
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            var prop = typeof(T).GetProperty("Estado");
            var query = _dbSet.AsQueryable().Where(predicate);

            if (prop != null)
                query = query.Where(e => EF.Property<sbyte?>(e, "Estado") == 1);

            return await query.ToListAsync();
        }

        public async Task SoftDeleteAsync(T entity)
        {
            var prop = entity.GetType().GetProperty("Estado");
            if (prop != null)
            {
                if (prop.PropertyType == typeof(sbyte?) || prop.PropertyType == typeof(sbyte))
                {
                    prop.SetValue(entity, (sbyte)0);
                }
                else
                {
                    prop.SetValue(entity, 0);
                }

                _context.Update(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
