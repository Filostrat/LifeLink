using Application.Contracts.Persistence;

using Domain;


namespace Persistence.Repositories;

public class DonationRequestNotificationRepository(DonorDbContext dbContext) 
	: GenericRepository<DonationRequestNotification>(dbContext), IDonationRequestNotificationRepository;
