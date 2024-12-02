using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TinkersTrove.Api.DAL;
using TinkersTrove.Api.Services.Interfaces;

namespace TinkersTrove.Api.Services.Implementations;

public class EntityService<T>(ApplicationDbContext db) : IEntityService<T>
    where T : class
{
    private DbSet<T> _dbSet = db.Set<T>();
    
    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true)
    {
        IQueryable<T> query = _dbSet;

        if (!tracked)
        {
            query = query.AsNoTracking();
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveAsync();
        
        return entity;
    }
    
    public async Task RemoveAsync(T entity)
    {
        _dbSet.Remove(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await db.SaveChangesAsync();
    }
}