



// Bind Grid By Satish Vishwakarma
$(document).ready(function () {
    $.ajax({
        type: "POST",
        url: "AdminJS.aspx/GetAllProjectsByCustId",

        contentType: "application/json;charset=utf-8",
        data: "{'customerId':'" + custid + "'}",
        dataType: "json",
        success: function (msg) {
            GetProjectdata(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
});


function GetProjectdata(Pdata) {
    $(dropDownProject).kendoDropDownList({
        dataTextField: "projName",
        dataValueField: "projId",
        dataSource: Pdata,
        cascade: onSetProject,
        change: function () {
            var value = this.value();
            //var text = this.text();

            //if (text) {
            //    alert(text);
            //}


        }
    });
}

var onSetProject = function () {
    var sst = $("#dropDownProject").val()
    //var sstnew = $("#dropDownProject option:selected").text();
    var text = this.text();
    var value = this.value();
    //if (text) {
    //    alert(text);
    //}
    //alert(sst);
    $.ajax({
        type: "POST",
        url: "AdminJS.aspx/SetSession",
        contentType: "application/json;charset=utf-8",
        data: "{'projId':'" + value + "','projName':'" + text + "'}",
        dataType: "json",
        success: function (msg) {
            //var dropdownlist = $("#dropDownProject");
            //Write your logic here to bind data for thie dropdown
            // disable the dropdown list
            //dropdownlist.enable(true);
            jQuery.parseJSON(msg.d);
           
            //if ($('#txt_proj').val() != value) {
            //    location.reload();
            //    $('#txt_proj').val(value);
            //}
            //else { location.reload(); }

        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
            event.preventDefault();
        }

    });
}

