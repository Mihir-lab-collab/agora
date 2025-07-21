$(document).ready(function () {

    var validator = $("#tickets").kendoValidator().data("kendoValidator"),
         status = $(".status");
    var validator = $("#popup-editor").kendoValidator().data("kendoValidator"),
         status = $(".status");
    var validator = $("#projecttemplate").kendoValidator().data("kendoValidator"),
        status = $(".status");

    GetCustomerUserData();

});


function isSpclChar(ob) {
    var invalidChars = /[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi;
    if (invalidChars.test(ob.value)) {
        ob.value = ob.value.replace(invalidChars, "");
    }
}
function isContact(ob) {
    var invalidChars = /[`~!@#$%^&*()_|\=?;:'",.<>\{\}\[\]\\\/^a-z\s{,}]/gi;
    if (invalidChars.test(ob.value)) {
        ob.value = ob.value.replace(invalidChars, "");
    }
}
function isChar(evt, field) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    return ((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122) || charCode == 8 || charCode == 9 || charCode == 32 || (charCode >= 48 && charCode <= 57)) ? true : false;
}
function GetCustomerUserData() {
    $.ajax(
      {
          type: "POST",
          url: "UserManager.aspx/GetAllCustUsersByCustID",
          contentType: "application/json;charset=utf-8",
          data: "{'CustId':'" + CustId + "'}",
          dataType: "json",
          cache: false,
          async: false,
          success: function (msg) {
              $("#gridCustomerUser").kendoGrid({
                  dataSource: {
                      data: jQuery.parseJSON(msg.d),
                      schema: {
                          model: {
                              fields: {
                                  UserMasterID: { type: "string", editable: false },
                                  CustID: { type: "string", editable: false },
                                  Password: { type: "string" },
                                  FName: { type: "string" },
                                  LName: { type: "string" },
                                  Email: { type: "string" },
                                  ContactNo: { type: "string" },
                                  IsAdmin: { type: "string" },
                                  Status: { type: "string" },
                                  InsertedOn: { type: "date", format: "{0:dd-MMM-yyyy}" },
                                  ModifiedOn: { type: "date", format: "{0:dd-MMM-yyyy}" },
                                  Name: { type: "string" },
                                  ProjectsAssignedToUser: { type: "string" },

                              }
                          }
                      },
                      pageSize: 25
                  },
                  scrollable: true,
                  sortable: true,
                  pageable: {
                      input: true,
                      numeric: false
                  },
                  columns: [

                        { field: "Name", title: "User Name", width: "100px" },
                        { field: "Email", title: "Email", width: "100px" },
                        { field: "ContactNo", title: "ContactNo", width: "100px" },
                        { field: "IsAdmin", title: "Is Admin", width: "50px" },
                        { field: "InsertedOn", title: "Inserted On", format: "{0:dd-MMM-yyyy}", width: "100px" },
                        { field: "ProjectsAssignedToUser", title: "Projects", width: "100px" },

                        {
                            command: [{
                                name: "edit", text: { edit: "Details", update: "Submit", cancel: "Close" }, title: "Actions"

                            }, ], width: "75px"
                        },

                        { command: { text: "Projects", click: showDetailsProject }, title: " ", width: "140px" }

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
                  editable: true,
                  editable: {
                      mode: "popup",
                      template: kendo.template($("#popup-editor").html())
                  },

                  cancel: function (e) {
                      e.preventDefault()
                      ClosingRateWindow(e);
                  },

                  // add & edit popup
                  edit: function (e) {
                      var editWindow = e.container.data("kendoWindow");
                      if (e.model.isNew()) {

                          e.container.data("kendoWindow").title('User Details');
                          // $(".k-grid-update").text = "Save";                        
                          //$('#chkIsAdminEdit').prop('checked', e.model.IsAdmin);
                          $('#chkIsAdminEdit').prop('checked', '');
                          if (e.model.IsAdmin == "true") {
                              $('#chkIsAdminEdit').prop('checked', 'checked');
                          }

                          $('#chkIsActiveEdit').prop('checked', '');
                          if (e.model.Status == "a") {
                              $('#chkIsActiveEdit').prop('checked', 'checked');
                          }

                          $('#chksendmailEdit').prop('checked', '');
                      }
                      else {
                          e.container.data("kendoWindow").title('User Details');
                      }
                  },

                  // for filter
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

                          id = e.model.UserMasterID;
                          if ($('#txtFirstName').val().length != 0) {
                              alert('Please fill first name');
                              e.preventDefault();
                          }
                          else {

                              var isadmin = $('#chkIsAdminEdit').is(':checked');
                              var isactive = $('#chkIsActiveEdit').is(':checked');
                              var sendmail = $('#chksendmailEdit').is(':checked');
                              UpdateCustomerUser(id, e.model.CustID, e.model.FName, e.model.LName, e.model.Email, e.model.ContactNo, isadmin, isactive, sendmail, e.model.Password);
                              ClosingRateWindow(e);
                              location.reload();
                          }

                      }
                      else {
                          id = "0";

                      }
                  },

                  dataBound: function (e) {                    
                      console.log("dataBound");
                      var grid = $("#gridCustomerUser").data("kendoGrid");
                      var gridData = grid.dataSource.view();
                      for (var i = 0; i < gridData.length; i++) {
                          var currentUid = gridData[i].uid;                         
                          if (gridData[i].IsAdmin != 'false') {
                              var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
                              //var editButton = $(currenRow).find(".k-grid-edit");
                              //editButton.hide();
                              var ProjectButton = $(currenRow).find(".k-grid-Projects");
                              ProjectButton.hide();
                          }
                          
                      }
                  }
              });
              //    wnd = $("#details")
              //.kendoWindow({
              //    // actions: ["Custom", "Refresh", "Maximize", "Minimize", "Close"],
              //    actions: ["Close"],
              //    title: "Report Details",
              //    modal: true,
              //    visible: false,
              //    resizable: false,
              //    width: 800
              //}).data("kendoWindow");

              detailsTemplate = kendo.template($("#popup-editor").html());
              function showDetails(e) {
                  e.preventDefault();
                  var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                  wnd.content(detailsTemplate(dataItem));
                  wnd.center().open();
              }





              wndproject = $("#projectdetails")
                        .kendoWindow({
                            title: "Project Details",
                            modal: true,
                            visible: false,
                            resizable: false,
                            width: 500
                        }).data("kendoWindow");
              detailsTemplateproject = kendo.template($("#projecttemplate").html());
              function showDetailsProject(e) {
                  e.preventDefault();
                  var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                  wndproject.content(detailsTemplateproject(dataItem));
                  wndproject.content();
                  wndproject.center().open();
                  //var CustId = $('#hdnCustId').val()
                  //alert(CustId);
                  SetProjectsandModules();


              }


          },
          error: function (x, e) {
              alert("The call to the server side failed. "
                    + x.responseText);
          }


      });

}
function SubmitProjects() {
    //var value =$('#txtUserId').val()
    if ($("#txtCustomerProjects").val() == '')
        alert('Please Select project');
    else {       
        var UserMasterID = $('#hdnUserMasterID').val();
        var ProjId = $("#txtCustomerProjects").val();
        var IsAdmin ='false';
        if ($("#chkIsAdminProject").is(':checked'))
            IsAdmin = 'true';
        var ModuleIds = $("input[name=chkModuleIDEdit]:checked").map(function () { return this.value; }).get().join(",");
        UpdateProjectModulesCurUser(UserMasterID, ProjId, IsAdmin, ModuleIds);      
        location.reload();
    }
}
function UpdateProjectModulesCurUser(UserMasterID, ProjId, IsAdmin, ModuleIds) {
    $.ajax(
           {
               type: "POST",
               url: "UserManager.aspx/UpdateProjectModulesCurUser",
               cache: false,
               async: false,
               contentType: "application/json;charset=utf-8",
               data: "{'UserMasterID':'" + UserMasterID + "','ProjId':'" + ProjId + "','IsAdmin':'" + IsAdmin + "','ModuleIds':'" + ModuleIds + "'}",
               dataType: "json",
               success: function (msg) {
                   var isupadted = jQuery.parseJSON(msg.d);
                   //RedirectPage();
                   alert(isupadted);
               },
               error: function (msg) {
                   alert("The call to the server side failed."
                         + msg.responseText);
               },
               complete: function () {


               }
           }
        );
}
function SetProjectsandModules() {
    $.ajax(
          {
              type: "POST",
              url: "UserManager.aspx/BindCustomerProjects",
              cache: false,
              async: false,
              contentType: "application/json;charset=utf-8",
              data: "{'CustId':'" + $('#hdnCustId').val() + "'}",
              dataType: "json",
              success: function (msg) {
                  $("#txtCustomerProjects").kendoDropDownList({
                      optionLabel: "Select Project",
                      dataTextField: "projName",
                      dataValueField: "projId",
                      dataSource: jQuery.parseJSON(msg.d),
                      change: function () {
                          //var value = this.value();
                          //alert(value);
                          FillModulesForCurrentProject(this.value());
                          GetProjectModulesForCurrentUser(this.value(), $('#hdnUserMasterID').val());
                      }
                  }).data("kendoDropDownList");
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
    );
}

function FillModulesForCurrentProject(ProjectId) {
    //Bind All Modules for current Project
    $.ajax(
               {
                   type: "POST",
                   url: "UserManager.aspx/GetModulesForCurrentProject",
                   contentType: "application/json;charset=utf-8",
                   data: "{'CurProjectId':'" + ProjectId + "'}",
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

               });
}

function GetProjectModulesForCurrentUser(ProjectId, UserMasterId) {

    $.ajax(
               {
                   type: "POST",
                   url: "UserManager.aspx/GetProjectModulesForCurrentUser",
                   contentType: "application/json;charset=utf-8",
                   data: "{'ProjectId':'" + ProjectId + "','UserMasterId':'" + UserMasterId + "'}",
                   async: false,
                   dataType: "json",
                   success: function (msg) {
                       var detailObj = jQuery.parseJSON(msg.d)
                       //alert(msg.d);
                       $.each(detailObj, function (key, output) {
                           var keywords = output.key;
                           var value = output.value;
                           if (keywords == 'ModuleIds') {
                               if (value != "") {
                                   //Bind For Modules
                                   var CurProjModules = value.split(",");
                                   for (var i = 0; i < CurProjModules.length; i++) {
                                       $("#chk" + CurProjModules[i]).attr("checked", true);
                                   }
                               }
                           }
                           if (keywords == 'IsAdmin') {
                               if (value != "") {
                                   if (value == 'true') {
                                       $('#chkIsAdminProject').prop('checked', true);
                                       $('#trModules').css('display', 'none');
                                   }
                                   else {
                                       $('#chkIsAdminProject').prop('checked', false);
                                       $('#trModules').css('display', 'table-row');
                                   }

                               }
                           }



                       });




                   },

                   error: function (x, e) {
                       alert("The call to the server side failed. "
                             + x.responseText);
                   }

               });
}



function ClosingRateWindow(e) {
    var grid = $("#gridCustomerUser").data("kendoGrid");
    grid.refresh();

}
function UpdateCustomerUser(UserMasterID, CustId, FName, LName, Email, Contactno, IsAdmin, Status, sendmail, password) {
    $.ajax(
           {
               type: "POST",
               url: "UserManager.aspx/UpdateCustomerUser",
               cache: false,
               async: false,
               contentType: "application/json;charset=utf-8",
               data: "{'UserMasterID':'" + UserMasterID + "','CustId':'" + CustId + "','FName':'" + FName + "','LName':'" + LName + "','Email':'" + Email + "','Contactno':'" + Contactno + "','IsAdmin':'" + IsAdmin + "','Isactive':'" + Status + "','sendmail':'" + sendmail + "','password':'" + password + "'}",
               dataType: "json",
               success: function (msg) {
                   var isupadted = jQuery.parseJSON(msg.d);
                   //RedirectPage();
                   alert(isupadted);
               },
               error: function (msg) {
                   alert("The call to the server side failed."
                         + msg.responseText);
               },
               complete: function () {


               }
           }
        );
}
//Add Customer functionality starts here
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
    //$("#ctl00_ContentPlaceHolder1_btnAddCustomer").attr("disabled", "disabled");
    //$('#spncomapnyexist').css('display', 'none');

    //$("#txtCustomerCompany").blur(function () {
    //    if (CheckCompanyExists()) {
    //        $("#ctl00_ContentPlaceHolder1_btnAddCustomer").removeAttr("disabled");
    //        $('#spncomapnyexist').css('display', 'none');
    //    }
    //    else {
    //        $("#ctl00_ContentPlaceHolder1_btnAddCustomer").attr("disabled", "disabled");
    //        if ($('#txtCustomerCompany').val().length != 0) {
    //            $('#spncomapnyexist').css('display', '');
    //        }
    //    }

    //});


}
function CheckCompanyExists() {
    var validcompanyname = false;

    $.ajax(
          {
              type: "POST",
              url: "UserManager.aspx/CheckCompanyExists",
              cache: false,
              async: false,
              contentType: "application/json;charset=utf-8",
              data: "{'ComapnyName':'" + $("#txtCustomerCompany").val() + "'}",
              dataType: "json",
              success: function (msg) {
                  var isvalid = jQuery.parseJSON(msg.d);
                  if (isvalid == 'valid')
                      validcompanyname = true;;
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              },
              complete: function () {

                  if (validcompanyname == true)
                      return true;
                  else
                      return false;
              }
          }
        );
    return validcompanyname;

    //if (validcompanyname = true)
    //    return true;
    //else
    //    return false;
    // 
}
function CheckUpdatedCompanyNotExists(custId, custCompany) {
    var validcompanyname = false;
    $.ajax(
          {
              type: "POST",
              url: "UserManager.aspx/CheckUpdatedCompanyNotExists",
              cache: false,
              async: false,
              contentType: "application/json;charset=utf-8",
              data: "{'custId':'" + custId + "','ComapnyName':'" + custCompany + "'}",
              dataType: "json",
              success: function (msg) {
                  var isvalid = jQuery.parseJSON(msg.d);
                  if (isvalid == 'valid')
                      validcompanyname = true;;
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              },
              complete: function () {

                  if (validcompanyname == true)
                      return true;
                  else
                      return false;
              }
          }
        );
    return validcompanyname;

    //if (validcompanyname = true)
    //    return true;
    //else
    //    return false;
    // 
}