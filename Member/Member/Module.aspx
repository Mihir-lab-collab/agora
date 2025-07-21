<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="Module.aspx.cs" Inherits="Member_Module" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="../Member/js/Module.js" type="text/javascript"></script>
    <script type="text/javascript">
        var gridModule = "#gridModule";
        function GetDataOnInsert(Buttonid) {

            var ParentName = $("#drpParentName").val();
            var DisplayName = $("#txtDisplayName").val();
            var MenuName = $("#txtMenuName").val();
            var PageURL = $("#txtPageURL").val();
            var Parameter = $("#txtparam").val();
            var SortOrder = $("#txtSortOrder").val();

            var MenuNameSpan = $("#lblerrmsgMenuName");
            var DisplayNameSpan = $("#lblmsgDisplayName");

            if (MenuName == "") {
                MenuNameSpan.html("Please enter Menu Name.");
                return false;
            }
            else {
                MenuNameSpan.html("");
            }
            if (DisplayName == "") {
                DisplayNameSpan.html("Please enter Display Name.");
                return false;
            }
            else {
                DisplayNameSpan.html("");
            }

            if (MenuName != "" && DisplayName != "") {
                document.getElementById("<%=hdnParentID.ClientID%>").value = ParentName;
                document.getElementById("<%=hdnDisplayName.ClientID%>").value = DisplayName;
                document.getElementById("<%=hdnMenuName.ClientID%>").value = MenuName;
                document.getElementById("<%=hdnPageURL.ClientID%>").value = PageURL;
                document.getElementById("<%=hndParameter.ClientID%>").value = Parameter;
                document.getElementById("<%=hdnChkIsMenuVisible.ClientID%>").value = $('#chkVisible').is(':checked');
                document.getElementById("<%=hdnChkIsGeneric.ClientID%>").value = $('#chkGeneric').is(':checked');
                document.getElementById("<%=hdnSortOrder.ClientID%>").value = SortOrder;
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
                                <asp:Label ID="lblModuleDetails" Text="Module Master" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                            <td align="right">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblType" Text="Type:" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                                            <asp:DropDownList ID="ddlType" runat="server" Font-Size="Small" CssClass="b_dropdown" Width="100px" AppendDataBoundItems="true" AutoPostBack="true">
                                                <%-- <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>--%>
                                                <asp:ListItem Text="Employee" Value="e"></asp:ListItem>
                                                <asp:ListItem Text="Customer" Value="c"></asp:ListItem>

                                            </asp:DropDownList>
                                            <span id="spn" runat="server" onclick="ShowAddPopup();callEditor();" class="small_button white_button open">Add New Module</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="gridModule"></div>
            </div>
        </div>
    </div>


    <div id="divAddPopupOverlay" runat="server"></div>

    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 90px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Add Module</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>
            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                <tr>
                    <th>Parent Name
                    </th>
                    <td>
                        <input id="drpParentName" style="width: 300px" onblur="return GetDataOnInsert(this.id);" />
                        <%--  <span style="color: Red;">*</span>
                        <span id="lblmsgParentName" style="color: Red;"></span>--%>
                    </td>
                </tr>
                <tr>
                    <th>Menu Name
                    </th>
                    <td>
                        <input id="txtMenuName" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgMenuName" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>Display Name
                    </th>
                    <td>
                        <input id="txtDisplayName" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgDisplayName" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>Page URL
                    </th>
                    <td>
                        <input id="txtPageURL" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <%--  <span style="color: Red;">*</span>
                        <span id="lblerrmsgPageURL" style="color: Red;"></span>--%>
                    </td>
                </tr>

                <tr>
                    <th>Parameter
                    </th>
                    <td>
                        <input id="txtparam" type="text" style="width: 300px;" class="k-textbox" />
                    </td>
                </tr>
                <tr>
                    <th>Is Visible
                    </th>
                    <td align="left" valign="top">
                        <input id="chkVisible" type="checkbox" />
                    </td>
                </tr>
                <tr>
                    <th>Is Generic
                    </th>
                    <td align="left" valign="top">
                        <input id="chkGeneric" type="checkbox" />
                    </td>
                </tr>
                <tr>
                    <th>Sort Order
                    </th>
                    <td>
                        <input id="txtSortOrder" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <asp:Button ID="lnkSaveModule" runat="server" Text="Save" CssClass="small_button red_button open" OnClientClick="javascript:return GetDataOnInsert(this.id);" OnClick="lnkSaveModule_Click" />
                        &nbsp;&nbsp;&nbsp;<input type="button" class="small_button red_button open" value="Cancel" id="btnCancel" onclick="closeAddPopUP();" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hdnParentID" runat="server" Value="0" />
        <asp:HiddenField ID="hdnMenuName" runat="server" />
        <asp:HiddenField ID="hdnDisplayName" runat="server" />
        <asp:HiddenField ID="hdnPageURL" runat="server" />
        <asp:HiddenField ID="hndParameter" runat="server" />
        <asp:HiddenField ID="hdnChkIsMenuVisible" runat="server" />
        <asp:HiddenField ID="hdnChkIsGeneric" runat="server" />
        <asp:HiddenField ID="hdnSortOrder" runat="server" />
    </div>

    <script type="text/x-kendo-template" id="popup-editor">
    <div id="details-container">
        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
            <tr id="trModule" class="manage_bg">
            </tr>
            <tr>
                <th>Parent Name</th>
                <td><input  id="drpEditParentName" style="width: 300px"/></td>
            </tr>
            <tr>
                <th>Menu Name
                </th>
                <td>
                    <input id="txtEditMenuName" type="text" data-bind="value:Menu"  required validationmessage="Please enter Menu Name"  style="width: 300px;" class="k-textbox" />
                </td>
            </tr>
            <tr>
                <th>Display Name
                </th>
                <td>
                    <input id="txtEditDisplayName" type="text" data-bind="value:Name"  required validationmessage="Please enter Display Name"  style="width: 300px;" class="k-textbox" />
                </td>
            </tr>
            <tr>
                <th>Page URL
                </th>
                <td>
                    <input id="txtEditPageURL" type="text" data-bind="value:EntryPage"   style="width: 300px;" class="k-textbox" />
                </td>
            </tr>
            <tr>
                <th>Parameter
                </th>
                <td>
                    <input id="txtEdiparam" type="text" data-bind="value:Parameter" style="width: 300px;" class="k-textbox" />
                </td>
            </tr>

            <tr>
                <th>Is Visible</th>
                <td align="left" valign="top">
                    <input id="chkIsVisibleEdit" type="checkbox" name="chkIsVisibleEdit" />
                </td>
            </tr>
            <tr>
                <th>Is Generic</th>
                <td align="left" valign="top">
                    <input id="chkIsGenericEdit" type="checkbox" name="chkIsGenericEdit" />
                </td>
            </tr>
            <tr>
                <th>Sort Order
                </th>
                <td>
                    <input id="txtEditSortOrder" type="text" data-bind="value:SortOrder"  style="width: 300px;" class="k-textbox" />
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

