var app = angular.module('myApp', ['ngGrid']); //, []
app.directive('ngBlur', ['$parse', function ($parse) {
    return function (scope, element, attr) {
        var fn = $parse(attr['ngBlur']);
        element.bind('blur', function (event) {
            scope.$apply(function () {
                fn(scope, { $event: event });
            });
        });
    }
}]);


app.controller('Invoice_milestoneCtrl', function ($scope, $http) {

    var User = $('[id$="hdninsertedby"]').val()
    var locationid = $('[id$="hdnLocationId"]').val()
    var ProjectName = $('[id$="hdnProjectName"]').val()
    var ProjectId = $('[id$="hdnProjID"]').val()

    $scope.invalidSubmitAttempt = false;
    var removeTemplate = '<input type="button" value="" ng-click="removeRow($index)" class="buttondel" style="width:22px;height:23px;text-align:center" />';
    //dipti
    //var addTemplate = '<div ng-show="row.rowIndex==$scope.Invoice.myData.length-1"><input type="button" value="Add" ng-click="addrow(row)" text="Add" style="width:40px;height:23px;" /></div>';
    var addTemplate = '<input type="button" value="" ng-click="addrow()" class="buttonadd" style="width:22px;height:23px;text-align:Center" ng-show="row.rowIndex==Invoice.myData.length-1"/>';
    //end

    $scope.Invoice = [];

    $scope.Invoice.myData = [];

    $scope.Projects = [];

    $scope.isInvoiceEdit = 0;

    $scope.addrow = function () {


        var myDataLen = $scope.Invoice.myData.length;
        var myDataobj = $scope.Invoice.myData[myDataLen - 1];
        var myDataobjMSId = myDataobj.ProjectMilestoneID;
        var myDataobjDesc = myDataobj.Description;
        if (!(myDataobjMSId == "0" && myDataobjDesc == "")) {
            //$scope.Invoice.myData.push({ projId: $scope.Invoice.Project, Description: "", Quantity: "", Rate: "", Amount: "0", ProjectMilestoneID: 0 });
            $scope.Invoice.myData.push({ projId: $scope.Invoice.Project, Description: "", OriginalAmount: 0, BalanceAmount: 0, Quantity: 1, Rate: "", Amount: "0", ProjectMilestoneID: 0 });
            //$scope.edit = false;
        }
        else { alert("Please fill current milestone details first!") }
    }

    $scope.FillMsDetails = function (row) {
        var myDataLen = $scope.Invoice.myData.length;
        var myDataobj = $scope.Invoice.myData[myDataLen - 1];
        var myDataobjMSId = myDataobj.ProjectMilestoneID;
        var myDataobjDesc = myDataobj.Description;
        if ((myDataobjMSId > 0)) {
            getmiledesc(myDataobjMSId, row);
        }
        else
            myDataobj.ProjectMilestoneID = 0;


    }

    $scope.pmile = function () {

        $('#divSummary').css('display', 'none');
        $('[id$="btnVoid"]').hide();
        $scope.IsVisiblePayment = true;
        $('#chkAddPayment').attr('checked', false);
        $scope.IsVisible = false;

        var val = $('[id$="hdnProjID"]').val(); //ProjectId;
        var locationID = $('[id$="hdnLocationId"]').val(); //LocationID;
        if (locationID <= 0) {
            //$scope.ClosePopUp();
            alert("Please select location before proceeding further.");
            $scope.ClosePopUp();
            return;
        }
        $scope.Invoice.Project = val;
        val = val + '@' + locationID;
        if (val != '') {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ProjectInvoices.aspx/GetInvoiceMile",
                data: "{'projid':'" + val + "'}",
                dataType: "json",
                async: false,
                success: function (data, status, headers, config) {
                    data = data.d;
                    $scope.TestInvoice = data;
                    $scope.Invoice.Milestone = data;

                    $scope.Invoice.customername = data[0].custName;
                    $scope.Invoice.customeraddress = data[0].custAddress;
                    $scope.Invoice.InCurrency = data[0].currSymbol;
                    $scope.Invoice.currencyId = data[0].currID;
                    $scope.Invoice.insertedby = User;
                    $scope.Invoice.ExRate = data[0].currExRate;
                    $scope.Invoice.InvoiceNo = data[0].PInvoiceNo;
                    $scope.CheckInvoiceNo = "";
                    $scope.Invoice.totalPrice = 0;
                    $scope.Invoice.myData = [];
                    //$scope.Invoice.myData.push({ projId: $scope.Invoice.Project, Description: "", Quantity: "", Rate: "", Amount: "0", ProjectMilestoneID: 0 });
                    $scope.Invoice.myData.push({ projId: $scope.Invoice.Project, Description: "", OriginalAmount: 0, BalanceAmount: 0, Quantity: 1, Rate: "", Amount: "0", ProjectMilestoneID: 0 });

                    //$scope.edit = false;
                    $scope.isInvoiceEdit = 0;
                    //alert($scope.Invoice.myData.length);

                    //if (data.length > 0)
                    //    $('[id$="btnAddMile"]').show();
                    //else
                    //    $('[id$="btnAddMile"]').hide();
                },
                error: function (data, status, headers, config) {
                    alert("some error occured in milestone ");
                }
            });
        }
        else {
            ClearFormData();
        }
        return true;

    };


    $("#ctl00_ContentPlaceHolder1_ProjectSacHsnCode").change(function ()
    {

        if ($("#ctl00_ContentPlaceHolder1_ProjectSacHsnCode").val() == 0)
        {
            $scope.Invoice.CGST = 0; //new 
            $scope.Invoice.SGST = 0;
            $scope.Invoice.IGST = 0;
            $scope.Invoice.GST = 0;
            document.getElementById("CGST").value = 0;
            document.getElementById("SGST").value = 0;
            $scope.caltax1();
        }

        else
        {
           
                var CodeID = $("#ctl00_ContentPlaceHolder1_ProjectSacHsnCode").val();
           // var Date = getCurrentDate();

            var currentDate = new Date();
            var mydate = new Date('2017-06-30');

            if (currentDate <= mydate) {
                document.getElementById("CGST").value = 0;
                document.getElementById("SGST").value = 0;
                $scope.Invoice.CGST = 0; //new 
                $scope.Invoice.SGST = 0;
                document.getElementById("IGST").value = 0;
                $scope.Invoice.IGST = 0; //new
                document.getElementById("GST").value = 0;
                $scope.Invoice.GST = 0;
            }
          else  {
                GetGSTPercent(CodeID);
                var GSTPercent = $('[id$="hdnProjectGSTPercentage"]').val();
                var CustState = $('[id$="hdnProjectCustStateId"]').val();
                var ClientState = $('[id$="hdnProjectClientStateId"]').val();
                var CustCountry = $('[id$="hdnProjectCustCountry"]').val();  //Added By Nikhil Shetye on 18-10-2017 for checking cust country
                if ((CustState == ClientState) && (CustCountry == "1")) {  //Added By Nikhil Shetye on 18-10-2017 for checking cust country
                    var percent = GSTPercent / 2;
                    document.getElementById("CGST").value = percent;
                    document.getElementById("SGST").value = percent;
                    $scope.Invoice.CGST = percent;
                    $scope.Invoice.SGST = percent; 
                }
                else {
                    document.getElementById("IGST").value = GSTPercent;
                    $scope.Invoice.IGST = GSTPercent;
                }

                if (CustCountry == "3") {
                    document.getElementById("IGST").value = 0;
                    $scope.Invoice.IGST = 0; //new
                }
            }
            $scope.caltax1();
        }

            $scope.$apply();
    });

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

    var BindMilestone = function () {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ProjectInvoices.aspx/GetInvoiceMile",
            data: "{'projid':'" + $scope.Invoice.Project + "'}",
            dataType: "json",
            async: false,
            success: function (data, status, headers, config) {
                data = data.d;
                $scope.Invoice.Milestone = data;
            },
            error: function (data, status, headers, config) {
                alert("some error occured in milestone ");
            }
        });
        return true;
    };


    var getProjects = function () {
        $http.get("ProjectInvoices.aspx/getProjects")
              .success(function (data, status, headers, config) {
                  $scope.Invoice.Projects = data;
              })
              .error(function (data, status, headers, config) {
                  alert("Some error occured1.");
              });
    }

    getProjects();
    
    $scope.EditInvoice = function (pInvId) {
        $('[id$="btnVoid"]').show();
       
        $('[id$="txtInvoiceDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
        $('[id$="txtDueDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
        $scope.IsVisiblePayment = false;
        $scope.IsVisible = false;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ProjectInvoices.aspx/GetInvoiceForEdit",
            data: "{'pInvId':'" + pInvId + "'}",
            dataType: "json",
            async: false,
            success: function (data, status, headers, config) {
                data = data.d;
                getProjects();

                //Added Code by Nikhil Shetye for setting hidden field
                var pid = "";
                var lid = "";
                var pid = $('[id$="hdnProjID"]').val();
                var lid = $('[id$="hdnLocationId"]').val();
                GetLocationDetails(pid, lid);
                //End Nikhil Shetye
                //Commented By Nikhil Shetye as there is no such hidden field on 16-10-2017
                //var CustState = $('[id$="hdnCustStateId"]').val();
                //var ClientState = $('[id$="hdnClientStateId"]').val();
                //End Nikhil Shetye on 16-10-2017
                //Added By Nikhil Shetye on 16-10-2017
                var CustState = $('[id$="hdnProjectCustStateId"]').val();
                var ClientState = $('[id$="hdnProjectClientStateId"]').val();
                var CustCountry = $('[id$="hdnProjectCustCountry"]').val();  //Added By Nikhil Shetye on 18-10-2017 for checking cust country
                //End
                var todayDate = getCurrentDate();
                $scope.edit = true;
                $scope.isInvoiceEdit = 1;

                $('[id$="hdnProjectInvoiceId"]').val(pInvId.toString());

                $scope.Invoice.Project = data.projId;
                $scope.Invoice.ProjectInvoiceID = pInvId;
                BindMilestone();
                $scope.Invoice.Milestone = data.Milestone;

                $("#ctl00_ContentPlaceHolder1_ProjectSacHsnCode").val(data.CodeId);
                var currentDate = new Date();
                var mydate = new Date('2017-06-30');
               
                

                //Commented by Trupti
               
                if (currentDate > mydate)
                {
                    if (CustCountry == 1) {

                        //if (CustState == ClientState) //Added By Nikhil Shetye on 18-10-2017 for checking cust country
                        if (CustState == 15) {
                            document.getElementById("IGST").disabled = true;
                            document.getElementById("txtGST").disabled = true;
                            $scope.Invoice.CGST = data.CGST;
                            $scope.Invoice.SGST = data.SGST;
                            $scope.Invoice.OtherCharge = data.OtherCharge;
                            //  $scope.Invoice.IGST = dat.IGST;//added by trupti

                        }
                        else {
                            document.getElementById("CGST").disabled = true;
                            document.getElementById("SGST").disabled = true;
                            document.getElementById("txtGST").disabled = true;
                            document.getElementById("txtTax1").disabled = true;
                            document.getElementById("tax2").disabled = true;
                            document.getElementById("tax3").disabled = true;
                            document.getElementById("txtVATCharges").disabled = true;
                            document.getElementById("txtCSTCharges").disabled = true;


                            $scope.Invoice.IGST = data.IGST;
                            $scope.Invoice.OtherCharge = data.OtherCharge;
                            //$scope.Invoice.GST = data.GST;
                        }


                    }
                    else if (CustCountry == 5) {
                        document.getElementById("txtTax1").disabled = true;
                        document.getElementById("tax2").disabled = true;
                        document.getElementById("tax3").disabled = true;
                        document.getElementById("txtVATCharges").disabled = true;
                        document.getElementById("txtCSTCharges").disabled = true;
                        document.getElementById("CGST").disabled = true;
                        document.getElementById("SGST").disabled = true;

                        document.getElementById("IGST").disabled = true;
                        document.getElementById("txtGST").disabled = true;
                        $scope.Invoice.OtherCharge = data.OtherCharge;
                    }
                    else {
                        document.getElementById("txtTax1").disabled = true;
                        document.getElementById("tax2").disabled = true;
                        document.getElementById("tax3").disabled = true;
                        document.getElementById("txtVATCharges").disabled = true;
                        document.getElementById("txtCSTCharges").disabled = true;
                        document.getElementById("CGST").disabled = true;
                        document.getElementById("SGST").disabled = true;

                        document.getElementById("IGST").disabled = true;
                        $scope.Invoice.GST = data.GST;
                        $scope.Invoice.OtherCharge = data.OtherCharge;

                    }
                   
                    document.getElementById("txtTax1").disabled = true;
                    document.getElementById("tax2").disabled = true;
                    document.getElementById("tax3").disabled = true;
                    document.getElementById("txtVATCharges").disabled = true;
                    document.getElementById("txtCSTCharges").disabled = true;

                }
                else
                {
                    document.getElementById("CGST").disabled = true;
                    document.getElementById("SGST").disabled = true;
                    document.getElementById("IGST").disabled = true;
                    document.getElementById("txtGST").disabled = true;
                    $scope.Invoice.tax1 = data.Tax1;
                    $scope.Invoice.tax2 = data.Tax2;
                    $scope.Invoice.tax3 = data.Tax3;
                    $scope.Invoice.CSTCharges = data.CSTCharge;
                    $scope.Invoice.VATCharges = data.VATCharge;
                }
                $scope.Invoice.transcharge = data.TransCharge;
               // $scope.Invoice.GST = data.GST;
                $scope.Invoice.customername = data.custCompany;

          
                $scope.Invoice.customeraddress = data.custAddress;
                $scope.Invoice.InCurrency = data.currSymbol;
                $scope.Invoice.currencyId = data.currID;
                $scope.Invoice.CodeId = data.CodeId;
                var date = data.InvoiceDate.toString().split('/');
                if (date.length > 0) {
                    var sdate = date[1].toString() + "/" + date[0].toString() + "/" + date[2].toString();
                }
                $scope.Invoice.invoicedate = sdate;
                var duedt = data.DueDate.toString().split('/');
                var duedtnew = "";
                if (duedt != "") {
                    if (duedt.length > 0) {
                        duedtnew = duedt[1].toString() + "/" + duedt[0].toString() + "/" + duedt[2].toString();
                    }
                }
                $scope.Invoice.duedate = duedtnew;
                $scope.Invoice.InvoiceNo = data.InvoiceNo;
                $scope.CheckInvoiceNo = data.InvoiceNo;
                $scope.Invoice.grandTotal = data.TotalAmount;
                $scope.Invoice.ExRate = data.ExRate;
                $scope.Invoice.myData = data.myData;
                $scope.Invoice.Comment = data.Comment;
                $scope.Invoice.TDSCheck = data.TDSCheck;
                if (data.TDSCheck == 1){
                    $('#TDSCheck').prop('checked', true);
                } else {
                    $('#TDSCheck').prop('checked', false);
                }
                var total = 0;
                if ($scope.Invoice.myData.length > 0) {
                    for (count = 0; count < $scope.Invoice.myData.length; count++) {
                        total += $scope.Invoice.myData[count].Amount * $scope.Invoice.myData[count].Quantity; //total += $scope.Invoice.myData[count].Rate * $scope.Invoice.myData[count].Quantity; //commented by elumalai
                    }
                    $scope.Invoice.totalPrice = total;
                }
                else { $scope.Invoice.totalPrice = 0; }

                var todayDate = kendo.toString((new Date(sdate)), 'dd/MM/yyyy');
                $("#txtInvoiceDate").data("kendoDatePicker").value(todayDate);

                $scope.caltax1(); // added by Elumalai


                $('#GridInvoiceStatus').css('display', 'none');  //   <--- invoice payment status

                $('#lblStatus').css('display', '');


                //getProjects();

                BindInvoiceStatus(pInvId);

            },
            error: function (data, status, headers, config) {
                alert("Some error occured3.");
            }
            ,
            complete: function () {
                //alert($('[id$="lblCustName"]').val());
            }
        });
        return true;
    };

    $scope.StoreBalc = function (row) {
        $scope.prevBalance = row.entity.BalanceAmount;
    }

    $scope.DisableRow = function (row) {
        //alert('1');
        if (row.entity.ProjectMilestoneID > 0) {
            return true;
        }
        return false;

    }

    $scope.DisableAmountRow = function (row) {
        //alert('1');
        if (row.entity.ProjectMilestoneID <= 0) {
            return true;
        }
        return false;

    }

    $scope.calculate = function calculate(row) {
        // -----------added by elumalai

        if (row.entity.ProjectMilestoneID <= 0) {
            row.entity.CalBalance = row.entity.Rate;
            row.entity.Amount = parseFloat(row.entity.Quantity) * parseFloat(row.entity.Rate);

        }
        else {
            var calbalance = row.entity.CalBalance
            if ($scope.isInvoiceEdit == 1)
                calbalance = row.entity.Rate;

            var rate = row.entity.Amount * row.entity.Quantity;

            if (calbalance - rate < 0) {
                row.entity.BalanceAmount = $scope.prevBalance;
                row.entity.Amount = 0;
            }
            else {
                row.entity.BalanceAmount = calbalance - rate;
            }
        }
        //-------------------- end by elmalai
        totalCalculatePrice();
    }

    var totalCalculatePrice = function () {
        var total = 0;
        for (count = 0; count < $scope.Invoice.myData.length; count++) {
            if ($scope.Invoice.myData[count].ProjectMilestoneID <= 0)
                total += $scope.Invoice.myData[count].Rate * $scope.Invoice.myData[count].Quantity;  //total += $scope.Invoice.myData[count].Rate * $scope.Invoice.myData[count].Quantity;  //--commented by elumalai
            else
                total += $scope.Invoice.myData[count].Amount * $scope.Invoice.myData[count].Quantity;  //total += $scope.Invoice.myData[count].Rate * $scope.Invoice.myData[count].Quantity;  //--commented by elumalai
        }
        $scope.Invoice.totalPrice = total;

        var tax1val = 0.00;
        var tax2val = 0.00;
        var tax3val = 0.00;
        var transcharge = 0.00;
        var GST = 0.00;
        var cstcharge = 0.00;
        var vatcharge = 0.00;

        var CGST = 0.00;
        var SGST = 0.00;
        var IGST = 0.00;

        if ($scope.Invoice.tax1 != null) {
            tax1val = parseFloat($scope.Invoice.tax1);
        }
        if ($scope.Invoice.tax2 != null) {
            tax2val = parseFloat($scope.Invoice.tax2);
        }
        if ($scope.Invoice.tax3 != null) {
            tax3val = parseFloat($scope.Invoice.tax3);
        }
        if ($scope.Invoice.transcharge != null) {
            transcharge = parseFloat($scope.Invoice.transcharge);
        }
        if ($scope.Invoice.GST != null) {
            GST = parseFloat($scope.Invoice.GST);
        }
       
        if ($scope.Invoice.VATCharges != null) {
            vatcharge = parseFloat($scope.Invoice.VATCharges);
        }
        if ($scope.Invoice.CSTCharges != null) {
            cstcharge = parseFloat($scope.Invoice.CSTCharges);
        }

        if ($scope.Invoice.CGST != null) {
            CGST = parseFloat($scope.Invoice.CGST);
        }
        if ($scope.Invoice.SGST != null) {
            SGST = parseFloat($scope.Invoice.SGST);
        }
        if ($scope.Invoice.IGST != null) {
            IGST = parseFloat($scope.Invoice.IGST);
        }

        var cal1 = $scope.InvoiceTax1 = ($scope.Invoice.totalPrice / 100) * tax1val;
        var cal2 = $scope.InvoiceTax2 = ($scope.Invoice.totalPrice / 100) * tax2val;
        var cal3 = $scope.OtherCharge = ($scope.Invoice.totalPrice / 100) * transcharge;
        var cal4 = $scope.InvoiceTax3 = ($scope.Invoice.totalPrice / 100) * tax3val;
        var cal5 = $scope.CSTCharge = ($scope.Invoice.totalPrice / 100) * cstcharge;
        var cal6 = $scope.VATCharges = ($scope.Invoice.totalPrice / 100) * vatcharge;

        var cal7 = $scope.CGST = ($scope.Invoice.totalPrice / 100) * CGST;
        var cal8 = $scope.SGST = ($scope.Invoice.totalPrice / 100) * SGST;
        var cal9 = $scope.IGST = ($scope.Invoice.totalPrice / 100) * IGST;
        var cal10 = $scope.GST = ($scope.Invoice.totalPrice / 100) * GST;

      //  var Date = getCurrentDate();
        var currentDate = new Date();
        var mydate = new Date('2017-06-30');
        if (currentDate <= mydate)
        {

            $scope.Invoice.grandTotal = parseFloat(Math.round(cal1 + cal2 + cal3 + cal4 + cal5 + cal6 + $scope.Invoice.totalPrice));
        }
        else
        {
            $scope.Invoice.grandTotal = parseFloat(Math.round(cal7 + cal8 + cal9 + cal10 + cal3 + $scope.Invoice.totalPrice));
        }

     
        $scope.Invoice.DisplayGrandTotal = parseFloat(Math.round(($scope.Invoice.grandTotal) * 100) / 100).toFixed(2);
    }

    $scope.caltax1 = function () {
        var tax1val = 0.00;
        var tax2val = 0.00;
        var tax3val = 0.00;
        var transcharge = 0.00;
        var GST = 0.00;
        var cstcharge = 0.00;
        var vatcharge = 0.00;

        var CGST = 0.00;
        var SGST = 0.00;
        var IGST = 0.00;

        if ($scope.Invoice.tax1 != null) {
            tax1val = parseFloat($scope.Invoice.tax1);
        }
        if ($scope.Invoice.tax2 != null) {
            tax2val = parseFloat($scope.Invoice.tax2);
        }
        if ($scope.Invoice.tax3 != null) {
            tax3val = parseFloat($scope.Invoice.tax3);
        }
        if ($scope.Invoice.transcharge != null) {
            transcharge = parseFloat($scope.Invoice.transcharge);
        }
        if ($scope.Invoice.GST != null) {
            GST = parseFloat($scope.Invoice.GST);
        }
        if ($scope.Invoice.VATCharges != null) {
            vatcharge = parseFloat($scope.Invoice.VATCharges);
        }
        if ($scope.Invoice.CSTCharges != null) {
            cstcharge = parseFloat($scope.Invoice.CSTCharges);
        }

        if ($scope.Invoice.CGST != null) {
            CGST = parseFloat($scope.Invoice.CGST);
        }
        if ($scope.Invoice.SGST != null) {
            SGST = parseFloat($scope.Invoice.SGST);
        }
        if ($scope.Invoice.IGST != null) {
            IGST = parseFloat($scope.Invoice.IGST);
        }

        var cal1 = $scope.InvoiceTax1 = ($scope.Invoice.totalPrice / 100) * tax1val;
        var cal2 = $scope.InvoiceTax2 = ($scope.Invoice.totalPrice / 100) * tax2val;
        var cal3 = $scope.OtherCharge = ($scope.Invoice.totalPrice / 100) * transcharge;
        var cal4 = $scope.InvoiceTax3 = ($scope.Invoice.totalPrice / 100) * tax3val;
        var cal5 = $scope.CSTCharge = ($scope.Invoice.totalPrice / 100) * cstcharge;
        var cal6 = $scope.VATCharge = ($scope.Invoice.totalPrice / 100) * vatcharge;

        var cal7 = $scope.CGST = ($scope.Invoice.totalPrice / 100) * CGST;
        var cal8 = $scope.SGST = ($scope.Invoice.totalPrice / 100) * SGST;
        var cal9 = $scope.IGST = ($scope.Invoice.totalPrice / 100) * IGST;
        var cal10 = $scope.GST = ($scope.Invoice.totalPrice / 100) * GST;

     //   var Date = getCurrentDate();
        var currentDate = new Date();
        var mydate = new Date('2017-06-30');
        if (currentDate <= mydate)
        {

            $scope.Invoice.grandTotal = parseFloat(Math.round(cal1 + cal2 + cal3 + cal4 + cal5 + cal6 + $scope.Invoice.totalPrice));
        }
        else {
            $scope.Invoice.grandTotal = parseFloat(Math.round(cal7 + cal8 + cal9 + cal10 + cal3 + $scope.Invoice.totalPrice));
        }

  

        $scope.InvoiceTax1 = parseFloat(Math.round($scope.InvoiceTax1));

        // -----------For Display purpose upto 2 decimal
        //$scope.InvoiceTax1 = parseFloat(Math.round($scope.InvoiceTax1 * 100) / 100).toFixed(2);
        //$scope.InvoiceTax2 = parseFloat(Math.round($scope.InvoiceTax2 * 100) / 100).toFixed(2);
        //$scope.OtherCharge = parseFloat(Math.round($scope.OtherCharge * 100) / 100).toFixed(2);
        //$scope.InvoiceTax3 = parseFloat(Math.round($scope.InvoiceTax3 * 100) / 100).toFixed(2);

        //$scope.Invoice.DisplayGrandTotal = parseFloat(Math.round(($scope.Invoice.grandTotal) * 100) / 100).toFixed(2);


        $scope.InvoiceTax1 = parseFloat(Math.round($scope.InvoiceTax1));
        $scope.InvoiceTax2 = parseFloat(Math.round($scope.InvoiceTax2));
        $scope.OtherCharge = parseFloat(Math.round($scope.OtherCharge));
        $scope.VATCharge = parseFloat(Math.round($scope.VATCharge));
        $scope.CSTCharge = parseFloat(Math.round($scope.CSTCharge));
        $scope.InvoiceTax3 = parseFloat(Math.round($scope.InvoiceTax3));

        $scope.CGST = parseFloat(Math.round($scope.CGST));
        $scope.SGST = parseFloat(Math.round($scope.SGST));
        $scope.IGST = parseFloat(Math.round($scope.IGST));

        $scope.Invoice.DisplayGrandTotal = parseFloat(Math.round(($scope.Invoice.grandTotal)));
    }

    $scope.SaveInvoice = function (ngForm) {
       
        if ($scope.CheckInvoiceNo != $('#txtInvoiceNo').val()) {
            if ($scope.CheckInvoiveNo())
                ngForm.$invalid = true;
        }
        if (ngForm.$invalid) {
            $scope.invalidSubmitAttempt = true;
            return;
        }

        $scope.Invoice.invoicedate = $('[id$="txtInvoiceDate"]').val();
        $scope.Invoice.duedate = $('[id$="txtDueDate"]').val();
        $scope.Invoice.PaymentDate = $('[id$="txtPaymentDate"]').val();
        if ($('[id$="hdnProjectInvoiceId"]').val() == "")
        {

            insertInvoice();
        }
        else {
            updateInvoice();

        }
    }

    function GetInvoices() {
        $.ajax({
            type: "POST",
            url: "ProjectInvoices.aspx/BindInvoices",
            contentType: "application/json;charset=utf-8",
            data: "{}",
            dataType: "json",
            async: false,
            success: function (msg) {
                GetInvoicesData(jQuery.parseJSON(msg.d));
            },
            error: function (x, e) {

            }
        });
    }

    var updateInvoice = function () {
        var postdata =
        {
            'ProjectInvoiceID': $scope.Invoice.ProjectInvoiceID,
            'projId': $scope.Invoice.Project,
            'InvoiceDate': $scope.Invoice.invoicedate,
            'DueDate': $scope.Invoice.duedate, ////
            'currID': $scope.Invoice.currencyId,
            'custId': $scope.Invoice.custId,
            'ExRate': parseFloat($scope.Invoice.ExRate),
            'Tax1': parseFloat($scope.Invoice.tax1),
            'Tax2': parseFloat($scope.Invoice.tax2),
            'Tax3': parseFloat($scope.Invoice.tax3),
            'TransCharge': parseFloat($scope.Invoice.transcharge),
            'VATCharge': parseFloat($scope.Invoice.VATCharges),
            'CSTCharge': parseFloat($scope.Invoice.CSTCharges),
            'TotalAmount': $scope.Invoice.grandTotal,
            'Comment': $scope.Invoice.Comment,
            'insertedBy': $scope.Invoice.insertedby,
            'InvoiceNo': $scope.Invoice.InvoiceNo,
            'AddPayment': $scope.Invoice.chkAddPayment,
            'PaymentType': $scope.Invoice.PaymentType,
            'PaymentComment': $scope.Invoice.PaymentComment,
            'myData': $scope.Invoice.myData,
            'CGST': parseFloat($scope.Invoice.CGST),
            'SGST': parseFloat($scope.Invoice.SGST),
                'IGST': parseFloat($scope.Invoice.IGST),
                'GST': parseFloat($scope.Invoice.GST),
                'CodeId': $("#ctl00_ContentPlaceHolder1_ProjectSacHsnCode").val(),
                'TDSCheck': $scope.Invoice.TDSCheck == true ? 1 : 0  

        }
        var re = JSON.stringify(postdata);
        console.log(re);
        $.ajax({
            type: "POST",
            url: "ProjectInvoices.aspx/updateinvoice",
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
                $('#popupmsg').text("Invoice Updated Successfully");
                $('#popup-box').addClass('visible');
                $('html').addClass('overlay');
                alert("Invoice Updated Successfully");
                ClearFormData();
                location.reload();
            },
            error: function (x, e) {
                alert("Invoice Not updated. "
                      + x.responseText);
                location.reload();
            }
        });
    }

    var taxCharges;
    var vatCharges;
    var insertInvoice = function () {
        var postdata =
        {

            'projId': $scope.Invoice.Project,
            'InvoiceDate': $scope.Invoice.invoicedate,
            'DueDate': $scope.Invoice.duedate,
            'currID': $scope.Invoice.currencyId,
            'ExRate': parseFloat($scope.Invoice.ExRate),
            'Tax1': parseFloat($scope.Invoice.tax1),
            'Tax2': parseFloat($scope.Invoice.tax2),
            'Tax3': parseFloat($scope.Invoice.tax3),
            'TransCharge': parseFloat($scope.Invoice.transcharge),
            'VATCharge': parseFloat($scope.Invoice.VATCharges),
            'CSTCharge': parseFloat($scope.Invoice.CSTCharges),
            'TotalAmount': $scope.Invoice.grandTotal,
            'Comment': $scope.Invoice.Comment,
            'insertedBy': $scope.Invoice.insertedby,
            'InvoiceNo': $scope.Invoice.InvoiceNo,
            'Inv_LocationID': parseInt($('[id$="hdnLocationId"]').val()),
            'AddPayment': $scope.Invoice.chkAddPayment,
            'PaymentType': $scope.Invoice.PaymentType,
            'PaymentComment': $scope.Invoice.PaymentComment,
            'PaymentDate': $scope.Invoice.PaymentDate,
            'CGST': parseFloat($scope.Invoice.CGST),
            'SGST': parseFloat($scope.Invoice.SGST),
                'IGST': parseFloat($scope.Invoice.IGST),
                'GST': parseFloat($scope.Invoice.GST),
                'CodeId': $("#ctl00_ContentPlaceHolder1_ProjectSacHsnCode").val(),
                'myData': $scope.Invoice.myData,
                'TDSCheck': $scope.Invoice.TDSCheck == true ? 1 : 0  
        }
        var re = JSON.stringify(postdata);
        $scope.isProcessing = true;
        $.ajax({
            type: "POST",
            url: "ProjectInvoices.aspx/PostInvoice",
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
                $('#popupmsg').text("Invoice Saved Successfully");
                $('#popup-box').addClass('visible');
                $('html').addClass('overlay');
                alert("Invoice Saved Successfully");
                ClearFormData();
                location.reload();
            },
            error: function (x, e) {
                alert("Invoice Not saved. "
                      + x.responseText);
                location.reload();
            }
        });
    }


    var deleteInvoiceMileStone = function (ProjectInvoiceDetailID) {

        $.ajax({
            type: "POST",
            url: "ProjectInvoices.aspx/DeleteInvoiceMile",
            data: "{'JSONData':'" + ProjectInvoiceDetailID + "'}",
            contentType: "application/json;charset=utf-8",
            success: function (ProjectInvoiceDetailID) {

            },
            error: function (x, e) {

                alert("The call to the server side failed. "
                      + x.responseText);
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
                totalCalculatePrice();
            }
        }
    };


    $scope.isDisable = false;
    $scope.gridInvoice_MileStone =
        {
            data: 'Invoice.myData',
            enableCellSelection: true,
            enableCellOnFocus: false,
            enableRowSelection: false,
            tabIndex: 0,
            noTabInterference: true,
            headerRowHeight: 45, // added to set the header height
            width: 722,
            columnDefs: [
                          {
                              field: 'ProjectMilestoneID',
                              displayName: 'Milestones',
                              cellTemplate: '<select ng-model="COL_FIELD" ng-options="m.projMilestoneID as m.name for m in Invoice.Milestone " style="width:125px;height:30px;" ng-change="FillMsDetails(row)"><option value="">--Select--</option></select>',
                              //cellTemplate: '<select ng-model="COL_FIELD" ng-options="m.projMilestoneID as m.name for m in Invoice.Milestone" style="width:125px;height:30px;"><option value="">--Select--</option></select>',
                              enableCellEdit: false,
                              width: 125
                          },
                          {
                              field: 'Description',
                              displayName: 'Description',
                              //cellTemplate: '<input name="desc" type="text" ng-model="COL_FIELD" style="width:337px;height:17px;" ng-blur="addrow(row)"/>',
                              cellTemplate: '<input name="desc" type="text" ng-model="COL_FIELD" style="width:357px;height:17px;"/>',
                              enableCellEdit: false,
                              width: 355//340//280
                          },
                          {
                              field: 'OriginalAmount',
                              displayName: 'Original Amount',
                              cellTemplate: '<input type="text"  name="OriginalAmount"  ng-model="COL_FIELD" style="width:60px;height:17px;text-align:right" onkeypress="return false;" readonly="readonly"/>',
                              enableCellEdit: false,
                              visible: false,
                              width: 70
                          },
                          {
                              field: 'BalanceAmount',
                              displayName: 'Balance Amount',
                              cellTemplate: '<input type="text"  name="BalAmount"  ng-model="COL_FIELD" style="width:60px;height:17px;text-align:right" onkeypress="return false;" readonly="readonly"/>',
                              enableCellEdit: false,
                              visible: false,
                              width: 70
                          },
                          {
                              field: 'CalBalance',
                              displayName: 'CalBalance',
                              cellTemplate: '<input type="text"  name="CalBalance"  ng-model="COL_FIELD" style="width:67px;height:17px;text-align:right" onkeypress="return false;" readonly="readonly"/>',
                              enableCellEdit: false,
                              visible: false,
                              width: 70
                          },
                          {
                              field: 'Quantity',
                              displayName: 'Quantity',
                              cellTemplate: '<input type="text" ng-model="COL_FIELD" name="quantity" ng-change="calculate(row)" ng-pattern="/^[0-9]+(\.[0-9]{1,2})?$/" required style="width:44px;height:17px;text-align:right"><div class="validation-error-grid" ng-show="(invalidSubmitAttempt &&(Invoice_Form.quantity.$error.required || Invoice_Form.quantity.$error.pattern))"></div>',
                              enableCellEdit: false,
                              width: 57
                          },
                          {
                              field: 'Rate',
                              displayName: 'Rate',
                              cellTemplate: '<input name="price" id ="txtRate" type="text" ng-model="COL_FIELD" ng-click="StoreBalc(row)" ng-change="calculate(row)" ng-disabled="DisableRow(row)"; ng-pattern="/^[0-9]+(\.[0-9]{1,2})?$/"  style="width:57px;height:17px;text-align:right" maxlength =8/><span ng-show="Invoice_Form.price.$error.pattern"></span>',
                              enableCellEdit: false,
                              width: 70
                          },
                          {
                              field: 'Amount',
                              displayName: 'Amount',
                              cellTemplate: '<input type="text"  ng-model="COL_FIELD" style="width:57px;height:17px;text-align:right" ng-change="calculate(row)" ng-disabled="DisableAmountRow(row)"; ng-pattern="/^[0-9]+(\.[0-9]{1,2})?$/" required />',
                              enableCellEdit: false,
                              width: 70
                          },
                          {
                              field: 'delete',
                              displayName: '',
                              cellTemplate: removeTemplate,
                              width: 24

                          },
                          {
                              field: 'add',
                              displayName: '',
                              cellTemplate: addTemplate,
                              width: 22

                          }
            ]
        };

    $('#txtInvoiceNo').bind("focusout", function () {

        if ($scope.CheckInvoiceNo == $('#txtInvoiceNo').val())
            return true;
        if ($('#txtInvoiceNo').val() == '')
            return false;

        $scope.CheckInvoiveNo();
        //$scope.Invoice.InvoiceNo = "";
        //$('#txtInvoiceNo').val('');
    });

    $scope.ClosePopUp = function () {
        $scope.invalidSubmitAttempt = false;
        $('#divAddPopup').css('display', 'none');
        $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
        ClearFormData();
        location.reload();

    };

    $scope.OpenPopUp = function () {

        $('[id$="hdnProjectInvoiceId"]').val('');

        if ($('[id$="hdnProjID"]').val() == "0" || $('[id$="hdnLocationId"]').val() == "0") {
            alert("Please select both Project and Location");
            return;
        }
        else {
           
            var pid = $('[id$="hdnProjID"]').val();
            var lid = $('[id$="hdnLocationId"]').val();
            GetLocationDetails(pid, lid);
            var CustState = $('[id$="hdnProjectCustStateId"]').val();
            var ClientState = $('[id$="hdnProjectClientStateId"]').val();
            var CustCountry = $('[id$="hdnProjectCustCountry"]').val();  //Added By Nikhil Shetye on 18-10-2017 for checking cust country
            var todayDate = getCurrentDate();
            var currentDate = new Date();
            var mydate = new Date('2017-06-30');
            if (currentDate > mydate)
            {
                if ((CustState == ClientState) && (CustCountry == "1")) //Added By Nikhil Shetye on 18-10-2017 for checking cust country
                {
                    //document.getElementById("IGST").disabled = true; // commented by Trupti
                    document.getElementById("txtGST").disabled = true;
                    //Added By Nikhil Shetye for Setting CGST & SGST values on 16-10-2017
                    //$scope.Invoice.CGST = 9;
                    //$scope.Invoice.SGST = 9;
                }
                else
                {
                    document.getElementById("CGST").disabled = true;
                    document.getElementById("SGST").disabled = true;
                    //Added By Nikhil Shetye for Setting IGST values on 16-10-2017
                    //$scope.Invoice.IGST = 18;
                }

                if (CustCountry == "3") {
                    document.getElementById("IGST").disabled = true;
                }
                document.getElementById("txtTax1").disabled = true;
                document.getElementById("tax2").disabled = true;
                document.getElementById("tax3").disabled = true;
                document.getElementById("txtVATCharges").disabled = true;
                document.getElementById("txtCSTCharges").disabled = true;


            }
            else
            {
                document.getElementById("CGST").disabled = true;
                document.getElementById("SGST").disabled = true;
                document.getElementById("IGST").disabled = true;
                document.getElementById("txtGST").disabled = true;

            }


            // $('[id$="btnAddMile"]').hide();        
            $('#GridInvoiceStatus').css('display', 'none');
            $('#lblStatus').css('display', 'none');

            $('#divAddPopup').css('display', '');
            $('#divAddPopupOverlay').addClass('k-overlay');

            $('[id$="txtInvoiceDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
            $('[id$="txtDueDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
            $('[id$="txtPaymentDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
            //added by Dipti on 6/4/2015
            var todayDate = getCurrentDate();
            $("#txtInvoiceDate").data("kendoDatePicker").value(todayDate);
            $("#txtDueDate").data("kendoDatePicker").value(todayDate);
            $("#txtPaymentDate").data("kendoDatePicker").value(todayDate);
            $scope.Invoice.invoicedate = todayDate;
            $scope.Invoice.duedate = todayDate;
            $scope.Invoice.PaymentDate = todayDate;
            //end added by Dipti on 6/4/2015

            //getProjects();
            $('[id$="lblProject"]').html(ProjectName);
            $scope.pmile();
        }
        // BindLocationKeyword();
    };

    function GetLocationDetails(pid, lid) {
        $.ajax({
            type: "POST",
            url: "ProformaInvoices.aspx/GetLocationDetails",
            contentType: "application/json;charset=utf-8",
            data: "{projID:'" + pid + "',locationId:'" + lid + "'}",
            dataType: "json",
            async: false,
            success: function (msg) {
                var JSONData = jQuery.parseJSON(msg.d);
                var ClientStateId = JSONData[0].ClientStateId;
                var CustStateId = JSONData[0].CustStateId;
                var CustCountry = JSONData[0].CustCountry; //Added By Nikhil Shetye on 18-10-2017 for checking cust country
                $('[id$="hdnProjectCustStateId"]').val(CustStateId);
                $('[id$="hdnProjectClientStateId"]').val(ClientStateId);
                $('[id$="hdnProjectCustCountry"]').val(CustCountry); //Added By Nikhil Shetye on 18-10-2017 for checking cust country
            },
            error: function (x, e) {
                alert("The call to the server side failed. "
                      + x.responseText);
            }
        });
    }

    function ClearFormData() {
        $scope.Invoice.Milestone = [];
        $scope.Invoice.customername = "";
        $scope.Invoice.customeraddress = "";
        $scope.Invoice.InCurrency = "";
        $scope.Invoice.currencyId = "";
        $scope.Invoice.ExRate = "";
        $scope.Invoice.Comment = "";
        $scope.Invoice.tax1 = 0;
        $scope.Invoice.tax2 = 0;
        $scope.Invoice.tax3 = 0;
        $scope.Invoice.transcharge = 0;
        $scope.Invoice.GST = 0;
        //  $scope.Invoice.insertedby = "1000";
        $scope.Invoice.totalPrice = "";
        $scope.Invoice.grandTotal = "";
        $scope.Invoice.invoicedate = "";
        $scope.Invoice.duedate = "";
        $scope.Invoice.myData = [];
        $scope.Invoice.Project = "";
        $('[id$="hdnProjectInvoiceId"]').val('');
        $scope.Invoice.InvoiceNo = "";
        $('[id$="spnDuplicateInvNo"]').text('');
        $('[id$="spnDuplicateInvNo"]').css('display', 'none');
        $('[id$="lblProject"]').val('');
        $scope.Invoice.PaymentComment = "";

        $scope.InvoiceTax1 = 0;
        $scope.InvoiceTax2 = 0;
        $scope.InvoiceTax3 = 0;
        $scope.OtherCharge = 0;
        $scope.CSTCharge = 0;
        $scope.VATCharge = 0;
        $scope.CGST = 0;
        $scope.SGST = 0;
        $scope.IGST = 0;
    }

    function getCurrentDate() {
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!
        var yyyy = today.getFullYear();

        if (dd < 10) {
            dd = '0' + dd
        }

        if (mm < 10) {
            mm = '0' + mm
        }

        today = dd + '/' + mm + '/' + yyyy;
        return today;
    }

    //added by dipti
    var getmiledesc = function (milestoneId, row) {
        var postdata =
       {
           'projID': $scope.Invoice.Project,
           'projMilestoneID': milestoneId,
       }
        var re = JSON.stringify(postdata);
        $scope.isProcessing = true;

        var milestonedesc = "";
        if (milestoneId > 0) {
            $.ajax({
                type: "POST",
                url: "ProjectInvoices.aspx/GetMileStoneDetails",
                data: "{'JSONData':'" + re + "'}",
                async: false,
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    data = data.d;
                    //if ($scope.Invoice.myData[row.rowIndex].Description == "" && $scope.Invoice.myData[row.rowIndex].Amount == 0 && $scope.Invoice.myData[row.rowIndex].Rate == "" && $scope.Invoice.myData[row.rowIndex].Quantity == "") {
                    $scope.Invoice.myData[row.rowIndex].Description = data[0].MileDescription;
                    $scope.Invoice.myData[row.rowIndex].OriginalAmount = data[0].OriginalAmount;
                    $scope.Invoice.myData[row.rowIndex].BalanceAmount = data[0].BalanceAmount;
                    $scope.Invoice.myData[row.rowIndex].Amount = data[0].BalanceAmount;
                    $scope.Invoice.myData[row.rowIndex].Rate = data[0].OriginalAmount; //data[0].BalanceAmount;
                    $scope.Invoice.myData[row.rowIndex].Quantity = 1;
                    $scope.Invoice.myData[row.rowIndex].CalBalance = data[0].BalanceAmount;
                    //$scope.Invoice.myData[row.rowIndex].ProjectMilestoneID = milestoneId;
                    //$scope.calculate(row);
                    // row.entity.Amount = row.entity.Rate * row.entity.Quantity;    //----------commented by Elumalai
                    totalCalculatePrice();
                    //$scope.edit = true;


                    // $("#txtRate").prop('disabled', true); // added by elumalai
                    // }
                },
                error: function (x, e) {
                    alert("some error occured in milestone "
                          + x.responseText);
                }
            });
        }
        else {
        }

    };

    //--------- 
    $scope.redirectToMilestone = function () {
        var projId = $scope.Invoice.Project;
        //var projectName = customRowDataItem.projName;
        var currExRate = parseFloat($scope.Invoice.ExRate)
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ProjectInvoices.aspx/SetProjIdForMilestone",
            data: "{'projid':'" + projId + "','currExRate':'" + currExRate + "'}",
            dataType: "json",
            async: false,
            success: function (msg) {

            },
            error: function (x, e) {
                alert("The call to the server side failed. "
                      + x.responseText);
            }
        });
        window.location.assign("Milestone.aspx");

    }

    $scope.CheckInvoiveNo = function () {
        //$scope.existsflag = false;
        var existsflag = false;
        $scope.invalidSubmitAttempt = true;
        var InvoiceNo = $('[id$="txtInvoiceNo"]').val();
        var ProjectInvoiceID = $('[id$="hdnProjectInvoiceId"]').val() == "" ? 0 : $('[id$="hdnProjectInvoiceId"]').val();
        var ProjID = $('[id$="hdnProjID"]').val();
        var LocationID = $('[id$="hdnLocationId"]').val();

        //$scope.Invoice.InvoiceNo = "";
        //var postdata =
        //{
        //    'ProjectInvoiceID': $('[id$="hdnProjectInvoiceId"]').val() == "" ? 0 : $('[id$="hdnProjectInvoiceId"]').val(),//$scope.Invoice.ProjectInvoiceID,
        //    'InvoiceNo': InvoiceNo,
        //}
        //var re = JSON.stringify(postdata);


        $scope.isProcessing = true;
        $.ajax({
            type: "POST",
            url: "ProjectInvoices.aspx/IfExistsInvoiceNo",
            data: "{'InvoiceNo':'" + InvoiceNo + "','ProjectInvoiceID':'" + parseInt(ProjectInvoiceID) + "','ProjID':'" + parseInt(ProjID) + "','LocationID':'" + parseInt(LocationID) + "' }",
            async: false,
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                if (data.d) {
                    $scope.invalidSubmitAttempt = true;
                    //$scope.existsflag = true;
                    existsflag = true;
                    var msg = "Invoice number " + InvoiceNo + " already exists!";
                    $('[id$="spnDuplicateInvNo"]').text(msg);
                    $('[id$="spnDuplicateInvNo"]').css('display', 'block');
                    $scope.Invoice.InvoiceNo = "";
                    $('#txtInvoiceNo').val('');
                }
                else {
                    $scope.invalidSubmitAttempt = false;
                    //$scope.existsflag = false;
                    existsflag = false;
                    $('[id$="spnDuplicateInvNo"]').css('display', 'none');
                }

            },
            error: function (x, e) {
                alert("some error occured4"
                      + x.responseText);
            }
        });
        return existsflag;
    };
    //end

    $scope.VoidInvoice = function () {
        if (!confirm("Cancelling this invoice will permanently void the current invoice.Please confirm if you want to proceed with cancellation of the invoice?"))
            return;

        $.ajax({
            type: "POST",
            url: "ProjectInvoices.aspx/VoidInvoice",
            contentType: "application/json;charset=utf-8",
            data: "{'pInvId':" + $scope.Invoice.ProjectInvoiceID + "}",
            dataType: "json",
            async: false,
            success: function (data) {
                alert('Updated Successfully.');
                $scope.ClosePopUp();
                location.reload();
            },
            error: function (x, e) {
                alert("The call to the server side failed. "
                      + x.responseText);
            }
        });
    }

    // ------------- for add payment section
    $scope.IsVisible = false;
    $scope.ShowHide = function () {
        $scope.IsVisible = $scope.Invoice.chkAddPayment;
    }


    //$scope.ChangeDate = function ()
    //{
    //    //alert(2);
    //    var dt = $("#txtInvoiceDate").val();
    //    $("#txtPaymentDate").data("kendoDatePicker").value(dt);
    //}
});






