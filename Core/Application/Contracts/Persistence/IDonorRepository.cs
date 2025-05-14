using Domain;
using NetTopologySuite.Geometries;


namespace Application.Contracts.Persistence;

public interface IDonorRepository : IGenericRepository<Donor>
{
	Task<IEnumerable<Donor>> GetDonorsByBloodTypeAndLocationAsync(List<int> bloodTypeId, Point location, CancellationToken cancellationToken);
	Task<Donor> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
