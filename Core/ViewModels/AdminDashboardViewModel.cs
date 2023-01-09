using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Enums.VetManEnums;

namespace Core.ViewModels
{
	public  class AdminDashboardViewModel
	{
        public List<CustomerViewModel>? Customers { get; set; }

        public int CustomerCount { get; set; }
        public int BookingGroupCount { get; set; }
        public int StaffCount { get; set; }
        public List<BookingGroupViewModel>? BookingGroup { get; set; }
        public List<CompanyBranchViewModel>? Branches { get; set; }
        public List<ApplicationUserViewModel>?  Staff { get; set; }
        public virtual IEnumerable<WalletHistoryViewModel>? Wallets { get; set; }
        public Guid? CompanyId { get; set; }
        public CompanySettings ModuleId { get; set; }
        public CompanySubcriptionStatus SubcriptionStatus { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int StopDate 
        { 
            get
            {
              var daysLeft = (ExpiryDate - DateTime.Now).TotalDays;
                return Convert.ToInt32(daysLeft);
            }
        } 
    }
}
