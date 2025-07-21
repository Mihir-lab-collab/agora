<%@ Page Language="VB" %>
<%@Import Namespace="System.Data"%>
<%@Import Namespace="System.Data.SqlClient"%>
<%@ Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@ Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>
<SCRIPT LANGUAGE="JAVASCRIPT">
	  function confirmSubmit()
	  {
	 if (document.forms[0].ddlModule.value==0)
     {
       var agree=confirm('Are you sure you want to Add Top Level Module?');
    
      if (agree)
       {
          
           return true ;
       }
       else
       {
       document.forms[0].txtModuleName.value=""
       document.forms[0].txtModuleDesc.value=""
          return false ;
       }
      }
	   }
	 
	   
  function ModuleIndexChange()
	   {
	 
   if (document.forms[0].ddlModule.value==0)
   {
    document.forms[0].strModuleName.value=""
   document.forms[0].txtModuleName.value=""
   document.forms[0].txtModuleDesc.value=""
   
    document.forms[0].btnEdit.disabled=true;
      document.forms[0].btnAdd.disabled=true;
     document.forms[0].btnSubmit.disabled=false;
    document.forms[0].txtModuleName.disabled=false;
    document.forms[0].txtModuleDesc.disabled=false;
    
   document.forms[0].strModuleName.value=""
   document.forms[0].strModuleName.style.visibility="hidden";
   document.getElementById("tdModuleName").style.visibility = "hidden";
   document.getElementById("tdModuleName").style.display = "none";
    }
   else
   if (document.forms[0].ddlModule.value!=0)
      {
      document.forms[0].btnEdit.disabled=false;
      document.forms[0].btnAdd.disabled=false;
      document.forms[0].btnSubmit.disabled=true;
    document.forms[0].txtModuleName.value=""
   document.forms[0].txtModuleDesc.value=""   
   document.forms[0].txtModuleName.disabled=true;
   document.forms[0].txtModuleDesc.disabled=true;
     
    document.forms[0].strModuleName.value=""
   document.forms[0].strModuleName.style.visibility="hidden";
   document.getElementById("tdModuleName").style.visibility = "hidden";
   document.getElementById("tdModuleName").style.display = "none";
  // document.getElementById("tdModuleName").style.height = 0;
  
      }
	  
	   }
	   
	
 </SCRIPT>
<script language="VB" runat="server">
     Dim IntProjId as Integer
    Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
	 Dim conn As SqlConnection = New SqlConnection(dsn1)
	 Dim strSQL as string
	 Dim intCheck as Integer	
    Dim gf As New generalFunction
Sub Page_Load(sender As Object, e As EventArgs) 
        gf.checkEmpLogin()
        Try
            ddlModule.Attributes.Add("OnChange", "javascript:return ModuleIndexChange();")
            btnSubmit.Attributes.Add("OnClick", "javascript:return confirmSubmit();")
            If Not IsPostBack Then
                strSQL = "select * from projectMaster where projId=" & IntProjId & "  ORDER BY projName"
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
                BindProjectDropDown()
                'BindDropDown(IntProjId)
                btnEdit.Enabled = False
                btnAdd.Enabled = False
 
            End If
            ' response.write(ddlModule.SelectedItem.Value)
            'If ddlModule.SelectedItem.Value=0 then
            'btnEdit.Enabled=False
            'btnAdd.Enabled=False
            'Else
            'btnEdit.Enabled=True
            'btnAdd.Enabled=True   
            'End IF
            IntProjId = ddlProj.SelectedItem.Value
                     
        Catch ex As Exception

        End Try
