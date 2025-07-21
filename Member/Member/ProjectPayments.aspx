<%@ Page Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="ProjectPayments.aspx.cs" Inherits="Member_ProjectPayments" EnableEventValidation="false" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link type="text/css" href="css/InvoiceValidation.css" rel="stylesheet" />

    <%--Bellow links are for ng-grid --%>
    
    <link href="css/ng-grid.css" rel="stylesheet" />
    <%--<script src="js/jquery-2.0.3.min.js"></script>--%>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.0.2/angular.min.js"></script>


   <%-- <script src="js/1.3.7_angular.js"></script>
    <script src="js/ng-grid.js"></script>
    <script src="js/ng-grid-flexible-height.js"></script>--%>

    <%--Bellow links are for kendo controls (do not change sequence)--%>

    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>


    
     <script src="js/1.3.7_angular.js"></script>    
    <script src="js/ng-grid.js"></script>    
    <script src="js/ng-grid-flexible-height.js"></script>


    <script src="../Member/js/ProjectInvoice.js" type="text/javascript"></script>
    <script src="../Member/js/ProjectPayment.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">

        var GridInvoices = "#gridInvoices";

        function isNumberKeyOrDot(evt)
        {
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode > 31 && (charCode < 46 || charCode > 57 || charCode == 47))
                return false;
            return true;
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

    </script>

    <style type="text/css">
        .ViewData {
            background-image: url('images/zoom.png');
            min-width: 10px;
            width: 20px;
            height: 20px;
            background-size: 20px;
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

        /*for displaying text right aligned*/
        .k-grid .ra,
        .k-numerictextbox .k-input {
            text-align: right;
        }
        /*end*/

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
            width: 770px;
            height: 150px;
        }

        .buttondel {
            background-image: url('images/delete.png');
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
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="grdInv" ng-app="myApp" ng-controller="Payment_milestoneCtrl">
        <div class="content_wrap">
            <div class="gride_table">
                <div class="box_border">
                    <div class="grid_head">
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblProjectsModule" Text="Payment" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                                <td style="width: 720px;">
                                    <div style="float: left; left: 30px;">
                                        <asp:Label ID="Label1" Text="From" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                                        <input type="text" id="txtFromDate" onkeypress="return false;" style="width: 140px" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblPaidTo" Text="To" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                                        <input type="text" id="txtTODate" onkeypress="return false;" style="width: 140px" />
                                        &nbsp;&nbsp;&nbsp;
                                        <input type="button" value="Search" onclick="SearchInvoices();" runat="server" class="small_button white_button" />
                                    </div>
                                    <div style="float: right;">
                                        <input type="button" id="btnAddpayments" value="Add Payments" ng-click="OpenPopUp()" runat="server" class="small_button white_button" />
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
        <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; height: 640px; padding-right: 10px; min-width: 700px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">

            <div ng-form="Invoice_Form">
                <div class="popup_head">
                    <table width="100%">
                        <tr>
                            <td colspan="2" align="center">
                                <span id="span2" style="font-size: large; font-weight: 100">Payment</span>
                                <img src="Images/delete_ic.png" class="close-button" alt="Close" ng-click="ClosePopUp()" />
                            </td>
                        </tr>
                    </table>
                    <div class="clear">
                    </div>
                </div>

                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                    <tr>
                        <th>Project:   
                        </th>
                        <td>
                            <input type="hidden" id="hdnProjID" runat="server" />
                            <%--<select id="ddlProject" name="ddlProject" ng-model="Invoice.Project" ng-disabled="isDiabled" ng-options="p.Value as p.Key for p in Invoice.Projects" ng-change="pInvoice()">
                                <option value="">--Select--</option>
                            </select>
                            <span class="validation-error" ng-show="invalidSubmitAttempt && Invoice_Form.ddlProject.$error.required">* 
                            </span>--%>
                            <label id="Label2" title="" ng-model="Invoice.ProjectName" style="width: 250px; font-weight: bold" runat="server">{{Invoice.ProjectName}}</label>
                        </td>
                        <th>Payment Date: 
                        </th>
                        <td>
                            <input type="text" id="txtInvoiceDate" onkeypress="return false;" name="txtInvoiceDate" style="width: 240px" ng-model="Invoice.InvoiceDate" required />
                            <span class="validation-error" ng-show="(invalidSubmitAttempt && Invoice_Form.txtInvoiceDate.$error.required)">*
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <th>Customer Name: 
                        </th>
                        <td>
                            <label id="lblCustName" title="" ng-model="Invoice.customerName" style="width: 250px" runat="server">{{Invoice.customerName}}</label>
                        </td>
                        <%--<th>Due Date: 
                        </th>
                        <td>
                            <input id="txtDueDate" onkeyup="dateInput(this)" onkeypress="return false;" name="txtDueDate" style="width: 240px" ng-model="Invoice.duedate" />
                        </td>--%>
                    </tr>
                    <tr>
                        <th rowspan="2">Customer Address: 
                        </th>
                        <td rowspan="2">
                            <label id="lblCustAddress" title="" ng-model="Invoice.customerAddress" style="width: auto;" runat="server">{{Invoice.customerAddress}}</label>
                        </td>
                        <th>Currency:  
                        </th>
                        <td>
                            <%-- <label id="lblCurrency" title="" ng-model="Invoice.currSymbol" style="width: auto" runat="server">{{Invoice.currSymbol}}</label>--%>
                            <select id="ddlCurrency" name="ddlCurrency" ng-model="Invoice.CurrencyID" ng-disabled="isDiabled" ng-options="c.Value as c.Key for c in Invoice.Currencys">
                                <option value="">--Select--</option>
                            </select>

                            <input type="hidden" id="hdncurrid" ng-model="Invoice.currSymbol" class="hdnval" runat="server" />
                        </td>

                    </tr>
                    <tr>

                        <th>Exchange Rate:  
                        </th>
                        <td>
                            <input type="text" id="ExRate" name="ExRate" ng-model="Invoice.ExRate" ng-disabled="isDiabled" onkeypress="return isNumberKeyOrDot(event);" style="width: 150px" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" />
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&( Invoice_Form.ExRate.$error.required || Invoice_Form.ExRate.$error.pattern)) ">* 
                            </span>
                        </td>

                    </tr>
                    <tr>
                        <th>Payment Type:  
                        </th>
                        <td>

                            <select id="ddlPaymentType" name="ddlPaymentType" ng-model="Invoice.PaymentType"  ng-options="p.Value as p.Key for p in Invoice.PaymentTypes" required>
                                <option value="">--Select--</option>
                            </select>
                            &nbsp;&nbsp;&nbsp;
                            <%--<asp:Button ID="btnAddTypes" runat="server" Text="+" Width="5px" ng-disabled="isDiabled" CssClass="small_button white_button open" ng-click="AddTypes()"></asp:Button>--%>
                            <input type="button" id="btnAddTypes" value="+" ng-disabled="isDiabled" style="width: 5px;" ng-click="AddTypes()" runat="server" class="small_button white_button" />
                        </td>
                    </tr>
                    <tr>
                        <th>Amount:
                        </th>
                        
                        <td>
                            <%--<label id="lblcurr" ng-model="Invoice.currSymbol" style="width: 40px; float: left; visibility: visible"><b>{{Invoice.currSymbol}}</b></label>--%>
                            <input type="text" id="txtAmount" maxlength="8" name="Amount" ng-model="Invoice.Amount" ng-blur="AdjustCerdit()" ng-change="calculateGridPayment()" onkeypress="return isNumberKey(event);" style="width: 150px; font-weight: bold" />
                        </td>
                        <td></td>
                        <td>
                            <label style="float: left; margin-left: 120px;"><b>{{lblPaymentTotal}}</b>:</label>
                            <%--<input type="text" id="txtTotal" name="BalanceAmount" ng-model="Invoice.BalanceAmount" onkeypress="return false;" readonly="readonly" style="width: 80px; float: right" />--%>
                            <label id="txtTotal" ng-model="Invoice.BalanceAmount" style="width: 80px; text-align: right; float: right"><b>{{Invoice.BalanceAmount}}</b></label>
                        </td>
                    </tr>

                    <tr>
                        <th>
                            Tax Collected:
                        </th>
                        <td>
                            <%--<label id="lblcurr" ng-model="Invoice.currSymbol" style="width: 40px; float: left; visibility: visible"><b>{{Invoice.currSymbol}}</b></label>--%>
                            <input type="text" id="txtTaxCollected" maxlength="8" name="TaxCollected" ng-model="Invoice.TaxCollected"  onkeypress="return isNumberKey(event);" style="width: 150px; font-weight: bold" />
                        </td>
                    </tr>
                    <tr>
                        <th>Credited Amount:</th>
                        <td>
                            <label id="lblCurrency" title="" ng-model="Invoice.currSymbol" style="width: auto" runat="server">{{Invoice.currSymbol}}</label>
                            <label id="txtCreditAmount" ng-model="Invoice.CreditAmount" style="width: 80px; visibility: visible"><b>{{DisplayCreditAmountFormated}}</b></label>
                        </td>

                    </tr>
                    <tr>
                        <th colspan="1">Comment: 
                        </th>
                        <td colspan="3">
                            <textarea id="txtComment" runat="server" class="k-textbox" name="txtComment" ng-model="Invoice.Description" style="width: 100%; height: 70px"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div>
                                <div id="ngGridMilestone" class="gridStyle" style="height: 155px; width: 720px;" ng-grid="gridInvoice_MileStone">
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form" align="right">
                    <tr>
                        <td colspan="4" style="text-align: center">
                            <input type="button" value="Save" ng-click="SaveInvoicePayment(Invoice_Form)" runat="server" style="width: 80px; height: 25px;" class="small_button white_button" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                            
                            <input type="button" value="Cancel" ng-click="ClosePopUp()" runat="server" style="width: 80px; height: 25px;" class="small_button white_button" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="button" id="btnSendMail" value="Send Mail" onclick="openPopUP()" runat="server" style="width: 90px; height: 25px;" class="small_button white_button" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="button" id="btnDelete" value="Delete" ng-click="DeletePayment()" runat="server" style="width: 90px; height: 25px;" class="small_button white_button" />
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" border="0" style="visibility: hidden" width="60%" class="manage_form" align="left">
                    <tr>
                        <th colspan="1">Comment: 
                        </th>
                        <td colspan="2">
                            <textarea id="txtComment123" runat="server" class="k-textbox" name="txtComment" ng-model="Invoice.Comment" style="width: 350px; height: 100px"></textarea>
                        </td>
                    </tr>

                </table>

                <input type="hidden" id="hdninsertedby" ng-model="Invoice.InsertedBy" class="hdnval" runat="server" />
                <input type="hidden" id="hdnProjectInvoiceId" ng-model="Invoice.ProjectInvoiceId" class="hdnval" runat="server" />
                <%--<ul>
                    <li ng-repeat="(key, errors) in Invoice_Form.$error track by $index"><strong>{{ key }}</strong> 
         
                        <ul>
                            <li ng-repeat="e in errors">{{ e.$name }} has an error: <strong>{{ key }}</strong>.</li>
                        </ul>
                    </li>
                </ul>--%>
            </div>

        </div>

        <div class="k-widget k-windowAdd" id="divPaymentType" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 90px; min-height: 50px; top: 15%; left: 486px; z-index: 10003; opacity: 1; transform: scale(1); border: solid" data-role="draggable">
            <div>
                <div class="popup_head">
                    <h3>Add Payment Type</h3>
                    <img src="Images/delete_ic.png" class="close-button" ng-click="CloseType()"
                        alt="Close" />
                    <div class="clear">
                    </div>
                </div>

                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                    <tr>
                        <th>Type</th>
                        <td align="center">
                            <input id="txtTypes" type="text" ng-model="Invoice.PTypes" style="width: 300px;" onkeypress="return isChar(event,this);" class="k-textbox" required />
                            <span style="color: Red;">*</span>

                            </span>
                        </td>
                    </tr>

                    <tr>
                        <th></th>
                        <td>
                            <%--<asp:Button ID="lnkSaveDesignation" runat="server" Text="Save" CssClass="small_button white_button open" ng-click="SavePaymentTypes()" />--%>
                            <input type="button" id="lnkSaveDesignation" value="Save" ng-click="SavePaymentTypes()" runat="server" class="small_button white_button" />
                            &nbsp;&nbsp;&nbsp;<input type="button" class="small_button white_button " value="Cancel" id="btnCancel" ng-click="CloseType();" />
                        </td>
                    </tr>
                </table>
                <div>&nbsp;</div>
            </div>
        </div>

        <div id="divConfirm" class="k-widget k-windowAdd" style="display: none; padding-top: 10px; height: 150px; padding-right: 10px; min-width: 300px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">

            <div class="popup_head">
                <table width="100%">
                    <tr>
                        <td colspan="2" align="center">
                            <span id="span2" style="font-size: large; font-weight: 100">Confirmation</span>
                            <img src="Images/delete_ic.png" class="close-button" alt="Close" ng-click="ClosePopUp()" />
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
            </div>
            <div class="body">
                Do you want to delete this record?
            </div>
            <hr />
            <br />
            <br />
            <br />
            <div class="footer" align="center">
                <asp:Button ID="btnYes" runat="server" Style="width: 80px; height: 25px;" Text="Yes" />&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnNo" runat="server" Style="width: 80px; height: 25px;" Text="No" />
            </div>

        </div>

        <div id="divMail" class="k-widget k-windowAdd" style="display: none; padding-top: 10px; height: 450px; padding-right: 10px; min-width: 300px; min-height: 50px; top: 15%; left: 486px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">
            <div class="popup_head">
                <table width="100%">
                    <tr>
                        <td colspan="2" align="center">
                            <span id="span2" style="font-size: large; font-weight: 100">Receipt Mail</span>
                            <img src="Images/delete_ic.png" class="close-button" alt="Close" ng-click="CloseMail()" />
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

                    <td style="visibility: hidden">Attached Documents:
                    <div id="dvAttachnemt" runat="server"></div>

                    </td>
                </tr>
                <tr>
                    <td>Bcc:
                    </td>
                    <td>
                        <asp:TextBox ID="txtBcc" size="60" runat="server" Text="accounts@intelgain.com"></asp:TextBox>

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
    </div>

</asp:Content>


