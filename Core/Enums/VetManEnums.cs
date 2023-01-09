using System.ComponentModel;

namespace Core.Enums
{
    public class VetManEnums
    {
        public enum Status
        {
            [Description("For Booked")]
            Booked = 1,
            [Description("For Canceled")]
            Canceled = 2,
            [Description("For Delivered")]
            Delivered = 3,
        }

        public enum TransactionType
        {
            [Description("For Credit")]
            Credit = 1,
            [Description("For Debit")]
            Debit = 2,
            [Description("For Refund")]
            Refund = 3,
        }

        public enum CompanySettings
        {
            [Description("DOC Booking Module")]
            DOCBookingModule,
            [Description("Vaccine Module")]
            VaccineModule,
            [Description("Transaction Module")]
            TransactionModule,
            [Description("Store Module")]
            StoreModule,

        }

        public enum PayStackResponses
        {
            [Description("For InProgress")]
            InProgress = 1,
            [Description("For Completed")]
            Completed = 2,
        }

        public enum ScreenEnums
        {
            [Description("The Page For User Product And Product Type Setup")]
            Settings = 1,

            [Description("The Page To Manage Group")]
            DOCBooking,

            [Description("To Cancel Booking")]
            CancelBooking,

            [Description("To View Transactions")]
            ViewTransactions,

            [Description("To View StoreModule")]
            StoreModule,
        }

        public enum SubscriptionStatus
        {
            [Description("For Active")]
            Active = 1,
            [Description("For Completed")]
            Completed,
            [Description("For Canceled")]
            Canceled
        }

        public enum CompanySubcriptionStatus
        {
            [Description("For Active")]
            Active = 1,
            [Description("For Expired")]
            Expired,
            [Description("For Expired")]
            Pending,
        }

        public enum DropdownEnums
        {
            [Description("For Bank Deposit")]
            BankDeposit = 1,
            [Description("For Cash Deposit")]
            CashDeposit,
            [Description("For returning the user gender")]
            GenderKey,
        }

        public enum ProductActivity
        {
            Newstock = 1,
            Sale,
            Restock,
            Returned,
            Expired
        }

        public enum Species
        {
            [Description("Canis familiaris")]
            Dog = 1,
            [Description("Felis catus")]
            Cat,
            [Description("Bos taurus")]
            Cow,
            [Description("Capra hircus Linnaeus")]
            Goat,
        }

    }
}
