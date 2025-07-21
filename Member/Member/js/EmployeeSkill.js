$(document).ready(function () {
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('=');
    if (url[1] == 'all')
        $('[id$="chkAll"]').prop("checked", true);

    onLoad();
});

function onLoad()
{
  ////  angular.element($("#grdEmpSkill")).scope().GetSkill();
   angular.element($("#grdEmpSkill")).scope().ShowEmployee();
}

var app = angular.module('myApp', ['ngGrid']);

app.controller('Employee_Skill', function ($scope, $http) {
    $scope.Skill = [];
    $scope.Skill.myData = [];
      
    $scope.gridEmp_Skill=
        {
            data: 'Skill.myData',
            groupHeaders: true,
            enableCellSelection: true,
            enableCellOnFocus: false,
            enableRowSelection: false,
            tabIndex: 0,
            noTabInterference: true,
            //headerRowHeight:120,
            enableColumnResize: true,
            columnDefs: [                         
                          {
                              //headerName: "Categoryfds",
                              field: 'Category',
                              displayName: 'Category',
                              //cellTemplate: '<input type="text" ng-model="COL_FIELD"  style="width:125px;height:17px;text-align:left" onkeypress="return false;" readonly="readonly">',
                              enableCellEdit: false,
                              width: 220
                          },
                          {
                              //headerName: "ActiveSkill",
                              field: 'ActiveSkill',
                              displayName: '',
                              cellTemplate: '<input type="text"   ng-model="COL_FIELD"  style="width:50px;height:17px;text-align:left" >',
                              enableCellEdit: false,
                              visible:false,
                              width: 50
                          },
                          {
                              //headerName: "Skill",
                              field: 'SkillName',
                              displayName: 'Skill',
                              //cellTemplate: '<input type="text" ng-model="COL_FIELD"  style="width:125px;height:17px;text-align:left" onkeypress="return false;" readonly="readonly">',
                              enableCellEdit: false,
                              width: 230
                          },
                          {
                              //headerName: "ActiveSkill",
                              field: 'ActiveSkill',
                              displayName: '',
                              cellTemplate: '<input type="checkbox" ng-checked="SetActiveSkill(row)" name="chkSkill" ng-model="COL_FIELD"  style="width:33px;height:17px;text-align:left">',
                              enableCellEdit: false,
                              width: 50
                          },
                          //{
                          //    field: 'Experience',
                          //    displayName: 'Experience (in months)',
                          //    cellTemplate: '<input type="number" ng-change="TickSkill(row)"  ng-model="COL_FIELD" min="0" max="999"  style="width:155px;height:25px;text-align:center">',
                          //    enableCellEdit: false,
                          //    width: 155
                          //},
                           {
                               //headerName: "Years",
                               field: 'Years',
                               displayName: 'Year',
                               headerGroup: "Experience1",
                               filter: 'text',
                               cellTemplate: '<input type="number" ng-change="TickSkill(row)"  ng-model="COL_FIELD" min="0" max="99"  style="width:55px;height:25px;text-align:center">',
                               enableCellEdit: false,
                               width: 55
                           },
                            {
                                //headerName: "Months",
                                field: 'Months',
                                displayName: 'Month',
                                headerGroup: "Experience1",
                                filter: 'text',
                                cellTemplate: '<select ng-model="COL_FIELD" ng-change="TickSkill(row)" style="width:55px;height:30px;" ><option value="1" >1</option><option value="2" >2</option><option value="3">3</option><option value="4">4</option><option value="5">5</option><option value="6">6</option><option value="7">7</option><option value="8">8</option><option value="9">9</option><option value="10">10</option><option value="11">11</option></select>',
                                enableCellEdit: false,
                                width: 55
                            },
                          {
                              //headerName: "Level",
                              field: 'Level',
                              displayName: 'Level',
                              cellTemplate: '<select ng-model="COL_FIELD" ng-change="TickSkill(row)" style="width:125px;height:30px;" ><option value="Beginner" >Beginner</option><option value="Intermediate" >Intermediate</option><option value="Expert">Expert</option></select>',
                              enableCellEdit: false,
                              width: 125
                          },
                          {
                              //headerName: "SkillID",
                              field: 'SkillID',
                              displayName: 'SkillID',
                              cellTemplate: '<input type="text" ng-model="COL_FIELD"  style="width:10px;height:17px;text-align:left" onkeypress="return false;" readonly="readonly">',
                              enableCellEdit: false,
                              visible: false,
                              width: 10
                          },
                          {
                              //headerName: "CategoryId",
                              field: 'CategoryId',
                              displayName: 'CategoryId',
                              cellTemplate: '<input type="text" ng-model="COL_FIELD"  style="width:10px;height:17px;text-align:left" onkeypress="return false;" readonly="readonly">',
                              enableCellEdit: false,
                              visible: false,
                              width: 10
                          },
                          {
                              //headerName: "EmployeeSkillID",
                              field: 'EmployeeSkillID',
                              displayName: 'EmployeeSkillID',
                              cellTemplate: '<input type="text" ng-model="COL_FIELD"  style="width:10px;height:17px;text-align:left" onkeypress="return false;" readonly="readonly">',
                              enableCellEdit: false,
                              visible: false,
                              width: 10
                          },
                          //{
                          //    field: 'delete',
                          //    displayName: '',
                          //    cellTemplate: removeTemplate

                          //}
            ]
        };
    ///----------------Bind grid Employee skill count 
    $scope.gridEmp =
        {
            data: 'Skill.myData',
            enableCellSelection: true,
            enableCellOnFocus: false,
            enableRowSelection: false,
            tabIndex: 0,
            noTabInterference: true,
            enableColumnResize: true,
            columnDefs: [
                          {
                              field: 'EmpName',
                              displayName: 'Name',
                              //cellTemplate: '<input type="text" ng-model="COL_FIELD"  style="width:125px;height:17px;text-align:left" onkeypress="return false;" readonly="readonly">',
                              enableCellEdit: false,
                              width: 220
                          },
                          {
                              field: 'EmpCount',
                              displayName: 'No of Skills',
                              cellClass: 'grid-align',
                              //cellTemplate: '<input type="text"   ng-model="COL_FIELD"  style="width:50px;height:17px;text-align:left" onkeypress="return false;"  readonly="readonly" >',
                              enableCellEdit: false,
                              width: 150
                          },
                          {
                              field: 'InsertedDate',
                              displayName: 'Last Skill Date',
                              //cellTemplate: '<input type="text" ng-model="COL_FIELD"  style="width:125px;height:17px;text-align:left" onkeypress="return false;" readonly="readonly">',
                              enableCellEdit: false,
                              width: 100
                          },
                          
            ]
    
        };
   
    // to bind all skill/category
    $scope.BindSkill = function () {
        $('#ngGridSkill').css('display', '');
        $scope.GetSkill();
        
    };

    $scope.ShowEmployee = function () {

        $('#ngGridSkill').css('display', 'none');
        $('#ngGridEmp').css('display', '');
        // to fill employee grid
        $scope.GetEmployee();
    }


    $('[id$="ddlEmployee"]').change(function () {
        //$scope.GetSkill();
        $('#ngGridSkill').css('display', 'none');
        $('#ngGridEmp').css('display', 'none');
    });
    
    $scope.GetSkill = function () {
      
        $('#ngGridEmp').css('display', 'none');
        $('#ngGridSkill').css('display', '');

        var skillName = $('[id$="txtSearchEmpSkill"]').val();
        var EmpID = $('[id$="ddlEmployee"]').val();
        var toggleSkill = 0;
        if (String(EmpID) == "undefined")
            EmpID = $('[id$="hdnLoginID"]').val();
        if ($('[id$="chkAll"]').prop("checked"))
            toggleSkill = -1;

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Member/SkillMatrix.aspx/GetEmployeeSkill",
            data: "{EmpID:'" + parseInt(EmpID) + "',skillName:'" + skillName + "',toggleSkill:'" + parseInt(toggleSkill) + "'}",
            dataType: "json",
            async: false,
            success: function (data, status, headers, config) {
                data = data.d;
                for (var i = 0 ; i < data.lstEmpSkill.length; i++)
                {
                    var exp = 0;
                    if (data.lstEmpSkill[i].Experience > 0)
                    {
                        exp = parseInt(data.lstEmpSkill[i].Experience);
                        data.lstEmpSkill[i].Years = Math.floor(exp / 12);
                        data.lstEmpSkill[i].Months = (exp % 12);
                    }
                }
                $scope.Skill.myData = data.lstEmpSkill;
            },
            error: function (data, status, headers, config) {
                alert("some error occured ");
            }
        });

        return true;

    };
    // to get all employees skill count
    $scope.GetEmployee = function () {

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Member/SkillMatrix.aspx/GetEmployeeSkillCount",
            //data: "{EmpID:'" + parseInt(EmpID) + "',skillName:'" + skillName + "',toggleSkill:'" + parseInt(toggleSkill) + "'}",
            dataType: "json",
            async: false,
            success: function (data, status, headers, config) {
                data = data.d;

                $scope.Skill.myData = data.lstEmpSkill;
            },
            error: function (data, status, headers, config) {
                alert("some error occured ");
            }
        });

        return true;

    };

    // to set skill checkbox value
    $scope.SetActiveSkill = function (row) {
        if (row.entity.ActiveSkill == true)
            return true;
        else
            return false;
    }

    // to tick checkbox if experience or level is changed
    $scope.TickSkill = function (row)
    {        
        if (row.entity.Years > 0 || row.entity.Level !="" || row.entity.Months !="") {
            //if (row.entity.ActiveSkill == true)
            //    return;
            row.entity.ActiveSkill = true;
        }
        else
            row.entity.ActiveSkill = false;
    }

     ////////save Employee Skills
    $scope.CheckListData = function () {
        for(var i=0 ; i < $scope.Skill.myData.length;i++ )
        {
            //if ($scope.Skill.myData[i].Experience == null)
            //    $scope.Skill.myData[i].Experience = 0;
            if ($scope.Skill.myData[i].Years == null)
                $scope.Skill.myData[i].Years = 0;
            var mnth=0;
            if ($scope.Skill.myData[i].Years > 0)
                mnth = parseInt($scope.Skill.myData[i].Years) * 12; // ears to month
            if ($scope.Skill.myData[i].Months > 0)
                mnth = mnth + parseInt($scope.Skill.myData[i].Months);

            $scope.Skill.myData[i].Experience = mnth;
        }
    }
    $scope.SaveEmployeeSkill = function () {
        
        $scope.CheckListData();

        var Empid = $('[id$="ddlEmployee"]').val();
        if (String(Empid) == "undefined")
            Empid = $('[id$="hdnLoginID"]').val();
        var UserID = $('[id$="hdnLoginID"]').val();
        var postdata =
        {
            'EmpID': Empid,
            'UserID':parseInt(UserID),

            'lstEmpSkill': $scope.Skill.myData
        }
        var re = JSON.stringify(postdata);

        $.ajax({
            type: "POST",
            url: "../Member/SkillMatrix.aspx/SaveEmployeeSkill",
            data: "{'JSONData':'" + re + "'}",
            async: false,
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                
                alert(data.d);
               
                location.reload();
            },
            error: function (x, e) {
                alert("Skill Matrix Not saved. "
                      + x.responseText);
                location.reload();
            }
        });

    };

});

////

function closeRSkillPopUP() {
    $('#divRSkill').css('display', 'none');
    $("#lblerrmsgRSkill").css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");    
}

function ShowRSkillPopup() {   
    $('#divRSkill').css('display', '');
    $('#divAddPopupOverlay').css('display', '');
    $('[id$="txtSkill"]').val('');
    $('[id$="txtSkillNote"]').val('');
}