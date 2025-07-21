using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.BLL
{
    public class changeRequest
    {

        public int chgid { get; set; }
        public int chgProjId { get; set; }
        public decimal chgEstCost { get; set; }

        public changeRequest()
        {

        }
        public changeRequest(int _chgid, int _chgProjId, decimal _chgEstCost)
        {
            this.chgid = _chgid;
            this.chgProjId = _chgProjId;
            this.chgEstCost = _chgEstCost;
           
        }
        public static List<changeRequest> GetAllchangeRequestByProjId(int ProjId)
        {
            changeRequestDAL objchangeRequest = new changeRequestDAL();
            return objchangeRequest.GetAllchangeRequestByProjId(ProjId);
        }
    }
}