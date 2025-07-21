s<%@ Page Language="VB" %>
<%@Import Namespace="System.Data"%>
<%@Import Namespace="System.Data.SqlClient"%>
<script language="VB" runat="server">
     Dim IntProjId as Integer
    Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
	 Dim conn As SqlConnection = New SqlConnection(dsn1)
	 Dim strSQL as string
    Dim gf As New generalFunction
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        gf.checkEmpLogin()
        IntProjId = CInt(Request.QueryString("projId"))
        btnClose.Attributes.Add("OnClick", "javascript:window.close();")
        If Not IsPostBack Then
            strSQL = "select * from projectMaster where projId=" & IntProjId
            conn.Open()
            Dim cmd As SqlCommand = New SqlCommand(strSQL, conn)
            Dim Rdr As SqlDataReader
            Rdr = cmd.ExecuteReader()
            If Rdr.HasRows Then
                If Rdr.Read Then
                    projName.Value = Rdr("projName")
                End If
            End If
            cmd.Dispose()
            Rdr.Close()
            conn.Close()
      
            BindDropDown(IntProjId)
            btnEdit.Enabled = False
            btnAdd.Enabled = False
 
        End If
   
        If ddlModule.SelectedItem.Value = 0 Then
            btnEdit.Enabled = False
            btnAdd.Enabled = False
        Else
            btnEdit.Enabled = True
            btnAdd.Enabled = True
        End If
	 
    End Sub
 Sub BindDropDown(IntProjId as Integer)
     ddlModule.items.clear()
     ddlModule.items.add(new listitem("Select Module",0))
     strSQL=""
    strSQL="select * from projectModuleMaster where projId="& IntProjId
       Dim cmd As SqlCommand = New SqlCommand(strSQL,conn)
        Conn.Open()
	Dim Rdr As SqlDataReader
   Rdr=Cmd.ExecuteReader()
   If Rdr.HAsRows then
     while Rdr.Read 
         ddlModule.items.add(new listitem(Rdr("moduleName"),Rdr("moduleId")))
     End While
   End If
   Cmd.Dispose
   Rdr.Close
   Conn.Close
  
 End Sub
 
 Sub ddlModule_SelectedIndexChanged(sender As Object, e As EventArgs)
  strModuleName.Value=""
   txtModuleName.Value=""
   txtModuleDesc.Text=""
   if ddlModule.selecteditem.value=0 then
     btnSubmit.Enabled="true"
    txtModuleName.Disabled="False"
    txtModuleDesc.Enabled="true"
   else
     btnSubmit.Enabled="False"
    txtModuleName.Disabled="True"
    txtModuleDesc.Enabled="False"
   end if
   
 End Sub
 
 Sub Edit_Click(sender As Object, e As EventArgs)
       strSQL=""
    strSQL="select * from projectModuleMaster where moduleId="& ddlModule.SelectedItem.Value
       Dim cmd As SqlCommand = New SqlCommand(strSQL,conn)
        Conn.Open()
	Dim Rdr As SqlDataReader
   Rdr=Cmd.ExecuteReader()
   If Rdr.HAsRows then
     If Rdr.Read 
         txtModuleName.Value=Rdr("moduleName")
       If IsDbNull(Rdr("moduleDescription"))=False Then
         txtModuleDesc.Text=Rdr("moduleDescription")
       End IF
     End IF
   End If
   Cmd.Dispose
   Rdr.Close
   Conn.Close 
   
    txtModuleName.Disabled="False"
    txtModuleDesc.Enabled="true"
	strFlag.Value="update"
	btnSubmit.Enabled="True"
 End Sub
 
 Sub Add_Click(sender As Object, e As EventArgs)
    strModuleName.Value=ddlModule.SelectedItem.Text
    strFlag.Value="insert"
     txtModuleName.Disabled="False"
    txtModuleDesc.Enabled="true"
    txtModuleName.Value=""
    txtModuleDesc.Text=""
    btnSubmit.Enabled="True"
    
 End Sub
 
 Sub Submit_Click(sender As Object, e As EventArgs)
 dim minModuleID as integer
 dim Cmd as SqlCommand
