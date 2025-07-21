<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="Member_ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Agora - Change Password</title>
    <link href="css/layout.css" rel="stylesheet" type="text/css" />
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
</head>
<body style="line-height: 30px">
    <form id="frmChangePassword" runat="server" autocomplete="off">
        <!--=======start login-->
        <div class="login-wrap">
            <div class="login_logo">
                <img src="images/login-logo.png" />
            </div>
            <div class="login-box">
                <div class="login-cent">
                    <h1>Change Password</h1>
                    <table style="width: 100%; border: 0" class="manage-form" id="tbllgn" runat="server">
                        <tr>
                            <th>New Password :
                            </th>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword"
                                    ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <input type="password" name="" id="txtNewPassword" class="textbox-a" runat="server" style="padding: 5px; width: 200px;" />
                            </td>
                        </tr>
                        <tr>
                            <th>Confirm Password :
                            </th>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword"
                                    ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <input type="password" name="" id="txtConfirmPassword" class="textbox-a" runat="server" style="padding: 5px; width: 200px" />                      
                            </td>
                        </tr>
                         
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:Button ID="btnsubmit" runat="server" Text="Submit" CssClass="small_button yellow_button open" OnClick="btnsubmit_Click" />
                              <!--   <asp:Button ID="btngotoLogin" runat="server" Text="Go To Login" CssClass="small_button yellow_button"  CausesValidation="false" Visible="false" />-->
                               <asp:CompareValidator ID="comparePasswords"
                                    runat="server"
                                    ControlToCompare="txtNewPassword"
                                    ControlToValidate="txtConfirmPassword"
                                    ErrorMessage="Password do not match."/>
                            </td>
                        </tr>

                    </table>

                </div>
            </div>
        </div>
        <!--End login-->
    </form>
</body>
</html>
