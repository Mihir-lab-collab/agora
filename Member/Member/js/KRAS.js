$(document).ready(function () {

    GetKRADetails();

});

function ClosingRateWindow(e) {

    var grid = $(gridKRAS).data("kendoGrid");
    grid.refresh();

}

function GetKRADetails() {
    $.ajax({
        type: "POST",
        url: "KRA.aspx/BindKRA",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetKRAData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetKRAData(Tdata) {
    $(gridKRAS).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        KRAID: { type: "number" },
                        KRANames: { type: "string" },

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
                    { field: "KRAID", title: "ID", width: "50px", hidden: true },
                    { field: "KRANames", title: "Names", width: "80px" },
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
                var KRAID = e.model.KRAID;
                $('[id$="hdnKRAID"]').val(KRAID);

                var KRANames = e.model.KRANames;

                $('[id$="txtEditKRA"]').val(KRANames);

                e.container.data("kendoWindow").title('KRA Details');
                var width = $("#trConfig").width();
                $(".k-edit-form-container").parent().width(width + 20).data("kendoWindow").center();
                updateButton = e.container.find(".k-grid-update");
                cancelbutton = e.container.find(".k-grid-cancel");
                $("#tdUpdate").append(updateButton);
                $("#tdUpdate").append(cancelbutton);
            }
            else {
                e.container.data("kendoWindow").title('KRA Details');
            }
        },
        save: function (e) {
            if (e.model.isNew()) {
                var KRAEditID = $('[id$="hdnKRAID"]').val();
                var KRAEdit = $('[id$="txtEditKRA"]').val();
                var verror = $("#lblError");

                if (KRAEdit == '') {
                }
                else {
                    UpdateKRA(KRAEditID, KRAEdit);
                    RedirectPage();
                }
            }
            else {
                id = "0";

            }
        }

    });

}

function UpdateKRA(KRAEditID, KRAEdit) {

    $.ajax(
           {

               type: "POST",
               url: "KRA.aspx/UpdateKRA",
               contentType: "application/json;charset=utf-8",
               data: "{'KRAID':'" + KRAEditID + "','KRANames':'" + KRAEdit + "'}",
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
    window.location.assign("KRA.aspx");
}
