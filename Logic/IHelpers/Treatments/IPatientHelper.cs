using Core.Models;
using Core.ViewModels.Treatments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IHelpers.Treatments
{
	public interface IPatientHelper
	{
        Task<Patient> GetPatinetByName(object Name);
        Task<bool> CreatePatientRecord(PatientViewModel patient);
       
        List<PatientViewModel> GetPatients();
        EditPatientViewModel getPatientById(int id);
        bool DeletePatient(PatientViewModel patientDetails);
        bool EditPatients(PatientEditViewModelDto patientDetails);

	}
}
