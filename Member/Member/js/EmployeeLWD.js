$(document).ready(function () {

    GetEmployeeLWDDetails();

});
function GetEmployeeLWDDetails() {
    
    $.ajax({
        type: "POST",
        url: "EmployeeLWD.aspx/BindEmployeeLWD",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        dataType: "json",
        async: false,
        success: function (msg) {           
            console.log(jQuery.parseJSON(msg.d));           
            GetEmployeeLWDData(jQuery.parseJSON(msg.d));
            //GetEmployeeData(jQuery.parseJSON(msg.d));

        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}
function GetEmployeeLWDData(Tdata) {
    $(gridEmployeeLWD).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        empid: { type: "number" },
                        empName: { type: "string" },
                        empExpectedLWD: { type: "date" },
                        daysPending: { type: "string" }

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
            { field: "empid", title: "Employee ID", width: "50px" },
            { field: "empName", title: "Employee Name", width: "80px" },
            { field: "empExpectedLWD", title: "Expected LWD", width: "50px", format: "{0:dd-MMM-yyyy}" },//LWD
            { field: "daysPending", title: "Days Pending", width: "80px" },
            //,
           /* { command: [{ name: "edit" }], width: "10px" }*/
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
    });
    //// vw header Hover display
    //$("#gridEmployeeLWD").kendoTooltip({
    //    filter: ".k-header"
    //});

}
