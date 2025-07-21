
// Date Selector  by Satish Vishwakarma 
var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

$(document).ready(function () {
    var today = new Date();
    $(Dpicker).kendoDatePicker({
        value: today,
        format: "MMM yyyy",
        footer: "Today - #=kendo.toString(data, 'd') #"
    });

    $(Dpicker).data("kendoDatePicker")
                    .dateView.calendar.element
                    .width(300);

    setMontYrs();
    bindGrid();
});


function GetSetTime(mMode) {
    var dt = new Date("01" + " " + $(Dpicker).val());

    var Mindex = dt.getMonth();
    if (mMode == 1) {
        if (Mindex != 0) {
            ResetDate(monthNames[dt.getMonth() - 1] + " " + dt.getFullYear());
        }
        else {
            Premonth = monthNames[monthNames.indexOf('Dec')];
            Preyears = parseInt(dt.getFullYear()) - 1;
            ResetDate(Premonth + " " + Preyears);
        }
    }
    else if (mMode == 2) {
        if (Mindex != 11) {
            ResetDate(monthNames[dt.getMonth() + 1] + " " + dt.getFullYear());
        }
        else {
            Nextmonth = monthNames[monthNames.indexOf('Jan')];
            Nextyears = parseInt(dt.getFullYear()) + 1;
            ResetDate(Nextmonth + " " + Nextyears);
        }
    }
    setMontYrs();
}

function setMontYrs() {
    var dt = new Date("01" + " " + $(Dpicker).val());
    mth = dt.getMonth() + 1;
    yr = dt.getFullYear()
}

function ResetDate(val) {
    var datepicker = $(Dpicker).data("kendoDatePicker");
    datepicker.value(new Date("01" + " " + val));
}

// Bind Grid By Satish Vishwakarma
function bindGrid() {
    setMontYrs();

        $.ajax({
            type: "POST",
            url: "TimeSheet.aspx/BindTimeSheetByProjectId",

            contentType: "application/json;charset=utf-8",
            data: "{'projId':'" + projId + "','month':'" + mth + "','year':'" + yr + "'}",
            dataType: "json",
            success: function (msg) {
                GetTData(jQuery.parseJSON(msg.d));
            },
            error: function (x, e) {
                alert("The call to the server side failed. "
                      + x.responseText);
            }
        });
}
function GetTData(Tdata) {
    $(GridTimesheet).kendoGrid({
        dataSource: {
            data: Tdata,
            aggregate: [{ field: "tsHour", aggregate: "sum"}],
            pageSize: 13,
            schema: {
                model: {
                    tsId: "tsId",
                    fields: {
                        tsDate: { type: "date", format: "{0:dd-MMM-yyyy}"},
                        moduleName: { type: "string" },
                        empName: { type: "string"},
                        tsHour: { type: "number" },
                        tsComment: { type: "string" }
                        //tsId: { type: "string", editable: false },
                        //empid: { type: "string", validation: { required: true } },
                        //tsEntryDate: { type: "string", validation: { required: true } },
                        // tsVerified: { type: "string", validation: { required: true } },
                        // tsVerifiedBy: {type: "string", validation: { required: true },
                        //  moduleId: { type: "string", validation: { required: true } },
                        // tsVerifiedOn: { type: "string", validation: { required: true } },
                        //projId: { type: "string", validation: { required: true }
                    }
                }
            }
        },
        //height: 705,
        scrollable: true,
        sortable: true,
        pageable: { input: true, numeric: false },
        
        columns: [
                     //editor: projectNameEditor, min: 1
                    { field: "tsDate", format: "{0:dd-MMM-yyyy}", title: "Date", width: "100px" },
                    { field: "moduleName", title: "Module Name" },
                    { field: "empName", title: "Team Members", footerTemplate: "<b>Total Hours</b>" },
                    { field: "tsHour", title: "Hours", width: "75px", footerTemplate: " <b> #=sum# </b>" },
                    { field: "tsComment", title: "Comments" }

                    // ,attributes:  { style:"text-align:right;" } right aligment
                   // { field: "projName", title: "Project Name" }, footerTemplate: "Total:#=sum#",  Min: #: min # Max: #: max #
                    //{ command: [{  text: "Editar", click: editFunction }, { text: "Eliminar", click: deleteFunction }]},
        ],
       // selectable: "row",
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
    });
}
