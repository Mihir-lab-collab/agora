$(document).ready(function () {
    FillEmployeeDropDown();
    $("#txtMeetingDate").kendoDatePicker({ format: "dd/MMM/yyyy" });
    var MeetingId = 0;
    var qrStr = window.location.search;
    var Flag = qrStr.split('?')[1].split('&')[0].split('=')[1];
    MeetingId = qrStr.split('?')[1].split('&')[1].split('=')[1];
    $("#hd_MeetingId").val(MeetingId);
    if (Flag == "Conduct") {
        Get_MeetingDetails(MeetingId);
        $('#InviteMeeting *').prop('disabled', true);
        document.getElementById("btnAddProjectsReview").style.display = "none";
        document.getElementById("btnCancelProjectsReview").style.display = "none";
        var datepicker = $("#txtMeetingDate").data("kendoDatePicker");
        datepicker.enable(false);
        var multiselect = $("#txtAttendees").data("kendoMultiSelect");
        multiselect.enable(false);
        var CalledBy = $("#txtMeetingCalledBy").data("kendoDropDownList");
        CalledBy.enable(false);
        var Facilitator = $("#txtFacilitator").data("kendoDropDownList");
        Facilitator.enable(false);
        document.getElementById("ConductMeeting").style.display = "block";
        var showOverHeads;
        var Added;
        var Initiated;
        var InProgress;
        var UnderUAT;
        var OnHold;
        var CompletedClosed;
        var Cancelled;
        var UnderWarranty;
        var TNM;
        var FixedCost;

        var pathname = window.location.pathname;
        var url = window.location.href;
        showOverHeads = true;
        Added = true;
        Initiated = true;
        InProgress = true;
        UnderUAT = true;
        OnHold = true;
        CompletedClosed = false;
        Cancelled = false;
        UnderWarranty = true;
        TNM = true;
        FixedCost = true;

        if (pathname.toString().indexOf('BITSManage') > 0) {
            GetProjectDetails_Manage(showOverHeads, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty, TNM, FixedCost);
        }
        else {
            GetProjectDetails(false, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty, TNM, FixedCost);
        }
        $("#grdProjectDetails  tr:has(td)").hover(function () {
            $(this).css("cursor", "pointer");
        });
    }
    else if (Flag == "Edit") {
        Get_MeetingDetails(MeetingId);
        document.getElementById("btn_SendMOM").style.display = "none";
        document.getElementById("ConductMeeting").style.display = "none";
        var btn = document.getElementById("btnAddProjectsReview");
        btn.value = 'Reschedule Meeting'; // will just add a hidden value
        btn.innerHTML = 'Reschedule Meeting';
    }
    else {
        document.getElementById("ConductMeeting").style.display = "none";
    }
});
function GetProjectDetails(showOverHeads, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty, TNM, FixedCost) {
    var PMID = $('[id$="hdnAdmin"]').val();
    $.ajax({
        type: "POST",
        url: "ProjectsReview.aspx/BindProjectDetails",
        contentType: "application/json;charset=utf-8",
        data: "{'PMID':'" + PMID + "','showOverHeads':'" + showOverHeads + "','Added':'" + Added + "','Initiated':'" + Initiated + "','InProgress':'" + InProgress + "','UnderUAT':'" + UnderUAT + "','OnHold':'" + OnHold + "','CompletedClosed':'" + CompletedClosed + "','Cancelled':'" + Cancelled + "','UnderWarranty':'" + UnderWarranty + "','TNM':'" + TNM + "','FixedCost':'" + FixedCost + "'}",
        dataType: "json",
        async: true,
        success: function (msg) {
            var data = $.parseJSON(msg.d);
            var mBox = $("#msgBox");
            if (data.length == 0) {
                GetProjectData(jQuery.parseJSON(msg.d));

            }
            else {
                var obj = $.parseJSON(msg.d);
                GetProjectData(obj);
                return false;
            }
        },
        error: function (x, e) {
            alert("The call to the server side failed. " + x.responseText);
        }
    });
}
function GetProjectData(Tdata) {
    $("#grdProjectDetails").empty();

    var grid = $("#grdProjectDetails").kendoGrid({
        dataSource: new kendo.data.DataSource({
            data: Tdata,
            schema: {
                model: {
                    id: "EmployeeCode",
                    fields: {
                        ID: {
                            type: "number",
                            editable: false
                        },
                        Name: {
                            type: "string"
                        },
                        Status: {
                            type: "string"
                        },
                        Duration:
                        {
                            type: "string"
                        },
                        PM: {
                            type: "string"
                        },
                        BA: {
                            type: "string"
                        },
                        AccManager: {
                            type: "string"
                        },
                        BudgetedHour: {
                            type: "number"
                        },
                        ActualHour: {
                            type: "number"
                        },
                        UnApprovedHours: {
                            type: "number"
                        },
                        status: {
                            type: "string"
                        },
                        BudgetedCost: {
                            type: "number"
                        },
                        ActualCost: {
                            type: "number"
                        },
                        ActualPayment: {
                            type: "number"
                        },
                        PaymentRatio: {
                            type: "number"
                        },
                        ProjectHealth_Effort: {
                            type: "number"
                        },
                        Reportdate: {
                            type: "date"
                        },
                        Status: {
                            type: "string"
                        },

                    }
                },
            },
            pageSize: 30,
            serverPaging: true,
            serverSorting: true
        }),
        scrollable: true,
        sortable: true,
        pageable: true,


        detailExpand: function (e) {
            e.sender.tbody.find('.k-detail-row').each(function (idx, item) {
                if (item !== e.detailRow[0]) {
                    e.sender.collapseRow($(item).prev());
                }
            })
        },
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
            {
                field: "ID",
                title: "ProjectID",
                width: "10px",
                hidden: true
            }, {
                field: "Name",
                title: "Project",
                width: "120px",
            },
            {
                field: "PM",
                title: "PM",
                width: "80px",
            }, {
                field: "BA",
                title: "BA",
                width: "80px",
            }, {
                field: "AccManager",
                title: "A/C Manager",
                width: "80px",
            },
            {
                field: "",
                title: "Status",
                width: "20px",
            }
        ],
        editable: false,

        detailTemplate: "<div><table><tr><td><b>Duration:</b> #: Duration #</td> <td><b>Report Date:</b> #:kendo.toString(kendo.parseDate(Reportdate, 'yyyy-MM-dd'), 'dd-MMM-yyyy')#</td></tr><tr><td><b>Budgeted Hours:</b> #: BudgetedHour #</td><td><b>Actual Hours:</b> #:ActualHour #</td></tr><tr><td><b>Budgeted Cost:</b> #: BudgetedCost #</td><td><b>Actual Cost:</b> #:ActualCost #</td></tr><tr><td><b>Payment Received:</b> #:ActualPayment#</td><td><b>Status:</b> #:Status #</td></tr></table><div class= 'popup_head'><h3>Timesheet Breakup - Work Wise</h3><div class='clear'></div></div><div id='grdTSBreakup' style='align-content: center;'></div><div class='clear'></div><div class='popup_head'><h3>Timesheet</h3><div class='clear'></div></div><div id='grdTSDetails' style='align-content: center;'></div></div> <div id='GridProjectStaus' style='align-content: center;'> ",
        detailInit: OpenPanel,
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
        //change: function (arg) {
        //    var window = $("#details");
        //    openPopUP();
        //}


    }).data("kendoGrid");
}
function OpenPanel(e) {
    ClearRecord();
    var prjId = e.data.ID;
    GetTSBreakupManag(prjId);
    GetTimesheetDetailsManage(prjId);
    GetProjUpdateStatus1(prjId);
}
function ClearRecord() {
    if ($("#grdTSBreakup").data('kendoGrid') != undefined && $("#grdTSBreakup").data('kendoGrid') != "") {
        $("#grdTSBreakup").data('kendoGrid').value("");
    }

}
function detailInit(e) {
    var prjId = e.data.ID;

    $("<div/>").appendTo(e.detailCell).kendoGrid({
        dataSource: {
            type: "odata",
            transport: {
                read:
                {
                    url: "ProjectsReview.aspx/GetTSBreakupDetails",
                    contentType: "application/json;charset=utf-8",
                    data: "{'prjId':'" + prjId + "'}",
                    dataType: "json",
                }

                //GetTSBreakupManage(e) //Action("GetTSBreakupDetails(ID)","ProjectsReview")

            },
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            pageSize: 3,
            filter: { field: "EmployeeID", operator: "eq", value: e.data.EmployeeID }
        },
        scrollable: false,
        sortable: true,
        pageable: true,
        columns: [
            { field: "OrderID", width: "110px" },
            { field: "ShipCountry", title: "Ship Country", width: "110px" },
            { field: "ShipAddress", title: "Ship Address" },
            { field: "ShipName", title: "Ship Name", width: "300px" }
        ]
    });

}
function GetTSBreakupManag(prjId) {


    $.ajax({
        type: "POST",
        url: "ProjectsReview.aspx/GetTSBreakupDetails",
        contentType: "application/json;charset=utf-8",
        data: "{'prjId':'" + prjId + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetTSBreakupDataManag(jQuery.parseJSON(msg.d));

        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}
function GetTSBreakupDataManag(Tdata) {
    $("#grdTSBreakup").kendoGrid({

        //$("<div/>").appendTo(e.detailCell).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        Module: {
                            type: "string"
                        },
                        ActualHour: {
                            type: "number"
                        },
                        UnApprovedHours: {
                            type: "number"
                        },
                        Cost: {
                            type: "number"
                        },
                    }
                }
            },
            //pageSize: 25,
        },

        serverPaging: true,
        serverSorting: true,
        serverFiltering: true,
        scrollable: true,
        sortable: true,
        //pageable: {
        //    input: true,
        //    numeric: false
        //},
        columns: [{
            field: "Module",
            title: "Work",
            width: "80px",
        }, {
            field: "ActualHour",
            title: "Hours",
            width: "50px",
            template: '<div class="ra">#= kendo.toString(ActualHour,"n0") #</div>'
        }, {
            field: "UnApprovedHours",
            title: "Unapproved Hours",
            width: "50px",
            template: '<div class="ra">#= kendo.toString(UnApprovedHours,"n0") #</div>'
        },
        {
            field: "Cost",
            title: "Cost",
            width: "50px",
            template: '<div class="ra">#= kendo.toString(Cost,"n0") #</div>'
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
        },
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
    });

}
function GetTimesheetDetailsManage(prjId) {
    $.ajax({
        type: "POST",
        url: "BITS.aspx/GetTimesheetDetailsMonthwise",
        contentType: "application/json;charset=utf-8",
        data: "{'prjId':'" + prjId + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetTimesheetDataManage(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}
function GetTimesheetDataManage(Tdata) {
    $("#grdTSDetails").kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        Module: {
                            type: "string"
                        },
                        TSYear: {
                            type: "string"
                        },
                        TSMonth: {
                            type: "string"
                        },
                        ActualHour: {
                            type: "number"
                        },
                        UnApprovedHours: {
                            type: "number"
                        },
                        Cost: {
                            type: "number"
                        },
                    }
                }
            },
            pageSize: 25,
        },
        scrollable: true,
        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        selectable: 'row',  //selects a row on click
        columns: [{
            field: "Module",
            title: "Month",
            width: "80px",
        },
        {
            field: "TSYear",
            title: "TSYear",
            width: "20px",
            hidden: true,
        },
        {
            field: "TSMonth",
            title: "TSMonth",
            width: "20px",
            hidden: true,
        }, {
            field: "ActualHour",
            title: "Hours",
            width: "50px",
            //format: "{0:n0}",
            template: '<div class="ra">#= kendo.toString(ActualHour,"n0") #</div>'
        }, {
            field: "UnApprovedHours",
            title: "Unapproved Hours",
            width: "50px",
            template: '<div class="ra">#= kendo.toString(UnApprovedHours,"n0") #</div>'
        },
        {
            field: "Cost",
            title: "Cost",
            width: "50px",
            template: '<div class="ra">#= kendo.toString(Cost,"n0") #</div>'
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
        },
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
        change: function (arg) {
            var window = $("#TSdetails");
            openTSPopUP();

            var gview = $("#grdTSDetails").data("kendoGrid");
            var selectedItem = gview.dataItem(gview.select());

            var gview1 = $("#grdProjectDetails").data("kendoGrid");
            var selectedItem1 = gview1.dataItem(gview1.select());

            $('[id$="hdnProjectId"]').val(selectedItem1.ID);
            $('[id$="hdnTSYear"]').val(selectedItem.TSYear);
            $('[id$="hdnTSMonth"]').val(selectedItem.TSMonth);

            GetTimesheetDetailsWorkwise($('[id$="hdnProjectId"]').val(), $('[id$="hdnTSYear"]').val(), $('[id$="hdnTSMonth"]').val());
            //showCursorPointerForMonthTSDetails()
        },
    });
    //var IsModuleAdmin = Boolean($('[id$="hdnIsModuleAdmin"]').val());
    //if (IsModuleAdmin == false) {
    //    $("#grdTSDetails").data("kendoGrid").showColumn("Cost");
    //}
    //else {
    //    $("#grdTSDetails").data("kendoGrid").hideColumn("Cost");
    //}

}
function GetProjUpdateStatus1(projId) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "MyProjects.aspx/projectStatus",
        data: "{'proid':'" + projId + "'}",
        dataType: "json",
        success: function (msg) {
            GetEmployeeUpdateStatus1(jQuery.parseJSON(msg.d));

        },
        error: function (result) {
            alert("Error");
        }
    });
}
function GetEmployeeUpdateStatus1(Tdata) {
    $(GridProjectStaus).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        projectstartdate: { type: "date" },
                        expcompleted: { type: "string" },
                        actualcompleted: { type: "string" },
                        remarks: { type: "string" },
                        PostedBy: { type: "string" }
                    }
                }
            },
            // pageSize: 5,
        },
        scrollable: false,
        sortable: true,
        //height: 600,
        //toolbar: ["create"],
        //pageable: {
        //    input: true,
        //    numeric: false
        //},
        columns: [
            { field: "projectstartdate", format: "{0:dd-MMM-yyyy}", title: "Status Date", width: "100px" },
            { field: "expcompleted", title: "Status", width: "100px" },
            { field: "actualcompleted", title: "Actual Completed", width: "100px" },
            { field: "remarks", title: "Remarks", width: "100px" },
            { field: "PostedBy", title: "Posted By", width: "100px" },

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
        editable: {
            mode: "popup",
            template: kendo.template($("#popup-editor").html())
        },
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
    });

}

