<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empattdetails.aspx.vb" Inherits="admin_empattdetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Register Src="../controls/adminMenu.ascx" TagName="adminMenu" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    
    <title>Attendance details</title>
    <link rel="stylesheet" href="../css/style.css" type="text/css" />
    <style type="text/css">
        div.DialogueBackground {
            position: fixed;
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
            background-color: #777;
            opacity: 0.9;
            filter: alpha(opacity=50);
            text-align: center;
        }

            div.DialogueBackground div.Dialogue {
                width: 500px;
                height: 400px;
                overflow: auto;
                position: absolute;
                left: 50%;
                top: 20%;
                margin-left: -150px;
                margin-top: -50px;
                padding: 10px;
                background-color: #fff;
                border: solid 2px #000;
                z-index: 10090;
            }


        div.close-button {
            float: right;
            margin: -10px -10px 0 0;
        }

        div.k-overlayDisplaynone {
            display: none;
        }

        div.k-overlay {
            display: block;
            z-index: 10002;
            opacity: 0.5;
        }

        div.a_popbox {
            position: absolute;
            padding: 25px;
            z-index: 10050;
            background-color: white;
        }

        div.popup_wrap {
            border: 6px solid #252e34;
            background-color: #FFF;
            position: absolute;
            padding: 15px;
            z-index: 2;
            overflow-x: scroll;
        }

            div.popup_wrap h1 {
                padding: 0 0 10px 0;
                margin-bottom: 15px;
                border-bottom: 1px solid #e5e7e4;
                font-size: 22px;
                color: #728c3a;
            }

        div.popup_msg {
            border: 6px solid #252e34;
            background-color: #FFF;
            position: absolute;
            padding: 15px;
            top: 20%;
            left: 25%;
            right: 25%;
            z-index: 1;
        }
    </style>
    
    <script src="../js/jquery.min.js" type="text/javascript"></script>
    <script src="../js/jquery.min.1.9.1.js" type="text/javascript"></script>
    <script src="../JSController/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../JSController/ScrollableGridPlugin.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">


        $(document).ready(function () {
            $('#<%=grdatt.ClientID %>').Scrollable({
                  ScrollHeight: 300,
                  IsInUpdatePanel: true
              });
        });

        function PopupHours(attDate) {
            window.open('TimeReports.aspx?strDate=' + attDate, 'Report', 'scrollbars=yes,ststus=no,toolbar=no,menubar=no,location=right,resizable=yes,width=700,height=600,left=220,top=110');
            return false;
        }

        //Open popup function
        function Popup(attDate) {
            window.open('empAttendanceDetail.aspx?strDate=' + attDate, 'Report', 'scrollbars=yes,ststus=no,toolbar=no,menubar=no,location=right,resizable=yes,width=600,height=600,left=220,top=110');
            return false;
        }
        //End open poup
        function doAttandance(empid, strdate, strUpdate) {

            var a = strdate.split(' ')
            strdate = a[1]
            a = strdate.split('-')

            var mon = getnummonth(a[1])

            if (strUpdate == null) {
                strdate = a[0] + '-' + mon + '-' + a[2]
                window.open("EditAttendance.aspx?id=" + empid + "&currDate=" + strdate, "pop", 'scrollbars=yes,ststus=no,toolbar=no,menubar=no,location=right,resizable=yes,width=400,height=300,left=220,top=110')
                //alert(empid);

            }
            else {
                strdate = a[0] + '-' + mon + '-' + a[2]
                window.open("EditAttendance.aspx?id=" + empid + "&currDate=" + strdate + "&strUpdate=" + strUpdate, "pop", 'scrollbars=yes,ststus=no,toolbar=no,menubar=no,location=right,resizable=yes,width=400,height=300,left=220,top=110')
            }

        }

        function popupLog() {
            window.open('ViewTimeLog.aspx', 'scrollbars=yes,ststus=no,toolbar=no,menubar=no,location=right,resizable=yes,width=400,height=300,left=220,top=110')
        }


        function getnummonth(strmonth) {
            var num;
            if (strmonth == "1") {
                num = "Jan"
                return num;
            }
            else if (strmonth == "2") {
                num = "Feb"
                return num;
            }
            else if (strmonth == "3") {
                num = "Mar"
                return num;
            }
            else if (strmonth == "4") {
                num = "Apr"
                return num;
            }
            else if (strmonth == "5") {
                num = "May"
                return num;
            }
            else if (strmonth == "6") {
                num = "Jun"
                return num;
            }
            else if (strmonth == "7") {
                num = "Jul"
                return num;
            }
            else if (strmonth == "8") {
                num = "Aug"
                return num;
            }
            else if (strmonth == "9") {
                num = "Sep"
                return num;
            }
            else if (strmonth == "10") {
                num = "Oct"
                return num;
            }
            else if (strmonth == "11") {
                num = "Nov"
                return num;
            }
            else if (strmonth == "12") {
                num = "Dec"
                return num;
            }
        }

        function ConfirmRptGen() {
            //if (document.getElementById("ddlYear").value="") {  
            if ($('#ddlYear').val() == "0" && $('#ddlMonth').val() == "0") {
                alert("Please select Year and Month");
                return false;
            }
            if ($('#ddlYear').val() == "0" || $('#ddlYear').val() == "") {
                alert("Please select Year");
                return false;
            }
            if ($('#ddlMonth').val() == "0" || $('#ddlMonth').val() == "") {
                alert("Please select Month");
                return false;
            }


            //code to check if all items in Checklist are checked- starts here
            var CHK = document.getElementById("<%= chkColumnsList.ClientID%>");
            // var CHK = $("#chkColumnsList");
            // var CHK=$("#chkColumnsList").selector;
            var checkbox = CHK.getElementsByTagName("input");
            var counter = 0;
            for (var i = 0; i < checkbox.length; i++) {
                if (checkbox[i].checked) {
                    counter++;
                }
            }
            if (counter < 1) {
                if (confirm("Do you want report for All the Employees ?") == true) {
                    return true;
                } else {
                    return false;
                }
            }
            //code to check if all items in Checklist are checked- ends here

            return true;

        }
        
    </script>
    <script language="JavaScript" src="../Includes/CalendarControl.js" type="text/javascript">
    </script>
     <script type="text/javascript">
         $(document).ready(function () {
             $('[id$="grdatt"]').fixedHeaderTable({ footer: true, cloneHeadToFoot: true, fixedColumn: false });
        });
    </script>
