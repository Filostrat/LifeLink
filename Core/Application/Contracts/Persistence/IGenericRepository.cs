namespace Application.Contracts.Persistence;

public interface IGenericRepository<T> where T : class
{
	Task<T> GetAsync(int id, CancellationToken cancellationToken);
	Task<T> AddAsync(T entity,CancellationToken cancellationToken);
	Task UpdateAsync(T entity, CancellationToken cancellationToke);
	Task DeleteAsync(T entity, CancellationToken cancellationToke);
	Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToke);
	Task<bool> Exists(int id, CancellationToken cancellationToke);
}
