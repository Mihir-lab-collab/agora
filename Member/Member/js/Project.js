
$(document).ready(function () {
    $('[id$="txtEmail"]').kendoEditor();
    if ($('[id$="hdnshowHidediv"]').val() == "true")
    {
        $('[id$="divMail"]').css("display", "block");
        $('#divAddPopupOverlay').addClass('k-overlay');
    }
 
    var validator = $("#tickets").kendoValidator().data("kendoValidator"),
           status = $(".status");
    FillAppraisalAuthorityDropDown();
    FillDevelopmentTeam();
    BindStatusDropDown();
    SetDefualtProjectData();
    BindAllDateCalender();

    $('[id$="chkEmail"]').change(function () {
        if ($('[id$="chkEmail"]').is(':checked')) {
            var CustID = $("#txtCustomer").val();
            var EmployeeId = $("#txtAccountMgr").val();
            if (CustID != "" && EmployeeId != "") {
                $('[id$="hdnSendMail"]').val("true");
            }
            else {
                if (CustID == "") {
                    alert("Please select customer");
                    $('[id$="chkEmail"]').attr('checked', false);
                }
                if (EmployeeId == "") {
                    alert("Please select Account Manager");
                    $('[id$="chkEmail"]').attr('checked', false);
                }

            }
        }
        else {
            $('[id$="hdnSendMail"]').val("false");

          }
    });
});

function CloseMailBox() {
    $('[id$="divMail"]').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}

function numericInput(ob) {
    var invalidChars = /[^0-9]/gi
    if (invalidChars.test(ob.value)) {
        ob.value = ob.value.replace(invalidChars, "");
    }
}

function dateInput(ob) {
    var invalidChars = /[^]/gi
    if (invalidChars.test(ob.value)) {
        ob.value = ob.value.replace(invalidChars, "");
    }
}

function isChar(evt, field) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    return ((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122) || charCode == 8 || charCode == 9 || charCode == 32 || (charCode >= 48 && charCode <= 57)) ? true : false;
}

var ProjectDurationType = [

    { text: "Week", value: "0.25" },
    { text: "Month", value: "1" },
    { text: "Year", value: "12" },
];

function GetPagesize() {
    $("#comboBox").width(70).kendoComboBox({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: [
            { text: 50 },
            { text: 100 },
            { text: 200 },
            { text: 300 },
            { text: 500 }
        ],
        change: function (e) {
            var grid = $("#gridProject").data("kendoGrid");
            grid.dataSource.pageSize(this.value());
        }
    });
}     


