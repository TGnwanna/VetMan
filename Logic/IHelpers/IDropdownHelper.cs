using Core.Models;
using Core.ViewModels;
using static Core.Enums.VetManEnums;

namespace Logic.IHelpers
{
    public interface IDropdownHelper
    {
        List<Product> GetProducts();
        List<ApplicationUser> GetAllStaff();
        List<CompanyBranch> GetAllCompanyBranch();

        ProductInventory GetProductDetails(int id);
        List<ModuleDropdownViewModel> GetCompanySetting();

        Task<List<CommonDropDown>> GetDropdownByKey(DropdownEnums dropdownKey, bool deleteOption = false);

        List<ModuleDropdownViewModel> GetSpecies();
        List<Breed> GetBreeds(Species id);
        List<Treatment> GetTreatments();
        List<Patient> GetPatients();


	}
}
