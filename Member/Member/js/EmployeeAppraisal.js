
var CheckRatingSelect = false;
var GetQuartersSelfAppDate = new Date();
var GetSelfAppRPTDate = new Date();

//On load get project wise data bind to 'gridProject' grid
FillAllProjectGrid();

function BindAppraisalData() {

    var projId = $("#txtProject").val();

    $.ajax({
        type: "POST",
        url: "EmpSelfAppraisal.aspx/GetSelfApprData",
        contentType: "application/json;charset=utf-8",
        data: "{projId:'" + projId + "'}",
        dataType: "json",
        async: false, 
        success: function (msg) {

            SetData(jQuery.parseJSON(msg.d));

        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function SetData(EventData) {

    

    var data = [];
    //var options = [{ Text: "Test1", Value: "1" },
    //   { Text: "Test2", Value: "2" },
    //   { Text: "Test3", Value: "3" }]

    for (var i = 0; i < 5; i++) {
        data.push({
            Name: "1" + i,
            FruitName: "1",
            FruitID: 1
        });
    }

    $("#gridKras").kendoGrid({
        editable: true,
        dataSource: {
            data: EventData,
            schema: {
                model: {
                    fields: {
                        KRANames: { type: "string", editable: false, },
                        FruitName: { type: "string" },
                        Id: { type: "number" },
                        empid: { type: "number" },
                        TransQuarterStartDate: { type: "date" },
                        AppraisalId: { type: "number" }
                    }
                }
            }
        },
        columns: [
            {
                field: "KRANames", title: "KRA", width: 150,
                template: '<img src="images/bullte.png" alt="image" />&nbsp;&nbsp;&nbsp;<span>#: KRANames #</span>',
                attributes: { style: 'white-space: wrap' }
            },
            { field: "AppraisalId", validation: { required: true }, title: "Ratings", width: 115, vinu: "yes", Name: "drpdown", editor: renderDropDown, template: "#=GetFruitName(AppraisalId)#" },                      
            { field: "EmpAprId", hidden: true },
            { field: "Id", hidden: true },
            { field: "EmpId", hidden: true },
            { field: "TransQuarterStartDate", hidden: true },
            //{
            //    template: "<input id ='ddltest' value==' " + options + " ' data-role='dropdownlist' data-source='options' data-text-field='Text' data-value-field='Value' style='width: 50%; height: 18px;' />",
            //    width: "200px",
            //    sortable: false,
            //    filterable: false
            //},
        ],
        editor: function (e) {
            var grid = $("#gridKras").data("kendoGrid")
            var container = grid.editable.element
            var service_container = container.find("[data-container-for=AppraisalId]")
            service_container.append('<span class="k-invalid-msg", data-for="#= field #">')
        },
        //dataBound: function (e) {
        //    var grid = this;
        //    
        //    this.tbody.find('tr').each(function () {
        //        var item = grid.dataItem(this);
        //        var content = item.Text;
        //        return content
                
        //    }).data("kendoTooltip");
        //}

    });

    $("#gridKras").kendoTooltip({
        filter: "td:nth-child(1)", //this filter selects the first column cells
        position: "middle",
        content: function (e) {
            //var dataItems = grid.dataItem(this);
            var dataItem = $("#gridKras").data("kendoGrid").dataItem(e.target.closest("tr"));
            var content = dataItem.KRANames;
            return content
        }
    }).data("kendoTooltip");
    
}





var dataSourceFruit = [
        //{ name: "", id: null },
        { FruitName: "-Select-", AppraisalId: 0 },
        { FruitName: "1", AppraisalId: 1 },
        { FruitName: "2", AppraisalId: 2 },
        { FruitName: "3", AppraisalId: 3 },
        { FruitName: "4", AppraisalId: 4 },
        { FruitName: "5", AppraisalId: 5 }
];

function GetFruitName(AppraisalId) {
    for (var i = 0; i < dataSourceFruit.length; i++) {
        if (dataSourceFruit[i].AppraisalId == AppraisalId) {
            return dataSourceFruit[i].FruitName;
        }
    }
}

function renderDropDown(container, options) {
    $('<input name="sl' + options.field + '" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    dataValueField: 'AppraisalId',
                    dataTextField: 'FruitName',
                    autoBind: false,
                    dataSource: dataSourceFruit,
                    // optionLabel : '-Select-',
                });
    $("<span class='k-invalid-msg' data-for='sl" + options.field + "'></span>").appendTo(container);

}


//Get on load all project wise report
function FillAllProjectGrid() {
    $.ajax(
          {
              type: "POST",
              url: "EmpSelfAppraisal.aspx/FillAllProject",
              contentType: "application/json;charset=utf-8",
              //data: "{}",
              async: true,
              dataType: "json",
              success: function (msg) {
                  GetProjectGridBind(jQuery.parseJSON(msg.d));
              },

              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
);
}

//Bind all project wise report
function GetProjectGridBind(Tdata) {

    var grid = $("#gridProject").kendoGrid({
        dataSource: new kendo.data.DataSource({
            data: Tdata,
            schema: {
                model: {
                    fields: {

                        Id: { type: "number", editable: false },
                        empid: { type: "number", editable: false },
                        //AppraiseBy: { type: "number", editable: false },
                        projId: { type: "number", editable: false },
                        ProjName: { type: "string" },
                        Quarter: { type: "string" },
                        Status: { type: "string" },
                        EmpSelfAppraisaDate: { type: "string" },
                        //EmpFinalAppraiseDate: { type: "string" },
                        SelfAppQuarterDate: { type: "date" },
                        ReportingManager: { type: "string" }
                    }
                },

            },
            pageSize: 10, //[*Note :- If you going to change page size you need also change in "$("#IsallCheck").change()" in same file.]
        }),
        change: onRowSelect, //On row select get record..
        scrollable: true,
        sortable: true,
        selectable: "multiple",
        pageable: { input: true, numeric: false },
        columns: [

                    { field: "Id", hidden: true },
                    { field: "empid", hidden: true },
                    //{ field: "AppraiseBy", hidden: true},
                    { field: "projId", hidden: true },
                    { field: "ProjName", title: "Project Name", width: "12px" },
                    { field: "Quarter", title: "Quarter", width: "10px", sortable: false, filterable: false },
                    { field: "Status", title: "Status", hidden: true, width: "20px", sortable: false, filterable: false },
                    { field: "EmpSelfAppraisaDate", hidden: false, title: "Employee Self Review", width: "10px", sortable: false, format: "{0:MM-dd-yyyy HH:mms}", filterable: false },
                    //{ field: "EmpFinalAppraiseDate", hidden: false, title: "Authority Appraisal", sortable: false, width: "10px", format: "{0:MM-dd-yyyy HH:mms}", filterable: false },
                    { field: "SelfAppQuarterDate", hidden: true, title: "Quarter Start Date", width: "15px", sortable: false, format: "{0:MM-dd-yyyy HH:mms}", filterable: false },
                    { field: "ReportingManager", title: "Reporting Managers", width: "12px" },
                  //{
                    //    command: [
                    //                 {
                    //                     name: "Add Ratings", click: AddRatings
                    //                     //name: "Add Ratings", click: EditProject
                    //                 }
                    //    ],
                    //    hidden: false,
                    //    title: "Add Ratings",
                    //    width: "20px", attributes: { style: "text-align:left;" }
                    //},
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
        editable: false,

        dataBound: function () {
            var grid = this;
            var model;
            grid.tbody.find("tr[role='row']").each(function () {
                model = grid.dataItem(this);

                $(this).css('color', '#9C3240');
                $(this).addClass('k-state-hover');
            });
        }

    }).data("kendoGrid");

    //On project row select events
    function onRowSelect(arg) {

        var gview = $("#gridProject").data("kendoGrid");
        //Getting selected item
        var selectedItem = gview.dataItem(gview.select());

       if (selectedItem.EmpSelfAppraisaDate == null || selectedItem.EmpSelfAppraisaDate === "") {
        //if (selectedItem.Status == "Employee Self Appraisal Pending") {
            //*On popup open*//
            $('#divAddPopup').css('display', '');
            $('#divAddPopupOverlay').addClass('k-overlay');

            //*On row select get record*//
            var gview = $("#gridProject").data("kendoGrid");
            //Getting selected item
            var selectedItem = gview.dataItem(gview.select());

            var GetprojctId = selectedItem.projId;
            var GetEmpID = selectedItem.empid;

            if (GetQuartersSelfAppDate != selectedItem.SelfAppQuarterDate) {
                GetQuartersSelfAppDate = (selectedItem.SelfAppQuarterDate.getMonth() + 1) + "-" + selectedItem.SelfAppQuarterDate.getDate() + "-" + selectedItem.SelfAppQuarterDate.getFullYear();
            }

            //alert(selectedItem.QuarterStartDate);
            //$(this).addClass('k-state-hover');
            $.ajax({
                type: "POST",
                url: "EmpSelfAppraisal.aspx/GetSelfApprData",
                contentType: "application/json;charset=utf-8",
                data: "{SubMode:'" + "EmpGetKRA" + "', EmpId:'" + GetEmpID + "', projId:'" + GetprojctId + "', QuarterStartDate:'" + GetQuartersSelfAppDate + "'}",
                dataType: "json",
                async: false,
                success: function (msg) {
                    SetData(jQuery.parseJSON(msg.d));
                },
                error: function (x, e) {
                    alert("The call to the server side failed. "
                          + x.responseText);
                }
            });
       }
       else {
           var empid = selectedItem.empid;
           var projId = selectedItem.projId;
           //var AppraiseBy = selectedItem.AppraiseBy;
           var AppraiseBy = 0;
           GetEmployeeSelfAppraisalReport(empid, projId, AppraiseBy);
       }    
    }

}


//Get employee self appraisal report
function GetEmployeeSelfAppraisalReport(empid, projId, AppraiseBy) {
    
    $('#divAddPopup2').css('display', '');
    $('#divAddPopupOverlay2').addClass('k-overlay');

    var gview = $("#gridProject").data("kendoGrid");
    //Getting selected item
    var selectedItem = gview.dataItem(gview.select());
    CurrentQuarterDate = selectedItem.SelfAppQuarterDate;

    //Get date format '10-25-2016' from => 'Fri Jul 01 2016 00:00:00 GMT+0530 (India Standard Time)' etc.. 

    if (GetSelfAppRPTDate != CurrentQuarterDate) {
        GetSelfAppRPTDate = (CurrentQuarterDate.getMonth() + 1) + "-" + CurrentQuarterDate.getDate() + "-" + CurrentQuarterDate.getFullYear();
    }

    $.ajax({
        type: "POST",
        url: "EmpSelfAppraisal.aspx/GetEmpSelfAppraisalReport",
        contentType: "application/json;charset=utf-8",
        data: "{'empid':'" + empid + "','projId':'" + projId + "','AppraiseBy':'" + AppraiseBy + "','CurrentQuarterDate':'" + GetSelfAppRPTDate + "'}",
        dataType: "json",
        async: true,
        success: function (msg) {
            //Fetch single record from json object..
            var obj = $.parseJSON(msg.d);
            jQuery("label[for='QuarterDate']").html(obj[0]['Quarter']);
            jQuery("label[for='EmpName']").html(obj[0]['empName']);
            jQuery("label[for='ProjName']").html(obj[0]['ProjName']);
            //jQuery("label[for='AuthorityName']").html(obj[0]['AuthorityName']);
            $("textarea#txtcom").val(obj[0]['Comments']);

            

            GetEmployeeReportGridBind(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

//Bind employee self appraisal report
function GetEmployeeReportGridBind(Tdata) {

    


    var grid = $("#GetEmpSelfAprReport").kendoGrid({
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
                        //ManagerRatings: { type: "number" },
                        Quarter: { type: "string" },
                    }
                },

            },
            pageSize: 10, //[*Note :- If you going to change page size you need also change in "$("#IsallCheck").change()" in same file.]
            aggregate: [
                       { field: "EmployeeRatings", aggregate: "sum" },
                       //{ field: "ManagerRatings", aggregate: "sum" }
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
                        field: "KRANames", sortable: false, title: "KRA", width: "80px",
                        template: '<img src="images/bullte.png" alt="image" />&nbsp;&nbsp;&nbsp;<span>#: KRANames #</span>',
                        filterable: true, attributes: { style: 'white-space: wrap' }
                    },
                    { field: "Quarter", title: "Quarter", hidden: true, width: "80px" },
                    {
                        field: "EmployeeRatings", title: "Employee Ratings",
                        width: "80px",
                        footerTemplate: "Total Employee Ratings: #: sum #"
                    },
                    //{
                    //    field: "ManagerRatings", title: "Manager Ratings",
                    //    width: "80px",
                    //    footerTemplate: "Total Manager Ratings: #: sum #"
                    //},

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
        editable: false,
    }).data("kendoGrid");

    $("#GetEmpSelfAprReport").kendoTooltip({
        filter: "td:nth-child(5)", //this filter selects the first column cells
        position: "middle",
        content: function (e) {
            //var dataItems = grid.dataItem(this);
            var dataItem = $("#GetEmpSelfAprReport").data("kendoGrid").dataItem(e.target.closest("tr"));
            var content = dataItem.KRANames;
            return content
        }
    }).data("kendoTooltip");
}

//Close self employee report popup
function closeReportPopup() {

    $('#divAddPopup2').css('display', 'none');
    $('#divAddPopupOverlay2').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    $("#GetEmpSelfAprReport").empty();

}


function closeAddPopUP() {
    
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");

}

function OnProjectChange() {
    $('[id$="hdnProjectId"]').val($("#txtProject").val());
    BindAppraisalData();

    //alert($("#txtProject").val());
}

function SetDefualtProject() {
    ProjectID = window.location.search.substring(1);
    var n = ProjectID.split("=");
    ID = ProjectID.split("=");
    if (ID[1] != null) {

        var dropdownlist = $("#txtProject").data("kendoDropDownList");
        var value = dropdownlist.value();
        dropdownlist.value(ID[1]);
        dropdownlist.enable(false);
    }
}

function SaveData() {
    
    var gridData = $("#gridKras").data("kendoGrid").dataSource.data();
    var pId = $("#ctl00_ContentPlaceHolder1_drpProject").val();
    var pIdd = 1;

    //Validation for every dropdown rating select
    $.each(gridData, function (i) {
        if (i >= 0) {
            if (gridData[i].AppraisalId == 0 || gridData[i].AppraisalId == null) {
                CheckRatingSelect = false;
                alert("Please give the ratings");
                return false;
            }
            else {
                CheckRatingSelect = true;
            }
        }
    });

    if (CheckRatingSelect) {

        //Insert data with the ratings
        $.ajax(
               {
                   type: "POST",
                   url: "EmpSelfAppraisal.aspx/SaveTDS",
                   contentType: "application/json;charset=utf-8",
                   data: JSON.stringify({ GridData: gridData, Project: pIdd }),
                   dataType: "json",
                   async: true,
                   success: function (msg) {
                       FillAllProjectGrid();
                       //alert("Data added Successfully");
                       window.location.href = "/Member/EmpSelfAppraisal.aspx";
                       //return false;
                   },
                   error: function (x, e) {
                       alert("The call to the server side failed. "
                             + x.responseText);
                   }
               }
         );
    }
}