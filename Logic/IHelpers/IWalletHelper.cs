using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Models.PaystackResponse;

namespace Logic.IHelpers
{
    public  interface IWalletHelper
    {
        IEnumerable<WalletHistoryViewModel> GetCompanyTransactions();
        Task<PaystackRepsonse> GeneratePaymentParameters(WalletHistoryViewModel wallet);
        bool GetPaymentResponse(Paystack paystack);
        public bool UpdateWalletHistory(BookingGroupViewModel bookingGroup, double amount, Wallet wallet);

        double GetBranchBalance();

    }
}
