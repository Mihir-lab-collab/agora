
var GetAppraisalCounter = 0;
var GetHiddenCounter = 0;
var QuartersSelfAppraiseDate = new Date();
var GetQuartersSelfAppDate = new Date();
var GetQrtAppRPTDate = new Date();
var GetAppraisalMode = "GET";

var RolesSelectCounterValue = false;
var RolesEditCounterValue = false;

$(document).ready(function () {
    var EmployeeAssignRoles = "";
    var EmployeeAssignprojId = "";
    var EmployeeSelectedId = "";
    var StoreSelectListAppraisalId = "";
    var t = 0;
    $('[id$=NEXT_ICON]').css("cursor", "pointer");

    GetHiddenCounter = $('#hdnQuarterCounter').val();
    if (GetHiddenCounter != "" && GetHiddenCounter != null) {
        GetAppraisalCounter = parseInt(GetHiddenCounter);
    }
    GetQuarterDate(GetAppraisalCounter)
    GetAppraisalData(GetAppraisalMode, GetAppraisalCounter);
});

function GetAppraisalData(GetAppraisalMode, GetAppraisalCounter) {

    LoginEmpId = $('#EmpId').val();
    $.ajax({
        type: "POST",
        url: "EmployeeAppraisalInitiation.aspx/GetAppraisalData",
        contentType: "application/json;charset=utf-8",
        data: "{'empId':'" + LoginEmpId + "','Mode':'" + 'GET' + "','Counter':'" + GetAppraisalCounter + "'}",
        dataType: "json",
        async: true,
        success: function (msg) {

            var getAppraiseData = $.parseJSON(msg.d);
            var mBox = $("#msgBox"); // No records available div id

            if (getAppraiseData.length == 0) {
                //jQuery("label[for='QuarterWiseDate']").html(obj[0]['Quarter']);
                //jQuery("label[for='QuarterWiseDate']").html("");
                GetAppraisalGridBind(jQuery.parseJSON(msg.d));
                //$('.ClcLnkbPrev').bind('click', false);
                //$('#GetApprGrid').kendoGrid('destroy').empty();                
            }
            else {
                var obj = $.parseJSON(msg.d);
                //jQuery("label[for='QuarterWiseDate']").html(obj[0]['Quarter']);
                GetAppraisalGridBind(jQuery.parseJSON(msg.d));
                //generateGrid(jQuery.parseJSON(msg.d));
                return false;
            }
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetAppraisalGridBind(Tdata) {
    var SelectedIdList = "";
    var SelectAllIdList = "";
    var SelectAllId = "";
    var IsStatusPending = "";

    // var getAppraiseData = $.parseJSON(Tdata.d);
    if (Tdata.length == 0)
    { }

    $("#GetApprGrid").empty();

    var grid = $("#GetApprGrid").kendoGrid({
        dataSource: new kendo.data.DataSource({
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        projId: { type: "string", editable: false },
                        ProjectMemberId: { type: "string", editable: false },
                        EmployeeAppraisalId: { type: "number", editable: false },
                        Id: { type: "number", editable: false },
                        empid: { type: "number", editable: false },
                        empName: { type: "string" },
                        ProjName: { type: "string" },
                        Status: { type: "string" },
                        StatusId: { type: "number", editable: false },
                        Quarter: { type: "string" },
                        ModifiedOn: { type: "string" },
                        EmpSelfAppraisaDate: { type: "string" },
                        EmpFinalAppraiseDate: { type: "string" },
                        QuarterStartDate: { type: "date" },
                        ReportingManager: { type: "string" },
                    }
                },
            },
            pageSize: 30, //[*Note :- If you going to change page size you need also change in "$("#IsallCheck").change()" in same file.]
        }),
        change: onRowSelect,
        scrollable: true,
        sortable: true,
        //filterable: true,
        //columnMenu: true,
        emptyMsg: 'This grid is empty', // This is optional
        messages: {
            noRecords: "There is no data on current page"
        },
        selectable: "row",
        pageable: {
            input: true,
            numeric: false
            //pageSizes: true
        },
        //columns: columnSchema,
        columns: [
                    { field: "Id", hidden: true },
                    { field: "EmployeeAppraisalId", hidden: true },
                    { field: "empid", hidden: true },
                    { field: "projId", hidden: true },
                    { field: "ProjectMemberId", hidden: true, sortable: false },
                    { field: "empName", title: "Employee Name", width: "50px", sortable: false },
                    { field: "ProjName", title: "Project Name", width: "40px", sortable: false },
                    { field: "Status", hidden: true, title: "Status", width: "50px" },
                    { field: "StatusId", hidden: true, title: "Status Id", width: "40px" },
                    { field: "Quarter", title: "Quarter", hidden: true, width: "30px", sortable: false, filterable: false },
                    { field: "ModifiedOn", hidden: false, title: "Authority Initiated", width: "32px", format: "{0:MM-dd-yyyy HH:mms}", filterable: false },
                    { field: "EmpSelfAppraisaDate", hidden: false, title: "Self Review", width: "35px", format: "{0:MM-dd-yyyy HH:mms}", filterable: false },
                    { field: "EmpFinalAppraiseDate", hidden: false, title: "Authority Review", width: "35px", format: "{0:MM-dd-yyyy HH:mms}", filterable: false },
                    { field: "QuarterStartDate", hidden: true, title: "QuarterStartDate", width: "40px", format: "{0:MM-dd-yyyy}" },
                    { field: "ReportingManager", title: "Reporting Managers", width: "50px", sortable: false },
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
            console.log(this);
            //var grid = $("#grid").data("kendoGrid");
            var mBox = $("#msgBox");
            //Clear the grid and show the message that no records found
            if (grid.dataSource.data().length === 0) {

                //if (!mBox.data("kendoWindow")) {
                //    mBox.kendoWindow({
                //        actions: ["Close"],
                //        animation: {
                //            open: {
                //                effects: "fade:in",
                //                duration: 500
                //            },
                //            close: {
                //                effects: "fade:out",
                //                duration: 500
                //            }
                //        },
                //        modal: true,
                //        resizable: false,
                //        title: "No items",
                //        width: 400
                //    }).data("kendoWindow").content("<p>No records available. Please try again later.</p>").center().open();
                //} else {
                //    mBox.data("kendoWindow").content("<p>No records available. Please try again later.</p>").open();
                //}

            }

            grid.tbody.find("tr[role='row']").each(function () {
                model = grid.dataItem(this);

                if (model.Status == "Appraisal Completed By Authority") {
                    $(this).css('color', '#E63570');
                    $(this).addClass('k-state-hover');
                }
                else {
                    $(this).find(".k-grid-SetKRA").remove();
                }
                if (model.Status == "Employee Self Appraisal Pending") {
                    $(this).css('color', '#415F69');
                    $(this).addClass('k-state-hover');
                }
                if (model.Status == "Completed By Appraisee") {
                    $(this).css('color', '#DE782F');
                    $(this).addClass('k-state-hover');
                }
                if (model.Status == "Pending") {
                    $(this).addClass('k-state-hover');
                }
                else {
                    $(this).find(".checkbox").attr("disabled", true);
                }
            });

        }

    }).data("kendoGrid");

    //*On Kendo Grid row click event*//
    function onRowSelect(arg) {


        //*On row select get record*//
        //var selected = $.map(this.select(), function (item) {
        //    alert($(item).text());
        //});
        //kendoConsole.log("Selected: " + selected.length + " item(s), [" + selected.join(", ") + "]");

        var gview = $("#GetApprGrid").data("kendoGrid");
        //Getting selected item
        var selectedItem = gview.dataItem(gview.select());

        $('[id$="hndAppraisalID"]').val(selectedItem.EmployeeAppraisalId);
        $('[id$="hdnProjectId"]').val(selectedItem.projId);
        $('[id$="hdnEmpId"]').val(selectedItem.ProjectMemberId);
        $('[id$="hdnStatusId"]').val(selectedItem.StatusId);
        $('[id$="hdnMode"]').val(GetAppraisalMode);

        //if (selectedItem.Status == "Pending") //*Popup open code*//
        if (selectedItem.ModifiedOn == null || selectedItem.ModifiedOn === "") {
            if (QuartersSelfAppraiseDate != selectedItem.QuarterStartDate) {
                QuartersSelfAppraiseDate = (selectedItem.QuarterStartDate.getMonth() + 1) + "-" + selectedItem.QuarterStartDate.getDate() + "-" + selectedItem.QuarterStartDate.getFullYear();
            }
            $('[id$="hndQuarterDate"]').val(QuartersSelfAppraiseDate);

            
            openAddPopUP();
        }

            //if (selectedItem.Status == "Employee Self Appraisal Pending") //*Popup open code*//
        else if (selectedItem.EmpSelfAppraisaDate == null || selectedItem.EmpSelfAppraisaDate === "") {
         

            $('#divAddPopup4').css('display', '');
            $('#divAddPopupOverlay4').addClass('k-overlay');

            var GetEmpID = selectedItem.Id;
            var GetprojctId = selectedItem.projId;

            if (GetQuartersSelfAppDate != selectedItem.QuarterStartDate) {
                GetQuartersSelfAppDate = (selectedItem.QuarterStartDate.getMonth() + 1) + "-" + selectedItem.QuarterStartDate.getDate() + "-" + selectedItem.QuarterStartDate.getFullYear();
            }

            $('[id$="hndQuarterDate"]').val(GetQuartersSelfAppDate);
            
            openEditrolePopup();


            $.ajax({
                type: "POST",
                url: "EmpSelfAppraisal.aspx/GetSelfApprData",
                contentType: "application/json;charset=utf-8",
                data: "{SubMode:'" + "AuthGetKRA" + "', EmpId:'" + GetEmpID + "', projId:'" + GetprojctId + "', QuarterStartDate:'" + GetQuartersSelfAppDate + "'}",
                dataType: "json",
                async: false,
                success: function (msg) {
                    GetEMPKRA(jQuery.parseJSON(msg.d));                   
                },
                error: function (x, e) {
                    alert("The call to the server side failed. "
                          + x.responseText);
                }
            });
        }
        //if (selectedItem.Status == "Completed By Appraisee") //*Popup open code*//
        else if (selectedItem.EmpFinalAppraiseDate == null || selectedItem.EmpFinalAppraiseDate === "") {
            //*Popup open code*//
            $('#divAddPopup3').css('display', '');
            $('#divAddPopupOverlay3').addClass('k-overlay');

            var ProjID = selectedItem.empid;

            $.ajax({
                type: "POST",
                url: "EmployeeAppraisalInitiation.aspx/GetManagerAprData",
                contentType: "application/json;charset=utf-8",
                data: "{projId:'" + ProjID + "'}",
                dataType: "json",
                async: false,
                success: function (msg) {
                    $("#txtcomMngr").val("");
                    SetData(jQuery.parseJSON(msg.d));
                },
                error: function (x, e) {
                    alert("The call to the server side failed. "
                          + x.responseText);
                }
            });
        }
        else {
            //(selectedItem.Status == "Appraisal Completed By Authority") //*Popup open code*//
            var empid = selectedItem.Id;
            var projId = selectedItem.projId;
            GetEmployeeAppraisalReport(empid, projId);
        }
    }

}

function PreLinkClick() {
    if (GetAppraisalCounter == 0) {
        $('[id$=NEXT_ICON]').css("cursor", "default");
    }

    $('[id$=NEXT_ICON]').css("cursor", "pointer");
    GetAppraisalCounter = (GetAppraisalCounter) + (-1);
    $('[id$="hdnQuarterCounter"]').val(GetAppraisalCounter);
    GetQuarterDate(GetAppraisalCounter);
    GetAppraisalData(GetAppraisalMode, GetAppraisalCounter);

}

function NextLinkClick() {
        if (GetAppraisalCounter == 0) {
        $('[id$=NEXT_ICON]').css("cursor", "default");
        //$('[id$=LNKNEXT_CLICK]').attr('disabled', 'disabled');
    }
    else {
        //$('[id$=NEXT_ICON]').css("cursor", "pointer");
        GetAppraisalCounter = (GetAppraisalCounter) + (1);
        if (GetAppraisalCounter == 0) {
            $('[id$=NEXT_ICON]').css("cursor", "default");
        }
        $('[id$="hdnQuarterCounter"]').val(GetAppraisalCounter);
        GetQuarterDate(GetAppraisalCounter);
        GetAppraisalData(GetAppraisalMode, GetAppraisalCounter);
    }

}

function GetQuarterDate(GetAppraisalCounter) {
    $.ajax({
        type: "POST",
        url: "EmployeeAppraisalInitiation.aspx/GetQuarter",
        contentType: "application/json;charset=utf-8",
        data: "{'Counter':'" + GetAppraisalCounter + "'}",
        dataType: "json",
        async: true,
        success: function (msg) {            
            var obj = $.parseJSON(msg.d);
            jQuery("label[for='QuarterWiseDate']").html(obj[0]['Quarter']);
            return false;
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

// GET EMPLOYEE KRA'S RECORDS
function GetEMPKRA(EventData) {
$("#gridEmpKra").kendoGrid({
        //editable: true,
        dataSource: {
            data: EventData,
            schema: {
                model: {
                    fields: {
                        KRANames: { type: "string", editable: false },
                        FruitName: { type: "string" },
                        Id: { type: "number" },
                        empid: { type: "number" },
                        TransQuarterStartDate: { type: "date" },
                        AppraisalId: { type: "number"}
                    }
                }
            }
        },
        columns: [
            {
                field: "KRANames", title: "KRA", 
                template: '<img src="images/bullte.png" alt="image" />&nbsp;&nbsp;&nbsp;<span>#: KRANames #</span>',
                width: 150,
                attributes: { style: 'white-space: wrap' }
            },
            {
                field: "AppraisalId",
                template: "#= (AppraisalId == 0) ? ' ' : '' #",
                title: "Employee Ratings",
                editable: false, nullable: true, width: 115
            },
            { field: "EmpAprId", hidden: true },
            { field: "Id", hidden: true },
            { field: "EmpId", hidden: true },
            { field: "TransQuarterStartDate", hidden: true },
        ],
    });

    $("#gridEmpKra").kendoTooltip({
        filter: "td:nth-child(1)", //this filter selects the first column cells
        position: "middle",
        content: function (e) {
            var dataItem = $("#gridEmpKra").data("kendoGrid").dataItem(e.target.closest("tr"));
            var content = dataItem.KRANames;
            return content
        }
    }).data("kendoTooltip");

}



//* Manager rating grid bind*//
function SetData(EventData) {

    $("#gridmgr").kendoGrid({
        editable: true,
        dataSource: {
            data: EventData,
            schema: {
                model: {
                    fields: {
                        //ProjName: { type: "string", editable: false },
                        KRANames: { type: "string", editable: false },
                        FruitName: { type: "string" },
                        Value: { type: "number", editable: false },
                        ManagerValue: { type: "number" },
                        AppraisalId: { type: "number" },
                        Id: { type: "number" },
                        AppraisalTransactionId: { type: "number" }
                    }
                }
            }
        },
        columns: [
             //{ field: "ProjName", title: "Project Name", width: 150, },
             { field: "AppraisalId", hidden: true },
             {
                 field: "KRANames", title: "KRA", width: 150, template: '<img src="images/bullte.png" alt="image" />&nbsp;&nbsp;&nbsp;<span>#: KRANames #</span>', attributes: { style: 'white-space: wrap' }
             },
             { field: "Value", title: "Employee Ratings", width: 115 },
             { field: "ManagerValue", title: "Authority Ratings", width: 115, editor: renderDropDown, template: "#=GetFruitName(ManagerValue)#" },
             { field: "Id", hidden: true },
             { field: "AppraisalTransactionId", hidden: true },
        ]
    });

    //Get Tooltip of row over
    $("#gridmgr").kendoTooltip({
        filter: "td:nth-child(2)", //this filter selects the first column cells
        position: "middle",
        content: function (e) {
            var dataItem = $("#gridmgr").data("kendoGrid").dataItem(e.target.closest("tr"));
            var content = dataItem.KRANames;
            return content
        }
    }).data("kendoTooltip");
}

var dataSourceFruit = [
       //{ name: "", id: null },
        { FruitName: "-Select-", ManagerValue: 0 },
       { FruitName: "1", ManagerValue: 1 },
       { FruitName: "2", ManagerValue: 2 },
       { FruitName: "3", ManagerValue: 3 },
       { FruitName: "4", ManagerValue: 4 },
       { FruitName: "5", ManagerValue: 5 }
];

function GetFruitName(ManagerValue) {
    for (var i = 0; i < dataSourceFruit.length; i++) {
        if (dataSourceFruit[i].ManagerValue == ManagerValue) {
            return dataSourceFruit[i].FruitName;
        }
    }
}

function renderDropDown(container, options) {
    // $('<select data-bind="source: products, value: selectedProduct" data-text-field="FruitName" data-value-field="Value" data-role="dropdownlist"></select>')

    $('<input data-bind="value:' + options.field + '" />')
                .appendTo(container)
                .kendoDropDownList({
                    dataValueField: 'Value',
                    dataTextField: 'FruitName',
                    dataSource: dataSourceFruit,
                    //optionLabel: '-Select-',
                })


}


//* Manager rating save records *//
function SaveData() {
    
    var ManagerComments = $('#txtcomMngr').val();
    var gridData = $("#gridmgr").data("kendoGrid").dataSource.data();

    //Validation for every dropdown manager rating select
    $.each(gridData, function (i) {
        if (i >= 0) {
            if (gridData[i].ManagerValue == 0 || gridData[i].ManagerValue == null) {
                CheckMngrRatingSelect = false;
                alert("Please give the ratings");
                return false;
            }
            else {
                CheckMngrRatingSelect = true;
            }
        }
    });

    if (CheckMngrRatingSelect) {

        //Insert data with the ratings

        $.ajax(
               {
                   type: "POST",
                   url: "EmployeeAppraisalInitiation.aspx/SaveTDS",
                   contentType: "application/json;charset=utf-8",
                   data: JSON.stringify({ GridData: gridData, Comments: ManagerComments }),
                   dataType: "json",
                   async: true,
                   success: function (msg) {
                       closeAddPopUP3();
                        var grid = $("#GetApprGrid").data("kendoGrid");
                       $("#GetApprGrid").empty();
                       $('[id$="hdnQuarterCounter"]').val(GetAppraisalCounter);
                       GetHiddenCounter = $('#hdnQuarterCounter').val();
                       GetAppraisalData(GetAppraisalMode, GetHiddenCounter);
                       
                       //$("#gridmgr").empty();
                       //$("#gridmgr").kendoGrid();
                       //alert("Data added Successfully");
                       //window.location.href = "/Member/EmployeeAppraisalInitiation.aspx";
                       //return true;
                   },
                   error: function (x, e) {
                       alert("The call to the server side failed. "
                             + x.responseText);
                   }
               }
         );
    }
}


//* Get Employee appraisal report *//
function GetEmployeeAppraisalReport(empid, projId) {
     $('#divAddPopup2').css('display', '');
    $('#divAddPopupOverlay2').addClass('k-overlay');

    var gridData = $("#GetApprGrid").data("kendoGrid").dataSource.data();
    //var obj = $.parseJSON(gridData);
    $.each(gridData, function (i) {
        CurrentQuarterDate = gridData[i].QuarterStartDate;
    })

    //CurrentQuarterDate = CurrentQuarterDate.getFullYear() + "-" + (CurrentQuarterDate.getMonth() + 1) + "-" + CurrentQuarterDate.getDate() + ' ' + CurrentQuarterDate.toString().split(' ')[4];
    if (GetQrtAppRPTDate != CurrentQuarterDate) {
        GetQrtAppRPTDate = (CurrentQuarterDate.getMonth() + 1) + "-" + CurrentQuarterDate.getDate() + "-" + CurrentQuarterDate.getFullYear();
    }

    $.ajax({
        type: "POST",
        url: "EmployeeAppraisalInitiation.aspx/GetEmpAppraisalReport",
        contentType: "application/json;charset=utf-8",
        data: "{'empid':'" + empid + "','projId':'" + projId + "','CurrentQuarterDate':'" + GetQrtAppRPTDate + "'}",
        dataType: "json",
        async: true,
        success: function (msg) {
            //Fetch single record from json object..
            var obj = $.parseJSON(msg.d);
            jQuery("label[for='QuarterDate']").html(obj[0]['Quarter']);
            jQuery("label[for='EmpName']").html(obj[0]['empName']);
            jQuery("label[for='ProjName']").html(obj[0]['ProjName']);
            jQuery("label[for='AuthorityName']").html(obj[0]['AuthorityName']);
            $("textarea#txtcom").val(obj[0]['Comments']);
            //var CheckDate=obj[0]
            GetEmployeeReportGridBind(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}


function GetEmployeeReportGridBind(Tdata) {

    var grid = $("#GetEmpAprReport").kendoGrid({
        dataSource: new kendo.data.DataSource({
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        Id: { type: "number", editable: false, }, //This is ProjectId
                        projId: { type: "number", editable: false },
                        empName: { type: "string" },
                        ProjName: { type: "string" },
                        KRANames: { type: "string" },
                        EmployeeRatings: { type: "number" },
                        ManagerRatings: { type: "number" },
                        Quarter: { type: "string" },
                    }
                },

            },
            pageSize: 10, //[*Note :- If you going to change page size you need also change in "$("#IsallCheck").change()" in same file.]
            aggregate: [
                       { field: "EmployeeRatings", aggregate: "sum" },
                       { field: "ManagerRatings", aggregate: "sum" }
            ]
        }),
        scrollable: true,
        //filterable: true,
        mobile: "phone",
        sortable: true,
        //groupable: true,
        //columnMenu: true, :-> Put "menu: false," that in column
        selectable: "multiple",
        //pageable: { input: true, numeric: false },

        columns: [
                    { field: "Id", hidden: true },
                    { field: "projId", hidden: true },
                    { field: "empName", sortable: false, hidden: true, title: "Employee Name", width: "100px" },
                    { field: "ProjName", sortable: false, hidden: true, title: "Project Name", width: "80px" },
                    {
                        field: "KRANames", sortable: false, title: "KRA", template: '<img src="images/bullte.png" alt="image" />&nbsp;&nbsp;&nbsp;<span>#: KRANames #</span>', width: "100px", attributes: { style: 'white-space: wrap' }
                    },
                    { field: "Quarter", title: "Quarter", hidden: true, width: "90px" },
                    {
                        field: "EmployeeRatings", title: "Employee Ratings",
                        width: "80px",
                        footerTemplate: "Total Employee Ratings: #: sum #"
                    },
                    {
                        field: "ManagerRatings", title: "Manager Ratings",
                        width: "80px",
                        footerTemplate: "Total Manager Ratings: #: sum #"
                    },

        ],
        //filterable: {
        //    extra: false,
        //    operators: {
        //        string: {
        //            startswith: "Starts with",
        //            contains: "Contains",
        //            eq: "Is equal to"
        //        }
        //    }
        //},
        editable: false,
    }).data("kendoGrid");

    $("#GetEmpAprReport").kendoTooltip({
        filter: "td:nth-child(5)", //this filter selects the first column cells
        position: "middle",
        content: function (e) {
            var dataItem = $("#GetEmpAprReport").data("kendoGrid").dataItem(e.target.closest("tr"));
            var content = dataItem.KRANames;
            return content
        }
    }).data("kendoTooltip");

}

function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    $("#txtRoles").data("kendoMultiSelect").value([]);
}

function closeAddPopUP2() {
    $('#divAddPopup2').css('display', 'none');
    $('#divAddPopupOverlay2').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}

function closeAddPopUP3() {
    $('#divAddPopup3').css('display', 'none');
    $('#divAddPopupOverlay3').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}

function closeAddPopUP4() {
    $('#divAddPopup4').css('display', 'none');
    $('#divAddPopupOverlay4').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    $("#txtEditRoles").data("kendoMultiSelect").value([]);
}


function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');

    //Code for Avoid to append dropdown list
    if (RolesSelectCounterValue == false) {
        FillRolesDropDown();
        RolesSelectCounterValue = true;
    }
}

function openEditrolePopup() {
    //Code for Avoid to append dropdown list
    if (RolesEditCounterValue == false) {
        FillEditRolesDropDown();
        RolesEditCounterValue = true;
    }
}

function EditProject(arg) {

    var tr = $(arg.target).closest("tr");
    var data = this.dataItem(tr);

    var projectid = data.projId;
    var empId = data.ProjectMemberId;

    $('[id$="hdnProjectId"]').val(projectid);
    $('[id$="hdnEmpId"]').val(empId);

    openAddPopUP();

}

function DropDownEditor(container, options) {
    $('<input data-bind="value:"+ >')
}

//DropDown For Project Name
function FillProjectDropDown() {

    $.ajax(
          {
              type: "POST",
              url: "EmployeeAppraisalInitiation.aspx/FillProjectDropdown",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              async: false,
              dataType: "json",
              success: function (msg) {
                  $("#txtProject").kendoDropDownList({
                      optionLabel: "Select Project",
                      dataTextField: "ProjName",
                      change: OnProjectChange,
                      dataValueField: "Id",
                      dataSource: jQuery.parseJSON(msg.d)

                  }).data("kendoDropDownList");
              },

              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          });
}

function OnProjectChange() {
    //    alert($("#txtProject").data("kendoDropDownList").text());
    //    alert($("#txtProject").val())

    EmployeeAssignprojId = $("#txtProject").val();
    //EmployeeAssignprojId = $("#txtEmployee").val();

    //alert(EmployeeAssignprojId);

    //$("#txtEmployee").show();
    FillEmployeeDropDown();
}

//DropDown For Employee Name
function FillEmployeeDropDown() {

    EmployeeAssignprojId = $("#txtProject").val();

    $.ajax({
        type: "POST",
        url: "EmployeeAppraisalInitiation.aspx/FillEmployeeDropdown",
        contentType: "application/json;charset=utf-8",
        data: "{projId:'" + EmployeeAssignprojId + "'}",
        async: true,
        dataType: "json",
        success: function (msg) {
            $("#txtEmployee").kendoDropDownList({
                optionLabel: "Select Employee",
                dataTextField: "empName",
                dataValueField: "ProjectMemberId",
                change: OnEmployeChange,
                dataSource: jQuery.parseJSON(msg.d)

            }).data("kendoDropDownList");
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    }
    );
}

function OnEmployeChange() {


    EmployeeSelectedId = $("#txtEmployee").val();
    //var multiSelect = $('#txtRoles').data("kendoMultiSelect");
    //multiSelect.value([]);
    FillRolesDropDown();
}



//DropDown For Roles
function FillRolesDropDown() {

    $.ajax({
        type: "POST",
        url: "EmployeeAppraisalInitiation.aspx/FillRolesDropDown?ProfileID=",
        contentType: "application/json;charset=utf-8",
        data: "{ProfileID:'" + '' + "'}",
        //async: false,
        //dataType: "json",
        type: "GET",
        success: function (msg) {
            $("#txtRoles").kendoMultiSelect({
                dataTextField: "Name",
                dataValueField: "ProfileID",
                change: onRolesSelect,
                autoBind: false,
                dataSource: jQuery.parseJSON(msg.d)
            }).data("kendoMultiSelect");           
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function FillEditRolesDropDown() {

    $.ajax({
        type: "POST",
        url: "EmployeeAppraisalInitiation.aspx/FillRolesDropDown?ProfileID=",
        contentType: "application/json;charset=utf-8",
        data: "{ProfileID:'" + '' + "'}",
        //async: false,
        //dataType: "json",
        type: "GET",
        success: function (msg) {
            $("#txtEditRoles").kendoMultiSelect({
                dataTextField: "Name",
                dataValueField: "ProfileID",
                change: onRolesSelect,
                autoBind: false,
                dataSource: jQuery.parseJSON(msg.d)
            }).data("kendoMultiSelect");
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function onRolesSelect() {

    var multiselect = $("#txtRoles").data("kendoMultiSelect");
    // get the value of the multiselect.
    EmployeeAssignRoles = multiselect.value();
}


function GetInitiationRecordId(SelectedIdList) {
    StoreSelectListAppraisalId = SelectedIdList;
}


$('.linkbutton').click(ShowPopUp);


function ShowPopUp() {
    TINY.box.show({
        iframe: 'User_News.aspx',
        boxid: 'frameless',
        width: 250,
        height: 250,
        fixed: false,
        maskid: 'bluemask',
        maskopacity: 40,
        closejs: function () { closeJS() }
    });
}
