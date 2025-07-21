 <%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Member_Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Agora - Login</title>
    <link href="css/layout.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.min.1.9.1.js" type="text/javascript"></script>
    <style type="text/css">
        .auto-style1 {
            width: 172px;
        }

        .auto-style3 {
            width: 2px;
        }

        .auto-style4 {
            width: 99px;
        }

        .auto-style5 {
            width: 99px;
            height: 34px;
        }

        .auto-style6 {
            width: 2px;
            height: 34px;
        }

        .auto-style7 {
            width: 172px;
            height: 34px;
        }

        .auto-style8 {
            width: 99px;
            height: 29px;
        }

        .auto-style9 {
            width: 2px;
            height: 29px;
        }

        .auto-style10 {
            width: 172px;
            height: 29px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            
            CleanQueryString();

        });
        function CleanQueryString() {

            var uri = window.location.toString();
            if (uri.indexOf("?") > 0) {
                var clean_uri = uri.substring(0, uri.indexOf("?"));
                window.history.replaceState({}, document.title, clean_uri);
            }
        }
    </script>
</head>
<body>
    <form id="frmLogin" runat="server">
        <!--=======start login-->
        <div class="login-wrap">
            <div class="login_logo">
                <img src="images/login-logo.png" />
            </div>
            <div class="login-box">
                <div class="login-cent">
                    <h1>Login</h1>
                    <table style="width: 100%; border: 0" class="manage-form" id="tbllgn" runat="server">
                        <tr>
                            <th>User ID :
                            </th>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvEmpid" runat="server" ControlToValidate="txtEmpId"
                                    ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <input type="text" name="" id="txtEmpId" class="textbox-a" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <th>Password :
                            </th>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                    ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <input name="" id="txtPassword" class="textbox-a" runat="server"
                                    style="padding: 5px;" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:RadioButtonList ID="rblUserType" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True" Text="AD" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Agora" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <th>Remember Me :
                            </th>
                            <td></td>
                            <td>
                                <input id="chkRemeber" name="" type="checkbox" value="" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="small_button yellow_button open"
                                    OnClick="doLogin" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:LinkButton ID="lnkforgotPassword" runat="server" Text="I can't access my account."
                                    OnClick="lnkforgotPassword_Click" CausesValidation="false"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%; border: 0" class="manage-form" id="tblChangepassword"
                        runat="server">
                        <tr>
                            <th>User ID :
                            </th>
                            <td></td>
                            <td>
                                <input type="text" name="" id="txtChnageUserid" class="textbox-a" runat="server"
                                    style="padding: 5px;" readonly="readonly" />
                            </td>
                        </tr>
                        <tr>
                            <th>New Password :
                            </th>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNewPassword"
                                    ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <input type="password" name="" id="txtNewPassword" class="textbox-a" runat="server"
                                    style="padding: 5px;" />
                            </td>
                        </tr>
                        <tr>
                            <th>Confirm Password :
                            </th>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtConfirmPassword"
                                    ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <input type="password" name="" id="txtConfirmPassword" class="textbox-a" runat="server"
                                    style="padding: 5px;" /><br />
                                <asp:CompareValidator ControlToValidate="txtConfirmPassword"
                                    runat="server" ControlToCompare="txtNewPassword"
                                    ErrorMessage="Password does not match." Font-Names="Verdana" Font-Size="9pt"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:Button ID="btnchangepass" runat="server" Text="Save" CssClass="small_button yellow_button open"
                                    OnClick="btnchangepass_Click" />
                            </td>
                        </tr>
                    </table>
                    <div style="text-align: center; padding-left: 55px;">
                        <asp:Label ID="lblStatus" Visible="false" runat="server" ForeColor="Red" ></asp:Label>
                        <asp:Label ID="lblMessage" Visible="false" runat="server" ForeColor="Red" ></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <!--End login-->
    </form>
</body>
</html>
