<%@ Page Language="VB" ContentType="text/html" ResponseEncoding="iso-8859-1" %>

<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Net.Mail" %>
<script language="VB" runat="server">
    Dim sql As String
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
    Dim dr As SqlDataReader
    Dim cmd As New SqlCommand
    Dim gf As New generalFunction
    Dim objdatareader As SqlDataReader
    Dim objcmd As SqlCommand
    Dim strfrmdate As String
    Dim strtodate As String
    Dim arrfrmdate() As String
    Dim arrtodate() As String
     Dim list As String
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        gf.checkEmpLogin()
        Dim empId = Session("dynoEmpIdSession")
        
        cmd = New SqlCommand("  select empProbationPeriod,empJoiningDate  From employeeMaster where empid  ='" & empId & "' and empProbationPeriod<>0 and DateAdd(mm,empProbationPeriod,empJoiningDate) >= GETDATE()", con)
        con.Open()
        Dim isprobationperiodEmp = cmd.ExecuteScalar()
        con.Close()
        
        cmd = New SqlCommand("SELECT statusdesc, statusid FROM empStatus WHERE sttatusLeave=1", con)
        con.Open()
        dr = cmd.ExecuteReader()
        While dr.Read()
            dropLeaveType.Items.Add(New ListItem(dr(0), dr(1)))
        End While
        dr.Close()
        con.Close()
        Dim plbal As Integer = 0
        
        Dim slbal As Integer = 0
        
        Dim clbal As Integer = 0
        
        Dim compoffbal As Integer = 0
      
        
        If isprobationperiodEmp <> 0 Then
            dropLeaveType.Items.Remove(New ListItem("Casual Leave", "CL"))
            dropLeaveType.Items.Remove(New ListItem("Sick Leave", "SL"))
            dropLeaveType.Items.Remove(New ListItem("Paid Leave", "PL"))
        Else
            If Integer.TryParse(Request.QueryString("CLBalance"), clbal)  and clbal =0 Then
                dropLeaveType.Items.Remove(New ListItem("Casual Leave", "CL"))
            End If
        
            If Integer.TryParse(Request.QueryString("SLBalance"), slbal) and slbal=0 Then
                dropLeaveType.Items.Remove(New ListItem("Sick Leave", "SL"))
            End If
        
            If Integer.TryParse(Request.QueryString("PLBalance"), plbal) and plbal=0 Then
                dropLeaveType.Items.Remove(New ListItem("Paid Leave", "PL"))
            End If
        
            If Integer.TryParse(Request.QueryString("CompOffBalance"), compoffbal) and  compoffbal =0 Then
                dropLeaveType.Items.Remove(New ListItem("Comp Off", "CO"))
            End If
        End If
        
     
       
        Using cmd As New SqlCommand("SELECT dbo.getBalanceLeave('" & empId & "')", con)
            con.Open()
            Dim result = cmd.ExecuteScalar()

            'Dim oSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
            'list = oSerializer.Serialize(result)
            lblnoofleave.Text = result.ToString()
        End Using
        
        dr.Close()
        con.Close()
        
       
        
    End Sub

    Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim strReason As String
        Dim strDateFrom As String
        Dim strDateTo As String
        
        strReason = Request.Form("txtRofLeave")
        strDateFrom = Request.Form("txtDateFrom")
        strDateTo = Request.Form("txtDateTo")
        arrfrmdate = txtDateFrom.Text.Split("-")
        arrtodate = txtDateTo.Text.Split("-")
        strfrmdate = arrfrmdate(0).ToString() & "-" & arrfrmdate(1).Substring(0, 3).ToString() & "-" & arrfrmdate(2).ToString()
        strtodate = arrtodate(0).ToString() & "-" & arrtodate(1).Substring(0, 3).ToString() & "-" & arrtodate(2).ToString()
       
        sql = "INSERT INTO empLeaveDetails(empId,leaveId,leaveFrom,leaveTo,leaveDesc) VALUES( " & _
        Session("dynoEmpIdSession") & ",'" & dropLeaveType.SelectedValue & "','" & strfrmdate & "','" & _
        strtodate & "','" & txtRofLeave.Text.Replace("'", "''") & "')"
        
      

        cmd = New SqlCommand(sql, con)
        con.Open()
        cmd.ExecuteNonQuery()
        cmd.Dispose()
        con.Close()
 
        cmd = New SqlCommand("SELECT empName,empEmail FROM employeeMaster WHERE empId= " & _
        Session("dynoEmpIdSession") & " ", con)
        con.Open()
        dr = cmd.ExecuteReader()
        If dr.Read() Then
            Dim strBody As String = dr("empName") & " " & "has applied for leave " & "<b>From</b>" & ":" & _
            strDateFrom & " " & "<b>TO</b>" & ":" & strDateTo & " " & "<br> Reason for Leave Is" & _
            "<b>:</b>" & strReason
            Dim strSubject As String = dr("empName") & " has applied for leave."
             'gf.SendEmail(strBody, strSubject, ConfigurationManager.AppSettings("HREmail").ToString(), dr("empEmail"), "", True)



            'Start by creating a mail message object
            Dim MyMailMessage As New MailMessage()
			
            'From requires an instance of the MailAddress type
            MyMailMessage.From = New MailAddress(dr("empEmail"), dr("empName"))
			
            'To is a collection of MailAddress types
            MyMailMessage.To.Add(ConfigurationManager.AppSettings("HREmail").ToString())
            MyMailMessage.CC.Add(dr("empEmail"))
            MyMailMessage.Subject = strSubject
            MyMailMessage.Body = strBody
            MyMailMessage.IsBodyHtml = True
			
            'Create the SMTPClient object and specify the SMTP GMail server
            Dim SMTPServer As New SmtpClient("smtp.gmail.com")
            SMTPServer.Port = 587
            SMTPServer.Credentials = New System.Net.NetworkCredential("emp@intelgain.com", "intelgain123")
            SMTPServer.EnableSsl = True
            ' SMTPServer.Send(MyMailMessage)


        End If
        dr.Close()
        cmd.Dispose()
        con.Close()
				
        Dim sp1 As String
        sp1 = "<Script language=JavaScript>"
        sp1 += "window.opener.location=window.opener.location;"
        sp1 += "window.close();"
        sp1 += "</" + "script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "script123", sp1)
    End Sub


   
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link rel="stylesheet" href="../includes/CalendarControl.css" type="text/css" />
    <%--<meta http-equiv="Content-Language" content="en-us">--%>
    <title>Employee Leave Management</title>
