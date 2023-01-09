using AutoMapper;
using Core.Db;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.Treatments;
using Logic.Helpers;
using Logic.IHelpers;
using Logic.IHelpers.Treatments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Core.Enums.VetManEnums;
using static Core.Models.PaystackResponse;

namespace Vetman.Controllers
{
	public class PatientController : Controller
	{
        private readonly IPatientHelper _patient;
        private readonly IMapper _mapper;
        private readonly IUserHelper _userHelper;
        private readonly IBookingHelper _bookingHelper;
        private readonly IDropdownHelper _dropdownHelper;
		private readonly AppDbContext _context;

        public PatientController(IPatientHelper patientHelper, IMapper mapper, IUserHelper userHelper, IBookingHelper bookingHelper, IDropdownHelper dropdownHelper, AppDbContext context)
        {
            _patient = patientHelper;
            _mapper = mapper;
            _userHelper = userHelper;
            _bookingHelper = bookingHelper;
            _dropdownHelper = dropdownHelper;
            _context = context;
        }
        public IActionResult Index()
		{
            ViewBag.Layout = _userHelper.GetRoleLayout();
            ViewBag.Gender = _dropdownHelper.GetDropdownByKey(DropdownEnums.GenderKey).Result;
            ViewBag.Species = _dropdownHelper.GetSpecies();
            var patients = _patient.GetPatients();
            return View(patients);
		}

        public IActionResult AddPatient()
        {
            ViewBag.Layout = _userHelper.GetRoleLayout();
            ViewBag.Customers = _bookingHelper.GetCustomers();
            ViewBag.Gender = _dropdownHelper.GetDropdownByKey(DropdownEnums.GenderKey).Result;
            ViewBag.Species = _dropdownHelper.GetSpecies();
            return View();
        }

        public JsonResult GetBreed(Species id)
        {
            var breeds = _dropdownHelper.GetBreeds(id);
            if (breeds != null)
                return Json(new { isError = false, data = breeds });

            return Json(new { isError = true });
        }


        public JsonResult CreatePatient(string patient, IFormFile picture)
        {
            if (patient != null)
            {
                var patientDetail = JsonConvert.DeserializeObject<PatientViewModel>(patient);
                if (patientDetail != null && patientDetail.IsExistingClient)
                {
                    if (patientDetail.CustomerId == Guid.Empty)
                        return Json(new { isError = true, msg = "Customer is required" });
                }
                if (picture != null)
                    patientDetail.Image = picture;
                
                if (patientDetail?.BreedId == 0)
                    return Json(new { isError = true, msg = "Breed is Required" });

                if (patientDetail?.SpecieId == 0)
                    return Json(new { isError = true, msg = "Specie is Required" });
                if (!patientDetail.IsExistingClient)
                {
                    if (patientDetail?.PhoneNumber == "")
                        return Json(new { isError = true, msg = "Please enter phone Number" });
                } 
                if (!patientDetail.IsExistingClient)
                {
                    var customer = new ClientInfoAndBookingViewModel()
                    {
                        Email = patientDetail.Email,
                        PhoneNumber = patientDetail.PhoneNumber,
                        Address = patientDetail.Address,
                        LastName = patientDetail.LastName,
                        FirstName = patientDetail.FirstName,
                        Profession = patientDetail.Profession,
                        GenderId = patientDetail.GenderId
                    };
                    var customerRecord = _bookingHelper.RegisterClient(customer);
                    if (customerRecord != null)
                    {
                        patientDetail.CustomerId = customerRecord.CustomerId;
                    }            
                }
                var isSuccessful = _patient.CreatePatientRecord(patientDetail).Result;
                if (isSuccessful)
                    return Json(new { isError = false, msg = "Patient created successfully" });
                
                return Json(new { isError = true, msg = "Unable to create Patient." });
            }
            return Json(new { isError = true, msg = "Error Occurred" });
        }

        public JsonResult EditPatient(int id)
        {
            if (id > 0)
            {
                var bookingInfo = _patient.getPatientById(id);
                if (bookingInfo != null)
                    return Json(new { isError = false, data = bookingInfo });
            }
            return Json(new { isError = true, msg = "Invalid selection" });
        }


		[HttpPost]
		public JsonResult EditPatients(string patientDetails)
		{
			if (patientDetails != null)
			{
				var patientViewModel = JsonConvert.DeserializeObject<PatientEditViewModelDto>(patientDetails);
				if (patientViewModel != null)
				{
					var createPatient = _patient.EditPatients(patientViewModel);
					if (createPatient)
					{
						return Json(new { isError = false, msg = "patient Updated successfully", url = "/Patient/Index" });
					}
				}
				return Json(new { isError = true, msg = "Unable to update patient" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}

		[HttpPost]
		public JsonResult DeletePatient(string patientDetails)
		{
			if (patientDetails != null)
			{
				var patientViewModel = JsonConvert.DeserializeObject<PatientViewModel>(patientDetails);
				if (patientViewModel != null)
				{
					var createPatient = _patient.DeletePatient(patientViewModel);
					if (createPatient)
					{
						return Json(new { isError = false, msg = "patient Deleted successfully" });

					}
				}
				return Json(new { isError = true, msg = "Unable to Delete patient" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}


		
	}
}
