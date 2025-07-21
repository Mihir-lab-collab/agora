<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayslipArchive.aspx.cs" Inherits="Member_PayslipArchive" MasterPageFile="~/Member/Admin.master" EnableEventValidation="false" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="../js/console.js"></script>

    <script src="js/PayslipArchive.js" type="text/javascript"></script>
   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">

        function PrintPayslip() {

            var divContents = $("#ctl00_ContentPlaceHolder1_dvContainer").html();
            var printWindow = window.open('', '', 'height=400,width=800');
            printWindow.document.write('<html><head><title></title>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(divContents);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            printWindow.print();
        }

        $(document).ready(function () {
            if (sessionTimeout == 0)
                sessionTimeout = $("[id$='spnTimeout']").text(); // This timeout is in minutes
            if (sessionTimeout == "") {
                sessionTimeout = 15;
            }
            var timeoutWarning = sessionTimeout;
            var sessionTimeoutInSeconds = (parseInt(sessionTimeout) * 60 * 1000) - 10000;
            timeoutWarningInSeconds = (parseInt(timeoutWarning) * 60 * 1000) - 10000;
            var UrlValue = window.location.href.substring(window.location.href.lastIndexOf('/'));
            if (!(UrlValue != null && UrlValue.toLowerCase().indexOf("batchprocess") >= 0)) {
                inactivityTimer = setTimeout('ExpiryWarning()', timeoutWarningInSeconds);
            }
            $('[id$="Btn11"]').click(function () {
                __doPostBack('ctl00$Btn11', '');
            });
        });

    </script>

    <input type="hidden" id="hdnEmpID" runat="server" />
    <input type="hidden" id="hdnMonth" runat="server" />
    <input type="hidden" id="hdnYear" runat="server" />
    <div id="temp"></div>

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <asp:Label ID="lbl" Text="Payslip Archive" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <div>
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    <table>
                                        <tr>
                                            <td class="generate_btn" valign="top" style="padding-right: 1px; /*float: left*/"></td>
                                            <%-- <td style="/*display:block;*/">
                                                <input style="width: 25px;" type="checkbox" id="chkInclude" onclick="Search();" />Include Archive&nbsp;&nbsp;
                                            </td>--%>
                                            <td valign="top" style="/*float: left*/">Employee ID:<span style="color:red">*</span>
                                                <input type="text" id="txtEmployeeID" runat="server" style="width: 80px" />&nbsp;<span id="errmsg"></span>
                                            </td>
                                            <%-- <td>
                                                From date:
                                               <%-- <input type="text" id="txtFromDate" onkeypress="return false;" style="width: 125px" />--%>


                                            <td valign="top">
                                                <label>Select Month: </label><span style="color:red">*</span>
                                                <asp:DropDownList ID="ddlMonth" AppendDataBoundItems="true" runat="server" Width="110">
                                                    <asp:ListItem Value="0" Text="Select Month" />
                                                    <%--Selected="True"--%>
                                                    <asp:ListItem Value="1" Text="January" />
                                                    <asp:ListItem Value="2" Text="February" />
                                                    <asp:ListItem Value="3" Text="March" />
                                                    <asp:ListItem Value="4" Text="April" />
                                                    <asp:ListItem Value="5" Text="May" />
                                                    <asp:ListItem Value="6" Text="June" />
                                                    <asp:ListItem Value="7" Text="July" />
                                                    <asp:ListItem Value="8" Text="August" />
                                                    <asp:ListItem Value="9" Text="September" />
                                                    <asp:ListItem Value="10" Text="October" />
                                                    <asp:ListItem Value="11" Text="November" />
                                                    <asp:ListItem Value="12" Text="December" />
                                                </asp:DropDownList>
                                            </td>
                                            <td valign="top">
                                                <label>Year: </label><span style="color:red">*</span>
                                                <asp:TextBox ID="SearchYear" runat="server" Width="100"></asp:TextBox>
                                            </td>
                                            <td valign="top">
                                                <%--<span id="spn" runat="server" onclick="Search();" class="small_button white_button open">Search</span>--%>
                                                <asp:Button ID="btnSearch" runat="server" OnClientClick="return Search();" OnClick="btnSearch_Click" class="small_button white_button open" Text="Search" />

                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>

                        <div class="grid_head">
                            <asp:Label ID="lblPayslip" Text="Payslip" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                        </div>

                        <table id="tblsalaryslip" runat="server" width="100%" class="manage_form" style="display: none;">
                            <tbody style="display: block; width: 100%;">
                                <tr style="display: block; width: 100%">
                                    <td colspan="4" align="right">
                                        <img id="imgLogo" align="right" alt="logo" runat="server" />
                                        <b>
                                            <asp:Label ID="lblCompanyName" runat="server" />
                                        </b>
                                        <br />
                                        <b>
                                            <asp:Label ID="lblCompanyAddr" runat="server" /></b>

                                    </td>
                                </tr>

                                <tr>
                                    <td><b>SALARY SLIP FOR THE MONTH <span id="lblSalaryDate" runat="server"></span>

                                    </b></td>
                                </tr>
                            </tbody>
                        </table>

                        <% if (flgsalgenerated == 1)
                           { %>
                        <table id="tdEmpDetails" runat="server" width="100%" class="manage_grid_a" style="border-top: none; border-bottom: none;">
                            <tr>
                                <th colspan="4" style="text-align: center">Employee Details</th>
                            </tr>
                            <tr>
                                <td width="25%"><b>Name</b> </td>
                                <td width="25%">
                                    <asp:Label ID="lblname" runat="server"></asp:Label>
                                </td>
                                <td width="25%"><b>No Of Days</b> </td>
                                <td width="25%">
                                    <asp:Label ID="lblnoofdays" runat="server">0</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b>Employed Since </b></td>
                                <td>
                                    <asp:Label ID="lblemployedsince" runat="server"></asp:Label>
                                </td>
                                <td><b>Days Present </b></td>
                                <td>
                                    <asp:Label ID="lbldayspresent" runat="server">0</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b>Designation</b></td>
                                <td>
                                    <asp:Label ID="lbldesignation" runat="server"></asp:Label>
                                </td>
                                <td><b>Days Absent</b>  </td>
                                <td b>
                                    <asp:Label ID="lbldaysabsent" runat="server">0</asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table id="tdBasic" runat="server" class="manage_grid_a" width="100%">
                            <tr>
                                <th style="width: 23.33%; text-align: center"><b>Receivable</b> </th>
                                <th style="width: 10%; text-align: center"><span class='WebRupee'>Rs</span></th>
                                <th style="width: 23.33%; text-align: center"><b>Deductions</b></th>
                                <th style="width: 10%; text-align: center"><span class='WebRupee'>Rs</span></th>
                                <th style="width: 23.33%; text-align: center"><b>Addition</b></th>
                                <th style="width: 10%; text-align: center"><span class='WebRupee'>Rs</span> </th>
                            </tr>
                            <tr>
                                <td><b>Basic Salary</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblbasicsal" runat="server">0</asp:Label>
                                </td>
                                <td><b>Advance</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lbladvancea" runat="server">0</asp:Label>
                                </td>
                                <td><b>(A-B)</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblab" runat="server">0</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b>HRA</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblhra" runat="server">0</asp:Label>
                                </td>

                                <td><b>Income Tax</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblincometax" runat="server">0</asp:Label>
                                </td>

                                <td><b>Bonus</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblbonus" runat="server">0</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b>TA</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblta" runat="server">0</asp:Label>
                                </td>
                                <td><b>Profession Tax</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblproffesiontax" runat="server">0</asp:Label>
                                </td>
                                <td><b>Other Addition</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lbladvanceab" runat="server">0</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b>Medical</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblmedical" runat="server">0</asp:Label>
                                </td>
                                <td><b>EPF</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblepf" runat="server">0</asp:Label>
                                </td>

                                <td><b>Loan</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblloan" runat="server">0</asp:Label>
                                </td>

                            </tr>
                            <tr>
                                <td><b>LTA</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lbllta" runat="server">0</asp:Label>
                                </td>

                                <td><b>Loan Payment</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblloanrepayment" runat="server">0</asp:Label>
                                </td>

                                <%--  <td><b>Paid Leaves</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblpaidleaves" runat="server">0</asp:Label>
                                </td>--%>
                                <td><b>Others</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblothersab" runat="server">0</asp:Label>
                                </td>

                            </tr>
                            <tr>
                                <td><b>Food Allow.</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblfoodallow" runat="server">0</asp:Label>
                                </td>

                                <td><b>Leave Deduction</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblleavededuction" runat="server">0</asp:Label>
                                </td>
                                <%-- <td><b>Others</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblothersab" runat="server">0</asp:Label>
                                </td>--%>
                                <td style="text-align: right">-
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="Label1" runat="server">-</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b>Special Allow.</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblspecialallow" runat="server">0</asp:Label>
                                </td>
                                <td><b>Group Insurance Premium</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblInsurance" runat="server">0</asp:Label>
                                </td>
                                <td style="text-align: right">-
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblblank" runat="server">-</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td style="text-align: right">&nbsp;
                                </td>
                                <td><b>Others</b></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblothers" runat="server">0</asp:Label>
                                </td>

                                <td>&nbsp;</td>
                                <td style="text-align: right">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td><b>Gross Salary </b>(A) </td>
                                <td style="text-align: right">
                                    <b><span class='WebRupee'>Rs</span>
                                        <asp:Label ID="lblgrosssal" runat="server">0</asp:Label>
                                    </b>
                                </td>
                                <td><b>Total Deduction (B)</b></td>
                                <td style="text-align: right">
                                    <b>
                                        <span class='WebRupee'>Rs</span>
                                        <asp:Label ID="lbltotaldeduction" runat="server">0</asp:Label>
                                    </b>
                                </td>
                                <td><b>Net Payable</b></td>
                                <td style="text-align: right">
                                    <b>
                                        <span class='WebRupee'>Rs</span>
                                        <asp:Label ID="lblnetpayable" runat="server">0</asp:Label>
                                    </b>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" style="text-align: center">

                                    <div id="dvContainer" runat="server" style="display: none">
                                    </div>
                                    <br />
                                    <input type="button" value="Print" id="btnPrint" runat="server" class="small_button white_button open" onclick="PrintPayslip();" />





                                </td>
                            </tr>
                        </table>

                        <% } %>
                        <% if (flgsalgenerated == 0)
                           { %>
                        <table id="tdGenerate" runat="server" width="100%" class="manage_form">
                            <tr>
                                <th style="text-align: center"><b>SALARY SLIP FOR THIS MONTH IS NOT GENERATED</b>
                                </th>
                            </tr>
                        </table>
                        <% } %>
                    </div>
                </div>
                <%-- <div id="gridLeaves"></div> content goes here--%>
            </div>
        </div>
    </div>

</asp:Content>

