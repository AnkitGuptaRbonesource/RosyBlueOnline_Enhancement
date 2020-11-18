using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.ServiceProviders.Abstraction
{
    public interface IOrderService
    {
        OrderViewModel PreBookOrder(string LotNos, int CustomerId, int ShippingMode);
        PlaceOrderViewModel BookOrder(string LotNos, int CreatedBy, int CustomerId, int ShippingMode, int BillingID, int ShippingID);
        IQueryable<OrderListView> OrderListView();
        OrderInfoViewModel OrderInfo(int OrderID);
        int OrderPartialCancel(int OrderID, string LotNo, int LoginID, int CustomerID);
        int OrderCancel(int OrderID, int LoginID, int CustomerID);
        int OrderComplete(int OrderID, int LoginID, int CustomerID, string ShippingCompany, string TrackingNumber);
        List<inventoryDetailsViewModel> GetOrderItemsByOrderID(string OrderID, int UserID);
        int OrderMerge(int LoginID, string MergeOrderList);
        void SendMailPreBookOrder(int OrderID, int CustomerId, string TemplatePath,string Subject, bool SentToCustomer = false);
        void SendForOrder(OrderInfoViewModel objOInfo, int CustomerId, string TemplatePath,string Message, bool SentToCustomer = false);
        DataTable OrderDetailForDownload(string OType, string OStatus, int FilterCustomerID);
        MemoTallyStockByRfidViewModel MemoTallyStockByRfid(int LoginID, int OrderID, string RFIDs);
        void SendMailForApiOrderBook(int OrderID, int CustomerId, string TemplatePath, string Subject, string CCEmail, string BCCEmail, bool SentToCustomer = false);

    }
}
