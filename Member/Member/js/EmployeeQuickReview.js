$(document).ready(function () {
    Get_EmpQuickReviewData();
});
function Get_EmpQuickReviewData() {
    $.ajax({
        type: "POST",
        url: "EmployeeQuickReview.aspx/Get_EmpQuickReviewList",
        contentType: "application/json;charset=utf-8",
        data: "{Mode:'GET_EmpReviewList'}",
        dataType: "json",
        async: true,
        success: function (msg) {
            var data = $.parseJSON(msg.d);
            var mBox = $("#msgBox");
            if (data.length == 0) {
                Bind_EmpQuickReviewGrid(jQuery.parseJSON(msg.d));
            }
            else {
                var obj = $.parseJSON(msg.d);
                Bind_EmpQuickReviewGrid(obj);
                return false;
            }
        },
        error: function (x, e) {
            alert("The call to the server side failed. " + x.responseText);
        }
    });
}
function Bind_EmpQuickReviewGrid(Tdata) {
    $("#Get_EmpQuickReviewGrid").empty();
    var grid = $("#Get_EmpQuickReviewGrid").kendoGrid({
        dataSource: new kendo.data.DataSource({
            data: Tdata,
            schema: {
                model: {
                    id: "EmployeeCode",
                    fields: {
                        EmployeeCode: { type: "number", editable: false },
                        EmployeeName: { type: "string", editable: false },
                        EmpReviewCount: { type: "number", editable: false },
                        LastReviewDate: { type: "date" },
                    }
                },
            },
            pageSize: 30,
        }),
        scrollable: true,
        sortable: true,
        emptyMsg: 'This grid is empty',
        messages: {
            noRecords: "There is no data on current page"
        },
        selectable: "row",
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
            { field: "EmployeeCode", title: "Employee Code", width: "50px", sortable: true },
            { field: "EmployeeName", title: "Employee Name", width: "50px", sortable: true },
            {
                field: "EmpReviewCount", title: "Emp Review Count", width: "50px", sortable: true
                , template: '<a style="padding-left: 100px;cursor:pointer;font-weight: bold;font-size: 15px;" onclick="ShowReviews(this)">#= EmpReviewCount #</a>'
            },
            { field: "LastReviewDate", hidden: false, title: "Last Review Date", width: "35px", format: "{0:dd-MMM-yyyy}", filterable: false },
        ],
        editable: false,
        filterable: {
            extra: false,
            operators: {
                string: {
                    startswith: "Starts with",
                    contains: "Contains",
                    eq: "Is equal to"
                }
            }
        }
    }).data("kendoGrid");
}
function ShowReviews(dataItem) {
    var dataItem = $(Get_EmpQuickReviewGrid).getKendoGrid().dataItem($(dataItem).closest("tr"));
    var EmployeeCode = dataItem.EmployeeCode;
    if (parseInt(dataItem.EmpCount) != 0) {
        $('[id$="lblEmpCode"]').text(dataItem.EmployeeCode);
        $('[id$="lblEmpName"]').text(dataItem.EmployeeName);
        ShowEmpReviews();
        GetEmpReviews(EmployeeCode);
        MoveTop();
    }
}
function GetEmpReviews(EmployeeCode) {
    $.ajax({
        type: "POST",
        url: "EmployeeQuickReview.aspx/Get_ReviewListByEmpCode",
        contentType: "application/json;charset=utf-8",
        data: "{EmployeeCode:'" + EmployeeCode + "',Mode:'" + 'Get_ReviewListByEmpCode' + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            var eventData = jQuery.parseJSON(msg.d);
            BindDetailData(eventData);
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}
function BindDetailData(EventData) {
    $(gridDetail_).kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "EmployeeReview.xlsx",
            allPages: true
        },
        dataSource: {
            data: EventData,
            schema: {
                model: {
                    id: "ReviewId",
                    fields: {
                        ReviewId: { type: "number", editable: false },
                        InsertedOn: { type: "date", editable: false },
                        ReviewText: { type: "string", editable: false },
                        ReviewCreatedBy: { type: "string", editable: false },
                        AcceptedStatus: { type: "string", editable: false },
                        AcceptedDateTime: { type: "date", editable: false },
                        EmployeeCode: { type: "number", editable: false },
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
            { command: [{ name: "edit", click: EditEmpReview }], title: "Action", width: "10px" },
            { field: "ReviewId", hidden: true, title: "Review Id", width: "10px" },
            { field: "EmployeeCode", hidden: true, title: "EmployeeCode", width: "10px" },
            { field: "InsertedOn", hidden: false, title: "Review Date", width: "15px", format: "{0:dd-MMM-yyyy hh:mm tt}" },
            { field: "ReviewCreatedBy", title: "Created By", width: "15px" },
            { field: "ReviewText", title: "Review Text", width: "40px" },
            { field: "AcceptedStatus", title: "Accepted Status", width: "12px" },
            { field: "AcceptedDateTime", hidden: false, title: "Accepted DateTime", width: "13px", format: "{0:dd-MMM-yyyy hh:mm tt}", filterable: false },
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
    });
}
function EditEmpReview(e) {
    var tr = $(e.target).closest("tr");
    var grid = $("#gridDetail_").data("kendoGrid");
    var data = grid.dataItem(tr);
    if (data.AcceptedStatus == "Pending") {
        var ReviewId = data.ReviewId; var EmployeeCode = data.EmployeeCode;
        $('[id$="hdn_ReviewId"]').val(ReviewId);
        $('#txt_EditReviewText').val(data.ReviewText);
        $('[id$="hf_EmpId"]').val(EmployeeCode);
        document.getElementById("div_ShowReviewTxtArea").style.display = "block";
    }
    //else {
    //    document.getElementById("div_ShowReviewTxtArea").style.display = "none";
    //    alert('Cannot edit accepted review.');
    //}
}
function closeEmpDetail() {
    document.getElementById("div_ShowReviewTxtArea").style.display = "none";
    $('#div_EmpReviewDtlPopUp').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}
function ShowEmpReviews() {
    $('#div_EmpReviewDtlPopUp').css('display', '');
}
function MoveTop() {
    $("html, body").animate({ scrollTop: 0 }, "slow");
}

function Show_AddReviewPopup() {
    ClearRecord();
    AddReviewPopup();
}
function AddReviewPopup() {
    $('#div_AddReviewPopup').css('display', '');
    FillEmployeeDropDown();
}
function ClearRecord() {
    if ($("#txtEmployee").data('kendoDropDownList') != undefined && $("#txtEmployee").data('kendoDropDownList') != "") {
        $("#txtEmployee").data('kendoDropDownList').value("");
    }
    $('[id$="txt_ReviewText"]').val("");
}
function closeReviewPopup() {
    $('#div_AddReviewPopup').css('display', 'none');
    $("#lblerrmsgdate").css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}
function FillEmployeeDropDown() {
    $.ajax({
        type: "POST",
        url: "EmployeeQuickReview.aspx/FillEmployeeDropDown",
        contentType: "application/json;charset=utf-8",
        data: "{Mode:'GetEmployee'}",
        dataType: 'json',
        async: false,
        success: function (msg) {
            $("#txtEmployee").kendoDropDownList({
                optionLabel: "Select Employee",
                dataTextField: "empName",
                dataValueField: "empid",
                dataSource: jQuery.parseJSON(msg.d)
            }).data("kendoDropDownList");
        },
        error: function (x, e) {
            alert("The call to the server side failed. " + x.responseText);
        }
    });
}








