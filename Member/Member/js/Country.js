$(document).ready(function () {

    GetCountrydetails();

});

function GetCountrydetails() {
    $.ajax({
        type: "POST",
        url: "Country.aspx/BindCountry",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        dataType: "json",
        async: false,
        success: function (msg) {
            //console.log("my data");
            //console.log(jQuery.parseJSON(msg.d));
            GetDesignationData(jQuery.parseJSON(msg.d));            
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}

function ClosingRateWindow(e) {

    var grid = $(gridCountry).data("kendoGrid");
    grid.refresh();

}
function GetDesignationData(Tdata) {
    $(gridCountry).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        CountryID: { type: "number" },
                        Country: { type: "string" },

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
            { field: "CountryID", title: "CountryID", width: "50px", hidden: true },
            { field: "Country", title: "Country", width: "80px" },
            { command: [{ name: "edit" }], width: "10px" }
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
        }
        ,
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
                // FillTextEdit();

                var CountryID = e.model.CountryID;
                $('[id$="hdnCountryID"]').val(CountryID);

                var CountryName = e.model.Country;

                $('[id$="txtEditCountryName"]').val(CountryName);

                e.container.data("kendoWindow").title('Country Details');
                var width = $("#trConfig").width();
                $(".k-edit-form-container").parent().width(width + 20).data("kendoWindow").center();
                updateButton = e.container.find(".k-grid-update");
                cancelbutton = e.container.find(".k-grid-cancel");
                $("#tdUpdate").append(updateButton);
                $("#tdUpdate").append(cancelbutton);
            }
            else {
                e.container.data("kendoWindow").title('Country Details');
            }
        },
        save: function (e) {
            if (e.model.isNew()) {
                var CountryID = $('[id$="hdnCountryID"]').val();
                var CountryName = $('[id$="txtEditCountryName"]').val();
                var verror = $("#lblError");

                if (CountryName == '') {
                    //verror.html("Designation cannot be blank.");
                    //alert("Designation cannot be blank.");
                    //return false;
                }
                else {
                    //verror.html("");
                    UpdateCountry(CountryID, CountryName);
                    RedirectPage();
                }
            }
            else {
                id = "0";

            }
        }

    });

}

function UpdateCountry(CountryID, CountryName) {

    $.ajax(
        {

            type: "POST",
            url: "Country.aspx/UpdateCountry",
            contentType: "application/json;charset=utf-8",
            data: "{'CountryID':'" + CountryID + "','CountryName':'" + CountryName + "'}",
            cache: false,
            async: false,
            dataType: "json",
            success: function (msg) {
                console.log("update ====");
                console.log(msg);
                console.log(jQuery.parseJSON(msg.d));
                var message = jQuery.parseJSON(msg.d);
                
                if (message == "2") {
                    alert("Something Went Wrong..\n Please contact to Administrator..");
                }                
                else {
                    if (message == "3") {
                        alert("Country already Exist..");
                    }
                    else {
                        alert('updated successfully.');
                    }
                    
                }
                
                $('body').css('cursor', 'default');

            },
            error: function (msg) {
                alert("The call to the server side failed."
                    + msg.responseText);
            }
        }
    );
}
function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    window.location.reload();
}

function ShowAddPopup() {
    openAddPopUP();
    $("#txtEditConfigID").val('');
}
function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}

function RedirectPage() {
    window.location.assign("Country.aspx");
}