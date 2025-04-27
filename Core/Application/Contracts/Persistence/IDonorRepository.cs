using Domain;
using NetTopologySuite.Geometries;


namespace Application.Contracts.Persistence;

public interface IDonorRepository : IGenericRepository<Donor>
{
	Task<IEnumerable<Donor>> GetDonorsByBloodTypeAndLocationAsync(int bloodTypeId, Point location);
	Task<Donor> GetByEmailAsync(string email);
}
