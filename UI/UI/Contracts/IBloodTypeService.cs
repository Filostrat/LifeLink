using UI.Services.Base;

namespace UI.Contracts;

public interface IBloodTypeService
{
	Task<ICollection<BloodTypeDto>> GetBloodTypes();
}