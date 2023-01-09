using AutoMapper;
using Core.Db;
using Core.Enums;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.Treatments;
using Logic.IHelpers;
using Logic.IHelpers.Treatments;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Helpers.Treatments
{
    public class PatientHelper : IPatientHelper
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserHelper _userHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PatientHelper(AppDbContext context, IMapper mapper, IUserHelper userHelper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _userHelper = userHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Patient> GetPatinetByName(object Name)
        {
            var patient = new Patient();
            var loggedInUser = Utitily.GetCurrentUser();
            if (Name != null)
                patient = await _context.Patients.Where(x => x.Name == Name && !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId)
                                        .FirstOrDefaultAsync().ConfigureAwait(false);
            return patient;
        }

        public async Task<bool> CreatePatientRecord(PatientViewModel patient)
        {
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
                loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

            }
            if (loggedInUser != null)
            {
                var count = _context.Patients.Where(x => x.CompanyBranchId == loggedInUser.CompanyBranchId && x.DateCreated.Date == DateTime.Today).Count();
                var dateConvert = DateTime.Now.ToString("d");
                int sequence = 1000 + (count+ 1);
                var num = sequence.ToString().Substring(1);                
                var caseNumber = dateConvert.Replace("/", "") + "-" + num;
                var myPatient = new Patient()
                {
                    Id = patient.Id,
                    Name = patient.Name,
                    Deleted = false,
                    BreedId = patient.BreedId,
                    DOB = DateTime.Parse(patient?.BirthDay),
                    CaseNumber = caseNumber,
                    Colour = patient.Colour,
                    CustomerId = patient.CustomerId,
                    GenderId = patient.GenderId,
                    Picture = UploadedFile(patient?.Image),
                    SpecialMarking = patient.SpecialMarking,
                    SpeciesId = patient.SpecieId,
                    CompanyBranchId = loggedInUser.CompanyBranchId,
                    Active = true,
                    DateCreated = DateTime.Now,
                };
                _context.Patients.Add(myPatient);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<PatientViewModel> GetPatients()
        {
            var bb = new List<PatientViewModel>();
            var loggedInUser = Utitily.GetCurrentUser();
            var patients = _context.Patients.Where(x => !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId)
                                            .Include(c => c.Breed).Include(y => y.Customer).Include(y => y.Gender).ToList();
            var myPatients = _mapper.Map<List<PatientViewModel>>(patients).ToList();
            if (myPatients.Count > 0)
            {
                return myPatients;
            }
            return bb;
        }

        
        public string UploadedFile(IFormFile image)
        {
            string uniqueFileName = string.Empty;
            string base64ImageRepresentation = string.Empty;
            if (image != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                string pathString = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(pathString))
                {
                    Directory.CreateDirectory(pathString);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
                byte[] imageArray = File.ReadAllBytes(filePath);
                base64ImageRepresentation = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(imageArray));
            }
            return base64ImageRepresentation;
        }

        public EditPatientViewModel getPatientById (int id)
        {
            var details = new EditPatientViewModel();
            var info = _context.Patients.Where(x => x.Id == id).Include(f => f.Breed)
                                        .Include(z => z.Gender).FirstOrDefault();
            if (info != null)
            {
                details.Patient = info;
                var breeds = _context.Breeds.Where(x => x.SpeciesId == info.SpeciesId && !x.Deleted).ToList();
                if (breeds.Count > 0)
                {
                    details.Breed = breeds;
                }
            }
                return details;
            return null;
        }


		public bool EditPatients(PatientEditViewModelDto patientDetails)
		{
			if (patientDetails != null)
			{ 
				var patientToEdit = _context.Patients.Where(c => c.Id == patientDetails.Id).Include(c => c.Breed).FirstOrDefault();
				if (patientToEdit != null)
				{
					patientToEdit.BreedId = patientDetails.BreedId;
					patientToEdit.GenderId = patientDetails.GenderId;
					patientToEdit.DOB = DateTime.Parse(patientDetails?.BirthDay);
					patientToEdit.SpeciesId = patientDetails.SpecieId;
					patientToEdit.Picture = patientDetails.Picture != null? patientDetails.Picture: patientToEdit.Picture;
					patientToEdit.Colour = patientDetails.Colour;
					patientToEdit.Name = patientDetails.Name;
					patientToEdit.SpecialMarking = patientDetails.SpecialMarking;

					_context.Patients.Update(patientToEdit);
					_context.SaveChanges();
					return true;
				}
			}
			return false;
		}

		public bool DeletePatient(PatientViewModel patientDetails)
		{
			if (patientDetails != null)
			{
				var patientToDelete = _context.Patients.Where(c => c.Id == patientDetails.Id && c.Active && !c.Deleted).FirstOrDefault();
				if (patientToDelete != null)
				{
					patientToDelete.Active = false;
					patientToDelete.Deleted = true;
				}
				_context.Patients.Update(patientToDelete);
				_context.SaveChanges();
				return true;
			}
			return false;
		}




	}
}
