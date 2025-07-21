var empid;
var LocationId;
var a = true;
$(document).ready(function () {

    $('[id$=msg]').html('');
    FillEmployeeDropDown();
    $('[id$="ddlEmp"]').val(0)
    $("#txtFromDate").val() == '';
    $("#txtToDate").val() == '';
    GetLateComingDetails(0, 0, '', '', $('#hdnLocationID').val());
    

    //BindLateComers("");
    

    $('[id$="txtFromDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
    $("#txtToDate").kendoDatePicker({ format: "dd/MM/yyyy" });
    
    empid = $('#hdnEmpID').val();
    LocationID = $('#hdnLocationID').val();
    
    
    $('[id$="ddlEmp"]').change(function () {

        if ($('[id$="ddlEmp"]').val() != 0) {
            selectedEmp = $('[id$="ddlEmp"]').val();
            $("#lblerrmsgEmp").html('');
        }
    });

    //debugger;
    $("#ddlStatus").change(function () {
        $('[id$=msg]').html('');
        $("#lblerrmsgEmp").html('');
        $("#lblDateError").html('');
            LocationID = $('#hdnLocationID').val();
    
        var Status = $('#ddlStatus :selected').val();
        
        var StartDate;
        var EndDate;
        var selectedEmp;
       
        if ($('[id$="ddlEmp"]').val() == 0) {
            selectedEmp = 0;
        }
        else {
            selectedEmp = $('[id$="ddlEmp"]').val();

        }
        if ($("#txtFromDate").val() == '') {
            StartDate = '';
        }
        else {
            StartDate = $("#txtFromDate").val();
        }

        if ($("#txtToDate").val() == '') {
            EndDate = '';
        }
        else {
            EndDate = $("#txtToDate").val();
        }
       
        //LocationID = $('#hdnLocationID').val();
        GetLateComingDetails(selectedEmp, Status, StartDate, EndDate, $('#hdnLocationID').val());

    });
});



function GetLateComingDetails(Empid, Status, StartDate, EndDate, LocationID) {

    $.ajax({
        type: "POST",
        url: "LateComers.aspx/GetLateComingData",
        contentType: "application/json;charset=utf-8",
        data: "{Empid:'" + Empid + "',Status:'" + Status + "',StartDate:'" + StartDate + "',EndDate:'" + EndDate + "',LocationID:'" + LocationID + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            var LateComingData = jQuery.parseJSON(msg.d);
            if (Status == 0) {
                $('#gridLateComer').css('display', 'block');
                $('#gridLateComingApproved').css('display', 'none');
                $('#gridLateComingRejected').css('display', 'none');
                $('#gridLateComingAdminCancel').css('display', 'none');
                GetLateComingPendingData(LateComingData);
            }
            else if (Status == 1) {
                $('#gridLateComer').css('display', 'none');
                $('#gridLateComingApproved').css('display', 'block');
                $('#gridLateComingRejected').css('display', 'none');
                $('#gridLateComingAdminCancel').css('display', 'none');
                GetLateComingApprovedData(LateComingData);
            }
            else if (Status == 2) {
                $('#gridLateComer').css('display', 'none');
                $('#gridLateComingApproved').css('display', 'none');
                $('#gridLateComingRejected').css('display', 'block');
                $('#gridLateComingAdminCancel').css('display', 'none');
                GetLateComingRejectedData(LateComingData);
            }
            else {
                $('#gridLateComer').css('display', 'none');
                $('#gridLateComingApproved').css('display', 'none');
                $('#gridLateComingRejected').css('display', 'none');
                $('#gridLateComingAdminCancel').css('display', 'block');
                GetLateComingAdminCancelData(LateComingData);
            }
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}


function GetLateComingPendingData(LateComingData) {
    $("#gridLateComer").empty();
    var gridLateComer = $("#gridLateComer").kendoGrid({
        dataSource: {
            data: LateComingData,
            schema: {
                model: {
                    fields: {
                        
                        ID: { type: "number" },
                        EmpCode: { type: "number" },
                        empName: { type: "string" },
                        ExpectedInTime: { type: "datetime", format: "{0:HH:mm tt}" },
                        ApplyDate: { type: "date", format: "{dd/MMM/yyyy}" },
                        CreatedOn: { type: "date", format: "{dd/MMM/yyyy}" },
                        CreatedBy: { type: "number" },
                        LateCommingReason: { type: "string" },
                        ApprovedOn: { type: "date", format: "{dd/MMM/yyyy}" },
                        ApprovedBy: { type: "number" },
                        ApprovalComment: { type: "string" },
                        IsApproveStatus: { type: "bit" }
                    }
                }
            },
            pageSize: 25,
        },
        scrollable: true,
        sortable: true,
        pageable:
            {
                input: true,
                numeric: false
            },
        columns: [
            { field: "ID", title: "ID", width: "30px" },
            { field: "empName", title: "EmpName", width: "30px" },
            { field: "ApplyDate", title: "LateComingDate", width: "30px", format: "{0:dd-MMM-yyyy}" },
            { field: "ExpectedInTime", title: "ExpectedInTime", width: "30px", format: "{0:HH:mm}" },
            { field: "LateCommingReason", title: "LateCommingReason", width: "50px" },
            { field: "ApprovalComment", title: "Admin Comment", width: "50px", template: '<textarea rows="4" cols="20" id="AdminComment"></textarea>' },
           // { field: "Statusflag", title: "IsApproveStatus", width: "30Px" },
            { command: [{ name: "approve", text: "Approve", id: "btnApprove", click: ApproveLateComer }, { name: "Reject", text: "Reject", id: "btnReject", click: RejectLateComer }], width: "40px" },

        ],
        filterable:
            {
                extra: false,
                operators:
                    {
                        string:
                            {
                                startswith: "Starts with",
                                contains: "Contains",
                                eq: "Is equal to"
                            }
                    }
            },

        editable: false,

        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
    }).data("kendoGrid");
}
function GetLateComingApprovedData(LateComingData) {
    $("#gridLateComingApproved").empty();
    var gridHolidayWokingApproved = $("#gridLateComingApproved").kendoGrid({
        dataSource: {
            data: LateComingData,
            schema: {
                model: {
                    fields: {
                        ID: { type: "number" },
                        EmpCode: { type: "number" },
                        empName: { type: "string" },
                        ExpectedInTime: { type: "datetime", format: "{0:HH:mm tt}" },
                        ApplyDate: { type: "date", format: "{dd/MMM/yyyy}" },
                        CreatedOn: { type: "date", format: "{dd/MMM/yyyy}" },
                        CreatedBy: { type: "number" },
                        LateCommingReason: { type: "string" },
                        ApprovedOn: { type: "date", format: "{dd/MMM/yyyy}" },
                        ApprovedBy: { type: "number" },
                        ApprovalComment: { type: "string" },
                        IsApproveStatus: { type: "bit" },
                        AdminCancelReason: {type: "string"}
                    }
                }
            },
            pageSize: 25,
        },
        scrollable: true,
        sortable: true,
        pageable:
            {
                input: true,
                numeric: false
            },
        columns: [

            { field: "ID", title: "ID", width: "30px" },
            { field: "empName", title: "EmpName", width: "30px" },
            { field: "ApplyDate", title: "LateComingDate", width: "30px", format: "{0:dd-MMM-yyyy}" },
            { field: "ExpectedInTime", title: "ExpectedInTime", width: "30px", format: "{0:HH:mm}" },
            { field: "LateCommingReason", title: "LateCommingReason", width: "50px" },
            { field: "ApprovalComment", title: "Admin Comment", width: "50px"},
            { field: "AdminCancelReason", title: "Admin Cancle Comment", width: "50px", template: '<textarea rows="4" cols="20" id="AdminCancelReason"></textarea>' },
          //  { field: "Statusflag", title: "IsApproveStatus", width: "30Px" }

            {
                command: [{ name: "CancelLateComer", text: "Cancel", id: "btnCancel", click: CancelLateComer }], width: "30px"
            }
        ],
        filterable:
            {
                extra: false,
                operators:
                    {
                        string:
                            {
                                startswith: "Starts with",
                                contains: "Contains",
                                eq: "Is equal to"
                            }
                    }
            },

        editable: false,
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
    }).data("kendoGrid");
}

function GetLateComingRejectedData(LateComingData) {
    $("#gridLateComingRejected").empty();
    var gridHolidayWokingRejected = $("#gridLateComingRejected").kendoGrid({
        dataSource: {
            data: LateComingData,
            schema: {
                model: {
                    fields: {
                        ID: { type: "number" },
                        EmpCode: { type: "number" },
                        empName: { type: "string" },
                        ExpectedInTime: { type: "datetime", format: "{0:HH:mm tt}" },
                        ApplyDate: { type: "date", format: "{dd/MMM/yyyy}" },
                        CreatedOn: { type: "date", format: "{dd/MMM/yyyy}" },
                        CreatedBy: { type: "number" },
                        LateCommingReason: { type: "string" },
                        ApprovedOn: { type: "date", format: "{dd/MMM/yyyy}" },
                        ApprovedBy: { type: "number" },
                        ApprovalComment: { type: "string" },
                        IsApproveStatus: { type: "bit" }
                    }
                }
            },
            pageSize: 25,
        },
        scrollable: true,
        sortable: true,
        pageable:
            {
                input: true,
                numeric: false
            },
        columns: [



            { field: "ID", title: "ID", width: "30px" },
            { field: "empName", title: "EmpName", width: "30px" },
            { field: "ApplyDate", title: "LateComingDate", width: "30px", format: "{0:dd-MMM-yyyy}" },
            { field: "ExpectedInTime", title: "ExpectedInTime", width: "30px", format: "{0:HH:mm}" },
            { field: "LateCommingReason", title: "LateCommingReason", width: "50px" },
            { field: "ApprovalComment", title: "Admin Comment", width: "50px" },
          //  { field: "Statusflag", title: "IsApproveStatus", width: "30Px" },

        ],
       

        filterable:
            {
                extra: false,
                operators:
                    {
                        string:
                            {
                                startswith: "Starts with",
                                contains: "Contains",
                                eq: "Is equal to"
                            }
                    }
            },

        editable: false,

        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
    }).data("kendoGrid");
}


function GetLateComingAdminCancelData(LateComingData) {
    $("#gridLateComingAdminCancel").empty();
    var gridHolidayWokingAdminCalcel = $("#gridLateComingAdminCancel").kendoGrid({
        dataSource: {
            data: LateComingData,
            schema: {
                model: {
                    fields: {
                        ID: { type: "number" },
                        EmpCode: { type: "number" },
                        empName: { type: "string" },
                        ExpectedInTime: { type: "datetime", format: "{0:HH:mm tt}" },
                        ApplyDate: { type: "date", format: "{dd/MMM/yyyy}" },
                        CreatedOn: { type: "date", format: "{dd/MMM/yyyy}" },
                        CreatedBy: { type: "number" },
                        LateCommingReason: { type: "string" },
                        ApprovedOn: { type: "date", format: "{dd/MMM/yyyy}" },
                        ApprovedBy: { type: "number" },
                        ApprovalComment: { type: "string" },
                        IsApproveStatus: { type: "bit" }
                    }
                }
            },
            pageSize: 25,
        },
        scrollable: true,
        sortable: true,
        pageable:
            {
                input: true,
                numeric: false
            },
        columns: [
            { field: "ID", title: "ID", width: "30px" },
            { field: "empName", title: "EmpName", width: "30px" },
            { field: "ApplyDate", title: "LateComingDate", width: "30px", format: "{0:dd-MMM-yyyy}" },
            { field: "ExpectedInTime", title: "ExpectedInTime", width: "30px", format: "{0:HH:mm}" },
            { field: "LateCommingReason", title: "LateCommingReason", width: "50px" },
            { field: "ApprovalComment", title: "Admin Comment", width: "50px" },
          //  { field: "Statusflag", title: "IsApproveStatus", width: "30Px" },

        ],
        

        filterable:
            {
                extra: false,
                operators:
                    {
                        string:
                            {
                                startswith: "Starts with",
                                contains: "Contains",
                                eq: "Is equal to"
                            }
                    }
            },

        editable: false,

        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
    }).data("kendoGrid");
}
function Search() {
    $('[id$=msg]').html('');
    var errEmp = $("#lblerrmsgEmp");
    var errDate = $("#lblDateError");
    errEmp.html('');
    errDate.html('');
    if ($('[id$="ddlEmp"]').val() == "0") {
        errEmp.html('Please select employee name');
        return false;
    }
    else if ($("#txtFromDate").val() == '') {
        errDate.html('Please select From Date');
        return false;
    }
    else if ($("#txtToDate").val() == '') {
        errDate.html('Please select End Date');
        return false;
    }
    else if ($("#txtFromDate").val() != '' && $("#txtToDate").val() != '') {
        var date1 = new Date($("#txtFromDate").val());
        var date2 = new Date($("#txtToDate").val());
        if (date1 > date2) {
            errDate.html('From date should be less than To Date');
            return false;
        }
        else {
            var startDate = $("#txtFromDate").val();
            var endDate = $("#txtToDate").val();
            GetLateComingDetails($('[id$="ddlEmp"]').val(), $('#ddlStatus :selected').val(), startDate, endDate, $('#hdnLocationID').val());
        }
    }
}


function BindLateComers(mDate) {

    $.ajax({
        type: "POST",
        url: "LateComers.aspx/BindLateComers",
        contentType: "application/json;charset=utf-8",
        data: "{mDate:'" + mDate + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {

            SetData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });


}



function SetData(EventData) {
    $("#gridLateComer").empty();
    var gridLateComer = $("#gridLateComer").kendoGrid({
    //$(gridLateComer).kendoGrid({
        dataSource: {
            data: EventData,
            schema: {
                model: {
                    fields: {
                        ID: { type: "number" },
                        EmpCode: { type: "number" },
                        EmpNameId: {type: "string"},
                        ExpectedInTime: { type: "datetime", format: "{0:HH:mm tt}" },
                        ApplyDate: { type: "date", format: "{dd/MMM/yyyy}"},
                        CreatedOn: { type: "date", format: "{dd/MMM/yyyy}" },
                        CreatedBy: { type: "number" },
                        LateCommingReason: { type: "string" },
                        ApprovedOn: { type: "date", format: "{dd/MMM/yyyy}"  },
                        ApprovedBy: { type: "number" },
                        ApprovalComment: { type: "string" },
                        IsApproveStatus: { type:"bit" }
                    }

                }
            },
            pageSize: 50,
        },
        scrollable: true,
        sortable: true,
        resizable: true,
        selectable: true,
        pageable: {
            input: true,
            numeric: false
        },
        columns: [

           { field: "ID", title: "ID", width: "30px" },
           { field: "EmpNameId", title: "EmpName", width: "30px" },
           {field: "ApplyDate", title: "LateComingDate", width: "30px", format: "{0:dd-MMM-yyyy}"},
           {field: "ExpectedInTime", title: "ExpectedInTime", width: "30px", format: "{0:HH:mm}"},
           {field: "LateCommingReason", title: "LateCommingReason", width: "50px"},
           {field: "ApprovalComment", title: "Admin Comment", width: "50px", template: '<textarea rows="4" cols="20" id="AdminComment"></textarea>' },
            //{ field: "Statusflag", title:"IsApproveStatus", width: "30Px"},
            { command: [{ name: "approve", text: "Approve", id: "btnApprove", click: ApproveLateComer }, { name: "Reject", text: "Reject", id: "btnReject", click: RejectLateComer }], width: "40px" },
        ],

        filterable: {
            extra: false,
            operators: {
                string: {
                    startswith: "Starts with",
                    contains: "Contains",
                    eq: "Is equal to"
                }
            }
        },
        editable: false,
        
    
}).data("kendoGrid");
}

function ApproveLateComer(e) {

    var tr = $(e.target).closest("tr");
    var row = $(e.target).closest("tr");
    var grid = $("#gridLateComer").data("kendoGrid");
    var data = grid.dataItem(row);
    var EmpCode = data.EmpCode;
    var ApprovedBy = $('[id$="hdnLoginID"]').val()
    var IsApproveStatus = 1;
    var adminComment = $(e.target).closest("tr").find('#AdminComment').val();
    $.ajax({
        type: "POST",
        url: "LateComers.aspx/ApproveLateComers",
        contentType: "application/json;charset=utf-8",
        
        data: JSON.stringify({ 'EmpCode': EmpCode, 'ApprovalComment': adminComment, 'ID': data.ID, 'IsApproveStatus': IsApproveStatus, 'ApprovedBy': ApprovedBy }),
        
        dataType: "json",
        async: false,
        success: function (msg) {
            //bindGrid();
            BindLateComers();
            alert("upload succed");
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }

    });
}


function RejectLateComer(e) {

    var tr = $(e.target).closest("tr");
    var row = $(e.target).closest("tr");
    var grid = $("#gridLateComer").data("kendoGrid");
    var data = grid.dataItem(row);
    var EmpCode = data.EmpCode;
    var ApprovedBy = $('[id$="hdnLoginID"]').val()
    var IsApproveStatus = 2;
    var adminComment = $(e.target).closest("tr").find('#AdminComment').val();
    $.ajax({
        type: "POST",
        url: "LateComers.aspx/ApproveLateComers",
        contentType: "application/json;charset=utf-8",

        data: JSON.stringify({ 'EmpCode': EmpCode, 'ApprovalComment': adminComment, 'ID': data.ID, 'IsApproveStatus': IsApproveStatus, 'ApprovedBy': ApprovedBy }),

        dataType: "json",
        async: false,
        success: function (msg) {
            //bindGrid();
            BindLateComers();
            alert("upload succed");
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }

    });
}


function CancelLateComer(e) {;
    var tr = $(e.target).closest("tr");
    var row = $(e.target).closest("tr");
    var grid = $("#gridLateComingApproved").data("kendoGrid");
    var data = grid.dataItem(row);
    var EmpCode = data.EmpCode;
    //var ApprovedBy = $('[id$="hdnLoginID"]').val()
    var IsApproveStatus = 1;
    
    var adminComment = $(e.target).closest("tr").find('#AdminCancelReason').val();
    
    $.ajax({
        type: "POST",
        url: "LateComers.aspx/CancleLateComing",//"empHolidayWorkDetails.aspx/CancelHolidayLeave",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify({ 'ID': data.ID ,'EmpCode': EmpCode, 'ApprovalComment': adminComment}),
        //data: "{ID:'" + data.ID + "',EmpCode:'" + data.EmpCode + "',AdminCancelReason:'" + adminComment + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            bindGrid();
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });



  
}


function GetHolidayWorkingRejectedData(LateComingData) {
    $("#gridLateComingRejected").empty();
    var gridHolidayWokingRejected = $("#gridLateComingRejected").kendoGrid({
        dataSource: {
            data: LateComingData,
            schema: {
                model: {
                    fields: {
                        EmpName: { type: "string" },
                        ID: { type: "number" },
                        HolidayDate: { type: "string" },
                        Empid: { type: "string" },
                        projId: { type: "string" },
                        ExpectedHours: { type: "string" },
                        UserReason: { type: "string" },
                        projName: { type: "string" },
                    }
                }
            },
            pageSize: 25,
        },
        scrollable: true,
        sortable: true,
        pageable:
            {
                input: true,
                numeric: false
            },
        columns: [
            { field: "ID", title: "ID", width: "10px", hidden: true },
            { field: "EmpName", title: "Employee Name", width: "20px" },
            { field: "projName", title: "Prject Name", width: "20px" },
            { field: "HolidayDate", title: "Holiday Date", width: "20px", format: "{0:dd-MMM-yyyy}", },
            { field: "ExpectedHours", title: "Expected Hours", width: "20px" },
            { field: "UserReason", title: "Reason", width: "20px" },
            { field: "AdminComment", title: "Admin Comment", width: "20px" }

        ],
        
        filterable:
            {
                extra: false,
                operators:
                    {
                        string:
                            {
                                startswith: "Starts with",
                                contains: "Contains",
                                eq: "Is equal to"
                            }
                    }
            },

        editable: false,

        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
    }).data("kendoGrid");
}





function FillEmployeeDropDown() {
    $.ajax({
        type: "POST",
        url: "LateComers.aspx/BindEmployee",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        async: false,
        dataType: "json",
        success: function (msg) {
            $("#ddlEmp").kendoDropDownList({
                optionLabel: "Select Employee",
                dataTextField: "empName",
                dataValueField: "empid",
                dataSource: jQuery.parseJSON(msg.d)

            }).data("kendoDropDownList");
        },

        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}


function SearchLateComing() {
    //var mDate = $('[id$="txtFromDate"]').val();
    //BindLateComers(mDate);

    $('[id$=msg]').html('');
    var errEmp = $("#lblerrmsgEmp");
    var errDate = $("#lblDateError");
    errEmp.html('');
    errDate.html('');
    if ($('[id$="ddlEmp"]').val() == "0") {
        errEmp.html('Please select employee name');
        return false;
    }
    else if ($("#txtFromDate").val() == '') {
        errDate.html('Please select From Date');
        return false;
    }
    else if ($("#txtToDate").val() == '') {
        errDate.html('Please select End Date');
        return false;
    }
    else if ($("#txtFromDate").val() != '' && $("#txtToDate").val() != '') {
        var date1 = new Date($("#txtFromDate").val());
        var date2 = new Date($("#txtToDate").val());
        if (date1 > date2) {
            errDate.html('From date should be less than To Date');
            return false;
        }
        else {
            var startDate = $("#txtFromDate").val();
            var endDate = $("#txtToDate").val();
            GetLateComingDetails($('[id$="ddlEmp"]').val(), $('#ddlStatus :selected').val(), startDate, endDate, $('#hdnLocationID').val());
        }
    }
}