using Application.Contracts.Persistence;

using Domain;


namespace Persistence.Repositories;

public class BloodTypeRepository(DonorDbContext dbContext) : GenericRepository<BloodType>(dbContext), IBloodTypeRepository;