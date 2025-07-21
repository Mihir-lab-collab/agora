using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ConstantTexts
/// </summary>
public static partial class ConstantTexts
{ 
    public static class TimeSheetCrud
    {
        public static string AddSuccess = "Timesheet details are saved successfully";
        public static string AddFailed = "Timesheet details couldn't be added.";
        public static string UpdateSuccess = "Timesheet details are updated successfully";
        public static string UpdateFailed = "Timesheet details couldn't be updated.";
        public static string DeleteSuccess = "Timesheet details are deleted successfully";
        public static string DeleteFailed = "Collection center details couldn't deleted";
        public static string ValidationDescription = "You can not enter more than 500 characters in Description.";        
        public static string AddValidationFailed3Days = "You are not allowed to fill timesheet for this date";
        public static string AddValidationFailedDateGreaterTahnCurrentdate = "Date should not be greater than currentdate";
        public static string AddUpdateValidation24hrs = "You can not enter more than 24 hrs working time in Timesheet for today.";
        public static string TimesheerZeroHrs = "Hrs should be greater than 0.";
    }
    public static class TimeSheetResponse
    {
        public static string Success = "Success";
        public static string Failed = "Failed";        
    }
    public static class TimeSheetStatusCode
    {
        public static int StatusCode = 400;
        public static string StatusDescription = "Bad Request";
    }
    public static class TimeSheetException
    {
        public static int StatusCode = 500;
        public static string StatusDescription = "Internal server Error";
    }
}