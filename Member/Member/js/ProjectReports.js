$(document).ready(function () {
    $("#txtFromDate").kendoDatePicker({ format: "dd/MM/yyyy" });
    $("#txtToDate").kendoDatePicker({ format: "dd/MM/yyyy" });
    $('[id$="txtFromDate"]').val('');
    $('[id$="txtToDate"]').val('');

    GetReportDetails();
    attachHtmlControl();
    fileUpload();

});


//------------------ functions for attachment ---------------------//

function fileUpload() {
    $("#files").kendoUpload({
        async: {
            saveUrl: "ProjectReports.aspx",
            removeUrl: "ProjectReports.aspx",
            autoUpload: false
        },
        error: onError,
        select: onSelect,
    });
}


function onSelect(e) {
    return $.map(e.files, function (file) {
        if (file.size <= 10485760) {
            return true;
        }
        else {
            return false;
            alert("File size must less than 10 MB !");
        }

    }).join(",");
}

function onError(e) {
    kendoConsole.log("Error (" + e.operation + ")");
}


function GetAttachment(reportId) {
    $.ajax(
          {
              type: "POST",
              url: "ProjectReports.aspx/GetReportAttachments",
              contentType: "application/json;charset=utf-8",
              data: "{'reportId':'" + reportId + "'}",
              dataType: "json",
              async: false,
              success: function (msg) {

                  var data = jQuery.parseJSON(msg.d);
                  $('[id$="attachment"]').html("");
                  if (data.length > 0) {
                      for (i = 0; i < data.length; i++) {

                          var Anchor = $('<a target="_blank" href="../Common/Download.aspx?m=PR&f=' + data[i].attachmentFile + '">' + data[i].attachmentFile + '</a>');

                          $('[id$="attachment"]').append(Anchor);
                          $('[id$="attachment"]').append('<br/>');
                      }
                  }
                  else {
                      var Anchor = "No Attachment."
                      $('[id$="attachment"]').append(Anchor);
                      $('[id$="attachment"]').addClass("ForeColor");
                  }
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
);
}

function checkFileUpload() {
    var filespendingtouload = $(".k-widget.k-upload").find(".k-button.k-upload-selected").is(':visible');
    if (filespendingtouload == true) {
        alert("Please upload the selected files first");
        return false;
    }

    if (confirm("Are you sure you want to send report to customer?")) {
        return true;
    } else {

        return false;
    }

}

function ClearTempFilesandSession() {
    $.ajax(
            {
                type: "POST",
                url: "ProjectReports.aspx/ClearTempFilesandSession",
                contentType: "application/json;charset=utf-8",
                data: "{}",
                cache: false,
                async: false,
                dataType: "json",
                success: function (msg) {
                },
                error: function (msg) {
                    alert("The call to the server side failed."
                          + msg.responseText);
                }
            }
         );
}

//------------------ functions for attachment end ---------------------//

//------------------ open and close popup/div -------------------//

function openAddPopUP() {

    $('#divAddPopup').css('display', 'block');
    $('#divAddPopupOverlay').addClass('k-overlay');
}
function closeAddPopUP() {

    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}

function ShowAddPopup() {

    openAddPopUP();

    attachDatepicker();
    
}

function attachHtmlControl() {
    $('[id$="txtDescription"]').kendoEditor();
}

function openPopUP() {
    $('#ProjectReportDetails').css('display', '');
    $('#divOverlay').addClass('k-overlay');
}

function closePopUP() {
    $('#ProjectReportDetails').css('display', 'none');
    $('#divOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}
function showDiv() {
    openPopUP();
}

//------------------ open and close popup/div end-------------------//

//-------------------- other functions start ----------------------------//

function UpdateReportDetails(id, reportTitle, Description, reportdate, empid, lastmodified, reportId) {

    $.ajax({

        type: "POST",
        url: "ProjectReports.aspx/UpdateReports",
        contentType: "application/json;charset=utf-8",
        //string mode, int projId, string ReportTitle, string Description, DateTime ReportDate, int ReportEmpId, string lastmodified, int reportId
        data: "{'mode':'Update','projId':'" + id + "','ReportTitle':'" + reportTitle + "','Description':'" + Description + "','ReportDate':'" + reportdate + "','ReportEmpId':'" + empid + "','lastmodified':'" + lastmodified + "','reportId':'" + reportId + "'}",
        cache: false,
        async: false,
        dataType: "json",
        success: function (msg) {
            var message = jQuery.parseJSON(msg.d);
            if (message == "Update Failed") {
                alert('Update Failed');
            }
            else {
                alert('Report updated successfully.');
            }
            $('body').css('cursor', 'default');
        },
        error: function (msg) {
            alert("The call to the server side failed." + msg.responseText);
        }
    });
}

function attachDatepicker() {
    var date = new Date();

    $("#txtReportDate").kendoDatePicker({
        format: "dd/MM/yyyy",
        max: date
    });

    var getDate = ((date.getDate() < 10 ? '0' : '') + date.getDate()) + '/' + (((date.getMonth() + 1) < 10 ? '0' : '') + (date.getMonth() + 1)) + '/' +
            date.getFullYear();

    $("#txtReportDate").val(getDate);

}

function ClosingRateWindow(e) {

    var grid = $("#grdReport").data("kendoGrid");
    grid.refresh();
}


function convert(str) {

    var monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"];

    var newDate = new Date(str);
    var formattedDate = newDate.getDate() + '-' + monthNames[newDate.getMonth()] + '-' + newDate.getFullYear();

    return formattedDate;
}


//-------------------- other functions end ----------------------------//


//---------------------- code for gridview bind ----------------------------//


function GetSearchReportDetails() {

    var FromDate = $('[id$="txtFromDate"]').val();
    var ToDate = $('[id$="txtToDate"]').val();

    dateFirst = FromDate.split('/');
    dateSecond = ToDate.split('/');
    var value = new Date(dateFirst[2], dateFirst[1], dateFirst[0]); //Year, Month, Date
    var current = new Date(dateSecond[2], dateSecond[1], dateSecond[0]);

    if (value > current) {
        alert('From Date should be less than To Date');
        return false;
    }
    else if (!FromDate & !ToDate) {
        alert('Please Select From Date and To Date');
        return false;
    }
    else {
        $.ajax({
            type: "POST",
            url: "ProjectReports.aspx/BindReports",
            contentType: "application/json;charset=utf-8",
            data: "{'jsFromDate':'" + FromDate + "','jsToDate':'" + ToDate + "'}",
            dataType: "json",
            async: false,
            success: function (msg) {
                GetReportData(jQuery.parseJSON(msg.d));
            },
            error: function (x, e) {
                alert("The call to the server side failed. "
                      + x.responseText);
            }
        });
    }
}

function GetReportDetails() {

    $('[id$="txtFromDate"]').val('');
    $('[id$="txtToDate"]').val('');

    date = new Date();
    var ToDate = ((date.getDate() < 10 ? '0' : '') + date.getDate()) + '/' + (((date.getMonth() + 1) < 10 ? '0' : '') + (date.getMonth() + 1)) + '/' +
           date.getFullYear();
    var FromDate = ((date.getDate() < 10 ? '0' : '') + date.getDate()) + '/' + (((date.getMonth() - 2) < 10 ? '0' : '') + (date.getMonth() - 2)) + '/' +
           date.getFullYear();

    $.ajax({
        type: "POST",
        url: "ProjectReports.aspx/BindReports",
        contentType: "application/json;charset=utf-8",
        data: "{'jsFromDate':'" + FromDate + "','jsToDate':'" + ToDate + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetReportData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetReportData(Tdata) {
    $("#grdReport").kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        projID: { type: "number", editable: false },
                        projectName: { type: "string" },
                        reportDate: { type: "date" },
                        reportTitle: { type: "string" },
                        lastModified: { type: "date" },
                        ReportEmpID: { type: "number", editable: false },
                        reportId: { type: "number", editable: false },
                        Description: { type: "string" },
                        ReportedBy: { type: "string" }
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
                    { field: "projId", title: "Project Id", width: "50px", hidden: true },
                    { field: "projectName", title: "Project", width: "90px" },
                    { field: "reportDate", format: "{0:dd-MMM-yyyy}", title: "Report Date", width: "90px" },
                    { field: "reportTitle", title: "Report Title", width: "90px" },
                    { field: "lastModified", format: "{0:dd-MMM-yyyy}", title: "Last Modified", width: "90px", hidden: true },
                    { field: "ReportEmpID", title: "Report Emp Id", width: "50px", hidden: true },
                    { field: "reportId", title: "Report Id", width: "50px", hidden: true },
                    { field: "Description", title: "Description", hidden: true },
                    { field: "ReportedBy", title: "Reported By", width: "90px" },
                    {
                        command: [{
                            name: "Details",
                            //click: addNewCustomRow
                            click: function (e) {
                                var window = $("#ProjectReportDetails");
                                openPopUP();
                                // e.target is the DOM element representing the button
                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
                                // get the data bound to the current table row
                                var data = this.dataItem(tr);

                                var projectName = data.projectName;
                                $('[id$="lblEditProjectTitle"]').html(projectName);

                                var reportdate = convert(data.reportDate);
                                $('[id$="lblEditReportDate"]').html(reportdate);

                                var reportTitle = data.reportTitle;
                                $('[id$="lblEditReportTitle"]').html(reportTitle);

                                var lastmodified = convert(data.lastModified);
                                $('[id$="lblEditLastModified"]').html(lastmodified);

                                var reportedBy = data.ReportedBy;
                                $('[id$="lblEditReportedBy"]').html(reportedBy);

                                var Description = data.Description;
                                $('[id$="lblEditReportDesc"]').html(Description);

                                var reportId = data.reportId;
                                GetAttachment(reportId);

                            }
                        }], width: "90px"
                    }],

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

        save: function (e) {

            if (e.model.isNew()) {
                var id = e.model.projID;
                var projectName = $('[id$="lblEditProjectTitle"]').val();
                var reportdate = $('[id$="lblEditReportDate"]').val();
                var reportTitle = $('[id$="lblEditReportTitle"]').val();
                var lastmodified = $('[id$="lblEditLastModified"]').val();
                var Description = $('[id$="lblEditReportDesc"]').val();
                var reportId = e.model.reportId;
                var empid = e.model.ReportEmpID;

                UpdateReportDetails(id, reportTitle, Description, reportdate, empid, lastmodified, reportId);
                window.location.assign("ProjectReports.aspx");
            }
            else {
                id = "0";
            }
        }

    });
}
