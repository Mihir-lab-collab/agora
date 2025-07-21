<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="CustomerUser.aspx.cs" Inherits="Customer_CustomerUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script src="JS/CustomerUser.js" type="text/javascript"></script>

    <script type="text/javascript">        
        var CustId= <%=CurCustid %>   
    </script>
    <style>
        .k-textbox {
            width: 11.8em;
        }

        #tickets {
        }

            #tickets h3 {
                font-weight: normal;
                font-size: 1.4em;
                border-bottom: 1px solid #ccc;
            }

            #tickets ul {
                list-style-type: none;
                margin: 0;
                padding: 0;
            }

            #tickets li {
                margin: 10px 0 0 0;
            }

        label {
            display: inline-block;
            width: 90px;
            text-align: right;
        }

        .required {
            font-weight: bold;
        }

        .accept, .status {
            padding-left: 90px;
        }

        .valid {
            color: green;
        }

        .invalid {
            color: red;
        }

        span.k-tooltip {
            margin-left: 6px;
        }

        .note.error span {
            background: transparent url(../images/error.png) 0px 0px no-repeat;
        }

        .note.check span {
            background: transparent url(../images/check.png) 0px 0px no-repeat;
        }
    </style>
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
            margin: 0!important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }


        .k-grid-content > table > tbody > .k-alt {
            background: #eceaea;
            padding: 0 !important;
            margin: 0!important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }

        /*selection*/

        .k-grid table tr.k-state-selected {
            background: #f99;
            color: #fff;
        }
    </style>
    <script type="text/javascript">

        function CheckInsert() {   
           
            document.getElementById("<%=hfCustID.ClientID%>").value = CustId;
           
            document.getElementById("<%=hftxtFirstName.ClientID%>").value = $("#txtFirstName").val();
            
            document.getElementById("<%=hftxtLastName.ClientID%>").value = $("#txtLastName").val();
           
            document.getElementById("<%=hftxtEmail.ClientID%>").value = $("#txtEmail").val();
           
            document.getElementById("<%=hftxtContact.ClientID%>").value = $("#txtContact").val();
           
            document.getElementById("<%=hfchkIsAdmin.ClientID%>").value = $('#chkIsAdmin').is(':checked');
           
            document.getElementById("<%=hfsendmail.ClientID%>").value = $('#chksendmail').is(':checked');           
           
            return true;
        }

    </script>
    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblCusomerUser" Text="Manage Users - " runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>

                                <div style="float: right;">
                                    <span id="spncustomeruser" onclick="ShowAddPopup()" class="small_button white_button open">Add New User</span>
                                </div>

                            </td>
                        </tr>
                    </table>
                </div>
                <div id="gridCustomerUser"></div>
            </div>
        </div>
    </div>
    <script id="popup-editor" type="text/x-kendo-template">
    <table width="100%" cellpadding="10" cellspacing="0" class="manage_form">
            <tr>
                <th>First Name</th>
                <td>
                    <input id="txtFirstName" type="text" name="txtFirstName" data-bind="value:FName" style="width: 300px" required validationmessage="Please enter First Name" class="k-textbox" />
                </td>
            </tr>
            <tr>
                <th>Last Name</th>
                <td>
                    <input id="txtLastName" rows="4" cols="40" name="txtLastName" data-bind="value:LName" style="width: 300px; resize: none;" required validationmessage="Please enter Last Name" class="k-textbox"></input>
                </td>
            </tr>
            <tr>
                <th>Email ID</th>
                <td>
                    <input id="txtEmail" type="email" name="txtEmail" data-bind="value:Email" style="width: 300px" class="k-textbox" placeholder="e.g. myname@example.net" required validationmessage="Please enter Email" required data-email-msg="Email format is not valid" />
                </td>
            </tr>
            <tr>
                <th>Contact No</th>
                <td>
                    <input id="txtContact" type="text" name="txtContact" onkeyup="isContact(this)" onkeypress="isContact(this)" maxlength="15" data-bind="value:ContactNo" style="width: 300px" class="k-textbox" />
                </td>
            </tr>
                <tr>
                <th>Is Admin</th>
                <td align="left" valign="top">
                    <input id="chkIsAdminEdit" type="checkbox" name="chkIsAdminEdit" />
                </td>
            </tr>
       
                <tr>
                <th>Is Active</th>
                <td align="left" valign="top">       
                    <input id="chkIsActiveEdit" type="checkbox" name="chkIsActiveEdit" />
                </td>
            </tr>
            <tr>
                <th>Send Mail</th>
                <td align="left" valign="top">
                    <input id="chksendmailEdit" type="checkbox" name="chksendmailEdit" />
                </td>
            </tr>
                                  
   </table>
        <br/>         
    </script>
    <%--  PopUP Div Starts --%>
    <div id="divAddPopupOverlay"></div>
    <div class="a_popbox" id="divAddPopup" style="display: none;">
        <div class="popup_wrap" style="width: 600px; top: -30%; left: 30%;">
            <img src="../Images/delete_ic.png" alt="Close" class="close-button" onclick="closeAddPopUP()" />
            <table width="100%">
                <tr>
                    <td align="center">
                        <span id="Span1" style="font-size: large; font-weight: 100">Add New User for :  <%= CurCompanyName %></span>
                        <br />
                        <br />
                    </td>

                </tr>
            </table>

            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">

                <tr>

                    <td align="center">

                        <asp:Panel ID="pnlAddCustomerUser" runat="server">
                            <div id="example" class="k-content">
                                <div id="tickets">
                                    <table class="manage_form" width="100%">

                                        <tr>
                                            <th>First Name</th>
                                            <td>
                                                <input id="txtFirstName" type="text" name="txtFirstName" style="width: 300px" required validationmessage="Please enter First Name" class="k-textbox" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Last Name</th>
                                            <td>
                                                <input id="txtLastName" rows="4" cols="40" name="txtLastName" style="width: 300px; resize: none;" required validationmessage="Please enter Last Name" class="k-textbox"></input>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Email ID</th>
                                            <td>
                                                <input id="txtEmail" type="email" name="txtEmail" style="width: 300px" class="k-textbox" placeholder="e.g. myname@example.net" required validationmessage="Please enter Email" required data-email-msg="Email format is not valid" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Contact No</th>
                                            <td>
                                                <input id="txtContact" type="text" name="txtContact" onkeyup="isContact(this)" onkeypress="isContact(this)" maxlength="15" style="width: 300px" class="k-textbox" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Is Admin</th>
                                            <td align="left" valign="top">
                                                <input id="chkIsAdmin" type="checkbox" name="chkIsAdmin" checked="checked" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Send Mail</th>
                                            <td align="left" valign="top">
                                                <input id="chksendmail" type="checkbox" name="chksendmail" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th></th>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnAddCustomerUser" runat="server" CssClass="small_button white_button open" CausesValidation="true" OnClientClick="javascript:return CheckInsert() " Text="Add" OnClick="btnAddCustomerUser_Click" />
                                                        </td>

                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>
                    </td>

                </tr>
            </table>
        </div>
    </div>
    <%--  PopUP Div Ends Here --%>

    <asp:HiddenField ID="hfCustID" runat="server" />
    <asp:HiddenField ID="hftxtFirstName" runat="server" />
    <asp:HiddenField ID="hftxtLastName" runat="server" />
    <asp:HiddenField ID="hftxtEmail" runat="server" />
    <asp:HiddenField ID="hftxtContact" runat="server" />
    <asp:HiddenField ID="hfchkIsAdmin" runat="server" />
    <asp:HiddenField ID="hfsendmail" runat="server" />
    <asp:HiddenField ID="hfCompanyName" runat="server" />
</asp:Content>
