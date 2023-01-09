using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Db
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<BookingGroup> BookingGroups { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerBooking> CustomerBookings { get; set; }
        public DbSet<CustomerBookingPayment> CustomerBookingPayments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<CompanyBranch> CompanyBranches { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanySetting> CompanySettings { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletHistory> WalletHistories { get; set; }
        public DbSet<Paystack> Paystacks { get; set; }
        public DbSet<ScreenRole> ScreenRoles { get; set; }
        public DbSet<Impersonation> Impersonations { get; set; }
        public DbSet<UserVerification> UserVerifications { get; set; }
        public DbSet<ProductVaccine> ProductVaccines { get; set; }
        public DbSet<VaccineSubscription> VaccineSubscriptions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Measurementunit> Measurementunits { get; set; }
        public DbSet<ShopCategory> ShopCategories { get; set; }
        public DbSet<TransactionSetting> TransactionSettings { get; set; }
        public DbSet<ProductInventory> ProductInventories { get; set; }
        public DbSet<ProductLog> ProductLogs { get; set; }
        public DbSet<SalesLog> SalesLogs { get; set; }
        public DbSet<UserLoginLog> UserLoginLogs { get; set; }
        public DbSet<ModuleCost> ModuleCosts { get; set; }
        public DbSet<ModuleOrderItem> ModuleOrderItems { get; set; }
        public DbSet<PaystackSubscription> PaystackSubscriptions { get; set; }
        public DbSet<CompanyModule> CompanyModules { get; set; }
        public DbSet<GuestBooking> GuestBookings { get; set; }
        public DbSet<SalesPayment> SalesPayments { get; set; }
        public DbSet<CommonDropDown> CommonDropDowns { get; set; }
        public DbSet<PaymentMeans> PaymentMeans { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<Routine> Routines { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<VisitTreatment> VisitTreatments { get; set; }
    }
}
