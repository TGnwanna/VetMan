using AutoMapper;
using Core.Config;
using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Logic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Core.Enums.VetManEnums;

namespace Logic.Helpers
{
    public class CompanyHelper : ICompanyHelper
    {
        private readonly IEmailHelper _emailHelper;
        private readonly AppDbContext _context;
		private readonly IUserHelper _userHelper;
		private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGeneralConfiguration _generalConfiguration;
        public CompanyHelper(AppDbContext context, IEmailService emailService, IMapper mapper, IEmailHelper emailHelper, UserManager<ApplicationUser> userManager, IGeneralConfiguration generalConfiguration)
        {
            _context = context;
            _emailHelper = emailHelper;
            _emailService = emailService;
            _mapper = mapper;
            _userManager = userManager;
            _generalConfiguration = generalConfiguration;
        }

        public List<CompanyBranchViewModel> GetCompanyBranchList()
        {
            var branchViewmodel = new List<CompanyBranchViewModel>();
            var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
            {
                var company = _context.CompanyBranches.Where(x => x.Id == loggedInUser.CompanyBranchId && x.CompanyId != Guid.Empty).Include(x => x.Company).FirstOrDefault();
                if (company != null)
                {
                    var branch = _context.CompanyBranches
                    .Where(x => !x.Deleted && x.CompanyId == company.CompanyId)
                    .Select(s => new CompanyBranchViewModel()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Active = s.Active,
                        Address = s.Address,
                        DateCreated = s.DateCreated,
                        CompanyId = company.CompanyId,
                    }).ToList();
                    return branch;
                }
            }
            return branchViewmodel;
        }
        public bool CreateCompanyBranchDetails(CompanyBranchViewModel companyBranchDetails)
        {
            var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
            {
                var company = _context.CompanyBranches.Where(x => x.Id == loggedInUser.CompanyBranchId && x.CompanyId != Guid.Empty).Include(x => x.Company).FirstOrDefault();
                if (company != null)
                {
                    var branchValidation = _context.CompanyBranches.Where(c => c.Name == companyBranchDetails.Name && !c.Deleted && c.Id == loggedInUser.CompanyBranchId).FirstOrDefault();
                    if (branchValidation == null)
                    {
                        var branchModel = new CompanyBranch()
                        {
                            Name = companyBranchDetails.Name,
                            DateCreated = DateTime.Now,
                            Active = true,
                            Deleted = false,
                            Address = companyBranchDetails.Address,
                            CompanyId = company.CompanyId,
                            IsMainBranch = false,
                        };
                        _context.CompanyBranches.Add(branchModel);
                        _context.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }
        public CompanyBranch EditCompanyBranchDetails(Guid CompanyId)
        {
            var companyBranchToEdit = _context.CompanyBranches.Where(x => x.Id == CompanyId && x.Active && !x.Deleted).FirstOrDefault();
            if (companyBranchToEdit != null)
                return companyBranchToEdit;
            return null;
        }
        public CompanyBranch EditedCompanyBranchDetails(CompanyBranchViewModel companyBranchDetails)
        {
            if (companyBranchDetails != null)
            {
                var compDetails = _context.CompanyBranches.Where(x => x.Id == companyBranchDetails.Id && x.Active && !x.Deleted).FirstOrDefault();
                if (compDetails != null)
                {

                    compDetails.Name = companyBranchDetails.Name;
                    compDetails.Address = companyBranchDetails.Address;

                    _context.Update(compDetails);
                    _context.SaveChanges();
                    return compDetails;
                }
            }
            return null;
        }
        public CompanyBranch DeleteCompanyDetails(CompanyBranchViewModel branchDetails)
        {
            if (branchDetails != null)
            {
                var companyBranch = _context.CompanyBranches.Where(x => x.Id == branchDetails.Id && x.Active && !x.Deleted).FirstOrDefault();
                if (companyBranch != null)
                {
                    companyBranch.Deleted = true;
                    companyBranch.Active = false;
                    _context.Update(companyBranch);
                    _context.SaveChanges();
                    return companyBranch;
                }
            }
            return null;
        }
        public async Task<Company> GetCompanyByAdminId(string userId)
        {
            var company = await _context.Companies.Where(x => !x.Deleted && x.Active && x.CreatedById == userId).Include(s => s.CreatedBy).FirstOrDefaultAsync().ConfigureAwait(false);
            if (company != null)
            {
                return company;
            }
            return null;
        }
        public bool DeleteCompany(Guid companyId)
        {
            var company = _context.Companies.Where(x => x.Id == companyId && x.Active).FirstOrDefault();
            if (company != null)
            {
                company.Active = false;
                company.Deleted = true;
                _context.Update(company);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<TransactionViewModel> AllTransactions()
        {
            var transactionList = new List<TransactionViewModel>();
            var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
            {
                var company = _context.CompanyBranches.Where(x => x.Id == loggedInUser.CompanyBranchId && x.CompanyId != Guid.Empty).Include(x => x.Company).FirstOrDefault();
                if (company != null)
                {
                    if (loggedInUser != null && loggedInUser.UserRole == Utitily.Constants.CompanyAdminRole)
                    {
                        var transactions = _context.Transactions.Where(x => x.Id != Guid.Empty && x.CompanyId == company.CompanyId).Include(x => x.ConfirmedBy).ToList();
                        if (transactions != null && transactions.Count > 0)
                        {
                            var messages = transactions.Select(s => new TransactionViewModel()
                            {
                                Id = s.Id,
                                Description = s.Description,
                                ConfirmDate = s.ConfirmDate?.ToString("D"),
                                ReceiptNo = s.ReceiptNo,
                                ConfirmedBy = s.ConfirmedBy?.FullName,

                            }).ToList();
                            return messages;
                        }
                    }
                    else
                    {
                        var transactions = _context.Transactions.Where(x => x.Id != Guid.Empty && x.CompanyId == company.CompanyId && !x.IsConfirmed).ToList();
                        if (transactions != null && transactions.Count > 0)
                        {
                            var messages = transactions.Select(s => new TransactionViewModel()
                            {
                                Id = s.Id,
                                Description = s.Description,
                                ConfirmDate = s.ConfirmDate?.ToString("D"),
                                ReceiptNo = s.ReceiptNo,
                                ConfirmedBy = s.ConfirmedBy?.FullName,

                            }).ToList();
                            return messages;
                        }
                    }
                }
            }
            return transactionList;
        }
        public Transaction SaveIncomingTrasactionMessage(string messageBody, string keyword, string validMessageChecker)
        {

            bool isValidMessage = messageBody.ToLower().Contains(validMessageChecker.ToLower());
            if (isValidMessage)
            {
                var incomingMessage = messageBody.ToLower().Split(keyword.ToLower());
                var messageToSend = incomingMessage[0];
                var transaction = new Transaction()
                {
                    Description = messageToSend,
                    CompanyId = Guid.Parse(_generalConfiguration.CompanyId),
                    DateCreated = DateTime.Now,
                    Active = true,
                    Deleted = false,
                    IsConfirmed = false,
                };
                _context.Add(transaction);
                _context.SaveChanges();
                return transaction;
            }
            return null;
        }
        public CompanySettingViewModel GetCompanyCustomSettings(Guid companyId, List<string> checkedCompanySettings, List<string> uncheckedCompanySettings)
        {
            var setting = new CompanySettingViewModel();
            if (companyId != Guid.Empty)
            {
                var companySettings = _context.CompanySettings.Where(c => c.CompanyId == companyId).Include(x => x.Company).FirstOrDefault();
                if (companySettings != null)
                {
                    foreach (var item in checkedCompanySettings)
                    {
                        if (CompanySettings.DOCBookingModule.ToString().ToLower() == item.ToLower())
                            companySettings.DOCBookingModule = true;

                        if (CompanySettings.VaccineModule.ToString().ToLower() == item.ToLower())
                            companySettings.VaccineModule = true;

                        if (CompanySettings.TransactionModule.ToString().ToLower() == item.ToLower())
                            companySettings.TransactionModule = true;
                        if (CompanySettings.StoreModule.ToString().ToLower() == item.ToLower())
                            companySettings.StoreModule = true;
                    }
                    foreach (var comp in uncheckedCompanySettings)
                    {
                        if (CompanySettings.DOCBookingModule.ToString().ToLower() == comp.ToLower())
                            companySettings.DOCBookingModule = false;
                        if (CompanySettings.VaccineModule.ToString().ToLower() == comp.ToLower())
                            companySettings.VaccineModule = false;
                        if (CompanySettings.TransactionModule.ToString().ToLower() == comp.ToLower())
                            companySettings.TransactionModule = false;
                        if (CompanySettings.StoreModule.ToString().ToLower() == comp.ToLower())
                            companySettings.StoreModule = false;
                    }
                    _context.Update(companySettings);
                    _context.SaveChanges();

                    var settings = new CompanySettingViewModel()
                    {
                        CompanyId = companySettings.CompanyId,
                        CompanyName = companySettings?.Company?.Name,
                        DOCBookingModule = companySettings.DOCBookingModule,
                        VaccineModule = companySettings.VaccineModule,
                        TransactionModule = companySettings.TransactionModule,

                    };
                    return settings;
                }
            }
            return setting;
        }
        public bool ConfirmTransaction(Guid transactionId, string reciept)
        {
            var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
            {
                var company = _context.CompanyBranches.Where(x => x.Id == loggedInUser.CompanyBranchId && x.CompanyId != Guid.Empty).Include(x => x.Company).FirstOrDefault();
                if (company != null)
                {
                    var transaction = _context.Transactions.Where(x => x.Id == transactionId && x.Active && x.CompanyId == company.CompanyId).FirstOrDefault();
                    if (transaction != null)
                    {
                        transaction.ConfirmDate = DateTime.Now;
                        transaction.ReceiptNo = reciept;
                        transaction.ConfirmedById = loggedInUser.Id;
                        transaction.IsConfirmed = true;
                        _context.Update(transaction);
                        _context.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }
        public async Task<CompanySettingViewModel> GetCompanySettings(Guid companyId)
        {
            var settings = new CompanySettingViewModel();
            var company = await GetCompanyById(companyId).ConfigureAwait(false);
            if (company != null)
            {
                var existingSettings = _context.CompanySettings.Where(c => c.CompanyId == companyId).FirstOrDefault();
                if (existingSettings == null)
                {
                    var companySetting = new CompanySetting()
                    {
                        CompanyId = companyId,
                        DOCBookingModule = false,
                        VaccineModule = false,
                        TransactionModule = false,
                        StoreModule = false,
                    };
                    _context.Add(companySetting);
                    _context.SaveChanges();

                    //instantiate the viewmodel for the view
                    settings.CompanyId = company.Id;
                    settings.CompanyName = company.Name;
                    settings.DOCBookingModule = companySetting.DOCBookingModule;
                    settings.VaccineModule = companySetting.VaccineModule;
                    settings.TransactionModule = companySetting.TransactionModule;
                    settings.StoreModule = companySetting.StoreModule;
                    return settings;
                }
                else
                {
                    settings.CompanyId = company.Id;
                    settings.CompanyName = company.Name;
                    settings.DOCBookingModule = existingSettings.DOCBookingModule;
                    settings.VaccineModule = existingSettings.VaccineModule;
                    settings.TransactionModule = existingSettings.TransactionModule;
                    settings.StoreModule = existingSettings.StoreModule;
                }
            }
            return settings;
        }
        public async Task<Company> GetCompanyById(Guid? companyId)
        {
            var company = await _context.Companies.Where(x => x.Id == companyId && x.Active).Include(x => x.CreatedBy).Include(x => x.CreatedBy).FirstOrDefaultAsync();
            if (company != null)
            {
                return company;
            }
            return null;
        }

        public bool SetUpTransaction(TransactionSettingsViewModel transactionSettingsViewModel)
        {
            var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
            {
                var transactionSetting = new TransactionSetting()
                {
                    Name = transactionSettingsViewModel.SenderName,
                    KeyWord = transactionSettingsViewModel.KeyWord,
                    ValidMessageChecker = transactionSettingsViewModel.ValidMessageChecker,
                    DateCreated = DateTime.Now,
                    Active = true,
                    Deleted = false,
                    CompanyBranchId = loggedInUser.CompanyBranchId,
                };
                _context.Add(transactionSetting);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool EditTransactionSetup(TransactionSettingsViewModel transactionSettingsViewModel)
        {
            var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
            {
                var transactionSetting = _context.TransactionSettings.Where(x => x.Id == transactionSettingsViewModel.Id && !x.Deleted).FirstOrDefault();
                if (transactionSetting != null)
                {
                    transactionSetting.Id = transactionSettingsViewModel.Id;
                    transactionSetting.Name = transactionSettingsViewModel.SenderName;
                    transactionSetting.KeyWord = transactionSettingsViewModel.KeyWord;
                    transactionSetting.ValidMessageChecker = transactionSettingsViewModel.ValidMessageChecker;
                    transactionSetting.Active = true;
                    transactionSetting.Deleted = false;
                    _context.Update(transactionSetting);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool DeleteTransactionSetUp(int setupId)
        {
            if (setupId != 0)
            {
                var transactionSetup = _context.TransactionSettings.Where(x => x.Id == setupId && x.Active).FirstOrDefault();
                if (transactionSetup != null)
                {
                    transactionSetup.Deleted = true;
                    transactionSetup.Active = false;
                    _context.Update(transactionSetup);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public List<TransactionSettingsViewModel> TransactionSetting()
        {
            var transactionSettings = new List<TransactionSettingsViewModel>();
            var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
            {
                var transactionSetting = _context.TransactionSettings.Where(x => x.Id != 0 && x.CompanyBranchId == loggedInUser.CompanyBranchId && !x.Deleted)
                    .Include(x => x.CompanyBranch).Select(c => new TransactionSettingsViewModel
                    {
                        SenderName = c.Name,
                        Id = c.Id,
                        KeyWord = c.KeyWord,
                        ValidMessageChecker = c.ValidMessageChecker,

                    }).ToList();
                if (transactionSetting != null && transactionSetting.Count > 0)
                {
                    return transactionSetting;
                }
            }
            return transactionSettings;
        }

    }
}

