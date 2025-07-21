$(document).ready(function () {
    GetSupplierDetails();
});

function GetSupplierDetails() {

    $.ajax({
        type: "POST",
        url: "InventorySupplier.aspx/BindSupplier",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetSupplierData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetSupplierData(Tdata) {
    $(gridSupplier).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        SupplierID: { type: "number" },
                        Name: { type: "string" },
                        City: { type: "string" },
                        State: { type: "string", },
                        CountryID: { type: "number" },
                        Country: { type: "string" },
                        Address: { type: "string" },
                        Mobile: { type: "string" },
                        Email: { type: "string" },
                        SortOrder: { type: "number" }
                    }
                }
            },
            //pageSize: 50,
        },
        scrollable: true,
        sortable: true,
        //pageable: {
        //    input: false,
        //    numeric: false
        //},
        columns: [
             { field: "SupplierID", title: "ID", width: "50px" },
                    { field: "Name", title: "Name", width: "120px" },
                  //  { field: "LegalName", title: "Company", width: "150px" },
                    { field: "CityName", title: "City", width: "100px" },
                    { field: "State", title: "State", width: "100px" },
                    { field: "Country", title: "Country", width: "100px" },
                    { field: "Mobile", title: "Mobile", width: "70px" },
                    { field: "Email", title: "Email", width: "120px" },
                    { field: "SortOrder", title: "SortOrder", width: "50px"},

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
        }
        ,
        edit: function (e) {
            var editWindow = e.container.data("kendoWindow");
            $('#divEdit').show();
            GetDataEdit();
            if (e.model.isNew()) {
             
                var SupplierID = e.model.SupplierID;
                $('[id$="txtEditName"]').val(e.model.Name);
                $('[id$="txtEditCity"]').val(e.model.CityName);
                $('[id$="txtEditState"]').val(e.model.State);
                var dropdownlist = $("#drpEditCountry").data("kendoDropDownList");
                dropdownlist.value(e.model.CountryID);
               // $('[id$="txtEditCountry"]').val(e.model.Country);
                $('[id$="txtEditAddress"]').val(e.model.Address);
                $('[id$="txtEditMobile"]').val(e.model.Mobile);
                $('[id$="txtEditEmail"]').val(e.model.Email);
                $('[id$="txtEditSortOrder"]').val(e.model.SortOrder);

                e.container.data("kendoWindow").title('Supplier Details');
                var width = $("#trSupplier").width();
                $(".k-edit-form-container").parent().width(width + 20).data("kendoWindow").center();
                updateButton = e.container.find(".k-grid-update");
                cancelbutton = e.container.find(".k-grid-cancel");
                $("#tdUpdate").append(updateButton);
                $("#tdUpdate").append(cancelbutton);
            }
            else {
                e.container.data("kendoWindow").title('Supplier Details');
            }
        },
        save: function (e) {
            if (e.model.isNew()) {

                var SupplierID = e.model.SupplierID;
                var Name = $("#txtEditName").val();
                var City = $("#txtEditCity").val();
                var State = $("#txtEditState").val();
                var Country = $("#drpEditCountry").val();
                var Address = $('[id$="txtEditAddress"]').val();
                var Mobile = $('[id$="txtEditMobile"]').val();
                var Email = $("#txtEditEmail").val();
                var SortOrder = $("#txtEditSortOrder").val();

                UpdateSupplier(SupplierID, Name, City, State, Country, Address, Mobile, Email, SortOrder);
                RedirectPage();
            }
            else {
                id = "0";
            }
        }

    });
}

function  UpdateSupplier(SupplierID, Name, City, State, Country, Address, Mobile, Email, SortOrder) {
    $.ajax(
           {
               type: "POST",
               url: "InventorySupplier.aspx/UpdateSupplier",
               contentType: "application/json;charset=utf-8",
               data: "{'SupplierID':'" + SupplierID + "','Name':'" + Name + "','City':'" + City + "','State':'" + State + "','Country':'" + Country + "','Address':'" + Address +
                   "','Mobile':'" + Mobile + "','Email':'" + Email + "','SortOrder':'" + SortOrder + "'}",
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
    var grid = $("#gridSupplier").data("kendoGrid");
    grid.refresh();
}
function RedirectPage() {
    window.location.assign("InventorySupplier.aspx");
}
function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}
function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}

function closeTeamPopUP() {
    $('#divProposal').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}

function ShowAddPopup() {
    openAddPopUP();
    GetData();

    var drpCountry = $("#drpCountry").data("kendoDropDownList");
    drpCountry.text(drpCity.options.optionLabel);
    drpCountry.element.val("");
    drpCountry.selectedIndex = -1

     $("#txtName").val('');
     $("#txtCity").val('');
     $("#txtState").val('');
    // $("#txtCountry").val('');
     $("#txtAddress").val('');
     $("#txtMobile").val('');
     $("#txtEmail").val('');

     $("#lblerrmsgName").html('');
     $("#lblmsgCity").html('');
     $("#lblerrmsgState").html('');
     $("#lblerrmsgCountry").html('');
     $("#lblerrmsgAddress").html('');
     $("#lblerrmsgMobile").html('');
     $("#lblerrmsgEmail").html('');
}
function InitialiseCountry(CityData) {
    var Type = $("#drpCountry").kendoDropDownList({
        optionLabel: "Select Country",
        dataTextField: "Country",
        dataValueField: "CountryID",
        dataSource: CityData,
    }).data("kendoDropDownList");
}
function InitialiseCountryEdit(CityDataEdit) {
    var Type = $("#drpEditCountry").kendoDropDownList({
        dataTextField: "Country",
        dataValueField: "CountryID",
        dataSource: CityDataEdit,
    }).data("kendoDropDownList");
}
function GetData() {
    $.ajax(
          {
              type: "POST",
              url: "InventorySupplier.aspx/FillCountryMaster",
              contentType: "application/json;charset=utf-8",
              dataType: "json",
              success: function (msg) {
                  InitialiseCountry(jQuery.parseJSON(msg.d));
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
              url: "InventorySupplier.aspx/FillCountryMaster",
              contentType: "application/json;charset=utf-8",
              dataType: "json",
              async: false,
              success: function (msg) {
                  InitialiseCountryEdit(jQuery.parseJSON(msg.d));
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
);
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