End Sub

  '==================================================
      'BIND DROPDOWN FROM DATABASE AND DISPLAY PROJECTS
    '===================================================   	
			Sub BindProjectDropDown()
				ddlProj.items.clear()
                 strSQL=""
                	strSQL="select projName,projId from projectMaster ORDER BY projName"
                
		        Dim cmd As SqlCommand=new SqlCommand(strSQL,Conn)
                 Conn.Open()
	                Dim Rdr As SqlDataReader
                Rdr=cmd.executereader
			    if(Rdr.hasrows)then
                	while Rdr.read
                        ddlProj.items.add(new listitem(Rdr("projName"),Rdr("projId")))
                     end while
                end if
				Conn.close
				Rdr.close
				cmd.dispose
				 BindModule(ddlProj.SelectedItem.Value)
			End Sub
	 '=====================================================
      'END BIND DROPDOWN FROM DATABASE AND DISPLAY PROJECTS
    '======================================================
  Sub ddlProj_SelectedIndexChanged(sender As Object, e As EventArgs)
      BindModule(ddlProj.SelectedItem.Value)
   
    btnEdit.Enabled=False
	btnAdd.Enabled=False
	
        If ddlModule.SelectedIndex = -1 Then

            btnSubmit.Enabled = "true"
            txtModuleName.Disabled = "False"
            txtModuleDesc.Enabled = "true"
            btnEdit.Enabled = False
            btnAdd.Enabled = False
        Else
            btnSubmit.Enabled = "False"
            txtModuleName.Disabled = "True"
            txtModuleDesc.Enabled = "False"
            btnEdit.Enabled = True
            btnAdd.Enabled = True
        End If
   intCheck=1
     txtModuleName.Value=""
    txtModuleDesc.Text=""
  End Sub



 'Sub BindDropDown(IntProjId as Integer)
  '   ddlModule.items.clear()
   '  ddlModule.items.add(new listitem("Select Module",0))
   '  strSQL=""
   '  strSQL="select * from projectModuleMaster where projId="& IntProjId

'		  Dim arrmodId(100) as integer
'		  Dim arrmodrefId(100) as integer
'		  Dim arrmodName(100) as string
'	      Dim i as integer=0
'		  Dim j as integer=0
'		  Dim k as integer=0

'     Dim cmd As SqlCommand = New SqlCommand(strSQL,conn)
 '    Conn.Open()
