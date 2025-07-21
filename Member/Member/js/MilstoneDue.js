$(document).ready(function () {
    BindMilestoneDue("");
    $('[id$="txtFromDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
});

function BindMilestoneDue(mDate) {

    $.ajax({
        type: "POST",
        url: "MilestoneDue.aspx/BindMilestoneDue",
        contentType: "application/json;charset=utf-8",
        data: "{mDate:'" + mDate + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {

            SetData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function SetData(EventData) {

    $(gridMiles).kendoGrid({
        dataSource: {
            data: EventData,
            schema: {
                model: {
                    fields: {
                        Name: { type: "string" },
                        Amount: { type: "string" },
                        Balance: { type: "string" },
                      //  ExRate: { type: "number" },
                        DueDate:  { type: "string" },//{ type: "date", format: "{0:dd-MMM-yyyy}" }
                        EstHours: { type: "number" },
                        //Description: { type: "string" },
                        isRecurring: { type: "string" },
                        ProjName: { type: "string" },
                        ProjID: { type: "number" },
                        DueFor: { type: "number" },
                        DueDays: { type: "number" }
                    }

                }
            },
            pageSize: 50,
        },
        scrollable: true,
        sortable: true,
        resizable: true,
        selectable: true,
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
                   
                    //{ field: "ProjID", title: "ProjID", width: "50px" },
                    { field: "ProjName", title: "Project Name", width: "70px" },
                  //  { field: "ProjID", title: "ProjID", width: "50px" },
                    {
                        field: "Name", title: "Name", width: "170px"
                        , template: '<a href="../Member/Milestone.aspx?ProjID=#=ProjID#"">#= Name #</a>'
                    },
                     {
                         field: "DueDate", title: "Due Date", width: "50px"
                    // , template: '<div class="ra" style="text-align:right;">#= kendo.toString(DueDate,"MM/dd/yyyy") # </div>'
                     },
                    //{
                    //    field: "DueFor", title: "Due For", width: "50px"
                    //    , template: '<div class="ra" style="text-align:center;">#= EstHours #</div>'
                    //},
                    {
                        field: "Amount", title: "Amount", width: "50px"
                       , template: '<div class="ra" style="text-align:right;">#= kendo.toString(Amount,"n0") #</div>'
                    },
                    {
                        field: "Balance", title: "Balance", width: "50px", format: "{0:c3}"
                        ,template: '<div class="ra" style="text-align:right;">#= kendo.toString(Balance,"n0") #</div>'
                    },
                    { field: "DueDays", title: "Due Days", width: "50px" },
                    //{
                    //    field: "ExRate", title: "ExRate", width: "30px"
                    //    ,template: '<div class="ra" style="text-align:right;">#= ExRate #</div>'
                    //},                    
                    {
                        field: "EstHours", title: "EstHours", width: "30px"
                       ,template: '<div class="ra" style="text-align:center;">#= EstHours #</div>'
                    },
                    //{ field: "Description", title: "Description", width: "150px" },
                    //{ field: "isRecurring", title: "Recurring", width: "50px" },
                    //{
                    //    command: [
                    //          { name: "edit", text: "edit", click: EditEvents }
                    //          , { name: "destroy", text: "delete", className: "ob-delete", click: DeleteEvent }
                    //    ], width: "20px"
                    //}

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
        //remove: function (e) {
        //    if (confirm("Are you sure you want to delete?")) {
        //        DeleteEvent(e.model.KEID);
        //    } else {

        //        e.preventDefault()
        //        ClosingRateWindow(e);

        //    }

        //},

    });

}

function Search()
{
    var mDate = $('[id$="txtFromDate"]').val();
    BindMilestoneDue(mDate);
}