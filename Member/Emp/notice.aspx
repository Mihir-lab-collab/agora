<%@ Page Language="vb" DEBUG=True validateRequest="false"%>
<%@Import Namespace="System.Data"%>
<%@Import Namespace="System.Data.SqlClient"%>
<%@ Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@ Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>  
 
<%@ Import NameSpace="System.Web.Mail" %>
<%@ Import NameSpace="System.Web.Mail.MailMessage" %>
<%@ Import NameSpace="System.Web.Mail.SmtpMail" %>

<%@ Import Namespace="System.Web.Hosting" %>
<%@ Import NameSpace="System.IO" %>
<%@ Import NameSpace="System.NET" %>
<%@ Import Namespace="System.IO.FileStream" %>	 


<html xmlns="http://www.w3.org/1999/xhtml" >
	<HEAD>
		<title>Dynamic Web Tech</title>

<script language="JavaScript">

function noticeClose()
			{
				 window.close();
				 return false;
			}

</script>

<script language="VB" runat="server">
         Dim conn As new SqlConnection  
         Dim Cmd as New SQLCommand()
         Dim da as SQLDataAdapter
         Dim ds as DataSet
         Dim strSQL As String
         Dim flg as integer
		 Public inttest as integer
    Dim gf As New generalFunction
    Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        gf.checkEmpLogin()
        btnclose.Attributes.Add("onclick", "javascript:return noticeClose();")
        
        If Not IsPostBack Then
            If Session("dynoAdminSession") = 1 Then
                Session.Add("dynoflgsession", "0")
                Session.Add("dynonoticeid", "1")
                'response.write session.add("dynonoticeid","1")
                'response.end
                Session.Add("dynoimageedit", "0")
                Call bindgrid()
            Else
                Call bindgrid()
            End If
        End If
    End Sub

Sub Cbindgrid()

   Conn=New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
   Conn.Open()

   Dim da As SqlDataAdapter = New SqlDataAdapter("select * from notice order by noticeid desc", conn)
   dim ds as new dataset()
   da.Fill(ds,"notice")

 End Sub


Sub bindgrid()
   Conn=New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
   Conn.Open()

   Dim da As SqlDataAdapter = New SqlDataAdapter("select * from notice order by noticeid desc", conn) 
   dim ds as new dataset()
   da.Fill(ds,"notice")
  
   dgrdnotice.DataSource = ds.tables("notice")
   dgrdnotice.DataBind()

 End Sub



Sub dgrdNotice_editCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)   
 
 Dim unoticeid as integer
 Dim unoticedate,unoticedesc as string

 
 Call bindgrid()

 noticeNotes.value= (e.item.cells(2).text)

 unoticeid=Convert.ToInt32(dgrdnotice.DataKeys(e.Item.ItemIndex))

 session.item("dynonoticeid")=unoticeid

 Session.item("dynoflgSession")="1"

 session.item("dynoimageedit")="1"
End Sub



 Sub ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
  Dim intNoticeid as integer
  Dim Dtnoticedt as string
  Dim strNoticedesc as String
  Dim Bdisplay as byte
  
  
  If e.item.itemindex > -1 Then

   intNoticeid=val(e.item.cells(0).text)
   dtNoticedt= (e.item.cells(1).text)

  strNoticedesc = Replace((e.item.cells(2).text),vbCrLf,"<br>")
 'Replace(Request.form("MyTextareaBox"),vbCrLf,"<br>")

  ' strNoticedesc = Replace((e.item.cells(2).text),vbCrLf,"<br>")
   Response.write (strNoticedesc)
   Bdisplay  = 1

  ' Response.Write ("hello")
   
  End if 
 
 End sub


 Sub dgrdNotice_deleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) 
     
    Dim deletesql as String

	 deletesql="delete from notice where  noticeid = " & (Convert.ToInt32(dgrdnotice.DataKeys(e.Item.ItemIndex))) & ""

                Dim deletecmd As sqlCommand = New sqlCommand(deletesql, conn)               
 
                Call bindgrid()	 

                deletecmd.Connection=conn
				deletecmd.commandtext=deletesql
	            deletecmd.ExecuteNonQuery()
                dgrdnotice.EditItemIndex = -1
                Call bindgrid()

 End Sub
 
 Sub imagebuttondelete_click(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)   
 Dim test as string
 Dim sp As String
        sp = "<Script language=JavaScript>"
   
        sp += " alert('Do you want to delete record  '); "  

        sp += "function getconfirm() "
        sp += "{ "
        sp +=" if (confirm('Do you want to delete record?')) "
        sp +=" return true; "
		sp +="else "
        sp +="return false;} "

    
        sp += "</" + "script>"
     RegisterStartupScript("script123", sp)   


 End Sub

 Sub btncancel_Click(ByVal sender As System.Object,ByVal e As System.EventArgs) 
 	Response.Redirect ("../emp/notice.aspx")
 End Sub
 
 Sub btnAdd_Click(ByVal sender As System.Object,ByVal e As System.EventArgs) 
  If session.item("dynoflgSession")="1" Then
     Dim updatesql as String
	 If noticenotes.value <> ""  Then
	  updatesql="update notice set notice_descr= '" & Trim(strParse(noticeNotes.value)) & _
	  "' where noticeid = " & (session.item("dynonoticeid")) & ""
	 
	  Dim updatecmd As sqlCommand = New sqlCommand(updatesql, conn)               
		     Call bindgrid()	 
                updatecmd.Connection=conn
				updatecmd.commandtext=updatesql
	            updatecmd.ExecuteNonQuery()
                dgrdnotice.EditItemIndex = -1
	
			    Call bindgrid()
			    noticenotes.value=""
     End if
  End If
  
  If session.item("dynoflgSession")="0" Then
     Dim insertsql as String
	 Dim ddate as string
     ddate=datetime.Now
  
     If noticenotes.value <> ""  then
  
 	  insertsql="insert into notice (notice_descr,display,date) Values('" & Trim(strParse(noticeNotes.value)) & "' ,'1',getdate())"

		Dim addcmd As sqlCommand = New sqlCommand(insertsql, conn)               

		Call bindgrid()	 

		addcmd.Connection=conn
		addcmd.commandtext=insertsql
		addcmd.ExecuteNonQuery()
		dgrdnotice.EditItemIndex = -1
		Call bindgrid()

		noticenotes.value=""
		
    End if
  End If
  
         
 End Sub

