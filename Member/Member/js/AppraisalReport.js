
var GetAppReportCounter = 0;
var GetEmployeeAppraisalProject = 0;
var GetAppraisalEmployeeId = 0;
var GetManagerRatingsData = "";
var CheckAuthName = "";
var GetAllAPPProjectId = 0;
var CheckAuthNameTest = "";
var fieldName;
var GetApraisalProjectID;
var GetCommentsData = "";
var CheckCommentsData = "";
var GetAppraisalEmployeeName = "";
var QuartersReportDate = new Date();
var GetRptHiddenCounter = 0;
var SelectAllIdList = "";
GetRptHiddenCounter = $('#hdnRptQuarterCounter').val();

$(document).ready(function () {
    if (GetRptHiddenCounter != "" && GetRptHiddenCounter != null) {
        GetAppReportCounter = parseInt(GetRptHiddenCounter);
    }
    GetReportQuarterDate(GetAppReportCounter)
    GetAppraisalReport(GetAppReportCounter)
})

//Get quarter as per pre next button click
function GetReportQuarterDate(GetAppReportCounter) {
    $.ajax({
        type: "POST",
        url: "EmployeeAppraisalInitiation.aspx/GetQuarter",
        contentType: "application/json;charset=utf-8",
        data: "{'Counter':'" + GetAppReportCounter + "'}",
        dataType: "json",
        async: true,
        success: function (msg) {
            var obj = $.parseJSON(msg.d);
            jQuery("label[for='QuarterWiseRptDate']").html(obj[0]['Quarter']);
            return false;
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function PreLinkClick() {
    if (GetAppReportCounter == 0) {
        $('[id$=NEXT_ICON]').css("cursor", "default");
    }
    $('[id$=NEXT_ICON]').css("cursor", "pointer");
    GetAppReportCounter = (GetAppReportCounter) + (-1);
    $('[id$="hdnRptQuarterCounter"]').val(GetAppReportCounter);
    GetReportQuarterDate(GetAppReportCounter);
    GetAppraisalReport(GetAppReportCounter)
}

function NextLinkClick() {
    if (GetAppReportCounter == 0) {
        $('[id$=NEXT_ICON]').css("cursor", "default");
        //$('[id$=LNKNEXT_CLICK]').attr('disabled', 'disabled');
    }
    else {
        GetAppReportCounter = (GetAppReportCounter) + (1);
        if (GetAppReportCounter == 0) {
            $('[id$=NEXT_ICON]').css("cursor", "default");
        }
        $('[id$="hdnRptQuarterCounter"]').val(GetAppReportCounter);
        GetReportQuarterDate(GetAppReportCounter);
        GetAppraisalReport(GetAppReportCounter)
    }

}

//Call total project data agains authority member
function GetAppraisalReport(GetAppReportCounter) {
    $.ajax({
        type: "Post",
        url: "AppraisalReport.aspx/GetAppraisalReport",
        contentType: "application/json;charset=utf-8",
        data: "{'Mode':'" + 'GETAPPFNLREPORT' + "','Counter':'" + GetAppReportCounter + "'}",
        dataType: "json",
        async: true,
        success: function (msg) {
            GetAppraisalReportGridBind(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

//Bind total project data agains authority member 
function GetAppraisalReportGridBind(Tdata) {

    if (Tdata.length == 0)
    { }

    $("#GetAppRptGrid").empty();

    var grid = $("#GetAppRptGrid").kendoGrid({
        dataSource: new kendo.data.DataSource({
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        //Id: { type: "number", editable: false },
                        empid: { type: "number", editable: false },
                        empName: { type: "string" },
                        ProjectCounts: { type: "number" },
                        QuarterStartDate: { type: "date" },
                    }
                },
            },
            pageSize: 10,
        }),
        change: onRowSelect,
        scrollable: true,
        sortable: true,
        //columnMenu: true,
        emptyMsg: 'This grid is empty', // This is optional
        messages: {
            noRecords: "There is no data on current page"
        },
        selectable: "multiple",
        pageable: { input: true, numeric: false },
        //pageable: { numeric: true, refresh: true, pageSizes: true, pageSizes: [ 5, 10 ,15, 20, 25, 30 ], previousNext: true,  input: false, input: true, numeric: false },
        columns: [
                    {
                        field: "empid", title: "Emp Id", width: "50px",
                        sortable: true, hidden: false
                    },
                    { field: "empName", title: "Employee Name", width: "50px", sortable: false, hidden: false },
                    {
                        field: "ProjectCounts", title: "Project Counts",
                        width: "40px", sortable: false, hidden: false
                    },
                    { field: "QuarterStartDate", hidden: true, title: "Quarter Start Date", width: "40px", format: "{0:MM-dd-yyyy}" },
        ],
        editable: false,
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
        dataBound: function () {
            var grid = this;
            var model;
            //var grid = $("#grid").data("kendoGrid");
            var mBox = $("#msgBox");
            //Clear the grid and show the message that no records found
            if (grid.dataSource.data().length === 0) {
            }
            grid.tbody.find("tr[role='row']").each(function () {
                model = grid.dataItem(this);
                $(this).addClass('k-state-hover');
                //$(this).css('color', '#E63570');
            });
        }
    }).data("kendoGrid");

    function onRowSelect(arg) {
        var selectedItem = grid.dataItem(grid.select());
        var GetQuarterlyEmpId = selectedItem.empid;        
        GetAppraisalReportData(GetQuarterlyEmpId, GetAppReportCounter);
        //kendo.ui.progress($('.ParentsClass'), true);
       
    }

}

//Call method for fetching employee quarterly assign project
function GetAppraisalReportData(GetQuarterlyEmpId, GetAppReportCounter) {
    $.ajax({
        type: "POST",
        url: "AppraisalReport.aspx/GetEmployeeAppraisalProject",
        contentType: "application/json;charset=utf-8",
        data: "{ EmpId:'" + GetQuarterlyEmpId + "', Counter:'" + GetAppReportCounter + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            var obj = $.parseJSON(msg.d);
            GetEmployeeWiseProject(obj);
            //OpenReportPop();
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

//Call method for fetching authority ratings for the employee as per project
function GetEmployeeWiseProject(DATA) {
    
    $.each(DATA, function (i) {
        GetAppraisalEmployeeId = DATA[i].empid;
        GetEmployeeAppraisalProject = DATA[i].projId;
        
        $('.ParentsClass').append('<div id="GetAllAppMngrRpt' + i + '" />').append('<br/>'); //Create parent div for multiple kendo grid /Ganesh Pawar
        
        $.ajax({
            type: "POST",
            url: "AppraisalReport.aspx/GetAppraisalManagerRatingsReport",
            contentType: "application/json;charset=utf-8",
            data: "{ EmpId:'" + GetAppraisalEmployeeId + "', ProjectId:'" + GetEmployeeAppraisalProject + "', Counter:'" + GetAppReportCounter + "'}",
            dataType: "json",
            async: false,
            success: function (msg) {
                GetManagerRatingsData = $.parseJSON(msg.d);
                GetAuthComments(GetManagerRatingsData, i);
                //kendo.ui.progress($('.ParentsClass'), false);
                //GetAllAppraisalManagerReport(GetManagerRatingsData, i, GetCommentsData);
                //OpenReportPop();
            },
            error: function (x, e) {
                alert("The call to the server side failed. "
                      + x.responseText);
            }
        });
    })
    //kendo.ui.progress($('.ParentsClass'), false);
}

//Call method for fetching authority comments as per employee and his project
function GetAuthComments(GetManagerRatingsData, i) {
    $.ajax({
        type: "POST",
        url: "AppraisalReport.aspx/GetManagerComments",
        contentType: "application/json;charset=utf-8",
        data: "{ EmpId:'" + GetAppraisalEmployeeId + "', ProjectId:'" + GetEmployeeAppraisalProject + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetCommentsData = $.parseJSON(msg.d);
            GetAllAppraisalManagerReport(GetManagerRatingsData, i, GetCommentsData);
            //$('#GetAllAppMngrRpt' + i).empty();
            OpenReportPop();
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

//Call function from kendo grid FooterTemplate
function GetComment(fieldName,GetAllAPPProjectId, GetCommentsData) {
    var GetTotalComment = '';
    $.each(GetCommentsData, function (i) {
        CheckAuthName = GetCommentsData[i].AuthorityName;
        GetApraisalProjectID = GetCommentsData[i].projId;
        CheckAuthNameTest = CheckAuthName.replace(/ /g, "_");
        if (fieldName == CheckAuthNameTest && GetAllAPPProjectId == GetApraisalProjectID) {
            CheckAuthNameTest = GetCommentsData[i].Comments;
        GetTotalComment = CheckAuthNameTest;
    }
    })
    return GetTotalComment;
}

//Bind appraisal report data to grid
function GetAllAppraisalManagerReport(obj, i, commentsdata) {

    var columns = [];
    var proccessed = $.map(obj, function (x) {
        var cols = [];
        var tf = [];
        var result = {};
       
        for (var i in x) {
            fieldName = i.replace(/[\|&;\$%@"<>\(\)\+,]/g, "").replace(/\s/g, "_");

            if (i == "Quarter") {
                if (x[i] != 0) {
                    jQuery("label[for='QuarterDate']").html(x[i]);
                }
            }
            if (i == "empName") {
                if (x[i] != 0) {
                    jQuery("label[for='EmpName']").html(x[i]);
                }
            }
            if (i == "ProjectID") {
                GetAllAPPProjectId = x[i];
            }
            if (x[i] == null) {
                result[fieldName] = "NA";
            }
            else {
                result[fieldName] = x[i];
            }

            if (i == "Id" || i == "ProjName" || i == "ProjectID" || i == "empid" || i == "KRA" || i == "Quarter" || i == "GETCOLS") {
                //if (fieldName=="ProjectID") {
                //    GetAllAPPProjectId = fieldName;
                //}
                cols.push({
                    field: fieldName, title: i, hidden: true,groupHeaderTemplate: "Project Name: #= value # "
                });
            }
            else {
                if (i == "KRANames") {
                    //cols.push({ field: fieldName, title: "KRA" });
                    cols.push({ field: fieldName, title: "KRA", template: '<img src="images/bullte.png" alt="image" />&nbsp;&nbsp;&nbsp;<span>#: KRANames #</span>', attributes: { style: 'white-space: wrap' } });
                }
                else if (i == "EmployeeRatings") {
                    cols.push({ field: fieldName, title: "Employee Ratings" });
                }
                else if (i == "Comments") {
                    cols.push({ field: fieldName, title: "Authority Comments" });
                }
                else if (i == "empName") {
                    cols.push({ field: fieldName, title: i, hidden: true }); //, groupHeaderTemplate: "Emloyee Name: #= value # "
                }
                else {
                    cols.push({                        
                        field: fieldName, filterable: true, title: i, footerTemplate: "Comments: " + GetComment(fieldName, GetAllAPPProjectId, GetCommentsData)
                    });
                }
            }
        }
        if (!columns.length) {
            columns = cols;
        }
        return result;
    });

    $('#GetAllAppMngrRpt' + i).kendoGrid({
        //selectable: "multiple, row",
        columns: columns,
        scrollable: true,
        //filterable: true,
        //sortable: true,
        dataSource: {
            data: proccessed,
            sort: { field: "EmployeeRatings", dir: "asc" },
            // group by "category" and then by "subcategory"
            group: [{
                field: "ProjName",
            },
            ],
        },
    });

    
    $('#GetAllAppMngrRpt' + i).kendoTooltip({
        filter: "td:nth-child(1)", //this filter selects the first column cells
        position: "middle",
        content: function (e) {
            //var dataItems = grid.dataItem(this);
            
            var dataItem = $('#GetAllAppMngrRpt' + i).data("kendoGrid").dataItem(e.target.closest("tr"));
            var myElement = e.target.closest("td").attr('class');

            if (typeof myElement !== "undefined" && myElement !=="") {
                var content = dataItem.KRANames;
                return content

            }
            else {
                //var content = dataItem.KRANames;
                return ""
            }
            //var TextInsideLi = e.target.find('p')[0].innerText;

           
        }
    }).data("kendoTooltip");

    
}

function OpenReportPop() {
    
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}

function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    //$('#GetAllAppMngrRpt' + i).empty();
    $('.ParentsClass').empty();
    //location.reload();
}
