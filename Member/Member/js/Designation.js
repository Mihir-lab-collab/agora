$(document).ready(function () {
   
    GetDesignationDetails();

});

function ClosingRateWindow(e) {

    var grid = $(gridDesignation).data("kendoGrid");
    grid.refresh();

}

function GetDesignationDetails() {
    $.ajax({
        type: "POST",
        url: "Designation.aspx/BindDesignation",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetDesignationData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetDesignationData(Tdata) {
    $(gridDesignation).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        DesigID: { type: "number" },
                        Designation: { type: "string"},

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
                    { field: "DesigID", title: "ID", width: "50px", hidden: true },
                    { field: "Designation", title: "Designation", width: "80px" },
                    { command: [{ name: "edit" }], width: "10px" }
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
        editable: true,
        editable: {
            mode: "popup",
            template: kendo.template($("#popup-editor").html())
        },
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
        edit: function (e) {
            var editWindow = e.container.data("kendoWindow");
            $('#divEdit').show();

            if (e.model.isNew()) {
                // FillTextEdit();
                var DesigID = e.model.DesigID;
                $('[id$="hdnDesigID"]').val(DesigID);

                var Designation = e.model.Designation;

                $('[id$="txtEditDesignation"]').val(Designation);

                e.container.data("kendoWindow").title('Designation Details');
                var width = $("#trConfig").width();
                $(".k-edit-form-container").parent().width(width + 20).data("kendoWindow").center();
                updateButton = e.container.find(".k-grid-update");
                cancelbutton = e.container.find(".k-grid-cancel");
                $("#tdUpdate").append(updateButton);
                $("#tdUpdate").append(cancelbutton);
            }
            else {
                e.container.data("kendoWindow").title('Designation Details');
            }
        },
        save: function (e) {
            if (e.model.isNew()) {
                var DesigEditID = $('[id$="hdnDesigID"]').val();
                var DesigEdit = $('[id$="txtEditDesignation"]').val();
                var verror = $("#lblError");

                if (DesigEdit == '') {
                    //verror.html("Designation cannot be blank.");
                    //alert("Designation cannot be blank.");
                    //return false;
                }
                else {
                    //verror.html("");
                    UpdateDesignation(DesigEditID, DesigEdit);
                    RedirectPage();
                }
            }
            else {
                id = "0";

            }
        }

    });

}

function UpdateDesignation(DesigEditID, DesigEdit) {

    $.ajax(
           {

               type: "POST",
               url: "Designation.aspx/UpdateDesignation",
               contentType: "application/json;charset=utf-8",
               data: "{'desigID':'" + DesigEditID + "','designation':'" + DesigEdit + "'}",
               cache: false,
               async: false,
               dataType: "json",
               success: function (msg) {
                   var message = jQuery.parseJSON(msg.d);
                   alert('updated successfully.');
                   $('body').css('cursor', 'default');

               },
               error: function (msg) {
                   alert("The call to the server side failed."
                         + msg.responseText);
               }
           }
        );
}

function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    window.location.reload();
}

function ShowAddPopup() {
    openAddPopUP();
    $("#txtEditConfigID").val('');
}
function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}

function RedirectPage() {
    window.location.assign("Designation.aspx");
}