'	 Dim Rdr As SqlDataReader
 '    Rdr=Cmd.ExecuteReader()
  '   If Rdr.HAsRows then
   '       while Rdr.Read 
	'	                 arrmodId(i) = Rdr("moduleId")
	'				     arrmodrefId(i) = Rdr("moduleRefId")
	'					 arrmodName(i) = Rdr("moduleName")
						
	'				     i = i + 1
	'				     j = j + 1
	'				     k = k + 1
            
     '     End While
     'End If

	        '           For i=0 To k-1
			'			   For j=0 To k-1
			'			       If arrmodId(i)  = arrmodrefId(j) And arrmodName(i) <> arrmodName(j) then
			'			              ddlModule.Items.Add( new  ListItem (arrmodName(i).ToString()+" >> "+arrmodName(j).ToString(),arrmodId(i).ToString()))
             '                  End if
			'				   If arrmodId(i) = arrmodrefId(j) And arrmodName(i) = arrmodName(j) then
			'				          ddlModule.Items.Add( new  ListItem (arrmodName(i).ToString(),arrmodId(i).ToString()))
			'				   End If
			'				Next
			'			Next
          'Cmd.Dispose
          'Rdr.Close
          'Conn.Close
  
 'End Sub
 
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
	btnEdit.Enabled=true
	btnAdd.Enabled=true
 End Sub
 
 Sub Add_Click(sender As Object, e As EventArgs)
    strModuleName.Value=ddlModule.SelectedItem.Text
    strFlag.Value="insert"
    txtModuleName.Disabled="False"
    txtModuleDesc.Enabled="true"
    txtModuleName.Value=""
    txtModuleDesc.Text=""
    btnSubmit.Enabled="True"
    btnEdit.Enabled=true
	btnAdd.Enabled=true
 End Sub
 
 Sub Submit_Click(sender As Object, e As EventArgs)
 
  dim Cmd as SqlCommand
  Dim intModuleID as Integer

      strSQL=""
      strSQL="select max(moduleId)as ModuleId from projectModuleMaster"
      Cmd = New SqlCommand(strSQL,conn)
      Conn.Open()
	  Dim Rdr As SqlDataReader
      Rdr=Cmd.ExecuteReader()
      If Rdr.HAsRows then
         If Rdr.Read 
            If IsDbNull(Rdr("ModuleId"))=False Then
            intModuleID=Rdr("ModuleId")
            else
            intModuleID=0
        End If
     End IF
   End If
   Cmd.Dispose
   Rdr.Close
   Conn.Close 
   intModuleID=intModuleID+1

     strSQL=""
   if txtModuleName.value<>"" then
    If strFlag.Value="update" and ddlModule.SelectedItem.Value<>0 Then
        strSQL="Update projectModuleMaster Set ModuleName='"& Trim(replace(txtModuleName.Value,"'","''")) &"',ModuleDescription='"& Trim(replace(txtModuleDesc.Text,"'","''")) &"' where moduleId="& ddlModule.SelectedItem.Value
    Elseif strFlag.Value="insert" and ddlModule.SelectedItem.Value<>0 Then
            strSQL="Insert Into projectModuleMaster(projId,moduleRefId,ModuleName,ModuleDescription)Values("& IntProjId &","& ddlModule.SelectedItem.Value &",'"& Trim(replace(txtModuleName.Value,"'","''")) &"','"& Trim(replace(txtModuleDesc.Text,"'","''")) &"')"
    Elseif ddlModule.SelectedItem.Value=0 then
  
        strSQL="Insert Into projectModuleMaster(projId,moduleRefId,ModuleName,ModuleDescription)Values("& IntProjId &","& intModuleID &",'"& Trim(replace(txtModuleName.Value,"'","''")) &"','"& Trim(replace(txtModuleDesc.Text,"'","''")) &"')"
   End If
 
     cmd= New SqlCommand(strSQL,conn)
     Conn.Open()
	 Cmd.ExecuteNonQuery()
  
     Cmd.Dispose
     Conn.Close 
     
      Dim sp1 As String
            sp1 = "<Script language=JavaScript>"
            sp1 += " alert('Module Details Added');"
            sp1 += "</" + "script>"
            RegisterStartupScript("script123", sp1)
    Else
            Dim sp As String
            sp = "<Script language=JavaScript>"
            sp += " alert('Please Enter Valid Module Name');"
            sp +="document.forms[0].txtModuleName.focus()"
            sp += "</" + "script>"
            RegisterStartupScript("script223", sp)
    End if
    txtModuleName.Value=""
    txtModuleDesc.Text=""
     BindModule(ddlProj.SelectedItem.Value)
 
		 
  '========================
  'SELECTED TRUE
  '========================
  Dim item As ListItem
  Dim i as Integer
		For i=0 to ddlModule.Items.Count-1
		
		If (ddlModule.Items(i).Text )= strModuleName.Value Then
			ddlModule.SelectedIndex=i
		  Exit For
		End If
			
		Next
		
 End Sub
 Sub BindModule(ByVal intProjId As Integer)
        ddlModule.items.clear()
        Dim strSQL As String, projId,selProject
        Dim objCmd as sqlCommand
        Dim gridAdapter  as SqlDataAdapter
        Dim gridDataset as DataSet
        'strConn.Open()
        strSQL = ""
        selProject = ddlProj.SelectedValue
        'strSQL = " select * from  " & _
          ' " ( " & _
           '" Select b.moduleId  as M_ID   ,a.moduleRefId as M_RID ,a.moduleName + ' >> ' + b.moduleName as ModuleName  from projectModuleMaster as a " & _
           ' " inner join  projectModuleMaster as b on  " & _
           ' " a.moduleId = b.moduleRefId where a.moduleId <> b.moduleId   and a.projId =   '" & selProject & "'" & _
         ' " union all  " & _
          '"  Select a.moduleId as M_ID ,a.moduleRefId M_RID ,a.moduleName as ModuleName from projectModuleMaster as a " & _
          '"   inner join  projectModuleMaster as b on  " & _
          '"   a.moduleId = b.moduleRefId where a.moduleId = b.moduleId   and a.projId =  '" & selProject & "'" & _
          '"  and  a.moduleId not in  " & _
          ' "  ( " & _
         ' "  Select a.moduleId   from projectModuleMaster as a " & _
          '"   inner join  projectModuleMaster as b on  " & _
          '"   a.moduleId = b.moduleRefId where a.projId =  '" & selProject & "'" & _
          '"  ) " & _
          '"  ) as  tblTMP  " & _
          '"  order by tblTMP.M_ID Asc "
        
        
        strSQL= " select * from " & _
                 "(" & _
                 " Select b.moduleId as M_ID ,a.moduleRefId as M_RID ,a.moduleName + ' >> ' + b.moduleName as ModuleName from projectModuleMaster as a " & _
                 " inner join projectModuleMaster as b on a.moduleId = b.moduleRefId where a.moduleId <> b.moduleId and a.projId = '" & selProject & "'" & _
                 "union all Select a.moduleId as M_ID ,a.moduleRefId M_RID ,a.moduleName as ModuleName from projectModuleMaster as a " & _
                 "inner join projectModuleMaster as b on a.moduleId = b.moduleRefId where a.moduleId = b.moduleId and a.projId = '" & selProject & "' )" & _
                 " as tblTMP order by moduleName"

          
