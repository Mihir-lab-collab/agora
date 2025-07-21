using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.BLL
{
    public class paymentMaster
    {

        public int payId { get; set; }
        public int payProjId { get; set; }
        public DateTime payDate { get; set; }
        public decimal payAmount { get; set; }
        public decimal payExRate { get; set; }
        public DateTime payConfirmedDate { get; set; }
        public string payComment { get; set; }
        public decimal payTransCharge { get; set; }
        public int paymentType { get; set; }
        public string PaymentMode { get; set; }
        public DateTime InvoiceSendOn { get; set; }
        public int crId { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsMiscellaneous { get; set; }


        public paymentMaster()
        {

        }
        public paymentMaster(int _payId, int _payProjId, DateTime _payDate, decimal _payAmount, decimal _payExRate,
            DateTime _payConfirmedDate, string _payComment, decimal _payTransCharge, int _paymentType, string _PaymentMode, DateTime _InvoiceSendOn,
            int _crId, DateTime _CreatedOn, bool _IsMiscellaneous)
        {
            this.payId = _payId;
            this.payProjId = _payProjId;
            this.payDate = _payDate;
            this.payAmount = _payAmount;
            this.payExRate = _payExRate;
            this.payConfirmedDate = _payConfirmedDate;
            this.payComment = _payComment;
            this.payTransCharge = _payTransCharge;
            this.paymentType = _paymentType;
            this.PaymentMode = _PaymentMode;
            this.InvoiceSendOn = _InvoiceSendOn;
            this.crId = _crId;
            this.CreatedOn = _CreatedOn;
            this.IsMiscellaneous = _IsMiscellaneous;
        }

        public static List<paymentMaster> GetAllpaymentMaster()
        {
            paymentMasterDAL objGetAllpaymentMaster = new paymentMasterDAL();
            return objGetAllpaymentMaster.GetAllpaymentMaster();
        }

        public static List<paymentMaster> GetAllpaymentMasterByProjId(int projId)
        {
            paymentMasterDAL objGetAllpaymentMasterByProjId = new paymentMasterDAL();
            return objGetAllpaymentMasterByProjId.GetAllpaymentMasterByProjId(projId);
        }





    }
}