<%@ Page Language="VB" Debug="TRUE" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.string" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta name="GENERATOR" content="Microsoft FrontPage 5.0">
    <meta name="ProgId" content="FrontPage.Editor.Document">
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <title>View Change Request</title>

    <script language="JavaScript" src="/includes/calender.js"></script>

</head>

<script language="VB" runat="server">
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim Conn As SqlConnection = New SqlConnection(dsn)
    Dim Cmd As New SqlCommand()
    Dim i As Integer
    Dim strSQL, SQL As String
    Dim dtrInfo As SqlDataReader
    Dim chgid As Integer
    Dim chgTitle As String
    Dim chgDesc As String
    Dim chgFeedBack As String
    Dim chgProjId As Integer
    Dim chgDate As DateTime
    Dim chgEstTime As String
    Dim chgApprovedBy As String
    Dim chgCompDate As DateTime
    Dim chgApprovalDate As String
    Dim projname As String
    Dim Approved As String = "0"
    Dim gf As New generalFunction
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        '**************************************************************
        'Select the Feedback Which you Select on Previous Page
        '**************************************************************
        gf.checkEmpLogin()
        chgid = Request.QueryString("id")
        strSQL = "SELECT * FROM changeRequest where chgid=" & chgid
        Conn.Open()
				
        Cmd = New SqlCommand(strSQL, Conn)
        dtrInfo = Cmd.ExecuteReader()
				
        If dtrInfo.Read() Then
            chgid = dtrInfo("chgid")
            chgTitle = dtrInfo("chgTitle")
            chgDesc = dtrInfo("chgDesc")
            chgProjId = dtrInfo("chgProjId")
            chgDate = dtrInfo("chgDate")
					
            '-------------------7/27/2007----------------------------------
            If IsDBNull(dtrInfo("chgFeedBack")) Then
                chgFeedBack = ""
            Else
                chgFeedBack = dtrInfo("chgFeedBack")
            End If
            '-----------------------------------------------------
				
            If Page.IsPostBack = False Then
                If (dtrInfo("chgApproved") = "P") Then
                    lblApproved.Text = "Pending"
                    btnAccept.Visible = True
                    btnReject.Visible = True
                    trCD.Visible = False
                    trECT.Visible = False
                    trECT1.Visible = False
                    trAB.Visible = False
                End If
					
                If (dtrInfo("chgApproved") = "A") Then
                    lblApproved.Visible = True
                    lblApproved.Text = "Accepted"
                    btnAccept.Visible = False
                    btnReject.Visible = False
                    trCD.Visible = True
                    trECT.Visible = True
                    trECT1.Visible = True
                    trAB.Visible = True
                    fillEmp()
                    ddlemp.SelectedValue = dtrInfo("chgApprovedBy")
                    estComTime.Value = dtrInfo("chgEstTime")
                    If (IsDBNull(dtrInfo("chgCompDate"))) Then
                        txtDate.Text = ""
                    Else
                        txtDate.Text = Day(dtrInfo("chgCompDate")) & "-" & Left(MonthName(Month(dtrInfo("chgCompDate"))), 3) & "-" & Right(Year(dtrInfo("chgCompDate")), 2)
                    End If
						
                    trAD.Visible = True
                    lblApprovalDate.Text = Day(dtrInfo("chgApprovalDate")) & "-" & Left(MonthName(Month(dtrInfo("chgApprovalDate"))), 3) & "-" & Right(Year(dtrInfo("chgApprovalDate")), 2)
                End If
					
                If (dtrInfo("chgApproved") = "R") Then
                    lblApproved.Visible = True
                    lblApproved.Text = "Rejected"
                    btnAccept.Visible = False
                    btnReject.Visible = False
                    trCD.Visible = False
                    trECT.Visible = False
                    trECT1.Visible = False
                    trAB.Visible = False
                End If
					
                '-----------------------------------------------------
                If IsDBNull(dtrInfo("chgEstTime")) Then
                    chgEstTime = ""
                Else
                    chgEstTime = dtrInfo("chgEstTime")
                End If
                If IsDBNull(dtrInfo("chgEstCost")) Then
                    estComCost.Text = ""
                Else
                    estComCost.Text = dtrInfo("chgEstCost")
                End If
                '-----------------------------------------------------
					
					
                '-----------------------------------------------------
                If IsDBNull(dtrInfo("chgApprovedBy")) Then
                    chgApprovedBy = ""
                Else
                    chgApprovedBy = dtrInfo("chgApprovedBy")
                End If
                '-----------------------------------------------------
					
                '-----------------------------------------------------
                If IsDBNull(dtrInfo("chgCompDate")) Then
						
                Else
                    chgCompDate = dtrInfo("chgCompDate")
                End If
                '-----------------------------------------------------
					
                '-----------------------------------------------------
                If IsDBNull(dtrInfo("chgApprovalDate")) Then
						
                Else
                    chgApprovalDate = dtrInfo("chgApprovalDate")
                End If
                '-----------------------------------------------------
									
            End If
            Conn.Close()
            Cmd.Dispose()
        End If
        '***************************************************************
        'Select the Project Name By THE Project Id
        '***************************************************************
			
        SQL = "SELECT projName from projectMaster where projId =" & chgProjId
        Conn.Open()
        Cmd = New SqlCommand(SQL, Conn)
        dtrInfo = Cmd.ExecuteReader()
        If dtrInfo.Read() Then
            projname = dtrInfo("projName")
        End If
        Cmd.Dispose()
        Conn.Close()
    End Sub




    Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        lblApproved.Text = "Accepted"
        btnAccept.Visible = False
        btnReject.Visible = True
        trCD.Visible = True
        trECT.Visible = True
        trECT1.Visible = True
        trAB.Visible = True
        fillEmp()
    End Sub

    Sub btnReject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        lblApproved.Text = "Rejected"
        btnAccept.Visible = True
        btnReject.Visible = False
        trCD.Visible = False
        trECT.Visible = False
        trECT1.Visible = False
        trAB.Visible = False
    End Sub


 
    Sub fillEmp()
        Dim Con As SqlConnection
        Dim cmdEmp As SqlCommand
        Dim strEmp As String
        Dim dtrEmp As SqlDataReader
        Con = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
        strEmp = "select * from employeeMaster"
        Con.Open()
        cmdEmp = New SqlCommand(strEmp, Con)
        dtrEmp = cmdEmp.ExecuteReader()
        ddlemp.Items.Clear()
        Do While (dtrEmp.Read())
            ddlemp.Items.Add(New ListItem(dtrEmp("empName").ToString(), dtrEmp("empid").ToString()))
        Loop
        cmdEmp.Dispose()
        Con.Close()
    End Sub
 
 
    Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim i As Integer
        Dim cmd As SqlCommand
        Dim strUpdateSql As String
        Dim commenthistory As String
	
        Dim strApproved As String
        Dim comment As String
        Dim strEstTime As String
        Dim strApprovedBy As String
        Dim strCompDate As String
        Dim strApprovalDate As String
	
        comment = Request.Form("comment")
        commenthistory = Request.Form("comment_history")
	
        '********************* No of Days validation*********
	
        If (lblApproved.Text = "Accepted") Then
            If (estComTime.Value.Length = 0) Then
                Dim spval As String
                spval = "<script language='JavaScript'>"
                spval += "alert('Estimated Completion Time Required');"
                spval += "</" + "script>"
                RegisterStartupScript("script2", spval)
                'goto l1
            End If
        End If
        '********************* No of Days validation*********
	
        If comment.Length > 0 Then
            comment = Session("dynoEmpNameSession") & ":" & DateTime.Now & ":" & comment & vbCrLf & "--------------------------------------------" & vbCrLf & commenthistory
        Else
            comment = commenthistory
        End If
		
	 	
        If (lblApproved.Text = "Accepted") Then
            strApproved = "A"
            strEstTime = estComTime.Value
            strApprovedBy = ddlemp.SelectedValue
            If trAD.Visible = True Then
                strApprovalDate = lblApprovalDate.Text
            Else
                strApprovalDate = DateTime.Now
            End If
            If txtDate.Text.Length > 0 Then
                strCompDate = txtDate.Text
                strUpdateSql = "Update changeRequest set chgApproved = " & "'" & sqlsave(strApproved) & "'" & ",chgFeedback='" & sqlsave(comment) & "'" & ",chgEstTime = '" & sqlsave(strEstTime) & "',chgApprovedBy = " & "'" & sqlsave(strApprovedBy) & "'" & ",chgCompDate =" & "'" & strCompDate & "'" & ",chgApprovalDate = " & "'" & strApprovalDate & "',chgEstCost='" & estComCost.Text & "' where chgid=" & chgid
            Else
                strUpdateSql = "Update changeRequest set chgApproved = " & "'" & sqlsave(strApproved) & "'" & ",chgFeedback='" & sqlsave(comment) & "'" & ",chgEstTime = '" & sqlsave(strEstTime) & "',chgApprovedBy = " & "'" & sqlsave(strApprovedBy) & "'" & ",chgApprovalDate = " & "'" & strApprovalDate & "',chgEstCost='" & sqlsave(estComCost.Text) & "' where chgid=" & chgid
			
            End If
        End If
	
        If (lblApproved.Text = "Rejected") Then
            strApproved = "R"
            strUpdateSql = "Update changeRequest set chgApproved = " & "'" & sqlsave(strApproved) & "'" & ",chgFeedback='" & sqlsave(comment) & "'" & " where chgid=" & chgid
        End If
	
        If (lblApproved.Text = "Pending") Then
            strUpdateSql = "Update changeRequest set chgFeedback='" & sqlsave(comment) & "' where chgid=" & chgid
        End If
	
        '*******************************************************************************
        'Update the changrRequest Table
        '********************************************************************************	
			
        cmd = New SqlCommand(strUpdateSql, Conn)
        cmd.Connection.Open()
        cmd.ExecuteNonQuery()
   		
        Session("type") = "atc"
        Session("projid") = chgProjId
        Session("body") = Request.Form("comment")
        Session("msgType") = "Change Request"
        Response.Redirect("../~controls/sendMail.aspx?home=1")
        Response.Write("<script language='javascript'> { window.close() ; } </" & "script>")
    End Sub
    Function sqlsave(ByVal str As String) As String
        str = str.Replace("'", "''")
        Return str
    End Function
 
    Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    End Sub
 
