using AutoMapper;
using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Core.Enums.VetManEnums;
using static Core.Models.PaystackResponse;

namespace Logic.Helpers
{
	public class SubscriptionHelper : ISubscriptionHelper
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserHelper _userHelper;
        private readonly IPaystackHelper _paystackHelper;
        private readonly IEmailHelper _emailHelper;

        public SubscriptionHelper(AppDbContext context, IMapper mapper, IUserHelper userHelper, IPaystackHelper paystackHelper, IEmailHelper emailHelper)
        {
           _context = context;
            _mapper = mapper;
            _userHelper = userHelper;
            _paystackHelper = paystackHelper;
            _emailHelper = emailHelper;
        }

        public ModulesData GetAllModule()
        {
            var moduleDatas = new ModulesData();
			moduleDatas.ModuleCost = new List<ModuleCostViewModel>();
			moduleDatas.Plans = new List<CompanyModuleViewModel>();
			var avaliableModule = new List<ModuleCostViewModel>();
            var subPlans = new List<CompanyModuleViewModel>();
            var modules = _context.ModuleCosts.Where(c => c.Active).ToList();
            if (modules.Count > 0)
            {
                var loggedInUser = Utitily.GetCurrentUser();
                foreach (var module in modules) { 
                    var companySubscription = _context.CompanyModules.Where(x => x.ModuleCostId == module.Id 
                            && x.CompanyId == loggedInUser.CompanyBranch.CompanyId && x.SubcriptionStatus == CompanySubcriptionStatus.Active).FirstOrDefault();
                    if (companySubscription != null)
                    {
                        if (companySubscription.SubcriptionStatus == CompanySubcriptionStatus.Active)
                        {
                            var compModule = new ModuleCostViewModel()
                            {
                               
                                CompanyModuleId = companySubscription.Id,
                                Discription = module.Discription,
                                ModuleId = module.ModuleId,
                                NoOfDays = module.NoOfDays,
                                Price = module.Price,
                                Id = module.Id,
                            };
                            avaliableModule.Add(compModule);

                            var currentplan = new CompanyModuleViewModel()
                            {
                                Active = true,
                                Name = GetEnumDescription((CompanySettings)module.ModuleId),
                                ModuleId = companySubscription.ModuleId,
                                Duration = module.NoOfDays,
                                ModuleCostId = module.Id,
                                Cost = module.Price,
                                SubcriptionStatus = CompanySubcriptionStatus.Active,
                                Id = module.Id,
                                StartDate = companySubscription.StartDate,
                                ExpiryDate = companySubscription.ExpiryDate,
                            };
                            subPlans.Add(currentplan);
                           
                        }
                    }
                    else
                    {
                        var compModule = new ModuleCostViewModel()
                        {
                            Active = false,
                            Discription = module.Discription,
                            ModuleId = module.ModuleId,
                            NoOfDays = module.NoOfDays,
                            Price = module.Price,
                            Id = module.Id,
                        };
                        avaliableModule.Add(compModule);
                    }
                }
                moduleDatas.ModuleCost = avaliableModule;
                moduleDatas.Plans = subPlans;
                return moduleDatas;
            }
            return moduleDatas;
        }

        public string CreateSubscriber(List<Guid> moduleIds)
        {
            if (moduleIds.Count > 0)
            {
                var loggedInUser = Utitily.GetCurrentUser();
                if (loggedInUser.Id == null)
                {
                    loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;

                }
                var totalAmount = 0.0;
                var selectedModules = new List<CompanyModule>();
                foreach (var id in moduleIds)
                {
                    var subscribered = _context.CompanyModules.Where(x => x.ModuleCostId == id && x.SubcriptionStatus == CompanySubcriptionStatus.Active).FirstOrDefault();
                    var module = _context.ModuleCosts.Where(x => x.Id == id).FirstOrDefault();
                    if (module != null)
                    {
                        if (subscribered != null)
                        {
                            var daysLeft = DateCalculate(DateTime.Now, subscribered.ExpiryDate);
                            var expiryDate = module.NoOfDays + daysLeft;
                            var companyModule = new CompanyModule()
                            {
                                CompanyId = loggedInUser?.CompanyBranch?.CompanyId,
                                StartDate = DateTime.Now,
                                ExpiryDate = DateTime.Now.AddDays(expiryDate),
                                ModuleId = module.ModuleId,
                                SubcriptionStatus = CompanySubcriptionStatus.Pending,
                                ModuleCostId = id
                            };
                            selectedModules.Add(companyModule);
                            totalAmount += module.Price;
                        }
                        else
                        {
                            var companyModule = new CompanyModule()
                            {
                                CompanyId = loggedInUser?.CompanyBranch?.CompanyId,
                                StartDate = DateTime.Now,
                                ExpiryDate = DateTime.Now.AddDays(module.NoOfDays),
                                ModuleId = module.ModuleId,
                                SubcriptionStatus = CompanySubcriptionStatus.Pending,
                                ModuleCostId = id
                            };
                            selectedModules.Add(companyModule);
                            totalAmount += module.Price;
                        }
                    } 

                }
                _context.CompanyModules.AddRange(selectedModules);
                _context.SaveChanges();
                
                var paystackResponse = _paystackHelper.SubscriptionPayment(totalAmount, loggedInUser?.CompanyBranch?.CompanyId);
                   
                if (paystackResponse != null)
                {
                    PaystackSubscription paystack = new PaystackSubscription()
                    {
                        CompanyId = loggedInUser?.CompanyBranch?.CompanyId,
                        authorization_url = paystackResponse.data.authorization_url,
                        access_code = paystackResponse.data.access_code,
                        amount = Convert.ToDecimal(totalAmount),
                        transaction_date = DateTime.Now,
                        reference = paystackResponse.data.reference,
                    };
                    _context.PaystackSubscriptions.Add(paystack);
                    _context.SaveChanges();
                    return paystackResponse.data.authorization_url;
                }
            }
            return null; 
        }

        public bool GetPaymentResponse(PaystackSubscription paystack)
        {
            var paymentStatus = _context.PaystackSubscriptions.Where(x => x.reference == paystack.reference).Include(c => c.Company).FirstOrDefault();
            if (paymentStatus != null && paymentStatus.Company != null)
            {
                var completedpayment = _paystackHelper.VerifyPayment(paymentStatus);
               // _emailHelper.SendSubscriptionMail(paymentStatus);
                return true;
            }
            return false;
        }


        public double DateCalculate(DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom != DateTime.MinValue && dateTo != DateTime.MinValue)
            {
                //Creating object of TimeSpan Class  
                TimeSpan objTimeSpan = dateTo - dateFrom;
                //years  
                int Years = dateTo.Year - dateFrom.Year;
                //months  
                int month = dateTo.Month - dateFrom.Month;
                //TotalDays  
                double Days = Convert.ToDouble(objTimeSpan.TotalDays);
                //Total Months  
                int TotalMonths = (Years * 12) + month;
                //Assining values to td tags  
                var year = Years + "  Year  " + month + "  Months";
                return Days;
            }
            return 0;
        }

        public  string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                var des =  attributes.First().Description;
                return des;
            }

            return value.ToString();
        }
    }
}
