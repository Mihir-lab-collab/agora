<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="Location.aspx.cs" Inherits="Member_Location" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="../Member/js/Location.js" type="text/javascript"></script>
  
     <script type="text/javascript">
         var gridLocation = "#gridLocation";

         function GetDataOnInsert(Buttonid) {
             var LocationName = $("#txtLocationName").val();
             var City = $("#drpCity").val();
             var CompanyName = $("#txtCompanyName").val();
             var CompanyAddress = $("#txtCompanyAddress").val();
             var PhoneNo = $("#txtPhoneNo").val();
             var Fax = $("#txtFax").val();
             var Logo = $("#txtLogo").val();
             var txtBank = $("#txtBank").val();
             var txtBankAccount = $("#txtBankAccount").val();
             var txtWireDetail = $("#txtWireDetail").val();
             var txtKeyword = $("#txtKeyword").val();
             var InvoicePDFConfigID = $("#txtInvoicePDFConfigID").val();

             var LocationNameSpan = $("#lblerrmsgLocationName");
             //var CitySpan = $("#lblmsgCity");
             var CompanyNameSpan = $("#lblerrmsgCompanyName");
             var CompanyAddressSpan = $("#lblerrmsgCompanyAddress");
             var PhoneNoSpan = $("#lblerrmsgPhoneNo");
             //var txtFaxSpan = $("#lblerrmsgFax");
             var lblerrmsgBank = $("#lblerrmsgBank");
             var lblerrmsgBankAccount = $("#lblerrmsgBankAccount");
             var lblerrmsgWireDetails = $("#lblerrmsgWireDetails");
             var lblerrmsgKeyword = $("#lblerrmsgKeyword");

             if (LocationName == "") {
                 LocationNameSpan.html("Please enter Location Name.");
                 return false;
             }
             else {
                 LocationNameSpan.html("");
             }
             if (City == "") {
                 City = "0"
                 //CitySpan.html("Please select City.");
                 //return false;
             }
             //else {
             //    CitySpan.html("");
             //}
             if (CompanyName == "") {
                 CompanyNameSpan.html("Please enter Company Name.");
                 return false;
             }
             else {
                 CompanyNameSpan.html("");
             }
             if (txtKeyword == "") {
                 lblerrmsgKeyword.html("Please enter Keyword.");
                 return false;
             }
             else {
                 var isExist = CheckExistsKeyword(txtKeyword, 0);
                 if (isExist == true) {
                     lblerrmsgKeyword.html("Keyword already Exists.");
                     return false;
                 }
                 else {
                     lblerrmsgKeyword.html("");
                 }
             }

             if (CompanyAddress == "") {
                 CompanyAddressSpan.html("Please enter Company Address.");
                 return false;
             }
             else {
                 CompanyAddressSpan.html("");
             }
             if (PhoneNo == "") {
                 PhoneNoSpan.html("Please enter Phone Number.");
                 return false;
             }
             else {
                 PhoneNoSpan.html("");
             }
             if (Fax == "") {
                 Fax = null;
                 //txtFaxSpan.html("Please enter Fax.");
                 //return false;
             }
             else {
             }

             if (txtBank == "") {
                 lblerrmsgBank.html("Please enter Bank.");
                 return false;
             }
             else {
                 lblerrmsgBank.html("");
             }
             if (txtBankAccount == "") {
                 lblerrmsgBankAccount.html("Please enter Bank Account.");
                 return false;
             }
             else {
                 lblerrmsgBankAccount.html("");
             }
             if (txtWireDetail == "") {
                 lblerrmsgWireDetails.html("Please enter Wire Detail.");
                 return false;
             }
             else {
                 lblerrmsgWireDetails.html("");
             }
             if (InvoicePDFConfigID == "") {
                 InvoicePDFConfigID = "0";
             }

             if (LocationName != "" && City != "" && CompanyName != "" && txtKeyword != "" && CompanyAddress != "" && PhoneNo != "" && Fax != "" && txtBank != "" && txtBankAccount != "" && txtWireDetail != "" && InvoicePDFConfigID != "") {
                 document.getElementById("<%=hdnLocName.ClientID%>").value = LocationName;
                 document.getElementById("<%=hdnCityId.ClientID%>").value = City;
                 document.getElementById("<%=hdnCompanyName.ClientID%>").value = CompanyName;
                 document.getElementById("<%=hdnCompanyAdd.ClientID%>").value = CompanyAddress;
                 document.getElementById("<%=hdnChkBiometric.ClientID%>").value = $('#chkgeneric').is(':checked');
                 document.getElementById("<%=hdnPhoneNo.ClientID%>").value = PhoneNo;
                 document.getElementById("<%=hdnFax.ClientID%>").value = Fax;
                 document.getElementById("<%=hdnLogo.ClientID%>").value = Logo;
                 document.getElementById("<%=hdnBank.ClientID%>").value = txtBank;
                 document.getElementById("<%=hdnBankAccount.ClientID%>").value = txtBankAccount;
                 document.getElementById("<%=hdnWireDetails.ClientID%>").value = txtWireDetail;
                 document.getElementById("<%=hdnKeyword.ClientID%>").value = txtKeyword;
                 document.getElementById("<%=hdnInvoicePDFConfigID.ClientID%>").value = InvoicePDFConfigID;
                 return true;
             }
         }

     </script>
    <style type="text/css">
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

        .DivShowEditor, .DivHideEditor {
            float: right;
            margin-right: 20px;
            cursor: pointer;
        }
        /*for history grid [e]*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <table width="100%">
                        <tr>
                            <td>
                                <span style ="font-size:medium;font-weight:bold">Location Master</span>
                            </td>
                            <td align="right">
                                <table>
                                    <tr>
                                        <td>
                                            <span id="spn" runat="server" onclick="ShowAddPopup();callEditor();" class="small_button white_button open">Add New Location</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="gridLocation"></div>
            </div>
        </div>
    </div>


    <div id="divAddPopupOverlay" runat="server"></div>

    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 90px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Add Location</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>
            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
             
                <tr>
                    <th>Location Name
                    </th>
                    <td>
                        <input id="txtLocationName" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgLocationName" style="color: Red;"></span>
                    </td>
                </tr>
                   <tr>
                    <th>City
                    </th>
                    <td>
                        <input id="drpCity" style="width: 300px" onblur="return GetDataOnInsert(this.id);" />
                        <%--<span style="color: Red;">*</span>
                        <span id="lblmsgCity" style="color: Red;"></span>--%>
                    </td>
                </tr>
                 <tr>
                    <th>Biometric
                    </th>
                   <td align="left" valign="top">
                        <input id="chkBiometric" type="checkbox" />
                    </td>
                </tr>
          
                <tr>
                    <th>Company Name
                    </th>
                    <td>
                        <input id="txtCompanyName" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgCompanyName" style="color: Red;"></span>
                    </td>
                </tr> 
                <tr>
                    <th>Keyword
                    </th>
                    <td>
                        <input id="txtKeyword" type="text" style="width: 300px;" maxlength="4" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgKeyword" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>Company Address
                    </th>
                    <td>
                        <textarea id="txtCompanyAddress" rows="4" cols="40" name="txtAddress" style="width: 300px; resize: none;" onblur="return GetDataOnInsert(this.id); " class="k-textbox"></textarea>
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgCompanyAddress" style="color: Red;"></span>
                    </td>
                </tr>
               
                 <tr>
                    <th>Phone Number
                    </th>
                    <td>
                        <input id="txtPhoneNo" type="text" style="width: 300px;" onkeypress="return isContact(this);" onkeyup="return isContact(this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgPhoneNo" style="color: Red;"></span>
                    </td>
                </tr>
                 <tr>
                    <th>Fax
                    </th>
                    <td>
                        <input id="txtFax" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                       <%-- <span style="color: Red;">*</span>
                        <span id="lblerrmsgFax" style="color: Red;"></span>--%>
                    </td>
                </tr>
                 <tr>
                    <th>Logo
                    </th>
                    <td>
                       <input id="txtLogo" type="text" style="width: 300px;" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgLogo" style="color: Red;"></span>
                    </td>
                </tr>
                 <tr>
                    <th>Bank
                    </th>
                    <td>
                        <textarea id="txtBank" rows="4" cols="40" name="txtAddress" style="width: 300px; resize: none;" onblur="return GetDataOnInsert(this.id); " class="k-textbox"></textarea>
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgBank" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>Bank Account
                    </th>
                    <td>
                        <input id="txtBankAccount" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgBankAccount" style="color: Red;"></span>
                    </td>
                </tr>
                 <tr>
                    <th>WireDetail 
                    </th>
                    <td>
                        <textarea id="txtWireDetail" rows="4" cols="40" name="txtAddress" style="width: 300px; resize: none;" onblur="return GetDataOnInsert(this.id); " class="k-textbox"></textarea>
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgWireDetails" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>PDF ConfigID
                    </th>
                    <td>
                        <input id="txtInvoicePDFConfigID" type="text" style="width: 300px;" onkeypress="return isContact(this);" onkeyup="return isContact(this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <asp:Button ID="lnkSaveLocation" runat="server" Text="Save" CssClass="small_button red_button open" OnClientClick="javascript:return GetDataOnInsert(this.id);" OnClick="lnkSaveLocation_Click" />
                        &nbsp;&nbsp;&nbsp;<input type="button" class="small_button red_button open" value="Cancel" id="btnCancel" onclick="closeAddPopUP();" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hdnLocID" runat="server" Value="0"/>
        <asp:HiddenField ID="hdnLocName" runat="server" />
        <asp:HiddenField ID="hdnCityId" runat="server" />
         <asp:HiddenField ID="hdnChkBiometric" runat="server" />
         <asp:HiddenField ID="hdnCompanyName" runat="server" />
         <asp:HiddenField ID="hdnCompanyAdd" runat="server" />
         <asp:HiddenField ID="hdnPhoneNo" runat="server" />
         <asp:HiddenField ID="hdnFax" runat="server" />
         <asp:HiddenField ID="hdnLogo" runat="server" />
         <asp:HiddenField ID="hdnBank" runat="server" />
         <asp:HiddenField ID="hdnBankAccount" runat="server" />
        <asp:HiddenField ID="hdnWireDetails" runat="server" />
         <asp:HiddenField ID="hdnKeyword" runat="server" />
         <asp:HiddenField ID="hdnInvoicePDFConfigID" runat="server" />
    </div>

    <script type="text/x-kendo-template" id="popup-editor">
    <div id="details-container">
        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
            <tr id="trLocation" class="manage_bg">
            </tr>
          
            <tr>
                <th>Location Name
                </th>
                <td>
                    <input id="txtEditLocationName" type="text" data-bind="value:Name" onkeypress="return isChar(event,this);" required validationmessage="Please enter Location Name"  style="width: 300px;" class="k-textbox" />
                </td>
            </tr>
            <tr>
                <th>City</th>
                <td><input  id="drpEditCity" style="width: 300px"/></td>
            </tr>
            <tr>
                <th>Biometric</th>
                <td align="left" valign="top">
                    <input id="chkBiometricEdit" type="checkbox" name="chkBiometricEdit" />
                </td>
            </tr>
            <tr>
                <th>Company Name
                </th>
                <td>
                    <input id="txtEditCompanyName" type="text" data-bind="value:LegalName" onkeypress="return isChar(event,this);"  required validationmessage="Please enter Company Name"  style="width: 300px;" class="k-textbox" />
                </td>
            </tr>
            <tr>
                <th>Keyword
                </th>
                <td>
                    <input id="txtEditKeyword" type="text" data-bind="value:Keyword" name="Keyword" maxlength="4" onkeypress="return isChar(event,this);" required validationmessage="Please enter Keyword"  style="width: 300px;" class="k-textbox" />
                </td>
            </tr>
            <tr>
                <th>Company Address
                </th>
                <td>
                    <textarea id="txtEditCompanyAddress" rows="4" cols="40" name="txtEditCompanyAddress" data-bind="value:Address" style="width: 300px; resize: none;" required validationmessage="Please enter Company Address" class="k-textbox"></textarea>
                </td>
            </tr>
          
            <tr>
                <th>Phone Number
                </th>
                <td>
                    <input id="txtEditPhoneNo" type="text" data-bind="value:PhoneNo"  onkeypress="return isContact(this);" onkeyup="return isContact(this);"  required validationmessage="Please enter Phone Number"  style="width: 300px;" class="k-textbox" />
                </td>
            </tr>
         <tr>
                <th>Fax
                </th>
                <td>
                    <input id="txtEditFax" type="text" data-bind="value:Fax"  style="width: 300px;" class="k-textbox" />
                </td>
            </tr>
            <tr>
                    <th>Logo </th>
                    <td>
                        <input id="txtEditLogo" type="text" data-bind="value:Logo"  required validationmessage="Please select Logo"  style="width: 300px;" class="k-textbox" />
                    </td>
                </tr>
             <tr>
                <th>Bank Address
                </th>
                <td>
                    <textarea id="txtEditBank" rows="4" cols="40" name="txtEditBank" data-bind="value:Bank" style="width: 300px; resize: none;" required validationmessage="Please enter Bank." class="k-textbox"></textarea>
                </td>
            </tr>
         <tr>
                <th>Bank Account
                </th>
                <td>
                    <input id="txtEditBankAccount" type="text" data-bind="value:BankAccount" onkeypress="return isChar(event,this);" required validationmessage="Please enter Bank Account"  style="width: 300px;" class="k-textbox" />
                </td>
            </tr>
         <tr>
                <th>Wire Details
                </th>
                <td>
                    <textarea id="txtEditWireDetails" rows="4" cols="40" name="txtEditWireDetails" data-bind="value:WireDetail" style="width: 300px; resize: none;" required validationmessage="Please enter Wire Detail" class="k-textbox"></textarea>
                </td>
            </tr>
        <tr>
                <th>PDF ConfigID
                </th>
                <td>
                    <input id="txtEditInvoicePDFConfigID" type="text" data-bind="value:txtEditInvoicePDFConfigID"   style="width: 300px;" class="k-textbox" />
                </td>
            </tr>
            <tr>
                <th></th>
                <td>
                    <div id="tdUpdate"></div>
                </td>
            </tr>
        </table>
    </div>
    </script>
</asp:Content>

