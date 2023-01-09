using Core.Models;

namespace Core.ViewModels
{
    public class ImpersonationViewModel
    {
        public virtual ApplicationUser AdminBeingImpersonated { get; set; }
        public virtual Impersonation ImpersonationRecord { get; set; }
        public virtual ApplicationUser Impersonator { get; set; }
        public bool IsImpersonatorAdmin { get; set; }
        public string ShowEndSession { get; set; }
        public virtual Company Impersonatee { get; set; }
    }
}
