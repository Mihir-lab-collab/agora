<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Forgotpassword.aspx.cs" Inherits="Member_Forgotpassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Agora - Forgot Password</title>
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
    <script type="text/javascript">
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
    </script>

</head>
<body style="line-height: 30px">
    <form id="frmForgotPassword" runat="server">
        <!--=======start login-->
        <div class="login-wrap">
            <div class="login_logo">
                <img src="images/login-logo.png" />
            </div>
            <div class="login-box">
                <div class="login-cent">
                    <h1>Forgot Password</h1>
                    <table style="width: 100%; border: 0" class="manage-form" id="tbllgn" runat="server">
                        <tr>
                            <th>User ID :
                            </th>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvEmpid" runat="server" ControlToValidate="txtEmpId"
                                    ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <input type="text" name="" id="txtEmpId" class="textbox-a" runat="server" style="width: 200px;" onkeypress="return isNumber(event)" maxlength="8"/>
                            </td>
                        </tr>
                        <tr>
                            <th>Email Id :
                            </th>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvEmailId" runat="server" ControlToValidate="txtEmailId"
                                    ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <input type="text" name="" id="txtEmailId" class="textbox-a" runat="server" style="padding: 5px; width: 200px" />
                                <asp:RegularExpressionValidator ID="regEmail" ControlToValidate="txtEmailId" runat="server" ErrorMessage="Invalid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            </td>
                        </tr>

                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:Button ID="btnsubmit" runat="server" Text="Submit" CssClass="small_button yellow_button open" OnClick="btnsubmit_Click" />
                                <asp:Button ID="btngotoLogin" runat="server" Text="Go To Login" CssClass="small_button yellow_button" OnClick="btngotoLogin_Click" CausesValidation="false" />

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
