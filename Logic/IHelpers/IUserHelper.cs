using Core.Models;
using Core.ViewModels;

namespace Logic.IHelpers
{
    public interface IUserHelper
    {
        Task<ApplicationUser> CreateCompanyAdminUser(CompanyViewModel applicationUserView);
        Task<ApplicationUser>? FindByEmailAsync(string email);
        Task<ApplicationUser>? UpdateSessionAsync(string session);


        Task<ApplicationUser> FindByUserNameAsync(string username);

        List<ApplicationUserViewModel> GetStaffList();
        Task<ApplicationUser> CreateStaffDetails(ApplicationUserViewModel applicationUserView);
        ApplicationUser EditStaffDetails(string staffId);
        ApplicationUser EditStaffDetails(ApplicationUserViewModel ApplicationUserDetails);
        bool DeleteStaffDetails(ApplicationUserViewModel ApplicationUserDetails);
        string GetRoleLayout();
        string GetValidatedUrl();
        Task<ImpersonationViewModel> CheckForImpersonation(ApplicationUser loggedInUser);
        ApplicationUser FindById(string Id);
        Task<UserVerification> GetUserToken(Guid token);
        Task<bool> MarkTokenAsUsed(UserVerification userVerification);
        List<SupplierViewModel> GetSupplierList();
        bool CreateSupplierDetails(SupplierViewModel supplierDetails);
        Supplier EditSupplier(int id);
        Supplier SupplierEditedDetails(SupplierViewModel supplierDetails);
        bool DeleteSupplierDetails(SupplierViewModel supplierDetails);
        bool CheckSupplierPhoneNumber(SupplierViewModel supplierDetails);
        List<UserLogsViewModel> GetLogInUserList();
        void CallHangFire(string ipAddress, string userId, string systemName);
        CompanyModule ModulesInfo();
        ApplicationUserViewModel ModulesInfoForStaff();
        List<PaymentMeans> GetPayementMeans(int id);
        List<CommonDropDown> GetBank();
        bool CreateBankDetails(CommonDropDowns bankDetails);
        List<CommonDropDown> GetActiveBanks();
        CommonDropDown DeleteBankDetails(CommonDropDowns? bankData);
        bool CreateBankPaymentMeansDetails(PaymentMeansViewModel paymentMeansViewModel);
        bool CreatePaymentDetails(SalesPaymentViewModel paymentDetails);
        void CreateUserLoginLog(string ipAddress, string userId, string systemName);
    }
}
