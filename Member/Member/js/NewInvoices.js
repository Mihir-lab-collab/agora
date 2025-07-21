$(document).ready(function () {

   // $('#fileupload').kendoUpload();
    //$("#fileupload").kendoUpload({
    //    multiple: false
    //});


    $('[id$="txtEmail"]').kendoEditor();


    GetProjInvoices();

    $("#divYes").click(function () {

        companyLogo = "Yes";
        CloseConfirm();
        GeneratePDF(pdfInvoicID, pdfInvoiceNo, 1, pdfLocationID, pdfProjectName);
    });

    $("#divNo").click(function () {
        companyLogo = "";
        CloseConfirm();
        GeneratePDF(pdfInvoicID, pdfInvoiceNo, 1, pdfLocationID, pdfProjectName);
    });
  
});

function GetGSTPercent(CodeID) {
    $.ajax({
        type: "POST",
        url: "ProformaInvoices.aspx/GetGSTPercent",
        contentType: "application/json;charset=utf-8",
        data: "{CodeID:'" + CodeID + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            var JSONData = jQuery.parseJSON(msg.d);
            var Percent = JSONData[0].GSTPercentage

            $('[id$="hdnProjectGSTPercentage"]').val(Percent);


        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}

function ChangeDate() {
    //alert(2);
    var dt = $("#txtInvoiceDate").val();
    $("#txtPaymentDate").data("kendoDatePicker").value(dt);
}

function GetProjInvoices() {
    //var pid = parseInt($('[id$="hdnProjID"]').val());
    //var locid = parseInt($('[id$="hdnLocationId"]').val());
    var status = $('[id$="hdnStatus"]').val();

    $.ajax({
        type: "POST",
        url: "ProjectInvoices.aspx/GetProjectInvoices",
        contentType: "application/json;charset=utf-8",
        data: "{'status':'" + status + "'}",
        //data: "{'isActive':'" + isactive + "','locationId':'" + LocationId + "','EmpId':'" + EmpId + "'}",
        dataType: "json",
        async: false,
        success: function (data) {
            var checkdata = jQuery.parseJSON(data.d);

            // var ProjInvoice = GetBalanceAmount(jQuery.parseJSON(data.d)); //added by AP
            // GetInvoicesData(ProjInvoice); //added by AP

            GetInvoicesData(jQuery.parseJSON(data.d));  //cmmnt by AP
            //if (checkdata.length == 0) { 
            //if ($('[id$="hdnProjID"]').val() != "0" && $('[id$="hdnLocationId"]').val() != "0" && checkdata.length == 0) {
            //    angular.element($("#grdInv")).scope().OpenPopUp();
            //}
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}
var IsVoid = "0";


//******************************************added by AP
//function GetBalanceAmount(projinvoice)
//{
//    //for (var i = 0; i < empData.length ; i++) {
//    //    var exp = empData[i].empExperince;
//    //    var jDate = empData[i].Event;
//    //    var d1 = new Date(Date.parse(jDate));
//    //    var d2 = new Date();
//    //    var months = '';
//    //    months = (d2.getFullYear() - d1.getFullYear()) * 12;
//    //    months -= d1.getMonth() + 1;
//    //    months += d2.getMonth();
//    //    months = months <= 0 ? 0 : months + 1;
//    //    exp = exp + months;
//    //    empData[i].Type = (Math.floor(exp / 12)).toString() + " yrs - " + (exp % 12).toString() + " mnths";
//    //}
//    return projinvoice;
//}
//******************************************added by AP
function OnGridDataBound(e) {
    var grid = $("#gridInvoices").data("kendoGrid");
    var gridData = grid.dataSource.view();
    for (var i = 0; i < gridData.length; i++) {
        var currentUid = gridData[i].uid;
        if (gridData[i].Status == "Full Paid") {
            var currentRow = grid.table.find("tr[data-uid='" + currentUid + "']");
            var createUserButton = $(currentRow).find(".k-grid-Reminder");
            createUserButton.hide();
        }
    }
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
                        Reminder_text: { type: "string", editable: false },
                        InvoiceDate: { type: "date", format: "{0:dd-MMM-yyyy}" },
                        ProjectInvoiceID: { type: "number" },
                        projName: { type: "string" },
                        custName: { type: "string" },
                        currSymbol: { type: "string" },
                        ExRate: { type: "number" },
                        Amount: { type: "string" },
                        InvoiceNo: { type: "string" },
                        DueDate: { type: "string" },//{ type: "date", format: "{0:dd-MMM-yyyy}" },
                        Inv_LocationID: { type: "int" },
                        Inv_LocationName: { type: "string" },
                        Inv_CurBalanceAmount: { type: "string" },
                        Inv_Delay: { type: "string" },
                        Status: { type: "string" },
                        EmailSentDate: { type: "string" },
                        TDSCheck: { type: "number" },
                    }
                }
            },
            pageSize: 50,
        },
        dataBound: OnGridDataBound,
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
                        name: "edit", text: "", click: EditInvoice, title: "Edit"
                    },
                    {
                        name: "mail", text: "", click: GetMailInfo, title: "mail", className: "mail", imageClass: "mail"

                    },
                    {
                        name: "pdf", text: "", click: InvoicePDF, title: "pdf", className: "ViewPDF", imageClass: "ViewPDF"
                    },
                    {
                        name: "Reminder", text: "", click: InvoiceDueReminder, title: "InvoiceDueReminder", className: "Reminder", imageClass: "Reminder"
                    },
                ],
                width: "82px"
            },
            {
                field: "Reminder_text", title: "Reminder Sent DateTime", width: "150px", sortable: true
            },
            { field: "InvoiceNo", title: "Invoice No", width: "70px" },

            { field: "DueDate", title: "Due Date", width: "70px", format: "{0:dd-MMM-yyyy}" },
            { field: "ProjectInvoiceID", title: "Invoice No", width: "70px", template: "DWT/" + "#= ProjectInvoiceID # ", hidden: true },
            { field: "ProjID", title: "Project Id", width: "50px", hidden: true },
            {
                field: "projName", title: "Project Name", width: "90px",
                template: '<a href="../Member/Project.aspx?Customer=#=custId#"">#= projName #</a>'
            },
            {
                field: "custName", title: "Customer Name", width: "70px",
                template: '<a href="../Member/Project.aspx?Customer=#=custId#"">#= custName #</a>'
            },
            { field: "Inv_LocationName", title: "Location", width: "70px" },
            {
                field: "Amount", title: "Total Amount", width: "70px",
                template: '<div class="ra" style="text-align:right;">#= kendo.toString(Amount,"n0") #</div>'
            },
            {
                field: "Inv_CurBalanceAmount", title: "Balance Amount", width: "70px",
                template: '<div class="ra" style="text-align:right;">#= kendo.toString(Inv_CurBalanceAmount,"n0") #</div>'
            },
            { field: "Inv_Delay", title: "Delay", width: "70px" },
            { field: "Status", title: "Status", width: "50px" },
            { field: "EmailSentDate", title: "Invoice Sent Date", width: "75px" },
            { field: "TDSCheck", title: "TDSCheck", width: "35px",  hidden: true },

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
var companyLogo = "";
var pdfInvoicID = "";
var pdfInvoiceNo = "";
var pdfLocationID = "";
var pdfProjectName = "";

