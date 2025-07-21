<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Invoice_Details.aspx.cs" Inherits="Admin_Invoice_Details" %>

<%@ Register Src="../Controls/adminMenu.ascx" TagName="adminMenu" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice Details</title>

    <script language="javascript" src="../includes/InvoiceCalendarControl.js" type="text/javascript"></script>
    <script type="text/javascript">

        function transferValue(ID) {
            var cost = document.getElementById("txtRegTotalCost" + ID).value;
            document.getElementById("txtRegAmount" + ID).value = cost;
            calculateRegAmount();
        }

        function setValue() {
            var dropdown = document.getElementById("drpCost");
            var myindex = dropdown.selectedIndex;
            var SelValue = dropdown.options[myindex].text;
            document.getElementById("lblTotalCost").innerHTML = SelValue;
        }

        function addRow(mode) {
            var number = document.getElementById("txtRowCount").value;
            number = parseInt(number) + 1;
            document.getElementById("tr" + mode + number).style.display = "";
            document.getElementById("txtRowCount").value = number;
            var desc = number - 1;
            if (mode == 'TM') {
                var tmDescription = document.getElementById("txtTMDescription" + desc).value;
                document.getElementById("txtTMDescription" + number).value = tmDescription;
            }

            else if (mode == 'Regular') {
                var regDescription = document.getElementById("txtRegDescription" + desc).value;
                document.getElementById("txtRegDescription" + number).value = regDescription;
            }
        }

        function removeRow(mode, ID) {
            if (confirm("Are you sure you want to delete the data?")) {
                if (mode == 'TM') {
                    document.getElementById('tr' + mode + ID).style.display = "none";
                    document.getElementById('txt' + mode + 'Description' + ID).value = '';
                    document.getElementById('txt' + mode + 'Rate' + ID).value = '';
                    document.getElementById('txt' + mode + 'Hours' + ID).value = '';
                    document.getElementById("payAmount" + ID).value = '';
                }
                else if (mode == 'Reg') {
                    document.getElementById('trRegular' + ID).style.display = "none";
                    document.getElementById('txt' + mode + 'Description' + ID).value = '';
                    document.getElementById('txt' + mode + 'TotalCost' + ID).value = '';
                    document.getElementById('txt' + mode + 'Amount' + ID).value = '';
                }
                calculateRegAmount();
            }
            return false;
        }

        function InvoiceCancellationConfirm()
        {
            var IsValid = false;
            if (confirm('Cancelling this invoice will permanently void the current invoice. Please confirm if you want to proceed with cancellation of the invoice?')) {
                if (document.getElementById('txtComment').value == '') {
                    alert("Please provide a reason for cancelling the invoice");
                    IsValid= false;
                }
                else 
                    IsValid = true;
            }
            else 
                IsValid = false;

            return IsValid;
        }

        function ShowRow(mode) {
            var number = document.getElementById("txtRowCount").value;

            for (var i = 1 ; i <= number; i++) {
                try {
                    document.getElementById("tr" + mode + i).style.display = "";
                }
                catch (e) {
                }
            }
        }

        function calculateAmount() {
            var total = 0;
            for (var i = 1; i <= 5; i++) {
                var amount = 0;
                var varTMRate = document.getElementById("txtTMRate" + i);
                var varTMHours = document.getElementById("txtTMHours" + i);
                try {
                    amount = parseFloat(varTMRate.value) * parseFloat(varTMHours.value);
                    if (isNaN(amount))
                        amount = 0;
                }
                catch (e) {

                }
                if (amount != 0) {
                    document.getElementById("lblpayamount" + i).innerHTML = amount;
                    total = total + amount;
                }
            }
            if (total != 0) {
                document.getElementById("lblTMAmount").innerHTML = total;
                document.getElementById("payAmount").value = total;
            }
        }

        function calculateRegAmount() {
            var total = 0;
            var number = document.getElementById("txtRowCount").value;
            for (var i = 1; i <= number; i++) {
                var RegAmount = 0;
                RegAmount = parseFloat(document.getElementById("txtRegAmount" + i).value);
                if (!isNaN(RegAmount)) {
                    total = total + RegAmount;
                }

            }
            if (total != 0) {
                document.getElementById("lblRegAmount").innerHTML = total;
                document.getElementById("payAmount").value = total;

            }
        }

        function validateAmount() {
            var Ok = true;
            var number = document.getElementById("txtRowCount").value;
            for (var i = 1; i <= number; i++) {
                var allEmpty = true;
                document.getElementById("tdReg" + i).innerHTML = "";
                var varDesc = document.getElementById("txtRegDescription" + i);
                var varRegAmount = document.getElementById("txtRegAmount" + i);
                var varRegTotalCost = document.getElementById("txtRegTotalCost" + i);
                if (varDesc.value != "")
                    allEmpty = false;
                if (varRegAmount.value != "")
                    allEmpty = false;
                if (varRegTotalCost.value != "")
                    allEmpty = false;


                if ((trim(varDesc.value) != "") && (varRegAmount.value != "") && (varRegTotalCost.value != "")) {
                    if ((parseFloat(varRegAmount.value)) > (parseFloat(varRegTotalCost.value))) {
                        document.getElementById("tdReg" + i).innerHTML = "Please enter Proper Amount.";
                        Ok = false;
                    }
                    else
                        document.getElementById("tdReg" + i).innerHTML = "";
                }
                else if (allEmpty == false) {
                    if ((trim(varDesc.value) == "") || (varRegAmount.value == "") || (varRegTotalCost.value == "")) {
                        document.getElementById("tdReg" + i).innerHTML = "Please Enter Proper Fields.";
                        Ok = false;
                    }
                    else
                        document.getElementById("tdReg" + i).innerHTML = "";
                }
            }

            if (trim(document.frmInvoiceDetails.payDate.value) == "" || trim(document.frmInvoiceDetails.payDate.value) == '1/1/1900') {
                document.getElementById("lblpayDate").innerHTML = "REQUIRED";
                Ok = false;
            }
            else
                document.getElementById("lblpayDate").innerHTML = "";


            if (trim(document.frmInvoiceDetails.payExRate.value) == "" || trim(document.frmInvoiceDetails.payExRate.value) == "0") {
                document.getElementById("lblExchangeRate").innerHTML = "REQUIRED";
                Ok = false;
            }

            else
                document.getElementById("lblExchangeRate").innerHTML = "";

            return Ok;
        }

        function validate() {
            var number = document.getElementById("txtRowCount").value;
            var allOk = true;
            for (var i = 1; i <= number; i++) {
                var allEmpty = true;
                var varDesc = document.getElementById("txtTMDescription" + i);
                var varTMRate = document.getElementById("txtTMRate" + i);
                var varTMHour = document.getElementById("txtTMHours" + i);
                if (varTMRate.value != "")
                    allEmpty = false;
                if (varTMHour.value != "")
                    allEmpty = false;
                if (varDesc.value != "")
                    allEmpty = false;
                if (allEmpty == false) {
                    if ((varTMRate.value == "") || (varTMHour.value == "") || (varDesc.value == "")) {
                        document.getElementById("tdTM" + i).innerHTML = "Please Enter Proper Fields.";
                        allOk = false;
                    }
                }

            }

            if (trim(document.frmInvoiceDetails.payDate.value) == "" || trim(document.frmInvoiceDetails.payDate.value) == '1/1/1900') {
                document.getElementById("lblpayDate").innerHTML = "REQUIRED";
                allOk = false;
            }
            else
                document.getElementById("lblpayDate").innerHTML = "";


            if (trim(document.frmInvoiceDetails.payExRate.value) == "" || trim(document.frmInvoiceDetails.payExRate.value) == "0") {
                document.getElementById("lblExchangeRate").innerHTML = "REQUIRED";
                allOk = false;
            }

            else
                document.getElementById("lblExchangeRate").innerHTML = "";


            return allOk;
        }

        function chkInvoiceType() {
            var type = '<%=strpayType %>';
        if (type == "1")
            calculateRegAmount(); //  Regular Type:
        else
            calculateAmount(); //  T&M Type:
    }



    function trim(stringToTrim) {
        return stringToTrim.replace(/^\s+|\s+$/g, "");
    }

    function extractNumber(obj, decimalPlaces, allowNegative) {

        var temp = obj.value;

        // avoid changing things if already formatted correctly
        var reg0Str = '[0-9]*';
        if (decimalPlaces > 0) {
            reg0Str += '\\.?[0-9]{0,' + decimalPlaces + '}';
        }
        else if (decimalPlaces < 0) {
            reg0Str += '\\.?[0-9]*';
        }
        reg0Str = allowNegative ? '^-?' + reg0Str : '^' + reg0Str;
        reg0Str = reg0Str + '$';
        var reg0 = new RegExp(reg0Str);
        if (reg0.test(temp)) return true;

        // first replace all non numbers
        var reg1Str = '[^0-9' + (decimalPlaces != 0 ? '.' : '') + (allowNegative ? '-' : '') + ']';
        var reg1 = new RegExp(reg1Str, 'g');
        temp = temp.replace(reg1, '');

        if (allowNegative) {
            // replace extra negative
            var hasNegative = temp.length > 0 && temp.charAt(0) == '-';
            var reg2 = /-/g;
            temp = temp.replace(reg2, '');
            if (hasNegative) temp = '-' + temp;
        }

        if (decimalPlaces != 0) {
            var reg3 = /\./g;
            var reg3Array = reg3.exec(temp);
            if (reg3Array != null) {
                // keep only first occurrence of .
                //  and the number of places specified by decimalPlaces or the entire string if decimalPlaces < 0
                var reg3Right = temp.substring(reg3Array.index + reg3Array[0].length);
                reg3Right = reg3Right.replace(reg3, '');
                reg3Right = decimalPlaces > 0 ? reg3Right.substring(0, decimalPlaces) : reg3Right;
                temp = temp.substring(0, reg3Array.index) + '.' + reg3Right;
            }
        }

        obj.value = temp;
    }

    function getCurreny() {
        document.getElementById("hdnCurreny").value = document.getElementById("drpCurrency").value;
        //alert(document.getElementById("hdnCurreny").value);
    }
    </script>

    <link rel="stylesheet" href="/includes/CalendarControl.css" type="text/css" />
    <link rel="stylesheet" href="../css/style.css" type="text/css" />
