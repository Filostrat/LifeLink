using Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Persistence.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
{
	protected readonly DonorDbContext _dbContext;

	public GenericRepository(DonorDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public virtual async Task<T> AddAsync(T entity)
	{
		await _dbContext.AddAsync(entity);
		await _dbContext.SaveChangesAsync();
		return entity;
	}

	public virtual async Task DeleteAsync(T entity)
	{
		_dbContext.Set<T>().Remove(entity);
		await _dbContext.SaveChangesAsync();
	}

	public virtual async Task<bool> Exists(int id)
	{
		var entity = await GetAsync(id);
		return entity != null;
	}

	public virtual async Task<IReadOnlyList<T>> GetAllAsync()
	{
		return await _dbContext.Set<T>().ToListAsync();
	}

	public virtual async Task<T> GetAsync(int id)
	{
		return await _dbContext.Set<T>().FindAsync(id);
	}

	public virtual async Task UpdateAsync(T entity)
	{
		_dbContext.Entry(entity).State = EntityState.Modified;
		await _dbContext.SaveChangesAsync();
	}
}