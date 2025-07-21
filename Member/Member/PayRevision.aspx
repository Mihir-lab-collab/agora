<%@ Page Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="PayRevision.aspx.cs" Inherits="Member_PayRevision" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        function isNumberKeyOrDot(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode > 31 && (charCode < 46 || charCode > 57 || charCode == 47))
                return false;
            return true;
        }

        function ValidateRequired() {
            var i = 0;
            $("[action-role='required']").each(function () {
                var element = $(this);
                if (element.is('input')) {
                    if (dovalidate(this) && ($(this).val() == '')) {
                        $(this).addClass('required-field');
                        i = 1;
                    }
                    else {
                        $(this).removeClass('required-field');
                    }
                }
                else if (element.is('select')) {
                    if (dovalidate(this) && ($(this).val() == '0')) {
                        $(this).addClass('required-field');
                        i = 1;
                    }
                    else {
                        $(this).removeClass('required-field');
                    }
                }
            });

            if (i == 0) {
                return true;
            }
            else {
                return false;
            }
        }
        function dovalidate(element) {
            if ((($(element).is(':disabled')) == false) && (($(element).is(':visible')) == true)) {
                return true;
            }
            else {
                return false;
            }
        }

        function btnSaveConfirm() {
            if (confirm("Are you sure you want to Submit?")) {
                return true;
            } else {

                return false;
            }
        }

    </script>

    <style type="text/css">
        .required-field {
            border-color: #f00 !important;
        }

        .manage_form1 td {
            padding: 7px 25px !important;
        }

        .white_button {
            margin: 0 auto;
            display: block;
        }

        .right_Align {
            text-align: right;
            display: block;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div>
        <div runat="server" id="SalDiv"></div>
    </div>

    <div id="fillEmpSalData" runat="server" style="margin-bottom: 30px;">
        <table cellpadding="0" cellspacing="0" border="1" width="100%" class="manage_form1">
            <tr>
                <td colspan="4" style="font-size: Medium; font-weight: bold;"><b>Pay Revision</b></td>
            </tr>
            <tr>
                <td width="25%">Employee:</td>
                <td width="25%">
                    <asp:DropDownList ID="ddlEmployee" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="25%">Annual CTC:</td>
                <td width="25%">
                    <asp:TextBox ID="txtAnnualFixedCTC" runat="server" action-role="required" onkeypress="return isNumberKeyOrDot(event);"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Revision Date:</td>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server"></asp:DropDownList>
                    <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
                </td>
                <td>Annual Bonus:</td>
                <td>
                    <asp:TextBox ID="txtAnnualBonus" Text="0" runat="server" action-role="required"></asp:TextBox>
                    <span style="margin-left: 5px; display: inline-block;">
                        <asp:CheckBox ID="chkFixed" runat="server" Text="Fixed" /></span>
                </td>
            </tr>

            <tr>
                <td>Provident Fund:</td>
                <td>
                    <asp:CheckBox ID="chkPF" runat="server" />
                </td>

                <td>Insurance Premium:</td>
                <td>
                    <asp:TextBox ID="txtInsurancePremium" Text="0" runat="server" action-role="required" onkeypress="return isNumberKeyOrDot(event);"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td colspan="4">
                    <asp:Button ID="btnCalculate" class="small_button white_button open" runat="server" OnClientClick="return ValidateRequired();" Text="Calculate" OnClick="btnCalculate_Click" />
                </td>
            </tr>
        </table>
    </div>

    <div id="showEmpSalData" runat="server">
        <table cellpadding="0" cellspacing="0" border="1" width="100%" class="manage_form1">

            <tr>
                <td width="25%">Employee Name:</td>
                <td width="25%">
                    <asp:Label ID="lblEmpName" runat="server"></asp:Label>
                </td>
                <td>Basic Salary:</td>
                <td>
                    <asp:Label class="right_Align" ID="lblBasicSal" runat="server"></asp:Label>
                </td>
            </tr>

            <tr>
                <td width="25%">Effective From</td>
                <td width="25%">
                    <asp:Label ID="lblEffectiveFrom" runat="server"></asp:Label>
                </td>

                <td>House Rent Allowance:</td>
                <td>
                    <asp:Label class="right_Align" ID="lblHRA" runat="server"></asp:Label>
                </td>

            </tr>


            <tr>
                <td>Annual CTC:</td>
                <td>
                    <asp:Label class="right_Align" ID="lblAnnualCTC" runat="server"></asp:Label>
                </td>

                <td>Conveyance:</td>
                <td>
                    <asp:Label class="right_Align" ID="lblConveyance" runat="server"></asp:Label>
                </td>

            </tr>

            <tr>
                <td>Annual Fixed CTC:</td>
                <td>
                    <asp:Label class="right_Align" ID="lblAnnualFixedCTC" runat="server"></asp:Label>
                </td>

                <td>Medical Allowance:</td>
                <td>
                    <asp:Label class="right_Align" ID="lblMedicalAllowance" runat="server"></asp:Label>
                </td>

            </tr>

            <tr>
                <td>Annual Bonus:</td>
                <td>
                    <asp:Label class="right_Align" ID="lblBonus" runat="server"></asp:Label>
                </td>

                <td>L T A:</td>
                <td>
                    <asp:Label class="right_Align" ID="lblLTA" runat="server"></asp:Label>
                </td>
            </tr>

            <tr>
                <td>Monthly CTC:</td>
                <td>
                    <asp:Label class="right_Align" ID="lblCtc" runat="server"></asp:Label>
                </td>
                
                 <td>Food Allowance:</td>
                <td>
                    <asp:Label class="right_Align" ID="lblFoodAllowance" runat="server"></asp:Label>
                </td>
            </tr>

            <tr>
                <td>Monthly Gross:</td>
                <td>
                    <asp:Label class="right_Align" ID="lblGross" runat="server"></asp:Label>
                </td>

                <td>Gratuity:</td>
                <td>
                    <asp:Label class="right_Align" ID="lblgratuity" runat="server"></asp:Label>
                </td>

            </tr>

            <tr>
                <td>Net Salary:</td>
                <td>
                    <asp:Label class="right_Align" ID="lblNetSalary" runat="server"></asp:Label>
                </td>

               <td>Special Allowance:</td>
                <td>
                    <asp:Label class="right_Align" ID="lblSpecialAllowance" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Provident Fund:</td>
                <td>
                    <asp:Label class="right_Align" ID="lblProvidentFund" runat="server"></asp:Label>
                </td>
                <td>Insurance Premium:</td>
                <td>
                    <asp:Label class="right_Align" ID="lblInsurancePremium" runat="server"></asp:Label>
                </td>
            </tr>
              <tr>
                <td></td>
                <td>
                </td>
               <td>Professional Tax:</td>
                <td>
                    <asp:Label class="right_Align" ID="lblPT" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <asp:Button ID="btnSave" class="small_button white_button open" runat="server" Text="Submit" OnClientClick="btnSaveConfirm();" OnClick="Save_Click" />
                </td>
            </tr>

        </table>
    </div>

    <asp:HiddenField ID="hdnEmpId" runat="server" />
    <asp:HiddenField ID="hdnpt" runat="server" />
    <asp:HiddenField ID="hdnbonus" runat="server" />
    <asp:HiddenField ID="hdnpbb" runat="server" />

</asp:Content>
