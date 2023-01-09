using Core.Models;
using Core.ViewModels;

namespace Logic.IHelpers
{
    public interface IEmailHelper
    {
        void GetCustomerAndSendMessage(BookingGroupViewModel groupViewModel);
        bool SendStaffEmail(ApplicationUserViewModel userDetail);
        bool PasswordResetConfirmation(ApplicationUser applicationUser);
        Task<UserVerification> CreateUserToken(string userEmail);
        bool PasswordResetLink(ApplicationUser applicationUser, string linkToClick);
        void NotifyCompnayOfSubScription(CompanyModule companyModule);
        bool SendSubscriptionMail(PaystackSubscription subscription);
    }
}
