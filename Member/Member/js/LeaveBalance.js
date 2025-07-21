$(document).ready(function () {
    GetLeaveBalancedetails();

});

function GetLeaveBalancedetails() {
    $.ajax({
        type: "POST",
        url: "LeaveBalanceReport.aspx/BindLeaveBalance",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetLeaveBalanceData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}
function GetLeaveBalanceData(Tdata) {
    $(gridLeaveBalance).kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "Employee Leave Balance.xlsx",
            allPages: true
        },
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        EmpID: { type: "number" },
                        EmpName: { type: "string" },
                        LeaveSummary: { type: "string" }
                    }
                }
            },
            pageSize: 50,
        },
        scrollable: true,
        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
            { field: "EmpID", title: "Employee Id", width: "50px" },
            { field: "EmpName", title: "Employee Name", width: "80px" },
            { field: "LeaveSummary", title: "Leave Balance", width: "80px" }
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
        selectable: "row",
        change: function () {
            var selectedRow = this.select();
            var dataItem = this.dataItem(selectedRow);
            onRowClick(dataItem);
        },
        dataBound: function () {
            if ($(".k-grid .leave-tip").length === 0) {
                $(".k-grid .k-grid-toolbar").append(
                    "<span class='leave-tip' style='color: red; margin-left: 20px; font-weight: bold;'>* If you select the row, you will get the entire leave details of the employee.</span>"
                );
            }
        }
    });
}

function onRowClick(rowData) {
    $("#ctl00_ContentPlaceHolder1_lblEmpId").text(rowData.EmpID);
    $("#ctl00_ContentPlaceHolder1_lblEmpName").text(rowData.EmpName);
    $.ajax({
        type: "POST",
        url: "LeaveBalanceReport.aspx/GetLeaveDetails",
        data: JSON.stringify({ empId: rowData.EmpID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            ShowAddPopup();
            var html = response.d;
            $("#leaveGridContainer").html(html);
        },
        error: function (xhr, status, error) {
            alert("Error: " + error);
        }
    });
}
function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}

function ShowAddPopup() {
    openAddPopUP();    
}
function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}