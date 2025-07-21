using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

[ServiceContract()]
public interface IAPIService
{
    #region Post methods
    [OperationContract]
    [WebInvoke(Method = "POST", UriTemplate = "/DoLogin", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> DoLogin(InputData objdata);

    [OperationContract]
    [WebInvoke(Method = "POST", UriTemplate = "/Getlogin", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> Getlogin(InputData objdata);

    [OperationContract]
    [WebInvoke(Method = "POST", UriTemplate = "/Dashboard", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> Dashboard();

    [OperationContract()]
    [WebInvoke(Method = "POST", UriTemplate = "/fnGetEmpList", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> fnGetEmpList(InputData objdata);

    [OperationContract()]
    [WebInvoke(Method = "POST", UriTemplate = "/fnInsertFavouriteEmp", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> fnInsertFavouriteEmp(InputData objdata);

    [OperationContract()]
    [WebInvoke(Method = "POST", UriTemplate = "/fnGetFavEmpList", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> fnGetFavEmpList(InputData objdata);

    [OperationContract()]
    [WebInvoke(Method = "POST", UriTemplate = "/fnGetMissedDates", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> fnGetMissedDates(InputData objdata);
    [OperationContract()]
    [WebInvoke(Method = "POST", UriTemplate = "/fnForgetPassword", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> fnForgetPassword(InputData objdata);

    [OperationContract()]
    [WebInvoke(Method = "POST", UriTemplate = "/fnChangedPassword", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> fnChangedPassword(InputData objdata);

    [OperationContract()]
    [WebInvoke(Method = "POST", UriTemplate = "/fnApplyingForLeaves", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> fnApplyingForLeaves(InputData objdata);

    [OperationContract()]
    [WebInvoke(Method = "POST", UriTemplate = "/fnLeaveStatus", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> fnLeaveStatus(InputData objdata);

    [OperationContract()]
    [WebInvoke(Method = "POST", UriTemplate = "/fnRemoveFavouriteEmp", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> fnRemoveFavouriteEmp(InputData objdata);

    [OperationContract()]
    [WebInvoke(Method = "POST", UriTemplate = "/fnAvailableLeaves", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> fnAvailableLeaves(InputData objdata);

    [OperationContract]
    [WebInvoke(Method = "POST", UriTemplate = "/fnDeleteAppliedLeave", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> fnDeleteAppliedLeave(InputData objdata);

    [OperationContract]
    [WebInvoke(Method = "POST", UriTemplate = "/GetTimesheetDetails", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> GetTimesheetDetails(InputData objdata);

    [OperationContract]
    [WebInvoke(Method = "POST", UriTemplate = "/defaultBindTimesheet", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> defaultBindTimesheet(InputData objdata);
    [OperationContract()]
    [WebInvoke(Method = "POST", UriTemplate = "/fnInsertLateComing", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> fnInsertLateComing(InputData objdata);
    #endregion

    #region Get methods
    [OperationContract]
    [WebInvoke(Method = "GET", UriTemplate = "/GetLeaveType", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> GetLeaveType();

    [OperationContract]
    [WebInvoke(Method = "GET", UriTemplate = "/fnEmpleaveRequests", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<Result> fnEmpleaveRequests();
    #endregion
}

