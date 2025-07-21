$(document).ready(function () {

    GetConfigDetails();
  
});

function GetConfigDetails() {
    $.ajax({
        type: "POST",
        url: "Config.aspx/BindConfig",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetConfigData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}



function GetConfigData(Tdata) {
    $(gridConfig).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        configID: { type: "number" },
                        category: { type: "string" },
                        Name: { type: "string" },
                        comment: { type: "string" },
                        value: { type: "string" },
                        value1: { type: "string" },
                        modifiedOn: { type: "date" },
                        modifiedBy: { type: "string" },

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
        columns: [
                    { field: "configID", title: "ID", width: "50px" },
                    { field: "category", title: "Category", width: "80px"
                    //, hidden: true
                    },
                    { field: "name", title: "Name", width: "150px" },
                    { field: "comment", title: "Comment", width: "150px" },
                    { field: "value", title: "Value", width: "150px", hidden: true },
                    { field: "value1", title: "Value", width: "150px", hidden: true },
                    { field: "modifiedOn", format: "{0:dd-MMM-yyyy}", title: "Modified On", width: "100px" },
                    { field: "modifiedBy", title: "Modified By", width: "80px" },
                    { command: [{ name: "edit" }], width: "60px" }],

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
                var configID = e.model.configID;
                $('[id$="txtEditConfigID"]').val(configID);

                var category = e.model.category;

                $('[id$="txtEditCategory"]').val(category);

                var name = e.model.name;
                $('[id$="txtEditName"]').val(name);
                $('[id$="txtEditValue"]').val((e.model.value));
                $('[id$="txtEditValue1"]').val((e.model.value1));
                //$('[id$="txtEditValue"]').val(htmlDecode(e.model.value));
                //$('[id$="txtEditValue1"]').val(htmlDecode(e.model.value1));
              //  var value = $('[id$="txtEditValue"]');
              //  value.kendoEditor({ value: e.model.value });
              //  var editor = value.data("kendoEditor");
              //  var value1 = $('[id$="txtEditValue1"]');
              //  value1.kendoEditor({ value: e.model.value1 });
              //  var editor1 = value1.data("kendoEditor");
                var comment = e.model.comment;
                $('[id$="txtEditComment"]').val(comment);
                e.container.data("kendoWindow").title('Config Details');
                var width = $("#trConfig").width();
                $(".k-edit-form-container").parent().width(width + 20).data("kendoWindow").center();
                updateButton = e.container.find(".k-grid-update");
                cancelbutton = e.container.find(".k-grid-cancel");
                $("#tdUpdate").append(updateButton);
                $("#tdUpdate").append(cancelbutton);
            }
            else {
                e.container.data("kendoWindow").title('Config Details');
            }
        },
        save: function (e) {
            if (e.model.isNew()) {
                var configEditID = $('[id$="txtEditConfigID"]').val();
                var categoryEdit = $('[id$="txtEditCategory"]').val();
                var nameEdit = $('[id$="txtEditName"]').val();
                var valueEdit = $('[id$="txtEditValue"]').val();
                var valueEdit1 = $('[id$="txtEditValue1"]').val();
                var commentEdit = $('[id$="txtEditComment"]').val();

                UpdateConfig(configEditID, categoryEdit, nameEdit, valueEdit, valueEdit1, commentEdit);
                RedirectPage();
            }
            else {
                id = "0";

            }
        }

    });

}
function htmlDecode(value) {
    return $('<div/>').html(value).text();
}

function FillTextEdit() {
    var target = $("#txtEditValue").kendoEditor({
        tools: [
            "bold",
            "italic",
            "underline",
            "strikethrough",
            "justifyLeft",
            "justifyCenter",
            "justifyRight",
            "justifyFull"
        ]
    }).data("kendoEditor");

}


function UpdateConfig(configEditID, categoryEdit, nameEdit, valueEdit, valueEdit1, commentEdit) {

    $.ajax(
           {

               type: "POST",
               url: "Config.aspx/UpdateConfig",
               contentType: "application/json;charset=utf-8",
               data: "{'configEditID':'" + configEditID + "','categoryEdit':'" + categoryEdit + "','nameEdit':'" + nameEdit + "','valueEdit':'" + valueEdit + "','valueEdit1':'" + valueEdit1 + "','commentEdit':'" + commentEdit + "'}",
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

function ClosingRateWindow(e) {

    var grid = $("#gridConfig").data("kendoGrid");
    grid.refresh();

}

function RedirectPage() {
    window.location.assign("Config.aspx");
}
function openAddPopUP() {
    FillDefaultID();
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');


}
function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    window.location.reload();
}

function closeTeamPopUP() {
    //$('#divTeamMembers').html('');
    // $('#txtDevelopmentTeam').html('');
    $('#divProposal').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}

function ShowAddPopup() {
    openAddPopUP();
    $("#txtName").val('');
    $("#txtComment").val('');
    $("#lblerrmsgName").html("");
    $("#lblerrmsgDesc").html("");
}

function FillDefaultID() {

    $.ajax(
          {
              type: "POST",
              async: true,
              url: "Config.aspx/BindDefaultID",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              dataType: "json",
              success: function (msg) {
                  var value = JSON.parse(msg.d)

                  $('[id$="txtConfigID"]').val((JSON.parse(msg.d)[0]['configID']) + 1);

              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }

          }

    );
}