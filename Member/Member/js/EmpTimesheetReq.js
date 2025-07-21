$(document).ready(function () {
    GetEvents(0);
    BindAllDateCalender();
   // BindTime();
});

function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    RedirectPage();
}

function ShowAddPopup() {
    $('[id$="hdnKEID"]').val("0");
   
    openAddPopUP();
}
function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
  
    Clearrecord();
}
function Clearrecord() {
    $('[id$="hdnEventDate"]').val('');
    $('[id$="txtEventDate"]').val('');
    $('[id$="hdnDescription"]').val('');
    $('[id$="txtNarration"]').val('');
}

function BindAllDateCalender() {
    $("#txtEventDate").kendoDatePicker({ format: "dd/MM/yyyy" });
}


function BindAllEditDateCalender() {
    $("#txtEventDate").kendoDatePicker({ format: "dd/MM/yyyy" });
}

function BindOldCIP() {
    if ($('[id$="chkOLD"]').prop("checked"))
        GetEvents(1);
    else
        GetEvents(0);
}

function GetEvents(isOLD) {
    var leavingstatus = $('#ctl00_ContentPlaceHolder1_Filterdropdown').val();
    $.ajax({
        type: "POST",
        url: "EmployeeTimesheetRequest.aspx/BindEvents",
        contentType: "application/json;charset=utf-8",
        data: "{KEID:'" + parseInt(leavingstatus) + "'}",
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

function ClosingRateWindow(e) {

    var grid = $(gridEvents).data("kendoGrid");
    grid.refresh();

}

function SetData(EventData) {

    $(gridEvents).kendoGrid({
        dataSource: {
            data: EventData,
            schema: {
                model: {
                    fields: {
                        Id: { type: "number" },
                        Requestdate1: { type: "date", format: "{0:dd-MMM-yyyy}" },
                        Descripation: { type: "string" },
                        EmployeeName: { type: "string" },
                        ApprovedBy: { type: "string" },
                        InsertedOn: { type: "date", format: "{0:dd-MMM-yyyy}" },
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
                    { field: "Id", title: "ID", width: "50px", hidden: true },
                    { field: "Requestdate1", title: "Date", width: "20px", format: "{0:dd-MMM-yyyy}" },
                    { field: "EmployeeName", title: "Requested By", width: "20px" },
                    { field: "Descripation", title: "Reason", width: "80px" },
                    { field: "ApprovedBy", title: "ApprovedBy", width: "20px" },
                    { field: "InsertedOn", title: "Approval Date", width: "20px", format: "{0:dd-MMM-yyyy}" },
                  

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


function RedirectPage() {
    window.location.assign("EmployeeTimesheetRequest.aspx");
}