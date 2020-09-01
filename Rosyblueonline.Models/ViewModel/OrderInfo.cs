using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
    public class OrderInfoViewModel
    {
        public orderDetailModel OrderDetail { get; set; }
        public List<inventoryDetailsViewModel> OrderItemDetail { get; set; }
        public MstBillingAddressViewModel BillingAddress { get; set; }
        public List<OrderChargesViewModel> Charges { get; set; }
        public UserDetailModel UserDetail { get; set; }

        public DataTable ConvertOrderChangesInDatetable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("actionMode");
            dt.Columns.Add("chargesName");
            //dt.Columns.Add("chargesType");
            //dt.Columns.Add("chargesValue");
            dt.Columns.Add("chargesAmount");
            for (int i = 0; i < this.Charges.Count; i++)
            {
                dt.Rows.Add(this.Charges[i].actionMode + " : " + this.Charges[i].chargesName, 
                            this.Charges[i].chargesType == 1 ? this.Charges[i].chargesValue.ToString() + " %" : this.Charges[i].chargesValue.ToString(), 
                            this.Charges[i].chargesAmount);
            }
            return dt;
        }
    }

    public class MstBillingAddressViewModel
    {
        public int billingId { get; set; }
        public int loginID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string companyName { get; set; }
        public string address01 { get; set; }
        public string address02 { get; set; }
        public string cityName { get; set; }
        public int stateID { get; set; }
        public string stateName { get; set; }
        public int countryID { get; set; }
        public string countryName { get; set; }
        public string zipCode { get; set; }
        public System.DateTime createdOn { get; set; }
        public System.DateTime? updatedOn { get; set; }
        public bool isActive { get; set; }
    }
}
