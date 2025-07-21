$(document).ready(function () {
    GetSearchReportDetails();

});

function GetSearchReportDetails() {
    var EmpID = $('[id$="txtEmailId"]').val();
    var LocationId = $('[id$="hdLocationId"]').val();
    var Date = $('[id$="lblMonth"]').text();

    if (EmpID == "")
        EmpID = 0;
   
    
        $.ajax({
            type: "POST",
            url: "TimesheetIncomplete.aspx/BindReports",
            contentType: "application/json;charset=utf-8",
            data: "{'Date':'" + Date + "','Month':'" + 7 + "','Year':'" + 2015 + "','EmpID':'" + EmpID + "','LocationID':'" + LocationId + "'}",
            dataType: "json",
            async: false,
            success: function (msg) {
                GetReportData(jQuery.parseJSON(msg.d));
            },
            error: function (x, e) {
                alert("The call to the server side failed. "
                      + x.responseText);
            }
        });
}


function GetReportData(Tdata) {
    $("#grdReport").kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        EmpID: { type: "number" },
                        EmpName: { type: "string" },
                        TSDate: { type: "date" },
                        AttHour: { type: "string" },
                        AttTSHour: { type: "string" }
                        
                    }
                }
            },
            pageSize: 100,
        },
        scrollable: true,
        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
                    { field: "EmpID", title: "Emp Id", width: "50px" },
                    { field: "EmpName", title: "EMP Name", width: "90px" },
                    { field: "TSDate", title: "Date", width: "90px", format:"{0:dd-MMM-yyyy}" },
                    { field: "AttHour", title: "Time Available", width: "90px" },
                    { field: "AttTSHour", title: "Time Reported", width: "90px" }
                      
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
    });
}