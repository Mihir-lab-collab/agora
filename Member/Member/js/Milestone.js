
var app = angular.module("myApp", ["ngGrid"]);

app.directive('ngBlur', function () {
    return function (scope, elem, attrs) {
        elem.bind('blur', function () {
            scope.$apply(attrs.ngBlur);
        });
    };
});

app.directive('onlyNum', function () {
    return function (scope, element, attrs) {

        var keyCode = [8, 9, 37, 39, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 110];
        element.bind("keydown", function (event) {
            console.log($.inArray(event.which, keyCode));
            if ($.inArray(event.which, keyCode) == -1) {
                scope.$apply(function () {
                    scope.$eval(attrs.onlyNum);
                    event.preventDefault();
                });
                event.preventDefault();
            }

        });
    };
})


app.controller('milestoneCtrl', function ($scope, $http) {
    // Calculate Balance Amount
    var getTotalBal = function () {
        $scope.totalBalance = 0;

        angular.forEach($scope.myData, function (row) {

            return getSumCol(row.BalanceAmount);
        });
        // alert($scope.totalBalance);
        return $scope.totalBalance;
    };


    function getSumCol(BalanceAmount) {
        $scope.totalBalance = $scope.totalBalance + BalanceAmount;
    }
    // End of Calculation of Balance Amount

    // Calculate EstHours

    var getTotalHr = function () {
        $scope.totalEstHr = 0;
        angular.forEach($scope.myData, function (row) {
            return getSumColHr(row.EstHours);
        });
        // alert($scope.totalEstHr);
        return $scope.totalEstHr;
    };

    function getSumColHr(EstHr) {
        $scope.totalEstHr = $scope.totalEstHr + EstHr;

    }

    // End of Calculation of EstHours

    angular.element(document).ready(function () {

        getTotalBal();
        getTotalHr();


    });

    var id = "";
    $scope.Projects = $("#hdnProjID").val();
    $scope.currExRate = 1;
    var user = document.getElementById("hdnEmpId").value;

    var removeTemplate = '<input type="button" value="" ng-click="removeRow($index)" class="buttondel" style="width:21px;height: 21px;" />';
    var copyTemplate = '<input type="button" style="width:40px;height: 23px;" value="Copy" ng-click = "copyRow();" / >';

    $scope.gridMileStone = {
        data: 'myData',
        enableCellSelection: true,
        enableCellOnFocus: false,
        enableRowSelection: false,
        tabIndex: 0,
        noTabInterference: true,


        columnDefs: [
                       {
                           field: 'name',
                           displayName: 'Name',
                           cellTemplate: '<input type="text"  name="name" ng-model="COL_FIELD" style="width:157px;height:18px;" maxlength="50" required ng-input="COL_FIELD" ng-change="validateCell()"/>',
                           enableCellEdit: false,
                           width: 170
                       },

                      {
                          field: 'amount',
                          displayName: 'Amount',
                          cellTemplate: '<input type="text" name="amount" ng-model="COL_FIELD" required style="width:90px;height:18px;text-align:right" only-num><span ng-show="milestone_form.amount.$error.pattern"></span>',
                          enableCellEdit: false,
                          width: 100

                      },
                      {
                          field: 'BalAmount',
                          displayName: 'Balance',
                          cellTemplate: '<input type="text" name="amount1" disabled ng-model="COL_FIELD" ng-class="col.colIndex()" required style="width:97px;height:18px;text-align:right" only-num><span ng-show="milestone_form.amount.$error.pattern"></span>',
                          enableCellEdit: false,
                          width: 110
                      },
                      {
                          field: 'ExRate',
                          displayName: 'ExRate',
                          cellTemplate: '<input type="text" name="ExRate" ng-model="COL_FIELD" required style="width:50px;height:18px;text-align:right" only-num><span ng-show="milestone_form.ExRate.$error.pattern"></span>',
                          enableCellEdit: false,
                          width: 60
                      },

                       {
                           field: "DeliveryDate",
                           displayName: "Delivery Date",
                           cellFilter: 'date',
                           cellTemplate: '<input type="date" ng-model="COL_FIELD" name="DeliveryDate" style="width:145px;height:26px;" ng-blur="validatedate(row);" />',
                           enableCellEdit: false,
                           width: 145
                       },
                     {
                         field: "dueDate",
                         displayName: "Due-Date",
                         cellFilter: 'date',
                         cellTemplate: '<input type="date" ng-model="COL_FIELD" name="dueDate" style="width:145px;height:26px;" ng-blur="validatedate(row);" />',
                         enableCellEdit: false,
                         width: 145
                     },
                      {
                          field: 'EstHours',
                          displayName: 'EstHours',
                          cellTemplate: '<input type="text" name="EstHours" ng-model="COL_FIELD" ng-class="col.colIndex()" required style="width:57px;height:18px;text-align:right" only-num><span ng-show="milestone_form.EstHours.$error.pattern"></span>',
                          enableCellEdit: false,
                          width: 70
                      },

                      {
                          field: 'Description',
                          displayName: 'Description',
                          cellTemplate: '<input type="text"  ng-model="COL_FIELD" style="width:237px;height:18px;"  ng-input="COL_FIELD" />',
                          enableCellEdit: false,
                          width: 250
                      },
                       {
                           field: 'IsRecurring',
                           displayName: 'IsRecurring',
                           cellTemplate: '<input type="checkbox" name="IsRecurring"  ng-model="COL_FIELD" style="width:90px;" >',
                           enableCellEdit: false,
                           width: 80
                       },
                      {
                          field: 'delete',
                          displayName: '',
                          cellTemplate: removeTemplate,
                          width: 30
                      },
                      {
                          field: 'copy',
                          displayName: '',
                          cellTemplate: copyTemplate,
                          width: 40
                      }

        ],


    };


    /*---------------------------------------------- FUNCTION To Get Saved Milestone -------------------------------------------------- */

    var Milestone = function () {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "Milestone.aspx/GetData",
            data: "{'id':'" + id + "'}",
            dataType: "json",
            async: false,
            success: function (data, status, headers, config) {

                $scope.Projects = parseInt(id);
                $scope.currExRate = 1;


                for (var i = 0; i < data.d.length; i++) {
                    var dateval = new Date(data.d[i].dueDate);
                    data.d[i].dueDate = ConvertDate(dateval);
                    var DeliveryDate = new Date(data.d[i].DeliveryDate);
                    data.d[i].DeliveryDate = ConvertDate(DeliveryDate);
                    $scope.currExRate = data.d[i].currExRate;
                    $scope.EndDate = data.d[0].MaxDueDate;

                }



                function ConvertDate(d) {
                    var year = d.getFullYear();
                    var month = d.getMonth() + 1;
                    if (month < 10) {
                        month = "0" + month;
                    }
                    var day = d.getDate();
                    if (day < 10) {
                        day = "0" + day;
                    }
                    return year + "-" + month + "-" + day;

                }
                $scope.myData = data.d;

            },
            error: function (data, status, headers, config) {
                alert("Error occured while fetching saved milestone. "
                      + x.responseText);
            }
        });

    };


    /*---------------------------------------------- FUNCTION To Insert Milestone -------------------------------------------------- */

    $scope.insertMileStone = function () {

        if ($scope.myData.length <= 0) {
            //HideLoading();
            alert('Add a milestone before saving');
            return;
        }
        else {
            for (var i = 0; i < $scope.myData.length; i++) {
                if ($scope.myData[i].EstHours == null)
                    $scope.myData[i].EstHours = 0;
            }
        }
        ShowLoading();
        var displayedDataAsJSON = JSON.stringify($scope.myData);
        // alert(displayedDataAsJSON);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "Milestone.aspx/PostData",
            data: "{'hdnJSONData':'" + displayedDataAsJSON + "'}",
            dataType: "json",
            async: false,
            success: function (data) {

                setTimeout(function () {
                    $('.popup').removeClass('transitioning');
                }, 0);
                alert('Milestone Saved Successfully');

                Milestone();
                getTotalBal();
                getTotalHr();
                HideLoading();
            },
            error: function (x, e) {
                alert("Error occured while fetching saved milestone. "
                      + x.responseText);
            }
        });
    };

    /*---------------------------------------------- FUNCTION To Delete Milestone -------------------------------------------------- */
    var deleteMileStone = function (projmilestoneid) {
        $.ajax({
            type: "POST",
            url: "Milestone.aspx/DeleteMile",
            data: "{'ProjectMileStoneID':'" + projmilestoneid + "'}",
            contentType: "application/json;charset=utf-8",
            success: function (projmilestoneid) {
            },
            error: function (x, e) {
                alert("The call to the server side failed. "
                      + x.responseText);
            }
        });
    }
    /*----------------------------------------- FUNCTION To Remove Row -------------------------------------------------- */
    $scope.removeRow = function () {
        if (confirm('Do you want to delete?')) {
            var index = this.row.rowIndex;
            deleteMileStone(this.row.entity.projMilestoneID);
            $scope.gridMileStone.selectItem(index, false);
            $scope.myData.splice(index, 1);
            validateGrid();
            validatedata();
        }
    };
    /*----------------------------------------- FUNCTION To Copy Row -------------------------------------------------- */
    $scope.copyRow = function () {
        $scope.milestone_form.$invalid = false;

        var Description = this.row.entity.Description;
        var amount = this.row.entity.amount;
        var ExRate = this.row.entity.ExRate;
        var DeliveryDate = this.row.entity.DeliveryDate;
        var DueDate = this.row.entity.dueDate;
        var EstHours = this.row.entity.EstHours;
        var name = this.row.entity.name;
        var IsRecurring = this.row.entity.IsRecurring;
        var projId = this.row.entity.projID;
        $scope.myData.push({ "Description": Description, "amount": amount, "ExRate": ExRate, "DeliveryDate": DeliveryDate, "dueDate": DueDate, "EstHours": EstHours, "insertedBy": user, "mode": null, "modifiedBy": user, "name": name, "projID": projId, "projMilestoneID": 0, "xml": null, "IsRecurring": IsRecurring });
    }

    /*----------------------------------------- FUNCTION To Add New Row -------------------------------------------------- */
    $scope.Add = function () {
        $scope.myData.push({ "Description": "", "amount": "", "ExRate": $("#hdnExRate").val(), "DeliveryDate": "", "dueDate": "", "EstHours": null, "insertedBy": user, "mode": null, "modifiedBy": user, "name": "", "projID": $scope.Projects, "projMilestoneID": 0, "xml": null, "IsRecurring": false });
    }
    /*-----------------------------------------GRID VALIDATION FUNCTION STARTS-------------------------------------------------- */
    $scope.validateCell = function () {
        validateGrid();
    }
    var validateGrid = function () {
        for (var i = 0; i < $scope.myData.length; i++) {
            if ($scope.myData[i].name == "" || $scope.myData[i].amount == "" || $scope.myData[i].ExRate == "" || $scope.myData[i].EstHours == "") {
                $scope.milestone_form.$invalid = true;
                break;
            }
            else {
                $scope.milestone_form.$invalid = false;
            }
        }

    }
    var validatedata = function () {
        if ($scope.myData.length == 0) {
            $scope.milestone_form.$invalid = true;
        }
    }
    $scope.validatedate = function (row) {

        var minDate = "01/01/1850";
        var minDt = new Date(minDate);
        var mnDt = minDt.getTime();
        var maxDate = "12/ 31/4025";
        var maxDt = new Date(maxDate);
        var mxDt = maxDt.getTime();
        var currentDate = row.entity.dueDate;
        var currDt = new Date(currentDate);
        var cuDt = currDt.getTime();
        if (cuDt > mnDt && cuDt < mxDt) {
            $scope.milestone_form.$invalid = false;
        }
        else {
            $scope.milestone_form.$invalid = true;
        }
    };
    /*-----------------------------------------GRID VALIDATION FUNCTION ENDS-------------------------------------------------- */

    var GetMilestoneDetails = function (projId) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "Milestone.aspx/BindMileStone",
            data: "{'projid':'" + projId + "'}",
            dataType: "json",
            async: false,
            success: function (msg) {
                id = projId;
                Milestone();
            },
            error: function (x, e) {
                alert("The call to the server side failed. "
                      + x.responseText);
            }
        });
    };
    ShowLoading();
    GetMilestoneDetails($("#hdnProjID").val());
    HideLoading();
});






















