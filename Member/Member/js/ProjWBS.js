
$(document).ready(function () {
    var isSubmitted = false;
    var counter = 0;
    var showCompletedStatus = 0;
    var checkExists = 0;
    GetMilestoneDetails();
    GetProjectWBSData(showCompletedStatus);
    GetProjectWBSDetails('', '');
    BindMilestone();
    FillMilestoneitems(counter);
    hideButton();
    jQuery('input.addrow').click(function (event) {
        event.preventDefault();
        counter++;
        var newRow = jQuery('<tr id="' + counter + '"> <td align="center" class="row" ><input id="txtMilestone' + counter + '"  type="text"  style="width: 120px" runat="server" name="txtMilestone' + counter + '" /><span id="lblerrmsgMilestone' + counter + '" style="color: Red;"></span> </td><td align="center" class="row"><input id="txteditWBS' + counter + '" type="text" name="txteditWBS' + counter + '" style="width: 120px" class="k-textbox" runat="server" /><span id="lblerrmsgWBS' + counter + '" style="color: Red;"></span></td> <td class="row"><input id="txtWBSSDate' + counter + '"  class="date" onkeyup="return false" onkeypress="return false" name="txtWBSSDate' + counter + '" style="width: 185px" runat="server" /><span id="lblerrmsgSDate' + counter + '" style="color: Red;"></span></td><td class="row"> <input id="txtWBSEDate' + counter + '"  class="date" onkeyup="return false" onkeypress="return false" name="txtWBSEDate' + counter + '" style="width: 185px" runat="server" /><span id="lblerrmsgEDate' + counter + '" style="color: Red;"></span></td><td class="row"><div  style="white-space:nowrap;"><input id="txteditPlannedHours' + counter + '" type="text" name="txteditPlannedHours' + counter + '" value="9" style="width:50px" class="k-textbox inputHours" runat="server" /><select id="dropDownMin' + counter + '" class="pannedhrs"><option value="0">00</option><option value="1">30</option></select></div> &nbsp;&nbsp;&nbsp;<span id="lblerrmsgtxteditPlannedHours' + counter + '" style="color: Red;"></span></td><td class="row"><input id="txtassignto' + counter + '" multiple="multiple" data-placeholder="select development team" name="txtassignto' + counter + '" style="width: 150px" class="k-textbox" runat="server" /><span id="lblerrmsgEmployeeSelect' + counter + '" style="color: Red;"></span> </td><td align="center" class="row"><input id="txtStatus' + counter + '" type="text" name="txtStatus' + counter + '" style="width: 100px" validationmessage="Please Enter Status" runat="server" /><span id="lblerrmsgStatus' + counter + '" style="color: Red;"></span> </td><td align="center" class="row"><textarea id="txtremark' + counter + '" rows="4" cols="50" style="width: 150px" validationmessage="Please Enter Remark" runat="server"></textarea></td>  <td><input type="button" id="btnDelete' + counter + '" class="buttondel" style="width:21px;height: 21px;" onclick="removeRow()"/></td></tr>');
        FillMilestoneitems(counter);
        jQuery('table.WBSLIST').append(newRow);
        setTimeout(function () {
            $('input[type=text].date').bind("change", function () {

                var dateval = $(this)[0].id;


                var trid = $(this).closest('tr').attr('id');

                var str1 = dateval.substring(0, dateval.length - 1);

                var str1 = dateval;
                var str2 = "txtWBSSDate";
                var str3 = "txtWBSEDate";
                if (str1.indexOf(str2) != -1 || str1.indexOf(str2)) {

                    if (trid == "undefined") {
                        var txtStartDate = $('[id$="txtWBSSDate' + trid + ']').val();
                        var txtEndDate = $('[id$=txtWBSEDate' + trid + ']').val();

                        ShowPlannedHours(txtStartDate, txtEndDate, trid, 0)
                    }
                    else {

                        var txtStartDate = $('[id$=txtWBSSDate' + trid + ']').val();
                        var txtEndDate = $('[id$=txtWBSEDate' + trid + ']').val();
                        ShowPlannedHours(txtStartDate, txtEndDate, trid, 0)
                    }
                }

            });
        }, 500);

        $('input[type=text].inputHours').keydown(function (event) {

            if (event.shiftKey == true) {
                event.preventDefault();
            }

            if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 46 || event.keyCode == 190) {

            } else {
                event.preventDefault();
            }

            if ($(this).val().indexOf('.') !== -1 && event.keyCode == 190)
                event.preventDefault();


        });

    });

    $('input[type=text].date').bind("change", function () {

        var dateval = $(this)[0].id;

        if (dateval === "ctl00_ContentPlaceHolder1_txtWBSSDate") {

            var txtStartDate = $('[id$=txtWBSSDate]').val();
            var txtEndDate = $('[id$=txtWBSEDate]').val();

            ShowPlannedHours(txtStartDate, txtEndDate, 0, 0)
        }

        if (dateval === "ctl00_ContentPlaceHolder1_txtWBSEDate") {

            var txtStartDate = $('[id$=txtWBSSDate]').val();
            var txtEndDate = $('[id$=txtWBSEDate]').val();

            ShowPlannedHours(txtStartDate, txtEndDate, 0, 0)
        }

        if (dateval === "ctl00_ContentPlaceHolder1_txtEditWBSSDate") {

            var txtStartDate = $('[id$=txtEditWBSSDate]').val();
            var txtEndDate = $('[id$=txtEditWBSEDate]').val();

            ShowPlannedHours(txtStartDate, txtEndDate, 0, 1)
        }

        if (dateval === "ctl00_ContentPlaceHolder1_txtEditWBSEDate") {

            var txtStartDate = $('[id$=txtEditWBSSDate]').val();
            var txtEndDate = $('[id$=txtEditWBSEDate]').val();

            ShowPlannedHours(txtStartDate, txtEndDate, 0, 1)
        }

    });

    $('select').on('change', function (e) {
        var optionSelected = $("option:selected", this);
        var valueSelected = this.value;
        $("#hdnMin").val(valueSelected);
    });


    $('input[type=text].inputHours').keydown(function (event) {


        if (event.shiftKey == true) {
            event.preventDefault();
        }

        if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 46 || event.keyCode == 190) {

        } else {
            event.preventDefault();
        }

        if ($(this).val().indexOf('.') !== -1 && event.keyCode == 190)
            event.preventDefault();


    });


    $('[id$="chkShowAllTasks"]').change(function () {

        if ($('[id$="chkShowAllTasks"]').is(':checked')) {
            showCompletedStatus = 1;
            GetProjectWBSData(showCompletedStatus);
        }
        else {
            showCompletedStatus = 0;
            GetProjectWBSData(showCompletedStatus);
        }

    });


    $('input[type=text].changeDate').bind("change", function () {
        var WBSSDate = $("#ctl00_ContentPlaceHolder1_txtSDate").val();
        var WBSEDate = $("#ctl00_ContentPlaceHolder1_txtEDate").val();
        var EmpID;
        var checkadmin = $('[id$="hdnAdmin"]').val();
        if (checkadmin === "true") {
            EmpID = $("#ctl00_ContentPlaceHolder1_txtName").val();
        }
        else {
            EmpID = $('[id$="hdnEmpId"]').val();
        }

        if (new Date(WBSSDate) > new Date(WBSEDate)) {
            alert("Selected DateTime is Invalid");
            return false;
        }
        else {
            GetProjectWBSDetails(WBSSDate, WBSEDate);
        }

        $.ajax({
            type: "POST",
            url: "ProjectWBS.aspx/checkWBSExists",
            contentType: "application/json;charset=utf-8",
            data: "{'StartDate':'" + WBSSDate + "','EndDate':'" + WBSEDate + "','EmpID':'" + EmpID + "'}",
            dataType: "json",
            async: false,
            cache: false,
            timeout: 30000,
            success: function (msg) {
                if (msg.d == "true") {

                    $('[id$="hdnCheckExists"]').val(1);

                }
                else {
                    $('[id$="hdnCheckExists"]').val(0);
                }
            },
            error: function (x, e) {

            }
        });
    });
});

var CheckEditValue = "";
var AssignStartDateValue = "";
var AssignEndDateValue = "";


