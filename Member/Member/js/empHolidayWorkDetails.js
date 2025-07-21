var empid;
var a = true;
$(document).ready(function () {
    $('[id$=msg]').html('');
    FillEmployeeDropDown();
    $('[id$="ddlEmp"]').val(0)
    $("#txtFromDate").val() == '';
    $("#txtToDate").val() == '';

    $("#txtFromDate").kendoDatePicker({ format: "dd/MM/yyyy" });
    $("#txtToDate").kendoDatePicker({ format: "dd/MM/yyyy" });
    GetHolidayWorkingDetails(0, 0, '', '', $('#hdnLocationID').val());
    empid = $('#hdnEmpID').val();


    $('[id$="ddlEmp"]').change(function () {

        if ($('[id$="ddlEmp"]').val() != 0) {
            $("#lblerrmsgEmp").html('');
        }
    });

    //Dropdown status changed
    $("#ddlStatus").change(function () {
        $('[id$=msg]').html('');
        $("#lblerrmsgEmp").html('');
        $("#lblDateError").html('');
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

        GetHolidayWorkingDetails(selectedEmp, Status, StartDate, EndDate, $('#hdnLocationID').val());

    });
});

function FillEmployeeDropDown() {
    $.ajax({
        type: "POST",
        url: "empHolidayWorkDetails.aspx/BindEmployee",
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

function Reset() {
    $('[id$=msg]').html('');
    var StartDate;
    var EndDate;
    var selectedEmp;

    //if ($('#ddlEmp').val() == 0) {
    //    selectedEmp = 0;
    //}
    //else {
    //    selectedEmp = $('#ddlEmp').val();
    //}
    //if ($("#txtFromDate").val() == '') {
    //    StartDate = '';
    //}
    //else {
    //    StartDate = $("#txtFromDate").val();
    //}

    //if ($("#txtToDate").val() == '') {
    //    EndDate = '';
    //}
    //else {
    //    EndDate = $("#txtToDate").val();
    //}

    $('#ddlEmp').val(0);

    $("#txtFromDate").val('');

    $("#txtToDate").val('');
    FillEmployeeDropDown();
    GetHolidayWorkingDetails(0, $('#ddlStatus :selected').val(), '', '', $('#hdnLocationID').val());
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
            GetHolidayWorkingDetails($('[id$="ddlEmp"]').val(), $('#ddlStatus :selected').val(), startDate, endDate, $('#hdnLocationID').val());
        }
    }
}

function GetHolidayWorkingDetails(Empid, Status, HolidayStartDate, HolidayEndDate, LocationID) {

    $.ajax({
        type: "POST",
        url: "empHolidayWorkDetails.aspx/GetHolidayWorkingData",
        contentType: "application/json;charset=utf-8",
        data: "{Empid:'" + Empid + "',Status:'" + Status + "',HolidayStartDate:'" + HolidayStartDate + "',HolidayEndDate:'" + HolidayEndDate + "',LocationID:'" + LocationID + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            var HolidayWorkingData = jQuery.parseJSON(msg.d);
            if (Status == 0) {
                $('#gridHolidayWokingPending').css('display', 'block');
                $('#gridHolidayWokingApproved').css('display', 'none');
                $('#gridHolidayWokingRejected').css('display', 'none');
                $('#gridHolidayWokingAdminCalcel').css('display', 'none');
                GetHolidayWorkingPendingData(HolidayWorkingData);
            }
            else if (Status == 1) {
                $('#gridHolidayWokingPending').css('display', 'none');
                $('#gridHolidayWokingApproved').css('display', 'block');
                $('#gridHolidayWokingRejected').css('display', 'none');
                $('#gridHolidayWokingAdminCalcel').css('display', 'none');
                GetHolidayWorkingApprovedData(HolidayWorkingData);
            }
            else if (Status == 2) {
                $('#gridHolidayWokingPending').css('display', 'none');
                $('#gridHolidayWokingApproved').css('display', 'none');
                $('#gridHolidayWokingRejected').css('display', 'block');
                $('#gridHolidayWokingAdminCalcel').css('display', 'none');
                GetHolidayWorkingRejectedData(HolidayWorkingData);
            }
            else {
                $('#gridHolidayWokingPending').css('display', 'none');
                $('#gridHolidayWokingApproved').css('display', 'none');
                $('#gridHolidayWokingRejected').css('display', 'none');
                $('#gridHolidayWokingAdminCalcel').css('display', 'block');
                GetHolidayWorkingAdminCancelData(HolidayWorkingData);
            }
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetHolidayWorkingPendingData(HolidayWorkingData) {
    $("#gridHolidayWokingPending").empty();
    var gridHolidayWokingPending = $("#gridHolidayWokingPending").kendoGrid({
        dataSource: {
            data: HolidayWorkingData,
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
                    { field: "AdminComment", title: "Admin Comment", width: "20px", template: '<textarea rows="4" cols="20" id="AdminComment"></textarea>' },
                    { command: [{ name: "approve", text: "Approve", id: "btnApprove", click: ApproveHoliday }, { name: "reject", text: "Reject", id: "btnReject", click: RejectHoliday }], width: "30px" }

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

function GetHolidayWorkingApprovedData(HolidayWorkingData) {
    $("#gridHolidayWokingApproved").empty();
    var gridHolidayWokingApproved = $("#gridHolidayWokingApproved").kendoGrid({
        dataSource: {
            data: HolidayWorkingData,
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
                        AdminComment: { type: "string" }
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
                    { field: "AdminComment", title: "Admin Comment", width: "20px" },
                    { field: "AdminCanReason", title: "Admin Cancel Reason", width: "30px", template: '<textarea rows="4" cols="20" id="AdminCancelReason"></textarea>' },
                    {
                        command: [{ name: "CancelHoliday", text: "Cancel", id: "btnCancel", click: CancelHoliday }, { name: "addCompOff", text: "Add CompOff", id: "btnAddCompOff", click: AddCompOff1 }], width: "30px"
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

function GetHolidayWorkingRejectedData(HolidayWorkingData) {
    $("#gridHolidayWokingRejected").empty();
    var gridHolidayWokingRejected = $("#gridHolidayWokingRejected").kendoGrid({
        dataSource: {
            data: HolidayWorkingData,
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
        //dataBound: function () {
        //    var grid = this;
        //    var model;

        //    if ($('#ddlStatus :selected').val() == 0) {
        //        grid.tbody.find("tr[role='row']").each(function () {
        //            model = grid.dataItem(this);
        //            $(this).find(".k-grid-cancel-changes").remove();
        //            $(this).find(".k-grid-addCompOff").remove();

        //        });
        //    }

        //    if ($('#ddlStatus :selected').val() == 1) {
        //        grid.tbody.find("tr[role='row']").each(function () {
        //            model = grid.dataItem(this);
        //            $(this).find(".k-grid-approve").remove();
        //            $(this).find(".k-grid-reject").remove();

        //        });
        //    }
        //},

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

function GetHolidayWorkingAdminCancelData(HolidayWorkingData) {
    $("#gridHolidayWokingAdminCalcel").empty();
    var gridHolidayWokingAdminCalcel = $("#gridHolidayWokingAdminCalcel").kendoGrid({
        dataSource: {
            data: HolidayWorkingData,
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
                        AdminCanReason: { type: "string" }
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
                    { field: "AdminComment", title: "Admin Comment", width: "20px" },
                    { field: "AdminCanReason", title: "Admin Cancel Comment", width: "30px" }

        ],
        //dataBound: function () {
        //    var grid = this;
        //    var model;

        //    if ($('#ddlStatus :selected').val() == 0) {
        //        grid.tbody.find("tr[role='row']").each(function () {
        //            model = grid.dataItem(this);
        //            $(this).find(".k-grid-cancel-changes").remove();
        //            $(this).find(".k-grid-addCompOff").remove();

        //        });
        //    }

        //    if ($('#ddlStatus :selected').val() == 1) {
        //        grid.tbody.find("tr[role='row']").each(function () {
        //            model = grid.dataItem(this);
        //            $(this).find(".k-grid-approve").remove();
        //            $(this).find(".k-grid-reject").remove();

        //        });
        //    }
        //},

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


//approve click
function ApproveHoliday(e) {
    var tr = $(e.target).closest("tr");
    var row = $(e.target).closest("tr");
    var grid = $("#gridHolidayWokingPending").data("kendoGrid");
    var data = grid.dataItem(row);
    var adminComment = $(e.target).closest("tr").find('#AdminComment').val();
    $.ajax({
        type: "POST",
        url: "empHolidayWorkDetails.aspx/ApproveHolidayWorking",
        contentType: "application/json;charset=utf-8",
        data: "{ID:'" + data.Id + "',Empid:'" + empid + "',Comment:'" + adminComment + "',compOffDate:'" + data.HolidayDate + "'}",
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

function bindGrid() {
    GetHolidayWorkingDetails($('[id$="ddlEmp"]').val(), $('#ddlStatus :selected').val(), $("#txtFromDate").val(), $("#txtToDate").val(), $('#hdnLocationID').val());
    return false;
}

//Reject click
function RejectHoliday(e) {
    var row = $(e.target).closest("tr");
    var grid = $("#gridHolidayWokingPending").data("kendoGrid");
    var data = grid.dataItem(row);
    var adminComment = $(e.target).closest("tr").find('#AdminComment').val();
    $.ajax({
        type: "POST",
        url: "empHolidayWorkDetails.aspx/RejectHolidayWorking",
        contentType: "application/json;charset=utf-8",
        data: "{Empid:'" + empid + "',Comment:'" + adminComment + "' ,ID:'" + data.Id + "'}",
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

function CancelHoliday(e) {
    var row = $(e.target).closest("tr");
    var grid = $("#gridHolidayWokingApproved").data("kendoGrid");
    var data = grid.dataItem(row);

    var adminComment = $(e.target).closest("tr").find('#AdminCancelReason').val();
    $.ajax({
        type: "POST",
        url: "empHolidayWorkDetails.aspx/CancelHolidayLeave",
        contentType: "application/json;charset=utf-8",
        data: "{ID:'" + data.Id + "',Empid:'" + data.EmpId + "' ,Comment:'" + adminComment + "',CompOffDate:'" + data.HolidayDate + "'}",
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

function AddCompOff1(e) {
    //alert('Hi');
    var row = $(e.target).closest("tr");
    var grid = $("#gridHolidayWokingApproved").data("kendoGrid");
    var data = grid.dataItem(row);
    var d = new Date("data.HolidayDate");
    var myDate = new Date(data.HolidayDate);
    var compoffdate = myDate.getFullYear() + '-' + ('0' + (myDate.getMonth() + 1)).slice(-2) + '-' + ('0' + myDate.getDate()).slice(-2);
    var trueorfalse = false;
    //Check if comp off already exists
    $.ajax({
        type: "POST",
        url: "empHolidayWorkDetails.aspx/checkCompOffExists",
        contentType: "application/json;charset=utf-8",
        data: "{Empid:'" + data.EmpId + "',CompOffDate:'" + compoffdate + "'}",
        dataType: "json",
        async: true,
        success: function (msg) {
            if (msg.d == "True") {
                $('[id$=msg]').html('');
                $('[id$=msg]').html('Comp Off already Exists!');        
                   
            }
            else {
                $('[id$=msg]').html('');
                compOffDetails(row, e);
                //  trueorfalse = true;
            }

        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
  //  return false;
}

function test() {
    alert("Comp Off already Exists!");
    return false;
}

function compOffDetails(row, e) {
  //  e.preventDefault();
    //Div Popup
    $('#divAddPopup').css('display', '');
    $('[id$="divAddPopupOverlay"]').addClass('k-overlay');
    $('[id$="txtCompOffComment"]').val('');
    var row = row;
    var grid = $("#gridHolidayWokingApproved").data("kendoGrid");
    var data = grid.dataItem(row);
    var adminComment = data.projName;
    $('#hdnCompOFfEmpID').val(data.EmpId);
    $('#hdnholidayDate').val(data.HolidayDate);
    $('#hdnProjectName').val(data.projName);
    $('[id$="lblName"]').html(data.EmpName);
    $('[id$="lblCompOffDate"]').html(data.HolidayDate);
    $('[id$="lblProjects"]').html(data.projName);
}

function SaveCompOff() {
    var comment = $('[id$="txtCompOffComment"]').val() + "...On " + $('#hdnProjectName').val();
    $.ajax({
        type: "POST",
        url: "empHolidayWorkDetails.aspx/CreateCompOff",
        contentType: "application/json;charset=utf-8",
        data: "{Empid:'" + $('#hdnCompOFfEmpID').val() + "',CompOffDate:'" + $('#hdnholidayDate').val() + "' ,Comment:'" + comment + "',EntryBy:'" + empid + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            alert("Comp Off  created successfully!");
            $('#hdnCompOFfEmpID').val('');
            $('#hdnholidayDate').val('');
            $('#hdnProjectName').val('');
            ClosePopUp();
            // bindGrid();
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function ClosePopUp() {
    $('#divAddPopup').css('display', 'none');
    $('[id$="divAddPopupOverlay"]').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}