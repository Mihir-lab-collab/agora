$(document).ready(function () {
   $("#ddlHolidayDt").kendoDatePicker({ format: "dd/MM/yyyy" });//Need to add by Trupti
    $('[id$="ddlHolidayDt1"]').hide();
    GetHolidayWorkingDetails();

});

function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    ClearData();
    window.location.reload();
}

function ShowAddPopup() {
    if ($('[id$="DDProjects"]').val() == 0)
    {
        alert("Please Select Project First.");
        return;
    }
    openAddPopUP();
    //bindHolidayDate();// Need to comment by Trupti
    bindHours();

}
function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}
function ClearData()
{
    $('[id$="ddlHolidayDt"]').val('');
    $('[id$="ddlHours"]').val('');
    $('[id$="txtReason"]').val('');

}


function GetHolidayWorkingDetails() {

    $.ajax({
        type: "POST",
        url: "HolidayWorking.aspx/BindHolidayWorking",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        dataType: "json",
        async: false,
        success: function (msg) {

            GetHolidayworkingData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function ClosingRateWindow(e) {

    var grid = $(gridHolidayWorking).data("kendoGrid");
    grid.refresh();

}

function GetHolidayworkingData(HolidyaData) {

    $(gridHolidayWorking).kendoGrid({
        dataSource: {
            data: HolidyaData,
            schema: {
                model: {
                    fields: {
                        Id: { type: "number" },
                        EmpId: { type: "number" },
                        HolidayDate: { type: "date" },
                        ProjId: { type: "number" },
                        ExpectedHours: { type: "number" },
                        UserReason: { type: "string" },
                        ProjectName: { type: "string" },
                        UserEntryDate: { type: "date" },
                        Status: { type: "int" },
                        AdminComment: { type: "string" },
                        AdminCanReason: { type: "string" },
                        Statusflag: { type: "string" }
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
                    { field: "Id", title: "ID", width: "10px", hidden: true },
                    { field: "EmpId", title: "EmpId", width: "10px", hidden: true },
                    { field: "HolidayDate", title: "Holiday Date", width: "30px", format: "{0:dd-MMM-yyyy}", },
                    { field: "ProjectName", title: "Project Name", width: "50px" },
                    { field: "ExpectedHours", title: "Expected Hours", width: "30px" },
                    { field: "UserReason", title: "Reason", width: "50px" },
                    { field: "UserEntryDate", title: "Entry Date", width: "50px", format: "{0:dd-MMM-yyyy hh:mm tt}", },
                    { field: "Statusflag", title: "Status", width: "30px" },
                    { field: "AdminComment", title: "Admin Comment", width: "50px" },
                    { field: "AdminCanReason", title: "Admin Cancel Comment", width: "50px" },
                    { command: [{ name: "edit", text: "Edit" }, { name: "destroy", text: "Cancel" }], width: "50px" }

        ],
        dataBound: function () {
            var grid = this;
            var model;

            grid.tbody.find("tr[role='row']").each(function () {
                model = grid.dataItem(this);

                if (model.HolidayDate != null || model.HolidayDate != "") {
                    var sel = $('[id$="ddlHolidayDt1"]');
                    var exists = false;
                    for (x = 0 ; x <= sel[0].options.length - 1 ; x++) { 
                        if (sel[0].options[x].value != -1) {   
                            if ((sel[0].options[x].value == kendo.toString(kendo.parseDate(model.HolidayDate, 'yyyy-MM-dd'), 'dd-MMM-yy'))) {
                                exists = true;
                                break;
                            }
                        }
                    }
                    if (!exists) {
                        $(this).find(".k-grid-edit").prop("disabled", true).addClass("k-state-disabled");
                        $(this).find(".k-grid-delete").prop("disabled", true).addClass("k-state-disabled");
                    }
                }
            });

            grid.tbody.find("tr[role='row']").each(function () {
                model = grid.dataItem(this);
                if (model.Status != 0) {
                    $(this).find(".k-grid-edit").prop("disabled", true).addClass("k-state-disabled");
                    $(this).find(".k-grid-delete").prop("disabled", true).addClass("k-state-disabled");
                }
                if (model.Status == 3) {
                    $(this).find(".k-grid-edit").remove();
                    $(this).find(".k-grid-delete").remove();
                }
            });
        },
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
        editable: false,
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
            
            //$("#ddlEditHolidayDt").kendoDatePicker({ format: "dd/MM/yyyy" }); //added  by trupti as on 21 May 2019
            if (e.model.isNew()) {
                var HolidayWorkingId = e.model.Id
                $('[id$="hdnHolidayWorkingID"]').val(HolidayWorkingId);

                var HolidayDate = e.model.HolidayDate;
                var ExpectedHours = e.model.ExpectedHours;
                var UserReason = e.model.UserReason;
                //bindHolidayDate();//Need to Comment By trupti
                bindHours();

                var dropdownlistDt = $("#ddlEditHolidayDt").data("kendoDropDownList");
                HolidayDate= kendo.toString(kendo.parseDate(HolidayDate, 'yyyy-MM-dd'), 'dd/MM/yyyy')
                //dropdownlistDt.value(HolidayDate);
                $("#ddlEditHolidayDt").val(HolidayDate);
                var dropdownlistHrs = $("#ddlEditHours").data("kendoDropDownList");
                dropdownlistHrs.value(ExpectedHours);

                e.container.data("kendoWindow").title('Holiday Working Details');
                var width = $("#trConfig").width();
                $(".k-edit-form-container").parent().width(width + 20).data("kendoWindow").center();
                updateButton = e.container.find(".k-grid-update");
                cancelbutton = e.container.find(".k-grid-cancel");
                $("#tdUpdate").append(updateButton);
                $("#tdUpdate").append(cancelbutton);
            }
            else {
                e.container.data("kendoWindow").title('Holiday Working Details');
            }
        },
        save: function (e) {
            if (e.model.isNew()) {
                var HolidayWEditID = $('[id$="hdnHolidayWorkingID"]').val();
                var ProjectIdEdit = $('[id$="DDProjects"]').val();
                var HolidayWDateEdit = $('[id$="ddlEditHolidayDt"]').val();
                var ExpHoursEdit = $('[id$="ddlEditHours"]').val();
                var ReasonEdit = $('[id$="txtEditReason"]').val();
                var EmpId = $('[id$="hdnEmpID"]').val();
                var verror = $("#lblError");

                if (HolidayWEditID == '') { }
                else {
                    
                    UpdateHolidayWorking(HolidayWEditID, EmpId,e.model.ProjId, HolidayWDateEdit, ExpHoursEdit, ReasonEdit);
                    RedirectPage();
                }
            }
            else {
                id = "0";

            }
        },
        remove: function (e) {
                e.preventDefault();
                CancelHolidayWorking(e.model.EmpId, kendo.toString(kendo.parseDate(e.model.HolidayDate, 'yyyy-MM-dd'), 'dd-MMM-yy'), e.model.ProjId);
                GetHolidayWorkingDetails();
                ClosingRateWindow(e);

        },
    });
}

function InitialiseHolidayDt(techId) {
    var bindHolidaydt = $("#ddlHolidayDt").kendoDropDownList({
        optionLabel: "---Select----",
        dataSource: techId,
        dataTextField: "HolidayDate",
        dataValueField: "HolidayDate",

    }).data("kendoDropDownList");


    var bindHolidaydt = $("#ddlEditHolidayDt").kendoDropDownList({
        optionLabel: "---Select----",
        dataSource: techId,
        dataTextField: "HolidayDate",
        dataValueField: "HolidayDate",
    }).data("kendoDropDownList");

}
function InitialiseExpHours(Id) {
    var tech = $("#ddlHours").kendoDropDownList({
        optionLabel: "---Select----",
        dataSource: Id,
        dataTextField: "d",
        dataValueField: "d",

    }).data("kendoDropDownList");


    var tech = $("#ddlEditHours").kendoDropDownList({
        optionLabel: "---Select----",
        dataSource: Id,
        dataTextField: "d",
        dataValueField: "d",
    }).data("kendoDropDownList");

}
function bindHours() {
    $.ajax(
          {
              type: "POST",
              url: "HolidayWorking.aspx/BindExpHours",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              cache: false,
              async: false,
              dataType: "json",
              success: function (msg) {
                  InitialiseExpHours(jQuery.parseJSON(msg.d));
              },
              error: function (msg) {
                  alert("The call to the server side failed."
                        + msg.responseText);
              }
          }
       );

}

function bindHolidayDate() {
    $.ajax(
          {
              type: "POST",
              url: "HolidayWorking.aspx/BindHolidayDate",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              cache: false,
              async: false,
              dataType: "json",
              success: function (msg) {
                  InitialiseHolidayDt(jQuery.parseJSON(msg.d));
              },
              error: function (msg) {
                  alert("The call to the server side failed."
                        + msg.responseText);
              }
          }
       );

}
function UpdateHolidayWorking(HolidayWEditID, EmpId, ProjectIdEdit, HolidayWDateEdit, ExpHoursEdit, ReasonEdit) {

    $.ajax(
           {

               type: "POST",
               url: "HolidayWorking.aspx/UpdateHolidayWorking",
               contentType: "application/json;charset=utf-8",
               data: "{'HolidayWorkingId':'" + HolidayWEditID + "','EmpID':'" + EmpId + "','ProjectId':'" + ProjectIdEdit + "','holidayWdate':'" + HolidayWDateEdit + "','exphours':'" + ExpHoursEdit + "','reason':'" + ReasonEdit + "'}",
               cache: false,
               async: false,
               dataType: "json",
               success: function (msg) {
                   var message = jQuery.parseJSON(msg.d);
                   if (message == "") {
                       alert('Updated successfully.');
                       $('body').css('cursor', 'default');
                   }
                   else
                       alert(message);
               },
               error: function (msg) {
                   alert("The call to the server side failed."
                         + msg.responseText);
               }
           }
        );
}


function CancelHolidayWorking(EmpID, HolidayDate, ProjId) {
    $.ajax(
           {

               type: "POST",
               url: "HolidayWorking.aspx/CancelHolidayWorking",
               contentType: "application/json;charset=utf-8",
               data: "{'EmpID':'" + EmpID + "','HolidayDate':'" + HolidayDate + "','ProjId':'" + ProjId + "'}",
               cache: false,
               async: false,
               dataType: "json",
               success: function (msg) {
                   var message = jQuery.parseJSON(msg.d);
                   //alert('Holiday working Cancelled.');
                   $('body').css('cursor', 'default');
               },
               error: function (msg) {
                   alert("The call to the server side failed."
                          + msg.responseText);
               }
           }
        );
}
function RedirectPage() {
    window.location.assign("HolidayWorking.aspx");
}