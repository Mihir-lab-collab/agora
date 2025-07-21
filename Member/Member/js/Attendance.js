
$(document).ready(function () {
     $('[id$="txtFromDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
    $('[id$="txtToDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });

});
function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}

function doOpen(obj,id, date, EmpName, strUpdate)
{
    var content = "<b>Name:</b> " + EmpName + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>Date:</b> " + getFormattedDate(date) + " <br /></span><div>&nbsp;</div> <span class='small_button white_button open' onclick=\"doLog('" + id + "','" + date + "')\" >Log</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class='small_button white_button open' onclick= \"doAttandance('" + id + "','" + date + "','" + EmpName + "','" + strUpdate + "')\" >Update</span>"
    document.getElementById("actionLink").innerHTML = content;
    openActionPopUP(obj);
}

function doLog(id, date)
{
    var FDate = date.split("/");;
    var DbFDate = FDate[2] + "/" + FDate[1] + "/" + FDate[0];

    $.ajax({
        type: "POST",
        url: "Attendance.aspx/BindAttLog",
        contentType: "application/json;charset=utf-8",
        data: "{'EmpId':'" + id + "','Date':'" + DbFDate + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetLogData(jQuery.parseJSON(msg.d));
            $('#divLogPopup').css('display', '');
            $('#divAddPopupOverlay').addClass('k-overlay');
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetLogData(Tdata) {
    $("#divAttLodDatils").kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        EmpID: { type: "number" },
                        PunchTime: { type: "string" },
                        IP: { type: "string" },
                        Mode: { type: "string" },
                        InsertedOn: { type: "string" }
                    }
                }
            },
            pageSize: 10,
        },
        scrollable: true,
        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
                    { field: "EmpID", title: "Emp Id", width: "20px" },
                    { field: "PunchTime", title: "Punch Time", width: "30px" },
                    { field: "IP", title: "IP", width: "20px" },
                    { field: "Mode", title: "Mode", width: "20px" },
                    { field: "InsertedOn", title: "Inserted On", width: "30px" }
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

function doClose()
{
    $('#divAction').css('display', 'none');
}
//debugger;
function doAttandance(id, date, EmpName, strUpdate) {
   
    openAddPopUP();

  //  $("#ctl00_ContentPlaceHolder1_txtFromDate").kendoDatePicker({ format: "dd/MM/yyyy" });
   // $("#ctl00_ContentPlaceHolder1_txtToDate").kendoDatePicker({ format: "dd/MM/yyyy" });
    //$('[id$="txtFromDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
    //$('[id$="txtToDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });

    $("#ctl00_ContentPlaceHolder1_txtFromDate").val(date);
    $("#ctl00_ContentPlaceHolder1_txtToDate").val(date);
    $("#ctl00_ContentPlaceHolder1_txtEmpName").val(EmpName);
    $("#ctl00_ContentPlaceHolder1_hdStatusUpdate").val(strUpdate);
    $("#ctl00_ContentPlaceHolder1_hdEmpid").val(id);
    var Time  = "09" + ":" + "30" + ":00";
    $("#ctl00_ContentPlaceHolder1_hdnTime").val(Time);
    $("#txtFromDate").kendoDatePicker();
    // $('[id$="txtFromDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
    //$('[id$="txtToDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
   // openAddPopUP();
}



function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}

function openActionPopUP(obj) {
        var offset = $(obj).position();
        var height = $('#divAction').height();
        var width = $('#divAction').width();
        leftVal = offset.left - (width) + "px";
        topVal = offset.top - (height) + "px";
        $('#divAction').css({ left: leftVal, top: topVal });
        $('#divAction').css('display', '');
}



//function OpendivFilter()
//{

//    var element = document.getElementById("display");
//    element.classList.toggle("mystyle");

//}

var isFlag = false;
function ClosedivFilter(obj)
{
    isFlag = !isFlag;
    if (isFlag)
    {
        $('#divFilter').css('display', '');
    }
    else
    {
        $('#divFilter').css('display', 'none');
    }

}
    

function closeActionPopUP() {
    $('#divAction').css('display', 'none');
}

function closeLogPopUP() {
    $('#divLogPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}

function PopupHours()
{
    $('#DivPopupHours').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
    $("#ctl00_ContentPlaceHolder1_hdnMonthNoHR").val($("#ctl00_ContentPlaceHolder1_hdnMonthNo").val());
    $("#ctl00_ContentPlaceHolder1_hdnYearHR").val($("#ctl00_ContentPlaceHolder1_hdnYear").val());
}

function closePopupHours() {
    $('#DivPopupHours').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}

function CompDate(adate, bdate) {
    if (Date.parse(adate) > Date.parse(bdate)) {
        alert("Start date should not be greater than end date");
        return false;
    }
    else {
        return true;
    }
}

function DateCompaire() {
    return CompDate($("#ctl00_ContentPlaceHolder1_txtFromDate").val(), $("#ctl00_ContentPlaceHolder1_txtToDate").val());
}

function openLoading() {
   
    $('#divLoading').css('display', '');
    $('#divAddLoadingOverlay').removeClass("k-overlayDisplaynone");
    $('#divAddLoadingOverlay').addClass('overlyload');
    $('#divAddLoadingOverlay').css('display', '');
    setTimeout("closeLoading()", 3000);
    
}

function closeLoading() {
   
    $('#divLoading').css('display', 'none');
    $('#divAddLoadingOverlay').removeClass("overlyload").addClass("k-overlayDisplaynone");
    $('#divAddLoadingOverlay').css('display', 'none');
}



function getFormattedDate(input) {
    var pattern = /(.*?)\/(.*?)\/(.*?)$/;
    var result = input.replace(pattern, function (match, p1, p2, p3) {
        var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        return (p1 < 10 ? "0" + p1 : p1) + " " + months[(p2 - 1)] + " " + p3;
    });
    return result;
}







