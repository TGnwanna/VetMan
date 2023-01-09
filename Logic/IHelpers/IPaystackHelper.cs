using Core.Models;
using Core.ViewModels;
using static Core.Models.PaystackResponse;

namespace Logic.IHelpers
{
	public interface IPaystackHelper
	{

		PaystackRepsonse MakeOrderPayment(WalletHistoryViewModel wallet);
		Task<PaystackRepsonse> VerifyPayment(Paystack payment);
		string GeneratePaymentParameterForGuest(GuestBooking guestBooking);
        PaystackRepsonse SubscriptionPayment(double amount, Guid? Id);
        Task<PaystackRepsonse> VerifyPayment(PaystackSubscription payment);
		Task<PaystackSubscription> UpdateSubscriptionResponse(PaystackRepsonse PaystackRepsonse);

    }
}
