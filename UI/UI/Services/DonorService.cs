using AutoMapper;
using UI.Contracts;
using UI.Models.Donors;
using UI.Services.Base;

namespace UI.Services;

public class DonorService : BaseHttpService, IDonorService
{
	private readonly IClient _client;
	private readonly IMapper _mapper;

	public DonorService(IClient client,
						IMapper mapper,
						ILocalStorageService localStorage) : base(client, localStorage)
	{
		_client = client;
		_mapper = mapper;
	}

	public async Task<DonorVM> DonorInformation()
	{
		AddBearerToken();
		var response = await _client.Donor_informationAsync();

		return _mapper.Map<DonorVM>(response.Result);
	}

	public async Task<DonorVM> UpdateDonorInformation(DonorVM donorVM)
	{
		var request = _mapper.Map<UpdateDonorRequestDTO>(donorVM);

		AddBearerToken();
		var response = await _client.Update_donor_infoAsync(request);

		return _mapper.Map<DonorVM>(response.Result);
	}
}