'          "   a.moduleId = b.moduleRefId where a.moduleId <> b.moduleId   and a.projId =  '" & selProject & "'" & _
        objCmd = New SqlCommand(strSQL, Conn)
        gridAdapter = New SqlDataAdapter(strSQL, Conn)
        gridDataset = New DataSet()
        gridAdapter.Fill(gridDataset)

        ddlModule.datasource = gridDataset
        ddlModule.DataTextField = "ModuleName"
        ddlModule.DataValueField = "M_ID"
        ddlModule.databind()
        'strConn.Close()
    End Sub
 
</script>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>Project Module</title>
<style>
		H3 { PADDING-RIGHT: 1px; PADDING-LEFT: 1px; FONT-WEIGHT: bold; PADDING-BOTTOM: 1px; MARGIN: 1px; COLOR: #a2921e; PADDING-TOP: 1px; TEXT-ALIGN: left; Font-Familly: Verdana }
		</style>
</head>

<body>
<script language="javascript">
function DescSize()
{
if (document.forms[0].txtModuleDesc.value.length>254)
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
<table id="Table3" height="100%" cellSpacing="0" cellPadding="2" width="100%" align="center"
				border="0">
				<TR>
						<TD>
							<TABLE id="Table3" cellSpacing="0" cellPadding="2" width="100%" border="0" height="1">
								<TR>
									<TD>
										<EMPHEADER:EMPHEADER id="Empheader" runat="server">
                                        </EMPHEADER:EMPHEADER></TD>
								</TR>
								<TR>
									<TD>
										<uc1:empMenuBar id="EmpMenuBar" runat="server">
                                        </uc1:empMenuBar></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
<tr>
	<td height="90%" valign="top">
    <table border="0" cellpadding="2" cellspacing="0" width="100%" bordercolor="#FBF9EC" BgColor="#edf2e6">				
<tr >
  <td   align="left" colspan="4"><font face="Verdana" color="#a2921e" size="2"><b><H3>Module Details</H3><b></font></td>
  </tr>
<tr>
		<td  align="left" style="height: 26px">
			<font face="Verdana" color="#a2921e" size="2"><b>Project:</b></font>
				</td>
					<td style="height: 26px">
						<asp:DropDownList id="ddlProj" runat="server" autopostback="true" OnSelectedIndexChanged="ddlProj_SelectedIndexChanged">
                            </asp:DropDownList>
							</td>
					</tr>
  <tr>

 
<tr>
<td valign="top" >
<font face="Verdana" color="#a2921e" size="2"><b>Modules:</b></font>
</td>
<td>
<asp:DropDownList id="ddlModule" runat="server" AutoPostBAck="True"></asp:DropDownList>
</td>
<td colspan="2">
<asp:button id="btnEdit" runat="server" text="Edit Module" width="100" 
 style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold;background-color: #C5D5AE" font-bold="true" 
 onclick="Edit_Click"/>


<asp:button id="btnAdd" runat="server" text="Add Module" width="100" style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold;background-color: #C5D5AE" font-bold="true" onclick="Add_Click"/>

</td>
</tr>

<tr id="tdModuleName">
<td colspan="4" align="Center">
<%  Try%>
<%If strFlag.Value="update" or ddlModule.selecteditem.value=0 Then
ElseIf ddlModule.selecteditem.value<>0 Then
    Response.Write("<font face=""Verdana"" color=""#990066"" size=""4""><b>"& strModuleName.Value &"</b></font>")
End If
%>
<%  Catch ex As Exception%>
<%End Try%>
</td>
</tr>

<tr>
<td valign="top">
<font face="Verdana" color="#a2921e" size="2"><b>Module Name:</b></font>

</td>
<td valign="top">
<input type="text" id="txtModuleName" runat="server" style="width:250;" />
</td>
<td valign="top">
<font face="Verdana" color="#a2921e" size="2"><b>Module Details:</b></font>

</td>
<td>
<asp:TextBox id="txtModuleDesc" TextMode="multiline" runat="server" height="100" width="300" OnKeyPress="return DescSize();"/>
</td>
</tr>

<tr>
<td  align="center" colspan="4">
<asp:button id="btnSubmit" runat="server" text="Submit" width="100" style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold;background-color: #C5D5AE" font-bold="true"  onclick="Submit_Click"></asp:button>
</td>
</tr>
</table>
</td>
</tr>

</table>

</form>
</body>
</html>