function CheckInsert() {
    var GetCurrentDate = document.getElementById('hdnGetCurrentDate').value;

    //edit
    if ($('#divEditPopupWBS').is(':visible')) {
        var errMilestone = $("#lblEditerrmsgMilestone");
        if ($("#ctl00_ContentPlaceHolder1_txtEditMilestone").val() == '') {
            errMilestone.html('Please select Milestone');
            return false;
        }
        else {
            errMilestone.html("");
        }
        var errWBS = $("#lblEditerrmsgWBS");
        if ($("#ctl00_ContentPlaceHolder1_txtEditeditWBS").val() == '') {
            errWBS.html('Please enter WBS details');
            return false;
        }
        else {
            errWBS.html("");
        }
        var errSDate = $("#lblEditerrmsgSDate");
        if ($("#ctl00_ContentPlaceHolder1_txtEditWBSSDate").val() == '') {
            errSDate.html('Please select Start Date');
            return false;
        }
        else {
            errSDate.html("");
        }
        var errEDate = $("#lblEditerrmsgEDate");
        if ($("#ctl00_ContentPlaceHolder1_txtEditWBSEDate").val() == '') {
            errEDate.html('Please select End Date');
            return false;
        }
        else {
            errEDate.html("");
        }

        var WBSSDate = $("#ctl00_ContentPlaceHolder1_txtEditWBSSDate").val();
        var WBSEDate = $("#ctl00_ContentPlaceHolder1_txtEditWBSEDate").val();
        var errDate = $("#lblEditerrmsgDate");
        if (new Date(WBSSDate) > new Date(WBSEDate)) {
            errDate.html("Selected DateTime is Invalid");
            return false;
        }
        else {
            errDate.html("");
        }

        var errStartDate = $("#ctl00_ContentPlaceHolder1_txtSDate").val();
        var errEndDate = $("#ctl00_ContentPlaceHolder1_txtEDate").val();
        var errorDate = $("#lblerrormsgDate");


        if (new Date(errStartDate) > new Date(errEndDate)) {
            errorDate.html("Selected DateTime is Invalid");
            return false;

        }
        else {
            errorDate.html("");

        }

        var errEmp = $("#lblEditerrmsgEmployeeSelect");
        if ($('[id$="hdAssignTo"]').val() == '') {
            errEmp.html('Please select development team');
            return false;
        }
        else {
            errEmp.html("");
        }
        var errstatus = $("#lblEditerrmsgStatus");
        if ($('[id$="ctl00_ContentPlaceHolder1_txtEditStatus"]').val() == '') {
            errstatus.html('Please select status');
            return false;
        }
        else {
            errstatus.html("");
        }




        if (WBSSDate == "" && WBSEDate == "") {

            var StartDate = $('[id$=txtSDate]').val();
            var EndDate = $('[id$=txtEDate]').val();

            if (StartDate > EndDate) {
                alert("Start date can not be greater than end date.");
                return false;
            }
            else if (EndDate < StartDate) {
                alert("End date can not be less than start date");
                return false;
            }

            var CheckEndDate = new Date(EndDate.substr(0, 11));
            var CheckStartDate = new Date(StartDate.substr(0, 11));

            var TotalTimes = CheckEndDate.getTime() - CheckStartDate.getTime();

            if (isNaN(TotalTimes)) {
                var TotalDays = 0;
                alert("Your start date and end date is not correct.");
                return false;
            }
            else {
                var start_actual_time = CheckStartDate;
                var end_actual_time = CheckEndDate;

                start_actual_time = new Date(start_actual_time);
                end_actual_time = new Date(end_actual_time);

                var diff = end_actual_time - start_actual_time;

                var diffSeconds = diff / 1000;
                var HH = Math.floor(diffSeconds / 3600);
                var MM = Math.floor(diffSeconds % 3600) / 60;

                if (parseInt(HH) > 8)
                {
                      alert("You can not fill timesheet for more than eight hours at a time.");
                      return false;
                }
                else if (typeof TotalTimes === 'undefined' || TotalTimes === null) {
                    alert("Difference between start date and end date should not be more than 1 day.");
                    return false;
                }
                else {
                    var TotalDays = Math.round(Math.abs(TotalTimes / (1000 * 60 * 60 * 24)));
                    if (TotalDays > 1) {
                        alert("Difference between start date and end date should not be more than 1 day.");
                        return false;
                    }
                }
            }
        }


        var errstatus = $("#lblerrmsgtxtPlannedHours");
        if ($('[id$="ctl00_ContentPlaceHolder1_txtPlannedHours"]').val() == '') {
            errstatus.html('Please Enter Hours');
            return false;
        }
        else {
            errstatus.html("");
        }


        return true;
    }
    //end


    //Timesheet edit validation 
    if ($('#divAddPopup').is(':visible')) {
        var checkupdate;
        if ($('[id$="btnSaveWBS"]').val() == "SAVE" || $('[id$="btnSaveWBS"]').val() == "Save") {
            checkupdate = 0;
        }

        if ($('[id$="btnSaveWBS"]').val() == "UPDATE") {
            checkupdate = 1;
        }

        var errTimesheet = $("#lblerrmsgTimesheet");
        var WBSName = $("#ctl00_ContentPlaceHolder1_txtWBS").val();

        if ($('[id$="btnSaveWBS"]').val('SAVE')) {
            if ($('[id$="hdnWBSName"]').val() == '') {
                $('[id$="hdnWBSName"]').val(WBSName);


            }
            // checkupdate = 1;
        }

        if ($('[id$="hdnWBSName"]').val() == '') {
            errTimesheet.html('Please select WBS');
            return false;
        }
        else {
            errTimesheet.html("");
        }


        var errName = $("#lblerrmsgName");
        if ($("#ctl00_ContentPlaceHolder1_txtName").val() == '') {
            errName.html('Please select Name');
            return false;
        }
        else {
            errName.html("");
        }

        
        var errModule = $("#lblerrmsgModule");
        if ($("#ctl00_ContentPlaceHolder1_txtModule").val() == '') {
            errModule.html('Please select Module');
            return false;
        }
        else {
            errModule.html("");
            $('[id$="hdnModuleName"]').val($("#ctl00_ContentPlaceHolder1_txtModule").data("kendoDropDownList").text());
            //$('[id$=hdnModuleName]').val(data.ModuleName);
        }

        var errSdate = $("#lblerrmsgstartDate");
        if ($("#ctl00_ContentPlaceHolder1_txtSDate").val() == '') {
            errSdate.html('Please select Start Date');
            return false;
        }
        else {
            errSdate.html("");
        }

        var errEdate = $("#lblerrmsgendDate");
        if ($("#ctl00_ContentPlaceHolder1_txtEDate").val() == '') {
            errEdate.html('Please select End Date');
            return false;
        }
        else {
            errEdate.html("");
        }





        var errStartDate = $("#ctl00_ContentPlaceHolder1_txtSDate").val();
        var errEndDate = $("#ctl00_ContentPlaceHolder1_txtEDate").val();
        var EmpID = $("#ctl00_ContentPlaceHolder1_txtName").val();
        var errorDate = $("#lblerrormsgDate");



        if (new Date(errStartDate) > new Date(errEndDate)) {
            errorDate.html("Selected DateTime is Invalid");
            return false;

        }
        else {
            errorDate.html("");

        }

        var StartDate = $('[id$=txtSDate]').val();
        var EndDate = $('[id$=txtEDate]').val();

        var CheckEndDate = new Date(EndDate.substr(0, 11));
        var CheckStartDate = new Date(StartDate.substr(0, 11));

        var TotalTimes = CheckEndDate.getTime() - CheckStartDate.getTime();
        var start_actual_time = StartDate;
        var end_actual_time = EndDate;

        start_actual_time = new Date(start_actual_time);
        end_actual_time = new Date(end_actual_time);

        var diff = end_actual_time - start_actual_time;

        var diffSeconds = diff / 1000;
        var HH = Math.floor(diffSeconds / 3600);
        var MM = Math.floor(diffSeconds % 3600) / 60;

        if (parseInt(HH) > 8) {
            alert("You can not fill timesheet for more than eight hours at a time.");
            return false;
        }
        else if (isNaN(TotalTimes)) {
            var TotalDays = 0;
            alert("Your start date and end date is not correct.");
            return false;
        }
        else {
            if (typeof TotalTimes === 'undefined' || TotalTimes === null) {
                alert("Difference between start date and end date should not be more than 1 day.");
                return false;
            }
            else {
                var TotalDays = Math.round(Math.abs(TotalTimes / (1000 * 60 * 60 * 24)));
                if (TotalDays > 1) {
                    alert("Difference between start date and end date should not be more than 1 day.");
                    return false;
                }
            }
        }
        if (checkupdate == 0) {
            var checkadmin = $('[id$="hdnAdmin"]').val();
            if (checkadmin === "true") {
                EmpID = $("#ctl00_ContentPlaceHolder1_txtName").val();
            }
            else {
                EmpID = $('[id$="hdnEmpId"]').val();
            }
            var c = 0;
            $.ajax({
                type: "POST",
                url: "ProjectWBS.aspx/checkWBSExists",
                contentType: "application/json;charset=utf-8",
                data: "{'StartDate':'" + errStartDate + "','EndDate':'" + errEndDate + "','EmpID':'" + EmpID + "'}",
                // data: "{'StartDate':'Dec 21 2016  8:30AM','EndDate':'Dec 21 2016  7:00PM','EmpID':'1179'}",
                dataType: "json",
                async: false,
                cache: false,
                //   timeout: 30000,
                success: function (msg) {
                    if (msg.d == "true") {

                        c = 1;
                    }
                    else {
                        c = 0;
                    }

                },
                error: function (x, e) {

                }
            });

            if (c == 1) {
                alert("Timeshee is already exists for above time span");
                return false;
            }
        }

        var check = $('[id$="hdnCheckExists"]').val();
        if (check == 1) {
            alert("Timeshee is already exists for above time span");
            return false;
        }
    }
    return true;
    // }
}




//------------------------------------------------- Saved Milestone Grid -----------------------------------------------------------------//
function GetMilestoneDetails() {

    $.ajax({
        type: "POST",
        url: "ProjectWBS.aspx/GetData",
        contentType: "application/json;charset=utf-8",

        data: "{}",
        dataType: "json",
        async: true,
        success: function (msg) {
            GetMilestoneData(jQuery.parseJSON(msg.d));

        },
        error: function (x, e) {
            //alert("Error occured while fetching saved milestone.  "
            //      + x.responseText);
        }
    });


}

