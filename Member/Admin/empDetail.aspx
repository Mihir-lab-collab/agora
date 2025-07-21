<%@ Page Language="VB" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Register TagPrefix="ucl" TagName="adminMenu" Src="~/controls/adminMenu.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
	<title>Dyno Admin Control</title>
  <link rel="stylesheet" href="../includes/CalendarControl.css" type="text/css" />
	<script language="JavaScript" type="text/javascript" src="/includes/CalendarControl.js">
	</script>

</head>
<body>
	<ucl:adminMenu ID="adminMenu" runat="server" />
	<div id="d" runat="server" />

	<script language="VB" runat="server">
		Dim SortField As String
		Dim empCode As String
		Dim sql As String
		Dim sql1 As String
		Dim rights As Integer
	    Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
		Dim conn As SqlConnection = New SqlConnection(dsn1)
	    Dim fid As Integer
   
		Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
			conn.Open()
			If Not IsPostBack Then 'bind for first time page load only
				loadData()
			End If
		End Sub

		Sub addBT_OnClick(ByVal objSource As Object, ByVal objArgs As EventArgs)
			If empId.Value = "" Then
				Dim Ex As Integer
				Ex = 12 * dropempExpyears.Value + dropempExpmonths.Value
				sql = "INSERT INTO employeeMaster(empName,empAddress,empContact" & _
			 ",skillId,empNotes,empJoiningDate,empProbationPeriod,empEmail,empAccountNo,empBDate,empADate,empPrevEmployer,empExperince) VALUES('" & empName.Value & _
				"','" & empAddress.Value & "','" & empContact.Value & "'," & _
				empSkill.Value & ",'" & empNotes.Value & "','" & empJoiningDate.Value & "'," & _
				empProbationPeriod.Value & ",'" & empEmail.Value & "','" & empAccountno.Value & "','" & empBDate.Value & "','" & empADate.Value & "','" & empPrevEmployer.Value & "','" & Ex & "')" & _
				" select @id=@@identity from employeemaster "
	
				Dim Cmd1 As New SqlCommand(sql, conn)
				Cmd1.Parameters.Add("@id", SqlDbType.Int)
				Cmd1.Parameters("@id").Direction = ParameterDirection.Output
				Cmd1.ExecuteNonQuery()
	            fid = Cmd1.Parameters("@id").Value
				Dim arr(20) As String
				Dim item As ListItem
				Dim i As Integer = 0
				For Each item In lstempQual.Items
					If item.Selected Then
						arr(i) = item.Value
	                    sql1 = "insert into empQualification(empId,qualificationId) values('" & fid & "','" & arr(i) & "')"
						Dim cmd2 As New SqlCommand(sql1, conn)
						cmd2.ExecuteNonQuery()
					End If
					i = i + 1
				Next
		     	
			Else
	   
	            Dim a As Integer = -1
				If right1.Checked = True Or CInt(empSkill.Value) = 1 Or CInt(empSkill.Value) = 2 Then
					a = 1
				End If
				If right2.Checked = True Then
					a = 0
				End If
				Dim Ex As Integer
				Ex = 12 * dropempExpyears.Value + dropempExpmonths.Value
				sql = "UPDATE employeeMaster SET empName='" & empName.Value & _
				"',empAddress='" & empAddress.Value & "',empContact='" & _
				empContact.Value & "',skillId=" & empSkill.Value & " ,empTester=" & a & _
				",empNotes='" & empNotes.Value & "',empJoiningDate='" & _
				empJoiningDate.Value & "'"
				If IsDate(empLeavingDate.Value) Then
					sql = sql & " ,empLeavingDate='" & empLeavingDate.Value & "'"
				End If
				If IsDate(empBDate.Value) Then
					sql = sql & ", empBDate ='" & empBDate.Value & "'"
				End If
				If IsDate(empADate.Value) Then
					sql = sql & ", empADate ='" & empADate.Value & "'"
				End If
                If (dropempExpyears.Visible = True) And (dropempExpmonths.Visible = True) Then
	                sql = sql & ", empExperince ='" & Ex & "'"
	            End If
				sql = sql & ",empProbationPeriod=" & _
	            empProbationPeriod.Value & ",empEmail='" & empEmail.Value & "', empAccountNo='" & empAccountno.Value & "', empPrevEmployer='" & empPrevEmployer.Value & "' WHERE empid=" & empId.Value

		
				Dim cmd3 As New SqlCommand(sql, conn)
				cmd3.ExecuteNonQuery()
				Dim sql3 As String
				sql3 = "delete from empQualification where empId=" & empId.Value
				Dim cmd5 As New SqlCommand(sql3, conn)
				cmd5.ExecuteNonQuery()
				Dim sql6 As String
				Dim arr(100) As String
				Dim item As ListItem
				Dim i As Integer
				For Each item In lstempQual.Items
					If item.Selected Then
						arr(i) = item.Value
						sql6 = "insert into empQualification(empId,qualificationId) values('" & empId.Value & "','" & arr(i) & "')"
						Dim cmd4 As New SqlCommand(sql6, conn)
						cmd4.ExecuteNonQuery()
					End If
					i = i + 1
				Next
			End If
	
			Dim sp As String
			sp = "<script language='JavaScript'>"
			sp += "alert('Record has been add/updated successfully.');"
	        sp += "</" + "script>"
	        d.InnerHtml = "Record has been add/updated successfully. " & _
	      "<a href='empView.aspx'>Click here</a> to continue."
		End Sub
 
		Sub Show_OnClick(ByVal objSource As Object, ByVal objArgs As EventArgs)
			    empCode = Request.QueryString("empId")
	        ' This is for Edit Button
	        ' Just for time being this is commented
	        
	        
	        
	        'If empId.Value <> "" Then
	        '	If dropempExpyears.Disabled = True Then
	        '		dropempExpyears.Disabled = False
	        '	End If
	        '	If dropempExpmonths.Disabled = True Then
	        '		dropempExpmonths.Disabled = False
	        '	End If
	        '	Dim sqlstr As String
	        '	Dim dtrShow As SqlDataReader
	        '	sqlstr = "select empid, empPassword, skillid, empName, empAddress, empContact, empJoiningDate, " & _
	        ' " empLeavingDate, empProbationPeriod, empNotes, empEmail, empTester, empAccountNo, empBDate, " & _
	        ' " empADate, empPrevEmployer,  empExperince " & _
	        ' " from EMPLOYEEMASTER where empid=" & empCode
	        '	Dim cmdshow As New SqlCommand(sqlstr, conn)
	        '	dtrShow = cmdshow.ExecuteReader()
	        '	If dtrShow.Read() Then
	        '		dropempExpyears.Value = Math.Floor(dtrShow("empExperince") / 12)
	        '		dropempExpmonths.Value = dtrShow("empExperince") Mod 12
	        '	End If
	        'End If
	      
	        dvlblExp.Visible = False
	        dvExp.Visible = True
	         btnCancel.Visible =True 
	        hidenShow("show")
		End Sub
            Sub Cancel_OnClick(ByVal objSource As Object, ByVal objArgs As EventArgs)
	        'ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(String), "hideshow", "hideshow(1);", True)
	     
	        hidenShow("hide")
	        dvExp.Visible = False
	        dvlblExp.Visible = True
	        
	    End Sub
	    Sub hidenShow(ByVal value As String)
	        If (value = "hide") Then
	            dropempExpyears.Visible = False
	            dropempExpmonths.Visible = False
	            
	        Else
	            dropempExpyears.Visible = True
	            dropempExpmonths.Visible = True
	        End If
	    End Sub
	Sub loadData()
	      
        empCode = Request.QueryString("empId")
        If (empCode = "") Then
	            dvlblExp.Visible = False
	            btnCancel.Visible =False 
	            dvExp.Visible = True
	            'ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(String), "hideshow", "hideshow(0);", True)
	           
	            hidenShow("Show")
	        
	        Else
	            dvExp.Visible = False 
	            dvlblExp.Visible = True
	             btnCancel.Visible =True 
	            hidenShow("hide")
	          
	          
	            'ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(String), "hideshow", "hideshow(1);", True)
        End If
	        
        Dim Rdr As SqlDataReader
        sql = "SELECT * FROM skillMaster"
        Dim Cmd1 As New SqlCommand(sql, conn)
        Rdr = Cmd1.ExecuteReader()
        empSkill.DataSource = Rdr
        empSkill.DataTextField = "skillDesc"
        empSkill.DataValueField = "skillId"
        empSkill.DataBind()
        Rdr.Close()
       
        sql = "SELECT * FROM empQualificationMaster ORDER BY qualificationDesc"
        Cmd1.CommandText = sql
        Rdr = Cmd1.ExecuteReader()
        lstempQual.DataSource = Rdr
        lstempQual.DataTextField = "qualificationDesc"
        lstempQual.DataValueField = "qualificationId"
        lstempQual.DataBind()
        Rdr.Close()

        empCode = Request.QueryString("empId")
        If empCode <> "" Then
         
            sql = "select empid, empPassword, skillid, empName, empAddress, empContact, empJoiningDate, " & _
        " empLeavingDate, empProbationPeriod, empNotes, empEmail, empTester, empAccountNo, empBDate, " & _
        " empADate, empPrevEmployer, empExperince,datediff(month,empJoiningdate,getdate())+ empExperince as TotalExp " & _
        " from EMPLOYEEMASTER where empid=" & empCode

            Dim Cmd2 As New SqlCommand(sql, conn)
            Rdr = Cmd2.ExecuteReader()

            If Rdr.Read() Then
                empId.Value = Rdr("empId")
                empName.Value = Rdr("empName")
                empAddress.Value = Rdr("empAddress") & ""
                empContact.Value = Rdr("empContact") & ""
                empSkill.Value = Rdr("skillId")
                '			emp_proj.value =Rdr("projName")     	
                empJoiningDate.Value = Rdr("empJoiningDate")
                empEmail.Value = Rdr("empEmail")
                empProbationPeriod.Value = Rdr("empProbationPeriod")
                If IsDate(Rdr("empLeavingDate")) Then
                    empLeavingDate.Value = Rdr("empLeavingDate")
                End If
                If Rdr.IsDBNull(0) Then
                    empAccountno.Value = ""
                Else
                    empAccountno.Value = Rdr("empAccountNo") & ""
                End If
                empNotes.Value = Rdr("empNotes") & ""
                If IsDate(Rdr("empBDate")) Then
                    empBDate.Value = Day(Rdr("empBDate")) & "-" & Mid(MonthName(Month(Rdr("empBDate"))), 1, 3) & "-" & Year(Rdr("empBDate"))
                End If
                If IsDate(Rdr("empADate")) Then
                    empADate.Value = Day(Rdr("empADate")) & "-" & Mid(MonthName(Month(Rdr("empADate"))), 1, 3) & "-" & Year(Rdr("empADate"))
                End If
                If Rdr("empTester") = True Or CInt(Rdr("skillId")) = 1 Or CInt(Rdr("skillId")) = 2 Then
                    right1.Checked = True
                Else
                    right2.Checked = True
                End If
                rights = CInt(Rdr("empTester"))
			
                If Not IsDBNull(Rdr("empAccountNo")) Then
                    empAccountno.Value = Rdr("empAccountNo")
                End If
    		
                If Not IsDBNull(Rdr("empPrevEmployer")) Then
                    empPrevEmployer.Value = Rdr("empPrevEmployer")
                End If

                If Not IsDBNull(Rdr("empExperince")) Then
                    dropempExpyears.Value = Math.Floor(Rdr("empExperince") / 12)
                    dropempExpmonths.Value = Rdr("empExperince") Mod 12
                End If
	                
                If Not IsDBNull(Rdr("TotalExp")) Then
                    lblExp.Text = Math.Floor(Rdr("TotalExp") / 12) & " yrs " & Rdr("TotalExp") Mod 12 & " months"

                  
                End If
            
	                
            End If
            Rdr.Close()
	 
            sql1 = "select * from empQualification where empid=" & empCode
            Dim item As ListItem
            Dim Cmd3 As New SqlCommand(sql1, conn)
            Rdr = Cmd3.ExecuteReader()
            While Rdr.Read
                For Each item In lstempQual.Items
                    If item.Value = Rdr("qualificationId") Then
                        lstempQual.Items(lstempQual.Items.IndexOf(item)).Selected = True
                    End If
                Next
            End While
            Rdr.Close()
        End If
    End Sub

	</script>

	<script language='JavaScript' type="text/javascript">

	    function changeright() {
	        var x = document.getElementById("empSkill")
	        var y = document.getElementById("right1")
	        var z = document.getElementById("right2")
	        if ((x.options[x.selectedIndex].value == 1) || (x.options[x.selectedIndex].value == 2)) {
	            y.checked = true
	            z.disabled = true
	        }

	        else {
	            z.checked = true
	            z.disabled = false

	        }
	    } 

	</script>

	<form runat="server" id="custForm">
		<table cellpadding="4" width="897" align="center" border="1" style="border-collapse: collapse;
			border-color: #e8e8e8; height: 310">
			<tr>
				<td width="214" height="23" bgcolor="#edf2e6">
					<font face="Verdana" color="#a2921e" size="2"><b>Employee Name</b></font></td>
				<td width="219" colspan="4" height="23">
					<input type="hidden" id="empId" runat="server" name="empcode1" size="20" />
					<input type="text" id="empName" runat="server" size="20" />&nbsp;</td>
				<td width="214" height="23" bgcolor="#edf2e6">
					<font face="Verdana" color="#a2921e" size="2"><b>Joining Date</b></font></td>
				<td width="213" height="23">
					<input type="text" id="empJoiningDate" runat="server" size="20" onclick="popupCalender('empJoiningDate')" /></td>
			</tr>
			<tr>
				<td width="214" height="36" bgcolor="#edf2e6">
					<font face="Verdana" color="#a2921e" size="2"><b>Address</b></font></td>
				<td width="219" colspan="4" height="36">
					<textarea id="empAddress" runat="server" rows="2" cols="20" name="empAddress"></textarea>
					&nbsp;</td>
				<td width="214" height="36" bgcolor="#edf2e6">
					<font face="Verdana" color="#a2921e" size="2"><b>Leaving Date</b></font></td>
				<td width="213" height="36">
					<input type="text" id="empLeavingDate" runat="server" size="20" name="3" onclick="popupCalender('empLeavingDate')" /></td>
			</tr>
			<tr>
				<td width="214" height="22" bgcolor="#edf2e6">
					<font face="Verdana" color="#a2921e" size="2"><b>Contact Number</b></font></td>
				<td width="219" colspan="4" height="22">
					<input type="text" id="empContact" runat="server" size="20" name="empContact" /></td>
				<td width="214" height="22" bgcolor="#edf2e6">
					<font face="Verdana" color="#a2921e" size="2"><b>Probation Period</b></font></td>
				<td width="213" height="22">
					<select runat="server" id="empProbationPeriod" name="empProbationPeriod">
						<option value="0">0</option>
						<option value="1">1</option>
						<option value="2" selected="selected">2</option>
						<option value="3">3</option>
						<option value="4">4</option>
						<option value="5">5</option>
						<option value="6">6</option>
					</select>
				</td>
			</tr>
			<tr>
				<td align="center" width="214" height="22" bgcolor="#edf2e6">
					<p align="left">
						<font face="Verdana" color="#a2921e" size="2"><b>Email Id</b></font></p>
				</td>
				<td align="center" width="219" colspan="4" height="22">
					<p align="left">
						<input type="text" name="empEmail" runat="server" id="empEmail" size="30"/></p>
				</td>
				<td align="left" width="214" height="22" bgcolor="#edf2e6">
					<font face="Verdana" color="#a2921e" size="2"><b>Designation</b></font></td>
				<td align="left" width="213" height="22">
					<select size="1" id="empSkill" name="combo_skill" runat="server" onchange="changeright()">
					</select>
				</td>
			</tr>
			<tr>
				<td width="214" height="23" bgcolor="#edf2e6">
					<font face="Verdana" color="#a2921e" size="2"><b>Rights</b></font></td>
				<td width="219" colspan="4" height="23">
					<asp:CheckBox ID="right1" runat="server" Text="Admin" Font-Names="Verdana" Font-Size="X-Small"
						ForeColor="#a2921e" Font-Bold="True"></asp:CheckBox>
					<asp:CheckBox ID="right2" runat="server" Text="Tester" Font-Names="Verdana" Font-Size="X-Small"
						ForeColor="#a2921e" Font-Bold="True"></asp:CheckBox></td>
				<td width="214" height="23" bgcolor="#edf2e6">
					<font face="Verdana" color="#a2921e" size="2"><b>Account No.</b></font></td>
				<td width="213" height="23">
					<input type="text" id="empAccountno" runat="server" size="20" /></td>
			</tr>
			<tr>
				<td width="214" height="23" bgcolor="#edf2e6">
					<font face="Verdana" color="#a2921e" size="2"><b>Employee BirthDate</b></font></td>
				<td width="219" colspan="4" height="23">
					<input type="text" id="empBDate" runat="server" name="empBDate" size="20" onclick="popupCalender('empBDate')" />
				</td>
				<td width="214" height="23" bgcolor="#edf2e6">
					<font face="Verdana" color="#a2921e" size="2"><b>Anniversary Date</b></font></td>
				<td width="213" height="23">
					<input type="text" id="empADate" runat="server" size="20" onclick="popupCalender('empADate')" /></td>
			</tr>
			<tr>
				<td align="center" width="214" height="22" bgcolor="#edf2e6">
					<p align="left">
						<font face="Verdana" color="#a2921e" size="2"><b>Previous Employer</b></font></p>
				</td>
				<td width="219" colspan="4" height="36">
					<textarea id="empPrevEmployer" runat="server" rows="4" cols="20" name="empPrevEmployer"></textarea>
					&nbsp;</td>
				<td align="left" width="214" height="22" bgcolor="#edf2e6">
					<font face="Verdana" color="#a2921e" size="2"><b>Qualification</b></font></td>
				<td align="left" width="213" height="22">
					<asp:ListBox ID="lstempQual" runat="server" Rows="4" cols="40" Name="empQualification"
						SelectionMode="Multiple" EnableViewState="true"></asp:ListBox>
				</td>
			</tr>
			<tr>
				<td bgcolor="#edf2e6">
					<font face="Verdana" color="#a2921e" size="2"><b>Experience</b></font></td>
				<td colspan="4">
                <div id="dvlblExp" runat="server" visible="false">
                
                    <asp:Label ID="lblExp" runat="server" Font-Names="Verdana" Font-Size="Smaller" ></asp:Label> &nbsp;
                    <input type="button" id="btnExp" runat="server" value="Edit" style="font-family: Verdana;
						font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE" font-bold="true"
						onserverclick="Show_OnClick" />
                    </div>
					<div id="dvExp" runat="server" visible="false" >
					<span style="font-size:smaller ;">Year:</span> <select runat="server" id="dropempExpyears" name="empExpyears">
						<option value="0" selected="selected">0</option>
						<option value="1">1</option>
						<option value="2">2</option>
						<option value="3">3</option>
						<option value="4">4</option>
						<option value="5">5</option>
						<option value="6">6</option>
						<option value="7">7</option>
						<option value="8">8</option>
						<option value="9">9</option>
						<option value="10">10</option>
						<option value="11">11</option>
						<option value="12">12</option>
						<option value="13">13</option>
						<option value="14">14</option>
						<option value="15">15</option>
						<option value="16">16</option>
						<option value="17">17</option>
						<option value="18">18</option>
						<option value="19">19</option>
						<option value="20">20</option>
						<option value="21">21</option>
						<option value="22">22</option>
						<option value="23">23</option>
						<option value="24">24</option>
						<option value="25">25</option>
					</select>
					<span style="font-size:smaller ;">Month:</span> <select runat="server" id="dropempExpmonths" name="empExpmonths">
						<option value="0" selected="selected">0</option>
						<option value="1">1</option>
						<option value="2">2</option>
						<option value="3">3</option>
						<option value="4">4</option>
						<option value="5">5</option>
						<option value="6">6</option>
						<option value="7">7</option>
						<option value="8">8</option>
						<option value="9">9</option>
						<option value="10">10</option>
						<option value="11">11</option>
						<option value="12">12</option>
					</select>
                        <div style=" text-align:center; padding:5px 10px 0 0"><input type="button" id="btnCancel" runat="server" value="Cancel" style="font-family: Verdana;font-size: 8pt; color: #A2921E; font-weight: bold;  background-color: #C5D5AE " font-bold="true" onserverclick="Cancel_OnClick" /></div>
				</div>
              
				</td>
				<td colspan="2">&nbsp;</td>
			</tr>
			<tr>
				<td width="887" colspan="7" align="center" height="122" bgcolor="#edf2e6">
					<font face="Verdana" color="#a2921e" size="2"><b>Notes</b></font>
					<br />
					<textarea id="empNotes" runat="server" rows="5" cols="70"></textarea>
				</td>
			</tr>
			<tr>
				<td colspan="7" width="887" height="27" align="center">
					<input type="button" id="addBT" runat="server" value="Save" align="right" style="font-family: Verdana;
						font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE" font-bold="true"
						onserverclick="addBT_OnClick"/>
				</td>
			</tr>
		</table>
	</form>
</body>
</html>
