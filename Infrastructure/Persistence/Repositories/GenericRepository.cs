using Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading;


namespace Persistence.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
{
	protected readonly DonorDbContext _dbContext;

	public GenericRepository(DonorDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public virtual async Task<T> AddAsync(T entity,CancellationToken cancellationToken)
	{
		await _dbContext.AddAsync(entity,cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);
		return entity;
	}

	public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken)
	{
		_dbContext.Set<T>().Remove(entity);
		await _dbContext.SaveChangesAsync(cancellationToken);
	}

	public virtual async Task<bool> Exists(int id, CancellationToken cancellationToken)
	{
		var entity = await GetAsync(id, cancellationToken);
		return entity != null;
	}

	public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken)
	{
		return await _dbContext.Set<T>().ToListAsync(cancellationToken);
	}

	public virtual async Task<T> GetAsync(int id, CancellationToken cancellationToken)
	{
		return await _dbContext.Set<T>().FindAsync([id], cancellationToken: cancellationToken);
	}

	public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken)
	{
		_dbContext.Entry(entity).State = EntityState.Modified;
		await _dbContext.SaveChangesAsync(cancellationToken);
	}
}