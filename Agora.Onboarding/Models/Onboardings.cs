using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Agora.Onboarding.Models
{
    public class Onboardings
    {

        public int Id { get; set; }
        public string Empname { get; set; }
        public bool Timesheet_check { get; set; }
        public bool Leave_check { get; set; }
        public bool WFH_check { get; set; }
        public bool HR_Manual_check { get; set; }
        public int EmpId { get; set; }
        public bool Dwn_Letter { get; set; }
        public bool CheckList_check { get; set; }
        public bool IsCompleted { get; set; }
        //[DataType(DataType.Upload)]
        private string _SignImage=string.Empty;
        public string SignImage
        {
            get { return _SignImage; }
            set { _SignImage = value; }
        }
        private string _InsertedOn = string.Empty;

        public string InsertedOn
        {
            get { return _InsertedOn; }
            set { _InsertedOn = value; }
        }

        public bool Confidentiality_check{ get; set; }
        public bool ITInduction_check { get; set; }
        public bool SkypeAccount_check { get; set; }
        public bool MS365_check { get; set; }
        public bool SkypeInvite_check { get; set; }
        public bool Registration_check { get; set; }
        public bool PI_check { get; set; }
        public bool Form_check { get; set; }
    }
}