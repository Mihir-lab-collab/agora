//Tooltip Changes done by Apurva on 20/01/2016
$(document).kendoTooltip({
    filter: "span", // if we filter as td it shows text present in each td of the table

    content: function (e) {
        var grid2 = $(GridProInvoices).data("kendoGrid");
        var retStr;
        $.each(grid2.columns[0].command, function (index, value) {
            if (e.target.hasClass(value.imageClass)) {
                retStr = value.title;
                return false
            }
        });
        return retStr

    },
    width: 80,
    position: "top"
}).data("kendoTooltip");


$(document).ready(function () {
    $('[id$="txtEmail"]').kendoEditor();
    GetProjInvoices();
});

function ChangeDate() {
    var dt = $("#txtInvoiceDate").val();
    $("#txtPaymentDate").data("kendoDatePicker").value(dt);
}

function GetProjInvoices() {
    var status = $('[id$="hdnStatus"]').val();
    $.ajax({
        type: "POST",
        url: "ProformaInvoices.aspx/GetProformaInvoices",
        contentType: "application/json;charset=utf-8",
        data: "{'status':'" + status + "'}",
        dataType: "json",
        async: false,
        success: function (data) {
            var checkdata = jQuery.parseJSON(data.d);
            GetInvoicesData(jQuery.parseJSON(data.d));
            if (data.status = 'Invoiced') {

            }
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

var IsVoid = "0";

function GetInvoicesData(Idata) {
    $(GridProInvoices).kendoGrid({
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
                                        InvoiceDate: { type: "date", format: "{0:dd-MMM-yyyy}" },
                                        ProformaInvoiceID: { type: "number" },
                                        projName: { type: "string" },
                                        custName: { type: "string" },
                                        currSymbol: { type: "string" },
                                        ExRate: { type: "number" },
                                        Amount: { type: "string" },
                                        InvoiceNo: { type: "string" },
                                        //DueDate: { type: "string" },
                                        Inv_LocationID: { type: "int" },
                                        Inv_LocationName: { type: "string" },
                                        // Inv_CurBalanceAmount: { type: "string" },
                                        Inv_Delay: { type: "string" },
                                        Status: { type: "string" },
                                        EmailSentDate: { type: "string" },

                                        TaxInvoice: { type: "string" },
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
                          //Tooltip Changes done by Apurva on 20/01/2016
                          command: [
                              {
                                  name: "edit", text: "", title: "Edit", Class: "test", imageClass: "k-icon k-i-pencil", click: EditInvoice
                              },
                              {
                                  name: "tax_invoice", text: "", title: "Tax Invoice", Class: "test", imageClass: "k-icon TaxInvoice", click: TaxInvoice,
                              },
                              {
                                  name: "pdf", text: "", title: "Pdf", Class: "test", imageClass: "k-icon ViewPDF", click: InvoicePDF
                              },
                              {
                                  name: "mail", text: "", title: "Mail", Class: "test", imageClass: "k-icon mail", click: GetMailInfo
                              },

                          ],
                          width: "55px"
                          //Tooltip Changes done by Apurva on 20/01/2016
                      },
                      { field: "InvoiceNo", title: "Invoice No", width: "70px", hidden: true },
                       { field: "ProformaInvoiceID", title: "Invoice No", width: "45px", template: "#= ProformaInvoiceID # " }, //"DWT/" +
                     // { field: "DueDate", title: "Due Date", width: "70px", format: "{0:dd-MMM-yyyy}" },

                      { field: "ProjID", title: "Project Id", width: "50px", hidden: true },
                      {
                          field: "projName", title: "Project Name", width: "90px",
                          template: '<a href="../Member/Project.aspx?Customer=#=custId#"">#= projName #</a>'
                      },
                      {
                          field: "custName", title: "Customer Name", width: "70px",
                          template: '<a href="../Member/Project.aspx?Customer=#=custId#"">#= custName #</a>'
                      },
                      { field: "Inv_LocationName", title: "Location", width: "50px" },
                      {
                          field: "Amount", title: "Total Amount", width: "70px",
                          template: '<div class="ra" style="text-align:right;">#= kendo.toString(Amount,"n0") #</div>'
                      },
                      //{
                      //   field: "Inv_CurBalanceAmount", title: "Balance Amount", width: "70px",
                      //   template: '<div class="ra" style="text-align:right;">#= kendo.toString(Inv_CurBalanceAmount,"n0") #</div>'
                      //},
                      { field: "Inv_Delay", title: "Delay", width: "40px" },
                      { field: "Status", title: "Status", width: "40px" },
                      { field: "EmailSentDate", title: "Invoice Sent Date", width: "55px" },

                      { field: "TaxInvoice", title: "Tax Invoice", width: "75px", template: '<a href="../Member/ProjectInvoices.aspx">#= TaxInvoice #</a>' },//?InvoiceNo=#=InvoiceNo#"
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
            },

        // Change done for Disable taxinvoice button if invoiced  by Apurva on 18/03/2016
        dataBound: function () {
            var grid = this;
            var model;

            grid.tbody.find("tr[role='row']").each(function () {
                model = grid.dataItem(this);

                if (model.Status == "Pending") {

                }
                else {
                    $(this).find(".k-grid-tax_invoice").remove();
                }
            });

        }




        //Tooltip Changes done by Apurva on 20/01/2016
    }).data("kendoGrid");

}


var companyLogo = "";

function InvoicePDF(e) {

    customRowDataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var data = this.dataItem(customRowDataItem);
    var pinvoiceid = customRowDataItem.ProformaInvoiceID;
    var InvoiceNo = customRowDataItem.InvoiceNo;
    var LocationID = customRowDataItem.Inv_LocationID;
    var ProjectName = customRowDataItem.projName;
    $('[id$="hdnProformaInvoiceId"]').val(pinvoiceid);

    if (confirm("Click Ok button to download pdf with logo or Cancel button to download pdf without logo."))
        companyLogo = "Yes";
    else
        companyLogo = "";
    GeneratePDF(pinvoiceid, InvoiceNo, 1, LocationID, ProjectName);
}

function GeneratePDF(pinvoiceid, InvoiceNo, pdf, LocationID, ProjectName) {
    $.ajax(
        {

            type: "POST",
            url: "ProformaInvoices.aspx/GeneratePDF",
            contentType: "application/json;charset=utf-8",
            data: "{'pinvoiceid':'" + pinvoiceid + "','InvoiceNo':'" + InvoiceNo + "','InvoiceProjectName':'" + ProjectName + "'}",
            cache: false,
            async: false,
            dataType: "json",
            success: function (msg) {
                if (pdf == 0)
                    window.location.assign("GeneratePDF.aspx?P=" + pdf + "&LocationID=" + LocationID + "&Logo=" + companyLogo);
                else
                    window.open("GeneratePDF.aspx?P=" + pdf + "&LocationID=" + LocationID + "&Logo=" + companyLogo, "_blank");
            },
            error: function (msg) {
                alert("The call to the server side failed."
                      + msg.responseText);
            }
        }
     );
}

function GetMailInfo(e) {
    customRowDataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var data = this.dataItem(customRowDataItem);
    var pinvoiceid = customRowDataItem.ProformaInvoiceID;
    var InvoiceNo = customRowDataItem.InvoiceNo;
    var LocationID = customRowDataItem.Inv_LocationID;
    var ProjectName = customRowDataItem.projName;
    $('[id$="hdnProformaInvoiceId"]').val(pinvoiceid);

    companyLogo = "Yes";
    GeneratePDF(pinvoiceid, InvoiceNo, 0, LocationID, ProjectName);// to generate pdf and add it in email attachment
    BindMail(pinvoiceid)

    ShowEditor();
}

function ShowEditor() {

}

var InvoiceNo;
function BindMail(pinvoiceid) {
    $.ajax(
          {

              type: "POST",
              url: "ProformaInvoices.aspx/GetMailInfo",
              contentType: "application/json;charset=utf-8",
              data: "{'pinvoiceid':'" + pinvoiceid + "'}",
              cache: false,
              async: false,
              dataType: "json",
              success: function (msg) {
                  var message = jQuery.parseJSON(msg.d);
                  var custName = message[0].custName;
                  var cMailID = message[0].custAddress;
                  var prjName = message[0].projName;
                  var description = message[0].Comment;
                  var emailCC = message[0].custCompany;
                  $('[id$="hdnInvoiceNo"]').val(message[0].InvoiceNo);
                  var subject = "Invoice #" + message[0].InvoiceNo + " for " + message[0].projName;
                  $('[id$="lblTo"]').text(cMailID);
                  $('[id$="txtCc"]').val(emailCC);
                  $('[id$="txtSubject"]').val(subject);
                  var editor = $('[id$="txtEmail"]').data("kendoEditor");
                  editor.value(description);
                  $('[id$="dvAttachnemt"]').html("");
                  if (message.length > 0) {
                      for (i = 0; i < message.length; i++) {

                          var Anchor = $('<a target="_blank" href="../Common/Download.aspx?m=INV&f=' + message[0].DueDate + '">' + message[0].DueDate + '</a>');

                          $('[id$="dvAttachnemt"]').append(Anchor);
                          $('[id$="dvAttachnemt"]').append('<br/>');
                      }
                  }
                  else {
                      var Anchor = "No Attachment."
                      $('[id$="dvAttachnemt"]').append(Anchor);
                      $('[id$="dvAttachnemt"]').addClass("ForeColor");
                  }


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
}

function BindInvoiceStatus(pInvId) {
    $.ajax({
        type: "POST",
        url: "ProformaInvoices.aspx/GetInvoiceStatus",
        contentType: "application/json;charset=utf-8",
        data: "{'pInvId':" + pInvId + "}",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.d.length > 0)
                SetInvoiceStatus(jQuery.parseJSON(data.d));
            else
                $("#GridInvoiceStatus").html('');
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function EditInvoice(e) {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
    customRowDataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var data = this.dataItem(customRowDataItem);
    var pinvoiceid = customRowDataItem.ProformaInvoiceID;
    $('[id$="lblProject"]').html(customRowDataItem.projName);
    IsVoid = customRowDataItem.IsVoid;
    // var val = $('[id$="hdnProjID"]').val(); //ProjectId;
    var val = customRowDataItem.ProjID
    // var locationID = $('[id$="hdnLocationId"]').val(); //LocationID;
    var locationID = customRowDataItem.Inv_LocationID
    val = val + '@' + locationID;
    angular.element($("#grdInv")).scope().EditInvoice(pinvoiceid, val);

    if (IsVoid == "True") {
        $('#ctl00_ContentPlaceHolder1_btnSave').hide();
        $('#ctl00_ContentPlaceHolder1_btnVoid').hide();
    }
}

function TaxInvoice(e) {
    if (confirm('Are You Sure to Create Tax Invoice?')) {
        
        // added by AP

        if ($('[id$="hdnProjID"]').val() == "0" || $('[id$="hdnLocationId"]').val() == "0") {
            alert("Please select both Project and Location");
            return;
        }
        else {
            $('#divAddTaxInvPopup').css('display', '');
            $('#divAddTaxPopupOverlay').addClass('k-overlay');
            customRowDataItem = this.dataItem($(e.currentTarget).closest("tr"));
            var data = this.dataItem(customRowDataItem);
            var pinvoiceid = customRowDataItem.ProformaInvoiceID;
            $('[id$="lblProjectID"]').html(customRowDataItem.projName);
            IsVoid = customRowDataItem.IsVoid;

            angular.element($("#grdInv")).scope().CreateTaxInvoice(pinvoiceid);
        }
    }
}

function BindHTML(Tdata) {
    $('#divSummary').css('display', '');
    var strHTML = '';

    strHTML = "<table width=\"250\" cellpadding=\"0\" cellspacing=\"0\">";
    strHTML = strHTML + "<tr><td colspan=\"2\">Payment Summary</td></tr>"
    strHTML = strHTML + "<tr><td style=\" width:74px;\">Date</td><td>paid Amount</td></tr>";
    strHTML = strHTML + "<tr><td colspan=\"2\">";
    strHTML = strHTML + "<div style=\"overflow-y:auto;height:66px;\">";
    strHTML = strHTML + "<table>";
    for (var i = 0; i < Tdata.length; i++) {
        strHTML = strHTML + "<tr><td>" + Tdata[i].DueDate + "</td><td>" + Tdata[i].Amount + "</td></tr>";
    }
    strHTML = strHTML + "</table>";
    strHTML = strHTML + " </div>";
    strHTML = strHTML + "</td> </tr>";

    strHTML = strHTML + "</table>";
    $('[id$="divSummary"]').html(strHTML);
}

function SetInvoiceStatus(Tdata) {
    if (Tdata.length > 0) {
        $('[id$="lblStatus"]').text(Tdata[0].custName);
        ShowHideBtn();

        BindHTML(Tdata);
    }
    else {
        $('[id$="lblStatus"]').text('UnPaid');
        $('#divSummary').css('display', 'none');
    }

    $(GridInvoiceStatus).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {

                        DueDate: { type: "string" },
                        Amount: { type: "string" }
                    }
                }
            },
            paging: true,
            pageSize: 2,
        },
        scrollable: false,
        sortable: true,

        pageable: {

            messages: {
                empty: ""
            }
        },
        columns: [

                    { field: "DueDate", title: "PaymentDate", width: "50px" },
                    { field: "Amount", title: "Paid Amount", width: "150px" },
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


    });

}

function ShowHideBtn() {
    if (IsVoid == "True") {
        $('[id$="btnSave"]').hide();
        $('[id$="btnVoid"]').hide();
        $('#divSummary').css('display', 'none');
    }
    else {
        $('[id$="btnSave"]').show();
        $('[id$="btnVoid"]').show();
        $('#divSummary').css('display', '');

    }
}

function CloseMailBox() {
    $('#divMail').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");


}

function sendmail() {

    var To = $('[id$="lblTo"]').text();
    var Cc = $('[id$="txtCc"]').val();
    var Bcc = $('[id$="txtBcc"]').val();
    var Subject = $('[id$="txtSubject"]').val();
    var editor = $('[id$="txtEmail"]').data("kendoEditor");
    var MsgBody = editor.value();

    var pInvId = parseInt($('[id$="hdnProformaInvoiceId"]').val());
    var InvoiceNo = $('[id$="hdnInvoiceNo"]').val();
    $.ajax(
          {

              type: "POST",
              url: "ProformaInvoices.aspx/SendMail",
              contentType: "application/json;charset=utf-8",
              data: "{To:'" + To + "',Cc:'" + Cc + "',Bcc:'" + Bcc + "',Subject:'" + Subject + "',MsgBody:'" + MsgBody + "',InvoiceNo:'" + InvoiceNo + "',pInvId:'" + pInvId + "' }",
              cache: false,
              async: false,
              dataType: "json",
              success: function (msg) {
                  var message = msg.d;
                  if (message != 'Mail Sent')
                      alert("Mail sent Failed.");
                  else
                      alert("Mail sent successfully.");
                  $('#divMail').css('display', 'none');
                  $('body').css('cursor', 'default');

              },
              error: function (msg) {
                  alert("Mail sent failed."
                        + msg.responseText);
              }
          }
       );
}

