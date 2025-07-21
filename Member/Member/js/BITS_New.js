$(document).ready(function () {

    GetProjectDetails();
    $("#grdProjectDetails  tr:has(td)").hover(function () {
        $(this).css("cursor", "pointer");
    });
     
});

function showCursorPointerForTSDetails() {
    $("#grdTSDetails  tr:has(td)").hover(function () {
        $(this).css("cursor", "pointer");
    });
}
//////////////////////////////     Project Details ///////////////////////////
function GetProjectDetails() {

    $.ajax({
        type: "POST",
        url: "BITS.aspx/BindProjectDetails",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetProjectData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetProjectData(Tdata) {
    //var grid =
    $("#grdProjectDetails").kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        projectID: {
                            type: "number",
                            editable: false
                        },
                        Name: {
                            type: "string"
                        },
                        PM: {
                            type: "string"
                        },
                        Duration: {
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
       // detailTemplate: kendo.template($("#grdTSBreakup").html()),
        detailInit: detailInit,
        dataBound: function () {
            this.expandRow(this.tbody.find("tr.k-master-row").first());
        },
        //selectable: 'row',  //selects a row on click
        columns: [{
            field: "projectID",
            title: "ProjectID",
            width: "50px",
            hidden: true
        }, {
            field: "Name",
            title: "Project",
            width: "120px",
        }, {
            field: "PM",
            title: "PM",
            width: "80px",
        }, {
            field: "Duration",
            title: "Duration",
            width: "150px"
        }, {
            field: "BudgetedHour",
            title: "Budgeted Hours",
            width: "100px",
            //template: '<div style="text-align:right;">#= BudgetedHour#</div>'
            template: "<div class='ra'>#= BudgetedHour #</div>" //added css for this in aspx page
        }, {
            field: "ActualHour",
            title: "Actual Hours",
            width: "100px",
            template: "<div class='ra'>#= ActualHour #</div>"
        }, {
            field: "UnApprovedHours",
            title: "Unapproved Hours",
            width: "100px",
            template: "<div class='ra'>#= UnApprovedHours #</div>"
        }, {
            field: "Status",
            title: "Status",
            width: "150px"
        },],

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
           //change: function (arg) {
              
           //    var window = $("#details");
           //    openPopUP();

           //    var gview = $("#grdProjectDetails").data("kendoGrid");
           //    var selectedItem = gview.dataItem(gview.select());
           //    //alert(selectedItem.ID);
           //    $('[id$="hdnProjectId"]').val(selectedItem.ID);
           //    //$('[id$="lblProjectId"]').html(selectedItem.ID + "," + selectedItem.Name);
           //    //alert($('[id$="hdnProjectId"]').val());

           //    var projectName = selectedItem.Name;
           //    $('[id$="lblProjectName"]').html(projectName);

           //    var PM = selectedItem.PM;
           //    $('[id$="lblPM"]').html(PM);

           //    var Duration = selectedItem.Duration;
           //    $('[id$="lblDuration"]').html(Duration);

           //    var BudgetedHour = selectedItem.BudgetedHour;
           //    $('[id$="lblBudgetedHrs"]').html(BudgetedHour);

           //    var Status = selectedItem.Status;
           //    $('[id$="lblStatus"]').html(Status);

           //    GetTSBreakup($('[id$="hdnProjectId"]').val());
           //    GetTimesheetDetails($('[id$="hdnProjectId"]').val());
           //    showCursorPointerForTSDetails();
           //},
    });
}

function detailInit(e) {
    $("<div/>").appendTo(e.detailCell).kendoGrid({
        dataSource: {
            type: "odata",
            //transport: {
            //    read: "BITS.aspx/GetTSBreakupDetails" 
            //},
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            pageSize: 10,
            filter: { field: "projectID", operator: "eq", value: e.data.ID }
        },
        scrollable: false,
        sortable: true,
        pageable: true,
        columns: [
            { field: "Module", width: "110px" , title: "Work"},
            { field: "ActualHour", title: "Actual Hour", width: "110px" },
            { field: "UnApprovedHours", title: "Unapproved Hours" },
        ]
    });
}

//////////////////////////////     TimeSheet Breakup Section (2nd Popup) ///////////////////////////

function GetTSBreakup(prjId) {

    $.ajax({
        type: "POST",
        url: "BITS.aspx/GetTSBreakupDetails",
        contentType: "application/json;charset=utf-8",
        data: "{'prjId':'" + prjId + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetTSBreakupData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetTSBreakupData(Tdata) {
    $("#grdTSBreakup").kendoGrid({
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
                      }
                }
            },
            //pageSize: 25,
        },
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
            template: "<div class='ra'>#= ActualHour #</div>"
        }, {
            field: "UnApprovedHours",
            title: "Unapproved Hours",
            width: "50px",
            template: "<div class='ra'>#= UnApprovedHours #</div>"
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


//////////////////////////////     TimeSheet Details Section (2nd Popup) ///////////////////////////

function GetTimesheetDetails(prjId) {

    $.ajax({
        type: "POST",
        url: "BITS.aspx/GetTimesheetDetails",
        contentType: "application/json;charset=utf-8",
        data: "{'prjId':'" + prjId + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetTimesheetData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetTimesheetData(Tdata) {
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
            hidden:true,
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
            template: "<div class='ra'>#= ActualHour #</div>"
        }, {
            field: "UnApprovedHours",
            title: "Unapproved Hours",
            width: "50px",
            template: "<div class='ra'>#= UnApprovedHours #</div>"
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
            //alert($('[id$="hdnProjectId"]').val() + " "+ $('[id$="hdnTSYear"]').val()+" "+ $('[id$="hdnTSMonth"]').val());

            GetTimesheetDetailsWorkwise($('[id$="hdnProjectId"]').val(),$('[id$="hdnTSYear"]').val(),$('[id$="hdnTSMonth"]').val());

           },
    });
}



//////////////////////////////     TimeSheet Details WorkWise (3rd Popup)///////////////////////////

function GetTimesheetDetailsWorkwise(prjId,year,month) {

    $.ajax({
        type: "POST",
        url: "BITS.aspx/GetTimesheetDetailsWorkwise",
        contentType: "application/json;charset=utf-8",
        data: "{'prjId':'" + prjId + "','year':'" + year + "','month':'" + month + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetTimesheetWorkWiseData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetTimesheetWorkWiseData(Tdata) {
    $("#grdTSModuleDetails").kendoGrid({
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
        columns: [{
            field: "Module",
            title: "Work",
            width: "80px",
        },{
            field: "ActualHour",
            title: "Hours",
            width: "50px",
            template: "<div class='ra'>#= ActualHour #</div>"
        }, {
            field: "UnApprovedHours",
            title: "Unapproved Hours",
            width: "50px",
            template: "<div class='ra'>#= UnApprovedHours #</div>"
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


///////////////////////////////////////////////////////////////////////////////////////////////////

function openPopUP() {
    $('#details').css('display', '');
    $('#divOverlay').addClass('k-overlay');
}

function closePopUP() {
    $('#details').css('display', 'none');
    $('#divOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}


function openTSPopUP() {
    $('#TSdetails').css('display', '');
    $('#divOverlay').addClass('k-overlay');
}

function closeTSPopUP() {
    $('#TSdetails').css('display', 'none');
    $('#divOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}


