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
    public class taxmaster
    {

        public int tax_id { get; set; }
        public int tax_parentid { get; set; }
        public string taxname { get; set; }
        public string taxdescription { get; set; }
        public decimal tax_rate { get; set; }
        public DateTime tax_Fromdate { get; set; }
        public DateTime tax_todate { get; set; }

        public taxmaster()
        {
        }

        public taxmaster(int tax_id, int tax_parentid, string taxname, string taxdescription, decimal tax_rate, DateTime tax_Fromdate, DateTime tax_todate)
        {
            this.tax_id = tax_id;
            this.tax_parentid = tax_parentid;
            this.taxname = taxname;
            this.taxdescription = taxdescription;
            this.tax_rate = tax_rate;
            this.tax_Fromdate = tax_Fromdate;
            this.tax_todate = tax_todate;
        }

        public static List<taxmaster> Get_taxdetail()
        {
            taxmasterDAL objtaxdetail = new taxmasterDAL();
            return objtaxdetail.Get_taxdetail();
        }

        public static int Inserttax(string taxname, string taxdescription, decimal tax_rate, DateTime tax_Fromdate, DateTime tax_todate)
        {
            taxmaster curtax = new taxmaster();
            curtax.taxname = taxname;
            curtax.taxdescription = taxdescription;
            curtax.tax_rate = tax_rate;
            curtax.tax_Fromdate = tax_Fromdate;
            curtax.tax_todate = tax_todate;
            taxmasterDAL objbugs = new taxmasterDAL();
            return objbugs.Inserttax(curtax);
        }

        public static bool UpdatetaxBytaxId(int tax_id, decimal tax_rate,DateTime tax_Fromdate, DateTime tax_todate)
        {
            taxmaster curtax = new taxmaster();
            curtax.tax_id = tax_id;
            curtax.tax_rate = tax_rate;
            curtax.tax_Fromdate = tax_Fromdate;
            curtax.tax_todate = tax_todate;
            taxmasterDAL objbugs = new taxmasterDAL();
            return objbugs.UpdatetaxBytaxId(curtax);
        }

        public static bool DeletetaxById(int tax_id)
        {
            taxmaster curtax = new taxmaster();
            curtax.tax_id = tax_id;

            taxmasterDAL objtax = new taxmasterDAL();
            return objtax.DeletetaxById(curtax);
        }

    }
}