$(document).ready(function () {

    GetLocationDetails();
    $("#files").kendoUpload({
        async: {
            saveUrl: "Location.aspx",
            removeUrl: "Location.aspx",
            autoUpload: false
        },
        error: onError,
        select: onSelect,
    });

});
function onSelect(e) {
    return $.map(e.files, function (file) {
        if (file.extension.toLowerCase() == ".png" || file.extension.toLowerCase() == ".gif" || file.extension.toLowerCase() == ".jpeg" || file.extension.toLowerCase() || file.extension.toLowerCase() == ".jpg") {
            return true;
        }
        else {
            alert("Invalid File Format");
            return false;
        }

    }).join(", ");
}
function onError(e) {
    kendoConsole.log("Error (" + e.operation + ") :: " + getFileInfo(e));
}
function GetLocationDetails() {

    $.ajax({
        type: "POST",
        url: "Location.aspx/BindLocation",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetLocationData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}

function GetLocationData(Tdata) {
    $(gridLocation).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        LocationID: { type: "number" },
                        Name: { type: "string" },
                        CityId: { type: "number" },
                        CityName: { type: "string" },
                        Biometric: { type: "string", },
                        LegalName: { type: "string" },
                        Address: { type: "string" },
                        PhoneNo: { type: "string" },
                        Fax: { type: "string" },
                        Logo: { type: "string" },
                        Bank: { type: "string" },
                        BankAccount: { type: "string" },
                        WireDetail: { type: "string" },
                        Keyword: {
                            type: "string",
                            validation:
                            {
                                KeywordValidation: function (input) {
                                    if ((input.is("[name='Keyword']") && input.val() != "")) {
                                        var exists = CheckExistsKeyword(input.val(), $('[id$="hdnLocID"]').val());
                                        if (exists == true) {
                                            input.attr("data-KeywordValidation-msg", "Keyword already exists.");
                                            return false;
                                        }
                                        return true;
                                    }
                                    return true;
                                }
                            }
                        },
                        InvoicePDFConfigID: { type: "number" },
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
            { field: "LocationID", title: "ID", width: "50px" },
            { field: "Name", title: "Name", width: "120px" },
            { field: "Keyword", title: "Keyword", width: "50px" },
            { field: "LegalName", title: "Company", width: "150px" },
            { field: "CityName", title: "City", width: "80px" },
            { field: "PhoneNo", title: "Telephone", width: "80px" },
            { field: "Fax", title: "Fax", width: "100px" },
            { field: "Logo", title: "Logo", width: "112px", template: '<img width="100" height="55" src="/Images/LocationLogo/#=Logo #" alt="No image" />' },
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
            GetDataEdit();
            if (e.model.isNew()) {
                var LocationID = e.model.LocationID;
                $('[id$="hdnLocID"]').val(e.model.LocationID);
                $('[id$="txtEditLocationName"]').val(e.model.Name);
                $('[id$="txtEditCompanyName"]').val(e.model.LegalName);
                $('[id$="txtEditKeyword"]').val(e.model.Keyword);
                $('[id$="txtEditCompanyAddress"]').val(e.model.Address);
                $('[id$="txtEditPhoneNo"]').val(e.model.PhoneNo);
                $('[id$="txtEditFax"]').val(e.model.Fax);
                if (e.model.Logo != "") {
                    $('[id$="logo"]').show();
                    $('[id$="logo"]').append("<img src='/Images/LocationLogo/" + e.model.Logo + "' alt='No image' />");
                }
                else {
                    $('[id$="logo"]').hide();
                }
                $('[id$="txtEditLogo"]').val(e.model.Logo);
                $('[id$="txtEditBank"]').val(e.model.Bank);
                $('[id$="txtEditBankAccount"]').val(e.model.BankAccount);
                $('[id$="txtEditWireDetails"]').val(e.model.WireDetail);
                if (e.model.InvoicePDFConfigID == 0) {
                    $('[id$="txtEditInvoicePDFConfigID"]').val('');
                }
                else {
                    $('[id$="txtEditInvoicePDFConfigID"]').val(e.model.InvoicePDFConfigID);
                }

                $('#chkBiometricEdit').prop('checked', '');
                if (e.model.Biometric == "true") {
                    $('#chkBiometricEdit').prop('checked', 'checked');
                }

                var dropdownlist = $("#drpEditCity").data("kendoDropDownList");
                if (e.model.CityId == 0) {

                    dropdownlist.text(dropdownlist.optionLabel = "Select City");
                    //dropdownlist.value('');
                    dropdownlist.element.val("");
                    dropdownlist.selectedIndex = -1;
                    dropdownlist.dataTextField = "CityName";
                    dropdownlist.dataValueField = "CityId";
                }
                else {
                    dropdownlist.value(e.model.CityId);
                }
                $("#Editfiles").kendoUpload({
                    async: {
                        saveUrl: "Location.aspx",
                        removeUrl: "Location.aspx",
                        autoUpload: false
                    },
                    error: onError,
                    select: onSelect,
                });
                e.container.data("kendoWindow").title('Location Details');
                // ClearTempFilesandSession();

                var width = $("#trLocation").width();
                $(".k-edit-form-container").parent().width(width + 20).data("kendoWindow").center();
                updateButton = e.container.find(".k-grid-update");
                cancelbutton = e.container.find(".k-grid-cancel");
                $("#tdUpdate").append(updateButton);
                $("#tdUpdate").append(cancelbutton);
            }
            else {
                e.container.data("kendoWindow").title('Location Details');
            }
        },
        save: function (e) {
            if (e.model.isNew()) {

                var LocationID = e.model.LocationID;
                var LocationName = $("#txtEditLocationName").val();
                var CityID = $("#drpEditCity").val();
                if (CityID == "") {
                    CityID = "0";
                }
                var CompanyName = $("#txtEditCompanyName").val();
                var CompanyAddress = $('[id$="txtEditCompanyAddress"]').val();
                var PhoneNo = $('[id$="txtEditPhoneNo"]').val();
                var Biometric = $('#chkBiometricEdit').is(':checked');
                var keyword = $('[id$="txtEditKeyword"]').val();
                var Fax = $("#txtEditFax").val();
                var Logo = e.model.Logo;
                var Bank = $("#txtEditBank").val();
                var BankAccount = $("#txtEditBankAccount").val();
                var WireDetails = $("#txtEditWireDetails").val();
                var InvoicePDFConfigID = $("#txtEditInvoicePDFConfigID").val();
                if (InvoicePDFConfigID == "") {

                    InvoicePDFConfigID = "0";
                }
                UpdateLocation(LocationID, LocationName, CityID, Biometric, CompanyName, CompanyAddress, PhoneNo, Fax, Logo, Bank, BankAccount, WireDetails, keyword, InvoicePDFConfigID);
                RedirectPage();
            }
            else {
                id = "0";
            }
        }

    });
}

