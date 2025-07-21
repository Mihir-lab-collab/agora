$(document).ready(function () {
    var skillName = "";
    $('[id$="spnMsg"]').hide();
    GetSkills(skillName);
});
//var grid = $("gridEvents").data("kendoGrid");
//grid.collapseGroup(".k-grouping-row:contains(Microsoft)");

function GetSkills(skillName) {

    $.ajax({
        type: "POST",
        url: "SkillMatrix.aspx/BindSkills",
        contentType: "application/json;charset=utf-8",
        data: "{skillName:'" + skillName + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            eventData = jQuery.parseJSON(msg.d);
            for (var i = 0; i < eventData.length; i++)
            {
                var exp = parseInt(eventData[i].MaxExperience);
                eventData[i].MaxExperience = (Math.floor(exp / 12)).toString() + " yrs - " + (exp % 12).toString() + " mnths";
            }
            BindData(eventData);
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}
var count = 0;
function SearchSkills() {
    count = count + 1;
    var skill = $('[id$="txtSearchSkill"]').val();
    if (skill == "")
        groupExpand = "Closed";
    else
        groupExpand = "Open";

    if (count == 1) {
        SearchSkills();
        count = 2;
    }
    GetSkills(skill);
    count = 0;
}

var groupExpand = "Closed";
function BindData(EventData) {

    $(gridEvents).kendoGrid({
        dataSource: {
            data: EventData,
            group: { field: "Category" },
            schema: {
                model: {
                    fields: {
                        Category: { type: "string" },
                        SkillName: { type: "string" },
                        MaxExperience: { type: "string" },
                        EmpCount: { type: "Number" },
                        SkillID: { type: "Number" },
                        CategoryId: { type: "Number" }

                    }

                }
            },
            // pageSize: 50,
        },
        groupable: true,
        scrollable: true,
        sortable: true,
        selectable: true,
        pageable: {
            input: true,
            numeric: false
        },
        //change: onChange,
        columns: [
                    { field: "CategoryId", title: "Category", width: "30px", hidden: true, },
                    { field: "SkillID", title: "Skill", width: "30px", hidden: true },
                    {
                        field: "SkillName", title: "Skill", width: "50px"
                        , template: '<a style="cursor:pointer;" onclick="ShowEditSkill(this)">#= SkillName #</a>'

                    },
                    {
                        field: "EmpCount", title: "No of People", width: "30px"
                        , template: '<a style="padding-left: 100px;cursor:pointer;font-weight: bold;font-size: 15px;" onclick="ShowEmployee(this)">#= EmpCount #</a>'
                    },
                    {
                        field: "MaxExperience", title: "Maximum Experience", width: "30px"
                        , template: '<span style="padding-left: 100px;">#= MaxExperience #</span>'
                    },
                    {
                        command: [
                              { name: "edit", text: "Category", click: EditCategory }
                              //, { name: "destroy", text: "delete", className: "ob-delete", click: DeleteEvent }
                        ], width: "20px"
                    }

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

    });

    var grid = $(gridEvents).data("kendoGrid");
    var dataView = grid.dataSource.view();

    for (var i = 0; i < dataView.length; i++) {
        for (var j = 0; j < dataView[i].items.length; j++) {
            if (dataView[i].items[j].Status == groupExpand) {//Closed
                var uid = dataView[i].items[j].uid;
                console.log($(gridEvents).find("tr[data-uid=" + uid + "]").prev("tr.k-grouping-row"));
                grid.collapseGroup($(gridEvents).find("tr[data-uid=" + uid + "]").prev("tr.k-grouping-row"));
            }
        }
    }
}

function EditCategory(e) {
    EventItems = this.dataItem($(e.currentTarget).closest("tr"));
    ShowCategory(EventItems)
}

function ShowEmployee(dataItem) {
    var dataItem = $(gridEvents).getKendoGrid().dataItem($(dataItem).closest("tr"));
    var SkillID = dataItem.SkillID;

    if (parseInt(dataItem.EmpCount) != 0) {
        $('[id$="lblCategory"]').text(dataItem.Category);
        $('[id$="lblSkill"]').text(dataItem.SkillName);
        ShowEmpDetail(); //grdEmpDetail  // grdEmpSkill   
        GetEmpDetails(SkillID);
        MoveTop();
    }
}

function ShowCategory(dataItem) {
    ClearRecord();
    //var dataItem = $(gridEvents).getKendoGrid().dataItem($(ditem).closest("tr"));

    $('[id$="hdnCategoryID"]').val(dataItem.CategoryId);

    $('[id$="txtCategoryName"]').val(dataItem.Category);

    CategoryPopup();
}

function ShowEditSkill(ditem) {
    ClearRecord();
    var dataItem = $(gridEvents).getKendoGrid().dataItem($(ditem).closest("tr"));
    SkillPopup();
    MoveTop();
    $('[id$="hdnSkillID"]').val(dataItem.SkillID);
    var catid = dataItem.CategoryId;
    var catg = dataItem.Category;
    //alert(catid);
    //$('#ddlCategory option[value="' + catg + '"]').attr("selected", "selected");
    $('[id$="ddlCategory"] option[value="' + catid + '"]').attr("selected", "selected");
    //$('[id$="ddlCategory"]')[0].selectedValue = parseInt(catid);

    $('[id$="txtSkillName"]').val(dataItem.SkillName);

    //SkillPopup();
}


////////////////////////////////////////////////
function DuplicateCategory() {
    alert('Category already exists.');
    ShowCategoryPopup();
}
function DuplicateSkill() {
    alert('Skill already exists.');
    ShowSkillPopup();
}

function ClearRecord() {
    $('[id$="txtCategoryName"]').val("");
    $('[id$="txtSkillName"]').val("");
}

function closeSkillPopUP() {
    $('#divSkillPopup').css('display', 'none');
    $("#lblerrmsgSkill").css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");

    //RedirectPage();
}

function ShowSkillPopup() {
    $('[id$="hdnSkillID"]').val("0");
    ClearRecord();
    SkillPopup();

}
function SkillPopup() {
    $('#divSkillPopup').css('display', '');
    //GetCategory();
}

function closeCategoryPopUP() {
    $('#divCategeoryPopup').css('display', 'none');
    $("#lblerrmsgdate").css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    //RedirectPage();
}

function ShowCategoryPopup() {
    $('[id$="hdnCategoryID"]').val("0");
    ClearRecord();
    //$('#divCategeoryPopup').css('display', '');
    //GetCategory();
    CategoryPopup();
}
function CategoryPopup() {
    $('#divCategeoryPopup').css('display', '');
    //GetCategory();
}

function ShowEmpSkill() {

    $('#divEmpSkill').css('display', '');

}
function closeEmpSkill() {
    $('#divEmpSkill').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");

}

function ShowEmpDetail() {

    $('#divEmpDetail').css('display', '');

}
function closeEmpDetail() {
    $('#divEmpDetail').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");

}

function MoveTop() {
    $("html, body").animate({ scrollTop: 0 }, "slow");
    //return false;
}

function ShowMsg() {
    ClearRecord();
    $('[id$="spnMsg"]').show();
    $('[id$="spnMsg"]').delay(1000).fadeOut('slow');
}

////////////////////////////////////////////////////////////////////////////////

function GetEmpDetails(SkillID) {
    $.ajax({
        type: "POST",
        url: "SkillMatrix.aspx/GetEmployeeDetail",
        contentType: "application/json;charset=utf-8",
        data: "{SkillID:'" + SkillID + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            var eventData = jQuery.parseJSON(msg.d);
            for (var i = 0; i < eventData.length; i++) {
                var exp = parseInt(eventData[i].Experience);
                eventData[i].MaxExperience = (Math.floor(exp / 12)).toString() + " yrs - " + (exp % 12).toString() + " mnths";
            }
            BindDetailData(eventData);
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function BindDetailData(EventData) {
    $(gridDetail_).kendoGrid({
        dataSource: {
            data: EventData,
            schema: {
                model: {
                    fields: {
                        EmpName: { type: "string" },
                        Category: { type: "string" },
                        SkillName: { type: "string" },
                        MaxExperience: { type: "string" },
                        Level: { type: "string" }
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
                    { field: "EmpName", title: "Name", width: "10px" },
                    //{ field: "Category", title: "Category", width: "20px" },
                    //{ field: "SkillName", title: "Skill Name", width: "20px" },
                    { field: "MaxExperience", title: "Experience", width: "10px" },
                    { field: "Level", title: "Level", width: "10px" },


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

    });
}

///////////////////////////////////////////////////////////// Employee Detail Angular Section /////////////////////////////////
//var app = angular.module('myApp', ['ngGrid']);

//app.controller('Employee_Detail', function ($scope, $http) {
//    $scope.EmpDetail = [];

//    $scope.EmpDetail.myData = [];

//    $scope.gridEmp_Detail =
//        {
//            data: 'EmpDetail.myData',
//            enableCellSelection: true,
//            enableCellOnFocus: false,
//            enableRowSelection: false,
//            tabIndex: 0,
//            noTabInterference: true,
//            columnDefs: [
//                          {
//                                field: 'EmpName',
//                                displayName: 'Name',
//                                cellTemplate: '<input type="checkbox" ng-checked="SetActiveSkill(row)" name="chkSkill" ng-model="COL_FIELD"  style="width:33px;height:17px;text-align:left">',
//                                enableCellEdit: false,
//                                width: 50
//                          },
//                          {
//                              field: 'Category',
//                              displayName: 'Category',
//                              cellTemplate: '<input type="text" ng-model="COL_FIELD"  style="width:125px;height:17px;text-align:left" onkeypress="return false;" readonly="readonly">',
//                              enableCellEdit: false,
//                              width: 125
//                          },                          
//                          {
//                              field: 'SkillName',
//                              displayName: 'Skill',
//                              cellTemplate: '<input type="text" ng-model="COL_FIELD"  style="width:125px;height:17px;text-align:left" onkeypress="return false;" readonly="readonly">',
//                              enableCellEdit: false,
//                              width: 125
//                          },

//                          {
//                              field: 'Experience',
//                              displayName: 'Experience (in months)',
//                              cellTemplate: '<input type="number"  ng-model="COL_FIELD" min="0" max="999"  style="width:155px;height:25px;text-align:center">',
//                              enableCellEdit: false,
//                              width: 155
//                          },
//                          {
//                              field: 'Level',
//                              displayName: 'Level',
//                              cellTemplate: '<select ng-model="COL_FIELD" style="width:125px;height:30px;" ><option value="Beginner" >Beginner</option><option value="Intermediate" >Intermediate</option><option value="Expert">Expert</option></select>',
//                              enableCellEdit: false,
//                              width: 125
//                          },
//                          {
//                              field: 'SkillID',
//                              displayName: 'SkillID',
//                              cellTemplate: '<input type="text" ng-model="COL_FIELD"  style="width:10px;height:17px;text-align:left" onkeypress="return false;" readonly="readonly">',
//                              enableCellEdit: false,
//                              visible: false,
//                              width: 10
//                          },
//                          {
//                              field: 'CategoryId',
//                              displayName: 'CategoryId',
//                              cellTemplate: '<input type="text" ng-model="COL_FIELD"  style="width:10px;height:17px;text-align:left" onkeypress="return false;" readonly="readonly">',
//                              enableCellEdit: false,
//                              visible: false,
//                              width: 10
//                          },
//                          //{
//                          //    field: 'EmployeeSkillID',
//                          //    displayName: 'EmployeeSkillID',
//                          //    cellTemplate: '<input type="text" ng-model="COL_FIELD"  style="width:10px;height:17px;text-align:left" onkeypress="return false;" readonly="readonly">',
//                          //    enableCellEdit: false,
//                          //    visible: false,
//                          //    width: 10
//                          //},
//                          //{
//                          //    field: 'delete',
//                          //    displayName: '',
//                          //    cellTemplate: removeTemplate

//                          //}
//            ]
//        };

//    // to bind all skill/category
//    $scope.BindSkill = function () {
//        //$scope.GetEmpDetail(SkillID);
//        alert(1);
//    };

//    $scope.GetEmpDetail = function (SkillID) {
//        $.ajax({
//            type: "POST",
//            contentType: "application/json; charset=utf-8",
//            url: "SkillMatrix.aspx/GetEmployeeDetail",
//            data: "{SkillID:'" + parseInt(SkillID) + "'}",
//            dataType: "json",
//            async: false,
//            success: function (data, status, headers, config) {
//                data = data.d;
//                //$scope.Invoice.customerName = data.customerName;

//                $scope.EmpDetail.myData = data.lstEmpSkill;
//                //$scope.SetActiveSkill(data.lstEmpSkill);

//            },
//            error: function (data, status, headers, config) {
//                alert("some error occured payment ");
//            }
//        });

//        return true;

//    };

//});