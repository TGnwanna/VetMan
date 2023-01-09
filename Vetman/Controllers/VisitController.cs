using Core.Config;
using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.Helpers;
using Logic.IHelpers;
using Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Core.Models.PaystackResponse;

namespace Vetman.Controllers
{
    [Authorize]
    public class VisitController : Controller
    {
        private readonly IAdminHelper _adminHelper;
        private readonly IUserHelper _userHelper;
        private readonly  IEmailHelper _emailHelper;
        private readonly AppDbContext _context;
        private readonly IGeneralConfiguration _generalConfiguration;
        private readonly IEmailService _emailService;
        private readonly IDropdownHelper _dropdownHelper;

        public VisitController(AppDbContext context, IUserHelper userHelper, IAdminHelper adminHelper, IEmailHelper emailHelper, IGeneralConfiguration generalConfiguration, IEmailService emailService, IDropdownHelper dropdownHelper)
        {
            _context = context;
            _emailHelper = emailHelper;
            _userHelper = userHelper;
            _adminHelper = adminHelper;
            _generalConfiguration = generalConfiguration;
            _emailService= emailService;
            _dropdownHelper = dropdownHelper;   
        }

        [HttpGet]
        public ActionResult Index()
        {
			ViewBag.Layout = _userHelper.GetRoleLayout();
			var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser != null)
            {
                var patientsVisits = _adminHelper.AllPatientVisits(loggedInUser);
                if (patientsVisits != null)
                {
                    return View(patientsVisits);
                }
                return View(patientsVisits);
            }
            return View();
        }

        [HttpGet]
        public ActionResult PatientVisit(int id)
        {
			ViewBag.Layout = _userHelper.GetRoleLayout();
			var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser != null && id != 0)
            {
                var patientsVisit = _adminHelper.PatientVisitById(id,loggedInUser);
                if (patientsVisit != null)
                {
                    return View(patientsVisit);
                }
                return View(patientsVisit);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
			ViewBag.Patient = _dropdownHelper.GetPatients();
			ViewBag.Layout = _userHelper.GetRoleLayout();
			return View();
        }

        [HttpPost]
        public async Task<JsonResult> AddPatientVisit(string patientVisitDeta)
        {
            try
            {
                var loggedInUser = Utitily.GetCurrentUser();
                if (patientVisitDeta != null && loggedInUser != null)
                {
                    var visitViewModel = JsonConvert.DeserializeObject<VisitViewModel>(patientVisitDeta);
                    if (visitViewModel != null)
                    {
                        var addPatientsHistory =await _adminHelper.AddPatientVisit(visitViewModel, loggedInUser).ConfigureAwait(false);
                        if (addPatientsHistory != null)
                        {
                            var returnUrl = "/Visit/AddVisitTreatment?visitId=" + addPatientsHistory.Id;
							return Json(new { isError = false, msg = "Data uploaded Successfully", returnUrl = returnUrl });
                        }
                    }
                }
                return Json(new { isError = true, msg = "Data upload failed" });
            }
            catch (Exception ex)
            {
                string toEmail = _generalConfiguration.DeveloperEmail;
                string subject = "Exception On AddPatientVisit Controller";
                string message = ex.Message + " , <br /> This exception message occurred while trying to Add Patient Visit";
                _emailService.SendEmail(toEmail, subject, message);
                throw;
            }
        }
        [HttpGet]
        public IActionResult AddVisitTreatment(int visitId)
        {
			ViewBag.Layout = _userHelper.GetRoleLayout();
			ViewBag.Treatment = _dropdownHelper.GetTreatments();
            if (visitId != 0)
            {
                ViewBag.VisitId = visitId;
                return View();
            }
            return View();
        }
        [HttpGet]
        public JsonResult GetTreatmentDetails(int treamentId)
        {
            if (treamentId != 0)
            {
                var treatment = _context.Treatments.Where(x => x.Id == treamentId && !x.Deleted).FirstOrDefault();
                if (treatment != null)
                {
                    return Json(treatment);
                } 
            }
			return Json(new { msg = "Error Occurred" });
		}
        [HttpGet]
		public JsonResult GetPatientDetails(int patientId)
		{
			if (patientId != 0)
			{
				var treatment = _context.Patients.Where(x => x.Id == patientId && !x.Deleted).FirstOrDefault();
				if (treatment != null)
				{
					return Json(treatment.CaseNumber);
				}
			}
			return Json(new { msg = "Error Occurred" });
		}
        [HttpPost]
        public async Task<JsonResult> AddVisitTreatment(string visitTreatments)
        {
            ViewBag.Layout = _userHelper.GetRoleLayout();
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser != null)
            {
				if (visitTreatments != null && visitTreatments != "[]")
				{
                    try
                    {
						var visitTreatmentViewModels = JsonConvert.DeserializeObject<List<VisitTreatmentViewModel>>(visitTreatments);
						if (visitTreatmentViewModels?.Count > 0)
						{
							var addVisitTreatmentPayment = await _adminHelper.AddVisitTreatment(visitTreatmentViewModels, loggedInUser.CompanyBranchId, loggedInUser.Id).ConfigureAwait(false);
							if (addVisitTreatmentPayment)
							{
								return Json(new { isError = false, msg = "Saved Successfully" });
							}
						}
					}
                    catch (Exception ex)
                    {
						string toEmail = _generalConfiguration.DeveloperEmail;
						string subject = "Exception On AddVisitTreatment Controller";
						string message = ex.Message + " , <br /> This exception message occurred while trying to Add Visit Treatment";
						_emailService.SendEmail(toEmail, subject, message);
						throw;
                    }
				}
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}
	}
}
