$(document).ready(function () {

    GetModuleDetails();

});

function GetModuleDetails() {
    var ddlType = $("[id$=ddlType]").val();
    $.ajax({
        type: "POST",
        url: "Module.aspx/BindModule",
        contentType: "application/json;charset=utf-8",
        data: "{'Type':'" + ddlType + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetModuleData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetModuleData(Tdata) {
    $(gridModule).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        ModuleID: { type: "number" },
                        ModuleID_Parent: { type: "number" },
                        ModuleID_ParentName: { type: "string", },
                        Menu: { type: "string" },
                        Name: { type: "string" },
                        EntryPage: { type: "string" },
                        Parameter: { type: "string" },
                        IsMenuVisible: { type: "string" },
                        IsGenric: { type: "string" },
                        SortOrder: { type: "string" },
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
                    { field: "ModuleID_ParentName", title: "Parent Name", width: "150px" },
                    { field: "Menu", title: "Menu Name", width: "150px" },
                    { field: "Name", title: "Display Name", width: "150px" },
                    { field: "EntryPage", title: "Page URL", width: "245px" },
                    { field: "Parameter", title: "Parameter", hidden: true },
                    { field: "IsMenuVisible", title: "Is Visible", width: "52px" },
                    { field: "IsGenric", title: "Is Generic", width: "60px" },
                    { field: "SortOrder", title: "Sort Order", width: "65px" },
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
            GetDataEdit();
            var editWindow = e.container.data("kendoWindow");
            $('#divEdit').show();

            if (e.model.isNew()) {
                var modulID = e.model.ModuleID;

                $('[id$="txtEditMenuName"]').val(e.model.Menu);
                $('[id$="txtEditDisplayName"]').val(e.model.Name);
                $('[id$="txtEditPageURL"]').val(e.model.EntryPage);
                $('[id$="txtEdiparam"]').val(e.model.Parameter);

                $('#chkIsVisibleEdit').prop('checked', '');
                if (e.model.IsMenuVisible == "true") {
                    $('#chkIsVisibleEdit').prop('checked', 'checked');
                }
                $('#chkIsGenericEdit').prop('checked', '');
                if (e.model.IsGenric == "true") {
                    $('#chkIsGenericEdit').prop('checked', 'checked');
                }
                var dropdownlist = $("#drpEditParentName").data("kendoDropDownList");
                dropdownlist.value(e.model.ModuleID_Parent);

                $('[id$="txtEditSortOrder"]').val(e.model.SortOrder);

                e.container.data("kendoWindow").title('Module Details');
                var width = $("#trModule").width();
                $(".k-edit-form-container").parent().width(width + 20).data("kendoWindow").center();
                updateButton = e.container.find(".k-grid-update");
                cancelbutton = e.container.find(".k-grid-cancel");
                $("#tdUpdate").append(updateButton);
                $("#tdUpdate").append(cancelbutton);
            }
            else {
                e.container.data("kendoWindow").title('Module Details');
            }
        },
        save: function (e) {
            if (e.model.isNew()) {

                var ModuleID = e.model.ModuleID;
                var ModuleParentID = $("#drpEditParentName").val();
                var DisplayName = $("#txtEditDisplayName").val();
                var MenuName = $('[id$="txtEditMenuName"]').val();
                var PageURL = $('[id$="txtEditPageURL"]').val();
                var Parameter = $('[id$="txtEdiparam"]').val();
                var IsVisible = $('#chkIsVisibleEdit').is(':checked');
                var IsGeneric = $('#chkIsGenericEdit').is(':checked');
                var SortOrder = $("#txtEditSortOrder").val();
                var type = $("[id$=ddlType]").val();
                UpdateModule(ModuleID, ModuleParentID, DisplayName, MenuName, PageURL, Parameter, IsVisible, IsGeneric, SortOrder, type);
                RedirectPage();
            }
            else {
                id = "0";
            }
        }

    });
}

function UpdateModule(ModuleID, ModuleParentID, DisplayName, MenuName, PageURL, Parameter, IsVisible, IsGeneric, SortOrder, type) { 
    $.ajax(
           {
               type: "POST",
               url: "Module.aspx/UpdateModule",
               contentType: "application/json;charset=utf-8",
               data: "{'ModuleID':'" + ModuleID + "','ModuleParentID':'" + ModuleParentID + "','DisplayName':'" + DisplayName + "','MenuName':'" + MenuName + "','PageURL':'" + PageURL + "','Parameter':'" + Parameter + "','IsVisible':'" + IsVisible + "','IsGeneric':'" + IsGeneric + "','SortOrder':'" + SortOrder + "','type':'" + type + "'}",
               cache: false,
               async: false,
               dataType: "json",
               success: function (msg) {
                   var message = jQuery.parseJSON(msg.d);
                   alert(msg.d);
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
    var grid = $("#gridModule").data("kendoGrid");
    grid.refresh();
}
function RedirectPage() {
    window.location.assign("Module.aspx");
}
function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}
function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    // window.location.reload();
}

function closeTeamPopUP() {
    $('#divProposal').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}

function ShowAddPopup() {
    openAddPopUP();
    GetData();
    InitialiseParentName();
    var drpParentName = $("#drpParentName").data("kendoDropDownList");
    drpParentName.text(drpParentName.options.optionLabel);
    drpParentName.element.val("");
    drpParentName.selectedIndex = -1

    $("#txtDisplayName").val('');
    $("#txtMenuName").val('');
    $("#txtPageURL").val('');
    $("#txtSortOrder").val('');
    $('#chkIsVisibleEdit').prop('checked', '');
    $('#chkIsGenericEdit').prop('checked', '');

    $("#lblmsgParentName").html("");
    $("#lblerrmsgMenuName").html("");
    $("#lblmsgDisplayName").html("");
    $("#lblerrmsgPageURL").html("");
}
function InitialiseParentName(ModuleData) {
    var Type = $("#drpParentName").kendoDropDownList({
        optionLabel: {
            ModuleID_ParentName: "Select Parent",
            ModuleID_Parent: "0"
        },
        dataTextField: "ModuleID_ParentName",
        dataValueField: "ModuleID_Parent",
        dataSource: ModuleData,
    }).data("kendoDropDownList");
}
function InitialiseParentNameEdit(ModuleDataEdit) {
    var Type = $("#drpEditParentName").kendoDropDownList({
        dataTextField: "ModuleID_ParentName",
        dataValueField: "ModuleID_Parent",
        dataSource: ModuleDataEdit,
        optionLabel: {
            ModuleID_ParentName: "Select Parent",
            ModuleID_Parent: "0"
        }
    }).data("kendoDropDownList");
}
function GetData() {
    $.ajax(
          {
              type: "POST",
              url: "Module.aspx/FillParentModule",
              contentType: "application/json;charset=utf-8",
              dataType: "json",
              success: function (msg) {
                  InitialiseParentName(jQuery.parseJSON(msg.d));
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
);
}
function GetDataEdit() {
    $.ajax(
          {
              type: "POST",
              url: "Module.aspx/FillParentModule",
              contentType: "application/json;charset=utf-8",
              dataType: "json",
              async: false,
              success: function (msg) {
                  InitialiseParentNameEdit(jQuery.parseJSON(msg.d));
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
);
}