minModuleID=1
  
     strSQL=""
   if txtModuleName.value<>"" then
    If strFlag.Value="update" Then
      strSQL="Update projectModuleMaster Set ModuleName='"& Trim(replace(txtModuleName.Value,"'","''")) &"',ModuleDescription='"& Trim(replace(txtModuleDesc.Text,"'","''")) &"' where moduleId="& ddlModule.SelectedItem.Value
    Elseif strFlag.Value="insert" Then
      strSQL="Insert Into projectModuleMaster(projId,moduleRefId,ModuleName,ModuleDescription)Values("& IntProjId &","& ddlModule.SelectedItem.Value &",'"& Trim(replace(txtModuleName.Value,"'","''")) &"','"& Trim(replace(txtModuleDesc.Text,"'","''")) &"')"
    Else
      strSQL="Insert Into projectModuleMaster(projId,moduleRefId,ModuleName,ModuleDescription)Values("& IntProjId &","& trim(minModuleID) &",'"& Trim(replace(txtModuleName.Value,"'","''")) &"','"& Trim(replace(txtModuleDesc.Text,"'","''")) &"')"
   End If

     cmd= New SqlCommand(strSQL,conn)
     Conn.Open()
	 Cmd.ExecuteNonQuery()
     Cmd.Dispose
     Conn.Close 
    Else
            Dim sp As String
            sp = "<Script language=JavaScript>"
            sp += " alert('Please Enter Valid Module Name');"
            sp +="document.forms[0].txtModuleName.focus()"
            sp += "</" + "script>"
            ClientScript.RegisterStartupScript(Me.GetType, "script223", sp)
    End if
    txtModuleName.Value=""
    txtModuleDesc.Text=""
     BindDropDown(IntProjId)
 End Sub
</script>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>Project Module</title>
</head>
<style>
		H3 { PADDING-RIGHT: 1px; PADDING-LEFT: 1px; FONT-WEIGHT: bold; PADDING-BOTTOM: 1px; MARGIN: 1px; COLOR: #0000ff; PADDING-TOP: 1px; TEXT-ALIGN: center; Font-Familly: Verdana,Arial }
		</style>
<body>
<script language="javascript">
function DescSize()
{
if (document.forms[0].txtModuleDesc.value.length>255)
{
return false;
}
else
{
return true;
}
}
</script>

<form id="Form1" runat="server" method="post">
<input Type="Hidden" id="projName" runat="Server">
<input type="hidden" id="strModuleName" Runat="Server">
<input type="hidden" id="strFlag" Runat="Server">
<Table cellpadding="4" width="100%"  border="1" style="border-collapse: collapse" bordercolor="#E8E8E8" bordercolorlight="#E8E8E8" bordercolordark="#E8E8E8" cellpadding="0" cellspacing="0">
<tr>
  <td  align="center" colspan="4">
  <FONT face="Verdana" size="2">
			<h3><%=projName.Value%></h3>
  </FONT>
   </td>
  </tr>
  <tr>
<tr >
  <td  align="left" colspan="4"><b>Module Details<b></td>
  </tr>
 
<tr>
<td valign="top" >Modules:</td>
<td>
<asp:DropDownList id="ddlModule" runat="server" Width="265" AutoPostBAck="True" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged"></asp:DropDownList>
</td>
<td colspan="2">
<asp:button id="btnEdit" runat="server" text="Edit Module" width="100" onclick="Edit_Click"/>

<asp:button id="btnAdd" runat="server" text="Add Module" width="100" onclick="Add_Click"/>

</td>
</tr>
<tr>
<td colspan="4" align="center">
<%If strFlag.Value="update" Then
Else
Response.Write("<b>"& strModuleName.Value &"</b>")
End If
%>

</td>
</tr>

<tr>
<td valign="top">
Module Name:
</td>
<td valign="top">
<input type="text" id="txtModuleName" runat="server" style="width:250;" />
</td>
<td valign="top">
Module Details:
</td>
<td>
<asp:TextBox id="txtModuleDesc" TextMode="multiline" runat="server" height="100" width="300" OnKeyPress="return DescSize();"/>
</td>
</tr>

<tr>
<td  align="center" colspan="4">
<asp:button id="btnSubmit" runat="server" text="Submit" width="100" onclick="Submit_Click"/>

<asp:button id="btnClose" runat="server" text="Close" width="100"/>
</td>
</tr>


</table>

</form>
</body>
</html>