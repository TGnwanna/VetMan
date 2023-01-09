using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IHelpers
{
	public  interface ISalesHelper
	{
        bool CreateSalesProduct(ProductInventoryViewModel data);
        bool UpdateSalesProduct(ProductInventoryViewModel data);
        ProductInventory GetProductDetail(int id, int supplierId);
        bool CheckInventoryName(ProductInventoryViewModel data);
        List<SaleLogsViewModel> GetCustomerDropDown();
        List<SaleLogsViewModel> GetItemList();
        SaleLogsViewModel GetCustomerSalesHistory(int id);
        List<SaleLogsViewModel> GetItemListFromSearch(Guid customerId, DateTime dateGetFrom, DateTime dateToEnd);
        SalesPaymentViewModel CreateSelectedProducts(List<SalesViewModel> sales, string customerId, string totalAmountPaid);
        List<PaymentViewModel> GetPaymentList();
    }
}
