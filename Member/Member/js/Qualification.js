$(document).ready(function () {

    GetQualDetails();

});

function GetQualDetails() {
    $.ajax({
        type: "POST",
        url: "Qualification.aspx/BindQualification",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetQualData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetQualData(Tdata) {
    $(gridQualification).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        QID: { type: "number" },
                        QualDesc: { type: "string", },
                        QualType: { type: "string" },
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
                    { field: "QualDesc", title: "Qualification Description", width: "80px" },
                    { field: "QualType", title: "Type", width: "150px" },
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
                var QID = e.model.QID;
                InitialiseQualificationTypeEdit();
                $('[id$="txtEditQualDesp"]').val(e.model.QualDesc);
                var dropdownlist = $("#drpQualTypeedit").data("kendoDropDownList");
                dropdownlist.value(e.model.QualType);
               
                e.container.data("kendoWindow").title('Qualification Details');
                var width = $("#trQual").width();
                $(".k-edit-form-container").parent().width(width + 20).data("kendoWindow").center();
                updateButton = e.container.find(".k-grid-update");
                cancelbutton = e.container.find(".k-grid-cancel");
                $("#tdUpdate").append(updateButton);
                $("#tdUpdate").append(cancelbutton);
            }
            else {
                e.container.data("kendoWindow").title('Qualification Details');
            }
        },
        save: function (e) {
            if (e.model.isNew()) {
                var Qualid = e.model.QID;
                var QualDesc = $('[id$="txtEditQualDesp"]').val();
                var QualType = $("#drpQualTypeedit").val();
                UpdateQualification(Qualid, QualDesc, QualType);
                RedirectPage();
            }
            else {
                id = "0";
            }
        }
    });
}

function UpdateQualification(Qualid, QualDesc, QualType) {

    $.ajax(
           {
               type: "POST",
               url: "Qualification.aspx/UpdateQualification",
               contentType: "application/json;charset=utf-8",
               data: "{'QualId':'" + Qualid + "','QualDesc':'" + QualDesc + "','QualType':'" + QualType + "'}",
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
    var grid = $("#gridQualification").data("kendoGrid");
    grid.refresh();
}
function RedirectPage() {
    window.location.assign("Qualification.aspx");
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
    InitialiseQualificationType();
    var drpQualType = $("#drpQualType").data("kendoDropDownList");
    drpQualType.text(drpQualType.options.optionLabel);
    drpQualType.element.val("");
    drpQualType.selectedIndex = -1

    $("#txtQualDesc").val('');
    $("#lblerrmsgQualDesc").html("");
    $("#lblmsgQualType").html("");
}
function InitialiseQualificationType() {
    var Type = $("#drpQualType").kendoDropDownList({
        optionLabel: "Select Type",
        dataSource: [
            { IsType: "Under Graduation", IsTypeID: "Under Graduation" },
            { IsType: "Graduation", IsTypeID: "Graduation" },
            { IsType: "Post Graduation", IsTypeID: "Post Graduation" }
        ],
        dataTextField: "IsType",
        dataValueField: "IsTypeID",
    }).data("kendoDropDownList");
}
function InitialiseQualificationTypeEdit() {
    var Type = $("#drpQualTypeedit").kendoDropDownList({
        dataSource: [
            { IsType: "Under Graduation", IsTypeID: "Under Graduation" },
            { IsType: "Graduation", IsTypeID: "Graduation" },
            { IsType: "Post Graduation", IsTypeID: "Post Graduation" }
        ],
        dataTextField: "IsType",
        dataValueField: "IsTypeID",
    }).data("kendoDropDownList");
}
