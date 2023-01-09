using Core.Db;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
	[SessionTimeout]
	public class MeasurementUnitController : Controller
	{

		private readonly IUserHelper _userHelper;
		private readonly IAdminHelper _adminHelper;
		private readonly AppDbContext _context;

		public MeasurementUnitController(IUserHelper userHelper, IAdminHelper adminHelper, AppDbContext context)
		{
			_userHelper = userHelper;
			_adminHelper = adminHelper;
			_context = context;
		}

		[HttpGet]
		public IActionResult Index()
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			var myUnit = _adminHelper.GetUnitList();
			if (myUnit != null && myUnit.Count > 0)
			{
				return View(myUnit);
			}
			return View(myUnit);
		}


		[HttpPost]
		public JsonResult CreateMeasurementUnit(string UnitDetails)
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			if (UnitDetails != null)
			{
				var MeasurementUnitViewModel = JsonConvert.DeserializeObject<MeasurementUnitViewModel>(UnitDetails);
				if (MeasurementUnitViewModel != null)
				{
					var createMeasurementUnit = _adminHelper.CreateMeasurementUnit(MeasurementUnitViewModel);
					if (createMeasurementUnit)
					{
						return Json(new { isError = false, msg = "MeasurementUnit created successfully" });

					}
					return Json(new { isError = true, msg = "Unit name already exist" });
				}
				return Json(new { isError = true, msg = "Unable to create MeasurementUnit" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}


		[HttpPost]
		public JsonResult EditMeasurementUnit(string UnitDetails)
		{
			if (UnitDetails != null)
			{
				var measurementUnitViewModel = JsonConvert.DeserializeObject<MeasurementUnitViewModel>(UnitDetails);
				if (measurementUnitViewModel != null)
				{
					var createUnit = _adminHelper.EditMeasurementUnit(measurementUnitViewModel);
					if (createUnit)
					{
						return Json(new { isError = false, msg = "measurementUnit Updated successfully" });
					}
				}
				return Json(new { isError = true, msg = "Unable to update measurementUnit" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}

		[HttpPost]
		public JsonResult DeleteMeasurementUnit(string UnitDetails)
		{
			if (UnitDetails != null)
			{
				var measurementUnitViewModel = JsonConvert.DeserializeObject<MeasurementUnitViewModel>(UnitDetails);
				if (measurementUnitViewModel != null)
				{
					var createUnit = _adminHelper.DeleteMeasurementUnit(measurementUnitViewModel);
					if (createUnit)
					{
						return Json(new { isError = false, msg = "measurementUnit Deleted successfully" });

					}
				}
				return Json(new { isError = true, msg = "Unable to Delete measurementUnit" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}


		public JsonResult GetMeasurementUnitByID(int measurementUnitId)
		{
			if (measurementUnitId != 0)
			{
				var measurementUnit = _context.Measurementunits.Where(c => c.Id == measurementUnitId).FirstOrDefault();
				if (measurementUnit != null)
				{
					return Json(new { isError = false, data = measurementUnit });
				}
			}
			return Json(new { isError = true }); ;

		}

	}
}