</head>
<body>

    <uc1:adminMenu ID="AdminMenu1" runat="server" />
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <table width="100%">
            <%--<tr bgcolor="#edf2e6">
            <td bgcolor="#edf2e6">
                <b><font face="Verdana" size="2"><a href="empattdetails.aspx" style="color:#A2921E;"><font color="#A2921E">
                    Attendance</font></a><%--|<a href="empHolidayWorkDetails.aspx" style="color:#A2921E;"><font color="#A2921E">Holiday
                        Working</font></a> </b>
            </td>
        </tr>--%>
            <tr>
                <td style="padding-top: 10px">
                    <b><font face="Verdana" size="2">Location:</font></b>
                    <asp:DropDownList ID="dlLocation" runat="server" AutoPostBack="true" CssClass="c_dropdown"
                        OnSelectedIndexChanged="dlLocation_SelectedIndexChanged" Visible="false" AppendDataBoundItems="true" Width="200px">
                        <asp:ListItem Text="All" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                    <font face="Verdana" size="2">
                        <asp:Label ID="lblLocationId" runat="server" Visible="false" />&nbsp;&nbsp; </font>

                    <%--      <input id="btnTimeLog" type="button" value="TimeLog" onclick="popupLog()" />--%>
                    <%--<asp:Button ID="btnGenReport" runat="server" Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE" 
             Text="Generate Att Report" OnClick="ShowHideDialogue" CommandArgument="show" />--%>
                    <asp:Button ID="btnGenReport" runat="server" Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE"
                        Text="Generate Report" OnClick="ShowHideDialogue" CommandArgument="show" />
                </td>
            </tr>
            <tr style="background-color: #C5D5AE">
                <td align="left">
                    <%
                        Dim strDate As Object
                        Dim nextMonth As Object
                        Dim prevMonth As Object
                        strDate = Request.QueryString("strDate")
                        If Not IsDate(strDate) Or Month(strDate) = Month(Date.Today) Then
                            strDate = Now()
                        End If
                        nextMonth = FormatDateTime((DateAdd("m", 1, "1-" & MonthName(Month(strDate)) & "-" & Year(strDate))), 2)
                        prevMonth = FormatDateTime((DateAdd("m", -1, "1-" & MonthName(Month(strDate)) & "-" & Year(strDate))), 2)
                    %>
                    <div style="float: left; padding-left: 10px">
                        <a href="empattdetails.aspx?strDate=<% =prevMonth%>" style="text-decoration: none"><font
                            color="#A2921E"><b><<</b></font></a>&nbsp; <font color="#A2921E"></font><font face="Verdana"
                                size="2" color="#A2921E"><b>
                                    <%=Left(MonthName(Month(strDate)), 3) & " " & Year(strDate)%>
                                </b></font><font color="#A2921E"></font><a href="empattdetails.aspx?strDate=<%=nextMonth%>"
                                    style="text-decoration: none"><font color="#A2921E"><b>&nbsp;>></b></font></a><font
                                        color="#A2921E"></font>
                    </div>
                    <%--	<div style="float:left;padding-left:20px"><a style="text-decoration:none" href="javascript:void(0);" onclick="return Popup('<%=strDate %>');"><font color="#A2921E"></font><font face="Verdana"
										size="2" color="#A2921E"><b>Attendance Report</b></font></a></div> --%>
                    <div style="float: left; padding-left: 20px">
                        <a style="text-decoration: none" href="javascript:void(0);" onclick="return PopupHours('<%=strDate %>');">
                            <font color="#A2921E"></font><font face="Verdana" size="2" color="#A2921E"><b>Hours
                            Report</b></font></a>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdatt" runat="server" AutoGenerateColumns="False" Width="3000px"
                        CssClass="manage">
                        <Columns>
                            <asp:TemplateField HeaderText="EmpID">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("EmpID") %>' Style="white-space: nowrap; font-family: Verdana; font-size: 12px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="widthname" Wrap="false" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 1">
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("aa1") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk3" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 2">
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("aa2") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk4" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 3">
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("aa3") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk5" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 4">
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("aa4") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk6" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 5">
                                <ItemTemplate>
                                    <asp:Label ID="Label7" runat="server" Text='<%# Bind("aa5") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk7" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 6">
                                <ItemTemplate>
                                    <asp:Label ID="Label8" runat="server" Text='<%# Bind("aa6") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk8" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 7">
                                <ItemTemplate>
                                    <asp:Label ID="Label9" runat="server" Text='<%# Bind("aa7") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"> </asp:Label>
                                    <asp:CheckBox ID="chk9" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 8">
                                <ItemTemplate>
                                    <asp:Label ID="Label10" runat="server" Text='<%# Bind("aa8") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk10" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 9">
                                <ItemTemplate>
                                    <asp:Label ID="Label11" runat="server" Text='<%# Bind("aa9") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk11" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 10">
                                <ItemTemplate>
                                    <asp:Label ID="Label12" runat="server" Text='<%# Bind("aa10") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk12" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 11">
                                <ItemTemplate>
                                    <asp:Label ID="Label13" runat="server" Text='<%# Bind("aa11") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk13" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 12">
                                <ItemTemplate>
                                    <asp:Label ID="Label14" runat="server" Text='<%# Bind("aa12") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk14" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 13">
                                <ItemTemplate>
                                    <asp:Label ID="Label15" runat="server" Text='<%# Bind("aa13") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk15" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 14">
                                <ItemTemplate>
                                    <asp:Label ID="Label16" runat="server" Text='<%# Bind("aa14") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk16" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 15">
                                <ItemTemplate>
                                    <asp:Label ID="Label17" runat="server" Text='<%# Bind("aa15") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk17" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <%--  <asp:TemplateField HeaderText="EmpID" >
                 <ItemTemplate>
                <asp:Label ID="lblEmpName" runat="server" Text='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                 </ItemTemplate>
                 </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Date 16">
                                <ItemTemplate>
                                    <asp:Label ID="Label18" runat="server" Text='<%# Bind("aa16") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk18" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 17">
                                <ItemTemplate>
                                    <asp:Label ID="Label19" runat="server" Text='<%# Bind("aa17") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk19" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 18">
                                <ItemTemplate>
                                    <asp:Label ID="Label20" runat="server" Text='<%# Bind("aa18") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk20" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 19">
                                <ItemTemplate>
                                    <asp:Label ID="Label21" runat="server" Text='<%# Bind("aa19") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk21" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 20">
                                <ItemTemplate>
                                    <asp:Label ID="Label22" runat="server" Text='<%# Bind("aa20") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk22" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 21">
                                <ItemTemplate>
                                    <asp:Label ID="Label23" runat="server" Text='<%# Bind("aa21") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk23" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 22">
                                <ItemTemplate>
                                    <asp:Label ID="Label24" runat="server" Text='<%# Bind("aa22") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk24" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 23">
                                <ItemTemplate>
                                    <asp:Label ID="Label25" runat="server" Text='<%# Bind("aa23") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk25" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 24">
                                <ItemTemplate>
                                    <asp:Label ID="Label26" runat="server" Text='<%# Bind("aa24") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk26" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 25">
                                <ItemTemplate>
                                    <asp:Label ID="Label27" runat="server" Text='<%# Bind("aa25") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk27" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 26">
                                <ItemTemplate>
                                    <asp:Label ID="Label28" runat="server" Text='<%# Bind("aa26") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk28" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 27">
                                <ItemTemplate>
                                    <asp:Label ID="Label29" runat="server" Text='<%# Bind("aa27") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk29" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 28">
                                <ItemTemplate>
                                    <asp:Label ID="Label30" runat="server" Text='<%# Bind("aa28") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk30" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 29">
                                <ItemTemplate>
                                    <asp:Label ID="Label31" runat="server" Text='<%# Bind("aa29") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk31" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 30">
                                <ItemTemplate>
                                    <asp:Label ID="Label32" runat="server" Text='<%# Bind("aa30") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk32" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date 31">
                                <ItemTemplate>
                                    <asp:Label ID="Label33" runat="server" Text='<%# Bind("aa31") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk33" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="hrdstyl" />
                    </asp:GridView>
                </td>
                <td>
                    <asp:Panel runat="server" ID="pnlColumns" Visible="false" CssClass="DialogueBackground">
                        <div class="Dialogue" id="divcolumnslistPopup">

                            <p><font size="3" face="Verdana, Arial, Helvetica, sans-serif"><strong>Generate Report</strong></font></p>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="tbColumnsList" width="100%" class="manage_form">
                                        <tr>
                                            <td align="left">
                                                <font color="#a2921e" size="2" face="Verdana, Arial, Helvetica, sans-serif"><strong>Year</strong></font>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" CssClass="c_dropdown" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" ClientIDMode="Static"
                                                    Visible="true" AppendDataBoundItems="true" Width="70px">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <span style="color: Red;">*</span>

                                            </td>
                                            <td align="left">
                                                <font color="#a2921e" size="2" face="Verdana, Arial, Helvetica, sans-serif"><strong>Month</strong> </font>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" CssClass="c_dropdown"
                                                    Visible="true" AppendDataBoundItems="true" Width="120px">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <span style="color: Red;">*</span>

                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="4" style="height: 18px;"></td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="4">
                                                <asp:CheckBox ID="CheckBox1" Text="Select All" TextAlign="Right" Font-Bold="true" runat="server" Checked="false" AutoPostBack="true" OnCheckedChanged="checkBox1_CheckedChanged" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="4">

                                                <asp:CheckBoxList ID="chkColumnsList" OnSelectedIndexChanged="chkColumnsList_SelectedIndexChanged" RepeatDirection="Vertical" RepeatLayout="Flow" AutoPostBack="true" TextAlign="Right" runat="server" Height="100%"></asp:CheckBoxList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">

                                                <%--<asp:Button ID="btnClose" Text="Close" runat="server"  OnClick="btnCloseAddCoumns_Click"  CssClass="small_button white_button open" CausesValidation="false" />--%>
                                                <asp:Button ID="btnClear" Text="Clear" runat="server" OnClick="btnClear_Click" CssClass="small_button white_button open" CausesValidation="false" />
                                                <asp:Button ID="btnAddColumns" Text="OK" runat="server" CssClass="small_button white_button open" OnClientClick="return ConfirmRptGen();" CausesValidation="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="4">
                                                <asp:Label ID="lblError" Visible="false" runat="server" ForeColor="Red" Text=""></asp:Label>
                                            </td>
                                        </tr>

                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlYear" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="CheckBox1" EventName="CheckedChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="chkColumnsList" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                    <asp:PostBackTrigger ControlID="btnAddColumns"/>

                                </Triggers>
                            </asp:UpdatePanel>

                            <%--<asp:Button ID="btnCloseAddCoumns" Text="Cancel" runat="server"  OnClick="btnCloseAddCoumns_Click"  CssClass="small_button white_button open" CausesValidation="false" />--%>

                            <asp:ImageButton ID="btnClose" Style="position: absolute; top: 0; right: 0;" runat="server" OnClick="btnCloseAddCoumns_Click" CausesValidation="false" ImageUrl="~/Images/delete_ic.png"></asp:ImageButton>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdLocationId" runat="server" />
    </form>
</body>
</html>