</head>
<script language="JavaScript" src="../includes/CalendarControl.js" type="text/javascript"></script>
<script type="text/javascript" src="../Javascript/json2.js"></script>
<script language="javascript" type="text/javascript">
    function chkvalues() {
        if (document.getElementById("txtDateFrom").value == "") {
            alert("Please select From Date");
            return false;
        }
        else if (document.getElementById("txtDateTo").value == "") {
            alert("Please select TO date");
            return false;
        }
        else if (document.getElementById("txtRofLeave").value == "") {
            alert("Please give the reason");
            return false;
        }
    }

</script>
<script type="text/javascript">
    function ajaxFunction() {
        var xmlHttp;
        try {
            xmlHttp = new XMLHttpRequest();
        }
        catch (e) {    // Internet Explorer
            try {
                xmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
            }
            catch (e) {
                try {
                    xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
                }
                catch (e) {
                    alert("Your browser does not support AJAX!");
                    return false;
                }
            }
        }
        return xmlHttp;
    }
    function setCalendarControlDate(year, month, day) {
        calendarControl.setDate(year, month, day);
        var offSt = document.getElementById("<%=txtDateFrom.ClientID%>").value;
        var offEn = document.getElementById("<%=txtDateTo.ClientID%>").value;
        var First = document.getElementById("<%= hdnf.ClientID %>").value;
        var Second = document.getElementById("<%= hdnS.ClientID %>").value;
        First = offSt;
        Second = offEn

        if (First != '' && Second != '') {

           
            // Below is for Convert 15-jan-2013 to 01/15/2013 for Comparison///
            var stMonthName = offSt.substring(3, 6);
            var stdt = new Date(stMonthName + '01, 1900');
            var stMonthNumber = stdt.getMonth() + 1;
            var NewStDt = stMonthNumber + '/' + offSt.substring(0, 2) + '/' + offSt.substring(7, 11)


            var enMonthName = offEn.substring(3, 6);
            var endt = new Date(enMonthName + '01, 1900');
            var enMonthName = endt.getMonth() + 1;
            var NewEnDt = enMonthName + '/' + offEn.substring(0, 2) + '/' + offEn.substring(7, 11)

            ///end///

            startDate = new Date(NewStDt);
            endDate = new Date(NewEnDt);

            if (startDate <= endDate) {
                var url = "/Webservice/Getleavecnt.ashx?Frmdt=" + First + "&Todt=" + Second;

                var xmlHttp = ajaxFunction();

                xmlHttp.open("GET", url, true);
                xmlHttp.onreadystatechange = function () {
                    if (xmlHttp.readyState == 4) {
                        var str = xmlHttp.responseText;
                        if (str != "") {
                            var obj = JSON.parse(str);
                            var Totleave = document.getElementById('Totleave');
                            Totleave.innerHTML = obj;
                        }
                    }

                }
                xmlHttp.send(null);
            }

            else {
                var Totleave = document.getElementById('Totleave');
                Totleave.innerHTML = '';
                alert("From date should be less than or equal to To date.");
            }

        }


    }
      function CheckDate() {


        var offSt = document.getElementById("<%=txtDateFrom.ClientID%>").value;
        var offEn = document.getElementById("<%=txtDateTo.ClientID%>").value;

        if (offSt != '' && offEn != '') {

            // Below is for Convert 15-jan-2013 to 01/15/2013 for Comparison///
            var stMonthName = offSt.substring(3, 6);
            var stdt = new Date(stMonthName + '01, 1900');
            var stMonthNumber = stdt.getMonth() + 1;
            var NewStDt = stMonthNumber + '/' + offSt.substring(0, 2) + '/' + offSt.substring(7, 11)


            var enMonthName = offEn.substring(3, 6);
            var endt = new Date(enMonthName + '01, 1900');
            var enMonthName = endt.getMonth() + 1;
            var NewEnDt = enMonthName + '/' + offEn.substring(0, 2) + '/' + offEn.substring(7, 11)

            ///end///

            startDate = new Date(NewStDt);
            endDate = new Date(NewEnDt);



            if (startDate > endDate) {
                var Totleave = document.getElementById('Totleave');
                Totleave.innerHTML = '';
                alert("From date should be less than or equal to To date.");
               
                return false;
            }
            else {
              return true;
            }


        }

      
    }
     
   
