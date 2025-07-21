<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProformaInvoices.aspx.cs" Inherits="Member_ProformaInvoices" MasterPageFile="~/Member/Admin.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--Bellow links are for ng-grid --%>

    <link type="text/css" href="css/InvoiceValidation.css" rel="stylesheet" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.0.2/angular.min.js"></script>
    <link href="css/ng-grid.css" rel="stylesheet" />


    <%--Bellow links are for kendo controls (do not change sequence)--%>

    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="js/1.3.7_angular.js" type="text/javascript"></script>
    <script src="js/ng-grid.js" type="text/javascript"></script>
    <script src="js/ng-grid-flexible-height.js" type="text/javascript"></script>

    <script src="js/1.3.7_angular.js" type="text/javascript"></script>
    <script src="js/ng-grid.js" type="text/javascript"></script>
    <script src="js/ng-grid-flexible-height.js" type="text/javascript"></script>

    <script src="../Member/js/ProformaInvoiceMilestone.js" type="text/javascript"></script>

    <script src="../Member/js/ProformaInvoice.js" type="text/javascript"></script>
    <script type="text/javascript">

        var GridProInvoices = "#gridProformaInvoices";
        $("#chkPaidInvoices").change(function () {
            if ($(this).is(':checked')) {
                var grid = $("#gridProformaInvoices").data("kendoGrid");
                grid.dataSource.read();
            }
        });

    </script>

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

        .TaxInvoice, .TaxInvoice:hover {
            background-image: url('../Member/images/folder_invoices.png');
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

        input.ng-invalid-pattern {
            border: 1px solid red;
            box-shadow: 0 0 10px red;
        }

        .ngHeaderText {
            text-overflow: clip;
            white-space: normal;
        }

        .thclass {
            width: 100px;
        }

        .tdclass {
            width: 180px;
        }

        .manage_form th {
            text-align: right;
        }

        div.k-windowAdd.mailbox {
            max-width: 610px;
            left: 50% !important;
            margin-left: -305px !important;
            height: auto;
        }
    </style>


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
                                    <asp:Label ID="lblProformaInvoice" Text="Proforma Invoices" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <div style="float: right;">
                                        <input type="button" ng-click="OpenPopUp()" class="small_button white_button" value="Add Invoices" id="btnAddInvoices" runat="server" />

                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="gridProformaInvoices"></div>
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
                                <span id="span2" style="font-size: large; font-weight: 100">Proforma Invoice</span>
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
                        <th>Currency:  
                        </th>
                        <td>
                            <label id="lblCurrency" title="" ng-model="Invoice.InCurrency" style="width: auto" runat="server">{{Invoice.InCurrency}}</label>
                            <input type="hidden" id="hdncurrid" ng-model="Invoice.currencyId" class="hdnval" runat="server" />
                        </td>
                    </tr>

                    <%-- <th>Due Date: 
                        </th>
                        <td>
                            <input  id="txtDueDate" onkeyup="dateInput(this)" onkeypress="return false;" name="txtDueDate" style="width: 240px" ng-model="Invoice.duedate" required  />
                            <span class="validation-error" ng-show="(invalidSubmitAttempt && Invoice_Form.txtDueDate.$error.required)">*
                            </span>
                        </td>--%>

                    <tr>
                        <th style="vertical-align: middle;">Customer Address: 
                        </th>
                        <td>
                            <label id="lblCustAddress" title="" ng-model="Invoice.customeraddress" style="width: 200px; text-align: justify" runat="server">{{Invoice.customeraddress}}</label>
                        </td>


                        <th>Exchange Rate:  
                        </th>
                        <td>
                            <input type="text" id="ExRate" name="ExRate" ng-model="Invoice.ExRate" style="width: 150px" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" />
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&( Invoice_Form.ExRate.$error.required || Invoice_Form.ExRate.$error.pattern)) ">* 
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <th>SAC/HSN Code:</th>
                        <td>
                               <asp:DropDownList ID="SacHsnCode" runat="server" Width="200px"></asp:DropDownList>
                      
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

                </table>
                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                    <tr>
                        <th width="40%" style="text-align: left;"></th>
                        
                        <th width="20%" class="thclass1">Sub Total:
                        </th>
                        <td width="40%" class="tdclass" style="text-align: right; font-weight: bold;">{{Invoice.totalPrice }}
                        </td>
                    </tr>
                    <tr>
                        <th width="40%" style="text-align: left;"></th>
                     
                        <th class="thclass1"><label id="lblTax1">Tax 1:</label>
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
                        <th width="40%" style="text-align: left;"></th>
                      
                        <th><label id="lblTax2">Tax 2:</label>
                        </th>
                        <td>
                            <input type="text" id="tax2" name="tax2" style="width: 30px; text-align: right;" ng-model="Invoice.tax2" ng-change="caltax1()" ng-init="Invoice.tax2=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&( Invoice_Form.tax2.$error.required || Invoice_Form.tax2.$error.pattern))">* </span>

                            <div style="float: right">
                                <label style="text-align: right; font-weight: bold;">{{InvoiceTax2 }}</label>
                            </div>
                        </td>

                    </tr>

                    <tr>
                        <th width="40%">
                             </th>

                        <th><label id="lblTax3">Tax 3:</label>
                        </th>

                        <td>
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
                        <th width="40%">
                             </th>

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
                        <th width="40%" style="text-align: left;"></th>
                      
                        <th class="thclass1">Other Charges: 
                        </th>
                        <td class="tdclass">
                            <input type="text" id="txtTransCharge" name="txtTransCharge" style="width: 30px; text-align: right;" ng-model="Invoice.transcharge" ng-change="caltax1()" ng-init="Invoice.transcharge=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&(Invoice_Form.txtTransCharge.$error.required || Invoice_Form.txtTransCharge.$error.pattern))">* </span>

                            <div style="float: right">
                                <label style="text-align: right; font-weight: bold;">{{OtherCharge }}</label>
                            </div>
                        </td>

                    </tr>

                    <tr>
                        <th width="40%"></th>
                        <th class="thclass1">VAT: 
                        </th>
                        <td class="tdclass">
                            <input type="text" id="txtVATCharges" name="txtVATCharges" style="width: 30px; text-align: right;" ng-model="Invoice.VATCharge" ng-change="caltax1()" ng-init="Invoice.VATCharge=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
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
                            <input type="text" id="txtCSTCharges" name="txtCSTCharges" style="width: 30px; text-align: right;"  ng-model="Invoice.CSTCharge" ng-change="caltax1()" ng-init="Invoice.CSTCharge=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&(Invoice_Form.txtCSTCharges.$error.required || Invoice_Form.txtCSTCharges.$error.pattern))">* </span>

                            <div style="float: right">
                                <label style="text-align: right; font-weight: bold;">{{ CSTCharge }}</label>
                            </div>
                        </td>

                    </tr>
                    <tr>
                        <th width="40%" style="text-align: left;"></th>
                      
                        <th class="thclass1">GST: 
                        </th>
                        <td class="tdclass">
                            <input type="text" id="txtGST" name="txtGST" style="width: 30px; text-align: right;" ng-model="Invoice.GST" ng-change="caltax1()" ng-init="Invoice.GST=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&(Invoice_Form.txtGST.$error.required || Invoice_Form.txtGST.$error.pattern))">* </span>

                            <div style="float: right">
                                <label style="text-align: right; font-weight: bold;">{{ GST }}</label>
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
                        <%--  <th>
                            <div id="divSummary">
                               
                            </div>

                        </th>--%>
                        <th width="40%" style="text-align: left;"></th>
                        <th>
                            <label><b>Comment: </b></label>
                        </th>
                        <td>
                            <textarea id="txtComment" runat="server" class="k-textbox" name="txtComment" ng-model="Invoice.Comment" style="width: 100%; height: 80px; float: left;"></textarea>
                        </td>
                    </tr>
                    <%-- <tr>
                        <td colspan="4">
                            <div id="GridInvoiceStatus"></div>
                        </td>
                    </tr>--%>
                    <tr>
                        <td colspan="4" style="text-align: center">
                            <input type="button" value="Save" ng-click="SaveInvoice(Invoice_Form)" runat="server" style="width: 80px; height: 25px;" id="btnSave" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="button" value="Cancel" ng-click="ClosePopUp()" runat="server" style="width: 80px; height: 25px;" id="btnCancel" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                           <input type="button" value="Void Invoice" ng-click="VoidProformaInvoice()" runat="server" style="width: 80px; height: 25px;" id="btnVoid" />
                        </td>
                    </tr>

                </table>



            </div>

        </div>

        <%--  PopUP Div Ends --%>
        <%--  PopUP for mail Div Starts --%>

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

                    <td style="visibility: visible">Attached Documents:
                    <div id="dvAttachnemt" runat="server"></div>

                    </td>
                </tr>
                <tr>
                    <td>Bcc:
                    </td>
                    <td>
                        <asp:TextBox ID="txtBcc" size="60" runat="server" Text="accounts@intelegain.com"></asp:TextBox>

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

        <%--  PopUP for mail Div Ends --%>
        <%--  PopUP Div Starts for TAX INVOICE --%>

        <div id="divAddTaxPopupOverlay" runat="server"></div>
        <div class="k-widget k-windowAdd" id="divAddTaxInvPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 740px; min-height: 50px; top: 1%; left: 350px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">

            <div ng-form="TaxInvoice_Form">

                <div class="popup_head">
                    <table width="100%">
                        <tr>
                            <td colspan="2" align="center">
                                <span id="span2" style="font-size: large; font-weight: 100">Tax Invoice</span>
                                <img src="Images/delete_ic.png" class="close-button" alt="Close" ng-click="CloseTaxPopUp()" />
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
                            <input type="hidden" id="hdnProjectID" runat="server" />
                            <label id="lblProjectID" title="" style="width: 250px" runat="server"></label>
                        </td>
                        <th>Invoice Date: 
                        </th>
                        <td>
                            <input type="text" id="txtTaxInvoiceDate" onkeypress="return false;" name="txtTaxInvoiceDate" onchange="ChangeDate()" style="width: 240px" ng-model="Invoice.invoicedate" required />
                            <span class="validation-error" ng-show="(invalidSubmitAttempt && TaxInvoice_Form.txtTaxInvoiceDate.$error.required)">*
                            </span>

                        </td>
                    </tr>
                    <tr>
                        <th>Customer Name: 
                        </th>
                        <td>
                            <label id="lblCustomerName" title="" ng-model="Invoice.customername" style="width: 250px" runat="server">{{Invoice.customername}}</label>
                        </td>
                        <th>Due Date: 
                        </th>
                        <td>
                            <input id="txtTaxDueDate" onkeyup="dateInput(this)" onkeypress="return false;" name="txtTaxDueDate" style="width: 240px" ng-model="Invoice.duedate" required />
                            <span class="validation-error" ng-show="(invalidSubmitAttempt && TaxInvoice_Form.txtTaxDueDate.$error.required)">*
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <th rowspan="3" style="vertical-align: middle;">Customer Address: 
                        </th>
                        <td rowspan="3">
                            <label id="lblCustomerAddress" title="" ng-model="Invoice.customeraddress" style="width: 200px; text-align: justify" runat="server">{{Invoice.customeraddress}}</label>
                        </td>
                        <th>Currency:  
                        </th>
                        <td>
                            <label id="lblTaxCurrency" title="" ng-model="Invoice.InCurrency" style="width: auto" runat="server">{{Invoice.InCurrency}}</label>
                            <input type="hidden" id="hdnCurrency" ng-model="Invoice.currencyId" class="hdnval" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <th>Exchange Rate:  
                        </th>
                        <td>
                            <input type="text" id="txtExRate" name="ExRate" ng-model="Invoice.ExRate" style="width: 150px" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" />
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&( TaxInvoice_Form.txtExRate.$error.required || Invoice_Form.txtExRate.$error.pattern)) ">* 
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <th>Invoice Number:  
                        </th>
                        <td>
                            <input type="text" id="txtTaxInvoiceNo" name="txtTaxInvoiceNo" ng-model="Invoice.InvoiceNo" style="width: 150px;" required />
                            <span class="validation-error" ng-show="(invalidSubmitAttempt && TaxInvoice_Form.txtTaxInvoiceNo.$error.required) ">* 
                            </span>
                            <span class="validation-error" style="display: none; color: Red; font-weight: normal !important" id="spnDuplicateTaxInvNo"></span>

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
                               <asp:DropDownList ID="ProjectSacHsnCode" runat="server" Width="200px"  ></asp:DropDownList>
                      
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
                                <div id="ngGridTaxMilestone" class="gridStyle" ng-grid="gridInvoice_MileStone" style="width: 725px;">
                                </div>
                            </div>
                        </td>
                    </tr>

                </table>
                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                    <tr>
                        <%--<th width="40%" style="text-align:left;">

                        </th>--%>
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

                        </th>
                        <th class="thclass1">  Tax 1:
                        </th>
                        <td class="tdclass">
                            <input type="text" id="txtInvTax1" name="txtInvTax1" style="width: 30px; text-align: right;" ng-model="Invoice.tax1" ng-change="caltax1()" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" ng-init="Invoice.tax1=0" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&(TaxInvoice_Form.txtInvTax1.$error.required || TaxInvoice_Form.txtInvTax1.$error.pattern))">* </span>

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
                        <th> Tax 2:
                        </th>
                        <td>
                            <input type="text" id="txttax2" name="txttax2" style="width: 30px; text-align: right;" ng-model="Invoice.tax2" ng-change="caltax1()" ng-init="Invoice.tax2=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&( TaxInvoice_Form.txttax2.$error.required || TaxInvoice_Form.txttax2.$error.pattern))">* </span>

                            <div style="float: right">
                                <label style="text-align: right; font-weight: bold;">{{InvoiceTax2 }}</label>
                            </div>
                        </td>

                    </tr>

                    <tr>
                        <th width="40%">
                            <label id="lblPayComment" ng-show="IsVisible" style="padding-left: 25px;">Comment :    </label>
                            <textarea id="Textarea1" runat="server" ng-show="IsVisible" class="k-textbox" name="txtComment" ng-model="Invoice.PaymentComment" style="width: 64%; height: 50px;"></textarea>
                        </th>

                        <th class="thclass1"> Tax 3:

                        </th>

                        <td>
                            <input type="text" id="txttax3" name="txtInvTax3" style="width: 30px; text-align: right;" ng-model="Invoice.tax3" ng-change="caltax1()" ng-init="Invoice.tax3=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&( TaxInvoice_Form.tax3.$error.required || TaxInvoice_Form.tax3.$error.pattern))">* </span>

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
                        <th width="40%">
                             </th>

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
                        <th class="thclass1">Other Charges: 
                        </th>
                        <td class="tdclass">
                            <input type="text" id="txtTaxTransCharge" name="txtTaxTransCharge" style="width: 30px; text-align: right;" ng-model="Invoice.transcharge" ng-change="caltax1()" ng-init="Invoice.transcharge=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&(TaxInvoice_Form.txtTaxTransCharge.$error.required || TaxInvoice_Form.txtTaxTransCharge.$error.pattern))">* </span>

                            <div style="float: right">
                                <label style="text-align: right; font-weight: bold;">{{OtherCharge }}</label>
                            </div>
                        </td>

                    </tr>

                    <tr>
                        <th width="40%"></th>
                        <th class="thclass1">VAT: 
                        </th>
                        <td class="tdclass">
                            <input type="text" id="txtVATCharges" name="txtVATCharges" style="width: 30px; text-align: right;" ng-model="Invoice.VATCharge" ng-change="caltax1()" ng-init="Invoice.VATCharge=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
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
                            <input type="text" id="txtCSTCharges" name="txtCSTCharges" style="width: 30px; text-align: right;" ng-model="Invoice.CSTCharge" ng-change="caltax1()" ng-init="Invoice.CSTCharge=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&(Invoice_Form.txtCSTCharges.$error.required || Invoice_Form.txtCSTCharges.$error.pattern))">* </span>

                            <div style="float: right">
                                <label style="text-align: right; font-weight: bold;">{{ CSTCharge }}</label>
                            </div>
                        </td>

                    </tr>

                    <tr>
                        <th width="40%"></th>
                        <th class="thclass1">GST: 
                        </th>
                        <td class="tdclass">
                            <input type="text" id="txtGST" name="txtGST" style="width: 30px; text-align: right;" ng-model="Invoice.GST" ng-change="caltax1()" ng-init="Invoice.GST=0" required ng-pattern="/^\d{0,18}(\.\d{1,2})?$/" /><b>%</b>
                            <span class="validation-error" ng-show="(invalidSubmitAttempt &&(Invoice_Form.txtGST.$error.required || Invoice_Form.txtGST.$error.pattern))">* </span>

                            <div style="float: right">
                                <label style="text-align: right; font-weight: bold;">{{ GST }}</label>
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
                            </div>

                        </th>
                        <th>
                            <label><b>Comment: </b></label>
                        </th>
                        <td>
                            <textarea id="txtTaxComment" runat="server" class="k-textbox" name="txtTaxComment" ng-model="Invoice.Comment" style="width: 100%; height: 80px; float: left;"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div id="GridInvoiceStatus"></div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: center">
                            <input type="button" value="Save" ng-click="SaveTaxInvoice(TaxInvoice_Form)" runat="server" style="width: 80px; height: 25px;" id="btnSaveTaxInv" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="button" value="Cancel" ng-click="CloseTaxPopUp()" runat="server" style="width: 80px; height: 25px;" id="btnCancelTaxInv" />
                            <%--  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         <input type="button" value="Void Invoice" ng-click="VoidProformaInvoice()" runat="server" style="width: 80px; height: 25px;" id="btnInvoiceVoid" />
                           </td>--%>
                    </tr>

                </table>



            </div>

        </div>





        <input type="hidden" id="hdninsertedby" ng-model="Invoice.insertedby" class="hdnval" runat="server" />
        <input type="hidden" id="hdnProformaInvoiceId" ng-model="Invoice.ProformaInvoiceId" class="hdnval" runat="server" />
        <input type="hidden" id="hdnLocationId" class="hdnval" runat="server" />
        <input type="hidden" id="hdnProjectName" class="hdnval" runat="server" />
        <input type="hidden" id="hdnRequest" class="hdnval" runat="server" />
        <input type="hidden" id="hdnInvoiceNo" class="hdnval" runat="server" />
        <input type="hidden" id="hdnStatus" class="hdnval" value="" runat="server" />


         <input type="hidden" id="hdnClientStateId" class="hdnval" value="" runat="server" />

          <input type="hidden" id="hdnCustStateId" class="hdnval" value="" runat="server" />
         <input type="hidden" id="hdnGSTPercentage" class="hdnval" value="" runat="server" />
        <%--Added New hidden field for understanding cust country by Nikhil Shetye--%>
        <input type="hidden" id="hdnCustCountry" class="hdnval" value="" runat="server" />

    </div>


</asp:Content>

