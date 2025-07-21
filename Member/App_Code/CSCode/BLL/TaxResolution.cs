using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;
namespace Customer.BLL
{
    /// <summary>
    /// Summary description for taxmaster
    /// </summary>
    /// 
    public class TaxResolution
    {

        public int tax_id { get; set; }
        public int tax_parentid { get; set; }
        public string taxname { get; set; }
        public string taxdescription { get; set; }
        public decimal tax_rate { get; set; }
        public DateTime tax_Fromdate { get; set; }
        public DateTime tax_todate { get; set; }

        public TaxResolution()
        {
        }

        public TaxResolution(int tax_id, int tax_parentid, string taxname, string taxdescription, decimal tax_rate, DateTime tax_Fromdate, DateTime tax_todate)
        {
            this.tax_id = tax_id;
            this.tax_parentid = tax_parentid;
            this.taxname = taxname;
            this.taxdescription = taxdescription;
            this.tax_rate = tax_rate;
            this.tax_Fromdate = tax_Fromdate;
            this.tax_todate = tax_todate;
        }

        public static List<TaxResolution> GetAlltaxResolutionBytaxId(int tax_id)
        {
            TaxResolutionDAL objtaxdetail = new TaxResolutionDAL();
            return objtaxdetail.GetAlltaxResolutionBytaxId(tax_id);
        }

    }
}