$(document).ready(function () {
    BindLeaveApproval("0", "", "", "", 0);
    $('[id$="txtFromDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
    $('[id$="txtFromTo"]').kendoDatePicker({ format: "dd/MM/yyyy" });
    $('[id$="txtFDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
    $('[id$="txtTDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    $('[id$="ddlStatus"]').change(function () {

        var name = $('[id$="txtName"]').val();
        var from = $('[id$="txtFromDate"]').val();
        var to = $('[id$="txtFromTo"]').val();
        var status = $('[id$="ddlStatus"]').val();

        var includeArchive = 0;
        if ($('[id$="chkInclude"]').is(":checked"))
            includeArchive = 1;

        BindLeaveApproval(status, name, from, to, includeArchive)
        // return false;
    });
});

function EndRequestHandler() {
    $("#chkAll").change(function () {
        if ($(this).is(":checked")) {
            $("[id*=chkEmplist] input").attr("checked", "checked");
        } else {
            $("[id*=chkEmplist] input").removeAttr("checked");
            $('[id$="btnClear"]').click();
        }
    });

}

function BindLeaveApproval(status, name, from, to, includeArchive) {
    var loginID = $('[id$="hdnLoginID"]').val();
    $.ajax({
        type: "POST",
        url: "EmpLeaveApproval.aspx/GetEmpLeaves",
        contentType: "application/json;charset=utf-8",
        data: "{status:'" + status + "',name:'" + name + "',from:'" + from + "',to:'" + to + "',loginID:'" + loginID + "',includeArchive:'" + includeArchive + "'}",
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
var showhide = "className";
function SetData(EventData) {

    $(gridLeaves).kendoGrid({
        dataSource: {
            data: EventData,
            schema: {
                model: {
                    fields: {
                        EmpID: { type: "string" },
                        EmpName: { type: "string" },
                        EmpNameID: { type: "string" },
                        LeaveType: { type: "string" },
                        LeaveFrom: { type: "string" },
                        LeaveTo: { type: "string" },
                        LeaveAppliedOn: { type: "string" },
                        LeaveReason: { type: "string" },
                        AdminComment: { type: "string" },
                        LeaveStatus: { type: "string" },
                        LeaveSanctionOn: { type: "string" },
                        LeaveSanctionBy: { type: "string" },
                        BalanceLeave: { type: "string" },
                        IsApproved: { type: "string" },
                        TotalLeave: { type: "number" },
                        EmpLeaveID: { type: "string" },
                        IsTeam: { type: "number" }
                        //Leave: { type: "string" }
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
                    //{ title: "select", width: "40px", headerTemplate: "<input type='checkbox' class='checkbox' />" ,},
                    { field: "EmpNameID", title: "Employee Name", width: "100px" },
                    //{
                    //    field: "Leave", width: "50px",
                    //    columns: [ {
                    //        field: "LeaveFrom",title:"From",
                    //        width: "25px"
                    //    },{
                    //        field: "LeaveTo",title:"To",
                    //        width: "25px"
                    //    }]
                    //},
                    { field: "LeaveFrom", title: "Leave From", width: "70px" },
                    { field: "LeaveTo", title: "Leave To", width: "70px" },
                    {
                        field: "LeaveType", title: "LeaveType", width: "50px"
                        , template: '<div class="ra" style="text-align:center;">#= LeaveType #</div>'
                    },
                    {
                        field: "TotalLeave", title: "No of Leaves Applied", width: "100px"
                        , template: '<div class="ra" style="text-align:center;">#= TotalLeave #</div>'
                    },
                    { field: "BalanceLeave", title: "Current Leave Balance", width: "100px" },
                    { field: "LeaveReason", title: "Leave Reason", width: "100px" },
                    { field: "LeaveAppliedOn", title: "Leave Applied On", width: "80px" },
                    { field: "IsApproved", title: "LeaveStatus", width: "70px" },
                    { field: "LeaveSanctionOn", title: "Leave Sanction Date", width: "100px" },
                    { field: "LeaveSanctionBy", title: "Leave Sanction By", width: "80px" },
                    { field: "AdminComment", title: "Admin Comments", width: "100px" },
                    {
                        command: [
                              {
                                  name: "edit", text: "Show", click: LeaveApproval
                              }
                              //, { name: "destroy", text: "delete", className: "ob-delete", click: DeleteEvent }
                        ], width: "70px"
                    }

        ],
        dataBound: function ()
        {
            var grid = this;
            var model;
            if ($('[id$="hdnHrProfileStatus"]').val() == '')
           {
            grid.tbody.find("tr[role='row']").each(function ()
            {
                model = grid.dataItem(this);
                var IsAdmin = model.IsTeam;
                if (IsAdmin == "0") {
                    $(this).find(".k-grid-edit").remove();
                }
                else {

                }
            });
            }
        },

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
        //remove: function (e) {
        //    if (confirm("Are you sure you want to delete?")) {
        //        DeleteEvent(e.model.KEID);
        //    } else {

        //        e.preventDefault()
        //        ClosingRateWindow(e);

        //    }

        //},

    });

}

function Search() {
    var name = $('[id$="txtName"]').val();
    var from = $('[id$="txtFromDate"]').val();
    var to = $('[id$="txtFromTo"]').val();
    var status = $('[id$="ddlStatus"]').val();
    var includeArchive = 0;
    if ($('[id$="chkInclude"]').is(":checked"))
        includeArchive = 1;

    BindLeaveApproval(status, name, from, to, includeArchive)
}

function LeaveApproval(e) {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
    customRowDataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var data = this.dataItem(customRowDataItem);

    $('[id$="hdnEmpID"]').val(customRowDataItem.EmpID);
    $('[id$="hdmEmpLeaveID"]').val(customRowDataItem.EmpLeaveID);
    $('[id$="lblName"]').html(customRowDataItem.EmpName);
    $('[id$="ddlLeaveType"]').val(customRowDataItem.LeaveType);
    $('[id$="ddlleaveStatus"]').val(customRowDataItem.LeaveStatus);
    $('[id$="lblEmpID"]').html(customRowDataItem.EmpID);
    $('[id$="txtAdminComment"]').html(customRowDataItem.AdminComment);
    $('[id$="txtFDate"]').val(customRowDataItem.LeaveFrom);
    $('[id$="txtTDate"]').val(customRowDataItem.LeaveTo);
    $('[id$="lblleavesApplied"]').html(customRowDataItem.TotalLeave);
    $('[id$="txtReason"]').html(customRowDataItem.LeaveReason);
    $('[id$="lblSanctionedBy"]').html(customRowDataItem.LeaveSanctionBy);
    $('[id$="lblleaveEntry"]').html(customRowDataItem.LeaveAppliedOn);
    $('[id$="lblLeaveSanctionedDate"]').html(customRowDataItem.LeaveSanctionOn);

}

function SaveStatus() {
    var empID = $('[id$="hdnEmpID"]').val();
    var empLeaveID = $('[id$="hdmEmpLeaveID"]').val();
    var leaveStatus = $('[id$="ddlleaveStatus"]').val();
    var LeaveType = $('[id$="ddlLeaveType"]').val();
    var AdminComment = $('[id$="txtAdminComment"]').val();
    var SanctionedBy = $('[id$="hdnLoginID"]').val();
    var fDate = $('[id$="txtFDate"]').val();
    var tDate = $('[id$="txtTDate"]').val();

    $.ajax({
        type: "POST",
        url: "EmpLeaveApproval.aspx/UpdateEmpLeaveStatus",
        contentType: "application/json;charset=utf-8",
        data: "{empID:'" + empID + "',empLeaveID:'" + empLeaveID + "',leaveStatus:'" + leaveStatus + "',LeaveType:'" + LeaveType + "',AdminComment:'" + AdminComment + "',SanctionedBy:'" + SanctionedBy + "',fDate:'" + fDate + "',tDate:'" + tDate + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            alert("saved successfully.");
            ClosePopUp();
            Search();
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function ClosePopUp() {
    $('#divAddPopup').css('display', 'none');
}

function CloseEmpPopUp() {
    $('[id$="CloseEmpPopUp"]').css('display', 'none');
}
function OpenEmpList() {
    $('#ngGridSkill').css('display', '');
    angular.element($("#grdEmpSkill")).scope().GetEmployee();
}
function CloseEmpList() {
    $('#ngGridSkill').css('display', 'none');
}