</head>
<body onload="javascript:chkInvoiceType();">
    <form id="frmInvoiceDetails" runat="server">
        <input type="hidden" id="payCode" runat="server" width="0" size="20" />
        <input type="hidden" id="projCode" runat="server" width="0" size="20" />
        <asp:Label ID="lblCRID" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="lblCRTitle" runat="server" Text="" Visible="false"></asp:Label>
        <table cellpadding="4" width="98%" border="0" align="center">
            <tr>
                <td colspan="4">
                    <uc1:adminMenu ID="adminMenu2" runat="server" />
                </td>
            </tr>
            <tr>
                <td width="25%" style="color: rgb(162, 146, 30); background-color: rgb(197, 213, 174); font-size: 9pt; font-weight: bold;">Project
                </td>
                <td width="25%" style="color: Black; background-color: rgb(255, 255, 238);">
                    <asp:Label runat="server" ID="projNameLbl" />
                </td>
                <td width="25%" style="color: rgb(162, 146, 30); background-color: rgb(197, 213, 174); font-size: 9pt; font-weight: bold;">Payment Due Date
                </td>
                <td width="25%" style="color: Black; background-color: rgb(255, 255, 238);">
                    <input type="text" id="payDate" runat="server" size="10" name="payDate" onclick="popupCalender('payDate')"
                        readonly="true" />
                    <font face="verdana" size="2" color="Red"><b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblpayDate" runat="server" Display="Dynamic" Font-Names="Verdana"
                        Font-Size="Xx-Small" Text=""></asp:Label>
                    </b></font>
                </td>
            </tr>
            <tr>
                <td width="25%" style="color: rgb(162, 146, 30); background-color: rgb(197, 213, 174); font-size: 9pt; font-weight: bold;">Customer
                </td>
                <td width="25%" style="color: Black; background-color: rgb(255, 255, 238);">
                    <asp:Label runat="server" ID="custCompany" />
                </td>
                <td width="25%" style="color: rgb(162, 146, 30); background-color: rgb(197, 213, 174); font-size: 9pt; font-weight: bold;">Amount
                </td>
                <td width="25%" style="color: Black; background-color: rgb(255, 255, 238);">
                    <input type="text" id="payAmount" readonly="true" runat="server" size="10" name="payAmount" />
                </td>
            </tr>
            <tr>
                <td width="25%" style="color: rgb(162, 146, 30); background-color: rgb(197, 213, 174); font-size: 9pt; font-weight: bold;">Currency
                </td>
                <td width="25%" style="color: Black; background-color: rgb(255, 255, 238);">
                    <asp:DropDownList ID="drpCurrency" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpCurrency_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:HiddenField id="hdnCurreny" Value="0" runat="server" /> 
                </td>
                <td width="25%" style="color: rgb(162, 146, 30); background-color: rgb(197, 213, 174); font-size: 9pt; font-weight: bold;">Exchange Rate
                </td>
                <td width="25%" style="color: Black; background-color: rgb(255, 255, 238);">
                    <input type="text" id="payExRate" runat="server" size="3" name="payExRate" onkeyup="javascript:return extractNumber(this,2,false);" />
                    <font face="verdana" size="2" color="Red"><b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblExchangeRate" runat="server" Display="Dynamic" Font-Names="Verdana"
                        Font-Size="Xx-Small" Text=""></asp:Label></b></font>
                </td>
            </tr>
            <tr>
                <td align="left" style="color: rgb(162, 146, 30); background-color: rgb(197, 213, 174); font-size: 9pt; font-weight: bold;">Trans Charges(%)
                </td>
                <td align="left" style="color: Black; background-color: rgb(255, 255, 238);">
                    <input id="payTransCharge" type="text" size="3" name="payTransCharge" value="0" readonly="true"
                        runat="server" />
                </td>
                <td align="left" style="color: rgb(162, 146, 30); background-color: rgb(197, 213, 174); font-size: 9pt; font-weight: bold;">Payment Date
                </td>
                <td align="left" style="color: Black; background-color: rgb(255, 255, 238);">
                    <input id="payConfirmDate" onclick="popupCalender('payConfirmDate')" type="text"
                        size="10" name="payConfirmDate" runat="server" readonly />
                    <asp:Button ID="update" runat="server" Text="Confirm Payment Date" OnClick="update_Click"/>
                </td>
            </tr>
            <tr>
                <td align="left" style="color: rgb(162, 146, 30); background-color: rgb(197, 213, 174); font-size: 9pt; font-weight: bold;"
                    valign="top">Comment</td>
                <td valign="top">

                    <asp:TextBox ID="txtComment" runat="server" Rows="1" Columns="22" TextMode="MultiLine" Style="margin-bottom: 5px"></asp:TextBox>
                    &nbsp;&nbsp;<asp:Button ID="btnAddComment" runat="server" Text="Add Comment"
                        OnClick="btnAddComment_Click" Enabled="false" />

                </td>
                <td>
                    <div style="height: 85px; overflow: scroll; padding-top: 5px;">
                        <asp:Label ID="lblComment" runat="server" Text=""></asp:Label>
                    </div>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="4">
                    <table border="0" cellpadding="0" cellspacing="0" width="98%">
                        <tr>
                            <td width="25%">Invoice Type
                            </td>
                            <td width="25%">
                                <asp:RadioButton onclick="javascript:ShowHide('tblRegularInvoice');" ID="rdRegular"
                                    Checked="true" GroupName="grpname" runat="server" Text="Regular" />&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton onclick="javascript:ShowHide('tblTMInvoice');" GroupName="grpname"
                                ID="rdTM" runat="server" Text="T&M" />
                            </td>
                            <td width="25%" align="left">
                                <asp:DropDownList ID="drpCost" runat="server" onchange="javascript:return setValue();">
                                    <asp:ListItem Text="Total Cost" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Total Cost/Month" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Total Cost/Hour" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td width="25%">
                                <asp:Button ID="btnCopy" CausesValidation="false" runat="server" Text="Copy Invoice" OnClick="btnCopy_Click" />&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnTMBack" CausesValidation="false" runat="server" Text="Back" OnClick="btnBack_Click" />&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCancelInvoice" CausesValidation="false" runat="server" Text="Cancel Invoice" OnClientClick="return InvoiceCancellationConfirm();"  OnClick="btnCancelInvoice_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <table id="tblTMInvoice" cellpadding="2" width="100%" border="0" runat="server">
                        <tr style="color: rgb(162, 146, 30); background-color: rgb(197, 213, 174); font-size: 9pt; font-weight: bold; width: 100px;">
                            <th width="35%">Description
                            </th>
                            <th width="15%">Rate/hour (<asp:Label ID="lblRateCurr" runat="server" Text=""></asp:Label>)
                            </th>
                            <th width="15%">Hours
                            </th>
                            <th width="35%">Amount Payable /Invoice Amount(<asp:Label ID="lblTMCurr" runat="server" Text=""></asp:Label>)
                            </th>
                        </tr>
                        <tr style="color: Black; background-color: rgb(255, 255, 238);" id="trTM1">
                            <td align="left">
                                <asp:TextBox ID="txtTMDescription1" size="75" runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtTMRate1" size="8" onblur="javascript:calculateAmount();" onkeyup="javascript:return extractNumber(this,2,false);"
                                    runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtTMHours1" size="10" onblur="javascript:calculateAmount();" onkeyup="javascript:return extractNumber(this,2,false);"
                                    runat="server"></asp:TextBox>
                            </td>
                            <td id="payAmount1" align="center">
                                <label id="lblpayamount1" runat="server">
                                </label>
                            </td>
                            <td style="width: 16px;">
                                <asp:ImageButton ID="imgDelete1" ImageUrl="../Images/delete.png" runat="server" OnClientClick="javascript:return removeRow('TM',1);" />
                            </td>
                        </tr>
                        <tr style="color: Black; background-color: rgb(255, 255, 238);">
                            <td colspan="5">
                                <div id="tdTM1" style="color: Red; font-family: Arial; font-size: 10px; float: right; padding-right: 30px;">
                                </div>
                            </td>
                        </tr>
                        <tr style="display: none; color: Black; background-color: rgb(255, 255, 238);" id="trTM2">
                            <td align="left">
                                <asp:TextBox ID="txtTMDescription2" size="75" runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtTMRate2" size="8" onblur="javascript:calculateAmount();" onkeyup="javascript:return extractNumber(this,2,false);"
                                    runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtTMHours2" size="10" MaxLength="6" onblur="javascript:calculateAmount();"
                                    onkeyup="javascript:return extractNumber(this,2,false);" runat="server"></asp:TextBox>
                            </td>
                            <td id="payAmount2" runat="server" align="center">
                                <label id="lblpayamount2" runat="server">
                                </label>
                            </td>
                            <td>
                                <asp:ImageButton ID="imgDelete2" ImageUrl="../Images/delete.png" runat="server" OnClientClick="javascript:return removeRow('TM',2);" />
                            </td>
                        </tr>
                        <tr style="color: Black; background-color: rgb(255, 255, 238);">
                            <td colspan="5">
                                <div id="tdTM2" style="color: Red; font-family: Arial; font-size: 10px; float: right; padding-right: 30px;">
                                </div>
                            </td>
                        </tr>
                        <tr style="display: none; color: Black; background-color: rgb(255, 255, 238);" id="trTM3">
                            <td align="left">
                                <asp:TextBox ID="txtTMDescription3" size="75" runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtTMRate3" onblur="javascript:calculateAmount();" onkeyup="javascript:return extractNumber(this,2,false);"
                                    size="8" runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtTMHours3" MaxLength="6" onblur="javascript:calculateAmount();"
                                    onkeyup="javascript:return extractNumber(this,2,false);" size="10" runat="server"></asp:TextBox>
                            </td>
                            <td id="payAmount3" runat="server" align="center">
                                <label id="lblpayamount3" runat="server">
                                </label>
                            </td>
                            <td>
                                <asp:ImageButton ID="imgDelete3" ImageUrl="../Images/delete.png" runat="server" OnClientClick="javascript:return removeRow('TM',3);" />
                            </td>
                        </tr>
                        <tr style="color: Black; background-color: rgb(255, 255, 238);">
                            <td colspan="5">
                                <div id="tdTM3" style="color: Red; font-family: Arial; font-size: 10px; float: right; padding-right: 30px;">
                                </div>
                            </td>
                        </tr>
                        <tr style="display: none; color: Black; background-color: rgb(255, 255, 238);" id="trTM4">
                            <td align="left">
                                <asp:TextBox ID="txtTMDescription4" size="75" runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtTMRate4" onblur="javascript:calculateAmount();" onkeyup="javascript:return extractNumber(this,2,false);"
                                    size="8" runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtTMHours4" MaxLength="6" onblur="javascript:calculateAmount();"
                                    onkeyup="javascript:return extractNumber(this,2,false);" size="10" runat="server"></asp:TextBox>
                            </td>
                            <td id="payAmount4" runat="server" align="center">
                                <label id="lblpayamount4" runat="server">
                                </label>
                            </td>
                            <td>
                                <asp:ImageButton ID="imgDelete4" ImageUrl="../Images/delete.png" runat="server" OnClientClick="javascript:return removeRow('TM',4);" />
                            </td>
                        </tr>
                        <tr style="color: Black; background-color: rgb(255, 255, 238);">
                            <td colspan="5">
                                <div id="tdTM4" style="color: Red; font-family: Arial; font-size: 10px; float: right; padding-right: 30px;">
                                </div>
                            </td>
                        </tr>
                        <tr style="display: none; color: Black; background-color: rgb(255, 255, 238);" id="trTM5">
                            <td align="left">
                                <asp:TextBox ID="txtTMDescription5" size="75" runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtTMRate5" onblur="javascript:calculateAmount();" onkeyup="javascript:return extractNumber(this,2,false);"
                                    size="8" runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtTMHours5" onblur="javascript:calculateAmount();" onkeyup="javascript:return extractNumber(this,2,false);"
                                    size="10" runat="server"></asp:TextBox>
                            </td>
                            <td id="payAmount5" runat="server" align="center">
                                <label id="lblpayamount5" runat="server">
                                </label>
                            </td>
                            <td>
                                <asp:ImageButton ID="imgDelete5" ImageUrl="../Images/delete.png" runat="server" OnClientClick="javascript:return removeRow('TM',5);" />
                            </td>
                        </tr>
                        <tr style="color: Black; background-color: rgb(255, 255, 238);">
                            <td colspan="5">
                                <div id="tdTM5" style="color: Red; font-family: Arial; font-size: 10px; float: right; padding-right: 30px;">
                                </div>
                            </td>
                        </tr>
                        <tr style="color: Black; background-color: rgb(255, 255, 238);">
                            <td colspan="4" style="text-align: right;">
                                <input type="button" value="add" onclick="javascript: addRow('TM')" />
                            </td>
                        </tr>
                        <tr style="color: Black; background-color: rgb(255, 255, 238);">
                            <td align="center">
                                <asp:Label ID="lblTMTotal" runat="server" Text=""></asp:Label>
                            </td>
                            <td align="center"></td>
                            <td align="center"></td>
                            <td align="center">
                                <asp:Label ID="lblTMAmount" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"></td>
                            <td align="right">
                                <asp:Button ID="btnTMGenerate" runat="server" Text="Generate Invoice" OnClick="btnTMGenerate_Click"
                                    ValidationGroup="grp1" OnClientClick="javascript:return validate();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <table id="tblRegularInvoice" cellpadding="2" width="100%" border="0" runat="server">
                        <tr style="color: rgb(162, 146, 30); background-color: rgb(197, 213, 174); font-size: 9pt; font-weight: bold; width: 100px;">
                            <th width="50%">Description
                            </th>
                            <th width="25%">
                                <asp:Label ID="lblTotalCost" runat="server" Text="Total Cost"></asp:Label>(<asp:Label ID="lblCostCurr" runat="server" Text=""></asp:Label>)
                            </th>
                            <th width="41%">Amount Payable / Invoice Amount (<asp:Label ID="lblRegCurr" runat="server" Text=""></asp:Label>)
                            </th>
                            <td width="4%">
                                <input type="button" id="btnAddNew" runat="server" value="Add" onclick="javascript: addRow('Regular')" title="Add New" /></td>
                        </tr>
                        <tr style="color: Black; background-color: rgb(255, 255, 238);" id="trRegular1">
                            <td align="left">
                                <asp:TextBox ID="txtRegDescription1" size="75" runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtRegTotalCost1" onkeyup="javascript:return extractNumber(this,2,false);" onblur="javascript:return transferValue(1);"
                                    size="15" runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtRegAmount1" onkeyup="javascript:return extractNumber(this,2,false);"
                                    onblur="javascript:calculateRegAmount();" size="15" runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 16px;">
                                <asp:ImageButton ID="regDelete1" ImageUrl="../Images/delete.png" runat="server" ToolTip="Delete" OnClientClick="javascript:return removeRow('Reg',1);" />
                            </td>
                        </tr>
                        <tr style="color: Black; background-color: rgb(255, 255, 238);">
                            <td colspan="3">
                                <div id="tdReg1" style="color: Red; font-family: Arial; font-size: 10px; float: right; padding-right: 30px;">
                                </div>
                            </td>
                        </tr>
                        <tr style="display: none; color: Black; background-color: rgb(255, 255, 238);" id="trRegular2">
                            <td align="left">
                                <asp:TextBox ID="txtRegDescription2" size="75" runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtRegTotalCost2" size="15" onkeyup="javascript:return extractNumber(this,2,false);" onblur="javascript:return transferValue(2);"
                                    runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtRegAmount2" size="15" onblur="javascript:calculateRegAmount();"
                                    onkeyup="javascript:return extractNumber(this,2,false);" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:ImageButton ID="regDelete2" ImageUrl="../Images/delete.png" runat="server" OnClientClick="javascript:return removeRow('Reg',2);" />
                            </td>
                        </tr>
                        <tr style="color: Black; background-color: rgb(255, 255, 238);">
                            <td colspan="3">
                                <div id="tdReg2" style="color: Red; font-family: Arial; font-size: 10px; float: right; padding-right: 30px;">
                                </div>
                            </td>
                        </tr>
                        <tr style="display: none; color: Black; background-color: rgb(255, 255, 238);" id="trRegular3">
                            <td align="left">
                                <asp:TextBox ID="txtRegDescription3" size="75" runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtRegTotalCost3" size="15" onkeyup="javascript:return extractNumber(this,2,false);" onblur="javascript:return transferValue(3);"
                                    runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtRegAmount3" size="15" onblur="javascript:calculateRegAmount();"
                                    onkeyup="javascript:return extractNumber(this,2,false);" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:ImageButton ID="regDelete3" ImageUrl="../Images/delete.png" runat="server" OnClientClick="javascript:return removeRow('Reg',3);" />
                            </td>
                        </tr>
                        <tr style="color: Black; background-color: rgb(255, 255, 238);">
                            <td colspan="3">
                                <div id="tdReg3" style="color: Red; font-family: Arial; font-size: 10px; float: right; padding-right: 30px;">
                                </div>
                            </td>
                        </tr>
                        <tr style="display: none; color: Black; background-color: rgb(255, 255, 238);" id="trRegular4">
                            <td align="left">
                                <asp:TextBox ID="txtRegDescription4" size="75" runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtRegTotalCost4" size="15" onkeyup="javascript:return extractNumber(this,2,false);" onblur="javascript:return transferValue(4);"
                                    runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtRegAmount4" size="15" onblur="javascript:calculateRegAmount();"
                                    onkeyup="javascript:return extractNumber(this,2,false);" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:ImageButton ID="regDelete4" ImageUrl="../Images/delete.png" runat="server" OnClientClick="javascript:return removeRow('Reg',4);" />
                            </td>
                        </tr>
                        <tr style="color: Black; background-color: rgb(255, 255, 238);">
                            <td colspan="3">
                                <div id="tdReg4" style="color: Red; font-family: Arial; font-size: 10px; float: right; padding-right: 30px;">
                                </div>
                            </td>
                        </tr>
                        <tr style="display: none; color: Black; background-color: rgb(255, 255, 238);" id="trRegular5">
                            <td align="left">
                                <asp:TextBox ID="txtRegDescription5" size="75" runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtRegTotalCost5" size="15" onkeyup="javascript:return extractNumber(this,2,false);" onblur="javascript:return transferValue(5);"
                                    runat="server"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtRegAmount5" size="15" onblur="javascript:calculateRegAmount();"
                                    onkeyup="javascript:return extractNumber(this,2,false);" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:ImageButton ID="regDelete5" ImageUrl="../Images/delete.png" runat="server" OnClientClick="javascript:return removeRow('Reg',5);" />
                            </td>
                        </tr>
                        <tr style="color: Black; background-color: rgb(255, 255, 238);">
                            <td colspan="3">
                                <div id="tdReg5" style="color: Red; font-family: Arial; font-size: 10px; float: right; padding-right: 30px;">
                                </div>
                            </td>
                        </tr>
                        <tr style="color: Black; background-color: rgb(255, 255, 238);">
                            <td colspan="4" style="text-align: right;"></td>
                        </tr>
                        <tr style="color: Black; background-color: rgb(255, 255, 238);">
                            <td align="center">
                                <asp:Label ID="lblRegTotal" runat="server" Text=""></asp:Label>
                            </td>
                            <td align="center"><b>Amount Payable / Invoice Amount(US $):</b>
                            </td>
                            <td align="center">
                                <b>
                                    <asp:Label ID="lblRegAmount" runat="server" Text=""></asp:Label></b>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                            <td align="right">
                                <asp:Button ID="btnRegGenerate" runat="server" Text="Generate Invoice" OnClientClick="javascript:return validateAmount();"
                                    ValidationGroup="grp1" OnClick="btnRegGenerate_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div style="display: none;">
            <input type="text" value='<%=strInvoices%>' id="txtRowCount" />
        </div>
    </form>
