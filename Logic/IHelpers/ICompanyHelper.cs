using Core.Models;
using Core.ViewModels;

namespace Logic.IHelpers
{
    public interface ICompanyHelper
    {
        List<CompanyBranchViewModel> GetCompanyBranchList();
        bool CreateCompanyBranchDetails(CompanyBranchViewModel companyBranchDetails);
        CompanyBranch EditCompanyBranchDetails(Guid CompanyId);
        CompanyBranch EditedCompanyBranchDetails(CompanyBranchViewModel companyBranchDetails);
        CompanyBranch DeleteCompanyDetails(CompanyBranchViewModel BranchDetails);
        Task<Company> GetCompanyByAdminId(string userId);
        bool DeleteCompany(Guid companyId);
        List<TransactionViewModel> AllTransactions();
        Transaction SaveIncomingTrasactionMessage(string messageBody, string keyword, string validMessageChecker);
        bool ConfirmTransaction(Guid transactionId, string reciept);
        Task<CompanySettingViewModel> GetCompanySettings(Guid companyId);
        Task<Company> GetCompanyById(Guid? companyId);
        CompanySettingViewModel GetCompanyCustomSettings(Guid companyId, List<string> checkedCompanySettings, List<string> uncheckedCompanySettings);
        bool SetUpTransaction(TransactionSettingsViewModel transactionSettingsViewModel);
        bool EditTransactionSetup(TransactionSettingsViewModel transactionSettingsViewModel);
        bool DeleteTransactionSetUp(int setupId);
        List<TransactionSettingsViewModel> TransactionSetting();
    }
}