</script>
<body>
    <form id="Form1" method="post" onsubmit="return chkvalues();" runat="server" action="empleaveApp.aspx?save=Yes">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" align="center">
        <tr>
            <td width="20%" height="41" align="center" bgcolor="#C5D5AE">
                <b><font face="Verdana" color="#a2921e" size="4">Employee Leave Details/Management</font></b>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <table width="100%" border="1" cellpadding="1" cellspacing="0" bordercolor="#C5D5AE">
                    <tr align="center">
                        <td align="left"> 
                            <table width="70%"  cellpadding="1" cellspacing="0" align="left">
                                <tr>
                                    <td  height="1" align="left">
                                        <font face="Verdana" size="2" color="#A2921E">Current Leave Balance:</font>
                                        <asp:Label ID="lblnoofleave" runat="server" Font-Names="Verdana" 
                                            Font-Size="9pt"></asp:Label>
                                    </td>
                                    
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table align="left">
                                <tr align="center">
                                    <td colspan="4" bgcolor="#C5D5AE" height="30">
                                        <b><font face="Verdana" color="#a2921e" size="2">Add Leave</font></b>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" bgcolor="#EDF2E6" height="29">
                                        <font face="Verdana" size="2" color="#A2921E">Leave Type </font>
                                    </td>
                                    <td width="276" bgcolor="#EDF2E6" colspan="3">
                                        <asp:DropDownList ID="dropLeaveType" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" bgcolor="#EDF2E6" height="29">
                                        <font face="Verdana" size="2" color="#A2921E">Leave From </font>
                                    </td>
                                    <td width="64" align="left" bgcolor="#EDF2E6">
                                        <asp:TextBox ID="txtDateFrom" size="7" onclick="popupCalender('txtDateFrom')" runat="server"
                                            Width="91px" onkeypress="return false;" />
                                    </td>
                                    <td width="31" bgcolor="#EDF2E6" height="29" align="center">
                                        <font face="Verdana" size="2" color="#A2921E">To</font>
                                    </td>
                                    <td width="276" align="left" bgcolor="#EDF2E6">
                                        <asp:TextBox ID="txtDateTo" size="7" onclick="popupCalender('txtDateTo')" runat="server"
                                            Width="91px" onkeypress="return false;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="140" bgcolor="#EDF2E6" height="29">
                                        <font face="Verdana" size="2" color="#A2921E">No. Of Leaves Applied</font>
                                    </td>
                                    <td width="64" align="left" bgcolor="#EDF2E6" colspan="3">
                                        <span id="Totleave" style="font-size: 9pt; font-family: Verdana"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" bgcolor="#EDF2E6" height="29">
                                        <font face="Verdana" size="2" color="#A2921E">Reason Of Leave </font>
                                    </td>
                                    <td colspan="3" align="left" bgcolor="#EDF2E6">
                                        <asp:TextBox ID="txtRofLeave" MaxLength="255" TextMode="SingleLine" Width="100%"
                                            runat="server" />
                                        <font face="Verdana" size="1" color="#A2921E"></font>
                                    </td>
                                </tr>
                                <tr align="center" bgcolor="#EDF2E6">
                                    <td height="44" colspan="4">
                                        <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click" OnClientClick="javascript:return CheckDate()" />
                                    </td>
                                    <asp:HiddenField ID="hdnf" runat="server" />
                                    <asp:HiddenField ID="hdnS" runat="server" />
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