</body>

<script language="javascript" type="text/javascript">
    function ShowHide(ID) {
        if (ID == 'tblRegularInvoice') {
            document.getElementById("drpCost").style.display = "";
            document.getElementById('txtRowCount').value = 1;
            document.getElementById('tblRegularInvoice').style.display = '';
            document.getElementById('tblTMInvoice').style.display = 'none';
            for (var i = 2 ; i <= 5; i++) {
                try {
                    document.getElementById("trRegular" + i).style.display = "none";
                    document.getElementById('txtRegDescription' + i).value = '';
                    document.getElementById('txtRegTotalCost' + i).value = '';
                    document.getElementById('txtRegAmount' + i).value = '';
                }
                catch (e) {
                }
            }
        }
        else {
            document.getElementById("drpCost").style.display = "none";
            document.getElementById('txtRowCount').value = 1;
            document.getElementById('tblTMInvoice').style.display = '';
            document.getElementById('tblRegularInvoice').style.display = 'none';
            for (var i = 2 ; i <= 5; i++) {
                try {
                    document.getElementById("trTM" + i).style.display = "none";
                    document.getElementById('txtTMDescription' + i).value = '';
                    document.getElementById('txtTMRate' + i).value = '';
                    document.getElementById('txtTMHours' + i).value = '';
                    document.getElementById("payAmount" + i).value = '';
                }
                catch (e) {
                }
            }
        }
    }
        <%
    if (strProject == "True")
    {
        %>
    ShowHide('tblRegularInvoice');
        <%
        }
        else
        { 
        %>

    if (document.getElementById('rdRegular').checked)
        ShowRow("Regular");
    else
        ShowRow("TM");
        <%
        }
        %>
    if (document.getElementById('rdRegular').checked)
        calculateAmount();
    else
        calculateRegAmount();

</script>

</html>
