<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="InventoryInvoiceDetails.aspx.cs" Inherits="Member_InventoryInvoiceDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="../Member/css/Autocomplete_textbox.css" type="text/css" />
    <link rel="stylesheet" type="text/css"  />                                           <%--commented by AP--%>        <%--href="http://cdn.webrupee.com/font"--%>
    <script src="../js/CommonFunc.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        //Added document ready function by nikhil shetye on 12/10/2017
        $(document).ready(function () {
            $('.ChkIsNumber').keypress(function (event) {
                return isNumber(event, this)
            });
        });
        function GetBrandID(source, eventArgs)
        {
            var _strCoCode = eval(eventArgs.get_value());
            document.getElementById('<%= hdnAutoBrandID.ClientID %>').value = _strCoCode;
        }

        function GetSupplierID(source, eventArgs)
        {
            var strCoCode = eval(eventArgs.get_value());
            document.getElementById('<%= hdnAutoSupplierID.ClientID %>').value = strCoCode;
        }

        function CalculateAmount()
        {
            var amount = parseFloat(($('#<%= lblTaxExemptAmount.ClientID %>').html()));
           
            <%--var vat = parseFloat($('#<%=txtVat.ClientID%>').val());--%> //Commented By Nikhil Shetye for GST implementation
            var vat = parseFloat($('#<%=txtCGST.ClientID%>').val());
            //added by AP
            <%--var serviceTax = parseFloat($('#<%=txtServiceTax.ClientID%>').val());--%> //Commented By Nikhil Shetye for GST implementation
            var serviceTax = parseFloat($('#<%=txtSGST.ClientID%>').val());
            var vatAmount = parseFloat(amount * (vat / 100)).toFixed(2);
            var serviceAmount = parseFloat(amount * (serviceTax / 100)).toFixed(2);
            var total = amount + parseFloat(vatAmount)+parseFloat(serviceAmount);
            <%--$('#<%= lblAmount.ClientID %>').html(vatAmount + "&nbsp;&nbsp;&nbsp;<b>Service Tax Amount: </b> " + serviceAmount + "&nbsp;&nbsp;&nbsp;<b>Total Amount: </b> " + parseFloat(total).toFixed(2));--%> //Commented By Nikhil Shetye for GST implementation
            $('#<%= lblAmount.ClientID %>').html(vatAmount + "&nbsp;&nbsp;&nbsp;<b>SGST Amount: </b> " + serviceAmount + "&nbsp;&nbsp;&nbsp;<b>Total Amount: </b> " + parseFloat(total).toFixed(2));
        }
        //Added isNumber function by nikhil shetye on 12/10/2017
        function isNumber(evt, element) {

            var charCode = (evt.which) ? evt.which : event.keyCode

            if (
                //(charCode != 45 || $(element).val().indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
                (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
                (charCode < 48 || charCode > 57))
                return false;

            return true;
        }  
    </script>

    <asp:ScriptManager runat="server" ID="scriptManageInvoice"></asp:ScriptManager>
    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <asp:Label ID="Label1" Text="Invoice Details" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <asp:Label ID="lblItemCategoryName" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                </div>
 <asp:Panel ID="pnlAddUpdate" runat="server">
 <asp:HiddenField ID="hdnItemInvoiceID" runat="server" Value="0" />
                   
<asp:Table ID="Table1" runat="server" Width="100%">

    <asp:TableRow>
        <asp:TableHeaderCell>
                Invoice No
        </asp:TableHeaderCell>
        <asp:TableCell >
                <asp:TextBox ID="InvoiceNo" runat="server" CssClass="a_textbox">
                </asp:TextBox>
                <span style="color: red;">*</span>
        <br />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldInvoiceNo" ValidationGroup="Inventory" ControlToValidate="InvoiceNo" ForeColor="Red" ErrorMessage="Enter invoice no." Display="Dynamic">
                </asp:RequiredFieldValidator>
        </asp:TableCell>

        <asp:TableHeaderCell>
                Invoice Date
        </asp:TableHeaderCell>
        <asp:TableCell>
                <asp:TextBox ID="txtPurchaseDate" runat="server" CssClass="a_textbox">
                </asp:TextBox>
                <span style="color: red;">*</span>
        <br />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldtxtPurchaseDate" ValidationGroup="Inventory" ControlToValidate="txtPurchaseDate" ForeColor="Red" ErrorMessage="Enter Invoice date." Display="Dynamic">
                </asp:RequiredFieldValidator>
                <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtPurchaseDate" CssClass="cal_Theme1" Format="dd/MM/yyyy">
                </ajax:CalendarExtender>
       </asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
         <asp:TableHeaderCell >
                Note
         </asp:TableHeaderCell>
         <asp:TableCell>
                <asp:TextBox ID="Note" runat="server" TextMode="MultiLine" Height="50px" Width="250px"></asp:TextBox>
         </asp:TableCell>
         <asp:TableHeaderCell>
               Supplier
         </asp:TableHeaderCell>
          <asp:TableCell>
                <asp:HiddenField ID="hdnAutoSupplierID" runat="server" Value="0" />
                <asp:TextBox ID="txtSupplier" runat="server" ToolTip="Select Supplier" CssClass="a_textbox"></asp:TextBox>
                <span style="color: red;">*</span>
                <ajax:AutoCompleteExtender ID="ajaxSupplier" TargetControlID="txtSupplier"
                                    ServiceMethod="GetCompletionSupplierList"
                                    MinimumPrefixLength="1"
                                    CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                                    CompletionListCssClass="autocomplete_completionListElement"
                                    CompletionListItemCssClass="autocomplete_listItem"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    ShowOnlyCurrentWordInCompletionListItem="true"
                                    FirstRowSelected="false" OnClientItemSelected="GetSupplierID"
                                    runat="server">
                 </ajax:AutoCompleteExtender>
         <br />
                 <asp:RequiredFieldValidator runat="server" ID="rfvtxtSupplier" ControlToValidate="txtSupplier" ValidationGroup="Inventory" ForeColor="Red" ErrorMessage="Enter supliier name." Display="Dynamic"></asp:RequiredFieldValidator>
         </asp:TableCell>
    </asp:TableRow>

   <%-- <asp:TableRow>
         <asp:TableHeaderCell>
                  Vat
         </asp:TableHeaderCell>
         <asp:TableCell>
                  <asp:TextBox ID="txtVat" runat="server" CssClass="d_textbox" Text="5.0" MaxLength="8"  onkeypress="return isNumberKeyOrDot(event)" onblur="javascript:CalculateAmount();" />
                          (%)
                  </asp:TableCell>
        <asp:TableHeaderCell>
                Vat Amount
        </asp:TableHeaderCell>
        <asp:TableCell>
                <asp:Label ID="lblAmount" runat="server" />
                <asp:Label ID="lblTaxExemptAmount" runat="server" Style="display: none;" Text="0.0" />
        </asp:TableCell>
   </asp:TableRow>

    <asp:TableRow>
         <asp:TableHeaderCell>
                 Service Tax
         </asp:TableHeaderCell>
         <asp:TableCell>
                  <asp:TextBox ID="txtServiceTax" runat="server" CssClass="d_textbox" Text="14.5" MaxLength="8" onkeypress="return isNumberKeyOrDot(event)" onblur="javascript:CalculateAmount();" />
                         (%)
         </asp:TableCell>
    </asp:TableRow>--%>

    <%--Added By Nikhil Shetye on 12/10/2017 for GST implementation--%>
                        <asp:TableRow>
         <asp:TableHeaderCell>
                  CGST 
         </asp:TableHeaderCell>
         <asp:TableCell>
                  <asp:TextBox ID="txtCGST" runat="server" CssClass="d_textbox ChkIsNumber" Text="9.0" MaxLength="8"  onkeypress="return isNumberKeyOrDot(event)" onblur="javascript:CalculateAmount();" />
                          (%)
                  </asp:TableCell><asp:TableHeaderCell>
                CGST Amount
        </asp:TableHeaderCell><asp:TableCell>
                <asp:Label ID="lblAmount" runat="server" />
                <asp:Label ID="lblTaxExemptAmount" runat="server" Style="display: none;" Text="0.0" />
        </asp:TableCell></asp:TableRow><asp:TableRow>
         <asp:TableHeaderCell>
                 SGST
         </asp:TableHeaderCell><asp:TableCell>
                  <asp:TextBox ID="txtSGST" runat="server" CssClass="d_textbox ChkIsNumber" Text="9.0" MaxLength="8" onkeypress="return isNumberKeyOrDot(event)" onblur="javascript:CalculateAmount();" />
                         (%)
         </asp:TableCell></asp:TableRow><%--Nikhil Shetye End--%><asp:TableRow>
          <asp:TableCell ColumnSpan="4">
                <div id="Div1" runat="server" style="text-align: right;">
                     <asp:Button ID="btnEditUpdateSave" runat="server" CausesValidation="true" Text="SAVE" ValidationGroup="Inventory" CssClass="small_button white_button open" OnClick="btnEditUpdateSave_Click" />
                     <asp:Button ID="BtnBack" runat="server" Text="Back" OnClick="BtnBack_Click" Visible="true" CausesValidation="false" CssClass="small_button white_button open" />
                </div>
          </asp:TableCell></asp:TableRow><asp:TableRow BackColor="Gray" Visible="false" ID="tbRow4">
          <asp:TableHeaderCell>
                   Category
          </asp:TableHeaderCell><asp:TableCell>
                   <asp:DropDownList CssClass="a_textbox" ID="CategoryName" runat="server" OnSelectedIndexChanged="CategoryName_SelectedIndexChanged" AutoPostBack="true">
                   </asp:DropDownList>
          </asp:TableCell><asp:TableCell ColumnSpan="4">
                    <asp:Button ID="btnAdd" Text="ADD" runat="server" CssClass="small_button white_button open" CausesValidation="false" OnClick="btnAdd_Click" />
          </asp:TableCell></asp:TableRow></asp:Table><asp:Panel runat="server" ID="panelItemDetails" Visible="false">
                        <table id="tdGrid" width="100%" runat="server" class="manage_form">
                            <tr>
                                <th width="10%">Brand</th><td width="40%">
                                    <div style="margin-top: -35px;">
                                        <asp:TextBox ID="txtBrand" runat="server" ToolTip="Select Brand" CssClass="a_textbox"></asp:TextBox><span style="color: red;">*</span> <asp:RequiredFieldValidator runat="server" ID="rfvtxtBrand" ControlToValidate="txtBrand" ValidationGroup="I" ForeColor="Red" ErrorMessage="Enter brand name." Display="Dynamic"></asp:RequiredFieldValidator></div><asp:HiddenField ID="hdnAutoBrandID" runat="server" Value="0" />
                                    <ajax:AutoCompleteExtender ID="ajxBrand" TargetControlID="txtBrand"
                                        ServiceMethod="GetCompletionBrandList"
                                        MinimumPrefixLength="1"
                                        CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                                        CompletionListCssClass="autocomplete_completionListElement"
                                        CompletionListItemCssClass="autocomplete_listItem"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        ShowOnlyCurrentWordInCompletionListItem="true"
                                        FirstRowSelected="false" OnClientItemSelected="GetBrandID"
                                        runat="server">
                                    </ajax:AutoCompleteExtender>
                                    <br />
                                </td>
                                <th align="right" width="10%">Quantity</th><td width="40%">
                                    <div>
                                        <asp:TextBox ID="txtQuantity1" runat="server" CssClass="a_textbox"></asp:TextBox><span style="color: red;">*</span> <asp:RequiredFieldValidator runat="server" ID="rfvtxtQuantity1" ValidationGroup="I" ControlToValidate="txtQuantity1" ForeColor="Red" ErrorMessage="Enter quantity." Display="Dynamic"></asp:RequiredFieldValidator><asp:CompareValidator ID="cvtxtQuantity1" ControlToValidate="txtQuantity1" Operator="DataTypeCheck" Type="Integer" runat="server" ErrorMessage="Enter whole number" ForeColor="Red"> </asp:CompareValidator></div><br /><br /></td></tr><tr>
                                <th>Price <span class='WebRupee'>Rs</span></th><td>
                                    <div>
                                        <asp:TextBox ID="txtPrice" runat="server" TextMode="SingleLine" CssClass="a_textbox"></asp:TextBox><span style="color: red;">*</span> <asp:RequiredFieldValidator runat="server" ID="rfvtxtPrice" ValidationGroup="I" ControlToValidate="txtPrice" ForeColor="Red" ErrorMessage="Enter price" Display="Dynamic"></asp:RequiredFieldValidator></div><br /><br /><asp:CompareValidator ID="cvtxtPrice" ControlToValidate="txtPrice" Operator="DataTypeCheck" Type="Double" ErrorMessage="Enter price." Display="Dynamic" runat="server" ForeColor="Red"></asp:CompareValidator></td><th align="right">Serial No</th><td>
                                    <div style="margin-top: -35px;">
                                        <asp:TextBox ID="txtSerialNo" runat="server" CssClass="a_textbox"></asp:TextBox><span style="color: red;">*</span> <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="I" ControlToValidate="txtSerialNo" ForeColor="Red" ErrorMessage="Enter serial no" Display="Dynamic"></asp:RequiredFieldValidator></div><br /></td></tr><tr>
                                <th>Expire in</th><td>
                                    <div class="tdbox">
                                        <asp:Label ID="Label9" runat="server" Text="Year"></asp:Label><br /><asp:TextBox ID="txtYear" runat="server" CssClass="c_textbox"></asp:TextBox><br /><asp:CompareValidator ID="CompareValidator1" ControlToValidate="txtYear" Operator="DataTypeCheck" Type="Integer" runat="server" ErrorMessage="Enter whole number" ForeColor="Red"> </asp:CompareValidator></div><div class="tdbox">
                                        <asp:Label ID="Label2" runat="server" Text="Month"></asp:Label><br /><asp:TextBox ID="txtMonth" runat="server" CssClass="c_textbox"></asp:TextBox><br /><asp:CompareValidator ID="CompareValidator2" ControlToValidate="txtMonth" Operator="DataTypeCheck" Type="Integer" runat="server" ErrorMessage="Enter whole number" ForeColor="Red"> </asp:CompareValidator></div><div class="tdbox">
                                        <asp:Label ID="Label3" runat="server" Text="Day"></asp:Label><br /><asp:TextBox ID="txtDay" runat="server" CssClass="c_textbox"></asp:TextBox><br /><asp:CompareValidator ID="CompareValidator3" ControlToValidate="txtDay" Operator="DataTypeCheck" Type="Integer" runat="server" ErrorMessage="Enter whole number" ForeColor="Red"> </asp:CompareValidator></div></td><th>Description</th><td>
                                    <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Height="50px" Width="250px"></asp:TextBox></td></tr><tr>
                                <th>Chalan Date </th><td>
                                    <div>
                                        <asp:TextBox ID="txtpurdate" runat="server" TextMode="SingleLine" CssClass="a_textbox"></asp:TextBox><span style="color: red;">*</span> <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtpurdate" ForeColor="Red" ErrorMessage="Enter Purchase date" Display="Dynamic"></asp:RequiredFieldValidator></div><br /><br /><ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtpurdate" CssClass="cal_Theme1" Format="dd/MM/yyyy"></ajax:CalendarExtender>

                                </td>
                                <th align="right"></th>
                                <td></td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvItemDetails" runat="server" AutoGenerateColumns="False" CssClass="manage_grid_a" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Category Attributes" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50%">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfValuesMain" runat="server" Value='<%#Eval("DefaultValue") %>' />
                                        <asp:Label ID="lblID" runat="server" Visible="false" Text='<%#Eval("AttributeID") %>'></asp:Label><asp:Label ID="lblAtrributeName" runat="server" Text='<%#Eval("Name") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Attribute Values" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50%">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfValues" runat="server" Value='<%#Eval("DefaultValue") %>' />
                                    </ItemTemplate>
                                    <ItemTemplate>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div class="grid_head">
                            <table align="right">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" CssClass="small_button white_button open" Text="SAVE" OnClick="btnSave_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCancel" runat="server" CssClass="small_button white_button open" Text="CANCEL" OnClick="btnCancel_Click" CausesValidation="false" />
                                    </td>

                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                
</asp:Panel>
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
               
<asp:Panel runat="server" ID="panelitemlist">
                    <asp:GridView ID="gvItemList" runat="server" Width="100%"
                        CellPadding="0" AutoGenerateColumns="False" ShowFooter="true" CssClass="manage_grid"
                        AllowSorting="true" OnRowEditing="gvItemList_RowEditing">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnEdit" CommandName="Edit" runat="server" ImageUrl="~/Member/images/edit.png" ToolTip="Edit" Height="20" Width="20" CommandArgument='<%#Eval("ItemID") %>' CausesValidation="false" OnClick="imgbtnEdit_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Category" ItemStyle-HorizontalAlign="Center" SortExpression="CategoryName">
                                <ItemTemplate>
                                    <asp:Label ID="lblCategoryName" runat="server" Text='<%#Eval("CategoryName") %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Brand" ItemStyle-HorizontalAlign="Center" SortExpression="Brand">
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%#Eval("BrandName") %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Supplier" ItemStyle-HorizontalAlign="Center" SortExpression="Supplier">
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%#Eval("Supplier") %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Price" ItemStyle-HorizontalAlign="Right" SortExpression="Price">
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# CSCode.Global.GetCurrencyFormat(Convert.ToDouble(Eval("Price"))) %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Center" SortExpression="Quantity">
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%#Eval("Quantity") %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="ExpDate" ItemStyle-HorizontalAlign="Center" SortExpression="ExpDate">
                                <ItemTemplate>
                                    <asp:Label ID="Label7" runat="server" Text='<%#((DateTime)Eval("ExpiryDate")).ToString ("dd/MM/yyyy") %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Description" ItemStyle-HorizontalAlign="Left" SortExpression="Description">
                                <ItemTemplate>
                                    <asp:Label ID="Label8" runat="server" Text='<%#Eval("Description") %>'></asp:Label></ItemTemplate></asp:TemplateField></Columns></asp:GridView></asp:Panel></div></div></div></asp:Content>