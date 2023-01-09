using Core.Models;
using Core.ViewModels;

namespace Logic.IHelpers
{
    public interface IAdminHelper
    {
        bool CustomerPostServer(CustomerViewModel customerDetails);
        bool GetEditCustomer(CustomerViewModel customerDetails);
        bool DeleteCustomer(CustomerViewModel customerDetails);
        List<Customer> GetCustomerList();
        List<ProductViewModel> GetProductList();
        bool CreateProductDetails(ProductViewModel productDetails);
        bool CreateProductTypeDetails(ProductTypeViewModel productTypeDetails);
        List<ProductTypeViewModel> GetProductTypeList();
        Product EditProductDetails(int productDetails);
        Product EditProductDetails(ProductViewModel productDetails);
        Product DeleteProductDetails(ProductViewModel productDetails);
        ProductType EditProductTypeDetails(int productTypeId);
        ProductType EditProductTypeDetails(ProductTypeViewModel productTypeDetails);
        ProductType DeleteProductTypeDetails(ProductTypeViewModel productTypeDetails);

        bool CreateBookingGroup(BookingGroupViewModel bookingGroupDetails);
        List<BookingGroupViewModel> GetBookingGroupList();
        bool EditBookingGroup(BookingGroupViewModel bookingGroupDetails);
        bool DeleteBookingGroup(BookingGroupViewModel bookingGroupDetails);
        bool UpdateArrivalReceipt(BookingGroupViewModel bookingGroupDetails);
        List<StaffViewModel> GetAvailableRoles();
        StaffViewModel GetAllScreensFromEnum(string roleId);
        string UpdateRoleAccess(StaffViewModel details, List<string> checkedRole, List<string> uncheckedRole);
        List<RoleViewModel> GetStaffRoles();
        string ImpersonateCompanyBranch(Guid companyBranchId);
        bool CreateMeasurementUnit(MeasurementUnitViewModel UnitDetails);
        List<MeasurementUnitViewModel> GetUnitList();
        bool EditMeasurementUnit(MeasurementUnitViewModel UnitDetails);
        bool DeleteMeasurementUnit(MeasurementUnitViewModel UnitDetails);

        List<ShopProductViewModel> GetProductCategoryList();

        bool CreateShopProductDetails(ShopProductViewModel shopProductViewModel);
        ShopCategory EditProductCatDetails(int productCatId);
        bool EditProductCAtDetails(ShopProductViewModel productCatDetails);
        bool DeleteProductCatDetails(ShopProductViewModel productCatDetails);
        List<Supplier> GetSuppliers();
        List<ShopCategory> GetProductCategories();
        List<Measurementunit> GetUnits();
        List<ProductInventory> GetShopProducts();
        List<ShopProductListViewModel> GetShopProductList();
        bool PhoneNumberCheck(CustomerViewModel customerDetails);
        bool EmailCheck(CustomerViewModel customerDetails);
        List<BookingGroupViewModel> GetAllBookingGroup();
        List<ProductInventoryViewModel> GetProductLogView(int id);
        ShopProductListViewModel GetProductInventoryById(int id);
        string EditProductInventory(ShopProductListViewModel shopProductListViewModel);
        string DeleteProductInventory(int id);
        List<VisitViewModel> AllPatientVisits(ApplicationUser loggednUser);
        VisitViewModel PatientVisitById(int patientId, ApplicationUser loggedInUser);
        Task<Visit> AddPatientVisit(VisitViewModel visitViewModel, ApplicationUser loggedInUser);
	
        bool CreateRoutine(RoutineViewModel routineDetails);
        List<RoutineViewModel> GetRoutineList();
        bool EditRoutine(RoutineViewModel routineDetails);
        bool DeleteRoutine(RoutineViewModel routineDetails);
        Task<bool> AddVisitTreatment(List<VisitTreatmentViewModel> visitTreatments, Guid? companyBranchId, string createdById);
        Task<bool> AddPatientsRoutine(VisitViewModel visitViewModel, Guid? companyBranchId);
	}

}

