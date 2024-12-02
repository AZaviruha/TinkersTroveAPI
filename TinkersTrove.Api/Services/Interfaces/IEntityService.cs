using System.Linq.Expressions;

namespace TinkersTrove.Api.Services.Interfaces;

public interface IEntityService<T>
{
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
    
    Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true);
    
    Task<T> CreateAsync(T entity);
    
    Task RemoveAsync(T villa);
    
    Task SaveAsync();
}
