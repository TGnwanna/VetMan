using Core.Db;
using Core.ViewModels;
using Logic.Helpers;
using Logic.IHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Vetman.Controllers
{
	public class RoutineController : Controller
	{
		private readonly IUserHelper _userHelper;
		private readonly IAdminHelper _adminHelper;
		private readonly AppDbContext _context;

		public RoutineController(IUserHelper userHelper, IAdminHelper adminHelper, AppDbContext context)
		{
			_userHelper = userHelper;
			_adminHelper = adminHelper;
			_context = context;
		}

		public IActionResult Index()
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			var myRoutine = _adminHelper.GetRoutineList();
			if (myRoutine != null && myRoutine.Count > 0)
			{
				return View(myRoutine);
			}
			return View(myRoutine);
		}

		[HttpPost]
		public JsonResult CreateRoutine(string routineDetails)
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			if (routineDetails != null)
			{
				var routineViewModel = JsonConvert.DeserializeObject<RoutineViewModel>(routineDetails);
				if (routineViewModel != null)
				{
					var createRoutine = _adminHelper.CreateRoutine(routineViewModel);
					if (createRoutine)
					{
						return Json(new { isError = false, msg = "Routine created successfully" });

					}
					return Json(new { isError = true, msg = "Routine name already exist" });
				}
				return Json(new { isError = true, msg = "Unable to create Routine" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}


		[HttpPost]
		public JsonResult EditRoutine(string routineDetails)
		{
			if (routineDetails != null)
			{
				var routeViewModel = JsonConvert.DeserializeObject<RoutineViewModel>(routineDetails);
				if (routeViewModel != null)
				{
					var createRoute = _adminHelper.EditRoutine(routeViewModel);
					if (createRoute)
					{
						return Json(new { isError = false, msg = "Routine Updated successfully" });
					}
				}
				return Json(new { isError = false, msg = "Unable to update Routine" });
			}
			return Json(new { isError = false, msg = "Error Occurred" });
		}

		[HttpPost]
		public JsonResult DeleteRoutine(string routineDetails)
		{
			if (routineDetails != null)
			{
				var routeViewModel = JsonConvert.DeserializeObject<RoutineViewModel>(routineDetails);
				if (routeViewModel != null)
				{
					var createRoute = _adminHelper.DeleteRoutine(routeViewModel);
					if (createRoute)
					{
						return Json(new { isError = false, msg = "Routine Deleted successfully" });

					}
				}
				return Json(new { isError = false, msg = "Unable to Delete Routine" });
			}
			return Json(new { isError = false, msg = "Error Occurred" });
		}


		[HttpGet]
		public JsonResult GetRoutineByID(string routineID)
		{
			if (routineID != null)
			{
				var routineId = JsonConvert.DeserializeObject<Guid>(routineID);
				var routine = _context.Routines.Where(c => c.Id == routineId).FirstOrDefault();
				if (routine != null)
				{
					return Json(routine);
				}
			}
			return null;

		}
	}
}