function GetMilestoneData(MilestoneData) {

    $(gridProjectMilestone).kendoGrid({
        dataSource: {
            data: MilestoneData,
            schema: {
                model: {
                    fields:
                        {
                            projID: { type: "number" },
                            projMilestoneID: { type: "number" },
                            name: { type: "string" },
                            dueDate: { type: "string" },
                            DeliveryDate: { type: "string" },
                            EstHours: { type: "number" },
                            MilestoneHours: { type: "string" },
                            ActualHrs: { type: "string" }
                        }

                }
            },
            pageSize: 10,
        },
        height: 250,
        groupable: true,

        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
                    { field: "projID", title: "projID", width: "50px", hidden: true },
                    { field: "projMilestoneID", title: "projMilestoneID", width: "50px", hidden: true },
                    { field: "name", title: "Name", width: "50px" },
                    { field: "dueDate", title: "Due Date", width: "50px" },
                    { field: "DeliveryDate", title: "Delivery Date", width: "50px" },
                    { field: "EstHours", title: "Budgeted Hours", width: "50px" },
                    { field: "MilestoneHours", title: "Planned Hours", width: "50px" },
                    { field: "ActualHrs", title: "Actual Hours", width: "50px" }
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
        }
        ,


        cancel: function (e) {
            e.preventDefault()

            ClosingRateWindow(e);
        }


    });

}

//------------------------------------------------- Project WBS Grid -----------------------------------------------------------------//

function GetProjectWBSData(showCompletedStatus) {

    $.ajax({
        type: "POST",
        url: "ProjectWBS.aspx/GetProjectWBS",
        contentType: "application/json;charset=utf-8",
        data: "{'showCompletedStatus':'" + showCompletedStatus + "'}",
        dataType: "json",
        // async: false,
        success: function (msg) {
            GetProjectWBS(jQuery.parseJSON(msg.d));


        },
        error: function (x, e) {
            //alert("Error occured while fetching saved milestone.  "
            //      + x.responseText);
        }
    });
}

function GetProjectWBS(ProjectWBSData) {
    $('#gridProject').kendoGrid({
        dataSource: {
            data: ProjectWBSData,
            schema: {
                model: {
                    fields:
                        {

                            projMilestoneID: { type: "number" },
                            Milestone: { type: "string" },
                            WBSID: { type: "number" },
                            WBS: { type: "string" },
                            StartDate: { type: "datetime", format: "{0:dd MMM yyyy}" },
                            EndDate: { type: "datetime", format: "{0:dd MMM yyyy}" },
                            Hours: { type: "string" },
                            AssignedTo: { type: "string" },
                            ActualHrs: { type: "string" },
                            ActualStartDate: { type: "string" },
                            ActualEndDate: { type: "string" },
                            Status: { type: "string" },
                            Remark: { type: "string" },
                            empId: { type: "string" }

                        }

                }
            },
            pageSize: 50,
        },
        height: 550,
        groupable: true,

        sortable: true,

        pageable: {
            input: true,
            numeric: false
        },
        columns: [
            {
                command: [
                            {
                                name: "edit", click: EditProject
                            },
                ],
                width: "45px", attributes: { style: "text-align:center;" }
            },
                    { field: "projMilestoneID", title: "projMilestoneID", width: 50, hidden: true },
                    { field: "Milestone", title: "Milestone", width: 50 },
                    { field: "WBSID", title: "WBSID", width: 50, hidden: true },
                    { field: "WBS", title: "WBS", width: 50 },
                    { field: "StartDate", title: "Start Date", width: 50, template: "#= kendo.toString(kendo.parseDate(StartDate, 'yyyy-MM-dd  hh:mm'), 'dd MMM yyyy  hh:mm tt') #" }, //:ss  :ss
                    { field: "EndDate", title: "End Date", width: 50, template: "#= kendo.toString(kendo.parseDate(EndDate, 'yyyy-MM-dd  hh:mm'), 'dd MMM yyyy  hh:mm tt') #" },
                    { field: "Hours", title: "Hours", width: 50 },
                    { field: "AssignedTo", title: "Assigned To", width: 50 },
                    { field: "ActualStartDate", title: "Actual Start Date", width: 50 },
                    { field: "ActualEndDate", title: "Actual End Date", width: 50 },
                    { field: "ActualHrs", title: "Actual Hours", width: 50 },
                    { field: "Status", title: "Status", width: 50 },
                    { field: "Remark", title: "Remark", width: 50, hidden: true },
                    { field: "empId", title: "EmpId", width: 50, hidden: true }


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
        }
         ,
        editable: false,

        filterMenuInit: function (e) {
            if (e.field == "name") {
                var firstValueDropDown = e.container.find("select:eq(0)").data("kendoDropDownList");
                firstValueDropDown.value("contains");
                var logicDropDown = e.container.find("select:eq(1)").data("kendoDropDownList");
                logicDropDown.value("or");
                var secondValueDropDown = e.container.find("select:eq(2)").data("kendoDropDownList");
                secondValueDropDown.value("contains");
            }
        },
        cancel: function (e) {
            e.preventDefault()

            ClosingRateWindow(e);
        },
    });
}




function hideButton() {
    var check = $('[id$="hdnAdmin"]').val();
    if (check === "true") {
        $(".k-grid-edit", "#gridProject").show();

    }
    else {
        $(".k-grid-edit", "#gridProject").hide();

    }

}

function FormatDateTime(date) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
   "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    var sdate = new Date(parseInt(date.substr(6)));

    var currdate = sdate.getDate();
    currdate = currdate < 10 ? '0' + currdate : currdate;

    var currMonth = sdate.getMonth();

    var curryear = sdate.getFullYear();
    var currhours = sdate.getHours();
    var ampm = currhours >= 12 ? 'PM' : 'AM';
    currhours = currhours < 10 ? '0' + currhours : currhours;
    var currMin = sdate.getMinutes();
    currMin = currMin < 10 ? '0' + currMin : currMin;

    //if (ampm == "PM" && currhours > 12) {
    //    currhours = currhours - 12;
    //}

    var Fdate = sdateTime = currdate + " " + monthNames[currMonth] + " " + curryear + " " + currhours + ":" + currMin + " " + ampm;
    return Fdate;
}

