$(document).ready(function () {

    var validator = $("#tickets").kendoValidator().data("kendoValidator"),
         status = $(".status");
    var validator = $("#popup-editor").kendoValidator().data("kendoValidator"),
         status = $(".status");

    GetCustomerData();
    GetPagesize();


});

function isChar(evt, field) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    return ((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122) || charCode == 8 || charCode == 9 || charCode == 32 || (charCode >= 48 && charCode <= 57)) ? true : false;
}
function GetCustomerData() {
    $.ajax(
      {
          type: "POST",
          url: "Customerservice.asmx/GetAllCustomerDetailsNew",
          contentType: "application/json;charset=utf-8",
          data: "{}",
          dataType: "json",
          cache: false,
          async: false,
          success: function (msg) {
              $("#gridCustomer").kendoGrid({
                  dataSource: {
                      data: jQuery.parseJSON(msg.d),
                      schema: {
                          model: {
                              fields: {
                                  custId: { type: "string", editable: false },
                                  custName: { type: "string" },
                                  custCompany: { type: "string" },
                                  custEmail: { type: "string" },
                                  custEmailCC: { type: "string" },
                                  custAddress: { type: "string" },
                                  custNotes: { type: "string" },
                                  custRegDate: { type: "date", format: "{0:dd-MMM-yyyy}" },
                                  custStatus: { type: "string" },
                                  TaskMailLevel: { type: "string" },
                                  InsertedOn: { type: "date", format: "{0:dd-MMM-yyyy}" },
                                  ModifiedOn: { type: "date", format: "{0:dd-MMM-yyyy}" },
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

                       // { field: "custId", title: "Customer Id", width: "50px" },
                         //{ field: "CustUserID", title: "User Id", width: "50px" },
                       // { field: "Password", title: "Password", width: "100px" },
                        { field: "custName", title: "Name", width: "100px" },
                        { field: "custCompany", title: "Company", width: "100px" },
                        { field: "custEmail", title: "Email", width: "130px" },
                        { field: "custEmailCC", title: "EmailCC", width: "130px",hidden:true },
                        { field: "custAddress", title: "Address", width: "130px" },
                        { field: "custRegDate", title: "Registered On", format: "{0:dd-MMM-yyyy}", width: "100px" },
                      
                       {
                        command: [
                            {
                                name: "edit",
                                text: { edit: "Details", update: "Submit", cancel: "Close" },
                                title: "Actions"
                        },
                            {
                                name: "Projects", click: showProjects
                           
                            },
                        {
                            name: "Users", click: showUsers

                        }],
                        width: "130px"
                        }
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
                      EditBindCountry();
                      EditBindState();
                      EditBindCity();
                      if (e.model.CountryName != "India")
                      {
                         $("#ddlEditState").kendoDropDownList({
                           
                              enable: false
                          });

                          $("#ddlEditCity").kendoDropDownList({
                              enable: false
                          });
                      }
                      else
                      {
                          $("#ddlEditState").kendoDropDownList({
                              
                              enable: true
                          });

                          $("#ddlEditCity").kendoDropDownList({
                              enable: true
                          });
                         
                      }
                      

                      $('#chkIsShowAllTask').prop('checked', '');
                      if (e.model.ShowAllTask == true)
                      {
                          $('#chkIsShowAllTask').prop('checked', 'checked');
                      }
                      if (e.model.isNew()) {

                          e.container.data("kendoWindow").title('Customer Details');
                         
                         
                      }
                      else {
                          e.container.data("kendoWindow").title('Customer Details');
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
                          id = e.model.custId;
                          
                          if ($("#chkIsShowAllTask").is(':checked')) {
                              e.model.ShowAllTask = "true";
                          }
                          else {
                              e.model.ShowAllTask = "false";
                          }
                          //var checkmail = $('#chksendmail').is(':checked');
                          //alert(checkmail);
                          if ($('#txtCustomerCompany').val().length != 0) {
                              //alert('hello');
                              e.preventDefault()
                          }
                          else {
                              if (CheckUpdatedCompanyNotExists(e.model.custId, e.model.custCompany))
                              {
                                  UpdateCustomer(id, e.model.custName, e.model.custCompany, e.model.custEmail, e.model.custAddress, e.model.custNotes, e.model.custEmailCC, e.model.ShowAllTask,e.model.CountryName,e.model.StateName,e.model.CityName,e.model.GSTIN);
                                  ClosingRateWindow(e);
                              }
                              else {
                                  alert('Company Name Already Exists.Update Failed.');
                                  e.preventDefault();
                              }


                          }
                          //if (e.model.custCompany == 'hi') {
                          //    alert('hello');
                          //    e.preventDefault()
                          //}
                          //else {
                          //    UpdateCustomer(id, e.model.custCompany, e.model.custAddress, e.model.custNotes);
                          //    ClosingRateWindow(e);
                          //}
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


              function showProjects(e) {
                  e.preventDefault();
                  var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                  //alert(dataItem.custId);
                  window.location.assign("Project.aspx?Customer=" + dataItem.custId);

              }

              function showUsers(e) {
                  e.preventDefault();
                  var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                  //alert(dataItem.custId);
                  window.location.assign("CustomerUser.aspx?Customer=" + dataItem.custId);
              }


          },
          error: function (x, e) {
              alert("The call to the server side failed. "
                    + x.responseText);
          }


      });

}
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
            var grid = $("#gridCustomer").data("kendoGrid");
            grid.dataSource.pageSize(this.value());
        }
    });
}
function ClosingRateWindow(e) {
    var grid = $("#gridCustomer").data("kendoGrid");
    grid.refresh();
}
function UpdateCustomer(custId, custName, custCompany, custEmail, custAddress, custNotes, custEmailCC,ShowAllTask,CountryName,StateName,CityName,GSTIN) {

    $.ajax(
           {
               type: "POST",
               url: "Customerservice.asmx/UpdateCustomer",
               cache: false,
               async: false,
               contentType: "application/json;charset=utf-8",
               data: "{'custId':'" + custId + "','custName':'" + custName + "','custCompany':'" + custCompany + "','custEmail':'" + custEmail + "','custAddress':'" + custAddress + "','custNotes':'" + custNotes + "','custEmailCC':'" + custEmailCC + "','ShowAllTask':'" + ShowAllTask + "','CountryName':'" + CountryName + "','StateName':'" + StateName + "','CityName':'" + CityName + "','GSTIN':'" + GSTIN + "'}",  //
               dataType: "json",
               success: function (msg) {
                   var isupadted = jQuery.parseJSON(msg.d);
                   //RedirectPage();
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
    BindCountry();
    BindState();
    BindCity();

  
}
function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}
function ShowAddPopup()
{
    // alert('Now its looking pretty good. weldone Satish.. you have done this.');
    openAddPopUP();
    $("#ctl00_ContentPlaceHolder1_btnAddCustomer").attr("disabled", "disabled");
    $('#spncomapnyexist').css('display', 'none');

    $("#txtCustomerCompany").blur(function () {
        if (CheckCompanyExists()) {
            $("#ctl00_ContentPlaceHolder1_btnAddCustomer").removeAttr("disabled");
            $('#spncomapnyexist').css('display', 'none');
        }
        else {
            $("#ctl00_ContentPlaceHolder1_btnAddCustomer").attr("disabled", "disabled");
            if ($('#txtCustomerCompany').val().length != 0) {
                $('#spncomapnyexist').css('display', '');
            }
        }

    });


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
function CheckEmailExists() {
    var validemailid = false;
    $.ajax(
          {
              type: "POST",
              url: "Customer.aspx/CheckEmailExists",
              contentType: "application/json;charset=utf-8",
              data: "{'EmailId':'2134'}",
              dataType: "json",
              success: function (msg) {
                  var isvalid = jQuery.parseJSON(msg.d)
                  //alert(isvalid);
                  if (isvalid = "valid")
                      validemailid = true;
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
        );
    if (validemailid = true)
        return true;
    else
        return false;
    // 
}


function BindCountry() {
    $.ajax(
         {
             type: "POST",
             url: "Customer.aspx/GetAllCountry",
             contentType: "application/json;charset=utf-8",
             data: "{}",
             dataType: "json",
             success: function (msg) {
                 $("#ddlCountry").kendoDropDownList({
                     optionLabel: "- Select Country -",
                     dataTextField: "Name",
                     dataValueField: "CountryID",
                     dataSource: jQuery.parseJSON(msg.d),
                     change: onChange,

                 }).data("kendoDropDownList");
             },
             error: function (x, e) {
                 alert("The call to the server side failed. "
                       + x.responseText);
             }
         }
   );


}

function onChange(e)

{
  
    if($("#ddlCountry").data("kendoDropDownList").text()!="India")
    {
        var Statelist = $("#ddlState").data("kendoDropDownList");
        Statelist.wrapper.hide();
        var Citylist = $("#ddlCity").data("kendoDropDownList");
        Citylist.wrapper.hide();
        document.getElementById("ctl00_ContentPlaceHolder1_lblState").style.display = 'none';
        document.getElementById("ctl00_ContentPlaceHolder1_lblCity").style.display = 'none';
    }
    else
    {
         var Statelist1 = $("#ddlState").data("kendoDropDownList");
             Statelist1.wrapper.show();
         var Citylist1 = $("#ddlCity").data("kendoDropDownList");
             Citylist1.wrapper.show();
         $("#ctl00_ContentPlaceHolder1_lblState").show();
         $("#ctl00_ContentPlaceHolder1_lblCity").show();
    }
};

function EditBindCountry() {
    $.ajax(
         {
             type: "POST",
             url: "Customer.aspx/GetAllCountry",
             contentType: "application/json;charset=utf-8",
             data: "{}",
             dataType: "json",
             success: function (msg) {
                 $("#ddlEditCountry").kendoDropDownList({
                     optionLabel: "- Select Country -",
                     dataTextField: "Name",
                     dataValueField: "CountryID",
                     dataSource: jQuery.parseJSON(msg.d),
                     change: onEditChange,


                 }).data("kendoDropDownList");
             },
             error: function (x, e) {
                 alert("The call to the server side failed. "
                       + x.responseText);
             }
         }
   );


}


function onEditChange(e) {

    if ($("#ddlEditCountry").data("kendoDropDownList").text() != "India") {
        var Statelist = $("#ddlEditState").data("kendoDropDownList");
        Statelist.wrapper.hide();
        var Citylist = $("#ddlEditCity").data("kendoDropDownList");
        Citylist.wrapper.hide();
        document.getElementById("ctl00_ContentPlaceHolder1_lblEditState").style.display = 'none';
        document.getElementById("ctl00_ContentPlaceHolder1_lblEditCity").style.display = 'none';
    }
    else
    {
          var Statelist1 = $("#ddlEditState").data("kendoDropDownList");
        Statelist1.wrapper.show();
       Statelist1.readonly(false);
        var Citylist1 = $("#ddlEditCity").data("kendoDropDownList");
        Citylist1.wrapper.show();
       Citylist1.readonly(false);
      
        $("#ctl00_ContentPlaceHolder1_lblEditState").show();
        $("#ctl00_ContentPlaceHolder1_lblEditCity").show();
    }
};
function BindState() {
    $.ajax(
         {
             type: "POST",
             url: "Customer.aspx/GetAllState",
             contentType: "application/json;charset=utf-8",
             data: "{}",
             dataType: "json",
             success: function (msg) {
                 $("#ddlState").kendoDropDownList({
                     optionLabel:"- Select State -",
                     dataTextField: "StateName",
                     dataValueField: "StateID",
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

function BindCity() {
    $.ajax(
         {
             type: "POST",
             url: "Customer.aspx/GetAllCity",
             contentType: "application/json;charset=utf-8",
             data: "{}",
             dataType: "json",
             success: function (msg) {
                 $("#ddlCity").kendoDropDownList({
                     optionLabel: "- Select City -",
                     dataTextField: "CityName",
                     dataValueField: "CityID",
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


function EditBindState() {
    $.ajax(
         {
             type: "POST",
             url: "Customer.aspx/GetAllState",
             contentType: "application/json;charset=utf-8",
             data: "{}",
             dataType: "json",
             success: function (msg) {
                 $("#ddlEditState").kendoDropDownList({
                     optionLabel: "- Select State -",
                     dataTextField: "StateName",
                     dataValueField: "StateID",
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

function EditBindCity() {
    $.ajax(
         {
             type: "POST",
             url: "Customer.aspx/GetAllCity",
             contentType: "application/json;charset=utf-8",
             data: "{}",
             dataType: "json",
             success: function (msg) {
                 $("#ddlEditCity").kendoDropDownList({
                     optionLabel: "- Select City -",
                     dataTextField: "CityName",
                     dataValueField: "CityID",
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