function GetProjectData(CustId, isChecked, status) {

    $.ajax(
      {
          type: "POST",
          url: "Project.aspx/ProjectList",
          contentType: "application/json;charset=utf-8",
          data: "{CustId:'" + CustId + "','isChecked':'" + isChecked + "','status':'" + status + "'}",
          dataType: "json",
          cache: false,
          async: false,
            success: function (msg) { 
                // Default page size
                let pageSize = 50;
                const comboBox = $("#comboBox").data("kendoComboBox");
                if (comboBox) {
                    pageSize = comboBox.value() || 50;
                }
              $("#gridProject").kendoGrid({
                  dataSource: {
                      data: jQuery.parseJSON(msg.d),
                      aggregate: [{ field: "projCost", aggregate: "sum" },

                           { field: "ReceivedPayment", aggregate: "sum" },
                           { field: "PendingPayment", aggregate: "sum" },
                           { field: "RevisedBudget", aggregate: "sum" },
                           { field: "CreditAmount", aggregate: "sum" }
                      ],
                      schema: {
                          model: {
                              fields: {
                                  projId: { type: "string", editable: false },
                                  custId: { type: "number" },
                                  CustComapny: { type: "string" },
                                  projName: { type: "string" },
                                  projDesc: { type: "string" },
                                  projManager: { type: "number" },
                                  AccountMgr: { type: "number" },
                                  BA: { type: "number" },
                                  currID: { type: "number" },
                                  DevelopmentTeam: { type: "string" },
                                  AppraisalAuthorityMembers: { type: "string" },
                                  Projectmodules: { type: "string" },
                                  OtherEmailId: { type: "string" },
                                  CurrSymbol: { type: "string" },
                                  projCost: { type: "number" },
                                  projStartDate: { type: "date", format: "{0:dd-MMM-yyyy}" },
                                  projTotalCost: { type: "number" },
                                  currExRate: { type: "number" },
                                  projDuration: { type: "string" },
                                  Status: { type: "string" },
                                  RevisedBudget: { type: "number" },
                                  CreditAmount: { type: "number" },
                                  InHouse: { type: "string" },
                                  OnGoing: { type: "string" },
                                  projReportDate: {
                                      type: "date", format: "{0:dd-MMM-yyyy}"
                                  },
                                  isSendEmail: { type: "number" },
                                  ProjectType: { type: "number" },
                                  ProjectTypeName: { type: "strig" },
                                  InitialProjectCost: { type: "number" },
                                  IsTracked: {type: "string"}
                              }
                          }
                      },
                      pageSize: 50,                     
                  },
                  scrollable: true,
                  sortable: true,
                  selectable: true,
                  pageable: {
                      input: true,
                      numeric: false
                  },
                  columns: [
                     {
                         command: [
                                         {
                                             name: "edit", click: EditProject
                                         },
                                         {
                                             name: "Milestone", click: redirectToMilestone
                                         },
                                         {
                                             name: "Invoices", click: redirectToInvoices
                                         },
                                         {
                                             name: "Payments", click: redirectToPayment
                                         }
                         ],
                         width: "45px", attributes: { style: "text-align:center;" }
                     },
                        { field: "projId", title: "Project Id", width: 40, },
                        { field: "projName", title: "Project Title", width: 120, },
                        { field: "CustComapny", title: "Company", width: 120, footerTemplate: "<b>Total</b>" },
                        { field: "CurrSymbol", title: "Currency", width: 30, attributes: { style: "text-align:center;" } },
                        {
                            field: "projCost", title: "Budget", width: 60, attributes: { style: "text-align:right;" },
                            template: "#=(projCost=='0')?'':kendo.toString(projCost, 'n0')#", footerTemplate: " <b>#=(sum=='0')?'':kendo.toString(sum, 'n0')# </b>"
                        },
                        {
                            field: "RevisedBudget", title: "Payment Received", width: 60, attributes: { style: "text-align:right;" },
                            template: "#=(RevisedBudget=='0')?'':kendo.toString(RevisedBudget, 'n0')#", footerTemplate: " <b>  #=(sum=='0')?'':kendo.toString(sum, 'n0')# </b>"
                        },
                         {
                             field: "CreditAmount", title: "Credit Amount", width: 60, attributes: { style: "text-align:right;" },
                             template: "#=(CreditAmount=='0')?'':kendo.toString(CreditAmount, 'n0')#", footerTemplate: " <b>  #=(sum=='0')?'':kendo.toString(sum, 'n0')# </b>"
                         },
                        { field: "projectDuration", title: "Duration <br> (months)", width: 30, attributes: { style: "text-align:center;" }, template: "#=projectDuration#" },
                        { field: "projReportDate", title: "Report Date", width: 50, attributes: { style: "text-align:center;" }, template: "#= kendo.toString(kendo.parseDate(projReportDate, 'yyyy-MM-dd'), 'dd-MMM-yyyy') #" },
                      { field: "Status", title: "Status", width: 40, attributes: { style: "text-align:center;" }, template: "#=Status#" },
                      { field: "isSendEmail", title: "isSendEmail", width: 40, },
                      { field: "IsTracked", title: "IsTracked", width: 40, },
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

                  cancel: function (e) {
                      e.preventDefault()
                      ClosingRateWindow(e);
                  },

                  filterMenuInit: function (e) {
                      if (e.field == "name") {
                          var firstValueDropDown = e.container.find("select:eq(0)").data("kendoDropDownList");
                          firstValueDropDown.value("contains");
                          var logicDropDown = e.container.find("select:eq(1)").data("kendoDropDownList");
                          logicDropDown.value("or");
                          var secondValueDropDown = e.container.find("select:eq(2)").data("kendoDropDownList");
                          secondValueDropDown.value("contains");
                      }
                  },

                  // Add &  edit
                  save: function (e) {
                      var id = "";
                      if (e.model.isNew()) {
                          id = e.model.projId;

                          if ($('#txtEditProjectTile').val().length == 0) {
                              alert('Please fill Project Title');
                              e.preventDefault();
                          }
                          else {
                              //UpdateProjectByProjId(id, $("#txtEditCustomer").val(), e.model.projName,
                              //    e.model.projDesc, $("#txtEditProjectManager").val(),
                              //    (e.model.projStartDate == '') ? '' : kendo.toString(e.model.projStartDate, 'dd/MM/yyyy'),
                              //    $("#txtEditProjectDuration").val(),
                              //     (e.model.projActComp == '') ? '' : kendo.toString(e.model.projActComp, 'dd/MM/yyyy'),
                              //     $("#txtEditPaymentCurrency").val(), e.model.currExRate, e.model.projCost,
                              //     $("#txtEditProcurementMonth").val(), $("#txtEditLastPaymentMonth").val(),
                              //    $("#txtEditCodeReviewTeam").data("kendoMultiSelect").value().toString(),
                              //    e.model.OtherEmailId, $("#txtEditDevelopmentTeam").data("kendoMultiSelect").value().toString(),
                              //    $("input[name=chkModuleIDEdit]:checked").map(function () { return this.value; }).get().join(","));

                              UpdateProjectByProjId(id, $("#txtEditCustomer").val(), e.model.projName, e.model.projDesc, $("#txtEditProjectManager").val(), (e.model.projStartDate == '') ? '' : kendo.toString(e.model.projStartDate, 'dd/MM/yyyy'), $("#txtEditProjectDuration").val(),
                                  (e.model.projActComp == '') ? '' : kendo.toString(e.model.projActComp, 'dd/MM/yyyy'), $("#txtEditPaymentCurrency").val(), e.model.currExRate, e.model.projCost, $("#txtEditProcurementMonth").val(), $("#txtEditLastPaymentMonth").val(),
                                  $("#txtEditCodeReviewTeam").data("kendoMultiSelect").value().toString(), e.model.OtherEmailId, $("#txtEditDevelopmentTeam").data("kendoMultiSelect").value().toString(), $("input[name=chkModuleIDEdit]:checked").map(function () { return this.value; }).get().join(","), e.model.isSendEmail, e.model.IsTracked);

                              ClosingRateWindow(e);
                              window.location.href = window.location.href;
                          }
                      }
                      else {
                          id = "0";
                      }
                  },
              });
              detailsTemplate = kendo.template($("#popup-editor").html());
              function showDetails(e) {
                  e.preventDefault();
                  var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                  wnd.content(detailsTemplate(dataItem));
                  wnd.center().open();
              }
          },
          error: function (x, e) {
              alert("The call to the server side failed. "
                    + x.responseText);
          }
      });
}

function EditProject(e) {

    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);


    
    openAddPopUP();
    //  $('#ctl00_ContentPlaceHolder1_btnAddMilstone').attr("style", 'display:block')

    $("#ctl00_ContentPlaceHolder1_btnAddMilstone").removeClass("displayNone");
    $("#ctl00_ContentPlaceHolder1_btnAddMilstone").addClass("small_button white_button open");
    
    var projectId = data.projId;
    $('[id$="hdnProjectId"]').val(projectId);

    $('#txtProjectTile').val(data.projName);
    if (data.currID != "") {
        var ddlPaymentCurrency = document.getElementById('txtPaymentCurrency');
        ddlPaymentCurrency.value = data.currID;
    }
    $('#txtProjectDescription').val(data.projDesc);
    $('#txtProjectCost').val(data.projCost);
    if (data.custId != "") {
        $("#txtCustomer").data("kendoDropDownList").value(data.custId);
    }
    
    if (data.ProjectType != "") {
        $("#txtProjectType").data("kendoDropDownList").value(data.ProjectType);

        if (data.ProjectType == 1) {
            $("#lblProjectCost").show();
            $("#txtInitialProjectCost").show();
            $("#txtInitialProjectCost").val(data.InitialProjectCost);
        }
        else
        {
            $("#lblProjectCost").hide();
            $("#txtInitialProjectCost").hide();
           
        }
    }

    $('#txtExchangeRate').val(data.currExRate);
    if (data.projManager != "") {
        var ddlProjectManager = document.getElementById('txtProjectManager');
        ddlProjectManager.value = data.projManager;
    }

    if (data.AccountMgr != 0) {
        var ddlAccountMgr = document.getElementById('txtAccountMgr');
        ddlAccountMgr.value = data.AccountMgr;
    }


    if (data.BA != 0) {
        var ddlBA = document.getElementById('txtBA');
        ddlBA.value = data.BA;

    }
    if (data.ProjectType != 0) {
        var ddlProjectType = document.getElementById('txtProjectType');
        ddlProjectType.value = data.ProjectType;

    }

    $('#txtStartDate').val(convert(data.projStartDate));
    $('#txtReportDate').val(convert(data.projReportDate));

    if (data.projDuration != "") {
        var duration = data.projDuration / 0.25;
        if ((duration % 12) == 0) {

            $("#txtProjectDurationData").val((duration / 4) / 12);
            $("#txtProjectDurationType").data("kendoDropDownList").value(12);
        }
        else if ((duration % 4) == 0) {

            $("#txtProjectDurationData").val(duration / 4);
            $("#txtProjectDurationType").data("kendoDropDownList").value(1);
        }
        else {

            $("#txtProjectDurationData").val(duration);
            $("#txtProjectDurationType").data("kendoDropDownList").value(0.25);
        }
    }
    $('#txtotherEmailIds').val(data.OtherEmailId);

    var DevelopmentTeam = $("#txtDevelopmentTeam").data("kendoMultiSelect");
    DevelopmentTeam.dataSource.filter({});
    DevelopmentTeam.value(data.DevelopmentTeam.split(","));

    var AppraisalAuthorityMembers = $("#txtAppraisalAuthority").data("kendoMultiSelect");
    AppraisalAuthorityMembers.dataSource.filter({});
    AppraisalAuthorityMembers.value(data.AppraisalAuthorityMembers.split(","));

    $('[id$="chkInHouse"]').prop("checked", (Boolean(parseInt(data.InHouse))));
    $('[id$="chkOnGoing"]').prop("checked", (Boolean(parseInt(data.OnGoing))));
    $('[id$="chkTSEmail"]').prop("checked", (Boolean(parseInt(data.isSendEmail))));
    $('[id$="chkIsTracked"]').prop("checked", (Boolean(parseInt(data.IsTracked))));
    $('#ctl00_ContentPlaceHolder1_txtReportDate').val(convert(data.projReportDate));
    
    //var IsSendEmail = data.isSendEmail;

    //if (IsSendEmail == 0) {
    //    $('[id$="chkTSEmail"]').checked() == false;//.prop("checked", (Boolean(parseInt(data.InHouse))));
    //}
    //else {
    //    $('[id$="chkTSEmail"]').checked() == true;
    //}
    //$('#txtReportDate').val(convert(data.projReportDate));
}

