using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Paystack
    {

        public Guid Id { get; set; }

        public decimal? amount { get; set; }

        public Guid? WalletHistoryId { get; set; }
        [Display(Name = "Wallet History")]
        [ForeignKey("WalletHistoryId")]
        public WalletHistory? WalletHistory { get; set; }
        public Guid? GuestBookingId { get; set; }
        [ForeignKey("GuestBookingId")]
        public GuestBooking? GuestBooking { get; set; }
        public string? currency { get; set; }
        public DateTime? transaction_date { get; set; }
        public string? status { get; set; }
        public string? reference { get; set; }
        public string? domain { get; set; }
        public string? gateway_response { get; set; }
        public string? message { get; set; }
        public string? channel { get; set; }
        public string? ip_address { get; set; }
        public string? fees { get; set; }
        public string? last4 { get; set; }
        public string? exp_month { get; set; }
        public string? exp_year { get; set; }
        public string? card_type { get; set; }
        public string? bank { get; set; }
        public string? country_code { get; set; }
        public string? brand { get; set; }
        public bool? reusable { get; set; }
        public string? signature { get; set; }
        public string? authorization_url { get; set; }
        public string? access_code { get; set; }
    }
}
