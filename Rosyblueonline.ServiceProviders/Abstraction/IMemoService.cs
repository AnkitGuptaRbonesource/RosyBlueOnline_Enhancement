using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.ServiceProviders.Abstraction
{
    public interface IMemoService
    {
        OrderInfoViewModel MemoInfo(int OrderID);
        MemoDetail CreateMemo(string LotNos, int LoginID, int CustomerID, int isConfirmed, int isSellDirect, string Remark, int FileID = 0);
        MemoDetail CancelPartialMemo(int OrderID, string LotNos, int LoginID);
        int SplitMemo(int OrderID, string LotNos, int LoginID, int CustomerID, int isConfirmed, int isSellDirect, string Remark);
        int SellFullMemo(int OrderID, int LoginID, int MemoMode, decimal salesAvgDiscount);
        int CancelFullMemo(int OrderID, int LoginID);
        int MergeMemo(int LoginID, int CustomerID, int isConfirmed, int isSellDirect, string Remark, string MergeOrderList);
        int UpdateOrder(int OrderID, int CustomerID, string Remark, int LoginID);
        int[] GetOrderIDFromLotNos(string LotNos);
        fileUploadLogModel GetFileByID(int FileID);
        int UpdateFile(fileUploadLogModel obj);
        int UpdateFile(int OrderID, int FileID);
        int MemoReturnSale(string OrderIDs, int UserID);
        //Web mode 1 & sale mode 2
        void SendMailMemo(int OrderID, string UserEmailID,string ToName, int CustomerId, string Subject, string MessageContent, string TemplatePath, OrderInfoViewModel objOInfo = null,string LotNos= null,int MemoMode=1,decimal salesAvgDiscount=0);
        int MemoPartialReturnSale(string LotNos, int LoginId);

        CustOrderDetailModel orderItemDetailsfrominvintory(int Inventory);
    }
}
