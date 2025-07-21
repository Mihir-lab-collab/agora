using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.BLL
{
    public class currencyMaster
    {

        public int currId { get; set; }
        public string currName { get; set; }
        public string currSymbol { get; set; }
       
        public currencyMaster()
        {

        }
        public currencyMaster(int _currId, string _currName, string _currSymbol)
        {
            this.currId = _currId;
            this.currName = _currName;
            this.currSymbol = _currSymbol;
           
        }

        public static currencyMaster GetcurrencyMasterBycurrId(int currId)
        {
            currencyMasterDAL objcurrencyMaster = new currencyMasterDAL();
            return objcurrencyMaster.GetcurrencyMasterBycurrId(currId);
        }

        public static List<currencyMaster> GetAllcurrencyMaster()
        {
            currencyMasterDAL objcurrencyMaster = new currencyMasterDAL();
            return objcurrencyMaster.GetAllcurrencyMaster();
        }
    }
}