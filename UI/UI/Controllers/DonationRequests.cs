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
		return View();
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
}