namespace Application.Contracts.Persistence;

public interface IGenericRepository<T> where T : class
{
	Task<T> GetAsync(int id);
	Task<T> AddAsync(T entity);
	Task UpdateAsync(T entity);
	Task DeleteAsync(T entity);
	Task<IReadOnlyList<T>> GetAllAsync();
	Task<bool> Exists(int id);
}
