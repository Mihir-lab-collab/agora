$(document).ready(function () {
    Get_ProjectReviewMeetingList();
});
function Get_ProjectReviewMeetingList() {
    $.ajax({
        type: "POST",
        url: "ProjectReviewMeeting.aspx/Get_ProjectReviewMeetingList",
        contentType: "application/json;charset=utf-8",
        data: "{Mode:'View_Meetings', MeetingId:'0' }",
        dataType: "json",
        async: true,
        success: function (msg) {
            var data = $.parseJSON(msg.d);
            var mBox = $("#msgBox");
            if (data.length == 0) {
                Bind_ProjectReviewMeetingGrid(jQuery.parseJSON(msg.d));
            }
            else {
                var obj = $.parseJSON(msg.d);
                Bind_ProjectReviewMeetingGrid(obj);
                return false;
            }
        },
        error: function (x, e) {
            alert("The call to the server side failed. " + x.responseText);
        }
    });
}
function Bind_ProjectReviewMeetingGrid(Tdata) {
    $("#Get_ProjectReviewMeetingGrid").empty();
    var grid = $("#Get_ProjectReviewMeetingGrid").kendoGrid({
        dataSource: new kendo.data.DataSource({
            data: Tdata,
            schema: {
                model: {
                    id: "MeetingId",
                    fields: {
                        MeetingId: { type: "number", editable: false },
                        MeetingDate: { type: "date", editable: false },
                        AgendaTopic: { type: "string", editable: false },
                        CalledById: { type: "number", editable: false },
                        CalledByName: { type: "string", editable: false },
                        AttendeesId: { type: "string", editable: false },
                        Attendees: { type: "string", editable: false },
                        MeetingType: { type: "string", editable: false },
                        FacilitatorId: { type: "string", editable: false },
                        FacilitatorName: { type: "string", editable: false },
                        TimeAlloted: { type: "string", editable: false },
                        MeetingStatus: { type: "string", editable: false },
                        InsertedOn: { type: "date", editable: false },
                        InsertedBy: { type: "number", editable: false },
                    }
                },
            },
            pageSize: 30,
        }),
        dataBound: OnGridDataBound,
        scrollable: true,
        sortable: true,
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
            { command: [{ name: "edit", click: EditMeeting }, { name: "Conduct", click: ConductMeeting }], title: "Action", width: "30px" },
            { field: "MeetingId", title: "MeetingId", hidden: true, width: "50px", sortable: true },
            { field: "MeetingDate", hidden: false, title: "MeetingDate", width: "35px", format: "{0:dd-MMM-yyyy}", filterable: false },
            { field: "AgendaTopic", title: "Agenda Topic", width: "50px", sortable: true },
            { field: "CalledById", hidden: true, title: "Called By Id", width: "50px", sortable: true },
            { field: "CalledByName", title: "Called By", width: "50px", sortable: true },
            { field: "AttendeesId", hidden: true, title: "AttendeesId", width: "50px", sortable: true },
            { field: "Attendees", title: "Attendees", width: "50px", sortable: true },
            { field: "MeetingType", title: "MeetingType", width: "50px", sortable: true },
            { field: "FacilitatorId", hidden: true, title: "FacilitatorId", width: "50px", sortable: true },
            { field: "FacilitatorName", title: "Facilitator", width: "50px", sortable: true },
            { field: "TimeAlloted", title: "Time Alloted", width: "50px", sortable: true },
            { field: "MeetingStatus", title: "Meeting Status", width: "50px", sortable: true }
        ],
        editable: false,
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
    }).data("kendoGrid");
}
function EditMeeting(e) {
    var tr = $(e.target).closest("tr");
    var grid = $("#Get_ProjectReviewMeetingGrid").data("kendoGrid");
    var data = grid.dataItem(tr);
    var MeetingId = data.MeetingId;
    window.location.href = 'ProjectsReview.aspx?Flag=Edit&MeetingId=' + MeetingId;
}
function ConductMeeting(e) {
    var tr = $(e.target).closest("tr");
    var grid = $("#Get_ProjectReviewMeetingGrid").data("kendoGrid");
    var data = grid.dataItem(tr);
    var MeetingId = data.MeetingId;
    window.open('ProjectsReview.aspx?Flag=Conduct&MeetingId=' + MeetingId);
    //window.location.href = 'ProjectsReview.aspx?Flag=Conduct&MeetingId=' + MeetingId;
}
function OnGridDataBound(e) {
    var grid = $("#Get_ProjectReviewMeetingGrid").data("kendoGrid");
    var gridData = grid.dataSource.view();
    for (var i = 0; i < gridData.length; i++) {
        var currentUid = gridData[i].uid;
        if (gridData[i].MeetingStatus == "Cancelled" || gridData[i].MeetingStatus == "Completed") {
            var currentRow = grid.table.find("tr[data-uid='" + currentUid + "']");
            var editButton = $(currentRow).find(".k-grid-edit");
            editButton.hide();
            var editButton = $(currentRow).find(".k-grid-Conduct");
            editButton.hide();
        }
    }
}
function AddNewMeeting(){
    //window.open('ProjectsReview.aspx?Flag=New&MeetingId=0');
    window.location.href = 'ProjectsReview.aspx?Flag=New&MeetingId=0';
}







