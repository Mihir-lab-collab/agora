$(document).ready(function () {
    var PrevDate = new Date();
   // PrevDate.setDate(PrevDate.getDate() - 30);
    PrevDate.setFullYear(PrevDate.getFullYear() - 1);
    var todayDate = kendo.toString((new Date()), 'dd/MM/yyyy');

  

    $('[id$="txtFromDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
    $('[id$="txtTODate"]').kendoDatePicker({ format: "dd/MM/yyyy" });

    $("#txtTODate").data("kendoDatePicker").value(todayDate);
    $("#txtFromDate").data("kendoDatePicker").value(PrevDate);

    $('[id$="txtEmail"]').kendoEditor();

    GetInvoices();

});

function SearchInvoices() {
    $(GridInvoices).data().kendoGrid.destroy();
    $(GridInvoices).empty();

    GetInvoices();
}

function GetInvoices() {
    var ProjID = parseInt($('[id$="hdnProjID"]').val());
    var FromDate = $('[id$="txtFromDate"]').val();
    var ToDate = $('[id$="txtTODate"]').val();


    $.ajax({
        type: "POST",
        url: "ProjectPayments.aspx/BindInvoices",
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
//---------tool tip


$(document).kendoTooltip({
    filter: 'span',
    content: function (e) {
        var grid2 = $(GridInvoices).data("kendoGrid");
        var retStr;
        $.each(grid2.columns[0].command, function (index, value) {
            if (e.target.hasClass(value.imageClass)) {
                retStr = value.title;
                return false
            }
        });
        return retStr

    },
    width: 65,
    height:10,
    position: "top"
}).data("kendoTooltip");

function GetProjInvoices() {
    var pid = parseInt($('[id$="hdnProjID"]').val());

    $.ajax({
        type: "POST",
        url: "ProjectPayments.aspx/GetProjectInvoices",
        contentType: "application/json;charset=utf-8",
        data: "{'projID':" + pid + "}",
        dataType: "json",
        async: false,
        success: function (data) {
            var checkdata = jQuery.parseJSON(data.d);
            GetInvoicesData(jQuery.parseJSON(data.d));
            if (checkdata.length == 0) {
                angular.element($("#grdInv")).scope().OpenPopUp();
            }
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetInvoicesData(Idata) {
    $(GridInvoices).kendoGrid({
        dataSource:
            {
                data: Idata,
                schema:
                    {
                        model:
                            {
                                fields:
                                    {
                                        custId: { type: "number", editable: false },
                                        InvoiceDate: { type: "string" },
                                        //ProjectInvoiceID: { type: "number" },
                                        projName: { type: "string" },
                                        Description: { type: "string" },
                                        BalanceAmount: { type: "string" },
                                        Amount: { type: "string" },
                                        TaxCollected: {type: "number"},
                                        ProjID: { type: "string" },
                                        ReceiptDate: { type: "string" },
                                    }
                            }
                    },
                pageSize: 50,
            },
        scrollable: true,
        sortable: true,
        resizable: true,
        pageable:
            {
                input: true,
                numeric: false
            },

        columns: [
                    {
                        command: [
                            {
                                name: "edit", text: "", click: EditInvoice, imageClass: "ViewData", title: "View Details"

                            },
                            
                        ],
                        width: "25px"
                    },
                    { field: "InvoiceDate", title: "Paid Date", width: "40px", format: "{0:dd-MMM-yyyy}" },
                   // { field: "ProjectInvoiceID", title: "Invoice No", width: "70px", template: "DWT/" + "#= ProjectInvoiceID # " },
                    { field: "ProjID", title: "Project Id", width: "50px", hidden: true },
                    {
                        field: "projName", title: "Project Name", width: "90px",
                        template: '<a href="../Member/Project.aspx?Customer=#=custId#"">#= projName #</a>'
                    },
                    //{
                    //    field: "custName", title: "Customer Name", width: "70px",
                    //    template: '<a href="../Member/Project.aspx?Customer=#=custId#"">#= custName #</a>'
                    //},
                    {
                        field: "Amount", title: "Amount", width: "70px",
                        template: '<div class="ra" style="text-align:right;">#= kendo.toString(Amount,"n0") #</div>'
            },
            {
                field: "Tax Collected", title: "TaxCollected", width: "70px",
                template: '<div class="ra" style="text-align:right;">#= kendo.toString(TaxCollected,"n0") #</div>'
            },
                    {
                        field: "BalanceAmount", title: " Credit Amount", width: "70px",
                        template: '<div class="ra" style="text-align:right;">#= kendo.toString(BalanceAmount,"n0") #</div>'
                    },
                    { field: "Description", title: " Payment Mode", width: "50px" },
                    { field: "ReceiptDate", title: " Receipt Sent Date", width: "50px" }

        ],
        filterable:
            {
                extra: false,
                operators:
                    {
                        string:
                            {
                                startswith: "Starts with",
                                contains: "Contains",
                                eq: "Is equal to"
                            }
                    }
            }
    });
}

function EditInvoice(e) {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
    customRowDataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var data = this.dataItem(customRowDataItem);
    var pinvoiceid = customRowDataItem.ProjectInvoiceID;
    var PaymentID = customRowDataItem.PaymentID;
    var ProjID = customRowDataItem.ProjID;
    $('[id$="hdnProjID"]').val(ProjID);
    angular.element($("#grdInv")).scope().EditInvoicePayment(PaymentID);

}

function openPopUP() {
    //$('#divMail').css('display', '');
    //$('#divAddPopupOverlay').addClass('k-overlay');
    var PaymentID = parseInt($('[id$="hdnProjectInvoiceId"]').val());//parseInt($('[id$="hdnProjID"]').val());

    $.ajax(
          {

              type: "POST",
              url: "ProjectPayments.aspx/GetMailInfo",
              contentType: "application/json;charset=utf-8",
              data: "{'PaymentID':'" + PaymentID + "'}",
              cache: false,
              async: false,
              dataType: "json",
              success: function (msg) {
                  var message = jQuery.parseJSON(msg.d);
                  var custName = message[0].customerName;
                  var cMailID = message[0].customerAddress;
                  //var prjName = message[0].ProjectName;
                  var description = message[0].Description;
                 
                  $('[id$="lblTo"]').text(cMailID);
                  //$('[id$="txtEmail"]').val(description);
                  var editor = $('[id$="txtEmail"]').data("kendoEditor");
                  editor.value(description);

                  $('#divMail').css('display', '');
                  $('#divAddPopupOverlay').addClass('k-overlay');

                  $('body').css('cursor', 'default');

              },
              error: function (msg) {
                  alert("The call to the server side failed."
                        + msg.responseText);
              }
          }
       );

};

function sendmail() {

    var To = $('[id$="lblTo"]').text();
    var Cc = $('[id$="txtCc"]').val();
    var Bcc = $('[id$="txtBcc"]').val();
    var Subject = $('[id$="txtSubject"]').val();
    var editor = $('[id$="txtEmail"]').data("kendoEditor");
    var MsgBody = editor.value();
    //var MsgBody = $('[id$="txtEmail"]').val();
    var PaymentID = parseInt($('[id$="hdnProjectInvoiceId"]').val());
    //data: "{'To':'" + To + "','Cc':'" + Cc + "','Bcc':'" + Bcc + "','Subject':'" + Subject + "','MsgBody':'" + MsgBody + "','PaymentID':'" + PaymentID + "' }",
    $.ajax(
          {

              type: "POST",
              url: "ProjectPayments.aspx/SendMail",
              contentType: "application/json;charset=utf-8",
              data: "{To:'" + To + "',Cc:'" + Cc + "',Bcc:'" + Bcc + "',Subject:'" + Subject + "',MsgBody:'" + MsgBody + "',PaymentID:'" + PaymentID + "' }",
              cache: false,
              async: false,
              dataType: "json",
              success: function (msg) {
                  //var message = jQuery.parseJSON(msg.d);
                  alert("Mail sent successfully.");
                  $('body').css('cursor', 'default');

              },
              error: function (msg) {
                  alert("Mail sent failed."
                        + msg.responseText);
              }
          }
       );
}