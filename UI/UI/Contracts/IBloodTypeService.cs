using UI.Services.Base;

namespace UI.Contracts;

public interface IBloodTypeService
{
	Task<ICollection<BloodTypeDTO>> GetBloodTypes();
}