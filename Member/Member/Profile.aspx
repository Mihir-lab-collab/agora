<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="Profile.aspx.cs" Inherits="Member_Profile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.common.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.rtl.min.css">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.default.min.css">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.dataviz.min.css">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.dataviz.default.min.css">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.mobile.all.min.css">

     <script src="https://cdn.kendostatic.com/2015.2.624/js/jszip.min.js"></script>
    <script src="https://cdn.kendostatic.com/2015.2.624/js/kendo.all.min.js"></script>

<%--    <script type="text/javascript" src="../js/console.js"></script>
    <script src="js/Employee.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $('[id$="txtEditAnniversaryDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
        });
        function CloseEditProfile()
        {
            $('#divEditprofile').css('display', 'none');
        }
        function OpenProfile()
        {
            $('[id$="txtEditAnniversaryDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
            $('#divEditprofile').css('display', '');
            $('[id$="txtEditCAddress"]').val($('[id$="lblEmpAddress"]').text());
            $('[id$="txtEditContact"]').val($('[id$="lblEmpContactNo"]').text());
            $('[id$="txtEditAnniversaryDate"]').val($('[id$="lblEmpADate"]').text());
        }

        function callmsg() {
            alert("saved successfully.")
        }

        function CheckData() {
            var flag=0;
            if ($('[id$="txtEditCAddress"]').val() == '')
                flag = 1;
            else if ($('[id$="txtEditContact"]').val() == '')
                flag = 1;

            if (flag == 1) {
                alert("current address and contact number field is mandatory.")
                return false;
            }
            else
                return true;
            
        }
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<link rel="stylesheet" type="text/css" href="http://cdn.webrupee.com/font" />--%>

    <asp:ScriptManager ID="ScriptManagerProfile" runat="server"></asp:ScriptManager>

    <%--<ajaxToolkit:ToolkitScriptManager ID="ScriptManagerProfile" runat="server"></ajaxToolkit:ToolkitScriptManager>--%>
    <asp:UpdatePanel runat="server" ID="updatepanelProfile">
        <ContentTemplate>
            <div class="content_wrap">
                <div class="gride_table">
                    <div class="box_border">
                        <div class="grid_head">
                            <h2>Employee Profile</h2>
                            <%--<asp:Label ID="lblWelcome" runat="server" CssClass="a_box"></asp:Label> --%>
                        </div>
                        <div runat="server">

                            <ajax:Accordion ID="accordionHome" runat="server" HeaderCssClass="accord_head "
                                HeaderSelectedCssClass="accord_head_a"
                                ContentCssClass="accord_cont" FadeTransitions="false" RequireOpenedPane="false" SelectedIndex="-1">
                                <Panes>
                                    <ajax:AccordionPane runat="server" ID="Panel1">
                                        <Header> Personal Details                                            
                                        </Header>
                                        <Content>
                                            <table id="tbProfile" class="manage_form" runat="server" width="100%">
                                                <tr>
                                                    <th style="width: 15%;">Employee ID</th>
                                                    <td>
                                                        <asp:Label ID="lblEmpId" runat="server"></asp:Label>
                                                        <asp:Label ID="lblEditStatus" runat="server" color="red" Visible="false" style="padding-left:875px;color:red" float="right"> <b>Approval Pending </b></asp:Label>
                                                        <input type="button" id="btnEditProfile" style="float: right" value="Edit Profile" onclick="OpenProfile();" class="small_button white_button open" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <th>Name</th>
                                                    <td>
                                                        <asp:Label ID="lblEmpName" runat="server"></asp:Label></td>
                                                </tr>

                                                <tr>
                                                    <th>Address</th>
                                                    <td>
                                                        <asp:Label ID="lblEmpAddress" runat="server"></asp:Label></td>
                                                </tr>

                                                <tr>
                                                    <th>Contact Number</th>
                                                    <td>
                                                        <asp:Label ID="lblEmpContactNo" runat="server"></asp:Label></td>
                                                </tr>

                                                <tr>
                                                    <th>E-Mail</th>
                                                    <td>
                                                        <asp:Label ID="lblEmpEMail" runat="server"></asp:Label>

                                                    </td>
                                                </tr>

                                                <tr>
                                                    <th>Designation</th>
                                                    <td>
                                                        <asp:Label ID="lblEmpSkills" runat="server"></asp:Label>

                                                    </td>
                                                </tr>

                                                <tr>
                                                    <th>Probation Period</th>
                                                    <td>
                                                        <asp:Label ID="lblEmpProbPeriod" runat="server"></asp:Label>

                                                    </td>
                                                </tr>

                                                <tr>
                                                    <th>Joining Date</th>
                                                    <td>
                                                        <asp:Label ID="lblEmpJoinDate" runat="server"></asp:Label></td>
                                                </tr>

                                                <tr>
                                                    <th>Account No.</th>
                                                    <td>
                                                        <asp:Label ID="lblEmpAccNo" runat="server"></asp:Label></td>
                                                </tr>

                                                <tr>
                                                    <th>Birth Date</th>
                                                    <td>
                                                        <asp:Label ID="lblEmpBdate" runat="server"></asp:Label></td>
                                                </tr>

                                                <tr>
                                                    <th>Anniversary Date</th>
                                                    <td>
                                                        <asp:Label ID="lblEmpADate" runat="server"></asp:Label></td>
                                                </tr>

                                            </table>
                                        </Content>
                                    </ajax:AccordionPane>

                                    <ajax:AccordionPane runat="server" ID="Panel3">
                                        <Header>Change Password</Header>
                                        <Content>
                                            <table id="tdchpwd" width="100%" runat="server" class="manage_form">
                                                <tr>
                                                    <th style="width: 15%;">User ID</th>

                                                    <td>
                                                        <input type="text" name="" id="txtChnageUserid" class="textbox-a" runat="server"
                                                            style="padding: 5px;" readonly="readonly" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th style="width: 15%;">Old Password</th>
                                                    <td>
                                                        <input type="password" name="" id="txtpwd" class="textbox-a" runat="server"
                                                            style="padding: 5px;" />

                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNewPassword" ValidationGroup="P"
                                                            ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th style="width: 15%;">New Password</th>
                                                    <td>
                                                        <input type="password" name="" id="txtNewPassword" class="textbox-a" runat="server"
                                                            style="padding: 5px;" />

                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNewPassword" ValidationGroup="P"
                                                            ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th style="width: 15%;">Confirm Password 

                                                    </th>
                                                    <td>
                                                        <input type="password" name="" id="txtConfirmPassword" class="textbox-a" runat="server"
                                                            style="padding: 5px;" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtConfirmPassword" ValidationGroup="P"
                                                            ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        <br />
                                                        <asp:CompareValidator ID="CompareValidator1" ControlToValidate="txtConfirmPassword"
                                                            runat="server" ControlToCompare="txtNewPassword"
                                                            ErrorMessage="Password does not match." Font-Names="Verdana" Font-Size="9pt"></asp:CompareValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 15%;"></td>
                                                    <td>
                                                        <asp:Button ID="btnchangepass" runat="server" Text="Save" CssClass="small_button yellow_button open"
                                                            ValidationGroup="P" OnClick="btnchangepass_Click" />
                                                    </td>
                                                </tr>
                                                <tr id="tdrow" runat="server" visible="false">
                                                    <td colspan="3">
                                                        <asp:Label ID="lblStatus" Visible="false" runat="server"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </Content>
                                    </ajax:AccordionPane>

                                    <ajax:AccordionPane runat="server" ID="Panel2">
                                        <Header>Your Current Package</Header>
                                        <Content>
                                            <%--<% if (DateTime.Now.Day >= 10)
                                               { %>--%>
                                            <table id="tdPackage" runat="server" width="100%" class="manage_form">
                                                <tr>
                                                    <th style="width: 15%;">Effective From </th>
                                                    <td>
                                                        <asp:Label ID="lblEffectiveFrom" runat="server">April 2012</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>Annual CTC</th>
                                                    <td><span class='WebRupee'>Rs</span>
                                                        <asp:Label ID="lblAnnualCTC" runat="server">0</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>Annual Bonus</th>
                                                    <td><span class='WebRupee'>Rs</span>
                                                        <asp:Label ID="lblAnnualBonus" runat="server" Text="0"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>Monthly Gross</th>
                                                    <td><span class='WebRupee'>Rs</span>
                                                        <asp:Label ID="lblMonthlyGross" runat="server">0</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                           <%-- <% } %>--%>
                                        </Content>
                                    </ajax:AccordionPane>

                                </Panes>
                            </ajax:Accordion>

                        </div>

                        <div class="k-widget k-windowAdd" id="divEditprofile" style="display: none; padding-top: 0px; padding-right: 4px; border-color:black; overflow: hidden; min-width: 240px; width: 446px; min-height: 254px; top: 39%; left: 609px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
                            <div class="popup_head" style="padding-left: 68px;">
                                <h3>Edit Profile</h3>
                                <img src="Images/delete_ic.png" class="close-button" onclick="CloseEditProfile();"
                                    alt="Close" />
                                <div class="clear">
                                </div>
                            </div>
                            <div style="width: 448px; height: 220px; overflow: auto">
                                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                                    <tr>
                                        <th rowspan="0">Current Address:</th>
                                        <td align="center" rowspan="0">
                                            <asp:TextBox runat="server" ID="txtEditCAddress" TextMode="MultiLine" Height="57px" Width="200px"></asp:TextBox>                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Contact Number:</th>
                                        <td align="center">  <%--isChar(event,this)--%> 
                                            <asp:TextBox runat="server" ID="txtEditContact" MaxLength="10" CssClass="k-textbox" onkeypress="return isNumber(event);" Width="200px"></asp:TextBox>                                           
                                            <asp:RegularExpressionValidator ID="RegularExpresphone2" Display="Dynamic" ControlToValidate="txtEditContact" runat="server" ErrorMessage="Enter Valid Phone Number." SetFocusOnError="True" ValidationExpression="^\d{10}$"></asp:RegularExpressionValidator>
                                        </td></tr>
                                    <tr>
                                        <th>Anniversary Date:</th>
                                        <td align="center">
                                            <input id="txtEditAnniversaryDate" runat="server" name="txtEditAnniversaryDate" onkeyup="dateInput(this)" onkeydown="return false;" onkeypress="dateInput(this)" style="width: 200px" class="k-textbox" />
                                        </td>
                                    </tr>
                                    <tr>                                        
                                        <td colspan="2" align="center">
                                            <asp:Button type="button" runat="server" id="btnSaveProfile" style="float:right" Text="Submit" class="small_button white_button open" OnClientClick="return CheckData();" OnClick="btnSaveProfile_Click" />
                                        </td>
                                    </tr>
                               </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