function convert(str) {

    var monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"];

    var newDate = new Date(str);
    var formattedDate = (newDate.getDate() < 10 ? '0' : '') + newDate.getDate() + '/' + monthNames[newDate.getMonth()] + '/' + newDate.getFullYear();

    return formattedDate;
}



/*----------------------------Code to Display Milestone Pop-up Starts-----------------*/
function redirectToMilestone(e) {



    customRowDataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var projId = customRowDataItem.projId;
    var projectName = customRowDataItem.projName;
    var currExRate = customRowDataItem.currExRate;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "Project.aspx/SetProjIdForMilestone",
        data: "{'projid':'" + projId + "','currExRate':'" + currExRate + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {

        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
    window.location.assign("Milestone.aspx");

}
/*----------------------------Code to Display Milestone Pop-up Ends-----------------*/

/*----------------------------Code to Display Invoice Pop-up Starts-----------------*/
function redirectToInvoices(e) {
    customRowDataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var data = this.dataItem(customRowDataItem);
    var projId = customRowDataItem.projId;
    var projectName = customRowDataItem.projName;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "Project.aspx/SetProjIdForInvoice",
        data: "{'projid':'" + projId + "','projname':'" + projectName + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {

        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
    window.location.assign("ProjectInvoices.aspx");
}
/*----------------------------Code to Display Invoice Pop-up Ends-----------------*/
function redirectToPayment(e) {
    customRowDataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var data = this.dataItem(customRowDataItem);
    var projId = customRowDataItem.projId;
    var projectName = customRowDataItem.projName;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "Project.aspx/SetProjIdForPayment",
        data: "{'projid':'" + projId + "','projname':'" + projectName + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {

        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
    window.location.assign("ProjectPayments.aspx");
}
/*----------------------------Code to Display Project Grid Starts-----------------*/
function SetDefualtProjectData() {
    var ischecked = false;
    var status = "2"; // 2 is for In Progress for Default Page Load
    var multiselect = $("#txtStatus").data("kendoMultiSelect");
    if (multiselect) {
        multiselect.value([2]); // Set the default value to 2
        multiselect.trigger("change"); // Ensure the UI updates to display "In Progress"
    }

    SetDefaultProjectDataValues(ischecked, status);   
}

function SetDefaultProjectDataValues(ischecked, status) {
    CustomerID = window.location.search.substring(1);
    var n = CustomerID.split("=");
    ID = CustomerID.split("=");

    if (ID[1] != null) {

        GetProjectData(ID[1], ischecked, status);
    }
    else {
        GetProjectData(0, ischecked, status);
    }
    GetPagesize();
}

function ClosingRateWindow(e) {
    var grid = $("#gridProject").data("kendoGrid");
    grid.refresh();
}

function FillCustomerDropDown() {
    $.ajax(
          {
              type: "POST",
              url: "Project.aspx/BindCustomersDropDown",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              async: false,
              dataType: "json",
              success: function (msg) {
                  $("#txtCustomer").kendoDropDownList({
                      optionLabel: "Select Customer",
                      dataTextField: "custCompany",
                      dataValueField: "custId",
                      dataSource: jQuery.parseJSON(msg.d)

                  }).data("kendoDropDownList");
              },

              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
);
}

function FillCodeReviewTeam() {
    $.ajax(
          {
              type: "POST",
              url: "Project.aspx/BindProjectManagerDropDown",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              dataType: "json",
              success: function (msg) {
                  $("#txtCodeReviewTeam").kendoMultiSelect({
                      optionLabel: "Select Code Review team",
                      dataTextField: "empName",
                      dataValueField: "empid",
                      dataSource: jQuery.parseJSON(msg.d),

                  }).data("kendoMultiSelect");
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
    );
}

function SetDefualtCustomer() {
    CustomerID = window.location.search.substring(1);
    var n = CustomerID.split("=");
    ID = CustomerID.split("=");
    if (ID[1] != null) {

        var dropdownlist = $("#txtCustomer").data("kendoDropDownList");
        var value = dropdownlist.value();
        dropdownlist.value(ID[1]);
        dropdownlist.enable(false);
    }
}

function checkboxEventBinding() {
    $('#checkall').bind('click', function (e) {

        if (this.checked) {
            alert(e.data.ModuleID);
            $('.item.click input').attr('checked', 'checked');
        }
        else {
            $('.item.click input').removeAttr('checked');
        }
    })
}

function FillProjectDurationType() {

    $("#txtProjectDurationType").kendoDropDownList({
        optionLabel: "Select...",
        dataTextField: "text",
        dataValueField: "value",
        dataSource: ProjectDurationType

    }).data("kendoDropDownList");
}

function BindAllDateCalender() {
    $("#txtStartDate").kendoDatePicker({ format: "dd/MMM/yyyy" });
    $("#txtProcurementMonth").kendoDatePicker({ format: "MMMM/yyyy" });
    $("#txtLastPaymentMonth").kendoDatePicker({ format: "MMMM/yyyy" });
    $("#txtActualCompletion").kendoDatePicker({ format: "dd/MMM/yyyy" });
    $("#ctl00_ContentPlaceHolder1_txtReportDate").kendoDatePicker({ format: "dd/MMM/yyyy" });
    $("#txtReportDate").kendoDatePicker({ format: "dd/MMM/yyyy" });
}

function FillPaymentCurrency() {
    $.ajax(
         {
             type: "POST",
             url: "Project.aspx/GetAllcurrencyMaster",
             contentType: "application/json;charset=utf-8",
             data: "{}",
             dataType: "json",
             success: function (msg) {
                 $("#txtPaymentCurrency").kendoDropDownList({
                     dataTextField: "currSymbol",
                     dataValueField: "currId",
                     dataSource: jQuery.parseJSON(msg.d),

                 }).data("kendoDropDownList");
             },
             error: function (x, e) {
                 alert("The call to the server side failed. "
                       + x.responseText);
             }
         }
   );
}
//Added by trupti for Adding Project type Dropdown 
function FillProjectTypeDropDown() {
    $.ajax(
        {
            type: "POST",
            url: "Project.aspx/getProjectType",
            contentType: "application/json;charset=utf-8",
            data: "{}",
            async: false,
            dataType: "json",
            success: function (msg) {
                $("#txtProjectType").kendoDropDownList({
                    optionLabel: "Select Project type",
                    dataTextField: "ProjectType",
                    dataValueField: "ProjTypeID",
                    dataSource: jQuery.parseJSON(msg.d)

                }).data("kendoDropDownList");
            },

            error: function (x, e) {
                alert("The call to the server side failed. "
                    + x.responseText);
            }
        }
    );
}

// function FillDevelopmentTeam() {
//    $.ajax(
//          {
//              type: "POST",
//              url: "Project.aspx/BindProjectManagerDropDown",
//              contentType: "application/json;charset=utf-8",
//              data: "{}",
//              dataType: "json",
//              success: function (msg) {
//                  $("#txtDevelopmentTeam").kendoMultiSelect({
//                      optionLabel: "Select development team",
//                      dataTextField: "empName",
//                      dataValueField: "empid",
//                      dataSource: jQuery.parseJSON(msg.d),

//                  }).data("kendoMultiSelect");
//              },
//              error: function (x, e) {
//                  alert("The call to the server side failed. "
//                        + x.responseText);
//              }
//          }
//    );
//}
async function FillDevelopmentTeam() {
    try {
        const msg = await $.ajax({
            type: "POST",
            url: "Project.aspx/BindProjectManagerDropDown",
            contentType: "application/json;charset=utf-8",
            data: "{}",
            dataType: "json"
        });

        $("#txtDevelopmentTeam").kendoMultiSelect({
            optionLabel: "Select development team",
            dataTextField: "empName",
            dataValueField: "empid",
            dataSource: jQuery.parseJSON(msg.d)
        }).data("kendoMultiSelect");
    } catch (error) {
        alert("The call to the server side failed. " + error.responseText);
    }
}

//function BindStatusDropDown() {
//    $.ajax(
//          {
//              type: "POST",
//              url: "Project.aspx/BindProjectStatusDropDown",
//              contentType: "application/json;charset=utf-8",
//              data: "{}",
//              dataType: "json",
//              success: function (msg) {
//                  $("#txtStatus").kendoMultiSelect({
//                      optionLabel: "Select Status...",
//                      dataTextField: "projDesc",
//                      dataValueField: "projStatusId",
//                      dataSource: jQuery.parseJSON(msg.d),

//                  }).data("kendoMultiSelect");
//              },
//              error: function (x, e) {
//                  alert("The call to the server side failed. "
//                        + x.responseText);
//              }
//          }
//    );
//}
async function BindStatusDropDown() {
    try {
        const msg = await $.ajax({
            type: "POST",
            url: "Project.aspx/BindProjectStatusDropDown",
            contentType: "application/json;charset=utf-8",
            data: "{}",
            dataType: "json"
        });

        $("#txtStatus").kendoMultiSelect({
            optionLabel: "Select Status...",
            dataTextField: "projDesc",
            dataValueField: "projStatusId",
            dataSource: jQuery.parseJSON(msg.d)            
        }).data("kendoMultiSelect");  

        // Set the default value to 2 and display "In Progress"
        var multiselect = $("#txtStatus").data("kendoMultiSelect");
        if (multiselect) {
            multiselect.value([2]); // Set the default value to 2
            multiselect.trigger("change"); 
        }

        // Call SetDefualtProjectData to initialize default project data
        SetDefualtProjectData();

    } catch (error) {
        alert("The call to the server side failed. " + error.responseText);
    }
}

function SearchProjectByStatus() {
    var status = $("#txtStatus").data("kendoMultiSelect").value();
    status = status.join();
    SetDefaultProjectDataValues(false, status);    
}

function GetInvoices() {
    var ProjID = parseInt($('[id$="hdnProjID"]').val());
    var FromDate = $('[id$="txtFromDate"]').val();
    var ToDate = $('[id$="txtTODate"]').val();  


    $.ajax({
        type: "POST",
        url: "Project.aspx/BindProjectsByStatus",
        contentType: "application/json;charset=utf-8",
        data: "{'ProjID':'" + ProjID + "','FromDate':'" + FromDate + "','ToDate':'" + ToDate + "' }",
        cache: false,
        async: false,
        dataType: "json",
        success: function (msg) {
            GetInvoicesData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function FillProjectModules() {
    $.ajax(
            {
                type: "POST",
                url: "Project.aspx/GetModulesForProjects",
                contentType: "application/json;charset=utf-8",
                data: "{}",
                async: false,
                dataType: "json",
                success: function (msg) {
                    $("#listViewEdit").kendoListView({
                        dataSource: jQuery.parseJSON(msg.d),
                        template: kendo.template($("#myTemplateEdit").html()),
                    });
                },
                error: function (x, e) {
                    alert("The call to the server side failed. "
                          + x.responseText);
                }
            }
        );
}



function OpenPopupinEditMode(projid) {
    //alert(projid);
    setTimeout(function () {
        $("#ctl00_ContentPlaceHolder1_btnAddMilstone").removeClass("displayNone");
        $("#ctl00_ContentPlaceHolder1_btnAddMilstone").addClass("small_button white_button open");
        openAddPopUP();
    }, 1000);

    setTimeout(function () {
        $.ajax(
            {
                type: "POST",
                url: "Project.aspx/SelectedProject",
                contentType: "application/json;charset=utf-8",
                data: "{projId:'" + projid + "'}",
                dataType: "json",
                cache: false,
                async: false,
                success: function (msg) {

                    var data = jQuery.parseJSON(msg.d);

                    var projectId = data.projId;
                    $('[id$="hdnProjectId"]').val(projectId);

                    $('#txtProjectTile').val(data.projName);
                    if (data.currID != "") {
                        var ddlPaymentCurrency = document.getElementById('txtPaymentCurrency');
                        ddlPaymentCurrency.value = data.currID;
                    }
                    $('#txtProjectDescription').val(data.projDesc);
                    if (data.custId != "") {
                        $("#txtCustomer").data("kendoDropDownList").value(data.custId);
                    }
                    $('#txtExchangeRate').val(data.currExRate);

                    if (data.projManager != "") {
                        //var ddlProjectManager = document.getElementById('txtProjectManager');
                        //ddlProjectManager.value = data.projManager;
                        $("#txtProjectManager").data("kendoDropDownList").value(data.projManager);
                    }
                    if (data.AccountMgr != 0) {

                        $("#txtAccountMgr").data("kendoDropDownList").value(data.AccountMgr);
                    }
                    if (data.BA != 0) {

                        $("#txtBA").data("kendoDropDownList").value(data.BA);
                    }
                    if (data.ProjectType != 0) {

                        $("#txtProjectType").data("kendoDropDownList").value(data.projectType);
                    }
                    $('#txtStartDate').val(convert(data.projStartDate));

                    $('#txtotherEmailIds').val(data.OtherEmailId);

                    var DevelopmentTeam = $("#txtDevelopmentTeam").data("kendoMultiSelect");
                    DevelopmentTeam.dataSource.filter({});
                    DevelopmentTeam.value(data.DevelopmentTeam.split(","));

                    var AppraisalAuthorityMembers = $("#txtAppraisalAuthority").data("kendoMultiSelect");
                    AppraisalAuthorityMembers.dataSource.filter({});
                    AppraisalAuthorityMembers.value(data.AppraisalAuthorityMembers.split(","));

                    $('[id$="chkInHouse"]').prop("checked", (Boolean(parseInt(data.InHouse))));
                    $('[id$="chkOnGoing"]').prop("checked", (Boolean(parseInt(data.OnGoing))));
                    $('[id$="chkTSEmail"]').prop("checked", (Boolean(parseInt(data.isSendEmail))));
                    $('[id$="chkIsTracked"]').prop("checked", (Boolean(parseInt(data.IsTracked))));
                },
                error: function (x, e) {
                    alert("The call to the server side failed. "
                          + x.responseText);
                }

            });
    }, 3000);
}

function FillProjectMangerDropDown() {
    $.ajax(
          {
              type: "POST",
              url: "Project.aspx/BindProjectManagerDropDown",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              dataType: "json",
              success: function (msg) {
                  $("#txtProjectManager").kendoDropDownList({
                      optionLabel: "Select Project Manager",
                      dataTextField: "empName",
                      dataValueField: "empid",
                      dataSource: jQuery.parseJSON(msg.d)
                  }).data("kendoDropDownList");
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
    );
}

function FillBADropDown() {
    $.ajax(
        {
            type: "POST",
            url: "Project.aspx/BindProjectManagerDropDown",
            contentType: "application/json;charset=utf-8",
            data: "{}",
            dataType: "json",
            success: function (msg) {
                $("#txtBA").kendoDropDownList({
                    optionLabel: "Select Business Analyst",
                    dataTextField: "empName",
                    dataValueField: "empid",
                    dataSource: jQuery.parseJSON(msg.d)
                }).data("kendoDropDownList");
            },
            error: function (x, e) {
                alert("The call to the server side failed. "
                    + x.responseText);
            }
        }
    );
}

function FillAccountMangerDropDown() {
    $.ajax(
          {
              type: "POST",
              url: "Project.aspx/BindAccountMgrDropDown",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              dataType: "json",
              success: function (msg) {
                  $("#txtAccountMgr").kendoDropDownList({
                      optionLabel: "Select Account Manager",
                      dataTextField: "empName",
                      dataValueField: "empid",
                      dataSource: jQuery.parseJSON(msg.d)
                  }).data("kendoDropDownList");
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
    );
}

// function FillAppraisalAuthorityDropDown() {
//    $.ajax(
//          {
//              type: "POST",
//              url: "Project.aspx/BindAppraisalAuthorityDropDown",
//              contentType: "application/json;charset=utf-8",
//              data: "{}",
//              dataType: "json",
//              success: function (msg) {
//                  $("#txtAppraisalAuthority").kendoMultiSelect({
//                      optionLabel: "Select  Appraisal Authority",
//                      dataTextField: "empName",
//                      dataValueField: "empid",
//                      dataSource: jQuery.parseJSON(msg.d),
//                  }).data("kendoMultiSelect");
//              },
//              error: function (x, e) {
//                  alert("The call to the server side failed. "
//                        + x.responseText);
//              }
//          }
//    );
//}
async function FillAppraisalAuthorityDropDown() {
    try {
        const msg = await $.ajax({
            type: "POST",
            url: "Project.aspx/BindAppraisalAuthorityDropDown",
            contentType: "application/json;charset=utf-8",
            data: "{}",
            dataType: "json"
        });

        $("#txtAppraisalAuthority").kendoMultiSelect({
            optionLabel: "Select Appraisal Authority",
            dataTextField: "empName",
            dataValueField: "empid",
            dataSource: jQuery.parseJSON(msg.d)
        }).data("kendoMultiSelect");
    } catch (error) {
        alert("The call to the server side failed. " + error.responseText);
    }
}
function openAddPopUPwithdata() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}

//Add Customer functionality starts here
function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
    FillCustomerDropDown();
    FillProjectTypeDropDown();//Added by trupti 
    FillBADropDown();//Added by Trupti
    FillProjectMangerDropDown();
    FillAccountMangerDropDown();
    FillPaymentCurrency();
    SetDefualtCustomer();
    $("#txtStartDate").kendoDatePicker({ format: "dd/MMM/yyyy" });
    $("#txtProcurementMonth").kendoDatePicker({ format: "MMMM/yyyy" });
    $("#txtLastPaymentMonth").kendoDatePicker({ format: "MMMM/yyyy" });
    $("#txtActualCompletion").kendoDatePicker({ format: "dd/MMM/yyyy" });
    // $("#ctl00_ContentPlaceHolder1_txtReportDate").kendoDatePicker({ format: "dd/MMM/yyyy" }).css('display', 'none');
    $('#lblReportDate').css('display', 'none');
    $("#ctl00_ContentPlaceHolder1_txtReportDate").prop('required', false);

    $('#test').css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtReportDate").removeClass('k-widget k-datepicker k-header k-textbox k-input');
    $("#ctl00_ContentPlaceHolder1_txtReportDate").css("display", "none");

    // $("#datepicker2").css("display", "none");
    // data-role="datepicker"
    FillCodeReviewTeam();
    FillProjectModules();
    FillProjectDurationType();
}



function openEditPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
    $('#lblReportDate').css('display', 'block');

    $('#test').css("display", "block");
    $("#ctl00_ContentPlaceHolder1_txtReportDate").prop('required', true);

    $('#ctl00_ContentPlaceHolder1_txtReportDate').css('display', 'block');

    FillCustomerDropDown();
    FillProjectTypeDropDown();
    FillProjectMangerDropDown();
    FillBADropDown();
    FillAccountMangerDropDown();
    FillPaymentCurrency();
    SetDefualtCustomer();
    BindAllDateCalender();
    FillCodeReviewTeam();
    FillProjectModules();
    FillProjectDurationType();
}


function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    clearFields();
}

function ShowAddPopup() {
    $('[id$="chkInHouse"]').prop("checked", false);
    $('[id$="chkOnGoing"]').prop("checked", false);
    $('[id$="chkTSEmail"]').prop("checked", false);
    $("#ctl00_ContentPlaceHolder1_btnAddMilstone").addClass("displayNone");
    $('[id$="hdnProjectId"]').val(0);
    openAddPopUP();
}

function closePopUP() {
    $('#divPopUP').css('display', 'None');
    $('#divOverlay').removeClass('k-overlay').addClass("k-overlayDisplaynone");
}
//Edit Grid Items ends here

function clearFields() {

    $('[id$="txtProjectTile"]').val('');
    $('[id$="txtPaymentCurrency"]').val('');
    $('[id$="txtProjectDescription"]').val('');
    //  $('[id$="txtProjectCost"]').val('');
    $('[id$="txtCustomer"]').val('');
    $('[id$="txtExchangeRate"]').val('1');
    $('[id$="txtProjectManager"]').val('');
    $('[id$="txtAccountMgr"]').val('');
    $('[id$="txtBA"]').val('');//Added by Trupti
    $('[id$="txtProjectType"]').val(''); //Added by Trupti
    $('[id$="txtInitialProjectCost"]').val(''); //Added by Trupti
    $("#txtAppraisalAuthority").data("kendoMultiSelect").value([]);

    $('[id$="txtStartDate"]').val('');
    $('[id$="txtReportDate"]').val('');
    //$('[id$="txtProjectDurationData"]').val('');
    //$('[id$="txtProjectDurationType"]').val('');
    $('[id$="txtotherEmailIds"]').val('');
    $("#txtDevelopmentTeam").data("kendoMultiSelect").value([]);
}

function sendmail() {
    var To = $('[id$="txtTo"]').val();
    var Cc = $('[id$="txtCc"]').val();
    var Bcc = $('[id$="txtBcc"]').val();
    var Subject = $('[id$="txtSubject"]').val();
    var editor = $('[id$="txtEmail"]').data("kendoEditor");
    var MsgBody = editor.value();

    if ($('.red').length != 0) {
        $('.red').html('');
    }

    if (To == "") {
        $('[id$="txtTo"]').after('<div class="red" style="color:red">Please enter recipient email id</div>');
        return false;
    }

    else if (To != "") {

        if (!ValidateEmail($('[id$="txtTo"]').val())) {
            $('[id$="txtTo"]').after('<div class="red" style="color:red">Please enter valid email id</div>');
            return false;
        }
        else {
            $.ajax({

                type: "POST",
                url: "Project.aspx/SendMail",
                contentType: "application/json;charset=utf-8",
                data: "{mailTo:'" + To + "',Cc:'" + Cc + "',Bcc:'" + Bcc + "',strSubject:'" + Subject + "',strMsgBody:'" + MsgBody + "'}",
                cache: false,
                dataType: "json",
                success: function (msg) {
                    var message = msg.d;
                    if (message != 'Mail Sent' || message=="")
                        alert("Mail sent Failed.");
                    else
                    $('#divMail').css('display', 'none');
                    $('[id$="txtTo"]').val('');
                    $('[id$="txtCc"]').val('');
                    $('[id$="txtBcc"]').val('');
                    $('[id$="txtSubject"]').val('');
                    $('[id$="txtEmail"]').val('');
                    $('body').css('cursor', 'default');
                    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
                    window.location.href = "/Member/Milestone.aspx";
                },
                error: function (msg) {
                    alert("Mail sent failed."
                          + msg.responseText);
                }
            });
        }

    }
}

function ValidateEmail(email) {
    var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    return expr.test(email);
};
function ChkProjectType(val) {
    
    console.log(val);
    alert(val);
}