//By Shubh
function FillEmployeeDropDown() {
    $.ajax({
        type: "POST",
        url: "ProjectsReview.aspx/FillEmployeeDropDown",
        contentType: "application/json;charset=utf-8",
        data: "{Mode:'GetEmployee'}",
        dataType: 'json',
        async: false,
        success: function (msg) {
            $("#txtAttendees").kendoMultiSelect({
                optionLabel: "Select Attendess List",
                dataTextField: "empName",
                dataValueField: "empid",
                dataSource: jQuery.parseJSON(msg.d)
            }).data("kendoMultiSelect");

            $("#txtMeetingCalledBy").kendoDropDownList({
                optionLabel: "Select Employee",
                dataTextField: "empName",
                dataValueField: "empid",
                dataSource: jQuery.parseJSON(msg.d)
            }).data("kendoDropDownList");

            $("#txtFacilitator").kendoDropDownList({
                optionLabel: "Select Facilitator",
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
function Get_MeetingDetails(MeetingId) {
    $.ajax({
        type: "POST",
        url: "ProjectsReview.aspx/Get_MeetingDetails",
        contentType: "application/json;charset=utf-8",
        data: "{MODE:'View_Meetings', MeetingId: '" + MeetingId + "'}",
        dataType: 'json',
        async: false,
        success: function (msg) {
            var data = $.parseJSON(msg.d);
            if (data.length > 0) {
                var MeetingId = data[0].MeetingId;
                var M_Date = data[0].MeetingDate;
                M_Date = eval(("new " + M_Date).replace(/\//g, ""));
                var CalledById = data[0].CalledById;
                var MeetingType = data[0].MeetingType;
                var Attendees = data[0].AttendeesId;
                var AgendaTopic = data[0].AgendaTopic;
                var FacilitatorId = data[0].FacilitatorId;
                var TimeAlloted = data[0].TimeAlloted;

                var MeetingDate = $("#txtMeetingDate").data("kendoDatePicker");
                MeetingDate.value(M_Date);
                $("#txtMeetingType").val(MeetingType);
                $("#txtAgendaTopic").val(AgendaTopic);
                var AttendeesId = Attendees.split(',').map(Number);
                var multiselect = $("#txtAttendees").data("kendoMultiSelect");
                multiselect.value(AttendeesId);
                var CalledBy = $("#txtMeetingCalledBy").data("kendoDropDownList");
                CalledBy.value(CalledById);
                var Facilitator = $("#txtFacilitator").data("kendoDropDownList");
                Facilitator.value(FacilitatorId);
                $("#txtTimeAlloted").val(TimeAlloted);
                $("#hd_MeetingId").val(MeetingId);
            }
        },
        error: function (x, e) {
            alert("The call to the server side failed. " + x.responseText);
        }
    });
}
function Save_MeetingDetails() {
    var MeetingId = $("#hd_MeetingId").val();
    if (MeetingId == '' || MeetingId == undefined) { MeetingId = 0 }

    var MeetingDate = $("#txtMeetingDate").val();
    var CalledBy = $("#txtMeetingCalledBy").val();
    var MeetingType = $("#txtMeetingType").val();
    var Attendees = $("#txtAttendees").data("kendoMultiSelect").value().toString();
    var AgendaTopic = $("#txtAgendaTopic").val();
    var Facilitator = $("#txtFacilitator").val();
    var TimeAlloted = $("#txtTimeAlloted").val();
    $.ajax({
        type: "POST",
        url: "ProjectsReview.aspx/Save_MeetingDetails",
        contentType: "application/json;charset=utf-8",
        data: "{MODE:'Save_MeetingDetails', MeetingId: '" + MeetingId + "', MeetingDate:'" + MeetingDate + "',CalledBy:'" + CalledBy + "',MeetingType:'" + MeetingType + "',Attendees:'" + Attendees + "',AgendaTopic:'" + AgendaTopic + "',Facilitator:'" + Facilitator + "',TimeAlloted:'" + TimeAlloted + "' }",
        dataType: 'json',
        async: false,
        success: function (msg) {
            var message = msg.d;
            if (message != 'Success')
            { alert("Meeting Invitation Failed."); }
            else {
                if (MeetingId > 0) {
                    alert("Meeting Invitation Rescheduled Successfully.");
                } else {
                    alert("Meeting Invitation Sent Successfully.");
                }
                window.location.href = 'ProjectReviewMeeting.aspx';
            }
        },
        error: function (x, e) {
            alert("The call to the server side failed. " + x.responseText);
        }
    });
}
function CancelMeeting() {
    var MeetingId = $("#hd_MeetingId").val();
    if (MeetingId == '' || MeetingId == undefined) { MeetingId = 0 }
    $.ajax({
        type: "POST",
        url: "ProjectsReview.aspx/Cancel_ProjectReviewMeeting",
        contentType: "application/json;charset=utf-8",
        data: "{Mode:'Cancel_Meeting',MeetingId:'" + MeetingId + "'}",
        dataType: "json",
        async: true,
        success: function (msg) {
            var message = msg.d;
            if (message != 'Success')
            { alert("Meeting Cancelled Failed."); }
            else {
                alert("Meeting Cancelled Successfully.");
                window.location.href = 'ProjectReviewMeeting.aspx';
            }
        },
        error: function (x, e) {
            alert("The call to the server side failed. " + x.responseText);
        }
    });
}
function clk_backButton() {
    window.location.href = 'ProjectReviewMeeting.aspx';
}








