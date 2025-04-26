using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Contracts;
using UI.Models.DonationRequest;

namespace UI.Controllers
{
	public class DonationRequestsController : Controller
	{
		private readonly IBloodTypeService _bloodTypeService;

		public DonationRequestsController(IBloodTypeService bloodTypeService)
		{
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
			return View();
		}
	}
}