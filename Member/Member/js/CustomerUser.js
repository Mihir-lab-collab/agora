$(document).ready(function () {

    var validator = $("#tickets").kendoValidator().data("kendoValidator"),
         status = $(".status");
    var validator = $("#popup-editor").kendoValidator().data("kendoValidator"),
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
          url: "Customerservice.asmx/GetAllCustUsersByCustID",
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
                                 
                              }
                          }
                      },
                      pageSize: 10
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

                        {
                            command: [{
                                name: "edit", text: { edit: "View Details", update: "Submit", cancel: "Close" }, title: "Actions"

                            }, ], width: "75px"
                        },

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
                              UpdateCustomerUser(id,e.model.CustID, e.model.FName, e.model.LName, e.model.Email, e.model.ContactNo, isadmin, isactive, sendmail, e.model.Password);
                              ClosingRateWindow(e);
                              location.reload();
                          }

                      }
                      else {
                          id = "0";

                      }
                  },


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
               url: "Customerservice.asmx/UpdateCustomerUser",
               cache: false,
               async: false,
               contentType: "application/json;charset=utf-8",
               data: "{'UserMasterID':'" + UserMasterID + "','CustId':'" + CustId + "','FName':'" + FName + "','LName':'" + LName + "','Email':'" + Email + "','Contactno':'" + Contactno + "','IsAdmin':'" + IsAdmin + "','Isactive':'" + Status + "','sendmail':'" + sendmail + "','password':'" + password + "'}",
               dataType: "json",
               success: function (msg) {
                   var isupadted = jQuery.parseJSON(msg.d);                  
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
              url: "Customerservice.asmx/CheckCompanyExists",
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
              url: "Customerservice.asmx/CheckUpdatedCompanyNotExists",
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