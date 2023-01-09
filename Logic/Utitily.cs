using Core.Models;
using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using System.Security.Claims;
using System.Text;

namespace Logic
{
	public static class AppHttpContext
	{
		static IServiceProvider services = null;

		/// <summary>
		/// Provides static access to the framework's services provider
		/// </summary>
		public static IServiceProvider Services
		{
			get { return services; }
			set
			{
				if (services != null)
				{
					throw new Exception("Can't set once a value has already been set.");
				}
				services = value;
			}
		}

		/// <summary>
		/// Provides static access to the current HttpContext
		/// </summary>
		public static HttpContext Current
		{
			get
			{
				IHttpContextAccessor httpContextAccessor = services.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
				return httpContextAccessor?.HttpContext;
			}
		}
		public class SessionTimeoutAttribute : ActionFilterAttribute
		{
			public override void OnActionExecuting(ActionExecutingContext filterContext)
			{
				HttpContext ctx = AppHttpContext.Current;
				if (Current.Session.GetString("myuser") == null)
				{
					filterContext.Result = new RedirectResult("~/Account/Login");
					return;
				}
				base.OnActionExecuting(filterContext);
			}
		}
	}
}



public class Utitily
{
	public static ApplicationUser GetCurrentUser()
	{
		var user = AppHttpContext.Current?.Session?.GetString("myuser");
		if (user != null)
		{
			var loggInUser = JsonConvert.DeserializeObject<ApplicationUser>(user);
			return loggInUser;
		}
		var updateSession = new ApplicationUser();
		var userName = AppHttpContext.Current?.User?.Claims?.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
		updateSession.Id = null;
		updateSession.UserName = userName;
		return updateSession;
	}

	public static List<ScreenRole> GetRoleScreen()
	{
		var access = AppHttpContext.Current.Session.GetString("accessRight");
		if (access != null)
		{
			var roleRight = JsonConvert.DeserializeObject<List<ScreenRole>>(access);
			return roleRight;
		}
		return null;
	}
	public static class Constants
	{
		public static string CompanyAdminRole = "CompanyAdmin";
		public static string SuperAdminRole = "SuperAdmin";
		public static string CompanyManagerRole = "CompanyManager";
		public static string CompanyStaffRole = "CompanyStaff";
		public static string DefaultLayout = "~/Views/Shared/_Layout.cshtml";
		public static string SuperAdminLayout = "~/Views/Shared/_SuperAdminLayout.cshtml";
		public static string CompanyAdminLayout = "~/Views/Shared/_CompanyAdminLayout.cshtml";
		public static string AdminLayout = "~/Views/Shared/_AdminLayout.cshtml";
		public static string CompanyMangerLayout = "~/Views/Shared/_CompanyManagerLayout.cshtml";
		public static string CompanyStaffLayout = "~/Views/Shared/_CompanyStaffLayout.cshtml";
	}

	public string GetRoleLayout()
	{
		var loggedInUser = Utitily.GetCurrentUser();
		if (loggedInUser != null)
		{
			var superAdmin = loggedInUser.Roles.Contains(Utitily.Constants.SuperAdminRole);
			if (superAdmin)
			{
				return Utitily.Constants.SuperAdminLayout;
			}
			else if (!superAdmin)
			{
				var isCompanyAdmin = loggedInUser.Roles.Contains(Utitily.Constants.CompanyAdminRole);
				if (isCompanyAdmin)
				{
					return Utitily.Constants.AdminLayout;
				}
				else
				{
					var isBrachManager = loggedInUser.Roles.Contains(Utitily.Constants.CompanyManagerRole);
					if (isBrachManager)
					{
						return Utitily.Constants.CompanyMangerLayout;
					}
					else
					{
						return Utitily.Constants.CompanyStaffLayout;
					}
				}
			}
		}
		return Utitily.Constants.DefaultLayout;
	}

	public bool CheckAdminIsLogin()
	{
		var result = AppHttpContext.Current.Session.GetString("isImpersonating");
		if (result == null)
		{
			return false;
		}
		return true;
	}

	public CompanySetting vaccineSubscription()
	{
		var result = AppHttpContext.Current.Session.GetString("vaccineSubscription");
		if (result != null)
		{
			var companySettings = JsonConvert.DeserializeObject<CompanySetting>(result);
			return companySettings;
		}
		return null;
	}

    public List<Guid> moduleSubscription()
    {
        var result = AppHttpContext.Current.Session.GetString("modules");
        if (result != null)
		{
            var moduleIds = JsonConvert.DeserializeObject<List<Guid>>(result);
            return moduleIds;
        }
        return null;
    }
}


