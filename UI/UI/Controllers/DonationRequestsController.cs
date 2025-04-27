using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using UI.Contracts;
using UI.Models.DonationRequest;


namespace UI.Controllers;

[Authorize(Roles = "Administrator")]
public class DonationRequestsController : Controller
{
	private readonly IDonationRequestService _donationRequestService;
	private readonly IBloodTypeService _bloodTypeService;

	public DonationRequestsController(IDonationRequestService donationRequestService, 
									  IBloodTypeService bloodTypeService)
	{
		_donationRequestService = donationRequestService;
		_bloodTypeService = bloodTypeService;
	}

	public async Task<ActionResult> Index()
	{
		var requests = await _donationRequestService.GetAllDonationRequest();
		return View(requests);
	}

	public async Task<ActionResult> Create()
	{
		var types = await _bloodTypeService.GetBloodTypes();

		ViewBag.BloodTypes = new SelectList(types, "Id", "Type");

		return View(new CreateDonationRequestVM());
	}

	[HttpPost]
	public async Task<ActionResult> Create(CreateDonationRequestVM createDonationRequestVM)
	{
		await _donationRequestService.CreateDonationRequest(createDonationRequestVM);

		return View();
	}

	[HttpGet]
	public async Task<ActionResult> Details(int id)
	{
		var vm = await _donationRequestService.GetDonationRequestByIdAsync(id);
		return View(vm);
	}

	[HttpGet]
	public async Task<ActionResult> Repeat(int id)
	{
		var vm = await _donationRequestService.GetDonationRequestByIdAsync(id);

		var createDonationRequestVM = new CreateDonationRequestVM()
		{
			BloodTypeId = vm.BloodTypeId,
			City = vm.City,
			Latitude = vm.Latitude,
			Longitude = vm.Longitude	
		};

		await _donationRequestService.CreateDonationRequest(createDonationRequestVM);
		return RedirectToAction(nameof(Index));
	}
}