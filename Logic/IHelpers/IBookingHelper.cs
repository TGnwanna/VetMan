using Core.Models;
using Core.ViewModels;

namespace Logic.IHelpers
{
    public interface IBookingHelper
    {
        IEnumerable<CustomerBookingReadDto> GetAllBookings();
        List<BookingGroupViewModel> GetBookingGroups();
        List<BookingGroupViewModel> GetGuestBookingGroups(Guid id);

        ClientInfoAndBookingViewModel RegisterClient(ClientInfoAndBookingViewModel customer);

        List<ClientInfoAndBookingViewModel> GetCustomers();

        string CreateClientBookings(ClientInfoAndBookingViewModel bookingDetails);

        List<ProductType> GetProductTypeDropDown();

        CustomerBookingReadDto getBookingForEdit(Guid id);

        bool EditClientBookings(CustomerBookingReadDto customerBooking);
        BookingGroupViewModel GetCurrentUserBookingGroup(Guid id);
        List<BookingGroupViewModel> GetBookingByProductId(int productId);
        List<BookingGroupViewModel> GetBookingGroupsByDateRange(DateTime startDate, DateTime endDate);
        GuestBooking AddBookingForGuest(GuestBookingViewModel guestBookingViewModel);

    }
}