Sub dgrdnotice_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) 

      dgrdnotice.CurrentPageIndex = e.NewPageIndex
	  Call bindgrid()
   End Sub

      
 Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 
'       Dim sp As String
'       sp = "<script language='JavaScript'>"
'       sp += "window.close();"
'       sp += "window.moveTo(0,0);"
'       sp += "window.resizeTo(screen.width,screen.height);"
'
'       sp += "</" + "script>"
'       RegisterStartupScript("script1",sp)
 End Sub

Function strParse(str)
	If str <> "" Then
		str = Replace(str,"'","''")
		str = Replace(str,"&lt;","<")
		str = Replace(str,"&gt;",">")
	End If
	return str
End function

        </script>		
	</HEAD>
	<body>
			<form id="Form1" method="post" runat="server">
			
			<table id="Table3"  cellSpacing="0" cellPadding="2" width="100%" align="center"
				 height=100% >

				 <% If Session("dynoAdminSession") = 1 then%>				
                     <tr valign="top" height="10%" bgcolor="#EDF2E6">
					   <td colspan="2" height="40">
                    <p align="center"><font face="Arial" color="#A2921E"><b>
                    <span style="font-size: 14pt">Notice Board</span></b></font><BR>
						</td>
			   	     </tr>
                  <% End If %> 

				<tr  valign="top" height=10% >
					<% If Session("dynoAdminSession") = 1 then%>				
					<td width="100%" colspan="2" align="center" bgcolor="#edf2e6">
							<textArea id="noticeNotes" runat="server" rows="5" cols="70" NAME="Textarea1"></textArea>
					</td>

					<tr width="100%" height="10%" colspan="2" align="center" bgcolor="#EDF2E6">
					<td colspan="2" height="40">
					<font face="Arial" color="#A2921E"></font>

                            <asp:Button id="btnadd" onclick="btnadd_Click" runat="server" Width="90px" Text="Submit" style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE">
                            </asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 

                            <asp:Button id="btncancel" onclick="btncancel_Click" runat="server" Width="90px" Text="Cancel" style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE">
                            </asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 

                           <% End If %>
                            <asp:Button id="btnclose" onclick="btnclose_Click" runat="server" Width="90px" Text="Close" style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE">
                            </asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
		              
                       

					</td></tr>
				</tr>

                   <tr height=60% valign="top">
                       <td height="100%">    

                         <ASP:DATAGRID id="dgrdNotice"  DataKeyField="noticeid" runat="server"  BorderColor="" Font-Size="10pt" Font-Name="Verdana"  BackColor="White" Font-Names="Verdana" AutoGenerateColumns="False" Footerstyle-HorizontalAlign="Right" width=100%      oneditcommand="dgrdNotice_editcommand"  
							Headerstyle-Font-Size="10pt" Headerstyle-Forecolor="#A2921E" Headerstyle-Font-Bold="True" AllowSorting="True" Headerstyle-BackColor="#C5D5AE" CellPadding="0"  ondeletecommand="dgrdnotice_deletecommand"  

							OnPageIndexChanged="dgrdnotice_PageIndexChanged"
							 OnItemDataBound = "ItemDataBound" PageSize="25" AllowPaging="True" >

						   <ItemStyle BackColor="#FFFFEE"></ItemStyle>                            
			
						<Columns>
						  <asp:BoundColumn  visible=false HeaderText="Noticeid" runat="server"  
							DataField="noticeid"  >
							<HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Left" VerticalAlign="middle"></ItemStyle>
							</asp:BoundColumn>							 
					  

							<asp:BoundColumn  HeaderText="Date" runat="server" DataField="date"  DataFormatString="{0:dd-MMM-yy}">
						   <HeaderStyle Width="10%" HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Left" VerticalAlign="middle"></ItemStyle>
							</asp:BoundColumn>

						<asp:TemplateColumn HeaderText="Notice" runat="server">
							<ItemTemplate>
                             <%#Replace(Container.DataItem("notice_descr"),vbcrlf,"<BR>")%>
                        </ItemTemplate>
                        </asp:TemplateColumn>

						<asp:TemplateColumn   >
						<ItemTemplate   >
                             <% If Session("dynoAdminSession") = 1 then%>
                              <asp:imagebutton   runat ="server"  id="imagebuttonedit" height=30  imageurl="../images/edit.jpg"  commandname=edit/>
							 <% End If %>
                        </ItemTemplate>
                        </asp:TemplateColumn>


                        <asp:TemplateColumn>
						 <ItemTemplate >
						    <% If Session("dynoAdminSession") = 1 then%>
                              <asp:imagebutton  visible=true runat ="server"  id="imagebuttondelete" height=30  imageurl="../images/delete.jpg" text=delete commandname="delete" />
							     <% End If %>
						  </ItemTemplate>

                        </asp:TemplateColumn>

					 </Columns>

                 	<pagerstyle Mode ="Numericpages" ></pagerstyle>
							
						</ASP:DATAGRID>
                           </td>
                          </tr>

			     </table>

    </form>
	</body>
</html>