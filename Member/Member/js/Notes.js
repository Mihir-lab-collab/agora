
function GetNotes(fromDate, toDate, noteTypeId, refID, reference) {
    $.ajax({
        type: "POST",
        url: "Notes.aspx/BindNotes",
        contentType: "application/json;charset=utf-8",
        data: "{fromDate:'" + fromDate + "',toDate:'" + toDate + "',noteTypeId:'" + noteTypeId + "',refID:'" + refID + "',reference:'" + reference + "'}",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.d.lst.length > 0)
            {
                $("#ctl00_ContentPlaceHolder1_txtTeference").val(data.d.Reference);
                $("#ctl00_ContentPlaceHolder1_txtFromDate").val(data.d.Fromdate);
                $("#ctl00_ContentPlaceHolder1_txtFromTo").val(data.d.Todate);
            }

            GetNotesData(data.d.lst);
           
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}


function GetNotesData(NoteData) {

    $("#gridNote").kendoGrid(
        {
            height: 500,
            dataSource: {
                data: NoteData,
                schema: {
                    model: {
                        fields: {
                            noteID: { type: "number" },
                            insertedOn: { type: "date", format: "{0:dd-MMM-yyyy}" },
                            reff: { type: "string" },
                            note: { type: "string" },
                            addedBy: { type: "string" }
                        }
                    }
                },
                pageSize: 25,
            },
            scrollable: true,
            sortable: true,
            selectable: true,
            pageable: {
                input: true,
                numeric: false
            },
            columns: [
                     { field: "noteID", title: "ID", width: 20 },
                     { field: "insertedOn", title: "Date", width: 30, template: "#= kendo.toString(kendo.parseDate(insertedOn, 'yyyy-MM-dd'), 'dd-MMM-yyyy') #" },
                     { field: "reff", title: "Reference", width: 50 },
                     { field: "note", title: "Note", width: 120 },
                     { field: "addedBy", title: "AddedBy", width: 50 }


            ],

            filterable:
               {
                   extra: false,
                   operators: {
                       string: {
                           startswith: "Starts with",
                           contains: "Contains",
                           eq: "Is equal to"
                       }
                   }
                   //ui:function(element){
                   //    element.kendoDatePicker({
                   //        format: "{0:dd-MMM-yyyy}"
                   //    });
                   //}
               },
            editable: false,

            cancel: function (e) {
                e.preventDefault()
                ClosingRateWindow(e);
            },
        });
    ///////////////////////////////////////////////// tooltip
    $(".k-grid-content").kendoTooltip({
        filter: "td[title]",
        content: toolTip,
        width: 160,
        height: 30,
        position: "top"
    });

    $(".k-grid-content").click(toolTip);

    function toolTip(e) {
        var target = $(e.target);
        var dataItem = $(gridEmployee).data("kendoGrid").dataItem(e.target.closest("tr"));

        return kendo.template($("#template").html())({
            ctcText: dataItem.CTC.toLocaleString(),
            GrossText: dataItem.Gross.toLocaleString()
        });
    }

    ///////////////////////////

    detailsTemplate = kendo.template($("#popup-editor").html());
    function showDetails(e) {
        e.preventDefault();
        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        wnd.content(detailsTemplate(dataItem));
        wnd.center().open();
    }
}



function ShowAddPopup() {
    openAddPopUP();
    clearFields();
}

function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}

function closeAddPopUP() {

    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    clearFields();

}

function clearFields() {
    $("#ctl00_ContentPlaceHolder1_ddlReference").val("0");
    $('[id$="txtNotes"]').val('');
}


function Clear() {
    $("#ctl00_ContentPlaceHolder1_txtTeference").val('');
    $("#ctl00_ContentPlaceHolder1_txtFromDate").val('');
    $("#ctl00_ContentPlaceHolder1_txtFromTo").val('');

    $.ajax({
        type: "POST",
        url: "Notes.aspx/Clear",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {

        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });

}



