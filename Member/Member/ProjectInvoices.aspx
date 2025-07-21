<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="ProjectInvoices.aspx.cs" Inherits="Member_Invoices" EnableEventValidation="false" ValidateRequest="false" %>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--Bellow links are for ng-grid --%>

    <link type="text/css" href="css/InvoiceValidation.css" rel="stylesheet" />

    <%--<link data-require="ng-grid@2.0.14" data-semver="2.0.14" rel="stylesheet" href="//cdn.rawgit.com/angular-ui/ng-grid/v2.0.14/ng-grid.css" />--%>
    <%--<script data-require="jquery@2.0.3" data-semver="2.0.3" src="http://code.jquery.com/jquery-2.0.3.min.js"></script>--%>

    <%--commented bcoz of dropdown of ng-grid not working in chrome--%>
    <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.0.2/angular.min.js"></script>--%>

    <link href="css/ng-grid.css" rel="stylesheet" />


    <%--Bellow links are for kendo controls (do not change sequence)--%>

    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../Member/js/kendo.all.min.js"></script>
    <%-- angular js --%>
    <script type="text/javascript" src="../Member/Kendu/js/kendo.upload.min.js"></script>


    <script type="text/javascript" src="js/1.3.7_angular.js"></script>
    <script type="text/javascript" src="js/ng-grid.js"></script>
    <script type="text/javascript" src="js/ng-grid-flexible-height.js"></script>

    <script type="text/javascript" src="../js/console.js"></script>
    <script src="../Member/js/InvoiceMilestones.js" type="text/javascript"></script>
    <script src="../Member/js/NewInvoices.js?26" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">

        var GridInvoices = "#gridInvoices";
        $("#chkPaidInvoices").change(function () {
            if ($(this).is(':checked')) {
                var grid = $("#gridInvoices").data("kendoGrid");
                grid.dataSource.read();
            }
        });

    </script>
    <%-- <script type="text/javascript">

        $(document).ready(function () {
           // $(document).on('click', '.DivShowEditor', function (e) { //  $('.DivShowEditor').click(function () {
                var NextDiv = $(this).next();
                var TxtObj = $(this).parent().find('textarea');
                var Text = TxtObj.val();
                TxtObj.kendoEditor();
                $(this).hide();
                NextDiv.show();
           // });
           
        });

    </script>--%>
    <style type="text/css">
        .k-grid tbody .k-button, .k-ie8 .k-grid tbody button.k-button {
            min-width: 40px;
            min-height: 30px;
        }

        .ViewPDF, .ViewPDF:hover {
            background-image: url('../images/icon-pdf.gif');
            min-width: 10px;
            width: 20px;
            height: 30px;
            background-size: 20px;
            background-repeat: no-repeat;
        }

        .mail, .mail:hover {
            background-image: url('images/send.png');
            min-width: 10px;
            width: 20px;
            height: 30px;
            background-size: 20px;
            background-repeat: no-repeat;
        }

        .Reminder, .Reminder:hover {
            background-image: url('images/Reminder.png');
            min-width: 10px;
            width: 20px;
            height: 30px;
            background-size: 20px;
            background-repeat: no-repeat;
        }
        /*headers*/
        .k-grid th.k-header,
        .k-grid-header {
            background: #252e34;
        }

            .k-grid th.k-header,
            .k-grid th.k-header .k-link {
                color: white;
            }
        /*rows*/
        .k-grid-content > table > tbody > tr {
            background: #cbc8c8;
            padding: 0 !important;
            margin: 0 !important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }

        .k-grid-content > table > tbody > .k-alt {
            background: #eceaea;
            padding: 0 !important;
            margin: 0 !important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }

        /*selection*/

        .k-grid table tr.k-state-selected {
            background: #f99;
            color: #fff;
        }
        /*for history grid [s]*/
        .k-alt {
            background: #cbc8c8;
            padding: 0 !important;
            margin: 0 !important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }
        /*for history grid [e]*/


        /*ng-grid*/
        .gridStyle {
            border: 1px solid rgb(212,212,212);
            /*width: 770px;*/
            width: 900px;
            height: 150px;
        }

        .buttondel {
            background-image: url('images/delete.png');
        }

        .buttonadd {
            background: url('images/addbtn_small.jpg') center center no-repeat;
        }

        /*input.ng-dirty.ng-invalid {
            border: 1px solid red;
            box-shadow: 0 0 10px red;
        }*/

        /*input.ng-invalid-required {
            border: 1px solid red;
            box-shadow: 0 0 10px red;
        }*/

        input.ng-invalid-pattern {
            border: 1px solid red;
            box-shadow: 0 0 10px red;
        }
        /*added by Dipti to wrap column header*/
        .ngHeaderText {
            text-overflow: clip;
            white-space: normal;
        }
        /*end added by Dipti to wrap column header*/
        /*end added by Dipti to wrap table column */
        .thclass {
            width: 100px;
        }

        .tdclass {
            width: 180px;
        }

        .manage_form th {
            text-align: right;
        }
        /*end added by Dipti to wrap table column */
    </style>

    <%--  div confirm style --%>
    <style type="text/css">
        .popup {
            background: none repeat scroll 0 0 #fff;
            box-shadow: 0 0 5px #3d3d4f;
            position: fixed;
            z-index: 9999;
            display: none;
            width: 550px;
            top: 20%;
            left: 50%;
            margin-left: -185px;
            box-shadow: 0 7px 20px rgba(0, 0, 0, 0.45);
            font-family: Arial, Helvetica, sans-serif;
        }

        .overlay {
            position: fixed;
            background-color: rgba(0, 0, 0,0.5 );
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            z-index: 8888;
            display: none;
        }

        .popupCont {
            background-color: #f0f0f0;
            border-top: 1px solid #CCC;
        }

        .popup_head {
            padding: 0 15px;
        }

            .popup_head h2 {
                font-size: 40px;
                padding: 30px 15px;
            }

        .close {
            background: #2b2d2d;
            color: #fff;
            display: block;
            float: right;
            padding: 9px 12px;
            text-align: center;
        }

        .loginfoot {
            border-top: 1px solid #ccc; /*padding:20px 16px;*/
            text-align: center;
            margin-top: 35px;
            padding: 25px 0;
            background: #fff;
            position: relative;
        }

        .or {
            margin-left: -24.5px;
            left: 50%;
            position: absolute;
            top: -24.5px;
        }

        .popupCont .row {
            overflow: visible;
        }

            .popupCont .row .col1 {
                float: none;
            }

        .border {
            border-top: 1px solid #ccc;
            padding: 10px 0;
            background: #fff;
            margin: 0 !important;
        }

        .popupCont .gray {
            padding: 12px 44px;
            behavior: url(../pie/PIE.htc);
            -webkit-border-radius: 5px;
            border-radius: 5px;
            -moz-border-radius: 5px;
            position: relative;
        }

            .popupCont .gray:hover {
                background: #7f7d7d;
            }



        /*-- ========================== subscription popup ===============================================-- */
        /*model_popup*/
        .model_popup {
            background: #fff;
            border-radius: 5px 5px 0 0;
            overflow: hidden;
            border-radius: 5px 5px 0 0;
            position: relative;
            behavior: url(../pie/PIE.htc);
            -webkit-border-radius: 5px 5px 0 0;
            -moz-border-radius: 5px 5px 0 0;
            margin-bottom: 5%;
            left: 43%;
            position: absolute;
        }

        .sub_popup .left_panel {
            min-height: 630px;
        }

        .pricing th, .subp_cont th {
            position: relative;
        }

        .subp_cont .price {
            margin-top: 0;
        }

        .closePopup_btn {
            color: #fff;
            text-decoration: none;
            display: block;
            float: right;
            font-family: Arial, Helvetica, sans-serif;
            position: absolute;
            right: 0;
            top: 0;
            padding: 5px 10px;
            background: rgba(0,0,0,0.5);
        }

        /*-------------------------------start css here------------------------------------------*/
        .box_SC {
            border-top: 3px solid #e69503;
            font-weight: 600;
            padding: 22px;
            margin: 30px 0 0 0;
            overflow: hidden;
            text-align: center;
        }

        .window_content {
            padding: 55px 30px 20px;
            font-weight: 300;
            font-size: 1.2em;
        }

        .remind_later {
            padding: 20px 30px;
            border-top: #d4d4d4 1px solid;
            text-align: right;
        }

        .selectS1 {
            height: 30px;
            line-height: 30px;
            padding: 5px;
            width: 100px;
        }

        .btn.close {
            color: whitesmoke;
            background: #252e34;
            text-align: center;
            width: 100%;
            transition: all .3s ease;
            font-size: 24px;
            font-weight: 500;
            padding: 10px 0px;
            border-radius: 3px;
            cursor: pointer;
            transition: all .3s ease;
            z-index: 5;
            margin-bottom: 3px;
        }

            .btn.close:hover {
                margin-top: 3px;
                margin-bottom: 0;
            }

            .btn.close:active {
                margin-top: 8px;
                margin-bottom: 0;
            }

        .submit_btn {
            background: #2f3840 !important;
            box-shadow: 0 8px 0 0 #000000;
        }

        .cancel_btn {
            background: #2f3840 !important;
            box-shadow: 0 8px 0 0 #000000;
        }

        .submit_btn:hover {
            box-shadow: 0 5px 0 0 #518324;
            background: #252e34 !important;
        }

        .cancel_btn:hover {
            box-shadow: 0 5px 0 0 #cb3939;
            background: #252e34 !important;
        }

        .col2 {
            padding: 15px 20px;
            width: 42%;
            float: left;
        }

        div.k-windowAdd.mailbox {
            max-width: 610px;
            left: 50% !important;
            margin-left: -305px !important;
            height: auto;
        }
    </style>
    <%-- div confirm style end --%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div id="grdInv" ng-app="myApp" ng-controller="Invoice_milestoneCtrl">
        <div class="content_wrap">
            <div class="gride_table">
                <div class="box_border">
                    <div class="grid_head">
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblProjectsModule" Text="Invoices" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <div style="float: right;">
                                        <%--<span id="spn" ng-click="OpenPopUp()" class="small_button white_button open">Add Invoices</span>--%>
                                        <input type="button" ng-click="OpenPopUp()" class="small_button white_button" value="Add Invoices" id="btnAddInvoices" runat="server" />

                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="gridInvoices"></div>
                </div>
            </div>
        </div>

        <%--  PopUP Div Starts --%>

        <div id="divAddPopupOverlay" runat="server"></div>

        <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 740px; min-height: 50px; top: 1%; left: 350px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">

            <div ng-form="Invoice_Form">

                <div class="popup_head">
                    <table width="100%">
                        <tr>
                            <td colspan="2" align="center">
                                <span id="span2" style="font-size: large; font-weight: 100">Tax Invoice</span>
                                <img src="Images/delete_ic.png" class="close-button" alt="Close" ng-click="ClosePopUp()" />
                            </td>
                        </tr>
                    </table>
                    <div class="clear">
                    </div>
                </div>

                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                    <tr>
                        <th>Projects:   
                        </th>
                        <td width="200px">
                            <input type="hidden" id="hdnProjID" runat="server" />
                            <%-- <select id="ddlProject" name="ddlProject" ng-model="Invoice.Project" ng-options="p.Value as p.Key for p in Invoice.Projects" ng-change="pmile()" required>
                                <option value="">--Select--</option>
                            </select>
                            <span class="validation-error" ng-show="invalidSubmitAttempt && Invoice_Form.ddlProject.$error.required">*  ng-click="ChangeDate()"
                            </span>--%>
                            <label id="lblProject" title="" style="width: 250px" runat="server"></label>
                        </td>
                        <th>Invoice Date: 
                        </th>
                        <td>
                            <input type="text" id="txtInvoiceDate" onkeypress="return false;" name="txtInvoiceDate" onchange="ChangeDate()" style="width: 240px" ng-model="Invoice.invoicedate" required />
                            <span class="validation-error" ng-show="(invalidSubmitAttempt && Invoice_Form.txtInvoiceDate.$error.required)">*
                            </span>

                        </td>
                    </tr>
                    <tr>
                        <th>Customer Name: 
                        </th>
                        <td>
                            <label id="lblCustName" title="" ng-model="Invoice.customername" style="width: 250px" runat="server">{{Invoice.customername}}</label>
                        </td>
                        <th>Due Date: 
                        </th>
                        <td>
                            <input id="txtDueDate" onkeyup="dateInput(this)" onkeypress="return false;" name="txtDueDate" style="width: 240px" ng-model="Invoice.duedate" required />
                            <span class="validation-error" ng-show="(invalidSubmitAttempt && Invoice_Form.txtDueDate.$error.required)">*
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <th rowspan="3" style="vertical-align: middle;">Customer Address: 
                        </th>
                        <td rowspan="3">
                            <label id="lblCustAddress" title="" ng-model="Invoice.customeraddress" style="width: 200px; text-align: justify" runat="server">{{Invoice.customeraddress}}</label>
                        </td>
                        <th>Currency:  
                        </th>
                        <td>
                            <label id="lblCurrency" title="" ng-model="Invoice.InCurrency" style="width: auto" runat="server">{{Invoice.InCurrency}}</label>
                            <input type="hidden" id="hdncurrid" ng-model="Invoice.currencyId" class="hdnval" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <th>Exchange Rate:  
                        </th>
                        <td>
                            <input type="text" id="ExRate" name="ExRate" ng-model="Invoice.ExRate" style="width: 150px" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" />
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&( Invoice_Form.ExRate.$error.required || Invoice_Form.ExRate.$error.pattern)) ">* 
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <th>Invoice Number:  
                        </th>
                        <td>
                            <input type="text" id="txtInvoiceNo" name="txtInvoiceNo" ng-model="Invoice.InvoiceNo" style="width: 150px;" required />
                            <%--disabled="disabled"--%>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt && Invoice_Form.txtInvoiceNo.$error.required) ">* 
                            </span>
                            <span class="validation-error" style="display: none; color: Red; font-weight: normal !important" id="spnDuplicateInvNo"></span>

                        </td>
                    </tr>
                    <tr>
                        <th>Status:  
                        </th>
                        <td colspan="1">
                            <h1><b style="color: red">
                                <label id="lblStatus"></label>
                            </b></h1>
                        </td>
                        <th>SAC/HSN Code:</th>
                        <td>
                            <asp:DropDownList ID="ProjectSacHsnCode" runat="server" Width="200px"></asp:DropDownList>

                        </td>

                    </tr>
                    <tr>
                        <td colspan="4">
                            <input type="button" id="AddMilestone" ng-click="redirectToMilestone()" value="Add Milestone" style="width: 100px;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div>
                                <div id="ngGridMilestone" class="gridStyle" ng-grid="gridInvoice_MileStone" style="width: 725px;">
                                </div>
                            </div>
                        </td>
                    </tr>
                    <%-- <tr>
                        <td colspan="4" style="text-align: right;">
                            <input type="button" value="Add" ng-click="addrow(row)" text="Add" style="width: 35px; height: 23px; display: none;" id="btnAddMile" /> onchange="checkInput('num1');"
                        </td>
                    </tr>--%>
                </table>
                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                    <tr>
                        <th width="40%" style="text-align: left;">
                            <label id="lblAddPayment" ng-show="IsVisiblePayment" style="padding-left: 4px;">Add Payment :</label>
                            <input type="checkbox" id="chkAddPayment" ng-checked="false" ng-show="IsVisiblePayment" name="chkAddPayment" ng-click="ShowHide()" style="width: 30px; text-align: left;" ng-model="Invoice.chkAddPayment" ng-init="Invoice.chkAddPayment=0" />
                        </th>
                        <th width="20%" class="thclass1">Sub Total:
                        </th>
                        <td width="40%" class="tdclass" style="text-align: right; font-weight: bold;">{{Invoice.totalPrice }}
                        </td>
                    </tr>
                    <tr>
                        <th style="text-align: left">
                            <label id="lblPaymentdate" ng-show="IsVisible">Payment Date :</label><div ng-show="IsVisible">
                                <input type="text" id="txtPaymentDate" onkeypress="return false;" name="txtPaymentDate" style="width: 177px; float: right; display: block; bottom: 17px;" ng-model="Invoice.PaymentDate" />
                            </div>
                            <%--<span class="validation-error" ng-show="(invalidSubmitAttempt && Invoice_Form.txtPaymentDate.$error.required)">*
                            </span>--%>                            
                        </th>
                        <th class="thclass1">Tax 1:
                        </th>
                        <td class="tdclass">
                            <input type="text" id="txtTax1" name="txtTax1" style="width: 30px; text-align: right;" ng-model="Invoice.tax1" ng-change="caltax1()" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" ng-init="Invoice.tax1=0" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&(Invoice_Form.txtTax1.$error.required || Invoice_Form.txtTax1.$error.pattern))">* </span>

                            <div style="float: right">
                                <label style="text-align: right; font-weight: bold;">{{InvoiceTax1 }}</label>
                            </div>
                        </td>

                    </tr>
                    <tr>
                        <th style="text-align: left">
                            <label id="lblPaymentType" ng-show="IsVisible">Payment Type : </label>
                            <select id="ddlPaymentType" name="ddlPaymentType" ng-show="IsVisible" ng-model="Invoice.PaymentType" ng-options="p.Value as p.Key for p in Invoice.PaymentTypes" required>
                            </select>
                        </th>
                        <th>Tax 2: 
                        </th>
                        <td>
                            <input type="text" id="tax2" name="tax2" style="width: 30px; text-align: right;" ng-model="Invoice.tax2" ng-change="caltax1()" ng-init="Invoice.tax2=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&( Invoice_Form.tax2.$error.required || Invoice_Form.tax2.$error.pattern))">* </span>

                            <div style="float: right">
                                <label style="text-align: right; font-weight: bold;">{{InvoiceTax2 }}</label>
                            </div>
                        </td>

                    </tr>

                    <%--Added TAX 3--%>
                    <tr>
                        <th width="40%">
                            <label id="lblPayComment" ng-show="IsVisible" style="padding-left: 25px;">Comment :    </label>
                            <textarea id="Textarea1" runat="server" ng-show="IsVisible" class="k-textbox" name="txtComment" ng-model="Invoice.PaymentComment" style="width: 64%; height: 50px;"></textarea>
                        </th>

                        <th class="thclass1">Tax 3:
                        </th>

                        <td class="thclass1">
                            <input type="text" id="tax3" name="tax3" style="width: 30px; text-align: right;" ng-model="Invoice.tax3" ng-change="caltax1()" ng-init="Invoice.tax3=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&( Invoice_Form.tax3.$error.required || Invoice_Form.tax3.$error.pattern))">* </span>

                            <div style="float: right">
                                <label style="text-align: right; font-weight: bold;">{{InvoiceTax3 }}</label>
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <th width="40%" style="text-align: left;"></th>

                        <th class="thclass1">CGST:
                        </th>
                        <td class="tdclass">
                            <input type="text" id="CGST" name="CGST" style="width: 30px; text-align: right;" ng-model="Invoice.CGST" ng-change="caltax1()" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" ng-init="Invoice.CGST=0" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&(Invoice_Form.CGST.$error.required || Invoice_Form.CGST.$error.pattern))">* </span>

                            <div style="float: right">
                                <label style="text-align: right; font-weight: bold;">{{CGST }}</label>
                            </div>
                        </td>

                    </tr>
                    <tr>
                        <th width="40%" style="text-align: left;"></th>

                        <th>SGST: 
                        </th>
                        <td>
                            <input type="text" id="SGST" name="SGST" style="width: 30px; text-align: right;" ng-model="Invoice.SGST" ng-change="caltax1()" ng-init="Invoice.SGST=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&( Invoice_Form.SGST.$error.required || Invoice_Form.SGST.$error.pattern))">* </span>

                            <div style="float: right">
                                <label style="text-align: right; font-weight: bold;">{{SGST }}</label>
                            </div>
                        </td>

                    </tr>

                    <tr>
                        <th width="40%"></th>

                        <th>IGST:
                        </th>

                        <td>
                            <input type="text" id="IGST" name="IGST" style="width: 30px; text-align: right;" ng-model="Invoice.IGST" ng-change="caltax1()" ng-init="Invoice.IGST=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&( Invoice_Form.IGST.$error.required || Invoice_Form.IGST.$error.pattern))">* </span>

                            <div style="float: right">
                                <label style="text-align: right; font-weight: bold;">{{IGST }}</label>
                            </div>
                        </td>
                    </tr>


                    <tr>
                        <th width="40%"></th>

                        <%-- <th width="40%">
                            <label id="lblPayComment" ng-show="IsVisible" style="padding-left: 25px;">Comment :    </label>
                            <textarea id="txtPaymentComment" runat="server" ng-show="IsVisible" class="k-textbox" name="txtComment" ng-model="Invoice.PaymentComment" style="width: 64%; height: 50px;"></textarea>
                        </th>--%>
                        <th class="thclass1">Other Charges: 
                        </th>
                        <td class="tdclass">
                            <input type="text" id="txtTransCharge" name="txtTransCharge" style="width: 30px; text-align: right;" ng-model="Invoice.transcharge" ng-change="caltax1()" ng-init="Invoice.transcharge=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&(Invoice_Form.txtTransCharge.$error.required || Invoice_Form.txtTransCharge.$error.pattern))">* </span>

                            <div style="float: right">
                                <label style="text-align: right; font-weight: bold;">{{ OtherCharge }}</label>
                            </div>
                        </td>

                    </tr>
                    <tr>
                        <th width="40%"></th>
                        <th class="thclass1">VAT: 
                        </th>
                        <td class="tdclass">
                            <input type="text" id="txtVATCharges" name="txtVATCharges" style="width: 30px; text-align: right;" ng-model="Invoice.VATCharges" ng-change="caltax1()" ng-init="Invoice.VATCharges=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&(Invoice_Form.txtVATCharges.$error.required || Invoice_Form.txtVATCharges.$error.pattern))">* </span>

                            <div style="float: right">
                                <label style="text-align: right; font-weight: bold;">{{ VATCharge }}</label>
                            </div>
                        </td>

                    </tr>
                    <tr>
                        <th width="40%"></th>
                        <th class="thclass1">CST: 
                        </th>
                        <td class="tdclass">
                            <input type="text" id="txtCSTCharges" name="txtCSTCharges" style="width: 30px; text-align: right;" ng-model="Invoice.CSTCharges" ng-change="caltax1()" ng-init="Invoice.CSTCharges=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&(Invoice_Form.txtCSTCharges.$error.required || Invoice_Form.txtCSTCharges.$error.pattern))">* </span>

                            <div style="float: right">
                                <label style="text-align: right; font-weight: bold;">{{ CSTCharge }}</label>
                            </div>
                        </td>

                    </tr>
                    <tr>

                        <th width="40%"></th>

                        <%-- <th width="40%">
                            <label id="lblPayComment" ng-show="IsVisible" style="padding-left: 25px;">Comment :    </label>
                            <textarea id="txtPaymentComment" runat="server" ng-show="IsVisible" class="k-textbox" name="txtComment" ng-model="Invoice.PaymentComment" style="width: 64%; height: 50px;"></textarea>
                        </th>--%>
                        <th class="thclass1">GST: 
                        </th>
                        <td class="tdclass">
                            <input type="text" id="txtGST" name="txtGST" style="width: 30px; text-align: right;" ng-model="Invoice.GST" ng-change="caltax1()" ng-init="Invoice.GST=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&(Invoice_Form.txtGST.$error.required || Invoice_Form.txtGST.$error.pattern))">* </span>

                            <div style="float: right">
                                <label style="text-align: right; font-weight: bold;">{{GST}}</label>
                            </div>
                        </td>

                    </tr>
                    <tr>
                        <th width="40%"></th>
                        <th class="thclass">Grand Total: 
                        </th>
                        <td style="text-align: right; font-weight: bold;">{{Invoice.DisplayGrandTotal}}
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <div id="divSummary">
                                <%-- <table width="250" cellpadding="0" cellspacing="0">
                                    <tr><td colspan="2">Payment Summary</td></tr>
                                    <tr><td>Date</td><td>paid Amount</td></tr>
                                    <tr><td colspan="2">
                                        <div style="overflow-y:auto;height:60px;">
                                            <table>
                                                <tr><td>01//05/2015</td><td>us$ 2000000</td></tr>
                                               <tr><td>01//05/2015</td><td>us$ 2000000</td></tr>
                                                <tr><td>01//05/2015</td><td>2us$ 2000000</td></tr>
                                                 <tr><td>01//05/2015</td><td>us$ 2000000</td></tr>
                                                <tr><td>01//05/2015</td><td>us$ 2000000</td></tr>                                            
                                            </table>
                                        </div>
                                    </td> </tr>
                                </table>--%>
                            </div>

                        </th>
                        <th>
                            <label><b>Comment: </b></label>
                        </th>
                        <td>
                            <textarea id="txtComment" runat="server" class="k-textbox" name="txtComment" ng-model="Invoice.Comment" style="width: 100%; height: 80px; float: left;"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:center">
                            <input type="checkbox" id="TDSCheck" ng-checked="false" name="CheckedTDS" ng-model="Invoice.TDSCheck" />
                        </td>
                        <td colspan="2">
                            <div>
                                <h3>INCOME TAX DECLARATION - TDS ON Software Sales</h3>
                                <br />
                                <p>We hereby declare that the software items mentioned in the invoice are sold</p>
                                <ul>
                                    <li>Without any modification</li>
                                    <li>The company has already deducted </li>
                                </ul>
                                <p>Withholding Tax u/s 194J/195 of the Income Tax on these software and made necessary arrangement for remitting the same as per the time line prescribed by Income Tax Act, 1961.</p>
                                <ul>
                                    <li>PAN of the company is AABCD4984E.</li>
                                </ul>
                                <p><b>#</b>As per notification on 21/2012, TDS deduction is exempted on reselling of software licenses and does not require to be deducted.</p>
                                <br />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div id="GridInvoiceStatus"></div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: center">
                            <input type="button" value="Save" ng-click="SaveInvoice(Invoice_Form)" runat="server" style="width: 80px; height: 25px;" id="btnSave" /><%--ng-disabled="isProcessing"--%>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="button" value="Cancel" ng-click="ClosePopUp()" runat="server" style="width: 80px; height: 25px;" id="btnCancel" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="button" value="Void Invoice" ng-click="VoidInvoice()" runat="server" style="width: 80px; height: 25px;" id="btnVoid" />
                        </td>
                    </tr>

                    <%--</tr>--%>
                </table>

                <input type="hidden" id="hdninsertedby" ng-model="Invoice.insertedby" class="hdnval" runat="server" />
                <input type="hidden" id="hdnProjectInvoiceId" ng-model="Invoice.ProjectInvoiceId" class="hdnval" runat="server" />
                <input type="hidden" id="hdnLocationId" name="hdnLocationId" class="hdnval" runat="server" />

                <input type="hidden" id="hdnProjectName" class="hdnval" runat="server" />
                <input type="hidden" id="hdnRequest" class="hdnval" runat="server" />
                <input type="hidden" id="hdnInvoiceNo" class="hdnval" runat="server" />
                <input type="hidden" id="hdnStatus" class="hdnval" value="" runat="server" />

                <input type="hidden" id="hdnProjectClientStateId" class="hdnval" value="" runat="server" />

                <input type="hidden" id="hdnProjectCustStateId" class="hdnval" value="" runat="server" />
                <input type="hidden" id="hdnProjectGSTPercentage" class="hdnval" value="" runat="server" />
                <%--Added new hidden field for identifying cust country by Nikhil Shetye--%>
                <input type="hidden" id="hdnProjectCustCountry" class="hdnval" value="" runat="server" />
                <%--<ul>
                    <li ng-repeat="(key, errors) in Invoice_Form.$error track by $index"><strong>{{ key }}</strong> 
         
                        <ul>
                            <li ng-repeat="e in errors">{{ e.$name }} has an error: <strong>{{ key }}</strong>.</li>
                        </ul>
                    </li>
                </ul>--%>
            </div>

        </div>

        <div id="divMail" class="k-widget k-windowAdd mailbox" style="display: none; padding-top: 10px; height: 450px; padding-right: 10px; min-width: 300px; min-height: 50px; top: 15%; left: 486px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">
            <div class="popup_head">
                <table width="100%">
                    <tr>
                        <td colspan="2" align="center">
                            <span id="span2" style="font-size: large; font-weight: 100">Invoice Mail</span>
                            <img src="Images/delete_ic.png" class="close-button" alt="Close" onclick="CloseMailBox()" />
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
            </div>
            <table id="tblInvMail1" align="center" height="100" border="0" cellpadding="3"
                cellspacing="3" runat="server">
                <tr>
                    <td>To:
                    </td>
                    <td>
                        <asp:Label ID="lblTo" runat="server" Text=""></asp:Label>
                    </td>

                </tr>
                <tr>
                    <td>Cc:
                    </td>
                    <td>
                        <asp:TextBox ID="txtCc" size="60" runat="server"></asp:TextBox>
                    </td>

                    <%-- <td style="visibility: visible">Attached Documents:
                    <div id="dvAttachnemt" runat="server"></div>
                    </td>--%>
                </tr>
                <tr>
                    <td>Bcc:
                    </td>
                    <td>
                        <%--<asp:TextBox ID="txtBcc" size="60" runat="server" Text="accounts@intelegain.com"></asp:TextBox>--%>
                        <asp:TextBox ID="txtBcc" size="60" runat="server" Text=""></asp:TextBox>

                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>Subject:
                    </td>
                    <td>
                        <asp:TextBox ID="txtSubject" size="60" runat="server"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="visibility: visible">Attached Documents:
                    </td>
                    <td>
                        <div id="dvAttachnemt" runat="server"></div>
                        <div id="dvAttachnemt_1"></div>
                        <form enctype="multipart/form-data" method="post" action="ProjectInvoices.aspx">
                            <input type="file" id="fileupload" name="file" onchange="previewFile()" />
                        </form>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td valign="top" colspan="3">
                        <%--<asp:TextBox ID="txtEmail" runat="server" Rows="15" Width="400" TextMode="MultiLine"></asp:TextBox>--%>
                        <textarea id="txtEmail" runat="server" style="width: 515px; height: 250px;" rows="15" cols="70" class="k-textbox"></textarea>
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="custbutton" colspan="2">
                        <center>
                            <asp:Button ID="btnSendReceipt" runat="server" CssClass="small_button white_button open" Text="Send Receipt" OnClientClick="sendmail();" />
                        </center>
                    </td>
                </tr>

            </table>
        </div>

        <div id="divConfirm" class="popup model_popup hvr-fade" style="display: none;">
            <a href="#" class="closePopup_btn" onclick="CloseConfirm()">X</a>
            <!--===============start subscription-->
            <div class="md-content" style="border: #f2f2f2 1px solid;">

                <div class="window_content" style="align-content: center">
                    <strong>Do you want to download pdf with logo ?</strong>
                </div>

                <div class="row box_SC" style="margin-top: 30px;">
                    <div id="divYes" class="col2">
                        <div class="btn close submit_btn">Yes</div>
                    </div>
                    <div id="divNo" class="col2">
                        <div class="btn close cancel_btn">No</div>
                    </div>


                    <div class="clear"></div>
                </div>
                <!--End subscription-->
            </div>
        </div>
    </div>
</asp:Content>

