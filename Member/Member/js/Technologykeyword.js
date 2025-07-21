$(document).ready(function () {

    GetTechnologyDetails('');

});
function GetTechnologyDetails(Techname) {
    $.ajax({
        type: "POST",
        url: "Technologykeyword.aspx/BindTechnologykeyword",
        contentType: "application/json;charset=utf-8",
        data: "{'techName':'" + Techname + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetAll(jQuery.parseJSON(msg.d));
            $('[id$=hdntechdata]').val(msg.d);
        }
    });

}




function GetAll(Tdata) {
    $("#gridtechnologykeyword").kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        techId: { type: "number" },
                        techName: { type: "string" },
                        subTechName: { type: "string" },
                    }
                }
            },
            pageSize: 20,
        },
        scrollable: true,
        sortable: true,
        editable: true,
        editable: {
            mode: "popup",
            template: kendo.template($("#popup-editor").html())
        },
     //   filterable: true,
      
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
        edit: function (e) {
            if (e.model.isNew()) {
            var techId = e.model.techId;
            var techName = e.model.techName;
            var subTechName = e.model.subTechName;

            $('[id$="hdnTechId"]').val(techId);

            e.container.data("kendoWindow").title('Technology Keyword');
            var width = $("#trtech").width();
            $(".k-edit-form-container").parent().width(width + 20).data("kendoWindow").center();
            updateButton = e.container.find(".k-grid-update");
            cancelbutton = e.container.find(".k-grid-cancel");
            $("#tdUpdate").append(updateButton);
            $("#tdUpdate").append(cancelbutton);

            }
            else {
                e.container.data("kendoWindow").title('Task Details');
            }


        },
        save: function (e) {
            if (e.model.isNew()) {
                var TechId = $('[id$="hdnTechId"]').val();

                var Techname = $('[id$="EdittechName"]').val();
               // var subtechname = $('[id$="EditsubTechName"]').val();
                var verror = $("#lblError");

                if (TechId != '') {
                    Updatetechnology(TechId, Techname, "");
                    RedirectPage();
                }
            }
            else {
                id = "0";

            }
        },
        remove: function (e) {
            var techId = e.model.techId;
            e.preventDefault();
            if (confirm("Are you sure you want to delete?")) {
                Deletetechnology(e.model.techId);

            } else {

                e.preventDefault();
                ClosingRateWindow(e);
            }
        },
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
        pageable: {
            input: true,
            numeric: false
        },

        columns: [
             { command: [{ name: "edit", text: "" }, { name: "destroy", text: "" }], width: "10px",title : "Action" },
            // { field: "techId", title: "techId", width: "100px", hidden: true },
             { field: "techName", title: "Technology", width: "100px"},
             { field: "subTechName", title: "SubTechnology", width: "100px",hidden:true },
        ],
    });
}
function Deletetechnology(techId) {
    $.ajax(
          {

              type: "POST",
              url: "Technologykeyword.aspx/DeleteTechnologyId",
              contentType: "application/json;charset=utf-8",
              data: "{'techId':'" + techId + "'}",
              cache: false,
              async: false,
              dataType: "json",
              success: function (msg) {
                  var message = jQuery.parseJSON(msg.d);
                  alert('deleted successfully.');
                  $('body').css('cursor', 'default');
              },
              error: function (msg) {
                  alert("The call to the server side failed."
                         + msg.responseText);
              }
          }
       );

}

function Updatetechnology(techId, Techname, subtechname) {
    $.ajax(
           {

               type: "POST",
               url: "Technologykeyword.aspx/UpdateTechnologyId",
               contentType: "application/json;charset=utf-8",
               data: "{'techId':'" + techId + "','techName':'" + Techname + "','subTechName':'" + subtechname + "'}",
               cache: false,
               async: false,
               dataType: "json",
               success: function (msg) {
                   var message = jQuery.parseJSON(msg.d);
                   alert('updated successfully.');
                   $('body').css('cursor', 'default');
                   closeAddPopUP();

               },
               error: function (msg) {
                   alert("The call to the server side failed."
                         + msg.responseText);
               }
           }
        );
}

function onSuccess(e) {
    var currentInitialFiles = JSON.parse(sessionStorage.initialFiles);
    for (var i = 0; i < e.files.length; i++) {
        var current = {
            name: e.files[i].name,
            extension: e.files[i].extension,
            size: e.files[i].size
        }

        if (e.operation == "upload") {
            currentInitialFiles.push(current);
        } else {
            var indexOfFile = currentInitialFiles.indexOf(current);
            currentInitialFiles.splice(indexOfFile, 1);
        }
    }
    sessionStorage.initialFiles = JSON.stringify(currentInitialFiles);
}

function RedirectPage() {
    window.location.assign("Technologykeyword.aspx");
}
function isChar(evt, field) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    return ((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122) || charCode == 8 || charCode == 9 || charCode == 32 || (charCode >= 48 && charCode <= 57)) ? true : false;
}
function openAddPopUP() {

    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}
function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}
function ShowAddPopup() {

    openAddPopUP();
    $("#txttechName").val('');
    $("#txtsubTechName").val('');
    $(".k-widget.k-upload").find("ul").remove();
    $(".k-widget.k-upload").find(".k-button.k-upload-selected").remove(); 
}

function ClosingRateWindow(e) {

    var grid = $("#gridtechnologykeyword").data("kendoGrid");
    grid.refresh();
}