function InvoicePDF(e) {

    customRowDataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var data = this.dataItem(customRowDataItem);
    var pinvoiceid = pdfInvoicID = customRowDataItem.ProjectInvoiceID;
    var InvoiceNo = pdfInvoiceNo = customRowDataItem.InvoiceNo;
    var LocationID = pdfLocationID = customRowDataItem.Inv_LocationID;
    var ProjectName = pdfProjectName = customRowDataItem.projName;
    $('[id$="hdnProjectInvoiceId"]').val(pinvoiceid);

    DivConfirm();

}
function InvoiceDueReminder(e) {
    var customRowDataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var data = this.dataItem(customRowDataItem);
    var pinvoiceid = customRowDataItem.ProjectInvoiceID;
    var InvoiceNo = customRowDataItem.InvoiceNo;
    var LocationID = customRowDataItem.Inv_LocationID;
    var ProjectName = customRowDataItem.projName;
    var Inv_Delay = customRowDataItem.Inv_Delay;
    $('[id$="hdnProjectInvoiceId"]').val(pinvoiceid);
    companyLogo = "Yes";
    //GeneratePDF(pinvoiceid, InvoiceNo, 0, LocationID, ProjectName);// to generate pdf and add it in email attachment
    BindMail_PaymentDue(pinvoiceid, Inv_Delay)
    ShowEditor();
}
function DivConfirm() {
    $('[id$="divConfirm"]').css('display', 'block');
}
function CloseConfirm() {
    $('[id$="divConfirm"]').css('display', 'none');
}
function GeneratePDF(pinvoiceid, InvoiceNo, pdf, LocationID, ProjectName) {
    $.ajax(
        {

            type: "POST",
            url: "ProjectInvoices.aspx/GeneratePDF",
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
    var customRowDataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var data = this.dataItem(customRowDataItem);
    var pinvoiceid = customRowDataItem.ProjectInvoiceID;
    var InvoiceNo = customRowDataItem.InvoiceNo;
    var LocationID = customRowDataItem.Inv_LocationID;
    var ProjectName = customRowDataItem.projName;
    $('[id$="hdnProjectInvoiceId"]').val(pinvoiceid);

    companyLogo = "Yes";
    GeneratePDF(pinvoiceid, InvoiceNo, 0, LocationID, ProjectName);// to generate pdf and add it in email attachment
    BindMail(pinvoiceid)

    ShowEditor();
}
function ShowEditor() {
    //$('[id$="txtEmail"]').kendoEditor();
}
var InvoiceNo;
function BindMail(pinvoiceid) {
    $.ajax({
        type: "POST",
        url: "ProjectInvoices.aspx/GetMailInfo",
        contentType: "application/json;charset=utf-8",
        data: "{'pinvoiceid':'" + pinvoiceid + "','Inv_Delay':''}",
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
            //$('[id$="txtEmail"]').val('');

            var editor = $('[id$="txtEmail"]').data("kendoEditor");
            editor.value(description);
            //$('[id$="txtEmail"]').val(description);

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
    });
}

function BindInvoiceStatus(pInvId) {
    $.ajax({
        type: "POST",
        url: "ProjectInvoices.aspx/GetInvoiceStatus",
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
    var pinvoiceid = customRowDataItem.ProjectInvoiceID;
    $('[id$="lblProject"]').html(customRowDataItem.projName);
    IsVoid = customRowDataItem.IsVoid;
    angular.element($("#grdInv")).scope().EditInvoice(pinvoiceid);

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
        //$('[id$="lblStatus"]').text(IStatus);
        //dynamic bind invoiec payment summary
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
                        //projectstartdate: { type: "date" },
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
        //height: 600,
        //toolbar: ["create"],
        pageable: {
            //input: false,
            //numeric: false
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
        //editable: false,
        //editable: {
        //    mode: "popup",
        //    template: kendo.template($("#popup-editor").html())
        //},
        //cancel: function (e) {
        //    e.preventDefault()
        //    ClosingRateWindow(e);
        //},
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
    var pInvId = parseInt($('[id$="hdnProjectInvoiceId"]').val());
    var InvoiceNo = $('[id$="hdnInvoiceNo"]').val();
    //debugger
    //var p_sendmail = {};
    //p_sendmail.To = To;
    //p_sendmail.Cc = Cc;
    //p_sendmail.Bcc = Bcc;
    //p_sendmail.Subject = Subject;
    //p_sendmail.editor = editor;
    //p_sendmail.MsgBody = MsgBody;
    //p_sendmail.pInvId = pInvId;
    //p_sendmail.InvoiceNo = InvoiceNo;

    //var frm_data = new FormData();
    //frm_data.append("To", To);
    //frm_data.append("Cc", Cc);
    //frm_data.append("Bcc", Bcc);
    //frm_data.append("Subject", Subject);
    //frm_data.append("MsgBody", MsgBody);
    //frm_data.append("pInvId", pInvId);
    //frm_data.append("InvoiceNo", InvoiceNo);

    //var fileUpload = $("#file").get(0);
    //var files = fileUpload.files;
    //for (var i = 0; i < files.length; i++) {
    //    frm_data.append("File", files[i]);
    //    p_sendmail.File = files[i];
    //}
    //debugger
    //frm_data.append("p_sendmail", p_sendmail);
    //data: "{To:'" + To + "',Cc:'" + Cc + "',Bcc:'" + Bcc + "',Subject:'" + Subject + "',MsgBody:'" + MsgBody + "',InvoiceNo:'" + InvoiceNo + "',pInvId:'" + pInvId + "' }",

    //To = "arun.s@intelegain.com";
    $.ajax({
        type: "POST",
        url: 'ProjectInvoices.aspx/SendMail',
        contentType: "application/json;charset=utf-8",
        data: "{To:'" + To + "',Cc:'" + Cc + "',Bcc:'" + Bcc + "',Subject:'" + Subject + "',MsgBody:'" + MsgBody + "',InvoiceNo:'" + InvoiceNo + "',pInvId:'" + pInvId + "' }",
        dataType: 'json',
        async: false,
        success: function (msg) {
            console.log("msg" + msg);
            var message = msg.d;
            if (message != 'Mail Sent')
                alert("Mail sent Failed.");
            else
                alert("Mail sent successfully.");
            location.reload();
            $('#divMail').css('display', 'none');
            $('body').css('cursor', 'default');

        },
        error: function (err) {
            //debugger
            // alert("Mail sent failed."+ msg);
            console.log("err=Mail sent failed");
            console.log(err.responseText);
        }
    });
}

function BindMail_PaymentDue(pinvoiceid, Inv_Delay) {
    $.ajax({
        type: "POST",
        url: "ProjectInvoices.aspx/GetMailInfo",
        contentType: "application/json;charset=utf-8",
        data: "{'pinvoiceid':'" + pinvoiceid + "','Inv_Delay':'" + Inv_Delay + "'}",
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
            var subject = "Unpaid Invoice #" + message[0].InvoiceNo + " for " + message[0].projName;
            $('[id$="lblTo"]').text(cMailID);
            $('[id$="txtCc"]').val(emailCC);
            $('[id$="txtSubject"]').val(subject);
            //$('[id$="txtEmail"]').val('');

            var editor = $('[id$="txtEmail"]').data("kendoEditor");
            editor.value(description);
            //$('[id$="txtEmail"]').val(description);

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
    });
}

function ShowReminderDetails(dataItem) {
    var dataItem = $(GridInvoices).getKendoGrid().dataItem($(dataItem).closest("tr"));
    var ProjectInvoiceID = dataItem.ProjectInvoiceID;
    $.ajax({
        type: "POST",
        url: "ProjectInvoices.aspx/Get_InvoiceReminderDetails",
        contentType: "application/json;charset=utf-8",
        data: "{'ProjectInvoiceID':'" + ProjectInvoiceID + "'}",
        dataType: "json",
        async: false,
        success: function (data) {
            var checkdata = data.d;//jQuery.parseJSON(data.d);
            $('[id$="remider_text"]').text(checkdata);
        },
        error: function (x, e) {
            alert("The call to the server side failed. " + x.responseText);
        }
    });
}

function ClearText(dataItem) {
    $('#remider_text').css('display', 'none');
}

function previewFile() {
    var filename = document.getElementById('fileupload').files[0].name;
    var file = document.querySelector('input[type=file]').files[0];
    var reader = new FileReader();
    reader.onloadend = function () {
        var Anchor = $('<a target="_blank" href="' + reader.result + '">' + filename + '</a>');
        $('[id$="dvAttachnemt_1"]').html("");
        $('[id$="dvAttachnemt_1"]').append(Anchor);
        $('[id$="dvAttachnemt_1"]').append('<br/>');
    }
    if (file) {
        reader.readAsDataURL(file);
    } else {
        Anchor = "";
    }

}  