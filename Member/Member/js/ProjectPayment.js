$(document).ready(function () {

});

var app = angular.module('myApp', ['ngGrid']);

//app.directive('ngBlur', ['$parse', function ($parse) {
//    return function (scope, element, attr) {
//        var fn = $parse(attr['ngBlur']);
//        element.bind('blur', function (event) {
//            scope.$apply(function () {
//                fn(scope, { $event: event });
//            });
//        });
//    }
//}]);
//var Display = "Amount";
app.controller('Payment_milestoneCtrl', function ($scope, $http) {

    var User = $('[id$="hdninsertedby"]').val();
    var total = 0;
    $scope.invalidSubmitAttempt = false;
    var removeTemplate = '<input type="button" value="" ng-click="removeRow($index)" class="buttondel" style="width:22px;height:23px;" />';
    $scope.Invoice = [];

    $scope.Invoice.myData = [];

    $scope.Projects = [];

    $scope.gridInvoice_MileStone =
        {
            data: 'Invoice.myData',
            enableCellSelection: true,
            enableCellOnFocus: false,
            enableRowSelection: false,
            tabIndex: 0,
            noTabInterference: true,
            columnDefs: [

                          {
                              field: 'InvoiceNo',
                              displayName: 'Invoice No',
                              cellTemplate: '<input type="text" ng-model="COL_FIELD" name="invoiceNo" required style="width:125px;height:17px;text-align:left" onkeypress="return false;" readonly="readonly">',
                              enableCellEdit: false,
                              width: 125
                          },
                          {
                              field: 'invoiceDate',
                              displayName: 'Date',
                              cellTemplate: '<input type="text" ng-model="COL_FIELD" name="invoiceDate" required style="width:125px;height:17px;text-align:left" onkeypress="return false;" readonly="readonly">',
                              enableCellEdit: false,
                              width: 125
                          },
                          {
                              field: 'DisplayAmount',
                              displayName: 'Amount',
                              cellTemplate: '<input name="Balance" type="text" ng-model="COL_FIELD" required style="width:117px;height:17px;text-align:right" onkeypress="return false;" readonly="readonly"/>',
                              enableCellEdit: false,
                              width: 125
                          },
                          {
                              field: 'DisplayBalance',
                              displayName: 'Balance',
                              cellTemplate: '<input name="price" type="text" ng-model="COL_FIELD" required style="width:109px;height:17px;text-align:right" onkeypress="return false;" readonly="readonly"/>',
                              enableCellEdit: false,
                              width: 125
                          },
                          {
                              field: 'Amount',
                              displayName: 'Amount',
                              cellTemplate: '<input name="Balance" type="text" ng-model="COL_FIELD" required style="width:117px;height:17px;text-align:right" onkeypress="return false;" readonly="readonly"/>',
                              enableCellEdit: false,
                              visible: false,
                              width: 125
                },

                
                          {
                              field: 'InvBalance',
                              displayName: 'Balance',
                              cellTemplate: '<input name="price" type="text" ng-model="COL_FIELD" required style="width:109px;height:17px;text-align:right" onkeypress="return false;" readonly="readonly"/>',
                              enableCellEdit: false,
                              visible: false,
                              width: 125
                          },
                          {
                              field: 'payAmount',
                              displayName: 'Payment',
                              cellTemplate: '<input type="text" ng-model="COL_FIELD" ng-change="calculatePayment()" style="width:153px;height:17px;text-align:right"/>',
                              enableCellEdit: true,
                              width: 165
                          },
                          {
                              field: 'ProjectInvoiceID',
                              displayName: 'InvoiceID',
                              cellTemplate: '<input type="text" ng-model="COL_FIELD" name="invoiceNo" required style="width:105px;height:17px;text-align:right">',
                              enableCellEdit: false,
                              visible: false,
                              width: 125
                          },
                        {
                            field: 'bFlag',
                            displayName: 'bFlag',
                            cellTemplate: '<input type="text" ng-model="COL_FIELD" name="invoiceNo" required style="width:105px;height:17px;text-align:right">',
                            enableCellEdit: false,
                            visible: false,
                            width: 125
                        },
                        {
                            field: 'AppliedCreditAmount',
                            displayName: 'AppliedCreditAmount',
                            cellTemplate: '<input type="text" ng-model="COL_FIELD" name="invoiceNo" required style="width:105px;height:17px;text-align:right">',
                            enableCellEdit: false,
                            visible: false,
                            width: 125
                        },

                          //{
                          //    field: 'delete',
                          //    displayName: '',
                          //    cellTemplate: removeTemplate

                          //}
            ]
        };

    $scope.addrow = function () {
        var aa = $scope.Invoice.myData.length;
        var ss = $scope.Invoice.myData[aa - 1];
        var pp = ss.ProjectMilestoneID;
        var oo = ss.Description;
        if (!(pp == "0" && oo == "")) {
            $scope.Invoice.myData.push({ projId: $scope.Invoice.Project, Description: "", Quantity: "", Rate: "", Amount: "0", ProjectMilestoneID: 0 });
        }
    }

    $scope.pInvoice = function ()
    {
        ClearFormData();
        var val = $('[id$="hdnProjID"]').val(); //$("#ddlProject").val().length;
        $scope.Invoice.Project = $('[id$="hdnProjID"]').val();
        if (val < 0) {
            return;
        }
        //else {
        //    //ClearFormData();
        //}
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ProjectPayments.aspx/GetInvoiceForPayment",
            data: "{'prjID':'" + val + "'}",
            dataType: "json",
            async: false,
            success: function (data, status, headers, config) {
                data = data.d;
                $scope.Invoice.customerName = data.customerName;

                $scope.Invoice.Milestone = data;

                $scope.Invoice.ProjectName = data.ProjectName;
                $scope.Invoice.customerName = data.customerName;
                $scope.Invoice.customerAddress = data.customerAddress;
                $scope.Invoice.CurrencyID = data.CurrencyID;
                $scope.Invoice.currSymbol = data.currSymbol;
                $scope.Invoice.ExRate = data.ExRate;
                $scope.Invoice.Description = data.Description;
                $scope.Invoice.InsertedBy = User;
                // $scope.Invoice.Amount = data.Amount;
                $scope.Invoice.ProjectPaymentID = data.ProjectPaymentID;
                $scope.Invoice.PaymentType = data.PaymentType;
                $scope.Invoice.BalanceAmount = data.BalanceAmount;
                $scope.Invoice.CreditAmount = data.CreditAmount;
                $scope.Invoice.PreviousCredit = data.PreviousCredit;
                $scope.Invoice.isCredited = data.isCredited;
                $scope.Invoice.CreditedPaymentID = data.CreditedPaymentID;
                $scope.Invoice.NoOfCredit = data.NoOfCredit;

                $scope.DisplayCreditAmountFormated = (data.CreditAmount).toLocaleString();
                $scope.DisplayCreditAmount = data.CreditAmount;
                $scope.lblPaymentTotal = "Payment + Credit ";

                //$scope.gridInvoice_MileStone.ngGrid.buildColumns();
                //Display = "Amount (" + $scope.Invoice.currSymbol + ")";
                for (var i = 0; i < data.lstModel.length; i++)
                    data.lstModel[i].payAmount = 0;
                $scope.Invoice.myData = data.lstModel;

                getPaymentType();
                getCurrency();
                //Enablecontrol();

                $scope.InvoiceID = "";
                $scope.Invoice.Amount = 0;
                AddCreditAmount();
                AdjustInvoicePayment();
                total = 0;
                GetInvoiceID(); // to get invoiceid if credit amount is added to tht invoices;

                var todayDate = kendo.toString((new Date()), 'dd/MM/yyyy');
                $("#txtInvoiceDate").data("kendoDatePicker").value(todayDate);
                $scope.Invoice.InvoiceDate = todayDate;
            },
            error: function (data, status, headers, config) {
                alert("some error occured payment ");
            }
        });

        return true;

    };

  

    var GetInvoiceID = function () {
        for (var i = 0; i < $scope.Invoice.myData.length ; i++) {
            if ($scope.Invoice.myData[i].payAmount > 0) {
                $scope.InvoiceID = $scope.InvoiceID + $scope.Invoice.myData[i].ProjectInvoiceID + ',';  // to get invoiceid in which credit amount is added
                total += $scope.Invoice.myData[i].payAmount;
            }
        }

    }

    var GetTotalPayAmount = function () {
        total = 0;
        for (var i = 0; i < $scope.Invoice.myData.length ; i++) {
            if ($scope.Invoice.myData[i].payAmount > 0) {
                total = parseFloat(total) + parseFloat($scope.Invoice.myData[i].payAmount);

            }
        }
        if (total < $scope.DisplayCreditAmount && $scope.AddAmount == 1)  // if creditamount used and total is less than available credit then minus the credit amount
            $scope.Invoice.CreditAmount = parseFloat($scope.DisplayCreditAmount) - parseFloat(total)
    }

    var BalanceInvoiceamount = function () {
        for (i = 0; i < $scope.Invoice.myData.length; i++) {
            $scope.Invoice.myData[i].InvBalance = parseInt($scope.Invoice.myData[i].Amount) - parseInt($scope.Invoice.myData[i].payAmount);
        }
    }

    $scope.calculateGridPayment = function () {

        AdjustInvoicePayment();
    }

    var ReCalculate = function (itirate) {
        for (i = itirate; i < $scope.Invoice.myData.length; i++) {
            $scope.Invoice.myData[i].payAmount = 0;
        }
    }
    var AdjustInvoicePayment = function () {
        var amount = 0;

        if ($scope.AddAmount == 1)
            amount = parseInt($scope.Invoice.Amount) + parseInt($scope.DisplayCreditAmount);
        else if ($scope.isEdit == 0)
            amount = parseInt($scope.Invoice.Amount);// + parseInt($scope.TempPreviousCredit); //$scope.Invoice.Amount; <-- edit mode
        else
            amount = parseInt($scope.Invoice.Amount);
        $scope.Invoice.BalanceAmount = amount
        for (i = 0; i < $scope.Invoice.myData.length; i++) {
            var invAmount = 0;
            if ($scope.isEdit > 0) {
                if ($scope.Invoice.myData[i].InvBalance > 0) {
                    invAmount = $scope.Invoice.myData[i].InvBalance;

                }
                else
                    invAmount = $scope.Invoice.myData[i].Amount; // Amount  //InvBalance
            }
            else
                invAmount = $scope.Invoice.myData[i].PreviousBalance; //to check with previous balance which was at the time of payment ,//  $scope.Invoice.myData[i].Amount;

            if (amount <= invAmount) {
                $scope.Invoice.myData[i].payAmount = amount;
                amount = 0;
                $scope.Invoice.CreditAmount = amount;
                ReCalculate(i + 1);
                return;
            }
            if (invAmount != amount) {
                var adjust = 0;
                adjust = amount;
                if (amount > invAmount) {
                    amount = amount - invAmount;
                    $scope.Invoice.myData[i].payAmount = (adjust - amount);
                }
                else
                    $scope.Invoice.myData[i].payAmount = amount;
            }
            // total += $scope.Invoice.myData[i].payAmount;

        }
        $scope.Invoice.CreditAmount = amount;
        //CheckCreditAmount();
    }

    var CheckCreditAmount = function () {

        if ($scope.Invoice.CreditAmount > 0) {

            var credit = $scope.Invoice.currSymbol + $scope.Invoice.CreditAmount;
            //alert(credit + " : This amount will be kept credited.");
            if (confirm(credit.toLocaleString() + " : This amount will be kept credited.")) {
                $scope.Invoice.CreditAmount = parseInt($scope.BalanceCredit) + parseInt($scope.Invoice.CreditAmount);
                return true;
            }
            else
                return false;

        }
        GetTotalPayAmount();
        return true;
    }

    $scope.calculatePayment = function () {
        if ($scope.isEdit > 0)
            totalCalculatePrice();
        else
            CalculateEditedPrice();
    }
    var totalCalculatePrice = function () {
        total = 0;

        for (i = 0; i < $scope.Invoice.myData.length; i++) {

            if ($scope.Invoice.myData[i].payAmount <= $scope.Invoice.myData[i].InvBalance) {
                total += parseInt($scope.Invoice.myData[i].payAmount);
            }
            else
                $scope.Invoice.myData[i].payAmount = 0;

        }
        $scope.Invoice.BalanceAmount = total;
    }

    var CalculateEditedPrice = function () {
        var total = 0;
        var invAmount = 0;
        for (i = 0; i < $scope.Invoice.myData.length; i++) {
            //if ($scope.Invoice.myData[i].InvBalance > 0)
            //    invAmount = $scope.Invoice.myData[i].InvBalance;
            //else
            invAmount = $scope.Invoice.myData[i].PreviousBalance;// to check with previuos balance which was there during the payment time .//$scope.Invoice.myData[i].Amount;

            if ($scope.Invoice.myData[i].payAmount <= invAmount) {
                total += parseInt($scope.Invoice.myData[i].payAmount);
            }
            else
                $scope.Invoice.myData[i].payAmount = 0;//$scope.Invoice.myData[i].Amount;

        }
        $scope.Invoice.BalanceAmount = total;
    }

    var getProjects = function () {
        $http.get("ProjectPayments.aspx/getProjects")
              .success(function (data, status, headers, config) {
                  $scope.Invoice.Projects = data;
              })
              .error(function (data, status, headers, config) {
                  alert("Some error occured.");
              });
    }

   
    getProjects();

    var getPaymentType = function () {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ProjectPayments.aspx/getPaymentType",
            dataType: "json",
            async: false,
            success: function (data, status, headers, config) {
                $scope.Invoice.PaymentTypes = data.d;
                $scope.Invoice.PaymentType = 2;
            },
            error: function (data, status, headers, config) {
                alert("Some error occured.");
            }
        });
    }
  
    getPaymentType();
    var Diablecontrol = function () {
        $scope.isDiabled = true;
        $scope.isEdit = 0;
    }
    var Enablecontrol = function () {
        $scope.isDiabled = false;
        $scope.isEdit = 1;
    }

    //----------toconfirm whter to use credit amount in payment or not during payments
    var AddCreditAmount = function () {
        //ConfirmBox();
        $scope.BalanceCredit = 0;
        if ($scope.Invoice.CreditAmount > 0) {
            if (confirm("There is a Credit of " + $scope.Invoice.currSymbol + $scope.Invoice.CreditAmount.toLocaleString() + ". Do you want to pay against invoice")) {
                // $scope.Invoice.Amount = $scope.Invoice.CreditAmount;
                $scope.BalanceCredit = 0;
                $scope.Invoice.CreditAmount = 0;
                $scope.Invoice.PaymentType = 7;
                $scope.calculateGridPayment();
                $scope.AddAmount = 1;
                $scope.Invoice.isCredited = 1;
                $scope.Invoice.TaxCollected = 0;
                // alert("balanceCredit : " + $scope.BalanceCredit);
            }
            else {
                $scope.Invoice.Amount = "";
                $scope.Invoice.PaymentType = 1;
                $scope.BalanceCredit = $scope.Invoice.CreditAmount;
                $scope.AddAmount = 0;
                $scope.Invoice.CreditAmount = 0;
            }
        }
        else {
            $scope.Invoice.Amount = "";
            $scope.Invoice.CreditAmount = 0;
        }
    }

    // ---- to add credit amount  to the amount entered

    $scope.AdjustCerdit = function () {
        if ($scope.AddAmount == 1) {
            // $scope.Invoice.Amount = $scope.Invoice.BalanceAmount = parseInt($scope.Invoice.Amount) + parseInt($scope.DisplayCreditAmount);
            // $scope.Invoice.BalanceAmount = parseInt($scope.Invoice.Amount) + parseInt($scope.DisplayCreditAmount);

            AdjustInvoicePayment();
        }

    }

    var getCurrency = function () {
        Enablecontrol();
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ProjectPayments.aspx/getCurrency",
            dataType: "json",
            async: false,
            success: function (data, status, headers, config) {
                $scope.Invoice.Currencys = data.d;
            },
            error: function (data, status, headers, config) {
                alert("Some error occured.");
            }
        });
    }
   

    getCurrency();
    //Enablecontrol();
    var Loaddate = function () {
        $('[id$="txtInvoiceDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
    }

    $scope.EditInvoicePayment = function (PaymentID) {
        $('[id$="btnSendMail"]').show();
        $('[id$="btnDelete"]').show();

        $scope.Invoice.ProjectPaymentID = PaymentID;
        Loaddate();
        // var val = $("#ddlProject").val().length;
        // if (val > 0) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ProjectPayments.aspx/GetInvoiceForEditPayment",
            data: "{'paymentID':'" + PaymentID + "'}",
            dataType: "json",
            cache: false,
            async: false,
            success: function (data) {
                data = data.d;
                $scope.Invoice.customerName = data.customerName;

                getProjects();
                $scope.Invoice.Milestone = data;

                $scope.Invoice.ProjectName = data.ProjectName;
                $scope.Invoice.customerName = data.customerName;
                $scope.Invoice.customerAddress = data.customerAddress;
                $scope.Invoice.CurrencyID = data.CurrencyID;
                $scope.Invoice.currSymbol = data.currSymbol;
                $scope.Invoice.ExRate = data.ExRate;
                $scope.Invoice.InvoiceDate = data.InvoiceDate;
                $scope.Invoice.InsertedBy = User;
                $scope.Invoice.Amount = data.Amount;
                $scope.Invoice.ProjectPaymentID = data.ProjectPaymentID;
                //$scope.Invoice.PaymentType = data.PaymentType;
                $scope.Invoice.Project = data.ProjID;
                $scope.Invoice.BalanceAmount = data.Amount; //"";
                $scope.Invoice.CreditAmount = data.CreditAmount;
                $scope.DisplayCreditAmount = data.CreditAmount;
                $scope.Invoice.PreviousCredit = data.PreviousCredit;
                //if ($scope.Invoice.CreditAmount <= 0)
                //    $scope.Invoice.CreditAmount = data.PreviousCredit;
                $scope.TempPreviousCredit = data.PreviousCredit;  //  <-- used for calcuation on amount texbox change
                $scope.Invoice.TaxCollected = data.TaxCollected;
                $scope.Invoice.isCredited = data.isCredited;
                $scope.Invoice.CreditedPaymentID = data.CreditedPaymentID;

                $scope.Invoice.Description = data.Description;
                $('[id$="hdnProjectInvoiceId"]').val(data.ProjectPaymentID);
                $scope.DisplayCreditAmountFormated = ($scope.Invoice.CreditAmount).toLocaleString();
                $scope.lblPaymentTotal = "Payment + Credit";

                $scope.Invoice.myData = data.lstModel;
                $scope.BalanceCredit = 0;
                $scope.AddAmount = 0;
                getPaymentType();
                $scope.Invoice.PaymentType = data.PaymentType;
                getCurrency();
                Diablecontrol();

                //var pdate = kendo.toString($scope.Invoice.InvoiceDate, 'dd/MM/yyyy');
                //$("#txtInvoiceDate").data("kendoDatePicker").value(pdate);
                //$scope.Invoice.InvoiceDate = pdate;
                //AddCreditAmount();
                //  AdjustInvoicePayment(); // <--- used to add cerdit + amount 
            },
            error: function (x,e) {
                alert("some error occured in Invoice Payments ");
            }            
        });
        return true;
    }
   
  

    $scope.SavePaymentTypes = function () {
        var type = $scope.Invoice.PTypes;
        var postdata =
         {
             'PTypes': $scope.Invoice.PTypes
         }
        //ShowLoading();
        var re = JSON.stringify(postdata);
        $scope.isProcessing = true;
        $.ajax({
            type: "POST",
            url: "ProjectPayments.aspx/PostPaymentTypes",
            data: "{'JSONData':'" + re + "'}",
            async: false,
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                $scope.isProcessing = false;
                if (data.d != "0") {
                    $("#divPaymentType").css("display", "none");
                    //HideLoading();
                    alert("PaymentType Saved Successfully");
                    getPaymentType();
                    Enablecontrol();

                }
                else {
                    //HideLoading();
                    alert("PaymentType already exists");
                }
                // location.reload();
            },
            error: function (x, e) {
                alert("PaymentType Not saved. "
                      + x.responseText);
                location.reload();
            }
        });
    }
   
    var tempcredit;
    var ReCalculateCreditAmount = function () {
        var PreviousCredit;
        var EnterdAmount = parseFloat($scope.Invoice.Amount) + parseFloat($scope.Invoice.PreviousCredit);
        if ($scope.DisplayCreditAmount <= 0)
            PreviousCredit = $scope.Invoice.PreviousCredit;
        else
            PreviousCredit = $scope.DisplayCreditAmount;
        tempcredit = 0;
        if (PreviousCredit == 0)
            return;

        for (var i = 0; i < $scope.Invoice.myData.length; i++) {
            if ($scope.Invoice.myData[i].payAmount != 0) {
                var temp = parseFloat($scope.Invoice.myData[i].PreviousBalance) - parseFloat(EnterdAmount);
                EnterdAmount = Math.abs(temp);
                if (temp < 0) { //---------add to previous credit if temp is below 0.
                    $scope.Invoice.myData[i].InvBalance = 0
                    tempcredit = EnterdAmount;
                }
                else {
                    $scope.Invoice.myData[i].InvBalance = temp;
                }
            }
            else {
                $scope.Invoice.myData[i].InvBalance = $scope.Invoice.myData[i].PreviousBalance;
            }
        }
        //if (tempcredit > 0)
        //    $scope.Invoice.PreviousCredit = parseFloat(PreviousCredit) + parseFloat(tempcredit);
    }

    $scope.SaveInvoicePayment = function (ngForm) {
        if ($scope.Invoice.BalanceAmount <= $scope.Invoice.Amount || $scope.Invoice.BalanceAmount == 0) {
            if ($scope.Invoice.BalanceAmount < $scope.Invoice.Amount || $scope.Invoice.BalanceAmount == 0) {
                $scope.Invoice.CreditAmount = parseFloat($scope.Invoice.Amount) - parseFloat($scope.Invoice.BalanceAmount); // to keep left out payment as credit amount

                var credit = $scope.Invoice.currSymbol + $scope.Invoice.CreditAmount;
                if ($scope.Invoice.CreditAmount != "0") {
                    if (confirm(credit.toLocaleString() + " : This amount will be kept credited.")) {
                        // $scope.Invoice.CreditAmount = parseInt($scope.BalanceCredit) + parseInt($scope.Invoice.CreditAmount);                   
                    }
                    else
                        return false;
                }
            }
            else if (!CheckCreditAmount()) {
                return;
            }
        }
        else if (!CheckCreditAmount()) {
            return;
        }
        else if ($scope.AddAmount != 1) {
            alert("Total should match than amount entered");
            return false;
        }

        insertInvoicePayment();

    }
  

    /// to get remainingg credit amount after credit amount is added to the invoice payments
    var CheckAvailableCredit = function () {

        GetTotalPayAmount();
        var credit = 0
        if ($scope.Invoice.Amount <= 0) {
            if ($scope.DisplayCreditAmount >= total) {
                credit = $scope.Invoice.CreditAmount = parseFloat($scope.DisplayCreditAmount) - parseFloat(total);
                if (credit > 0) {
                    credit = $scope.Invoice.currSymbol + credit.toLocaleString();
                    if (!(confirm(credit + " : This amount will be kept credited.")))
                        return false;
                }
                return true;
            }
        }
        else {
            var grandAmount = parseFloat($scope.DisplayCreditAmount) + parseFloat($scope.Invoice.Amount)
            credit = $scope.Invoice.CreditAmount = parseFloat(grandAmount) - parseFloat(total);
            if (credit > 0) {
                credit = $scope.Invoice.currSymbol + credit.toLocaleString();
                credit = credit.toLocaleString();
                if (!(confirm(credit + " : This amount will be kept credited.")))
                    return false;
            }
            return true;
        }

        return true;
    }

    var getInvoiceIDFromGrid = function () {

        for (var i = 0; i < $scope.Invoice.myData.length ; i++) {
            $scope.Invoice.myData[i].IDs = $scope.InvoiceID; // to get invoiceid in which credit amount is added

        }
        //---------------calcuation for  how much credit amount is used in which invoices
        GetCreditUsedInvoices();
    }

    var GetCreditUsedInvoices = function () {


        if ($scope.Invoice.isCredited == 1) {

            var Amount = $scope.Invoice.Amount; // enterd amount
            var DiffAmount, RemaingCredit, CreditAmount, Adjust, PayAmount;
            CreditAmount = $scope.DisplayCreditAmount;
            var InvoiceID = "";
            //if (Amount > "0") {
            for (var i = 0; i < $scope.Invoice.myData.length ; i++) {
                var InvAmount = $scope.Invoice.myData[i].Amount;

                //---------- to get paymount from the amount entered for further calcualtion purpose
                if (InvAmount < Amount) {
                    $scope.Invoice.myData[i].bFlag = 1; // it is for to check payment from credit or not, New = 1 means payment not from credit amount
                    PayAmount = InvAmount
                    Amount = parseFloat(Amount) - parseFloat(InvAmount);
                }
                else {
                    Adjust = Math.abs(parseFloat(InvAmount) - parseFloat(Amount));
                    PayAmount = parseFloat(InvAmount) - Adjust;
                    Amount = Adjust;
                }

                if (InvAmount > PayAmount) {
                    $scope.Invoice.myData[i].bFlag = 0;
                    InvoiceID = InvoiceID + $scope.Invoice.myData[i].ProjectInvoiceID + ","; // invoiceid in which credit is applied
                    DiffAmount = parseFloat(InvAmount) - parseFloat(PayAmount); // to get difference bwtween invoice amount and pay amount                    
                    RemaingCredit = Math.abs(parseFloat(DiffAmount) - parseFloat(CreditAmount)); // calculate how much credit amount is used in this invoice
                    CreditAmount = RemaingCredit;

                    $scope.Invoice.myData[i].IDs = $scope.Invoice.myData[i].ProjectInvoiceID;   // to get invoiceid in which credit amount is added
                    $scope.Invoice.myData[i].AppliedCreditAmount = DiffAmount // to store credit amount applied the particular invoice
                    $scope.Invoice.myData[i].AmountEntered = PayAmount;
                }

                //$scope.Invoice.myData[i].IDs = $scope.Invoice.myData[i].ProjectInvoiceID;   // to get invoiceid in which credit amount is added
            }
            var AppliedCreditAmount = parseFloat($scope.DisplayCreditAmount) - parseFloat(RemaingCredit);
            $scope.AppliedCreditAmount = AppliedCreditAmount; // credit amount used in payments
            $scope.InvoiceID = InvoiceID;
        }
    }

    var insertInvoicePayment = function () {
        $scope.Invoice.InvoiceDate = $('[id$="txtInvoiceDate"]').val();
       // $scope.Invoice.TaxCollected = $('[id$="TaxCollected"]').val();
        if ($scope.Invoice.CreditAmount == 0) {
            if ($scope.AddAmount == 1) {
                //$scope.Invoice.Amount = parseFloat($scope.Invoice.Amount) + parseFloat($scope.Invoice.DisplayCreditAmount);
                $scope.Invoice.CreditAmount = 0;
            }
            //else
            //    $scope.Invoice.CreditAmount = $scope.DisplayCreditAmount;

        }

        if ($scope.isEdit == 1)// for new payment
        {
            //if ($scope.Invoice.isCredited != 1)
            $scope.Invoice.ProjectPaymentID = 0;

            $scope.Invoice.ProjectDetailID = 0;
        }
        getInvoiceIDFromGrid();
        var postdata =
        {

            'ProjectPaymentID': $scope.Invoice.ProjectPaymentID,
            'ProjID': $scope.Invoice.Project,
            'Amount': $scope.Invoice.Amount,
            'InsertedBy': $scope.Invoice.InsertedBy,
            'PaymentType': $scope.Invoice.PaymentType,
            'CurrencyID': $scope.Invoice.CurrencyID,
            'ExRate': $scope.Invoice.ExRate,
            'CreditAmount': $scope.Invoice.CreditAmount,
            'isEdited': $scope.isEdit,
            'Description': $scope.Invoice.Description,
            'InvoiceDate': $scope.Invoice.InvoiceDate,
            'isCredited': $scope.Invoice.isCredited,
            'AppliedCreditAmount': $scope.AppliedCreditAmount,
            'CreditedPaymentID': $scope.Invoice.CreditedPaymentID,
            'NoOfCredit': $scope.Invoice.NoOfCredit,
            'lstModel': $scope.Invoice.myData,
            'TaxCollected': $scope.Invoice.TaxCollected

        }
        var re = JSON.stringify(postdata);
        $scope.isProcessing = true;
        $.ajax({
            type: "POST",
            url: "ProjectPayments.aspx/PostInvoicePayments",
            data: "{'JSONData':'" + re + "'}",
            async: false,
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                $scope.isProcessing = false;
                $("#divAddPopup").css("display", "none");
                setTimeout(function () {
                    $('.popup').removeClass('transitioning');
                }, 0);
                $scope.Invoice = [];
                $('#popupmsg').text("Invoice Payments Saved Successfully");

                $('#popup-box').addClass('visible');
                $('html').addClass('overlay');
                alert("Invoice Payments Saved Successfully");
                ClearFormData();
                location.reload();
            },
            error: function (x, e) {
                alert("Invoice Payments Not saved. "
                      + x.responseText);
                location.reload();
            }
        });
    }
   


    $scope.removeRow = function () {
        if ($scope.Invoice.myData.length > 1) {
            if (confirm('Do you want to delete?')) {
                var index = this.row.rowIndex;
                deleteInvoiceMileStone(this.row.entity.ProjectInvoiceDetailID);
                $scope.gridInvoice_MileStone.selectItem(index, false);
                $scope.Invoice.myData.splice(index, 1);
            }
        }
    };

    $scope.CloseType = function () {
        $('#divPaymentType').css('display', 'none');
        Enablecontrol();
    }

    $scope.ClosePopUp = function () {

        $scope.invalidSubmitAttempt = false;
        $('#divAddPopup').css('display', 'none');
        $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
        $('#divPaymentType').css('display', 'none');
        $('#divMail').css('display', 'none');

        ClearFormData();

        // ConfirmBox();
    };

    $scope.CloseMail = function () {

        $scope.invalidSubmitAttempt = false;
        $('#divMail').css('display', 'none');
        $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");


    };

    //$scope.ConfirmTest = function ()
    //{
    //    ConfirmBox();
    //}
    $scope.OpenPopUp = function ()
    {
        $('#divAddPopup').css('display', '');
        $('#divAddPopupOverlay').addClass('k-overlay');

        $('[id$="txtInvoiceDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
        $('[id$="txtDueDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
        $('[id$="btnSendMail"]').hide();
        $('[id$="btnDelete"]').hide();
        getProjects();
        getPaymentType();
        getCurrency();
        $scope.pInvoice();
    };

    $scope.AddTypes = function () {
        Diablecontrol();
        $('#divPaymentType').css('display', '');
        $('#divAddPopupOverlay').addClass('k-overlay');

    };

    var ConfirmBox = function () {
        $('#divConfirm').css('display', '');
        $('#divAddPopupOverlay').addClass('k-overlay');
    }
    function ClearFormData() {
        $scope.Invoice.Milestone = [];
        $scope.Invoice.customerName = "";
        $scope.Invoice.customerAddress = "";
        $scope.Invoice.InCurrency = "";
        $scope.Invoice.CurrencyID = "";
        $scope.Invoice.ExRate = "";
        $scope.Invoice.Comment = "";
        $scope.Invoice.tax1 = 0;
        $scope.Invoice.tax2 = 0;
        $scope.Invoice.transcharge = 0;
        //  $scope.Invoice.insertedby = "1000";
        $scope.Invoice.totalPrice = "";
        $scope.Invoice.grandTotal = "";
        $scope.Invoice.InvoiceDate = "";
        $scope.Invoice.myData = [];
        $scope.Invoice.Project = "";
        $scope.Invoice.Amount = "";
        $scope.Invoice.PaymentType = "";
        $scope.Invoice.payAmount = "";
        $scope.Invoice.BalanceAmount = "";
        $scope.DisplayCreditAmount = "";
        $scope.Invoice.TaxCollected = 0;

    }

    $scope.DeletePayment = function () {
        if (!confirm("Are you sure you want to delete this payment?"))
            return;

        $.ajax({
            type: "POST",
            url: "ProjectPayments.aspx/DeletePayment",
            contentType: "application/json;charset=utf-8",
            data: "{'PaymentID':" + parseInt($scope.Invoice.ProjectPaymentID) + "}",
            dataType: "json",
            async: false,
            success: function (data) {
                alert('Deleted Successfully.');
                $scope.ClosePopUp();
                location.reload();
            },
            error: function (x, e) {
                alert("The call to the server side failed. "
                      + x.responseText);
            }
        });
    }
});