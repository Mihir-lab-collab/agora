$(document).ready(function () {
    GetEvents(0);
    BindAllDateCalender();
    BindTime();
});

function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    RedirectPage();
}

function ShowAddPopup() {
    $('[id$="hdnKEID"]').val("0"); 
    //$('[id$="lblerrmsgdate"]').hide();
    //$('[id$="lblerrmsgnarration"]').hide();
    openAddPopUP();     
}
function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
    //$('[id$="hdnEventDate"]').val('');
    //BindAllDateCalender();
    Clearrecord();
}
function Clearrecord()
{
    $('[id$="hdnEventDate"]').val('');
    $('[id$="txtEventDate"]').val('');
    $('[id$="hdnDescription"]').val('');
    $('[id$="txtNarration"]').val('');
}

function BindAllDateCalender() {
    $("#txtEventDate").kendoDatePicker({ format: "dd/MM/yyyy" });
}

function BindTime()
{
        // create TimePicker from input HTML element
    $("#timepicker").kendoTimePicker();
    //$("#timepicker").val("11:00 AM")
}

function BindAllEditDateCalender() {
    $("#txtEventDate").kendoDatePicker({ format: "dd/MM/yyyy" });
}

function BindOldCIP() {
    if($('[id$="chkOLD"]').prop("checked"))
        GetEvents(1);
    else
        GetEvents(0);
}

function GetEvents(isOLD) {

    $.ajax({
        type: "POST",
        url: "CIP.aspx/BindEvents",
        contentType: "application/json;charset=utf-8",
        data: "{KEID:'" + parseInt(isOLD) + "'}",
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

function ClosingRateWindow(e) {

    var grid = $(gridEvents).data("kendoGrid");
    grid.refresh();

}

function SetData(EventData) {

    $(gridEvents).kendoGrid({
        dataSource: {
            data: EventData,
            schema: {
                model: {
                    fields: {
                        KEID: { type: "number" },
                        EventDate: { type: "string" },
                        Description: { type: "string" },
                        Time:{type: "string" }
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
                    { field: "KEID", title: "ID", width: "50px", hidden: true },
                    { field: "EventDate", title: "Event Date", width: "20px", hidden: true },
                    { field: "EventDateTime", title: "Event Date", width: "20px" },
                    { field: "Time", title: "Time", width: "50px", hidden: true },
                    { field: "Description", title: "Description", width: "80px" },                    
                    {
                      command: [
                            { name: "edit",text:"edit",click: EditEvents }
                            , { name: "destroy",text:"delete",className: "ob-delete", click: DeleteEvent }
                        ], width: "20px"
                    }

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

function EditEvents(e)
{
    openAddPopUP();
    EventItems = this.dataItem($(e.currentTarget).closest("tr"));

    $('[id$="hdnKEID"]').val(EventItems.KEID);    
    $('[id$="hdnDescription"]').val(EventItems.Description);
    $('[id$="txtNarration"]').val(EventItems.Description);
    $("#timepicker").val(EventItems.Time); 
    $('[id$="hdnTime"]').val(EventItems.Time);

    var eventDate = EventItems.EventDate;
    var hdate = new Date(Date.parse(eventDate));
            var day = hdate.getDate();
            if (day.toString().length <= 1) {
                day = "0" + day;
            }
            var month = hdate.getMonth() + 1;
            if (month.toString().length <= 1) {
                month = "0" + month;
            }
            var year = hdate.getFullYear();
            eventDate = day + '/' + month + '/' + year;

            $('[id$="hdnEventDate"]').val(eventDate);
            $('[id$="txtEventDate"]').val(eventDate);
    //openAddPopUP();
}

function DeleteEvent(e)
{
    if (!confirm("Are you sure you want to delete?"))
            return;
    EventItems = this.dataItem($(e.currentTarget).closest("tr"));
    var KEID = EventItems.KEID;
    Delete(KEID);
}

function Delete(KEID) {
    ////EventItems = this.dataItem($(e.currentTarget).closest("tr"));
    ////var KEID = EventItems.KEID;
    //if (!confirm("Are you sure you want to delete?"))
    //    return;
    $.ajax(
           {

               type: "POST",
               url: "CIP.aspx/DeleteEvent",
               contentType: "application/json;charset=utf-8",
               data: "{'KEID':'" +parseInt(KEID) + "'}",
               cache: false,
               async: false,
               dataType: "json",
               success: function (msg) {
                   var message = jQuery.parseJSON(msg.d);
                   alert('Event deleted successfully.');
                   $('body').css('cursor', 'default');
                   RedirectPage();
               },
               error: function (msg) {
                   alert("The call to the server side failed."
                          + msg.responseText);
               }
           }
        );
}
function RedirectPage() {
    window.location.assign("CIP.aspx");
}