</script>

<body topmargin="0" leftmargin="0">
    <form id="Form1" runat="server">
        <table align="center" height="156" cellspacing="0" cellpadding="4" width="100%" border="0"
            style="border-collapse: collapse" bordercolor="#111111">
            <tr>
                <td height="36" width="549">
                    <b><font face="Verdana">Change Request</font></b></td>
            </tr>
            <td bgcolor="#C5D5AE" align="left" height="18" width="549">
                <font face="Verdana" color="black" size="2"><b>Project</b></font>
                <!--<select name="client_id"  onChange="document.forms[0].submit()"></td>-->
                &nbsp;&nbsp;&nbsp;&nbsp;<font face="Verdana" color="black" size="2"><%=projName%></font>
                <font face="verdana" color="black" size="2"><b>&nbsp;&nbsp;&nbsp;Date</b></font>
                &nbsp;&nbsp;<font face="Verdana" color="black" size="2"><%=day(chgDate)%>-<%=left(monthname(month(chgDate)),3)%>-<%=right(year(chgDate),2)%></font>
            </td>
            <tr>
                <td height="25" valign="top" width="549">
                    <table border="1" cellpadding="4" cellspacing="1" style="border-collapse: collapse"
                        bordercolor="white" width="635" id="AutoNumber1">
                        <tr>
                            <td width="25%" bgcolor="#C5D5AE" nowrap="nowrap">
                                <font face="verdana" size="2" color="#0000FF">&nbsp;Change Title:</font></td>
                            <td width="75%" valign="middle" bgcolor="#edf2e6">
                                <font face="verdana" size="2" color="black"><b>
                                    <%=chgTitle%>
                                </b></font>&nbsp;</td>
                        </tr>
                        <tr>
                            <td width="25%" bgcolor="#C5D5AE" nowrap="nowrap">
                                <font face="verdana" size="2" color="#0000FF">&nbsp;Change Description:</font></td>
                            <td width="75%" valign="middle" bgcolor="#edf2e6">
                                <font face="verdana" size="2" color="black"><b>
                                    <%=chgDesc%>
                                </b></font>&nbsp;</td>
                        </tr>
                        <tr>
                            <td width="25%" bgcolor="#C5D5AE" nowrap="nowrap">
                                <font face="verdana" size="2" color="#0000FF">&nbsp;Change Approval:</font></td>
                            <td width="25%" valign="middle" bgcolor="#edf2e6">
                                <asp:Label ID="lblApproved" runat="server" Font-Bold="True" Font-Size="10pt" Font-Names="Verdana"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnAccept"
                                    runat="server" Width="90px" Text="Accept" OnClick="btnAccept_Click"></asp:Button>
                                <asp:Button ID="btnReject" runat="server" Width="90px" Text="Reject" OnClick="btnReject_Click">
                                </asp:Button>
                            </td>
                        </tr>
                        <tr id="trECT" runat="server">
                            <td width="25%" bgcolor="#C5D5AE" nowrap="nowrap">
                                <font face="verdana" size="2" color="#0000FF">&nbsp;Estimated Completion Time :</font></td>
                            <td width="75%" valign="middle" bgcolor="#edf2e6">
                                <input id="estComTime" size="3" runat="server" type="text" maxlength="3"><font face="verdana"
                                    size="2" color="black"><b>&nbsp;&nbsp;Days</b></font></td>
                        </tr>
                        <tr id="trECT1" runat="server">
                            <td width="25%" bgcolor="#C5D5AE" nowrap="nowrap">
                                <font face="verdana" size="2" color="#0000FF">&nbsp;Estimated Completion Cost :</font></td>
                            <td width="75%" valign="middle" bgcolor="#edf2e6">
                                <asp:TextBox ID="estComCost" runat="server" onKeyUp="if(isNaN(this.value)){alert('Invalid number');return false;}" /></td>
                        </tr>
                        <tr id="trAB" runat="server">
                            <td width="25%" bgcolor="#C5D5AE" nowrap="nowrap">
                                <font face="verdana" size="2" color="#0000FF">&nbsp;Approved By :</font></td>
                            <td width="75%" valign="middle" bgcolor="#edf2e6">
                                <asp:DropDownList ID="ddlemp" runat="server" CssClass="cssData">
                                </asp:DropDownList></td>
                        </tr>
                        <tr id="trAD" runat="server" visible="False">
                            <td width="25%" bgcolor="#C5D5AE" nowrap="nowrap">
                                <font face="verdana" size="2" color="#0000FF">&nbsp;Approval Date :</font></td>
                            <td width="75%" valign="middle" bgcolor="#edf2e6">
                                <asp:Label ID="lblApprovalDate" runat="server" Font-Bold="True" Font-Size="10pt"
                                    Font-Names="Verdana"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trCD" runat="server">
                            <td width="25%" bgcolor="#C5D5AE" nowrap="nowrap">
                                <font face="verdana" size="2" color="#0000FF">&nbsp;Completion Date :</font></td>
                            <td width="75%" valign="middle" bgcolor="#edf2e6">
                                <asp:TextBox ID="txtDate" runat="server" size="14" onclick="popupCalender('txtDate')"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td width="25%" bgcolor="#C5D5AE" nowrap="nowrap">
                                <font face="verdana" size="2" color="#0000FF">&nbsp;Comments:</font></td>
                            <td width="75%" valign="middle" bgcolor="#edf2e6">
                                <textarea rows="3" name="comment" cols="48"></textarea></center></td>
                        </tr>
                        <tr>
                            <td width="25%" bgcolor="#C5D5AE" nowrap="nowrap">
                                <font face="verdana" size="2" color="#0000FF">&nbsp;Comment History:</font></td>
                            <td width="75%" valign="middle" bgcolor="#EAEAEA">
                                <textarea rows="5" name="comment_history" cols="48" readonly="true" style="color: #0000FF;
                                    border: 1px solid #F4F4F4"><%=chgfeedback%></textarea></center></td>
                        </tr>
                        <tr height="7">
                            <td td width="25%" bgcolor="#C5D5AE" nowrap="nowrap">
                                <font face="verdana" size="2" color="#0000FF">Attachments:</font>
                            </td>
                            <!--start code here for attachment-->
                            <td bgcolor="#EAEAEA" width="75%" valign="middle">
                                <div style="overflow-x: hidden; overflow: scroll; height: 100px">
                                    <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                        <%	
                                            'response.write (intRepId)
                                            Dim strFileName As String
                                            Dim dtrFileName As SqlDataReader
                                            Dim strFileCmd As SqlCommand
                                            Conn.Open()
                                            strFileName = "select chgattachmentFile from Changerequestattachment where chgid=" & chgid & ""
			 
                                            strFileCmd = New SqlCommand(strFileName, Conn)
                                            dtrFileName = strFileCmd.ExecuteReader
                                            If dtrFileName.HasRows Then
                                                While dtrFileName.Read
                                        %>
                                        <tr>
                                            <td>
                                                <a href="/Attachment/complaint/<%=dtrFileName("chgattachmentFile")%>" target="_blank">
                                                    <font face="Verdana" size="2" color="#A2921E">
                                                        <%=dtrFileName("chgattachmentFile")%>
                                                    </font></a>
                                            </td>
                                        </tr>
                                        <%
                                        End While
                                    End If
                                    strFileCmd.Dispose()
                                    dtrFileName.Close()
                                    Conn.Close()
                                        %>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td width="100%" bgcolor="#C5D5AE" colspan="2">
                                <p align="center">
                                    <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Width="55px" Text="Submit">
                                    </asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <input id="btnclose" type="button" onclick="javascript:window.close();" value="Close"
                                        width="110px" text="Close" />
                            </td>
                        </tr>
                    </table>
                    <input type="hidden" id="hdnid" runat="server">
                    <input type="hidden" id="repId" runat="server">
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
