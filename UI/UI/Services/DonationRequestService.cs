using AutoMapper;

using UI.Contracts;
using UI.Models.DonationRequest;
using UI.Services.Base;


namespace UI.Services;

public class DonationRequestService : BaseHttpService, IDonationRequestService
{
	private readonly IMapper _mapper;

	public DonationRequestService(ILocalStorageService localStorage,
								  IMapper mapper,
								  IClient client) : base(client, localStorage)
	{
		_mapper = mapper;
	}

	public async Task CreateDonationRequest(CreateDonationRequestVM createDonationRequestVM)
	{
		AddBearerToken();
		var map = _mapper.Map<CreateDonationRequestDTO>(createDonationRequestVM);
		await _client.Create_donation_requestAsync(map);
	}

	public async Task<List<DonationRequestVM>> GetAllDonationRequest()
	{
		AddBearerToken();
		var response = (await _client.All_donation_requestsAsync()).Result;
		return _mapper.Map<List<DonationRequestVM>>(response);
	}

	public async Task<DonationRequestVM> GetDonationRequestByIdAsync(int id)
	{
		AddBearerToken();
		var response = ( await _client.Donation_requestAsync(id)).Result;

		return _mapper.Map<DonationRequestVM>(response);
	}
}