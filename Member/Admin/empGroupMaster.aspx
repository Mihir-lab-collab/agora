<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empGroupMaster.aspx.vb"
    Inherits="Admin_empGroupMaster" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/Controls/adminMenu.ascx" TagName="adminMenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Office Group</title>
    <script src="../Javascript/jquery-1.7.min.js" type="text/javascript"></script>
    <script src="../Javascript/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../Css/jquery-ui.css" rel="stylesheet" type="text/css" />
 
    <script src="../Javascript/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
  
    <script type="text/jscript">
        function load() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(BindData);
        }
        
    </script>
     <style type="text/css">
        .ui-timepicker-div .ui-widget-header
        {
            margin-bottom: 8px;
        }
        .ui-timepicker-div dl
        {
            text-align: left;
        }
        .ui-timepicker-div dl dt
        {
            height: 25px;
            margin-bottom: -25px;
        }
        .ui-timepicker-div dl dd
        {
            margin: 0 10px 10px 65px;
        }
        .ui-timepicker-div td
        {
            font-size: 10%;
        }
        .ui-tpicker-grid-label
        {
            background: none;
            border: none;
            margin: 0;
            padding: 0;
        }
        
        .ui-timepicker-rtl
        {
            direction: rtl;
        }
        .ui-timepicker-rtl dl
        {
            text-align: right;
        }
        .ui-timepicker-rtl dl dd
        {
            margin: 0 65px 10px 10px;
        }
    </style>
   <script type="text/javascript">
       $(document).ready(function () {
           BindData();
       });
       function BindData() {
           $("#<%=txtStartTime.ClientID %>").timepicker();
           $("#<%=txtEndTime.ClientID %>").timepicker();

       }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            EndRequestHandler();
        });
        function EndRequestHandler() {
            $("#<%=txtStartTime.ClientID %>").timepicker();
            $("#<%=txtEndTime.ClientID %>").timepicker();
        }
    </script>
    <script language="javascript" type="text/javascript">
        function validate() {

            if (document.getElementById("<%=txtGrp.ClientID%>").value == "") {
                var grp = document.getElementById('grp');
                grp.innerHTML = "Required!";

            }
            else {
                var grp = document.getElementById('grp');
                grp.innerHTML = "";
            }
            if (document.getElementById("<%=txtStartTime.ClientID%>").value == "") {
                var offst = document.getElementById('offst');
                offst.innerHTML = "Required!";
            }

            if (document.getElementById("<%=txtEndTime.ClientID%>").value == "") {
                var offen = document.getElementById('offen');
                offen.innerHTML = "Required!";
            }

            if (document.getElementById("txtGrp").value == "" || document.getElementById("<%=txtStartTime.ClientID%>").value == "" || document.getElementById("<%=txtEndTime.ClientID%>").value == "") {
                return false;
            }
            else {
                return true;

            }
        }

        function CheckStartTm() {
            if (document.getElementById("<%=txtStartTime.ClientID%>").value == "") {
                var offst = document.getElementById('offst');
                offst.innerHTML = "Required!";
            }
            else {
                var offst = document.getElementById('offst');
                offst.innerHTML = "";
                var lbltotalhr = document.getElementById('lbltotalhr');
                lbltotalhr.innerHTML = "";
                if ((document.getElementById("<%=txtStartTime.ClientID%>").value != "") && (document.getElementById("<%=txtEndTime.ClientID%>").value != "")) {

                    var offSt = document.getElementById("<%=txtStartTime.ClientID%>").value;
                    var offEn = document.getElementById("<%=txtEndTime.ClientID%>").value;
                    var StartTime = offSt.split(":");
                    var Endtime = offEn.split(":");
                    var tosthrs = (parseInt(StartTime[0]) * 60) + parseInt(StartTime[1]);
                    var toEnhrs = (parseInt(Endtime[0]) * 60) + parseInt(Endtime[1]);
                    var totalHr = toEnhrs - tosthrs;
                    var remainder = parseInt(totalHr % 60);
                    var quotient = parseInt(totalHr / 60);
                    if (remainder < 10 && remainder >=0) {
                        remainder = "0" + remainder;
                    }
                    if (quotient < 10 && quotient >=0) {
                        quotient = "0" + quotient;
                    }
                    lbltotalhr.innerHTML = quotient + ":" + remainder
                }


            } return false;
        }
        function CheckEndTm() {
            if (document.getElementById("<%=txtEndTime.ClientID%>").value == "") {
                var offen = document.getElementById('offen');
                offen.innerHTML = "Required!";
            }
            else {
                var offen = document.getElementById('offen');
                offen.innerHTML = "";
                var lbltotalhr = document.getElementById('lbltotalhr');
                lbltotalhr.innerHTML = "";
                if ((document.getElementById("<%=txtStartTime.ClientID%>").value != "") && (document.getElementById("<%=txtEndTime.ClientID%>").value != "")) {

                    var offSt = document.getElementById("<%=txtStartTime.ClientID%>").value;
                    var offEn = document.getElementById("<%=txtEndTime.ClientID%>").value;
                    var StartTime = offSt.split(":");
                    var Endtime = offEn.split(":");
                    var tosthrs = (parseInt(StartTime[0], 10) * 60) + parseInt(StartTime[1]);
                    var toEnhrs = (parseInt(Endtime[0], 10) * 60) + parseInt(Endtime[1]);
                    var totalHr = toEnhrs - tosthrs;
                    var remainder = parseInt(totalHr % 60, 10);
                    var quotient = parseInt(totalHr / 60, 10);
                    if (remainder < 10 && remainder>=0) {
                        remainder = "0" + remainder;
                    }
                    if (quotient < 10 && quotient>=0) {
                        quotient = "0" + quotient;
                    }

                    lbltotalhr.innerHTML = quotient + ":" + remainder;
                }
            } return false;
        }
       
    </script>

  
