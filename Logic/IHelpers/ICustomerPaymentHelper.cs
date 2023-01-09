using Core.Models;
using Core.ViewModels;
using Logic.Helpers;

namespace Logic.IHelpers
{
    public interface ICustomerPaymentHelper
    {
        CustomerPaymentGeneralViewModel GetCustomersInGroup(Guid bookingGroupId);
        CustomerPaymentGeneralViewModel GetCustomerBookingPayment(Guid customerBookingId, ApplicationUser loggedInUser);
        CustomerBookingPayment AddPayment(CustomerBookingPaymentViewModel paymentDetails);
        BookingGroup UpdatePrice(BookingGroupViewModel paymentDetails);
        void UpdateGroupExpectedPayment(Guid groupId);
        void UpdateCustomersBookingPayment(Guid customersBookingPaymentId);
        void UpdateGroupPaymenWithHangFire(Guid bookingId);
        BookingGroup UpdateDate(BookingGroupViewModel dateDetails);
        CustomerBookingViewModel GetGroupSummary(Guid groupId);
        Task<bool> CreateCompany(CompanyViewModel companyViewModel);
        List<CompanyViewModel> GetAllCompanies();
        Guid? UpdateCustomerBookingStatus(CustomerBooking customerBooking, SubscriptionOptionsViewModel customerBookings);
        CustomerBooking MoveToNewGroup(CustomerBookingViewModel customerBookingViewModel);
        void SyncCustmerPayment(Guid bookingGroupId, Guid newGroupId, Guid? companyBranchId);
        List<drp> GetAllBookingGroup();
        List<VaccineSubscriptionViewModel> SubscribersList();
        VaccineSubscription GetVaccineSubscriberData(int SubDetails);
        VaccineSubscription EditSubDetails(VaccineSubscriptionViewModel subData);
        VaccineSubscription CancelSubscriber(VaccineSubscriptionViewModel subscriberData);
        VaccineSubscription CompletedSubscriber(VaccineSubscriptionViewModel subscriberData);
        VaccineSubscription DeleteSubscriber(VaccineSubscriptionViewModel subscriberData);
        List<Customer> GetCustomerDropDown();
        List<Product> GetProductDropDown();
        bool CreateCustomerDetails(VaccineSubscriptionViewModel customerDetails);
        List<CustomerBooking> GetBookedCustomer(Guid bookingGroupId, Guid? companyBranchId);
        void CompanyFreeDays(Guid companyId);
    }
}
