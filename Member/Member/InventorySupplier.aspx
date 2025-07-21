<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="InventorySupplier.aspx.cs" Inherits="Member_InventorySupplier" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="../Member/js/Supplier.js" type="text/javascript"></script>

    <script type="text/javascript">
        var gridSupplier = "#gridSupplier";

        function GetDataOnInsert(Buttonid) {
            var Name = $("#txtName").val();
            var City = $("#txtCity").val();
            var txtState = $("#txtState").val();
            var txtCountry = $("#drpCountry").val();
            var txtAddress = $("#txtAddress").val();
            var txtMobile = $("#txtMobile").val();
            var txtEmail = $("#txtEmail").val();

            var NameSpan = $("#lblerrmsgName");
            var CitySpan = $("#lblmsgCity");
            var StateSpan = $("#lblerrmsgState");
            var CountrySpan = $("#lblerrmsgCountry");
            var AddressSpan = $("#lblerrmsgAddress");
            var lblerrmsgMobile = $("#lblerrmsgMobile");
            var lblerrmsgEmail = $("#lblerrmsgEmail");

            if (Name == "") {
                NameSpan.html("Please enter Name.");
                return false;
            }
            else {
                NameSpan.html("");
            }
            if (txtCountry == "") {
                CountrySpan.html("Please select Country.");
                return false;
            }
            else {
                CountrySpan.html("");
            }
            if (txtState == "") {
                StateSpan.html("Please enter State.");
                return false;
            }
            else {
                StateSpan.html("");
            }
            if (City == "") {
                CitySpan.html("Please enter City.");
                return false;
            }
            else {
                CitySpan.html("");
            }
            if (txtAddress == "") {
                AddressSpan.html("Please enter Address.");
                return false;
            }
            else {
                AddressSpan.html("");
            }
            if (txtMobile == "") {
                lblerrmsgMobile.html("Please enter Mobile.");
                return false;
            }
            else {
                lblerrmsgMobile.html("");
            }
            if (txtEmail == "") {
                lblerrmsgEmail.html("Please enter Email.");
                return false;
            }
            else {
                lblerrmsgEmail.html("");
            }

            if (Name != "" && City != "" && txtState != "" && txtCountry != "" && txtAddress != "" && txtMobile != "" && txtEmail != "") {
                document.getElementById("<%=hdnName.ClientID%>").value = Name;
                 document.getElementById("<%=hdnCity.ClientID%>").value = City;
                 document.getElementById("<%=hdnState.ClientID%>").value = txtState;
                 document.getElementById("<%=hdnCountry.ClientID%>").value = txtCountry;
                 document.getElementById("<%=hdnAddress.ClientID%>").value = txtAddress;
                 document.getElementById("<%=hdnMobile.ClientID%>").value = txtMobile;
                 document.getElementById("<%=hdnEmail.ClientID%>").value = txtEmail;

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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <table width="100%">
                        <tr>
                            <td>
                                <span style="font-size: medium; font-weight: bold">Supplier Master</span>
                                <%-- <asp:Label ID="lblDetails" Text="Module Master" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>--%>
                            </td>
                            <td align="right">
                                <table>
                                    <tr>
                                        <td>
                                            <span id="spn" runat="server" onclick="ShowAddPopup();callEditor();" class="small_button white_button open">Add New Supplier</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="gridSupplier"></div>
            </div>
        </div>
    </div>

    <div id="divAddPopupOverlay" runat="server"></div>

    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 90px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Add Supplier</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>
            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                <tr>
                    <th>Name
                    </th>
                    <td>
                        <input id="txtName" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgName" style="color: Red;"></span>
                    </td>
                </tr>
                  <tr>
                    <th>Country
                    </th>
                    <td>
                         <input id="drpCountry" style="width: 300px" onblur="return GetDataOnInsert(this.id);" />
                       <%-- <input id="txtCountry" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />--%>
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgCountry" style="color: Red;"></span>
                    </td>
                </tr>
                  <tr>
                    <th>State
                    </th>
                    <td>
                        <input id="txtState" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgState" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>City
                    </th>
                    <td>
                        <input id="txtCity" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgCity" style="color: Red;"></span>
                    </td>
                </tr>
              
              
                <tr>
                    <th>Address
                    </th>
                    <td>
                        <textarea id="txtAddress" rows="4" cols="40" name="txtAddress" style="width: 300px; resize: none;" onblur="return GetDataOnInsert(this.id); " class="k-textbox"></textarea>
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgAddress" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>Mobile
                    </th>
                    <td>
                        <input id="txtMobile" type="text" style="width: 300px;" onkeypress="return isContact(this);" onkeyup="return isContact(this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgMobile" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>Email
                    </th>
                    <td>
                        <input id="txtEmail" type="text" style="width: 300px;" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgEmail" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>Sort Order
                    </th>
                    <td>
                        <input id="txtSortOrder" type="text" style="width: 300px;" onkeypress="return isContact(this);" onkeyup="return isContact(this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />

                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <asp:Button ID="lnkSaveSupplier" runat="server" Text="Save" CssClass="small_button red_button open" OnClientClick="javascript:return GetDataOnInsert(this.id);" OnClick="lnkSaveSupplier_Click" />
                        &nbsp;&nbsp;&nbsp;<input type="button" class="small_button red_button open" value="Cancel" id="btnCancel" onclick="closeAddPopUP();" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hdnSupID" runat="server" Value="0" />
        <asp:HiddenField ID="hdnName" runat="server" />
        <asp:HiddenField ID="hdnCity" runat="server" />
        <asp:HiddenField ID="hdnAddress" runat="server" />
        <asp:HiddenField ID="hdnState" runat="server" />
        <asp:HiddenField ID="hdnCountry" runat="server" />
        <asp:HiddenField ID="hdnMobile" runat="server" />
        <asp:HiddenField ID="hdnEmail" runat="server" />
        <asp:HiddenField ID="hdnSortOrder" runat="server" />
    </div>

    <script type="text/x-kendo-template" id="popup-editor">
    <div id="details-container">
        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
            <tr id="trSupplier" class="manage_bg">
            </tr>
            <tr>
                <th>Name
                </th>
                <td>
                    <input id="txtEditName" type="text" data-bind="value:Name" onkeypress="return isChar(event,this);" required validationmessage="Please enter Name"  style="width: 300px;" class="k-textbox" />
                </td>
            </tr>
           <tr>
                <th>Country
                </th>
                <td>
                 <input  id="drpEditCountry" style="width: 300px"/>
                    <%--<input id="txtEditCountry" type="text" data-bind="value:Country" onkeypress="return isChar(event,this);" required validationmessage="Please enter Country"  style="width: 300px;" class="k-textbox" />--%>
                </td>
            </tr>
             <tr>
                <th>State
                </th>
                <td>
                    <input id="txtEditState" type="text" data-bind="value:State" onkeypress="return isChar(event,this);" required validationmessage="Please enter State"  style="width: 300px;" class="k-textbox" />
                </td>
            </tr>
            <tr>
                <th>City
                </th>
                <td>
                    <input id="txtEditCity" type="text" data-bind="value:City" onkeypress="return isChar(event,this);" required validationmessage="Please enter City"  style="width: 300px;" class="k-textbox" />
                </td>
            </tr>
            <tr>
                <th>Address
                </th>
                <td>
                    <textarea id="txtEditAddress" rows="4" cols="40" name="txtEditAddress" data-bind="value:Address" style="width: 300px; resize: none;" required validationmessage="Please enter Address" class="k-textbox"></textarea>
                </td>
            </tr>
          
            <tr>
                <th>Mobile
                </th>
                <td>
                    <input id="txtEditMobile" type="text" data-bind="value:Mobile"  onkeypress="return isContact(this);" onkeyup="return isContact(this);"  required validationmessage="Please enter Mobile"  style="width: 300px;" class="k-textbox" />
                </td>
            </tr>
         <tr>
                <th>Email
                </th>
                <td>
                    <input id="txtEditEmail" type="text" data-bind="value:Email" onkeypress="return isChar(event,this);"  required validationmessage="Please enter Email"  style="width: 300px;" class="k-textbox" />
                </td>
            </tr>
       
         <tr>
                <th>Sort Order
                </th>
                <td>
                    <input id="txtEditSortOrder" type="text" data-bind="value:SortOrder" onkeypress="return isContact(this);" onkeyup="return isContact(this);"  required validationmessage="Please enter Sort Order"  style="width: 300px;" class="k-textbox" />
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