</head>
<body>
    <form id="form1" runat="server" method="post">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table cellspacing="0" cellpadding="4" width="100%" border="0" style="border-collapse: collapse;
        border-color: #111111;" align="center">
        <tr>
            <td>
                <uc1:adminMenu ID="AdminMenu2" runat="server" />
            </td>
        </tr>
        <tr style="padding-top: 20px">
            <td>
                <asp:UpdatePanel ID="pnl" runat="server">
                    <ContentTemplate>
                        <table cellspacing="0" cellpadding="4" width="70%" border="0" style="border-collapse: collapse;
                            border-color: #c5d5ae;" align="left">
                            <tr>
                                <td>
                                    <table cellspacing="0" cellpadding="4" width="80%" border="1" style="border-collapse: collapse;
                                        border-color: #c5d5ae" align="left">
                                        <tr>
                                            <td bgcolor="#edf2e6" align="left" width="20%">
                                                <b><font face="Verdana" color="#a2921e" size="2">Group Name</font></b>
                                            </td>
                                            <td>
                                              <asp:TextBox ID="txtGrp" runat="server" Width="150px" onchange="javascript:return validate()"></asp:TextBox>
                                                 <span id="grp" style="font-size: 9pt; font-family: Verdana; color: Red"></span>   
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="left" bgcolor="#edf2e6">
                                                <table cellspacing="0" cellpadding="0" width="80%" border="0" style="border-collapse: collapse;
                                                    border-color: #c5d5ae" align="left" bgcolor="#edf2e6">
                                                    <tr>
                                                        <td bgcolor="#edf2e6" width="25%" align="center">
                                                            <b><font face="Verdana" color="#a2921e" size="2">All Employee</font></b>
                                                        </td>
                                                        <td bgcolor="#edf2e6">
                                                        </td>
                                                        <td bgcolor="#edf2e6" width="25%" align="center">
                                                            <b><font face="Verdana" color="#a2921e" size="2">Selected Employee</font></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td bgcolor="#edf2e6" align="right" width="25%">
                                                            <asp:ListBox ID="lstAllEmp" runat="server" Width="200px" Height="250px" SelectionMode="Multiple">
                                                            </asp:ListBox>
                                                        </td>
                                                        <td align="center" bgcolor="#edf2e6" width="20%">
                                                            <table align="center" width="100%" bgcolor="#edf2e6" cellpadding="8" cellspacing="8">
                                                                <tr>
                                                                    <td bgcolor="#edf2e6">
                                                                        <asp:Button ID="btnAllNext" runat="server" Style="font-family: Verdana; font-size: 10pt;
                                                                            color: #A2921E; font-weight: bold; background-color: #C5D5AE" Text=">>" 
                                                                            Width="50px" Font-Bold="True" Height="30px">
                                                                        </asp:Button>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td bgcolor="#edf2e6">
                                                                        <asp:Button ID="btnSingleNext" runat="server" Style="font-family: Verdana; font-size: 10pt;
                                                                            color: #A2921E; font-weight: bold; background-color: #C5D5AE" Text=">" 
                                                                            Width="50px" Font-Bold="True" Height="30px">
                                                                        </asp:Button>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td bgcolor="#edf2e6">
                                                                        <asp:Button ID="btnSingleBack" runat="server" Style="font-family: Verdana; font-size: 10pt;
                                                                            color: #A2921E; font-weight: bold; background-color: #C5D5AE" Text="<" 
                                                                            Width="50px" Font-Bold="True" Height="30px">
                                                                        </asp:Button>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td bgcolor="#edf2e6">
                                                                        <asp:Button ID="btnAllBack" runat="server" Style="font-family: Verdana; font-size: 10pt;
                                                                            color: #A2921E; font-weight: bold; background-color: #C5D5AE" Text="<<" 
                                                                            Width="50px" Font-Bold="True" Height="30px">
                                                                        </asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td bgcolor="#edf2e6" align="left" width="25%">
                                                            <asp:ListBox ID="lstSelEmp" runat="server" Width="200px" Height="250px" SelectionMode="Multiple">
                                                            </asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td bgcolor="#edf2e6" align="left" width="30%">
                                                <b><font face="Verdana" color="#a2921e" size="2">Office Start Time</font></b>
                                            </td>
                                            <td width="75%" align="left">
                                                <asp:TextBox ID="txtStartTime" runat="server" Width="100px"  
                                                    onchange="javascript:return CheckStartTm()"></asp:TextBox>
                                               <span id="offst" style="font-size:9pt;font-family:Verdana;color:Red">  </span>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td bgcolor="#edf2e6" align="left" width="30%">
                                                <b><font face="Verdana" color="#a2921e" size="2">Office End Time</font></b>
                                            </td>
                                            <td width="75%" align="left">
                                                <asp:TextBox ID="txtEndTime" runat="server" Width="100px"  onchange="javascript:return CheckEndTm()"></asp:TextBox>
                                                <span id="offen" style="font-size:9pt;font-family:Verdana;color:Red">  </span>
                                            </td>
                                        </tr>
                                        <tr id="trhr" runat="server">
                                            <td bgcolor="#edf2e6" align="left" width="30%">
                                                <b><font face="Verdana" color="#a2921e" size="2">Total Hours</font></b>
                                            </td>
                                            <td width="75%" align="left">
                                                <asp:Label ID="lbltotalhr" runat="server" Font-Names="Verdana" Font-Size="9pt" ></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:Button ID="btnSave" runat="server" Style="font-family: Verdana; font-size: 8pt;
                                                    color: #A2921E; font-weight: bold; background-color: #C5D5AE; height: 21px;"
                                                    Text="Submit" OnClientClick="return validate()"  ></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="padding-top: 10px">
                                <td>
                                    <asp:DataGrid ID="dgoffGroup" runat="server" BorderColor="Black" Font-Size="10pt"
                                        Font-Name="Verdana" BackColor="White" Font-Names="Verdana" AutoGenerateColumns="False"
                                        FooterStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="10pt" HeaderStyle-BackColor="LightGray"
                                        CellPadding="2" ShowFooter="True" Width="100%" PageSize="20" AllowPaging="true"
                                        AllowSorting="true">
                                        <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="True" HorizontalAlign="Left">
                                        </ItemStyle>
                                        <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                            Font-Size="10pt"></HeaderStyle>
                                        <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False" Font-Bold="True"
                                            HorizontalAlign="Right"></FooterStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="False" DataField="PkID" HeaderText="PkID"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="GroupName" SortExpression="GroupName" HeaderText="Group Name">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Left">
                                                </ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                                    HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="OfficeStartTime" SortExpression="OfficeStartTime" HeaderText="Office Start Time">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Center">
                                                </ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                                    HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="OfficeEndTime" SortExpression="OfficeEndTime" HeaderText="Office End Time"
                                                ItemStyle-BackColor="#FFFFEE">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Center">
                                                </ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="AdminEntryDate" SortExpression="AdminEntryDate" DataFormatString="{0:dd-MMM-yy hh:mm tt}"
                                                HeaderText="Entry Date">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Center">
                                                </ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                                    HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn>
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" Width="10%"></ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False">
                                                </HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                                <ItemTemplate>
                                                    <asp:Button ID="update" runat="server" CommandName="Update" Text="Update" Style="font-family: Verdana;
                                                        font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE">
                                                    </asp:Button>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Left" ForeColor="#003399" BackColor="#C5D5AE" Mode="NumericPages">
                                        </PagerStyle>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