function FillMilestoneitems(counter) {
    $.ajax(
          {
              type: "POST",
              url: "ProjectWBS.aspx/FillMilestoneList",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              dataType: "json",
              async: true,
              success: function (msg) {

                  if (counter == 0) {
                      counter = "";
                  }

                  $('[id$="txtMilestone' + counter + '"]').kendoDropDownList({
                      optionLabel: "Select Milestone",
                      dataTextField: "Milestone",
                      dataValueField: "projMilestoneID",
                      change: OnMilestoneChange,
                      dataSource: jQuery.parseJSON(msg.d)
                  }).data("kendoDropDownList");

                  var projstrtDate = new Date();
                  projstrtDate.setHours(10, 0, 0);


                  $('[id$="txtWBSSDate' + counter + '"]').kendoDateTimePicker({ format: "dd MMM yyyy hh:mm tt", change: kendostartdate });

                  $('[id$="txtWBSSDate' + counter + '"]').data("kendoDateTimePicker").value(projstrtDate);

                  var projendDate = new Date();
                  projendDate.setHours(19, 0, 0);
                  $('[id$="txtWBSEDate' + counter + '"]').kendoDateTimePicker({ format: "dd MMM yyyy hh:mm tt", change: kendoEnddate });
                  $('[id$="txtWBSEDate' + counter + '"]').data("kendoDateTimePicker").value(projendDate);

                  FillEmployeeMultiselect(counter);


              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
    );
}

function FillEmployeeMultiselect(counter) {
    $.ajax(
          {
              type: "POST",
              url: "ProjectWBS.aspx/FillEmployeeMultiselect",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              dataType: "json",
              async: true,
              success: function (msg) {

                  if (counter == 0) {
                      counter = "";
                  }
                  $('[id$="txtassignto' + counter + '"]').kendoMultiSelect({
                      optionLabel: "Select Employee",
                      dataTextField: "empName",
                      dataValueField: "empid",
                      change: OnEmployeeChange,
                      dataSource: jQuery.parseJSON(msg.d)
                  }).data("kendoMultiSelect");
                  FillStatusitems(counter);
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
    );
}

function FillStatusitems(counter) {
    $.ajax(
          {
              type: "POST",
              url: "ProjectWBS.aspx/FillStatusList",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              dataType: "json",
              async: true,
              success: function (msg) {
                  $('[id$="hdnCount"]').val(counter);

                  if (counter == 0) {
                      counter = "";
                  }

                  $('[id$="txtStatus' + counter + '"]').kendoDropDownList({
                      optionLabel: "Select Status",
                      dataTextField: "Text",
                      dataValueField: "Value",
                      dataSource: jQuery.parseJSON(msg.d)
                  }).data("kendoDropDownList");

              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
    );
}

function convert(str) {

    var monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"];

    var newDate = new Date(str);
    var formattedDate = (newDate.getDate() < 10 ? '0' : '') + newDate.getDate() + '-' + monthNames[newDate.getMonth()] + '-' + newDate.getFullYear();

    return formattedDate;
}

function getCurrentDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }

    today = dd + '/' + mm + '/' + yyyy;
    return today;
}

function OnMilestoneChange() {
    $('[id$="hdprojectMileId"]').val($("#ctl00_ContentPlaceHolder1_txtMilestone").val());
}


function OnEmployeeChange() {
    var str = "";
    $("#ctl00_ContentPlaceHolder1_txtassignto option:selected").each(function () {
        str += $(this).val() + ",";
    });
    $('[id$="hdAssignTo"]').val(str);
}

function ShowAddPopup() {
    if ($('[id$="hdProjId"]').val() == '0') {
        $('[id$="dvall"]').show();
        $('[id$="dvbg"]').show();
    }
    else {
        openAddPopUP();
    }
}

function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}

function openEditPopUP() {
    $('#divAddPopupWBS').css('display', '');
    $('#divAddPopupOverlayWBS').addClass('k-overlay');


}

function kendostartdate() {
    var SDate = $("#ctl00_ContentPlaceHolder1_txtWBSSDate").val();
    var EDate = $("#ctl00_ContentPlaceHolder1_txtWBSEDate").val();
    if (!EDate == "") {
        if (new Date(SDate) > new Date(EDate)) {

        }
    }
}

function kendoEnddate() {
    var SDate = kendo.toString($("#ctl00_ContentPlaceHolder1_txtWBSSDate").data("kendoDateTimePicker").value(), 'dd MMM yyyy hh:mm tt');
    var EDate = kendo.toString($("#ctl00_ContentPlaceHolder1_txtWBSEDate").data("kendoDateTimePicker").value(), 'dd MMM yyyy hh:mm tt');
    var stime = kendo.toString($("#ctl00_ContentPlaceHolder1_txtWBSSDate").data("kendoDateTimePicker").value(), 'HH:mm');
    var Etime = kendo.toString($("#ctl00_ContentPlaceHolder1_txtWBSEDate").data("kendoDateTimePicker").value(), 'HH:mm');
    if (SDate === "" || SDate === null) {
        SDate = $('[id$="hdStartTime"]').val();
    }

    if (SDate != "" && EDate != "" && Etime != "00:00:00") {
        if (new Date(SDate) > new Date(EDate)) {

        }
        else {
            $('[id$="hdStartTime"]').val(SDate);
            $('[id$="hdEndTime"]').val(EDate);
        }
    }

}

function closeEditPopUP() {

    $('#divAddPopupWBS').css('display', 'none');
    $('#divAddPopupOverlayWBS').removeClass("k-overlay").addClass("k-overlayDisplaynone");

    window.location.assign("ProjectWBS.aspx");
}

function clearFields() {

    var c = $('[id$="hdnCount"]').val();

    var strtDate = new Date();
    strtDate.setHours(10, 0, 0);

    var endDate = new Date();
    endDate.setHours(19, 0, 0);
    for (i = 0; i <= c; i++) {

        if (i == 0) {
            $("#ctl00_ContentPlaceHolder1_txtMilestone").data("kendoDropDownList").value('');

            $('[id$="hdprojectWBSID"]').val('');
            $("#ctl00_ContentPlaceHolder1_txteditWBS").val('');
            $("#ctl00_ContentPlaceHolder1_txtWBSSDate").data("kendoDateTimePicker").value(strtDate);
            $("#ctl00_ContentPlaceHolder1_txtWBSEDate").data("kendoDateTimePicker").value(endDate);
            $("#ctl00_ContentPlaceHolder1_txtStatus").data("kendoDropDownList").value('');

            $("#ctl00_ContentPlaceHolder1_txtremark").val('');

            $('[id$="ctl00_ContentPlaceHolder1_txtassignto"]').val('');
            $('[id$="hdAssignTo"]').val('');

            $("#ctl00_ContentPlaceHolder1_txtassignto").data("kendoMultiSelect").value([]);

            $("#lblerrmsgMilestone").empty();
            $("#lblerrmsgWBS").empty();
            $("#lblerrmsgSDate").empty();
            $("#lblerrmsgEDate").empty();
            $("#lblerrmsgEmployeeSelect").empty();
            $("#lblerrmsgDate").empty();
            $("#lblerrmsgStatus").empty();
        }
        else {

            $('[id$="txtMilestone' + i + '"]').data("kendoDropDownList").value('');

            $('[id$="hdprojectWBSID"]').val('');
            $('[id$="txteditWBS' + i + '"]').val('');



            $('[id$="txtWBSSDate' + i + '"]').data("kendoDateTimePicker").value(strtDate);

            $('[id$="txtWBSEDate' + i + '"]').data("kendoDateTimePicker").value(endDate);
            $('[id$="txtStatus' + i + '"]').data("kendoDropDownList").value('');

            $('[id$="txtremark' + i + '"]').val('');

            $('[id$="txtassignto' + i + '"]').val('');
            $('[id$="hdAssignTo"]').val('');

            $('[id$="txtassignto' + i + '"]').data("kendoMultiSelect").value([]);

            $("#lblerrmsgMilestone" + i + "").empty();
            $("#lblerrmsgWBS" + i + "").empty();
            $("#lblerrmsgSDate" + i + "").empty();
            $("#lblerrmsgEDate" + i + "").empty();
            $("#lblerrmsgEmployeeSelect" + i + "").empty();
            $("#lblerrmsgDate" + i + "").empty();
            $("#lblerrmsgStatus" + i + "").empty();
            //}
        }
    }

    //EDIT PROJECT WBS

    $("#ctl00_ContentPlaceHolder1_txtEditMilestone").data("kendoDropDownList").value('');


    $("#ctl00_ContentPlaceHolder1_txtEditeditWBS").val('');

    $("#ctl00_ContentPlaceHolder1_txtEditWBSSDate").val('');

    $("#ctl00_ContentPlaceHolder1_txtEditWBSEDate").val('');
    $("#ctl00_ContentPlaceHolder1_txtEditStatus").data("kendoDropDownList").value('');

    $("#ctl00_ContentPlaceHolder1_txtEditremark").val('');

    $('[id$="ctl00_ContentPlaceHolder1_txtEditassignto"]').val('');
    $("#ctl00_ContentPlaceHolder1_txtEditassignto").data("kendoMultiSelect").value([]);

    $("#lblEditerrmsgMilestone").empty();
    $("#lblEditerrmsgWBS").empty();
    $("#lblEditerrmsgSDate").empty();
    $("#lblEditerrmsgEDate").empty();
    $("#lblEditerrmsgEmployeeSelect").empty();
    $("#lblEditerrmsgDate").empty();
}

function removeRow() {
    var count = $('[id$="hdnCount"]').val();
    $("#testtab").delegate(".buttondel", "click", function () {
        var btnid = this.id.slice(-1);
        var test = $('[id$="hdnTab"]').val();
        $('[id$="hdnTab"]').val(test + "," + btnid);
        $(this).closest("tr").remove();


    });

}


function CheckSave() {



    var value = true;
    var deletedrows = [];
    var result = "";


    $('[id$="hdntest"]').val('');
    deletedrows = $('[id$="hdnTab"]').val().split(',').sort();


    for (k = 0; k < deletedrows.length; k++) {
        var te = deletedrows[k];

        if (te != $('[id$="hdntest"]').val()) {
            $('[id$="hdntest"]').val(te);

            result += deletedrows[k] + ",";
        }
    }
    $('[id$="hdnarray"]').val(result);



    // $('#btn_asp_save').prop("disabled", true);


    var count = $('[id$="hdnCount"]').val();

    if ($('#divAddPopupWBS').is(':visible')) {
        for (i = 0; i <= count; i++) {

            if (i == 0) {
                var errMilestone = $("#lblerrmsgMilestone");
                if ($("#ctl00_ContentPlaceHolder1_txtMilestone").val() == '') {
                    errMilestone.html('Please select Milestone');
                    value = false;
                    return value;
                }
                else {
                    errMilestone.html("");
                }
                var errWBS = $("#lblerrmsgWBS");
                if ($("#ctl00_ContentPlaceHolder1_txteditWBS").val() == '') {
                    errWBS.html('Please enter WBS details');
                    value = false;
                    return value;
                }
                else {
                    errWBS.html("");
                }
                var errSDate = $("#lblerrmsgSDate");
                if ($("#ctl00_ContentPlaceHolder1_txtWBSSDate").val() == '') {
                    errSDate.html('Please select Start Date');
                    value = false;
                    return value;
                }
                else {
                    errSDate.html("");
                }
                var errEDate = $("#lblerrmsgEDate");
                if ($("#ctl00_ContentPlaceHolder1_txtWBSEDate").val() == '') {
                    errEDate.html('Please select End Date');
                    value = false;
                    return value;
                }
                else {
                    errEDate.html("");
                }

                var WBSSDate = $("#ctl00_ContentPlaceHolder1_txtWBSSDate").val();
                var WBSEDate = $("#ctl00_ContentPlaceHolder1_txtWBSEDate").val();
                var errDate = $("#lblerrmsgDate");
                if (new Date(WBSSDate) > new Date(WBSEDate)) {
                    errDate.html("Selected DateTime is Invalid");
                    value = false;
                    return value;
                }
                else {
                    errDate.html("");
                }


                var errEmp = $("#lblerrmsgEmployeeSelect");
                var str = "";
                $('[id$=txtassignto] option:selected').each(function () {

                    str += $(this).val() + ",";
                });

                if (str == '') {
                    errEmp.html('Please select development team');
                    value = false;
                    return value;
                }
                else {
                    errEmp.html("");
                }
                var errstatus = $("#lblerrmsgStatus");
                if ($('[id$="ctl00_ContentPlaceHolder1_txtStatus"]').val() == '') {
                    errstatus.html('Please select status');
                    value = false;
                    return value;
                }
                else {
                    errstatus.html("");
                }

                var errstatus = $("#lblerrmsgtxteditPlannedHours");
                if ($('[id$="ctl00_ContentPlaceHolder1_txteditPlannedHours"]').val() == '') {
                    errstatus.html('Please Enter Hours');
                    value = false;
                    return value;
                }
                else {
                    errstatus.html("");
                }


            }

            else {

                if (result.includes(i)) {

                }
                else {
                    var errMilestone = $('[id$=lblerrmsgMilestone' + i + ']');
                    if ($('[id$=txtMilestone' + i + ']').val() == '') {
                        errMilestone.html('Please select Milestone');
                        value = false;
                        return value;
                    }
                    else {
                        errMilestone.html("");
                    }
                    var errWBS = $('[id$=lblerrmsgWBS' + i + ']');
                    if ($('[id$=txteditWBS' + i + ']').val() == '') {
                        errWBS.html('Please enter WBS details');
                        value = false;
                        return value;
                    }
                    else {
                        errWBS.html("");
                    }

                    var errSDate = $('[id$=lblerrmsgSDate' + i + ']');
                    if ($('[id$=txtWBSSDate' + i + ']').val() == '') {
                        errSDate.html('Please select Start Date');
                        value = false;
                        return value;
                    }
                    else {
                        errSDate.html("");
                    }
                    var errEDate = $('[id$=lblerrmsgEDate' + i + ']');
                    if ($('[id$=txtWBSEDate' + i + ']').val() == '') {
                        errEDate.html('Please select End Date');
                        value = false;
                        return value;
                    }
                    else {
                        errEDate.html("");
                    }


                    var WBSSDate = $('[id$=txtWBSSDate' + i + ']').val();
                    var WBSEDate = $('[id$=txtWBSEDate' + i + ']').val();
                    var errDate = $('[id$=lblerrmsgDate]');
                    if (new Date(WBSSDate) > new Date(WBSEDate)) {
                        errDate.html("Selected DateTime is Invalid");
                        value = false;
                        return value;
                    }
                    else {
                        errDate.html("");
                    }

                    var errEmp = $('[id$=lblerrmsgEmployeeSelect' + i + ']');

                    var str = "";
                    $('[id$=txtassignto' + i + '] option:selected').each(function () {

                        str += $(this).val() + ",";
                    });

                    if (str == '') {
                        errEmp.html('Please select development team');
                        value = false;
                        return value;
                    }
                    else {
                        errEmp.html("");
                    }

                    var errstatus = $('[id$=lblerrmsgStatus' + i + ']');
                    if ($('[id$=txtStatus' + i + ']').val() == '') {
                        errstatus.html('Please select status');
                        value = false;
                        return value;
                    }
                    else {
                        errstatus.html("");
                    }


                    var errstatus = $('[id$=lblerrmsgtxteditPlannedHours' + i + ']');
                    if ($('[id$=txteditPlannedHours' + i + ']').val() == '') {
                        errstatus.html('Please Enter Hours');
                        value = false;
                        return value;
                    }
                    else {
                        errstatus.html("");
                    }
                }
            }
        }
        return value;
    }
}


function SaveWBS() {



    var str = "";
    var result = "";
    var count = $('[id$="hdnCount"]').val();

    var validation = CheckSave();

    if (validation == true) {

        $('[id$="btn_asp_save"]').prop("disabled", true);

        for (i = 0; i <= count; i++) {
            if (i == 0) {
                $("#ctl00_ContentPlaceHolder1_txtassignto option:selected").each(function () {
                    str += $(this).val() + ",";
                });

                var txtMile = $('[id$=txtMilestone]').val();
                var txtWBS = $('[id$=txteditWBS]').val();
                var txtStartDate = $('[id$=txtWBSSDate]').val();
                var txtEndDate = $('[id$=txtWBSEDate]').val();
                var txtAssignto = str;
                var txtStatus = $('[id$=txtStatus]').val();
                var txtRemark = $('[id$=txtremark]').val();
                //  var plannedhour = $('[id$="txteditPlannedHours"]').val();
                var hours = $('[id$=txteditPlannedHours]').val();
                // var min = $('[id$=dropDownMin]').val();
                var min = $("#dropDownMin option:selected").text();

                SaveProjWBS(txtMile, txtWBS, txtStartDate, txtEndDate, txtStatus, txtRemark, txtAssignto, hours, min);
                result = "success";
            }
            else {
                var array = $('[id$="hdnarray"]').val();

                if (array.includes(i)) {

                }
                else {
                    str = "";
                    var txtMile = $('[id$=txtMilestone' + i + ']').val();
                    var txtWBS = $('[id$=txteditWBS' + i + ']').val();
                    var txtStartDate = $('[id$=txtWBSSDate' + i + ']').val();
                    var txtEndDate = $('[id$=txtWBSEDate' + i + ']').val();

                    $('[id$=txtassignto' + i + '] option:selected').each(function () {
                        str += $(this).val() + ",";
                    });

                    var txtAssignto = str;
                    var txtStatus = $('[id$=txtStatus' + i + ']').val();
                    var txtRemark = $('[id$=txtremark' + i + ']').val();

                    var hours = $('[id$=txteditPlannedHours' + i + ']').val();
                    // var min = $('[id$=dropDownMin]').val();
                    var min = $("#dropDownMin" + i + " option:selected").text();

                    SaveProjWBS(txtMile, txtWBS, txtStartDate, txtEndDate, txtStatus, txtRemark, txtAssignto, hours, min);
                    result = "success";
                }
            }
        }
    }

    if (result == "success") {
        window.location.assign("ProjectWBS.aspx");

    }



}

function ShowPlannedHours(txtStartDate, txtEndDate, id, editid) {

    $.ajax({
        type: "POST",
        url: "ProjectWBS.aspx/CalculateHours",
        data: "{'StartDate':'" + txtStartDate + "','EndDate':'" + txtEndDate + "'}",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        async: true,
        success: function (data) {
            var hoursAndMin = data.d;
            var hoursAndMinArray = hoursAndMin.split('$');
            var hours = hoursAndMinArray[0];
            var mins = hoursAndMinArray[1];

            if (id == 0) {
                if (editid == 0) {
                    $('[id$=txteditPlannedHours]').val(hours);
                    if (parseInt(mins) == 0 || parseInt(mins) < 30) {
                        $('[id$="dropDownMin"]').val(0);
                    }
                    else {
                        $('[id$="dropDownMin"]').val(1);
                    }
                }
                else {

                    $('[id$=txtPlannedHours]').val(hours);
                    if (parseInt(mins) == 0 || parseInt(mins) < 30) {
                        $('[id$="dropEditDownMin"]').val(0);
                    }
                    else {
                        $('[id$="dropEditDownMin"]').val(1);
                    }
                }

            }
            else {

                $('[id$=txteditPlannedHours' + id + ']').val(hours);
                if (parseInt(mins) == 0 || parseInt(mins) < 30) {
                    $('[id$=dropDownMin' + id + ']').val(0);
                }
                else {
                    $('[id$=dropDownMin' + id + ']').val(1);
                }
            }

        },
        error: function (x, e) {
            alert("Error occured while fetching saved WBS. "
              + x.responseText);
        }
    });
}

function SaveProjWBS(txtMile1, txtWBS1, txtStartDate1, txtEndDate1, txtStatus1, txtRemark1, txtAssignto1, hours, min) {

    $.ajax({
        type: "POST",
        url: "ProjectWBS.aspx/SaveWBS",
        data: "{'ProjMileId':'" + txtMile1 + "','WBS':'" + txtWBS1 + "','StartDate':'" + txtStartDate1 + "','EndDate':'" + txtEndDate1 + "','Status':'" + txtStatus1 + "','Remark':'" + txtRemark1 + "','Assignto':'" + txtAssignto1 + "','hours':'" + hours + "', 'min':'" + min + "'}",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        async: true,
        success: function (data) {
            //$('[id$=hdAssignTo]').val("");
            //alert('ProjectWBS Saved Successfully');
        },
        error: function (x, e) {
            //alert("Error occured while fetching saved WBS. "
            //   + x.responseText);
        }
    });

}


// For Editing WBS

function openPopUP() {

    $('#divEditPopupWBS').css('display', '');
    $('#divEditPopupOverlayWBS').addClass('k-overlay');
}

function closePopUP() {

    $('#divEditPopupWBS').css('display', 'none');
    $('#divEditPopupOverlayWBS').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    // 
    clearFields();

}

function BindMilestone() {
    $.ajax(
          {
              type: "POST",
              url: "ProjectWBS.aspx/FillMilestoneList",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              dataType: "json",
              async: true,
              success: function (msg) {



                  $('[id$="txtEditMilestone"]').kendoDropDownList({
                      optionLabel: "Select Milestone",
                      dataTextField: "Milestone",
                      dataValueField: "projMilestoneID",
                      change: OnEditMilestoneChange,
                      dataSource: jQuery.parseJSON(msg.d)
                  }).data("kendoDropDownList");

                  $('[id$="txtEditWBSSDate"]').kendoDateTimePicker({ format: "dd MMM yyyy hh:mm tt", change: kendoEditstartdate });
                  $('[id$="txtEditWBSEDate"]').kendoDateTimePicker({ format: "dd MMM yyyy hh:mm tt", change: kendoEditEnddate });

                  BindEmployeeMultiselect();


              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
    );
}

function kendoEditstartdate() {

    var SDate = $("#ctl00_ContentPlaceHolder1_txtEditWBSSDate").val();
    $('[id$="hdStartTime"]').val(SDate);
    var EDate = $("#ctl00_ContentPlaceHolder1_txtEditWBSEDate").val();
    if (!EDate == "") {
        if (new Date(SDate) > new Date(EDate)) {

        }
    }
}

function kendoEditEnddate() {

    var SDate = kendo.toString($("#ctl00_ContentPlaceHolder1_txtEditWBSSDate").data("kendoDateTimePicker").value(), 'dd MMM yyyy hh:mm tt');
    var EDate = kendo.toString($("#ctl00_ContentPlaceHolder1_txtEditWBSEDate").data("kendoDateTimePicker").value(), 'dd MMM yyyy hh:mm tt');
    var stime = kendo.toString($("#ctl00_ContentPlaceHolder1_txtEditWBSSDate").data("kendoDateTimePicker").value(), 'HH:mm');
    var Etime = kendo.toString($("#ctl00_ContentPlaceHolder1_txtEditWBSEDate").data("kendoDateTimePicker").value(), 'HH:mm');
    if (SDate === "" || SDate === null) {
        SDate = $('[id$="hdStartTime"]').val();
    }

    if (SDate != "" && EDate != "" && Etime != "00:00:00") {
        if (new Date(SDate) > new Date(EDate)) {

        }
        else {
            $('[id$="hdStartTime"]').val(SDate);
            $('[id$="hdEndTime"]').val(EDate);
        }
    }

}

function BindEmployeeMultiselect() {
    $.ajax(
          {
              type: "POST",
              url: "ProjectWBS.aspx/FillEmployeeMultiselect",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              dataType: "json",
              async: true,
              success: function (msg) {


                  $('[id$="txtEditassignto"]').kendoMultiSelect({
                      optionLabel: "Select Employee",
                      dataTextField: "empName",
                      dataValueField: "empid",
                      change: OnEditEmployeeChange,
                      dataSource: jQuery.parseJSON(msg.d)
                  }).data("kendoMultiSelect");

                  BindStatus();
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
    );
}

function BindStatus() {
    $.ajax(
          {
              type: "POST",
              url: "ProjectWBS.aspx/FillStatusList",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              dataType: "json",
              async: true,
              success: function (msg) {
                  $('[id$="txtEditStatus"]').kendoDropDownList({
                      optionLabel: "Select Status",
                      dataTextField: "Text",
                      dataValueField: "Value",
                      dataSource: jQuery.parseJSON(msg.d)
                  }).data("kendoDropDownList");

              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
    );
}

function EditProject(e) {

    
   // var tr = $(e.target).closest("tr");
   // var data = this.dataItem(tr);

    var data = $("#gridProject").data("kendoGrid").dataItem(e.target.closest("tr"));

    openPopUP();

    if (data.projMilestoneID != "") {
        $("#ctl00_ContentPlaceHolder1_txtEditMilestone").data("kendoDropDownList").value(data.projMilestoneID);
        $('[id$="hdprojectMileId"]').val(data.projMilestoneID);
    }
    var hoursandmin = data.Hours;
    var hoursAndMinArray = hoursandmin.split(':');

    var hours = hoursAndMinArray[0];
    var mins = hoursAndMinArray[1];

    var projectWBSID = data.WBSID;
    $('[id$="hdprojectWBSID"]').val(projectWBSID);

    $('#ctl00_ContentPlaceHolder1_txtEditeditWBS').val(data.WBS);

    $('#ctl00_ContentPlaceHolder1_txtEditWBSSDate').val(FormatDateTime(data.StartDate));
    $('[id$="hdStartTime"]').val(FormatDateTime(data.StartDate));
    $('#ctl00_ContentPlaceHolder1_txtEditWBSEDate').val(FormatDateTime(data.EndDate));
    $('[id$="hdEndTime"]').val(FormatDateTime(data.EndDate));

    $('[id$="txtPlannedHours"]').val(hours);
    $('[id$="dropEditDownMin"]').val(mins);
    $("#hdnMin").val(mins);

    $('#ctl00_ContentPlaceHolder1_txtEditStatus').data("kendoDropDownList").value(data.Status);
    $('#ctl00_ContentPlaceHolder1_txtEditremark').val(data.Remark);

    var DevelopmentTeam = $("#ctl00_ContentPlaceHolder1_txtEditassignto").data("kendoMultiSelect");
    DevelopmentTeam.dataSource.filter({});
    DevelopmentTeam.value(data.empId.split(","));
    $('[id$="hdAssignTo"]').val(data.empId + ",");


}


function OnEditMilestoneChange() {
    $('[id$="hdprojectMileId"]').val($("#ctl00_ContentPlaceHolder1_txtEditMilestone").val());
}

function OnEditEmployeeChange() {
    var str = "";
    $("#ctl00_ContentPlaceHolder1_txtEditassignto option:selected").each(function () {
        str += $(this).val() + ",";
    });
    $('[id$="hdAssignTo"]').val(str);
}

//END

//------------------------------------------------------------WBS TIMESHEET -----------------------------------------------------------------------------------//

/*AP*/
function GetProjectWBSDetails(startDate, endDate) {

    var StartDate = startDate;
    var EndDate = endDate;
    $.ajax({
        type: "POST",
        url: "ProjectWBS.aspx/GetProjectWBSDetails",
        contentType: "application/json;charset=utf-8",
        data: "{'StartDate':'" + StartDate + "','EndDate':'" + EndDate + "'}",
        dataType: "json",
        async: true,
        success: function (msg) {

            var GetHoursArray = "";
            var GetTotalHours = "";

            $.each(msg.d, function (i) {
                GetHoursArray = msg.d[i].strHours;
                GetHoursArray = GetHoursArray.split(':');
                var Hours = GetHoursArray[0];
                var Minute = GetHoursArray[1];
                var minutesss = Hours * 60;
                //var minutesss = Minute % 60;

                var totalMinute = parseInt(Minute) + parseInt(minutesss);

                var hours = Math.trunc(totalMinute / 60);
                var minutes = totalMinute % 60;

                GetTotalHours = hours + ":" + minutes;
            })

            GetProjectWBSDetailsData(msg.d);

        },
        error: function (x, e) {
            //alert("Error occured while fetching saved Project WBS Details.  "
            //      + x.responseText);
        }
    });


}


function GetProjectWBSDetailsData(ProjectWBSDetails) {
    $('#gridProjectWBSDetail').kendoGrid({

        dataSource: {
            data: ProjectWBSDetails,
            schema: {
                model: {
                    fields:
                        {

                            WBS: { type: "string" },
                            Name: { type: "string" },
                            ModuleID: { type: "number" },
                            ModuleName: { type: "string" },
                            SDate: { type: "datetime", format: "{0:dd MMM yyyy}" },
                            EDate: { type: "datetime", format: "{0:dd MMM yyyy}" },
                            strHours: { type: "string" },
                            strMinutes: { type: "string" },
                            Comment: { type: "string" },
                            empId: { type: "string" },
                            ProjectWBSID: { type: "number" },
                            WBSId: { type: "number" },
                            Status: { type: "string" },

                        },
                }
            },
            //group: {
            //    field: "ModuleName", aggregates: [
            //       {
            //           field: "strHours", aggregate: "sum"
            //       },
            //    ]
            //},
            aggregates: [
                 { field: "strHours", aggregate: "sum" }
            ],

            pageSize: 50,
        },
        height: 550,
        groupable: true,

        sortable: true,

        pageable: {
            input: true,
            numeric: false
        },
        columns: [

                    { field: "WBS", title: "WBS", width: 50 },
                    { field: "Name", title: "Name", width: 50 },
                    { field: "ModuleID", title: "ModuleID", hidden: true },
                    {
                        field: "ModuleName", title: "Module Name",
                        width: 50
                    },
                    { field: "SDate", title: "Start Date", width: 50, template: "#= kendo.toString(kendo.parseDate(SDate, 'yyyy-MM-dd hh:mm'), 'dd MMM yyyy hh:mm tt') #" },
                    { field: "EDate", title: "End Date", width: 50, template: "#= kendo.toString(kendo.parseDate(EDate, 'yyyy-MM-dd hh:mm'), 'dd MMM yyyy hh:mm tt') #" },

                    {
                        field: "strMinutes", title: "Minutes", width: 50, hidden: true
                    },

                    {
                        field: "strHours", title: "Hours",
                        width: 50,
                        //format: "{H:mm}",
                        format: "{0:n2} Hrs",
                        aggregates: ["sum"],

                        //ClientFooterTemplate: "Total hours: #= GetHours(kendo.toString(sum, \"C\"))# Hrs",
                        groupFooterTemplate: "Total hours: #= GetHours(kendo.toString(sum, \"C\"))# Hrs",
                    },
                    { field: "Comment", title: "Comment", width: 50, hidden: true },
                    { field: "empId", title: "empId", hidden: true },
                    {
                        field: "ProjectWBSID", title: "ProjectWBSID", width: 50, hidden: true
                    },
                    { field: "WBSId", title: "WBSId", hidden: true },
                    { field: "Status", title: "WBSStatus", hidden: true },
                    {

                        command: [
                                    {
                                        name: "edit", click: EditProjWBSDetails
                                    },

                                    //Created by Ganesh Pawar : 17/11/2016
                                    {
                                        name: "delete", text: "Delete", imageClass: "k-icon k-i-close", click: DeleteTimesheet
                                    }
                        ],
                        title: "&nbsp;", width: "50px" //width: "40px", attributes: { style: "text-align:center;" },                                            
                    },
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
        }
         ,
        editable: false,

        dataBound: function () {

            

            var grid = this;
            var model;

            grid.tbody.find("tr[role='row']").each(function () {
                model = grid.dataItem(this);
                var IsAdmin = $('[id$="hdnAdmin"]').val();
                var IsProfileAdmin = $('[id$="hdnGetProfileAccess"]').val();
                if (IsAdmin != "true") {
                    $(this).find(".k-grid-edit").remove();
                    //$(this).find(".k-button k-button-icontext k-grid-Delete").remove();
                }
                if (IsProfileAdmin != "true") {
                    $(this).find(".k-grid-delete").remove();
                }
                else {
                    $(this).find(".k-grid-edit").show();
                    $(this).find(".k-grid-delete").show();
                }
            });
        },

        cancel: function (e) {
            e.preventDefault()

            ClosingRateWindow(e);
        },
    });
}

//TL,PL,Manager access for delete the timesheet.
function DeleteTimesheet(e) {
    
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var ProjectWBSTimeID = dataItem.ProjectWBSID;
    if (confirm("Are you sure you want to delete?")) {
        $('[id$="hdnProjWBSTimesheetId"]').val(ProjectWBSTimeID);
        $('[id$="btnDeleteWBSTimeId"]').click();
    }
    else { }
}


function GetHours(value) {

    

    value = value.substr(1) // Remove first characters from string
    //value = '0' + value;

    //value=09:00 --> break 10:34
    var AppendString = "";
    var Mycount = 5;
    var GetHours = "";
    var GetMinute = 0;
    var GetTotalMinute = 0;
    var GetTotalMinutes = 0;
    var SpliteTheString = 0;
    var Result = "";
    for (i = 0; i < value.length; i++) {
        if (i === Mycount) {
            AppendString += '$';
            Mycount = Mycount + 5;
            //break;
        }
        AppendString += value[i];
    }

    var GetString = AppendString;
    var bar = GetString.split("$");     // => ["09:00$10:34"]
    //var baz = bar.split(":")[i];  // => "123", the same as bar[1]

    $.each(bar, function (i) {
        GetHours = bar[i];
        SpliteTheString = GetHours.split(":");
        GetMinute = parseInt(SpliteTheString[0]) * parseInt(60);
        GetTotalMinute = parseInt(GetMinute) + parseInt(SpliteTheString[1]);
        GetTotalMinutes = parseInt(GetTotalMinutes) + parseInt(GetTotalMinute);
        var h = Math.floor(GetTotalMinutes / 60);
        var m = GetTotalMinutes % 60;
        h = h < 10 ? '0' + h : h;
        m = m < 10 ? '0' + m : m;
        Result = h + ':' + m;
    })

    //alert(Result);

    return Result;
}

function OpenPopUp(CheckEditValue) {

    $('[id$="hdnCheckExists"]').val('');
    var Moduleid = $('[id$="hdnModuleID"]').val()

    //alert("Moduleid"+$('[id$="hdnModuleID"]').val());

    // var moduleid=  $('[id$="hdnModuleID"]').val();

    //$(".k-datepicker input").bind('click dblclick', function () {
    //    return false;
    //});

    ////make it readonly
    //$(".k-datepicker input").prop("readonly", true);

    //$('#txtEDate').data('kendoDateTimePicker').enable(false);

    $(window).scrollTop(0);
    $('#ctl00_ContentPlaceHolder1_txtWBSName').css('display', 'none');

    var test = $('[id$="hdnAdmin"]').val();

    if (test === "true") {
        FillNameDropDown();
        var id = $('[id$="hdnEmpId"]').val();

        $("#ctl00_ContentPlaceHolder1_txtName").data("kendoDropDownList").value(id);

        BindAllWBS();

    }
    else {
        $('#ctl00_ContentPlaceHolder1_txtName').css('display', 'block');
        var EmpName = $('[id$="hdnEmpName"]').val();
        $('#ctl00_ContentPlaceHolder1_txtName').val(EmpName);
        $('#ctl00_ContentPlaceHolder1_txtName').attr('readonly', true);

        BindWBS();
    }
    FillModuleDropDown();
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');

    
    if ($('[id$="hdnModuleID"]').val() != "") {
        $("#ctl00_ContentPlaceHolder1_txtModule").data("kendoDropDownList").value(Moduleid);
    }


   
   // var a = $("#ct100_ContentPlaceHolder1_txtModule option:selected").text();
    var sDate = new Date();
    sDate.setHours(10, 0, 0);
    $('[id$="txtSDate"]').kendoDateTimePicker({ format: "dd MMM yyyy hh:mm tt", change: startdate }); // 
    //$('[id$="txtSDate"]').data("kendoDateTimePicker").value();
    $('[id$="txtSDate"]').data("kendoDateTimePicker").value(sDate);
    var eDate = new Date();
    eDate.setHours(19, 0, 0);
    $('[id$="txtEDate"]').kendoDateTimePicker({ format: "dd MMM yyyy hh:mm tt", change: enddate });
    //$('[id$="txtEDate"]').data("kendoDateTimePicker").value();
    $('[id$="txtEDate"]').data("kendoDateTimePicker").value(eDate);


    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
                    "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    var sdate = new Date();

    var currdate = sdate.getDate();
    currdate = currdate < 10 ? '0' + currdate : currdate;

    var currMonth = sdate.getMonth();

    var curryear = sdate.getFullYear();
    var Fdate = currdate + " " + monthNames[currMonth] + " " + curryear;
    var s = Fdate + " " + "10:00:00 AM";

    $('[id$="hdnSTime"]').val(s);
    var e = Fdate + " " + "19:00:00 PM";
    $('[id$="hdnETime"]').val(e);

    if (CheckEditValue == "Edit" || CheckEditValue != null) {
        //On Edit time date will not allow to select.
        var EndDate = $('[id$="txtEDate"]').data("kendoDateTimePicker");
        //EndDate.destroy();
        //$('[id$=hdnModuleName]').val(data.ModuleName);
    }



    
   // $('[id$="hdnSelectedEmpName"]').val($("#ctl00_ContentPlaceHolder1_txtName").data("kendoDropDownList").text());
   // $('[id$="hdnWBS"]').val($("#ctl00_ContentPlaceHolder1_txtWBS").data("kendoDropDownList").text());
    //$('[id$="hdnWBSName"]').val($("#ctl00_ContentPlaceHolder1_txtWBS").val());
    //if (typeof CheckEditValue === 'undefined' || CheckEditValue === null) {
    //    //On Edit time date will not allow to select.
    //    var EndDate = $('[id$="txtEDate"]').data("kendoDateTimePicker");
    //    EndDate.destroy();
    //}


}



function startdate() {

    var SDate = $("#ctl00_ContentPlaceHolder1_txtSDate").val();
    var EDate = $('[id$="hdnETime"]').val();

    SDate = SDate.replace(/-/g, " ");

    if (!EDate == "") {
        if (SDate > EDate) {
            //alert("Start date not should be greater than en date.");
            //return false;
        }
    }


    $('[id$="hdnSTime"]').val(SDate);
    $('[id$="hdnETime"]').val(EDate);

    //SDate = kendo.toString($('#ctl00_ContentPlaceHolder1_txtSDate').data("kendoDateTimePicker").value(), 'dd MMM yyyy hh:mm tt');
    //stime = kendo.toString($("#ctl00_ContentPlaceHolder1_txtSDate").data("kendoDateTimePicker").value(), 'HH:mm');
}

function enddate() {




    //var s = $('[id$="hdnSTime"]').val();
    //s.date
    //var t = $('[id$="hdnETime"]').val();
    //if (t) {

    //}

    var SDate = "";
    var stime = "";
    var EDate = "";
    var Etime = "";
    if (AssignStartDateValue == "" && AssignEndDateValue == "") {
        SDate = kendo.toString($("#ctl00_ContentPlaceHolder1_txtSDate").data("kendoDateTimePicker").value(), 'dd MMM yyyy hh:mm tt');
        stime = kendo.toString($("#ctl00_ContentPlaceHolder1_txtSDate").data("kendoDateTimePicker").value(), 'HH:mm');
        EDate = kendo.toString($("#ctl00_ContentPlaceHolder1_txtEDate").data("kendoDateTimePicker").value(), 'dd MMM yyyy hh:mm tt');
        Etime = kendo.toString($("#ctl00_ContentPlaceHolder1_txtEDate").data("kendoDateTimePicker").value(), 'HH:mm');
    }
    else {
        //SDate = kendo.toString($('#ctl00_ContentPlaceHolder1_txtSDate').data("kendoDateTimePicker").value(), 'dd MMM yyyy hh:mm tt');
        //stime = kendo.toString($("#ctl00_ContentPlaceHolder1_txtSDate").data("kendoDateTimePicker").value(), 'HH:mm');
        EDate = kendo.toString($("#ctl00_ContentPlaceHolder1_txtEDate").data("kendoDateTimePicker").value(), 'dd MMM yyyy hh:mm tt');
        Etime = kendo.toString($("#ctl00_ContentPlaceHolder1_txtEDate").data("kendoDateTimePicker").value(), 'HH:mm');
        //EDate = $('[id$="hdnETime"]').val();
        //EDate = kendo.toString($('#ctl00_ContentPlaceHolder1_txtEDate').data("kendoDateTimePicker").value(), 'dd MMM yyyy hh:mm tt');
        //Etime = kendo.toString($("#ctl00_ContentPlaceHolder1_txtEDate").data("kendoDateTimePicker").value(), 'HH:mm');
    }



    //var EDate = kendo.toString($("#ctl00_ContentPlaceHolder1_txtEDate").data("kendoDateTimePicker").value(), 'dd MMM yyyy hh:mm tt');
    //var stime = kendo.toString($("#ctl00_ContentPlaceHolder1_txtSDate").data("kendoDateTimePicker").value(), 'HH:mm');
    //var Etime = kendo.toString($("#ctl00_ContentPlaceHolder1_txtEDate").data("kendoDateTimePicker").value(), 'HH:mm');
    if (SDate === "" || SDate === null) {
        SDate = $('[id$="hdnSTime"]').val();
    }
    if (SDate != "" && EDate != "" && Etime != "00:00:00") {
        if (new Date(SDate) > new Date(EDate)) {

        }
        else {
            $('[id$="hdnSTime"]').val(SDate);
            $('[id$="hdnETime"]').val(EDate);
        }
    }
}

function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    GetProjectWBSDetails('', '');
    clear();

}
/*Bind Module*/

function FillModuleDropDown() {
    $.ajax(
          {
              type: "POST",
              url: "ProjectWBS.aspx/BindModuleDropDown",
              contentType: "application/json;charset=utf-8",
              async: false,
              data: "{}",
              dataType: "json",
              success: function (msg) {

                  $("#ctl00_ContentPlaceHolder1_txtModule").kendoDropDownList({
                      optionLabel: "Select Module Name",
                      dataTextField: "Name",
                      dataValueField: "ID",
                     change: chModuleChange,
                      dataSource: jQuery.parseJSON(msg.d)
                      
                  }).data("kendoDropDownList");


              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
    );
}


function chModuleChange() {

    //var text= $("#ctl00_ContentPlaceHolder1_txtModule").data("kendoDropDownList").text();
    $('[id$="hdnModuleName"]').val($("#ctl00_ContentPlaceHolder1_txtModule").data("kendoDropDownList").text());
   

}
/*Bind Name*/

function FillNameDropDown() {
    $.ajax(
          {
              type: "POST",
              url: "ProjectWBS.aspx/BindNameDropDown",
              contentType: "application/json;charset=utf-8",
              async: false,
              data: "{}",
              dataType: "json",
              success: function (msg) {

                  $("#ctl00_ContentPlaceHolder1_txtName").kendoDropDownList({
                      optionLabel: "Select Employee Name",
                      dataTextField: "empName",
                      dataValueField: "empid",
                      change: chNameChange,
                      dataSource: jQuery.parseJSON(msg.d)
                  }).data("kendoDropDownList");


              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
    );
}

function chNameChange() {

    $('[id$="hdnSelectedEmpName"]').val($("#ctl00_ContentPlaceHolder1_txtName").data("kendoDropDownList").text());
}

/*Bind WBS */

function BindWBS() {
    $.ajax(
          {
              type: "POST",
              url: "ProjectWBS.aspx/BindWBS",
              contentType: "application/json;charset=utf-8",
              async: false,
              data: "{}",
              dataType: "json",
              success: function (msg) {

                  var data = jQuery.parseJSON(msg.d);
                  data.push({ Description: "Unplanned", ProjectWBSID: 0 });
                  $("#ctl00_ContentPlaceHolder1_txtWBS").kendoDropDownList({
                      optionLabel: "Select WBS",
                      dataTextField: "Description",
                      dataValueField: "ProjectWBSID",
                      change: OnWBSChange,
                      dataSource: data
                  }).data("kendoDropDownList");


              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
    );
}

function BindAllWBS() {
    $.ajax(
          {
              type: "POST",
              url: "ProjectWBS.aspx/BindAllWBS",
              contentType: "application/json;charset=utf-8",
              async: false,
              data: "{}",
              dataType: "json",
              success: function (msg) {
                  var data = jQuery.parseJSON(msg.d);
                  data.push({ Description: "Unplanned", ProjectWBSID: 0 });
                  $("#ctl00_ContentPlaceHolder1_txtWBS").kendoDropDownList({
                      optionLabel: "Select WBS",
                      dataTextField: "Description",
                      dataValueField: "ProjectWBSID",
                      change: OnWBSAllChange,
                      dataSource: data
                  }).data("kendoDropDownList");
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
    );
}

function OnWBSChange() {
    $('[id$="hdnWBSName"]').val($("#ctl00_ContentPlaceHolder1_txtWBS").val());
    $('[id$="hdnWBS"]').val($("#ctl00_ContentPlaceHolder1_txtWBS").data("kendoDropDownList").text());

    //alert($('[id$="hdnWBS"]').val());
}

function OnWBSAllChange() {
    $('[id$="hdnWBSName"]').val($("#ctl00_ContentPlaceHolder1_txtWBS").val());
    $('[id$="hdnWBS"]').val($("#ctl00_ContentPlaceHolder1_txtWBS").data("kendoDropDownList").text());
  //  alert($('[id$="hdnWBS"]').val());
}


function EditProjWBSDetails(e) {

    $('[id$="btnSaveWBS"]').val('UPDATE');
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);

    CheckEditValue = "Edit";

    OpenPopUp(CheckEditValue);

    var projectWBSID = data.ProjectWBSID;
    $('[id$="hdnWBSID"]').val(projectWBSID);

    
   

    //alert(projectWBSID);
    var WBS = data.WBSId

    if (data.WBSId != null) {
        if (data.Status != 'Completed') {
            $("#ctl00_ContentPlaceHolder1_txtWBSName").css('display', 'none');
            $("#ctl00_ContentPlaceHolder1_txtWBS").data("kendoDropDownList").value(WBS);
            $('[id$="hdnWBSName"]').val(WBS);
        }
        else {
            $('#ctl00_ContentPlaceHolder1_txtWBS').data("kendoDropDownList").wrapper.hide();
            $("#ctl00_ContentPlaceHolder1_txtWBSName").css('display', 'block');
            $('#ctl00_ContentPlaceHolder1_txtWBSName').val(data.WBS);
            $('[id$="hdnWBSName"]').val(WBS);
            $('#ctl00_ContentPlaceHolder1_txtWBSName').attr('readonly', true);
        }
    }
    $('[id$="hdnWBS"]').val($("#ctl00_ContentPlaceHolder1_txtWBS").data("kendoDropDownList").text());
    var EmpName = data.empId
    if (data.empId != null) {
        $("#ctl00_ContentPlaceHolder1_txtName").data("kendoDropDownList").value(EmpName);
    }
    if (data.ModuleID != null) {
        $('[id$="txtModule"]').data("kendoDropDownList").value(data.ModuleID);

       // $('[id$=hdnModuleName]').val(data.ModuleName);
    }

    

    //var str = FormatDateTime(data.SDate);
    //var t = str.replace(/\s/g, '')
    $('[id$="hdnSelectedEmpName"]').val($("#ctl00_ContentPlaceHolder1_txtName").data("kendoDropDownList").text());
    $('#ctl00_ContentPlaceHolder1_txtSDate').val(FormatDateTime(data.SDate));
    $('[id$="hdnSTime"]').val(FormatDateTime(data.SDate));

    //kendo.toString($('#ctl00_ContentPlaceHolder1_txtSDate').data("kendoDateTimePicker").value(FormatDateTime(data.SDate)), 'dd MMM yyyy hh:mm tt');
    AssignStartDateValue = FormatDateTime(data.SDate);

    $('#ctl00_ContentPlaceHolder1_txtEDate').val(FormatDateTime(data.EDate));
    $('[id$="hdnETime"]').val(FormatDateTime(data.EDate));

    //kendo.toString($('#ctl00_ContentPlaceHolder1_txtEDate').data("kendoDateTimePicker").value(FormatDateTime(data.EDate)), 'dd MMM yyyy hh:mm tt');

    AssignEndDateValue = FormatDateTime(data.EDate);

    $('#ctl00_ContentPlaceHolder1_txtHours').val(data.DHours);
    $('#ctl00_ContentPlaceHolder1_txtComment').val(data.Comment);
}


function clear() {
    $("#ctl00_ContentPlaceHolder1_txtWBS").data("kendoDropDownList").value('');
    $("#ctl00_ContentPlaceHolder1_txtName").data("kendoDropDownList").value('');
    $('[id$="txtModule"]').data("kendoDropDownList").value('');
    $("#ctl00_ContentPlaceHolder1_txtSDate").val('');
    $("#ctl00_ContentPlaceHolder1_txtEDate").val('');
    $("#ctl00_ContentPlaceHolder1_txtComment").val('');

    $('[id$="btnSaveWBS"]').val('SAVE');

    $("#lblerrmsgTimesheet").empty();
    $("#lblerrmsgName").empty();
    $("#lblerrmsgstartDate").empty();
    $("#lblerrmsgendDate").empty();
    $("#lblerrormsgDate").empty();
    $("#lblerrmsgModule").empty();

}

/*AP*/