function UpdateLocation(LocationID, LocationName, CityID, Biometric, CompanyName, CompanyAddress, PhoneNo, Fax, Logo, Bank, BankAccount, WireDetails, keyword, InvoicePDFConfigID) {
    $.ajax(
        {
            type: "POST",
            url: "Location.aspx/UpdateLocation",
            contentType: "application/json;charset=utf-8",
            data: "{'LocationID':'" + LocationID + "','LocationName':'" + LocationName + "','CityID':'" + CityID + "','Biometric':'" + Biometric + "','CompanyName':'" + CompanyName + "','CompanyAddress':'" + CompanyAddress +
                "','PhoneNo':'" + PhoneNo + "','Fax':'" + Fax + "','Logo':'" + Logo + "','Bank':'" + Bank + "','BankAccount':'" + BankAccount + "','WireDetails':'" + WireDetails + "','keyword':'" + keyword + "','InvoicePDFConfigID':'" + InvoicePDFConfigID + "'}",
            cache: false,
            async: false,
            dataType: "json",
            success: function (msg) {
                var message = jQuery.parseJSON(msg.d);
                alert(msg.d);
                $('[id$=hdnLocID]').val('');
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
    var grid = $("#gridLocation").data("kendoGrid");
    grid.refresh();
}
function RedirectPage() {
    window.location.assign("Location.aspx");
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

    var drpCity = $("#drpCity").data("kendoDropDownList");
    drpCity.text(drpCity.options.optionLabel);
    drpCity.element.val("");
    drpCity.selectedIndex = -1

    $("#txtLocationName").val('');
    $("#txtCompanyName").val('');
    $("#txtCompanyAddress").val('');
    $("#txtPhoneNo").val('');
    $("#txtFax").val('');
    $("#txtLogo").val('');
    $("#txtBank").val('');
    $("#txtBankAccount").val('');
    $("#txtWireDetail").val('');
    $("#txtInvoicePDFConfigID").val('');
    $("#lblerrmsgLocationName").html('');
    $("#lblmsgCity").html('');
    $("#lblerrmsgCompanyName").html('');
    $("#lblerrmsgCompanyAddress").html('');
    $("#lblerrmsgPhoneNo").html('');
    $("#lblerrmsgFax").html('');
    $("#lblerrmsgLogo").html('');
    $("#lblerrmsgBank").html('');
    $("#lblerrmsgBankAccount").html('');
    $("#lblerrmsgWireDetails").html('');
}
function InitialiseCity(CityData) {
    var Type = $("#drpCity").kendoDropDownList({
        optionLabel: "Select City",
        dataTextField: "CityName",
        dataValueField: "CityId",
        dataSource: CityData,
    }).data("kendoDropDownList");
}
function InitialiseCityEdit(CityDataEdit) {
    var Type = $("#drpEditCity").kendoDropDownList({
        dataTextField: "CityName",
        dataValueField: "CityId",
        dataSource: CityDataEdit,
    }).data("kendoDropDownList");
}
function GetData() {
    $.ajax(
        {
            type: "POST",
            url: "Location.aspx/FillCityMaster",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (msg) {
                InitialiseCity(jQuery.parseJSON(msg.d));
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
            url: "Location.aspx/FillCityMaster",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                InitialiseCityEdit(jQuery.parseJSON(msg.d));
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
function CheckExistsKeyword(txtKeyword, LocId) {
    var isExist = true;
    $.ajax(
        {
            type: "POST",
            url: "Location.aspx/CheckExistsKeyword",
            contentType: "application/json;charset=utf-8",
            data: "{'LocationId':'" + LocId + "','keyword':'" + txtKeyword + "'}",
            dataType: "json",
            async: false,
            success: function (msg) {
                isExist = msg.d;
            },
            error: function (x, e) {
                alert("The call to the server side failed. "
                    + x.responseText);
            }
        }
    );
    return isExist;
}