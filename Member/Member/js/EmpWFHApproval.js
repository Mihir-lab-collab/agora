$(document).ready(function () {
    BindWFHApproval("0", "", "", "", 0);
    $('[id$="txtFromDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
    $('[id$="txtFromTo"]').kendoDatePicker({ format: "dd/MM/yyyy" });
    $('[id$="txtFDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
    $('[id$="txtTDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
    $('[id$="txtFromDateWFH"]').kendoDatePicker({ format: "dd/MM/yyyy", min: new Date()  });
    $('[id$="txtToDateWFH"]').kendoDatePicker({ format: "dd/MM/yyyy", min: new Date()  });

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    $('[id$="ddlStatus"]').change(function () {
        var name = $('[id$="txtName"]').val();
        var from = $('[id$="txtFromDate"]').val();
        var to = $('[id$="txtFromTo"]').val();
        var status = $('[id$="ddlStatus"]').val();

        var includeArchive = 0;
        if ($('[id$="chkInclude"]').is(":checked"))
            includeArchive = 1;

        BindWFHApproval(status, name, from, to, includeArchive)
        // return false;
    });
});
function BindWFHApproval(status, name, from, to, includeArchive) {
    var loginID = $('[id$="hdnLoginID"]').val();
    $.ajax({
        type: "POST",
        url: "EmpWFHApproval.aspx/GetEmpWFH",
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
function SetData(EventData) {

    $(gridWFH).kendoGrid({

        dataSource: {
            data: EventData,
            schema: {
                model: {
                    fields: {
                        EmpID: { type: "string" },
                        EmpName: { type: "string" },
                        EmpNameID: { type: "string" },
                        WHFFrom: { type: "string" },
                        WHFTo: { type: "string" },
                        WFHAppliedOn: { type: "string" },
                        WHFReason: { type: "string" },
                        AdminComment: { type: "string" },
                        WHFStatus: { type: "string" },
                        WFHSanctionOn: { type: "string" },
                        WFHSanctionBy: { type: "string" },
                        IsApproved: { type: "string" },
                        TotalWFH: { type: "number" },
                        EmpWHFID: { type: "string" },
                        IsTeam: { type: "number" },
                        WFHEntryDate: { type: "string" },
                        AttInTime: { type: "string" },
                        AttOutTime: { type: "string" }
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
            { field: "EmpNameID", title: "Employee Name", width: "100px" },
            { field: "WFHFrom", title: "WFH From", width: "80px" },
            { field: "WFHTo", title: "WFH To", width: "70px" },
            { field: "WFHReason", title: "WFH Reason", width: "100px" },
            { field: "WFHAppliedOn", title: "WFH Applied On", width: "80px" },
            { field: "IsApproved", title: "WFH Status", width: "80px" },
            { field: "WFHSanctionOn", title: "WFH Sanction Date", width: "100px" },
            { field: "WFHSanctionBy", title: "WFH Sanction By", width: "80px" },
            { field: "AdminComment", title: "Admin Comments", width: "100px" },
            {
                command: [
                    {

                        name: "edit", text: "Show", click: WFHApproval
                    }
                    //, { name: "destroy", text: "delete", className: "ob-delete", click: DeleteEvent }
                ], width: "70px"
            }

        ],
        dataBound: function () {
            var grid = this;
            var model;

            if ($('[id$="hdnHrProfileStatus"]').val() == '') {

                grid.tbody.find("tr[role='row']").each(function () {
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
    });

}
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
function CheckAllEmployee() {
    // Remove any existing change event handlers from the checkbox
    $("#chkAllEmployee").off("change").on("change", function () {
        if ($(this).is(":checked")) {
            $("[id*=cblEmployeeWFH] input").prop("checked", true);
        } else {
            $("[id*=cblEmployeeWFH] input").prop("checked", false);
            //$('[id$="btnBulkWFHClear"]').click();
        }
    });
}
function CheckDate() {
    var reasonError = $("#lblReasonError");
    var reason = $("#txtReasonForWFH").val();
    var fromDateError = $("#lblFromDateError");
    var toDateError = $("#lblToDateError");
    var dateSpan = $("#lblDateError");
    let fromDate = $('#txtFromDateWFH').val();
    let toDate = $('#txtToDateWFH').val();
    $('[id$="hdnFromDateWFH"]').val(fromDate);
    $('[id$="hdnToDateWFH"]').val(toDate);
    if (fromDate == "") {
        fromDateError.html("Date field cannot be blank");
        return false;
    }
    else {
        fromDateError.html("");

    }
    if (toDate == "") {
        toDateError.html("Date field cannot be blank");
        return false;
    }
    else {
        toDateError.html("");
    }
    if (reason == "") {
        reason.html("Reason field cannot be blank");
        return false;
    }
    if (fromDate > toDate) {
        fromDateError.html("From date should be less than or equal to To date.");
        return false;
    }
    else {
        reason.html("");

    }

    return true;


}
function WFHApproval(e) {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
    customRowDataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var data = this.dataItem(customRowDataItem);

    $('[id$="hdnEmpID"]').val(customRowDataItem.EmpID);
    $('[id$="hdnEmpWFHID"]').val(customRowDataItem.EmpWFHID);
    $('[id$="lblName"]').html(customRowDataItem.EmpName);
    $('[id$="ddlWFHStatus"]').val(customRowDataItem.WFHStatus);
    $('[id$="lblEmpID"]').html(customRowDataItem.EmpID);
    $('[id$="txtAdminComment"]').html(customRowDataItem.AdminComment);
    $('[id$="txtFDate"]').val(customRowDataItem.WFHFrom);
    $('[id$="txtTDate"]').val(customRowDataItem.WFHTo);
    $('[id$="txtReason"]').html(customRowDataItem.WFHReason);
    $('[id$="lblSanctionedBy"]').html(customRowDataItem.WFHSanctionBy);
    $('[id$="lblWFHEntry"]').html(customRowDataItem.WFHEntryDate);
    $('[id$="lblWHFSanctionedDate"]').html(customRowDataItem.WFHSanctionOn);
    $('[id$="lblAttendanceIn"]').html(customRowDataItem.AttInTime);
    $('[id$="lblAttendanceOut"]').html(customRowDataItem.AttOutTime);

}
function Search() {
    var name = $('[id$="txtName"]').val();
    var from = $('[id$="txtFromDate"]').val();
    var to = $('[id$="txtFromTo"]').val();
    var status = $('[id$="ddlStatus"]').val();
    if (from != "" && to != "") {
        if (from > to) {
            alert("From date should be less than or equal to To date.");
            return false;
        }
    }

    var includeArchive = 0;
    if ($('[id$="chkInclude"]').is(":checked"))
        includeArchive = 1;

    BindWFHApproval(status, name, from, to, includeArchive)
}
function SaveStatus() {
    document.getElementById("divOverlay").style.display = "flex";
    document.getElementById("loading-overlay").style.display = "flex";
    var empID = $('[id$="hdnEmpID"]').val();
    var empWFHID = $('[id$="hdnEmpWFHID"]').val();
    var WFHStatus = $('[id$="ddlWFHStatus"]').val();
    var AdminComment = $('[id$="txtAdminComment"]').val();
    var SanctionedBy = $('#ctl00_ContentPlaceHolder1_hdnEmpName').val();
    var LoginEmpId = $('#ctl00_ContentPlaceHolder1_hdnLoginID').val();
    var fDate = $('[id$="txtFDate"]').val();
    var tDate = $('[id$="txtTDate"]').val();
        $.ajax({
            type: "POST",
            url: "EmpWFHApproval.aspx/UpdateEmpWFHStatus",
            contentType: "application/json;charset=utf-8",
            data: "{empID:'" + empID + "',empWFHID:'" + empWFHID + "',WFHStatus:'" + WFHStatus + "',AdminComment:'" + AdminComment + "',SanctionedBy:'" + SanctionedBy + "',fDate:'" + fDate + "',tDate:'" + tDate + "',LoginEmpId:'" + LoginEmpId + "'}",
            dataType: "json",
            async: true,
            success: function (msg) {
                HidePopup();
                alert("Saved Successfully.");
                ClosePopUp();
                Search();
            },
            error: function (x, e) {
                alert("The call to the server side failed. "
                    + x.responseText);
            }
        });
    function HidePopup() {
        document.getElementById("divOverlay").style.display = "none";
        document.getElementById("loading-overlay").style.display = "none";
    }
}

function ClosePopUp() {
    $('#divAddPopup').css('display', 'none');
}