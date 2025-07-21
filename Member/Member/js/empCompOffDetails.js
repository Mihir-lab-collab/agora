function Search() {
    if ($("#txtFromDate").val() == '') {
        alert('Please select From Date');
        return false;
    }
    else if ($("#txtToDate").val() == '') {
         alert('Please select End Date');
        return false;
    }
    else if ($("#txtFromDate").val() != '' && $("#txtToDate").val() != '') {
        var date1 = new Date($("#txtFromDate").val());
        var date2 = new Date($("#txtToDate").val());
        if (date1 > date2) {
            alert('From date should be less than To Date');
            return false;
        }
        else {
            var startDate = $("#txtFromDate").val();
            var endDate = $("#txtToDate").val();
            return true;
        }
    }
}
