$(document).ready(function () {

    $("#txtStartDate").kendoDatePicker({ format: "dd/MM/yyyy" });
    $("#txtEndDate").kendoDatePicker({ format: "dd/MM/yyyy" });
    $('[id$="txtStartDate"]').val('');
    $('[id$="txtEndDate"]').val('');
    $('#txtStartDate').bind("change", function () {
            CheckDate();
    });
    $('#txtEndDate').bind("change", function () {
            CheckDate();
    });
    // For WFH
    $("#txtFromDate").kendoDatePicker({ format: "dd/MM/yyyy", min: new Date() });
    $("#txtToDate").kendoDatePicker({ format: "dd/MM/yyyy", min: new Date() });
    $('[id$="txtFromDate"]').val('');
    $('[id$="txtToDate"]').val('');
    $('#txtFromDate').bind("change", function () {
        CheckDate();
    });
    $('#txtToDate').bind("change", function () {
        CheckDate();
    });


});
function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    //window.location.reload();
}
function closeAttendancePopUp() {
    $('#divAddPopupForFillAttendance').css('display', 'none');
    $('#divAddPopupOverlayForFillAttendance').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}
function ShowAddPopup() {    
    openAddPopUP();
    //$("#txtEditConfigID").val('');
}
function openAddPopUP() {    
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}
function dateInput(ob) {
    var invalidChars = /[^]/gi
    if (invalidChars.test(ob.value)) {
        ob.value = ob.value.replace(invalidChars, "");
    }
}
function ShowAddPopupForFillAttendance() {
    openAddPopUPForFillAttendance();
    //$("#txtEditConfigID").val('');
}
function openAddPopUPForFillAttendance() {
    $('#divAddPopupForFillAttendance').css('display', '');
    $('#divAddPopupOverlayForFillAttendance').addClass('k-overlay');
}